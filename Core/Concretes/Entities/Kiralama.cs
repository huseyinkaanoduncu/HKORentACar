using HKORentACar.Core.Concretes.Enums;

namespace HKORentACar.Core.Concretes.Entities
{
    public class Kiralama : BaseEntity
    {
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public int TeslimAlinanKM { get; set; }
        public int? TeslimEdilenKM { get; set; }
        public decimal ToplamUcret { get; set; }
        public KiralamaDurum Durum { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int AracId { get; set; }
        public Arac Arac { get; set; }

        public int SubeId { get; set; }
        public Sube Sube { get; set; }

        public ICollection<Odeme> Odemeler { get; set; }
        public HasarKaydi? HasarKaydi { get; set; }
    }
}