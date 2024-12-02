using System;
using System.Collections.Generic;
using System.Linq;
using GestionUniversidad.Models;

public class ProfesorController
{
    private List<Profesor> _profesores;

    public ProfesorController(List<Profesor> profesores)
    {
        _profesores = profesores ?? new List<Profesor>();
    }

    public List<Profesor> BuscarPorNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return _profesores;

        return _profesores.Where(p => NormalizarTexto(p.Nombre).Contains(NormalizarTexto(nombre))).ToList();
    }

    public List<Profesor> FiltrarPorDepartamento(string departamento)
    {
        if (string.IsNullOrWhiteSpace(departamento))
            return new List<Profesor>();

        return _profesores.Where(p => NormalizarTexto(p.Departamento).Contains(NormalizarTexto(departamento))).ToList();
    }

    public List<Profesor> ListarTodos()
    {
        return _profesores;
    }

    public List<Profesor> ListarPorAsignatura(string asignatura)
    {
        if (string.IsNullOrWhiteSpace(asignatura))
            return new List<Profesor>();

        return _profesores.Where(p => p.Asignaturas.Any(a => NormalizarTexto(a).Contains(NormalizarTexto(asignatura)))).ToList();
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
