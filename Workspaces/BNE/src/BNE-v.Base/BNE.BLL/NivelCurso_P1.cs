//-- Data: 14/07/2010 12:21
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class NivelCurso // Tabela: TAB_Nivel_Curso
	{
		#region Atributos
		private int _idNivelCurso;
		private string _descricaoNivelCurso;
		private DateTime _dataCadastro;
		private GrauEscolaridade _grauEscolaridade;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdNivelCurso
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdNivelCurso
		{
			get
			{
				return this._idNivelCurso;
			}
			set
			{
				this._idNivelCurso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoNivelCurso
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoNivelCurso
		{
			get
			{
				return this._descricaoNivelCurso;
			}
			set
			{
				this._descricaoNivelCurso = value;
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

		#endregion

		#region Construtores
		public NivelCurso()
		{
		}
		public NivelCurso(int idNivelCurso)
		{
			this._idNivelCurso = idNivelCurso;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Nivel_Curso (Idf_Nivel_Curso, Des_Nivel_Curso, Dta_Cadastro, Idf_Grau_Escolaridade, Flg_Inativo) VALUES (@Idf_Nivel_Curso, @Des_Nivel_Curso, @Dta_Cadastro, @Idf_Grau_Escolaridade, @Flg_Inativo);";
		private const string SPUPDATE = "UPDATE TAB_Nivel_Curso SET Des_Nivel_Curso = @Des_Nivel_Curso, Dta_Cadastro = @Dta_Cadastro, Idf_Grau_Escolaridade = @Idf_Grau_Escolaridade, Flg_Inativo = @Flg_Inativo WHERE Idf_Nivel_Curso = @Idf_Nivel_Curso";
		private const string SPDELETE = "DELETE FROM TAB_Nivel_Curso WHERE Idf_Nivel_Curso = @Idf_Nivel_Curso";
		private const string SPSELECTID = "SELECT * FROM TAB_Nivel_Curso WHERE Idf_Nivel_Curso = @Idf_Nivel_Curso";
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
			parms.Add(new SqlParameter("@Idf_Nivel_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Nivel_Curso", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Grau_Escolaridade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idNivelCurso;
			parms[1].Value = this._descricaoNivelCurso;
			parms[3].Value = this._grauEscolaridade.IdGrauEscolaridade;
			parms[4].Value = this._flagInativo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de NivelCurso no banco de dados.
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
		/// Método utilizado para inserir uma instância de NivelCurso no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de NivelCurso no banco de dados.
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
		/// Método utilizado para atualizar uma instância de NivelCurso no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de NivelCurso no banco de dados.
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
		/// Método utilizado para salvar uma instância de NivelCurso no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de NivelCurso no banco de dados.
		/// </summary>
		/// <param name="idNivelCurso">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idNivelCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Nivel_Curso", SqlDbType.Int, 4));

			parms[0].Value = idNivelCurso;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de NivelCurso no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNivelCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idNivelCurso, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Nivel_Curso", SqlDbType.Int, 4));

			parms[0].Value = idNivelCurso;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de NivelCurso no banco de dados.
		/// </summary>
		/// <param name="idNivelCurso">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idNivelCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Nivel_Curso where Idf_Nivel_Curso in (";

			for (int i = 0; i < idNivelCurso.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idNivelCurso[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idNivelCurso">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idNivelCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Nivel_Curso", SqlDbType.Int, 4));

			parms[0].Value = idNivelCurso;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNivelCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idNivelCurso, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Nivel_Curso", SqlDbType.Int, 4));

			parms[0].Value = idNivelCurso;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Niv.Idf_Nivel_Curso, Niv.Des_Nivel_Curso, Niv.Dta_Cadastro, Niv.Idf_Grau_Escolaridade, Niv.Flg_Inativo FROM TAB_Nivel_Curso Niv";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de NivelCurso a partir do banco de dados.
		/// </summary>
		/// <param name="idNivelCurso">Chave do registro.</param>
		/// <returns>Instância de NivelCurso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static NivelCurso LoadObject(int idNivelCurso)
		{
			using (IDataReader dr = LoadDataReader(idNivelCurso))
			{
				NivelCurso objNivelCurso = new NivelCurso();
				if (SetInstance(dr, objNivelCurso))
					return objNivelCurso;
			}
			throw (new RecordNotFoundException(typeof(NivelCurso)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de NivelCurso a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNivelCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de NivelCurso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static NivelCurso LoadObject(int idNivelCurso, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idNivelCurso, trans))
			{
				NivelCurso objNivelCurso = new NivelCurso();
				if (SetInstance(dr, objNivelCurso))
					return objNivelCurso;
			}
			throw (new RecordNotFoundException(typeof(NivelCurso)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de NivelCurso a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idNivelCurso))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de NivelCurso a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idNivelCurso, trans))
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
		/// <param name="objNivelCurso">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, NivelCurso objNivelCurso)
		{
			try
			{
				if (dr.Read())
				{
					objNivelCurso._idNivelCurso = Convert.ToInt32(dr["Idf_Nivel_Curso"]);
					objNivelCurso._descricaoNivelCurso = Convert.ToString(dr["Des_Nivel_Curso"]);
					objNivelCurso._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objNivelCurso._grauEscolaridade = new GrauEscolaridade(Convert.ToInt32(dr["Idf_Grau_Escolaridade"]));
					objNivelCurso._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objNivelCurso._persisted = true;
					objNivelCurso._modified = false;

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