using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BNE.Mapper.Models.Indicacao;
using BNE.PessoaFisica.Domain.Command;
using BNE.PessoaFisica.Domain.Repositories;
using CrossCutting.Infrastructure.Repository;

namespace BNE.PessoaFisica.Data.Repositories
{
    public class PessoaFisicaRepository : BaseRepository<Domain.Model.PessoaFisica, DbContext>, IPessoaFisicaRepository
    {
        private readonly IMapper _mapper;

        public PessoaFisicaRepository(IMapper mapper, DbContext dataContext) : base(dataContext)
        {
            _mapper = mapper;
        }

        public Domain.Model.PessoaFisica Get(decimal cpf, DateTime data)
        {
            return GetMany(p => p.CPF == cpf && p.DataNascimento == data.Date).FirstOrDefault();
        }

        public string GetNomeJaExiste(decimal cpf, DateTime dataNascimento)
        {
            var nomePessoa = GetMany(p => p.CPF == cpf && p.DataNascimento == dataNascimento.Date).Select(x => x.Nome).FirstOrDefault();

            //Checa no BNE Velho
            if (string.IsNullOrWhiteSpace(nomePessoa))
                nomePessoa = new Mapper.ToOld.PessoaFisica().RecuperarNomePessoaPorCPFDataNascimento(cpf, dataNascimento);

            return nomePessoa;
        }

        public bool Inativa(decimal cpf)
        {
            return new Mapper.ToOld.PessoaFisica().PessoaFisicaInativa(cpf);
        }

        public bool IndicarAmigos(SalvarIndicarAmigoCommand objIndicarAmigoIndicadoCommand)
        {
            var command = _mapper.Map<SalvarIndicarAmigoCommand, Indicacao>(objIndicarAmigoIndicadoCommand);

            return new Mapper.ToOld.PessoaFisica().IndicarAmigos(command);
        }

        public virtual IQueryable<Domain.Model.PessoaFisica> GetMany(Expression<Func<Domain.Model.PessoaFisica, bool>> where, params Expression<Func<Domain.Model.PessoaFisica, object>>[] includes)
        {
            var query = DbSet.Where(where).AsQueryable();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

    }
}