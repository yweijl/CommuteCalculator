using AutoMapper;
using Infrastructure.Entities;
using Infrastructure.HttpClients.GoogleEntities;
using Infrastructure.Mappers;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.HttpClients
{
    public class GoogleMapsClient : HttpClientBase, IGoogleMapsClient
    {
        private readonly string key;

        public GoogleMapsClient(IOptions<GoogleMapsConfig> options, HttpClient client, IMapper mapper) : base(client, mapper)
        {
            if (string.IsNullOrWhiteSpace(options.Value.ApiKey) || string.IsNullOrWhiteSpace(options.Value.BaseUrl))
            {
                throw new ArgumentNullException($"Google Maps Api configuration error: {nameof(options.Value.BaseUrl)} is missing or empty. or {nameof(options.Value.ApiKey)} is missing or empty.");
            }
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
            this.key = options.Value.ApiKey;
        }

        public async Task<CalculatedRoutesEntity> GetCalculatedRoutesAsync(RouteMatrix route)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "destinations", string.Join('|', route.Destinations.Select(x => x.ToString())) },
                { "origins", route.Origin.ToString() },
                { "units", "metric" },
                { "language", "nl" },
                { "mode", "driving" },
                { "region", "nl" },
                { "key", key }
            };

            var response = await base.GetAsync("/maps/api/distancematrix/json?", queryParams);
            var content = await response.Content.ReadAsStringAsync();
            var googleMapsDistanceEntity = JsonSerializer.Deserialize<GoogleMapsDistanceEntity>(content);
            if (googleMapsDistanceEntity == null || !googleMapsDistanceEntity.Status.ToLower().Equals("ok"))
            {
                throw new InvalidOperationException($"Could not get distance of origin {route.Origin.ToString} to {string.Join(',', route.Destinations.Select(x => x.ToString()))}");
            }

            var calculatedRoute = new CalculatedRoutesEntity
            {
                Origin = _mapper.Map<AddressEntity>(route.Origin),
                Destinations = new()
            };

            var destionationAddresses = _mapper.Map<List<AddressEntity>>(route.Destinations);
            
            for (int i = 0, n = destionationAddresses.Count; i < n; i++)
            {
                calculatedRoute.Destinations.Add(
                    new DestinationEntity
                    {
                        Address = destionationAddresses[i],
                        Distance = googleMapsDistanceEntity.Rows.First().Elements[i].Distance.Value
                    });
            }

            return calculatedRoute;
        }
    }
}