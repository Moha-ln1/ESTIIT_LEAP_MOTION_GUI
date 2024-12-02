using System;
using System.Collections.Generic;
using System.Linq;
using GestionUniversidad.Models;

public class HorarioController
{
    private List<Asignatura> _asignaturas;

    public HorarioController(List<Asignatura> asignaturas)
    {
        _asignaturas = asignaturas ?? new List<Asignatura>();
    }

    public List<Asignatura> BuscarPorNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return _asignaturas;

        return _asignaturas.Where(a => NormalizarTexto(a.Nombre).Contains(NormalizarTexto(nombre))).ToList();
    }

    public List<Asignatura> ListarTodas()
    {
        return _asignaturas;
    }

    public List<string> ListarAsignaturasConHorarios()
    {
        return _asignaturas.Select(a => $"{a.Nombre} ({a.Siglas})\nHorarios:\n" +
                                         string.Join("\n", a.Horarios.Select(h => h.ToString())))
                            .ToList();
    }

    private string NormalizarTexto(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        texto = texto.Replace("\u00A0", " ").Replace("\u200B", "");
        texto = string.Join(" ", texto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        return texto.Trim().ToLowerInvariant();
    }
}
