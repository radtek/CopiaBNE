using BNE.PessoaFisica.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain
{
    public class InstituicaoEnsino
    {
        private readonly IInstituicaoEnsinoRepository _instituicaoEnsinoRepository;

        public InstituicaoEnsino(IInstituicaoEnsinoRepository instituicaoEnsinoRepository)
        {
            _instituicaoEnsinoRepository = instituicaoEnsinoRepository;
        }

        #region ListaSugestaoInstituicaoEnsino
        public IEnumerable<string> ListaSugestaoInstituicaoEnsino(string nome, int limiteRegistros)
        {
            var q = _instituicaoEnsinoRepository.GetMany(x => x.Nome.ToLower().StartsWith(nome) || x.Sigla.StartsWith(nome));
            q.OrderBy(n => n.Nome);
            q.Distinct().Take(limiteRegistros);
            return q.Select(e => e.Nome);
        }
        #endregion

        #region GetByName
        public Model.InstituicaoEnsino GetByName(string nome)
        {
            return _instituicaoEnsinoRepository.Get(p => p.Nome.ToLower() == nome.ToLower());
        }
        #endregion
    }
}