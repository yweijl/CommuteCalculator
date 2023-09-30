using Core.Models.Travelplans;

namespace Core.Interfaces;

public interface IUserTravelplanService
{
    Task<UserTravelplan> GetPersistedTravelplansAsync(Guid userId);
    Task<bool> DeleteTravelplanByIdAsync(Guid id, Guid userId);
    Task<bool> SaveAsync(Guid userId, Travelplan travelPlan);
    Task<(string, byte[])> DownloadTravelplanAsync(int monthNumber, Guid userId);
}