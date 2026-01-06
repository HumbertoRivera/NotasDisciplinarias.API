using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotasDisciplinarias.API.Data;
using NotasDisciplinarias.API.DTOs;
using NotasDisciplinarias.API.Models;

[ApiController]
[Route("api/[controller]")]
public class CasosController : ControllerBase
{
    private readonly NotasDbContext _context;

    public CasosController(NotasDbContext context)
    {
        _context = context;
    }

    [HttpGet("usuario/{idUsuario}")]
    public async Task<IActionResult> GetCasosByUsuario(int idUsuario)
    {
        var casos = await (
            from c in _context.Casos
            join cat in _context.Categorias on c.IdCategoria equals cat.Id_Categoria
            where c.IdUsuario == idUsuario
            select new
            {
                Id = c.IdCaso,
                Motivo = cat.Nombre,
                Descripcion = c.Descripcion,
                FechaCreacion = c.FechaRegistro,
                Estado = c.Estatus == 1 ? "En proceso" : c.Estatus == 2 ? "Completado" : "Detenido"
            }
        ).ToListAsync();

        return Ok(casos);
    }

    [HttpPost("crear")]
    public async Task<IActionResult> CrearCaso([FromBody] CasosCreateDto dto)
    {
        var caso = new Casos
        {
            IdUsuario = dto.Id_usuario,
            IdCategoria = dto.Id_categoria,
            Descripcion = dto.Descripcion,
            FechaRegistro = DateTime.Now,
            Estatus = 1
        };

        _context.Casos.Add(caso);
        await _context.SaveChangesAsync();

        return Ok(caso);
    }
}
