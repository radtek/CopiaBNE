//-- Data: 23/08/2016 10:36
//-- Autor: Marty Sroka

using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class StatusNotificacaoVIP // Tabela: BNE_Status_Notificacao_VIP
	{
		#region Atributos
		private Int16 _idStatusNotificacaoVIP;
		private string _descricaoStatusNotificacaoVIP;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdStatusNotificacaoVIP
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public Int16 IdStatusNotificacaoVIP
		{
			get
			{
				return this._idStatusNotificacaoVIP;
			}
		}
		#endregion 

		#region DescricaoStatusNotificacaoVIP
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoStatusNotificacaoVIP
		{
			get
			{
				return this._descricaoStatusNotificacaoVIP;
			}
			set
			{
				this._descricaoStatusNotificacaoVIP = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public StatusNotificacaoVIP()
		{
		}
		public StatusNotificacaoVIP(Int16 idStatusNotificacaoVIP)
		{
			this._idStatusNotificacaoVIP = idStatusNotificacaoVIP;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Status_Notificacao_VIP (Des_Status_Notificacao_VIP) VALUES (@Des_Status_Notificacao_VIP);SET @Idf_Status_Notificacao_VIP = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Status_Notificacao_VIP SET Des_Status_Notificacao_VIP = @Des_Status_Notificacao_VIP WHERE Idf_Status_Notificacao_VIP = @Idf_Status_Notificacao_VIP";
		private const string SPDELETE = "DELETE FROM BNE_Status_Notificacao_VIP WHERE Idf_Status_Notificacao_VIP = @Idf_Status_Notificacao_VIP";
		private const string SPSELECTID = "SELECT * FROM BNE_Status_Notificacao_VIP WITH(NOLOCK) WHERE Idf_Status_Notificacao_VIP = @Idf_Status_Notificacao_VIP";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Marty Sroka</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Notificacao_VIP", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Des_Status_Notificacao_VIP", SqlDbType.VarChar, 50));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Marty Sroka</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idStatusNotificacaoVIP;

			if (!String.IsNullOrEmpty(this._descricaoStatusNotificacaoVIP))
				parms[1].Value = this._descricaoStatusNotificacaoVIP;
			else
				parms[1].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de StatusNotificacaoVIP no banco de dados.
		/// </summary>
		/// <remarks>Marty Sroka</remarks>
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
						this._idStatusNotificacaoVIP = Convert.ToInt16(cmd.Parameters["@Idf_Status_Notificacao_VIP"].Value);
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
		/// Método utilizado para inserir uma instância de StatusNotificacaoVIP no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idStatusNotificacaoVIP = Convert.ToInt16(cmd.Parameters["@Idf_Status_Notificacao_VIP"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de StatusNotificacaoVIP no banco de dados.
		/// </summary>
		/// <remarks>Marty Sroka</remarks>
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
		/// Método utilizado para atualizar uma instância de StatusNotificacaoVIP no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
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
		/// Método utilizado para salvar uma instância de StatusNotificacaoVIP no banco de dados.
		/// </summary>
		/// <remarks>Marty Sroka</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de StatusNotificacaoVIP no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
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
		/// Método utilizado para excluir uma instância de StatusNotificacaoVIP no banco de dados.
		/// </summary>
		/// <param name="idStatusNotificacaoVIP">Chave do registro.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(Int16 idStatusNotificacaoVIP)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Notificacao_VIP", SqlDbType.Int, 2));

			parms[0].Value = idStatusNotificacaoVIP;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de StatusNotificacaoVIP no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusNotificacaoVIP">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(Int16 idStatusNotificacaoVIP, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Notificacao_VIP", SqlDbType.Int, 2));

			parms[0].Value = idStatusNotificacaoVIP;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de StatusNotificacaoVIP no banco de dados.
		/// </summary>
		/// <param name="idStatusNotificacaoVIP">Lista de chaves.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(List<Int16> idStatusNotificacaoVIP)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Status_Notificacao_VIP where Idf_Status_Notificacao_VIP in (";

			for (int i = 0; i < idStatusNotificacaoVIP.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 2));
				parms[i].Value = idStatusNotificacaoVIP[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idStatusNotificacaoVIP">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static IDataReader LoadDataReader(Int16 idStatusNotificacaoVIP)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Notificacao_VIP", SqlDbType.Int, 2));

			parms[0].Value = idStatusNotificacaoVIP;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusNotificacaoVIP">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static IDataReader LoadDataReader(Int16 idStatusNotificacaoVIP, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Notificacao_VIP", SqlDbType.Int, 2));

			parms[0].Value = idStatusNotificacaoVIP;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sta.Idf_Status_Notificacao_VIP, Sta.Des_Status_Notificacao_VIP FROM BNE_Status_Notificacao_VIP Sta";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de StatusNotificacaoVIP a partir do banco de dados.
		/// </summary>
		/// <param name="idStatusNotificacaoVIP">Chave do registro.</param>
		/// <returns>Instância de StatusNotificacaoVIP.</returns>
		/// <remarks>Marty Sroka</remarks>
		public static StatusNotificacaoVIP LoadObject(Int16 idStatusNotificacaoVIP)
		{
			using (IDataReader dr = LoadDataReader(idStatusNotificacaoVIP))
			{
				StatusNotificacaoVIP objStatusNotificacaoVIP = new StatusNotificacaoVIP();
				if (SetInstance(dr, objStatusNotificacaoVIP))
					return objStatusNotificacaoVIP;
			}
			throw (new RecordNotFoundException(typeof(StatusNotificacaoVIP)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de StatusNotificacaoVIP a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusNotificacaoVIP">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de StatusNotificacaoVIP.</returns>
		/// <remarks>Marty Sroka</remarks>
		public static StatusNotificacaoVIP LoadObject(Int16 idStatusNotificacaoVIP, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idStatusNotificacaoVIP, trans))
			{
				StatusNotificacaoVIP objStatusNotificacaoVIP = new StatusNotificacaoVIP();
				if (SetInstance(dr, objStatusNotificacaoVIP))
					return objStatusNotificacaoVIP;
			}
			throw (new RecordNotFoundException(typeof(StatusNotificacaoVIP)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de StatusNotificacaoVIP a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idStatusNotificacaoVIP))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de StatusNotificacaoVIP a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idStatusNotificacaoVIP, trans))
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
		/// <param name="objStatusNotificacaoVIP">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static bool SetInstance(IDataReader dr, StatusNotificacaoVIP objStatusNotificacaoVIP, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objStatusNotificacaoVIP._idStatusNotificacaoVIP = Convert.ToInt16(dr["Idf_Status_Notificacao_VIP"]);
					if (dr["Des_Status_Notificacao_VIP"] != DBNull.Value)
						objStatusNotificacaoVIP._descricaoStatusNotificacaoVIP = Convert.ToString(dr["Des_Status_Notificacao_VIP"]);

					objStatusNotificacaoVIP._persisted = true;
					objStatusNotificacaoVIP._modified = false;

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