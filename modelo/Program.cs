using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // Rutas a los archivos CSV (ajusta estas rutas según tu estructura de carpetas)
        string alumnosCsvPath = "../data_csv/alumnos_asignaturas.csv";
        string profesoresCsvPath = "../data_csv/profesores_ugr.csv";
        string horariosCsvPath = "../data_csv/horarios_ugr.csv";
        string menuCsvPath = "../data_csv/menu_semanal.csv";

        // Crear una instancia de DatabaseModel
        DatabaseModel dbModel = new DatabaseModel(alumnosCsvPath, profesoresCsvPath, horariosCsvPath, menuCsvPath);

        // 1. Obtener y mostrar todas las asignaturas
        Console.WriteLine("Asignaturas disponibles:");
        List<string> asignaturas = dbModel.GetAsignaturas();
        foreach (var asignatura in asignaturas)
        {
            Console.WriteLine(asignatura);
        }
        Console.WriteLine();

        // 2. Obtener y mostrar alumnos de una asignatura específica
        string asignaturaEjemplo = "Informática Industrial";
        Console.WriteLine($"Alumnos de {asignaturaEjemplo}:");
        List<Alumno> alumnos = dbModel.GetAlumnosByAsignatura(asignaturaEjemplo);
        foreach (var alumno in alumnos)
        {
            Console.WriteLine($"Nombre: {alumno.Nombre}, Grupo: {alumno.Grupo}");
        }
        Console.WriteLine();

        // 3. Obtener y mostrar horarios de una asignatura específica
        string asignaturaHorarios = "Cálculo";
        Console.WriteLine($"Horarios de {asignaturaHorarios}:");
        List<Horario> horarios = dbModel.GetHorariosByAsignatura(asignaturaHorarios);
        foreach (var horario in horarios)
        {
            Console.WriteLine($"Asignatura: {horario.Nombre}, Grupo: {horario.Grupo}, Aula: {horario.Aula}, Profesor: {horario.Profesor}, Horario: {horario.HorarioClase}");
        }
        Console.WriteLine();

        // 4. Obtener y mostrar profesores que enseñan una asignatura específica
        string asignaturaProfesores = "Metodología de la Programación";
        Console.WriteLine($"Profesores de {asignaturaProfesores}:");
        List<Profesor> profesores = dbModel.GetProfesoresByAsignatura(asignaturaProfesores);
        foreach (var profesor in profesores)
        {
            Console.WriteLine($"Nombre: {profesor.Nombre}, Departamento: {profesor.Departamento}, Horario de Tutorías: {profesor.HorarioTutorias}");
        }
        Console.WriteLine();

        // 5. Obtener y mostrar menús del comedor para un día específico
        string diaMenu = "LUNES";
        Console.WriteLine($"Menús del comedor para {diaMenu}:");
        List<MenuComedor> menus = dbModel.GetMenusByDia(diaMenu);
        foreach (var menu in menus)
        {
            Console.WriteLine($"Tipo de Menú: {menu.TipoMenu}, Cremas y Sopas: {menu.CremasSopas}, Entrante: {menu.Entrante}, Primero: {menu.Primero}, Segundo: {menu.Segundo}, Acompañamiento: {menu.Acompanamiento}, Postre: {menu.Postre}, Alérgenos: {menu.Alergenos}");
        }
        Console.WriteLine();

        // 6. Obtener y mostrar horarios por profesor
        string profesorHorarios = "Abraham Rueda Zoca";
        Console.WriteLine($"Horarios impartidos por {profesorHorarios}:");
        List<Horario> horariosPorProfesor = dbModel.GetHorariosByAsignatura(profesorHorarios);
        foreach (var horario in horariosPorProfesor)
        {
            Console.WriteLine($"Asignatura: {horario.Nombre}, Grupo: {horario.Grupo}, Aula: {horario.Aula}, Profesor: {horario.Profesor}, Horario: {horario.HorarioClase}");
        }
        Console.WriteLine();

        // 7. Solicitar el nombre de un alumno y mostrar las asignaturas en las que está matriculado
        Console.WriteLine("Introduce el nombre de un alumno para ver sus asignaturas:");
        string nombreAlumno = Console.ReadLine();
        List<string> asignaturasAlumno = dbModel.GetAsignaturasByAlumno(nombreAlumno);
        if (asignaturasAlumno.Count > 0)
        {
            Console.WriteLine($"{nombreAlumno} está matriculado en las siguientes asignaturas:");
            foreach (var asignatura in asignaturasAlumno)
            {
                Console.WriteLine(asignatura);
            }
        }
        else
        {
            Console.WriteLine($"No se encontraron asignaturas para el alumno: {nombreAlumno}");
        }

        Console.WriteLine("\nFin de las pruebas.");
    }
}
