//-- Data: 18/09/2013 15:11
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Celular // Tabela: BNE_Celular
	{
		#region Atributos
		private int _idCelular;
		private string _codigoImeiCelular;
		private string _codigoTokenCelular;
		private DateTime _dataCadastro;
		private DateTime? _dataInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCelular
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCelular
		{
			get
			{
				return this._idCelular;
			}
		}
		#endregion 

		#region CodigoImeiCelular
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string CodigoImeiCelular
		{
			get
			{
				return this._codigoImeiCelular;
			}
			set
			{
				this._codigoImeiCelular = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoTokenCelular
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string CodigoTokenCelular
		{
			get
			{
				return this._codigoTokenCelular;
			}
			set
			{
				this._codigoTokenCelular = value;
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

		#region DataInativo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataInativo
		{
			get
			{
				return this._dataInativo;
			}
			set
			{
				this._dataInativo = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Celular()
		{
		}
		public Celular(int idCelular)
		{
			this._idCelular = idCelular;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Celular (Cod_Imei_Celular, Cod_Token_Celular, Dta_Cadastro, Dta_Inativo) VALUES (@Cod_Imei_Celular, @Cod_Token_Celular, @Dta_Cadastro, @Dta_Inativo);SET @Idf_Celular = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Celular SET Cod_Imei_Celular = @Cod_Imei_Celular, Cod_Token_Celular = @Cod_Token_Celular, Dta_Cadastro = @Dta_Cadastro, Dta_Inativo = @Dta_Inativo WHERE Idf_Celular = @Idf_Celular";
		private const string SPDELETE = "DELETE FROM BNE_Celular WHERE Idf_Celular = @Idf_Celular";
		private const string SPSELECTID = "SELECT * FROM BNE_Celular WITH(NOLOCK) WHERE Idf_Celular = @Idf_Celular";
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
			parms.Add(new SqlParameter("@Idf_Celular", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Cod_Imei_Celular", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Cod_Token_Celular", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Inativo", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idCelular;
			parms[1].Value = this._codigoImeiCelular;
			parms[2].Value = this._codigoTokenCelular;

			if (this._dataInativo.HasValue)
				parms[4].Value = this._dataInativo;
			else
				parms[4].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de Celular no banco de dados.
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
						this._idCelular = Convert.ToInt32(cmd.Parameters["@Idf_Celular"].Value);
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
		/// Método utilizado para inserir uma instância de Celular no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCelular = Convert.ToInt32(cmd.Parameters["@Idf_Celular"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Celular no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Celular no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Celular no banco de dados.
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
		/// Método utilizado para salvar uma instância de Celular no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Celular no banco de dados.
		/// </summary>
		/// <param name="idCelular">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCelular)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Celular", SqlDbType.Int, 4));

			parms[0].Value = idCelular;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Celular no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCelular">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCelular, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Celular", SqlDbType.Int, 4));

			parms[0].Value = idCelular;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Celular no banco de dados.
		/// </summary>
		/// <param name="idCelular">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCelular)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Celular where Idf_Celular in (";

			for (int i = 0; i < idCelular.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCelular[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCelular">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCelular)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Celular", SqlDbType.Int, 4));

			parms[0].Value = idCelular;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCelular">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCelular, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Celular", SqlDbType.Int, 4));

			parms[0].Value = idCelular;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cel.Idf_Celular, Cel.Cod_Imei_Celular, Cel.Cod_Token_Celular, Cel.Dta_Cadastro, Cel.Dta_Inativo FROM BNE_Celular Cel";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Celular a partir do banco de dados.
		/// </summary>
		/// <param name="idCelular">Chave do registro.</param>
		/// <returns>Instância de Celular.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Celular LoadObject(int idCelular)
		{
			using (IDataReader dr = LoadDataReader(idCelular))
			{
				Celular objCelular = new Celular();
				if (SetInstance(dr, objCelular))
					return objCelular;
			}
			throw (new RecordNotFoundException(typeof(Celular)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Celular a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCelular">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Celular.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Celular LoadObject(int idCelular, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCelular, trans))
			{
				Celular objCelular = new Celular();
				if (SetInstance(dr, objCelular))
					return objCelular;
			}
			throw (new RecordNotFoundException(typeof(Celular)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Celular a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCelular))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Celular a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCelular, trans))
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
		/// <param name="objCelular">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Celular objCelular)
		{
			try
			{
				if (dr.Read())
				{
					objCelular._idCelular = Convert.ToInt32(dr["Idf_Celular"]);
					objCelular._codigoImeiCelular = Convert.ToString(dr["Cod_Imei_Celular"]);
					objCelular._codigoTokenCelular = Convert.ToString(dr["Cod_Token_Celular"]);
					objCelular._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Dta_Inativo"] != DBNull.Value)
						objCelular._dataInativo = Convert.ToDateTime(dr["Dta_Inativo"]);

					objCelular._persisted = true;
					objCelular._modified = false;

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