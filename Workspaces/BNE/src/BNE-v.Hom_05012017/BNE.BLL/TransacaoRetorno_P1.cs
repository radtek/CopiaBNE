//-- Data: 28/02/2013 11:38
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TransacaoRetorno // Tabela: BNE_Transacao_Retorno
	{
		#region Atributos
		private int _idTransacaoRetorno;
		private Transacao _transacao;
		private DateTime _dataCadastro;
		private DateTime? _dataStatus;
		private bool? _flagAprovada;
		private string _descricaoAutorizacao;
		private string _descricaoMotivoNaoFinalizada;
		private string _descricaoNaoFinalizada;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTransacaoRetorno
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdTransacaoRetorno
		{
			get
			{
				return this._idTransacaoRetorno;
			}
		}
		#endregion 

		#region Transacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Transacao Transacao
		{
			get
			{
				return this._transacao;
			}
			set
			{
				this._transacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadastro
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataCadastro
		{
			get
			{
				return this._dataCadastro;
			}
		}
		#endregion 

		#region DataStatus
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataStatus
		{
			get
			{
				return this._dataStatus;
			}
			set
			{
				this._dataStatus = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagAprovada
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagAprovada
		{
			get
			{
				return this._flagAprovada;
			}
			set
			{
				this._flagAprovada = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoAutorizacao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoAutorizacao
		{
			get
			{
				return this._descricaoAutorizacao;
			}
			set
			{
				this._descricaoAutorizacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMotivoNaoFinalizada
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoMotivoNaoFinalizada
		{
			get
			{
				return this._descricaoMotivoNaoFinalizada;
			}
			set
			{
				this._descricaoMotivoNaoFinalizada = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoNaoFinalizada
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string DescricaoNaoFinalizada
		{
			get
			{
				return this._descricaoNaoFinalizada;
			}
			set
			{
				this._descricaoNaoFinalizada = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public TransacaoRetorno()
		{
		}
		public TransacaoRetorno(int idTransacaoRetorno)
		{
			this._idTransacaoRetorno = idTransacaoRetorno;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Transacao_Retorno (Idf_Transacao, Dta_Cadastro, Dta_Status, Flg_Aprovada, Des_Autorizacao, Des_Motivo_Nao_Finalizada, Des_Nao_Finalizada) VALUES (@Idf_Transacao, @Dta_Cadastro, @Dta_Status, @Flg_Aprovada, @Des_Autorizacao, @Des_Motivo_Nao_Finalizada, @Des_Nao_Finalizada);SET @Idf_Transacao_Retorno = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Transacao_Retorno SET Idf_Transacao = @Idf_Transacao, Dta_Cadastro = @Dta_Cadastro, Dta_Status = @Dta_Status, Flg_Aprovada = @Flg_Aprovada, Des_Autorizacao = @Des_Autorizacao, Des_Motivo_Nao_Finalizada = @Des_Motivo_Nao_Finalizada, Des_Nao_Finalizada = @Des_Nao_Finalizada WHERE Idf_Transacao_Retorno = @Idf_Transacao_Retorno";
		private const string SPDELETE = "DELETE FROM BNE_Transacao_Retorno WHERE Idf_Transacao_Retorno = @Idf_Transacao_Retorno";
		private const string SPSELECTID = "SELECT * FROM BNE_Transacao_Retorno WITH(NOLOCK) WHERE Idf_Transacao_Retorno = @Idf_Transacao_Retorno";
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
			parms.Add(new SqlParameter("@Idf_Transacao_Retorno", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Status", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Aprovada", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_Autorizacao", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Des_Motivo_Nao_Finalizada", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Des_Nao_Finalizada", SqlDbType.VarChar));
			return(parms);
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
			parms[0].Value = this._idTransacaoRetorno;
			parms[1].Value = this._transacao.IdTransacao;

			if (this._dataStatus.HasValue)
				parms[3].Value = this._dataStatus;
			else
				parms[3].Value = DBNull.Value;


			if (this._flagAprovada.HasValue)
				parms[4].Value = this._flagAprovada;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoAutorizacao))
				parms[5].Value = this._descricaoAutorizacao;
			else
				parms[5].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoMotivoNaoFinalizada))
				parms[6].Value = this._descricaoMotivoNaoFinalizada;
			else
				parms[6].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoNaoFinalizada))
				parms[7].Value = this._descricaoNaoFinalizada;
			else
				parms[7].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TransacaoRetorno no banco de dados.
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
						this._idTransacaoRetorno = Convert.ToInt32(cmd.Parameters["@Idf_Transacao_Retorno"].Value);
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
		/// Método utilizado para inserir uma instância de TransacaoRetorno no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idTransacaoRetorno = Convert.ToInt32(cmd.Parameters["@Idf_Transacao_Retorno"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de TransacaoRetorno no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TransacaoRetorno no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TransacaoRetorno no banco de dados.
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
		/// Método utilizado para salvar uma instância de TransacaoRetorno no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TransacaoRetorno no banco de dados.
		/// </summary>
		/// <param name="idTransacaoRetorno">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTransacaoRetorno)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Retorno", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoRetorno;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TransacaoRetorno no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTransacaoRetorno">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTransacaoRetorno, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Retorno", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoRetorno;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TransacaoRetorno no banco de dados.
		/// </summary>
		/// <param name="idTransacaoRetorno">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idTransacaoRetorno)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Transacao_Retorno where Idf_Transacao_Retorno in (";

			for (int i = 0; i < idTransacaoRetorno.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTransacaoRetorno[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTransacaoRetorno">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTransacaoRetorno)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Retorno", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoRetorno;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTransacaoRetorno">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTransacaoRetorno, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Retorno", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoRetorno;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tra.Idf_Transacao_Retorno, Tra.Idf_Transacao, Tra.Dta_Cadastro, Tra.Dta_Status, Tra.Flg_Aprovada, Tra.Des_Autorizacao, Tra.Des_Motivo_Nao_Finalizada, Tra.Des_Nao_Finalizada FROM BNE_Transacao_Retorno Tra";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TransacaoRetorno a partir do banco de dados.
		/// </summary>
		/// <param name="idTransacaoRetorno">Chave do registro.</param>
		/// <returns>Instância de TransacaoRetorno.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TransacaoRetorno LoadObject(int idTransacaoRetorno)
		{
			using (IDataReader dr = LoadDataReader(idTransacaoRetorno))
			{
				TransacaoRetorno objTransacaoRetorno = new TransacaoRetorno();
				if (SetInstance(dr, objTransacaoRetorno))
					return objTransacaoRetorno;
			}
			throw (new RecordNotFoundException(typeof(TransacaoRetorno)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TransacaoRetorno a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTransacaoRetorno">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TransacaoRetorno.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TransacaoRetorno LoadObject(int idTransacaoRetorno, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTransacaoRetorno, trans))
			{
				TransacaoRetorno objTransacaoRetorno = new TransacaoRetorno();
				if (SetInstance(dr, objTransacaoRetorno))
					return objTransacaoRetorno;
			}
			throw (new RecordNotFoundException(typeof(TransacaoRetorno)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TransacaoRetorno a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTransacaoRetorno))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TransacaoRetorno a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTransacaoRetorno, trans))
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
		/// <param name="objTransacaoRetorno">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, TransacaoRetorno objTransacaoRetorno)
		{
			try
			{
				if (dr.Read())
				{
					objTransacaoRetorno._idTransacaoRetorno = Convert.ToInt32(dr["Idf_Transacao_Retorno"]);
					objTransacaoRetorno._transacao = new Transacao(Convert.ToInt32(dr["Idf_Transacao"]));
					objTransacaoRetorno._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Dta_Status"] != DBNull.Value)
						objTransacaoRetorno._dataStatus = Convert.ToDateTime(dr["Dta_Status"]);
					if (dr["Flg_Aprovada"] != DBNull.Value)
						objTransacaoRetorno._flagAprovada = Convert.ToBoolean(dr["Flg_Aprovada"]);
					if (dr["Des_Autorizacao"] != DBNull.Value)
						objTransacaoRetorno._descricaoAutorizacao = Convert.ToString(dr["Des_Autorizacao"]);
					if (dr["Des_Motivo_Nao_Finalizada"] != DBNull.Value)
						objTransacaoRetorno._descricaoMotivoNaoFinalizada = Convert.ToString(dr["Des_Motivo_Nao_Finalizada"]);
					if (dr["Des_Nao_Finalizada"] != DBNull.Value)
						objTransacaoRetorno._descricaoNaoFinalizada = Convert.ToString(dr["Des_Nao_Finalizada"]);

					objTransacaoRetorno._persisted = true;
					objTransacaoRetorno._modified = false;

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