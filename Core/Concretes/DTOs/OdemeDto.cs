using System.ComponentModel.DataAnnotations;

namespace HKORentACar.Core.Concretes.DTOs
{
    public class OdemeDto
    {
        public int AracId { get; set; }
        public DateTime AlisTarihi { get; set; }
        public DateTime TeslimTarihi { get; set; }
        public decimal ToplamTutar { get; set; }

        [Required(ErrorMessage = "Kart üzerindeki isim zorunludur.")]
        public string KartUzerindekiIsim { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kart numarası zorunludur.")]
        [CreditCard(ErrorMessage = "Geçerli bir kart numarası giriniz.")]
        public string KartNumarasi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Son kullanma ayı zorunludur.")]
        [Range(1, 12, ErrorMessage = "Geçerli bir ay giriniz.")]
        public int SonKullanmaAy { get; set; }

        [Required(ErrorMessage = "Son kullanma yılı zorunludur.")]
        [Range(2026, 2035, ErrorMessage = "Geçerli bir yıl giriniz.")]
        public int SonKullanmaYil { get; set; }

        [Required(ErrorMessage = "CVC kodu zorunludur.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVC 3 haneli olmalıdır.")]
        public string Cvc { get; set; } = string.Empty;
    }
}