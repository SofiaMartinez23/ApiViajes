using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiViajes.Models
{

    public class UsuarioModel
    {

        public int IdUsuario { get; set; }

        public string Correo { get; set; }

        public string Nombre { get; set; }

        public string Clave { get; set; }

        public string ConfirmarClave { get; set; }

        public string PreferenciaViaje { get; set; }

        public string AvatarUrl { get; set; } = "";
    }
}