using System.Linq;
using System.Threading.Tasks;
using BNE.PessoaFisica.ApplicationService.NavegacaoVaga.Command;
using BNE.PessoaFisica.ApplicationService.NavegacaoVaga.Interface;
using BNE.PessoaFisica.ApplicationService.NavegacaoVaga.Model;
using BNE.PessoaFisica.Domain.Services.SOLR;

namespace BNE.PessoaFisica.ApplicationService.NavegacaoVaga
{
    public class JobNavigationApplicationService : IJobNavigationApplicationService
    {
        private readonly ISOLRService _solrService;

        public JobNavigationApplicationService(ISOLRService solrService)
        {
            _solrService = solrService;
        }

        public async Task<JobNavigationResponse> GetNavigation(JobNavigationCommand command)
        {
            var isJobSearch = command.JobSearch.HasValue && command.JobIndex.HasValue;
            var navigation = isJobSearch ? await _solrService.GetNavigation(command.SourceJob, command.JobSearch.Value, command.JobIndex.Value) : await _solrService.GetNavigation(command.SourceJob);

            if (navigation != null && navigation.Any(v => v.Id == command.Job))
            {
                var jobIndex = navigation.FindIndex(c => c.Id == command.Job);
                int previousJob, nextJob;

                var previousJobUrl = string.Empty;
                var nextJobUrl = string.Empty;

                if (jobIndex == -1)
                {
                    previousJob = -1;
                    nextJob = 0;
                }
                else
                {
                    previousJob = jobIndex - 1;
                    nextJob = jobIndex + 1;
                }

                if (previousJob >= 0)
                {
                    previousJobUrl = navigation[previousJob].Url;

                    if (isJobSearch)
                    {
                        previousJobUrl += $"?jobindex={command.JobIndex.Value - 1}";
                    }
                    else
                    {
                        previousJobUrl += $"?sourcejob={command.SourceJob}";
                    }
                }

                if (nextJob >= 0 && nextJob < navigation.Count)
                {
                    nextJobUrl = navigation[nextJob].Url;

                    if (isJobSearch)
                    {
                        nextJobUrl += $"?jobindex={command.JobIndex.Value + 1}";
                    }
                    else
                    {
                        nextJobUrl += $"?sourcejob={command.SourceJob}";
                    }
                }

                return await Task.FromResult(new JobNavigationResponse
                {
                    URLVagaAnterior = previousJobUrl,
                    URLVagaProxima = nextJobUrl
                });
            }
            return await Task.FromResult(new JobNavigationResponse());
        }
    }
}