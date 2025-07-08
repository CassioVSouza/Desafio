using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class AmostraDTO
    {
        [StringLength(100)]
        [Required]
        public string Descricao { get; set; } = null!;
        [Required]
        public DateTime DataRecebimento { get; set; }
        [StringLength(30)]
        [Required]
        public string Status { get; set; } = null!;


        public void LimparPossiveisEspacosBrancos()
        {
            Descricao = Descricao.Trim();
            Status = Status.Trim();
        }
    }
}
