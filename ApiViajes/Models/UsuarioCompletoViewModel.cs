using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiViajes.Models
{

    public class UsuarioCompletoViewModel
    {

        public int IdUsuario { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public int Edad { get; set; }

        public string Nacionalidad { get; set; }

        public string PreferenciaViaje { get; set; } 

        public string Imagen { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string CorreoLogin { get; set; }

        public string Clave { get; set; }

        public string ConfirmarClave { get; set; }

        public string AvatarUrl { get; set; }
    }
}