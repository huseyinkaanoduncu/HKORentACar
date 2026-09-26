using HKORentACar.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HKORentACar.DAL.Contexts
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

            // Precision Ayarları (Para birimleri için hassasiyet)
            builder.Entity<Arac>().Property(a => a.GunlukUcret).HasPrecision(18, 2);
            builder.Entity<Kiralama>().Property(k => k.ToplamUcret).HasPrecision(18, 2);
            builder.Entity<Odeme>().Property(o => o.Tutar).HasPrecision(18, 2);
            builder.Entity<HasarKaydi>().Property(h => h.Tutar).HasPrecision(18, 2);

            // Benzersiz Plaka Kuralı
            builder.Entity<Arac>().HasIndex(a => a.Plaka).IsUnique();

            // Foreign Key Çakışma Önleme (Delete Behavior)
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
        }
    }
}