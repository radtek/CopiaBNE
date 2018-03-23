//-- Data: 23/07/2013 15:25
//-- Autor: Equipe BNE

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class AlertaVagas // Tabela: BNE_Notificacao.alerta.TAB_Mensagem_Mailing
	{
		#region Atributos

        private int _idAlertaVagas;
        private string _descricaoMensagem;
        private string _descricaoEmailRemetente;
        private string _descricaoEmailDestino;
        private string _descricaoAssunto;
        private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		
        #endregion Atributos

		#region Propriedades

		#region IdAlertaVagas
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdAlertaVagas
		{
			get
			{
				return this._idAlertaVagas;
			}
		}
		#endregion 

		#region DescricaoMensagem
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoMensagem
		{
			get
			{
				return this._descricaoMensagem;
			}
			set
			{
				this._descricaoMensagem = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEmailRemetente
		/// <summary>
		/// Campo obrigatório.
        /// Tamanho: 100.
		/// </summary>
		public string DescricaoEmailRemetente
		{
			get
			{
				return this._descricaoEmailRemetente;
			}
			set
			{
				this._descricaoEmailRemetente = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEmailDestino
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public string DescricaoEmailDestino
		{
			get
			{
				return this._descricaoEmailDestino;
			}
			set
			{
				this._descricaoEmailDestino = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoAssunto
		/// <summary>
		/// Campo opcional.
        /// Tamanho: 100.
		/// </summary>
		public string DescricaoAssunto
		{
			get
			{
				return this._descricaoAssunto;
			}
			set
			{
				this._descricaoAssunto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadastro
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime DataCadastro
		{
			get
			{
				return this._dataCadastro;
			}
			set
			{
				this._dataCadastro = value;
				this._modified = true;
			}
		}
		#endregion 

        #endregion Propriedades

        #region Construtores

        public AlertaVagas()
		{
		}
		public AlertaVagas(int idAlertaVagas)
		{
			this._idAlertaVagas = idAlertaVagas;
            this._persisted = true;
		}

		#endregion Construtores

		#region Consultas
        private const string SPINSERT = "INSERT INTO alerta.TAB_Mensagem_Mailing (Idf_Mensagem_CS, Des_Mensagem, Des_Email_Remetente, Des_Email_Destino, Des_Assunto, Dta_Cadastro) VALUES (@Idf_Mensagem_CS, @Des_Mensagem, @Des_Email_Remetente, @Des_Email_Destino, @Des_Assunto, @Dta_Cadastro); SET @Idf_Mensagem_CS = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE alerta.TAB_Mensagem_Mailing SET Des_Mensagem = @Des_Mensagem, Des_Email_Remetente = @Des_Email_Remetente, Des_Email_Destino = @Des_Email_Destino, Des_Assunto = @Des_Assunto, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Mensagem_CS = @Idf_Mensagem_CS";
		private const string SPDELETE = "DELETE FROM alerta.TAB_Mensagem_Mailing WHERE Idf_Mensagem_CS = @Idf_Mensagem_CS";
		private const string SPSELECTID = "SELECT * FROM alerta.TAB_Mensagem_Mailing WITH(NOLOCK) WHERE Idf_Mensagem_CS = @Idf_Mensagem_CS";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Equipe BNE</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Des_Email_Remetente", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Email_Destino", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Des_Assunto", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime));

			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Equipe BNE</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
            parms[0].Value = this._idAlertaVagas;
			parms[1].Value = this._descricaoMensagem;
			parms[2].Value = this._descricaoEmailRemetente;
			parms[3].Value = this._descricaoEmailDestino;
			parms[4].Value = this._descricaoAssunto;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[5].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de AlertaVagas no banco de dados.
		/// </summary>
		/// <remarks>Equipe BNE</remarks>
		private void Insert()
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);

			using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
			{
				conn.Open();
				using (SqlTransaction trans = conn.BeginTransaction())
				{
					try
					{
						SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
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
		/// Método utilizado para inserir uma instância de AlertaVagas no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Equipe BNE</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

        #region BulkInsert
        /// <summary>
        /// Método utilizado para inserir em massa todas as vagas com alerta na AlertaVagas.
        /// </summary>
        /// <remarks>Equipe BNE</remarks>
        private static void BulkInsert(DataTable dt)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.SaveBulkTable(dt, "alerta.TAB_Mensagem_Mailing", trans);
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
        #endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de AlertaVagas no banco de dados.
		/// </summary>
		/// <remarks>Equipe BNE</remarks>
		private void Update()
		{
			if (this._modified)
			{
				List<SqlParameter> parms = GetParameters();
				SetParameters(parms);
                using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {

                            DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPUPDATE, parms);
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
		}
		/// <summary>
		/// Método utilizado para atualizar uma instância de AlertaVagas no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Equipe BNE</remarks>
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
		/// Método utilizado para salvar uma instância de AlertaVagas no banco de dados.
		/// </summary>
		/// <remarks>Equipe BNE</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de AlertaVagas no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Equipe BNE</remarks>
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
		/// Método utilizado para excluir uma instância de AlertaVagas no banco de dados.
		/// </summary>
		/// <param name="idAlertaVagas">Chave do registro.</param>
		/// <remarks>Equipe BNE</remarks>
		public static void Delete(int idAlertaVagas)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idAlertaVagas;

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
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
		/// Método utilizado para excluir uma instância de AlertaVagas no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAlertaVagas">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Equipe BNE</remarks>
		public static void Delete(int idAlertaVagas, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idAlertaVagas;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idAlertaVagas">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Equipe BNE</remarks>
		private static IDataReader LoadDataReader(int idAlertaVagas)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idAlertaVagas;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms, DataAccessLayer.CONN_NOTIFICACAO);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAlertaVagas">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Equipe BNE</remarks>
		private static IDataReader LoadDataReader(int idAlertaVagas, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idAlertaVagas;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, mm.Idf_Mensagem_CS, mm.Des_Mensagem, mm.Des_Email_Remetente, mm.Des_Email_Destino, mm.Des_Assunto, mm.Dta_Cadastro FROM alerta.TAB_Mensagem_Mailing mm";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null, DataAccessLayer.CONN_NOTIFICACAO);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaVagas a partir do banco de dados.
		/// </summary>
		/// <param name="idAlertaVagas">Chave do registro.</param>
		/// <returns>Instância de AlertaVagas.</returns>
		/// <remarks>Equipe BNE</remarks>
		public static AlertaVagas LoadObject(int idAlertaVagas)
		{
			using (IDataReader dr = LoadDataReader(idAlertaVagas))
			{
				AlertaVagas objAlertaVagas = new AlertaVagas();
				if (SetInstance(dr, objAlertaVagas))
					return objAlertaVagas;
			}
			throw (new RecordNotFoundException(typeof(AlertaVagas)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaVagas a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAlertaVagas">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de AlertaVagas.</returns>
		/// <remarks>Equipe BNE</remarks>
		public static AlertaVagas LoadObject(int idAlertaVagas, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idAlertaVagas, trans))
			{
				AlertaVagas objAlertaVagas = new AlertaVagas();
				if (SetInstance(dr, objAlertaVagas))
					return objAlertaVagas;
			}
			throw (new RecordNotFoundException(typeof(AlertaVagas)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaVagas a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Equipe BNE</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idAlertaVagas))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaVagas a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Equipe BNE</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idAlertaVagas, trans))
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
        /// <param name="objAlertaVagas">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Equipe BNE</remarks>
        private static bool SetInstance(IDataReader dr, AlertaVagas objAlertaVagas)
        {
            try
            {
                if (dr.Read())
                {
                    objAlertaVagas._idAlertaVagas = Convert.ToInt32(dr["Idf_Mensagem_CS"]);
                    objAlertaVagas._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);
                    objAlertaVagas._descricaoEmailRemetente = Convert.ToString(dr["Des_Email_Remetente"]);
                    if (dr["Des_Email_Destino"] != DBNull.Value)
                        objAlertaVagas._descricaoEmailDestino = Convert.ToString(dr["Des_Email_Destino"]);
                    if (dr["Des_Assunto"] != DBNull.Value)
                        objAlertaVagas._descricaoAssunto = Convert.ToString(dr["Des_Assunto"]);
                    objAlertaVagas._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                    objAlertaVagas._persisted = true;
                    objAlertaVagas._modified = false;

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

        #region SetInstance_NoDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objAlertaVagas">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Equipe BNE</remarks>
        private static bool SetInstance_NoDispose(IDataReader dr, AlertaVagas objAlertaVagas)
        {
            objAlertaVagas._idAlertaVagas = Convert.ToInt32(dr["Idf_Mensagem_CS"]);
            objAlertaVagas._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);
            objAlertaVagas._descricaoEmailRemetente = Convert.ToString(dr["Des_Email_Remetente"]);
            if (dr["Des_Email_Destino"] != DBNull.Value)
                objAlertaVagas._descricaoEmailDestino = Convert.ToString(dr["Des_Email_Destino"]);
            if (dr["Des_Assunto"] != DBNull.Value)
                objAlertaVagas._descricaoAssunto = Convert.ToString(dr["Des_Assunto"]);
            objAlertaVagas._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

            objAlertaVagas._persisted = true;
            objAlertaVagas._modified = false;

            return true;
        }
        #endregion

		#endregion
	}
}