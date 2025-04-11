using System;
using System.Collections.Generic;

namespace webAPI.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Lote> Lotes { get; set; } = new List<Lote>();
}
