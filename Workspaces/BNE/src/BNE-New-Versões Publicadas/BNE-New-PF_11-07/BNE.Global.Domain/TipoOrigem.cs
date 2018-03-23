using BNE.Global.Data.Repositories;

namespace BNE.Global.Domain
{
    public class TipoOrigem
    {
        private readonly ITipoOrigemRepository _tipoOrigemRepository;

        public TipoOrigem(ITipoOrigemRepository curriculoOrigemRepository)
        {
            _tipoOrigemRepository = curriculoOrigemRepository;
        }

        public Model.TipoOrigemGlobal GetById(byte id)
        {
            return _tipoOrigemRepository.GetById(id);
        }
    }
}