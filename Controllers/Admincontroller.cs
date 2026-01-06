using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotasDisciplinarias.API.Data;
using NotasDisciplinarias.API.DTOs;
using NotasDisciplinarias.API.Models;

[ApiController]
[Route("api/admin")]
//[Authorize(Roles = "RH,JEFE")]  // Comentado para pruebas
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
        // var plazaJefe = User.FindFirst("Plaza")?.Value;

        // if (string.IsNullOrEmpty(plazaJefe))
        //     return Unauthorized("No se pudo determinar la plaza del usuario");

        var casos = await (
            from c in _context.Casos
            join u in _context.UsuariosVista on c.IdUsuario equals u.id
            join cat in _context.Categorias on c.IdCategoria equals cat.Id_Categoria
            where c.Estatus == 1  // && u.plaza_jefe == plazaJefe
            select new CasoAdminResponseDto
            {
                IdCaso = c.IdCaso,
                IdUsuario = u.id,
                Empleado = u.Nombre_Completo,
                Categoria = cat.Nombre,
                Descripcion = c.Descripcion,
                FechaRegistro = c.FechaRegistro,
                Estatus = c.Estatus.ToString()
            }
        ).ToListAsync();

        return Ok(casos);
    }
}
