
using Microsoft.AspNetCore.Identity;

namespace HKORentACar.Core.Concretes.Entities
{
    public class AppUser : IdentityUser
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string? TC { get; set; }
        public string? EhliyetNo { get; set; }
        public DateTime? EhliyetTarihi { get; set; }
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        public ICollection<Kiralama> Kiralamalar { get; set; }
    }
}