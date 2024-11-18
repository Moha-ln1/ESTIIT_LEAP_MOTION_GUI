using System;
using System.Collections.Generic;
using System.Linq;

public class AsignaturaController
{
    private List<Asignatura> _asignaturas;

    public AsignaturaController(List<Asignatura> asignaturas)
    {
        _asignaturas = asignaturas ?? new List<Asignatura>();
    }

    public List<Asignatura> BuscarPorNombre(string nombre)
    {
        return _asignaturas.Where(a => a.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Horario> ObtenerHorarios(string nombre)
    {
        var asignatura = _asignaturas.FirstOrDefault(a => a.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        return asignatura?.Horarios ?? new List<Horario>();
    }

    public List<string> ObtenerSiglas()
    {
        return _asignaturas.Select(a => a.Siglas).ToList();
    }
}
