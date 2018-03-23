//-- Data: 18/01/2016 16:18
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class RastreadorCurriculoFuncao // Tabela: BNE_Rastreador_Curriculo_Funcao
	{
		#region Atributos
		private int _idRastreadorCurriculoFuncao;
		private RastreadorCurriculo _rastreadorCurriculo;
		private Funcao _funcao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdRastreadorCurriculoFuncao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdRastreadorCurriculoFuncao
		{
			get
			{
				return this._idRastreadorCurriculoFuncao;
			}
		}
		#endregion 

		#region RastreadorCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public RastreadorCurriculo RastreadorCurriculo
		{
			get
			{
				return this._rastreadorCurriculo;
			}
			set
			{
				this._rastreadorCurriculo = value;
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
		public RastreadorCurriculoFuncao()
		{
		}
		public RastreadorCurriculoFuncao(int idRastreadorCurriculoFuncao)
		{
			this._idRastreadorCurriculoFuncao = idRastreadorCurriculoFuncao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Rastreador_Curriculo_Funcao (Idf_Rastreador_Curriculo, Idf_Funcao) VALUES (@Idf_Rastreador_Curriculo, @Idf_Funcao);SET @Idf_Rastreador_Curriculo_Funcao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Rastreador_Curriculo_Funcao SET Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo, Idf_Funcao = @Idf_Funcao WHERE Idf_Rastreador_Curriculo_Funcao = @Idf_Rastreador_Curriculo_Funcao";
		private const string SPDELETE = "DELETE FROM BNE_Rastreador_Curriculo_Funcao WHERE Idf_Rastreador_Curriculo_Funcao = @Idf_Rastreador_Curriculo_Funcao";
		private const string SPSELECTID = "SELECT * FROM BNE_Rastreador_Curriculo_Funcao WITH(NOLOCK) WHERE Idf_Rastreador_Curriculo_Funcao = @Idf_Rastreador_Curriculo_Funcao";
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
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
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
			parms[0].Value = this._idRastreadorCurriculoFuncao;
			parms[1].Value = this._rastreadorCurriculo.IdRastreadorCurriculo;
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
		/// Método utilizado para inserir uma instância de RastreadorCurriculoFuncao no banco de dados.
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
						this._idRastreadorCurriculoFuncao = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Curriculo_Funcao"].Value);
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
		/// Método utilizado para inserir uma instância de RastreadorCurriculoFuncao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idRastreadorCurriculoFuncao = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Curriculo_Funcao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de RastreadorCurriculoFuncao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de RastreadorCurriculoFuncao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de RastreadorCurriculoFuncao no banco de dados.
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
		/// Método utilizado para salvar uma instância de RastreadorCurriculoFuncao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de RastreadorCurriculoFuncao no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoFuncao">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorCurriculoFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoFuncao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de RastreadorCurriculoFuncao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculoFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idRastreadorCurriculoFuncao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoFuncao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de RastreadorCurriculoFuncao no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoFuncao">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idRastreadorCurriculoFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Rastreador_Curriculo_Funcao where Idf_Rastreador_Curriculo_Funcao in (";

			for (int i = 0; i < idRastreadorCurriculoFuncao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idRastreadorCurriculoFuncao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoFuncao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorCurriculoFuncao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoFuncao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculoFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idRastreadorCurriculoFuncao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo_Funcao", SqlDbType.Int, 4));

			parms[0].Value = idRastreadorCurriculoFuncao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ras.Idf_Rastreador_Curriculo_Funcao, Ras.Idf_Rastreador_Curriculo, Ras.Idf_Funcao FROM BNE_Rastreador_Curriculo_Funcao Ras";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorCurriculoFuncao a partir do banco de dados.
		/// </summary>
		/// <param name="idRastreadorCurriculoFuncao">Chave do registro.</param>
		/// <returns>Instância de RastreadorCurriculoFuncao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorCurriculoFuncao LoadObject(int idRastreadorCurriculoFuncao)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorCurriculoFuncao))
			{
				RastreadorCurriculoFuncao objRastreadorCurriculoFuncao = new RastreadorCurriculoFuncao();
				if (SetInstance(dr, objRastreadorCurriculoFuncao))
					return objRastreadorCurriculoFuncao;
			}
			throw (new RecordNotFoundException(typeof(RastreadorCurriculoFuncao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de RastreadorCurriculoFuncao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idRastreadorCurriculoFuncao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de RastreadorCurriculoFuncao.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static RastreadorCurriculoFuncao LoadObject(int idRastreadorCurriculoFuncao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idRastreadorCurriculoFuncao, trans))
			{
				RastreadorCurriculoFuncao objRastreadorCurriculoFuncao = new RastreadorCurriculoFuncao();
				if (SetInstance(dr, objRastreadorCurriculoFuncao))
					return objRastreadorCurriculoFuncao;
			}
			throw (new RecordNotFoundException(typeof(RastreadorCurriculoFuncao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorCurriculoFuncao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorCurriculoFuncao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de RastreadorCurriculoFuncao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idRastreadorCurriculoFuncao, trans))
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
		/// <param name="objRastreadorCurriculoFuncao">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, RastreadorCurriculoFuncao objRastreadorCurriculoFuncao, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objRastreadorCurriculoFuncao._idRastreadorCurriculoFuncao = Convert.ToInt32(dr["Idf_Rastreador_Curriculo_Funcao"]);
					objRastreadorCurriculoFuncao._rastreadorCurriculo = new RastreadorCurriculo(Convert.ToInt32(dr["Idf_Rastreador_Curriculo"]));
					objRastreadorCurriculoFuncao._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));

					objRastreadorCurriculoFuncao._persisted = true;
					objRastreadorCurriculoFuncao._modified = false;

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