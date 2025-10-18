using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudPark_Back.Controllers;

[ApiController]
[Route("api/vehiculos")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    // GET: api/Vehicles
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(new
            {
                success = true,
                message = "Vehículos obtenidos exitosamente",
                data = vehicles
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener los vehículos",
                error = ex.Message
            });
        }
    }

    // GET: api/Vehicles/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            
            if (vehicle == null)
                return NotFound(new
                {
                    success = false,
                    message = "Vehículo no encontrado"
                });

            return Ok(new
            {
                success = true,
                message = "Vehículo obtenido exitosamente",
                data = vehicle
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener el vehículo",
                error = ex.Message
            });
        }
    }

    // GET: api/Vehicles/by-plate/ABC123
    [HttpGet("by-plate/{licensePlate}")]
    public async Task<IActionResult> GetByLicensePlate(string licensePlate)
    {
        try
        {
            var vehicle = await _vehicleService.GetVehicleByLicensePlateAsync(licensePlate);
            
            if (vehicle == null)
                return NotFound(new
                {
                    success = false,
                    message = "Vehículo no encontrado"
                });

            return Ok(new
            {
                success = true,
                message = "Vehículo obtenido exitosamente",
                data = vehicle
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener el vehículo",
                error = ex.Message
            });
        }
    }

    // POST: api/Vehicles
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
    {
        try
        {
            var vehicle = await _vehicleService.CreateVehicleAsync(request);
            
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, new
            {
                success = true,
                message = "Vehículo creado exitosamente",
                data = vehicle
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
                message = "Error al crear el vehículo",
                error = ex.Message
            });
        }
    }

    // PUT: api/Vehicles/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVehicleRequest request)
    {
        try
        {
            var vehicle = await _vehicleService.UpdateVehicleAsync(id, request);
            
            return Ok(new
            {
                success = true,
                message = "Vehículo actualizado exitosamente",
                data = vehicle
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
                message = "Error al actualizar el vehículo",
                error = ex.Message
            });
        }
    }

    // DELETE: api/Vehicles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _vehicleService.DeleteVehicleAsync(id);
            
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = "Vehículo no encontrado"
                });

            return Ok(new
            {
                success = true,
                message = "Vehículo eliminado exitosamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al eliminar el vehículo",
                error = ex.Message
            });
        }
    }
}