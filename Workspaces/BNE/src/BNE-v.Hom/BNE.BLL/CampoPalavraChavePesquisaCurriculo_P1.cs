//-- Data: 20/01/2016 16:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CampoPalavraChavePesquisaCurriculo // Tabela: BNE_Campo_Palavra_Chave_Pesquisa_Curriculo
	{
		#region Atributos
		private int _idCampoPalavraChavePesquisaCurriculo;
		private PesquisaCurriculo _pesquisaCurriculo;
		private CampoPalavraChave _campoPalavraChave;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCampoPalavraChavePesquisaCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCampoPalavraChavePesquisaCurriculo
		{
			get
			{
				return this._idCampoPalavraChavePesquisaCurriculo;
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

		#region CampoPalavraChave
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CampoPalavraChave CampoPalavraChave
		{
			get
			{
				return this._campoPalavraChave;
			}
			set
			{
				this._campoPalavraChave = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CampoPalavraChavePesquisaCurriculo()
		{
		}
		public CampoPalavraChavePesquisaCurriculo(int idCampoPalavraChavePesquisaCurriculo)
		{
			this._idCampoPalavraChavePesquisaCurriculo = idCampoPalavraChavePesquisaCurriculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Campo_Palavra_Chave_Pesquisa_Curriculo (Idf_Pesquisa_Curriculo, Idf_Campo_Palavra_Chave) VALUES (@Idf_Pesquisa_Curriculo, @Idf_Campo_Palavra_Chave);SET @Idf_Campo_Palavra_Chave_Pesquisa_Curriculo = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Campo_Palavra_Chave_Pesquisa_Curriculo SET Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo, Idf_Campo_Palavra_Chave = @Idf_Campo_Palavra_Chave WHERE Idf_Campo_Palavra_Chave_Pesquisa_Curriculo = @Idf_Campo_Palavra_Chave_Pesquisa_Curriculo";
		private const string SPDELETE = "DELETE FROM BNE_Campo_Palavra_Chave_Pesquisa_Curriculo WHERE Idf_Campo_Palavra_Chave_Pesquisa_Curriculo = @Idf_Campo_Palavra_Chave_Pesquisa_Curriculo";
		private const string SPSELECTID = "SELECT * FROM BNE_Campo_Palavra_Chave_Pesquisa_Curriculo WITH(NOLOCK) WHERE Idf_Campo_Palavra_Chave_Pesquisa_Curriculo = @Idf_Campo_Palavra_Chave_Pesquisa_Curriculo";
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
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave_Pesquisa_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCampoPalavraChavePesquisaCurriculo;
			parms[1].Value = this._pesquisaCurriculo.IdPesquisaCurriculo;
			parms[2].Value = this._campoPalavraChave.IdCampoPalavraChave;

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
		/// Método utilizado para inserir uma instância de CampoPalavraChavePesquisaCurriculo no banco de dados.
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
						this._idCampoPalavraChavePesquisaCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Campo_Palavra_Chave_Pesquisa_Curriculo"].Value);
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
		/// Método utilizado para inserir uma instância de CampoPalavraChavePesquisaCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCampoPalavraChavePesquisaCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Campo_Palavra_Chave_Pesquisa_Curriculo"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CampoPalavraChavePesquisaCurriculo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CampoPalavraChavePesquisaCurriculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CampoPalavraChavePesquisaCurriculo no banco de dados.
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
		/// Método utilizado para salvar uma instância de CampoPalavraChavePesquisaCurriculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CampoPalavraChavePesquisaCurriculo no banco de dados.
		/// </summary>
		/// <param name="idCampoPalavraChavePesquisaCurriculo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCampoPalavraChavePesquisaCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave_Pesquisa_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCampoPalavraChavePesquisaCurriculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CampoPalavraChavePesquisaCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampoPalavraChavePesquisaCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCampoPalavraChavePesquisaCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave_Pesquisa_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCampoPalavraChavePesquisaCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CampoPalavraChavePesquisaCurriculo no banco de dados.
		/// </summary>
		/// <param name="idCampoPalavraChavePesquisaCurriculo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCampoPalavraChavePesquisaCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Campo_Palavra_Chave_Pesquisa_Curriculo where Idf_Campo_Palavra_Chave_Pesquisa_Curriculo in (";

			for (int i = 0; i < idCampoPalavraChavePesquisaCurriculo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCampoPalavraChavePesquisaCurriculo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCampoPalavraChavePesquisaCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCampoPalavraChavePesquisaCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave_Pesquisa_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCampoPalavraChavePesquisaCurriculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampoPalavraChavePesquisaCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCampoPalavraChavePesquisaCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave_Pesquisa_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCampoPalavraChavePesquisaCurriculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cam.Idf_Campo_Palavra_Chave_Pesquisa_Curriculo, Cam.Idf_Pesquisa_Curriculo, Cam.Idf_Campo_Palavra_Chave FROM BNE_Campo_Palavra_Chave_Pesquisa_Curriculo Cam";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CampoPalavraChavePesquisaCurriculo a partir do banco de dados.
		/// </summary>
		/// <param name="idCampoPalavraChavePesquisaCurriculo">Chave do registro.</param>
		/// <returns>Instância de CampoPalavraChavePesquisaCurriculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CampoPalavraChavePesquisaCurriculo LoadObject(int idCampoPalavraChavePesquisaCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idCampoPalavraChavePesquisaCurriculo))
			{
				CampoPalavraChavePesquisaCurriculo objCampoPalavraChavePesquisaCurriculo = new CampoPalavraChavePesquisaCurriculo();
				if (SetInstance(dr, objCampoPalavraChavePesquisaCurriculo))
					return objCampoPalavraChavePesquisaCurriculo;
			}
			throw (new RecordNotFoundException(typeof(CampoPalavraChavePesquisaCurriculo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CampoPalavraChavePesquisaCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampoPalavraChavePesquisaCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CampoPalavraChavePesquisaCurriculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CampoPalavraChavePesquisaCurriculo LoadObject(int idCampoPalavraChavePesquisaCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCampoPalavraChavePesquisaCurriculo, trans))
			{
				CampoPalavraChavePesquisaCurriculo objCampoPalavraChavePesquisaCurriculo = new CampoPalavraChavePesquisaCurriculo();
				if (SetInstance(dr, objCampoPalavraChavePesquisaCurriculo))
					return objCampoPalavraChavePesquisaCurriculo;
			}
			throw (new RecordNotFoundException(typeof(CampoPalavraChavePesquisaCurriculo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CampoPalavraChavePesquisaCurriculo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCampoPalavraChavePesquisaCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CampoPalavraChavePesquisaCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCampoPalavraChavePesquisaCurriculo, trans))
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
		/// <param name="objCampoPalavraChavePesquisaCurriculo">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CampoPalavraChavePesquisaCurriculo objCampoPalavraChavePesquisaCurriculo, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objCampoPalavraChavePesquisaCurriculo._idCampoPalavraChavePesquisaCurriculo = Convert.ToInt32(dr["Idf_Campo_Palavra_Chave_Pesquisa_Curriculo"]);
					objCampoPalavraChavePesquisaCurriculo._pesquisaCurriculo = new PesquisaCurriculo(Convert.ToInt32(dr["Idf_Pesquisa_Curriculo"]));
					objCampoPalavraChavePesquisaCurriculo._campoPalavraChave = new CampoPalavraChave(Convert.ToInt32(dr["Idf_Campo_Palavra_Chave"]));

					objCampoPalavraChavePesquisaCurriculo._persisted = true;
					objCampoPalavraChavePesquisaCurriculo._modified = false;

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