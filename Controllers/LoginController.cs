                                                                                                                                                                                                                                                                                                                                                                                                                                                                    using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NotasDisciplinarias.API.Models.DTOs;

namespace NotasDisciplinarias.API.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;

        public LoginController(
            IConfiguration configuration,
            IUsuarioService usuarioService)
        {
            _configuration = configuration;
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var usuario = _usuarioService.ValidarUsuario(
                request.Usuario,
                request.Password
            );

            if (usuario == null)
                return Unauthorized("Usuario o contraseña incorrectos");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Usuario),
                new Claim(ClaimTypes.Role, usuario.Rol),

                new Claim("Region", usuario.Region),
                new Claim("Plaza", usuario.Plaza)
            };

            var token = GenerarToken(claims);

            var response = new LoginResponseDto
            {
                Token = token,
               Usuario = new UsuarioResponseDto
                {
                    id_usuario = usuario.Id,
                    Rol = usuario.Rol,
                    Nombre_Completo = "ivan careaga panduro", // o Nombre + Apellido si lo tienes
                    Correo = usuario.Usuario + "@megacable.com.mx" //  campo real
                }

            };

            return Ok(response);
        }

        private string GenerarToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"] 
                    ?? throw new InvalidOperationException("JWT Key no configurada")
                )

            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
