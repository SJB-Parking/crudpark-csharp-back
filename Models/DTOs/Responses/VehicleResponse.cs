namespace CrudPark_Back.Models.DTOs.Responses;

public class VehicleResponse
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public bool IsPrimary { get; set; }
}