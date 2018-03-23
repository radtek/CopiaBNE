//-- Data: 03/09/2010 12:13
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Contato // Tabela: TAB_Contato
	{
		#region Atributos
		private int _idContato;
		private PessoaFisicaComplemento _pessoaFisicaComplemento;
		private string _nomeContato;
		private string _tipoContato;
		private string _numeroDDDCelular;
		private string _numeroCelular;
		private string _numeroDDDTelefone;
		private string _numeroTelefone;
		private string _numeroRamalTelefone;
		private string _numeroDDDFax;
		private string _numeroFax;
		private string _emailContato;
		private bool _flagInativo;
		private DateTime _dataCadastro;
		private DateTime _dataAlteracao;
		private bool _flagImportado;
		private TipoContato _tipoContato_;
        private OperadoraCelular _operadoraCelular;

		private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

		#region IdContato
		/// <summary>
		/// Campo obrigatório.
		/// Campo auto-numerado.
		/// </summary>
		public int IdContato
		{
			get
			{
				return this._idContato;
			}
		}
		#endregion 

		#region PessoaFisicaComplemento
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public PessoaFisicaComplemento PessoaFisicaComplemento
		{
			get
			{
				return this._pessoaFisicaComplemento;
			}
			set
			{
				this._pessoaFisicaComplemento = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NomeContato
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string NomeContato
		{
			get
			{
				return this._nomeContato;
			}
			set
			{
				this._nomeContato = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoContato
		/// <summary>
		/// Tamanho do campo: 30.
		/// Campo opcional.
		/// </summary>
		public string TipoContato
		{
			get
			{
				return this._tipoContato;
			}
			set
			{
				this._tipoContato = value;
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

		#region NumeroDDDTelefone
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo opcional.
		/// </summary>
		public string NumeroDDDTelefone
		{
			get
			{
				return this._numeroDDDTelefone;
			}
			set
			{
				this._numeroDDDTelefone = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroTelefone
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string NumeroTelefone
		{
			get
			{
				return this._numeroTelefone;
			}
			set
			{
				this._numeroTelefone = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroRamalTelefone
		/// <summary>
		/// Tamanho do campo: 4.
		/// Campo opcional.
		/// </summary>
		public string NumeroRamalTelefone
		{
			get
			{
				return this._numeroRamalTelefone;
			}
			set
			{
				this._numeroRamalTelefone = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroDDDFax
		/// <summary>
		/// Tamanho do campo: 2.
		/// Campo opcional.
		/// </summary>
		public string NumeroDDDFax
		{
			get
			{
				return this._numeroDDDFax;
			}
			set
			{
				this._numeroDDDFax = value;
				this._modified = true;
			}
		}
		#endregion 

		#region NumeroFax
		/// <summary>
		/// Tamanho do campo: 10.
		/// Campo opcional.
		/// </summary>
		public string NumeroFax
		{
			get
			{
				return this._numeroFax;
			}
			set
			{
				this._numeroFax = value;
				this._modified = true;
			}
		}
		#endregion 

		#region EmailContato
		/// <summary>
		/// Tamanho do campo: 100.
		/// Campo opcional.
		/// </summary>
		public string EmailContato
		{
			get
			{
				return this._emailContato;
			}
			set
			{
				this._emailContato = value;
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

		#region FlagImportado
		/// <summary>
		/// Campo obrigatório.
		/// </summary>
		public bool FlagImportado
		{
			get
			{
				return this._flagImportado;
			}
			set
			{
				this._flagImportado = value;
				this._modified = true;
			}
		}
		#endregion 

		#region TipoContato
		/// <summary>
		/// Campo opcional.
		/// </summary>
		public TipoContato TipoContato_
		{
			get
			{
				return this._tipoContato_;
			}
			set
			{
				this._tipoContato_ = value;
				this._modified = true;
			}
		}
		#endregion 

        #region OperadoraCelular
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public OperadoraCelular OperadoraCelular
        {
            get
            {
                return this._operadoraCelular;
            }
            set
            {
                this._operadoraCelular = value;
                this._modified = true;
            }
        }
        #endregion 

		#endregion

		#region Construtores
		public Contato()
		{
		}
		public Contato(int idContato)
		{
			this._idContato = idContato;
			this._persisted = true;
		}
		#endregion

		#region Consultas
		private const string SPINSERT = "INSERT INTO TAB_Contato (Idf_Pessoa_Fisica, Nme_Contato, Tip_Contato, Num_DDD_Celular, Num_Celular, Num_DDD_Telefone, Num_Telefone, Num_Ramal_Telefone, Num_DDD_Fax, Num_Fax, Eml_Contato, Flg_Inativo, Dta_Cadastro, Dta_Alteracao, Flg_Importado, Idf_Tipo_Contato, Idf_Operadora_Celular) VALUES (@Idf_Pessoa_Fisica, @Nme_Contato, @Tip_Contato, @Num_DDD_Celular, @Num_Celular, @Num_DDD_Telefone, @Num_Telefone, @Num_Ramal_Telefone, @Num_DDD_Fax, @Num_Fax, @Eml_Contato, @Flg_Inativo, @Dta_Cadastro, @Dta_Alteracao, @Flg_Importado, @Idf_Tipo_Contato, @Idf_Operadora_Celular);SET @Idf_Contato = SCOPE_IDENTITY();";
		private const string SPUPDATE = "UPDATE TAB_Contato SET Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica, Nme_Contato = @Nme_Contato, Tip_Contato = @Tip_Contato, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular, Num_DDD_Telefone = @Num_DDD_Telefone, Num_Telefone = @Num_Telefone, Num_Ramal_Telefone = @Num_Ramal_Telefone, Num_DDD_Fax = @Num_DDD_Fax, Num_Fax = @Num_Fax, Eml_Contato = @Eml_Contato, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Importado = @Flg_Importado, Idf_Tipo_Contato = @Idf_Tipo_Contato, Idf_Operadora_Celular = @Idf_Operadora_Celular WHERE Idf_Contato = @Idf_Contato";
		private const string SPDELETE = "DELETE FROM TAB_Contato WHERE Idf_Contato = @Idf_Contato";
		private const string SPSELECTID = "SELECT * FROM TAB_Contato WHERE Idf_Contato = @Idf_Contato";
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
			parms.Add(new SqlParameter("@Idf_Contato", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Contato", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Tip_Contato", SqlDbType.VarChar, 30));
			parms.Add(new SqlParameter("@Num_DDD_Celular", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Celular", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Num_DDD_Telefone", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Telefone", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Num_Ramal_Telefone", SqlDbType.Char, 4));
			parms.Add(new SqlParameter("@Num_DDD_Fax", SqlDbType.Char, 2));
			parms.Add(new SqlParameter("@Num_Fax", SqlDbType.Char, 10));
			parms.Add(new SqlParameter("@Eml_Contato", SqlDbType.VarChar, 100));
			parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
			parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
			parms.Add(new SqlParameter("@Flg_Importado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Tipo_Contato", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Operadora_Celular", SqlDbType.Int, 4));
            return (parms);
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
			parms[0].Value = this._idContato;
			parms[1].Value = this._pessoaFisicaComplemento.PessoaFisica.IdPessoaFisica;

			if (!String.IsNullOrEmpty(this._nomeContato))
				parms[2].Value = this._nomeContato;
			else
				parms[2].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._tipoContato))
				parms[3].Value = this._tipoContato;
			else
				parms[3].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroDDDCelular))
				parms[4].Value = this._numeroDDDCelular;
			else
				parms[4].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroCelular))
				parms[5].Value = this._numeroCelular;
			else
				parms[5].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroDDDTelefone))
				parms[6].Value = this._numeroDDDTelefone;
			else
				parms[6].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroTelefone))
				parms[7].Value = this._numeroTelefone;
			else
				parms[7].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroRamalTelefone))
				parms[8].Value = this._numeroRamalTelefone;
			else
				parms[8].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroDDDFax))
				parms[9].Value = this._numeroDDDFax;
			else
				parms[9].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._numeroFax))
				parms[10].Value = this._numeroFax;
			else
				parms[10].Value = DBNull.Value;


			if (!String.IsNullOrEmpty(this._emailContato))
				parms[11].Value = this._emailContato;
			else
				parms[11].Value = DBNull.Value;

			parms[12].Value = this._flagInativo;
			parms[15].Value = this._flagImportado;

            if (this._tipoContato_ != null)
                parms[16].Value = this._tipoContato_.IdTipoContato;
            else
                parms[16].Value = DBNull.Value;


            if (this._operadoraCelular != null)
                parms[17].Value = this._operadoraCelular.IdOperadoraCelular;
            else
                parms[17].Value = DBNull.Value;


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
			this._dataAlteracao = DateTime.Now;
			parms[14].Value = this._dataAlteracao;
		}
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de Contato no banco de dados.
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
						this._idContato = Convert.ToInt32(cmd.Parameters["@Idf_Contato"].Value);
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
		/// Método utilizado para inserir uma instância de Contato no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			this._idContato = Convert.ToInt32(cmd.Parameters["@Idf_Contato"].Value);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de Contato no banco de dados.
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
		/// Método utilizado para atualizar uma instância de Contato no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de Contato no banco de dados.
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
		/// Método utilizado para salvar uma instância de Contato no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de Contato no banco de dados.
		/// </summary>
		/// <param name="idContato">Chave do registro.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idContato)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Contato", SqlDbType.Int, 4));

			parms[0].Value = idContato;

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma instância de Contato no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idContato">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(int idContato, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Contato", SqlDbType.Int, 4));

			parms[0].Value = idContato;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de Contato no banco de dados.
		/// </summary>
		/// <param name="idContato">Lista de chaves.</param>
		/// <remarks>Gieyson Stelmak</remarks>
		public static void Delete(List<int> idContato)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from TAB_Contato where Idf_Contato in (";

			for (int i = 0; i < idContato.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idContato[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idContato">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idContato)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Contato", SqlDbType.Int, 4));

			parms[0].Value = idContato;

			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idContato">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static IDataReader LoadDataReader(int idContato, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Contato", SqlDbType.Int, 4));

			parms[0].Value = idContato;

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

			string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Con.Idf_Contato, Con.Idf_Pessoa_Fisica, Con.Nme_Contato, Con.Tip_Contato, Con.Num_DDD_Celular, Con.Num_Celular, Con.Num_DDD_Telefone, Con.Num_Telefone, Con.Num_Ramal_Telefone, Con.Num_DDD_Fax, Con.Num_Fax, Con.Eml_Contato, Con.Flg_Inativo, Con.Dta_Cadastro, Con.Dta_Alteracao, Con.Flg_Importado, Con.Idf_Tipo_Contato, Con.Idf_Operadora_Celular FROM TAB_Contato Con";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de Contato a partir do banco de dados.
		/// </summary>
		/// <param name="idContato">Chave do registro.</param>
		/// <returns>Instância de Contato.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Contato LoadObject(int idContato)
		{
			using (IDataReader dr = LoadDataReader(idContato))
			{
				Contato objContato = new Contato();
				if (SetInstance(dr, objContato))
					return objContato;
			}
			throw (new RecordNotFoundException(typeof(Contato)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de Contato a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idContato">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de Contato.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public static Contato LoadObject(int idContato, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idContato, trans))
			{
				Contato objContato = new Contato();
				if (SetInstance(dr, objContato))
					return objContato;
			}
			throw (new RecordNotFoundException(typeof(Contato)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de Contato a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idContato))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de Contato a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idContato, trans))
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
		/// <param name="objContato">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Gieyson Stelmak</remarks>
		private static bool SetInstance(IDataReader dr, Contato objContato)
		{
			try
			{
				if (dr.Read())
				{
					objContato._idContato = Convert.ToInt32(dr["Idf_Contato"]);
                    objContato._pessoaFisicaComplemento = new PessoaFisicaComplemento(new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"])));
					if (dr["Nme_Contato"] != DBNull.Value)
						objContato._nomeContato = Convert.ToString(dr["Nme_Contato"]);
					if (dr["Tip_Contato"] != DBNull.Value)
						objContato._tipoContato = Convert.ToString(dr["Tip_Contato"]);
					if (dr["Num_DDD_Celular"] != DBNull.Value)
						objContato._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
					if (dr["Num_Celular"] != DBNull.Value)
						objContato._numeroCelular = Convert.ToString(dr["Num_Celular"]);
					if (dr["Num_DDD_Telefone"] != DBNull.Value)
						objContato._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
					if (dr["Num_Telefone"] != DBNull.Value)
						objContato._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
					if (dr["Num_Ramal_Telefone"] != DBNull.Value)
						objContato._numeroRamalTelefone = Convert.ToString(dr["Num_Ramal_Telefone"]);
					if (dr["Num_DDD_Fax"] != DBNull.Value)
						objContato._numeroDDDFax = Convert.ToString(dr["Num_DDD_Fax"]);
					if (dr["Num_Fax"] != DBNull.Value)
						objContato._numeroFax = Convert.ToString(dr["Num_Fax"]);
					if (dr["Eml_Contato"] != DBNull.Value)
						objContato._emailContato = Convert.ToString(dr["Eml_Contato"]);
					objContato._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
					objContato._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
					objContato._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
					objContato._flagImportado = Convert.ToBoolean(dr["Flg_Importado"]);
                    if (dr["Idf_Tipo_Contato"] != DBNull.Value)
                        objContato._tipoContato_ = new TipoContato(Convert.ToInt32(dr["Idf_Tipo_Contato"]));
                    if (dr["Idf_Operadora_Celular"] != DBNull.Value)
                        objContato._operadoraCelular = new OperadoraCelular(Convert.ToInt32(dr["Idf_Operadora_Celular"]));

					objContato._persisted = true;
					objContato._modified = false;

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