using Backend.DTOs;
using Frontend.Enums;
using Frontend.Models;
using Frontend.Servicos.Interfaces;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using System.Security.Cryptography;
using System.Text;

namespace Frontend.Servicos.Principal
{
    public class LoginServico : ILoginServico
    {
        private readonly ILogServico _logServico;
        private readonly HttpClient _httpClient;
        private readonly ProtectedSessionStorage _session;

        public LoginServico(ILogServico log, IHttpClientFactory client, ProtectedSessionStorage httpContextAccessor)
        {
            _logServico = log;
            _httpClient = client.CreateClient("ApiClient"); //Pega a URL base da API Backend, o caminho pode ser mudado no program.cs
            _session = httpContextAccessor;
        }

        public async Task<ERespostaAPI> FazerLoginAsync(LoginModel login)
        {
            try
            {
                var resposta = await _httpClient.PostAsJsonAsync("/Login", login);

                if (resposta.IsSuccessStatusCode)
                {
                    var conteudo = await resposta.Content.ReadFromJsonAsync<Token>();

                    if (conteudo != null)
                    {
                        await _session.SetAsync("authToken", conteudo.token);
                        return ERespostaAPI.Ok; ;
                    }
                    return ERespostaAPI.ErroServidor;
                }

                var erro = await resposta.Content.ReadFromJsonAsync<ErroAPI>();
                ERespostaAPI respostaAPI = (ERespostaAPI)erro!.codigoErro;

                return respostaAPI;
            }
            catch (Exception ex) {
                _logServico.EnviarLog($"Erro em {nameof(LoginServico)}, função {nameof(FazerLoginAsync)}: {ex.Message}");
                return ERespostaAPI.ErroServidor;
            }
        }
    }
}
