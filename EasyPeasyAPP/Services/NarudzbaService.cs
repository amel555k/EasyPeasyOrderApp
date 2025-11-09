using EasyPeasyAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyPeasyAPP.Services
{
    public class NarudzbaService : INarudzbaService
    {
        private const string FIREBASE_DB_URL = "***REMOVED***";
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public NarudzbaService(IAuthService authService)
        {
            _httpClient = new HttpClient();
            _authService = authService;
        }

        // Kreiranje nove narudžbe
        public async Task<NarudzbaModel> KreirajNarudzbuAsync(
            UserModel korisnik,
            List<KorpaStavka> stavke,
            string adresaDostave = null,
            string napomena = null)
        {
            if (stavke == null || !stavke.Any())
                throw new Exception("Korpa je prazna.");

            var narudzbaId = $"narudzba_{Guid.NewGuid():N}";

            var narudzba = new NarudzbaModel
            {
                Id = narudzbaId,
                BrojNarudzbe = GenerisiBrojNarudzbe(),
                KorisnikUid = korisnik.Uid,
                KorisnikIme = korisnik.Ime,
                KorisnikEmail = korisnik.Email,
                Adresa = korisnik.Adresa ?? "",
                Telefon = korisnik.Telefon ?? "",
                Datum = DateTime.Now,
                Status = "nova",
                Ukupno = stavke.Sum(s => s.Ukupno),
                Stavke = stavke.ToList(),
                AdresaDostave = adresaDostave ?? "",
                Napomena = napomena ?? ""
            };

            var token = await SecureStorage.Default.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
                throw new Exception("Niste prijavljeni.");

            var content = new StringContent(JsonSerializer.Serialize(narudzba), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(
                $"{FIREBASE_DB_URL}narudzbe/{narudzbaId}.json?auth={token}",
                content
            );

            response.EnsureSuccessStatusCode();
            return narudzba;
        }

        // Dohvatanje svih narudžbi korisnika
        public async Task<List<NarudzbaModel>> DohvatiNarudzbeKorisnikaAsync(string email)
        {
            var token = await SecureStorage.Default.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
                throw new Exception("Niste prijavljeni.");

            var response = await _httpClient.GetAsync($"{FIREBASE_DB_URL}narudzbe.json?auth={token}");
            if (!response.IsSuccessStatusCode)
                return new List<NarudzbaModel>();

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(json) || json == "null")
                return new List<NarudzbaModel>();

            var allOrders = JsonSerializer.Deserialize<Dictionary<string, NarudzbaModel>>(json);

            return allOrders?.Values
                .Where(n => n.KorisnikEmail == email)
                .OrderByDescending(n => n.Datum)
                .ToList() ?? new List<NarudzbaModel>();
        }

        // Promjena statusa narudžbe
        public async Task<bool> PromijeniStatusNarudzbeAsync(string narudzbaId, string noviStatus)
        {
            var token = await SecureStorage.Default.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
                throw new Exception("Niste prijavljeni.");

            var updateContent = new StringContent(
                JsonSerializer.Serialize(new { Status = noviStatus }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PatchAsync(
                $"{FIREBASE_DB_URL}narudzbe/{narudzbaId}.json?auth={token}",
                updateContent
            );

            return response.IsSuccessStatusCode;
        }

        // Generisanje broja narudžbe
        private string GenerisiBrojNarudzbe()
        {
            return $"EP{DateTime.Now:yyyyMMddHHmmss}";
        }
    }
}
