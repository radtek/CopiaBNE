using BNE.PessoaFisica.ApplicationService.CodigoConfirmacaoEmail.Command;
using BNE.PessoaFisica.ApplicationService.CodigoConfirmacaoEmail.Interface;
using BNE.PessoaFisica.Domain.Repositories;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.CodigoConfirmacaoEmail
{
    public class CodigoConfirmacaoEmailApplicationService : SharedKernel.ApplicationService.ApplicationService,
        ICodigoConfirmacaoEmailApplicationService
    {
        private readonly ICodigoConfirmacaoEmailRepository _codigoConfirmacaoEmailRepository;

        public CodigoConfirmacaoEmailApplicationService(
            ICodigoConfirmacaoEmailRepository codigoConfirmacaoEmailRepository,
            IUnitOfWork unitOfWork,
            EventPoolHandler<AssertError> assertEventPool,
            IBus bus) : base(unitOfWork, assertEventPool, bus)
        {
            _codigoConfirmacaoEmailRepository = codigoConfirmacaoEmailRepository;
        }

        public string GetByCodigo(GetByCodigoCommand command)
        {
            var objCodigo = _codigoConfirmacaoEmailRepository.GetByCodigo(command.Codigo);

            if (objCodigo != null)
            {
                objCodigo.Utilizar();
                _codigoConfirmacaoEmailRepository.Update(objCodigo);

                if (Commit())
                    return objCodigo.Codigo;
            }
            return string.Empty;
        }

        public string GerarCodigoConfirmacao(string email)
        {
            var objCodigo = new Domain.Model.CodigoConfirmacaoEmail(email);
            _codigoConfirmacaoEmailRepository.Add(objCodigo);
            if (Commit())
                return objCodigo.Codigo;
            return string.Empty;
        }
    }
}