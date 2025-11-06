using System;

namespace EasyPeasyAPP.Models
{
    public class KorpaStavka
    {
        public string ArtikalId { get; set; }
        public string Naziv { get; set; }
        public double Cijena { get; set; }
        public int Kolicina { get; set; }
        public string Slika { get; set; }
        public double Ukupno => Cijena * Kolicina;
    }
}
