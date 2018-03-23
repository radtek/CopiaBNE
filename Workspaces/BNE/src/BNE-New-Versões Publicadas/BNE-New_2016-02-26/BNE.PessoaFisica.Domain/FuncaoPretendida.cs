using BNE.Data.Infrastructure;
using BNE.ExceptionLog.Interface;
using BNE.PessoaFisica.Data.Repositories;
using System.Linq;

namespace BNE.PessoaFisica.Domain
{
	public class FuncaoPretendida
	{
		private readonly IFuncaoPretendidaRepository _funcaoPretendidaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

		public FuncaoPretendida(IFuncaoPretendidaRepository funcaoPretendidaRepository, ILogger logger)
		{
			_funcaoPretendidaRepository = funcaoPretendidaRepository;
            _logger = logger;
		}

		public Model.FuncaoPretendida GetById(int id)
		{
			return _funcaoPretendidaRepository.GetById(id);
		}

        public Model.FuncaoPretendida GetByName(string nomeFuncao)
        {
            return _funcaoPretendidaRepository.GetMany(p => p.Descricao.ToLower() == nomeFuncao.ToLower()).FirstOrDefault();
        }

        public bool Salvar(Model.FuncaoPretendida funcaoPretendida)
        {
            try
            {
                _funcaoPretendidaRepository.Add(funcaoPretendida);

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex,"Erro ao Salvar função Pretendida do candidato");
                return false;                
            }
        }
		
	}
}