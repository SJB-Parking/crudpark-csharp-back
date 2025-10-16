using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CrudPark_Back.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Models.Entities;

[Table("payments")]
public class Payment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ticket_id")]
    public int TicketId { get; set; }

    [Column("operator_id")]
    public int OperatorId { get; set; }

    [Column("amount")]
    [Precision(10, 2)]
    public decimal Amount { get; set; }

    [Column("payment_method")]
    public PaymentMethod PaymentMethod { get; set; }

    [Column("payment_datetime")]
    public DateTime PaymentDatetime { get; set; } = DateTime.UtcNow;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("TicketId")]
    public Ticket Ticket { get; set; } = null!;

    [ForeignKey("OperatorId")]
    public Operator Operator { get; set; } = null!;
}