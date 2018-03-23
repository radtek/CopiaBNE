//-- Data: 14/10/2015 18:55
//-- Autor: Ribeiro

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PesquisaCurriculoFuncao // Tabela: TAB_Pesquisa_Curriculo_Funcao
	{
		#region Atributos
		private int _idPesquisaCurriculoFuncao;
		private PesquisaCurriculo _pesquisaCurriculo;
		private Funcao _funcao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPesquisaCurriculoFuncao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPesquisaCurriculoFuncao
		{
			get
			{
				return this._idPesquisaCurriculoFuncao;
			}
		}
		#endregion 

		#region PesquisaCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public PesquisaCurriculo PesquisaCurriculo
		{
			get
			{
				return this._pesquisaCurriculo;
			}
			set
			{
				this._pesquisaCurriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Funcao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Funcao Funcao
		{
			get
			{
				return this._funcao;
			}
			set
			{
				this._funcao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PesquisaCurriculoFuncao()
		{
		}
		public PesquisaCurriculoFuncao(int idPesquisaCurriculoFuncao)
		{
			this._idPesquisaCurriculoFuncao = idPesquisaCurriculoFuncao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pesquisa_Curriculo_Funcao (Idf_Pesquisa_Curriculo, Idf_Funcao) VALUES (@Idf_Pesquisa_Curriculo, @Idf_Funcao);SET @Idf_Pesquisa_Curriculo_Funcao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pesquisa_Curriculo_Funcao SET Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo, Idf_Funcao = @Idf_Funcao WHERE Idf_Pesquisa_Curriculo_Funcao = @Idf_Pesquisa_Curriculo_Funcao";
		private const string SPDELETE = "DELETE FROM TAB_Pesquisa_Curriculo_Funcao WHERE Idf_Pesquisa_Curriculo_Funcao = @Idf_Pesquisa_Curriculo_Funcao";
		private const string SPSELECTID = "SELECT * FROM TAB_Pesquisa_Curriculo_Funcao WITH(NOLOCK) WHERE Idf_Pesquisa_Curriculo_Funcao = @Idf_Pesquisa_Curriculo_Funcao";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Ribeiro</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Ribeiro</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idPesquisaCurriculoFuncao;
			parms[1].Value = this._pesquisaCurriculo.IdPesquisaCurriculo;
			parms[2].Value = this._funcao.IdFuncao;

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
		/// Método utilizado para inserir uma instância de PesquisaCurriculoFuncao no banco de dados.
		/// </summary>
		/// <remarks>Ribeiro</remarks>
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
						this._idPesquisaCurriculoFuncao = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo_Funcao"].Value);
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
		/// Método utilizado para inserir uma instância de PesquisaCurriculoFuncao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Ribeiro</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPesquisaCurriculoFuncao = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo_Funcao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PesquisaCurriculoFuncao no banco de dados.
		/// </summary>
		/// <remarks>Ribeiro</remarks>
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
		/// Método utilizado para atualizar uma instância de PesquisaCurriculoFuncao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Ribeiro</remarks>
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
		/// Método utilizado para salvar uma instância de PesquisaCurriculoFuncao no banco de dados.
		/// </summary>
		/// <remarks>Ribeiro</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de PesquisaCurriculoFuncao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Ribeiro</remarks>
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
		/// Método utilizado para excluir uma instância de PesquisaCurriculoFuncao no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoFuncao">Chave do registro.</param>
		/// <remarks>Ribeiro</remarks>
		public static void Delete(int idPesquisaCurriculoFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoFuncao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PesquisaCurriculoFuncao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculoFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Ribeiro</remarks>
		public static void Delete(int idPesquisaCurriculoFuncao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoFuncao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PesquisaCurriculoFuncao no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoFuncao">Lista de chaves.</param>
		/// <remarks>Ribeiro</remarks>
		public static void Delete(List<int> idPesquisaCurriculoFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pesquisa_Curriculo_Funcao where Idf_Pesquisa_Curriculo_Funcao in (";

			for (int i = 0; i < idPesquisaCurriculoFuncao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPesquisaCurriculoFuncao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoFuncao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Ribeiro</remarks>
		private static IDataReader LoadDataReader(int idPesquisaCurriculoFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoFuncao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculoFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Ribeiro</remarks>
		private static IDataReader LoadDataReader(int idPesquisaCurriculoFuncao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoFuncao;

			return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar uma consulta paginada do banco de dados.
		/// </summary>
		/// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
		/// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
		/// <param name="paginaCorrente">Número da página que será exibida.</param>
		/// <param name="tamanhoPagina">QuantidadeFuncoes de itens em cada página.</param>
		/// <param name="totalRegistros">QuantidadeFuncoes total de registros na tabela.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
		{
			int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
			int fim = paginaCorrente * tamanhoPagina;

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Curriculo_Funcao, Pes.Idf_Pesquisa_Curriculo, Pes.Idf_Funcao FROM TAB_Pesquisa_Curriculo_Funcao Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaCurriculoFuncao a partir do banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoFuncao">Chave do registro.</param>
		/// <returns>Instância de PesquisaCurriculoFuncao.</returns>
		/// <remarks>Ribeiro</remarks>
		public static PesquisaCurriculoFuncao LoadObject(int idPesquisaCurriculoFuncao)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaCurriculoFuncao))
			{
				PesquisaCurriculoFuncao objPesquisaCurriculoFuncao = new PesquisaCurriculoFuncao();
				if (SetInstance(dr, objPesquisaCurriculoFuncao))
					return objPesquisaCurriculoFuncao;
			}
			throw (new RecordNotFoundException(typeof(PesquisaCurriculoFuncao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaCurriculoFuncao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculoFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PesquisaCurriculoFuncao.</returns>
		/// <remarks>Ribeiro</remarks>
		public static PesquisaCurriculoFuncao LoadObject(int idPesquisaCurriculoFuncao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaCurriculoFuncao, trans))
			{
				PesquisaCurriculoFuncao objPesquisaCurriculoFuncao = new PesquisaCurriculoFuncao();
				if (SetInstance(dr, objPesquisaCurriculoFuncao))
					return objPesquisaCurriculoFuncao;
			}
			throw (new RecordNotFoundException(typeof(PesquisaCurriculoFuncao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaCurriculoFuncao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Ribeiro</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaCurriculoFuncao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaCurriculoFuncao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Ribeiro</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaCurriculoFuncao, trans))
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
		/// <param name="objPesquisaCurriculoFuncao">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Ribeiro</remarks>
		private static bool SetInstance(IDataReader dr, PesquisaCurriculoFuncao objPesquisaCurriculoFuncao, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objPesquisaCurriculoFuncao._idPesquisaCurriculoFuncao = Convert.ToInt32(dr["Idf_Pesquisa_Curriculo_Funcao"]);
					objPesquisaCurriculoFuncao._pesquisaCurriculo = new PesquisaCurriculo(Convert.ToInt32(dr["Idf_Pesquisa_Curriculo"]));
					objPesquisaCurriculoFuncao._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));

					objPesquisaCurriculoFuncao._persisted = true;
					objPesquisaCurriculoFuncao._modified = false;

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