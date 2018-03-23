using System;

namespace BNE.Web.Services.Solr.Models
{
    public class FuncaoNaoEncontrada
    {
        public int IdFuncaoNaoEncontrada { get; set; }
        public string DescricaoConteudoBuscado { get; set; }
        public string DescricaoOrigem { get; set; }
        public DateTime DataCadastro { get; set; }

        public FuncaoNaoEncontrada()
        {
            DataCadastro = DateTime.Now;
        }
    }
}