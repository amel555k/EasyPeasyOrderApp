namespace EasyPeasyAPP.Models
{
    public class UserModel
    {
        public string Ime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Sifra { get; set; } = string.Empty; // pohranjuje se samo u Auth
        public string Uloga { get; set; } = "kupac";     // kupac ili radnik
        public string? Adresa { get; set; }
        public string? Telefon { get; set; }
        public string Uid { get; set; } = string.Empty;  // Firebase uid
    }
}
