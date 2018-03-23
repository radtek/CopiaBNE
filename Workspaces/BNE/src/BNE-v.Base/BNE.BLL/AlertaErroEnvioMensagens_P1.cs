//-- Data: 06/09/2013 16:34
//-- Autor: Luan Fernandes

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class AlertaErroEnvioMensagens // Tabela: alerta.Tab_Alerta_ErroEnvioMensagens
	{
		#region Atributos
		private int _idErroEnvioMensagens;
		private int _idCurriculo;
		private string _nomeDestinatario;
		private string _emailDestinatario;
		private string _descricaoMensagem;
		private string _descricaoErro;
		private DateTime _DatCadastro;
		private DateTime? _DatPeriodoIni;
		private DateTime? _DatPeriodoFim;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdErroEnvioMensagens
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdErroEnvioMensagens
		{
			get
			{
				return this._idErroEnvioMensagens;
			}
		}
		#endregion 

		#region IdCurriculo
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public int IdCurriculo
		{
			get
			{
				return this._idCurriculo;
			}
			set
			{
				this._idCurriculo = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeDestinatario
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo opcional.
		/// </summary>
		public string NomeDestinatario
		{
			get
			{
				return this._nomeDestinatario;
			}
			set
			{
				this._nomeDestinatario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailDestinatario
		/// <summary>
		/// Tamanho do campo: 500.
		/// Campo opcional.
		/// </summary>
		public string EmailDestinatario
		{
			get
			{
				return this._emailDestinatario;
			}
			set
			{
				this._emailDestinatario = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMensagem
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
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

		#region DescricaoErro
		/// <summary>
		/// Tamanho do campo: -1.
		/// Campo opcional.
		/// </summary>
		public string DescricaoErro
		{
			get
			{
				return this._descricaoErro;
			}
			set
			{
				this._descricaoErro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DatCadastro
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DatCadastro
		{
			get
			{
				return this._DatCadastro;
			}
			set
			{
				this._DatCadastro = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DatPeriodoIni
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DatPeriodoIni
		{
			get
			{
				return this._DatPeriodoIni;
			}
			set
			{
				this._DatPeriodoIni = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DatPeriodoFim
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public DateTime? DatPeriodoFim
		{
			get
			{
				return this._DatPeriodoFim;
			}
			set
			{
				this._DatPeriodoFim = value;
				this._modified = true;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public AlertaErroEnvioMensagens()
		{
		}
		public AlertaErroEnvioMensagens(int idErroEnvioMensagens, int idCurriculo)
		{
			this._idErroEnvioMensagens = idErroEnvioMensagens;
			this._idCurriculo = idCurriculo;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO alerta.Tab_Alerta_ErroEnvioMensagens (Idf_Curriculo, Nme_Destinatario, Eml_Destinatario, Des_Mensagem, Des_Erro, Dat_Cadastro, Dat_Periodo_Ini, Dat_Periodo_Fim) VALUES (@Idf_Curriculo, @Nme_Destinatario, @Eml_Destinatario, @Des_Mensagem, @Des_Erro, @Dat_Cadastro, @Dat_Periodo_Ini, @Dat_Periodo_Fim);SET @Idf_ErroEnvioMensagens = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE alerta.Tab_Alerta_ErroEnvioMensagens SET Nme_Destinatario = @Nme_Destinatario, Eml_Destinatario = @Eml_Destinatario, Des_Mensagem = @Des_Mensagem, Des_Erro = @Des_Erro, Dat_Cadastro = @Dat_Cadastro, Dat_Periodo_Ini = @Dat_Periodo_Ini, Dat_Periodo_Fim = @Dat_Periodo_Fim WHERE Idf_ErroEnvioMensagens = @Idf_ErroEnvioMensagens AND Idf_Curriculo = @Idf_Curriculo";
		private const string SPDELETE = "DELETE FROM alerta.Tab_Alerta_ErroEnvioMensagens WHERE Idf_ErroEnvioMensagens = @Idf_ErroEnvioMensagens AND Idf_Curriculo = @Idf_Curriculo";
		private const string SPSELECTID = "SELECT * FROM alerta.Tab_Alerta_ErroEnvioMensagens WITH(NOLOCK) WHERE Idf_ErroEnvioMensagens = @Idf_ErroEnvioMensagens AND Idf_Curriculo = @Idf_Curriculo";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_ErroEnvioMensagens", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Destinatario", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Eml_Destinatario", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Des_Erro", SqlDbType.VarChar));
			parms.Add(new SqlParameter("@Dat_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dat_Periodo_Ini", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dat_Periodo_Fim", SqlDbType.DateTime, 8));
			return(parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Luan Fernandes</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idErroEnvioMensagens;
			parms[1].Value = this._idCurriculo;

			if (!String.IsNullOrEmpty(this._nomeDestinatario))
				parms[2].Value = this._nomeDestinatario;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._emailDestinatario))
				parms[3].Value = this._emailDestinatario;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoMensagem))
				parms[4].Value = this._descricaoMensagem;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._descricaoErro))
				parms[5].Value = this._descricaoErro;
			else
				parms[5].Value = DBNull.Value;

			parms[6].Value = this._DatCadastro;

			if (this._DatPeriodoIni.HasValue)
				parms[7].Value = this._DatPeriodoIni;
			else
				parms[7].Value = DBNull.Value;


			if (this._DatPeriodoFim.HasValue)
				parms[8].Value = this._DatPeriodoFim;
			else
				parms[8].Value = DBNull.Value;


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
		/// Método utilizado para inserir uma instância de AlertaErroEnvioMensagens no banco de dados.
		/// </summary>
		/// <remarks>Luan Fernandes</remarks>
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
						this._idErroEnvioMensagens = Convert.ToInt32(cmd.Parameters["@Idf_ErroEnvioMensagens"].Value);
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
		/// Método utilizado para inserir uma instância de AlertaErroEnvioMensagens no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idErroEnvioMensagens = Convert.ToInt32(cmd.Parameters["@Idf_ErroEnvioMensagens"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de AlertaErroEnvioMensagens no banco de dados.
		/// </summary>
		/// <remarks>Luan Fernandes</remarks>
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
		/// Método utilizado para atualizar uma instância de AlertaErroEnvioMensagens no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
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
		/// Método utilizado para salvar uma instância de AlertaErroEnvioMensagens no banco de dados.
		/// </summary>
		/// <remarks>Luan Fernandes</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de AlertaErroEnvioMensagens no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
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
		/// Método utilizado para excluir uma instância de AlertaErroEnvioMensagens no banco de dados.
		/// </summary>
		/// <param name="idErroEnvioMensagens">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idErroEnvioMensagens, int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_ErroEnvioMensagens", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idErroEnvioMensagens;
			parms[1].Value = idCurriculo;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de AlertaErroEnvioMensagens no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idErroEnvioMensagens">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idErroEnvioMensagens, int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_ErroEnvioMensagens", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idErroEnvioMensagens;
			parms[1].Value = idCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idErroEnvioMensagens">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idErroEnvioMensagens, int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_ErroEnvioMensagens", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idErroEnvioMensagens;
			parms[1].Value = idCurriculo;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idErroEnvioMensagens">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idErroEnvioMensagens, int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_ErroEnvioMensagens", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idErroEnvioMensagens;
			parms[1].Value = idCurriculo;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, AEM.Idf_ErroEnvioMensagens, AEM.Idf_Curriculo, AEM.Nme_Destinatario, AEM.Eml_Destinatario, AEM.Des_Mensagem, AEM.Des_Erro, AEM.Dat_Cadastro, AEM.Dat_Periodo_Ini, AEM.Dat_Periodo_Fim FROM alerta.Tab_Alerta_ErroEnvioMensagens AEM";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaErroEnvioMensagens a partir do banco de dados.
		/// </summary>
		/// <param name="idErroEnvioMensagens">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Instância de AlertaErroEnvioMensagens.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaErroEnvioMensagens LoadObject(int idErroEnvioMensagens, int idCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idErroEnvioMensagens, idCurriculo))
			{
				AlertaErroEnvioMensagens objAlertaErroEnvioMensagens = new AlertaErroEnvioMensagens();
				if (SetInstance(dr, objAlertaErroEnvioMensagens))
					return objAlertaErroEnvioMensagens;
			}
			throw (new RecordNotFoundException(typeof(AlertaErroEnvioMensagens)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaErroEnvioMensagens a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idErroEnvioMensagens">Chave do registro.</param>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de AlertaErroEnvioMensagens.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaErroEnvioMensagens LoadObject(int idErroEnvioMensagens, int idCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idErroEnvioMensagens, idCurriculo, trans))
			{
				AlertaErroEnvioMensagens objAlertaErroEnvioMensagens = new AlertaErroEnvioMensagens();
				if (SetInstance(dr, objAlertaErroEnvioMensagens))
					return objAlertaErroEnvioMensagens;
			}
			throw (new RecordNotFoundException(typeof(AlertaErroEnvioMensagens)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaErroEnvioMensagens a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idErroEnvioMensagens, this._idCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaErroEnvioMensagens a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idErroEnvioMensagens, this._idCurriculo, trans))
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
		/// <param name="objAlertaErroEnvioMensagens">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static bool SetInstance(IDataReader dr, AlertaErroEnvioMensagens objAlertaErroEnvioMensagens)
		{
			try
			{
				if (dr.Read())
				{
					objAlertaErroEnvioMensagens._idErroEnvioMensagens = Convert.ToInt32(dr["Idf_ErroEnvioMensagens"]);
					objAlertaErroEnvioMensagens._idCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
					if (dr["Nme_Destinatario"] != DBNull.Value)
						objAlertaErroEnvioMensagens._nomeDestinatario = Convert.ToString(dr["Nme_Destinatario"]);
					if (dr["Eml_Destinatario"] != DBNull.Value)
						objAlertaErroEnvioMensagens._emailDestinatario = Convert.ToString(dr["Eml_Destinatario"]);
					if (dr["Des_Mensagem"] != DBNull.Value)
						objAlertaErroEnvioMensagens._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);
					if (dr["Des_Erro"] != DBNull.Value)
						objAlertaErroEnvioMensagens._descricaoErro = Convert.ToString(dr["Des_Erro"]);
					objAlertaErroEnvioMensagens._DatCadastro = Convert.ToDateTime(dr["Dat_Cadastro"]);
					if (dr["Dat_Periodo_Ini"] != DBNull.Value)
						objAlertaErroEnvioMensagens._DatPeriodoIni = Convert.ToDateTime(dr["Dat_Periodo_Ini"]);
					if (dr["Dat_Periodo_Fim"] != DBNull.Value)
						objAlertaErroEnvioMensagens._DatPeriodoFim = Convert.ToDateTime(dr["Dat_Periodo_Fim"]);

					objAlertaErroEnvioMensagens._persisted = true;
					objAlertaErroEnvioMensagens._modified = false;

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