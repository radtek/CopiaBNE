using System.Collections.Generic;
using BNE.PessoaFisica.Domain.Command;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface IInstituicaoEnsinoRepository : IBaseRepository<Model.InstituicaoEnsino>
    {
        IEnumerable<Model.InstituicaoEnsino> GetList(GetInstituicaoEnsinoCommand map);
        Model.InstituicaoEnsino GetByNome(string nome);
    }
}