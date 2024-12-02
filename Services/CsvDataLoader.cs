using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GestionUniversidad.Models;

public class CsvDataLoader
{


    public List<Usuario> LoadUsuarios(List<Alumno> alumnos)
    {
        var usuarios = new List<Usuario>();
        foreach (var alumno in alumnos)
        {
            var usuario = new Usuario(alumno);
            usuarios.Add(usuario);
        }
        return usuarios;
    }

    public List<Alumno> LoadAlumnos(string filePath)
    {
        var alumnos = new List<Alumno>();
        foreach (var line in File.ReadAllLines(filePath).Skip(1))
        {
            var data = line.Split(',');
            var nombre = data[0].Trim('"').Trim();
            var asignatura = data[1].Trim();
            var grupo = data[2].Trim();

            // Buscar o crear el alumno
            var alumno = alumnos.FirstOrDefault(a => a.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (alumno == null)
            {
                alumno = new Alumno(nombre);
                alumnos.Add(alumno);
            }

            // Agregar asignatura y grupo (maneja automáticamente la lógica de duplicado)
            alumno.AgregarAsignatura(asignatura, grupo);
        }
        return alumnos;
    }


    public List<Asignatura> LoadAsignaturas(string filePath)
    {
        var asignaturas = new List<Asignatura>();
        foreach (var line in File.ReadAllLines(filePath).Skip(1))
        {
            var data = line.Split(',');
            var nombre = data[0].Trim();
            var siglas = data[1].Trim();
            var grupo = data[2].Trim();
            var aula = data[3].Trim();
            var fechaInicio = data[4].Trim();
            var fechaFinal = data[5].Trim();
            var horario = data[6].Trim();
            var profesor = data[7].Trim();
            var url = data[8].Trim();

            var asignatura = asignaturas.FirstOrDefault(a => a.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (asignatura == null)
            {
                asignatura = new Asignatura(nombre, siglas);
                asignaturas.Add(asignatura);
            }

            // Validar si el horario ya existe para evitar duplicados
            if (!asignatura.Horarios.Any(h => h.Grupo.Equals(grupo, StringComparison.OrdinalIgnoreCase) &&
                                              h.HorarioClase.Equals(horario, StringComparison.OrdinalIgnoreCase)))
            {
                asignatura.AgregarHorario(new Horario(siglas, grupo, aula, fechaInicio, fechaFinal, horario, profesor, url));
            }
        }
        return asignaturas;
    }

    public List<MenuComedor> LoadMenus(string filePath)
    {
        var menus = new List<MenuComedor>();
        foreach (var line in File.ReadAllLines(filePath).Skip(1))
        {
            var data = line.Split(',');

            // Validación básica para evitar índices fuera de rango
            if (data.Length < 9)
                continue;

            var dia = data[0].Trim();
            var tipoMenu = data[1].Trim();
            var cremasSopas = data[2].Trim();
            var entrante = data[3].Trim();
            var primero = data[4].Trim();
            var segundo = data[5].Trim();
            var acompanamiento = data[6].Trim();
            var postre = data[7].Trim();
            var alergenos = data[8].Trim();

            menus.Add(new MenuComedor(dia, tipoMenu, cremasSopas, entrante, primero, segundo, acompanamiento, postre, alergenos));
        }
        return menus;
    }

    public List<Profesor> LoadProfesores(string filePath)
    {
        var profesores = new List<Profesor>();
        foreach (var line in File.ReadAllLines(filePath).Skip(1))
        {
            var data = line.Split(',');
            var nombre = data[0].Trim();
            var departamento = data[1].Trim();
            var asignaturas = data[2].Split(';').Select(a => a.Trim()).ToList();
            var horario = data.Length > 3 ? data[3].Trim() : string.Empty;
            var url = data.Length > 4 ? data[4].Trim() : string.Empty;

            var profesor = profesores.FirstOrDefault(p => p.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (profesor == null)
            {
                profesor = new Profesor(nombre, departamento, asignaturas, horario, url);
                profesores.Add(profesor);
            }
            else
            {
                // Agregar asignaturas adicionales si no están ya presentes
                foreach (var asignatura in asignaturas)
                {
                    profesor.AgregarAsignatura(asignatura);
                }
            }
        }
        return profesores;
    }
}
