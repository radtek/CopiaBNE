//-- Data: 08/02/2018 12:39
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class LogEnvioCandidatura // Tabela: BNE_Log_Envio_Candidatura
	{
		#region Atributos
		private int _idLogEnvioCandidatura;
		private DateTime? _dataProcessamentoCandidatura;
		private DateTime? _dataProcessamentoCandidaturaEnvio;
		private DateTime? _dataAnaliseCV;
		private DateTime? _dataAnaliseCVEnvio;
		private DateTime? _dataEnvioCV;
		private DateTime? _dataEnvioCVEnvio;
		private Curriculo _curriculo;
		private Vaga _vaga;
		private VagaCandidato _vagaCandidato;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdLogEnvioCandidatura
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdLogEnvioCandidatura
		{
			get
			{
				return this._idLogEnvioCandidatura;
			}
		}
		#endregion 

		#region DataProcessamentoCandidatura
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataProcessamentoCandidatura
		{
			get
			{
				return this._dataProcessamentoCandidatura;
			}
			set
			{
				this._dataProcessamentoCandidatura = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataProcessamentoCandidaturaEnvio
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataProcessamentoCandidaturaEnvio
		{
			get
			{
				return this._dataProcessamentoCandidaturaEnvio;
			}
			set
			{
				this._dataProcessamentoCandidaturaEnvio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataAnaliseCV
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataAnaliseCV
		{
			get
			{
				return this._dataAnaliseCV;
			}
			set
			{
				this._dataAnaliseCV = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataAnaliseCVEnvio
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataAnaliseCVEnvio
		{
			get
			{
				return this._dataAnaliseCVEnvio;
			}
			set
			{
				this._dataAnaliseCVEnvio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataEnvioCV
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataEnvioCV
		{
			get
			{
				return this._dataEnvioCV;
			}
			set
			{
				this._dataEnvioCV = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataEnvioCVEnvio
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataEnvioCVEnvio
		{
			get
			{
				return this._dataEnvioCVEnvio;
			}
			set
			{
				this._dataEnvioCVEnvio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Curriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Curriculo Curriculo
		{
			get
			{
				return this._curriculo;
			}
			set
			{
				this._curriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Vaga
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Vaga Vaga
		{
			get
			{
				return this._vaga;
			}
			set
			{
				this._vaga = value;
				this._modified = true;
			}
		}
		#endregion 

		#region VagaCandidato
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public VagaCandidato VagaCandidato
		{
			get
			{
				return this._vagaCandidato;
			}
			set
			{
				this._vagaCandidato = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public LogEnvioCandidatura()
		{
		}
		public LogEnvioCandidatura(int idLogEnvioCandidatura)
		{
			this._idLogEnvioCandidatura = idLogEnvioCandidatura;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Log_Envio_Candidatura (Dta_Processamento_Candidatura, Dta_Processamento_Candidatura_Envio, Dta_Analise_CV, Dta_Analise_CV_Envio, Dta_Envio_CV, Dta_Envio_CV_Envio, Idf_Curriculo, Idf_Vaga, Idf_Vaga_Candidato) VALUES (@Dta_Processamento_Candidatura, @Dta_Processamento_Candidatura_Envio, @Dta_Analise_CV, @Dta_Analise_CV_Envio, @Dta_Envio_CV, @Dta_Envio_CV_Envio, @Idf_Curriculo, @Idf_Vaga, @Idf_Vaga_Candidato);SET @Idf_Log_Envio_Candidatura = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Log_Envio_Candidatura SET Dta_Processamento_Candidatura = @Dta_Processamento_Candidatura, Dta_Processamento_Candidatura_Envio = @Dta_Processamento_Candidatura_Envio, Dta_Analise_CV = @Dta_Analise_CV, Dta_Analise_CV_Envio = @Dta_Analise_CV_Envio, Dta_Envio_CV = @Dta_Envio_CV, Dta_Envio_CV_Envio = @Dta_Envio_CV_Envio, Idf_Curriculo = @Idf_Curriculo, Idf_Vaga = @Idf_Vaga, Idf_Vaga_Candidato = @Idf_Vaga_Candidato WHERE Idf_Log_Envio_Candidatura = @Idf_Log_Envio_Candidatura";
		private const string SPDELETE = "DELETE FROM BNE_Log_Envio_Candidatura WHERE Idf_Log_Envio_Candidatura = @Idf_Log_Envio_Candidatura";
		private const string SPSELECTID = "SELECT * FROM BNE_Log_Envio_Candidatura WITH(NOLOCK) WHERE Idf_Log_Envio_Candidatura = @Idf_Log_Envio_Candidatura";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Mailson</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Log_Envio_Candidatura", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Processamento_Candidatura", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Processamento_Candidatura_Envio", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Analise_CV", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Analise_CV_Envio", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Envio_CV", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Envio_CV_Envio", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Vaga_Candidato", SqlDbType.Int, 4));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Mailson</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idLogEnvioCandidatura;

			if (this._dataProcessamentoCandidatura.HasValue)
				parms[1].Value = this._dataProcessamentoCandidatura;
			else
				parms[1].Value = DBNull.Value;


			if (this._dataProcessamentoCandidaturaEnvio.HasValue)
				parms[2].Value = this._dataProcessamentoCandidaturaEnvio;
			else
				parms[2].Value = DBNull.Value;


			if (this._dataAnaliseCV.HasValue)
				parms[3].Value = this._dataAnaliseCV;
			else
				parms[3].Value = DBNull.Value;


			if (this._dataAnaliseCVEnvio.HasValue)
				parms[4].Value = this._dataAnaliseCVEnvio;
			else
				parms[4].Value = DBNull.Value;


			if (this._dataEnvioCV.HasValue)
				parms[5].Value = this._dataEnvioCV;
			else
				parms[5].Value = DBNull.Value;


			if (this._dataEnvioCVEnvio.HasValue)
				parms[6].Value = this._dataEnvioCVEnvio;
			else
				parms[6].Value = DBNull.Value;

			parms[7].Value = this._curriculo.IdCurriculo;
			parms[8].Value = this._vaga.IdVaga;
			parms[9].Value = this._vagaCandidato.IdVagaCandidato;

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
		/// Método utilizado para inserir uma instância de LogEnvioCandidatura no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
						this._idLogEnvioCandidatura = Convert.ToInt32(cmd.Parameters["@Idf_Log_Envio_Candidatura"].Value);
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
		/// Método utilizado para inserir uma instância de LogEnvioCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idLogEnvioCandidatura = Convert.ToInt32(cmd.Parameters["@Idf_Log_Envio_Candidatura"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de LogEnvioCandidatura no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para atualizar uma instância de LogEnvioCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para salvar uma instância de LogEnvioCandidatura no banco de dados.
		/// </summary>
		/// <remarks>Mailson</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de LogEnvioCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
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
		/// Método utilizado para excluir uma instância de LogEnvioCandidatura no banco de dados.
		/// </summary>
		/// <param name="idLogEnvioCandidatura">Chave do registro.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idLogEnvioCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Log_Envio_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idLogEnvioCandidatura;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de LogEnvioCandidatura no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idLogEnvioCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(int idLogEnvioCandidatura, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Log_Envio_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idLogEnvioCandidatura;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de LogEnvioCandidatura no banco de dados.
		/// </summary>
		/// <param name="idLogEnvioCandidatura">Lista de chaves.</param>
		/// <remarks>Mailson</remarks>
		public static void Delete(List<int> idLogEnvioCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Log_Envio_Candidatura where Idf_Log_Envio_Candidatura in (";

			for (int i = 0; i < idLogEnvioCandidatura.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idLogEnvioCandidatura[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idLogEnvioCandidatura">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idLogEnvioCandidatura)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Log_Envio_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idLogEnvioCandidatura;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idLogEnvioCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Mailson</remarks>
		private static IDataReader LoadDataReader(int idLogEnvioCandidatura, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Log_Envio_Candidatura", SqlDbType.Int, 4));

			parms[0].Value = idLogEnvioCandidatura;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Log.Idf_Log_Envio_Candidatura, Log.Dta_Processamento_Candidatura, Log.Dta_Processamento_Candidatura_Envio, Log.Dta_Analise_CV, Log.Dta_Analise_CV_Envio, Log.Dta_Envio_CV, Log.Dta_Envio_CV_Envio, Log.Idf_Curriculo, Log.Idf_Vaga, Log.Idf_Vaga_Candidato FROM BNE_Log_Envio_Candidatura Log";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de LogEnvioCandidatura a partir do banco de dados.
		/// </summary>
		/// <param name="idLogEnvioCandidatura">Chave do registro.</param>
		/// <returns>Instância de LogEnvioCandidatura.</returns>
		/// <remarks>Mailson</remarks>
		public static LogEnvioCandidatura LoadObject(int idLogEnvioCandidatura)
		{
			using (IDataReader dr = LoadDataReader(idLogEnvioCandidatura))
			{
				LogEnvioCandidatura objLogEnvioCandidatura = new LogEnvioCandidatura();
				if (SetInstance(dr, objLogEnvioCandidatura))
					return objLogEnvioCandidatura;
			}
			throw (new RecordNotFoundException(typeof(LogEnvioCandidatura)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de LogEnvioCandidatura a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idLogEnvioCandidatura">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de LogEnvioCandidatura.</returns>
		/// <remarks>Mailson</remarks>
		public static LogEnvioCandidatura LoadObject(int idLogEnvioCandidatura, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idLogEnvioCandidatura, trans))
			{
				LogEnvioCandidatura objLogEnvioCandidatura = new LogEnvioCandidatura();
				if (SetInstance(dr, objLogEnvioCandidatura))
					return objLogEnvioCandidatura;
			}
			throw (new RecordNotFoundException(typeof(LogEnvioCandidatura)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de LogEnvioCandidatura a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idLogEnvioCandidatura))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de LogEnvioCandidatura a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idLogEnvioCandidatura, trans))
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
		/// <param name="objLogEnvioCandidatura">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Mailson</remarks>
		private static bool SetInstance(IDataReader dr, LogEnvioCandidatura objLogEnvioCandidatura)
		{
			try
			{
				if (dr.Read())
				{
					objLogEnvioCandidatura._idLogEnvioCandidatura = Convert.ToInt32(dr["Idf_Log_Envio_Candidatura"]);
					if (dr["Dta_Processamento_Candidatura"] != DBNull.Value)
						objLogEnvioCandidatura._dataProcessamentoCandidatura = Convert.ToDateTime(dr["Dta_Processamento_Candidatura"]);
					if (dr["Dta_Processamento_Candidatura_Envio"] != DBNull.Value)
						objLogEnvioCandidatura._dataProcessamentoCandidaturaEnvio = Convert.ToDateTime(dr["Dta_Processamento_Candidatura_Envio"]);
					if (dr["Dta_Analise_CV"] != DBNull.Value)
						objLogEnvioCandidatura._dataAnaliseCV = Convert.ToDateTime(dr["Dta_Analise_CV"]);
					if (dr["Dta_Analise_CV_Envio"] != DBNull.Value)
						objLogEnvioCandidatura._dataAnaliseCVEnvio = Convert.ToDateTime(dr["Dta_Analise_CV_Envio"]);
					if (dr["Dta_Envio_CV"] != DBNull.Value)
						objLogEnvioCandidatura._dataEnvioCV = Convert.ToDateTime(dr["Dta_Envio_CV"]);
					if (dr["Dta_Envio_CV_Envio"] != DBNull.Value)
						objLogEnvioCandidatura._dataEnvioCVEnvio = Convert.ToDateTime(dr["Dta_Envio_CV_Envio"]);
					objLogEnvioCandidatura._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objLogEnvioCandidatura._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
					objLogEnvioCandidatura._vagaCandidato = new VagaCandidato(Convert.ToInt32(dr["Idf_Vaga_Candidato"]));

					objLogEnvioCandidatura._persisted = true;
					objLogEnvioCandidatura._modified = false;

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