using System;
using System.Collections.Generic;
using System.Linq;
using GestionUniversidad.Models;

namespace GestionUniversidad.Models{
    public class Usuario
    {
        public string ID { get; private set; } // Identificador único del usuario.
        public Alumno Alumno { get; private set; } // Relación con el alumno.
        public string QR { get; private set; } // QR generado para el usuario.

        public Usuario(Alumno alumno)
        {
            ID = Guid.NewGuid().ToString(); // Generar un ID único.
            Alumno = alumno;
            QR = GenerarQR(ID);
        }

        private string GenerarQR(string identificador)
        {
            // Genera un QR único basado en el identificador.
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
        }

        public override string ToString()
        {
            return $"Usuario: {Alumno.Nombre}, ID: {ID}, QR: {QR}";
        }
    }
}