using BNE.PessoaFisica.Data.Repositories;
using System.Linq;

namespace BNE.PessoaFisica.Domain
{
    public class Curriculo
    {
        private readonly ICurriculoRepository _curriculoRepository;

        public Curriculo(ICurriculoRepository curriculoRepository)
        {
            _curriculoRepository = curriculoRepository;
        }

        public Model.Curriculo GetById(int id)
        {
            return _curriculoRepository.GetById(id);
        }

        public Model.Curriculo GetByIdPessoaFisica(int idPessoaFisica)
        {
            return _curriculoRepository.GetMany(p => p.PessoaFisica.Id == idPessoaFisica).SingleOrDefault();
        }


        /// <summary>
        /// Validar se o Curriculo está ativo
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <returns></returns>
        public bool CurriculoAtivo(int idPessoaFisica)
        {
            var curriculo = _curriculoRepository.GetMany(p => p.PessoaFisica.Id == idPessoaFisica).SingleOrDefault();

            if (curriculo != null)
            {
                return curriculo.SituacaoCurriculo.Id == (int)(Enumeradores.SituacaoCurriculo.Bloqueado) ? false : 
                    curriculo.SituacaoCurriculo.Id == (int)(Enumeradores.SituacaoCurriculo.Cancelado) ? false : true;
            }
            return false;
        }

        public bool SalvarMiniCurriculo(Model.Curriculo curriculo)
        {
            try
            {
                _curriculoRepository.Add(curriculo);

                return true;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }
}