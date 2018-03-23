//-- Data: 14/05/2013 12:54
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CursoFuncaoTecla // Tabela: BNE_Curso_Funcao_Tecla
	{
		#region Atributos
		private int _idCursoFuncaoTecla;
		private CursoTecla _cursoTecla;
		private Funcao _funcao;
		private bool _flagInativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCursoFuncaoTecla
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCursoFuncaoTecla
		{
			get
			{
				return this._idCursoFuncaoTecla;
			}
		}
		#endregion 

		#region CursoTecla
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CursoTecla CursoTecla
		{
			get
			{
				return this._cursoTecla;
			}
			set
			{
				this._cursoTecla = value;
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

		#region FlagInativo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagInativo
		{
			get
			{
				return this._flagInativo;
			}
			set
			{
				this._flagInativo = value;
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

		#endregion

		#region Construtores
		public CursoFuncaoTecla()
		{
		}
		public CursoFuncaoTecla(int idCursoFuncaoTecla)
		{
			this._idCursoFuncaoTecla = idCursoFuncaoTecla;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Curso_Funcao_Tecla (Idf_Curso_Tecla, Idf_Funcao, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Curso_Tecla, @Idf_Funcao, @Flg_Inativo, @Dta_Cadastro);SET @Idf_Curso_Funcao_Tecla = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Curso_Funcao_Tecla SET Idf_Curso_Tecla = @Idf_Curso_Tecla, Idf_Funcao = @Idf_Funcao, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Curso_Funcao_Tecla = @Idf_Curso_Funcao_Tecla";
		private const string SPDELETE = "DELETE FROM BNE_Curso_Funcao_Tecla WHERE Idf_Curso_Funcao_Tecla = @Idf_Curso_Funcao_Tecla";
		private const string SPSELECTID = "SELECT * FROM BNE_Curso_Funcao_Tecla WITH(NOLOCK) WHERE Idf_Curso_Funcao_Tecla = @Idf_Curso_Funcao_Tecla";
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
			parms.Add(new SqlParameter("@Idf_Curso_Funcao_Tecla", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curso_Tecla", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idCursoFuncaoTecla;
			parms[1].Value = this._cursoTecla.IdCursoTecla;
			parms[2].Value = this._funcao.IdFuncao;
			parms[3].Value = this._flagInativo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[4].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CursoFuncaoTecla no banco de dados.
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
						this._idCursoFuncaoTecla = Convert.ToInt32(cmd.Parameters["@Idf_Curso_Funcao_Tecla"].Value);
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
		/// Método utilizado para inserir uma instância de CursoFuncaoTecla no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCursoFuncaoTecla = Convert.ToInt32(cmd.Parameters["@Idf_Curso_Funcao_Tecla"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CursoFuncaoTecla no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CursoFuncaoTecla no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CursoFuncaoTecla no banco de dados.
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
		/// Método utilizado para salvar uma instância de CursoFuncaoTecla no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CursoFuncaoTecla no banco de dados.
		/// </summary>
		/// <param name="idCursoFuncaoTecla">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCursoFuncaoTecla)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Funcao_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idCursoFuncaoTecla;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CursoFuncaoTecla no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCursoFuncaoTecla">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCursoFuncaoTecla, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Funcao_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idCursoFuncaoTecla;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CursoFuncaoTecla no banco de dados.
		/// </summary>
		/// <param name="idCursoFuncaoTecla">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCursoFuncaoTecla)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Curso_Funcao_Tecla where Idf_Curso_Funcao_Tecla in (";

			for (int i = 0; i < idCursoFuncaoTecla.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCursoFuncaoTecla[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCursoFuncaoTecla">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCursoFuncaoTecla)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Funcao_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idCursoFuncaoTecla;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCursoFuncaoTecla">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCursoFuncaoTecla, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso_Funcao_Tecla", SqlDbType.Int, 4));

			parms[0].Value = idCursoFuncaoTecla;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curso_Funcao_Tecla, Cur.Idf_Curso_Tecla, Cur.Idf_Funcao, Cur.Flg_Inativo, Cur.Dta_Cadastro FROM BNE_Curso_Funcao_Tecla Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CursoFuncaoTecla a partir do banco de dados.
		/// </summary>
		/// <param name="idCursoFuncaoTecla">Chave do registro.</param>
		/// <returns>Instância de CursoFuncaoTecla.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CursoFuncaoTecla LoadObject(int idCursoFuncaoTecla)
		{
			using (IDataReader dr = LoadDataReader(idCursoFuncaoTecla))
			{
				CursoFuncaoTecla objCursoFuncaoTecla = new CursoFuncaoTecla();
				if (SetInstance(dr, objCursoFuncaoTecla))
					return objCursoFuncaoTecla;
			}
			throw (new RecordNotFoundException(typeof(CursoFuncaoTecla)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CursoFuncaoTecla a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCursoFuncaoTecla">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CursoFuncaoTecla.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CursoFuncaoTecla LoadObject(int idCursoFuncaoTecla, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCursoFuncaoTecla, trans))
			{
				CursoFuncaoTecla objCursoFuncaoTecla = new CursoFuncaoTecla();
				if (SetInstance(dr, objCursoFuncaoTecla))
					return objCursoFuncaoTecla;
			}
			throw (new RecordNotFoundException(typeof(CursoFuncaoTecla)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CursoFuncaoTecla a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCursoFuncaoTecla))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CursoFuncaoTecla a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCursoFuncaoTecla, trans))
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
		/// <param name="objCursoFuncaoTecla">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CursoFuncaoTecla objCursoFuncaoTecla)
		{
			try
			{
				if (dr.Read())
				{
					objCursoFuncaoTecla._idCursoFuncaoTecla = Convert.ToInt32(dr["Idf_Curso_Funcao_Tecla"]);
					objCursoFuncaoTecla._cursoTecla = new CursoTecla(Convert.ToInt32(dr["Idf_Curso_Tecla"]));
					objCursoFuncaoTecla._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
					objCursoFuncaoTecla._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objCursoFuncaoTecla._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objCursoFuncaoTecla._persisted = true;
					objCursoFuncaoTecla._modified = false;

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