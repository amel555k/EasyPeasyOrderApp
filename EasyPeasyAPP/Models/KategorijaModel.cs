namespace EasyPeasyAPP.Models
{
    public class KategorijaModel
    {
        public string Id { get; set; }
        public string Naziv { get; set; }
        public string NazivPrikaz { get; set; }
        public string Ikonica { get; set; }
        public List<ArtikalModel> Artikli { get; set; } = new();
    }
}