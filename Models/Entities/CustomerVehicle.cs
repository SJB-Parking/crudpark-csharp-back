using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudPark_Back.Models.Entities;

[Table("customer_vehicles")]
public class CustomerVehicle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("vehicle_id")]
    public int VehicleId { get; set; }

    [Column("is_primary")]
    public bool IsPrimary { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; } = null!;

    [ForeignKey("VehicleId")]
    public Vehicle Vehicle { get; set; } = null!;
}