using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class EstatisticaPlano // Tabela: BNE_Estatistica_Plano
    {
        #region Atributos
        private int _idEstatisticaPlano;
        private int _idEstatistica;
        private int _idParametro;
        private int _qtdCurriculos;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdEstatisticaPlano
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdEstatisticaPlano
        {
            get
            {
                return this._idEstatisticaPlano;
            }
        }
        #endregion

        #region IdEstatistica
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int IdEstatistica
        {
            get
            {
                return this._idEstatistica;
            }
        }
        #endregion

        #region IdParametro
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int IdParametro
        {
            get
            {
                return this._idParametro;
            }
        }
        #endregion

        #region QuantidadeCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int QuantidadeCurriculo
        {
            get
            {
                return this._qtdCurriculos;
            }
            set
            {
                this._qtdCurriculos = value;
                this._modified = true;
            }
        }
        #endregion
       
        #endregion

        #region Construtores
		public EstatisticaPlano()
		{
		}
        public EstatisticaPlano(int idEstatisticaPlano)
		{
			this._idEstatisticaPlano = idEstatisticaPlano;
			this._persisted = true;
		}
		#endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Estatistica_Plano (Qtd_Curriculo, Idf_Estatistica, Idf_Parametro) VALUES (@Qtd_Curriculos, @Idf_Estatistica, @Idf_Parametro);SET @Idf_Estatistica_Plano = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Estatistica_Plano SET Qtd_Curriculos = @Qtd_Curriculos WHERE Idf_Estatistica_Plano = @Idf_Estatistica_Plano";
        private const string SPDELETE = "DELETE FROM BNE_Estatistica_Plano WHERE Idf_Estatistica_Plano = @Idf_Estatistica_Plano";
        private const string SPSELECTID = "SELECT * FROM BNE_Estatistica_Plano WHERE Idf_Estatistica_Plano = @Idf_Estatistica_Plano";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Estatistica_Plano", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Estatistica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_Curriculos", SqlDbType.Int, 4));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Bruno Flammarion</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idEstatisticaPlano;
            parms[1].Value = this._qtdCurriculos;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de Estatistica_Plano no banco de dados.
        /// </summary>
        /// <remarks>Bruno Flammarion</remarks>
        private void Insert()
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
                        this._idEstatisticaPlano = Convert.ToInt32(cmd.Parameters["@Idf_Estatistica_Plano"].Value);
                        cmd.Parameters.Clear();
                        this._persisted = true;
                        this._modified = false;
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
       
        /// <summary>
        /// Método utilizado para inserir uma instância de Estatistica_Plano no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Bruno Flammarion</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idEstatisticaPlano = Convert.ToInt32(cmd.Parameters["@Idf_Estatistica_Plano"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Estatistica_Plano no banco de dados.
        /// </summary>
        /// <remarks>Bruno Flammarion</remarks>
        private void Update()
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATE, parms);
                this._modified = false;
            }
        }

        /// <summary>
        /// Método utilizado para atualizar uma instância de Estatistica_Plano no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Bruno Flammarion</remarks>
        private void Update(SqlTransaction trans)
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                this._modified = false;
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Método utilizado para salvar uma instância de Estatistica no banco de dados.
        /// </summary>
        /// <remarks>Bruno Flammarion</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de Estatistica no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Bruno Flammarion</remarks>
        public void Save(SqlTransaction trans)
        {
            if (!this._persisted)
                this.Insert(trans);
            else
                this.Update(trans);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Método utilizado para excluir uma instância de Estatistica_Plano no banco de dados.
        /// </summary>
        /// <param name="idEstatisticaPlano">Chave do registro.</param>
        /// <remarks>Bruno Flammarion</remarks>
        public static void Delete(int idEstatisticaPlano)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Estatistica_Plano", SqlDbType.Int, 4));

            parms[0].Value = idEstatisticaPlano;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de Estatistica no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idEstatistica">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Bruno Flammarion</remarks>
        public static void Delete(int idEstatisticaPlano, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Estatistica_Plano", SqlDbType.Int, 4));

            parms[0].Value = idEstatisticaPlano;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idEstatisticaPlano">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        private static IDataReader LoadDataReader(int idEstatisticaPlano)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Estatistica_Plano", SqlDbType.Int, 4));

            parms[0].Value = idEstatisticaPlano;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idEstatisticaPlano">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        private static IDataReader LoadDataReader(int idEstatisticaPlano, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Estatistica_Plano", SqlDbType.Int, 4));

            parms[0].Value = idEstatisticaPlano;

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
        }

        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Estatistica a partir do banco de dados.
        /// </summary>
        /// <param name="idEstatisticaPlano">Chave do registro.</param>
        /// <returns>Instância de Estatistica.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        public static EstatisticaPlano LoadObject(int idEstatisticaPlano)
        {
            using (IDataReader dr = LoadDataReader(idEstatisticaPlano))
            {
                EstatisticaPlano objEstatisticaPlano = new EstatisticaPlano();
                if (SetInstance(dr, objEstatisticaPlano))
                    return objEstatisticaPlano;
            }
            throw (new RecordNotFoundException(typeof(EstatisticaPlano)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Estatistica_Plano a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idEstatistica">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Estatistica.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        public static EstatisticaPlano LoadObject(int idEstatisticaPlano, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idEstatisticaPlano, trans))
            {
                EstatisticaPlano objEstatisticaPlano = new EstatisticaPlano();
                if (SetInstance(dr, objEstatisticaPlano))
                    return objEstatisticaPlano;
            }
            throw (new RecordNotFoundException(typeof(EstatisticaPlano)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Estatistica_Plano a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idEstatisticaPlano))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Estatistica_Plano a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idEstatisticaPlano, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objEstatisticaPlano">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        private static bool SetInstance(IDataReader dr, EstatisticaPlano objEstatisticaPlano)
        {
            try
            {
                if (dr.Read())
                {
                    objEstatisticaPlano._idEstatisticaPlano = Convert.ToInt32(dr["Idf_Estatistica_Plano"]);
                    objEstatisticaPlano._idEstatistica = Convert.ToInt32(dr["Idf_Estatistica"]);                    
                    objEstatisticaPlano._idParametro = Convert.ToInt32(dr["Idf_Parametro"]);
                    objEstatisticaPlano._qtdCurriculos = Convert.ToInt32(dr["Qtd_Curriculos"]);

                    objEstatisticaPlano._persisted = true;
                    objEstatisticaPlano._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

        #endregion
    }
}
