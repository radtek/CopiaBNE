using BNE.PessoaJuridica.Data.Repositories;

namespace BNE.PessoaJuridica.Domain
{
    public class Perfil
    {

        private readonly IPerfilRepository _perfilRepository;

        public Perfil(IPerfilRepository perfilRepository)
        {
            _perfilRepository = perfilRepository;
        }

        public Model.Perfil GetById(int idPerfil)
        {
            return _perfilRepository.GetById(idPerfil);
        }

    }
}
