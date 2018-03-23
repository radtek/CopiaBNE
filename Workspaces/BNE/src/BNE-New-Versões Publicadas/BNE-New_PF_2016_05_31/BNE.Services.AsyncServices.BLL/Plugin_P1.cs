//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL.Exceptions;

namespace BNE.Services.AsyncServices.BLL
{
	public partial class Plugin // Tabela: plataforma.TAB_Plugin
	{
		#region Atributos
		private int _idPlugin;
		private TipoPlugin _tipoPlugin;
		private string _descricaoPlugin;
		private string _descricaoPluginMetadata;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private int? _codigoPlugin;
		private TipoAtividade _tipoAtividade;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdPlugin
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdPlugin
		{
			get
			{
				return this._idPlugin;
			}
		}
		#endregion 

		#region TipoPlugin
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoPlugin TipoPlugin
		{
			get
			{
				return this._tipoPlugin;
			}
			set
			{
				this._tipoPlugin = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPlugin
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoPlugin
		{
			get
			{
				return this._descricaoPlugin;
			}
			set
			{
				this._descricaoPlugin = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoPluginMetadata
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoPluginMetadata
		{
			get
			{
				return this._descricaoPluginMetadata;
			}
			set
			{
				this._descricaoPluginMetadata = value;
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

		#region CodigoPlugin
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? CodigoPlugin
		{
			get
			{
				return this._codigoPlugin;
			}
			set
			{
				this._codigoPlugin = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoAtividade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoAtividade TipoAtividade
		{
			get
			{
				return this._tipoAtividade;
			}
			set
			{
				this._tipoAtividade = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Plugin()
		{
		}
		public Plugin(int idPlugin)
		{
			this._idPlugin = idPlugin;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Plugin (Idf_Tipo_Plugin, Des_Plugin, Des_Plugin_Metadata, Flg_Inativo, Dta_Cadastro, Cod_Plugin, Idf_Tipo_Atividade) VALUES (@Idf_Tipo_Plugin, @Des_Plugin, @Des_Plugin_Metadata, @Flg_Inativo, @Dta_Cadastro, @Cod_Plugin, @Idf_Tipo_Atividade);SET @Idf_Plugin = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Plugin SET Idf_Tipo_Plugin = @Idf_Tipo_Plugin, Des_Plugin = @Des_Plugin, Des_Plugin_Metadata = @Des_Plugin_Metadata, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Cod_Plugin = @Cod_Plugin, Idf_Tipo_Atividade = @Idf_Tipo_Atividade WHERE Idf_Plugin = @Idf_Plugin";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Plugin WHERE Idf_Plugin = @Idf_Plugin";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Plugin WHERE Idf_Plugin = @Idf_Plugin";
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
			parms.Add(new SqlParameter("@Idf_Plugin", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Plugin", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Plugin", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Plugin_Metadata", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Cod_Plugin", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Atividade", SqlDbType.Int, 4));
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
			parms[0].Value = this._idPlugin;
			parms[1].Value = this._tipoPlugin.IdTipoPlugin;
			parms[2].Value = this._descricaoPlugin;
			parms[3].Value = this._descricaoPluginMetadata;
			parms[4].Value = this._flagInativo;

			if (this._codigoPlugin.HasValue)
				parms[6].Value = this._codigoPlugin;
			else
				parms[6].Value = DBNull.Value;

			parms[7].Value = this._tipoAtividade.IdTipoAtividade;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[5].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Plugin no banco de dados.
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
						this._idPlugin = Convert.ToInt32(cmd.Parameters["@Idf_Plugin"].Value);
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
		/// Método utilizado para inserir uma instância de Plugin no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idPlugin = Convert.ToInt32(cmd.Parameters["@Idf_Plugin"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Plugin no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Plugin no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Plugin no banco de dados.
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
		/// Método utilizado para salvar uma instância de Plugin no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Plugin no banco de dados.
		/// </summary>
		/// <param name="idPlugin">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPlugin)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plugin", SqlDbType.Int, 4));

			parms[0].Value = idPlugin;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Plugin no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlugin">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idPlugin, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plugin", SqlDbType.Int, 4));

			parms[0].Value = idPlugin;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Plugin no banco de dados.
		/// </summary>
		/// <param name="idPlugin">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idPlugin)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Plugin where Idf_Plugin in (";

			for (int i = 0; i < idPlugin.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idPlugin[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idPlugin">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPlugin)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plugin", SqlDbType.Int, 4));

			parms[0].Value = idPlugin;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlugin">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idPlugin, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Plugin", SqlDbType.Int, 4));

			parms[0].Value = idPlugin;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Plu.Idf_Plugin, Plu.Idf_Tipo_Plugin, Plu.Des_Plugin, Plu.Des_Plugin_Metadata, Plu.Flg_Inativo, Plu.Dta_Cadastro, Plu.Cod_Plugin, Plu.Idf_Tipo_Atividade FROM plataforma.TAB_Plugin Plu";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Plugin a partir do banco de dados.
		/// </summary>
		/// <param name="idPlugin">Chave do registro.</param>
		/// <returns>Instância de Plugin.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Plugin LoadObject(int idPlugin)
		{
			using (IDataReader dr = LoadDataReader(idPlugin))
			{
				Plugin objPlugin = new Plugin();
				if (SetInstance(dr, objPlugin))
					return objPlugin;
			}
			throw (new RecordNotFoundException(typeof(Plugin)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Plugin a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idPlugin">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Plugin.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Plugin LoadObject(int idPlugin, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idPlugin, trans))
			{
				Plugin objPlugin = new Plugin();
				if (SetInstance(dr, objPlugin))
					return objPlugin;
			}
			throw (new RecordNotFoundException(typeof(Plugin)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Plugin a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idPlugin))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Plugin a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idPlugin, trans))
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
		/// <param name="objPlugin">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Plugin objPlugin)
		{
			try
			{
				if (dr.Read())
				{
					objPlugin._idPlugin = Convert.ToInt32(dr["Idf_Plugin"]);
					objPlugin._tipoPlugin = new TipoPlugin(Convert.ToInt32(dr["Idf_Tipo_Plugin"]));
					objPlugin._descricaoPlugin = Convert.ToString(dr["Des_Plugin"]);
					objPlugin._descricaoPluginMetadata = Convert.ToString(dr["Des_Plugin_Metadata"]);
					objPlugin._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objPlugin._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Cod_Plugin"] != DBNull.Value)
						objPlugin._codigoPlugin = Convert.ToInt32(dr["Cod_Plugin"]);
					objPlugin._tipoAtividade = new TipoAtividade(Convert.ToInt32(dr["Idf_Tipo_Atividade"]));

					objPlugin._persisted = true;
					objPlugin._modified = false;

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