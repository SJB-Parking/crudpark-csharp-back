namespace CrudPark_Back.Models.DTOs.Requests;

public class CreateSubscriptionRequest
{
    public int CustomerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal AmountPaid { get; set; }
    public int MaxVehicles { get; set; } = 2;
    public List<int> VehicleIds { get; set; } = new();
}

public class UpdateSubscriptionRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? AmountPaid { get; set; }
    public bool? IsActive { get; set; }
}

public class AddVehicleToSubscriptionRequest
{
    public int VehicleId { get; set; }
}