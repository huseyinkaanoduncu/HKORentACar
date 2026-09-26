using HKORentACar.Core.Concretes.Enums;
using Microsoft.AspNetCore.Http;

namespace HKORentACar.Core.Concretes.DTOs
{
    public class AracDto
    {
        public int Id { get; set; }
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
        public int SubeId { get; set; }
        public IFormFile? ResimDosyasi { get; set; }
    }
}