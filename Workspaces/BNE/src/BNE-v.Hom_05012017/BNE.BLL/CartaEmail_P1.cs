//-- Data: 15/02/2013 10:12
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CartaEmail // Tabela: BNE_Carta_Email
	{
		#region Atributos
		private int _idCartaEmail;
		private string _nomeCartaEmail;
		private string _valorCartaEmail;
		private DateTime _dataCadastro;
		private string _descricaoAssunto;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCartaEmail
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCartaEmail
		{
			get
			{
				return this._idCartaEmail;
			}
			set
			{
				this._idCartaEmail = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeCartaEmail
		/// <summary>
		/// Tamanho do campo: 70.
		/// Campo obrigatório.
		/// </summary>
		public string NomeCartaEmail
		{
			get
			{
				return this._nomeCartaEmail;
			}
			set
			{
				this._nomeCartaEmail = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorCartaEmail
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string ValorCartaEmail
		{
			get
			{
				return this._valorCartaEmail;
			}
			set
			{
				this._valorCartaEmail = value;
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

		#region DescricaoAssunto
		/// <summary>
		/// Tamanho do campo: 100.
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

		#endregion

		#region Construtores
		public CartaEmail()
		{
		}
		public CartaEmail(int idCartaEmail)
		{
			this._idCartaEmail = idCartaEmail;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Carta_Email (Idf_Carta_Email, Nme_Carta_Email, Vlr_Carta_Email, Dta_Cadastro, Des_Assunto) VALUES (@Idf_Carta_Email, @Nme_Carta_Email, @Vlr_Carta_Email, @Dta_Cadastro, @Des_Assunto);";
		private const string SPUPDATE = "UPDATE BNE_Carta_Email SET Nme_Carta_Email = @Nme_Carta_Email, Vlr_Carta_Email = @Vlr_Carta_Email, Dta_Cadastro = @Dta_Cadastro, Des_Assunto = @Des_Assunto WHERE Idf_Carta_Email = @Idf_Carta_Email";
		private const string SPDELETE = "DELETE FROM BNE_Carta_Email WHERE Idf_Carta_Email = @Idf_Carta_Email";
		private const string SPSELECTID = "SELECT * FROM BNE_Carta_Email WITH(NOLOCK) WHERE Idf_Carta_Email = @Idf_Carta_Email";
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
			parms.Add(new SqlParameter("@Idf_Carta_Email", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Carta_Email", SqlDbType.VarChar, 70));
			parms.Add(new SqlParameter("@Vlr_Carta_Email", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Assunto", SqlDbType.VarChar, 100));
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
			parms[0].Value = this._idCartaEmail;
			parms[1].Value = this._nomeCartaEmail;
			parms[2].Value = this._valorCartaEmail;
			parms[4].Value = this._descricaoAssunto;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CartaEmail no banco de dados.
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
		/// Método utilizado para inserir uma instância de CartaEmail no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de CartaEmail no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CartaEmail no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CartaEmail no banco de dados.
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
		/// Método utilizado para salvar uma instância de CartaEmail no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CartaEmail no banco de dados.
		/// </summary>
		/// <param name="idCartaEmail">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCartaEmail)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_Email", SqlDbType.Int, 4));

			parms[0].Value = idCartaEmail;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CartaEmail no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCartaEmail">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCartaEmail, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_Email", SqlDbType.Int, 4));

			parms[0].Value = idCartaEmail;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CartaEmail no banco de dados.
		/// </summary>
		/// <param name="idCartaEmail">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCartaEmail)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Carta_Email where Idf_Carta_Email in (";

			for (int i = 0; i < idCartaEmail.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCartaEmail[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCartaEmail">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCartaEmail)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_Email", SqlDbType.Int, 4));

			parms[0].Value = idCartaEmail;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCartaEmail">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCartaEmail, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_Email", SqlDbType.Int, 4));

			parms[0].Value = idCartaEmail;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Car.Idf_Carta_Email, Car.Nme_Carta_Email, Car.Vlr_Carta_Email, Car.Dta_Cadastro, Car.Des_Assunto FROM BNE_Carta_Email Car";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CartaEmail a partir do banco de dados.
		/// </summary>
		/// <param name="idCartaEmail">Chave do registro.</param>
		/// <returns>Instância de CartaEmail.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CartaEmail LoadObject(int idCartaEmail)
		{
			using (IDataReader dr = LoadDataReader(idCartaEmail))
			{
				CartaEmail objCartaEmail = new CartaEmail();
				if (SetInstance(dr, objCartaEmail))
					return objCartaEmail;
			}
			throw (new RecordNotFoundException(typeof(CartaEmail)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CartaEmail a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCartaEmail">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CartaEmail.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CartaEmail LoadObject(int idCartaEmail, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCartaEmail, trans))
			{
				CartaEmail objCartaEmail = new CartaEmail();
				if (SetInstance(dr, objCartaEmail))
					return objCartaEmail;
			}
			throw (new RecordNotFoundException(typeof(CartaEmail)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CartaEmail a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCartaEmail))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CartaEmail a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCartaEmail, trans))
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
		/// <param name="objCartaEmail">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CartaEmail objCartaEmail)
		{
			try
			{
				if (dr.Read())
				{
					objCartaEmail._idCartaEmail = Convert.ToInt32(dr["Idf_Carta_Email"]);
					objCartaEmail._nomeCartaEmail = Convert.ToString(dr["Nme_Carta_Email"]);
					objCartaEmail._valorCartaEmail = Convert.ToString(dr["Vlr_Carta_Email"]);
					objCartaEmail._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objCartaEmail._descricaoAssunto = Convert.ToString(dr["Des_Assunto"]);

					objCartaEmail._persisted = true;
					objCartaEmail._modified = false;

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