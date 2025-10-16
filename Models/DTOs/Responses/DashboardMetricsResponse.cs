namespace CrudPark_Back.Models.DTOs.Responses;

public class DashboardMetricsResponse
{
    public int VehiclesInside { get; set; }
    public decimal TodayIncome { get; set; }
    public int ActiveSubscriptions { get; set; }
    public int ExpiringSoon { get; set; }
    public int ExpiredSubscriptions { get; set; }
    public int TodayEntries { get; set; }
    public int TodayExits { get; set; }
}