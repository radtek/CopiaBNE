using BNE.PessoaJuridica.Domain.Repositories;

namespace BNE.PessoaJuridica.Domain.Services
{
    public class PerfilService
    {

        private readonly IPerfilRepository _perfilRepository;

        public PerfilService(IPerfilRepository perfilRepository)
        {
            _perfilRepository = perfilRepository;
        }

        public Model.Perfil GetById(int idPerfil)
        {
            return _perfilRepository.GetById(idPerfil);
        }

    }
}
