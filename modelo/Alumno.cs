using System;

public class Alumno
{
    public string Nombre { get; set; }
    public string Asignatura { get; set; }
    public string Grupo { get; set; }

    // Constructor
    public Alumno(string nombre, string asignatura, string grupo)
    {
        Nombre = nombre;
        Asignatura = asignatura;
        Grupo = grupo;
    }
}

