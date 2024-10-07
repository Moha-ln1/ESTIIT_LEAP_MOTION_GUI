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
    writer.writerow(['Día', 'Tipo de Menú', 'Entrante', 'Plato Principal', 'Acompañamiento', 'Postre', 'Alérgenos'])

    for menu in menus:
        rows = menu.find_all('tr')
        day = ''
        menu_type = ''
        entrante = ''
        plato_principal = ''
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
                    if entrante or plato_principal or acompanamiento or postre:
                        writer.writerow([day, menu_type, entrante, plato_principal, acompanamiento, postre, alergenos])
                    menu_type = columns[0].text.strip()
                    entrante = ''
                    plato_principal = ''
                    acompanamiento = ''
                    postre = ''
                    alergenos = ''
                else:
                    category = columns[0].text.strip()
                    value = columns[1].text.strip()
                    alergen = columns[2].text.strip() if len(columns) > 2 else ''
                    
                    if category == 'Entrante':
                        entrante = value
                    elif category == 'Primero':
                        plato_principal = value
                    elif category == 'Acompañamiento':
                        acompanamiento = value
                    elif category == 'Postre':
                        postre = value
                    
                    if alergen:
                        alergenos += f"{alergen} "

        if entrante or plato_principal or acompanamiento or postre:
            writer.writerow([day, menu_type, entrante, plato_principal, acompanamiento, postre, alergenos])

print(f"Información extraída y guardada en {output_csv}")
