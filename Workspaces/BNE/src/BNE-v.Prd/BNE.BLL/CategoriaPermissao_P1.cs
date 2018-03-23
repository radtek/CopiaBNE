//-- Data: 04/11/2010 14:03
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CategoriaPermissao // Tabela: plataforma.TAB_Categoria_Permissao
	{
		#region Atributos
		private int _idCategoriaPermissao;
		private string _descricaoCategoriaPermissao;
		private bool _flagInativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCategoriaPermissao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCategoriaPermissao
		{
			get
			{
				return this._idCategoriaPermissao;
			}
			set
			{
				this._idCategoriaPermissao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCategoriaPermissao
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCategoriaPermissao
		{
			get
			{
				return this._descricaoCategoriaPermissao;
			}
			set
			{
				this._descricaoCategoriaPermissao = value;
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
		public CategoriaPermissao()
		{
		}
		public CategoriaPermissao(int idCategoriaPermissao)
		{
			this._idCategoriaPermissao = idCategoriaPermissao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Categoria_Permissao (Idf_Categoria_Permissao, Des_Categoria_Permissao, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Categoria_Permissao, @Des_Categoria_Permissao, @Flg_Inativo, @Dta_Cadastro);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Categoria_Permissao SET Des_Categoria_Permissao = @Des_Categoria_Permissao, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Categoria_Permissao = @Idf_Categoria_Permissao";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Categoria_Permissao WHERE Idf_Categoria_Permissao = @Idf_Categoria_Permissao";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Categoria_Permissao WHERE Idf_Categoria_Permissao = @Idf_Categoria_Permissao";
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
			parms.Add(new SqlParameter("@Idf_Categoria_Permissao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Categoria_Permissao", SqlDbType.VarChar, 100));
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
		/// <remarks>Bruno Flammarion</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idCategoriaPermissao;
			parms[1].Value = this._descricaoCategoriaPermissao;
			parms[2].Value = this._flagInativo;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CategoriaPermissao no banco de dados.
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
		/// Método utilizado para inserir uma instância de CategoriaPermissao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de CategoriaPermissao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CategoriaPermissao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CategoriaPermissao no banco de dados.
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
		/// Método utilizado para salvar uma instância de CategoriaPermissao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CategoriaPermissao no banco de dados.
		/// </summary>
		/// <param name="idCategoriaPermissao">Chave do registro.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idCategoriaPermissao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Categoria_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idCategoriaPermissao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CategoriaPermissao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCategoriaPermissao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(int idCategoriaPermissao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Categoria_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idCategoriaPermissao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CategoriaPermissao no banco de dados.
		/// </summary>
		/// <param name="idCategoriaPermissao">Lista de chaves.</param>
		/// <remarks>Bruno Flammarion</remarks>
		public static void Delete(List<int> idCategoriaPermissao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Categoria_Permissao where Idf_Categoria_Permissao in (";

			for (int i = 0; i < idCategoriaPermissao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCategoriaPermissao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCategoriaPermissao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idCategoriaPermissao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Categoria_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idCategoriaPermissao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCategoriaPermissao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static IDataReader LoadDataReader(int idCategoriaPermissao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Categoria_Permissao", SqlDbType.Int, 4));

			parms[0].Value = idCategoriaPermissao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cat.Idf_Categoria_Permissao, Cat.Des_Categoria_Permissao, Cat.Flg_Inativo, Cat.Dta_Cadastro FROM plataforma.TAB_Categoria_Permissao Cat";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CategoriaPermissao a partir do banco de dados.
		/// </summary>
		/// <param name="idCategoriaPermissao">Chave do registro.</param>
		/// <returns>Instância de CategoriaPermissao.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static CategoriaPermissao LoadObject(int idCategoriaPermissao)
		{
			using (IDataReader dr = LoadDataReader(idCategoriaPermissao))
			{
				CategoriaPermissao objCategoriaPermissao = new CategoriaPermissao();
				if (SetInstance(dr, objCategoriaPermissao))
					return objCategoriaPermissao;
			}
			throw (new RecordNotFoundException(typeof(CategoriaPermissao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CategoriaPermissao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCategoriaPermissao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CategoriaPermissao.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public static CategoriaPermissao LoadObject(int idCategoriaPermissao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCategoriaPermissao, trans))
			{
				CategoriaPermissao objCategoriaPermissao = new CategoriaPermissao();
				if (SetInstance(dr, objCategoriaPermissao))
					return objCategoriaPermissao;
			}
			throw (new RecordNotFoundException(typeof(CategoriaPermissao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CategoriaPermissao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCategoriaPermissao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CategoriaPermissao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCategoriaPermissao, trans))
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
		/// <param name="objCategoriaPermissao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion</remarks>
		private static bool SetInstance(IDataReader dr, CategoriaPermissao objCategoriaPermissao)
		{
			try
			{
				if (dr.Read())
				{
					objCategoriaPermissao._idCategoriaPermissao = Convert.ToInt32(dr["Idf_Categoria_Permissao"]);
					objCategoriaPermissao._descricaoCategoriaPermissao = Convert.ToString(dr["Des_Categoria_Permissao"]);
					objCategoriaPermissao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objCategoriaPermissao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objCategoriaPermissao._persisted = true;
					objCategoriaPermissao._modified = false;

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