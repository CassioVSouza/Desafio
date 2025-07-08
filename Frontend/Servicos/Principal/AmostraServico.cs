using Frontend.Models;
using Frontend.Servicos.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Frontend.Servicos.Principal
{
    public class AmostraServico : IAmostraServico
    {
        private readonly ILogServico _logServico;
        private readonly HttpClient _httpClient;
        private readonly ProtectedSessionStorage _session;

        public AmostraServico(ILogServico logServico, IHttpClientFactory httpClient, ProtectedSessionStorage session)
        {
            _logServico = logServico;
            _httpClient = httpClient.CreateClient("ApiClient");
            _session = session;
        }

        public bool ValidarAmostra(Amostra amostra)
        {
            try
            {
                amostra.LimparPossiveisEspacosBrancos();
                var contexto = new ValidationContext(amostra, null, null);

                var resposta = Validator.TryValidateObject(amostra, contexto, null);

                return resposta;
            }catch(Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(ValidarAmostra)}: {ex.Message}");
                return false;
            } 
        }

        public async Task<bool> AdicionarAmostraAsync(Amostra amostra)
        {
            try
            {
                var token = await _session.GetAsync<string>("authToken");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);

                var resposta = await _httpClient.PostAsJsonAsync("/AdicionarAmostra", amostra);

                if (resposta.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(AdicionarAmostraAsync)}: {ex.Message}");
                return false;
            }
        }
    }
}
