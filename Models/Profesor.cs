using System;
using System.Collections.Generic;

namespace GestionUniversidad.Models
{
    public class Profesor
    {
        public string Nombre { get; set; }
        public string Departamento { get; set; }
        public List<string> Asignaturas { get; private set; }
        public string HorarioTutorias { get; set; }
        public string URL { get; set; }

        public Profesor(string nombre, string departamento, List<string> asignaturas, string horarioTutorias, string url)
        {
            Nombre = nombre;
            Departamento = departamento;
            Asignaturas = asignaturas ?? new List<string>();
            HorarioTutorias = horarioTutorias;
            URL = url;
        }

        public void AgregarAsignatura(string asignatura)
        {
            var asignaturaNormalizada = NormalizarTexto(asignatura);
            if (!Asignaturas.Contains(asignaturaNormalizada))
            {
                Asignaturas.Add(asignaturaNormalizada);
            }
        }

        public override string ToString()
        {
            var asignaturasStr = string.Join(", ", Asignaturas);
            return $"Nombre: {Nombre}, Departamento: {Departamento}, Asignaturas: {asignaturasStr}, Tutorías: {HorarioTutorias}, URL: {URL}";
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
}
