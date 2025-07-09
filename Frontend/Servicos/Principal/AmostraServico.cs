using Backend.DTOs;
using Frontend.Components.Pages.Amostras;
using Frontend.Enums;
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

        public async Task<bool> RemoverAmostraAsync(string codigo)
        {
            try
            {
                var token = await _session.GetAsync<string>("authToken");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);

                var resposta = await _httpClient.DeleteAsync($"/DeletarAmostra?codigo={codigo}");

                if (resposta.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(RemoverAmostraAsync)}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Amostra>?> ConsultarAmostrasFiltradasAsync(Filtro filtro)
        {
            try
            {
                var token = await _session.GetAsync<string>("authToken");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);

                var resposta = await _httpClient.PostAsJsonAsync("/ConsultarAmostrasFiltradas", filtro);

                if (resposta.IsSuccessStatusCode)
                {
                    var amostras = await resposta.Content.ReadFromJsonAsync<List<Amostra>>();

                    if (amostras != null)
                        return amostras;

                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(ConsultarAmostrasFiltradasAsync)}: {ex.Message}");
                return null;
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

        public async Task<ERespostaAPI> EditarAmostraAsync(Amostra amostra)
        {
            try
            {
                var token = await _session.GetAsync<string>("authToken");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);

                var resposta = await _httpClient.PutAsJsonAsync("/AtualizarAmostra", amostra);

                if (resposta.IsSuccessStatusCode)
                {
                    return ERespostaAPI.Ok;
                }

                var erro = await resposta.Content.ReadFromJsonAsync<ErroAPI>();
                ERespostaAPI respostaAPI = (ERespostaAPI)erro!.codigoErro;
                return respostaAPI;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(EditarAmostraAsync)}: {ex.Message}");
                return ERespostaAPI.ErroServidor;
            }
        }

        public async Task<List<Amostra>?> ConsultarAmostrasAsync()
        {
            try
            {
                var token = await _session.GetAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);

                var resposta = await _httpClient.GetAsync("/ConsultarAmostras");

                if (resposta.IsSuccessStatusCode)
                {
                    var amostras = await resposta.Content.ReadFromJsonAsync<List<Amostra>>();

                    if (amostras == null)
                        return null;

                    return amostras;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(ConsultarAmostrasAsync)}: {ex.Message}");
                return null;
            }
        }

        public async Task<(ERespostaAPI? respostaErro, Amostra? amostra)> ConsultarAmostraPorCodigoAsync(string codigo)
        {
            try
            {
                var token = await _session.GetAsync<string>("authToken");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);

                var resposta = await _httpClient.PostAsJsonAsync("/ConsultarAmostraPorCodigo", codigo);

                if (resposta.IsSuccessStatusCode)
                {
                    var amostra = await resposta.Content.ReadFromJsonAsync<Amostra>();

                    if (amostra == null)
                        return (ERespostaAPI.NaoEncontrado, null);

                    return (null, amostra);
                }

                var erro = await resposta.Content.ReadFromJsonAsync<ErroAPI>();

                ERespostaAPI respostaAPI = (ERespostaAPI)erro!.codigoErro;
                return (respostaAPI, null);
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(ConsultarAmostraPorCodigoAsync)}: {ex.Message}");
                return (ERespostaAPI.ErroServidor, null);
            }
        }
    }
}
