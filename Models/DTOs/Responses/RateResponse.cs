namespace CrudPark_Back.Models.DTOs.Responses;

public class RateResponse
{
    public int Id { get; set; }
    public string RateName { get; set; } = string.Empty;
    
    public string VehicleType { get; set; } = string.Empty; 
    public decimal HourlyRate { get; set; }
    public decimal FractionRate { get; set; }
    public decimal? DailyCap { get; set; }
    public int GracePeriodMinutes { get; set; }
    public bool IsActive { get; set; }
    public DateTime EffectiveFrom { get; set; }
}