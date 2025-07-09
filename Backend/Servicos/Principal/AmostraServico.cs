using Backend.DTOs;
using Backend.Models;
using Backend.Repositorio.Interfaces;
using Backend.Repositorio.Principal;
using Backend.Servicos.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Backend.Servicos.Principal
{
    public class AmostraServico : IAmostraServico
    {
        private readonly ILogServico _logServico;
        private readonly IAmostraRepositorio _amostraRepo;

        public AmostraServico(ILogServico logServico, IAmostraRepositorio amostraRepo)
        {
            _logServico = logServico;
            _amostraRepo = amostraRepo;
        }

        public async Task<bool> AdicionarAmostraAsync(Amostra amostra)
        {
            try
            {
                var resposta = await _amostraRepo.AdicionarAmostraAsync(amostra);

                if (!resposta)
                    return false;

                return true;

            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(AdicionarAmostraAsync)}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AtualizarAmostraAsync(Amostra amostra)
        {
            try
            {

                var resposta = await _amostraRepo.AtualizarAmostraAsync(amostra);

                if (!resposta)
                    return false;

                return true;

            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(AtualizarAmostraAsync)}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ExcluirAmostraAsync(string codigo)
        {
            try
            {
                var excluiu = await _amostraRepo.DeletarAmostraAsync(codigo);

                return excluiu;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(ExcluirAmostraAsync)}: {ex.Message}");
                return false;
            }
        }

        public async Task<Amostra?> ConsultarAmostraPorCodigoAsync(string codigo)
        {
            try
            {
                var amostra = await _amostraRepo.ConsultarAmostraPorCodigoAsync(codigo);

                if (amostra == null)
                    return null;

                return amostra;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(ConsultarAmostraPorCodigoAsync)}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Amostra>> ConsultarAmostrasAsync()
        {
            try
            {
                var amostras = await _amostraRepo.ConsultarAmostrasAsync();

                return amostras;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(ConsultarAmostrasAsync)}: {ex.Message}");
                return new List<Amostra>();
            }
        }

        public async Task<List<Amostra>> ConsultarAmostrasFiltradasAsync(DateTime? date, string? status)
        {
            try
            {
                status = string.IsNullOrEmpty(status) ? null : status;
                var amostras = await _amostraRepo.ConsultarAmostrasFiltradasAsync(date, status);

                return amostras;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(ConsultarAmostrasFiltradasAsync)}: {ex.Message}");
                return new List<Amostra>();
            }
        }


        public Amostra? ValidarInformacoes(AmostraDTO amostra, bool isEdicao)
        {
            try
            {
                amostra.LimparPossiveisEspacosBrancos();

                var contexto = new ValidationContext(amostra, null, null);

                var valido = Validator.TryValidateObject(amostra, contexto, null, true);

                if (!valido)
                    return null!;

                var amostraEntity = new Amostra()
                {
                    DataRecebimento = amostra.DataRecebimento,
                    Descricao = amostra.Descricao,
                    Status = amostra.Status,
                };

                if (isEdicao)
                {
                    amostraEntity.Codigo = amostra.Codigo!;
                }
                else
                {
                    amostraEntity.Codigo = Guid.NewGuid().ToString();
                }

                    return amostraEntity;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraServico)}, função {nameof(ValidarInformacoes)}: {ex.Message}");
                return null!;
            }
        }

    }
}
