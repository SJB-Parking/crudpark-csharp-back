using CrudPark_Back.Models.Enums;

namespace CrudPark_Back.Models.DTOs.Requests;

public class CreateVehicleRequest
{
    public string LicensePlate { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
}

public class UpdateVehicleRequest
{
    public VehicleType? VehicleType { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
}