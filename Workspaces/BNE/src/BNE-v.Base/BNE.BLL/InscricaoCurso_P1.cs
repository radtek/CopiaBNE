//-- Data: 16/04/2013 16:57
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class InscricaoCurso // Tabela: BNE_Inscricao_Curso
	{
		#region Atributos
		private int _idInscricaoCurso;
		private Curriculo _curriculo;
		private CursoParceiroTecla _cursoParceiroTecla;
		private DateTime _dataInscricao;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdInscricaoCurso
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdInscricaoCurso
		{
			get
			{
				return this._idInscricaoCurso;
			}
		}
		#endregion 

		#region Curriculo
		/// <summary>
		/// Campo obrigatório.
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

		#region CursoParceiroTecla
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CursoParceiroTecla CursoParceiroTecla
		{
			get
			{
				return this._cursoParceiroTecla;
			}
			set
			{
				this._cursoParceiroTecla = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataInscricao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataInscricao
		{
			get
			{
				return this._dataInscricao;
			}
			set
			{
				this._dataInscricao = value;
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
		public InscricaoCurso()
		{
		}
		public InscricaoCurso(int idInscricaoCurso)
		{
			this._idInscricaoCurso = idInscricaoCurso;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Inscricao_Curso (Idf_Curriculo, Idf_Curso_Parceiro_Tecla, Dta_Inscricao, Flg_Inativo) VALUES (@Idf_Curriculo, @Idf_Curso_Parceiro_Tecla, @Dta_Inscricao, @Flg_Inativo);SET @Idf_Inscricao_Curso = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Inscricao_Curso SET Idf_Curriculo = @Idf_Curriculo, Idf_Curso_Parceiro_Tecla = @Idf_Curso_Parceiro_Tecla, Dta_Inscricao = @Dta_Inscricao, Flg_Inativo = @Flg_Inativo WHERE Idf_Inscricao_Curso = @Idf_Inscricao_Curso";
		private const string SPDELETE = "DELETE FROM BNE_Inscricao_Curso WHERE Idf_Inscricao_Curso = @Idf_Inscricao_Curso";
		private const string SPSELECTID = "SELECT * FROM BNE_Inscricao_Curso WITH(NOLOCK) WHERE Idf_Inscricao_Curso = @Idf_Inscricao_Curso";
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
			parms.Add(new SqlParameter("@Idf_Inscricao_Curso", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curso_Parceiro_Tecla", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Inscricao", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idInscricaoCurso;
			parms[1].Value = this._curriculo.IdCurriculo;
			parms[2].Value = this._cursoParceiroTecla.IdCursoParceiroTecla;
			parms[3].Value = this._dataInscricao;
			parms[4].Value = this._flagInativo;

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
		/// Método utilizado para inserir uma instância de InscricaoCurso no banco de dados.
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
						this._idInscricaoCurso = Convert.ToInt32(cmd.Parameters["@Idf_Inscricao_Curso"].Value);
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
		/// Método utilizado para inserir uma instância de InscricaoCurso no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idInscricaoCurso = Convert.ToInt32(cmd.Parameters["@Idf_Inscricao_Curso"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de InscricaoCurso no banco de dados.
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
		/// Método utilizado para atualizar uma instância de InscricaoCurso no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de InscricaoCurso no banco de dados.
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
		/// Método utilizado para salvar uma instância de InscricaoCurso no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de InscricaoCurso no banco de dados.
		/// </summary>
		/// <param name="idInscricaoCurso">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idInscricaoCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Inscricao_Curso", SqlDbType.Int, 4));

			parms[0].Value = idInscricaoCurso;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de InscricaoCurso no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idInscricaoCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idInscricaoCurso, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Inscricao_Curso", SqlDbType.Int, 4));

			parms[0].Value = idInscricaoCurso;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de InscricaoCurso no banco de dados.
		/// </summary>
		/// <param name="idInscricaoCurso">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idInscricaoCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Inscricao_Curso where Idf_Inscricao_Curso in (";

			for (int i = 0; i < idInscricaoCurso.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idInscricaoCurso[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idInscricaoCurso">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idInscricaoCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Inscricao_Curso", SqlDbType.Int, 4));

			parms[0].Value = idInscricaoCurso;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idInscricaoCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idInscricaoCurso, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Inscricao_Curso", SqlDbType.Int, 4));

			parms[0].Value = idInscricaoCurso;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ins.Idf_Inscricao_Curso, Ins.Idf_Curriculo, Ins.Idf_Curso_Parceiro_Tecla, Ins.Dta_Inscricao, Ins.Flg_Inativo FROM BNE_Inscricao_Curso Ins";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de InscricaoCurso a partir do banco de dados.
		/// </summary>
		/// <param name="idInscricaoCurso">Chave do registro.</param>
		/// <returns>Instância de InscricaoCurso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static InscricaoCurso LoadObject(int idInscricaoCurso)
		{
			using (IDataReader dr = LoadDataReader(idInscricaoCurso))
			{
				InscricaoCurso objInscricaoCurso = new InscricaoCurso();
				if (SetInstance(dr, objInscricaoCurso))
					return objInscricaoCurso;
			}
			throw (new RecordNotFoundException(typeof(InscricaoCurso)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de InscricaoCurso a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idInscricaoCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de InscricaoCurso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static InscricaoCurso LoadObject(int idInscricaoCurso, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idInscricaoCurso, trans))
			{
				InscricaoCurso objInscricaoCurso = new InscricaoCurso();
				if (SetInstance(dr, objInscricaoCurso))
					return objInscricaoCurso;
			}
			throw (new RecordNotFoundException(typeof(InscricaoCurso)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de InscricaoCurso a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idInscricaoCurso))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de InscricaoCurso a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idInscricaoCurso, trans))
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
		/// <param name="objInscricaoCurso">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, InscricaoCurso objInscricaoCurso)
		{
			try
			{
				if (dr.Read())
				{
					objInscricaoCurso._idInscricaoCurso = Convert.ToInt32(dr["Idf_Inscricao_Curso"]);
					objInscricaoCurso._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objInscricaoCurso._cursoParceiroTecla = new CursoParceiroTecla(Convert.ToInt32(dr["Idf_Curso_Parceiro_Tecla"]));
					objInscricaoCurso._dataInscricao = Convert.ToDateTime(dr["Dta_Inscricao"]);
					objInscricaoCurso._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objInscricaoCurso._persisted = true;
					objInscricaoCurso._modified = false;

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