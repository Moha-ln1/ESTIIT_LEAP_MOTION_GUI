import csv
import re
from collections import defaultdict

# Función para procesar los alumnos y extraer el nombre de la asignatura y el grupo
def procesar_alumnos(alumnos):
    grupos_por_asignatura = defaultdict(list)
    alumnos_formateados = []

    for alumno in alumnos:
        # Eliminar los números del nombre
        nombre_limpio = re.sub(r'\d+', '', alumno['Nombre']).strip()
        
        # Eliminar números al inicio del nombre de la asignatura
        asignatura_limpia = re.sub(r'^\d+', '', alumno['Asignatura']).strip()
        
        # Dividir el string de asignatura y grupo (Ej: 'Informática Industrial A1')
        partes = asignatura_limpia.rsplit(' ', 1)
        if len(partes) == 2:
            asignatura, grupo = partes
            if grupo not in grupos_por_asignatura[asignatura]:
                grupos_por_asignatura[asignatura].append(grupo)
            alumnos_formateados.append({
                'Nombre': nombre_limpio,
                'Asignatura': asignatura,
                'Grupo': grupo
            })
    
    return grupos_por_asignatura, alumnos_formateados

# Función para generar el grupo formateado con la nueva lógica de 3 grupos de prácticas
def generar_grupo_formateado(grupo_horario):
    try:
        # Convertir el grupo a número
        grupo_num = int(grupo_horario)
        
        # Calcular la letra del grupo ('A', 'B', 'C') basado en bloques de 3
        letras_grupo = ['A', 'B', 'C']
        letra_index = (grupo_num - 1) // 3  # Dividir por 3 y restar 1 para índice basado en 0
        letra = letras_grupo[letra_index % 3]
        
        # Calcular el subgrupo ('1', '2', '3')
        subgrupo_num = (grupo_num - 1) % 3 + 1
        
        # Crear el grupo formateado
        return f"{letra}{subgrupo_num}"
    
    except ValueError:
        # Si el grupo_horario no es un número, devolverlo sin cambios
        return grupo_horario

# Función para actualizar los horarios reemplazando el contenido de la columna 'Grupo'
def actualizar_horarios(horarios):
    horarios_actualizados = []
    for horario in horarios:
        grupo_horario = horario['Grupo']
        grupo_formateado = generar_grupo_formateado(grupo_horario)
        
        # Reemplazar el valor de la columna 'Grupo' con el grupo formateado
        horario_actualizado = horario.copy()
        horario_actualizado['Grupo'] = grupo_formateado
        horarios_actualizados.append(horario_actualizado)
    
    return horarios_actualizados

# Cargar datos de alumnos desde un archivo CSV
def cargar_alumnos(file_path):
    alumnos = []
    with open(file_path, mode='r', encoding='utf-8') as file:
        reader = csv.DictReader(file)
        for row in reader:
            alumnos.append(row)
    return alumnos

# Cargar datos de horarios desde un archivo CSV
def cargar_horarios(file_path):
    horarios = []
    with open(file_path, mode='r', encoding='utf-8') as file:
        reader = csv.DictReader(file)
        for row in reader:
            horarios.append(row)
    return horarios

# Guardar los datos en un archivo CSV
def guardar_datos(datos, file_path, fieldnames):
    with open(file_path, mode='w', newline='', encoding='utf-8') as file:
        writer = csv.DictWriter(file, fieldnames=fieldnames)
        writer.writeheader()
        writer.writerows(datos)

# Ejecución del script
if __name__ == "__main__":
    # Cargar datos de alumnos y horarios
    alumnos = cargar_alumnos('./data_csv/alumnos_asignaturas.csv')
    horarios = cargar_horarios('./data_csv/horarios_ugr.csv')

    # Procesar los alumnos para obtener grupos por asignatura y generar nueva tabla de alumnos
    grupos_por_asignatura, alumnos_formateados = procesar_alumnos(alumnos)

    # Guardar los alumnos formateados
    guardar_datos(alumnos_formateados, './data_csv/alumnos_asignaturas.csv', ['Nombre', 'Asignatura', 'Grupo'])

    # Actualizar los horarios reemplazando la columna 'Grupo'
    horarios_actualizados = actualizar_horarios(horarios)

    # Guardar los horarios actualizados
    guardar_datos(horarios_actualizados, './data_csv/horarios_ugr.csv', horarios[0].keys())

    print("Alumnos formateados guardados en 'alumnos_asignaturas.csv'.")
    print("Horarios actualizados guardados en 'horarios_ugr.csv'.")
