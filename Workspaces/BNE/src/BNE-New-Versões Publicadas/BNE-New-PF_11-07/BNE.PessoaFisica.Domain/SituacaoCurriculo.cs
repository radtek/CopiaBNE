using BNE.PessoaFisica.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain
{
    public class SituacaoCurriculo
    {

        private readonly ISituacaoCurriculoRepository _situacaoCurriculoRepository;

        public SituacaoCurriculo(ISituacaoCurriculoRepository situacaoCurriculoRepository)
        {
            _situacaoCurriculoRepository = situacaoCurriculoRepository;
        }

        public Model.SituacaoCurriculo GetById(int id)
        {
            return _situacaoCurriculoRepository.GetById(id);
        }
    }
}		