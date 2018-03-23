using System.Collections;
using System.Collections.Generic;
using BNE.PessoaFisica.ApplicationService.RamoAtividade.Command;

namespace BNE.PessoaFisica.ApplicationService.RamoAtividade.Interface
{
    public interface IRamoAtividadeApplicationService
    {
        IEnumerable<string> GetList(GetRamoAtividadeCommand command);
    }
}