using ApiViajes.Models;
using Microsoft.EntityFrameworkCore;
using ViajesMvcNetCore.Models;

namespace ApiViajes.Data
{
    public class ViajesContext: DbContext
    {
        public ViajesContext(DbContextOptions<ViajesContext>
            options) : base(options) { }

        public DbSet<UsuarioCompletoView> Usuarios { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<LugarFavorito> LugaresFavoritos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UsuarioSeguidoPerfil> UsuarioSeguidoPerfiles { get; set; }
    }
}
