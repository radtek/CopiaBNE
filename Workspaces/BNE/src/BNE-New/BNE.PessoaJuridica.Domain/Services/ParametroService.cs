using BNE.PessoaJuridica.Domain.Repositories;

namespace BNE.PessoaJuridica.Domain.Services
{
    public class ParametroService
    {

        private readonly IParametroRepository _parametroRepository;

        public ParametroService(IParametroRepository parametroRepository)
        {
            _parametroRepository = parametroRepository;
        }

        private Model.Parametro GetById(Model.Enumeradores.Parametro enumerador)
        {
            return _parametroRepository.GetById((int)enumerador);
        }

        public string RecuperarValor(Model.Enumeradores.Parametro enumerador)
        {
            return _parametroRepository.GetById((int)enumerador).Valor;
        }

    }
}
