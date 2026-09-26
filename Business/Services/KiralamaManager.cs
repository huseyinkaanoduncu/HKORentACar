using HKORentACar.Core.Abstracts.IServices;
using HKORentACar.Core.Concretes.Entities;
using HKORentACar.Core.Concretes.Enums;
using HKORentACar.Core.Utils.GenericRepositoryPattern;

namespace HKORentACar.Business.Services
{
    public class KiralamaManager : IKiralamaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public KiralamaManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Kiralama>> GetUserKiralamalarAsync(string userId)
        {
            var tumKiralamalar = await _unitOfWork.GetRepository<Kiralama>().GetAllAsync();
            var kullaniciKiralamalari = tumKiralamalar
                .Where(k => k.AppUserId == userId)
                .OrderByDescending(k => k.BaslangicTarihi)
                .ToList();

            foreach (var kiralama in kullaniciKiralamalari)
            {
                if (kiralama.Arac == null && kiralama.AracId > 0)
                    kiralama.Arac = await _unitOfWork.GetRepository<Arac>().GetByIdAsync(kiralama.AracId);

                if (kiralama.Sube == null && kiralama.SubeId > 0)
                    kiralama.Sube = await _unitOfWork.GetRepository<Sube>().GetByIdAsync(kiralama.SubeId);
            }

            return kullaniciKiralamalari;
        }

        public async Task<List<Kiralama>> GetAllKiralamalarWithDetailsAsync()
        {
            var tumKiralamalar = await _unitOfWork.GetRepository<Kiralama>().GetAllAsync();
            foreach (var kiralama in tumKiralamalar)
            {
                if (kiralama.Arac == null && kiralama.AracId > 0)
                    kiralama.Arac = await _unitOfWork.GetRepository<Arac>().GetByIdAsync(kiralama.AracId);

                if (kiralama.Sube == null && kiralama.SubeId > 0)
                    kiralama.Sube = await _unitOfWork.GetRepository<Sube>().GetByIdAsync(kiralama.SubeId);
            }

            return tumKiralamalar.OrderByDescending(x => x.BaslangicTarihi).ToList();
        }

        public async Task<Kiralama?> GetByIdAsync(int id)
        {
            return await _unitOfWork.GetRepository<Kiralama>().GetByIdAsync(id);
        }

        public async Task<(bool IsSuccess, string Message)> KiralaAsync(int aracId, string userId, DateTime baslangicTarihi, DateTime bitisTarihi)
        {
            var arac = await _unitOfWork.GetRepository<Arac>().GetByIdAsync(aracId);
            if (arac == null)
                return (false, "Araç bulunamadı.");

            if (bitisTarihi <= baslangicTarihi)
                return (false, "Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");

            int gunSayisi = (bitisTarihi - baslangicTarihi).Days;
            if (gunSayisi == 0) gunSayisi = 1;
            decimal hesaplananUcret = gunSayisi * arac.GunlukUcret;

            // Erken rezervasyon %15 indirim kuralı
            if ((baslangicTarihi.Date - DateTime.Now.Date).TotalDays >= 7)
            {
                hesaplananUcret = Math.Round(hesaplananUcret * 0.85m, 2);
            }

            var yeniKiralama = new Kiralama
            {
                AracId = aracId,
                AppUserId = userId,
                BaslangicTarihi = baslangicTarihi,
                BitisTarihi = bitisTarihi,
                ToplamUcret = hesaplananUcret,
                Durum = KiralamaDurum.Aktif,
                SubeId = arac.SubeId,
                TeslimAlinanKM = arac.KM
            };

            arac.Durum = AracDurum.Kirada;

            await _unitOfWork.GetRepository<Kiralama>().AddAsync(yeniKiralama);
            _unitOfWork.GetRepository<Arac>().Update(arac);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Araç kiralama işleminiz başarıyla tamamlandı!");
        }

        public async Task<(bool IsSuccess, string Message)> IptalEtAsync(int kiralamaId, string userId, bool isAdmin)
        {
            var kiralama = await _unitOfWork.GetRepository<Kiralama>().GetByIdAsync(kiralamaId);
            if (kiralama == null)
                return (false, "İptal edilecek kiralama kaydı bulunamadı.");

            if (kiralama.AppUserId != userId && !isAdmin)
                return (false, "Bu işlem için yetkiniz yok.");

            if (kiralama.Durum != KiralamaDurum.Aktif)
                return (false, "Yalnızca aktif olan kiralamalar iptal edilebilir.");

            kiralama.Durum = KiralamaDurum.IptalEdildi;

            var arac = await _unitOfWork.GetRepository<Arac>().GetByIdAsync(kiralama.AracId);
            if (arac != null)
            {
                arac.Durum = AracDurum.Musait;
                _unitOfWork.GetRepository<Arac>().Update(arac);
            }

            _unitOfWork.GetRepository<Kiralama>().Update(kiralama);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Kiralama rezervasyonunuz başarıyla iptal edildi ve araç boşa çıkarıldı.");
        }

        public async Task<(bool IsSuccess, string Message)> TeslimAlAsync(int kiralamaId, int teslimEdilenKm)
        {
            var kiralama = await _unitOfWork.GetRepository<Kiralama>().GetByIdAsync(kiralamaId);
            if (kiralama == null)
                return (false, "Kiralama kaydı bulunamadı.");

            var arac = await _unitOfWork.GetRepository<Arac>().GetByIdAsync(kiralama.AracId);
            if (arac == null)
                return (false, "İlgili araç bulunamadı.");

            kiralama.Durum = KiralamaDurum.Tamamlandi;
            kiralama.TeslimEdilenKM = teslimEdilenKm;

            arac.KM = teslimEdilenKm;
            arac.Durum = AracDurum.Musait;

            _unitOfWork.GetRepository<Kiralama>().Update(kiralama);
            _unitOfWork.GetRepository<Arac>().Update(arac);
            await _unitOfWork.SaveChangesAsync();

            return (true, $"{arac.Marka} {arac.Model} başarıyla teslim alındı ve filo durumuna 'Müsait' olarak yansıtıldı.");
        }
    }
}