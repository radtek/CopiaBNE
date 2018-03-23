using System;
using BNE.PessoaFisica.ApplicationService.Parametro.Interface;
using BNE.PessoaFisica.Domain.Repositories;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.Parametro
{
    public class ParametroApplicationService : SharedKernel.ApplicationService.ApplicationService,
        IParametroApplicationService
    {
        private readonly IParametroRepository _parametroRepository;

        public ParametroApplicationService(IParametroRepository parametroRepository,
            IUnitOfWork unitOfWork,
            EventPoolHandler<AssertError> assertEventPool,
            IBus bus) : base(unitOfWork, assertEventPool, bus)
        {
            _parametroRepository = parametroRepository;
        }

        public decimal RecuperarValorMinimoNacional()
        {
            var objParametro = _parametroRepository.GetById((int) Domain.Enumeradores.Parametro.SalarioMinimoNacional);
            if (objParametro != null)
                return Convert.ToDecimal(objParametro.Valor);
            return decimal.Zero;
        }
    }
}