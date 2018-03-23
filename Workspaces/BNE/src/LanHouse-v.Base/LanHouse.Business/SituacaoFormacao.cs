using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;
using System.Collections;

namespace LanHouse.Business
{
    public class SituacaoFormacao
    {
        #region ListarSituacaoFormacao

        /// <summary>
        /// Listar todas as Situações de  Formação
        /// </summary>
        /// <returns></returns>
        public static IList ListarSituacaoFormacao()
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from sit in entity.BNE_Situacao_Formacao
                    where sit.Flg_Inativo == false
                    orderby sit.Des_Situacao_Formacao ascending
                    select new { id = sit.Idf_Situacao_Formacao, text = sit.Des_Situacao_Formacao });

                return query.ToList();
            }
        }

        #endregion

        #region CarregarIdSituacaopelaDescricao
        /// <summary>
        /// Carregar id da Situacao pela descrição
        /// </summary>
        /// <param name="desSituacao"></param>
        /// <returns></returns>
        public static short CarregarIdSituacaopelaDescricao(string desSituacao)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from sit in entity.BNE_Situacao_Formacao
                    where sit.Des_Situacao_Formacao.ToLower() == desSituacao.ToLower() && sit.Flg_Inativo == false
                    select sit.Idf_Situacao_Formacao).FirstOrDefault();

                return query;
            }
        }

        #endregion
    }
}
