using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;
using System.Collections;

namespace LanHouse.Business
{
    public class Escolaridade
    {
        #region CarregarGraudaEscolaridade

        /// <summary>
        /// Carrega o grau da escolaridade
        /// </summary>
        /// <param name="idEscolaridade"></param>
        /// <returns></returns>
        public static string CarregarGraudaEscolaridade(int idEscolaridade)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from esc in entity.TAB_Escolaridade
                    where esc.Idf_Escolaridade == idEscolaridade
                    select new { esc.Idf_Grau_Escolaridade }).FirstOrDefault();

                return query.Idf_Grau_Escolaridade.ToString();
            }
        }

        #endregion

        #region ChecarNiveldaEscolaridade

        /// <summary>
        /// Checar o se a escolaridade é 3º completo
        /// </summary>
        /// <param name="idEscolaridade"></param>
        /// <returns></returns>
        public static bool ChecarNiveldaEscolaridade(int idEscolaridade)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from esc in entity.TAB_Escolaridade
                    where esc.Idf_Escolaridade == idEscolaridade && esc.Flg_Escolaridade_Completa == true
                    select new { }).FirstOrDefault();

                return query != null;
            }
        }

        #endregion

        #region ListarEscolaridadeBNE

        /// <summary>
        /// Listar as escolaridades até o 3º nível
        /// </summary>
        /// <returns></returns>
        public static IList ListarEscolaridadeBNE()
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from esc in entity.TAB_Escolaridade
                    where esc.Flg_Inativo == false && esc.Flg_BNE == true && esc.Idf_Escolaridade != 18 && esc.Idf_Grau_Escolaridade < 4
                    orderby esc.Seq_BNE ascending
                    select new { id = esc.Idf_Escolaridade, text = esc.Des_BNE, esc.Idf_Grau_Escolaridade });

                return query.ToList();
            }
        }

        #endregion

        #region ListarEspecializacoesBNE

        /// <summary>
        /// Listar todas as especializacoes
        /// </summary>
        /// <returns></returns>
        public static IList ListarEspecializacoesBNE()
        {
            using(var entity = new LanEntities())
            {
                var query = (
                from esc in entity.TAB_Escolaridade
                where esc.Flg_Inativo == false && esc.Flg_BNE == true && esc.Idf_Grau_Escolaridade >= 4 && esc.Idf_Escolaridade != 18
                orderby esc.Seq_BNE ascending
                select new { id = esc.Idf_Escolaridade, text = esc.Des_BNE, esc.Idf_Grau_Escolaridade });

                return query.ToList();
            }       
        }

        #endregion
    }
}
