import requests
from bs4 import BeautifulSoup
import csv
import re

# URL del fichero HTML inicial
main_page_url = "https://etsiit.ugr.es/informacion-academica/grados/graduadoa-ingenieria-informatica"

# Nombre del archivo CSV de salida
output_file = "./data/asignaturas_ugr.csv"

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

# Función para extraer horarios, profesores y aulas de la página específica de cada asignatura
def extract_additional_info(subject_url):
    # Comprobar si la URL es válida antes de realizar la solicitud
    if not subject_url or subject_url == "N/A":
        return "No disponible", "No disponible", "No disponible", "No disponible"

    try:
        response = requests.get(subject_url)
        response.raise_for_status()  # Comprobar si la solicitud fue exitosa
    except requests.exceptions.RequestException as e:
        print(f"Error al acceder a la página: {subject_url}. Error: {e}")
        return "No disponible", "No disponible", "No disponible", "No disponible"
    
    soup = BeautifulSoup(response.content, 'html.parser')
    
    # Extraer profesores y grupos
    profesores = []
    profesor_divs = soup.find_all('div', class_='profesorado-asignatura')
    for profesor_div in profesor_divs:
        profesor_nombre = profesor_div.find('a').get_text(strip=True)
        grupo_span = profesor_div.find('div', class_='grupos')
        grupo_text = grupo_span.get_text(strip=True) if grupo_span else "Desconocido"
        profesores.append(f"{profesor_nombre} ({grupo_text})")
    
    # Unir los profesores en un solo string
    profesores_str = '; '.join(profesores) if profesores else "No disponible"
    
    # Extraer horarios y aulas
    horarios = []
    aulas = []
    grupos = []
    horario_table = soup.find('table', class_='horario')
    if horario_table:
        for row in horario_table.find_all('tr')[1:]:  # Omitir la primera fila de encabezados
            columns = row.find_all('td')
            if len(columns) > 1:  # Verificar que haya datos en la fila
                hora = row.find('th').get_text(strip=True)
                for idx, column in enumerate(columns):
                    div_clase = column.find('div', class_='clase')
                    if div_clase:
                        grupo = div_clase.find('div', class_='grupo').get_text(strip=True)
                        aula_div = div_clase.find('div', text=re.compile('Aula:'))
                        aula = aula_div.get_text(strip=True).split(':')[-1].strip() if aula_div else "No disponible"
                        horarios.append(f"{hora} - Día {idx + 1}")
                        aulas.append(aula)
                        grupos.append(grupo)
    
    # Unir los horarios, aulas y grupos en strings
    horarios_str = '; '.join(horarios) if horarios else "No disponible"
    aulas_str = '; '.join(aulas) if aulas else "No disponible"
    grupos_str = '; '.join(grupos) if grupos else "No disponible"
    
    return horarios_str, profesores_str, aulas_str, grupos_str

# Función para guardar los datos en un archivo CSV
def save_to_csv(subjects_info):
    with open(output_file, mode='w', newline='', encoding='utf-8') as file:
        writer = csv.writer(file)
        # Escribir encabezado
        writer.writerow(['Nombre', 'Siglas', 'Horario', 'Profesor', 'Aula', 'Grupo', 'URL'])
        
        # Escribir la información de cada asignatura
        for subject in subjects_info:
            nombre = subject['Nombre']
            siglas = subject['Siglas']
            url = subject['URL']
            
            # Extraer información adicional
            horario, profesores, aula, grupo = extract_additional_info(url)
            
            # Escribir la fila
            writer.writerow([nombre, siglas, horario, profesores, aula, grupo, url])

# Ejecución del script
subjects_info = extract_subject_info()
save_to_csv(subjects_info)

print(f"Información extraída y guardada en {output_file}")
