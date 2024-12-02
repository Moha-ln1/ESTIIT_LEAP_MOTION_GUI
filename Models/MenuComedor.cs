using System;

namespace GestionUniversidad.Models
{
    public class MenuComedor
    {
        public string Dia { get; set; }
        public string TipoMenu { get; set; }
        public string CremasSopas { get; set; }
        public string Entrante { get; set; }
        public string Primero { get; set; }
        public string Segundo { get; set; }
        public string Acompanamiento { get; set; }
        public string Postre { get; set; }
        public string Alergenos { get; set; }

        public MenuComedor(string dia, string tipoMenu, string cremasSopas, string entrante, string primero, string segundo, string acompanamiento, string postre, string alergenos)
        {
            Dia = dia;
            TipoMenu = tipoMenu;
            CremasSopas = cremasSopas;
            Entrante = entrante;
            Primero = primero;
            Segundo = segundo;
            Acompanamiento = acompanamiento;
            Postre = postre;
            Alergenos = alergenos;
        }

        public override string ToString()
        {
            return $"Día: {Dia}, Tipo de Menú: {TipoMenu}, Cremas y Sopas: {CremasSopas}, Entrante: {Entrante}, Primero: {Primero}, Segundo: {Segundo}, Acompañamiento: {Acompanamiento}, Postre: {Postre}, Alérgenos: {Alergenos}";
        }
    }
}
