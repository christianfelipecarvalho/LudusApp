using System;
using System.ComponentModel.DataAnnotations;

namespace LudusApp.Application.DTOs
{
    public class TemaReadDto
    {

        [Required]
        public string BorderRadius { get; set; }

        [Required]
        public bool DarkMode { get; set; }

        [Required]
        public string PrimaryColor { get; set; }

        [Required]
        public string SecondaryColor { get; set; }

        [Required]
        public string UsuarioId { get; set; }
    }
}