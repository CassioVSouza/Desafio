using Frontend.Servicos.Interfaces;

namespace Frontend.Servicos.Principal
{
    public class LogServico : ILogServico
    {
        public void EnviarLog(string mensagem)
        {

            string caminhoDaAplicacao = AppContext.BaseDirectory;


            string caminhoDaPasta = Path.Combine(caminhoDaAplicacao, "Log");

            if (!Directory.Exists(caminhoDaPasta))
            {
                Directory.CreateDirectory(caminhoDaPasta);
            }

            string caminhoDoArquivo = Path.Combine(caminhoDaPasta, "log.txt");

            string logFormatado = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensagem}{Environment.NewLine}";

            File.AppendAllText(caminhoDoArquivo, logFormatado);
        }
    }
}
