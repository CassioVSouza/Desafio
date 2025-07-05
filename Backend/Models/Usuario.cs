using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Usuario
    {
        [Key]
        [StringLength(20)]
        public string User { get; set; } = null!;
        [StringLength(30)]
        public string Senha { get; set; } = null!;
    }
}
