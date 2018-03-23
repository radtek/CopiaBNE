//-- Data: 25/04/2014 15:33
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CobrancaBoletoArquivoRemessa // Tabela: GLO_Cobranca_Boleto_Arquivo_Remessa
	{
		#region Atributos
		private int _idCobrancaBoletoArquivoRemessa;
		private DateTime? _dataRemessa;
		private string _arquivoRemessa;
		private string _nomeArquivo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCobrancaBoletoArquivoRemessa
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCobrancaBoletoArquivoRemessa
		{
			get
			{
				return this._idCobrancaBoletoArquivoRemessa;
			}
		}
		#endregion 

		#region DataRemessa
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataRemessa
		{
			get
			{
				return this._dataRemessa;
			}
			set
			{
				this._dataRemessa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ArquivoRemessa
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string ArquivoRemessa
		{
			get
			{
				return this._arquivoRemessa;
			}
			set
			{
				this._arquivoRemessa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeArquivo
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string NomeArquivo
		{
			get
			{
				return this._nomeArquivo;
			}
			set
			{
				this._nomeArquivo = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CobrancaBoletoArquivoRemessa()
		{
		}
		public CobrancaBoletoArquivoRemessa(int idCobrancaBoletoArquivoRemessa)
		{
			this._idCobrancaBoletoArquivoRemessa = idCobrancaBoletoArquivoRemessa;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO GLO_Cobranca_Boleto_Arquivo_Remessa (Dta_Remessa, Arq_Remessa, Nme_Arquivo) VALUES (@Dta_Remessa, @Arq_Remessa, @Nme_Arquivo);SET @Idf_Cobranca_Boleto_Arquivo_Remessa = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE GLO_Cobranca_Boleto_Arquivo_Remessa SET Dta_Remessa = @Dta_Remessa, Arq_Remessa = @Arq_Remessa, Nme_Arquivo = @Nme_Arquivo WHERE Idf_Cobranca_Boleto_Arquivo_Remessa = @Idf_Cobranca_Boleto_Arquivo_Remessa";
		private const string SPDELETE = "DELETE FROM GLO_Cobranca_Boleto_Arquivo_Remessa WHERE Idf_Cobranca_Boleto_Arquivo_Remessa = @Idf_Cobranca_Boleto_Arquivo_Remessa";
		private const string SPSELECTID = "SELECT * FROM GLO_Cobranca_Boleto_Arquivo_Remessa WITH(NOLOCK) WHERE Idf_Cobranca_Boleto_Arquivo_Remessa = @Idf_Cobranca_Boleto_Arquivo_Remessa";
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
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Arquivo_Remessa", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Remessa", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Arq_Remessa", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Nme_Arquivo", SqlDbType.VarChar, 100));
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
			parms[0].Value = this._idCobrancaBoletoArquivoRemessa;

			if (this._dataRemessa.HasValue)
				parms[1].Value = this._dataRemessa;
			else
				parms[1].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._arquivoRemessa))
				parms[2].Value = this._arquivoRemessa;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._nomeArquivo))
				parms[3].Value = this._nomeArquivo;
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
		/// Método utilizado para inserir uma instância de CobrancaBoletoArquivoRemessa no banco de dados.
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
						this._idCobrancaBoletoArquivoRemessa = Convert.ToInt32(cmd.Parameters["@Idf_Cobranca_Boleto_Arquivo_Remessa"].Value);
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
		/// Método utilizado para inserir uma instância de CobrancaBoletoArquivoRemessa no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCobrancaBoletoArquivoRemessa = Convert.ToInt32(cmd.Parameters["@Idf_Cobranca_Boleto_Arquivo_Remessa"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CobrancaBoletoArquivoRemessa no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CobrancaBoletoArquivoRemessa no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoArquivoRemessa no banco de dados.
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoArquivoRemessa no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CobrancaBoletoArquivoRemessa no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoArquivoRemessa">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoArquivoRemessa)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Arquivo_Remessa", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoArquivoRemessa;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CobrancaBoletoArquivoRemessa no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoArquivoRemessa">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoArquivoRemessa, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Arquivo_Remessa", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoArquivoRemessa;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CobrancaBoletoArquivoRemessa no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoArquivoRemessa">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idCobrancaBoletoArquivoRemessa)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from GLO_Cobranca_Boleto_Arquivo_Remessa where Idf_Cobranca_Boleto_Arquivo_Remessa in (";

			for (int i = 0; i < idCobrancaBoletoArquivoRemessa.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCobrancaBoletoArquivoRemessa[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoArquivoRemessa">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoArquivoRemessa)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Arquivo_Remessa", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoArquivoRemessa;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoArquivoRemessa">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoArquivoRemessa, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Arquivo_Remessa", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoArquivoRemessa;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cob.Idf_Cobranca_Boleto_Arquivo_Remessa, Cob.Dta_Remessa, Cob.Arq_Remessa, Cob.Nme_Arquivo FROM GLO_Cobranca_Boleto_Arquivo_Remessa Cob";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoArquivoRemessa a partir do banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoArquivoRemessa">Chave do registro.</param>
		/// <returns>Instância de CobrancaBoletoArquivoRemessa.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoArquivoRemessa LoadObject(int idCobrancaBoletoArquivoRemessa)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoArquivoRemessa))
			{
				CobrancaBoletoArquivoRemessa objCobrancaBoletoArquivoRemessa = new CobrancaBoletoArquivoRemessa();
				if (SetInstance(dr, objCobrancaBoletoArquivoRemessa))
					return objCobrancaBoletoArquivoRemessa;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoArquivoRemessa)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoArquivoRemessa a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoArquivoRemessa">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CobrancaBoletoArquivoRemessa.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoArquivoRemessa LoadObject(int idCobrancaBoletoArquivoRemessa, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoArquivoRemessa, trans))
			{
				CobrancaBoletoArquivoRemessa objCobrancaBoletoArquivoRemessa = new CobrancaBoletoArquivoRemessa();
				if (SetInstance(dr, objCobrancaBoletoArquivoRemessa))
					return objCobrancaBoletoArquivoRemessa;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoArquivoRemessa)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoArquivoRemessa a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoArquivoRemessa))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoArquivoRemessa a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoArquivoRemessa, trans))
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
		/// <param name="objCobrancaBoletoArquivoRemessa">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, CobrancaBoletoArquivoRemessa objCobrancaBoletoArquivoRemessa)
		{
			try
			{
				if (dr.Read())
				{
					objCobrancaBoletoArquivoRemessa._idCobrancaBoletoArquivoRemessa = Convert.ToInt32(dr["Idf_Cobranca_Boleto_Arquivo_Remessa"]);
					if (dr["Dta_Remessa"] != DBNull.Value)
						objCobrancaBoletoArquivoRemessa._dataRemessa = Convert.ToDateTime(dr["Dta_Remessa"]);
					if (dr["Arq_Remessa"] != DBNull.Value)
						objCobrancaBoletoArquivoRemessa._arquivoRemessa = Convert.ToString(dr["Arq_Remessa"]);
					if (dr["Nme_Arquivo"] != DBNull.Value)
						objCobrancaBoletoArquivoRemessa._nomeArquivo = Convert.ToString(dr["Nme_Arquivo"]);

					objCobrancaBoletoArquivoRemessa._persisted = true;
					objCobrancaBoletoArquivoRemessa._modified = false;

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