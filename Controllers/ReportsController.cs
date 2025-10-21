using CrudPark_Back.Data;
using CrudPark_Back.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Controllers;

[ApiController]
[Route("api/reportes")]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Reports/income?period=day|week|month
    [HttpGet("ingresos")]
    public async Task<IActionResult> GetIncomeReport([FromQuery] string period = "day")
    {
        try
        {
            var now = DateTime.UtcNow;
            DateTime startDate;

            switch (period.ToLower())
            {
                case "week":
                    startDate = now.AddDays(-7);
                    break;
                case "month":
                    startDate = now.AddMonths(-1);
                    break;
                default: // day
                    startDate = now.Date;
                    break;
            }

            var payments = await _context.Payments
                .Where(p => p.PaymentDatetime >= startDate)
                .GroupBy(p => p.PaymentDatetime.Date)
                .Select(g => new
                {
                    date = g.Key,
                    totalIncome = g.Sum(p => p.Amount),
                    transactionCount = g.Count()
                })
                .OrderBy(x => x.date)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = $"Reporte de ingresos ({period}) obtenido exitosamente",
                period,
                startDate,
                endDate = now,
                data = payments
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener el reporte de ingresos",
                error = ex.Message
            });
        }
    }

    // GET: api/Reports/occupancy
    [HttpGet("ocupacion")]
    public async Task<IActionResult> GetOccupancyReport([FromQuery] int days = 7)
    {
        try
        {
            var startDate = DateTime.UtcNow.AddDays(-days).Date;

            var occupancyData = await _context.Tickets
                .Where(t => t.EntryDatetime >= startDate)
                .GroupBy(t => t.EntryDatetime.Date)
                .Select(g => new
                {
                    date = g.Key,
                    totalEntries = g.Count(),
                    guestEntries = g.Count(t => t.TicketType == TicketType.Guest),
                    subscriberEntries = g.Count(t => t.TicketType == TicketType.MonthlySubscriber),
                    averageDurationMinutes = g.Where(t => t.ParkingDurationMinutes.HasValue)
                                               .Average(t => (double?)t.ParkingDurationMinutes) ?? 0
                })
                .OrderBy(x => x.date)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Reporte de ocupación obtenido exitosamente",
                days,
                data = occupancyData
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener el reporte de ocupación",
                error = ex.Message
            });
        }
    }

    // GET: api/Reports/subscribers-vs-guests
    [HttpGet("subscribers-vs-guests")]
    public async Task<IActionResult> GetSubscribersVsGuests([FromQuery] int days = 30)
    {
        try
        {
            var startDate = DateTime.UtcNow.AddDays(-days).Date;

            var totalGuests = await _context.Tickets
                .CountAsync(t => t.EntryDatetime >= startDate && t.TicketType == TicketType.Guest);

            var totalSubscribers = await _context.Tickets
                .CountAsync(t => t.EntryDatetime >= startDate && t.TicketType == TicketType.MonthlySubscriber);

            var guestRevenue = await _context.Payments
                .Where(p => p.PaymentDatetime >= startDate)
                .SumAsync(p => (decimal?)p.Amount) ?? 0;

            var subscriberRevenue = await _context.MonthlySubscriptions
                .Where(s => s.CreatedAt >= startDate)
                .SumAsync(s => (decimal?)s.AmountPaid) ?? 0;

            return Ok(new
            {
                success = true,
                message = "Comparativa obtenida exitosamente",
                period = $"{days} días",
                data = new
                {
                    guests = new
                    {
                        count = totalGuests,
                        revenue = guestRevenue,
                        percentage = totalGuests + totalSubscribers > 0 
                            ? (totalGuests * 100.0 / (totalGuests + totalSubscribers)) 
                            : 0
                    },
                    subscribers = new
                    {
                        count = totalSubscribers,
                        revenue = subscriberRevenue,
                        percentage = totalGuests + totalSubscribers > 0 
                            ? (totalSubscribers * 100.0 / (totalGuests + totalSubscribers)) 
                            : 0
                    },
                    total = new
                    {
                        entries = totalGuests + totalSubscribers,
                        revenue = guestRevenue + subscriberRevenue
                    }
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener la comparativa",
                error = ex.Message
            });
        }
    }

    // GET: api/Reports/top-vehicles
    [HttpGet("top-vehicles")]
    public async Task<IActionResult> GetTopVehicles([FromQuery] int limit = 10)
    {
        try
        {
            var topVehicles = await _context.Tickets
                .Include(t => t.Vehicle)
                .GroupBy(t => t.Vehicle)
                .Select(g => new
                {
                    vehicle = new
                    {
                        g.Key.LicensePlate,
                        g.Key.Brand,
                        g.Key.Model,
                        VehicleType = g.Key.VehicleType.ToString()
                    },
                    totalVisits = g.Count(),
                    totalTimeMinutes = g.Where(t => t.ParkingDurationMinutes.HasValue)
                                        .Sum(t => t.ParkingDurationMinutes) ?? 0
                })
                .OrderByDescending(x => x.totalVisits)
                .Take(limit)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Top vehículos obtenidos exitosamente",
                data = topVehicles
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener los top vehículos",
                error = ex.Message
            });
        }
    }

    // GET: api/Reports/operator-performance
    [HttpGet("operator-performance")]
    public async Task<IActionResult> GetOperatorPerformance([FromQuery] int days = 7)
    {
        try
        {
            var startDate = DateTime.UtcNow.AddDays(-days).Date;

            var operatorStats = await _context.Operators
                .Select(o => new
                {
                    operatorId = o.Id,
                    operatorName = o.FullName,
                    ticketsProcessed = o.Tickets.Count(t => t.EntryDatetime >= startDate),
                    paymentsCollected = o.Payments.Count(p => p.PaymentDatetime >= startDate),
                    totalCollected = o.Payments
                        .Where(p => p.PaymentDatetime >= startDate)
                        .Sum(p => (decimal?)p.Amount) ?? 0
                })
                .Where(x => x.ticketsProcessed > 0 || x.paymentsCollected > 0)
                .OrderByDescending(x => x.ticketsProcessed)
                .ToListAsync();

            return Ok(new
            {
                success = true,
                message = "Rendimiento de operadores obtenido exitosamente",
                period = $"{days} días",
                data = operatorStats
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener el rendimiento de operadores",
                error = ex.Message
            });
        }
    }
}