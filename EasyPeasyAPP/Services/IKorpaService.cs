using EasyPeasyAPP.Models;
using System.Collections.Generic;

namespace EasyPeasyAPP.Services
{
    public interface IKorpaService
    {
        void DodajUKorpu(ArtikalModel artikal);
        void UkloniIzKorpe(string artikalId);
        void OcistiKorpu();
        List<KorpaStavka> DohvatiKorpu();
        int BrojStavki();
        double UkupnaCijena();
        void PromijeniKolicinu(string artikalId, int novaKolicina);
    }
}