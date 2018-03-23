//-- Data: 05/08/2016 10:59
//-- Autor: Ramalho

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL.Notificacao
{
	public partial class UtmUrlVip // Tabela: alerta.TAB_UtmUrl_VIP
	{
		#region Atributos
		private int _idUtmUrl;
		private string _nomeUtmUrl;
		private string _valorUtmUrl;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdUtmUrl
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdUtmUrl
		{
			get
			{
				return this._idUtmUrl;
			}
			set
			{
				this._idUtmUrl = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeUtmUrl
		/// <summary>
		/// Tamanho do campo: 70.
		/// Campo obrigatório.
		/// </summary>
		public string NomeUtmUrl
		{
			get
			{
				return this._nomeUtmUrl;
			}
			set
			{
				this._nomeUtmUrl = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ValorUtmUrl
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo obrigatório.
		/// </summary>
		public string ValorUtmUrl
		{
			get
			{
				return this._valorUtmUrl;
			}
			set
			{
				this._valorUtmUrl = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public UtmUrlVip()
		{
		}
		public UtmUrlVip(int idUtmUrl)
		{
			this._idUtmUrl = idUtmUrl;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO alerta.TAB_UtmUrl_VIP (Idf_UtmUrl_VIP, Nme_UtmUrl_VIP, Vlr_UtmUrl_VIP) VALUES (@Idf_UtmUrl, @Nme_UtmUrl_VIP, @Vlr_UtmUrl_VIP);";
		private const string SPUPDATE = "UPDATE alerta.TAB_UtmUrl_VIP SET Nme_UtmUrl_VIP = @Nme_UtmUrl_VIP, Vlr_UtmUrl_VIP = @Vlr_UtmUrl_VIP WHERE Idf_UtmUrl = @Idf_UtmUrl";
		private const string SPDELETE = "DELETE FROM alerta.TAB_UtmUrl_VIP WHERE Idf_UtmUrl = @Idf_UtmUrl_VIP";
		private const string SPSELECTID = "SELECT * FROM alerta.TAB_UtmUrl_VIP WITH(NOLOCK) WHERE Idf_UtmUrl_VIP = @Idf_UtmUrl_VIP";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Ramalho</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_UtmUrl_VIP", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_UtmUrl_VIP", SqlDbType.VarChar, 70));
			parms.Add(new SqlParameter("@Vlr_UtmUrl_VIP", SqlDbType.VarChar));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Ramalho</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idUtmUrl;
			parms[1].Value = this._nomeUtmUrl;
			parms[2].Value = this._valorUtmUrl;

			if (!this._persisted)
			{
			}
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de UtmUrl no banco de dados.
		/// </summary>
		/// <remarks>Ramalho</remarks>
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
		/// Método utilizado para inserir uma instância de UtmUrl no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Ramalho</remarks>
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
		/// Método utilizado para atualizar uma instância de UtmUrl no banco de dados.
		/// </summary>
		/// <remarks>Ramalho</remarks>
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
		/// Método utilizado para atualizar uma instância de UtmUrl no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Ramalho</remarks>
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
		/// Método utilizado para salvar uma instância de UtmUrl no banco de dados.
		/// </summary>
		/// <remarks>Ramalho</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de UtmUrl no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Ramalho</remarks>
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
		/// Método utilizado para excluir uma instância de UtmUrl no banco de dados.
		/// </summary>
		/// <param name="idUtmUrl">Chave do registro.</param>
		/// <remarks>Ramalho</remarks>
		public static void Delete(int idUtmUrl)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_UtmUrl_VIP", SqlDbType.Int, 4));

			parms[0].Value = idUtmUrl;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de UtmUrl no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUtmUrl">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Ramalho</remarks>
		public static void Delete(int idUtmUrl, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_UtmUrl_VIP", SqlDbType.Int, 4));

			parms[0].Value = idUtmUrl;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de UtmUrl no banco de dados.
		/// </summary>
		/// <param name="idUtmUrl">Lista de chaves.</param>
		/// <remarks>Ramalho</remarks>
		public static void Delete(List<int> idUtmUrl)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from alerta.TAB_UtmUrl_VIP where Idf_UtmUrl_VIP in (";

			for (int i = 0; i < idUtmUrl.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idUtmUrl[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idUtmUrl">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Ramalho</remarks>
		private static IDataReader LoadDataReader(int idUtmUrl)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_UtmUrl_VIP", SqlDbType.Int, 4));

			parms[0].Value = idUtmUrl;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUtmUrl">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Ramalho</remarks>
		private static IDataReader LoadDataReader(int idUtmUrl, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_UtmUrl_VIP", SqlDbType.Int, 4));

			parms[0].Value = idUtmUrl;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, ta..Idf_UtmUrl_VIP, ta..Nme_UtmUrl_VIP, ta..Vlr_UtmUrl_VIP FROM alerta.TAB_UtmUrl_VIP ta";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de UtmUrl a partir do banco de dados.
		/// </summary>
		/// <param name="idUtmUrlVip">Chave do registro.</param>
		/// <returns>Instância de UtmUrlVip.</returns>
		/// <remarks>Ramalho</remarks>
		public static UtmUrlVip LoadObject(int idUtmUrl)
		{
			using (IDataReader dr = LoadDataReader(idUtmUrl))
			{
                UtmUrlVip objUtmUrl = new UtmUrlVip();
				if (SetInstance(dr, objUtmUrl))
					return objUtmUrl;
			}
			throw (new RecordNotFoundException(typeof(UtmUrl)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de UtmUrl a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idUtmUrl">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de UtmUrl.</returns>
		/// <remarks>Ramalho</remarks>
		public static UtmUrlVip LoadObject(int idUtmUrl, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idUtmUrl, trans))
			{
                UtmUrlVip objUtmUrl = new UtmUrlVip();
				if (SetInstance(dr, objUtmUrl))
					return objUtmUrl;
			}
			throw (new RecordNotFoundException(typeof(UtmUrl)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de UtmUrl a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Ramalho</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idUtmUrl))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de UtmUrl a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Ramalho</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idUtmUrl, trans))
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
		/// <param name="objUtmUrl">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Ramalho</remarks>
		private static bool SetInstance(IDataReader dr, UtmUrlVip objUtmUrl, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objUtmUrl._idUtmUrl = Convert.ToInt32(dr["Idf_UtmUrl_VIP"]);
					objUtmUrl._nomeUtmUrl = Convert.ToString(dr["Nme_UtmUrl_VIP"]);
					objUtmUrl._valorUtmUrl = Convert.ToString(dr["Vlr_UtmUrl_VIP"]);

					objUtmUrl._persisted = true;
					objUtmUrl._modified = false;

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
				if (dispose)
					dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}