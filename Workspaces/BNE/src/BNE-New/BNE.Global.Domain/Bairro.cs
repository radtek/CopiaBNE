using BNE.Global.Data.Repositories;

namespace BNE.Global.Domain
{
    public class Bairro
    {
        private readonly IBairroRepository _bairroRepository;

        public Bairro(IBairroRepository bairroRepository)
        {
            _bairroRepository = bairroRepository;
        }

        public Model.Bairro GetByNome(Model.Cidade objCidade, string nome)
        {
            return _bairroRepository.Get(n => n.Nome == nome && n.Cidade.Id == objCidade.Id);
        }

    }
}
