//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.BLL.Custom;
using BNE.Cache;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CNAESubClasse : ICloneable // Tabela: TAB_CNAE_Sub_Classe
    {

        #region Configuração de cache
        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        private static List<CacheObject> CachedObject
        {
            get
            {
                return Cache.GetItem(typeof(CNAESubClasse).ToString(), Load, 60 * 24);
            }
        }
        private class CacheObject
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
            public string Codigo { get; set; }
        }
        private static List<CacheObject> Load()
        {
            var lista = new List<CacheObject>();

            const string select = @"
            SELECT  Idf_CNAE_Sub_Classe, 
                    Cod_CNAE_Sub_Classe,
                    Des_CNAE_Sub_Classe 
            FROM    plataforma.TAB_CNAE_Sub_Classe
            WHERE   1 = 1";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, select, null))
            {
                while (dr.Read())
                {
                    lista.Add(new CacheObject
                    {
                        Identificador = Convert.ToInt32(dr["Idf_CNAE_Sub_Classe"]),
                        Codigo = dr["Cod_CNAE_Sub_Classe"].ToString(),
                        Descricao = dr["Des_CNAE_Sub_Classe"].ToString()
                    });
                }
            }

            return lista;
        }
        #endregion

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas
        private const string Spselectcodsubclasse = @" 
        SELECT  Idf_CNAE_Sub_Classe, 
                Cod_CNAE_Sub_Classe,
                Des_CNAE_Sub_Classe, 
                Idf_CNAE_Classe 
        FROM    plataforma.TAB_CNAE_Sub_Classe WITH(NOLOCK)
        WHERE   Cod_CNAE_Sub_Classe LIKE @Cod_CNAE_Sub_Classe";
        #endregion

        #region Métodos

        #region ListarPorCodigo
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader ListarPorCodigo(string codigoCNAE, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ParameterName =  "@Cod_CNAE_Sub_Classe", SqlDbType = SqlDbType.Char, Size = 7, Value = codigoCNAE}
                };

            if (trans != null)
                return DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectcodsubclasse, parms);
            
            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcodsubclasse, parms);
        }
        #endregion

        #region CarregarPorCodigo
        /// <summary>
        /// Método utilizado para retornar uma instância de CNAESubClasse a partir do banco de dados.
        /// </summary>
        /// <param name="codigoCNAE">Chave do registro.</param>
        /// <param name="objCNAESubClasse">Objeto CNAE Sub Classe </param>
        /// <param name="trans">Objeto Transação </param>
        /// <returns>Instância de CNAESubClasse.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCodigo(string codigoCNAE, out CNAESubClasse objCNAESubClasse, SqlTransaction trans = null)
        {
            objCNAESubClasse = new CNAESubClasse();

            #region Cache
            if (CachedObject != null)
            {
                var cnae = CachedObject.FirstOrDefault(d => d.Codigo.NormalizarStringLINQ() == codigoCNAE.NormalizarStringLINQ());

                if (cnae != null)
                {
                    objCNAESubClasse = new CNAESubClasse
                    {
                        IdCNAESubClasse = cnae.Identificador,
                        DescricaoCNAESubClasse = cnae.Descricao,
                        CodigoCNAESubClasse = cnae.Codigo
                    };
                    return true;
                }
                return false;
            }
            #endregion

            IDataReader dr = trans != null ? ListarPorCodigo(codigoCNAE, trans) : ListarPorCodigo(codigoCNAE);

            if (SetInstance(dr, objCNAESubClasse))
                return true;

            objCNAESubClasse = null;
            return false;
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de CNAESubClasse a partir do banco de dados.
        /// </summary>
        /// <param name="idCNAESubClasse">Chave do registro.</param>
        /// <returns>Instância de CNAESubClasse.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CNAESubClasse LoadObject(int idCNAESubClasse)
        {
            #region Cache
            if (CachedObject != null)
            {
                var cnae = CachedObject.FirstOrDefault(d => d.Identificador == idCNAESubClasse);

                if (cnae != null)
                {
                    return new CNAESubClasse
                    {
                        IdCNAESubClasse = cnae.Identificador,
                        DescricaoCNAESubClasse = cnae.Descricao,
                        CodigoCNAESubClasse = cnae.Codigo
                    };
                }

                throw new RecordNotFoundException(typeof(CNAESubClasse));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idCNAESubClasse))
            {
                CNAESubClasse objCNAESubClasse = new CNAESubClasse();
                if (SetInstance(dr, objCNAESubClasse))
                    return objCNAESubClasse;
            }
            throw (new RecordNotFoundException(typeof(CNAESubClasse)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de CNAESubClasse a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCNAESubClasse">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de CNAESubClasse.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CNAESubClasse LoadObject(int idCNAESubClasse, SqlTransaction trans)
        {
            #region Cache
            if (CachedObject != null)
            {
                var cnae = CachedObject.FirstOrDefault(d => d.Identificador == idCNAESubClasse);

                if (cnae != null)
                {
                    return new CNAESubClasse
                    {
                        IdCNAESubClasse = cnae.Identificador,
                        DescricaoCNAESubClasse = cnae.Descricao,
                        CodigoCNAESubClasse = cnae.Codigo
                    };
                }

                throw new RecordNotFoundException(typeof(CNAESubClasse));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idCNAESubClasse, trans))
            {
                CNAESubClasse objCNAESubClasse = new CNAESubClasse();
                if (SetInstance(dr, objCNAESubClasse))
                    return objCNAESubClasse;
            }
            throw (new RecordNotFoundException(typeof(CNAESubClasse)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de CNAESubClasse a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (CachedObject != null)
            {
                var cnae = CachedObject.FirstOrDefault(d => d.Identificador == this._idCNAESubClasse);

                if (cnae != null)
                {
                    this.DescricaoCNAESubClasse = cnae.Descricao;
                    this.CodigoCNAESubClasse = cnae.Codigo;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idCNAESubClasse))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de CNAESubClasse a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (CachedObject != null)
            {
                var cnae = CachedObject.FirstOrDefault(d => d.Identificador == this._idCNAESubClasse);

                if (cnae != null)
                {
                    this.DescricaoCNAESubClasse = cnae.Descricao;
                    this.CodigoCNAESubClasse = cnae.Codigo;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idCNAESubClasse, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #endregion

    }
}