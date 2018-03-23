//-- Data: 13/07/2015 17:59
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class TipoRetornoCampanhaRecrutamento // Tabela: BNE_Tipo_Retorno_Campanha_Recrutamento
    {
        #region Atributos
        private Int16 _idTipoRetornoCampanhaRecrutamento;
        private string _descricaoTipoRetornoCampanhaRecrutamento;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdTipoRetornoCampanhaRecrutamento
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public Int16 IdTipoRetornoCampanhaRecrutamento
        {
            get
            {
                return this._idTipoRetornoCampanhaRecrutamento;
            }
        }
        #endregion

        #region DescricaoTipoRetornoCampanhaRecrutamento
        /// <summary>
        /// Tamanho do campo: 30.
        /// Campo opcional.
        /// </summary>
        public string DescricaoTipoRetornoCampanhaRecrutamento
        {
            get
            {
                return this._descricaoTipoRetornoCampanhaRecrutamento;
            }
            set
            {
                this._descricaoTipoRetornoCampanhaRecrutamento = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public TipoRetornoCampanhaRecrutamento()
        {
        }
        public TipoRetornoCampanhaRecrutamento(Int16 idTipoRetornoCampanhaRecrutamento)
        {
            this._idTipoRetornoCampanhaRecrutamento = idTipoRetornoCampanhaRecrutamento;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Tipo_Retorno_Campanha_Recrutamento (Des_Tipo_Retorno_Campanha_Recrutamento) VALUES (@Des_Tipo_Retorno_Campanha_Recrutamento);SET @Idf_Tipo_Retorno_Campanha_Recrutamento = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Tipo_Retorno_Campanha_Recrutamento SET Des_Tipo_Retorno_Campanha_Recrutamento = @Des_Tipo_Retorno_Campanha_Recrutamento WHERE Idf_Tipo_Retorno_Campanha_Recrutamento = @Idf_Tipo_Retorno_Campanha_Recrutamento";
        private const string SPDELETE = "DELETE FROM BNE_Tipo_Retorno_Campanha_Recrutamento WHERE Idf_Tipo_Retorno_Campanha_Recrutamento = @Idf_Tipo_Retorno_Campanha_Recrutamento";
        private const string SPSELECTID = "SELECT * FROM BNE_Tipo_Retorno_Campanha_Recrutamento WITH(NOLOCK) WHERE Idf_Tipo_Retorno_Campanha_Recrutamento = @Idf_Tipo_Retorno_Campanha_Recrutamento";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Tipo_Retorno_Campanha_Recrutamento", SqlDbType.Int, 1));
            parms.Add(new SqlParameter("@Des_Tipo_Retorno_Campanha_Recrutamento", SqlDbType.VarChar, 30));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idTipoRetornoCampanhaRecrutamento;

            if (!String.IsNullOrEmpty(this._descricaoTipoRetornoCampanhaRecrutamento))
                parms[1].Value = this._descricaoTipoRetornoCampanhaRecrutamento;
            else
                parms[1].Value = DBNull.Value;


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
        /// Método utilizado para inserir uma instância de TipoRetornoCampanhaRecrutamento no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
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
                        this._idTipoRetornoCampanhaRecrutamento = Convert.ToInt16(cmd.Parameters["@Idf_Tipo_Retorno_Campanha_Recrutamento"].Value);
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
        /// Método utilizado para inserir uma instância de TipoRetornoCampanhaRecrutamento no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idTipoRetornoCampanhaRecrutamento = Convert.ToInt16(cmd.Parameters["@Idf_Tipo_Retorno_Campanha_Recrutamento"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de TipoRetornoCampanhaRecrutamento no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
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
        /// Método utilizado para atualizar uma instância de TipoRetornoCampanhaRecrutamento no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
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
        /// Método utilizado para salvar uma instância de TipoRetornoCampanhaRecrutamento no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de TipoRetornoCampanhaRecrutamento no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
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
        /// Método utilizado para excluir uma instância de TipoRetornoCampanhaRecrutamento no banco de dados.
        /// </summary>
        /// <param name="idTipoRetornoCampanhaRecrutamento">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(Int16 idTipoRetornoCampanhaRecrutamento)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Tipo_Retorno_Campanha_Recrutamento", SqlDbType.Int, 1));

            parms[0].Value = idTipoRetornoCampanhaRecrutamento;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de TipoRetornoCampanhaRecrutamento no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idTipoRetornoCampanhaRecrutamento">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(Int16 idTipoRetornoCampanhaRecrutamento, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Tipo_Retorno_Campanha_Recrutamento", SqlDbType.Int, 1));

            parms[0].Value = idTipoRetornoCampanhaRecrutamento;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de TipoRetornoCampanhaRecrutamento no banco de dados.
        /// </summary>
        /// <param name="idTipoRetornoCampanhaRecrutamento">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<Int16> idTipoRetornoCampanhaRecrutamento)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Tipo_Retorno_Campanha_Recrutamento where Idf_Tipo_Retorno_Campanha_Recrutamento in (";

            for (int i = 0; i < idTipoRetornoCampanhaRecrutamento.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 1));
                parms[i].Value = idTipoRetornoCampanhaRecrutamento[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idTipoRetornoCampanhaRecrutamento">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(Int16 idTipoRetornoCampanhaRecrutamento)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Tipo_Retorno_Campanha_Recrutamento", SqlDbType.Int, 1));

            parms[0].Value = idTipoRetornoCampanhaRecrutamento;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idTipoRetornoCampanhaRecrutamento">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(Int16 idTipoRetornoCampanhaRecrutamento, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Tipo_Retorno_Campanha_Recrutamento", SqlDbType.Int, 1));

            parms[0].Value = idTipoRetornoCampanhaRecrutamento;

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar uma consulta paginada do banco de dados.
        /// </summary>
        /// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
        /// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
        /// <param name="paginaCorrente">Número da página que será exibida.</param>
        /// <param name="tamanhoPagina">Quantidade de itens em cada página.</param>
        /// <param name="totalRegistros">Quantidade total de registros na tabela.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
            int fim = paginaCorrente * tamanhoPagina;

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Tipo_Retorno_Campanha_Recrutamento, Tip.Des_Tipo_Retorno_Campanha_Recrutamento FROM BNE_Tipo_Retorno_Campanha_Recrutamento Tip";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de TipoRetornoCampanhaRecrutamento a partir do banco de dados.
        /// </summary>
        /// <param name="idTipoRetornoCampanhaRecrutamento">Chave do registro.</param>
        /// <returns>Instância de TipoRetornoCampanhaRecrutamento.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static TipoRetornoCampanhaRecrutamento LoadObject(Int16 idTipoRetornoCampanhaRecrutamento)
        {
            using (IDataReader dr = LoadDataReader(idTipoRetornoCampanhaRecrutamento))
            {
                TipoRetornoCampanhaRecrutamento objTipoRetornoCampanhaRecrutamento = new TipoRetornoCampanhaRecrutamento();
                if (SetInstance(dr, objTipoRetornoCampanhaRecrutamento))
                    return objTipoRetornoCampanhaRecrutamento;
            }
            throw (new RecordNotFoundException(typeof(TipoRetornoCampanhaRecrutamento)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de TipoRetornoCampanhaRecrutamento a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idTipoRetornoCampanhaRecrutamento">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de TipoRetornoCampanhaRecrutamento.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static TipoRetornoCampanhaRecrutamento LoadObject(Int16 idTipoRetornoCampanhaRecrutamento, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idTipoRetornoCampanhaRecrutamento, trans))
            {
                TipoRetornoCampanhaRecrutamento objTipoRetornoCampanhaRecrutamento = new TipoRetornoCampanhaRecrutamento();
                if (SetInstance(dr, objTipoRetornoCampanhaRecrutamento))
                    return objTipoRetornoCampanhaRecrutamento;
            }
            throw (new RecordNotFoundException(typeof(TipoRetornoCampanhaRecrutamento)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de TipoRetornoCampanhaRecrutamento a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idTipoRetornoCampanhaRecrutamento))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de TipoRetornoCampanhaRecrutamento a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idTipoRetornoCampanhaRecrutamento, trans))
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
        /// <param name="objTipoRetornoCampanhaRecrutamento">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, TipoRetornoCampanhaRecrutamento objTipoRetornoCampanhaRecrutamento, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objTipoRetornoCampanhaRecrutamento._idTipoRetornoCampanhaRecrutamento = Convert.ToInt16(dr["Idf_Tipo_Retorno_Campanha_Recrutamento"]);
                    if (dr["Des_Tipo_Retorno_Campanha_Recrutamento"] != DBNull.Value)
                        objTipoRetornoCampanhaRecrutamento._descricaoTipoRetornoCampanhaRecrutamento = Convert.ToString(dr["Des_Tipo_Retorno_Campanha_Recrutamento"]);

                    objTipoRetornoCampanhaRecrutamento._persisted = true;
                    objTipoRetornoCampanhaRecrutamento._modified = false;

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
                if (dispose)
                    dr.Dispose();
            }
        }
        #endregion

        #endregion
    }
}