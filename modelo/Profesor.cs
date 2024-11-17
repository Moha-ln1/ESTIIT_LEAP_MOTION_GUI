using System;
using System.Collections.Generic;

public class Profesor
{
    public string Nombre { get; set; }
    public string Departamento { get; set; }
    public List<string> Asignaturas { get; set; }
    public string HorarioTutorias { get; set; }
    public string Url { get; set; }

    // Constructor
    public Profesor(string nombre, string departamento, List<string> asignaturas, string horarioTutorias, string url)
    {
        Nombre = nombre;
        Departamento = departamento;
        Asignaturas = asignaturas ?? new List<string>();
        HorarioTutorias = horarioTutorias;
        Url = url;
    }
}
