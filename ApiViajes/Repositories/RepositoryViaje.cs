using ApiViajes.Data;
using ApiViajes.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiViajes.Repositories
{
    public class RepositoryViaje
    {
        private ViajesContext context;

        public RepositoryViaje(ViajesContext context)
        {
            this.context = context;
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await this.context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> LogInUsuarioAsync(string nombre, string clave)
        {
            return await this.context.Usuarios
                .Where(x => x.Nombre == nombre && x.Clave == clave)
                .FirstOrDefaultAsync();
        }


        public async Task<Usuario> FindUsuarioAsync(int idUsuario)
        {
            return await this.context.Usuarios
                .FirstOrDefaultAsync(z => z.IdUsuario == idUsuario);
        }
    }
}
