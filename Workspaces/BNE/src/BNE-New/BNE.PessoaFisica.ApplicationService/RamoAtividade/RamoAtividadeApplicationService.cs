using System.Collections.Generic;
using BNE.PessoaFisica.ApplicationService.RamoAtividade.Command;
using BNE.PessoaFisica.ApplicationService.RamoAtividade.Interface;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.RamoAtividade
{
    public class RamoAtividadeApplicationService : SharedKernel.ApplicationService.ApplicationService,
        IRamoAtividadeApplicationService
    {
        private readonly Global.Domain.RamoAtividade _ramoAtividadeGlobal;

        public RamoAtividadeApplicationService(Global.Domain.RamoAtividade ramoAtividadeGlobal, IUnitOfWork unitOfWork,
            EventPoolHandler<AssertError> assertEventPool, IBus bus) : base(unitOfWork, assertEventPool, bus)
        {
            _ramoAtividadeGlobal = ramoAtividadeGlobal;
        }

        public IEnumerable<string> GetList(GetRamoAtividadeCommand command)
        {
            return _ramoAtividadeGlobal.ListaRamoAtividades(command.Query);
        }
    }
}