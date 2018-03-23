//-- Data: 20/01/2016 16:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CampoPalavraChaveRastreadorCurriculo // Tabela: BNE_Campo_Palavra_Chave_Rastreador_Curriculo
	{
		#region Atributos
		private int _idCampoPalavraChaveRastreadorCurriculo;
		private RastreadorCurriculo _rastreadorCurriculo;
		private CampoPalavraChave _campoPalavraChave;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCampoPalavraChaveRastreadorCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCampoPalavraChaveRastreadorCurriculo
		{
			get
			{
				return this._idCampoPalavraChaveRastreadorCurriculo;
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
		public CampoPalavraChaveRastreadorCurriculo()
		{
		}
		public CampoPalavraChaveRastreadorCurriculo(int idCampoPalavraChaveRastreadorCurriculo)
		{
			this._idCampoPalavraChaveRastreadorCurriculo = idCampoPalavraChaveRastreadorCurriculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Campo_Palavra_Chave_Rastreador_Curriculo (Idf_Rastreador_Curriculo, Idf_Campo_Palavra_Chave) VALUES (@Idf_Rastreador_Curriculo, @Idf_Campo_Palavra_Chave);SET @Idf_Campo_Palavra_Chave_Rastreador_Curriculo = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Campo_Palavra_Chave_Rastreador_Curriculo SET Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo, Idf_Campo_Palavra_Chave = @Idf_Campo_Palavra_Chave WHERE Idf_Campo_Palavra_Chave_Rastreador_Curriculo = @Idf_Campo_Palavra_Chave_Rastreador_Curriculo";
		private const string SPDELETE = "DELETE FROM BNE_Campo_Palavra_Chave_Rastreador_Curriculo WHERE Idf_Campo_Palavra_Chave_Rastreador_Curriculo = @Idf_Campo_Palavra_Chave_Rastreador_Curriculo";
		private const string SPSELECTID = "SELECT * FROM BNE_Campo_Palavra_Chave_Rastreador_Curriculo WITH(NOLOCK) WHERE Idf_Campo_Palavra_Chave_Rastreador_Curriculo = @Idf_Campo_Palavra_Chave_Rastreador_Curriculo";
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
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave_Rastreador_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCampoPalavraChaveRastreadorCurriculo;
			parms[1].Value = this._rastreadorCurriculo.IdRastreadorCurriculo;
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
		/// Método utilizado para inserir uma instância de CampoPalavraChaveRastreadorCurriculo no banco de dados.
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
						this._idCampoPalavraChaveRastreadorCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Campo_Palavra_Chave_Rastreador_Curriculo"].Value);
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
		/// Método utilizado para inserir uma instância de CampoPalavraChaveRastreadorCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCampoPalavraChaveRastreadorCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Campo_Palavra_Chave_Rastreador_Curriculo"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CampoPalavraChaveRastreadorCurriculo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CampoPalavraChaveRastreadorCurriculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CampoPalavraChaveRastreadorCurriculo no banco de dados.
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
		/// Método utilizado para salvar uma instância de CampoPalavraChaveRastreadorCurriculo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CampoPalavraChaveRastreadorCurriculo no banco de dados.
		/// </summary>
		/// <param name="idCampoPalavraChaveRastreadorCurriculo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCampoPalavraChaveRastreadorCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave_Rastreador_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCampoPalavraChaveRastreadorCurriculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CampoPalavraChaveRastreadorCurriculo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampoPalavraChaveRastreadorCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCampoPalavraChaveRastreadorCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave_Rastreador_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCampoPalavraChaveRastreadorCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CampoPalavraChaveRastreadorCurriculo no banco de dados.
		/// </summary>
		/// <param name="idCampoPalavraChaveRastreadorCurriculo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCampoPalavraChaveRastreadorCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Campo_Palavra_Chave_Rastreador_Curriculo where Idf_Campo_Palavra_Chave_Rastreador_Curriculo in (";

			for (int i = 0; i < idCampoPalavraChaveRastreadorCurriculo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCampoPalavraChaveRastreadorCurriculo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCampoPalavraChaveRastreadorCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCampoPalavraChaveRastreadorCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave_Rastreador_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCampoPalavraChaveRastreadorCurriculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampoPalavraChaveRastreadorCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCampoPalavraChaveRastreadorCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Campo_Palavra_Chave_Rastreador_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCampoPalavraChaveRastreadorCurriculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cam.Idf_Campo_Palavra_Chave_Rastreador_Curriculo, Cam.Idf_Rastreador_Curriculo, Cam.Idf_Campo_Palavra_Chave FROM BNE_Campo_Palavra_Chave_Rastreador_Curriculo Cam";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CampoPalavraChaveRastreadorCurriculo a partir do banco de dados.
		/// </summary>
		/// <param name="idCampoPalavraChaveRastreadorCurriculo">Chave do registro.</param>
		/// <returns>Instância de CampoPalavraChaveRastreadorCurriculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CampoPalavraChaveRastreadorCurriculo LoadObject(int idCampoPalavraChaveRastreadorCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idCampoPalavraChaveRastreadorCurriculo))
			{
				CampoPalavraChaveRastreadorCurriculo objCampoPalavraChaveRastreadorCurriculo = new CampoPalavraChaveRastreadorCurriculo();
				if (SetInstance(dr, objCampoPalavraChaveRastreadorCurriculo))
					return objCampoPalavraChaveRastreadorCurriculo;
			}
			throw (new RecordNotFoundException(typeof(CampoPalavraChaveRastreadorCurriculo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CampoPalavraChaveRastreadorCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCampoPalavraChaveRastreadorCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CampoPalavraChaveRastreadorCurriculo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CampoPalavraChaveRastreadorCurriculo LoadObject(int idCampoPalavraChaveRastreadorCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCampoPalavraChaveRastreadorCurriculo, trans))
			{
				CampoPalavraChaveRastreadorCurriculo objCampoPalavraChaveRastreadorCurriculo = new CampoPalavraChaveRastreadorCurriculo();
				if (SetInstance(dr, objCampoPalavraChaveRastreadorCurriculo))
					return objCampoPalavraChaveRastreadorCurriculo;
			}
			throw (new RecordNotFoundException(typeof(CampoPalavraChaveRastreadorCurriculo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CampoPalavraChaveRastreadorCurriculo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCampoPalavraChaveRastreadorCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CampoPalavraChaveRastreadorCurriculo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCampoPalavraChaveRastreadorCurriculo, trans))
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
		/// <param name="objCampoPalavraChaveRastreadorCurriculo">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CampoPalavraChaveRastreadorCurriculo objCampoPalavraChaveRastreadorCurriculo, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objCampoPalavraChaveRastreadorCurriculo._idCampoPalavraChaveRastreadorCurriculo = Convert.ToInt32(dr["Idf_Campo_Palavra_Chave_Rastreador_Curriculo"]);
					objCampoPalavraChaveRastreadorCurriculo._rastreadorCurriculo = new RastreadorCurriculo(Convert.ToInt32(dr["Idf_Rastreador_Curriculo"]));
					objCampoPalavraChaveRastreadorCurriculo._campoPalavraChave = new CampoPalavraChave(Convert.ToInt32(dr["Idf_Campo_Palavra_Chave"]));

					objCampoPalavraChaveRastreadorCurriculo._persisted = true;
					objCampoPalavraChaveRastreadorCurriculo._modified = false;

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