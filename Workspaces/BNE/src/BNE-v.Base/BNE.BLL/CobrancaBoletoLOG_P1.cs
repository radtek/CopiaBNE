//-- Data: 25/04/2014 15:33
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CobrancaBoletoLOG // Tabela: GLO_Cobranca_Boleto_LOG
	{
		#region Atributos
		private int _idCobrancaBoletoLOG;
		private CobrancaBoleto _cobrancaBoleto;
		private DateTime? _dataTransacao;
		private string _descricaoTransacao;
		private MensagemRetornoBoleto _mensagemRetornoBoleto;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCobrancaBoletoLOG
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCobrancaBoletoLOG
		{
			get
			{
				return this._idCobrancaBoletoLOG;
			}
		}
		#endregion 

		#region CobrancaBoleto
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public CobrancaBoleto CobrancaBoleto
		{
			get
			{
				return this._cobrancaBoleto;
			}
			set
			{
				this._cobrancaBoleto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataTransacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataTransacao
		{
			get
			{
				return this._dataTransacao;
			}
			set
			{
				this._dataTransacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoTransacao
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string DescricaoTransacao
		{
			get
			{
				return this._descricaoTransacao;
			}
			set
			{
				this._descricaoTransacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region MensagemRetornoBoleto
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public MensagemRetornoBoleto MensagemRetornoBoleto
		{
			get
			{
				return this._mensagemRetornoBoleto;
			}
			set
			{
				this._mensagemRetornoBoleto = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CobrancaBoletoLOG()
		{
		}
		public CobrancaBoletoLOG(int idCobrancaBoletoLOG)
		{
			this._idCobrancaBoletoLOG = idCobrancaBoletoLOG;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO GLO_Cobranca_Boleto_LOG (Idf_Cobranca_Boleto, Dta_Transacao, Des_Transacao, Idf_Mensagem_Retorno_Boleto) VALUES (@Idf_Cobranca_Boleto, @Dta_Transacao, @Des_Transacao, @Idf_Mensagem_Retorno_Boleto);SET @Idf_Cobranca_Boleto_LOG = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE GLO_Cobranca_Boleto_LOG SET Idf_Cobranca_Boleto = @Idf_Cobranca_Boleto, Dta_Transacao = @Dta_Transacao, Des_Transacao = @Des_Transacao, Idf_Mensagem_Retorno_Boleto = @Idf_Mensagem_Retorno_Boleto WHERE Idf_Cobranca_Boleto_LOG = @Idf_Cobranca_Boleto_LOG";
		private const string SPDELETE = "DELETE FROM GLO_Cobranca_Boleto_LOG WHERE Idf_Cobranca_Boleto_LOG = @Idf_Cobranca_Boleto_LOG";
		private const string SPSELECTID = "SELECT * FROM GLO_Cobranca_Boleto_LOG WITH(NOLOCK) WHERE Idf_Cobranca_Boleto_LOG = @Idf_Cobranca_Boleto_LOG";
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
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_LOG", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Transacao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Transacao", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Idf_Mensagem_Retorno_Boleto", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCobrancaBoletoLOG;

			if (this._cobrancaBoleto != null)
				parms[1].Value = this._cobrancaBoleto.IdCobrancaBoleto;
			else
				parms[1].Value = DBNull.Value;


			if (this._dataTransacao.HasValue)
				parms[2].Value = this._dataTransacao;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoTransacao))
				parms[3].Value = this._descricaoTransacao;
			else
				parms[3].Value = DBNull.Value;


			if (this._mensagemRetornoBoleto != null)
				parms[4].Value = this._mensagemRetornoBoleto.IdMensagemRetornoBoleto;
			else
				parms[4].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de CobrancaBoletoLOG no banco de dados.
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
						this._idCobrancaBoletoLOG = Convert.ToInt32(cmd.Parameters["@Idf_Cobranca_Boleto_LOG"].Value);
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
		/// Método utilizado para inserir uma instância de CobrancaBoletoLOG no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCobrancaBoletoLOG = Convert.ToInt32(cmd.Parameters["@Idf_Cobranca_Boleto_LOG"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CobrancaBoletoLOG no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CobrancaBoletoLOG no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoLOG no banco de dados.
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoLOG no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CobrancaBoletoLOG no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoLOG">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoLOG)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_LOG", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoLOG;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CobrancaBoletoLOG no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoLOG">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoLOG, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_LOG", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoLOG;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CobrancaBoletoLOG no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoLOG">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idCobrancaBoletoLOG)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from GLO_Cobranca_Boleto_LOG where Idf_Cobranca_Boleto_LOG in (";

			for (int i = 0; i < idCobrancaBoletoLOG.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCobrancaBoletoLOG[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoLOG">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoLOG)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_LOG", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoLOG;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoLOG">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoLOG, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_LOG", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoLOG;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cob.Idf_Cobranca_Boleto_LOG, Cob.Idf_Cobranca_Boleto, Cob.Dta_Transacao, Cob.Des_Transacao, Cob.Idf_Mensagem_Retorno_Boleto FROM GLO_Cobranca_Boleto_LOG Cob";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoLOG a partir do banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoLOG">Chave do registro.</param>
		/// <returns>Instância de CobrancaBoletoLOG.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoLOG LoadObject(int idCobrancaBoletoLOG)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoLOG))
			{
				CobrancaBoletoLOG objCobrancaBoletoLOG = new CobrancaBoletoLOG();
				if (SetInstance(dr, objCobrancaBoletoLOG))
					return objCobrancaBoletoLOG;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoLOG)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoLOG a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoLOG">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CobrancaBoletoLOG.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoLOG LoadObject(int idCobrancaBoletoLOG, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoLOG, trans))
			{
				CobrancaBoletoLOG objCobrancaBoletoLOG = new CobrancaBoletoLOG();
				if (SetInstance(dr, objCobrancaBoletoLOG))
					return objCobrancaBoletoLOG;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoLOG)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoLOG a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoLOG))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoLOG a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoLOG, trans))
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
		/// <param name="objCobrancaBoletoLOG">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, CobrancaBoletoLOG objCobrancaBoletoLOG)
		{
			try
			{
				if (dr.Read())
				{
					objCobrancaBoletoLOG._idCobrancaBoletoLOG = Convert.ToInt32(dr["Idf_Cobranca_Boleto_LOG"]);
					if (dr["Idf_Cobranca_Boleto"] != DBNull.Value)
						objCobrancaBoletoLOG._cobrancaBoleto = new CobrancaBoleto(Convert.ToInt32(dr["Idf_Cobranca_Boleto"]));
					if (dr["Dta_Transacao"] != DBNull.Value)
						objCobrancaBoletoLOG._dataTransacao = Convert.ToDateTime(dr["Dta_Transacao"]);
					if (dr["Des_Transacao"] != DBNull.Value)
						objCobrancaBoletoLOG._descricaoTransacao = Convert.ToString(dr["Des_Transacao"]);
					if (dr["Idf_Mensagem_Retorno_Boleto"] != DBNull.Value)
						objCobrancaBoletoLOG._mensagemRetornoBoleto = new MensagemRetornoBoleto(Convert.ToInt32(dr["Idf_Mensagem_Retorno_Boleto"]));

					objCobrancaBoletoLOG._persisted = true;
					objCobrancaBoletoLOG._modified = false;

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