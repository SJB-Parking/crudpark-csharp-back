using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudPark_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    // GET: api/Subscriptions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var subscriptions = await _subscriptionService.GetAllSubscriptionsAsync();
            return Ok(new
            {
                success = true,
                message = "Mensualidades obtenidas exitosamente",
                data = subscriptions
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener las mensualidades",
                error = ex.Message
            });
        }
    }

    // GET: api/Subscriptions/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id);
            
            if (subscription == null)
                return NotFound(new
                {
                    success = false,
                    message = "Mensualidad no encontrada"
                });

            return Ok(new
            {
                success = true,
                message = "Mensualidad obtenida exitosamente",
                data = subscription
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener la mensualidad",
                error = ex.Message
            });
        }
    }

    // GET: api/Subscriptions/active
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            var subscriptions = await _subscriptionService.GetActiveSubscriptionsAsync();
            return Ok(new
            {
                success = true,
                message = "Mensualidades activas obtenidas exitosamente",
                data = subscriptions
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener las mensualidades activas",
                error = ex.Message
            });
        }
    }

    // GET: api/Subscriptions/expiring-soon?days=3
    [HttpGet("expiring-soon")]
    public async Task<IActionResult> GetExpiringSoon([FromQuery] int days = 3)
    {
        try
        {
            var subscriptions = await _subscriptionService.GetExpiringSoonAsync(days);
            return Ok(new
            {
                success = true,
                message = $"Mensualidades próximas a vencer (en {days} días)",
                data = subscriptions
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener las mensualidades próximas a vencer",
                error = ex.Message
            });
        }
    }

    // GET: api/Subscriptions/expired
    [HttpGet("expired")]
    public async Task<IActionResult> GetExpired()
    {
        try
        {
            var subscriptions = await _subscriptionService.GetExpiredSubscriptionsAsync();
            return Ok(new
            {
                success = true,
                message = "Mensualidades vencidas obtenidas exitosamente",
                data = subscriptions
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener las mensualidades vencidas",
                error = ex.Message
            });
        }
    }

    // POST: api/Subscriptions
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubscriptionRequest request)
    {
        try
        {
            var subscription = await _subscriptionService.CreateSubscriptionAsync(request);
            
            return CreatedAtAction(nameof(GetById), new { id = subscription.Id }, new
            {
                success = true,
                message = "Mensualidad creada exitosamente. Se ha enviado un correo de confirmación.",
                data = subscription
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
                message = "Error al crear la mensualidad",
                error = ex.Message
            });
        }
    }

    // PUT: api/Subscriptions/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSubscriptionRequest request)
    {
        try
        {
            var subscription = await _subscriptionService.UpdateSubscriptionAsync(id, request);
            
            return Ok(new
            {
                success = true,
                message = "Mensualidad actualizada exitosamente",
                data = subscription
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
                message = "Error al actualizar la mensualidad",
                error = ex.Message
            });
        }
    }

    // DELETE: api/Subscriptions/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _subscriptionService.DeleteSubscriptionAsync(id);
            
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = "Mensualidad no encontrada"
                });

            return Ok(new
            {
                success = true,
                message = "Mensualidad eliminada exitosamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al eliminar la mensualidad",
                error = ex.Message
            });
        }
    }

    // POST: api/Subscriptions/5/vehicles
    [HttpPost("{subscriptionId}/vehicles")]
    public async Task<IActionResult> AddVehicle(int subscriptionId, [FromBody] AddVehicleToSubscriptionRequest request)
    {
        try
        {
            var result = await _subscriptionService.AddVehicleToSubscriptionAsync(subscriptionId, request);
            
            if (!result)
                return BadRequest(new
                {
                    success = false,
                    message = "No se pudo agregar el vehículo (límite alcanzado o vehículo ya existe)"
                });

            return Ok(new
            {
                success = true,
                message = "Vehículo agregado a la mensualidad exitosamente"
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
                message = "Error al agregar vehículo",
                error = ex.Message
            });
        }
    }

    // DELETE: api/Subscriptions/5/vehicles/2
    [HttpDelete("{subscriptionId}/vehicles/{vehicleId}")]
    public async Task<IActionResult> RemoveVehicle(int subscriptionId, int vehicleId)
    {
        try
        {
            var result = await _subscriptionService.RemoveVehicleFromSubscriptionAsync(subscriptionId, vehicleId);
            
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = "Vehículo no encontrado en esta mensualidad"
                });

            return Ok(new
            {
                success = true,
                message = "Vehículo removido de la mensualidad exitosamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al remover vehículo",
                error = ex.Message
            });
        }
    }

    // POST: api/Subscriptions/check-and-notify
    [HttpPost("check-and-notify")]
    public async Task<IActionResult> CheckAndNotifyExpiring()
    {
        try
        {
            await _subscriptionService.CheckAndNotifyExpiringSubscriptionsAsync();
            
            return Ok(new
            {
                success = true,
                message = "Notificaciones enviadas exitosamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al enviar notificaciones",
                error = ex.Message
            });
        }
    }
}