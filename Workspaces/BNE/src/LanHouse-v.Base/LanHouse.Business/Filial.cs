using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE.Repositories;
using LanHouse.Entities.BNE.Infrastructure;
using LanHouse.Entities.BNE;

namespace LanHouse.Business
{
    public class Filial
    {
        #region CarregarPorDiretorio
        public static bool CarregarPorDiretorio(string diretorio, out TAB_Origem_Filial objOrigemFilial)
        {
            using (var entity = new LanEntities())
            {
                objOrigemFilial =
                    entity
                    .TAB_Origem_Filial
                    .Include("Tab_Filial").FirstOrDefault(of => of.Des_Diretorio.Equals(diretorio));

                return objOrigemFilial != null;
            }
        }
        #endregion

        #region RecuperarInfoISS
        public static bool RecuperarInfoISS(int idFilial, out bool flgIss, out string textoPersonalizadoNF, out int idCidade)
        {
            flgIss = false;
            textoPersonalizadoNF = string.Empty;
            bool retorno = false;
            idCidade = 0;

            using (var entity = new LanEntities())
            {
                idCidade = (from filial in entity.TAB_Filial
                            join endereco in entity.TAB_Endereco on filial.Idf_Endereco equals endereco.Idf_Endereco
                            select endereco.Idf_Cidade).FirstOrDefault();

                flgIss = (from parametro in entity.TAB_Parametro_Filial
                          where parametro.Idf_Filial == idFilial
                          && parametro.Idf_Parametro == 347
                          select parametro.Vlr_Parametro).FirstOrDefault() == "1" ? true : false;

                textoPersonalizadoNF = (from parametro in entity.TAB_Parametro_Filial
                                        where parametro.Idf_Filial == idFilial
                                        && parametro.Idf_Parametro == 348
                                        select parametro.Vlr_Parametro).FirstOrDefault() ?? "";


                if (flgIss || !string.IsNullOrEmpty(textoPersonalizadoNF))
                    retorno = true;

                return retorno;
            }
        }
        #endregion

        #region RecuperarGeoLocalizacao
        public static string RecuperarGeoLocalizacao(int idFilial)
        {
            try
            { 
                using (var entity = new LanEntities()){
                    var retorno = (from filial in entity.TAB_Filial
                                          join endereco in entity.TAB_Endereco on filial.Idf_Endereco equals endereco.Idf_Endereco
                                          join cidade in entity.TAB_Cidade on endereco.Idf_Cidade equals cidade.Idf_Cidade
                                          where filial.Idf_Filial == idFilial
                                          select cidade.Geo_Localizacao).FirstOrDefault();

                    return string.Format("{0}, {1}", retorno.YCoordinate.ToString().Replace(",", "."), retorno.XCoordinate.ToString().Replace(",","."));
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion

    }
}
