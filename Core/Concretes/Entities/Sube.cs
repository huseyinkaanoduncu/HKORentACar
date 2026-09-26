

namespace HKORentACar.Core.Concretes.Entities
{
    public class Sube : BaseEntity
    {
        public string SehirAdi { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }

        public ICollection<Arac> Araclar { get; set; }
        public ICollection<Kiralama> Kiralamalar { get; set; }
    }
}