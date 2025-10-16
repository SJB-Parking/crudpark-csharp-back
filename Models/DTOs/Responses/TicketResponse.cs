namespace CrudPark_Back.Models.DTOs.Responses;

public class TicketResponse
{
    public int Id { get; set; }
    public string Folio { get; set; } = string.Empty;
    public VehicleResponse Vehicle { get; set; } = null!;
    public DateTime EntryDatetime { get; set; }
    public DateTime? ExitDatetime { get; set; }
    public string TicketType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? ParkingDurationMinutes { get; set; }
}