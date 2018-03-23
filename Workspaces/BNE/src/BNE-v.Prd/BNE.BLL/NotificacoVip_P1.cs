//-- Data: 23/08/2016 10:36
//-- Autor: Marty Sroka

using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class NotificacaoVip // Tabela: BNE_Notificacao_Vip
	{
		#region Atributos
		private Int16 _idNotificacaoVip;
		private string _descricaoNotificacaoVip;
		private Int16 _numeroDia;
		private string _descricaoClasseResponsavel;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdNotificacaoVip
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public Int16 IdNotificacaoVip
		{
			get
			{
				return this._idNotificacaoVip;
			}
		}
		#endregion 

		#region DescricaoNotificacaoVip
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoNotificacaoVip
		{
			get
			{
				return this._descricaoNotificacaoVip;
			}
			set
			{
				this._descricaoNotificacaoVip = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDia
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Int16 NumeroDia
		{
			get
			{
				return this._numeroDia;
			}
			set
			{
				this._numeroDia = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoClasseResponsavel
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoClasseResponsavel
		{
			get
			{
				return this._descricaoClasseResponsavel;
			}
			set
			{
				this._descricaoClasseResponsavel = value;
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

		#endregion

		#region Construtores
		public NotificacaoVip()
		{
		}
		public NotificacaoVip(Int16 idNotificacaoVip)
		{
			this._idNotificacaoVip = idNotificacaoVip;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Notificacao_Vip (Des_Notificacao_Vip, Num_Dia, Des_Classe_Responsavel, Flg_Inativo) VALUES (@Des_Notificacao_Vip, @Num_Dia, @Des_Classe_Responsavel, @Flg_Inativo);SET @Idf_Notificacao_Vip = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Notificacao_Vip SET Des_Notificacao_Vip = @Des_Notificacao_Vip, Num_Dia = @Num_Dia, Des_Classe_Responsavel = @Des_Classe_Responsavel, Flg_Inativo = @Flg_Inativo WHERE Idf_Notificacao_Vip = @Idf_Notificacao_Vip";
		private const string SPDELETE = "DELETE FROM BNE_Notificacao_Vip WHERE Idf_Notificacao_Vip = @Idf_Notificacao_Vip";
		private const string SPSELECTID = "SELECT * FROM BNE_Notificacao_Vip WITH(NOLOCK) WHERE Idf_Notificacao_Vip = @Idf_Notificacao_Vip";
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
			parms.Add(new SqlParameter("@Idf_Notificacao_Vip", SqlDbType.Int, 2));
			parms.Add(new SqlParameter("@Des_Notificacao_Vip", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Num_Dia", SqlDbType.Int, 1));
			parms.Add(new SqlParameter("@Des_Classe_Responsavel", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idNotificacaoVip;
			parms[1].Value = this._descricaoNotificacaoVip;
			parms[2].Value = this._numeroDia;
			parms[3].Value = this._descricaoClasseResponsavel;
			parms[4].Value = this._flagInativo;

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
		/// Método utilizado para inserir uma instância de NotificacaoVip no banco de dados.
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
						this._idNotificacaoVip = Convert.ToInt16(cmd.Parameters["@Idf_Notificacao_Vip"].Value);
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
		/// Método utilizado para inserir uma instância de NotificacaoVip no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idNotificacaoVip = Convert.ToInt16(cmd.Parameters["@Idf_Notificacao_Vip"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de NotificacaoVip no banco de dados.
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
		/// Método utilizado para atualizar uma instância de NotificacaoVip no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de NotificacaoVip no banco de dados.
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
		/// Método utilizado para salvar uma instância de NotificacaoVip no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de NotificacaoVip no banco de dados.
		/// </summary>
		/// <param name="idNotificacaoVip">Chave do registro.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(Int16 idNotificacaoVip)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Notificacao_Vip", SqlDbType.Int, 2));

			parms[0].Value = idNotificacaoVip;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de NotificacaoVip no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNotificacaoVip">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(Int16 idNotificacaoVip, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Notificacao_Vip", SqlDbType.Int, 2));

			parms[0].Value = idNotificacaoVip;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de NotificacaoVip no banco de dados.
		/// </summary>
		/// <param name="idNotificacaoVip">Lista de chaves.</param>
		/// <remarks>Marty Sroka</remarks>
		public static void Delete(List<Int16> idNotificacaoVip)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Notificacao_Vip where Idf_Notificacao_Vip in (";

			for (int i = 0; i < idNotificacaoVip.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 2));
				parms[i].Value = idNotificacaoVip[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idNotificacaoVip">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static IDataReader LoadDataReader(Int16 idNotificacaoVip)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Notificacao_Vip", SqlDbType.Int, 2));

			parms[0].Value = idNotificacaoVip;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNotificacaoVip">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static IDataReader LoadDataReader(Int16 idNotificacaoVip, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Notificacao_Vip", SqlDbType.Int, 2));

			parms[0].Value = idNotificacaoVip;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Not.Idf_Notificacao_Vip, Not.Des_Notificacao_Vip, Not.Num_Dia, Not.Des_Classe_Responsavel, Not.Flg_Inativo FROM BNE_Notificacao_Vip Not";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de NotificacaoVip a partir do banco de dados.
		/// </summary>
		/// <param name="idNotificacaoVip">Chave do registro.</param>
		/// <returns>Instância de NotificacaoVip.</returns>
		/// <remarks>Marty Sroka</remarks>
		public static NotificacaoVip LoadObject(Int16 idNotificacaoVip)
		{
			using (IDataReader dr = LoadDataReader(idNotificacaoVip))
			{
				NotificacaoVip objNotificacaoVip = new NotificacaoVip();
				if (SetInstance(dr, objNotificacaoVip))
					return objNotificacaoVip;
			}
			throw (new RecordNotFoundException(typeof(NotificacaoVip)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de NotificacaoVip a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idNotificacaoVip">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de NotificacaoVip.</returns>
		/// <remarks>Marty Sroka</remarks>
		public static NotificacaoVip LoadObject(Int16 idNotificacaoVip, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idNotificacaoVip, trans))
			{
				NotificacaoVip objNotificacaoVip = new NotificacaoVip();
				if (SetInstance(dr, objNotificacaoVip))
					return objNotificacaoVip;
			}
			throw (new RecordNotFoundException(typeof(NotificacaoVip)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de NotificacaoVip a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idNotificacaoVip))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de NotificacaoVip a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idNotificacaoVip, trans))
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
		/// <param name="objNotificacaoVip">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Marty Sroka</remarks>
		private static bool SetInstance(IDataReader dr, NotificacaoVip objNotificacaoVip, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objNotificacaoVip._idNotificacaoVip = Convert.ToInt16(dr["Idf_Notificacao_Vip"]);
					objNotificacaoVip._descricaoNotificacaoVip = Convert.ToString(dr["Des_Notificacao_Vip"]);
					objNotificacaoVip._numeroDia = Convert.ToInt16(dr["Num_Dia"]);
					objNotificacaoVip._descricaoClasseResponsavel = Convert.ToString(dr["Des_Classe_Responsavel"]);
					objNotificacaoVip._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objNotificacaoVip._persisted = true;
					objNotificacaoVip._modified = false;

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