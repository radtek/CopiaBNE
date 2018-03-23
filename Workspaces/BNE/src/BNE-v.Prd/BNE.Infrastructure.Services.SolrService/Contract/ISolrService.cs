using System.Collections.Generic;
using System.Threading.Tasks;
using BNE.Infrastructure.Services.SolrService.Model;

namespace BNE.Infrastructure.Services.SolrService.Contract
{
    public interface ISolrService
    {
        Task<Vaga> GetJobById(int idVaga);
        Task<List<Vaga>> GetJobsById(List<string> vagas);
        Task<List<FacetJornal>> GetFacetByJobAndCity(List<Vaga> vagas);
        Task<List<FacetJornal>> GetFacetByJobAndArea(List<Vaga> vagas);
    }
}