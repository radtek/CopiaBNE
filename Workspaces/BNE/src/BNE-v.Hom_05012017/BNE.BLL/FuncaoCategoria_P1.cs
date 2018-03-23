//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class FuncaoCategoria // Tabela: plataforma.TAB_Funcao_Categoria
	{
		#region Atributos
		private int _idFuncaoCategoria;
		private string _descricaoFuncaoCategoria;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private char _codigoFuncaoCategoria;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFuncaoCategoria
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdFuncaoCategoria
		{
			get
			{
				return this._idFuncaoCategoria;
			}
			set
			{
				this._idFuncaoCategoria = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoFuncaoCategoria
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoFuncaoCategoria
		{
			get
			{
				return this._descricaoFuncaoCategoria;
			}
			set
			{
				this._descricaoFuncaoCategoria = value;
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

		#region CodigoFuncaoCategoria
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public char CodigoFuncaoCategoria
		{
			get
			{
				return this._codigoFuncaoCategoria;
			}
			set
			{
				this._codigoFuncaoCategoria = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public FuncaoCategoria()
		{
		}
		public FuncaoCategoria(int idFuncaoCategoria)
		{
			this._idFuncaoCategoria = idFuncaoCategoria;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Funcao_Categoria (Idf_Funcao_Categoria, Des_Funcao_Categoria, Flg_Inativo, Dta_Cadastro, Cod_Funcao_Categoria) VALUES (@Idf_Funcao_Categoria, @Des_Funcao_Categoria, @Flg_Inativo, @Dta_Cadastro, @Cod_Funcao_Categoria);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Funcao_Categoria SET Des_Funcao_Categoria = @Des_Funcao_Categoria, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Cod_Funcao_Categoria = @Cod_Funcao_Categoria WHERE Idf_Funcao_Categoria = @Idf_Funcao_Categoria";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Funcao_Categoria WHERE Idf_Funcao_Categoria = @Idf_Funcao_Categoria";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Funcao_Categoria WHERE Idf_Funcao_Categoria = @Idf_Funcao_Categoria";
        private const string SPSELECT = "SELECT * FROM plataforma.TAB_Funcao_Categoria ORDER BY DES_FUNCAO_CATEGORIA";

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
			parms.Add(new SqlParameter("@Idf_Funcao_Categoria", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Funcao_Categoria", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Cod_Funcao_Categoria", SqlDbType.Char, 1));
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
			parms[0].Value = this._idFuncaoCategoria;
			parms[1].Value = this._descricaoFuncaoCategoria;
			parms[2].Value = this._flagInativo;
			parms[4].Value = this._codigoFuncaoCategoria;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de FuncaoCategoria no banco de dados.
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
		/// Método utilizado para inserir uma instância de FuncaoCategoria no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de FuncaoCategoria no banco de dados.
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
		/// Método utilizado para atualizar uma instância de FuncaoCategoria no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de FuncaoCategoria no banco de dados.
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
		/// Método utilizado para salvar uma instância de FuncaoCategoria no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de FuncaoCategoria no banco de dados.
		/// </summary>
		/// <param name="idFuncaoCategoria">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFuncaoCategoria)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Categoria", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoCategoria;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de FuncaoCategoria no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoCategoria">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFuncaoCategoria, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Categoria", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoCategoria;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de FuncaoCategoria no banco de dados.
		/// </summary>
		/// <param name="idFuncaoCategoria">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idFuncaoCategoria)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Funcao_Categoria where Idf_Funcao_Categoria in (";

			for (int i = 0; i < idFuncaoCategoria.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idFuncaoCategoria[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFuncaoCategoria">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFuncaoCategoria)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Categoria", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoCategoria;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoCategoria">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFuncaoCategoria, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Categoria", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoCategoria;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Fun.Idf_Funcao_Categoria, Fun.Des_Funcao_Categoria, Fun.Flg_Inativo, Fun.Dta_Cadastro, Fun.Cod_Funcao_Categoria FROM plataforma.TAB_Funcao_Categoria Fun";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

        #region Listar
        /// <summary>
        /// Método utilizado para retornar todos os registros de Categoria
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Welington Silva</remarks>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECT, null);
        }
        #endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de FuncaoCategoria a partir do banco de dados.
		/// </summary>
		/// <param name="idFuncaoCategoria">Chave do registro.</param>
		/// <returns>Instância de FuncaoCategoria.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static FuncaoCategoria LoadObject(int idFuncaoCategoria)
		{
			using (IDataReader dr = LoadDataReader(idFuncaoCategoria))
			{
				FuncaoCategoria objFuncaoCategoria = new FuncaoCategoria();
				if (SetInstance(dr, objFuncaoCategoria))
					return objFuncaoCategoria;
			}
			throw (new RecordNotFoundException(typeof(FuncaoCategoria)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de FuncaoCategoria a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoCategoria">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de FuncaoCategoria.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static FuncaoCategoria LoadObject(int idFuncaoCategoria, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFuncaoCategoria, trans))
			{
				FuncaoCategoria objFuncaoCategoria = new FuncaoCategoria();
				if (SetInstance(dr, objFuncaoCategoria))
					return objFuncaoCategoria;
			}
			throw (new RecordNotFoundException(typeof(FuncaoCategoria)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de FuncaoCategoria a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idFuncaoCategoria))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de FuncaoCategoria a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idFuncaoCategoria, trans))
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
		/// <param name="objFuncaoCategoria">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, FuncaoCategoria objFuncaoCategoria)
		{
			try
			{
				if (dr.Read())
				{
					objFuncaoCategoria._idFuncaoCategoria = Convert.ToInt32(dr["Idf_Funcao_Categoria"]);
					objFuncaoCategoria._descricaoFuncaoCategoria = Convert.ToString(dr["Des_Funcao_Categoria"]);
					objFuncaoCategoria._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objFuncaoCategoria._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objFuncaoCategoria._codigoFuncaoCategoria = Convert.ToChar(dr["Cod_Funcao_Categoria"]);

					objFuncaoCategoria._persisted = true;
					objFuncaoCategoria._modified = false;

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