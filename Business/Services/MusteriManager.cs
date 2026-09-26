using HKORentACar.Core.Abstracts.IServices;
using HKORentACar.Core.Concretes.Entities;
using HKORentACar.Core.Concretes.Enums;
using HKORentACar.Core.Utils.GenericRepositoryPattern;

namespace HKORentACar.Business.Services
{
    public class MusteriManager : IMusteriService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MusteriManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AppUser>> GetAllMusterilerAsync()
        {
            var musteriler = await _unitOfWork.GetRepository<AppUser>().GetAllAsync();
            return musteriler.ToList();
        }

        public async Task<(bool IsSuccess, string Message)> DeleteMusteriAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return (false, "Geçersiz müşteri kimliği.");

            // Aktif kiralama kontrolü (İş Kuralı)
            var tumKiralamalar = await _unitOfWork.GetRepository<Kiralama>().GetAllAsync();
            bool aktifKiralamaVarMi = tumKiralamalar.Any(k => k.AppUserId == id && k.Durum == KiralamaDurum.Aktif);

            if (aktifKiralamaVarMi)
            {
                return (false, "Bu müşterinin şu anda teslim etmediği aktif bir araç kiralaması bulunmaktadır. Önce araç teslim alınmalıdır!");
            }

            var musteriler = await _unitOfWork.GetRepository<AppUser>().GetAllAsync();
            var musteri = musteriler.FirstOrDefault(u => u.Id == id);

            if (musteri == null)
                return (false, "Müşteri bulunamadı.");

            _unitOfWork.GetRepository<AppUser>().Delete(musteri);
            await _unitOfWork.SaveChangesAsync();

            return (true, $"{musteri.Ad} {musteri.Soyad} isimli müşteri başarıyla silindi.");
        }
    }
}