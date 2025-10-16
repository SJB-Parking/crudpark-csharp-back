using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudPark_Back.Models.Entities;

[Table("subscription_vehicles")]
public class SubscriptionVehicle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("subscription_id")]
    public int SubscriptionId { get; set; }

    [Column("vehicle_id")]
    public int VehicleId { get; set; }

    [Column("added_at")]
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("SubscriptionId")]
    public MonthlySubscription MonthlySubscription { get; set; } = null!;

    [ForeignKey("VehicleId")]
    public Vehicle Vehicle { get; set; } = null!;
}