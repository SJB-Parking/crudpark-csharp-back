using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Models.Entities;

[Table("rates")]
public class Rate
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("rate_name")]
    public string RateName { get; set; } = string.Empty;

    [Column("hourly_rate")]
    [Precision(10, 2)]
    public decimal HourlyRate { get; set; }

    [Column("fraction_rate")]
    [Precision(10, 2)]
    public decimal FractionRate { get; set; }

    [Column("daily_cap")]
    [Precision(10, 2)]
    public decimal? DailyCap { get; set; }

    [Column("grace_period_minutes")]
    public int GracePeriodMinutes { get; set; } = 30;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("effective_from")]
    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}