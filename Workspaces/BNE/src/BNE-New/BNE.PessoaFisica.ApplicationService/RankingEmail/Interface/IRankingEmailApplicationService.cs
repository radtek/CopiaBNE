using System.Collections.Generic;
using BNE.PessoaFisica.ApplicationService.RankingEmail.Command;

namespace BNE.PessoaFisica.ApplicationService.RankingEmail.Interface
{
    public interface IRankingEmailApplicationService
    {
        IEnumerable<string> GetList(GetRankingEmailCommand command);
    }
}