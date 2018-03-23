//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL.AsyncServices
{
	public partial class PluginsCompatibilidade // Tabela: plataforma.TAB_Plugins_Compatibilidade
	{
		#region Atributos
		private int _idPluginsCompatibilidade;
		private Plugin _pluginEntrada;
		private Plugin _pluginSaida;
		private bool _flagInativo;
		private DateTime _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPluginsCompatibilidade
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPluginsCompatibilidade
		{
			get
			{
				return this._idPluginsCompatibilidade;
			}
		}
		#endregion 

		#region PluginEntrada
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Plugin PluginEntrada
		{
			get
			{
				return this._pluginEntrada;
			}
			set
			{
				this._pluginEntrada = value;
				this._modified = true;
			}
		}
		#endregion 

		#region PluginSaida
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Plugin PluginSaida
		{
			get
			{
				return this._pluginSaida;
			}
			set
			{
				this._pluginSaida = value;
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
		public PluginsCompatibilidade()
		{
		}
		public PluginsCompatibilidade(int idPluginsCompatibilidade)
		{
			this._idPluginsCompatibilidade = idPluginsCompatibilidade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Plugins_Compatibilidade (Idf_Plugin_Entrada, Idf_Plugin_Saida, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Plugin_Entrada, @Idf_Plugin_Saida, @Flg_Inativo, @Dta_Cadastro);SET @Idf_Plugins_Compatibilidade = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Plugins_Compatibilidade SET Idf_Plugin_Entrada = @Idf_Plugin_Entrada, Idf_Plugin_Saida = @Idf_Plugin_Saida, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Plugins_Compatibilidade = @Idf_Plugins_Compatibilidade";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Plugins_Compatibilidade WHERE Idf_Plugins_Compatibilidade = @Idf_Plugins_Compatibilidade";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Plugins_Compatibilidade WHERE Idf_Plugins_Compatibilidade = @Idf_Plugins_Compatibilidade";
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
			parms.Add(new SqlParameter("@Idf_Plugins_Compatibilidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Plugin_Entrada", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Plugin_Saida", SqlDbType.Int, 4));
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
			parms[0].Value = this._idPluginsCompatibilidade;
			parms[1].Value = this._pluginEntrada.IdPlugin;
			parms[2].Value = this._pluginSaida.IdPlugin;
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
		/// Método utilizado para inserir uma instância de PluginsCompatibilidade no banco de dados.
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
						this._idPluginsCompatibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Plugins_Compatibilidade"].Value);
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
		/// Método utilizado para inserir uma instância de PluginsCompatibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPluginsCompatibilidade = Convert.ToInt32(cmd.Parameters["@Idf_Plugins_Compatibilidade"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de PluginsCompatibilidade no banco de dados.
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
		/// Método utilizado para atualizar uma instância de PluginsCompatibilidade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de PluginsCompatibilidade no banco de dados.
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
		/// Método utilizado para salvar uma instância de PluginsCompatibilidade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de PluginsCompatibilidade no banco de dados.
		/// </summary>
		/// <param name="idPluginsCompatibilidade">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPluginsCompatibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plugins_Compatibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPluginsCompatibilidade;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de PluginsCompatibilidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPluginsCompatibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPluginsCompatibilidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plugins_Compatibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPluginsCompatibilidade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de PluginsCompatibilidade no banco de dados.
		/// </summary>
		/// <param name="idPluginsCompatibilidade">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPluginsCompatibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Plugins_Compatibilidade where Idf_Plugins_Compatibilidade in (";

			for (int i = 0; i < idPluginsCompatibilidade.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPluginsCompatibilidade[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPluginsCompatibilidade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPluginsCompatibilidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plugins_Compatibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPluginsCompatibilidade;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPluginsCompatibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPluginsCompatibilidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plugins_Compatibilidade", SqlDbType.Int, 4));

			parms[0].Value = idPluginsCompatibilidade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Plu.Idf_Plugins_Compatibilidade, Plu.Idf_Plugin_Entrada, Plu.Idf_Plugin_Saida, Plu.Flg_Inativo, Plu.Dta_Cadastro FROM plataforma.TAB_Plugins_Compatibilidade Plu";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de PluginsCompatibilidade a partir do banco de dados.
		/// </summary>
		/// <param name="idPluginsCompatibilidade">Chave do registro.</param>
		/// <returns>Instância de PluginsCompatibilidade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PluginsCompatibilidade LoadObject(int idPluginsCompatibilidade)
		{
			using (IDataReader dr = LoadDataReader(idPluginsCompatibilidade))
			{
				PluginsCompatibilidade objPluginsCompatibilidade = new PluginsCompatibilidade();
				if (SetInstance(dr, objPluginsCompatibilidade))
					return objPluginsCompatibilidade;
			}
			throw (new RecordNotFoundException(typeof(PluginsCompatibilidade)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de PluginsCompatibilidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPluginsCompatibilidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de PluginsCompatibilidade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static PluginsCompatibilidade LoadObject(int idPluginsCompatibilidade, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPluginsCompatibilidade, trans))
			{
				PluginsCompatibilidade objPluginsCompatibilidade = new PluginsCompatibilidade();
				if (SetInstance(dr, objPluginsCompatibilidade))
					return objPluginsCompatibilidade;
			}
			throw (new RecordNotFoundException(typeof(PluginsCompatibilidade)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de PluginsCompatibilidade a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPluginsCompatibilidade))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de PluginsCompatibilidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPluginsCompatibilidade, trans))
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
		/// <param name="objPluginsCompatibilidade">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, PluginsCompatibilidade objPluginsCompatibilidade)
		{
			try
			{
				if (dr.Read())
				{
					objPluginsCompatibilidade._idPluginsCompatibilidade = Convert.ToInt32(dr["Idf_Plugins_Compatibilidade"]);
					objPluginsCompatibilidade._pluginEntrada = new Plugin(Convert.ToInt32(dr["Idf_Plugin_Entrada"]));
					objPluginsCompatibilidade._pluginSaida = new Plugin(Convert.ToInt32(dr["Idf_Plugin_Saida"]));
					objPluginsCompatibilidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objPluginsCompatibilidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objPluginsCompatibilidade._persisted = true;
					objPluginsCompatibilidade._modified = false;

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