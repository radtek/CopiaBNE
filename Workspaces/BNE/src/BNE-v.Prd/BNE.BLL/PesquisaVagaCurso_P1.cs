//-- Data: 21/12/2016 12:00
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class PesquisaVagaCurso // Tabela: BNE_Pesquisa_Vaga_Curso
	{
		#region Atributos
		private int _idPesquisaVagaCurso;
		private PesquisaVaga _pesquisaVaga;
		private string _descricaoCurso;
		private Curso _curso;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPesquisaVagaCurso
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPesquisaVagaCurso
		{
			get
			{
				return this._idPesquisaVagaCurso;
			}
		}
		#endregion 

		#region PesquisaVaga
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public PesquisaVaga PesquisaVaga
		{
			get
			{
				return this._pesquisaVaga;
			}
			set
			{
				this._pesquisaVaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCurso
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCurso
		{
			get
			{
				return this._descricaoCurso;
			}
			set
			{
				this._descricaoCurso = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Curso
		/// <summary>
		/// Campo opcional.
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

		#endregion

		#region Construtores
		public PesquisaVagaCurso()
		{
		}
		public PesquisaVagaCurso(int idPesquisaVagaCurso)
		{
			this._idPesquisaVagaCurso = idPesquisaVagaCurso;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Pesquisa_Vaga_Curso (Idf_Pesquisa_Vaga, Des_Curso, Idf_Curso) VALUES (@Idf_Pesquisa_Vaga, @Des_Curso, @Idf_Curso);SET @Idf_Pesquisa_Vaga_Curso = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Pesquisa_Vaga_Curso SET Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga, Des_Curso = @Des_Curso, Idf_Curso = @Idf_Curso WHERE Idf_Pesquisa_Vaga_Curso = @Idf_Pesquisa_Vaga_Curso";
		private const string SPDELETE = "DELETE FROM BNE_Pesquisa_Vaga_Curso WHERE Idf_Pesquisa_Vaga_Curso = @Idf_Pesquisa_Vaga_Curso";
		private const string SPSELECTID = "SELECT * FROM BNE_Pesquisa_Vaga_Curso WITH(NOLOCK) WHERE Idf_Pesquisa_Vaga_Curso = @Idf_Pesquisa_Vaga_Curso";
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
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Curso", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));
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
			parms[0].Value = this._idPesquisaVagaCurso;
			parms[1].Value = this._pesquisaVaga.IdPesquisaVaga;

			if (!String.IsNullOrEmpty(this._descricaoCurso))
				parms[2].Value = this._descricaoCurso;
			else
				parms[2].Value = DBNull.Value;


			if (this._curso != null)
				parms[3].Value = this._curso.IdCurso;
			else
				parms[3].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de PesquisaVagaCurso no banco de dados.
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
						this._idPesquisaVagaCurso = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga_Curso"].Value);
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
		/// Método utilizado para inserir uma instância de PesquisaVagaCurso no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPesquisaVagaCurso = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Vaga_Curso"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PesquisaVagaCurso no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PesquisaVagaCurso no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PesquisaVagaCurso no banco de dados.
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
		/// Método utilizado para salvar uma instância de PesquisaVagaCurso no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PesquisaVagaCurso no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaCurso">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idPesquisaVagaCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Curso", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaCurso;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PesquisaVagaCurso no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idPesquisaVagaCurso, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Curso", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaCurso;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PesquisaVagaCurso no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaCurso">Lista de chaves.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(List<int> idPesquisaVagaCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Pesquisa_Vaga_Curso where Idf_Pesquisa_Vaga_Curso in (";

			for (int i = 0; i < idPesquisaVagaCurso.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPesquisaVagaCurso[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaCurso">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idPesquisaVagaCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Curso", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaCurso;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idPesquisaVagaCurso, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga_Curso", SqlDbType.Int, 4));

			parms[0].Value = idPesquisaVagaCurso;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Vaga_Curso, Pes.Idf_Pesquisa_Vaga, Pes.Des_Curso, Pes.Idf_Curso FROM BNE_Pesquisa_Vaga_Curso Pes";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaVagaCurso a partir do banco de dados.
		/// </summary>
		/// <param name="idPesquisaVagaCurso">Chave do registro.</param>
		/// <returns>Instância de PesquisaVagaCurso.</returns>
		/// <remarks>Mailson</remarks>
		public static PesquisaVagaCurso LoadObject(int idPesquisaVagaCurso)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaVagaCurso))
			{
				PesquisaVagaCurso objPesquisaVagaCurso = new PesquisaVagaCurso();
				if (SetInstance(dr, objPesquisaVagaCurso))
					return objPesquisaVagaCurso;
			}
			throw (new RecordNotFoundException(typeof(PesquisaVagaCurso)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PesquisaVagaCurso a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPesquisaVagaCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PesquisaVagaCurso.</returns>
		/// <remarks>Mailson</remarks>
		public static PesquisaVagaCurso LoadObject(int idPesquisaVagaCurso, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPesquisaVagaCurso, trans))
			{
				PesquisaVagaCurso objPesquisaVagaCurso = new PesquisaVagaCurso();
				if (SetInstance(dr, objPesquisaVagaCurso))
					return objPesquisaVagaCurso;
			}
			throw (new RecordNotFoundException(typeof(PesquisaVagaCurso)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaVagaCurso a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaVagaCurso))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PesquisaVagaCurso a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPesquisaVagaCurso, trans))
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
		/// <param name="objPesquisaVagaCurso">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, PesquisaVagaCurso objPesquisaVagaCurso, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objPesquisaVagaCurso._idPesquisaVagaCurso = Convert.ToInt32(dr["Idf_Pesquisa_Vaga_Curso"]);
					objPesquisaVagaCurso._pesquisaVaga = new PesquisaVaga(Convert.ToInt32(dr["Idf_Pesquisa_Vaga"]));
					if (dr["Des_Curso"] != DBNull.Value)
						objPesquisaVagaCurso._descricaoCurso = Convert.ToString(dr["Des_Curso"]);
					if (dr["Idf_Curso"] != DBNull.Value)
						objPesquisaVagaCurso._curso = new Curso(Convert.ToInt32(dr["Idf_Curso"]));

					objPesquisaVagaCurso._persisted = true;
					objPesquisaVagaCurso._modified = false;

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