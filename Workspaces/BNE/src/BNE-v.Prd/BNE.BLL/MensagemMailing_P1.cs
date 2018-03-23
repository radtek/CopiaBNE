//-- Data: 04/08/2016 17:08
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class MensagemMailing // Tabela: TAB_Mensagem_Mailing
	{
		#region Atributos
		private int _idMensagemCS;
		private DateTime? _dataEnvio;
		private int _idTipoMensagem;
		private int _idStatusMensagem;
		private string _descricaoMensagem;
		private string _descricaoEmailRemetente;
		private string _descricaoEmailDestino;
		private string _descricaoAssunto;
		private string _nomeAnexo;
		private byte[] _arquivoAnexo;
		private string _numeroDDDCelular;
		private string _numeroCelular;
		private int _idSistema;
		private DateTime _dataCadastro;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdMensagemCS
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdMensagemCS
		{
			get
			{
				return this._idMensagemCS;
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

		#region DescricaoMensagem
		/// <summary>
		/// Tamanho do campo: -1.
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

		#region DescricaoEmailRemetente
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
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

		#region DescricaoEmailDestino
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoEmailDestino
		{
			get
			{
				return this._descricaoEmailDestino;
			}
			set
			{
				this._descricaoEmailDestino = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoAssunto
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string DescricaoAssunto
		{
			get
			{
				return this._descricaoAssunto;
			}
			set
			{
				this._descricaoAssunto = value;
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

		#region ArquivoAnexo
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public byte[] ArquivoAnexo
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

		#region IdSistema
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdSistema
		{
			get
			{
				return this._idSistema;
			}
			set
			{
				this._idSistema = value;
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

		#endregion
		
		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Mensagem_Mailing (Dta_Envio, Idf_Tipo_Mensagem, Idf_Status_Mensagem, Des_Mensagem, Des_Email_Remetente, Des_Email_Destino, Des_Assunto, Nme_Anexo, Arq_Anexo, Num_DDD_Celular, Num_Celular, Idf_Sistema, Dta_Cadastro, Flg_Inativo) VALUES (@Dta_Envio, @Idf_Tipo_Mensagem, @Idf_Status_Mensagem, @Des_Mensagem, @Des_Email_Remetente, @Des_Email_Destino, @Des_Assunto, @Nme_Anexo, @Arq_Anexo, @Num_DDD_Celular, @Num_Celular, @Idf_Sistema, @Dta_Cadastro, @Flg_Inativo);SET @Idf_Mensagem_CS = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Mensagem_Mailing SET Idf_Mensagem_CS = @Idf_Mensagem_CS, Dta_Envio = @Dta_Envio, Idf_Tipo_Mensagem = @Idf_Tipo_Mensagem, Idf_Status_Mensagem = @Idf_Status_Mensagem, Des_Mensagem = @Des_Mensagem, Des_Email_Remetente = @Des_Email_Remetente, Des_Email_Destino = @Des_Email_Destino, Des_Assunto = @Des_Assunto, Nme_Anexo = @Nme_Anexo, Arq_Anexo = @Arq_Anexo, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular, Idf_Sistema = @Idf_Sistema, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo";
		private const string SPDELETE = "DELETE FROM TAB_Mensagem_Mailing";
		private const string SPSELECTID = "SELECT * FROM TAB_Mensagem_Mailing WITH(NOLOCK) ";
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
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Envio", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Tipo_Mensagem", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Status_Mensagem", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Des_Email_Remetente", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Email_Destino", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Assunto", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Nme_Anexo", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Arq_Anexo", SqlDbType.VarBinary));
			parms.Add(new SqlParameter("@Num_DDD_Celular", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Celular", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Idf_Sistema", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
			parms[0].Value = this._idMensagemCS;

			if (this._dataEnvio.HasValue)
				parms[1].Value = this._dataEnvio;
			else
				parms[1].Value = DBNull.Value;

			parms[2].Value = this._idTipoMensagem;
			parms[3].Value = this._idStatusMensagem;
			parms[4].Value = this._descricaoMensagem;
			parms[5].Value = this._descricaoEmailRemetente;

			if (!String.IsNullOrEmpty(this._descricaoEmailDestino))
				parms[6].Value = this._descricaoEmailDestino;
			else
				parms[6].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoAssunto))
				parms[7].Value = this._descricaoAssunto;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._nomeAnexo))
				parms[8].Value = this._nomeAnexo;
			else
				parms[8].Value = DBNull.Value;


			if (this._arquivoAnexo != null)
				parms[9].Value = this._arquivoAnexo;
			else
				parms[9].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroDDDCelular))
				parms[10].Value = this._numeroDDDCelular;
			else
				parms[10].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCelular))
				parms[11].Value = this._numeroCelular;
			else
				parms[11].Value = DBNull.Value;

			parms[12].Value = this._idSistema;
			parms[14].Value = this._flagInativo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[13].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de MensagemMailing no banco de dados.
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
						this._idMensagemCS = Convert.ToInt32(cmd.Parameters["@Idf_Mensagem_CS"].Value);
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
		/// Método utilizado para inserir uma instância de MensagemMailing no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idMensagemCS = Convert.ToInt32(cmd.Parameters["@Idf_Mensagem_CS"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de MensagemMailing no banco de dados.
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
		/// Método utilizado para atualizar uma instância de MensagemMailing no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de MensagemMailing no banco de dados.
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
		/// Método utilizado para salvar uma instância de MensagemMailing no banco de dados, dentro de uma transação.
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

		#region SetInstance
		/// <summary>
		/// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
		/// </summary>
		/// <param name="dr">Cursor de leitura do banco de dados.</param>
		/// <param name="objMensagemMailing">Instância a ser manipulada.</param>
		/// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, MensagemMailing objMensagemMailing, bool dispose = true)
		{
			try
			{
				if (dr.Read())
				{
					objMensagemMailing._idMensagemCS = Convert.ToInt32(dr["Idf_Mensagem_CS"]);
					if (dr["Dta_Envio"] != DBNull.Value)
						objMensagemMailing._dataEnvio = Convert.ToDateTime(dr["Dta_Envio"]);
					objMensagemMailing._idTipoMensagem = Convert.ToInt32(dr["Idf_Tipo_Mensagem"]);
					objMensagemMailing._idStatusMensagem = Convert.ToInt32(dr["Idf_Status_Mensagem"]);
					objMensagemMailing._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);
					objMensagemMailing._descricaoEmailRemetente = Convert.ToString(dr["Des_Email_Remetente"]);
					if (dr["Des_Email_Destino"] != DBNull.Value)
						objMensagemMailing._descricaoEmailDestino = Convert.ToString(dr["Des_Email_Destino"]);
					if (dr["Des_Assunto"] != DBNull.Value)
						objMensagemMailing._descricaoAssunto = Convert.ToString(dr["Des_Assunto"]);
					if (dr["Nme_Anexo"] != DBNull.Value)
						objMensagemMailing._nomeAnexo = Convert.ToString(dr["Nme_Anexo"]);
					if (dr["Arq_Anexo"] != DBNull.Value)
						objMensagemMailing._arquivoAnexo = ((byte[])(dr["Arq_Anexo"]));
					if (dr["Num_DDD_Celular"] != DBNull.Value)
						objMensagemMailing._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
					if (dr["Num_Celular"] != DBNull.Value)
						objMensagemMailing._numeroCelular = Convert.ToString(dr["Num_Celular"]);
					objMensagemMailing._idSistema = Convert.ToInt32(dr["Idf_Sistema"]);
					objMensagemMailing._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objMensagemMailing._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

					objMensagemMailing._persisted = true;
					objMensagemMailing._modified = false;

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
				if (dispose)
					dr.Dispose();
			}
		}
		#endregion

		#endregion
	}
}