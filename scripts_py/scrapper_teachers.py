import requests
from bs4 import BeautifulSoup
import csv

# URL del fichero HTML inicial
main_page_url = "https://etsiit.ugr.es/docencia/profesorado"

# Nombre del archivo CSV de salida
output_file = "./data/profesores_ugr.csv"

# Función para extraer la información de los profesores desde la página principal
def extract_professor_info():
    response = requests.get(main_page_url)
    soup = BeautifulSoup(response.content, 'html.parser')
    
    # Lista para almacenar la información de los profesores
    professors_info = []

    # Buscar todos los divs de clase 'row' que contienen información de los profesores
    rows = soup.find_all('div', class_='row')

    for row in rows:
        # Extraer el nombre y el enlace a la página personal
        nombre_div = row.find('div', class_='nombre')
        nombre = nombre_div.get_text(strip=True) if nombre_div else "N/A"
        personal_url = nombre_div.find('a')['href'] if nombre_div and nombre_div.find('a') else "N/A"
        
        # Extraer el departamento
        departamento_div = row.find('div', class_='departamento')
        departamento = departamento_div.get_text(strip=True) if departamento_div else "N/A"
        
        # Agregar a la lista
        professors_info.append({
            'Nombre': nombre,
            'Departamento': departamento,
            'URL': personal_url
        })
    
    return professors_info
# Función para extraer asignaturas y horarios de tutorías de la página personal de cada profesor
def extract_additional_info(personal_url):
    # Comprobar si la URL es válida antes de realizar la solicitud
    if not personal_url or personal_url == "N/A":
        return "No disponible", "No disponible"

    try:
        response = requests.get(personal_url)
        response.raise_for_status()  # Comprobar si la solicitud fue exitosa
    except requests.exceptions.RequestException as e:
        print(f"Error al acceder a la página: {personal_url}. Error: {e}")
        return "No disponible", "No disponible"
    
    soup = BeautifulSoup(response.content, 'html.parser')
    
    # Extraer asignaturas (se asume que están dentro de 'div' con clase 'asignaturas')
    asignaturas = []
    grados = soup.find_all('div', class_='titulo')
    for grado in grados:
        grado_nombre = grado.get_text(strip=True)
        asignaturas_div = grado.find_next_sibling('div', class_='asignaturas')
        if asignaturas_div:
            for asignatura in asignaturas_div.find_all('div', class_='asignatura'):
                asignatura_text = asignatura.get_text(strip=True)
                asignaturas.append(f"{grado_nombre}: {asignatura_text}")
    
    # Unir las asignaturas en un solo string
    asignaturas_str = '; '.join(asignaturas) if asignaturas else "No disponible"
    
    # Extraer horario de tutorías (se asume que está en algún div relacionado con 'tutorias')
    tutorias_div = soup.find('div', class_='tutorias')  # Ajustar según la estructura
    tutorias = tutorias_div.get_text(strip=True) if tutorias_div else "No disponible"
    
    return asignaturas_str, tutorias


# Función para guardar los datos en un archivo CSV
def save_to_csv(professors_info):
    with open(output_file, mode='w', newline='', encoding='utf-8') as file:
        writer = csv.writer(file)
        # Escribir encabezado
        writer.writerow(['Nombre', 'Departamento', 'Asignaturas', 'Horario de Tutorías', 'URL'])
        
        # Escribir la información de cada profesor
        for professor in professors_info:
            nombre = professor['Nombre']
            departamento = professor['Departamento']
            url = professor['URL']
            
            # Extraer información adicional
            asignaturas, tutorias = extract_additional_info(url)
            
            # Escribir la fila
            writer.writerow([nombre, departamento, asignaturas, tutorias, url])

# Ejecución del script
professors_info = extract_professor_info()
save_to_csv(professors_info)

print(f"Información extraída y guardada en {output_file}")
