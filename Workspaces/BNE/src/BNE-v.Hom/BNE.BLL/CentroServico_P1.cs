//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CentroServico // Tabela: plataforma.TAB_Centro_Servico
	{
		#region Atributos
		private int _idCentroServico;
		private string _descricaoCentroServico;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private string _loginDB;
		private string _senhaDB;
		private string _descricaoSchema;
		private bool _flagRoboSMS;
		private bool _flagRoboEmail;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCentroServico
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCentroServico
		{
			get
			{
				return this._idCentroServico;
			}
			set
			{
				this._idCentroServico = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCentroServico
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoCentroServico
		{
			get
			{
				return this._descricaoCentroServico;
			}
			set
			{
				this._descricaoCentroServico = value;
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

		#region LoginDB
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo obrigatório.
		/// </summary>
		public string LoginDB
		{
			get
			{
				return this._loginDB;
			}
			set
			{
				this._loginDB = value;
				this._modified = true;
			}
		}
		#endregion 

		#region SenhaDB
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo obrigatório.
		/// </summary>
		public string SenhaDB
		{
			get
			{
				return this._senhaDB;
			}
			set
			{
				this._senhaDB = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoSchema
		/// <summary>
		/// Tamanho do campo: 20.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoSchema
		{
			get
			{
				return this._descricaoSchema;
			}
			set
			{
				this._descricaoSchema = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagRoboSMS
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagRoboSMS
		{
			get
			{
				return this._flagRoboSMS;
			}
			set
			{
				this._flagRoboSMS = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagRoboEmail
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagRoboEmail
		{
			get
			{
				return this._flagRoboEmail;
			}
			set
			{
				this._flagRoboEmail = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CentroServico()
		{
		}
		public CentroServico(int idCentroServico)
		{
			this._idCentroServico = idCentroServico;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Centro_Servico (Idf_Centro_Servico, Des_Centro_Servico, Flg_Inativo, Dta_Cadastro, Log_DB, Sen_DB, Des_Schema, Flg_Robo_SMS, Flg_Robo_Email) VALUES (@Idf_Centro_Servico, @Des_Centro_Servico, @Flg_Inativo, @Dta_Cadastro, @Log_DB, @Sen_DB, @Des_Schema, @Flg_Robo_SMS, @Flg_Robo_Email);";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Centro_Servico SET Des_Centro_Servico = @Des_Centro_Servico, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Log_DB = @Log_DB, Sen_DB = @Sen_DB, Des_Schema = @Des_Schema, Flg_Robo_SMS = @Flg_Robo_SMS, Flg_Robo_Email = @Flg_Robo_Email WHERE Idf_Centro_Servico = @Idf_Centro_Servico";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Centro_Servico WHERE Idf_Centro_Servico = @Idf_Centro_Servico";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Centro_Servico WHERE Idf_Centro_Servico = @Idf_Centro_Servico";
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
			parms.Add(new SqlParameter("@Idf_Centro_Servico", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Centro_Servico", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Log_DB", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Sen_DB", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Des_Schema", SqlDbType.VarChar, 20));
			parms.Add(new SqlParameter("@Flg_Robo_SMS", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Robo_Email", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idCentroServico;
			parms[1].Value = this._descricaoCentroServico;
			parms[2].Value = this._flagInativo;
			parms[4].Value = this._loginDB;
			parms[5].Value = this._senhaDB;
			parms[6].Value = this._descricaoSchema;
			parms[7].Value = this._flagRoboSMS;
			parms[8].Value = this._flagRoboEmail;

			if (!this._persisted)
			{
				this._dataCadastro = DateTime.Now;
			}
			parms[3].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CentroServico no banco de dados.
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
		/// Método utilizado para inserir uma instância de CentroServico no banco de dados, dentro de uma transação.
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
		/// Método utilizado para atualizar uma instância de CentroServico no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CentroServico no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CentroServico no banco de dados.
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
		/// Método utilizado para salvar uma instância de CentroServico no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CentroServico no banco de dados.
		/// </summary>
		/// <param name="idCentroServico">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCentroServico)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Centro_Servico", SqlDbType.Int, 4));

			parms[0].Value = idCentroServico;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CentroServico no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCentroServico">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idCentroServico, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Centro_Servico", SqlDbType.Int, 4));

			parms[0].Value = idCentroServico;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CentroServico no banco de dados.
		/// </summary>
		/// <param name="idCentroServico">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idCentroServico)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Centro_Servico where Idf_Centro_Servico in (";

			for (int i = 0; i < idCentroServico.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCentroServico[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCentroServico">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCentroServico)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Centro_Servico", SqlDbType.Int, 4));

			parms[0].Value = idCentroServico;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCentroServico">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idCentroServico, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Centro_Servico", SqlDbType.Int, 4));

			parms[0].Value = idCentroServico;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cen.Idf_Centro_Servico, Cen.Des_Centro_Servico, Cen.Flg_Inativo, Cen.Dta_Cadastro, Cen.Log_DB, Cen.Sen_DB, Cen.Des_Schema, Cen.Flg_Robo_SMS, Cen.Flg_Robo_Email FROM plataforma.TAB_Centro_Servico Cen";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CentroServico a partir do banco de dados.
		/// </summary>
		/// <param name="idCentroServico">Chave do registro.</param>
		/// <returns>Instância de CentroServico.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CentroServico LoadObject(int idCentroServico)
		{
			using (IDataReader dr = LoadDataReader(idCentroServico))
			{
				CentroServico objCentroServico = new CentroServico();
				if (SetInstance(dr, objCentroServico))
					return objCentroServico;
			}
			throw (new RecordNotFoundException(typeof(CentroServico)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CentroServico a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCentroServico">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CentroServico.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static CentroServico LoadObject(int idCentroServico, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCentroServico, trans))
			{
				CentroServico objCentroServico = new CentroServico();
				if (SetInstance(dr, objCentroServico))
					return objCentroServico;
			}
			throw (new RecordNotFoundException(typeof(CentroServico)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CentroServico a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCentroServico))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CentroServico a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCentroServico, trans))
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
		/// <param name="objCentroServico">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, CentroServico objCentroServico)
		{
			try
			{
				if (dr.Read())
				{
					objCentroServico._idCentroServico = Convert.ToInt32(dr["Idf_Centro_Servico"]);
					objCentroServico._descricaoCentroServico = Convert.ToString(dr["Des_Centro_Servico"]);
					objCentroServico._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objCentroServico._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objCentroServico._loginDB = Convert.ToString(dr["Log_DB"]);
					objCentroServico._senhaDB = Convert.ToString(dr["Sen_DB"]);
					objCentroServico._descricaoSchema = Convert.ToString(dr["Des_Schema"]);
					objCentroServico._flagRoboSMS = Convert.ToBoolean(dr["Flg_Robo_SMS"]);
					objCentroServico._flagRoboEmail = Convert.ToBoolean(dr["Flg_Robo_Email"]);

					objCentroServico._persisted = true;
					objCentroServico._modified = false;

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