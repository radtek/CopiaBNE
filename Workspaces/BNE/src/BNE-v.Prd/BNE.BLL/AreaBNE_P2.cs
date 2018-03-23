//-- Data: 02/03/2010 09:54
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.Cache;
using BNE.EL;

namespace BNE.BLL
{
    public partial class AreaBNE // Tabela: TAB_Area_BNE
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "TAB_Area_BNE";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region AreasBNE
        private static List<AreaBNECACHE> AreasBNE
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarAreasBNECACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarAreasBNECACHE
        /// <summary>
        /// Método que retorna uma lista de itens do sistema.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<AreaBNECACHE> ListarAreasBNECACHE()
        {
            var lista = new List<AreaBNECACHE>();

            const string spselecttodasareasbne = @"
            SELECT  Idf_Area_BNE, 
                    Des_Area_BNE 
            FROM    plataforma.TAB_Area_BNE WITH(NOLOCK) 
            WHERE   Flg_Inativo = 0 
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselecttodasareasbne, null))
            {
                while (dr.Read())
                {
                    lista.Add(new AreaBNECACHE { Identificador = Convert.ToInt32(dr["Idf_Area_BNE"]), Descricao = dr["Des_Area_BNE"].ToString() });
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #endregion

        #region CacheObjects

        #region AreaBNECACHE
        private class AreaBNECACHE
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consulta

        private const string Selectareabne = @" SELECT * FROM plataforma.TAB_Area_BNE WITH(NOLOCK) WHERE Flg_Inativo = 0 ORDER BY Des_Area_BNE";
        private const string SelectareabnePorDescricao = @" SELECT * FROM plataforma.TAB_Area_BNE WITH(NOLOCK) WHERE Des_Area_BNE = @desArea AND Flg_Inativo = 0 ORDER BY Des_Area_BNE";


        #endregion

        #region Listar
        /// <summary>
        /// Método retorna Consulta para carregar DropDownList
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> Listar()
        {
            #region Cache
            if (HabilitaCache)
                return AreasBNE.OrderBy(ab => ab.Descricao).ToDictionary(ab => ab.Identificador.ToString(), ab => ab.Descricao);
            #endregion

            var dicionarioSexo = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Selectareabne, null))
            {
                while (dr.Read())
                {
                    dicionarioSexo.Add(dr["Idf_Area_BNE"].ToString(), dr["Des_Area_BNE"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionarioSexo;
        }
        #endregion

        #region CarregarPorDescricao
        public static bool CarregarPorDescricao(string descricao, out AreaBNE objAreaBNE)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@desArea", SqlDbType = SqlDbType.VarChar, Size = 100, Value = descricao },
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SelectareabnePorDescricao, parms))
            {
                objAreaBNE = new AreaBNE();
                if (SetInstance(dr, objAreaBNE))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }

            return false;
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de AreaBNE a partir do banco de dados.
        /// </summary>
        /// <param name="idAreaBNE">Chave do registro.</param>
        /// <returns>Instância de AreaBNE.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static AreaBNE LoadObject(int idAreaBNE)
        {
            #region Cache
            if (HabilitaCache)
            {
                var areaBNE = AreasBNE.FirstOrDefault(s => s.Identificador == idAreaBNE);

                if (areaBNE != null)
                    return new AreaBNE { IdAreaBNE = areaBNE.Identificador, DescricaoAreaBNE = areaBNE.Descricao };

                throw (new RecordNotFoundException(typeof(AreaBNE)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idAreaBNE))
            {
                AreaBNE objAreaBNE = new AreaBNE();
                if (SetInstance(dr, objAreaBNE))
                    return objAreaBNE;
            }
            throw (new RecordNotFoundException(typeof(AreaBNE)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de AreaBNE a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAreaBNE">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de AreaBNE.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static AreaBNE LoadObject(int idAreaBNE, SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var areaBNE = AreasBNE.FirstOrDefault(s => s.Identificador == idAreaBNE);

                if (areaBNE != null)
                    return new AreaBNE { IdAreaBNE = areaBNE.Identificador, DescricaoAreaBNE = areaBNE.Descricao };

                throw (new RecordNotFoundException(typeof(AreaBNE)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idAreaBNE, trans))
            {
                AreaBNE objAreaBNE = new AreaBNE();
                if (SetInstance(dr, objAreaBNE))
                    return objAreaBNE;
            }
            throw (new RecordNotFoundException(typeof(AreaBNE)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de AreaBNE a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var areaBNE = AreasBNE.FirstOrDefault(s => s.Identificador == this._idAreaBNE);

                if (areaBNE != null)
                {
                    this.DescricaoAreaBNE = areaBNE.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idAreaBNE))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de AreaBNE a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var areaBNE = AreasBNE.FirstOrDefault(s => s.Identificador == this._idAreaBNE);

                if (areaBNE != null)
                {
                    this.DescricaoAreaBNE = areaBNE.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idAreaBNE, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion


        #region SetInstance_NotDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objAreaBNE">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool SetInstance_NotDispose(IDataReader dr, AreaBNE objAreaBNE)
        {
            try
            {
                objAreaBNE._idAreaBNE = Convert.ToInt32(dr["Idf_Area_BNE"]);
                objAreaBNE._descricaoAreaBNE = Convert.ToString(dr["Des_Area_BNE"]);
                objAreaBNE._descricaoAreaBNEPesquisa = Convert.ToString(dr["Des_Area_BNE_Pesquisa"]);
                objAreaBNE._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objAreaBNE._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                objAreaBNE._persisted = true;
                objAreaBNE._modified = false;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public DateTime MigrationDataCadastro
        {
            set
            {
                this._dataCadastro = value;
            }
            get { return this._dataCadastro; }
        }
        #endregion

    }
}