//-- Data: 14/02/2011 08:58
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class MensagemCS // Tabela: BNE_Mensagem_CS
	{
		#region Atributos
		private int _idMensagemCS;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private Curriculo _curriculo;
		private string _descricaoMensagem;
		private DateTime? _dataEnvio;
		private RedeSocialCS _redeSocialCS;
		private TipoMensagemCS _tipoMensagemCS;
		private string _descricaoEmailDestinatario;
		private string _descricaoEmailAssunto;
		private StatusMensagemCS _statusMensagemCS;
		private DateTime _dataCadastro;
		private string _descricaoEmailRemetente;
		private string _numeroDDDCelular;
		private string _numeroCelular;
		private bool _flagInativo;
		private string _nomeAnexo;
		private string _descricaoObs;
		private int _idSistema;
		private int _idCentroServico;
		private byte[] _arquivoAnexo;
		private MensagemSistema _mensagemSistema;
		private UsuarioFilialPerfil _usuarioFilialDes;
		private bool _flagLido;

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

		#region UsuarioFilialPerfil
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public UsuarioFilialPerfil UsuarioFilialPerfil
		{
			get
			{
				return this._usuarioFilialPerfil;
			}
			set
			{
				this._usuarioFilialPerfil = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Curriculo
		/// <summary>
		/// Campo opcional.
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

		#region RedeSocialCS
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public RedeSocialCS RedeSocialCS
		{
			get
			{
				return this._redeSocialCS;
			}
			set
			{
				this._redeSocialCS = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoMensagemCS
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public TipoMensagemCS TipoMensagemCS
		{
			get
			{
				return this._tipoMensagemCS;
			}
			set
			{
				this._tipoMensagemCS = value;
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

		#region StatusMensagemCS
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public StatusMensagemCS StatusMensagemCS
		{
			get
			{
				return this._statusMensagemCS;
			}
			set
			{
				this._statusMensagemCS = value;
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

		#region MensagemSistema
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public MensagemSistema MensagemSistema
		{
			get
			{
				return this._mensagemSistema;
			}
			set
			{
				this._mensagemSistema = value;
				this._modified = true;
			}
		}
		#endregion 

		#region UsuarioFilialDes
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public UsuarioFilialPerfil UsuarioFilialDes
		{
			get
			{
				return this._usuarioFilialDes;
			}
			set
			{
				this._usuarioFilialDes = value;
				this._modified = true;
			}
		}
		#endregion 

		#region FlagLido
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagLido
		{
			get
			{
				return this._flagLido;
			}
			set
			{
				this._flagLido = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public MensagemCS()
		{
		}
		public MensagemCS(int idMensagemCS)
		{
			this._idMensagemCS = idMensagemCS;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Mensagem_CS (Idf_Usuario_Filial_Perfil, Idf_Curriculo, Des_Mensagem, Dta_Envio, Idf_Rede_Social_CS, Idf_Tipo_Mensagem_CS, Des_Email_Destinatario, Des_Email_Assunto, Idf_Status_Mensagem_CS, Dta_Cadastro, Des_Email_Remetente, Num_DDD_Celular, Num_Celular, Flg_Inativo, Nme_Anexo, Des_Obs, Idf_Sistema, Idf_Centro_Servico, Arq_Anexo, Idf_Mensagem_Sistema, Idf_Usuario_Filial_Des, Flg_Lido) VALUES (@Idf_Usuario_Filial_Perfil, @Idf_Curriculo, @Des_Mensagem, @Dta_Envio, @Idf_Rede_Social_CS, @Idf_Tipo_Mensagem_CS, @Des_Email_Destinatario, @Des_Email_Assunto, @Idf_Status_Mensagem_CS, @Dta_Cadastro, @Des_Email_Remetente, @Num_DDD_Celular, @Num_Celular, @Flg_Inativo, @Nme_Anexo, @Des_Obs, @Idf_Sistema, @Idf_Centro_Servico, @Arq_Anexo, @Idf_Mensagem_Sistema, @Idf_Usuario_Filial_Des, @Flg_Lido);SET @Idf_Mensagem_CS = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Mensagem_CS SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Curriculo = @Idf_Curriculo, Des_Mensagem = @Des_Mensagem, Dta_Envio = @Dta_Envio, Idf_Rede_Social_CS = @Idf_Rede_Social_CS, Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS, Des_Email_Destinatario = @Des_Email_Destinatario, Des_Email_Assunto = @Des_Email_Assunto, Idf_Status_Mensagem_CS = @Idf_Status_Mensagem_CS, Dta_Cadastro = @Dta_Cadastro, Des_Email_Remetente = @Des_Email_Remetente, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular, Flg_Inativo = @Flg_Inativo, Nme_Anexo = @Nme_Anexo, Des_Obs = @Des_Obs, Idf_Sistema = @Idf_Sistema, Idf_Centro_Servico = @Idf_Centro_Servico, Arq_Anexo = @Arq_Anexo, Idf_Mensagem_Sistema = @Idf_Mensagem_Sistema, Idf_Usuario_Filial_Des = @Idf_Usuario_Filial_Des, Flg_Lido = @Flg_Lido WHERE Idf_Mensagem_CS = @Idf_Mensagem_CS";
		private const string SPDELETE = "DELETE FROM BNE_Mensagem_CS WHERE Idf_Mensagem_CS = @Idf_Mensagem_CS";
		private const string SPSELECTID = "SELECT * FROM BNE_Mensagem_CS WHERE Idf_Mensagem_CS = @Idf_Mensagem_CS";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Dta_Envio", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Idf_Rede_Social_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Email_Destinatario", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Des_Email_Assunto", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Status_Mensagem_CS", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Des_Email_Remetente", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Num_DDD_Celular", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Celular", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Nme_Anexo", SqlDbType.VarChar, 50));
			parms.Add(new SqlParameter("@Des_Obs", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Sistema", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Centro_Servico", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Arq_Anexo", SqlDbType.VarBinary));
			parms.Add(new SqlParameter("@Idf_Mensagem_Sistema", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Des", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Lido", SqlDbType.Bit, 1));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Vinicius Maciel</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idMensagemCS;

			if (this._usuarioFilialPerfil != null)
				parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
			else
				parms[1].Value = DBNull.Value;


			if (this._curriculo != null)
				parms[2].Value = this._curriculo.IdCurriculo;
			else
				parms[2].Value = DBNull.Value;

			parms[3].Value = this._descricaoMensagem;

			if (this._dataEnvio.HasValue)
				parms[4].Value = this._dataEnvio;
			else
				parms[4].Value = DBNull.Value;


			if (this._redeSocialCS != null)
				parms[5].Value = this._redeSocialCS.IdRedeSocialCS;
			else
				parms[5].Value = DBNull.Value;

			parms[6].Value = this._tipoMensagemCS.IdTipoMensagemCS;

			if (!String.IsNullOrEmpty(this._descricaoEmailDestinatario))
				parms[7].Value = this._descricaoEmailDestinatario;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoEmailAssunto))
				parms[8].Value = this._descricaoEmailAssunto;
			else
				parms[8].Value = DBNull.Value;

			parms[9].Value = this._statusMensagemCS.IdStatusMensagemCS;
			parms[11].Value = this._descricaoEmailRemetente;

			if (!String.IsNullOrEmpty(this._numeroDDDCelular))
				parms[12].Value = this._numeroDDDCelular;
			else
				parms[12].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCelular))
				parms[13].Value = this._numeroCelular;
			else
				parms[13].Value = DBNull.Value;

			parms[14].Value = this._flagInativo;

			if (!String.IsNullOrEmpty(this._nomeAnexo))
				parms[15].Value = this._nomeAnexo;
			else
				parms[15].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoObs))
				parms[16].Value = this._descricaoObs;
			else
				parms[16].Value = DBNull.Value;

			parms[17].Value = this._idSistema;
			parms[18].Value = this._idCentroServico;

			if (this._arquivoAnexo != null)
				parms[19].Value = this._arquivoAnexo;
			else
				parms[19].Value = DBNull.Value;


			if (this._mensagemSistema != null)
				parms[20].Value = this._mensagemSistema.IdMensagemSistema;
			else
				parms[20].Value = DBNull.Value;


			if (this._usuarioFilialDes != null)
				parms[21].Value = this._usuarioFilialDes.IdUsuarioFilialPerfil;
			else
				parms[21].Value = DBNull.Value;

			parms[22].Value = this._flagLido;

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
		/// Método utilizado para inserir uma instância de MensagemCS no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para inserir uma instância de MensagemCS no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para atualizar uma instância de MensagemCS no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para atualizar uma instância de MensagemCS no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para salvar uma instância de MensagemCS no banco de dados.
		/// </summary>
		/// <remarks>Vinicius Maciel</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de MensagemCS no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
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
		/// Método utilizado para excluir uma instância de MensagemCS no banco de dados.
		/// </summary>
		/// <param name="idMensagemCS">Chave do registro.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idMensagemCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idMensagemCS;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de MensagemCS no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagemCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(int idMensagemCS, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idMensagemCS;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de MensagemCS no banco de dados.
		/// </summary>
		/// <param name="idMensagemCS">Lista de chaves.</param>
		/// <remarks>Vinicius Maciel</remarks>
		public static void Delete(List<int> idMensagemCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Mensagem_CS where Idf_Mensagem_CS in (";

			for (int i = 0; i < idMensagemCS.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idMensagemCS[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idMensagemCS">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idMensagemCS)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idMensagemCS;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagemCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static IDataReader LoadDataReader(int idMensagemCS, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4));

			parms[0].Value = idMensagemCS;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Men.Idf_Mensagem_CS, Men.Idf_Usuario_Filial_Perfil, Men.Idf_Curriculo, Men.Des_Mensagem, Men.Dta_Envio, Men.Idf_Rede_Social_CS, Men.Idf_Tipo_Mensagem_CS, Men.Des_Email_Destinatario, Men.Des_Email_Assunto, Men.Idf_Status_Mensagem_CS, Men.Dta_Cadastro, Men.Des_Email_Remetente, Men.Num_DDD_Celular, Men.Num_Celular, Men.Flg_Inativo, Men.Nme_Anexo, Men.Des_Obs, Men.Idf_Sistema, Men.Idf_Centro_Servico, Men.Arq_Anexo, Men.Idf_Mensagem_Sistema, Men.Idf_Usuario_Filial_Des, Men.Flg_Lido FROM BNE_Mensagem_CS Men";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de MensagemCS a partir do banco de dados.
		/// </summary>
		/// <param name="idMensagemCS">Chave do registro.</param>
		/// <returns>Instância de MensagemCS.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static MensagemCS LoadObject(int idMensagemCS)
		{
			using (IDataReader dr = LoadDataReader(idMensagemCS))
			{
				MensagemCS objMensagemCS = new MensagemCS();
				if (SetInstance(dr, objMensagemCS))
					return objMensagemCS;
			}
			throw (new RecordNotFoundException(typeof(MensagemCS)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de MensagemCS a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idMensagemCS">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de MensagemCS.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public static MensagemCS LoadObject(int idMensagemCS, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idMensagemCS, trans))
			{
				MensagemCS objMensagemCS = new MensagemCS();
				if (SetInstance(dr, objMensagemCS))
					return objMensagemCS;
			}
			throw (new RecordNotFoundException(typeof(MensagemCS)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de MensagemCS a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idMensagemCS))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de MensagemCS a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idMensagemCS, trans))
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
		/// <param name="objMensagemCS">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Vinicius Maciel</remarks>
		private static bool SetInstance(IDataReader dr, MensagemCS objMensagemCS)
		{
			try
			{
				if (dr.Read())
				{
					objMensagemCS._idMensagemCS = Convert.ToInt32(dr["Idf_Mensagem_CS"]);
					if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
						objMensagemCS._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					if (dr["Idf_Curriculo"] != DBNull.Value)
						objMensagemCS._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
					objMensagemCS._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);
					if (dr["Dta_Envio"] != DBNull.Value)
						objMensagemCS._dataEnvio = Convert.ToDateTime(dr["Dta_Envio"]);
					if (dr["Idf_Rede_Social_CS"] != DBNull.Value)
						objMensagemCS._redeSocialCS = new RedeSocialCS(Convert.ToInt32(dr["Idf_Rede_Social_CS"]));
					objMensagemCS._tipoMensagemCS = new TipoMensagemCS(Convert.ToInt32(dr["Idf_Tipo_Mensagem_CS"]));
					if (dr["Des_Email_Destinatario"] != DBNull.Value)
						objMensagemCS._descricaoEmailDestinatario = Convert.ToString(dr["Des_Email_Destinatario"]);
					if (dr["Des_Email_Assunto"] != DBNull.Value)
						objMensagemCS._descricaoEmailAssunto = Convert.ToString(dr["Des_Email_Assunto"]);
					objMensagemCS._statusMensagemCS = new StatusMensagemCS(Convert.ToInt32(dr["Idf_Status_Mensagem_CS"]));
					objMensagemCS._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objMensagemCS._descricaoEmailRemetente = Convert.ToString(dr["Des_Email_Remetente"]);
					if (dr["Num_DDD_Celular"] != DBNull.Value)
						objMensagemCS._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
					if (dr["Num_Celular"] != DBNull.Value)
						objMensagemCS._numeroCelular = Convert.ToString(dr["Num_Celular"]);
					objMensagemCS._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					if (dr["Nme_Anexo"] != DBNull.Value)
						objMensagemCS._nomeAnexo = Convert.ToString(dr["Nme_Anexo"]);
					if (dr["Des_Obs"] != DBNull.Value)
						objMensagemCS._descricaoObs = Convert.ToString(dr["Des_Obs"]);
					objMensagemCS._idSistema = Convert.ToInt32(dr["Idf_Sistema"]);
					objMensagemCS._idCentroServico = Convert.ToInt32(dr["Idf_Centro_Servico"]);
					if (dr["Arq_Anexo"] != DBNull.Value)
						objMensagemCS._arquivoAnexo = ((byte[])(dr["Arq_Anexo"]));
					if (dr["Idf_Mensagem_Sistema"] != DBNull.Value)
						objMensagemCS._mensagemSistema = new MensagemSistema(Convert.ToInt32(dr["Idf_Mensagem_Sistema"]));
					if (dr["Idf_Usuario_Filial_Des"] != DBNull.Value)
						objMensagemCS._usuarioFilialDes = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Des"]));
					objMensagemCS._flagLido = Convert.ToBoolean(dr["Flg_Lido"]);

					objMensagemCS._persisted = true;
					objMensagemCS._modified = false;

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