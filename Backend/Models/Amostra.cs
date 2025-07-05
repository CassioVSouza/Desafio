using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Amostra
    {
        [Key]
        public string Codigo { get; set; } = null!;
        [StringLength(100)]
        public string Descricao { get; set; } = null!;
        public DateTime DataRecebimento { get; set; }
        [StringLength(30)]
        public string Status { get; set; } = null!;
    }
}
