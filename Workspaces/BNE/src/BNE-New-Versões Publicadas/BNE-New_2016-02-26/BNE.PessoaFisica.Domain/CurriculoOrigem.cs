using BNE.ExceptionLog.Interface;
using BNE.PessoaFisica.Data.Repositories;

namespace BNE.PessoaFisica.Domain
{
	public class CurriculoOrigem
	{
		private readonly ICurriculoOrigemRepository _curriculoOrigemRepository;
		private readonly ILogger _logger;

		public CurriculoOrigem(ICurriculoOrigemRepository curriculoOrigemRepository, ILogger logger)
		{
			_curriculoOrigemRepository = curriculoOrigemRepository;
			_logger = logger;
		}

		public Model.CurriculoOrigem GetById(int id)
		{
			return _curriculoOrigemRepository.GetById(id);
		}

		public bool Salvar(Model.CurriculoOrigem curriculoOrigem)
		{
			try
			{
				_curriculoOrigemRepository.Add(curriculoOrigem);

				return true;
			}
			catch (System.Exception ex)
			{
				_logger.Error(ex, "Erro ao Salvar função Curriculo Origem do candidato");
				throw;
			}
		}
		
	}
}