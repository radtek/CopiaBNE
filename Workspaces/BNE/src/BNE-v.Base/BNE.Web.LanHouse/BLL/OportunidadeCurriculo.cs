using System;
using System.Linq;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.BLL
{
    public static class OportunidadeCurriculo
    {
        private static LanEntities entity = new LanEntities();

        public static int QuantidadeCandidaturas(int idPessoaFisica)
        {
            return entity.LAN_Oportunidade_Curriculo.Include("BNE_Curriculo").Count(oc => oc.BNE_Curriculo.Idf_Pessoa_Fisica == idPessoaFisica);
        }

        public static bool Salvar(int idPessoaFisica, decimal cnpjOportunidade, int idFilial, Enumeradores.FaseCadastro faseCadastro)
        {
            bool retorno = false;

            LAN_Oportunidade_Curriculo objOportunidadeCurriculo =
                entity
                    .LAN_Oportunidade_Curriculo
                    .Include("BNE_Curriculo")
                    .Include("LAN_Oportunidade")
                    .Include("LAN_Oportunidade.LAN_Companhia")
                    .FirstOrDefault(oc =>
                        oc.BNE_Curriculo.Idf_Pessoa_Fisica == idPessoaFisica &&
                        (oc.LAN_Oportunidade.LAN_Companhia.Num_CNPJ == cnpjOportunidade || oc.LAN_Oportunidade.LAN_Companhia.TAB_Filial.Num_CNPJ == cnpjOportunidade) &&
                        oc.Idf_Filial == idFilial);

            if (objOportunidadeCurriculo != null)
            {
                objOportunidadeCurriculo.Idf_Fase_Cadastro = (byte)faseCadastro;
                entity
                    .Entry(objOportunidadeCurriculo)
                    .Property(oc => oc.Idf_Fase_Cadastro).IsModified = true;

                if (entity.SaveChanges() == 1)
                    retorno = true;
            }
            else
            {
                var objCurriculo = entity.BNE_Curriculo.FirstOrDefault(c => c.Idf_Pessoa_Fisica == idPessoaFisica);

                var objCompanhia = entity.LAN_Companhia.FirstOrDefault(c => c.Num_CNPJ == cnpjOportunidade || c.TAB_Filial.Num_CNPJ == cnpjOportunidade);

                if (objCurriculo != null && objCompanhia != null)
                {
                    int idCurriculo = objCurriculo.Idf_Curriculo;
                    int idCompanhia = objCompanhia.Idf_Companhia;

                    objOportunidadeCurriculo = new LAN_Oportunidade_Curriculo
                    {
                        Idf_Filial = idFilial,
                        Idf_Curriculo = idCurriculo,
                        Idf_Companhia = idCompanhia,
                        Idf_Fase_Cadastro = (byte)faseCadastro,
                        Dta_Cadastro = DateTime.Now,
                        Flg_Inativo = false
                    };

                    entity.LAN_Oportunidade_Curriculo.Add(objOportunidadeCurriculo);

                    if (entity.SaveChanges() == 1 && OportunidadeCurriculoHistorico.Salvar(idPessoaFisica, cnpjOportunidade, idFilial))
                        retorno = true;
                }
            }

            return retorno;
        }
    }
}