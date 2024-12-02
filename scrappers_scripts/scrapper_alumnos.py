import os
import re
import csv
from PyPDF2 import PdfReader

# Ruta a la carpeta que contiene los PDFs
pdf_folder = './data_fuentes/Subgrupos'

# Nombre del archivo CSV de salida
output_csv = './data/alumnos_asignaturas.csv'

# Expresiones regulares para extraer información
asignatura_re = re.compile(r'^(.*?)\s*Grupo:\s*(\w+)')
subgrupo_re = re.compile(r'(\d+)$')

def extract_info_from_pdf(pdf_path):
    with open(pdf_path, 'rb') as file:
        reader = PdfReader(file)
        text = ''
        for page in reader.pages:
            text += page.extract_text()

    # Extraer el nombre de la asignatura y el grupo
    asignatura_match = asignatura_re.search(text)
    if asignatura_match:
        asignatura_nombre = asignatura_match.group(1).strip()
        grupo = asignatura_match.group(2).strip()
    else:
        asignatura_nombre = "Desconocida"
        grupo = "N/A"

    # Encontrar los estudiantes y sus subgrupos
    estudiantes_info = []
    for line in text.splitlines():
        if ',' in line:
            nombre = line.strip()
            subgrupo_match = subgrupo_re.search(line)
            if subgrupo_match:
                subgrupo = subgrupo_match.group(1)
                nombre_asignatura_completo = f"{asignatura_nombre} {grupo}{subgrupo}"
                estudiantes_info.append((nombre, nombre_asignatura_completo))

    return estudiantes_info

def process_pdfs(folder_path):
    all_students = []

    for filename in os.listdir(folder_path):
        if filename.endswith('.pdf'):
            pdf_path = os.path.join(folder_path, filename)
            estudiantes_info = extract_info_from_pdf(pdf_path)
            all_students.extend(estudiantes_info)

    return all_students

def save_to_csv(students_info):
    with open(output_csv, mode='w', newline='', encoding='utf-8') as file:
        writer = csv.writer(file)
        writer.writerow(['Nombre', 'Asignatura'])

        for nombre, asignatura in students_info:
            writer.writerow([nombre, asignatura])

# Ejecutar el script
pdf_folder = './data_fuentes/Subgrupos'
students_info = process_pdfs(pdf_folder)
save_to_csv(students_info)

print(f"Información extraída y guardada en {output_csv}")
