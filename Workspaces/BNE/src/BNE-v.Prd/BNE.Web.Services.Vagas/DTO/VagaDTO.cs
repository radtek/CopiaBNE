using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BNE.Web.Services.Vagas.DTO
{
    public class VagaDTO
    {
        public int? Idf_Vaga { get; set; }
        //Verificar se enviar nulo
        public List<string> TipoVinculo { get; set; }

        [Required(ErrorMessage = "A função da vaga é obrigatória.")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "A função deve ter no mínimo 4 caracteres e no máximo 200.")]
        public string Funcao { get; set; }

        [Required(ErrorMessage = "A cidade da vaga é obrigatória.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "A cidade deve ter no mínimo 5 caracteres e no máximo 200.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "A quantidade de vagas é obrigatória.")]
        [Range(1, Double.MaxValue, ErrorMessage = "O valor mínimo para a quantidade é 1.")]
        public short? Quantidade { get; set; }


        public string Escolaridade { get; set; }


        public decimal? SalarioMin { get; set; }
        public decimal? SalarioMax { get; set; }
        public string Beneficios { get; set; }
        public short? IdadeMin { get; set; }
        public short? IdadeMax { get; set; }
        public string Sexo { get; set; }
        public string Requisitos { get; set; }
        public string Atribuicoes { get; set; }
        public List<string> Disponibilidade { get; set; }

        [Required(ErrorMessage = "Nome fantasia é obrigatório.")]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "Número de DDD é obrigatório.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O DDD deve ter dois caracteres.")]
        public string NumDDD { get; set; }

        [Required(ErrorMessage = "Número de telefone é obrigatório.")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "O Número de telefone deve ter entre 8 e 9 caracteres.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Não foi informado a confidencialidade da vaga.")]
        public bool? Confidencial { get; set; }

        [Required(ErrorMessage = "O email confidencial de retorno é obrigatório.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "O email informádo é inválido.")]
        [EmailAddress]
        public string Email { get; set; }

        public bool ReceberCadaCV { get; set; }
        public bool ReceberTodosCV { get; set; }
        public string PalavrasChave { get; set; }
        public List<PerguntaDTO> Perguntas { get; set; }
        public string Deficiencia { get; set; }

        public VagaDTO()
        {


        }
    }
}