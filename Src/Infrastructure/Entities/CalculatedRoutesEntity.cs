namespace Infrastructure.Entities;

public class CalculatedRoutesEntity : EntityBase
{
    public AddressEntity Origin { get; set; } = default!;
    public List<DestinationEntity> Destinations { get; set; } = new();
}

