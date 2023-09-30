using Core.Models.Travelplans;

namespace Core.Interfaces;

public interface ITravelplanService
{
    Task<List<RouteRegistration>> CalculateTravelplanAsync(Guid userId, List<WayPoints> routes);
}