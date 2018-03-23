using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class Curriculo
    {
        private static LanEntities entity = new LanEntities();

        public static bool CarregarPorPessoaFisica(int idPessoaFisica, out BNE_Curriculo objCurriculo)
        {
            entity = new LanEntities();

            objCurriculo =
                entity
                    .BNE_Curriculo
                    .Include("BNE_Funcao_Pretendida")
                    .Include("TAB_Pessoa_Fisica")
                    .Include("TAB_Cidade_Endereco")
                    .FirstOrDefault(c => c.Idf_Pessoa_Fisica == idPessoaFisica);

            return objCurriculo != null;
        }
    }
}