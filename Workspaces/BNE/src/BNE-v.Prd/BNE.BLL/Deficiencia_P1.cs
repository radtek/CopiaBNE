//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Deficiencia // Tabela: plataforma.TAB_Deficiencia
	{
		#region Atributos
		private int _idDeficiencia;
		private string _descricaoDeficiencia;
		private int _codigoCaged;
		private bool _flagInativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdDeficiencia
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdDeficiencia
		{
			get
			{
				return this._idDeficiencia;
			}
			set
			{
				this._idDeficiencia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoDeficiencia
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoDeficiencia
		{
			get
			{
				return this._descricaoDeficiencia;
			}
			set
			{
				this._descricaoDeficiencia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CodigoCaged
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int CodigoCaged
		{
			get
			{
				return this._codigoCaged;
			}
			set
			{
				this._codigoCaged = value;
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
		public Deficiencia()
		{
		}
		public Deficiencia(int idDeficiencia)
		{
			this._idDeficiencia = idDeficiencia;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Deficiencia (Idf_Deficiencia, Des_Deficiencia, Cod_Caged, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Deficiencia, @Des_Deficiencia, @Cod_Caged, @Flg_Inativo, @Dta_Cadastro);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Deficiencia SET Des_Deficiencia = @Des_Deficiencia, Cod_Caged = @Cod_Caged, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Deficiencia = @Idf_Deficiencia";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Deficiencia WHERE Idf_Deficiencia = @Idf_Deficiencia";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Deficiencia WHERE Idf_Deficiencia = @Idf_Deficiencia";
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
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Deficiencia", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Cod_Caged", SqlDbType.Int, 4));
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
			parms[0].Value = this._idDeficiencia;
			parms[1].Value = this._descricaoDeficiencia;
			parms[2].Value = this._codigoCaged;
			parms[3].Value = this._flagInativo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[4].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Deficiencia no banco de dados.
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
		/// Método utilizado para inserir uma instância de Deficiencia no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de Deficiencia no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Deficiencia no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Deficiencia no banco de dados.
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
		/// Método utilizado para salvar uma instância de Deficiencia no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Deficiencia no banco de dados.
		/// </summary>
		/// <param name="idDeficiencia">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idDeficiencia)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));

			parms[0].Value = idDeficiencia;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Deficiencia no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idDeficiencia">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idDeficiencia, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));

			parms[0].Value = idDeficiencia;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Deficiencia no banco de dados.
		/// </summary>
		/// <param name="idDeficiencia">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idDeficiencia)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Deficiencia where Idf_Deficiencia in (";

			for (int i = 0; i < idDeficiencia.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idDeficiencia[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idDeficiencia">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idDeficiencia)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));

			parms[0].Value = idDeficiencia;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idDeficiencia">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idDeficiencia, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));

			parms[0].Value = idDeficiencia;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Def.Idf_Deficiencia, Def.Des_Deficiencia, Def.Cod_Caged, Def.Flg_Inativo, Def.Dta_Cadastro FROM plataforma.TAB_Deficiencia Def";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Deficiencia a partir do banco de dados.
		/// </summary>
		/// <param name="idDeficiencia">Chave do registro.</param>
		/// <returns>Instância de Deficiencia.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Deficiencia LoadObject(int idDeficiencia)
		{
			using (IDataReader dr = LoadDataReader(idDeficiencia))
			{
				Deficiencia objDeficiencia = new Deficiencia();
				if (SetInstance(dr, objDeficiencia))
					return objDeficiencia;
			}
			throw (new RecordNotFoundException(typeof(Deficiencia)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Deficiencia a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idDeficiencia">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Deficiencia.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Deficiencia LoadObject(int idDeficiencia, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idDeficiencia, trans))
			{
				Deficiencia objDeficiencia = new Deficiencia();
				if (SetInstance(dr, objDeficiencia))
					return objDeficiencia;
			}
			throw (new RecordNotFoundException(typeof(Deficiencia)));
		}
		#endregion

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objDeficiencia">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Deficiencia objDeficiencia)
		{
			try
			{
				if (dr.Read())
				{
					objDeficiencia._idDeficiencia = Convert.ToInt32(dr["Idf_Deficiencia"]);
					objDeficiencia._descricaoDeficiencia = Convert.ToString(dr["Des_Deficiencia"]);
					objDeficiencia._codigoCaged = Convert.ToInt32(dr["Cod_Caged"]);
					objDeficiencia._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objDeficiencia._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objDeficiencia._persisted = true;
					objDeficiencia._modified = false;

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