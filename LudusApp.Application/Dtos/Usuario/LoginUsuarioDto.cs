﻿using System.ComponentModel.DataAnnotations;

namespace LudusApp.Application.Dtos.Usuario;

public class LoginUsuarioDto
{
    [Required]
    public string Identificador { get; set; }
    [Required]
    public string Password { get; set; }
}
