namespace Frontend.Servicos.Interfaces
{
    public interface IValidacaoServico
    {
        Task<bool> ValidarTokenDeAcessoAsync();
    }
}
