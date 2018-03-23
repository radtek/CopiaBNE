using BNE.Global.Data.Repositories;

namespace BNE.Global.Domain
{
    public class FuncaoSinonimo
    {

        private readonly IFuncaoSinonimoRepository _funcaoSinonimoRepository;

        public FuncaoSinonimo(IFuncaoSinonimoRepository funcaoSinonimoRepository)
        {
            _funcaoSinonimoRepository = funcaoSinonimoRepository;
        }

        public Model.FuncaoSinonimo GetByNome(string nome)
        {
            return _funcaoSinonimoRepository.Get(n => n.NomeSinonimo == nome);
        }

        public Model.FuncaoSinonimo GetByIdSubstituto(int id)
        {
            return _funcaoSinonimoRepository.Get(n => n.IdSinonimoSubstituto == id);
        }

    }
}
