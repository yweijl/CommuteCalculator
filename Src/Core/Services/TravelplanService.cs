using Core.Interfaces;
using Core.Models.Travelplans;

namespace Core.Services;

public class TravelplanService : ITravelplanService
{
    private readonly ICalculatedRoutesRepository _travelplanRepository;

    public TravelplanService(ICalculatedRoutesRepository travelplanRepository)
    {
        _travelplanRepository = travelplanRepository;
    }

    public async Task<List<RouteRegistration>> CalculateTravelplanAsync(Guid userId, List<WayPoints> selectedRoutes)
    {
        var calculatedRoutes = await GetCalculatedRoutesAsync(selectedRoutes);

        var travelPlans = new List<RouteRegistration>();
        foreach (var route in selectedRoutes)
        {
            var calculatedRoute = calculatedRoutes.First(x => x.Origin.ContactId == route.OriginContactId);
            var origin = calculatedRoute.Origin;

            var destination = calculatedRoute.Destinations.Single(x => x.Address.ContactId == route.DestinationContactId);

            travelPlans.Add(new RouteRegistration
            {
                Origin = calculatedRoute.Origin,
                Destination = destination.Address,
                Distance = destination.Distance
            });
        }

        return travelPlans;
    }

    private async Task<IEnumerable<CalculatedRoutes>> GetCalculatedRoutesAsync(List<WayPoints> selectedRoutes)
    {
        var persistedRoutes = await _travelplanRepository.GetPersistedRouteAsync(selectedRoutes.Select(x => x.OriginContactId));
        var unpersistedRoutes = await GetAndAddUnpersistedRoutesAsync(persistedRoutes, selectedRoutes);

        return persistedRoutes.Concat(unpersistedRoutes).GroupBy(x => x.Id).Select(x => new CalculatedRoutes
        {
            Id = x.Key,
            Origin = x.First().Origin,
            Destinations = x.SelectMany(y => y.Destinations).ToList()
        });
    }

    private Task<List<CalculatedRoutes>> GetAndAddUnpersistedRoutesAsync(List<CalculatedRoutes> persistedRoutes, List<WayPoints> selectedRoutes)
    {
        var unpersistedRoutes = new Dictionary<Guid, HashSet<Guid>>();

        var groupedRoutes = selectedRoutes
            .GroupBy(x => x.OriginContactId)
            .Select(grouping => (OriginContactId: grouping.Key, Destionations: grouping.Select(x => x.DestinationContactId)));

        foreach (var (OriginContactId, Destinations) in groupedRoutes)
        {
            var origin = persistedRoutes.SingleOrDefault(x => x.Origin.ContactId == OriginContactId);
            if (origin == null)
            {
                unpersistedRoutes.Add(OriginContactId, Destinations.ToHashSet());
                continue;
            }
            foreach (var destinationContactId in Destinations)
            {
                var destination = origin.Destinations.SingleOrDefault(x => x.Address.ContactId == destinationContactId);
                if (destination == null)
                {
                    if (!unpersistedRoutes.ContainsKey(OriginContactId))
                    {
                        unpersistedRoutes.Add(OriginContactId, new HashSet<Guid> { destinationContactId });
                    }
                    else
                    {
                        unpersistedRoutes[OriginContactId].Add(destinationContactId);
                    }
                }
            }
        }

        return _travelplanRepository.GetAndAddUnpersistedRoutesAsync(unpersistedRoutes);
    }
}