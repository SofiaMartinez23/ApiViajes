using ApiViajes.Data;
using ApiViajes.Models;
using Microsoft.EntityFrameworkCore;
using ViajesMvcNetCore.Models;

namespace ApiViajes.Repositories
{
    public class RepositoryViaje
    {
        private ViajesContext context;

        public RepositoryViaje(ViajesContext context)
        {
            this.context = context;
        }

        // Obtener todos los usuarios
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await this.context.Usuarios.ToListAsync();
        }

        // Login de un usuario
        public async Task<Usuario> LogInUsuarioAsync(string correo, string clave)
        {
            return await this.context.Usuarios
                .Where(x => x.Correo == correo && x.Clave == clave)
                .FirstOrDefaultAsync();
        }

        // Obtener usuario por ID
        public async Task<Usuario> FindUsuarioAsync(int idUsuario)
        {
            return await this.context.Usuarios
                .FirstOrDefaultAsync(z => z.IdUsuario == idUsuario);
        }

        // Obtener perfil completo del usuario
        public async Task<UsuarioCompletoView> GetUsuarioPerfilAsync(int idUsuario)
        {
            return await this.context.UsuarioCompletoViews
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
        }

        // Actualizar perfil del usuario
        public async Task<bool> UpdateUsuarioAsync(Usuario usuario)
        {
            var existingUser = await this.context.Usuarios.FindAsync(usuario.IdUsuario);
            if (existingUser != null)
            {
                existingUser.Nombre = usuario.Nombre;
                existingUser.Correo = usuario.Correo;
                existingUser.PreferenciaViaje = usuario.PreferenciaViaje;
                existingUser.AvatarUrl = usuario.AvatarUrl;

                await this.context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Obtener seguidores de un usuario
        public async Task<List<UsuarioSeguidoPerfil>> GetSeguidoresAsync(int usuarioId)
        {
            return await this.context.UsuarioSeguidoPerfiles
                .Where(s => s.IdUsuarioSeguidor == usuarioId)
                .ToListAsync();
        }

        // Obtener usuarios seguidos por un usuario
        public async Task<List<UsuarioSeguidoPerfil>> GetSeguidosAsync(int usuarioId)
        {
            return await this.context.UsuarioSeguidoPerfiles
                .Where(s => s.IdUsuarioSeguido == usuarioId)
                .ToListAsync();
        }

        // Seguir a otro usuario
        public async Task<bool> FollowUserAsync(int usuarioSeguidorId, int usuarioSeguidoId)
        {
            // Verificar que no se puede seguir a uno mismo
            if (usuarioSeguidorId == usuarioSeguidoId)
            {
                return false;  // No se puede seguir a sí mismo
            }

            // Verificar si ya existe una relación de seguimiento
            var existingFollow = await this.context.Seguidores
                .FirstOrDefaultAsync(s => s.IdUsuarioSeguidor == usuarioSeguidorId && s.IdUsuarioSeguido == usuarioSeguidoId);

            if (existingFollow != null)
            {
                return false;  // El usuario ya está siguiendo a este otro usuario
            }

            // Crear el objeto de seguimiento
            var follow = new Seguidor
            {
                IdUsuarioSeguidor = usuarioSeguidorId,
                IdUsuarioSeguido = usuarioSeguidoId,
                FechaSeguimiento = DateTime.UtcNow
            };

            // Agregar el objeto 'follow' a la tabla correcta 'Seguidores'
            this.context.Seguidores.Add(follow);

            // Guardar cambios en la base de datos
            await this.context.SaveChangesAsync();

            return true;  // El seguimiento fue exitoso
        }


        // Dejar de seguir a un usuario
        public async Task<bool> UnfollowUserAsync(int usuarioSeguidorId, int usuarioSeguidoId)
        {
            var follow = await this.context.UsuarioSeguidoPerfiles
                .FirstOrDefaultAsync(s => s.IdUsuarioSeguidor == usuarioSeguidorId && s.IdUsuarioSeguido == usuarioSeguidoId);
            if (follow != null)
            {
                this.context.UsuarioSeguidoPerfiles.Remove(follow);
                await this.context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Obtener chat entre dos usuarios
        public async Task<List<Chat>> GetChatAsync(int usuarioId1, int usuarioId2)
        {
            return await this.context.Chats
                .Where(c => (c.IdUsuarioRemitente == usuarioId1 && c.IdUsuarioDestinatario == usuarioId2) ||
                           (c.IdUsuarioRemitente == usuarioId2 && c.IdUsuarioDestinatario == usuarioId1))
                .OrderBy(c => c.FechaEnvio)
                .ToListAsync();
        }

        // Enviar mensaje
        public async Task<bool> SendMessageAsync(int usuarioRemitenteId, int usuarioDestinatarioId, string mensaje)
        {
            var chat = new Chat
            {
                IdUsuarioRemitente = usuarioRemitenteId,
                IdUsuarioDestinatario = usuarioDestinatarioId,
                Mensaje = mensaje,
                FechaEnvio = DateTime.UtcNow
            };
            this.context.Chats.Add(chat);
            await this.context.SaveChangesAsync();
            return true;
        }
    }
}
