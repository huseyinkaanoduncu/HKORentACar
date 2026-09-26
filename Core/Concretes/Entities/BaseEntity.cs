namespace HKORentACar.Core.Concretes.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}