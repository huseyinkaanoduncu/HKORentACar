namespace HKORentACar.Core.Concretes.Entities
{
    public class AracKategori : BaseEntity
    {
        public string Ad { get; set; }
        public string? Aciklama { get; set; }

        public ICollection<Arac> Araclar { get; set; }
    }
}