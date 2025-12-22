using NotasDisciplinarias.API.Models.DTOs;

public class LoginResponseDto
{
  public required string Token { get; set; }
    public required UsuarioResponseDto Usuario { get; set; }
}
