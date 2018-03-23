//-- Data: 01/10/2010 11:40
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PesquisaCurriculoIdioma // Tabela: TAB_Pesquisa_Curriculo_Idioma
	{
		#region Atributos
		private int _idPesquisaCurriculoIdioma;
		private PesquisaCurriculo _pesquisaCurriculo;
		private Idioma _idioma;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPesquisaCurriculoIdioma
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPesquisaCurriculoIdioma
		{
			get
			{
				return this._idPesquisaCurriculoIdioma;
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

		#region Idioma
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Idioma Idioma
		{
			get
			{
				return this._idioma;
			}
			set
			{
				this._idioma = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public PesquisaCurriculoIdioma()
		{
		}
		public PesquisaCurriculoIdioma(int idPesquisaCurriculoIdioma)
		{
			this._idPesquisaCurriculoIdioma = idPesquisaCurriculoIdioma;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Pesquisa_Curriculo_Idioma (Idf_Pesquisa_Curriculo, Idf_Idioma) VALUES (@Idf_Pesquisa_Curriculo, @Idf_Idioma);SET @Idf_Pesquisa_Curriculo_Idioma = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Pesquisa_Curriculo_Idioma SET Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo, Idf_Idioma = @Idf_Idioma WHERE Idf_Pesquisa_Curriculo_Idioma = @Idf_Pesquisa_Curriculo_Idioma";
		private const string SPDELETE = "DELETE FROM TAB_Pesquisa_Curriculo_Idioma WHERE Idf_Pesquisa_Curriculo_Idioma = @Idf_Pesquisa_Curriculo_Idioma";
		private const string SPSELECTID = "SELECT * FROM TAB_Pesquisa_Curriculo_Idioma WHERE Idf_Pesquisa_Curriculo_Idioma = @Idf_Pesquisa_Curriculo_Idioma";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Idioma", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Idioma", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Bruno Flammarion</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idPesquisaCurriculoIdioma;
			parms[1].Value = this._pesquisaCurriculo.IdPesquisaCurriculo;
			parms[2].Value = this._idioma.IdIdioma;

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
		/// Método utilizado para inserir uma instância de PesquisaCurriculoIdioma no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
						this._idPesquisaCurriculoIdioma = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo_Idioma"].Value);
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
		/// Método utilizado para inserir uma instância de PesquisaCurriculoIdioma no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPesquisaCurriculoIdioma = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo_Idioma"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PesquisaCurriculoIdioma no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para atualizar uma instância de PesquisaCurriculoIdioma no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para salvar uma instância de PesquisaCurriculoIdioma no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de PesquisaCurriculoIdioma no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para excluir uma instância de PesquisaCurriculoIdioma no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoIdioma">Chave do registro.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idPesquisaCurriculoIdioma)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoIdioma;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PesquisaCurriculoIdioma no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculoIdioma">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idPesquisaCurriculoIdioma, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoIdioma;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PesquisaCurriculoIdioma no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoIdioma">Lista de chaves.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(List<int> idPesquisaCurriculoIdioma)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Pesquisa_Curriculo_Idioma where Idf_Pesquisa_Curriculo_Idioma in (";

			for (int i = 0; i < idPesquisaCurriculoIdioma.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPesquisaCurriculoIdioma[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoIdioma">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idPesquisaCurriculoIdioma)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoIdioma;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculoIdioma">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idPesquisaCurriculoIdioma, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Idioma", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaCurriculoIdioma;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Curriculo_Idioma, Pes.Idf_Pesquisa_Curriculo, Pes.Idf_Idioma FROM TAB_Pesquisa_Curriculo_Idioma Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaCurriculoIdioma a partir do banco de dados.
		/// </summary>
		/// <param name="idPesquisaCurriculoIdioma">Chave do registro.</param>
		/// <returns>Instância de PesquisaCurriculoIdioma.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static PesquisaCurriculoIdioma LoadObject(int idPesquisaCurriculoIdioma)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaCurriculoIdioma))
			{
				PesquisaCurriculoIdioma objPesquisaCurriculoIdioma = new PesquisaCurriculoIdioma();
				if (SetInstance(dr, objPesquisaCurriculoIdioma))
					return objPesquisaCurriculoIdioma;
			}
			throw (new RecordNotFoundException(typeof(PesquisaCurriculoIdioma)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaCurriculoIdioma a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaCurriculoIdioma">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PesquisaCurriculoIdioma.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static PesquisaCurriculoIdioma LoadObject(int idPesquisaCurriculoIdioma, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaCurriculoIdioma, trans))
			{
				PesquisaCurriculoIdioma objPesquisaCurriculoIdioma = new PesquisaCurriculoIdioma();
				if (SetInstance(dr, objPesquisaCurriculoIdioma))
					return objPesquisaCurriculoIdioma;
			}
			throw (new RecordNotFoundException(typeof(PesquisaCurriculoIdioma)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaCurriculoIdioma a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaCurriculoIdioma))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaCurriculoIdioma a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaCurriculoIdioma, trans))
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
		/// <param name="objPesquisaCurriculoIdioma">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static bool SetInstance(IDataReader dr, PesquisaCurriculoIdioma objPesquisaCurriculoIdioma)
		{
			try
			{
				if (dr.Read())
				{
					objPesquisaCurriculoIdioma._idPesquisaCurriculoIdioma = Convert.ToInt32(dr["Idf_Pesquisa_Curriculo_Idioma"]);
					objPesquisaCurriculoIdioma._pesquisaCurriculo = new PesquisaCurriculo(Convert.ToInt32(dr["Idf_Pesquisa_Curriculo"]));
					objPesquisaCurriculoIdioma._idioma = new Idioma(Convert.ToInt32(dr["Idf_Idioma"]));

					objPesquisaCurriculoIdioma._persisted = true;
					objPesquisaCurriculoIdioma._modified = false;

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