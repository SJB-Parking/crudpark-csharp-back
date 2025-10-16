using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CrudPark_Back.Models.Enums;

namespace CrudPark_Back.Models.Entities;

[Table("vehicles")]
public class Vehicle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    [Column("license_plate")]
    public string LicensePlate { get; set; } = string.Empty;

    [Column("vehicle_type")]
    public VehicleType VehicleType { get; set; } = VehicleType.Car;

    [MaxLength(100)]
    [Column("brand")]
    public string? Brand { get; set; }

    [MaxLength(100)]
    [Column("model")]
    public string? Model { get; set; }

    [MaxLength(50)]
    [Column("color")]
    public string? Color { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<CustomerVehicle> CustomerVehicles { get; set; } = new List<CustomerVehicle>();
    public ICollection<SubscriptionVehicle> SubscriptionVehicles { get; set; } = new List<SubscriptionVehicle>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}