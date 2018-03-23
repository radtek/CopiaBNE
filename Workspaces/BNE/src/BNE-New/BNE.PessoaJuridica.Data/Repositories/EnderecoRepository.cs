using System.Data.Entity;
using BNE.PessoaJuridica.Domain.Model;
using BNE.PessoaJuridica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaJuridica.Data.Repositories
{
    public class EnderecoRepository : BaseRepository<Endereco, DbContext>, IEnderecoRepository
    {
        public EnderecoRepository(DbContext dataContext) : base(dataContext)
        {
        }
    }
}