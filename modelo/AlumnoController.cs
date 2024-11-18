using System;
using System.Collections.Generic;
using System.Linq;

public class AlumnoController
{
    private List<Alumno> _alumnos;

    public AlumnoController(List<Alumno> alumnos)
    {
        _alumnos = alumnos ?? new List<Alumno>();
    }

    public List<Alumno> BuscarPorNombre(string nombre)
    {
        return _alumnos.Where(a => a.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Alumno> FiltrarPorAsignatura(string asignatura)
    {
        return _alumnos.Where(a => a.Asignatura.Equals(asignatura, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Alumno> ObtenerPorGrupo(string grupo)
    {
        return _alumnos.Where(a => a.Grupo.Equals(grupo, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
