using ApiViajes.Helpers;
using ApiViajes.Models;
using ApiViajes.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using ViajesMvcNetCore.Models;

namespace ApiViajes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private RepositoryViaje repo;
        private HelperUsuarioToken helper;

        public UsuariosController(RepositoryViaje repo, HelperUsuarioToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        // Obtener todos los usuarios
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {
            var usuarios = await this.repo.GetUsuariosAsync();
            return Ok(usuarios);
        }

        // Obtener un usuario por su ID
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> FindEmpleado(int id)
        {
            var usuario = await this.repo.FindUsuarioAsync(id);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }
            return Ok(usuario);
        }

        // Obtener el perfil completo del usuario (requiere autenticación)
        [Authorize]
        [HttpGet("perfil")]
        public async Task<ActionResult<UsuarioCompletoView>> Perfil()
        {
            // Obtenemos el usuario del token actual
            var usuarioId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var usuarioPerfil = await this.repo.GetUsuarioPerfilAsync(usuarioId);
            if (usuarioPerfil == null)
            {
                return NotFound("Perfil no encontrado.");
            }
            return Ok(usuarioPerfil);
        }

        // Actualizar el perfil del usuario (requiere autenticación)
        [Authorize]
        [HttpPut("perfil")]
        public async Task<ActionResult> UpdatePerfil([FromBody] Usuario usuario)
        {
            var usuarioId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (usuarioId != usuario.IdUsuario)
            {
                return BadRequest("No puedes actualizar un perfil que no te pertenece.");
            }

            var resultado = await this.repo.UpdateUsuarioAsync(usuario);
            if (resultado)
            {
                return Ok("Perfil actualizado correctamente.");
            }

            return BadRequest("Hubo un problema al actualizar el perfil.");
        }

        // Seguir a otro usuario (requiere autenticación)
        [Authorize]
        [HttpPost("seguir/{idSeguido}")]
        public async Task<ActionResult> FollowUser(int idSeguido)
        {
            var usuarioId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var resultado = await this.repo.FollowUserAsync(usuarioId, idSeguido);
            if (resultado)
            {
                return Ok("Siguiendo al usuario.");
            }

            return BadRequest("No puedes seguir a este usuario.");
        }

        // Dejar de seguir a un usuario (requiere autenticación)
        [Authorize]
        [HttpDelete("unfollow/{idSeguido}")]
        public async Task<ActionResult> UnfollowUser(int idSeguido)
        {
            var usuarioId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var resultado = await this.repo.UnfollowUserAsync(usuarioId, idSeguido);
            if (resultado)
            {
                return Ok("Dejado de seguir al usuario.");
            }

            return BadRequest("No estás siguiendo a este usuario.");
        }

        // Obtener los seguidores de un usuario (requiere autenticación)
        [Authorize]
        [HttpGet("seguidores")]
        public async Task<ActionResult<List<UsuarioSeguidoPerfil>>> GetSeguidores()
        {
            var usuarioId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var seguidores = await this.repo.GetSeguidoresAsync(usuarioId);
            if (seguidores == null || !seguidores.Any())
            {
                return NotFound("No tienes seguidores.");
            }
            return Ok(seguidores);
        }

        // Obtener los usuarios que un usuario sigue (requiere autenticación)
        [Authorize]
        [HttpGet("seguidos")]
        public async Task<ActionResult<List<UsuarioSeguidoPerfil>>> GetSeguidos()
        {
            var usuarioId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var seguidos = await this.repo.GetSeguidosAsync(usuarioId);
            if (seguidos == null || !seguidos.Any())
            {
                return NotFound("No estás siguiendo a nadie.");
            }
            return Ok(seguidos);
        }

        // Obtener el chat entre dos usuarios (requiere autenticación)
        [Authorize]
        [HttpGet("chat/{idUsuario2}")]
        public async Task<ActionResult<List<Chat>>> GetChat(int idUsuario2)
        {
            var usuarioId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var chat = await this.repo.GetChatAsync(usuarioId, idUsuario2);
            if (chat == null || !chat.Any())
            {
                return NotFound("No hay mensajes entre estos usuarios.");
            }
            return Ok(chat);
        }

        // Enviar un mensaje (requiere autenticación)
        [Authorize]
        [HttpPost("chat/{idDestinatario}")]
        public async Task<ActionResult> SendMessage(int idDestinatario, [FromBody] string mensaje)
        {
            var usuarioId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var resultado = await this.repo.SendMessageAsync(usuarioId, idDestinatario, mensaje);
            if (resultado)
            {
                return Ok("Mensaje enviado correctamente.");
            }

            return BadRequest("No se pudo enviar el mensaje.");
        }
    }
}
