using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionUniversidad.Models
{
  public class Asignatura
{
    public string Nombre { get; set; }
    public string Siglas { get; set; }
    public List<Horario> Horarios { get; private set; }

    public Asignatura(string nombre, string siglas)
    {
        Nombre = nombre;
        Siglas = siglas;
        Horarios = new List<Horario>();
    }

    public void AgregarHorario(Horario horario)
    {
        Horarios.Add(horario);
    }

    public override string ToString()
    {
        return $"{Nombre} ({Siglas})";
    }
}

}
