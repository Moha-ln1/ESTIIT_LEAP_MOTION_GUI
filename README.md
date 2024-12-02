
# **Gestión Universidad**

Este proyecto es una solución modular que permite gestionar información académica como alumnos, asignaturas, horarios, profesores y menús de comedor. Está diseñado para integrarse fácilmente en proyectos que requieran una interfaz gráfica o textual, proporcionando una base sólida para aplicaciones más complejas.

## **Índice**
- [Descripción](#descripción)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Requisitos](#requisitos)
- [Instalación](#instalación)
- [Uso](#uso)
- [Ejecución](#ejecución)
- [Clases Principales](#clases-principales)
  - [Models](#models)
  - [Controllers](#controllers)
  - [CsvDataLoader](#csvdataloader)
- [Extensiones Posibles](#extensiones-posibles)

---

## **Descripción**

Gestión Universidad es un proyecto que organiza y gestiona datos académicos. Los datos se cargan desde archivos CSV y se procesan para generar horarios, filtrar asignaturas, listar menús del comedor y más. La arquitectura es modular, facilitando su integración con distintas interfaces gráficas como Unity o Flutter.

---

## **Estructura del Proyecto**

```
GestionUniversidad/
├── Models/                       # Modelos de datos
│   ├── Alumno.cs
│   ├── Profesor.cs
│   ├── Horario.cs
│   ├── MenuComedor.cs
│   └── Asignatura.cs
│   └── Usuario.cs                #Gestor QR
├── Services/                     # Servicios de soporte
│   ├── CsvDataLoader.cs          # Carga y procesamiento de archivos CSV
│   └── DatabaseModel.cs          # Modelo central de datos
├── Controllers/                  # Controladores para cada funcionalidad
│   ├── AlumnoController.cs
│   ├── ProfesorController.cs
│   ├── HorarioController.cs
│   ├── MenuController.cs
│   └── AsignaturaController.cs
│   └── UsuarioController.cs
├── data/                         # Archivos CSV
│   ├── alumnos_asignaturas.csv
│   ├── profesores_ugr.csv
│   ├── horarios_ugr.csv
│   └── menu_semanal.csv
├── data_fuentes/                 # PDF con info de los alumnos
├── scrappers_scripts/                 # Scrappers html / pdf 
|   ├── database_formater.py
|   ├── scrappers_scripts.py
|   ├── scrappers_alumnos.py 
|   ├── scrappers_comedores.py
|   ├── scrappers_horarios.py
|   └── scrappers_profesores.py
|   
├── Program.cs                    # Interfaz textual principal (main)
└── README.md                     # Documentación del proyecto
```

---

## **Requisitos**

- [.NET 6 o superior](https://dotnet.microsoft.com/download)
- Editor de texto o IDE como Visual Studio Code o JetBrains Rider.
- Archivos CSV con los datos requeridos (se encuentran en la carpeta `Data`).

---

## **Instalación**

1. Clona este repositorio:
   ```bash
   git clone https://github.com/tu_usuario/GestionUniversidad.git
   cd GestionUniversidad
   ```

2. Restaura las dependencias y compila el proyecto:
   ```bash
   dotnet build
   ```

3. Asegúrate de que los archivos CSV estén en la carpeta `Data/`.

---

## **Uso**

Este proyecto está diseñado para ser ampliado con interfaces gráficas. La lógica de negocio y los datos están completamente separados de la presentación, permitiendo una integración limpia con frameworks como:

- **Unity:** Para simulaciones académicas o juegos educativos.
- **Flutter:** Para aplicaciones móviles multiplataforma.
- **Windows Forms o WPF:** Para aplicaciones de escritorio.
De aqui elegir lo que os de la gana pero son las recomendaciones que me hizo gpt

### **Cómo trabajar con los datos**

1. **Carga de Datos:**
   Los datos se cargan desde los archivos CSV mediante `CsvDataLoader`. Esto incluye:
   - Alumnos con sus asignaturas y grupos.
   - Asignaturas con sus horarios.
   - Profesores con sus asignaturas y horarios de tutorías.
   - Menús del comedor.

2. **Acceso a la lógica de negocio:**
   Usa los controladores (`Controllers/`) para manejar datos. Ejemplo:
   - Buscar un alumno por nombre.
   - Generar horarios basados en asignaturas.
   - Listar menús por día.

3. **Generar horarios de alumnos:**
   ```csharp
   var alumnoController = new AlumnoController(alumnos);
   var asignaturaController = new AsignaturaController(asignaturas);

   var horario = alumnoController.GenerarHorarioPorAlumno("John Doe", asignaturaController.ObtenerTodas());
   foreach (var (asignatura, detalles) in horario)
   {
       Console.WriteLine($"Asignatura: {asignatura}");
       detalles.ForEach(d => Console.WriteLine($"  Detalle: {d}"));
   }
   ```

---

## **Ejecución**

1. Para ejecutar el programa en consola:
   ```bash
   dotnet run
   ```

2. Sigue las opciones del menú textual para interactuar con el sistema:
   - Buscar alumnos, profesores o asignaturas.
   - Filtrar horarios o menús.
   - Generar horarios completos por alumno.

---

## **Clases Principales**

### **Models**

1. **Alumno.cs**
   Representa un estudiante con su lista de asignaturas y grupos.

2. **Asignatura.cs**
   Contiene el nombre, las siglas y los horarios disponibles para una asignatura.

3. **Horario.cs**
   Detalla los horarios para cada asignatura, incluyendo el grupo, aula y profesor.

4. **MenuComedor.cs**
   Representa un menú semanal con detalles como cremas, sopas, entrantes, y alérgenos.

5. **Profesor.cs**
   Contiene información del profesor, como departamento, asignaturas y horarios de tutoría.

---

### **Controllers**

1. **AlumnoController.cs**
   Gestiona alumnos: búsquedas, filtrados y generación de horarios.

2. **AsignaturaController.cs**
   Gestiona asignaturas: búsquedas por nombre o siglas, y listado completo.

3. **HorarioController.cs**
   Facilita el listado y búsqueda de asignaturas junto con sus horarios.

4. **MenuController.cs**
   Gestiona menús: búsquedas por día y filtrado por alérgenos.

5. **ProfesorController.cs**
   Gestiona profesores: búsqueda por nombre y listado.

---

### **CsvDataLoader**

`CsvDataLoader` es responsable de leer los datos de los archivos CSV y convertirlos en objetos que las demás clases puedan utilizar.

---

