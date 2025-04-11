using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using NugetViajesSMG.Models;
using Microsoft.AspNetCore.Authorization;
using ApiViajes.Repositories;

namespace ApiViajes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguidoresController : ControllerBase
    {
        private RepositorySeguidos repo;

        public SeguidoresController(RepositorySeguidos repo)
        {
            this.repo = repo;
        }

        [Authorize]
        [HttpGet("Seguidores/{idUsuario}")]
        public async Task<ActionResult<List<UsuarioSeguidoPerfil>>> GetSeguidores(int idUsuario)
        {
            var seguidores = await this.repo.GetSeguidoresAsync(idUsuario);
            return Ok(seguidores);
        }

        [Authorize]
        [HttpGet("Seguidos/{idUsuario}")]
        public async Task<ActionResult<List<UsuarioSeguidoPerfil>>> GetSeguidos(int idUsuario)
        {
            var seguidos = await this.repo.GetSeguidosAsync(idUsuario);
            return Ok(seguidos);
        }

        [Authorize]
        [HttpPost("Seguir")]
        public async Task<ActionResult> FollowUser([FromQuery] int seguidorId, [FromQuery] int seguidoId)
        {
            bool result = await this.repo.FollowUserAsync(seguidorId, seguidoId);
            if (result)
            {
                return Ok(new { mensaje = "Usuario seguido correctamente." });
            }
            else
            {
                return BadRequest(new { mensaje = "No se pudo seguir al usuario." });
            }
        }

        
        [Authorize]
        [HttpDelete("DejarSeguir")]
        public async Task<ActionResult> UnfollowUser([FromQuery] int seguidorId, [FromQuery] int seguidoId)
        {
            bool result = await this.repo.UnfollowUserAsync(seguidorId, seguidoId);
            if (result)
            {
                return Ok(new { mensaje = "Has dejado de seguir al usuario." });
            }
            else
            {
                return NotFound(new { mensaje = "Relación de seguimiento no encontrada." });
            }
        }
    }
}
