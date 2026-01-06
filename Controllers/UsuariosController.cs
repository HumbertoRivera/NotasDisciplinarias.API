using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotasDisciplinarias.API.Data;
using NotasDisciplinarias.API.DTOs;
using NotasDisciplinarias.API.Models;
using NotasDisciplinarias.API.Models.DTOs;

namespace NotasDisciplinarias.API.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly NotasDbContext _context;

        public UsuariosController(NotasDbContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            // Solo datos de seguridad (tabla Usuarios)
            var usuarios = await _context.Usuarios
                .Select(u => new
                {
                    u.Id,
                    u.Usuario,
                    u.Rol,
                    u.Activo
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuario no encontrado.");

            return Ok(new
            {
                usuario.Id,
                usuario.Usuario,
                usuario.Rol,
                usuario.Activo
            });
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Si UsuarioCreateDto trae Contrasena y Rol, perfecto.
            // Si no, ahorita lo ajustamos con el nombre real.
            var entity = new Usuarios
            {
                Usuario = dto.Usuario,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                Rol = dto.Rol,
                Activo = true
            };

            _context.Usuarios.Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = entity.Id }, new
            {
                entity.Id,
                entity.Usuario,
                entity.Rol,
                entity.Activo
            });
        }

        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] UsuarioUpdateDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuario no encontrado.");

            usuario.Rol = dto.Rol;

            // Opcional: si tu update DTO trae Activo
            // usuario.Activo = dto.Activo;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound("Usuario no encontrado.");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
