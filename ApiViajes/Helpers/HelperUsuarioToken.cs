
using ApiViajes.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiViajes.Helpers
{
    public class HelperUsuarioToken
    {
        private IHttpContextAccessor contextAccessor;

        public HelperUsuarioToken(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public UsuarioCompletoViewModel GetUsuario()
        {
            Claim claim =
                contextAccessor.HttpContext
                .User.FindFirst(x => x.Type == "UserData");
            string json = claim.Value;
            string jsonUsuario =
                HelperCryptography.DecryptString(json);
            UsuarioCompletoViewModel model = JsonConvert
                .DeserializeObject<UsuarioCompletoViewModel>(jsonUsuario);
            return model;
        }
    }
}
