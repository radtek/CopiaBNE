using BNE.Data.Infrastructure;
using BNE.ExceptionLog.Interface;
using BNE.PessoaFisica.Data.Repositories;
using System.Linq;

namespace BNE.PessoaFisica.Domain
{
    public class Formacao
    {
        private readonly IFormacaoRepository _formacaoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public Formacao(IFormacaoRepository formacao, ILogger logger)
        {
            _formacaoRepository = formacao;
            _logger = logger;
        }

        public Model.Formacao GetById(int id)
        {
            return _formacaoRepository.GetById(id);
        }

        /// <summary>
        /// Retornar o numero de Formações de uma pessoa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetByIdPessoa(int idPessoa)
        {
            return _formacaoRepository.GetMany(p => p.PessoaFisica.Id == idPessoa).Count();
        }

        public bool Salvar(Model.Formacao formacao)
        {
            try
            {
                _formacaoRepository.Add(formacao);

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "Erro ao persistir Formação");
                return false;
            }
        }
    }
}