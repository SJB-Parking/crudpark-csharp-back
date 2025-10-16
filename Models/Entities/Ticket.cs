using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CrudPark_Back.Models.Enums;

namespace CrudPark_Back.Models.Entities;

[Table("tickets")]
public class Ticket
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("folio")]
    public string Folio { get; set; } = string.Empty;

    [Column("vehicle_id")]
    public int VehicleId { get; set; }

    [Column("operator_id")]
    public int OperatorId { get; set; }

    [Column("subscription_id")]
    public int? SubscriptionId { get; set; }

    [Column("entry_datetime")]
    public DateTime EntryDatetime { get; set; } = DateTime.UtcNow;

    [Column("exit_datetime")]
    public DateTime? ExitDatetime { get; set; }

    [Column("ticket_type")]
    public TicketType TicketType { get; set; }

    [Column("status")]
    public TicketStatus Status { get; set; } = TicketStatus.Open;

    [Column("parking_duration_minutes")]
    public int? ParkingDurationMinutes { get; set; }

    [Column("qr_code_data")]
    [MaxLength(500)]
    public string QrCodeData { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("VehicleId")]
    public Vehicle Vehicle { get; set; } = null!;

    [ForeignKey("OperatorId")]
    public Operator Operator { get; set; } = null!;

    [ForeignKey("SubscriptionId")]
    public MonthlySubscription? MonthlySubscription { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}