using Microsoft.AspNetCore.Mvc;
using GestionProducto.Application.Interfaces;
using GestionProducto.DTOs.Movimiento;
using GestionProducto.Models;
using GestionProducto.Models.enums;
using GestionProducto.Application.DTOs.Movimiento;

namespace GestionProducto.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientosController : ControllerBase
{
    private readonly IMovimientoService _movimientoService;

    public MovimientosController(IMovimientoService movimientoService)
    {
        _movimientoService = movimientoService;
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Movimiento>> ObtenerPorId(int id)
    {
        var movimiento = await _movimientoService.ObtenerPorId(id);
        return Ok(movimiento);
    }

    [HttpGet("producto/{id:int}")]
    public async Task<ActionResult<IEnumerable<Movimiento>>> ObtenerPorProducto(int id)
    {
        var movimientos = await _movimientoService.ObtenerPorIdProducto(id);
        return Ok(movimientos);
    }

    [HttpGet]
    public async Task<IActionResult> Obtener([FromQuery] MovimientoFiltroDto filtro)
    {
        var result = await _movimientoService.Obtener(filtro);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Agregar([FromBody] MovimientoAgregarDto dto)
    {
        await _movimientoService.Agregar(dto);
        return Ok(new { mensaje = "Movimiento creado correctamente." });
    }
}