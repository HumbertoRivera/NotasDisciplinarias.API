public class Usuarios
{
    public int Id { get; set; }
     public required string Usuario { get; set; }
    public required string PasswordHash { get; set; }

    public required string Rol { get; set; }
    public required string Region { get; set; }
    public required string Plaza { get; set; }

    public required string Plaza_jefe  { get; set; }

    public bool Activo { get; set; }
}
