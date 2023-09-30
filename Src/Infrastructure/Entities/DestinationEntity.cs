namespace Infrastructure.Entities
{
    public class DestinationEntity
    {
        public AddressEntity Address { get; set; } = default!;
        public int Distance { get; set; } = default!;
    }
}