namespace CrudPark_Back.Models.DTOs.Requests;

public class CreateCustomerRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? IdentificationNumber { get; set; }
    public List<int>? VehicleIds { get; set; } // IDs de vehículos existentes
}

public class UpdateCustomerRequest
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? IdentificationNumber { get; set; }
}

public class AddVehicleToCustomerRequest
{
    public int VehicleId { get; set; }
    public bool IsPrimary { get; set; }
}