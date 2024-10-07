import requests
from bs4 import BeautifulSoup
import csv

# URL de la página web de los comedores
url = 'https://scu.ugr.es/pages/menu/comedor'

# Realizar la solicitud a la página web
response = requests.get(url)
soup = BeautifulSoup(response.content, 'html.parser')

# Encontrar las tablas de los menús semanales
menus = soup.find_all('table', class_='inline')

# Nombre del archivo CSV de salida
output_csv = './data_csv/menu_semanal.csv'

# Guardar datos en el CSV
with open(output_csv, 'w', newline='', encoding='utf-8') as file:
    writer = csv.writer(file)
    writer.writerow(['Día', 'Tipo de Menú', 'Cremas y Sopas', 'Entrante', 'Primero', 'Segundo', 'Acompañamiento', 'Postre', 'Alérgenos'])

    for menu in menus:
        rows = menu.find_all('tr')
        day = ''
        menu_type = ''
        cremas_y_sopas = ''
        entrante = ''
        primero = ''
        segundo = ''
        acompanamiento = ''
        postre = ''
        alergenos = ''

        for row in rows:
            headers = row.find_all('th')
            if headers:
                day = headers[0].text.strip()
            else:
                columns = row.find_all('td')
                if 'Menú' in columns[0].text:
                    # Guardar el menú anterior antes de reiniciar los valores
                    if primero or segundo or acompanamiento or postre or cremas_y_sopas:
                        writer.writerow([day, menu_type, cremas_y_sopas, entrante, primero, segundo, acompanamiento, postre, alergenos.strip()])
                    
                    # Actualizar el tipo de menú y resetear los valores
                    menu_type = columns[0].text.strip()
                    cremas_y_sopas = ''
                    entrante = ''
                    primero = ''
                    segundo = ''
                    acompanamiento = ''
                    postre = ''
                    alergenos = ''
                else:
                    category = columns[0].text.strip()
                    value = columns[1].text.strip()
                    alergen = columns[2].text.strip() if len(columns) > 2 else ''
                    
                    if category == 'Cremas y Sopas':
                        cremas_y_sopas = value
                    elif category == 'Entrante':
                        entrante = value
                    elif category == 'Primero':
                        primero = value
                    elif category == 'Segundo':
                        segundo = value
                    elif category == 'Acompañamiento':
                        acompanamiento = value
                    elif category == 'Postre':
                        postre = value
                    
                    if alergen:
                        alergenos += f"{alergen} "

        # Guardar el último menú del día
        if primero or segundo or acompanamiento or postre or cremas_y_sopas:
            writer.writerow([day, menu_type, cremas_y_sopas, entrante, primero, segundo, acompanamiento, postre, alergenos.strip()])

print(f"Información extraída y guardada en {output_csv}")
