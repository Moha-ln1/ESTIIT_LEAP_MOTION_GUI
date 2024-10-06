import requests
from bs4 import BeautifulSoup
import csv

# URL de la página del menú
menu_url = 'https://scu.ugr.es/pages/menu/comedor'

# Archivo CSV de salida
output_csv = './data/menus_semanales.csv'

def extract_menus():
    response = requests.get(menu_url)
    soup = BeautifulSoup(response.content, 'html.parser')

    menus = []

    # Buscar las tablas que contienen los menús
    menu_tables = soup.find_all('table', class_='inline')

    for table in menu_tables:
        # Extraer las filas de la tabla
        rows = table.find_all('tr')
        current_day = ""
        current_menu = ""
        menu_details = {"Entrante": "", "Primero": "", "Segundo": "", "Acompañamiento": "", "Postre": ""}

        for row in rows:
            # Extraer el día
            day_header = row.find('th', colspan="2")
            if day_header:
                # Si ya tenemos un menú recogido, lo agregamos a la lista antes de empezar un nuevo día
                if current_day and current_menu:
                    menus.append([current_day, current_menu, menu_details["Entrante"], menu_details["Primero"], menu_details["Segundo"], menu_details["Acompañamiento"], menu_details["Postre"]])
                    # Reseteamos detalles del menú para el siguiente
                    menu_details = {"Entrante": "", "Primero": "", "Segundo": "", "Acompañamiento": "", "Postre": ""}

                current_day = day_header.get_text(strip=True)
                continue

            # Identificar si es un menú
            menu_header = row.find('td', colspan="2")
            if menu_header and 'Menú' in menu_header.get_text():
                # Guardar el menú anterior si hay uno ya en proceso
                if current_menu and any(menu_details.values()):
                    menus.append([current_day, current_menu, menu_details["Entrante"], menu_details["Primero"], menu_details["Segundo"], menu_details["Acompañamiento"], menu_details["Postre"]])
                    # Reseteamos detalles del menú para el siguiente
                    menu_details = {"Entrante": "", "Primero": "", "Segundo": "", "Acompañamiento": "", "Postre": ""}

                current_menu = menu_header.get_text(strip=True)
                continue

            # Extraer los detalles del menú
            cells = row.find_all('td', class_='leftalign')
            if len(cells) == 2:
                dish_type = cells[0].get_text(strip=True)
                dish_name = cells[1].get_text(strip=True)
                if dish_type in menu_details:
                    menu_details[dish_type] = dish_name

        # Agregar el último menú del día a la lista
        if current_day and current_menu:
            menus.append([current_day, current_menu, menu_details["Entrante"], menu_details["Primero"], menu_details["Segundo"], menu_details["Acompañamiento"], menu_details["Postre"]])

    return menus

def save_to_csv(menus):
    with open(output_csv, mode='w', newline='', encoding='utf-8') as file:
        writer = csv.writer(file)
        # Encabezados
        writer.writerow(['Día', 'Menú', 'Entrante', 'Primero', 'Segundo', 'Acompañamiento', 'Postre'])

        # Escribir los menús
        for menu in menus:
            writer.writerow(menu)

# Ejecutar el script
menus = extract_menus()
save_to_csv(menus)

print(f"Información extraída y guardada en {output_csv}")
