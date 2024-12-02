using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Rutas relativas a los archivos CSV
        string alumnosCsvPath = "./data/alumnos_asignaturas.csv";
        string profesoresCsvPath = "./data/profesores_ugr.csv";
        string horariosCsvPath = "./data/horarios_ugr.csv";
        string menuCsvPath = "./data/menu_semanal.csv";

        // Carga de datos
        var csvLoader = new CsvDataLoader();
        var database = new DatabaseModel(csvLoader, alumnosCsvPath, profesoresCsvPath, horariosCsvPath, menuCsvPath);

        var alumnoController = new AlumnoController(database.Alumnos);
        var profesorController = new ProfesorController(database.Profesores);
        var horarioController = new HorarioController(database.Asignaturas);
        var menuController = new MenuController(database.Menus);
        var usuarioController = new UsuarioController(database.Usuarios); // Incluyendo gestión de usuarios

        while (true)
        {
            Console.Clear();
            Console.WriteLine("====== Gestión de Datos ======");
            Console.WriteLine("1. Buscar alumnos");
            Console.WriteLine("2. Buscar profesores");
            Console.WriteLine("3. Gestión de horarios");
            Console.WriteLine("4. Consultar menú del comedor");
            Console.WriteLine("5. Gestión de usuarios"); // Nueva opción
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione una opción: ");

            var opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    GestionarAlumnos(alumnoController, horarioController);
                    break;
                case "2":
                    GestionarProfesores(profesorController);
                    break;
                case "3":
                    GestionarHorarios(horarioController, alumnoController);
                    break;
                case "4":
                    ConsultarMenu(menuController);
                    break;
                case "5":
                    GestionarUsuarios(usuarioController); // Nueva función de usuarios
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

    static void GestionarAlumnos(AlumnoController alumnoController, HorarioController horarioController)
    {
        Console.Clear();
        Console.WriteLine("===== Gestión de Alumnos =====");
        Console.WriteLine("1. Buscar por nombre");
        Console.WriteLine("2. Generar horario por alumno");
        Console.WriteLine("3. Volver");
        Console.Write("Seleccione una opción: ");

        var opcion = Console.ReadLine();
        switch (opcion)
        {
            case "1":
                Console.Write("Ingrese el nombre del alumno: ");
                var nombre = Console.ReadLine();
                var resultados = alumnoController.BuscarPorNombre(nombre);
                if (resultados.Count == 0)
                {
                    Console.WriteLine("No se encontraron alumnos.");
                }
                else
                {
                    resultados.ForEach(a => Console.WriteLine(a.ToString()));
                }
                break;
            case "2":
                Console.Write("Ingrese el nombre del alumno: ");
                var nombreAlumno = Console.ReadLine();
                var horario = alumnoController.GenerarHorarioPorAlumno(nombreAlumno, horarioController.ListarTodas());
                if (horario == null || horario.Count == 0)
                {
                    Console.WriteLine("No se pudo generar el horario. Verifique el nombre del alumno.");
                }
                else
                {
                    Console.WriteLine($"Horario para {nombreAlumno}:");
                    foreach (var asignatura in horario)
                    {
                        Console.WriteLine($"Asignatura: {asignatura.Key}");
                        asignatura.Value.ForEach(h => Console.WriteLine($"  {h}"));
                    }
                }
                break;
            default:
                Console.WriteLine("Volviendo...");
                break;
        }
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    static void GestionarProfesores(ProfesorController profesorController)
    {
        Console.Clear();
        Console.WriteLine("===== Gestión de Profesores =====");
        Console.WriteLine("1. Buscar por nombre");
        Console.WriteLine("2. Listar todos los profesores");
        Console.WriteLine("3. Volver");
        Console.Write("Seleccione una opción: ");

        var opcion = Console.ReadLine();
        switch (opcion)
        {
            case "1":
                Console.Write("Ingrese el nombre del profesor: ");
                var nombre = Console.ReadLine();
                var resultados = profesorController.BuscarPorNombre(nombre);
                if (resultados.Count == 0)
                {
                    Console.WriteLine("No se encontraron profesores.");
                }
                else
                {
                    resultados.ForEach(p => Console.WriteLine(p.ToString()));
                }
                break;
            case "2":
                var todos = profesorController.ListarTodos();
                todos.ForEach(p => Console.WriteLine(p.ToString()));
                break;
            default:
                Console.WriteLine("Volviendo...");
                break;
        }
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    static void GestionarHorarios(HorarioController horarioController, AlumnoController alumnoController)
    {
        Console.Clear();
        Console.WriteLine("===== Gestión de Horarios =====");
        Console.WriteLine("1. Buscar asignaturas por nombre");
        Console.WriteLine("2. Listar todas las asignaturas con horarios");
        Console.WriteLine("3. Generar horario por alumno");
        Console.WriteLine("4. Volver");
        Console.Write("Seleccione una opción: ");

        var opcion = Console.ReadLine();
        switch (opcion)
        {
            case "1":
                Console.Write("Ingrese el nombre de la asignatura: ");
                var nombre = Console.ReadLine();
                var asignaturas = horarioController.BuscarPorNombre(nombre);
                asignaturas.ForEach(a => Console.WriteLine(a.ToString()));
                break;
            case "2":
                var listado = horarioController.ListarAsignaturasConHorarios();
                listado.ForEach(Console.WriteLine);
                break;
            case "3":
                Console.Write("Ingrese el nombre del alumno: ");
                var nombreAlumno = Console.ReadLine();
                var horario = alumnoController.GenerarHorarioPorAlumno(nombreAlumno, horarioController.ListarTodas());
                if (horario == null || horario.Count == 0)
                {
                    Console.WriteLine("No se pudo generar el horario. Verifique el nombre del alumno.");
                }
                else
                {
                    Console.WriteLine($"Horario para {nombreAlumno}:");
                    foreach (var asignatura in horario)
                    {
                        Console.WriteLine($"Asignatura: {asignatura.Key}");
                        asignatura.Value.ForEach(h => Console.WriteLine($"  {h}"));
                    }
                }
                break;
            default:
                Console.WriteLine("Volviendo...");
                break;
        }
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    static void ConsultarMenu(MenuController menuController)
    {
        Console.Clear();
        Console.WriteLine("===== Consultar Menú del Comedor =====");
        Console.WriteLine("1. Buscar menú por día");
        Console.WriteLine("2. Filtrar menú por alérgenos");
        Console.WriteLine("3. Volver");
        Console.Write("Seleccione una opción: ");

        var opcion = Console.ReadLine();
        switch (opcion)
        {
            case "1":
                Console.Write("Ingrese el día: ");
                var dia = Console.ReadLine();
                var menus = menuController.BuscarPorDia(dia);
                menus.ForEach(m => Console.WriteLine(m.ToString()));
                break;
            case "2":
                Console.Write("Ingrese los alérgenos: ");
                var alergenos = Console.ReadLine();
                var filtrados = menuController.FiltrarPorAlergenos(alergenos);
                filtrados.ForEach(m => Console.WriteLine(m.ToString()));
                break;
            default:
                Console.WriteLine("Volviendo...");
                break;
        }
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }

    static void GestionarUsuarios(UsuarioController usuarioController)
    {
        Console.Clear();
        Console.WriteLine("===== Gestión de Usuarios =====");
        Console.WriteLine("1. Listar todos los usuarios");
        Console.WriteLine("2. Buscar usuario por QR");
        Console.WriteLine("3. Volver");
        Console.Write("Seleccione una opción: ");

        var opcion = Console.ReadLine();
        switch (opcion)
        {
            case "1":
                var usuarios = usuarioController.ListarTodos();
                usuarios.ForEach(u => Console.WriteLine(u.ToString()));
                break;
            case "2":
                Console.Write("Ingrese el QR del usuario: ");
                var qr = Console.ReadLine();
                var usuario = usuarioController.BuscarPorQR(qr);
                Console.WriteLine(usuario != null ? usuario.ToString() : "Usuario no encontrado.");
                break;
            default:
                Console.WriteLine("Volviendo...");
                break;
        }
        Console.WriteLine("Presione cualquier tecla para continuar...");
        Console.ReadKey();
    }
}
