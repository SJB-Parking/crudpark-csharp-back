using CrudPark_Back.Data;
using CrudPark_Back.Models.DTOs.Responses;
using CrudPark_Back.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Dashboard/metrics
    [HttpGet("metrics")]
    public async Task<IActionResult> GetMetrics()
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            // Vehículos actualmente dentro (tickets abiertos)
            var vehiclesInside = await _context.Tickets
                .CountAsync(t => t.Status == TicketStatus.Open);

            // Ingresos del día
            var todayIncome = await _context.Payments
                .Where(p => p.PaymentDatetime >= today && p.PaymentDatetime < tomorrow)
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            // Mensualidades activas
            var activeSubscriptions = await _context.MonthlySubscriptions
                .CountAsync(s => s.IsActive && s.EndDate >= today);

            // Mensualidades próximas a vencer (3 días)
            var thresholdDate = today.AddDays(3);
            var expiringSoon = await _context.MonthlySubscriptions
                .CountAsync(s => s.IsActive && s.EndDate >= today && s.EndDate <= thresholdDate);

            // Mensualidades vencidas
            var expiredSubscriptions = await _context.MonthlySubscriptions
                .CountAsync(s => s.EndDate < today);

            // Entradas del día
            var todayEntries = await _context.Tickets
                .CountAsync(t => t.EntryDatetime >= today && t.EntryDatetime < tomorrow);

            // Salidas del día
            var todayExits = await _context.Tickets
                .CountAsync(t => t.ExitDatetime.HasValue && 
                               t.ExitDatetime.Value >= today && 
                               t.ExitDatetime.Value < tomorrow);

            var metrics = new DashboardMetricsResponse
            {
                VehiclesInside = vehiclesInside,
                TodayIncome = todayIncome,
                ActiveSubscriptions = activeSubscriptions,
                ExpiringSoon = expiringSoon,
                ExpiredSubscriptions = expiredSubscriptions,
                TodayEntries = todayEntries,
                TodayExits = todayExits
            };

            return Ok(new
            {
                success = true,
                message = "Métricas obtenidas exitosamente",
                data = metrics,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener las métricas",
                error = ex.Message
            });
        }
    }

    // GET: api/Dashboard/recent-activity
    [HttpGet("recent-activity")]
    public async Task<IActionResult> GetRecentActivity([FromQuery] int limit = 10)
    {
        try
        {
            var recentTickets = await _context.Tickets
                .Include(t => t.Vehicle)
                .Include(t => t.Operator)
                .OrderByDescending(t => t.CreatedAt)
                .Take(limit)
                .Select(t => new
                {
                    t.Id,
                    t.Folio,
                    Vehicle = new
                    {
                        t.Vehicle.LicensePlate,
                        VehicleType = t.Vehicle.VehicleType.ToString()
                    },
                    Operator = new
                    {
                        t.Operator.FullName
                    },
                    t.EntryDatetime,
                    t.ExitDatetime,
                    Status = t.Status.ToString(),
                    TicketType = t.TicketType.ToString()
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Actividad reciente obtenida exitosamente",
                data = recentTickets
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener la actividad reciente",
                error = ex.Message
            });
        }
    }

    // GET: api/Dashboard/summary
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        try
        {
            var totalCustomers = await _context.Customers.CountAsync(c => c.IsActive);
            var totalVehicles = await _context.Vehicles.CountAsync();
            var totalOperators = await _context.Operators.CountAsync(o => o.IsActive);
            var totalSubscriptions = await _context.MonthlySubscriptions.CountAsync();
            var totalTickets = await _context.Tickets.CountAsync();
            var totalRevenue = await _context.Payments.SumAsync(p => (decimal?)p.Amount) ?? 0;

            return Ok(new
            {
                success = true,
                message = "Resumen general obtenido exitosamente",
                data = new
                {
                    totalCustomers,
                    totalVehicles,
                    totalOperators,
                    totalSubscriptions,
                    totalTickets,
                    totalRevenue
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener el resumen",
                error = ex.Message
            });
        }
    }
}