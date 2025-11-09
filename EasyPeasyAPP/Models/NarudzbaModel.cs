using System;
using System.Collections.Generic;

namespace EasyPeasyAPP.Models
{
    public class NarudzbaModel
    {
        public string Id { get; set; }
        public string BrojNarudzbe { get; set; }
        public string KorisnikUid { get; set; }
        public string KorisnikIme { get; set; }
        public string KorisnikEmail { get; set; }
        public string Adresa { get; set; }
        public string Telefon { get; set; }
        public DateTime Datum { get; set; }
        public string Status { get; set; }
        public double Ukupno { get; set; }
        public List<KorpaStavka> Stavke { get; set; }

        // Nova polja
        public string AdresaDostave { get; set; }
        public string Napomena { get; set; }
    }
}