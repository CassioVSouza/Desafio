using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class Amostra
    {
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Esse campo precisa conter entre 5 e 100 caractéres!")]
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        public string Descricao { get; set; } = null!;
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        public DateTime DataRecebimento { get; set; }
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        public string Status { get; set; } = null!;


        public void LimparPossiveisEspacosBrancos()
        {
            if(Descricao != null)
            {
                Descricao = Descricao.Trim();
            }
        }
    }
}
