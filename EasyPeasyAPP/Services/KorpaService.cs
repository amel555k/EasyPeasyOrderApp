using EasyPeasyAPP.Models;
using System.Collections.Generic;
using System.Linq;

namespace EasyPeasyAPP.Services
{
    public class KorpaService : IKorpaService
    {
        private List<KorpaStavka> _korpa = new List<KorpaStavka>();

        public void DodajUKorpu(ArtikalModel artikal)
        {
            var postojecaStavka = _korpa.FirstOrDefault(s => s.ArtikalId == artikal.Id);

            if (postojecaStavka != null)
            {
                postojecaStavka.Kolicina++;
            }
            else
            {
                _korpa.Add(new KorpaStavka
                {
                    ArtikalId = artikal.Id,
                    Naziv = artikal.Naziv,
                    Cijena = artikal.Cijena,
                    Kolicina = 1,
                    Slika = artikal.Slika
                });
            }
        }

        public void UkloniIzKorpe(string artikalId)
        {
            var stavka = _korpa.FirstOrDefault(s => s.ArtikalId == artikalId);
            if (stavka != null)
            {
                _korpa.Remove(stavka);
            }
        }

        public void OcistiKorpu()
        {
            _korpa.Clear();
        }

        public List<KorpaStavka> DohvatiKorpu()
        {
            return _korpa;
        }

        public int BrojStavki()
        {
            return _korpa.Sum(s => s.Kolicina);
        }

        public double UkupnaCijena()
        {
            return _korpa.Sum(s => s.Ukupno);
        }

        public void PromijeniKolicinu(string artikalId, int novaKolicina)
        {
            var stavka = _korpa.FirstOrDefault(s => s.ArtikalId == artikalId);
            if (stavka != null)
            {
                if (novaKolicina <= 0)
                {
                    _korpa.Remove(stavka);
                }
                else
                {
                    stavka.Kolicina = novaKolicina;
                }
            }
        }
    }
}