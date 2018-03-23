//-- Data: 22/03/2016 10:49
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class UsuarioWebService // Tabela: BNE_Usuario_WebService
	{
		#region Atributos
		private int _idUsuarioWebService;
		private string _descricaoUsuario;
		private string _senhaUsuario;
		private bool? _flagAtivo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdUsuarioWebService
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdUsuarioWebService
		{
			get
			{
				return this._idUsuarioWebService;
			}
		}
		#endregion 

		#region DescricaoUsuario
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoUsuario
		{
			get
			{
				return this._descricaoUsuario;
			}
			set
			{
				this._descricaoUsuario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region SenhaUsuario
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo obrigatório.
		/// </summary>
		public string SenhaUsuario
		{
			get
			{
				return this._senhaUsuario;
			}
			set
			{
				this._senhaUsuario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagAtivo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public bool? FlagAtivo
		{
			get
			{
				return this._flagAtivo;
			}
			set
			{
				this._flagAtivo = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public UsuarioWebService()
		{
		}
		public UsuarioWebService(int idUsuarioWebService)
		{
			this._idUsuarioWebService = idUsuarioWebService;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Usuario_WebService (Des_Usuario, Sen_Usuario, Flg_Ativo) VALUES (@Des_Usuario, @Sen_Usuario, @Flg_Ativo);SET @Idf_Usuario_WebService = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Usuario_WebService SET Des_Usuario = @Des_Usuario, Sen_Usuario = @Sen_Usuario, Flg_Ativo = @Flg_Ativo WHERE Idf_Usuario_WebService = @Idf_Usuario_WebService";
		private const string SPDELETE = "DELETE FROM BNE_Usuario_WebService WHERE Idf_Usuario_WebService = @Idf_Usuario_WebService";
		private const string SPSELECTID = "SELECT * FROM BNE_Usuario_WebService WITH(NOLOCK) WHERE Idf_Usuario_WebService = @Idf_Usuario_WebService";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_WebService", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Usuario", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Sen_Usuario", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Flg_Ativo", SqlDbType.Bit, 1));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idUsuarioWebService;
			parms[1].Value = this._descricaoUsuario;
			parms[2].Value = this._senhaUsuario;

			if (this._flagAtivo.HasValue)
				parms[3].Value = this._flagAtivo;
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
		/// Método utilizado para inserir uma instância de UsuarioWebService no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
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
						this._idUsuarioWebService = Convert.ToInt32(cmd.Parameters["@Idf_Usuario_WebService"].Value);
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
		/// Método utilizado para inserir uma instância de UsuarioWebService no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idUsuarioWebService = Convert.ToInt32(cmd.Parameters["@Idf_Usuario_WebService"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de UsuarioWebService no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para atualizar uma instância de UsuarioWebService no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para salvar uma instância de UsuarioWebService no banco de dados.
		/// </summary>
		/// <remarks>Francisco Ribas</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de UsuarioWebService no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
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
		/// Método utilizado para excluir uma instância de UsuarioWebService no banco de dados.
		/// </summary>
		/// <param name="idUsuarioWebService">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idUsuarioWebService)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_WebService", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioWebService;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de UsuarioWebService no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUsuarioWebService">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idUsuarioWebService, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_WebService", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioWebService;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de UsuarioWebService no banco de dados.
		/// </summary>
		/// <param name="idUsuarioWebService">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idUsuarioWebService)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Usuario_WebService where Idf_Usuario_WebService in (";

			for (int i = 0; i < idUsuarioWebService.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idUsuarioWebService[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idUsuarioWebService">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idUsuarioWebService)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_WebService", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioWebService;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUsuarioWebService">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idUsuarioWebService, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Usuario_WebService", SqlDbType.Int, 4));

			parms[0].Value = idUsuarioWebService;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Usu.Idf_Usuario_WebService, Usu.Des_Usuario, Usu.Sen_Usuario, Usu.Flg_Ativo FROM BNE_Usuario_WebService Usu";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de UsuarioWebService a partir do banco de dados.
		/// </summary>
		/// <param name="idUsuarioWebService">Chave do registro.</param>
		/// <returns>Instância de UsuarioWebService.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static UsuarioWebService LoadObject(int idUsuarioWebService)
		{
			using (IDataReader dr = LoadDataReader(idUsuarioWebService))
			{
				UsuarioWebService objUsuarioWebService = new UsuarioWebService();
				if (SetInstance(dr, objUsuarioWebService))
					return objUsuarioWebService;
			}
			throw (new RecordNotFoundException(typeof(UsuarioWebService)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de UsuarioWebService a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUsuarioWebService">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de UsuarioWebService.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static UsuarioWebService LoadObject(int idUsuarioWebService, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idUsuarioWebService, trans))
			{
				UsuarioWebService objUsuarioWebService = new UsuarioWebService();
				if (SetInstance(dr, objUsuarioWebService))
					return objUsuarioWebService;
			}
			throw (new RecordNotFoundException(typeof(UsuarioWebService)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de UsuarioWebService a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idUsuarioWebService))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de UsuarioWebService a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idUsuarioWebService, trans))
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
		/// <param name="objUsuarioWebService">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, UsuarioWebService objUsuarioWebService)
		{
			try
			{
				if (dr.Read())
				{
					objUsuarioWebService._idUsuarioWebService = Convert.ToInt32(dr["Idf_Usuario_WebService"]);
					objUsuarioWebService._descricaoUsuario = Convert.ToString(dr["Des_Usuario"]);
					objUsuarioWebService._senhaUsuario = Convert.ToString(dr["Sen_Usuario"]);
					if (dr["Flg_Ativo"] != DBNull.Value)
						objUsuarioWebService._flagAtivo = Convert.ToBoolean(dr["Flg_Ativo"]);

					objUsuarioWebService._persisted = true;
					objUsuarioWebService._modified = false;

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