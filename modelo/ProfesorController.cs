using System;
using System.Collections.Generic;
using System.Linq;

public class ProfesorController
{
    private List<Profesor> _profesores;

    public ProfesorController(List<Profesor> profesores)
    {
        _profesores = profesores ?? new List<Profesor>();
    }

    public List<Profesor> BuscarPorNombre(string nombre)
    {
        return _profesores.Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Profesor> FiltrarPorDepartamento(string departamento)
    {
        return _profesores.Where(p => p.Departamento.Equals(departamento, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<string> ObtenerAsignaturas(string profesorNombre)
    {
        var profesor = _profesores.FirstOrDefault(p => p.Nombre.Equals(profesorNombre, StringComparison.OrdinalIgnoreCase));
        return profesor?.Asignaturas ?? new List<string>();
    }

        public List<Profesor> ListarTodosLosProfesores()
    {
        return _profesores;
    }

    public Profesor BuscarProfesorPorNombre(string nombreBuscado)
    {
        if (string.IsNullOrWhiteSpace(nombreBuscado))
            return null;

        // Normalizar el nombre buscado
        nombreBuscado = NormalizarTexto(nombreBuscado);

        foreach (var profesor in _profesores)
        {
            string nombreProfesorNormalizado = NormalizarTexto(profesor.Nombre);


            if (nombreProfesorNormalizado == nombreBuscado)
            {
                return profesor;
            }
        }

        return null;
    }


    private string NormalizarTexto(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return string.Empty;

        // Normaliza el texto y elimina caracteres invisibles
        return texto.Trim() // Elimina espacios alrededor
                    .Replace("\u00A0", " ") // Reemplaza espacios no separables
                    .Replace("\u200B", "") // Elimina caracteres invisibles
                    .ToLowerInvariant() // Convierte a minúsculas
                    .Normalize(System.Text.NormalizationForm.FormC); // Usa FormC para consistencia
    }



}
