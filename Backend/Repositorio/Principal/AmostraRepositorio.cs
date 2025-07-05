using Backend.Data;
using Backend.Models;
using Backend.Repositorio.Interfaces;
using Backend.Servicos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositorio.Principal
{
    public class AmostraRepositorio : IAmostraRepositorio
    {
        private readonly SqlContext _sqlContext;
        private readonly ILogServico _logServico;

        public AmostraRepositorio(SqlContext sqlContext, ILogServico logServico)
        {
            _sqlContext = sqlContext;
            _logServico = logServico;
        }

        public async Task<bool> AdicionarAmostraAsync(Amostra amostra)
        {
            try
            {
                await _sqlContext.Amostras.AddAsync(amostra);

                int mudancas = await _sqlContext.SaveChangesAsync();

                if(mudancas > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }catch(Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(AdicionarAmostraAsync)}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeletarAmostraAsync(int codigo)
        {
            try
            {
                var amostra = await _sqlContext.Amostras.FindAsync(codigo);

                if (amostra == null)
                    return false;


                _sqlContext.Amostras.Remove(amostra);

                int mudancas = await _sqlContext.SaveChangesAsync();

                if (mudancas > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(DeletarAmostraAsync)}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AtualizarAmostraAsync(Amostra amostra)
        {
            try
            {
                _sqlContext.Amostras.Update(amostra);

                int mudancas = await _sqlContext.SaveChangesAsync();

                if (mudancas > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(AtualizarAmostraAsync)}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Amostra>> ConsultarAmostrasAsync()
        {
            try
            {
                var amostras = await _sqlContext.Amostras.ToListAsync();

                if (amostras == null)
                    amostras = new List<Amostra>();

                return amostras;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(ConsultarAmostrasAsync)}: {ex.Message}");
                return new List<Amostra>();
            }
        }

        public async Task<List<Amostra>> ConsultarAmostrasFiltradasAsync(DateTime Data, string Status)
        {
            try
            {
                var amostras = await _sqlContext.Amostras.Where(o => o.DataRecebimento.Date == Data.Date && o.Status == Status).ToListAsync();

                if(amostras == null)
                    amostras = new List<Amostra>();

                return amostras;
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(ConsultarAmostrasFiltradasAsync)}: {ex.Message}");
                return new List<Amostra>();
            }
        }
    }
}
