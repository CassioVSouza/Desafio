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

        public async Task<bool> AdicionarAmostra(Amostra amostra)
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
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(AdicionarAmostra)}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeletarAmostra(int codigo)
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
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(DeletarAmostra)}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AtualizarAmostra(Amostra amostra)
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
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(AtualizarAmostra)}: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Amostra>> ConsultarAmostras()
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
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(ConsultarAmostras)}: {ex.Message}");
                return new List<Amostra>();
            }
        }

        public async Task<List<Amostra>> ConsultarAmostrasFiltradas(DateTime Data, string Status)
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
                _logServico.EnviarLog($"Erro em {nameof(AmostraRepositorio)}, função {nameof(ConsultarAmostrasFiltradas)}: {ex.Message}");
                return new List<Amostra>();
            }
        }
    }
}
