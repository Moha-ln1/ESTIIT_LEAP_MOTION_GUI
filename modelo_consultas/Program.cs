using System;

class Program
{
    static void Main(string[] args)
    {
        // Rutas a los archivos CSV
        string alumnosCsvPath = "../data_csv/alumnos_asignaturas.csv";
        string horariosCsvPath = "../data_csv/horarios_ugr.csv";

        // Crear una instancia de DatabaseModel
        var databaseModel = new DatabaseModel(alumnosCsvPath, horariosCsvPath);

        // Ejemplo de consultas
        var asignaturas = databaseModel.GetAsignaturas();
        Console.WriteLine("Asignaturas disponibles:");
        foreach (var asignatura in asignaturas)
        {
            Console.WriteLine(asignatura);
        }

        var alumnosDeAsignatura = databaseModel.GetAlumnosByAsignatura("Informática Industrial");
        Console.WriteLine("\nAlumnos de Informática Industrial:");
        databaseModel.PrintAlumnos(alumnosDeAsignatura);

        var horariosDeAsignatura = databaseModel.GetHorariosByAsignatura("Cálculo");
        Console.WriteLine("\nHorarios de Cálculo:");
        databaseModel.PrintHorarios(horariosDeAsignatura);

        var horariosPorProfesor = databaseModel.GetHorariosByProfesor("Abraham Rueda Zoca");
        Console.WriteLine("\nHorarios impartidos por Abraham Rueda Zoca:");
        databaseModel.PrintHorarios(horariosPorProfesor);

        var alumnosDelGrupo = databaseModel.GetAlumnosByGrupo("Informática Industrial", "A1");
        Console.WriteLine("\nAlumnos en Informática Industrial, Grupo A1:");
        databaseModel.PrintAlumnos(alumnosDelGrupo);
    }
}
