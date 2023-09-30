using Core.Models.Travelplans;

namespace Core.Interfaces;

public interface IUserTravelplansRepository
{
    Task<bool> DeleteTravelplanByIdAsync(Guid travelplanId, Guid userId);
    Task<UserTravelplan> GetByUserIdAsync(Guid userId);
    Task<bool> SaveAsync(Guid userId, Travelplan userTravelPlan);
    Task<List<Travelplan>> GetTravelplansByMonthAsync(int monthNumber, Guid UserId);
}
