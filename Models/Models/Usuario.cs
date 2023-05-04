using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Correo { get; set; } = null!;

    public string Contraseña { get; set; } = null!;
}
