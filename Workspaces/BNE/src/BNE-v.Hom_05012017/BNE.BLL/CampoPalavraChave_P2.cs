//-- Data: 20/01/2016 16:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BNE.Cache;

namespace BNE.BLL
{
    public partial class CampoPalavraChave // Tabela: BNE_Campo_Palavra_Chave
    {

        #region Cache

        static readonly ICachingService CacheService = CachingServiceProvider.Instance;

        #region Cache
        private static List<CampoPalavraChave> Cache
        {
            get
            {
                return CacheService.GetItem(typeof(CampoPalavraChave).ToString(), Listar, 1440);
            }
        }
        #endregion

        #region Listar
        /// <summary>
        /// Método que retorna uma lista.
        /// </summary>
        /// <returns>Lista com valores recuperados do banco.</returns>
        private static List<CampoPalavraChave> Listar()
        {
            var lista = new List<CampoPalavraChave>();

            const string spselectall = @"
            SELECT  Idf_Campo_Palavra_Chave ,
                    Nme_Campo_Palavra_Chave ,
                    Nme_Campo_Palavra_Chave_SOLR ,
                    Flg_Inativo
            FROM    BNE_Campo_Palavra_Chave (NOLOCK) F
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselectall, null))
            {
                while (dr.Read())
                {
                    var obj = new CampoPalavraChave();
                    if (SetInstance(dr, obj, false))
                        lista.Add(obj);
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #endregion

        #region BuscaCampos

        #region Métodos
        /// <summary>
        /// Retorna um dicionário com os items ativos
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> BuscaCampos()
        {
            return Cache.Where(item => item.FlagInativo == false).ToDictionary(item => item.IdCampoPalavraChave.ToString(), item => item.NomeCampoPalavraChave);
        }
        #endregion

        #endregion

    }
}