using GestionProducto.Application.DTOs.Producto;
using GestionProducto.Application.Interfaces;
using GestionProducto.DTOs.Producto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;
        private readonly IMovimientoService _movimientoService;

        public ProductoController(IProductoService service,
        IMovimientoService movimientoService)
        {
            _service = service;
            _movimientoService = movimientoService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDto>> ObtenerPorId(int id)
        {
            var result = await _service.ObtenerPorId(id);

            if (result == null)
                return NotFound(new { mensaje = "Producto no encontrado" });

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Obtener([FromQuery] ProductoFiltroDto filtro)
        {
            var result = await _service.Obtener(filtro);
            return Ok(result);
        }

        [HttpGet("{id:int}/movimientos")]
        public async Task<IActionResult> ObtenerMovimientosPorProducto(int id)
        {
            var result = await _movimientoService.ObtenerPorIdProducto(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Agregar([FromBody] ProductoAgregarDto dto)
        {
            var id = await _service.CrearProducto(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id },
                new
                {
                    mensaje = "Producto creado correctamente",
                    id
                }
            );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ProductoActualizarDto dto)
        {
            dto.Id = id;
            await _service.ActualizarProducto(dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarProducto(id);
            return NoContent();
        }

        [HttpPatch("{id:int}/activar")]
        public async Task<IActionResult> Activar(int id)
        {
            await _service.Activar(id);
            return NoContent();
        }

    }
}
