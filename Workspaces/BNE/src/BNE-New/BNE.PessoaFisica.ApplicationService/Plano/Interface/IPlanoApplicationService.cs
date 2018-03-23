using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BNE.PessoaFisica.ApplicationService.Plano.Command;

namespace BNE.PessoaFisica.ApplicationService.Plano.Interface
{
    public interface IPlanoApplicationService
    {
        Model.PlanoResponse GetPlano(GetPlanoCommand command);
    }
}
