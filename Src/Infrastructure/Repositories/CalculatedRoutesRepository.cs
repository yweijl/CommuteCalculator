using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Core.Models.Travelplans;
using Infrastructure.CosmosDb;
using Infrastructure.Entities;
using Infrastructure.HttpClients;

namespace Infrastructure.Repositories;

public class CalculatedRoutesRepository : GenericRepository, ICalculatedRoutesRepository
{
    private readonly IGoogleMapsClient _googleMapsClient;

    public CalculatedRoutesRepository(IGoogleMapsClient googleMapsClient, CommuteCalculatorContext context, IMapper mapper) : base(context, mapper)
    {
        _googleMapsClient = googleMapsClient;
    }

    public async Task<List<CalculatedRoutes>> GetAndAddUnpersistedRoutesAsync(Dictionary<Guid, HashSet<Guid>> unpersistedRoutes)
    {
        if (!unpersistedRoutes.Any())
        {
            return new List<CalculatedRoutes>();
        }

        var calculatedRoutes = await GetNewCalculatedRoutesAsync(unpersistedRoutes);

        var persistedCalculatedRoutesEntities = await GetPersistedCalculatedRoutesAsync(calculatedRoutes);
        var persistedOriginContactIds = persistedCalculatedRoutesEntities.Select(x => x.Origin.ContactId);
        
        var updatedRoutes = UpdateAndGetNewPersistedCalculatedRoutes(calculatedRoutes, persistedCalculatedRoutesEntities);

        var newCalculatedRouteEntities = calculatedRoutes
            .Where(x => !persistedOriginContactIds.Contains(x.Origin.ContactId)).ToList();

        await this.AddRangeAsync(newCalculatedRouteEntities);

        await this.SaveChangesAsync();

        var concatedCalculatedRoutes = newCalculatedRouteEntities.Concat(updatedRoutes).ToList();
        return _mapper.Map<List<CalculatedRoutes>>(concatedCalculatedRoutes);
    }

    private List<CalculatedRoutesEntity> UpdateAndGetNewPersistedCalculatedRoutes(List<CalculatedRoutesEntity> calculatedRoutes, List<CalculatedRoutesEntity> persistedCalculatedRoutesEntities)
    {
        var updatedRoutes = new List<CalculatedRoutesEntity>();
        persistedCalculatedRoutesEntities.ForEach(persistedEntity =>
        {
            var destinations = calculatedRoutes.Single(x => x.Origin.ContactId == persistedEntity.Origin.ContactId).Destinations;
            persistedEntity.Destinations.AddRange(destinations);

            updatedRoutes.Add(new CalculatedRoutesEntity 
            { 
                Id = persistedEntity.Id,
                Origin = persistedEntity.Origin,
                CreateDate = persistedEntity.CreateDate,
                Destinations = destinations,
                Etag = persistedEntity.Etag,
                ModifyDate = persistedEntity.ModifyDate
            });

        });
        return updatedRoutes;
    }

    private async Task<List<CalculatedRoutesEntity>> GetPersistedCalculatedRoutesAsync(List<CalculatedRoutesEntity> calculatedRoutes)
    {
        var originContactIds = calculatedRoutes.Select(x => x.Origin.ContactId);

        var persistedCalculatedRoutesEntities
            = await this.ListAsync<CalculatedRoutesEntity>(x => originContactIds.Contains(x.Origin.ContactId), true);
        return persistedCalculatedRoutesEntities;
    }

    private async Task<List<CalculatedRoutesEntity>> GetNewCalculatedRoutesAsync(Dictionary<Guid, HashSet<Guid>> unpersistedRoutes)
    {
        var routeMatrices = await GetGroupedContactsByOriginAsync(unpersistedRoutes);
        var calculatedRoutes = routeMatrices
            .Select(async x => await _googleMapsClient.GetCalculatedRoutesAsync(x))
            .Select(x => x.Result)
            .ToList();
        return calculatedRoutes;
    }

    private async Task<List<RouteMatrix>> GetGroupedContactsByOriginAsync(Dictionary<Guid, HashSet<Guid>> unpersistedRoutes)
    {
        var uniqueContactsIds = new HashSet<Guid>();

        foreach (var origin in unpersistedRoutes)
        {
            uniqueContactsIds.Add(origin.Key);
            foreach (var destination in origin.Value)
            {
                uniqueContactsIds.Add(destination);
            }
        }

        var contacts = await this.ListAsync<ContactEntity>(x => uniqueContactsIds.Contains(x.Id));
        var routes = new List<RouteMatrix>();

        foreach (var origin in unpersistedRoutes)
        {
            var originContact = contacts.Single(x => x.Id == origin.Key);
            var destinations = contacts.Where(x => origin.Value.Any(y => y == x.Id))
                .Select(dest =>
                {
                    var address = _mapper.Map<Address>(dest.Address);
                    address.ContactId = dest.Id;
                    return address;
                }).ToList();

            routes.Add(new RouteMatrix
            {
                Origin = _mapper.Map<Address>(originContact.Address), 
                Destinations = destinations
            });
        }

        return routes;
    }

    public async Task<List<CalculatedRoutes>> GetPersistedRouteAsync(IEnumerable<Guid> originContactIds)
    {
        var calculatedRoutes = await this.ListAsync<CalculatedRoutesEntity>(x => originContactIds.Contains(x.Origin.ContactId));
        return _mapper.Map<List<CalculatedRoutes>>(calculatedRoutes);
    }
}