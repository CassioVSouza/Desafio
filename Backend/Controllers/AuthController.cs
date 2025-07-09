using Backend.DTOs;
using Backend.Enums;
using Backend.Repositorio.Principal;
using Backend.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthServico _authServico;
        private readonly ILogServico _logServico;

        public AuthController(IAuthServico authServico, ILogServico logServico)
        {
            _authServico = authServico;
            _logServico = logServico;
        }

        [HttpPost]
        [Route("/AdicionarUsuario")]
        public async Task<IActionResult> AdicionarUsuario()
        {
            try
            {
                string usuario = "Usuario"; //dados do usuario hardcodado para simplificar os testes
                string senha = "SenhaTeste123!";

                var respostaUsuarioNaoEncontrado = await _authServico.UsuarioNaoExisteAsync(usuario);

                if (respostaUsuarioNaoEncontrado == false)
                    return BadRequest(new ErroAPIDTO()
                    {
                        codigoErro = (int)ERespostasAPI.InfosInvalidas,
                        Motivo = "Usuário já existe!"
                    });

                var usuarioModel = _authServico.ValidarInformacoes(usuario, senha);

                if (usuarioModel != null)
                {
                    var respostaUsuarioAdicionado = await _authServico.AdicionarUsuarioAsync(usuarioModel);

                    if (respostaUsuarioAdicionado)
                        return Ok();

                    return StatusCode(500, "Ocorreu um erro, tente novamente mais tarde!");
                }
                return BadRequest("Informações Inválidas!");

            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AuthController)}, função {nameof(AdicionarUsuario)}: {ex.Message}");
                return StatusCode(500, "Ocorreu um erro, tente novamente mais tarde!");
            }
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> Login([FromBody]UsuarioDTO login)
        {
            try
            {
                var respostaValidacao = await _authServico.ValidarUsuarioAsync(login.user, login.senha);

                if (!respostaValidacao)
                    return BadRequest(new ErroAPIDTO()
                    {
                        codigoErro = (int)ERespostasAPI.InfosInvalidas,
                        Motivo = "Usuário ou senha inválidos!"
                    });

                var tokenDeAcesso = _authServico.RetornarTokenDeAcessoAsync(login.user);

                if(tokenDeAcesso == null)
                    return StatusCode(500, "Ocorreu um erro, tente novamente mais tarde!");

                return Ok(new {
                    token = tokenDeAcesso
                });
            }
            catch (Exception ex)
            {
                _logServico.EnviarLog($"Erro em {nameof(AuthController)}, função {nameof(Login)}: {ex.Message}");
                return StatusCode(500, "Ocorreu um erro, tente novamente mais tarde!");
            }
        }
    }
}
