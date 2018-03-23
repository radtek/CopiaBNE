//-- Data: 25/04/2014 15:33
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CobrancaBoletoListaRetorno // Tabela: GLO_Cobranca_Boleto_Lista_Retorno
	{
		#region Atributos
		private int _idCobrancaBoletoListaRetorno;
		private CobrancaBoleto _cobrancaBoleto;
		private int? _idCobrancaBoletoArquivoRetorno;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCobrancaBoletoListaRetorno
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCobrancaBoletoListaRetorno
		{
			get
			{
				return this._idCobrancaBoletoListaRetorno;
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

		#region IdCobrancaBoletoArquivoRetorno
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? IdCobrancaBoletoArquivoRetorno
		{
			get
			{
				return this._idCobrancaBoletoArquivoRetorno;
			}
			set
			{
				this._idCobrancaBoletoArquivoRetorno = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CobrancaBoletoListaRetorno()
		{
		}
		public CobrancaBoletoListaRetorno(int idCobrancaBoletoListaRetorno)
		{
			this._idCobrancaBoletoListaRetorno = idCobrancaBoletoListaRetorno;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO GLO_Cobranca_Boleto_Lista_Retorno (Idf_Cobranca_Boleto, Idf_Cobranca_Boleto_Arquivo_Retorno) VALUES (@Idf_Cobranca_Boleto, @Idf_Cobranca_Boleto_Arquivo_Retorno);SET @Idf_Cobranca_Boleto_Lista_Retorno = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE GLO_Cobranca_Boleto_Lista_Retorno SET Idf_Cobranca_Boleto = @Idf_Cobranca_Boleto, Idf_Cobranca_Boleto_Arquivo_Retorno = @Idf_Cobranca_Boleto_Arquivo_Retorno WHERE Idf_Cobranca_Boleto_Lista_Retorno = @Idf_Cobranca_Boleto_Lista_Retorno";
		private const string SPDELETE = "DELETE FROM GLO_Cobranca_Boleto_Lista_Retorno WHERE Idf_Cobranca_Boleto_Lista_Retorno = @Idf_Cobranca_Boleto_Lista_Retorno";
		private const string SPSELECTID = "SELECT * FROM GLO_Cobranca_Boleto_Lista_Retorno WITH(NOLOCK) WHERE Idf_Cobranca_Boleto_Lista_Retorno = @Idf_Cobranca_Boleto_Lista_Retorno";
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
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Lista_Retorno", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Arquivo_Retorno", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCobrancaBoletoListaRetorno;

			if (this._cobrancaBoleto != null)
				parms[1].Value = this._cobrancaBoleto.IdCobrancaBoleto;
			else
				parms[1].Value = DBNull.Value;


			if (this._idCobrancaBoletoArquivoRetorno.HasValue)
				parms[2].Value = this._idCobrancaBoletoArquivoRetorno;
			else
				parms[2].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de CobrancaBoletoListaRetorno no banco de dados.
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
						this._idCobrancaBoletoListaRetorno = Convert.ToInt32(cmd.Parameters["@Idf_Cobranca_Boleto_Lista_Retorno"].Value);
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
		/// Método utilizado para inserir uma instância de CobrancaBoletoListaRetorno no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCobrancaBoletoListaRetorno = Convert.ToInt32(cmd.Parameters["@Idf_Cobranca_Boleto_Lista_Retorno"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CobrancaBoletoListaRetorno no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CobrancaBoletoListaRetorno no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoListaRetorno no banco de dados.
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoListaRetorno no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CobrancaBoletoListaRetorno no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRetorno">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoListaRetorno)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Lista_Retorno", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoListaRetorno;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CobrancaBoletoListaRetorno no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRetorno">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoListaRetorno, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Lista_Retorno", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoListaRetorno;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CobrancaBoletoListaRetorno no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRetorno">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idCobrancaBoletoListaRetorno)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from GLO_Cobranca_Boleto_Lista_Retorno where Idf_Cobranca_Boleto_Lista_Retorno in (";

			for (int i = 0; i < idCobrancaBoletoListaRetorno.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCobrancaBoletoListaRetorno[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRetorno">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoListaRetorno)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Lista_Retorno", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoListaRetorno;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRetorno">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoListaRetorno, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Lista_Retorno", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoListaRetorno;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cob.Idf_Cobranca_Boleto_Lista_Retorno, Cob.Idf_Cobranca_Boleto, Cob.Idf_Cobranca_Boleto_Arquivo_Retorno FROM GLO_Cobranca_Boleto_Lista_Retorno Cob";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoListaRetorno a partir do banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRetorno">Chave do registro.</param>
		/// <returns>Instância de CobrancaBoletoListaRetorno.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoListaRetorno LoadObject(int idCobrancaBoletoListaRetorno)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoListaRetorno))
			{
				CobrancaBoletoListaRetorno objCobrancaBoletoListaRetorno = new CobrancaBoletoListaRetorno();
				if (SetInstance(dr, objCobrancaBoletoListaRetorno))
					return objCobrancaBoletoListaRetorno;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoListaRetorno)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoListaRetorno a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRetorno">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CobrancaBoletoListaRetorno.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoListaRetorno LoadObject(int idCobrancaBoletoListaRetorno, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoListaRetorno, trans))
			{
				CobrancaBoletoListaRetorno objCobrancaBoletoListaRetorno = new CobrancaBoletoListaRetorno();
				if (SetInstance(dr, objCobrancaBoletoListaRetorno))
					return objCobrancaBoletoListaRetorno;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoListaRetorno)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoListaRetorno a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoListaRetorno))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoListaRetorno a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoListaRetorno, trans))
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
		/// <param name="objCobrancaBoletoListaRetorno">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, CobrancaBoletoListaRetorno objCobrancaBoletoListaRetorno)
		{
			try
			{
				if (dr.Read())
				{
					objCobrancaBoletoListaRetorno._idCobrancaBoletoListaRetorno = Convert.ToInt32(dr["Idf_Cobranca_Boleto_Lista_Retorno"]);
					if (dr["Idf_Cobranca_Boleto"] != DBNull.Value)
						objCobrancaBoletoListaRetorno._cobrancaBoleto = new CobrancaBoleto(Convert.ToInt32(dr["Idf_Cobranca_Boleto"]));
					if (dr["Idf_Cobranca_Boleto_Arquivo_Retorno"] != DBNull.Value)
						objCobrancaBoletoListaRetorno._idCobrancaBoletoArquivoRetorno = Convert.ToInt32(dr["Idf_Cobranca_Boleto_Arquivo_Retorno"]);

					objCobrancaBoletoListaRetorno._persisted = true;
					objCobrancaBoletoListaRetorno._modified = false;

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