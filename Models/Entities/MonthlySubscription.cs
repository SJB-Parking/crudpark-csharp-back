using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Models.Entities;

[Table("monthly_subscriptions")]
public class MonthlySubscription
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("subscription_code")]
    public string SubscriptionCode { get; set; } = string.Empty;

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("amount_paid")]
    [Precision(10, 2)]
    public decimal AmountPaid { get; set; }

    [Column("max_vehicles")]
    public int MaxVehicles { get; set; } = 2;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; } = null!;

    public ICollection<SubscriptionVehicle> SubscriptionVehicles { get; set; } = new List<SubscriptionVehicle>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}