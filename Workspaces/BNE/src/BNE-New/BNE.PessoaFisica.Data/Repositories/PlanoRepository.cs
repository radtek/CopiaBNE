using System.Data.Entity;
using BNE.PessoaFisica.Domain.Command;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class PlanoRepository : BaseRepository<Plano, DbContext>, IPlanoRepository
    {
        public PlanoRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Plano GetPlano(GetPlanoCommand command)
        {
            var plano = new Mapper.ToOld.PessoaFisica().CarregarPlanoPremium(command.CPF);
            var planoR = new Plano {PrecoCandidatura = plano.PrecoCandidatura, PrecoVip = plano.PrecoVip};

            return planoR;
        }
    }
}