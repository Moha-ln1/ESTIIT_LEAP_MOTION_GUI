using System;

namespace GestionUniversidad.Models
{
    public class Horario
    {
        public string Siglas { get; set; }
        public string Grupo { get; set; }
        public string Aula { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFinal { get; set; }
        public string HorarioClase { get; set; }
        public string Profesor { get; set; }
        public string URL { get; set; }

        public Horario(string siglas, string grupo, string aula, string fechaInicio, string fechaFinal, string horarioClase, string profesor, string url)
        {
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
            return $"{Siglas} - Grupo: {Grupo}, Aula: {Aula}, Horario: {HorarioClase}, Profesor: {Profesor}, URL: {URL}";
        }
    }
}
