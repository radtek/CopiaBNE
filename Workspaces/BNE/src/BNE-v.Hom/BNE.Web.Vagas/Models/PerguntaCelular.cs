using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BNE.Web.Vagas.Models
{
    [Serializable]
    public class PerguntaCelular
    {
        public int Identificador { get; set; }

        

        //[RegularExpression(@"^\d{2}$", ErrorMessage = "DDD inválido.")]
        [DisplayName("DDD")]
        //[Required(ErrorMessage = "Informe o DDD.")]
        public string NumeroDDDCelular { get; set; }

        
        //[RegularExpression(@"^\d{9}|^\d{8}$", ErrorMessage = "Número inválido.")]
        [DisplayName("Celular")]
        //[Required(ErrorMessage = "Informe o número do telefone.")]
        public string NumeroCelular { get; set; }

        public int IdentificadorVaga { get; set; }
        public int IdPerguntaHistorico { get; set; }
        public string NumeroDDDCelularAntigo { get; set; }
        public string NumeroCelularAntigo { get; set; }

        [DisplayName("Digite o código de validação")]
        public string CodigoValidacao { get; set; }
        public bool FlagCelularConfirmado { get; set; }
    }
}