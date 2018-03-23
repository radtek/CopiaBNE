using BNE.ExceptionLog.Interface;
using BNE.PessoaFisica.Data.Repositories;
using System;
using System.Linq;

namespace BNE.PessoaFisica.Domain
{
    public class CurriculoParametro
    {
        private readonly ICurriculoParametroRepository _curriculoParametroRepository;
        private readonly ILogger _logger;

        public CurriculoParametro(ICurriculoParametroRepository curriculoParametroRepository, ILogger logger)
        {
            _curriculoParametroRepository = curriculoParametroRepository;
            _logger = logger;
        }

        public Model.CurriculoParametro GetById(int id)
        {
            return _curriculoParametroRepository.GetById(id);
        }

        public Model.CurriculoParametro GetByIdCurriculo(int idCurriculo)
        {
            return _curriculoParametroRepository.GetMany(p=>p.Curriculo.Id == idCurriculo).FirstOrDefault();
        }

        public bool Salvar(Model.CurriculoParametro curriculoParametro)
        {
            try
            {
                _curriculoParametroRepository.Add(curriculoParametro);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao salvar Curriculo parametro");
                throw;
            }
        }

        public bool Atualizar(Model.CurriculoParametro curriculoParametro)
        {
            try
            {
                _curriculoParametroRepository.Update(curriculoParametro);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao atualizar Curriculo parametro");
                throw;
            }
        }
    }
}