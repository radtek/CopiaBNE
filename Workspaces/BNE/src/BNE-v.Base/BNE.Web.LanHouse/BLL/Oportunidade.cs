using BNE.Web.LanHouse.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace BNE.Web.LanHouse.BLL
{
    public static class Oportunidade
    {
        private static readonly LanEntities entity = new LanEntities();

        public static List<BUSCA_EMPRESA_RETORNO> CarregarOportunidades(TAB_Cidade objCidade, string nomeEmpresa, int? idCurriculo, int paginaAtual, int quantidadeRegistros)
        {
            List<BUSCA_EMPRESA_RETORNO> oportunidades = null;
            using (var context = new LanEntities())
            {
                oportunidades = context.BUSCA_EMPRESA(paginaAtual, quantidadeRegistros, objCidade.Idf_Cidade, objCidade.Sig_Estado, nomeEmpresa, idCurriculo).ToList();
            }
            return oportunidades;
        }

        public static bool JaSeCandidatou(decimal cnpjOportunidade, int idFilial, int idPessoaFisica)
        {
            var objCompanhia = entity.LAN_Companhia.FirstOrDefault(c => c.Num_CNPJ == cnpjOportunidade || c.TAB_Filial.Num_CNPJ == cnpjOportunidade);

            if (objCompanhia == null)
                return false;

            var objCurriculo = entity.BNE_Curriculo.FirstOrDefault(c => c.Idf_Pessoa_Fisica == idPessoaFisica);

            if (objCurriculo == null)
                return false;

            int idCurriculo = objCurriculo.Idf_Curriculo;
            int idCompanhia = objCompanhia.Idf_Companhia;

            LAN_Oportunidade_Curriculo objOportunidadeCurriculo =
                entity
                    .LAN_Oportunidade_Curriculo
                    .FirstOrDefault(och =>
                        och.Idf_Companhia == idCompanhia &&
                        och.Idf_Filial == idFilial &&
                        och.Idf_Curriculo == idCurriculo);

            return objOportunidadeCurriculo != null;
        }
    }
}