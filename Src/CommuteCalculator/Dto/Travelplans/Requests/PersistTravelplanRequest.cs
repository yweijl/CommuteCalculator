namespace CommuteCalculator.Dto.Travelplans.Requests
{
    public class PersistTravelplanRequest
    {
        public string Name { get; set; } = default!;
        public DateTime RegistrationDate { get; set; }
        public List<RouteRegistrationRequest> Routes { get; set; } = new List<RouteRegistrationRequest>();
    }
}