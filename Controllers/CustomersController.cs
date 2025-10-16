using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudPark_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: api/Customers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(new
            {
                success = true,
                message = "Clientes obtenidos exitosamente",
                data = customers
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener los clientes",
                error = ex.Message
            });
        }
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            
            if (customer == null)
                return NotFound(new
                {
                    success = false,
                    message = "Cliente no encontrado"
                });

            return Ok(new
            {
                success = true,
                message = "Cliente obtenido exitosamente",
                data = customer
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener el cliente",
                error = ex.Message
            });
        }
    }

    // POST: api/Customers
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        try
        {
            var customer = await _customerService.CreateCustomerAsync(request);
            
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, new
            {
                success = true,
                message = "Cliente creado exitosamente",
                data = customer
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
                message = "Error al crear el cliente",
                error = ex.Message
            });
        }
    }

    // PUT: api/Customers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerRequest request)
    {
        try
        {
            var customer = await _customerService.UpdateCustomerAsync(id, request);
            
            return Ok(new
            {
                success = true,
                message = "Cliente actualizado exitosamente",
                data = customer
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
                message = "Error al actualizar el cliente",
                error = ex.Message
            });
        }
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = "Cliente no encontrado"
                });

            return Ok(new
            {
                success = true,
                message = "Cliente eliminado exitosamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al eliminar el cliente",
                error = ex.Message
            });
        }
    }

    // POST: api/Customers/5/vehicles
    [HttpPost("{customerId}/vehicles")]
    public async Task<IActionResult> AddVehicle(int customerId, [FromBody] AddVehicleToCustomerRequest request)
    {
        try
        {
            var result = await _customerService.AddVehicleToCustomerAsync(
                customerId, 
                request.VehicleId, 
                request.IsPrimary);
            
            return Ok(new
            {
                success = true,
                message = "Vehículo agregado al cliente exitosamente"
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
}

public class AddVehicleToCustomerRequest
{
    public int VehicleId { get; set; }
    public bool IsPrimary { get; set; }
}