using System;
using System.Collections.Generic;
using System.Linq;

public class DatabaseModel
{
    public List<Alumno> Alumnos { get; private set; }
    public List<Profesor> Profesores { get; private set; }
    public List<Horario> Horarios { get; private set; }
    public List<MenuComedor> Menus { get; private set; }
    public List<Asignatura> Asignaturas { get; private set; }

    public DatabaseModel(string alumnosCsvPath, string profesoresCsvPath, string horariosCsvPath, string menuCsvPath)
    {
        Alumnos = LoadAlumnos(alumnosCsvPath);
        Profesores = LoadProfesores(profesoresCsvPath);
        Horarios = LoadHorarios(horariosCsvPath);
        Menus = LoadMenus(menuCsvPath);
        Asignaturas = ExtractAsignaturas();
    }

    private List<Alumno> LoadAlumnos(string filePath)
    {
        var alumnosList = new List<Alumno>();
        var lines = System.IO.File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1))
        {
            var data = ParseCsvLine(line);
            if (data.Length >= 3)
            {
                alumnosList.Add(new Alumno(
                    data[0].Trim('"'),
                    data[1].Trim(),
                    data[2].Trim()));
            }
        }
        return alumnosList;
    }

    private List<Profesor> LoadProfesores(string filePath)
    {
        var profesoresList = new List<Profesor>();
        var lines = System.IO.File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1))
        {
            var data = line.Split(',');
            if (data.Length >= 5)
            {
                profesoresList.Add(new Profesor(
                    data[0].Trim(),
                    data[1].Trim(),
                    data[2].Split(';').Select(a => a.Trim()).ToList(),
                    data[3].Trim(),
                    data[4].Trim()));
            }
        }
        return profesoresList;
    }

    private List<Horario> LoadHorarios(string filePath)
    {
        var horariosList = new List<Horario>();
        var lines = System.IO.File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1))
        {
            var data = ParseCsvLine(line);
            if (data.Length >= 9)
            {
                horariosList.Add(new Horario(
                    data[0].Trim(),
                    data[1].Trim(),
                    data[2].Trim(),
                    data[3].Trim(),
                    data[4].Trim(),
                    data[5].Trim(),
                    data[6].Trim(),
                    data[7].Trim(),
                    data[8].Trim()));
            }
        }
        return horariosList;
    }

    private List<MenuComedor> LoadMenus(string filePath)
    {
        var menusList = new List<MenuComedor>();
        var lines = System.IO.File.ReadAllLines(filePath);

        foreach (var line in lines.Skip(1))
        {
            var data = line.Split(',');
            if (data.Length >= 9)
            {
                menusList.Add(new MenuComedor(
                    data[0].Trim('"'),
                    data[1].Trim(),
                    data[2].Trim(),
                    data[3].Trim(),
                    data[4].Trim(),
                    data[5].Trim(),
                    data[6].Trim(),
                    data[7].Trim(),
                    data[8].Trim()));
            }
        }
        return menusList;
    }

    private List<Asignatura> ExtractAsignaturas()
    {
        var asignaturasList = new List<Asignatura>();
        var asignaturaNames = Horarios.Select(h => h.Nombre).Distinct();

        foreach (var nombre in asignaturaNames)
        {
            var horarios = Horarios.Where(h => h.Nombre == nombre).ToList();
            asignaturasList.Add(new Asignatura(nombre, horarios));
        }

        return asignaturasList;
    }

    private string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var current = string.Empty;
        var inQuotes = false;

        foreach (var c in line)
        {
            if (c == '"' && !inQuotes)
                inQuotes = true;
            else if (c == '"' && inQuotes)
                inQuotes = false;
            else if (c == ',' && !inQuotes)
            {
                result.Add(current);
                current = string.Empty;
            }
            else
                current += c;
        }
        result.Add(current);
        return result.ToArray();
    }
}
