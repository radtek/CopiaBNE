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
    public partial class NaturezaJuridica : ICloneable // Tabela: TAB_Natureza_Juridica
    {

        #region Configuração de cache
        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        private static List<CacheObject> CachedObject
        {
            get
            {
                return Cache.GetItem(typeof(NaturezaJuridica).ToString(), Load, 60 * 24);
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
            SELECT  Idf_Natureza_Juridica,
                    Des_Natureza_Juridica,
                    Cod_Natureza_Juridica
            FROM    plataforma.TAB_Natureza_Juridica
            WHERE   1 = 1";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, select, null))
            {
                while (dr.Read())
                {
                    lista.Add(new CacheObject
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Natureza_Juridica"]),
                        Descricao = dr["Des_Natureza_Juridica"].ToString(),
                        Codigo = dr["Cod_Natureza_Juridica"].ToString()
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
        private const string SPSELECTCODIGO = "SELECT * FROM plataforma.TAB_Natureza_Juridica WHERE Cod_Natureza_Juridica = @Cod_Natureza_Juridica";
        #endregion

        #region Métodos

        #region ListarPorCodigo
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader ListarPorCodigo(string codigoNatureza, SqlTransaction trans = null)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Cod_Natureza_Juridica", SqlDbType.Char, 4));

            parms[0].Value = codigoNatureza;

            IDataReader dr = null;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTCODIGO, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTCODIGO, parms);

            return dr;
        }
        #endregion

        #region CarregarPorCodigo
        /// <summary>
        /// Método utilizado para retornar uma instância de NaturezaJuridica a partir do banco de dados.
        /// </summary>
        /// <param name="codigoNatureza">Chave do registro.</param>
        /// <returns>Instância de NaturezaJuridica.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCodigo(string codigoNatureza, out NaturezaJuridica objNaturezaJuridica, SqlTransaction trans = null)
        {
            objNaturezaJuridica = new NaturezaJuridica();

            #region Cache
            if (CachedObject != null)
            {
                var naturezaJuridica = CachedObject.FirstOrDefault(d => d.Codigo.NormalizarStringLINQ().Equals(codigoNatureza.NormalizarStringLINQ()));

                if (naturezaJuridica != null)
                {
                    objNaturezaJuridica = new NaturezaJuridica
                    {
                        IdNaturezaJuridica = naturezaJuridica.Identificador,
                        DescricaoNaturezaJuridica = naturezaJuridica.Descricao,
                        CodigoNaturezaJuridica = naturezaJuridica.Codigo
                    };
                    return true;
                }
                return false;
            }
            #endregion

            IDataReader dr = ListarPorCodigo(codigoNatureza, trans);

            if (SetInstance(dr, objNaturezaJuridica))
                return true;

            if (!dr.IsClosed)
                dr.Close();

            objNaturezaJuridica = null;
            return false;
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de NaturezaJuridica a partir do banco de dados.
        /// </summary>
        /// <param name="idNaturezaJuridica">Chave do registro.</param>
        /// <returns>Instância de NaturezaJuridica.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static NaturezaJuridica LoadObject(int idNaturezaJuridica)
        {
            #region Cache
            if (CachedObject != null)
            {
                var naturezaJuridica = CachedObject.FirstOrDefault(d => d.Identificador == idNaturezaJuridica);

                if (naturezaJuridica != null)
                {
                    return new NaturezaJuridica
                    {
                        IdNaturezaJuridica = naturezaJuridica.Identificador,
                        DescricaoNaturezaJuridica = naturezaJuridica.Descricao,
                        CodigoNaturezaJuridica = naturezaJuridica.Codigo
                    };
                }

                throw new RecordNotFoundException(typeof(NaturezaJuridica));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idNaturezaJuridica))
            {
                NaturezaJuridica objNaturezaJuridica = new NaturezaJuridica();
                if (SetInstance(dr, objNaturezaJuridica))
                    return objNaturezaJuridica;
            }
            throw (new RecordNotFoundException(typeof(NaturezaJuridica)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de NaturezaJuridica a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idNaturezaJuridica">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de NaturezaJuridica.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static NaturezaJuridica LoadObject(int idNaturezaJuridica, SqlTransaction trans)
        {
            #region Cache
            if (CachedObject != null)
            {
                var naturezaJuridica = CachedObject.FirstOrDefault(d => d.Identificador == idNaturezaJuridica);

                if (naturezaJuridica != null)
                {
                    return new NaturezaJuridica
                    {
                        IdNaturezaJuridica = naturezaJuridica.Identificador,
                        DescricaoNaturezaJuridica = naturezaJuridica.Descricao,
                        CodigoNaturezaJuridica = naturezaJuridica.Codigo
                    };
                }

                throw new RecordNotFoundException(typeof(NaturezaJuridica));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idNaturezaJuridica, trans))
            {
                NaturezaJuridica objNaturezaJuridica = new NaturezaJuridica();
                if (SetInstance(dr, objNaturezaJuridica))
                    return objNaturezaJuridica;
            }
            throw (new RecordNotFoundException(typeof(NaturezaJuridica)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de NaturezaJuridica a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (CachedObject != null)
            {
                var naturezaJuridica = CachedObject.FirstOrDefault(d => d.Identificador == this._idNaturezaJuridica);

                if (naturezaJuridica != null)
                {
                    this.DescricaoNaturezaJuridica = naturezaJuridica.Descricao;
                    this.CodigoNaturezaJuridica = naturezaJuridica.Codigo;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idNaturezaJuridica))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de NaturezaJuridica a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (CachedObject != null)
            {
                var naturezaJuridica = CachedObject.FirstOrDefault(d => d.Identificador == this._idNaturezaJuridica);

                if (naturezaJuridica != null)
                {
                    this.DescricaoNaturezaJuridica = naturezaJuridica.Descricao;
                    this.CodigoNaturezaJuridica = naturezaJuridica.Codigo;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idNaturezaJuridica, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #endregion

    }
}