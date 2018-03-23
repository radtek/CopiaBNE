//-- Data: 25/04/2014 15:33
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CobrancaBoletoListaRemessa // Tabela: GLO_Cobranca_Boleto_Lista_Remessa
	{
		#region Atributos
		private int _idCobrancaBoletoListaRemessa;
		private CobrancaBoleto _cobrancaBoleto;
		private int? _idCobrancaBoletoRemessa;
		private MensagemRetornoBoleto _statusCobrancaBoleto;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCobrancaBoletoListaRemessa
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCobrancaBoletoListaRemessa
		{
			get
			{
				return this._idCobrancaBoletoListaRemessa;
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

		#region IdCobrancaBoletoRemessa
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? IdCobrancaBoletoRemessa
		{
			get
			{
				return this._idCobrancaBoletoRemessa;
			}
			set
			{
				this._idCobrancaBoletoRemessa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region StatusCobrancaBoleto
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public MensagemRetornoBoleto StatusCobrancaBoleto
		{
			get
			{
				return this._statusCobrancaBoleto;
			}
			set
			{
				this._statusCobrancaBoleto = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CobrancaBoletoListaRemessa()
		{
		}
		public CobrancaBoletoListaRemessa(int idCobrancaBoletoListaRemessa)
		{
			this._idCobrancaBoletoListaRemessa = idCobrancaBoletoListaRemessa;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO GLO_Cobranca_Boleto_Lista_Remessa (Idf_Cobranca_Boleto, Idf_Cobranca_Boleto_Remessa, Idf_Status_Cobranca_Boleto) VALUES (@Idf_Cobranca_Boleto, @Idf_Cobranca_Boleto_Remessa, @Idf_Status_Cobranca_Boleto);SET @Idf_Cobranca_Boleto_Lista_Remessa = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE GLO_Cobranca_Boleto_Lista_Remessa SET Idf_Cobranca_Boleto = @Idf_Cobranca_Boleto, Idf_Cobranca_Boleto_Remessa = @Idf_Cobranca_Boleto_Remessa, Idf_Status_Cobranca_Boleto = @Idf_Status_Cobranca_Boleto WHERE Idf_Cobranca_Boleto_Lista_Remessa = @Idf_Cobranca_Boleto_Lista_Remessa";
		private const string SPDELETE = "DELETE FROM GLO_Cobranca_Boleto_Lista_Remessa WHERE Idf_Cobranca_Boleto_Lista_Remessa = @Idf_Cobranca_Boleto_Lista_Remessa";
		private const string SPSELECTID = "SELECT * FROM GLO_Cobranca_Boleto_Lista_Remessa WITH(NOLOCK) WHERE Idf_Cobranca_Boleto_Lista_Remessa = @Idf_Cobranca_Boleto_Lista_Remessa";
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
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Lista_Remessa", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Remessa", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Status_Cobranca_Boleto", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCobrancaBoletoListaRemessa;

			if (this._cobrancaBoleto != null)
				parms[1].Value = this._cobrancaBoleto.IdCobrancaBoleto;
			else
				parms[1].Value = DBNull.Value;


			if (this._idCobrancaBoletoRemessa.HasValue)
				parms[2].Value = this._idCobrancaBoletoRemessa;
			else
				parms[2].Value = DBNull.Value;


			if (this._statusCobrancaBoleto != null)
				parms[3].Value = this._statusCobrancaBoleto.IdMensagemRetornoBoleto;
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
		/// Método utilizado para inserir uma instância de CobrancaBoletoListaRemessa no banco de dados.
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
						this._idCobrancaBoletoListaRemessa = Convert.ToInt32(cmd.Parameters["@Idf_Cobranca_Boleto_Lista_Remessa"].Value);
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
		/// Método utilizado para inserir uma instância de CobrancaBoletoListaRemessa no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCobrancaBoletoListaRemessa = Convert.ToInt32(cmd.Parameters["@Idf_Cobranca_Boleto_Lista_Remessa"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CobrancaBoletoListaRemessa no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CobrancaBoletoListaRemessa no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoListaRemessa no banco de dados.
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoListaRemessa no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CobrancaBoletoListaRemessa no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRemessa">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoListaRemessa)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Lista_Remessa", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoListaRemessa;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CobrancaBoletoListaRemessa no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRemessa">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoListaRemessa, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Lista_Remessa", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoListaRemessa;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CobrancaBoletoListaRemessa no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRemessa">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idCobrancaBoletoListaRemessa)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from GLO_Cobranca_Boleto_Lista_Remessa where Idf_Cobranca_Boleto_Lista_Remessa in (";

			for (int i = 0; i < idCobrancaBoletoListaRemessa.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCobrancaBoletoListaRemessa[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRemessa">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoListaRemessa)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Lista_Remessa", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoListaRemessa;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRemessa">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoListaRemessa, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Cobranca_Boleto_Lista_Remessa", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoListaRemessa;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cob.Idf_Cobranca_Boleto_Lista_Remessa, Cob.Idf_Cobranca_Boleto, Cob.Idf_Cobranca_Boleto_Remessa, Cob.Idf_Status_Cobranca_Boleto FROM GLO_Cobranca_Boleto_Lista_Remessa Cob";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoListaRemessa a partir do banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRemessa">Chave do registro.</param>
		/// <returns>Instância de CobrancaBoletoListaRemessa.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoListaRemessa LoadObject(int idCobrancaBoletoListaRemessa)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoListaRemessa))
			{
				CobrancaBoletoListaRemessa objCobrancaBoletoListaRemessa = new CobrancaBoletoListaRemessa();
				if (SetInstance(dr, objCobrancaBoletoListaRemessa))
					return objCobrancaBoletoListaRemessa;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoListaRemessa)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoListaRemessa a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoListaRemessa">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CobrancaBoletoListaRemessa.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoListaRemessa LoadObject(int idCobrancaBoletoListaRemessa, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoListaRemessa, trans))
			{
				CobrancaBoletoListaRemessa objCobrancaBoletoListaRemessa = new CobrancaBoletoListaRemessa();
				if (SetInstance(dr, objCobrancaBoletoListaRemessa))
					return objCobrancaBoletoListaRemessa;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoListaRemessa)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoListaRemessa a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoListaRemessa))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoListaRemessa a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoListaRemessa, trans))
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
		/// <param name="objCobrancaBoletoListaRemessa">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, CobrancaBoletoListaRemessa objCobrancaBoletoListaRemessa)
		{
			try
			{
				if (dr.Read())
				{
					objCobrancaBoletoListaRemessa._idCobrancaBoletoListaRemessa = Convert.ToInt32(dr["Idf_Cobranca_Boleto_Lista_Remessa"]);
					if (dr["Idf_Cobranca_Boleto"] != DBNull.Value)
						objCobrancaBoletoListaRemessa._cobrancaBoleto = new CobrancaBoleto(Convert.ToInt32(dr["Idf_Cobranca_Boleto"]));
					if (dr["Idf_Cobranca_Boleto_Remessa"] != DBNull.Value)
						objCobrancaBoletoListaRemessa._idCobrancaBoletoRemessa = Convert.ToInt32(dr["Idf_Cobranca_Boleto_Remessa"]);
					if (dr["Idf_Status_Cobranca_Boleto"] != DBNull.Value)
						objCobrancaBoletoListaRemessa._statusCobrancaBoleto = new MensagemRetornoBoleto(Convert.ToInt32(dr["Idf_Status_Cobranca_Boleto"]));

					objCobrancaBoletoListaRemessa._persisted = true;
					objCobrancaBoletoListaRemessa._modified = false;

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