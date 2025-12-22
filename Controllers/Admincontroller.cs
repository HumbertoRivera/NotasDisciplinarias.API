using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotasDisciplinarias.API.Data;
using NotasDisciplinarias.API.DTOs;

namespace NotasDisciplinarias.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly NotasDbContext _context;

        public AdminController(NotasDbContext context)
        {
            _context = context;
        }

 // GET: api/admin/casos-activos
        [HttpGet("casos-activos")]
        public async Task<IActionResult> GetCasosActivos()
        {
            // 🔐 Plaza del jefe desde el token
            var plazaJefe = User.FindFirst("Plaza")?.Value;

            if (string.IsNullOrEmpty(plazaJefe))
                return Unauthorized("No se pudo determinar la plaza del usuario");

            var casos = await (
                from c in _context.Casos
                join u in _context.Usuarios on c.IdUsuario equals u.Id
                join cat in _context.Categorias on c.IdCategoria equals cat.Id_Categoria
                where u.Plaza_jefe == plazaJefe
                      && c.Estatus == 1
                select new CasoAdminResponseDto
                {
                    IdCaso = c.IdCaso,
                    IdUsuario = u.Id,
                    Empleado = u.Usuario,          // consistente
                    Categoria = cat.Nombre,
                    Descripcion = c.Descripcion,
                    FechaRegistro = c.FechaRegistro,
                    Estatus = c.Estatus.ToString()
                }
            ).ToListAsync();

            return Ok(casos);
        }
    }
}
