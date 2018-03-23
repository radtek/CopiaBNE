//-- Data: 16/07/2015 17:45
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CelularLog // Tabela: plataforma.Tab_Celular_Log
	{
		#region Atributos
		private int _idCelularLog;
		private string _numeroDDDCelularLog;
		private string _numeroCelularLog;
		private OperadoraCelular _operadoraCelular;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCelularLog
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCelularLog
		{
			get
			{
				return this._idCelularLog;
			}
		}
		#endregion 

		#region NumeroDDDCelularLog
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo obrigatório.
		/// </summary>
		public string NumeroDDDCelularLog
		{
			get
			{
				return this._numeroDDDCelularLog;
			}
			set
			{
				this._numeroDDDCelularLog = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCelularLog
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo obrigatório.
		/// </summary>
		public string NumeroCelularLog
		{
			get
			{
				return this._numeroCelularLog;
			}
			set
			{
				this._numeroCelularLog = value;
				this._modified = true;
			}
		}
		#endregion 

		#region OperadoraCelular
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public OperadoraCelular OperadoraCelular
		{
			get
			{
				return this._operadoraCelular;
			}
			set
			{
				this._operadoraCelular = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CelularLog()
		{
		}
		public CelularLog(int idCelularLog)
		{
			this._idCelularLog = idCelularLog;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.Tab_Celular_Log (Num_DDD_Celular_Log, Num_Celular_Log, Idf_Operadora_Celular) VALUES (@Num_DDD_Celular_Log, @Num_Celular_Log, @Idf_Operadora_Celular);SET @Idf_Celular_Log = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE plataforma.Tab_Celular_Log SET Num_DDD_Celular_Log = @Num_DDD_Celular_Log, Num_Celular_Log = @Num_Celular_Log, Idf_Operadora_Celular = @Idf_Operadora_Celular WHERE Idf_Celular_Log = @Idf_Celular_Log";
		private const string SPDELETE = "DELETE FROM plataforma.Tab_Celular_Log WHERE Idf_Celular_Log = @Idf_Celular_Log";
		private const string SPSELECTID = "SELECT * FROM plataforma.Tab_Celular_Log WITH(NOLOCK) WHERE Idf_Celular_Log = @Idf_Celular_Log";
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
			parms.Add(new SqlParameter("@Idf_Celular_Log", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Num_DDD_Celular_Log", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Celular_Log", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Idf_Operadora_Celular", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCelularLog;
			parms[1].Value = this._numeroDDDCelularLog;
			parms[2].Value = this._numeroCelularLog;

			if (this._operadoraCelular != null)
				parms[3].Value = this._operadoraCelular.IdOperadoraCelular;
			else
				parms[3].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de CelularLog no banco de dados.
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
						this._idCelularLog = Convert.ToInt32(cmd.Parameters["@Idf_Celular_Log"].Value);
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
		/// Método utilizado para inserir uma instância de CelularLog no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCelularLog = Convert.ToInt32(cmd.Parameters["@Idf_Celular_Log"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CelularLog no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CelularLog no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CelularLog no banco de dados.
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
		/// Método utilizado para salvar uma instância de CelularLog no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CelularLog no banco de dados.
		/// </summary>
		/// <param name="idCelularLog">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCelularLog)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Celular_Log", SqlDbType.Int, 4));

			parms[0].Value = idCelularLog;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CelularLog no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCelularLog">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCelularLog, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Celular_Log", SqlDbType.Int, 4));

			parms[0].Value = idCelularLog;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CelularLog no banco de dados.
		/// </summary>
		/// <param name="idCelularLog">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCelularLog)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.Tab_Celular_Log where Idf_Celular_Log in (";

			for (int i = 0; i < idCelularLog.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCelularLog[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCelularLog">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCelularLog)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Celular_Log", SqlDbType.Int, 4));

			parms[0].Value = idCelularLog;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCelularLog">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCelularLog, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Celular_Log", SqlDbType.Int, 4));

			parms[0].Value = idCelularLog;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cel.Idf_Celular_Log, Cel.Num_DDD_Celular_Log, Cel.Num_Celular_Log, Cel.Idf_Operadora_Celular FROM plataforma.Tab_Celular_Log Cel";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CelularLog a partir do banco de dados.
		/// </summary>
		/// <param name="idCelularLog">Chave do registro.</param>
		/// <returns>Instância de CelularLog.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CelularLog LoadObject(int idCelularLog)
		{
			using (IDataReader dr = LoadDataReader(idCelularLog))
			{
				CelularLog objCelularLog = new CelularLog();
				if (SetInstance(dr, objCelularLog))
					return objCelularLog;
			}
			throw (new RecordNotFoundException(typeof(CelularLog)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CelularLog a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCelularLog">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CelularLog.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CelularLog LoadObject(int idCelularLog, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCelularLog, trans))
			{
				CelularLog objCelularLog = new CelularLog();
				if (SetInstance(dr, objCelularLog))
					return objCelularLog;
			}
			throw (new RecordNotFoundException(typeof(CelularLog)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CelularLog a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCelularLog))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CelularLog a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCelularLog, trans))
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
		/// <param name="objCelularLog">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CelularLog objCelularLog, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objCelularLog._idCelularLog = Convert.ToInt32(dr["Idf_Celular_Log"]);
					objCelularLog._numeroDDDCelularLog = Convert.ToString(dr["Num_DDD_Celular_Log"]);
					objCelularLog._numeroCelularLog = Convert.ToString(dr["Num_Celular_Log"]);
					if (dr["Idf_Operadora_Celular"] != DBNull.Value)
						objCelularLog._operadoraCelular = new OperadoraCelular(Convert.ToInt32(dr["Idf_Operadora_Celular"]));

					objCelularLog._persisted = true;
					objCelularLog._modified = false;

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