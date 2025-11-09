using EasyPeasyAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Services
{
    public interface INarudzbaService
    {
        Task<NarudzbaModel> KreirajNarudzbuAsync(
            UserModel korisnik,
            List<KorpaStavka> stavke,
            string adresaDostave = null,
            string napomena = null
        );

        Task<List<NarudzbaModel>> DohvatiNarudzbeKorisnikaAsync(string email);

        Task<bool> PromijeniStatusNarudzbeAsync(string narudzbaId, string noviStatus);
    }
}
