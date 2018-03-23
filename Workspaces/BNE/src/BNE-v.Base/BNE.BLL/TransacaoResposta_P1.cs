//-- Data: 28/02/2013 11:38
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class TransacaoResposta // Tabela: BNE_Transacao_Resposta
	{
		#region Atributos
		private int _idTransacaoResposta;
		private Transacao _transacao;
		private bool _flagTransacaoAprovada;
		private string _descricaoResultadoSolicitacaoAprovacao;
		private string _descricaoCodigoAutorizacao;
		private string _descricaoTransacao;
		private string _descricaoCartaoMascarado;
		private decimal? _numeroSequencialUnico;
		private string _descricaoComprovanteAdministradora;
		private string _descricaoNacionalidadeEmissor;
		private DateTime? _dataCadastro;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdTransacaoResposta
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdTransacaoResposta
		{
			get
			{
				return this._idTransacaoResposta;
			}
		}
		#endregion 

		#region Transacao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Transacao Transacao
		{
			get
			{
				return this._transacao;
			}
			set
			{
				this._transacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagTransacaoAprovada
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagTransacaoAprovada
		{
			get
			{
				return this._flagTransacaoAprovada;
			}
			set
			{
				this._flagTransacaoAprovada = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoResultadoSolicitacaoAprovacao
		/// <summary>
		/// Tamanho do campo: 200.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoResultadoSolicitacaoAprovacao
		{
			get
			{
				return this._descricaoResultadoSolicitacaoAprovacao;
			}
			set
			{
				this._descricaoResultadoSolicitacaoAprovacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoCodigoAutorizacao
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCodigoAutorizacao
		{
			get
			{
				return this._descricaoCodigoAutorizacao;
			}
			set
			{
				this._descricaoCodigoAutorizacao = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoTransacao
		/// <summary>
		/// Tamanho do campo: 14.
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

		#region DescricaoCartaoMascarado
		/// <summary>
		/// Tamanho do campo: 19.
		/// Campo opcional.
		/// </summary>
		public string DescricaoCartaoMascarado
		{
			get
			{
				return this._descricaoCartaoMascarado;
			}
			set
			{
				this._descricaoCartaoMascarado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroSequencialUnico
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public decimal? NumeroSequencialUnico
		{
			get
			{
				return this._numeroSequencialUnico;
			}
			set
			{
				this._numeroSequencialUnico = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoComprovanteAdministradora
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string DescricaoComprovanteAdministradora
		{
			get
			{
				return this._descricaoComprovanteAdministradora;
			}
			set
			{
				this._descricaoComprovanteAdministradora = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoNacionalidadeEmissor
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string DescricaoNacionalidadeEmissor
		{
			get
			{
				return this._descricaoNacionalidadeEmissor;
			}
			set
			{
				this._descricaoNacionalidadeEmissor = value;
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

		#endregion

		#region Construtores
		public TransacaoResposta()
		{
		}
		public TransacaoResposta(int idTransacaoResposta)
		{
			this._idTransacaoResposta = idTransacaoResposta;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Transacao_Resposta (Idf_Transacao, Flg_Transacao_Aprovada, Des_Resultado_Solicitacao_Aprovacao, Des_Codigo_Autorizacao, Des_Transacao, Des_Cartao_Mascarado, Num_Sequencial_Unico, Des_Comprovante_Administradora, Des_Nacionalidade_Emissor, Dta_Cadastro) VALUES (@Idf_Transacao, @Flg_Transacao_Aprovada, @Des_Resultado_Solicitacao_Aprovacao, @Des_Codigo_Autorizacao, @Des_Transacao, @Des_Cartao_Mascarado, @Num_Sequencial_Unico, @Des_Comprovante_Administradora, @Des_Nacionalidade_Emissor, @Dta_Cadastro);SET @Idf_Transacao_Resposta = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Transacao_Resposta SET Idf_Transacao = @Idf_Transacao, Flg_Transacao_Aprovada = @Flg_Transacao_Aprovada, Des_Resultado_Solicitacao_Aprovacao = @Des_Resultado_Solicitacao_Aprovacao, Des_Codigo_Autorizacao = @Des_Codigo_Autorizacao, Des_Transacao = @Des_Transacao, Des_Cartao_Mascarado = @Des_Cartao_Mascarado, Num_Sequencial_Unico = @Num_Sequencial_Unico, Des_Comprovante_Administradora = @Des_Comprovante_Administradora, Des_Nacionalidade_Emissor = @Des_Nacionalidade_Emissor, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Transacao_Resposta = @Idf_Transacao_Resposta";
		private const string SPDELETE = "DELETE FROM BNE_Transacao_Resposta WHERE Idf_Transacao_Resposta = @Idf_Transacao_Resposta";
		private const string SPSELECTID = "SELECT * FROM BNE_Transacao_Resposta WITH(NOLOCK) WHERE Idf_Transacao_Resposta = @Idf_Transacao_Resposta";
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
			parms.Add(new SqlParameter("@Idf_Transacao_Resposta", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Transacao_Aprovada", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Des_Resultado_Solicitacao_Aprovacao", SqlDbType.VarChar, 200));
			parms.Add(new SqlParameter("@Des_Codigo_Autorizacao", SqlDbType.VarChar, 10));
			parms.Add(new SqlParameter("@Des_Transacao", SqlDbType.VarChar, 14));
			parms.Add(new SqlParameter("@Des_Cartao_Mascarado", SqlDbType.VarChar, 19));
			parms.Add(new SqlParameter("@Num_Sequencial_Unico", SqlDbType.Decimal, 5));
			parms.Add(new SqlParameter("@Des_Comprovante_Administradora", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Des_Nacionalidade_Emissor", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
			parms[0].Value = this._idTransacaoResposta;
			parms[1].Value = this._transacao.IdTransacao;
			parms[2].Value = this._flagTransacaoAprovada;
			parms[3].Value = this._descricaoResultadoSolicitacaoAprovacao;

			if (!String.IsNullOrEmpty(this._descricaoCodigoAutorizacao))
				parms[4].Value = this._descricaoCodigoAutorizacao;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoTransacao))
				parms[5].Value = this._descricaoTransacao;
			else
				parms[5].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoCartaoMascarado))
				parms[6].Value = this._descricaoCartaoMascarado;
			else
				parms[6].Value = DBNull.Value;


			if (this._numeroSequencialUnico.HasValue)
				parms[7].Value = this._numeroSequencialUnico;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoComprovanteAdministradora))
				parms[8].Value = this._descricaoComprovanteAdministradora;
			else
				parms[8].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoNacionalidadeEmissor))
				parms[9].Value = this._descricaoNacionalidadeEmissor;
			else
				parms[9].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[10].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de TransacaoResposta no banco de dados.
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
						this._idTransacaoResposta = Convert.ToInt32(cmd.Parameters["@Idf_Transacao_Resposta"].Value);
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
		/// Método utilizado para inserir uma instância de TransacaoResposta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idTransacaoResposta = Convert.ToInt32(cmd.Parameters["@Idf_Transacao_Resposta"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de TransacaoResposta no banco de dados.
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
		/// Método utilizado para atualizar uma instância de TransacaoResposta no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de TransacaoResposta no banco de dados.
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
		/// Método utilizado para salvar uma instância de TransacaoResposta no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de TransacaoResposta no banco de dados.
		/// </summary>
		/// <param name="idTransacaoResposta">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTransacaoResposta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Resposta", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoResposta;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de TransacaoResposta no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTransacaoResposta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idTransacaoResposta, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Resposta", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoResposta;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de TransacaoResposta no banco de dados.
		/// </summary>
		/// <param name="idTransacaoResposta">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idTransacaoResposta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Transacao_Resposta where Idf_Transacao_Resposta in (";

			for (int i = 0; i < idTransacaoResposta.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idTransacaoResposta[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idTransacaoResposta">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTransacaoResposta)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Resposta", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoResposta;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTransacaoResposta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idTransacaoResposta, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Transacao_Resposta", SqlDbType.Int, 4));

			parms[0].Value = idTransacaoResposta;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tra.Idf_Transacao_Resposta, Tra.Idf_Transacao, Tra.Flg_Transacao_Aprovada, Tra.Des_Resultado_Solicitacao_Aprovacao, Tra.Des_Codigo_Autorizacao, Tra.Des_Transacao, Tra.Des_Cartao_Mascarado, Tra.Num_Sequencial_Unico, Tra.Des_Comprovante_Administradora, Tra.Des_Nacionalidade_Emissor, Tra.Dta_Cadastro FROM BNE_Transacao_Resposta Tra";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de TransacaoResposta a partir do banco de dados.
		/// </summary>
		/// <param name="idTransacaoResposta">Chave do registro.</param>
		/// <returns>Instância de TransacaoResposta.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TransacaoResposta LoadObject(int idTransacaoResposta)
		{
			using (IDataReader dr = LoadDataReader(idTransacaoResposta))
			{
				TransacaoResposta objTransacaoResposta = new TransacaoResposta();
				if (SetInstance(dr, objTransacaoResposta))
					return objTransacaoResposta;
			}
			throw (new RecordNotFoundException(typeof(TransacaoResposta)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de TransacaoResposta a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idTransacaoResposta">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de TransacaoResposta.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static TransacaoResposta LoadObject(int idTransacaoResposta, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idTransacaoResposta, trans))
			{
				TransacaoResposta objTransacaoResposta = new TransacaoResposta();
				if (SetInstance(dr, objTransacaoResposta))
					return objTransacaoResposta;
			}
			throw (new RecordNotFoundException(typeof(TransacaoResposta)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de TransacaoResposta a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idTransacaoResposta))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de TransacaoResposta a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idTransacaoResposta, trans))
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
		/// <param name="objTransacaoResposta">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, TransacaoResposta objTransacaoResposta)
		{
			try
			{
				if (dr.Read())
				{
					objTransacaoResposta._idTransacaoResposta = Convert.ToInt32(dr["Idf_Transacao_Resposta"]);
					objTransacaoResposta._transacao = new Transacao(Convert.ToInt32(dr["Idf_Transacao"]));
					objTransacaoResposta._flagTransacaoAprovada = Convert.ToBoolean(dr["Flg_Transacao_Aprovada"]);
					objTransacaoResposta._descricaoResultadoSolicitacaoAprovacao = Convert.ToString(dr["Des_Resultado_Solicitacao_Aprovacao"]);
					if (dr["Des_Codigo_Autorizacao"] != DBNull.Value)
						objTransacaoResposta._descricaoCodigoAutorizacao = Convert.ToString(dr["Des_Codigo_Autorizacao"]);
					if (dr["Des_Transacao"] != DBNull.Value)
						objTransacaoResposta._descricaoTransacao = Convert.ToString(dr["Des_Transacao"]);
					if (dr["Des_Cartao_Mascarado"] != DBNull.Value)
						objTransacaoResposta._descricaoCartaoMascarado = Convert.ToString(dr["Des_Cartao_Mascarado"]);
					if (dr["Num_Sequencial_Unico"] != DBNull.Value)
						objTransacaoResposta._numeroSequencialUnico = Convert.ToDecimal(dr["Num_Sequencial_Unico"]);
					if (dr["Des_Comprovante_Administradora"] != DBNull.Value)
						objTransacaoResposta._descricaoComprovanteAdministradora = Convert.ToString(dr["Des_Comprovante_Administradora"]);
					if (dr["Des_Nacionalidade_Emissor"] != DBNull.Value)
						objTransacaoResposta._descricaoNacionalidadeEmissor = Convert.ToString(dr["Des_Nacionalidade_Emissor"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objTransacaoResposta._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

					objTransacaoResposta._persisted = true;
					objTransacaoResposta._modified = false;

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