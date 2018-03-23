//-- Data: 02/05/2014 15:19
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CobrancaBoletoTransacao // Tabela: GLO_Transacao
	{
		#region Atributos
		private int _idCobrancaBoletoTransacao;
		private string _codigoGuid;
		private DateTime? _dataCadastro;
		private Sistema _sistema;
		private string _UrlRetorno;
		private string _descricaoIdentificacao;
		private CobrancaBoletoStatusTransacao _cobrancaBoletoStatusTransacao;
        private CobrancaBoletoTipoTransacao _cobrancaBoletoTipoTransacao;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdCobrancaBoletoTransacao
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdCobrancaBoletoTransacao
		{
			get
			{
				return this._idCobrancaBoletoTransacao;
			}
		}
		#endregion 

		#region CodigoGuid
		/// <summary>
		/// Tamanho do campo: 36.
		/// Campo opcional.
		/// </summary>
		public string CodigoGuid
		{
			get
			{
				return this._codigoGuid;
			}
			set
			{
				this._codigoGuid = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataCadastro
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataCadastro
		{
			get
			{
				return this._dataCadastro;
			}
		}
		#endregion 

		#region Sistema
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public Sistema Sistema
		{
			get
			{
				return this._sistema;
			}
			set
			{
				this._sistema = value;
				this._modified = true;
			}
		}
		#endregion 

		#region UrlRetorno
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string UrlRetorno
		{
			get
			{
				return this._UrlRetorno;
			}
			set
			{
				this._UrlRetorno = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoIdentificacao
		/// <summary>
		/// Tamanho do campo: 80.
		/// Campo opcional.
		/// </summary>
		public string DescricaoIdentificacao
		{
			get
			{
				return this._descricaoIdentificacao;
			}
			set
			{
				this._descricaoIdentificacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region CobrancaBoletoStatusTransacao
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public CobrancaBoletoStatusTransacao CobrancaBoletoStatusTransacao
		{
			get
			{
				return this._cobrancaBoletoStatusTransacao;
			}
			set
			{
				this._cobrancaBoletoStatusTransacao = value;
				this._modified = true;
			}
		}
		#endregion 

        #region CobrancaBoletoTipoTransacao
        /// <summary>
		/// Campo opcional.
		/// </summary>
        public CobrancaBoletoTipoTransacao CobrancaBoletoTipoTransacao
		{
			get
			{
				return this._cobrancaBoletoTipoTransacao;
			}
			set
			{
				this._cobrancaBoletoTipoTransacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public CobrancaBoletoTransacao()
		{
		}
		public CobrancaBoletoTransacao(int idCobrancaBoletoTransacao)
		{
			this._idCobrancaBoletoTransacao = idCobrancaBoletoTransacao;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO GLO_Transacao (Cod_Guid, Dta_Cadastro, Idf_Sistema, Url_Retorno, Des_Identificacao, Idf_Status_Transacao, Idf_Tipo_Transacao) VALUES (@Cod_Guid, @Dta_Cadastro, @Idf_Sistema, @Url_Retorno, @Des_Identificacao, @Idf_Status_Transacao, @Idf_Tipo_Transacao);SET @Idf_Transacao = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE GLO_Transacao SET Cod_Guid = @Cod_Guid, Dta_Cadastro = @Dta_Cadastro, Idf_Sistema = @Idf_Sistema, Url_Retorno = @Url_Retorno, Des_Identificacao = @Des_Identificacao, Idf_Status_Transacao = @Idf_Status_Transacao, Idf_Tipo_Transacao = @Idf_Tipo_Transacao WHERE Idf_Transacao = @Idf_Transacao";
		private const string SPDELETE = "DELETE FROM GLO_Transacao WHERE Idf_Transacao = @Idf_Transacao";
		private const string SPSELECTID = "SELECT * FROM GLO_Transacao WITH(NOLOCK) WHERE Idf_Transacao = @Idf_Transacao";
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
			parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Cod_Guid", SqlDbType.VarChar, 36));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Sistema", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Url_Retorno", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Identificacao", SqlDbType.VarChar, 80));
			parms.Add(new SqlParameter("@Idf_Status_Transacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Transacao", SqlDbType.Int, 4));
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
			parms[0].Value = this._idCobrancaBoletoTransacao;

			if (!String.IsNullOrEmpty(this._codigoGuid))
				parms[1].Value = this._codigoGuid;
			else
				parms[1].Value = DBNull.Value;


			if (this._sistema != null)
				parms[3].Value = this._sistema.IdSistema;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._UrlRetorno))
				parms[4].Value = this._UrlRetorno;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoIdentificacao))
				parms[5].Value = this._descricaoIdentificacao;
			else
				parms[5].Value = DBNull.Value;


			if (this._cobrancaBoletoStatusTransacao != null)
				parms[6].Value = this._cobrancaBoletoStatusTransacao.IdCobrancaBoletoStatusTransacao;
			else
				parms[6].Value = DBNull.Value;


			if (this._cobrancaBoletoTipoTransacao != null)
				parms[7].Value = this._cobrancaBoletoTipoTransacao.IdCobrancaBoletoTipoTransacao;
			else
				parms[7].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[2].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de CobrancaBoletoTransacao no banco de dados.
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
						this._idCobrancaBoletoTransacao = Convert.ToInt32(cmd.Parameters["@Idf_Transacao"].Value);
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
		/// Método utilizado para inserir uma instância de CobrancaBoletoTransacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idCobrancaBoletoTransacao = Convert.ToInt32(cmd.Parameters["@Idf_Transacao"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de CobrancaBoletoTransacao no banco de dados.
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
		/// Método utilizado para atualizar uma instância de CobrancaBoletoTransacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoTransacao no banco de dados.
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
		/// Método utilizado para salvar uma instância de CobrancaBoletoTransacao no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de CobrancaBoletoTransacao no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoTransacao">Chave do registro.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoTransacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoTransacao;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de CobrancaBoletoTransacao no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoTransacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(int idCobrancaBoletoTransacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoTransacao;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de CobrancaBoletoTransacao no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoTransacao">Lista de chaves.</param>
		/// <remarks>Francisco Ribas</remarks>
		public static void Delete(List<int> idCobrancaBoletoTransacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from GLO_CobrancaBoletoTransacao where Idf_Transacao in (";

			for (int i = 0; i < idCobrancaBoletoTransacao.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCobrancaBoletoTransacao[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoTransacao">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoTransacao)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoTransacao;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoTransacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static IDataReader LoadDataReader(int idCobrancaBoletoTransacao, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));

			parms[0].Value = idCobrancaBoletoTransacao;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tra.Idf_Transacao, Tra.Cod_Guid, Tra.Dta_Cadastro, Tra.Idf_Sistema, Tra.Url_Retorno, Tra.Des_Identificacao, Tra.Idf_Status_Transacao, Tra.Idf_Tipo_Transacao FROM GLO_CobrancaBoletoTransacao Tra";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoTransacao a partir do banco de dados.
		/// </summary>
		/// <param name="idCobrancaBoletoTransacao">Chave do registro.</param>
		/// <returns>Instância de CobrancaBoletoTransacao.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoTransacao LoadObject(int idCobrancaBoletoTransacao)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoTransacao))
			{
				CobrancaBoletoTransacao objCobrancaBoletoTransacao = new CobrancaBoletoTransacao();
				if (SetInstance(dr, objCobrancaBoletoTransacao))
					return objCobrancaBoletoTransacao;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoTransacao)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de CobrancaBoletoTransacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCobrancaBoletoTransacao">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de CobrancaBoletoTransacao.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public static CobrancaBoletoTransacao LoadObject(int idCobrancaBoletoTransacao, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCobrancaBoletoTransacao, trans))
			{
				CobrancaBoletoTransacao objCobrancaBoletoTransacao = new CobrancaBoletoTransacao();
				if (SetInstance(dr, objCobrancaBoletoTransacao))
					return objCobrancaBoletoTransacao;
			}
			throw (new RecordNotFoundException(typeof(CobrancaBoletoTransacao)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoTransacao a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoTransacao))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de CobrancaBoletoTransacao a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCobrancaBoletoTransacao, trans))
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
		/// <param name="objCobrancaBoletoTransacao">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Francisco Ribas</remarks>
		private static bool SetInstance(IDataReader dr, CobrancaBoletoTransacao objCobrancaBoletoTransacao)
		{
			try
			{
				if (dr.Read())
				{
					objCobrancaBoletoTransacao._idCobrancaBoletoTransacao = Convert.ToInt32(dr["Idf_Transacao"]);
					if (dr["Cod_Guid"] != DBNull.Value)
						objCobrancaBoletoTransacao._codigoGuid = Convert.ToString(dr["Cod_Guid"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objCobrancaBoletoTransacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Idf_Sistema"] != DBNull.Value)
						objCobrancaBoletoTransacao._sistema = new Sistema(Convert.ToInt32(dr["Idf_Sistema"]));
					if (dr["Url_Retorno"] != DBNull.Value)
						objCobrancaBoletoTransacao._UrlRetorno = Convert.ToString(dr["Url_Retorno"]);
					if (dr["Des_Identificacao"] != DBNull.Value)
						objCobrancaBoletoTransacao._descricaoIdentificacao = Convert.ToString(dr["Des_Identificacao"]);
					if (dr["Idf_Status_Transacao"] != DBNull.Value)
						objCobrancaBoletoTransacao._cobrancaBoletoStatusTransacao = new CobrancaBoletoStatusTransacao(Convert.ToInt32(dr["Idf_Status_Transacao"]));
					if (dr["Idf_Tipo_Transacao"] != DBNull.Value)
                        objCobrancaBoletoTransacao._cobrancaBoletoTipoTransacao = new CobrancaBoletoTipoTransacao(Convert.ToInt32(dr["Idf_Tipo_Transacao"]));

					objCobrancaBoletoTransacao._persisted = true;
					objCobrancaBoletoTransacao._modified = false;

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