using System;
using System.Collections.Generic;
using System.Linq;


class Program
{
    static void Main(string[] args)
    {
        // Rutas relativas de las bases de datos
        string alumnosCsvPath = "../data_csv/alumnos_asignaturas.csv";
        string profesoresCsvPath = "../data_csv/profesores_ugr.csv";
        string horariosCsvPath = "../data_csv/horarios_ugr.csv";
        string menuCsvPath = "../data_csv/menu_semanal.csv";

        // Inicializamos DatabaseModel con las rutas correctas
        DatabaseModel db = new DatabaseModel(alumnosCsvPath, profesoresCsvPath, horariosCsvPath, menuCsvPath);

        // Inicializamos controladores
        var alumnoController = new AlumnoController(db.Alumnos);
        var profesorController = new ProfesorController(db.Profesores);
        var horarioController = new HorarioController(db.Horarios);
        var menuController = new MenuComedorController(db.Menus);
        var asignaturaController = new AsignaturaController(db.Asignaturas);

        // Continuamos con el menú principal
        while (true)
        {
            Console.Clear();
            Console.WriteLine("====== Gestión de Datos ======");
            Console.WriteLine("1. Buscar alumnos");
            Console.WriteLine("2. Buscar profesores");
            Console.WriteLine("3. Buscar horarios");
            Console.WriteLine("4. Consultar menú del comedor");
            Console.WriteLine("5. Consultar asignaturas");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione una opción: ");

            var opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    GestionarAlumnos(alumnoController);
                    break;
                case "2":
                    GestionarProfesores(profesorController);
                    break;
                case "3":
                    GestionarHorarios(horarioController);
                    break;
                case "4":
                    ConsultarMenu(menuController);
                    break;
                case "5":
                    ConsultarAsignaturas(asignaturaController);
                    break;
                case "6":
                    Console.WriteLine("Saliendo...");
                    return;
                default:
                    Console.WriteLine("Opción no válida. Presione cualquier tecla para continuar.");
                    Console.ReadKey();
                    break;
            }
        }
    }


    static void GestionarAlumnos(AlumnoController alumnoController)
    {
        Console.Clear();
        Console.WriteLine("===== Gestión de Alumnos =====");
        Console.Write("Ingrese el nombre del alumno (o vacío para listar todos): ");
        var nombre = Console.ReadLine();
        var alumnos = string.IsNullOrWhiteSpace(nombre) 
            ? alumnoController.BuscarPorNombre("") 
            : alumnoController.BuscarPorNombre(nombre);

        Console.WriteLine("\nResultados:");
        alumnos.ForEach(a => Console.WriteLine($"Nombre: {a.Nombre}, Asignatura: {a.Asignatura}, Grupo: {a.Grupo}"));
        Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
        Console.ReadKey();
    }

    static void GestionarProfesores(ProfesorController profesorController)
    {
        Console.Clear();
        Console.WriteLine("Gestión de Profesores");
        Console.WriteLine("1. Listar todos los profesores");
        Console.WriteLine("2. Buscar profesor por nombre");
        Console.WriteLine("3. Volver al menú principal");

        Console.Write("Selecciona una opción: ");
        string opcion = Console.ReadLine();

        switch (opcion)
        {
            case "1":
                Console.Clear();
                Console.WriteLine("Listado de Profesores:");
                var profesores = profesorController.ListarTodosLosProfesores();
                foreach (var profesor in profesores)
                {
                    Console.WriteLine(profesor.Nombre);
                }
                break;

            case "2":
                Console.Clear();
                Console.Write("Introduce el nombre del profesor que quieres buscar: ");
                string nombreBuscado = Console.ReadLine();

                Console.WriteLine($"Buscando al profesor '{nombreBuscado}'...");

                var profesorEncontrado = profesorController.BuscarProfesorPorNombre(nombreBuscado);

                if (profesorEncontrado != null)
                {
                    Console.WriteLine("Información del Profesor:");
                    Console.WriteLine($"Nombre: {profesorEncontrado.Nombre}");
                    Console.WriteLine($"Departamento: {profesorEncontrado.Departamento}");
                    Console.WriteLine($"Asignaturas: {string.Join(", ", profesorEncontrado.Asignaturas)}");
                    Console.WriteLine($"Horario de Tutorías: {profesorEncontrado.HorarioTutorias}");
                    Console.WriteLine($"URL: {profesorEncontrado.Url}");
                }
                else
                {
                    Console.WriteLine($"No se encontró un profesor con el nombre '{nombreBuscado}'.");
                }
                break;

            case "3":
                Console.WriteLine("Volviendo al menú principal...");
                return;

            default:
                Console.WriteLine("Opción no válida. Por favor, selecciona una opción válida.");
                break;
        }

        Console.WriteLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }


    static void GestionarHorarios(HorarioController horarioController)
    {
        Console.Clear();
        Console.WriteLine("===== Gestión de Horarios =====");
        Console.Write("Ingrese el nombre de la asignatura (o vacío para listar todos): ");
        var nombre = Console.ReadLine();
        var horarios = string.IsNullOrWhiteSpace(nombre) 
            ? horarioController.BuscarPorAsignatura("") 
            : horarioController.BuscarPorAsignatura(nombre);

        Console.WriteLine("\nResultados:");
        horarios.ForEach(h => Console.WriteLine($"Nombre: {h.Nombre}, Grupo: {h.Grupo}, Aula: {h.Aula}, Horario: {h.HorarioClase}, Profesor: {h.Profesor}"));
        Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
        Console.ReadKey();
    }

    static void ConsultarMenu(MenuComedorController menuController)
    {
        Console.Clear();
        Console.WriteLine("===== Consultar Menú del Comedor =====");
        Console.Write("Ingrese el día (o vacío para listar todos): ");
        var dia = Console.ReadLine();
        var menus = string.IsNullOrWhiteSpace(dia) 
            ? menuController.BuscarPorDia("") 
            : menuController.BuscarPorDia(dia);

        Console.WriteLine("\nResultados:");
        menus.ForEach(m => Console.WriteLine($"Día: {m.Dia}, Tipo: {m.TipoMenu}, Entrante: {m.Entrante}, Postre: {m.Postre}"));
        Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
        Console.ReadKey();
    }

    static void ConsultarAsignaturas(AsignaturaController asignaturaController)
    {
        Console.Clear();
        Console.WriteLine("===== Consultar Asignaturas =====");
        Console.Write("Ingrese el nombre de la asignatura (o vacío para listar todas): ");
        var nombre = Console.ReadLine();
        var asignaturas = string.IsNullOrWhiteSpace(nombre) 
            ? asignaturaController.BuscarPorNombre("") 
            : asignaturaController.BuscarPorNombre(nombre);

        Console.WriteLine("\nResultados:");
        asignaturas.ForEach(a => Console.WriteLine(a.ToString()));
        Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
        Console.ReadKey();
    }
    
}
