using BNE.Data.Infrastructure;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain
{
    public class ExperienciaProfissional
    {
        private readonly IExperienciaProfissionalRepository _experienciaProfissionalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ExperienciaProfissional(IExperienciaProfissionalRepository experienciaProfissionalRepository, ILogger logger)
        {
            _experienciaProfissionalRepository = experienciaProfissionalRepository;
            _logger = logger;
        }

        public Model.ExperienciaProfissional GetById(int id)
        {
            return _experienciaProfissionalRepository.GetById(id);
        }

        /// <summary>
        /// Retornar o numero de experiências Profissionais de uma pessoa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetByIdPessoa(int idPessoa)
        {
            return _experienciaProfissionalRepository.GetMany(p=>p.PessoaFisica.Id == idPessoa).Count();
        }

        public bool Salvar(Model.ExperienciaProfissional experienciaProfissional)
        {
            try
            {
                _experienciaProfissionalRepository.Add(experienciaProfissional);

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex,"Erro ao persistir Experiência profissional");
                return false;                
            }
        }
    }
}