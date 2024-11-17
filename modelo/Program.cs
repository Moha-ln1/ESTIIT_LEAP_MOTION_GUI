using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string alumnosCsvPath = "../data_csv/alumnos_asignaturas.csv";
        string profesoresCsvPath = "../data_csv/profesores_ugr.csv";
        string horariosCsvPath = "../data_csv/horarios_ugr.csv";
        string menuCsvPath = "../data_csv/menu_semanal.csv";

        DatabaseModel db = new DatabaseModel(alumnosCsvPath, profesoresCsvPath, horariosCsvPath, menuCsvPath);

        Console.WriteLine("10 Primeros Alumnos:");
        foreach (var alumno in db.Alumnos.Take(10))
        {
            Console.WriteLine($"Nombre: {alumno.Nombre}, Asignatura: {alumno.Asignatura}, Grupo: {alumno.Grupo}");
        }

        Console.WriteLine("\n10 Primeros Profesores:");
        foreach (var profesor in db.Profesores.Take(10))
        {
            Console.WriteLine($"Nombre: {profesor.Nombre}, Departamento: {profesor.Departamento}");
        }

        Console.WriteLine("\n10 Primeros Horarios:");
        foreach (var horario in db.Horarios.Take(10))
        {
            Console.WriteLine($"Asignatura: {horario.Nombre}, Grupo: {horario.Grupo}, Aula: {horario.Aula}");
        }

        Console.WriteLine("\n10 Primeros Menús del Comedor:");
        foreach (var menu in db.Menus.Take(10))
        {
            Console.WriteLine($"Día: {menu.Dia}, Tipo: {menu.TipoMenu}, Principal: {menu.Primero}, Postre: {menu.Postre}");
        }

        Console.WriteLine("\n3 Primeras Asignaturas:");
        foreach (var asignatura in db.Asignaturas.Take(3))
        {
            Console.WriteLine(asignatura.ToString());
            Console.WriteLine(new string('-', 50)); // Línea separadora entre asignaturas
        }


    }
}
