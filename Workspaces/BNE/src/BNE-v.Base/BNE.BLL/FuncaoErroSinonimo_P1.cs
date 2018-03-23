//-- Data: 30/03/2010 10:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class FuncaoErroSinonimo // Tabela: BNE_Funcao_Erro_Sinonimo
	{
		#region Atributos
		private int _idFuncaoErroSinonimo;
		private string _descricaoFuncaoErroSinonimo;
		private bool _flagErro;
		private DateTime _dataCadastro;
		private bool _flagInativo;
		private Funcao _funcao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdFuncaoErroSinonimo
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdFuncaoErroSinonimo
		{
			get
			{
				return this._idFuncaoErroSinonimo;
			}
		}
		#endregion 

		#region DescricaoFuncaoErroSinonimo
		/// <summary>
		/// Tamanho do campo: 255.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoFuncaoErroSinonimo
		{
			get
			{
				return this._descricaoFuncaoErroSinonimo;
			}
			set
			{
				this._descricaoFuncaoErroSinonimo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagErro
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagErro
		{
			get
			{
				return this._flagErro;
			}
			set
			{
				this._flagErro = value;
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

		#region Funcao
		/// <summary>
		/// Campo opcional.
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

		#endregion

		#region Construtores
		public FuncaoErroSinonimo()
		{
		}
		public FuncaoErroSinonimo(int idFuncaoErroSinonimo)
		{
			this._idFuncaoErroSinonimo = idFuncaoErroSinonimo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Funcao_Erro_Sinonimo (Des_Funcao_Erro_Sinonimo, Flg_Erro, Dta_Cadastro, Flg_Inativo, Idf_Funcao) VALUES (@Des_Funcao_Erro_Sinonimo, @Flg_Erro, @Dta_Cadastro, @Flg_Inativo, @Idf_Funcao);SET @Idf_Funcao_Erro_Sinonimo = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Funcao_Erro_Sinonimo SET Des_Funcao_Erro_Sinonimo = @Des_Funcao_Erro_Sinonimo, Flg_Erro = @Flg_Erro, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Idf_Funcao = @Idf_Funcao WHERE Idf_Funcao_Erro_Sinonimo = @Idf_Funcao_Erro_Sinonimo";
		private const string SPDELETE = "DELETE FROM BNE_Funcao_Erro_Sinonimo WHERE Idf_Funcao_Erro_Sinonimo = @Idf_Funcao_Erro_Sinonimo";
		private const string SPSELECTID = "SELECT * FROM BNE_Funcao_Erro_Sinonimo WHERE Idf_Funcao_Erro_Sinonimo = @Idf_Funcao_Erro_Sinonimo";
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
			parms.Add(new SqlParameter("@Idf_Funcao_Erro_Sinonimo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Funcao_Erro_Sinonimo", SqlDbType.VarChar, 255));
			parms.Add(new SqlParameter("@Flg_Erro", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
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
			parms[0].Value = this._idFuncaoErroSinonimo;
			parms[1].Value = this._descricaoFuncaoErroSinonimo;
			parms[2].Value = this._flagErro;
			parms[4].Value = this._flagInativo;

			if (this._funcao != null)
				parms[5].Value = this._funcao.IdFuncao;
			else
				parms[5].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de FuncaoErroSinonimo no banco de dados.
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
						this._idFuncaoErroSinonimo = Convert.ToInt32(cmd.Parameters["@Idf_Funcao_Erro_Sinonimo"].Value);
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
		/// Método utilizado para inserir uma instância de FuncaoErroSinonimo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idFuncaoErroSinonimo = Convert.ToInt32(cmd.Parameters["@Idf_Funcao_Erro_Sinonimo"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de FuncaoErroSinonimo no banco de dados.
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
		/// Método utilizado para atualizar uma instância de FuncaoErroSinonimo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de FuncaoErroSinonimo no banco de dados.
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
		/// Método utilizado para salvar uma instância de FuncaoErroSinonimo no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de FuncaoErroSinonimo no banco de dados.
		/// </summary>
		/// <param name="idFuncaoErroSinonimo">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFuncaoErroSinonimo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Erro_Sinonimo", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoErroSinonimo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de FuncaoErroSinonimo no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoErroSinonimo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idFuncaoErroSinonimo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Erro_Sinonimo", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoErroSinonimo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de FuncaoErroSinonimo no banco de dados.
		/// </summary>
		/// <param name="idFuncaoErroSinonimo">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idFuncaoErroSinonimo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Funcao_Erro_Sinonimo where Idf_Funcao_Erro_Sinonimo in (";

			for (int i = 0; i < idFuncaoErroSinonimo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idFuncaoErroSinonimo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idFuncaoErroSinonimo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFuncaoErroSinonimo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Erro_Sinonimo", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoErroSinonimo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoErroSinonimo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idFuncaoErroSinonimo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Funcao_Erro_Sinonimo", SqlDbType.Int, 4));

			parms[0].Value = idFuncaoErroSinonimo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Fun.Idf_Funcao_Erro_Sinonimo, Fun.Des_Funcao_Erro_Sinonimo, Fun.Flg_Erro, Fun.Dta_Cadastro, Fun.Flg_Inativo, Fun.Idf_Funcao FROM BNE_Funcao_Erro_Sinonimo Fun";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de FuncaoErroSinonimo a partir do banco de dados.
		/// </summary>
		/// <param name="idFuncaoErroSinonimo">Chave do registro.</param>
		/// <returns>Instância de FuncaoErroSinonimo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static FuncaoErroSinonimo LoadObject(int idFuncaoErroSinonimo)
		{
			using (IDataReader dr = LoadDataReader(idFuncaoErroSinonimo))
			{
				FuncaoErroSinonimo objFuncaoErroSinonimo = new FuncaoErroSinonimo();
				if (SetInstance(dr, objFuncaoErroSinonimo))
					return objFuncaoErroSinonimo;
			}
			throw (new RecordNotFoundException(typeof(FuncaoErroSinonimo)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de FuncaoErroSinonimo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idFuncaoErroSinonimo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de FuncaoErroSinonimo.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static FuncaoErroSinonimo LoadObject(int idFuncaoErroSinonimo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idFuncaoErroSinonimo, trans))
			{
				FuncaoErroSinonimo objFuncaoErroSinonimo = new FuncaoErroSinonimo();
				if (SetInstance(dr, objFuncaoErroSinonimo))
					return objFuncaoErroSinonimo;
			}
			throw (new RecordNotFoundException(typeof(FuncaoErroSinonimo)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de FuncaoErroSinonimo a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idFuncaoErroSinonimo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de FuncaoErroSinonimo a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idFuncaoErroSinonimo, trans))
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
		/// <param name="objFuncaoErroSinonimo">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, FuncaoErroSinonimo objFuncaoErroSinonimo)
		{
			try
			{
				if (dr.Read())
				{
					objFuncaoErroSinonimo._idFuncaoErroSinonimo = Convert.ToInt32(dr["Idf_Funcao_Erro_Sinonimo"]);
					objFuncaoErroSinonimo._descricaoFuncaoErroSinonimo = Convert.ToString(dr["Des_Funcao_Erro_Sinonimo"]);
					objFuncaoErroSinonimo._flagErro = Convert.ToBoolean(dr["Flg_Erro"]);
					objFuncaoErroSinonimo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objFuncaoErroSinonimo._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					if (dr["Idf_Funcao"] != DBNull.Value)
						objFuncaoErroSinonimo._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));

					objFuncaoErroSinonimo._persisted = true;
					objFuncaoErroSinonimo._modified = false;

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