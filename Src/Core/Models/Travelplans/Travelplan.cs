namespace Core.Models.Travelplans;

public class Travelplan
{
    public Guid Id { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<RouteRegistration> Routes { get; set; } = new List<RouteRegistration>();
    public Guid UserId { get; set; }
}
