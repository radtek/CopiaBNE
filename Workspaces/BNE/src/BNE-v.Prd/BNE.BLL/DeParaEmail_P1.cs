//-- Data: 27/06/2014 11:37
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class DeParaEmail // Tabela: plataforma.TAB_DePara_Email
	{
		#region Atributos
		private int _idDeParaEmail;
		private string _descricaoEmailErro;
		private string _descricaoEmailCorreto;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdDeParaEmail
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdDeParaEmail
		{
			get
			{
				return this._idDeParaEmail;
			}
		}
		#endregion 

		#region DescricaoEmailErro
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoEmailErro
		{
			get
			{
				return this._descricaoEmailErro;
			}
			set
			{
				this._descricaoEmailErro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEmailCorreto
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoEmailCorreto
		{
			get
			{
				return this._descricaoEmailCorreto;
			}
			set
			{
				this._descricaoEmailCorreto = value;
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

		#endregion

		#region Construtores
		public DeParaEmail()
		{
		}
		public DeParaEmail(int idDeParaEmail)
		{
			this._idDeParaEmail = idDeParaEmail;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_DePara_Email (Des_Email_Erro, Des_Email_Correto, Dta_Cadastro) VALUES (@Des_Email_Erro, @Des_Email_Correto, @Dta_Cadastro);SET @Idf_DePara_Email = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE plataforma.TAB_DePara_Email SET Des_Email_Erro = @Des_Email_Erro, Des_Email_Correto = @Des_Email_Correto, Dta_Cadastro = @Dta_Cadastro WHERE Idf_DePara_Email = @Idf_DePara_Email";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_DePara_Email WHERE Idf_DePara_Email = @Idf_DePara_Email";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_DePara_Email WHERE Idf_DePara_Email = @Idf_DePara_Email";
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
			parms.Add(new SqlParameter("@Idf_DePara_Email", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Email_Erro", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Des_Email_Correto", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idDeParaEmail;
			parms[1].Value = this._descricaoEmailErro;
			parms[2].Value = this._descricaoEmailCorreto;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de DeParaEmail no banco de dados.
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
						this._idDeParaEmail = Convert.ToInt32(cmd.Parameters["@Idf_DePara_Email"].Value);
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
		/// Método utilizado para inserir uma instância de DeParaEmail no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idDeParaEmail = Convert.ToInt32(cmd.Parameters["@Idf_DePara_Email"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de DeParaEmail no banco de dados.
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
		/// Método utilizado para atualizar uma instância de DeParaEmail no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de DeParaEmail no banco de dados.
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
		/// Método utilizado para salvar uma instância de DeParaEmail no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de DeParaEmail no banco de dados.
		/// </summary>
		/// <param name="idDeParaEmail">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idDeParaEmail)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_DePara_Email", SqlDbType.Int, 4));

			parms[0].Value = idDeParaEmail;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de DeParaEmail no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idDeParaEmail">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idDeParaEmail, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_DePara_Email", SqlDbType.Int, 4));

			parms[0].Value = idDeParaEmail;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de DeParaEmail no banco de dados.
		/// </summary>
		/// <param name="idDeParaEmail">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idDeParaEmail)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_DePara_Email where Idf_DePara_Email in (";

			for (int i = 0; i < idDeParaEmail.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idDeParaEmail[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idDeParaEmail">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idDeParaEmail)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_DePara_Email", SqlDbType.Int, 4));

			parms[0].Value = idDeParaEmail;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idDeParaEmail">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idDeParaEmail, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_DePara_Email", SqlDbType.Int, 4));

			parms[0].Value = idDeParaEmail;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, DeP.Idf_DePara_Email, DeP.Des_Email_Erro, DeP.Des_Email_Correto, DeP.Dta_Cadastro FROM plataforma.TAB_DePara_Email DeP";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de DeParaEmail a partir do banco de dados.
		/// </summary>
		/// <param name="idDeParaEmail">Chave do registro.</param>
		/// <returns>Instância de DeParaEmail.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static DeParaEmail LoadObject(int idDeParaEmail)
		{
			using (IDataReader dr = LoadDataReader(idDeParaEmail))
			{
				DeParaEmail objDeParaEmail = new DeParaEmail();
				if (SetInstance(dr, objDeParaEmail))
					return objDeParaEmail;
			}
			throw (new RecordNotFoundException(typeof(DeParaEmail)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de DeParaEmail a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idDeParaEmail">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de DeParaEmail.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static DeParaEmail LoadObject(int idDeParaEmail, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idDeParaEmail, trans))
			{
				DeParaEmail objDeParaEmail = new DeParaEmail();
				if (SetInstance(dr, objDeParaEmail))
					return objDeParaEmail;
			}
			throw (new RecordNotFoundException(typeof(DeParaEmail)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de DeParaEmail a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idDeParaEmail))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de DeParaEmail a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idDeParaEmail, trans))
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
		/// <param name="objDeParaEmail">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, DeParaEmail objDeParaEmail)
		{
			try
			{
				if (dr.Read())
				{
					objDeParaEmail._idDeParaEmail = Convert.ToInt32(dr["Idf_DePara_Email"]);
					objDeParaEmail._descricaoEmailErro = Convert.ToString(dr["Des_Email_Erro"]);
					objDeParaEmail._descricaoEmailCorreto = Convert.ToString(dr["Des_Email_Correto"]);
					objDeParaEmail._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objDeParaEmail._persisted = true;
					objDeParaEmail._modified = false;

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