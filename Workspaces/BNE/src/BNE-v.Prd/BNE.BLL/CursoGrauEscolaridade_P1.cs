//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CursoGrauEscolaridade // Tabela: TAB_Curso_Grau_Escolaridade
	{
		#region Atributos
		private Curso _curso;
		private GrauEscolaridade _grauEscolaridade;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region Curso
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Curso Curso
		{
			get
			{
				return this._curso;
			}
			set
			{
				this._curso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region GrauEscolaridade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public GrauEscolaridade GrauEscolaridade
		{
			get
			{
				return this._grauEscolaridade;
			}
			set
			{
				this._grauEscolaridade = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CursoGrauEscolaridade()
		{
		}
		public CursoGrauEscolaridade(Curso curso, GrauEscolaridade grauEscolaridade)
		{
			this._curso = curso;
			this._grauEscolaridade = grauEscolaridade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Curso_Grau_Escolaridade (Idf_Curso, Idf_Grau_Escolaridade) VALUES (@Idf_Curso, @Idf_Grau_Escolaridade);";
		private const string SPUPDATE = "UPDATE TAB_Curso_Grau_Escolaridade SET  WHERE Idf_Curso = @Idf_Curso AND Idf_Grau_Escolaridade = @Idf_Grau_Escolaridade";
		private const string SPDELETE = "DELETE FROM TAB_Curso_Grau_Escolaridade WHERE Idf_Curso = @Idf_Curso AND Idf_Grau_Escolaridade = @Idf_Grau_Escolaridade";
		private const string SPSELECTID = "SELECT * FROM TAB_Curso_Grau_Escolaridade WHERE Idf_Curso = @Idf_Curso AND Idf_Grau_Escolaridade = @Idf_Grau_Escolaridade";
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
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Grau_Escolaridade", SqlDbType.Int, 4));
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
			parms[0].Value = this._curso.IdCurso;
			parms[1].Value = this._grauEscolaridade.IdGrauEscolaridade;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CursoGrauEscolaridade no banco de dados.
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
		/// Método utilizado para inserir uma instância de CursoGrauEscolaridade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de CursoGrauEscolaridade no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CursoGrauEscolaridade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CursoGrauEscolaridade no banco de dados.
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
		/// Método utilizado para salvar uma instância de CursoGrauEscolaridade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CursoGrauEscolaridade no banco de dados.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <param name="idGrauEscolaridade">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCurso, int idGrauEscolaridade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Grau_Escolaridade", SqlDbType.Int, 4));

			parms[0].Value = idCurso;
			parms[1].Value = idGrauEscolaridade;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CursoGrauEscolaridade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <param name="idGrauEscolaridade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCurso, int idGrauEscolaridade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Grau_Escolaridade", SqlDbType.Int, 4));

			parms[0].Value = idCurso;
			parms[1].Value = idGrauEscolaridade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <param name="idGrauEscolaridade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCurso, int idGrauEscolaridade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Grau_Escolaridade", SqlDbType.Int, 4));

			parms[0].Value = idCurso;
			parms[1].Value = idGrauEscolaridade;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <param name="idGrauEscolaridade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCurso, int idGrauEscolaridade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Grau_Escolaridade", SqlDbType.Int, 4));

			parms[0].Value = idCurso;
			parms[1].Value = idGrauEscolaridade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curso, Cur.Idf_Grau_Escolaridade FROM TAB_Curso_Grau_Escolaridade Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CursoGrauEscolaridade a partir do banco de dados.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <param name="idGrauEscolaridade">Chave do registro.</param>
		/// <returns>Instância de CursoGrauEscolaridade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CursoGrauEscolaridade LoadObject(int idCurso, int idGrauEscolaridade)
		{
			using (IDataReader dr = LoadDataReader(idCurso, idGrauEscolaridade))
			{
				CursoGrauEscolaridade objCursoGrauEscolaridade = new CursoGrauEscolaridade();
				if (SetInstance(dr, objCursoGrauEscolaridade))
					return objCursoGrauEscolaridade;
			}
			throw (new RecordNotFoundException(typeof(CursoGrauEscolaridade)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CursoGrauEscolaridade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <param name="idGrauEscolaridade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CursoGrauEscolaridade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CursoGrauEscolaridade LoadObject(int idCurso, int idGrauEscolaridade, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurso, idGrauEscolaridade, trans))
			{
				CursoGrauEscolaridade objCursoGrauEscolaridade = new CursoGrauEscolaridade();
				if (SetInstance(dr, objCursoGrauEscolaridade))
					return objCursoGrauEscolaridade;
			}
			throw (new RecordNotFoundException(typeof(CursoGrauEscolaridade)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CursoGrauEscolaridade a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._curso.IdCurso, this._grauEscolaridade.IdGrauEscolaridade))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CursoGrauEscolaridade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._curso.IdCurso, this._grauEscolaridade.IdGrauEscolaridade, trans))
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
		/// <param name="objCursoGrauEscolaridade">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CursoGrauEscolaridade objCursoGrauEscolaridade)
		{
			try
			{
				if (dr.Read())
				{
					objCursoGrauEscolaridade._curso = new Curso(Convert.ToInt32(dr["Idf_Curso"]));
					objCursoGrauEscolaridade._grauEscolaridade = new GrauEscolaridade(Convert.ToInt32(dr["Idf_Grau_Escolaridade"]));

					objCursoGrauEscolaridade._persisted = true;
					objCursoGrauEscolaridade._modified = false;

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