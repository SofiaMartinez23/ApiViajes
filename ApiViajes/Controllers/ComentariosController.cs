using ApiViajes.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetViajesSMG.Models;

namespace ApiViajes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private RepositoryComentarios repo;

        public ComentariosController(RepositoryComentarios repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Comentario>>> GetComentarios()
        {
            return await this.repo.GetComentariosAsync();
        }

        [Authorize]
        [HttpGet("{idLugar}")]
        public async Task<ActionResult<Comentario>> FindLugarComentarios(int idLugar)
        {
            return await this.repo.FindComentariosLugarAsync(idLugar);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> InsertComentarios(Comentario coment)
        {
            await this.repo.InsertComentarioAsync(
                coment.IdLugar,coment.IdUsuario,
                coment.Comentarios,coment.NombreUsuario
            );

            return Ok();
        }

        [Authorize]
        [HttpPut]
        [Route("[action]/{idComentario}")]
        public async Task<ActionResult> UpdateComentarios(int idComentario, Comentario coment)
        {
            await this.repo.UpdateComentarioAsync
                (coment.IdComentario, coment.IdLugar, coment.IdUsuario,
                coment.Comentarios, coment.NombreUsuario);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{idLugar}")]
        public async Task<ActionResult> DeleteComentarios(int idLugar)
        {
            await this.repo.DeleteComentarioAsync(idLugar);
            return Ok();
        }
    }
}
