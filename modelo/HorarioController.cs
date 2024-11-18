using System;
using System.Collections.Generic;
using System.Linq;

public class HorarioController
{
    private List<Horario> _horarios;

    public HorarioController(List<Horario> horarios)
    {
        _horarios = horarios ?? new List<Horario>();
    }

    public List<Horario> BuscarPorAsignatura(string asignatura)
    {
        return _horarios.Where(h => h.Nombre.Equals(asignatura, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Horario> FiltrarPorGrupo(string grupo)
    {
        return _horarios.Where(h => h.Grupo.Equals(grupo, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Horario> BuscarPorProfesor(string profesor)
    {
        return _horarios.Where(h => h.Profesor.Contains(profesor, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
