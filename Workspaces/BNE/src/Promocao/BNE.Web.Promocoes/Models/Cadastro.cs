using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BNE.Web.Promocoes.Models
{
    public class Cadastro
    {
      
        [Required(ErrorMessage = "Campo Obrigatório.")]
        public String nome { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public string cpf { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public DateTime datanascimento { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public String cidade { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public String funcao { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email inválido")]
        public String email { get; set; }

        [Required(ErrorMessage = "Função Inválida.")]
        public String idFuncao { get; set; }

        [Required(ErrorMessage = "Cidade Inválida.")]
        public String idCidade { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public String pretensao { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public int sexo { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        public String celular { get; set; }
    }
}