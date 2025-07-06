using Frontend.Enums;
using Frontend.Servicos.Interfaces;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Frontend.Servicos.Principal
{
    public class ValidacaoServico : IValidacaoServico
    {
        private readonly IJSRuntime _JS;
        private readonly ILogServico _logServico;

        public ValidacaoServico(IJSRuntime jSRuntime, ILogServico logServico)
        {
            _JS = jSRuntime;
            _logServico = logServico;
        }

        public async Task<bool> ValidarTokenDeAcessoAsync()
        {
            try
            {
                var token = await _JS.InvokeAsync<string>("localStorage.getItem", "authToken");

                var handler = new JwtSecurityTokenHandler();

                var jwtToken = handler.ReadJwtToken(token);
                var expClaim = jwtToken.Payload.Expiration;

                if (expClaim == null)
                    return false;

                var expTime = DateTimeOffset.FromUnixTimeSeconds((long)expClaim);
                var expirado = expTime < DateTimeOffset.UtcNow;

                if (expirado)
                    return false;

                var perm = jwtToken.Claims.FirstOrDefault(o => o.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && o.Value == "User");

                if (perm != null)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(LoginServico)}, função {nameof(ValidarTokenDeAcessoAsync)}: {ex.Message}");
                return false;
            }
        }
    }
}
