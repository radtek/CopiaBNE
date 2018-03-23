using BNE.BLL;
using System.Collections.Generic;
using System.Web.Http;

namespace BNE.Dashboard.API.Controllers
{
    public class JobController : ApiController
    {
        [Route("api/job/gettotalprojetosparados")]
        public int GetTotalProjetosParados()
        {
            return BNE.BLL.VagasSINE.ListarTotalProjetosSemRodar();
        }

        /// <summary>
        /// Situação consolidada das origens de importacao
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/job/getstatusprojetos")]
        public List<ProjetosParados> GetStatusProjetos()
        {
            return BNE.BLL.VagasSINE.ListarStatusProjetos();
        }

        [HttpGet]
        [Route("api/job/getprojetosparados")]
        public List<ProjetosParados> getprojetosparados()
        {
            return BNE.BLL.VagasSINE.ListarProjetosSemRodar();
        }

        /// <summary>
        /// Retorna os projetos com problemas, ordenando pelos projetos com mais vagas a mais dias sem rodar
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/job/getprojetosqtdvagas")]
        public List<ProjetosParados> GetProjetosQtdVagas()
        {
            return BNE.BLL.VagasSINE.ListarProjetosQtdVagas();
        }

        [HttpGet]
        [Route("api/job/gettotalvagasimportadas")]
        public List<ProjetosParados> GetTotalVagasImportadas()
        {
            return BNE.BLL.VagasSINE.ListarTotalVagasImportadas();
        }
    }
}
