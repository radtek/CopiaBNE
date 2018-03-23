using BNE.Data.Infrastructure;
using BNE.Global.Data.Repositories;
using BNE.PessoaFisica.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BNE.PessoaFisica.Domain
{
	public class RankingEmail
	{
		private readonly IRankingEmailRepository _rankingEmailRepository;

		public RankingEmail(IRankingEmailRepository RankingEmailRepository)
		{
			_rankingEmailRepository = RankingEmailRepository;
		}

		#region ListarTodos
		/// <summary>
		/// Listar sugestão de email ao digitar @ no campo email
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> ListarTodos(string email, int limiteRegistros)
		{
			if (email == null || !email.Contains("@"))
				return null;

			string sulfixo = email.Substring(email.IndexOf('@'));
			email = email.Replace(sulfixo, string.Empty);

			var q = _rankingEmailRepository.GetMany(x => x.DescricaoEmail.ToLower().StartsWith(sulfixo));
			q.OrderBy(n => n.DescricaoEmail);
			q.Distinct().Take(limiteRegistros);
			return q.Select(e => email + e.DescricaoEmail);
		}
		#endregion
	}
}