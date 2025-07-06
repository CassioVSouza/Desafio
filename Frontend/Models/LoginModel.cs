using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "O campo precisa ter entre 5 e 30 caractéres!")]
        public string user { set; get; } = string.Empty;

        [Required(ErrorMessage = "Esse campo é obrigatório!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{5,20}$",
        ErrorMessage = "A string deve conter entre 5 e 20 caracteres, com pelo menos uma letra maiúscula, uma minúscula, um número e um símbolo.")]
        public string senha { get; set; } = string.Empty;

    }
}
