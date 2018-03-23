//-- Data: 17/11/2010 11:08
//-- Autor: Bruno Flammarion Chervisnki Boscolo

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Agradecimento // Tabela: BNE_Agradecimento
	{
		#region Atributos
		private int _idAgradecimento;
		private UsuarioFilialPerfil _usuarioFilialPerfil;
		private string _nomePessoa;
		private string _emailPessoa;
		private Cidade _cidade;
		private string _descricaoMensagem;
		private DateTime? _dataCadastro;
		private bool _flagMostrarSite;
		private bool _flagAuditado;
		private bool _flagInativo;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdAgradecimento
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdAgradecimento
		{
			get
			{
				return this._idAgradecimento;
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

		#region NomePessoa
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string NomePessoa
		{
			get
			{
				return this._nomePessoa;
			}
			set
			{
				this._nomePessoa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailPessoa
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo obrigatório.
		/// </summary>
		public string EmailPessoa
		{
			get
			{
				return this._emailPessoa;
			}
			set
			{
				this._emailPessoa = value;
				this._modified = true;
			}
		}
		#endregion 

		#region Cidade
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public Cidade Cidade
		{
			get
			{
				return this._cidade;
			}
			set
			{
				this._cidade = value;
				this._modified = true;
			}
		}
		#endregion 

		#region DescricaoMensagem
		/// <summary>
		/// Tamanho do campo: 500.
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

		#region FlagAuditado
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagAuditado
		{
			get
			{
				return this._flagAuditado;
			}
			set
			{
				this._flagAuditado = value;
				this._modified = true;
			}
		}
        #endregion

	    #region FlagMostrarSite
	    /// <summary>
	    /// Campo obrigatório.
	    /// </summary>
	    public bool FlagMostrarSite
	    {
	        get
	        {
	            return this._flagMostrarSite;
	        }
	        set
	        {
	            this._flagMostrarSite = value;
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

		#endregion

		#region Construtores
		public Agradecimento()
		{
		}
		public Agradecimento(int idAgradecimento)
		{
			this._idAgradecimento = idAgradecimento;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO BNE_Agradecimento (Idf_Usuario_Filial_Perfil, Nme_Pessoa, Eml_Pessoa, Idf_Cidade, Des_Mensagem, Dta_Cadastro, Flg_Auditado, Flg_Inativo, Flg_Mostrar_Site) VALUES (@Idf_Usuario_Filial_Perfil, @Nme_Pessoa, @Eml_Pessoa, @Idf_Cidade, @Des_Mensagem, @Dta_Cadastro, @Flg_Auditado, @Flg_Inativo, @Flg_Mostrar_Site);SET @Idf_Agradecimento = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE BNE_Agradecimento SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Nme_Pessoa = @Nme_Pessoa, Eml_Pessoa = @Eml_Pessoa, Idf_Cidade = @Idf_Cidade, Des_Mensagem = @Des_Mensagem, Dta_Cadastro = @Dta_Cadastro, Flg_Auditado = @Flg_Auditado, Flg_Inativo = @Flg_Inativo, Flg_Mostrar_Site = @Flg_Mostrar_Site WHERE Idf_Agradecimento = @Idf_Agradecimento";
		private const string SPDELETE = "DELETE FROM BNE_Agradecimento WHERE Idf_Agradecimento = @Idf_Agradecimento";
		private const string SPSELECTID = "SELECT * FROM BNE_Agradecimento WHERE Idf_Agradecimento = @Idf_Agradecimento";
		#endregion

		#region Métodos

		#region GetParameters
		/// <summary>
		/// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
		/// </summary>
		/// <returns>Lista de parâmetros SQL.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private List<SqlParameter> GetParameters()
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Agradecimento", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar, 500));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Auditado", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Flg_Mostrar_Site", SqlDbType.Bit, 1));
            
            return (parms);
		}
		#endregion

		#region SetParameters
		/// <summary>
		/// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
		/// </summary>
		/// <param name="parms">Lista de parâmetros SQL.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private void SetParameters(List<SqlParameter> parms)
		{
			parms[0].Value = this._idAgradecimento;

			if (this._usuarioFilialPerfil != null)
				parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
			else
				parms[1].Value = DBNull.Value;

			parms[2].Value = this._nomePessoa;
			parms[3].Value = this._emailPessoa;
			parms[4].Value = this._cidade.IdCidade;
			parms[5].Value = this._descricaoMensagem;
			parms[7].Value = this._flagAuditado;
			parms[8].Value = this._flagInativo;
		    parms[9].Value = this._flagMostrarSite;

            if (!this._persisted)
			{
				parms[0].Direction = ParameterDirection.Output;
				this._dataCadastro = DateTime.Now;
			}
			else
			{
				parms[0].Direction = ParameterDirection.Input;
			}
			parms[6].Value = this._dataCadastro;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Agradecimento no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
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
						this._idAgradecimento = Convert.ToInt32(cmd.Parameters["@Idf_Agradecimento"].Value);
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
		/// Método utilizado para inserir uma instância de Agradecimento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idAgradecimento = Convert.ToInt32(cmd.Parameters["@Idf_Agradecimento"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Agradecimento no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
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
		/// Método utilizado para atualizar uma instância de Agradecimento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
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
		/// Método utilizado para salvar uma instância de Agradecimento no banco de dados.
		/// </summary>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public void Save()
		{
			if (!this._persisted)
				this.Insert();
			else
				this.Update();
		}
		/// <summary>
		/// Método utilizado para salvar uma instância de Agradecimento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
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
		/// Método utilizado para excluir uma instância de Agradecimento no banco de dados.
		/// </summary>
		/// <param name="idAgradecimento">Chave do registro.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public static void Delete(int idAgradecimento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Agradecimento", SqlDbType.Int, 4));

			parms[0].Value = idAgradecimento;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Agradecimento no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAgradecimento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public static void Delete(int idAgradecimento, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Agradecimento", SqlDbType.Int, 4));

			parms[0].Value = idAgradecimento;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Agradecimento no banco de dados.
		/// </summary>
		/// <param name="idAgradecimento">Lista de chaves.</param>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public static void Delete(List<int> idAgradecimento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from BNE_Agradecimento where Idf_Agradecimento in (";

			for (int i = 0; i < idAgradecimento.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idAgradecimento[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idAgradecimento">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private static IDataReader LoadDataReader(int idAgradecimento)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Agradecimento", SqlDbType.Int, 4));

			parms[0].Value = idAgradecimento;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAgradecimento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private static IDataReader LoadDataReader(int idAgradecimento, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Agradecimento", SqlDbType.Int, 4));

			parms[0].Value = idAgradecimento;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Agr.Idf_Agradecimento, Agr.Idf_Usuario_Filial_Perfil, Agr.Nme_Pessoa, Agr.Eml_Pessoa, Agr.Idf_Cidade, Agr.Des_Mensagem, Agr.Dta_Cadastro, Agr.Flg_Auditado, Agr.Flg_Inativo, Agr.Flg_Mostrar_Site FROM BNE_Agradecimento Agr";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Agradecimento a partir do banco de dados.
		/// </summary>
		/// <param name="idAgradecimento">Chave do registro.</param>
		/// <returns>Instância de Agradecimento.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public static Agradecimento LoadObject(int idAgradecimento)
		{
			using (IDataReader dr = LoadDataReader(idAgradecimento))
			{
				Agradecimento objAgradecimento = new Agradecimento();
				if (SetInstance(dr, objAgradecimento))
					return objAgradecimento;
			}
			throw (new RecordNotFoundException(typeof(Agradecimento)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Agradecimento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idAgradecimento">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Agradecimento.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public static Agradecimento LoadObject(int idAgradecimento, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idAgradecimento, trans))
			{
				Agradecimento objAgradecimento = new Agradecimento();
				if (SetInstance(dr, objAgradecimento))
					return objAgradecimento;
			}
			throw (new RecordNotFoundException(typeof(Agradecimento)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Agradecimento a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idAgradecimento))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Agradecimento a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idAgradecimento, trans))
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
		/// <param name="objAgradecimento">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Bruno Flammarion Chervisnki Boscolo</remarks>
		private static bool SetInstance(IDataReader dr, Agradecimento objAgradecimento)
		{
			try
			{
				if (dr.Read())
				{
					objAgradecimento._idAgradecimento = Convert.ToInt32(dr["Idf_Agradecimento"]);
					if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
						objAgradecimento._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
					objAgradecimento._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
					objAgradecimento._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
					objAgradecimento._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
					objAgradecimento._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);
					if (dr["Dta_Cadastro"] != DBNull.Value)
						objAgradecimento._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objAgradecimento._flagAuditado = Convert.ToBoolean(dr["Flg_Auditado"]);
					objAgradecimento._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
				    objAgradecimento._flagMostrarSite = Convert.ToBoolean(dr["Flg_Mostrar_Site"]);

                    objAgradecimento._persisted = true;
					objAgradecimento._modified = false;

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