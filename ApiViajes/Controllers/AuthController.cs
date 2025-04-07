using ApiViajes.Helpers;
using ApiViajes.Models;
using ApiViajes.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiViajes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryViaje repo;
        private HelperActionServicesOAuth helper;

        public AuthController(RepositoryViaje repo
            , HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult>Login(LoginModel model)
        {
            UsuarioCompletoView usuario = await
                this.repo.LogInUsuarioAsync(model.Email
                , model.Clave);
            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials =
                    new SigningCredentials
                    (this.helper.GetKeyToken(),
                    SecurityAlgorithms.HmacSha256);
                UsuarioCompletoViewModel modelUser = new UsuarioCompletoViewModel();
                modelUser.IdUsuario = usuario.IdUsuario;
                modelUser.Nombre = usuario.Nombre;
                modelUser.Email = usuario.Email;
                modelUser.Edad = usuario.Edad;
                modelUser.Nacionalidad = usuario.Nacionalidad;
                modelUser.PreferenciaViaje = usuario.PreferenciaViaje;
                modelUser.Imagen = usuario.Imagen;
                modelUser.FechaRegistro = usuario.FechaRegistro;
                modelUser.CorreoLogin = usuario.CorreoLogin;
                modelUser.Clave = usuario.Clave;
                modelUser.ConfirmarClave = usuario.ConfirmarClave;
                modelUser.AvatarUrl = usuario.AvatarUrl;
                string jsonUsuario =
                    JsonConvert.SerializeObject(modelUser);
                string jsonCrifado =
                    HelperCryptography.EncryptString(jsonUsuario);
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonCrifado)
                };
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: informacion,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        notBefore: DateTime.UtcNow
                        );
                return Ok(new
                {
                    response =
                    new JwtSecurityTokenHandler()
                    .WriteToken(token)
                });
            }
        }
    }
}
