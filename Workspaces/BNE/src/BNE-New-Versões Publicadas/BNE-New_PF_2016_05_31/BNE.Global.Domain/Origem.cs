using BNE.Global.Data.Repositories;

namespace BNE.Global.Domain
{
	public class Origem
	{
		private readonly IOrigemRepository _origemRepository;

		public Origem(IOrigemRepository origemRepository)
		{
			_origemRepository = origemRepository;
		}

		public Model.OrigemGlobal GetById(int id)
		{
			return _origemRepository.GetById(id);
		}
		
	}
}