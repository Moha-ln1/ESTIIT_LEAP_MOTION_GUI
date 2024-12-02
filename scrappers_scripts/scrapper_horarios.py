import requests
from bs4 import BeautifulSoup
import csv
import re

# URL del fichero HTML inicial
main_page_url = "https://etsiit.ugr.es/informacion-academica/grados/graduadoa-ingenieria-informatica"

# Nombre del archivo CSV de salida
output_file = "./data/horarios_ugr.csv"

# Función para calcular las siglas a partir del nombre de la asignatura
def calcular_siglas(nombre_asignatura):
    palabras = nombre_asignatura.split()
    siglas = ''.join([palabra[0].upper() for palabra in palabras if palabra[0].isalpha()])
    return siglas

# Función para extraer la información de las asignaturas desde la página principal
def extract_subject_info():
    response = requests.get(main_page_url)
    soup = BeautifulSoup(response.content, 'html.parser')
    
    # Lista para almacenar la información de las asignaturas
    subjects_info = []

    # Buscar todas las asignaturas en los 'td' con clase 'asignatura'
    asignaturas = soup.find_all('td', class_='asignatura')

    for asignatura in asignaturas:
        # Extraer el nombre y URL específica de la asignatura
        nombre = asignatura.a.get_text(strip=True)
        siglas = calcular_siglas(nombre)
        asignatura_url = asignatura.a['href']
        if not asignatura_url.startswith('http'):
            asignatura_url = "https://etsiit.ugr.es" + asignatura_url
        
        # Agregar a la lista
        subjects_info.append({
            'Nombre': nombre,
            'Siglas': siglas,
            'URL': asignatura_url
        })
    
    return subjects_info

# Función para extraer profesores y grupos de la sección de "Profesorado"
def extract_professor_groups(soup):
    professor_groups = []
    profesor_divs = soup.find_all('li', class_='profesor')
    for profesor_div in profesor_divs:
        profesor_nombre = profesor_div.find('a').get_text(strip=True)
        grupo_span = profesor_div.find('span', class_='grupos')
        grupo_text = grupo_span.get_text(strip=True).replace("Grupo", "").replace("Grupos", "").strip() if grupo_span else "Desconocido"
        
        # Dividir los grupos si hay varios (por ejemplo, "A, B y F" o "1, 10, 11")
        grupos = re.split(r'[,\sy]+', grupo_text)
        for grupo in grupos:
            if grupo.strip():
                professor_groups.append({
                    'Profesor': profesor_nombre,
                    'Grupo': grupo.strip()
                })
    
    return professor_groups

# Función para extraer detalles de los grupos (incluyendo aula, fechas, y horario)
def extract_group_details(soup):
    group_details_list = []
    group_divs = soup.find_all('div', class_='clase')
    
    for group_div in group_divs:
        # Extraer el grupo
        grupo_div = group_div.find('div', class_='grupo')
        grupo = grupo_div.get_text(strip=True).replace("Grupo:", "").strip() if grupo_div else "Desconocido"
        
        # Extraer detalles adicionales: aula, fecha de inicio, fecha final, horario
        aula_div = group_div.find('div', string=re.compile('Aula:'))
        aula = aula_div.get_text(strip=True).split(':')[-1].strip() if aula_div else "No disponible"
        
        fecha_inicio_div = group_div.find('div', string=re.compile('Fecha de inicio:'))
        fecha_inicio = fecha_inicio_div.get_text(strip=True).split(':')[-1].strip() if fecha_inicio_div else "No disponible"
        
        fecha_final_div = group_div.find('div', string=re.compile('Fecha final:'))
        fecha_final = fecha_final_div.get_text(strip=True).split(':')[-1].strip() if fecha_final_div else "No disponible"
        
        horario_div = group_div.find('div', string=re.compile('Horario:'))
        horario = horario_div.get_text(strip=True).split(':', 1)[-1].strip() if horario_div else "No disponible"
        
        # Formatear el horario para representarlo como "09:30-10:30"
        horario_match = re.search(r'(\d{2}:\d{2})\s*a\s*(\d{2}:\d{2})', horario)
        if horario_match:
            horario = f"{horario_match.group(1)}-{horario_match.group(2)}"
        
        # Guardar la información del grupo
        group_details_list.append({
            'Grupo': grupo,
            'Aula': aula,
            'Fecha de inicio': fecha_inicio,
            'Fecha final': fecha_final,
            'Horario': horario
        })
    
    return group_details_list

# Función para extraer y combinar toda la información necesaria
def extract_additional_info(subject_url):
    try:
        response = requests.get(subject_url)
        response.raise_for_status()
    except requests.exceptions.RequestException as e:
        print(f"Error al acceder a la página: {subject_url}. Error: {e}")
        return []

    soup = BeautifulSoup(response.content, 'html.parser')
    
    # Obtener los detalles de los grupos
    group_details_list = extract_group_details(soup)
    
    # Obtener los profesores y grupos
    professor_groups = extract_professor_groups(soup)
    
    # Combinar la información de los profesores con los detalles del grupo
    combined_info = []
    for group_detail in group_details_list:
        grupo = group_detail['Grupo']
        
        # Encontrar el profesor correspondiente al grupo
        profesor = "Desconocido"
        for prof_group in professor_groups:
            if grupo.strip() == prof_group['Grupo'].strip():
                profesor = prof_group['Profesor']
                break
        
        # Añadir toda la información combinada
        combined_info.append({
            'Profesor': profesor,
            'Grupo': grupo,
            'Aula': group_detail['Aula'],
            'Fecha de inicio': group_detail['Fecha de inicio'],
            'Fecha final': group_detail['Fecha final'],
            'Horario': group_detail['Horario']
        })
    
    return combined_info

# Función para guardar los datos en un archivo CSV
def save_to_csv(subjects_info):
    with open(output_file, mode='w', newline='', encoding='utf-8') as file:
        writer = csv.writer(file)
        # Escribir encabezado
        writer.writerow(['Nombre', 'Siglas', 'Grupo', 'Aula', 'Fecha de inicio', 'Fecha final', 'Horario', 'Profesor', 'URL'])
        
        # Escribir la información de cada asignatura
        for subject in subjects_info:
            nombre = subject['Nombre']
            siglas = subject['Siglas']
            url = subject['URL']
            
            # Extraer información adicional para cada grupo
            group_info_list = extract_additional_info(url)
            for group_info in group_info_list:
                grupo = group_info['Grupo']
                aula = group_info['Aula']
                fecha_inicio = group_info['Fecha de inicio']
                fecha_final = group_info['Fecha final']
                horario = group_info['Horario']
                profesor = group_info['Profesor']
                
                # Escribir una fila para cada grupo
                writer.writerow([nombre, siglas, grupo, aula, fecha_inicio, fecha_final, horario, profesor, url])

# Ejecución del script
subjects_info = extract_subject_info()
save_to_csv(subjects_info)

print(f"Información extraída y guardada en {output_file}")
