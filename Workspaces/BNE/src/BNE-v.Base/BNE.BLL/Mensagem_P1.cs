//-- Data: 09/04/2010 14:10
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Mensagem // Tabela: plataforma.TAB_Mensagem
	{
		#region Atributos
		private int _idMensagem;
		private int _idMensagemRecebida;
		private int _idTipoMensagem;
		private Sistema _sistema;
		private CentroServico _centroServico;
		private string _descricaoMensagem;
		private DateTime? _dataEnvio;
		private string _descricaoEmailDestinatario;
		private string _descricaoEmailAssunto;
		private string _arquivoAnexo;
		private int _idStatusMensagem;
		private DateTime _dataCadastro;
		private string _descricaoEmailRemetente;
		private string _numeroDDDCelular;
		private string _numeroCelular;
		private bool _flagInativo;
		private string _nomeAnexo;
		private string _descricaoObs;
		private int? _idRedeSocial;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdMensagem
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdMensagem
		{
			get
			{
				return this._idMensagem;
			}
		}
		#endregion 

		#region IdMensagemRecebida
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdMensagemRecebida
		{
			get
			{
				return this._idMensagemRecebida;
			}
			set
			{
				this._idMensagemRecebida = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdTipoMensagem
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdTipoMensagem
		{
			get
			{
				return this._idTipoMensagem;
			}
			set
			{
				this._idTipoMensagem = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Sistema
		/// <summary>
		/// Campo obrigatório.
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

		#region CentroServico
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public CentroServico CentroServico
		{
			get
			{
				return this._centroServico;
			}
			set
			{
				this._centroServico = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMensagem
		/// <summary>
		/// Tamanho do campo: 2000.
		/// Campo obrigatório.
		/// </summary>
		public string DescricaoMensagem
		{
			get
			{
				return this._descricaoMensagem;
			}
			set
			{
				this._descricaoMensagem = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DataEnvio
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DataEnvio
		{
			get
			{
				return this._dataEnvio;
			}
			set
			{
				this._dataEnvio = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEmailDestinatario
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoEmailDestinatario
		{
			get
			{
				return this._descricaoEmailDestinatario;
			}
			set
			{
				this._descricaoEmailDestinatario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoEmailAssunto
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoEmailAssunto
		{
			get
			{
				return this._descricaoEmailAssunto;
			}
			set
			{
				this._descricaoEmailAssunto = value;
				this._modified = true;
			}
		}
		#endregion 

		#region ArquivoAnexo
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string ArquivoAnexo
		{
			get
			{
				return this._arquivoAnexo;
			}
			set
			{
				this._arquivoAnexo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdStatusMensagem
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdStatusMensagem
		{
			get
			{
				return this._idStatusMensagem;
			}
			set
			{
				this._idStatusMensagem = value;
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

		#region DescricaoEmailRemetente
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoEmailRemetente
		{
			get
			{
				return this._descricaoEmailRemetente;
			}
			set
			{
				this._descricaoEmailRemetente = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDDDCelular
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo opcional.
		/// </summary>
		public string NumeroDDDCelular
		{
			get
			{
				return this._numeroDDDCelular;
			}
			set
			{
				this._numeroDDDCelular = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroCelular
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string NumeroCelular
		{
			get
			{
				return this._numeroCelular;
			}
			set
			{
				this._numeroCelular = value;
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

		#region NomeAnexo
		/// <summary>
		/// Tamanho do campo: 50.
		/// Campo opcional.
		/// </summary>
		public string NomeAnexo
		{
			get
			{
				return this._nomeAnexo;
			}
			set
			{
				this._nomeAnexo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoObs
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoObs
		{
			get
			{
				return this._descricaoObs;
			}
			set
			{
				this._descricaoObs = value;
				this._modified = true;
			}
		}
		#endregion 

		#region IdRedeSocial
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public int? IdRedeSocial
		{
			get
			{
				return this._idRedeSocial;
			}
			set
			{
				this._idRedeSocial = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public Mensagem()
		{
		}
		public Mensagem(int idMensagem)
		{
			this._idMensagem = idMensagem;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO plataforma.TAB_Mensagem (Idf_Mensagem_Recebida, Idf_Tipo_Mensagem, Idf_Sistema, Idf_Centro_Servico, Des_Mensagem, Dta_Envio, Des_Email_Destinatario, Des_Email_Assunto, Arq_Anexo, Idf_Status_Mensagem, Dta_Cadastro, Des_Email_Remetente, Num_DDD_Celular, Num_Celular, Flg_Inativo, Nme_Anexo, Des_Obs, Idf_Rede_Social) VALUES (@Idf_Mensagem_Recebida, @Idf_Tipo_Mensagem, @Idf_Sistema, @Idf_Centro_Servico, @Des_Mensagem, @Dta_Envio, @Des_Email_Destinatario, @Des_Email_Assunto, @Arq_Anexo, @Idf_Status_Mensagem, @Dta_Cadastro, @Des_Email_Remetente, @Num_DDD_Celular, @Num_Celular, @Flg_Inativo, @Nme_Anexo, @Des_Obs, @Idf_Rede_Social);SET @Idf_Mensagem = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE plataforma.TAB_Mensagem SET Idf_Mensagem_Recebida = @Idf_Mensagem_Recebida, Idf_Tipo_Mensagem = @Idf_Tipo_Mensagem, Idf_Sistema = @Idf_Sistema, Idf_Centro_Servico = @Idf_Centro_Servico, Des_Mensagem = @Des_Mensagem, Dta_Envio = @Dta_Envio, Des_Email_Destinatario = @Des_Email_Destinatario, Des_Email_Assunto = @Des_Email_Assunto, Arq_Anexo = @Arq_Anexo, Idf_Status_Mensagem = @Idf_Status_Mensagem, Dta_Cadastro = @Dta_Cadastro, Des_Email_Remetente = @Des_Email_Remetente, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular, Flg_Inativo = @Flg_Inativo, Nme_Anexo = @Nme_Anexo, Des_Obs = @Des_Obs, Idf_Rede_Social = @Idf_Rede_Social WHERE Idf_Mensagem = @Idf_Mensagem";
		private const string SPDELETE = "DELETE FROM plataforma.TAB_Mensagem WHERE Idf_Mensagem = @Idf_Mensagem";
		private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Mensagem WHERE Idf_Mensagem = @Idf_Mensagem";
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
			parms.Add(new SqlParameter("@Idf_Mensagem", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Mensagem_Recebida", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Mensagem", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Sistema", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Centro_Servico", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar, 2000));
			parms.Add(new SqlParameter("@Dta_Envio", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Email_Destinatario", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Email_Assunto", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Arq_Anexo", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Idf_Status_Mensagem", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Email_Remetente", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Num_DDD_Celular", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Celular", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Nme_Anexo", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Des_Obs", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Rede_Social", SqlDbType.Int, 4));
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
			parms[0].Value = this._idMensagem;
			parms[1].Value = this._idMensagemRecebida;
			parms[2].Value = this._idTipoMensagem;
			parms[3].Value = this._sistema.IdSistema;
			parms[4].Value = this._centroServico.IdCentroServico;
			parms[5].Value = this._descricaoMensagem;

			if (this._dataEnvio.HasValue)
				parms[6].Value = this._dataEnvio;
			else
				parms[6].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoEmailDestinatario))
				parms[7].Value = this._descricaoEmailDestinatario;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoEmailAssunto))
				parms[8].Value = this._descricaoEmailAssunto;
			else
				parms[8].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._arquivoAnexo))
				parms[9].Value = this._arquivoAnexo;
			else
				parms[9].Value = DBNull.Value;

			parms[10].Value = this._idStatusMensagem;

			if (!String.IsNullOrEmpty(this._descricaoEmailRemetente))
				parms[12].Value = this._descricaoEmailRemetente;
			else
				parms[12].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroDDDCelular))
				parms[13].Value = this._numeroDDDCelular;
			else
				parms[13].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCelular))
				parms[14].Value = this._numeroCelular;
			else
				parms[14].Value = DBNull.Value;

			parms[15].Value = this._flagInativo;

			if (!String.IsNullOrEmpty(this._nomeAnexo))
				parms[16].Value = this._nomeAnexo;
			else
				parms[16].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoObs))
				parms[17].Value = this._descricaoObs;
			else
				parms[17].Value = DBNull.Value;


			if (this._idRedeSocial.HasValue)
				parms[18].Value = this._idRedeSocial;
			else
				parms[18].Value = DBNull.Value;


			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[11].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Mensagem no banco de dados.
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
						this._idMensagem = Convert.ToInt32(cmd.Parameters["@Idf_Mensagem"].Value);
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
		/// Método utilizado para inserir uma instância de Mensagem no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idMensagem = Convert.ToInt32(cmd.Parameters["@Idf_Mensagem"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Mensagem no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Mensagem no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Mensagem no banco de dados.
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
		/// Método utilizado para salvar uma instância de Mensagem no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Mensagem no banco de dados.
		/// </summary>
		/// <param name="idMensagem">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMensagem)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem", SqlDbType.Int, 4));

			parms[0].Value = idMensagem;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Mensagem no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagem">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idMensagem, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem", SqlDbType.Int, 4));

			parms[0].Value = idMensagem;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Mensagem no banco de dados.
		/// </summary>
		/// <param name="idMensagem">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idMensagem)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from plataforma.TAB_Mensagem where Idf_Mensagem in (";

			for (int i = 0; i < idMensagem.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idMensagem[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idMensagem">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMensagem)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem", SqlDbType.Int, 4));

			parms[0].Value = idMensagem;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagem">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idMensagem, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem", SqlDbType.Int, 4));

			parms[0].Value = idMensagem;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Men.Idf_Mensagem, Men.Idf_Mensagem_Recebida, Men.Idf_Tipo_Mensagem, Men.Idf_Sistema, Men.Idf_Centro_Servico, Men.Des_Mensagem, Men.Dta_Envio, Men.Des_Email_Destinatario, Men.Des_Email_Assunto, Men.Arq_Anexo, Men.Idf_Status_Mensagem, Men.Dta_Cadastro, Men.Des_Email_Remetente, Men.Num_DDD_Celular, Men.Num_Celular, Men.Flg_Inativo, Men.Nme_Anexo, Men.Des_Obs, Men.Idf_Rede_Social FROM plataforma.TAB_Mensagem Men";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Mensagem a partir do banco de dados.
		/// </summary>
		/// <param name="idMensagem">Chave do registro.</param>
		/// <returns>Instância de Mensagem.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Mensagem LoadObject(int idMensagem)
		{
			using (IDataReader dr = LoadDataReader(idMensagem))
			{
				Mensagem objMensagem = new Mensagem();
				if (SetInstance(dr, objMensagem))
					return objMensagem;
			}
			throw (new RecordNotFoundException(typeof(Mensagem)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Mensagem a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagem">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Mensagem.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Mensagem LoadObject(int idMensagem, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMensagem, trans))
			{
				Mensagem objMensagem = new Mensagem();
				if (SetInstance(dr, objMensagem))
					return objMensagem;
			}
			throw (new RecordNotFoundException(typeof(Mensagem)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Mensagem a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idMensagem))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Mensagem a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idMensagem, trans))
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
		/// <param name="objMensagem">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Mensagem objMensagem)
		{
			try
			{
				if (dr.Read())
				{
					objMensagem._idMensagem = Convert.ToInt32(dr["Idf_Mensagem"]);
					objMensagem._idMensagemRecebida = Convert.ToInt32(dr["Idf_Mensagem_Recebida"]);
					objMensagem._idTipoMensagem = Convert.ToInt32(dr["Idf_Tipo_Mensagem"]);
					objMensagem._sistema = new Sistema(Convert.ToInt32(dr["Idf_Sistema"]));
					objMensagem._centroServico = new CentroServico(Convert.ToInt32(dr["Idf_Centro_Servico"]));
					objMensagem._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);
					if (dr["Dta_Envio"] != DBNull.Value)
						objMensagem._dataEnvio = Convert.ToDateTime(dr["Dta_Envio"]);
					if (dr["Des_Email_Destinatario"] != DBNull.Value)
						objMensagem._descricaoEmailDestinatario = Convert.ToString(dr["Des_Email_Destinatario"]);
					if (dr["Des_Email_Assunto"] != DBNull.Value)
						objMensagem._descricaoEmailAssunto = Convert.ToString(dr["Des_Email_Assunto"]);
					if (dr["Arq_Anexo"] != DBNull.Value)
						objMensagem._arquivoAnexo = Convert.ToString(dr["Arq_Anexo"]);
					objMensagem._idStatusMensagem = Convert.ToInt32(dr["Idf_Status_Mensagem"]);
					objMensagem._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					if (dr["Des_Email_Remetente"] != DBNull.Value)
						objMensagem._descricaoEmailRemetente = Convert.ToString(dr["Des_Email_Remetente"]);
					if (dr["Num_DDD_Celular"] != DBNull.Value)
						objMensagem._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
					if (dr["Num_Celular"] != DBNull.Value)
						objMensagem._numeroCelular = Convert.ToString(dr["Num_Celular"]);
					objMensagem._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					if (dr["Nme_Anexo"] != DBNull.Value)
						objMensagem._nomeAnexo = Convert.ToString(dr["Nme_Anexo"]);
					if (dr["Des_Obs"] != DBNull.Value)
						objMensagem._descricaoObs = Convert.ToString(dr["Des_Obs"]);
					if (dr["Idf_Rede_Social"] != DBNull.Value)
						objMensagem._idRedeSocial = Convert.ToInt32(dr["Idf_Rede_Social"]);

					objMensagem._persisted = true;
					objMensagem._modified = false;

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