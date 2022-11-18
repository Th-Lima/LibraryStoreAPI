using System.ComponentModel.DataAnnotations;

namespace LibraryStore.Api.Dtos.AuthUserDtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(10, ErrorMessage = "O campo {0} precisa ter entre {1} e {2} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não são correspondentes")]
        public string ConfirmPassword { get; set; }
    }
}
