using CrudPark_Back.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudPark_Back.Controllers;

[ApiController]
[Route("api/prueba")]
public class TestController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TestController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            var operatorsCount = await _context.Operators.CountAsync();
            var vehiclesCount = await _context.Vehicles.CountAsync();
            var customersCount = await _context.Customers.CountAsync();
            var subscriptionsCount = await _context.MonthlySubscriptions.CountAsync();

            return Ok(new
            {
                success = true,
                message = "✅ Conexión exitosa a PostgreSQL",
                database = "crudpark_sjb",
                tables = new
                {
                    operators = operatorsCount,
                    vehicles = vehiclesCount,
                    customers = customersCount,
                    subscriptions = subscriptionsCount
                },
                timestamp = DateTime.UtcNow,
                serverIp = HttpContext.Connection.LocalIpAddress?.ToString(),
                clientIp = HttpContext.Connection.RemoteIpAddress?.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "❌ Error al conectar con la base de datos",
                error = ex.Message,
                innerError = ex.InnerException?.Message
            });
        }
    }

    [HttpGet("operators")]
    public async Task<IActionResult> GetOperators()
    {
        var operators = await _context.Operators
            .Select(o => new
            {
                o.Id,
                o.FullName,
                o.Username,
                o.Email,
                o.IsActive,
                o.CreatedAt
            })
            .ToListAsync();

        return Ok(new
        {
            success = true,
            message = "Lista de operadores",
            count = operators.Count,
            data = operators
        });
    }

    [HttpGet("customers-with-vehicles")]
    public async Task<IActionResult> GetCustomersWithVehicles()
    {
        var customers = await _context.Customers
            .Include(c => c.CustomerVehicles)
                .ThenInclude(cv => cv.Vehicle)
            .Include(c => c.MonthlySubscriptions)
            .Select(c => new
            {
                c.Id,
                c.FullName,
                c.Email,
                c.Phone,
                Vehicles = c.CustomerVehicles.Select(cv => new
                {
                    cv.Vehicle.LicensePlate,
                    cv.Vehicle.VehicleType,
                    cv.Vehicle.Brand,
                    cv.Vehicle.Model,
                    cv.Vehicle.Color,
                    cv.IsPrimary
                }),
                ActiveSubscriptions = c.MonthlySubscriptions
                    .Where(s => s.IsActive && s.EndDate >= DateTime.UtcNow)
                    .Select(s => new
                    {
                        s.SubscriptionCode,
                        s.StartDate,
                        s.EndDate,
                        s.AmountPaid,
                        DaysRemaining = (s.EndDate - DateTime.UtcNow).Days
                    })
            })
            .ToListAsync();

        return Ok(new
        {
            success = true,
            message = "Clientes con vehículos y mensualidades",
            count = customers.Count,
            data = customers
        });
    }

    [HttpGet("database-stats")]
    public async Task<IActionResult> GetDatabaseStats()
    {
        var stats = new
        {
            totalCustomers = await _context.Customers.CountAsync(),
            activeCustomers = await _context.Customers.CountAsync(c => c.IsActive),
            totalVehicles = await _context.Vehicles.CountAsync(),
            totalOperators = await _context.Operators.CountAsync(),
            activeOperators = await _context.Operators.CountAsync(o => o.IsActive),
            totalSubscriptions = await _context.MonthlySubscriptions.CountAsync(),
            activeSubscriptions = await _context.MonthlySubscriptions
                .CountAsync(s => s.IsActive && s.EndDate >= DateTime.UtcNow),
            expiredSubscriptions = await _context.MonthlySubscriptions
                .CountAsync(s => s.EndDate < DateTime.UtcNow),
            totalTickets = await _context.Tickets.CountAsync(),
            openTickets = await _context.Tickets.CountAsync(t => t.Status == Models.Enums.TicketStatus.Open),
            totalPayments = await _context.Payments.CountAsync(),
            totalRevenue = await _context.Payments.SumAsync(p => (decimal?)p.Amount) ?? 0
        };

        return Ok(new
        {
            success = true,
            message = "Estadísticas de la base de datos",
            timestamp = DateTime.UtcNow,
            stats
        });
    }

    // Endpoint para probar desde otro dispositivo
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok(new
        {
            success = true,
            message = "¡Pong! El servidor está funcionando correctamente",
            timestamp = DateTime.UtcNow,
            serverTime = DateTime.Now.ToString("HH:mm:ss"),
            yourIp = HttpContext.Connection.RemoteIpAddress?.ToString()
        });
    }
}
