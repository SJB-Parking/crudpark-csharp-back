using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CrudPark_Back.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Models.Entities;

[Table("shifts")]
public class Shift
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("operator_id")]
    public int OperatorId { get; set; }

    [Column("shift_start")]
    public DateTime ShiftStart { get; set; } = DateTime.UtcNow;

    [Column("shift_end")]
    public DateTime? ShiftEnd { get; set; }

    [Column("initial_cash")]
    [Precision(10, 2)]
    public decimal InitialCash { get; set; }

    [Column("final_cash")]
    [Precision(10, 2)]
    public decimal? FinalCash { get; set; }

    [Column("status")]
    public ShiftStatus Status { get; set; } = ShiftStatus.Open;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("OperatorId")]
    public Operator Operator { get; set; } = null!;
}