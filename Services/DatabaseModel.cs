using System.Collections.Generic;
using GestionUniversidad.Models;

public class DatabaseModel
{
    public List<Alumno> Alumnos { get; private set; }
    public List<Asignatura> Asignaturas { get; private set; }
    public List<MenuComedor> Menus { get; private set; }
    public List<Profesor> Profesores { get; private set; }
    public List<Usuario> Usuarios { get; private set; } // Usuarios generados a partir de alumnos

    public DatabaseModel(CsvDataLoader csvLoader, string alumnosCsvPath, string profesoresCsvPath, string horariosCsvPath, string menuCsvPath)
    {
        Alumnos = csvLoader.LoadAlumnos(alumnosCsvPath);
        Asignaturas = csvLoader.LoadAsignaturas(horariosCsvPath);
        Menus = csvLoader.LoadMenus(menuCsvPath);
        Profesores = csvLoader.LoadProfesores(profesoresCsvPath);
        Usuarios = csvLoader.LoadUsuarios(Alumnos); // Generar usuarios a partir de alumnos
    }

    // Método para agregar usuarios adicionales si es necesario
    public void AgregarUsuario(Usuario usuario)
    {
        Usuarios.Add(usuario);
    }
}
