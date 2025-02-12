﻿using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.SalarioBR;

namespace LanHouse.Business
{
    public class PesquisaSalarial
    {
        #region CarregarMedialSalarialFuncao
        /// <summary>
        /// Resultado da pesquisa salarial do BNE
        /// </summary>
        /// <param name="idFuncao"></param>
        /// <param name="idEstado"></param>
        /// <param name="objPesquisaSalarial"></param>
        /// <returns></returns>
        public static bool CarregarMedialSalarialFuncao(int idFuncao, int idEstado, out TAB_Pesquisa_Salarial objPesquisaSalarial)
        {
            using (var entity = new LanEntities())
            {
                objPesquisaSalarial =
                    entity.TAB_Pesquisa_Salarial
                    .FirstOrDefault(p => p.Idf_Estado == idEstado && p.Idf_Funcao == idFuncao);

                return objPesquisaSalarial != null;
            }
        }
        #endregion

        #region CarregarMedialSalarialFuncao
        /// <summary>
        /// Resultado da pesquisa salarial do Salário BR
        /// </summary>
        /// <param name="idFuncao"></param>
        /// <param name="siglaEstado"></param>
        /// <param name="objPesquisaSalarial"></param>
        /// <returns></returns>
        public static bool CarregarMedialSalarialFuncao(int idFuncao, string siglaEstado, out TAB_Resultado_Pesquisa_Salarial objPesquisaSalarial)
        {
            using (var entity = new SALARIOBREntities())
            {
                objPesquisaSalarial =
                    entity.TAB_Resultado_Pesquisa_Salarial
                    .FirstOrDefault(p => p.Sig_Estado == siglaEstado && p.Idf_Funcao == idFuncao);

                return objPesquisaSalarial != null;
            }
        }
        #endregion
    }
}
