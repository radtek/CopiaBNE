//-- Data: 18/02/2016 17:07
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class ParametroVaga // Tabela: BNE_Parametro_Vaga
	{
		#region Atributos
		private Parametro _parametro;
		private Vaga _vaga;
		private DateTime _dataCadastro;
		private string _valorParametro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region Parametro
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Parametro Parametro
		{
			get
			{
				return this._parametro;
			}
			set
			{
				this._parametro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Vaga
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Vaga Vaga
		{
			get
			{
				return this._vaga;
			}
			set
			{
				this._vaga = value;
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

		#region ValorParametro
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string ValorParametro
		{
			get
			{
				return this._valorParametro;
			}
			set
			{
				this._valorParametro = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public ParametroVaga()
		{
		}
		public ParametroVaga(Parametro parametro, Vaga vaga)
		{
			this._parametro = parametro;
			this._vaga = vaga;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Parametro_Vaga (Idf_Parametro, Idf_Vaga, Dta_Cadastro, Vlr_Parametro) VALUES (@Idf_Parametro, @Idf_Vaga, @Dta_Cadastro, @Vlr_Parametro);";
		private const string SPUPDATE = "UPDATE BNE_Parametro_Vaga SET Dta_Cadastro = @Dta_Cadastro, Vlr_Parametro = @Vlr_Parametro WHERE Idf_Parametro = @Idf_Parametro AND Idf_Vaga = @Idf_Vaga";
		private const string SPDELETE = "DELETE FROM BNE_Parametro_Vaga WHERE Idf_Parametro = @Idf_Parametro AND Idf_Vaga = @Idf_Vaga";
		private const string SPSELECTID = "SELECT * FROM BNE_Parametro_Vaga WITH(NOLOCK) WHERE Idf_Parametro = @Idf_Parametro AND Idf_Vaga = @Idf_Vaga";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Mailson</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 3));
			parms.Add(new SqlParameter("@Vlr_Parametro", SqlDbType.VarChar));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Mailson</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._parametro.IdParametro;
			parms[1].Value = this._vaga.IdVaga;

			if (!String.IsNullOrEmpty(this._valorParametro))
				parms[3].Value = this._valorParametro;
			else
				parms[3].Value = DBNull.Value;


			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de ParametroVaga no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para inserir uma instância de ParametroVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para atualizar uma instância de ParametroVaga no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para atualizar uma instância de ParametroVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para salvar uma instância de ParametroVaga no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de ParametroVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para excluir uma instância de ParametroVaga no banco de dados.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idVaga">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idParametro, int idVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idParametro;
			parms[1].Value = idVaga;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de ParametroVaga no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idParametro, int idVaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idParametro;
			parms[1].Value = idVaga;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idVaga">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idParametro, int idVaga)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idParametro;
			parms[1].Value = idVaga;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idParametro, int idVaga, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Parametro", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

			parms[0].Value = idParametro;
			parms[1].Value = idVaga;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Par.Idf_Parametro, Par.Idf_Vaga, Par.Dta_Cadastro, Par.Vlr_Parametro FROM BNE_Parametro_Vaga Par";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de ParametroVaga a partir do banco de dados.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idVaga">Chave do registro.</param>
		/// <returns>Instância de ParametroVaga.</returns>
		/// <remarks>Mailson</remarks>
		public static ParametroVaga LoadObject(int idParametro, int idVaga)
		{
			using (IDataReader dr = LoadDataReader(idParametro, idVaga))
			{
				ParametroVaga objParametroVaga = new ParametroVaga();
				if (SetInstance(dr, objParametroVaga))
					return objParametroVaga;
			}
			throw (new RecordNotFoundException(typeof(ParametroVaga)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de ParametroVaga a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idParametro">Chave do registro.</param>
		/// <param name="idVaga">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de ParametroVaga.</returns>
		/// <remarks>Mailson</remarks>
		public static ParametroVaga LoadObject(int idParametro, int idVaga, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idParametro, idVaga, trans))
			{
				ParametroVaga objParametroVaga = new ParametroVaga();
				if (SetInstance(dr, objParametroVaga))
					return objParametroVaga;
			}
			throw (new RecordNotFoundException(typeof(ParametroVaga)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de ParametroVaga a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._parametro.IdParametro, this._vaga.IdVaga))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de ParametroVaga a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._parametro.IdParametro, this._vaga.IdVaga, trans))
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
		/// <param name="objParametroVaga">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, ParametroVaga objParametroVaga, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objParametroVaga._parametro = new Parametro(Convert.ToInt32(dr["Idf_Parametro"]));
					objParametroVaga._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					objParametroVaga._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Vlr_Parametro"] != DBNull.Value)
						objParametroVaga._valorParametro = Convert.ToString(dr["Vlr_Parametro"]);

					objParametroVaga._persisted = true;
					objParametroVaga._modified = false;

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