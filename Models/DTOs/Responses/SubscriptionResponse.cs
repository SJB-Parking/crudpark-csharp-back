namespace CrudPark_Back.Models.DTOs.Responses;

public class SubscriptionResponse
{
    public int Id { get; set; }
    public string SubscriptionCode { get; set; } = string.Empty;
    public CustomerResponse Customer { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public decimal AmountPaid { get; set; }
    public int MaxVehicles { get; set; }
    public List<VehicleResponse> Vehicles { get; set; } = new();
    public int DaysRemaining { get; set; }
}