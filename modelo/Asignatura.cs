using System;
using System.Collections.Generic;
using System.Linq;

public class Asignatura
{
    public string Nombre { get; set; }
    public string Siglas { get; set; }
    public List<Horario> Horarios { get; set; }

    public Asignatura(string nombre, List<Horario> horarios)
    {
        Nombre = nombre;
        Horarios = horarios ?? new List<Horario>();
        Siglas = CalcularSiglas(nombre);
    }

    public static string CalcularSiglas(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return "N/A";

        var palabras = nombre.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        return string.Join("", palabras.Select(p => char.IsLetter(p[0]) ? p[0].ToString().ToUpper() : ""));
    }

    public override string ToString()
    {
        var horariosStr = Horarios.Any()
            ? string.Join(Environment.NewLine, Horarios.Select(h => $"  - {h.Grupo}, Aula: {h.Aula}, Horario: {h.HorarioClase}, Profesor: {h.Profesor}"))
            : "  Sin horarios";

        return $"Asignatura: {Nombre}, Siglas: {Siglas}{Environment.NewLine}Horarios:{Environment.NewLine}{horariosStr}";
    }
}
