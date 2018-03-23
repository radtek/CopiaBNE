//-- Data: 28/11/2011 15:41
//-- Autor: Jhonatan Taborda

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class EmailDestinatarioCidade // Tabela: BNE_Email_Destinatario_Cidade
	{

		#region Atributos
		private int _idEmailDestinatarioCidade;
		private GrupoCidade _grupoCidade;
		private EmailDestinatario _emailDestinatario;
		private bool _flagResponsavel;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;
		private bool _flagInativo;
		private UsuarioFilialPerfil _usuarioGerador;
		private Filial _idfFilial;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdEmailDestinatarioCidade
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdEmailDestinatarioCidade
		{
			get
			{
				return this._idEmailDestinatarioCidade;
			}
		}
		#endregion 

		#region GrupoCidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public GrupoCidade GrupoCidade
		{
			get
			{
				return this._grupoCidade;
			}
			set
			{
				this._grupoCidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailDestinatario
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public EmailDestinatario EmailDestinatario
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

		#region FlagResponsavel
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagResponsavel
		{
			get
			{
				return this._flagResponsavel;
			}
			set
			{
				this._flagResponsavel = value;
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

		#region DataAlteracao
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public DateTime DataAlteracao
		{
			get
			{
				return this._dataAlteracao;
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

		#region UsuarioGerador
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public UsuarioFilialPerfil UsuarioGerador
		{
			get
			{
				return this._usuarioGerador;
			}
		}
		#endregion 

		#endregion

		#region Construtores
		public EmailDestinatarioCidade()
		{
		}
		public EmailDestinatarioCidade(int idEmailDestinatarioCidade)
		{
			this._idEmailDestinatarioCidade = idEmailDestinatarioCidade;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Email_Destinatario_Cidade (Idf_Grupo_Cidade, Idf_Email_Destinatario, Flg_Responsavel, Dta_Cadastro, Dta_Alteracao, Flg_Inativo, Idf_Usuario_Gerador, idf_Filial) VALUES (@Idf_Grupo_Cidade, @Idf_Email_Destinatario, @Flg_Responsavel, @Dta_Cadastro, @Dta_Alteracao, @Flg_Inativo, @Idf_Usuario_Gerador, @idf_Filial);SET @Idf_Email_Destinatario_Cidade = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Email_Destinatario_Cidade SET Idf_Grupo_Cidade = @Idf_Grupo_Cidade, Idf_Email_Destinatario = @Idf_Email_Destinatario, Flg_Responsavel = @Flg_Responsavel, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Inativo = @Flg_Inativo, Idf_Usuario_Gerador = @Idf_Usuario_Gerador WHERE Idf_Email_Destinatario_Cidade = @Idf_Email_Destinatario_Cidade";
		//private const string SPDELETE = "UPDATE BNE_Email_Destinatario_Cidade SET Idf_Usuario_Gerador = @Idf_Usuario_Gerador WHERE Idf_Email_Destinatario_Cidade = @Idf_Email_Destinatario_Cidade ; DELETE FROM BNE_Email_Destinatario_Cidade WHERE Idf_Email_Destinatario_Cidade = @Idf_Email_Destinatario_Cidade";
        private const string SPDELETE = "DELETE FROM BNE_Email_Destinatario_Cidade WHERE Idf_Email_Destinatario_Cidade = @Idf_Email_Destinatario_Cidade";
		private const string SPSELECTID = "SELECT * FROM BNE_Email_Destinatario_Cidade WHERE Idf_Email_Destinatario_Cidade = @Idf_Email_Destinatario_Cidade";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Jhonatan Taborda</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Destinatario_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Grupo_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Email_Destinatario", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Flg_Responsavel", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Idf_Usuario_Gerador", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@idf_Filial", SqlDbType.Int, 4));
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
			parms[0].Value = this._idEmailDestinatarioCidade;
			parms[1].Value = this._grupoCidade.IdGrupoCidade;
			parms[2].Value = this._emailDestinatario.IdEmailDestinatario;
			parms[3].Value = this._flagResponsavel;
			parms[6].Value = this._flagInativo;

			if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[4].Value = this._dataCadastro;
			this._dataAlteracao = DateTime.Now;
			parms[5].Value = this._dataAlteracao;

            //Problemas gerador de classes
            if (this._usuarioGerador != null)
                parms[7].Value = this._usuarioGerador.IdUsuarioFilialPerfil;
            else
                parms[7].Value = DBNull.Value;

            if (this._idfFilial != null)
				parms[8].Value = this._idfFilial.IdFilial;
			else
				parms[8].Value = DBNull.Value;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de EmailDestinatarioCidade no banco de dados.
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
						this._idEmailDestinatarioCidade = Convert.ToInt32(cmd.Parameters["@Idf_Email_Destinatario_Cidade"].Value);
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
		/// Método utilizado para inserir uma instância de EmailDestinatarioCidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idEmailDestinatarioCidade = Convert.ToInt32(cmd.Parameters["@Idf_Email_Destinatario_Cidade"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de EmailDestinatarioCidade no banco de dados.
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
		/// Método utilizado para atualizar uma instância de EmailDestinatarioCidade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de EmailDestinatarioCidade no banco de dados.
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
		/// Método utilizado para salvar uma instância de EmailDestinatarioCidade no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de EmailDestinatarioCidade no banco de dados.
		/// </summary>
		/// <param name="idEmailDestinatarioCidade">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEmailDestinatarioCidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Destinatario_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idEmailDestinatarioCidade;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de EmailDestinatarioCidade no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmailDestinatarioCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idEmailDestinatarioCidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Destinatario_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idEmailDestinatarioCidade;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de EmailDestinatarioCidade no banco de dados.
		/// </summary>
		/// <param name="idEmailDestinatarioCidade">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idEmailDestinatarioCidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Email_Destinatario_Cidade where Idf_Email_Destinatario_Cidade in (";

			for (int i = 0; i < idEmailDestinatarioCidade.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idEmailDestinatarioCidade[i];
			}
			parms.Add(new SqlParameter("@Idf_Usuario_Gerador", SqlDbType.Int, 4));

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idEmailDestinatarioCidade">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEmailDestinatarioCidade)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Destinatario_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idEmailDestinatarioCidade;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmailDestinatarioCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idEmailDestinatarioCidade, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Email_Destinatario_Cidade", SqlDbType.Int, 4));

			parms[0].Value = idEmailDestinatarioCidade;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ema.Idf_Email_Destinatario_Cidade, Ema.Idf_Grupo_Cidade, Ema.Idf_Email_Destinatario, Ema.Flg_Responsavel, Ema.Dta_Cadastro, Ema.Dta_Alteracao, Ema.Flg_Inativo, Ema.Idf_Usuario_Gerador FROM BNE_Email_Destinatario_Cidade Ema";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de EmailDestinatarioCidade a partir do banco de dados.
		/// </summary>
		/// <param name="idEmailDestinatarioCidade">Chave do registro.</param>
		/// <returns>Instância de EmailDestinatarioCidade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static EmailDestinatarioCidade LoadObject(int idEmailDestinatarioCidade)
		{
			using (IDataReader dr = LoadDataReader(idEmailDestinatarioCidade))
			{
				EmailDestinatarioCidade objEmailDestinatarioCidade = new EmailDestinatarioCidade();
				if (SetInstance(dr, objEmailDestinatarioCidade))
					return objEmailDestinatarioCidade;
			}
			throw (new RecordNotFoundException(typeof(EmailDestinatarioCidade)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de EmailDestinatarioCidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idEmailDestinatarioCidade">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de EmailDestinatarioCidade.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static EmailDestinatarioCidade LoadObject(int idEmailDestinatarioCidade, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idEmailDestinatarioCidade, trans))
			{
				EmailDestinatarioCidade objEmailDestinatarioCidade = new EmailDestinatarioCidade();
				if (SetInstance(dr, objEmailDestinatarioCidade))
					return objEmailDestinatarioCidade;
			}
			throw (new RecordNotFoundException(typeof(EmailDestinatarioCidade)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de EmailDestinatarioCidade a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idEmailDestinatarioCidade))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de EmailDestinatarioCidade a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idEmailDestinatarioCidade, trans))
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
		/// <param name="objEmailDestinatarioCidade">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, EmailDestinatarioCidade objEmailDestinatarioCidade)
		{
			try
			{
				if (dr.Read())
				{
					objEmailDestinatarioCidade._idEmailDestinatarioCidade = Convert.ToInt32(dr["Idf_Email_Destinatario_Cidade"]);
					objEmailDestinatarioCidade._grupoCidade = new GrupoCidade(Convert.ToInt32(dr["Idf_Grupo_Cidade"]));
					objEmailDestinatarioCidade._emailDestinatario = new EmailDestinatario(Convert.ToInt32(dr["Idf_Email_Destinatario"]));
					objEmailDestinatarioCidade._flagResponsavel = Convert.ToBoolean(dr["Flg_Responsavel"]);
					objEmailDestinatarioCidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objEmailDestinatarioCidade._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objEmailDestinatarioCidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objEmailDestinatarioCidade._usuarioGerador = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Gerador"]));

					objEmailDestinatarioCidade._persisted = true;
					objEmailDestinatarioCidade._modified = false;

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