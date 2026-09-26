using HKORentACar.DAL.Entities.Enums;

namespace HKORentACar.DAL.Entities
{
    public class Arac : BaseEntity
    {
        public string Marka { get; set; }
        public string Model { get; set; }
        public int Yil { get; set; }
        public string Plaka { get; set; }
        public decimal GunlukUcret { get; set; }
        public int KM { get; set; }
        public string ResimUrl { get; set; }

        public AracDurum Durum { get; set; }
        public YakitTipi YakitTipi { get; set; }
        public VitesTipi VitesTipi { get; set; }

        public int KategoriId { get; set; }
        public AracKategori Kategori { get; set; }

        public int SubeId { get; set; }
        public Sube Sube { get; set; }

        public ICollection<Kiralama> Kiralamalar { get; set; }
        public ICollection<HasarKaydi> HasarKayitlari { get; set; }
    }
}
