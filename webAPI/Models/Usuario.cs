﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace webAPI.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Username { get; set; }

    [JsonIgnore]
    public string? Password { get; set; }

    public DateTime? FechaCreacion { get; set; }
}
