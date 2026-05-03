using GestionProducto.Application.Interfaces;
using GestionProducto.DTOs;
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

        public ProductoController(IProductoService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> ObtenerTodos()
        {
            var result = await _service.ObtenerTodos();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProdutoDto>> ObtenerPorId(int id)
        {
            var result = await _service.ObtenerPorId(id);

            if (result == null)
                return NotFound(new { mensaje = "Producto no encontrado" });

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

        [HttpGet("{codigo}")]
        public async Task<ActionResult> ObtenerPorCodigo(string codigo)
        {
            var result = await _service.ObtenerPorCodigo(codigo);

            if (result == null)
                return NotFound(new { mensaje = "Producto no encontrado" });

            return Ok(result);
        }

        [HttpGet("stock/{id:int}")]
        public async Task<ActionResult<int>> ObtenerStock(int id)
        {
            var stock = await _service.ObtenerStock(id);
            return Ok(stock);
        }
    }
}
