using BNE.PessoaFisica.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain
{
	public class Curso
	{
		private readonly ICursoRepository _cursoRepository;

		public Curso(ICursoRepository cursoRepository)
		{
			_cursoRepository = cursoRepository;
		}

		#region ListaSugestaoCurso
		public IEnumerable<string> ListaSugestaoCurso(string nome, int limiteRegistros)
		{
			var q = _cursoRepository.GetMany(x => x.Descricao.ToLower().StartsWith(nome) && x.Ativo);
			q.OrderBy(n => n.Descricao);
			q.Distinct().Take(limiteRegistros);
			return q.Select(e => e.Descricao);
		}
		#endregion

		#region GetByName
		public Model.Curso GetByName(string descricao)
		{
			return _cursoRepository.Get(p => p.Descricao.ToLower() == descricao.ToLower());
		}
		#endregion
	}
}