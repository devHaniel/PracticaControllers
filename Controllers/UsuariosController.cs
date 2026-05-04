using GestionProducto.Application.DTOs.Usuario;
using GestionProducto.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    
[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("registrar")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Registrar(UsuarioCrearDto dto)
    {
        var usuario = await _usuarioService.Crear(dto);
        return Ok(usuario);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var respuesta = await _usuarioService.Login(dto);
        return Ok(respuesta);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ObtenerTodos()
    {
        var usuarios = await _usuarioService.ObtenerTodos();
        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var usuario = await _usuarioService.ObtenerPorId(id);

        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        return Ok(usuario);
    }

    [HttpPatch("{id:int}/estado")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool activo)
    {
        await _usuarioService.CambiarEstado(id, activo);
        return NoContent();
    }
}
}
