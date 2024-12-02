using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionUniversidad.Models
{
    public class Alumno
    {
        public string Nombre { get; set; }
        public List<(string Asignatura, string Grupo)> Asignaturas { get; private set; }

        public Alumno(string nombre)
        {
            Nombre = nombre;
            Asignaturas = new List<(string Asignatura, string Grupo)>();
        }

        public void AgregarAsignatura(string asignatura, string grupo)
        {
            // Agregar grupo original
            if (!Asignaturas.Any(a => a.Asignatura.Equals(asignatura, StringComparison.OrdinalIgnoreCase) &&
                                    a.Grupo.Equals(grupo, StringComparison.OrdinalIgnoreCase)))
            {
                Asignaturas.Add((asignatura, grupo));
            }

            // Derivar grupo de teoría si corresponde
            var grupoTeoria = grupo.Length > 1 ? grupo.Substring(0, 1) : grupo;
            if (!Asignaturas.Any(a => a.Asignatura.Equals(asignatura, StringComparison.OrdinalIgnoreCase) &&
                                    a.Grupo.Equals(grupoTeoria, StringComparison.OrdinalIgnoreCase)))
            {
                Asignaturas.Add((asignatura, grupoTeoria));
            }
        }
    }


}
