using Backend.DTOs;
using Backend.Enums;
using Backend.Repositorio.Principal;
using Backend.Servicos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/api/[controller]")]
    public class AmostraController : ControllerBase
    {
        private readonly ILogServico _logServico;
        private readonly IAmostraServico _amostraServico;

        public AmostraController(ILogServico logServico, IAmostraServico amostraServico)
        {
            _logServico = logServico;
            _amostraServico = amostraServico;
        }

        [HttpPost]
        [Route("/AdicionarAmostra")]
        public async Task<IActionResult> AdicionarAmostra([FromBody] AmostraDTO amostra)
        {
            try
            {
                var amostraFiltrada = _amostraServico.ValidarInformacoes(amostra);

                if (amostraFiltrada == null)
                    return BadRequest(new ErroAPIDTO()
                    {
                        codigoErro = (int)ERespostasAPI.InfosInvalidas,
                        Motivo = "Os dados não passaram na validação"
                    });

                var resposta = await _amostraServico.AdicionarAmostraAsync(amostraFiltrada);

                if (resposta)
                    return Ok();

                return StatusCode(500, (int)ERespostasAPI.ErroServidor);
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraController)}, função {nameof(AdicionarAmostra)}: {ex.Message}");
                return StatusCode(500, (int)ERespostasAPI.ErroServidor);
            }
        }

        [HttpPut]
        [Route("/AtualizarAmostra")]
        public async Task<IActionResult> AtualizarAmostra([FromBody] AmostraDTO amostra)
        {
            try
            {
                var amostraFiltrada = _amostraServico.ValidarInformacoes(amostra);

                if (amostraFiltrada == null)
                    return BadRequest(new ErroAPIDTO()
                    {
                        codigoErro = (int)ERespostasAPI.InfosInvalidas,
                        Motivo = "Os dados não passaram na validação"
                    });

                var resposta = await _amostraServico.AtualizarAmostraAsync(amostraFiltrada);

                if (resposta)
                    return Ok();

                return StatusCode(500, (int)ERespostasAPI.ErroServidor);
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraController)}, função {nameof(AtualizarAmostra)}: {ex.Message}");
                return StatusCode(500, (int)ERespostasAPI.ErroServidor);
            }
        }

        [HttpDelete]
        [Route("/DeletarAmostra")]
        public async Task<IActionResult> DeletarAmostra([FromBody] string codigo)
        {
            try
            {
                var resposta = await _amostraServico.ExcluirAmostraAsync(codigo);

                if (!resposta)
                    return BadRequest(new ErroAPIDTO()
                    {
                        codigoErro = (int)ERespostasAPI.ErroServidor,
                        Motivo = "Ocorreu um erro ao tentar deletar a amostra"
                    });

                return Ok();
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraController)}, função {nameof(DeletarAmostra)}: {ex.Message}");
                return StatusCode(500, (int)ERespostasAPI.ErroServidor);
            }
        }

        [HttpGet]
        [Route("/ConsultarAmostras")]
        public async Task<IActionResult> ConsultarAmostras()
        {
            try
            {
                var amostras = await _amostraServico.ConsultarAmostrasAsync();

                return Ok(amostras);
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraController)}, função {nameof(ConsultarAmostras)}: {ex.Message}");
                return StatusCode(500, (int)ERespostasAPI.ErroServidor);
            }
        }

        [HttpPost]
        [Route("/ConsultarAmostraPorCodigo")]
        public async Task<IActionResult> ConsultarAmostraPorCodigo([FromBody] string codigo)
        {
            try
            {
                var amostra = await _amostraServico.ConsultarAmostraPorCodigoAsync(codigo);

                if (amostra == null)
                    return BadRequest(new ErroAPIDTO()
                    {
                       codigoErro = (int)ERespostasAPI.NaoEncontrado,
                       Motivo = "Esse codigo nao corresponde a nenhuma amostra do banco de dados."
                    });

                return Ok(amostra);
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraController)}, função {nameof(ConsultarAmostraPorCodigo)}: {ex.Message}");
                return StatusCode(500, (int)ERespostasAPI.ErroServidor);
            }
        }

        [HttpPost]
        [Route("/ConsultarAmostrasFiltradas")]
        public async Task<IActionResult> ConsultarAmostrasFiltradas([FromBody] FiltroDTO filtro)
        {
            try
            {
                var amostras = await _amostraServico.ConsultarAmostrasFiltradasAsync(filtro.Data, filtro.Status);

                return Ok(amostras);
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AmostraController)}, função {nameof(ConsultarAmostrasFiltradas)}: {ex.Message}");
                return StatusCode(500, (int)ERespostasAPI.ErroServidor);
            }
        }
    }
}
