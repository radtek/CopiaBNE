using BNE.Global.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BNE.Global.Domain
{
    public class Escolaridade
    {
        private readonly IEscolaridadeRepository _escolaridadeRepository;

        public Escolaridade(IEscolaridadeRepository escolaridadeRepository)
        {
            _escolaridadeRepository = escolaridadeRepository;
        }

        public Model.EscolaridadeGlobal GetById(int id)
        {
            return _escolaridadeRepository.GetById(id);
        }

        #region ListaEscolaridades
        public List<Model.EscolaridadeGlobal> ListaEscolaridades(string nome, int limiteRegistros)
        {
            var q = _escolaridadeRepository.GetMany(x => x.GrauEscolaridade.Id < 5 && x.Id > 1);
            q.OrderBy(n => n.GrauEscolaridade.Id);
            return q.ToList();
        }
        #endregion
    }
}
