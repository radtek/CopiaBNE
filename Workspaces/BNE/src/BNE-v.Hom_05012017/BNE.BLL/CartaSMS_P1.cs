//-- Data: 02/04/2013 15:45
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CartaSMS // Tabela: BNE_Carta_SMS
	{
		#region Atributos
		private int _idCartaSMS;
		private string _nomeCartaSMS;
		private string _valorCartaSMS;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCartaSMS
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCartaSMS
		{
			get
			{
				return this._idCartaSMS;
			}
			set
			{
				this._idCartaSMS = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeCartaSMS
		/// <summary>
		/// Tamanho do campo: 70.
		/// Campo obrigatório.
		/// </summary>
		public string NomeCartaSMS
		{
			get
			{
				return this._nomeCartaSMS;
			}
			set
			{
				this._nomeCartaSMS = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorCartaSMS
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string ValorCartaSMS
		{
			get
			{
				return this._valorCartaSMS;
			}
			set
			{
				this._valorCartaSMS = value;
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
		public CartaSMS()
		{
		}
		public CartaSMS(int idCartaSMS)
		{
			this._idCartaSMS = idCartaSMS;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Carta_SMS (Idf_Carta_SMS, Nme_Carta_SMS, Vlr_Carta_SMS, Dta_Cadastro) VALUES (@Idf_Carta_SMS, @Nme_Carta_SMS, @Vlr_Carta_SMS, @Dta_Cadastro);";
		private const string SPUPDATE = "UPDATE BNE_Carta_SMS SET Nme_Carta_SMS = @Nme_Carta_SMS, Vlr_Carta_SMS = @Vlr_Carta_SMS, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Carta_SMS = @Idf_Carta_SMS";
		private const string SPDELETE = "DELETE FROM BNE_Carta_SMS WHERE Idf_Carta_SMS = @Idf_Carta_SMS";
		private const string SPSELECTID = "SELECT * FROM BNE_Carta_SMS WITH(NOLOCK) WHERE Idf_Carta_SMS = @Idf_Carta_SMS";
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
			parms.Add(new SqlParameter("@Idf_Carta_SMS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Carta_SMS", SqlDbType.VarChar, 70));
			parms.Add(new SqlParameter("@Vlr_Carta_SMS", SqlDbType.VarChar));
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
			parms[0].Value = this._idCartaSMS;
			parms[1].Value = this._nomeCartaSMS;
			parms[2].Value = this._valorCartaSMS;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CartaSMS no banco de dados.
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
		/// Método utilizado para inserir uma instância de CartaSMS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de CartaSMS no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CartaSMS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CartaSMS no banco de dados.
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
		/// Método utilizado para salvar uma instância de CartaSMS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CartaSMS no banco de dados.
		/// </summary>
		/// <param name="idCartaSMS">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCartaSMS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_SMS", SqlDbType.Int, 4));

			parms[0].Value = idCartaSMS;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CartaSMS no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCartaSMS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCartaSMS, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_SMS", SqlDbType.Int, 4));

			parms[0].Value = idCartaSMS;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CartaSMS no banco de dados.
		/// </summary>
		/// <param name="idCartaSMS">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCartaSMS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Carta_SMS where Idf_Carta_SMS in (";

			for (int i = 0; i < idCartaSMS.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCartaSMS[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCartaSMS">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCartaSMS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_SMS", SqlDbType.Int, 4));

			parms[0].Value = idCartaSMS;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCartaSMS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCartaSMS, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Carta_SMS", SqlDbType.Int, 4));

			parms[0].Value = idCartaSMS;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Car.Idf_Carta_SMS, Car.Nme_Carta_SMS, Car.Vlr_Carta_SMS, Car.Dta_Cadastro FROM BNE_Carta_SMS Car";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CartaSMS a partir do banco de dados.
		/// </summary>
		/// <param name="idCartaSMS">Chave do registro.</param>
		/// <returns>Instância de CartaSMS.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CartaSMS LoadObject(int idCartaSMS)
		{
			using (IDataReader dr = LoadDataReader(idCartaSMS))
			{
				CartaSMS objCartaSMS = new CartaSMS();
				if (SetInstance(dr, objCartaSMS))
					return objCartaSMS;
			}
			throw (new RecordNotFoundException(typeof(CartaSMS)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CartaSMS a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCartaSMS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CartaSMS.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CartaSMS LoadObject(int idCartaSMS, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCartaSMS, trans))
			{
				CartaSMS objCartaSMS = new CartaSMS();
				if (SetInstance(dr, objCartaSMS))
					return objCartaSMS;
			}
			throw (new RecordNotFoundException(typeof(CartaSMS)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CartaSMS a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCartaSMS))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CartaSMS a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCartaSMS, trans))
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
		/// <param name="objCartaSMS">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CartaSMS objCartaSMS)
		{
			try
			{
				if (dr.Read())
				{
					objCartaSMS._idCartaSMS = Convert.ToInt32(dr["Idf_Carta_SMS"]);
					objCartaSMS._nomeCartaSMS = Convert.ToString(dr["Nme_Carta_SMS"]);
					objCartaSMS._valorCartaSMS = Convert.ToString(dr["Vlr_Carta_SMS"]);
					objCartaSMS._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objCartaSMS._persisted = true;
					objCartaSMS._modified = false;

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