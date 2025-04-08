using ApiViajes.Models;
using Microsoft.EntityFrameworkCore;
using ViajesMvcNetCore.Models;

namespace ApiViajes.Data
{
    public class ViajesContext: DbContext
    {
        public ViajesContext(DbContextOptions<ViajesContext>
            options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<LugarFavorito> LugaresFavoritos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UsuarioSeguidoPerfil> UsuarioSeguidoPerfiles { get; set; }
        public DbSet<Seguidor> Seguidores { get; set; }

        public DbSet<UsuarioCompletoView> UsuarioCompletoViews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.UsuarioRemitente)
                .WithMany() // Adjust this based on your Usuario model
                .HasForeignKey(c => c.IdUsuarioRemitente)
                .OnDelete(DeleteBehavior.Restrict); // Or your desired delete behavior

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.UsuarioDestinatario)
                .WithMany() // Adjust this based on your Usuario model
                .HasForeignKey(c => c.IdUsuarioDestinatario)
                .OnDelete(DeleteBehavior.Restrict); // Or your desired delete behavior
        }


    }
}
