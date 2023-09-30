    namespace Infrastructure.Entities;

public abstract class EntityBase
{
    public Guid Id { get; set; }
    public string Etag { get; set; } = default!;
    public DateTime CreateDate { get; set; }
    public DateTime ModifyDate { get; set; }
}