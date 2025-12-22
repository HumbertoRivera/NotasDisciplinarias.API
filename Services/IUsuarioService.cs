using NotasDisciplinarias.API.Models;

public interface IUsuarioService
{
    Usuarios? ValidarUsuario(string usuario, string password);
}
