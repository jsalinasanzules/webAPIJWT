using System;
using System.Collections.Generic;

namespace webAPI.Models;

public partial class Lote
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    public string Codigo { get; set; } = null!;

    public DateOnly? FechaCreacion { get; set; }

    public DateOnly? FechaExpiracion { get; set; }

    public int Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public string? Estado { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
