using NotasDisciplinarias.API.Data;




public class UsuarioService : IUsuarioService
{
    private readonly NotasDbContext _context;

    public UsuarioService(NotasDbContext context)
    {
        _context = context;
    }

    public Usuarios? ValidarUsuario(string usuario, string password)
    {
        var user = _context.Usuarios
            .FirstOrDefault(u => u.Usuario == usuario && u.Activo);

        if (user == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }
}
