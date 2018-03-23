using LanHouse.Entities.BNE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business
{
    public class Cidade
    {
        #region ValidarCidade
        public static bool ValidarCidade(string cidade)
        {
            using (var entity = new LanEntities())
            {
                bool retorno = false;

                if (!String.IsNullOrEmpty(cidade))
                {
                    var array = cidade.Split('/');

                    if (array.Length == 2)
                    {
                        string _cid = array[0];
                        string _uf = array[1];

                        var query = (
                            from cid in entity.TAB_Cidade
                            where cid.Nme_Cidade == _cid && cid.Sig_Estado == _uf && cid.Flg_Inativo == false
                            select cid).Count();

                        retorno = (query >= 1);
                    }
                }

                return retorno;
            }
        }
        #endregion

        #region ListarSugestaodeCidade

        public static IList ListarSugestaodeCidade(string nomeCidade, int limiteRegistros)
        {
            using (var entity = new LanEntities())
            {
                var query = (
                    from cid in entity.TAB_Cidade
                    where cid.Flg_Inativo == false
                    && cid.Nme_Cidade.ToLower().StartsWith(nomeCidade.ToLower())
                    orderby cid.Nme_Cidade.ToLower().StartsWith(nomeCidade.ToLower()) descending, cid.Nme_Cidade ascending
                    select new { id = cid.Idf_Cidade, text = cid.Nme_Cidade + "/" + cid.Sig_Estado }).Take(limiteRegistros);

                return query.ToList();
            }
        }

        #endregion

        #region CarregarCidade

        public static bool CarregarCidade(int idCidade, out TAB_Cidade objCidade)
        {
            using (var entity = new LanEntities())
            {

                objCidade =
                    entity.TAB_Cidade.FirstOrDefault(c => c.Idf_Cidade == idCidade);

                return objCidade != null;
            }
        }

        #endregion

        #region CarregarCidadePorNome
        /// <summary>
        /// Carregar uma cidade por nome
        /// </summary>
        public static bool CarregarCidadeporNome(string cidade, string uf, out TAB_Cidade objCidade)
        {
            using (var entity = new LanEntities())
            {
                objCidade = entity.TAB_Cidade
                    .FirstOrDefault(cid => cid.Nme_Cidade == cidade && cid.Sig_Estado == uf && cid.Flg_Inativo == false);

                return objCidade != null;
            }
        }
        #endregion

        #region CarregarCidadePorNomeEstado
        /// <summary>
        /// Carregar uma cidade por nome
        /// </summary>
        public static TAB_Cidade CarregarCidadePorNomeEstado(string nome)
        {
            string cidade;
            string sigla;
            string[] dadosCidade = nome.Split('/');

            cidade = dadosCidade[0];
            sigla = dadosCidade[1];

            using (var entity = new LanEntities())
            {
                var objCidade = entity.TAB_Cidade
                    .FirstOrDefault(cid => cid.Nme_Cidade == cidade && cid.Sig_Estado == sigla && cid.Flg_Inativo == false);

                return objCidade;
            }
        }
        #endregion

        #region RecuperarTaxaISS
        public static void RecuperarTaxaISS(int idCidade, out decimal vlrTaxaISS)
        {
            using (var entity = new LanEntities())
            {
                vlrTaxaISS = 0;

                var retorno = (from cidade in entity.TAB_Cidade
                               where cidade.Idf_Cidade == idCidade
                               select cidade.Txa_ISS).FirstOrDefault();
                if (retorno != null)
                    vlrTaxaISS = Convert.ToDecimal(retorno);
            }
        }
        #endregion
    }
}
