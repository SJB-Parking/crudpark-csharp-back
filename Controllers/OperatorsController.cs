using CrudPark_Back.Models.DTOs.Requests;
using CrudPark_Back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudPark_Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperatorsController : ControllerBase
{
    private readonly IOperatorService _operatorService;

    public OperatorsController(IOperatorService operatorService)
    {
        _operatorService = operatorService;
    }

    // GET: api/Operators
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var operators = await _operatorService.GetAllOperatorsAsync();
            return Ok(new
            {
                success = true,
                message = "Operadores obtenidos exitosamente",
                data = operators
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener los operadores",
                error = ex.Message
            });
        }
    }

    // GET: api/Operators/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var operatorEntity = await _operatorService.GetOperatorByIdAsync(id);
            
            if (operatorEntity == null)
                return NotFound(new
                {
                    success = false,
                    message = "Operador no encontrado"
                });

            return Ok(new
            {
                success = true,
                message = "Operador obtenido exitosamente",
                data = operatorEntity
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener el operador",
                error = ex.Message
            });
        }
    }

    // GET: api/Operators/active
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            var operators = await _operatorService.GetActiveOperatorsAsync();
            return Ok(new
            {
                success = true,
                message = "Operadores activos obtenidos exitosamente",
                data = operators
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al obtener los operadores activos",
                error = ex.Message
            });
        }
    }

    // POST: api/Operators
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOperatorRequest request)
    {
        try
        {
            var operatorEntity = await _operatorService.CreateOperatorAsync(request);
            
            return CreatedAtAction(nameof(GetById), new { id = operatorEntity.Id }, new
            {
                success = true,
                message = "Operador creado exitosamente",
                data = operatorEntity
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
                message = "Error al crear el operador",
                error = ex.Message
            });
        }
    }

    // PUT: api/Operators/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOperatorRequest request)
    {
        try
        {
            var operatorEntity = await _operatorService.UpdateOperatorAsync(id, request);
            
            return Ok(new
            {
                success = true,
                message = "Operador actualizado exitosamente",
                data = operatorEntity
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
                message = "Error al actualizar el operador",
                error = ex.Message
            });
        }
    }

    // DELETE: api/Operators/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _operatorService.DeleteOperatorAsync(id);
            
            if (!result)
                return NotFound(new
                {
                    success = false,
                    message = "Operador no encontrado"
                });

            return Ok(new
            {
                success = true,
                message = "Operador desactivado exitosamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al desactivar el operador",
                error = ex.Message
            });
        }
    }

    // POST: api/Operators/validate
    [HttpPost("validate")]
    public async Task<IActionResult> ValidateCredentials([FromBody] ValidateOperatorRequest request)
    {
        try
        {
            var operatorEntity = await _operatorService.ValidateOperatorCredentialsAsync(
                request.Username, 
                request.Password);
            
            if (operatorEntity == null)
                return Unauthorized(new
                {
                    success = false,
                    message = "Credenciales inválidas o operador inactivo"
                });

            return Ok(new
            {
                success = true,
                message = "Credenciales válidas",
                data = operatorEntity
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Error al validar credenciales",
                error = ex.Message
            });
        }
    }
}

public class ValidateOperatorRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}