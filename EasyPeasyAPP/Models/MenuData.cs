namespace EasyPeasyAPP.Models
{
    public static class MenuData
    {
        public static List<KategorijaModel> SveKategorije = new()
        {
            new KategorijaModel
            {
                Id = "burgeri",
                Naziv = "EASY BURGERI",
                NazivPrikaz = "EASY\nBURGERI",
                Ikonica = "burger.png",
                Artikli = new List<ArtikalModel>
                {
                    new ArtikalModel
                    {
                        Id = "burger1",
                        Naziv = "Easy Crunchy Burger",
                        Sastojci = "Pljeskavica, zelena salata, Easy Peasy umak od luka, hrskavi luk",
                        Cijena = 7.0,
                        Slika = "burger1.png",
                        Kategorija = "burgeri"
                    },
                    new ArtikalModel
                    {
                        Id = "burger2",
                        Naziv = "Easy Classic Burger",
                        Sastojci = "Pljeskavica, zelena salata, paradajz, ketchup, majoneza",
                        Cijena = 6.5,
                        Slika = "burger2.png",
                        Kategorija = "burgeri"
                    },
                    new ArtikalModel
                    {
                        Id = "burger3",
                        Naziv = "Easy Cheese Burger",
                        Sastojci = "Pljeskavica, cheddar sir, zelena salata, paradajz, majoneza",
                        Cijena = 7.5,
                        Slika = "burger3.png",
                        Kategorija = "burgeri"
                    },
                    new ArtikalModel
                    {
                        Id = "burger4",
                        Naziv = "Easy BBQ Burger",
                        Sastojci = "Pljeskavica, bacon, BBQ sos, cheddar sir, hrskavi luk",
                        Cijena = 8.5,
                        Slika = "burger4.png",
                        Kategorija = "burgeri"
                    },
                    new ArtikalModel
                    {
                        Id = "burger5",
                        Naziv = "Easy Spicy Burger",
                        Sastojci = "Pljeskavica, ljuti sos, jalapeno papričice, cheddar sir, luk",
                        Cijena = 8.0,
                        Slika = "burger5.png",
                        Kategorija = "burgeri"
                    }
                }
            },

            new KategorijaModel
            {
                Id = "dogs",
                Naziv = "EASY DOGS",
                NazivPrikaz = "EASY\nDOGS",
                Ikonica = "hotdog.png",
                Artikli = new List<ArtikalModel>
                {
                    new ArtikalModel
                    {
                        Id = "dog1",
                        Naziv = "Easy Peasy Chilli Dog",
                        Sastojci = "Handmade hrenovka, Easy Peasy sos od chilli-a, senf, kupus",
                        Cijena = 6.5,
                        Slika = "hotdog1.png",
                        Kategorija = "hotdogs"
                    },
                    new ArtikalModel
                    {
                        Id = "dog2",
                        Naziv = "Easy Peasy Classic Dog",
                        Sastojci = "Hrenovka, senf, ketchup, kupus, luk",
                        Cijena = 5.5,
                        Slika = "hotdog2.png",
                        Kategorija = "hotdogs"
                    },
                    new ArtikalModel
                    {
                        Id = "dog3",
                        Naziv = "Easy Peasy Cheese Dog",
                        Sastojci = "Hrenovka, cheddar sir, senf, majoneza, luk",
                        Cijena = 6.0,
                        Slika = "hotdog3.png",
                        Kategorija = "hotdogs"
                    },
                    new ArtikalModel
                    {
                        Id = "dog4",
                        Naziv = "Easy Peasy BBQ Dog",
                        Sastojci = "Hrenovka, BBQ sos, bacon, luk, kupus",
                        Cijena = 7.0,
                        Slika = "hotdog4.png",
                        Kategorija = "hotdogs"
                    },
                    new ArtikalModel
                    {
                        Id = "dog5",
                        Naziv = "Easy Peasy Vege Dog",
                        Sastojci = "Vegetarijanska kobasica, senf, ketchup, svježe povrće",
                        Cijena = 6.0,
                        Slika = "hotdog5.png",
                        Kategorija = "hotdogs"
                    }
                }
            },

            new KategorijaModel
            {
                Id = "sendvici",
                Naziv = "SENDVIČI",
                NazivPrikaz = "SENDVIČI",
                Ikonica = "sendvic.png",
                Artikli = new List<ArtikalModel>
                {
                    new ArtikalModel
                    {
                        Id = "sendvic1",
                        Naziv = "Easy Peasy školski sendvič",
                        Sastojci = "Pureća premium šunka, majoneza, ketchup, krastavice, sos",
                        Cijena = 4.0,
                        Slika = "sendvic1.png",
                        Kategorija = "sendvici"
                    },
                    new ArtikalModel
                    {
                        Id = "sendvic2",
                        Naziv = "Easy Peasy chicken sendvič",
                        Sastojci = "Piletina, zelena salata, paradajz, majoneza",
                        Cijena = 4.5,
                        Slika = "sendvic2.png",
                        Kategorija = "sendvici"
                    },
                    new ArtikalModel
                    {
                        Id = "sendvic3",
                        Naziv = "Easy Peasy chilli sendvič",
                        Sastojci = "Piletina, ljuti sos, paprika, sir, majoneza",
                        Cijena = 5.0,
                        Slika = "sendvic3.png",
                        Kategorija = "sendvici"
                    }
                }
            },

            new KategorijaModel
            {
                Id = "sokovi",
                Naziv = "SOKOVI",
                NazivPrikaz = "SOKOVI",
                Ikonica = "sokovi.png",
                Artikli = new List<ArtikalModel>
                {
                    new ArtikalModel
                    {
                        Id = "sok1",
                        Naziv = "Coca Cola",
                        Sastojci = "Osvježenje koje je neminovno uz Easy Peasy produkte",
                        Cijena = 2.5,
                        Slika = "sok1.png",
                        Kategorija = "sokovi"
                    },
                    new ArtikalModel
                    {
                        Id = "sok2",
                        Naziv = "Sprite",
                        Sastojci = "Limunada koja osvježava",
                        Cijena = 2.5,
                        Slika = "sok2.png",
                        Kategorija = "sokovi"
                    },
                    new ArtikalModel
                    {
                        Id = "sok3",
                        Naziv = "Fanta",
                        Sastojci = "Narandžasti osvježavajući sok",
                        Cijena = 2.5,
                        Slika = "sok3.png",
                        Kategorija = "sokovi"
                    }
                }
            },

            new KategorijaModel
            {
                Id = "prilozi",
                Naziv = "PRILOZI",
                NazivPrikaz = "PRILOZI",
                Ikonica = "prilozi.png",
                Artikli = new List<ArtikalModel>
                {
                    new ArtikalModel
                    {
                        Id = "prilog1",
                        Naziv = "Pomfrit",
                        Sastojci = "Neizostavan prilog uz Vašu užinu od pomno odabranih krompira iz našeg vrta",
                        Cijena = 2.0,
                        Slika = "pomfrit.png",
                        Kategorija = "prilozi"
                    },
                    new ArtikalModel
                    {
                        Id = "prilog2",
                        Naziv = "Onion rings",
                        Sastojci = "Ekscentričan ali i ukusan prilog, luk pohovan i detaljno oblikovan",
                        Cijena = 2.5,
                        Slika = "onion.png",
                        Kategorija = "prilozi"
                    },
                    new ArtikalModel
                    {
                        Id = "prilog3",
                        Naziv = "Dodatni Ketchup",
                        Sastojci = "Nestalo Vam je kečapa i želite još? Stojimo Vam na raspolaganju",
                        Cijena = 0.5,
                        Slika = "kecap.png",
                        Kategorija = "prilozi"
                    },
                    new ArtikalModel
                    {
                        Id = "prilog4",
                        Naziv = "Dodatna majoneza",
                        Sastojci = "Nestalo Vam je majoneze i želite još? Stojimo Vam na raspolaganju",
                        Cijena = 0.5,
                        Slika = "majoneza.png",
                        Kategorija = "prilozi"
                    }
                }
            }
        };
    }
}