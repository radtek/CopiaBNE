using System.Collections.Generic;
using System.Linq;

namespace BNE.Solr
{
    public class CurriculoAvaliacao : SolrGroupedResponse<GroupedPorIdfCurriculo<CurriculoAvaliacao.Response>, CurriculoAvaliacao.Response>
    {
        public class Response : Docs
        {
            public int Idf_Curriculo { get; set; }
            public string Idf_Usuario_Filial_Perfil { get; set; }
            public int Idf_Curriculo_Classificacao { get; set; }
            public string id { get; set; }
            public int Idf_Avaliacao { get; set; }
            public string Dta_Cadastro { get; set; }
            public string Des_Observacao { get; set; }
            public int Idf_Filial { get; set; }
        }

        public static List<int> RequisitarItensPorAvaliacao(List<Matches<Response>> response, List<int> avaliacoes = null, string comentario = null)
        {
            if (avaliacoes == null)
                avaliacoes = new List<int>();

            List<int> result = new List<int>();
            
            if (avaliacoes.Any() && !string.IsNullOrEmpty(comentario))
            {
                result = (from p in response
                          where avaliacoes.Contains(p.doclist.docs[0].Idf_Avaliacao) && p.doclist.docs[0].Des_Observacao != null && p.doclist.docs[0].Des_Observacao.Contains(comentario)
                          select p.groupValue).ToList();
            }
            else if (avaliacoes.Any())
            {
                result = (from p in response
                          where avaliacoes.Contains(p.doclist.docs[0].Idf_Avaliacao)
                          select p.groupValue).ToList();
            }
            else if (!string.IsNullOrEmpty(comentario))
            {
                result = (from p in response
                          where p.doclist.docs[0].Des_Observacao != null && p.doclist.docs[0].Des_Observacao.Contains(comentario)
                          select p.groupValue).ToList();
            }
            
            return result;
        }


        public List<int> RequisitarItensPorAvaliacao(List<int> avaliacoes = null, string comentario = null)
        {
            if (avaliacoes == null)
                avaliacoes = new List<int>();

            List<int> result = new List<int>();

            if (avaliacoes.Any() && !string.IsNullOrEmpty(comentario))
            {
                result = (from p in this.grouped.Idf_Curriculo.groups
                          where avaliacoes.Contains(p.doclist.docs[0].Idf_Avaliacao) && p.doclist.docs[0].Des_Observacao.Contains(comentario)
                          select p.groupValue).ToList();
            }
            else if (avaliacoes.Any())
            {
                result = (from p in this.grouped.Idf_Curriculo.groups
                          where avaliacoes.Contains(p.doclist.docs[0].Idf_Avaliacao)
                          select p.groupValue).ToList();
            }
            else if (!string.IsNullOrEmpty(comentario))
            {
                result = (from p in this.grouped.Idf_Curriculo.groups
                          where p.doclist.docs[0].Des_Observacao.Contains(comentario)
                          select p.groupValue).ToList();
            }

            return result;
        }
    }
}
