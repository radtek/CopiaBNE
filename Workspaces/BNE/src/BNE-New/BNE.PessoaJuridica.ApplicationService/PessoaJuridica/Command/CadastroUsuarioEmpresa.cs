using System;
using System.ComponentModel.DataAnnotations;

namespace BNE.PessoaJuridica.ApplicationService.PessoaJuridica.Command
{
    public class CadastroUsuarioEmpresa
    {

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage =  "O campo Sexo é obrigatório.")]
        [RegularExpression("[F]|[M]", ErrorMessage = "O campo Sexo deve conter a letra maiúscula F ou M.")]
        public string Sexo { get; set; }
        [Required(ErrorMessage =  "O campo NumeroCPF é obrigatório.")]
        public string NumeroCPF { get; set; }
        [Required(ErrorMessage = "O campo DataNascimento é obrigatório.")]
        public DateTime DataNascimento { get; set; }
        [Required(ErrorMessage = "O campo Funcao é obrigatório.")]
        public string Funcao { get; set; }
        public string NumeroDDDCelular { get; set; }
        [Required(ErrorMessage = "O campo NumeroCelular é obrigatório.")]
        public string NumeroCelular { get; set; }
        public string NumeroDDDComercial { get; set; }
        public string NumeroComercial { get; set; }
        public string NumeroComercialRamal { get; set; }
        public string IP { get; set; }
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email do usuário responsável inválido.")]
        public string Email { get; set; }

        public string NumeroCNPJ { get; set; }
        public string EmailOriginal { get; set; }

        public bool FlgWhatsApp { get; set; }

    }
}