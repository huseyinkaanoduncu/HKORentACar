using HKORentACar.Core.Concretes.Entities;

namespace HKORentACar.Core.Abstracts.IServices
{
    public interface IMusteriService
    {
        Task<List<AppUser>> GetAllMusterilerAsync();
        Task<(bool IsSuccess, string Message)> DeleteMusteriAsync(string id);
    }
}