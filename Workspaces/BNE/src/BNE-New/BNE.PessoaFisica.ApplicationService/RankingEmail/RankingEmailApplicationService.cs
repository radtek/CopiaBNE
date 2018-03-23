using System.Collections.Generic;
using System.Linq;
using BNE.Global.Data.Repositories;
using BNE.PessoaFisica.ApplicationService.RankingEmail.Command;
using BNE.PessoaFisica.ApplicationService.RankingEmail.Interface;
using BNE.PessoaFisica.Domain.Specs;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;

namespace BNE.PessoaFisica.ApplicationService.RankingEmail
{
    public class RankingEmailApplicationService : SharedKernel.ApplicationService.ApplicationService,
        IRankingEmailApplicationService
    {
        private readonly IRankingEmailRepository _rankingEmailRepository;

        public RankingEmailApplicationService(IRankingEmailRepository rankingEmailRepository, IUnitOfWork unitOfWork,
            EventPoolHandler<AssertError> assertEventPool, IBus bus) : base(unitOfWork, assertEventPool, bus)
        {
            _rankingEmailRepository = rankingEmailRepository;
        }

        public IEnumerable<string> GetList(GetRankingEmailCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Query) || !command.Query.Contains("@"))
                return null;

            var sulfixo = command.Query.Substring(command.Query.IndexOf('@'));

            var email = command.Query.Replace(sulfixo, string.Empty);

            return _rankingEmailRepository.GetMany(RankingEmailSpecs.GetList(command.Query, sulfixo))
                .OrderBy(x => x.DescricaoEmail).Distinct().Take(command.Limit).Select(e => email + e.DescricaoEmail);
        }
    }
}