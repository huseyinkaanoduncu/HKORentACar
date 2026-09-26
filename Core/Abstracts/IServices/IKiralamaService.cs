using HKORentACar.Core.Concretes.Entities;

namespace HKORentACar.Core.Abstracts.IServices
{
    public interface IKiralamaService
    {
        Task<List<Kiralama>> GetUserKiralamalarAsync(string userId);
        Task<List<Kiralama>> GetAllKiralamalarWithDetailsAsync();
        Task<Kiralama?> GetByIdAsync(int id);
        Task<(bool IsSuccess, string Message)> KiralaAsync(int aracId, string userId, DateTime baslangicTarihi, DateTime bitisTarihi);
        Task<(bool IsSuccess, string Message)> IptalEtAsync(int kiralamaId, string userId, bool isAdmin);
        Task<(bool IsSuccess, string Message)> TeslimAlAsync(int kiralamaId, int teslimEdilenKm);
    }
}