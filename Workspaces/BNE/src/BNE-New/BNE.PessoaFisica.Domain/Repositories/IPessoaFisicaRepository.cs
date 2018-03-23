using System;
using SharedKernel.Repositories.Contracts;
using BNE.PessoaFisica.Domain.Command;

namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface IPessoaFisicaRepository : IBaseRepository<Model.PessoaFisica>
    {
        Domain.Model.PessoaFisica Get(decimal cpf, DateTime data);
        string GetNomeJaExiste(decimal cpf, DateTime dataNascimento);
        bool Inativa(decimal cpf);
        bool IndicarAmigos(SalvarIndicarAmigoCommand objIndicacaoAmigo);
    }
}