namespace Infrastructure.Entities;

public class UserTravelplanEntity : EntityBase
{
    public Guid UserId { get; set; }
    public List<TravelplanEntity> Travelplans { get; set; } = new List<TravelplanEntity>();
}

