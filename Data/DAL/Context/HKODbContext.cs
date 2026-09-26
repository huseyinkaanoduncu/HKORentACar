using HKORentACar.Core.Concretes.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HKORentACar.Data.Context
{
    public class HKODbContext : IdentityDbContext<AppUser>
    {
        public HKODbContext(DbContextOptions<HKODbContext> options) : base(options)
        {
        }

        public DbSet<Arac> Araclar { get; set; }
        public DbSet<AracKategori> Kategoriler { get; set; }
        public DbSet<Sube> Subeler { get; set; }
        public DbSet<Kiralama> Kiralamalar { get; set; }
        public DbSet<Odeme> Odemeler { get; set; }
        public DbSet<HasarKaydi> HasarKayitlari { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Arac>().Property(a => a.GunlukUcret).HasPrecision(18, 2);
            builder.Entity<Kiralama>().Property(k => k.ToplamUcret).HasPrecision(18, 2);
            builder.Entity<Odeme>().Property(o => o.Tutar).HasPrecision(18, 2);
            builder.Entity<HasarKaydi>().Property(h => h.Tutar).HasPrecision(18, 2);

            builder.Entity<Arac>().HasIndex(a => a.Plaka).IsUnique();

            builder.Entity<Kiralama>()
                .HasOne(k => k.Arac)
                .WithMany(a => a.Kiralamalar)
                .HasForeignKey(k => k.AracId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HasarKaydi>()
                .HasOne(h => h.Kiralama)
                .WithOne(k => k.HasarKaydi)
                .HasForeignKey<HasarKaydi>(h => h.KiralamaId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<AracKategori>().HasData(
                new AracKategori { Id = 1, Ad = "Sedan", Aciklama = "Konforlu binek araçlar" },
                new AracKategori { Id = 2, Ad = "SUV", Aciklama = "Geniş arazi araçları" },
                new AracKategori { Id = 3, Ad = "Hatchback", Aciklama = "Şehir içi pratik araçlar" },
                new AracKategori { Id = 4, Ad = "Lüks / VIP", Aciklama = "Üst segment araçlar" }
            );

           
            builder.Entity<Sube>().HasData(
                new Sube { Id = 1, SehirAdi = "İstanbul Havalimanı Şubesi", Adres = "İstanbul Havalimanı Gelen Yolcu", Telefon = "02120000001" },
                new Sube { Id = 2, SehirAdi = "Sabiha Gökçen Havalimanı Şubesi", Adres = "Sabiha Gökçen Havalimanı Gelen Yolcu", Telefon = "02160000002" },
                new Sube { Id = 3, SehirAdi = "Ankara Esenboğa Şubesi", Adres = "Esenboğa Havalimanı İç Hatlar", Telefon = "03120000003" },
                new Sube { Id = 4, SehirAdi = "İzmir Adnan Menderes Şubesi", Adres = "Adnan Menderes Havalimanı", Telefon = "02320000004" }
            );
        }
    }
}