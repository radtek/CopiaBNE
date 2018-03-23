using BNE.Mensagem.Data.Repositories;

namespace BNE.Mensagem.Domain
{
    public class Sistema
    {

        private readonly ISistemaRepository _sistemaRepository;

        public Sistema(ISistemaRepository sistemaRepository)
        {
            _sistemaRepository = sistemaRepository;
        }

        public Model.Sistema RecuperarSistema(string sistema)
        {
            return _sistemaRepository.Get(n => n.Nome == sistema);
        }

    }
}
