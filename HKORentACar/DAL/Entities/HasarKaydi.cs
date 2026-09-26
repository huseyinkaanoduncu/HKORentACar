namespace HKORentACar.DAL.Entities
{
    public class HasarKaydi : BaseEntity
    {
        public string Aciklama { get; set; }
        public decimal Tutar { get; set; }
        public DateTime Tarih { get; set; } = DateTime.Now;

        public int AracId { get; set; }
        public Arac Arac { get; set; }

        public int KiralamaId { get; set; }
        public Kiralama Kiralama { get; set; }
    }
}
