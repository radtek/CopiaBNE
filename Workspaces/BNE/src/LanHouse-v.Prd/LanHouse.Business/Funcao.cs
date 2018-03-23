using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanHouse.Entities.BNE;
using System.Collections;
using LanHouse.Business.Extension.CustomExtension;

namespace LanHouse.Business
{
    public class Funcao
    {
        #region RecuperarIDporNome
        /// <summary>
        /// Retornar o ID pela descrição da Função
        /// </summary>
        /// <param name="funcao"></param>
        public static int RecuperarIDporNome(string funcao)
        {
            using (var entity = new LanEntities())
            {
                funcao = funcao ?? string.Empty;

                var valorPesquisa = funcao.RemoveAcentos().ToUpper();

                var query = (
                    from f in entity.TAB_Funcao
                    where f.Des_Funcao.ToUpper() == valorPesquisa && f.Flg_Inativo == false
                    select f.Idf_Funcao);

                return query.FirstOrDefault();
            }
        }
        #endregion

        #region RecuperarNomeporID
        /// <summary>
        /// Retornar a descrição da Função pelo ID
        /// </summary>
        /// <param name="funcao"></param>
        public static string RecuperarNomeporID(int idFucao)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from f in entity.TAB_Funcao
                    where f.Idf_Funcao == idFucao && f.Flg_Inativo == false
                    select f.Des_Funcao);

                return query.FirstOrDefault();
            }
        }
        #endregion

        #region ListarSugestaodeFuncao
        public static IList ListarSugestaodeFuncao(string nomeFuncao, int limiteRegistros)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from fun in entity.TAB_Funcao
                    where fun.Flg_Inativo == false
                    && fun.Des_Funcao.ToLower().StartsWith(nomeFuncao.ToLower())
                    orderby fun.Des_Funcao.ToLower().StartsWith(nomeFuncao.ToLower()) ascending
                    select new { id = fun.Idf_Funcao, text = fun.Des_Funcao }).Take(limiteRegistros);

                return query.ToList();
            }
        }
        #endregion

        #region ListarSugestaoAtividades
        public static string ListarSugestaoAtividades(string nomeFuncao)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from fun in entity.TAB_Funcao
                    where fun.Flg_Inativo == false
                    && fun.Des_Funcao.ToLower().StartsWith(nomeFuncao.ToLower())
                    orderby fun.Des_Funcao.ToLower().StartsWith(nomeFuncao.ToLower()) ascending
                    select fun.Des_Job).FirstOrDefault();

                return query;
            }
        }
        #endregion

        #region RecuperarPorNome
        /// <summary>
        /// Retornar o objeto pela descrição da Função
        /// </summary>
        /// <param name="funcao"></param>
        public static TAB_Funcao RecuperarPorNome(string funcao, LanEntities context)
        {
            using (var entity = new LanEntities())
            {
                funcao = funcao ?? string.Empty;

                var valorPesquisa = funcao.RemoveAcentos().ToUpper();

                var query = (
                    from f in entity.TAB_Funcao
                    where f.Des_Funcao_Pesquisa.ToUpper() == valorPesquisa && f.Flg_Inativo == false
                    select f);

                return query.FirstOrDefault();
            }
        }
        #endregion
    }
}
