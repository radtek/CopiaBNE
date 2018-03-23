using BNE.Web.Services.Solr.Database.Repositories;

namespace BNE.Web.Services.Solr.Domain
{
    public class FuncaoNaoEncontrada
    {

        private readonly IFuncaoNaoEncontradaRepository _funcaoNaoEncontradaRepository;

        public FuncaoNaoEncontrada(IFuncaoNaoEncontradaRepository funcaoNaoEncontradaRepository)
        {
            this._funcaoNaoEncontradaRepository = funcaoNaoEncontradaRepository;
        }

        #region SalvarFuncaoNaoEncontrada
        public bool SalvarFuncaoNaoEncontrada(string queryPesquisada, string origem)
        {
            if (string.IsNullOrWhiteSpace(queryPesquisada))
                return false;

            if (queryPesquisada.Length >= 2)
            {
                var objFuncaoNaoEncontrada = new Models.FuncaoNaoEncontrada
                {
                    DescricaoConteudoBuscado = queryPesquisada,
                    DescricaoOrigem = origem
                };
                _funcaoNaoEncontradaRepository.Add(objFuncaoNaoEncontrada);
            }
            return true;
        }
        #endregion

    }
}