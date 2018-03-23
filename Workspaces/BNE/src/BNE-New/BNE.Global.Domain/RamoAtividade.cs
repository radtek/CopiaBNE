using System;
using System.Collections.Generic;
using System.Linq;
using BNE.Global.Data.Repositories;
using BNE.Global.Model;

namespace BNE.Global.Domain
{
    public class RamoAtividade
    {
        private readonly IRamoAtividadeRepository _ramoAtividadeRepository;

        public RamoAtividade(IRamoAtividadeRepository ramoAtividadeRepository)
        {
            _ramoAtividadeRepository = ramoAtividadeRepository;
        }

        public RamoAtividadeGlobal GetById(int id)
        {
            return _ramoAtividadeRepository.GetById(id);
        }

        public bool Salvar(RamoAtividadeGlobal ramoAtividade)
        {
            try
            {
                _ramoAtividadeRepository.Add(ramoAtividade);

                return true;
            }
            catch (Exception ex)
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
            return q.Select(p => p.Descricao).ToList();
        }

        #endregion
    }
}