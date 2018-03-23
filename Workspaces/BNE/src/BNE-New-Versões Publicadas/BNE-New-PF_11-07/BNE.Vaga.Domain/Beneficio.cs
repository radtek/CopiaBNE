using BNE.Vaga.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Vaga.Domain
{
    public class Beneficio
    {
        private readonly IBeneficioRepository _beneficioRepository;

        public Beneficio(IBeneficioRepository curriculoRepository)
        {
            _beneficioRepository = curriculoRepository;
        }

        public Model.Beneficio GetById(int id)
        {
            return _beneficioRepository.GetById(id);
        }
    }
}
