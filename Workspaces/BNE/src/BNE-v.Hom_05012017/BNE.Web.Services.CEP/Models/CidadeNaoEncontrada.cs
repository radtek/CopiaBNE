using System;

namespace BNE.Web.Services.Solr.Models
{
    public class CidadeNaoEncontrada
    {
        public int IdCidadeNaoEncontrada { get; set; }
        public string DescricaoConteudoBuscado { get; set; }
        public string DescricaoOrigem { get; set; }
        public DateTime DataCadastro { get; set; }

        public CidadeNaoEncontrada()
        {
            DataCadastro = DateTime.Now;
        }
    }
}