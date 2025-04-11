using ApiViajes.Data;
using Microsoft.EntityFrameworkCore;
using NugetViajesSMG.Models;

namespace ApiViajes.Repositories
{
    public class RepositorySeguidos
    {

        private ViajesContext context;

        public RepositorySeguidos(ViajesContext context)
        {
            this.context = context;
        }

        public async Task<List<UsuarioSeguidoPerfil>> GetSeguidoresAsync(int usuarioId)
        {
            return await this.context.UsuarioSeguidoPerfiles
                .Where(s => s.IdUsuarioSeguidor == usuarioId).ToListAsync();
        }

        public async Task<List<UsuarioSeguidoPerfil>> GetSeguidosAsync(int usuarioId)
        {
            return await this.context.UsuarioSeguidoPerfiles
                .Where(s => s.IdUsuarioSeguido == usuarioId).ToListAsync();
        }


        public async Task<bool> FollowUserAsync(int usuarioSeguidorId, int usuarioSeguidoId)
        {
            if (usuarioSeguidorId == usuarioSeguidoId)
            {
                return false;
            }

            var existingFollow = await this.context.Seguidores
                .FirstOrDefaultAsync(s => s.IdUsuarioSeguidor == usuarioSeguidorId && s.IdUsuarioSeguido == usuarioSeguidoId);

            if (existingFollow != null)
            {
                return false;
            }

            var maxId = await this.context.Seguidores.MaxAsync(s => (int?)s.IdSeguidor) ?? 0;

            var follow = new Seguidor
            {
                IdSeguidor = maxId + 1,
                IdUsuarioSeguidor = usuarioSeguidorId,
                IdUsuarioSeguido = usuarioSeguidoId,
                FechaSeguimiento = DateTime.UtcNow
            };

            this.context.Seguidores.Add(follow);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnfollowUserAsync(int usuarioSeguidorId, int usuarioSeguidoId)
        {
            var follow = await this.context.Seguidores
                .FirstOrDefaultAsync(s => s.IdUsuarioSeguidor == usuarioSeguidorId &&
                                          s.IdUsuarioSeguido == usuarioSeguidoId);

            if (follow != null)
            {
                this.context.Seguidores.Remove(follow);
                await this.context.SaveChangesAsync();
                return true;
            }

            return false;
        }

    }
}
