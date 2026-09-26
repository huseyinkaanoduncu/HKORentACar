using HKORentACar.Core.Concretes.Enums;

namespace HKORentACar.Core.Concretes.Entities
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

        // Varsayılan olarak Müsait (1) başlasın, 0 olup bakıma düşmesin
        public AracDurum Durum { get; set; } = AracDurum.Musait;

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