using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class DatabaseModel
{
    private List<Asignatura> asignaturas;
    private List<Alumno> alumnos;
    private List<Horario> horarios;

    public DatabaseModel(string alumnosCsvPath, string horariosCsvPath)
    {
        alumnos = LoadAlumnos(alumnosCsvPath);
        horarios = LoadHorarios(horariosCsvPath);
        asignaturas = ExtractAsignaturas();
    }

    // Cargar los alumnos desde el archivo CSV
    private List<Alumno> LoadAlumnos(string filePath)
    {
        var alumnosList = new List<Alumno>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1)) // Skip header
        {
            var data = line.Split(',');
            if (data.Length >= 3)
            {
                alumnosList.Add(new Alumno
                {
                    Nombre = data[0].Trim('\"'),
                    Asignatura = data[1].Trim(),
                    Grupo = data[2].Trim()
                });
            }
        }
        return alumnosList;
    }

    // Cargar los horarios desde el archivo CSV
    private List<Horario> LoadHorarios(string filePath)
    {
        var horariosList = new List<Horario>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1)) // Skip header
        {
            var data = line.Split(',');
            if (data.Length >= 8)
            {
                horariosList.Add(new Horario
                {
                    Nombre = data[0].Trim(),
                    Siglas = data[1].Trim(),
                    Grupo = data[2].Trim(),
                    Aula = data[3].Trim(),
                    FechaInicio = DateTime.ParseExact(data[4].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    FechaFinal = DateTime.ParseExact(data[5].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    HorarioClase = data[6].Trim(),
                    Profesor = data[7].Trim(),
                    Url = data[8].Trim()
                });
            }
        }
        return horariosList;
    }

    // Extraer asignaturas únicas de los horarios y alumnos
    private List<Asignatura> ExtractAsignaturas()
    {
        var asignaturasSet = new HashSet<string>(alumnos.Select(a => a.Asignatura).Union(horarios.Select(h => h.Nombre)));
        return asignaturasSet.Select(a => new Asignatura { Nombre = a }).ToList();
    }

    // Consultas
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

    public List<Horario> GetHorariosByProfesor(string profesor)
    {
        return horarios.Where(h => h.Profesor.Equals(profesor, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Alumno> GetAlumnosByGrupo(string asignatura, string grupo)
    {
        return alumnos.Where(a => a.Asignatura.Equals(asignatura, StringComparison.OrdinalIgnoreCase) && a.Grupo.Equals(grupo, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Horario> GetHorariosByAula(string aula)
    {
        return horarios.Where(h => h.Aula.Equals(aula, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Horario> GetHorariosBetweenDates(DateTime startDate, DateTime endDate)
    {
        return horarios.Where(h => h.FechaInicio >= startDate && h.FechaFinal <= endDate).ToList();
    }

    public void PrintAlumnos(List<Alumno> alumnos)
    {
        foreach (var alumno in alumnos)
        {
            Console.WriteLine($"Nombre: {alumno.Nombre}, Asignatura: {alumno.Asignatura}, Grupo: {alumno.Grupo}");
        }
    }

    public void PrintHorarios(List<Horario> horarios)
    {
        foreach (var horario in horarios)
        {
            Console.WriteLine($"Asignatura: {horario.Nombre}, Grupo: {horario.Grupo}, Aula: {horario.Aula}, Profesor: {horario.Profesor}, Horario: {horario.HorarioClase}");
        }
    }
}

// Clases de datos
public class Alumno
{
    public string Nombre { get; set; }
    public string Asignatura { get; set; }
    public string Grupo { get; set; }
}

public class Horario
{
    public string Nombre { get; set; }
    public string Siglas { get; set; }
    public string Grupo { get; set; }
    public string Aula { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFinal { get; set; }
    public string HorarioClase { get; set; }
    public string Profesor { get; set; }
    public string Url { get; set; }
}

public class Asignatura
{
    public string Nombre { get; set; }
}
