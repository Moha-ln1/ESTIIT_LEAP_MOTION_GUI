using System;
using System.Collections.Generic;
using System.Linq;
using GestionUniversidad.Models;

public class AsignaturaController
{
    private List<Asignatura> _asignaturas;

    public AsignaturaController(List<Asignatura> asignaturas)
    {
        _asignaturas = asignaturas ?? new List<Asignatura>();
    }

    public List<Asignatura> BuscarPorNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return new List<Asignatura>();

        return _asignaturas.Where(a => NormalizarTexto(a.Nombre).Contains(NormalizarTexto(nombre))).ToList();
    }

    public List<Asignatura> BuscarPorSiglas(string siglas)
    {
        if (string.IsNullOrWhiteSpace(siglas))
            return new List<Asignatura>();

        return _asignaturas.Where(a => NormalizarTexto(a.Siglas) == NormalizarTexto(siglas)).ToList();
    }

    public List<Asignatura> ObtenerTodas()
    {
        return _asignaturas;
    }

    public string ListarAsignaturas()
    {
        return _asignaturas.Count > 0 
            ? string.Join("\n", _asignaturas.Select(a => a.ToString())) 
            : "No hay asignaturas disponibles.";
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
