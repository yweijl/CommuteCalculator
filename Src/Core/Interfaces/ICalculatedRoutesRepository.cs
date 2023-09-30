using Core.Models.Travelplans;

namespace Core.Interfaces
{
    public interface ICalculatedRoutesRepository
    {
        Task<List<CalculatedRoutes>> GetPersistedRouteAsync(IEnumerable<Guid> originContactIds);
        Task<List<CalculatedRoutes>> GetAndAddUnpersistedRoutesAsync(Dictionary<Guid, HashSet<Guid>> unpersistedRoutes);
    }
}