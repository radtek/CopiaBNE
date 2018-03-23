using LanHouse.Entities.BNE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class FuncaoPretendida
    {
        #region CarregarPorCurriculo
        /// <summary>
        /// Carregar um objeto de Função pretendida a partir do seu ID
        /// </summary>
        public static bool CarregarPorCurriculo(int idCurriculo, out IList listaFuncoes)
        {
            using (var entity = new LanEntities())
            {
                listaFuncoes = (from cv in entity.BNE_Funcao_Pretendida
                                join fun in entity.TAB_Funcao on cv.Idf_Funcao equals fun.Idf_Funcao
                                where cv.Idf_Curriculo == idCurriculo
                                orderby cv.Dta_Cadastro descending
                                select new { Idf_Funcao_Pretendida = cv.Idf_Funcao_Pretendida, Des_Funcao = fun.Des_Funcao, idFuncao = fun.Idf_Funcao }).Take(1).ToList();

                if(listaFuncoes.Count ==0)
                {
                    listaFuncoes = (from cv in entity.BNE_Funcao_Pretendida
                                        where cv.Idf_Curriculo == idCurriculo
                                        orderby cv.Dta_Cadastro descending
                                        select new { Idf_Funcao_Pretendida = cv.Idf_Funcao_Pretendida, Des_Funcao = cv.Des_Funcao_Pretendida, idFuncao = cv.Idf_Funcao }).Take(1).ToList();
                }

                return listaFuncoes != null;
            }
        }
        #endregion

        #region CarregarDescricaoFuncaoPretendidaPorCurriculo
        /// <summary>
        /// Carregar um objeto de Função pretendida a partir do seu ID
        /// </summary>
        public static string CarregarDescricaoFuncaoPretendidaPorCurriculo(int idCurriculo)
        {
            using (var entity = new LanEntities())
            {
               var descricaoFuncao = (from cv in entity.BNE_Funcao_Pretendida
                                join fun in entity.TAB_Funcao on cv.Idf_Funcao equals fun.Idf_Funcao
                                where cv.Idf_Curriculo == idCurriculo
                                orderby cv.Dta_Cadastro descending
                                select fun.Des_Funcao).FirstOrDefault();

                if (descricaoFuncao == "")
                {
                    descricaoFuncao = (from cv in entity.BNE_Funcao_Pretendida
                                    where cv.Idf_Curriculo == idCurriculo
                                    orderby cv.Dta_Cadastro descending
                                    select cv.Des_Funcao_Pretendida).FirstOrDefault();
                }

                return descricaoFuncao;
            }
        }
        #endregion
    }
}
