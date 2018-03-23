using BNE.PessoaFisica.Data.Repositories;

namespace BNE.PessoaFisica.Domain
{
    public class TipoCurriculo
    {
        private readonly ITipoCurriculoRepository _tipoCurriculoRepository;

        public TipoCurriculo(ITipoCurriculoRepository tipoCurriculoRepository)
        {
            _tipoCurriculoRepository = tipoCurriculoRepository;
        }

        public Model.TipoCurriculo GetById(int id)
        {
            return _tipoCurriculoRepository.GetById(id);
        }
    }
}
