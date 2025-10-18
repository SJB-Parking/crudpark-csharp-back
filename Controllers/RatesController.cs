using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudPark_Back.Controllers;

[ApiController]
[Route("api/tarifas")]
public class RatesController : ControllerBase
{
    private readonly IRateService _rateService;

    public RatesController(IRateService rateService)
    {
        _rateService = rateService;
    }

    // GET: api/Rates
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var rates = await _rateService.GetAllRatesAsync();
            return Ok(new
            {
                success = true,
                message = "Tarifas obtenidas exitosamente",
                data = rates
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener las tarifas",
                error = ex.Message
            });
        }
    }

    // GET: api/Rates/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var rate = await _rateService.GetRateByIdAsync(id);
            
            if (rate == null)
                return NotFound(new
                {
                    success = false,
                    message = "Tarifa no encontrada"
                });

            return Ok(new
            {
                success = true,
                message = "Tarifa obtenida exitosamente",
                data = rate
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener la tarifa",
                error = ex.Message
            });
        }
    }

    // GET: api/Rates/active
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            var rate = await _rateService.GetActiveRateAsync();
            
            if (rate == null)
                return NotFound(new
                {
                    success = false,
                    message = "No hay una tarifa activa configurada"
                });

            return Ok(new
            {
                success = true,
                message = "Tarifa activa obtenida exitosamente",
                data = rate
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener la tarifa activa",
                error = ex.Message
            });
        }
    }

    // POST: api/Rates
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRateRequest request)
    {
        try
        {
            var rate = await _rateService.CreateRateAsync(request);
            
            return CreatedAtAction(nameof(GetById), new { id = rate.Id }, new
            {
                success = true,
                message = "Tarifa creada exitosamente",
                data = rate
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al crear la tarifa",
                error = ex.Message
            });
        }
    }

    // PUT: api/Rates/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRateRequest request)
    {
        try
        {
            var rate = await _rateService.UpdateRateAsync(id, request);
            
            return Ok(new
            {
                success = true,
                message = "Tarifa actualizada exitosamente",
                data = rate
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al actualizar la tarifa",
                error = ex.Message
            });
        }
    }

    // DELETE: api/Rates/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _rateService.DeleteRateAsync(id);
            
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = "Tarifa no encontrada"
                });

            return Ok(new
            {
                success = true,
                message = "Tarifa eliminada exitosamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al eliminar la tarifa",
                error = ex.Message
            });
        }
    }

    // POST: api/Rates/calculate-fee
    [HttpPost("calculate-fee")]
    public async Task<IActionResult> CalculateFee([FromBody] CalculateFeeRequest request)
    {
        try
        {
            var fee = await _rateService.CalculateParkingFeeAsync(request.EntryTime, request.ExitTime);
            
            var duration = (request.ExitTime - request.EntryTime).TotalMinutes;
            
            return Ok(new
            {
                success = true,
                message = "Tarifa calculada exitosamente",
                data = new
                {
                    entryTime = request.EntryTime,
                    exitTime = request.ExitTime,
                    durationMinutes = (int)duration,
                    durationFormatted = $"{(int)(duration / 60)}h {(int)(duration % 60)}m",
                    fee = fee,
                    feeFormatted = $"${fee:N0} COP"
                }
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al calcular la tarifa",
                error = ex.Message
            });
        }
    }
}

public class CalculateFeeRequest
{
    public DateTime EntryTime { get; set; }
    public DateTime ExitTime { get; set; }
}