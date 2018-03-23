using BNE.Mensagem.Data.Repositories;

namespace BNE.Mensagem.Domain
{
    public class Status
    {

        private readonly IStatusRepository _statusRepository;

        public Status(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public Model.Status RecuperarStatus(Enumeradores.Status status)
        {
            return _statusRepository.Get(n => n.Id == (int)status);
        }

    }
}
