namespace Infrastructure.Entities;

public class TravelplanEntity : EntityBase
{
    public DateTime RegistrationDate { get; set; }
    public List<RouteRegistrationEntity> Routes { get; set; } = new List<RouteRegistrationEntity>();
    public Guid UserTravelplanId { get; set; }
}
