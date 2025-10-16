namespace CrudPark_Back.Models.DTOs.Responses;

public class CustomerResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? IdentificationNumber { get; set; }
    public List<VehicleResponse> Vehicles { get; set; } = new();
}