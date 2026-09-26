namespace HKORentACar.DAL.Entities
{
    public class Odeme : BaseEntity
    {
        public decimal Tutar { get; set; }
        public DateTime OdemeTarihi { get; set; } = DateTime.Now;
        public string OdemeTipi { get; set; }

        public int KiralamaId { get; set; }
        public Kiralama Kiralama { get; set; }
    }
}
