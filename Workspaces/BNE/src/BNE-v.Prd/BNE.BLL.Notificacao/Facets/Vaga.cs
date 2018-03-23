using System.Collections.Generic;
using System.Linq;
using BNE.BLL.Common;

namespace BNE.BLL.Notificacao.Facets
{
    public class Vaga
    {
        public int QuantidadeFuncaoCidade { get; set; }
        public string ConteudoFuncaoCidade { get; set; }
        public int QuantidadeFuncaoArea { get; set; }
        public string ConteudoFuncaoArea { get; set; }
    }

    public class FacetVaga
    {
        public FacetVaga()
        {
            FuncaoCidade = new List<Facet>();
            FuncaoArea = new List<Facet>();
        }

        public List<Facet> FuncaoCidade { get; set; }

        public int QuantidadeFuncaoCidade
        {
            get { return FuncaoCidade.Sum(c => c.QuantidadeVaga); }
        }

        public List<Facet> FuncaoArea { get; set; }

        public int QuantidadeFuncaoArea
        {
            get { return FuncaoArea.Sum(c => c.QuantidadeVaga); }
        }

        public string TemplateFuncaoCidade => @"<tr><td bgcolor=""#ffffff"" style=""border-bottom:solid 4px #78909c""><a style=""color:#202020;"" href=""{5}"" style=""color:#202020;""><p style=""font-family: Arial,sans-serif;margin:0;padding:0;color:#757575;font-size:0.8em"">{3} {4} para <strong>{0} em {1}/{2} </strong></p></a></td></tr>";

        public string TemplateFuncaoArea => @"<tr><td bgcolor=""#ffffff"" style=""border-bottom:solid 4px #78909c""><a style=""color:#202020;"" href=""{5}"" style=""color:#202020;""><p style=""font-family: Arial,sans-serif;margin:0;padding:0;color:#757575;font-size:0.8em"">{3} {4} para <strong>{0} em {1}/{2} </strong></p></a></td></tr>";
    }

    public class Facet
    {
        public readonly string Cidade;
        public readonly string Funcao;
        public readonly string SiglaEstado;
        public readonly string Url;
        public readonly string Utm;
        public readonly int QuantidadeVaga;

        public Facet(int quantidadeVaga, string funcao, string cidade, string siglaEstado, string utm)
        {
            QuantidadeVaga = quantidadeVaga;
            Funcao = funcao;
            Cidade = cidade;
            SiglaEstado = siglaEstado;

            Url = $"/vagas-de-emprego-para-{funcao.NormalizarURL()}-em-{cidade.NormalizarURL()}-{siglaEstado}";
            Utm = utm;
        }
    }
}