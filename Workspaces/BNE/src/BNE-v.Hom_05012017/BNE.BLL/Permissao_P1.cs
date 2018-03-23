//-- Data: 04/11/2010 14:03
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Permissao // Tabela: plataforma.TAB_Permissao
	{
		#region Atributos
		private int _idPermissao;
		private string _descricaoPermissao;
		private CategoriaPermissao _categoriaPermissao;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private string _codigoPermissao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPermissao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdPermissao
		{
			get
			{
				return this._idPermissao;
			}
			set
			{
				this._idPermissao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPermissao
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoPermissao
		{
			get
			{
				return this._descricaoPermissao;
			}
			set
			{
				this._descricaoPermissao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CategoriaPermissao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CategoriaPermissao CategoriaPermissao
		{
			get
			{
				return this._categoriaPermissao;
			}
			set
			{
				this._categoriaPermissao = value;
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

		#region CodigoPermissao
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo obrigatório.
		/// </summary>
		public string CodigoPermissao
		{
			get
			{
				return this._codigoPermissao;
			}
			set
			{
				this._codigoPermissao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Permissao()
		{
		}
		public Permissao(int idPermissao)
		{
			this._idPermissao = idPermissao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Permissao (Idf_Permissao, Des_Permissao, Idf_Categoria_Permissao, Flg_Inativo, Dta_Cadastro, Cod_Permissao) VALUES (@Idf_Permissao, @Des_Permissao, @Idf_Categoria_Permissao, @Flg_Inativo, @Dta_Cadastro, @Cod_Permissao);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Permissao SET Des_Permissao = @Des_Permissao, Idf_Categoria_Permissao = @Idf_Categoria_Permissao, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Cod_Permissao = @Cod_Permissao WHERE Idf_Permissao = @Idf_Permissao";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Permissao WHERE Idf_Permissao = @Idf_Permissao";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Permissao WHERE Idf_Permissao = @Idf_Permissao";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Permissao", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Categoria_Permissao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Cod_Permissao", SqlDbType.VarChar, 10));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Bruno Flammarion</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idPermissao;
			parms[1].Value = this._descricaoPermissao;
			parms[2].Value = this._categoriaPermissao.IdCategoriaPermissao;
			parms[3].Value = this._flagInativo;
			parms[5].Value = this._codigoPermissao;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[4].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Permissao no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para inserir uma instância de Permissao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para atualizar uma instância de Permissao no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para atualizar uma instância de Permissao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para salvar uma instância de Permissao no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de Permissao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
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
		/// Método utilizado para excluir uma instância de Permissao no banco de dados.
		/// </summary>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idPermissao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idPermissao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Permissao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idPermissao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idPermissao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Permissao no banco de dados.
		/// </summary>
		/// <param name="idPermissao">Lista de chaves.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(List<int> idPermissao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Permissao where Idf_Permissao in (";

			for (int i = 0; i < idPermissao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPermissao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idPermissao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idPermissao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idPermissao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idPermissao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Per.Idf_Permissao, Per.Des_Permissao, Per.Idf_Categoria_Permissao, Per.Flg_Inativo, Per.Dta_Cadastro, Per.Cod_Permissao FROM plataforma.TAB_Permissao Per";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Permissao a partir do banco de dados.
		/// </summary>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <returns>Instância de Permissao.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static Permissao LoadObject(int idPermissao)
		{
			using (IDataReader dr = LoadDataReader(idPermissao))
			{
				Permissao objPermissao = new Permissao();
				if (SetInstance(dr, objPermissao))
					return objPermissao;
			}
			throw (new RecordNotFoundException(typeof(Permissao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Permissao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPermissao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Permissao.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static Permissao LoadObject(int idPermissao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPermissao, trans))
			{
				Permissao objPermissao = new Permissao();
				if (SetInstance(dr, objPermissao))
					return objPermissao;
			}
			throw (new RecordNotFoundException(typeof(Permissao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Permissao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPermissao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Permissao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPermissao, trans))
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
		/// <param name="objPermissao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static bool SetInstance(IDataReader dr, Permissao objPermissao)
		{
			try
			{
				if (dr.Read())
				{
					objPermissao._idPermissao = Convert.ToInt32(dr["Idf_Permissao"]);
					objPermissao._descricaoPermissao = Convert.ToString(dr["Des_Permissao"]);
					objPermissao._categoriaPermissao = new CategoriaPermissao(Convert.ToInt32(dr["Idf_Categoria_Permissao"]));
					objPermissao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objPermissao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objPermissao._codigoPermissao = Convert.ToString(dr["Cod_Permissao"]);

					objPermissao._persisted = true;
					objPermissao._modified = false;

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