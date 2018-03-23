using BNE.Web.Services.Solr.Database.Repositories;

namespace BNE.Web.Services.Solr.Domain
{
    public class CidadeNaoEncontrada
    {

        private readonly ICidadeNaoEncontradaRepository _cidadeNaoEncontradaRepository;

        public CidadeNaoEncontrada(ICidadeNaoEncontradaRepository cidadeNaoEncontradaRepository)
        {
            this._cidadeNaoEncontradaRepository = cidadeNaoEncontradaRepository;
        }

        #region SalvarCidadeNaoEncontrada
        public bool SalvarCidadeNaoEncontrada(string queryPesquisada, string origem)
        {
            if (string.IsNullOrWhiteSpace(queryPesquisada))
                return false;

            if (queryPesquisada.Length >= 2)
            {
                var objCidadeNaoEncontrada = new Models.CidadeNaoEncontrada
                {
                    DescricaoConteudoBuscado = queryPesquisada,
                    DescricaoOrigem = origem
                };
                _cidadeNaoEncontradaRepository.Add(objCidadeNaoEncontrada);
            }
            return true;
        }
        #endregion

    }
}