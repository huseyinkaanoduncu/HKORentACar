namespace HKORentACar.DAL.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
