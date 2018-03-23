//-- Data: 09/04/2010 14:13
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class StatusMensagemCS // Tabela: BNE_Status_Mensagem_CS
	{
		#region Atributos
		private int _idStatusMensagemCS;
		private DateTime _dataCadastro;
		private bool _flagInativo;
		private string _descricaoStatusMensagem;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdStatusMensagemCS
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdStatusMensagemCS
		{
			get
			{
				return this._idStatusMensagemCS;
			}
			set
			{
				this._idStatusMensagemCS = value;
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

		#region DescricaoStatusMensagem
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoStatusMensagem
		{
			get
			{
				return this._descricaoStatusMensagem;
			}
			set
			{
				this._descricaoStatusMensagem = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public StatusMensagemCS()
		{
		}
		public StatusMensagemCS(int idStatusMensagemCS)
		{
			this._idStatusMensagemCS = idStatusMensagemCS;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Status_Mensagem_CS (Idf_Status_Mensagem_CS, Dta_Cadastro, Flg_Inativo, Des_Status_Mensagem) VALUES (@Idf_Status_Mensagem_CS, @Dta_Cadastro, @Flg_Inativo, @Des_Status_Mensagem);";
		private const string SPUPDATE = "UPDATE BNE_Status_Mensagem_CS SET Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Des_Status_Mensagem = @Des_Status_Mensagem WHERE Idf_Status_Mensagem_CS = @Idf_Status_Mensagem_CS";
		private const string SPDELETE = "DELETE FROM BNE_Status_Mensagem_CS WHERE Idf_Status_Mensagem_CS = @Idf_Status_Mensagem_CS";
		private const string SPSELECTID = "SELECT * FROM BNE_Status_Mensagem_CS WHERE Idf_Status_Mensagem_CS = @Idf_Status_Mensagem_CS";
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
			parms.Add(new SqlParameter("@Idf_Status_Mensagem_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_Status_Mensagem", SqlDbType.VarChar, 50));
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
			parms[0].Value = this._idStatusMensagemCS;
			parms[2].Value = this._flagInativo;
			parms[3].Value = this._descricaoStatusMensagem;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[1].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de StatusMensagemCS no banco de dados.
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
		/// Método utilizado para inserir uma instância de StatusMensagemCS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de StatusMensagemCS no banco de dados.
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
		/// Método utilizado para atualizar uma instância de StatusMensagemCS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de StatusMensagemCS no banco de dados.
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
		/// Método utilizado para salvar uma instância de StatusMensagemCS no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de StatusMensagemCS no banco de dados.
		/// </summary>
		/// <param name="idStatusMensagemCS">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idStatusMensagemCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idStatusMensagemCS;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de StatusMensagemCS no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusMensagemCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idStatusMensagemCS, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idStatusMensagemCS;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de StatusMensagemCS no banco de dados.
		/// </summary>
		/// <param name="idStatusMensagemCS">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idStatusMensagemCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Status_Mensagem_CS where Idf_Status_Mensagem_CS in (";

			for (int i = 0; i < idStatusMensagemCS.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idStatusMensagemCS[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idStatusMensagemCS">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idStatusMensagemCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idStatusMensagemCS;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusMensagemCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idStatusMensagemCS, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Status_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idStatusMensagemCS;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sta.Idf_Status_Mensagem_CS, Sta.Dta_Cadastro, Sta.Flg_Inativo, Sta.Des_Status_Mensagem FROM BNE_Status_Mensagem_CS Sta";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de StatusMensagemCS a partir do banco de dados.
		/// </summary>
		/// <param name="idStatusMensagemCS">Chave do registro.</param>
		/// <returns>Instância de StatusMensagemCS.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static StatusMensagemCS LoadObject(int idStatusMensagemCS)
		{
			using (IDataReader dr = LoadDataReader(idStatusMensagemCS))
			{
				StatusMensagemCS objStatusMensagemCS = new StatusMensagemCS();
				if (SetInstance(dr, objStatusMensagemCS))
					return objStatusMensagemCS;
			}
			throw (new RecordNotFoundException(typeof(StatusMensagemCS)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de StatusMensagemCS a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idStatusMensagemCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de StatusMensagemCS.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static StatusMensagemCS LoadObject(int idStatusMensagemCS, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idStatusMensagemCS, trans))
			{
				StatusMensagemCS objStatusMensagemCS = new StatusMensagemCS();
				if (SetInstance(dr, objStatusMensagemCS))
					return objStatusMensagemCS;
			}
			throw (new RecordNotFoundException(typeof(StatusMensagemCS)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de StatusMensagemCS a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idStatusMensagemCS))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de StatusMensagemCS a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idStatusMensagemCS, trans))
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
		/// <param name="objStatusMensagemCS">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, StatusMensagemCS objStatusMensagemCS)
		{
			try
			{
				if (dr.Read())
				{
					objStatusMensagemCS._idStatusMensagemCS = Convert.ToInt32(dr["Idf_Status_Mensagem_CS"]);
					objStatusMensagemCS._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objStatusMensagemCS._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objStatusMensagemCS._descricaoStatusMensagem = Convert.ToString(dr["Des_Status_Mensagem"]);

					objStatusMensagemCS._persisted = true;
					objStatusMensagemCS._modified = false;

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