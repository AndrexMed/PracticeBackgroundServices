namespace AutomaticProcess.Data.Models;

public partial class Persona
{
    public decimal Id { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public bool EstadoRegistro { get; set; }
}