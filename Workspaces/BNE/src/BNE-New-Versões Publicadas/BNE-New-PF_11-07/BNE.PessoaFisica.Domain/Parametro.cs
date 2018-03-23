using BNE.PessoaFisica.Data.Repositories;

namespace BNE.PessoaFisica.Domain
{
    public class Parametro
    {
        private readonly IParametroRepository _parametroRepository;

        public Parametro(IParametroRepository parametroRepository)
        {
            _parametroRepository = parametroRepository;
        }

        public Model.Parametro GetById(Model.Enumeradores.Parametro enumerador)
        {
            return _parametroRepository.GetById((int)enumerador);
        }

        public string RecuperarValor(Model.Enumeradores.Parametro enumerador)
        {
            return _parametroRepository.GetById((int)enumerador).Valor;
        }
    }
}
