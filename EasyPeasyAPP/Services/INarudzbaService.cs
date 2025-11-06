using EasyPeasyAPP.Models;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Services
{
    public interface INarudzbaService
    {
        Task<NarudzbaModel> KreirajNarudzbuAsync(UserModel korisnik, List<KorpaStavka> stavke);
        Task<List<NarudzbaModel>> DohvatiNarudzbeKorisnikaAsync(string email);
    }
}