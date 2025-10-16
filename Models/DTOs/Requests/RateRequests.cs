namespace CrudPark_Back.Models.DTOs.Requests;

public class CreateRateRequest
{
    public string RateName { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public decimal FractionRate { get; set; }
    public decimal? DailyCap { get; set; }
    public int GracePeriodMinutes { get; set; } = 30;
    public DateTime EffectiveFrom { get; set; }
}

public class UpdateRateRequest
{
    public string? RateName { get; set; }
    public decimal? HourlyRate { get; set; }
    public decimal? FractionRate { get; set; }
    public decimal? DailyCap { get; set; }
    public int? GracePeriodMinutes { get; set; }
    public bool? IsActive { get; set; }
}