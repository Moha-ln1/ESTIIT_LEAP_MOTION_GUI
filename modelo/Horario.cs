using System;

public class Horario
{
    public string Nombre { get; set; }
    public string Siglas { get; set; }
    public string Grupo { get; set; }
    public string Aula { get; set; }
    public string FechaInicio { get; set; }
    public string FechaFinal { get; set; }
    public string HorarioClase { get; set; }
    public string Profesor { get; set; }
    public string URL { get; set; }

    public Horario(string nombre, string siglas, string grupo, string aula, string fechaInicio, string fechaFinal, string horarioClase, string profesor, string url)
    {
        Nombre = nombre;
        Siglas = siglas;
        Grupo = grupo;
        Aula = aula;
        FechaInicio = fechaInicio;
        FechaFinal = fechaFinal;
        HorarioClase = horarioClase;
        Profesor = profesor;
        URL = url;
    }

    public override string ToString()
    {
        return $"Nombre: {Nombre}, Grupo: {Grupo}, Aula: {Aula}, Horario: {HorarioClase}, Profesor: {Profesor}";
    }
}
