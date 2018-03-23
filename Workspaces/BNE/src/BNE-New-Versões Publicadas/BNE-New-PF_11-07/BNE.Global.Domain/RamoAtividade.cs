using BNE.Data.Infrastructure;
using BNE.Global.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Global.Domain
{
    public class RamoAtividade
    {
        private readonly IRamoAtividadeRepository _ramoAtividadeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RamoAtividade(IRamoAtividadeRepository ramoAtividadeRepository, IUnitOfWork unitOfWork)
        {
            _ramoAtividadeRepository = ramoAtividadeRepository;
            _unitOfWork = unitOfWork;
        }

        public Model.RamoAtividadeGlobal GetById(int id)
        {
            return _ramoAtividadeRepository.GetById(id);
        }

        public bool Salvar(Model.RamoAtividadeGlobal ramoAtividade)
        {
            try
            {
                _ramoAtividadeRepository.Add(ramoAtividade);

                return true;
            }
            catch (System.Exception ex)
            {
                //_logger.Error(ex, "Erro ao persistir Experiência profissional");
                return false;
            }
        }

        #region ListaRamoAtividades
        public List<string> ListaRamoAtividades(string nome)
        {
            var q = _ramoAtividadeRepository.GetAll();
            q.OrderBy(n => n.Descricao);
            return q.Select(p=>p.Descricao).ToList();
        }
        #endregion
    }
}
