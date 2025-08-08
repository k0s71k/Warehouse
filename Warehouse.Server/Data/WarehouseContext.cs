using Microsoft.EntityFrameworkCore;
using Warehouse.Server.Models;

namespace Warehouse.Server.Data;

public class WarehouseContext: DbContext
{
    public WarehouseContext(DbContextOptions options) : base(options)
    {
        this.Database.EnsureCreated();
    }

    protected WarehouseContext()
    {
    }

    public DbSet<Resource> Resources { get; set; }
    public DbSet<Balance> Balances { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<MeasureUnit> MeasureUnits { get; set; }
    public DbSet<ReceiveResource> ReceiveResources { get; set; }
    public DbSet<ReceiveDocument> ReceiveDocuments { get; set; }
    public DbSet<SendResource> SendResources { get; set; }
    public DbSet<SendDocument> SendDocuments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Resource>()
            .HasMany(x => x.MeasureUnits)
            .WithMany(x => x.Resources)
            .UsingEntity<Balance>(x =>
            {
                x.HasOne(x => x.Resource)
                    .WithMany(x => x.Balances)
                    .HasForeignKey(x => x.ResourceId);

                x.HasOne(x => x.MeasureUnit)
                    .WithMany(x => x.Balances)
                    .HasForeignKey(x => x.MeasureUnitId);

                x.HasKey(x => new { x.ResourceId, x.MeasureUnitId });
            });

        modelBuilder.Entity<Resource>()
            .HasIndex(x => x.Name)
            .IsUnique(true);

        modelBuilder.Entity<MeasureUnit>()
            .HasIndex(x => x.Name)
            .IsUnique(true);

        // Client
        modelBuilder.Entity<Client>()
            .HasMany(x => x.SendDocuments)
            .WithOne(x => x.Client)
            .HasForeignKey(x => x.ClientId);

        modelBuilder.Entity<Client>()
            .HasIndex(x => x.Name)
            .IsUnique(true);

        // Documents
        modelBuilder.Entity<ReceiveDocument>()
            .HasMany(x => x.Resources)
            .WithOne(x => x.Document)
            .HasForeignKey(x => x.DocumentId);

        modelBuilder.Entity<ReceiveDocument>()
            .HasIndex(x => x.Number)
            .IsUnique(true);

        modelBuilder.Entity<SendDocument>()
            .HasMany(x => x.Resources)
            .WithOne(x => x.Document)
            .HasForeignKey(x => x.DocumentId);

        modelBuilder.Entity<SendDocument>()
            .HasIndex(x => x.Number)
            .IsUnique(true);

        // Document Resources
        modelBuilder.Entity<SendResource>()
            .HasOne(x => x.Resource)
            .WithMany(x => x.SendResources)
            .HasForeignKey(x => x.ResourceId);

        modelBuilder.Entity<SendResource>()
            .HasOne(x => x.MeasureUnit)
            .WithMany(x => x.SendResources)
            .HasForeignKey(x => x.MeasureUnitId);

        modelBuilder.Entity<ReceiveResource>()
            .HasOne(x => x.Resource)
            .WithMany(x => x.ReceiveResources)
            .HasForeignKey(x => x.ResourceId);

        modelBuilder.Entity<ReceiveResource>()
            .HasOne(x => x.MeasureUnit)
            .WithMany(x => x.ReceiveResources)
            .HasForeignKey(x => x.MeasureUnitId);
    }
}
