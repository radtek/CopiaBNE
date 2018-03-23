//-- Data: 15/07/2010 10:47
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Curso // Tabela: TAB_Curso
	{
		#region Atributos
		private int _idCurso;
		private string _descricaoCurso;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private bool _flagAuditado;
		private bool _flagMEC;
		private NivelCurso _nivelCurso;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCurso
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCurso
		{
			get
			{
				return this._idCurso;
			}
            set
            {
                this._idCurso = value;
                this._modified = true;
            }
		}
		#endregion 

		#region DescricaoCurso
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
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

		#region FlagAuditado
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagAuditado
		{
			get
			{
				return this._flagAuditado;
			}
			set
			{
				this._flagAuditado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagMEC
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagMEC
		{
			get
			{
				return this._flagMEC;
			}
			set
			{
				this._flagMEC = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NivelCurso
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public NivelCurso NivelCurso
		{
			get
			{
				return this._nivelCurso;
			}
			set
			{
				this._nivelCurso = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Curso()
		{
		}
		public Curso(int idCurso)
		{
			this._idCurso = idCurso;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Curso (Des_Curso, Flg_Inativo, Dta_Cadastro, Flg_Auditado, Flg_MEC, Idf_Nivel_Curso) VALUES (@Des_Curso, @Flg_Inativo, @Dta_Cadastro, @Flg_Auditado, @Flg_MEC, @Idf_Nivel_Curso);SET @Idf_Curso = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Curso SET Des_Curso = @Des_Curso, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Flg_Auditado = @Flg_Auditado, Flg_MEC = @Flg_MEC, Idf_Nivel_Curso = @Idf_Nivel_Curso WHERE Idf_Curso = @Idf_Curso";
		private const string SPDELETE = "DELETE FROM TAB_Curso WHERE Idf_Curso = @Idf_Curso";
		private const string SPSELECTID = "SELECT * FROM TAB_Curso WHERE Idf_Curso = @Idf_Curso";
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
			parms.Add(new SqlParameter("@Des_Curso", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Auditado", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_MEC", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Nivel_Curso", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCurso;
			parms[1].Value = this._descricaoCurso;
			parms[2].Value = this._flagInativo;
			parms[4].Value = this._flagAuditado;
			parms[5].Value = this._flagMEC;
			parms[6].Value = this._nivelCurso.IdNivelCurso;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Curso no banco de dados.
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
						this._idCurso = Convert.ToInt32(cmd.Parameters["@Idf_Curso"].Value);
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
		/// Método utilizado para inserir uma instância de Curso no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCurso = Convert.ToInt32(cmd.Parameters["@Idf_Curso"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Curso no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Curso no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Curso no banco de dados.
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
		/// Método utilizado para salvar uma instância de Curso no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Curso no banco de dados.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));

			parms[0].Value = idCurso;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Curso no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCurso, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));

			parms[0].Value = idCurso;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Curso no banco de dados.
		/// </summary>
		/// <param name="idCurso">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Curso where Idf_Curso in (";

			for (int i = 0; i < idCurso.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCurso[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCurso)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));

			parms[0].Value = idCurso;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCurso, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curso", SqlDbType.Int, 4));

			parms[0].Value = idCurso;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curso, Cur.Des_Curso, Cur.Flg_Inativo, Cur.Dta_Cadastro, Cur.Flg_Auditado, Cur.Flg_MEC, Cur.Idf_Nivel_Curso FROM TAB_Curso Cur";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Curso a partir do banco de dados.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <returns>Instância de Curso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Curso LoadObject(int idCurso)
		{
			using (IDataReader dr = LoadDataReader(idCurso))
			{
				Curso objCurso = new Curso();
				if (SetInstance(dr, objCurso))
					return objCurso;
			}
			throw (new RecordNotFoundException(typeof(Curso)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Curso a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurso">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Curso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Curso LoadObject(int idCurso, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurso, trans))
			{
				Curso objCurso = new Curso();
				if (SetInstance(dr, objCurso))
					return objCurso;
			}
			throw (new RecordNotFoundException(typeof(Curso)));
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objCurso">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Curso objCurso)
		{
			try
			{
				if (dr.Read())
				{
					objCurso._idCurso = Convert.ToInt32(dr["Idf_Curso"]);
					objCurso._descricaoCurso = Convert.ToString(dr["Des_Curso"]);
					objCurso._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objCurso._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objCurso._flagAuditado = Convert.ToBoolean(dr["Flg_Auditado"]);
					objCurso._flagMEC = Convert.ToBoolean(dr["Flg_MEC"]);
					objCurso._nivelCurso = new NivelCurso(Convert.ToInt32(dr["Idf_Nivel_Curso"]));

					objCurso._persisted = true;
					objCurso._modified = false;

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