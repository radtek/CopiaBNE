using System;
using System.Collections.Generic;
using SharedKernel.Repositories.Contracts;
using BNE.PessoaFisica.Domain.Aggregates;
using BNE.PessoaFisica.Domain.Command;

namespace BNE.PessoaFisica.Domain.Repositories
{
    public interface ICurriculoRepository : IBaseRepository<Model.Curriculo>
    {
        InformacaoCurriculo GetInformacaoCurriculo(decimal cpf, int idVaga);
        bool VIP(int idCurriculo);
        int SaldoCandidatura(int idCurriculo);
        int GetIdCurriculo(decimal cpf);
        bool SalvarCandidatura(decimal cpf, int idVaga, bool indicacaoAmigo, List<Tuple<int, bool?, string>> list);
        void MapFormacao(SalvarFormacaoCommand command);
        void MapExperienciaProfissional(SalvarExperienciaProfissionalCommand command, bool salvarExperiencia);
        void MapCurriculoParametro(SalvarExperienciaProfissionalCommand command);
        void Map(SalvarCurriculoCommand command, out bool candidatura);
    }
}