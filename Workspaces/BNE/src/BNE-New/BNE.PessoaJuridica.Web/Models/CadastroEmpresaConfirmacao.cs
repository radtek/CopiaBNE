using System;

namespace BNE.PessoaJuridica.Web.Models
{
    public class CadastroEmpresaConfirmacao
    {
        public CadastroEmpresaConfirmacao(string linkPlanoFree, string linkPlanoPago, string link, bool receitaOnline, bool empresaJaCadastrada, decimal valorPlanoPagoDe, decimal valorPlanoPagoPara)
        {
            LinkPlanoFree = linkPlanoFree;
            LinkPlanoPago = linkPlanoPago;
            Link = link;
            ReceitaOnline = receitaOnline;
            EmpresaJaCadastrada = empresaJaCadastrada;
            ValorPlanoPagoDe = valorPlanoPagoDe;
            ValorPlanoPagoPara = valorPlanoPagoPara;
            PercentualDesconto = Convert.ToInt32((1 - Convert.ToDecimal(valorPlanoPagoPara / valorPlanoPagoDe)) * 100);
        }

        public string LinkPlanoFree { get; private set; }
        public string LinkPlanoPago { get; private set; }
        public bool ReceitaOnline { get; private set; }
        public string Link { get; private set; }
        public bool EmpresaJaCadastrada { get; private set; }
        public decimal ValorPlanoPagoDe { get; private set; }
        public decimal ValorPlanoPagoPara { get; private set; }
        public int PercentualDesconto { get; private set; }

    }
}