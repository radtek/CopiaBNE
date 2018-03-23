using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sample.DTO
{
    public class CobrancaRecorrencia
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dta_cadastro { get; set; }

        public string des_resultado_solicitacao_aprovacao { get; set; }

        public decimal Vlr_Documento { get; set; }
        public bool flg_transacao_aprovada { get; set; }

        public string nme_pessoa { get; set; }
        public string des_plano_situacao { get; set; }

        public decimal num_cnpj { get; set; }

        public string Telefone { get; set; }

        public string des_plano { get; set; }
    }
}