using HKORentACar.Core.Concretes.DTOs;

namespace HKORentACar.Core.Abstracts.IServices
{
    public interface IAracService
    {
        Task<List<AracDto>> GetAllAsync();
        Task<AracDto?> GetByIdAsync(int id);
        Task AddAsync(AracDto dto);
        Task UpdateAsync(AracDto dto);
        Task DeleteAsync(int id);
    }
}