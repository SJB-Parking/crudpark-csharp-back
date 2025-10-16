using Microsoft.EntityFrameworkCore;
using CrudPark_Back.Models.Entities;
using CrudPark_Back.Models.Enums;

namespace CrudPark_Back.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<CustomerVehicle> CustomerVehicles { get; set; }
    public DbSet<MonthlySubscription> MonthlySubscriptions { get; set; }
    public DbSet<SubscriptionVehicle> SubscriptionVehicles { get; set; }
    public DbSet<Operator> Operators { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Shift> Shifts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar conversiones de enums a strings
        modelBuilder.Entity<Vehicle>()
            .Property(v => v.VehicleType)
            .HasConversion<string>();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.TicketType)
            .HasConversion<string>();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Payment>()
            .Property(p => p.PaymentMethod)
            .HasConversion<string>();

        modelBuilder.Entity<Shift>()
            .Property(s => s.Status)
            .HasConversion<string>();

        // Indices únicos
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.LicensePlate)
            .IsUnique();

        modelBuilder.Entity<Operator>()
            .HasIndex(o => o.Username)
            .IsUnique();

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<MonthlySubscription>()
            .HasIndex(ms => ms.SubscriptionCode)
            .IsUnique();

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => t.Folio)
            .IsUnique();

        // Relaciones
        modelBuilder.Entity<CustomerVehicle>()
            .HasOne(cv => cv.Customer)
            .WithMany(c => c.CustomerVehicles)
            .HasForeignKey(cv => cv.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CustomerVehicle>()
            .HasOne(cv => cv.Vehicle)
            .WithMany(v => v.CustomerVehicles)
            .HasForeignKey(cv => cv.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MonthlySubscription>()
            .HasOne(ms => ms.Customer)
            .WithMany(c => c.MonthlySubscriptions)
            .HasForeignKey(ms => ms.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SubscriptionVehicle>()
            .HasOne(sv => sv.MonthlySubscription)
            .WithMany(ms => ms.SubscriptionVehicles)
            .HasForeignKey(sv => sv.SubscriptionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SubscriptionVehicle>()
            .HasOne(sv => sv.Vehicle)
            .WithMany(v => v.SubscriptionVehicles)
            .HasForeignKey(sv => sv.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Vehicle)
            .WithMany(v => v.Tickets)
            .HasForeignKey(t => t.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Operator)
            .WithMany(o => o.Tickets)
            .HasForeignKey(t => t.OperatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.MonthlySubscription)
            .WithMany(ms => ms.Tickets)
            .HasForeignKey(t => t.SubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Ticket)
            .WithMany(t => t.Payments)
            .HasForeignKey(p => p.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Operator)
            .WithMany(o => o.Payments)
            .HasForeignKey(p => p.OperatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Shift>()
            .HasOne(s => s.Operator)
            .WithMany(o => o.Shifts)
            .HasForeignKey(s => s.OperatorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed Data
        SeedData(modelBuilder);
    }

     private void SeedData(ModelBuilder modelBuilder)
    {
        // Fechas estáticas para evitar el error de modelo no determinístico
        var seedDate = new DateTime(2025, 1, 15, 0, 0, 0, DateTimeKind.Utc);
        var subscriptionStart = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var subscriptionEnd = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc);

        // Hashes pre-generados (estáticos) para evitar el error
        // admin123 -> $2a$11$ZK5L5Y5Y5Y5Y5Y5Y5Y5Y5OqH0J0J0J0J0J0J0J0J0J0J0J0J0J0J0
        // operator123 -> $2a$11$XK5L5Y5Y5Y5Y5Y5Y5Y5Y5OqH0J0J0J0J0J0J0J0J0J0J0J0J0J0J1
        
        var adminPasswordHash = "$2a$11$kH3ulu5AEQGioyzXDx.pg.4JDqE9/mACqZbtdymRdAm.zgUN2rX7.";
        var operatorPasswordHash = "$2a$11$a2ISYKBQqXw.27Xd8WwykOc9YpYSZCfKVvV/WqdEZ7c3mdAY4d88K";

        // Seed Operator por defecto
        modelBuilder.Entity<Operator>().HasData(
            new Operator
            {
                Id = 1,
                FullName = "Administrator",
                Email = "admin@crudpark.com",
                Username = "admin",
                PasswordHash = adminPasswordHash,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Operator
            {
                Id = 2,
                FullName = "Juan Pérez",
                Email = "juan@crudpark.com",
                Username = "jperez",
                PasswordHash = operatorPasswordHash,
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            }
        );

        // Seed Rate por defecto
        modelBuilder.Entity<Rate>().HasData(
            new Rate
            {
                Id = 1,
                RateName = "Tarifa Estándar 2025",
                HourlyRate = 3000m,
                FractionRate = 1000m,
                DailyCap = 30000m,
                GracePeriodMinutes = 30,
                IsActive = true,
                EffectiveFrom = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            }
        );

        // Seed Customer de prueba
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = 1,
                FullName = "María González",
                Email = "maria.gonzalez@email.com",
                Phone = "3001234567",
                IdentificationNumber = "1234567890",
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            }
        );

        // Seed Vehicles de prueba
        modelBuilder.Entity<Vehicle>().HasData(
            new Vehicle
            {
                Id = 1,
                LicensePlate = "ABC123",
                VehicleType = VehicleType.Car,
                Brand = "Toyota",
                Model = "Corolla",
                Color = "Blanco",
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new Vehicle
            {
                Id = 2,
                LicensePlate = "XYZ789",
                VehicleType = VehicleType.Motorcycle,
                Brand = "Yamaha",
                Model = "FZ",
                Color = "Negro",
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            }
        );

        // Seed CustomerVehicle
        modelBuilder.Entity<CustomerVehicle>().HasData(
            new CustomerVehicle
            {
                Id = 1,
                CustomerId = 1,
                VehicleId = 1,
                IsPrimary = true,
                CreatedAt = seedDate
            },
            new CustomerVehicle
            {
                Id = 2,
                CustomerId = 1,
                VehicleId = 2,
                IsPrimary = false,
                CreatedAt = seedDate
            }
        );

        // Seed MonthlySubscription
        modelBuilder.Entity<MonthlySubscription>().HasData(
            new MonthlySubscription
            {
                Id = 1,
                CustomerId = 1,
                SubscriptionCode = "SUB-2025-001",
                StartDate = subscriptionStart,
                EndDate = subscriptionEnd,
                IsActive = true,
                AmountPaid = 150000m,
                MaxVehicles = 2,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            }
        );

        // Seed SubscriptionVehicles
        modelBuilder.Entity<SubscriptionVehicle>().HasData(
            new SubscriptionVehicle
            {
                Id = 1,
                SubscriptionId = 1,
                VehicleId = 1,
                AddedAt = seedDate
            },
            new SubscriptionVehicle
            {
                Id = 2,
                SubscriptionId = 1,
                VehicleId = 2,
                AddedAt = seedDate
            }
        );
    }
}

        