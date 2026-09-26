using HKORentACar.Core.Abstracts.IServices;
using HKORentACar.Core.Concretes.DTOs;
using HKORentACar.Core.Concretes.Entities;
using HKORentACar.Core.Utils.GenericRepositoryPattern;

namespace HKORentACar.Business.Services
{
    public class AracManager : IAracService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AracManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AracDto>> GetAllAsync()
        {
            var araclar = await _unitOfWork.GetRepository<Arac>().GetAllAsync();
            return araclar.Select(a => new AracDto
            {
                Id = a.Id,
                Marka = a.Marka,
                Model = a.Model,
                Yil = a.Yil,
                Plaka = a.Plaka,
                GunlukUcret = a.GunlukUcret,
                KM = a.KM,
                ResimUrl = a.ResimUrl,
                Durum = a.Durum,
                YakitTipi = a.YakitTipi,
                VitesTipi = a.VitesTipi,
                KategoriId = a.KategoriId,
                SubeId = a.SubeId
            }).ToList();
        }

        public async Task<AracDto?> GetByIdAsync(int id)
        {
            var arac = await _unitOfWork.GetRepository<Arac>().GetByIdAsync(id);
            if (arac == null) return null;

            return new AracDto
            {
                Id = arac.Id,
                Marka = arac.Marka,
                Model = arac.Model,
                Yil = arac.Yil,
                Plaka = arac.Plaka,
                GunlukUcret = arac.GunlukUcret,
                KM = arac.KM,
                ResimUrl = arac.ResimUrl,
                Durum = arac.Durum,
                YakitTipi = arac.YakitTipi,
                VitesTipi = arac.VitesTipi,
                KategoriId = arac.KategoriId,
                SubeId = arac.SubeId
            };
        }

        public async Task AddAsync(AracDto dto)
        {
            var entity = new Arac
            {
                Marka = dto.Marka,
                Model = dto.Model,
                Yil = dto.Yil,
                Plaka = dto.Plaka,
                GunlukUcret = dto.GunlukUcret,
                KM = dto.KM,
                ResimUrl = dto.ResimUrl,
                Durum = dto.Durum,
                YakitTipi = dto.YakitTipi,
                VitesTipi = dto.VitesTipi,
                KategoriId = dto.KategoriId,
                SubeId = dto.SubeId
            };

            await _unitOfWork.GetRepository<Arac>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(AracDto dto)
        {
            var entity = await _unitOfWork.GetRepository<Arac>().GetByIdAsync(dto.Id);
            if (entity != null)
            {
                entity.Marka = dto.Marka;
                entity.Model = dto.Model;
                entity.Yil = dto.Yil;
                entity.Plaka = dto.Plaka;
                entity.GunlukUcret = dto.GunlukUcret;
                entity.KM = dto.KM;
                entity.ResimUrl = dto.ResimUrl;
                entity.Durum = dto.Durum;
                entity.YakitTipi = dto.YakitTipi;
                entity.VitesTipi = dto.VitesTipi;
                entity.KategoriId = dto.KategoriId;
                entity.SubeId = dto.SubeId;

                _unitOfWork.GetRepository<Arac>().Update(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.GetRepository<Arac>().GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.GetRepository<Arac>().Delete(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}