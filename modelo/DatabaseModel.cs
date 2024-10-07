using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class DatabaseModel
{
    private List<Alumno> alumnos;
    private List<Profesor> profesores;
    private List<Asignatura> asignaturas;
    private List<Horario> horarios;
    private List<MenuComedor> menus;

    public DatabaseModel(string alumnosCsvPath, string profesoresCsvPath, string horariosCsvPath, string menuCsvPath)
    {
        alumnos = LoadAlumnos(alumnosCsvPath);
        profesores = LoadProfesores(profesoresCsvPath);
        horarios = LoadHorarios(horariosCsvPath);
        menus = LoadMenus(menuCsvPath);
        asignaturas = ExtractAsignaturas();
    }

    // Cargar los alumnos desde el archivo CSV
    private List<Alumno> LoadAlumnos(string filePath)
    {
        var alumnosList = new List<Alumno>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1)) // Saltar la cabecera
        {
            // Uso de expresiones regulares para extraer correctamente los valores entre comillas
            var match = System.Text.RegularExpressions.Regex.Matches(line, "(\"[^\"]+\"|[^,]+)");
            if (match.Count >= 3)
            {
                alumnosList.Add(new Alumno
                {
                    Nombre = match[0].Value.Trim('"').Trim(),
                    Asignatura = match[1].Value.Trim(),
                    Grupo = match[2].Value.Trim()
                });
            }
        }
        return alumnosList;
    }


    // Cargar los profesores desde el archivo CSV
    private List<Profesor> LoadProfesores(string filePath)
    {
        var profesoresList = new List<Profesor>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1)) // Saltar encabezado
        {
            var data = line.Split(',');
            if (data.Length >= 5)
            {
                profesoresList.Add(new Profesor
                {
                    Nombre = data[0].Trim(),
                    Departamento = data[1].Trim(),
                    Asignaturas = data[2].Split(';').Select(a => a.Trim()).ToList(),
                    HorarioTutorias = data[3].Trim(),
                    Url = data[4].Trim()
                });
            }
        }
        return profesoresList;
    }

    // Cargar los horarios desde el archivo CSV
    private List<Horario> LoadHorarios(string filePath)
    {
        var horariosList = new List<Horario>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1)) // Saltar encabezado
        {
            var data = line.Split(',');
            if (data.Length >= 9)
            {
                horariosList.Add(new Horario
                {
                    Nombre = data[0].Trim(),
                    Siglas = data[1].Trim(),
                    Grupo = data[2].Trim(),
                    Aula = data[3].Trim(),
                    FechaInicio = data[4].Trim(),
                    FechaFinal = data[5].Trim(),
                    HorarioClase = data[6].Trim(),
                    Profesor = data[7].Trim(),
                    Url = data[8].Trim()
                });
            }
        }
        return horariosList;
    }

    // Cargar los menús desde el archivo CSV
    private List<MenuComedor> LoadMenus(string filePath)
    {
        var menusList = new List<MenuComedor>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1)) // Saltar encabezado
        {
            var data = line.Split(',');
            if (data.Length >= 9)
            {
                menusList.Add(new MenuComedor
                {
                    Dia = data[0].Trim(),
                    TipoMenu = data[1].Trim(),
                    CremasSopas = data[2].Trim(),
                    Entrante = data[3].Trim(),
                    Primero = data[4].Trim(),
                    Segundo = data[5].Trim(),
                    Acompanamiento = data[6].Trim(),
                    Postre = data[7].Trim(),
                    Alergenos = data[8].Trim()
                });
            }
        }
        return menusList;
    }

    // Extraer asignaturas únicas
    private List<Asignatura> ExtractAsignaturas()
    {
        var asignaturasSet = new HashSet<string>(alumnos.Select(a => a.Asignatura).Union(horarios.Select(h => h.Nombre)));
        return asignaturasSet.Select(a => new Asignatura { Nombre = a }).ToList();
    }

    // Consultas (ejemplos)
    public List<string> GetAsignaturas()
    {
        return asignaturas.Select(a => a.Nombre).ToList();
    }

    public List<Alumno> GetAlumnosByAsignatura(string asignatura)
    {
        return alumnos.Where(a => a.Asignatura.Equals(asignatura, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Horario> GetHorariosByAsignatura(string asignatura)
    {
        return horarios.Where(h => h.Nombre.Equals(asignatura, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Profesor> GetProfesoresByAsignatura(string asignatura)
    {
        return profesores.Where(p => p.Asignaturas.Contains(asignatura, StringComparer.OrdinalIgnoreCase)).ToList();
    }

    public List<MenuComedor> GetMenusByDia(string dia)
    {
        return menus.Where(m => m.Dia.Contains(dia, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    public List<string> GetAsignaturasByAlumno(string nombreAlumno)
    {
        var asignaturasAlumno = alumnos
            .Where(a => a.Nombre.Equals(nombreAlumno, StringComparison.OrdinalIgnoreCase))
            .Select(a => a.Asignatura)
            .Distinct()
            .ToList();

        return asignaturasAlumno;
    }

}
