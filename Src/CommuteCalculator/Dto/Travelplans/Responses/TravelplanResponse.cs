namespace CommuteCalculator.Dto.Travelplans.Responses;

public class TravelplanResponse
{
    public Guid Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<RouteRegistrationResponse> Routes { get; set; } = new List<RouteRegistrationResponse>();
}