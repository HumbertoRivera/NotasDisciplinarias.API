using Microsoft.EntityFrameworkCore;
using NotasDisciplinarias.API.Models;

namespace NotasDisciplinarias.API.Data
{
    public class NotasDbContext : DbContext
{
    public NotasDbContext(DbContextOptions<NotasDbContext> options)
        : base(options)
    {
    }

    public DbSet<Casos> Casos { get; set; }
    public DbSet<Usuarios> Usuarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Accion> Acciones { get; set; }
    public DbSet<Evidencias> Evidencias { get; set; }
}

}
