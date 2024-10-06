import os
import re
import csv
from PyPDF2 import PdfReader

# Archivo PDF de entrada
pdf_path = './fuentes_data/Horarios/HorariosGITT(24-25).pdf'

# Nombre del archivo CSV de salida
output_csv = 'asignaturas_horarios.csv'

# Expresión regular para encontrar asignaturas y sus abreviaturas
asignatura_re = re.compile(r'(\w+)\s*\((\w+)\)')

# Función para extraer la leyenda de abreviaturas (asignaturas y nombres completos)
def extract_legend(pdf_text):
    legend = {}
    lines = pdf_text.splitlines()
    for line in lines:
        match = asignatura_re.search(line)
        if match:
            abbreviation = match.group(1).strip()
            full_name = match.group(2).strip()
            legend[abbreviation] = full_name
    return legend

# Función para extraer horarios y asignaturas del PDF
def extract_info_from_pdf(pdf_path):
    with open(pdf_path, 'rb') as file:
        reader = PdfReader(file)
        text = ''
        for page in reader.pages:
            text += page.extract_text()

    legend = extract_legend(text)

    # Buscar todas las líneas con asignaturas y horas
    horarios_re = re.compile(r'(\d{1,2}:\d{2})-(\d{1,2}:\d{2})\s*(\w+)\s*(\(.*\))?\s*(\w+)?')
    horarios_info = []

    for match in horarios_re.finditer(text):
        start_time = match.group(1)
        end_time = match.group(2)
        abbreviation = match.group(3)
        group = match.group(4) if match.group(4) else ''
        room = match.group(5) if match.group(5) else 'N/A'

        # Obtener el nombre completo de la asignatura
        asignatura_name = legend.get(abbreviation, 'Desconocida')
        if group:
            asignatura_name += f' {group}'

        horarios_info.append((asignatura_name, start_time, end_time, room))

    return horarios_info

# Función para guardar los datos en un archivo CSV
def save_to_csv(horarios_info):
    with open(output_csv, mode='w', newline='', encoding='utf-8') as file:
        writer = csv.writer(file)
        writer.writerow(['Asignatura', 'Hora Inicio', 'Hora Fin', 'Aula'])

        for asignatura, start_time, end_time, room in horarios_info:
            writer.writerow([asignatura, start_time, end_time, room])

# Ejecutar el script
horarios_info = extract_info_from_pdf(pdf_path)
save_to_csv(horarios_info)

print(f"Información extraída y guardada en {output_csv}")
