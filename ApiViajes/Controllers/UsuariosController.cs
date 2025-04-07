using ApiViajes.Helpers;
using ApiViajes.Models;
using ApiViajes.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiViajes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private RepositoryViaje repo;
        private HelperUsuarioToken helper;
        public UsuariosController
            (RepositoryViaje repo
            , HelperUsuarioToken helper)
        {
            this.helper = helper;
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioCompletoView>>> GetUsuarios()
        {
            return await this.repo.GetUsuariosAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioCompletoView>>FindEmpleado(int id)
        {
            return await this.repo.FindUsuarioAsync(id);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<UsuarioCompletoViewModel>>Perfil()
        {
            UsuarioCompletoViewModel model = this.helper.GetUsuario();
            return model;
        }   
    }
}
