using BNE.Global.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Global.Domain
{
    public class Deficiencia
    {

        private readonly IDeficienciaRepository _deficienciaRepository;

		public Deficiencia(IDeficienciaRepository deficienciaRepository)
		{
            _deficienciaRepository = deficienciaRepository;
		}

		public Model.DeficienciaGlobal GetById(int id)
		{
            return _deficienciaRepository.GetById(id);
		}
    }
}