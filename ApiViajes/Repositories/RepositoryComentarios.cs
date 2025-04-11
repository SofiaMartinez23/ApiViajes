using ApiViajes.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NugetViajesSMG.Models;

namespace ApiViajes.Repositories
{
    public class RepositoryComentarios
    {

        private ViajesContext context;

        public RepositoryComentarios(ViajesContext context)
        {
            this.context = context;
        }

        public async Task<List<Comentario>> GetComentariosAsync()
        {
            return await this.context.Comentarios.ToListAsync();
        }

        public async Task<Comentario> FindComentariosLugarAsync(int idLugar)
        {
            return await this.context.Comentarios.FirstOrDefaultAsync(x => x.IdLugar == idLugar);
        }

        public async Task InsertComentarioAsync(int idLugar, int idUsuario, string comentario, string nombreusuario)
        {
            Comentario coment = new Comentario();
            coment.IdLugar = idLugar;
            coment.IdUsuario = idUsuario;
            coment.Comentarios = comentario;
            coment.FechaComentario = DateTime.Now;
            coment.NombreUsuario = nombreusuario;

            await this.context.Comentarios.AddAsync(coment);
            await this.context.SaveChangesAsync();
        }


        public async Task UpdateComentarioAsync(int idComentario, int idLugar,
          int idUsuario, string comentario, string nombreusuario)
        {
            Comentario coment = await this.FindComentariosLugarAsync(idLugar);
            coment.IdComentario = idComentario;
            coment.IdLugar = idLugar;
            coment.IdUsuario = idUsuario;
            coment.Comentarios = comentario;
            coment.FechaComentario = DateTime.Now;
            coment.NombreUsuario = nombreusuario;

            await this.context.SaveChangesAsync();
        }

        public async Task DeleteComentarioAsync(int idLugar)
        {
            Comentario coment = await this.FindComentariosLugarAsync(idLugar);
            this.context.Comentarios.Remove(coment);
            await this.context.SaveChangesAsync();
        }
    }
}
