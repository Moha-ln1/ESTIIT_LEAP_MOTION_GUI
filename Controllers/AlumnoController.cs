using System;
using System.Collections.Generic;
using System.Linq;
using GestionUniversidad.Models;
public class AlumnoController
{
    private List<Alumno> _alumnos;

    public AlumnoController(List<Alumno> alumnos)
    {
        _alumnos = alumnos ?? new List<Alumno>();
    }

    public List<Alumno> BuscarPorNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return _alumnos;

        return _alumnos.Where(a => a.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public Alumno BuscarAlumnoExacto(string nombre)
    {
        return _alumnos.FirstOrDefault(a => string.Equals(a.Nombre, nombre, StringComparison.OrdinalIgnoreCase));
    }

    public List<Alumno> FiltrarPorAsignatura(string asignatura)
    {
        return _alumnos.Where(a => a.Asignaturas.Any(asig => asig.Asignatura.Equals(asignatura, StringComparison.OrdinalIgnoreCase))).ToList();
    }

    public Dictionary<string, List<string>> GenerarHorarioPorAlumno(string nombreAlumno, List<Asignatura> asignaturas)
    {
        // Buscar al alumno por nombre exacto
        var alumno = BuscarAlumnoExacto(nombreAlumno);
        if (alumno == null)
            return null; // Retorna null si no se encuentra el alumno

        var horario = new Dictionary<string, List<string>>();

        // Iterar sobre las asignaturas del alumno
        foreach (var (asignatura, grupo) in alumno.Asignaturas)
        {
            // Buscar detalles de la asignatura
            var asignaturaDetalles = asignaturas.FirstOrDefault(a => 
                string.Equals(a.Nombre, asignatura, StringComparison.OrdinalIgnoreCase));

            if (asignaturaDetalles != null)
            {
                // Filtrar horarios según el grupo y el grupo principal
                var horariosGrupo = asignaturaDetalles.Horarios
                    .Where(h => 
                        string.Equals(h.Grupo, grupo, StringComparison.OrdinalIgnoreCase) || 
                        string.Equals(h.Grupo, grupo.Substring(0, 1), StringComparison.OrdinalIgnoreCase))
                    .Select(h => $"{h.HorarioClase} - Aula: {h.Aula}, Profesor: {h.Profesor}")
                    .ToList();

                // Agregar al horario del alumno
                if (horariosGrupo.Any())
                {
                    horario[asignatura] = horariosGrupo;
                }
            }
        }

        return horario;
    }

}
