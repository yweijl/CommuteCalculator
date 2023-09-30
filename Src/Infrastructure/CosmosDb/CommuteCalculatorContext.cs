using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CosmosDb;

public class CommuteCalculatorContext : DbContext
{
    public CommuteCalculatorContext(DbContextOptions<CommuteCalculatorContext> options)
            : base(options)
    {}

    public DbSet<UserEntity> Users { get; set; } = default!;
    public DbSet<ContactEntity> Contacts { get; set; } = default!;
    public DbSet<CalculatedRoutesEntity> CalculatedRoutes{ get; set; } = default!;
    public DbSet<UserTravelplanEntity> UserTravelplans{ get; set; } = default!;
    public DbSet<TravelplanEntity> TravelPlans{ get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactEntity>(c =>
        {
            c.Property(x => x.Id).ValueGeneratedOnAdd();
            c.Property(x => x.CreateDate).HasValueGenerator<UtcDateTimeGenerator>().ValueGeneratedOnAdd();
            c.Property(x => x.ModifyDate).HasValueGenerator<UtcDateTimeGenerator>().ValueGeneratedOnUpdate();
            c.ToContainer("Contacts");
            c.HasNoDiscriminator();
            c.HasPartitionKey(x => x.Id);
            c.HasKey(x => x.Id);
            c.Property(x => x.Etag).IsETagConcurrency();
            c.OwnsOne(x => x.Address, a =>
            {
                a.WithOwner().HasForeignKey(x => x.ContactId);
            });
            c.HasIndex(x => x.UserId);
            c.HasIndex(nameof(ContactEntity.FirstName), nameof(ContactEntity.LastName)).IsUnique();
        });

        modelBuilder.Entity<UserEntity>(u =>
        {
            u.ToContainer("Users");
            u.Property(x => x.Id).ValueGeneratedOnAdd();
            u.Property(x => x.CreateDate).HasValueGenerator<UtcDateTimeGenerator>().ValueGeneratedOnAdd();
            u.Property(x => x.ModifyDate).HasValueGenerator<UtcDateTimeGenerator>().ValueGeneratedOnUpdate();
            u.HasNoDiscriminator();
            u.HasPartitionKey(x => x.Id);
            u.HasKey(x => x.Id);
            u.Property(x => x.Etag).IsETagConcurrency();
            u.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<CalculatedRoutesEntity>(c =>
        {
            c.ToContainer("CalculatedRoutes");
            c.Property(x => x.Id).ValueGeneratedOnAdd();
            c.Property(x => x.CreateDate).HasValueGenerator<UtcDateTimeGenerator>().ValueGeneratedOnAdd();
            c.Property(x => x.ModifyDate).HasValueGenerator<UtcDateTimeGenerator>().ValueGeneratedOnUpdate();
            c.HasNoDiscriminator();
            c.HasPartitionKey(x => x.Id);
            c.HasKey(x => x.Id);
            c.Property(x => x.Etag).IsETagConcurrency();
        });

        modelBuilder.Entity<UserTravelplanEntity>(u =>
        {
            u.ToContainer("UserTravelPlans");
            u.Property(x => x.Id).ValueGeneratedOnAdd();
            u.Property(x => x.CreateDate).HasValueGenerator<UtcDateTimeGenerator>().ValueGeneratedOnAdd();
            u.Property(x => x.ModifyDate).HasValueGenerator<UtcDateTimeGenerator>().ValueGeneratedOnUpdate();
            u.HasNoDiscriminator();
            u.HasPartitionKey(x => x.UserId);
            u.Property(x => x.Id).ToJsonProperty("id");
            u.HasKey(x => x.Id);
            u.Property(x => x.Etag).IsETagConcurrency();
            u.OwnsMany(x => x.Travelplans, t =>
            {
                t.Ignore(x => x.Etag);
                t.WithOwner().HasForeignKey(x => x.UserTravelplanId);
                t.HasKey(x => x.Id);
                t.Property(x => x.Id).ValueGeneratedOnAdd();
                t.Property(x => x.CreateDate).HasValueGenerator<UtcDateTimeGenerator>().ValueGeneratedOnAdd();
                t.Property(x => x.ModifyDate).HasValueGenerator<UtcDateTimeGenerator>().ValueGeneratedOnUpdate();
            });
        });
    }
}