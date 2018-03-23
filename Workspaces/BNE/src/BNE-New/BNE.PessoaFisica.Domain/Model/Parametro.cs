using BNE.Comum.Model;
using BNE.PessoaFisica.Domain.Repositories;

namespace BNE.PessoaFisica.Domain.Model
{
    public class Parametro : ParametroComum
    {
        private readonly IParametroRepository _parametroRepository;

        internal Parametro()
        {
            //EF
        }

        public Parametro(IParametroRepository parametroRepository)
        {
            _parametroRepository = parametroRepository;
        }

        public Parametro GetById(Enumeradores.Parametro enumerador)
        {
            return _parametroRepository.GetById((int) enumerador);
        }

        public string RecuperarValor(Enumeradores.Parametro enumerador)
        {
            return _parametroRepository.GetById((int) enumerador).Valor;
        }
    }
}