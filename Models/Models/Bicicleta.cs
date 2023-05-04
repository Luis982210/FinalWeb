using System;
using System.Collections.Generic;

namespace Models.Models;

public partial class Bicicleta
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public decimal Tamano { get; set; }

    public int Cantidadplatos { get; set; }

    public int Cantidadpinones { get; set; }

    public byte[]? Imagen { get; set; }
}
