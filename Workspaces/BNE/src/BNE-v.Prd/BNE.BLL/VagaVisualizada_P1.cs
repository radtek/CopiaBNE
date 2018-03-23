//-- Data: 05/03/2015 12:05
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class VagaVisualizada // Tabela: BNE_Vaga_Visualizada
	{
		#region Atributos
		private int _idVagaVisualizada;
		private Curriculo _curriculo;
		private Vaga _vaga;
		private DateTime _dataVisualizacao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdVagaVisualizada
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdVagaVisualizada
		{
			get
			{
				return this._idVagaVisualizada;
			}
		}
		#endregion 

		#region Curriculo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Curriculo Curriculo
		{
			get
			{
				return this._curriculo;
			}
			set
			{
				this._curriculo = value;
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

		#region DataVisualizacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataVisualizacao
		{
			get
			{
				return this._dataVisualizacao;
			}
			set
			{
				this._dataVisualizacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public VagaVisualizada()
		{
		}
		public VagaVisualizada(int idVagaVisualizada)
		{
			this._idVagaVisualizada = idVagaVisualizada;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Vaga_Visualizada (Idf_Curriculo, Idf_Vaga, Dta_Visualizacao) VALUES (@Idf_Curriculo, @Idf_Vaga, @Dta_Visualizacao);SET @Idf_Vaga_Visualizada = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Vaga_Visualizada SET Idf_Curriculo = @Idf_Curriculo, Idf_Vaga = @Idf_Vaga, Dta_Visualizacao = @Dta_Visualizacao WHERE Idf_Vaga_Visualizada = @Idf_Vaga_Visualizada";
		private const string SPDELETE = "DELETE FROM BNE_Vaga_Visualizada WHERE Idf_Vaga_Visualizada = @Idf_Vaga_Visualizada";
		private const string SPSELECTID = "SELECT * FROM BNE_Vaga_Visualizada WITH(NOLOCK) WHERE Idf_Vaga_Visualizada = @Idf_Vaga_Visualizada";
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
			parms.Add(new SqlParameter("@Idf_Vaga_Visualizada", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Visualizacao", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idVagaVisualizada;

			if (this._curriculo != null)
				parms[1].Value = this._curriculo.IdCurriculo;
			else
				parms[1].Value = DBNull.Value;

			parms[2].Value = this._vaga.IdVaga;
			parms[3].Value = this._dataVisualizacao;

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
		/// Método utilizado para inserir uma instância de VagaVisualizada no banco de dados.
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
						this._idVagaVisualizada = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Visualizada"].Value);
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
		/// Método utilizado para inserir uma instância de VagaVisualizada no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idVagaVisualizada = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Visualizada"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de VagaVisualizada no banco de dados.
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
		/// Método utilizado para atualizar uma instância de VagaVisualizada no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de VagaVisualizada no banco de dados.
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
		/// Método utilizado para salvar uma instância de VagaVisualizada no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de VagaVisualizada no banco de dados.
		/// </summary>
		/// <param name="idVagaVisualizada">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idVagaVisualizada)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Visualizada", SqlDbType.Int, 4));

			parms[0].Value = idVagaVisualizada;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de VagaVisualizada no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaVisualizada">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idVagaVisualizada, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Visualizada", SqlDbType.Int, 4));

			parms[0].Value = idVagaVisualizada;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de VagaVisualizada no banco de dados.
		/// </summary>
		/// <param name="idVagaVisualizada">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idVagaVisualizada)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Vaga_Visualizada where Idf_Vaga_Visualizada in (";

			for (int i = 0; i < idVagaVisualizada.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idVagaVisualizada[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idVagaVisualizada">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idVagaVisualizada)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Visualizada", SqlDbType.Int, 4));

			parms[0].Value = idVagaVisualizada;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaVisualizada">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idVagaVisualizada, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Vaga_Visualizada", SqlDbType.Int, 4));

			parms[0].Value = idVagaVisualizada;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Vag.Idf_Vaga_Visualizada, Vag.Idf_Curriculo, Vag.Idf_Vaga, Vag.Dta_Visualizacao FROM BNE_Vaga_Visualizada Vag";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaVisualizada a partir do banco de dados.
		/// </summary>
		/// <param name="idVagaVisualizada">Chave do registro.</param>
		/// <returns>Instância de VagaVisualizada.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static VagaVisualizada LoadObject(int idVagaVisualizada)
		{
			using (IDataReader dr = LoadDataReader(idVagaVisualizada))
			{
				VagaVisualizada objVagaVisualizada = new VagaVisualizada();
				if (SetInstance(dr, objVagaVisualizada))
					return objVagaVisualizada;
			}
			throw (new RecordNotFoundException(typeof(VagaVisualizada)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de VagaVisualizada a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idVagaVisualizada">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de VagaVisualizada.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static VagaVisualizada LoadObject(int idVagaVisualizada, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idVagaVisualizada, trans))
			{
				VagaVisualizada objVagaVisualizada = new VagaVisualizada();
				if (SetInstance(dr, objVagaVisualizada))
					return objVagaVisualizada;
			}
			throw (new RecordNotFoundException(typeof(VagaVisualizada)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de VagaVisualizada a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idVagaVisualizada))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de VagaVisualizada a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idVagaVisualizada, trans))
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
		/// <param name="objVagaVisualizada">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, VagaVisualizada objVagaVisualizada)
		{
			try
			{
				if (dr.Read())
				{
					objVagaVisualizada._idVagaVisualizada = Convert.ToInt32(dr["Idf_Vaga_Visualizada"]);
					if (dr["Idf_Curriculo"] != DBNull.Value)
						objVagaVisualizada._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objVagaVisualizada._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					objVagaVisualizada._dataVisualizacao = Convert.ToDateTime(dr["Dta_Visualizacao"]);

					objVagaVisualizada._persisted = true;
					objVagaVisualizada._modified = false;

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