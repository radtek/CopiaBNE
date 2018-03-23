using LanHouse.Entities.BNE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class Fonte
    {
        #region PesquisarInstituicao
        /// <summary>
        /// Pesquisar as instituições de ensino
        /// </summary>
        /// <param name="nomeFonte"></param>
        /// <param name="limiteRegistros"></param>
        /// <returns></returns>
        public static IList PesquisarInstituicao(string nomeFonte, int limiteRegistros)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from fon in entity.TAB_Fonte
                    where fon.Nme_Fonte.ToLower().StartsWith(nomeFonte.ToLower()) || fon.Sig_Fonte.ToLower().StartsWith(nomeFonte.ToLower())
                    orderby fon.Nme_Fonte ascending
                    select new { id = fon.Idf_Fonte, text = fon.Nme_Fonte }).Take(limiteRegistros);

                return query.ToList();
            }
        }
        #endregion

        #region CarregarIdInstituicaoPelaDescricao
        /// <summary>
        /// Carregar uma instituição de ensino
        /// </summary>
        /// <param name="nomeInstituicao"></param>
        /// <returns></returns>
        public static int CarregarIdInstituicaoPelaDescricao(string nomeInstituicao)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from fon in entity.TAB_Fonte
                    where fon.Nme_Fonte.ToLower() == nomeInstituicao.ToLower()
                    orderby fon.Nme_Fonte ascending
                    select fon.Idf_Fonte).FirstOrDefault();

                return query;
            }
        }
        #endregion
    }
}
