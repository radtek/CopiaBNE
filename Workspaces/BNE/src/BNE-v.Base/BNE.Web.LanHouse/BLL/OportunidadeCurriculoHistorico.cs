using System;
using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class OportunidadeCurriculoHistorico
    {
        private static LanEntities entity = new LanEntities();

        public static bool Salvar(int idPessoaFisica, decimal cnpjOportunidade, int idFilial)
        {
            bool retorno = false;

            var objCurriculo = entity.BNE_Curriculo.FirstOrDefault(c => c.Idf_Pessoa_Fisica == idPessoaFisica);

            var objCompanhia = entity.LAN_Companhia.FirstOrDefault(c => c.Num_CNPJ == cnpjOportunidade || c.TAB_Filial.Num_CNPJ == cnpjOportunidade);

            if (objCurriculo != null && objCompanhia != null)
            {
                int idCurriculo = objCurriculo.Idf_Curriculo;
                int idCompanhia = objCompanhia.Idf_Companhia;

                var obj = new LAN_Oportunidade_Curriculo_Historico
                {
                    Idf_Filial = idFilial,
                    Idf_Curriculo = idCurriculo,
                    Idf_Companhia = idCompanhia,
                    Dta_Cadastro = DateTime.Now
                };

                entity
                    .LAN_Oportunidade_Curriculo_Historico
                    .Add(obj);

                if (entity.SaveChanges() == 1)
                    retorno = true;
            }

            return retorno;
        }
    }
}