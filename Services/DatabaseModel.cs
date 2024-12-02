using System.Collections.Generic;
using GestionUniversidad.Models;

public class DatabaseModel
{
    public List<Alumno> Alumnos { get; private set; }
    public List<Asignatura> Asignaturas { get; private set; }
    public List<MenuComedor> Menus { get; private set; }
    public List<Profesor> Profesores { get; private set; }

    public DatabaseModel(CsvDataLoader csvLoader, string alumnosCsvPath, string profesoresCsvPath, string horariosCsvPath, string menuCsvPath)
    {
        Alumnos = csvLoader.LoadAlumnos(alumnosCsvPath);
        Asignaturas = csvLoader.LoadAsignaturas(horariosCsvPath);
        Menus = csvLoader.LoadMenus(menuCsvPath);
        Profesores = csvLoader.LoadProfesores(profesoresCsvPath);
    }
}
