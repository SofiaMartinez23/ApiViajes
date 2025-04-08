using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiViajes.Models
{

    [Table("USUARIOLOGIN")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("CORREO")]
        public string Correo { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("CLAVE")]
        public string Clave { get; set; }

        [Column("CONFIRMARCLAVE")]
        public string ConfirmarClave { get; set; }

        [Column("PREFERENCIASDEVIAJE")]
        public string PreferenciaViaje { get; set; }

        [Column("AVATARURL")]
        public string AvatarUrl { get; set; } = "";
    }
}

