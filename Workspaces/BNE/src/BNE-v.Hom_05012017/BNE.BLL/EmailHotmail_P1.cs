//-- Data: 16/06/2016 12:26
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class EmailHotmail // Tabela: TAB_Email_Hotmail
	{
		#region Atributos
		private int _idEmailHotmail;
		private DateTime _dataCadastro;
		private DateTime? _dataEnvio;
		private string _emailDestinatario;
		private string _emailRemetente;
		private string _descricaoAssunto;
		private string _descricaoMensagem;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdEmailHotmail
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdEmailHotmail
		{
			get
			{
				return this._idEmailHotmail;
			}
			set
			{
				this._idEmailHotmail = value;
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

		#region DataEnvio
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataEnvio
		{
			get
			{
				return this._dataEnvio;
			}
			set
			{
				this._dataEnvio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailDestinatario
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string EmailDestinatario
		{
			get
			{
				return this._emailDestinatario;
			}
			set
			{
				this._emailDestinatario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailRemetente
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string EmailRemetente
		{
			get
			{
				return this._emailRemetente;
			}
			set
			{
				this._emailRemetente = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoAssunto
		/// <summary>
		/// Tamanho do campo: 400.
		/// Campo obrigatório.
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

		#region DescricaoMensagem
		/// <summary>
		/// Tamanho do campo: -1.
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

		#endregion

		#region Construtores
		public EmailHotmail()
		{
		}
		public EmailHotmail(int idEmailHotmail)
		{
			this._idEmailHotmail = idEmailHotmail;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Email_Hotmail (Dta_Cadastro, Dta_Envio, Eml_Destinatario, Eml_Remetente, Des_Assunto, Des_Parametros, Des_Mensagem) VALUES (@Dta_Cadastro, @Dta_Envio, @Eml_Destinatario, @Eml_Remetente, @Des_Assunto, @Des_Parametros, @Des_Mensagem);SET @Idf_Email_Hotmail = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Email_Hotmail SET Dta_Cadastro = @Dta_Cadastro, Dta_Envio = @Dta_Envio, Eml_Destinatario = @Eml_Destinatario, Eml_Remetente = @Eml_Remetente, Des_Assunto = @Des_Assunto, Des_Parametros = @Des_Parametros, Des_Mensagem = @Des_Mensagem WHERE Idf_Email_Hotmail = @Idf_Email_Hotmail";
		private const string SPDELETE = "DELETE FROM TAB_Email_Hotmail WHERE Idf_Email_Hotmail = @Idf_Email_Hotmail";
		private const string SPSELECTID = "SELECT * FROM TAB_Email_Hotmail WITH(NOLOCK) WHERE Idf_Email_Hotmail = @Idf_Email_Hotmail";
		#endregion

		#region Métodos

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de EmailHotmail no banco de dados.
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
		/// Método utilizado para inserir uma instância de EmailHotmail no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
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

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de EmailHotmail no banco de dados.
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
		/// Método utilizado para atualizar uma instância de EmailHotmail no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de EmailHotmail no banco de dados.
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
		/// Método utilizado para salvar uma instância de EmailHotmail no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de EmailHotmail no banco de dados.
		/// </summary>
		/// <param name="idEmailHotmail">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEmailHotmail)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Hotmail", SqlDbType.Int, 4));

			parms[0].Value = idEmailHotmail;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de EmailHotmail no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmailHotmail">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEmailHotmail, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Hotmail", SqlDbType.Int, 4));

			parms[0].Value = idEmailHotmail;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de EmailHotmail no banco de dados.
		/// </summary>
		/// <param name="idEmailHotmail">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idEmailHotmail)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Email_Hotmail where Idf_Email_Hotmail in (";

			for (int i = 0; i < idEmailHotmail.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idEmailHotmail[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idEmailHotmail">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEmailHotmail)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Hotmail", SqlDbType.Int, 4));

			parms[0].Value = idEmailHotmail;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmailHotmail">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEmailHotmail, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Hotmail", SqlDbType.Int, 4));

			parms[0].Value = idEmailHotmail;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ema.Idf_Email_Hotmail, Ema.Dta_Cadastro, Ema.Dta_Envio, Ema.Eml_Destinatario, Ema.Eml_Remetente, Ema.Des_Assunto, Ema.Des_Parametros, Ema.Des_Mensagem FROM TAB_Email_Hotmail Ema";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de EmailHotmail a partir do banco de dados.
		/// </summary>
		/// <param name="idEmailHotmail">Chave do registro.</param>
		/// <returns>Instância de EmailHotmail.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static EmailHotmail LoadObject(int idEmailHotmail)
		{
			using (IDataReader dr = LoadDataReader(idEmailHotmail))
			{
				EmailHotmail objEmailHotmail = new EmailHotmail();
				if (SetInstance(dr, objEmailHotmail))
					return objEmailHotmail;
			}
			throw (new RecordNotFoundException(typeof(EmailHotmail)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de EmailHotmail a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmailHotmail">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de EmailHotmail.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static EmailHotmail LoadObject(int idEmailHotmail, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idEmailHotmail, trans))
			{
				EmailHotmail objEmailHotmail = new EmailHotmail();
				if (SetInstance(dr, objEmailHotmail))
					return objEmailHotmail;
			}
			throw (new RecordNotFoundException(typeof(EmailHotmail)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de EmailHotmail a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idEmailHotmail))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de EmailHotmail a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idEmailHotmail, trans))
			{
				return SetInstance(dr, this);
			}
		}
		#endregion

		#endregion
	}
}