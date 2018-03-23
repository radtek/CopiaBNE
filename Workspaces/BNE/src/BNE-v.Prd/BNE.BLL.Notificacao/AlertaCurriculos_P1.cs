//-- Data: 23/07/2013 15:25
//-- Autor: Luan Fernandes

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL.Notificacao
{
	public partial class AlertaCurriculos // Tabela: alerta.Tab_Alerta_Curriculos
	{
		#region Atributos
		private int _idCurriculo;
		private string _nomePessoa;
		private string _emailPessoa;
        private bool _flg_VIP;
        private decimal? _vlrPretensaoSalarial;
        private DateTime? _dataCadastroAlertaCurriculo;
        private DateTime? _dataUltimoEnvio;
        private string _descricaoFuncao;
        private string _nomeCidade;
        private string _siglaEstado;
        private decimal? _numeroCPF;
        private DateTime? _dataNascimento;
        private int? _idDeficiencia;

        private bool _persisted;
		private bool _modified;
		#endregion

		#region Propriedades

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

		#region NomePessoa
		/// <summary>
		/// Tamanho do campo: 250.
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
		/// Tamanho do campo: 250.
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

        #region Flg_VIP

        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagVIP
        {
            get
            {
                return this._flg_VIP;
            }
            set
            {
                this._flg_VIP = value;
                this._modified = true;
            }
        }

        #endregion

        #region ValorPretensaoSalarial
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? ValorPretensaoSalarial
        {
            get
            {
                return this._vlrPretensaoSalarial;
            }
            set
            {
                this._vlrPretensaoSalarial = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataCadastroAlertaCurriculo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataCadastroAlertaCurriculo
        {
            get
            {
                return this._dataCadastroAlertaCurriculo;
            }
        }
        #endregion

        #region DataUltimoEnvio
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataUltimoEnvio
        {
            get
            {
                return this._dataUltimoEnvio;
            }
            set
            {
                this._dataUltimoEnvio = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoFuncao
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string DescricaoFuncao
        {
            get
            {
                return this._descricaoFuncao;
            }
            set
            {
                this._descricaoFuncao = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomeCidade
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string NomeCidade
        {
            get
            {
                return this._nomeCidade;
            }
            set
            {
                this._nomeCidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region SiglaEstado
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string SiglaEstado
        {
            get
            {
                return this._siglaEstado;
            }
            set
            {
                this._siglaEstado = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroCPF
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? NumeroCPF
        {
            get
            {
                return this._numeroCPF;
            }
            set
            {
                this._numeroCPF = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataNascimento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataNascimento
        {
            get
            {
                return this._dataNascimento;
            }
            set
            {
                this._dataNascimento = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdDeficiencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? IdDeficiencia
        {
            get
            {
                return this._idDeficiencia;
            }
            set
            {
                this._idDeficiencia = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public AlertaCurriculos()
		{
		}
		public AlertaCurriculos(int idCurriculo)
		{
			this._idCurriculo = idCurriculo;
			this._persisted = true;
		}
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO alerta.Tab_Alerta_Curriculos (Idf_Curriculo, Nme_Pessoa, Eml_Pessoa, Flg_VIP, Vlr_Pretensao_Salarial, Dta_Cadastro_Alerta_Curriculo, Dta_Ultimo_Envio, Des_Funcao, Nme_Cidade, Sig_Estado, Num_CPF, Dta_Nascimento, Idf_Deficiencia) VALUES (@Idf_Curriculo, @Nme_Pessoa, @Eml_Pessoa, @Flg_VIP, @Vlr_Pretensao_Salarial, @Dta_Cadastro_Alerta_Curriculo, @Dta_Ultimo_Envio, @Des_Funcao, @Nme_Cidade, @Sig_Estado, @Num_CPF, @Dta_Nascimento, @Idf_Deficiencia);";
        private const string SPUPDATE = "UPDATE alerta.Tab_Alerta_Curriculos SET Nme_Pessoa = @Nme_Pessoa, Eml_Pessoa = @Eml_Pessoa, Flg_VIP = @Flg_VIP, Vlr_Pretensao_Salarial = @Vlr_Pretensao_Salarial, Dta_Cadastro_Alerta_Curriculo = @Dta_Cadastro_Alerta_Curriculo, Dta_Ultimo_Envio = @Dta_Ultimo_Envio, Des_Funcao = @Des_Funcao, Nme_Cidade = @Nme_Cidade, Sig_Estado = @Sig_Estado, Num_CPF = @Num_CPF, Dta_Nascimento = @Dta_Nascimento, Idf_Deficiencia = @Idf_Deficiencia WHERE Idf_Curriculo = @Idf_Curriculo";
        private const string SPDELETE = "DELETE FROM alerta.Tab_Alerta_Curriculos WHERE Idf_Curriculo = @Idf_Curriculo";
        private const string SPSELECTID = "SELECT * FROM alerta.Tab_Alerta_Curriculos WITH(NOLOCK) WHERE Idf_Curriculo = @Idf_Curriculo";
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
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
			parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 250));
			parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 250));
            parms.Add(new SqlParameter("@Flg_VIP", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Vlr_Pretensao_Salarial", SqlDbType.Decimal, 10));
            parms.Add(new SqlParameter("@Dta_Cadastro_Alerta_Curriculo", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Ultimo_Envio", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Des_Funcao", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Nme_Cidade", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Dta_Nascimento", SqlDbType.DateTime, 3));
            parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
            return (parms);
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
			parms[0].Value = this._idCurriculo;
			parms[1].Value = this._nomePessoa;
			parms[2].Value = this._emailPessoa;
            parms[3].Value = this._flg_VIP;

            if (this._vlrPretensaoSalarial.HasValue)
                parms[4].Value = this._vlrPretensaoSalarial.Value;
            else
                parms[4].Value = DBNull.Value;

            if (this._dataUltimoEnvio.HasValue)
                parms[6].Value = this._dataUltimoEnvio;
            else
                parms[6].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoFuncao))
                parms[7].Value = this._descricaoFuncao;
            else
                parms[7].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._nomeCidade))
                parms[8].Value = this._nomeCidade;
            else
                parms[8].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._siglaEstado))
                parms[9].Value = this._siglaEstado;
            else
                parms[9].Value = DBNull.Value;


            if (this._numeroCPF.HasValue)
                parms[10].Value = this._numeroCPF;
            else
                parms[10].Value = DBNull.Value;


            if (this._dataNascimento.HasValue)
                parms[11].Value = this._dataNascimento;
            else
                parms[11].Value = DBNull.Value;


            if (this._idDeficiencia.HasValue)
                parms[12].Value = this._idDeficiencia;
            else
                parms[12].Value = DBNull.Value;


            if (!this._persisted)
            {
                this._dataCadastroAlertaCurriculo = DateTime.Now;
            }
            parms[5].Value = this._dataCadastroAlertaCurriculo;
        }
		#endregion

		#region Insert
		/// <summary>
		/// Método utilizado para inserir uma instância de AlertaCurriculos no banco de dados.
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
		/// Método utilizado para inserir uma instância de AlertaCurriculos no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
		private void Insert(SqlTransaction trans)
		{
			List<SqlParameter> parms = GetParameters();
			SetParameters(parms);
			SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
			cmd.Parameters.Clear();
			this._persisted = true;
			this._modified = false;
		}
		#endregion

		#region Update
		/// <summary>
		/// Método utilizado para atualizar uma instância de AlertaCurriculos no banco de dados.
		/// </summary>
		/// <remarks>Luan Fernandes</remarks>
		private void Update()
		{
			if (this._modified)
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

                            DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPUPDATE, parms);
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
		}
		/// <summary>
		/// Método utilizado para atualizar uma instância de AlertaCurriculos no banco de dados, dentro de uma transação.
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
		/// Método utilizado para salvar uma instância de AlertaCurriculos no banco de dados.
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
		/// Método utilizado para salvar uma instância de AlertaCurriculos no banco de dados, dentro de uma transação.
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
		/// Método utilizado para excluir uma instância de AlertaCurriculos no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;

			DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
		}
		/// <summary>
		/// Método utilizado para excluir uma lista de AlertaCurriculos no banco de dados.
		/// </summary>
		/// <param name="idCurriculo">Lista de chaves.</param>
		/// <remarks>Luan Fernandes</remarks>
		public static void Delete(List<int> idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			string query = "delete from alerta.Tab_Alerta_Curriculos where Idf_Curriculo in (";

			for (int i = 0; i < idCurriculo.Count; i++)
			{
				string nomeParametro = "@parm" + i.ToString();

				if (i > 0)
				{
					query += ", ";
				}
				query += nomeParametro;
				parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
				parms[i].Value = idCurriculo[i];
			}

			query += ")";

			DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
		}
		#endregion

		#region LoadDataReader
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idCurriculo)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms, DataAccessLayer.CONN_STRING);
		}
		/// <summary>
		/// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Cursor de leitura do banco de dados.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static IDataReader LoadDataReader(int idCurriculo, SqlTransaction trans)
		{
			List<SqlParameter> parms = new List<SqlParameter>();
			parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

			parms[0].Value = idCurriculo;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, bAl.Idf_Curriculo, bAl.Nme_Pessoa, bAl.Eml_Pessoa, bAl.Flg_VIP, bAl.Vlr_Pretensao_Salarial FROM alerta.Tab_Alerta_Curriculos bAl";
			string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
			SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

			totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
			return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null, DataAccessLayer.CONN_STRING);
		}
		#endregion

		#region LoadObject
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaCurriculos a partir do banco de dados.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <returns>Instância de AlertaCurriculos.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaCurriculos LoadObject(int idCurriculo)
		{
			using (IDataReader dr = LoadDataReader(idCurriculo))
			{
				AlertaCurriculos objAlertaCurriculos = new AlertaCurriculos();
				if (SetInstance(dr, objAlertaCurriculos))
					return objAlertaCurriculos;
			}
			throw (new RecordNotFoundException(typeof(AlertaCurriculos)));
		}
		/// <summary>
		/// Método utilizado para retornar uma instância de AlertaCurriculos a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="idCurriculo">Chave do registro.</param>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Instância de AlertaCurriculos.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public static AlertaCurriculos LoadObject(int idCurriculo, SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(idCurriculo, trans))
			{
				AlertaCurriculos objAlertaCurriculos = new AlertaCurriculos();
				if (SetInstance(dr, objAlertaCurriculos))
					return objAlertaCurriculos;
			}
			throw (new RecordNotFoundException(typeof(AlertaCurriculos)));
		}
		#endregion

		#region CompleteObject
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaCurriculos a partir do banco de dados.
		/// </summary>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject()
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculo))
			{
				return SetInstance(dr, this);
			}
		}
		/// <summary>
		/// Método utilizado para completar uma instância de AlertaCurriculos a partir do banco de dados, dentro de uma transação.
		/// </summary>
		/// <param name="trans">Transação existente no banco de dados.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		public bool CompleteObject(SqlTransaction trans)
		{
			using (IDataReader dr = LoadDataReader(this._idCurriculo, trans))
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
		/// <param name="objAlertaCurriculos">Instância a ser manipulada.</param>
		/// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
		/// <remarks>Luan Fernandes</remarks>
		private static bool SetInstance(IDataReader dr, AlertaCurriculos objAlertaCurriculos)
		{
			try
			{
				if (dr.Read())
				{
					objAlertaCurriculos._idCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
					objAlertaCurriculos._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
					objAlertaCurriculos._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
                                        objAlertaCurriculos._flg_VIP = Convert.ToBoolean(dr["Flg_VIP"]);
                                        if (dr["Vlr_Pretensao_Salarial"] != DBNull.Value)
                                            objAlertaCurriculos._vlrPretensaoSalarial = Convert.ToDecimal(dr["Vlr_Pretensao_Salarial"]);
                    if (dr["Dta_Cadastro_Alerta_Curriculo"] != DBNull.Value)
                        objAlertaCurriculos._dataCadastroAlertaCurriculo = Convert.ToDateTime(dr["Dta_Cadastro_Alerta_Curriculo"]);
                    if (dr["Dta_Ultimo_Envio"] != DBNull.Value)
                        objAlertaCurriculos._dataUltimoEnvio = Convert.ToDateTime(dr["Dta_Ultimo_Envio"]);
                    if (dr["Des_Funcao"] != DBNull.Value)
                        objAlertaCurriculos._descricaoFuncao = Convert.ToString(dr["Des_Funcao"]);
                    if (dr["Nme_Cidade"] != DBNull.Value)
                        objAlertaCurriculos._nomeCidade = Convert.ToString(dr["Nme_Cidade"]);
                    if (dr["Sig_Estado"] != DBNull.Value)
                        objAlertaCurriculos._siglaEstado = Convert.ToString(dr["Sig_Estado"]);
                    if (dr["Num_CPF"] != DBNull.Value)
                        objAlertaCurriculos._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
                    if (dr["Dta_Nascimento"] != DBNull.Value)
                        objAlertaCurriculos._dataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
                    if (dr["Idf_Deficiencia"] != DBNull.Value)
                        objAlertaCurriculos._idDeficiencia = Convert.ToInt32(dr["Idf_Deficiencia"]);

                    objAlertaCurriculos._persisted = true;
					objAlertaCurriculos._modified = false;

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