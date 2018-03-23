using BNE.PessoaJuridica.Data.Repositories;

namespace BNE.PessoaJuridica.Domain
{
    public class Parametro
    {

        private readonly IParametroRepository _parametroRepository;

        public Parametro(IParametroRepository parametroRepository)
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
