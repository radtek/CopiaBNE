using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class BoletoBancario //BNE_BOLETO_BANCARIO
    {
        #region Atributos
        private int _idBoletoBancario;
        private Pagamento _pagamento;
        private Banco _banco;
        private DateTime? _dataEmissao;
        private DateTime? _dataVencimento;

        //Cedente
        private string _cedenteNumCNPJCPF;
        private string _cedenteNome;
        private string _cedenteAgencia;
        private string _cedenteDVAgencia;
        private string _cedenteNumeroConta;
        private string _cedenteDVConta;
        private string _cedenteCodigo;

        //Sacado
        private string _sacadoNumCNPJCPF;
        private string _sacadoNome;
        private string _sacadoEnderecoLogradouro;
        private string _sacadoEnderecoNumero;
        private string _sacadoEnderecoComplemento;
        private string _sacadoEnderecoBairro;
        private Cidade _sacadoEnderecoCidade;
        private string _sacadoEnderecoCEP;
        //Instrucao
        private string _instrucaoDescricao;
        private string _numeroNossoNumero;
        private decimal _valorBoleto;
        private string _UrlBoleto;

        private bool _flagEmpresa;
        private bool _persisted;
        private bool _modified;

        #endregion

        #region Construtores
        public BoletoBancario()
        {
        }
        public BoletoBancario(int idBoletoBancario)
        {
            this._idBoletoBancario = idBoletoBancario;
            this._persisted = true;
        }
        #endregion

        #region Propriedades

        #region IdBoletoBancario
        public int IdBoletoBancario
        {
            get
            {
                return this._idBoletoBancario;
            }
        }
        #endregion

        #region Pagamento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Pagamento Pagamento
        {
            get
            {
                return this._pagamento;
            }
            set
            {
                this._pagamento = value;
                this._modified = true;
            }
        }
        #endregion

        #region Banco
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Banco Banco
        {
            get
            {
                return this._banco;
            }
            set
            {
                this._banco = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataEmissao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataEmissao
        {
            get { return _dataEmissao; }
            set
            {
                _dataEmissao = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataVencimento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataVencimento
        {
            get { return _dataVencimento; }
            set
            {
                _dataVencimento = value;
                this._modified = true;
            }
        }
        #endregion

        //CEDENTE

        #region CedenteNumCNPJCPF
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string CedenteNumCNPJCPF
        {
            get
            {
                return this._cedenteNumCNPJCPF;
            }
            set
            {
                this._cedenteNumCNPJCPF = value;
                this._modified = true;
            }
        }
        #endregion

        #region CedenteNome
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string CedenteNome
        {
            get
            {
                return this._cedenteNome;
            }
            set
            {
                this._cedenteNome = value;
                this._modified = true;
            }
        }

        #endregion

        #region CedenteAgencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string CedenteAgencia
        {
            get
            {
                return this._cedenteAgencia;
            }
            set
            {
                this._cedenteAgencia = value;
                this._modified = true;
            }
        }
        #endregion

        #region CedenteDVAgencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string CedenteDVAgencia
        {
            get
            {
                return this._cedenteDVAgencia;
            }
            set
            {
                this._cedenteDVAgencia = value;
                this._modified = true;
            }
        }
        #endregion

        #region CedenteNumeroConta
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string CedenteNumeroConta
        {
            get
            {
                return this._cedenteNumeroConta;
            }
            set
            {
                this._cedenteNumeroConta = value;
                this._modified = true;
            }
        }
        #endregion

        #region CedenteDVConta
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string CedenteDVConta
        {
            get
            {
                return this._cedenteDVConta;
            }
            set
            {
                this._cedenteDVConta = value;
                this._modified = true;
            }
        }

        #endregion

        #region CedenteCodigo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string CedenteCodigo
        {
            get { return _cedenteCodigo; }
            set { _cedenteCodigo = value; }
        }
        #endregion

        //SACADO 

        #region SacadoNumCNPJCPF
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string SacadoNumCNPJCPF
        {
            get { return _sacadoNumCNPJCPF; }
            set
            {
                _sacadoNumCNPJCPF = value;
                this._modified = true;
            }
        }
        #endregion

        #region SacadoNome
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string SacadoNome
        {
            get { return _sacadoNome; }
            set
            {
                _sacadoNome = value;
                this._modified = true;
            }
        }
        #endregion

        #region SacadoEnderecoLogradouro
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string SacadoEnderecoLogradouro
        {
            get { return _sacadoEnderecoLogradouro; }
            set
            {
                _sacadoEnderecoLogradouro = value;
                this._modified = true;
            }
        }
        #endregion

        #region SacadoEnderecoNumero
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string SacadoEnderecoNumero
        {
            get { return _sacadoEnderecoNumero; }
            set
            {
                _sacadoEnderecoNumero = value;
                this._modified = true;
            }
        }
        #endregion

        #region SacadoEnderecoComplemento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string SacadoEnderecoComplemento
        {
            get { return _sacadoEnderecoComplemento; }
            set
            {
                _sacadoEnderecoComplemento = value;
                this._modified = true;
            }
        }
        #endregion

        #region SacadoEnderecoBairro
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string SacadoEnderecoBairro
        {
            get { return _sacadoEnderecoBairro; }
            set
            {
                _sacadoEnderecoBairro = value;
                this._modified = true;
            }
        }
        #endregion

        #region SacadoEnderecoCidade
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Cidade SacadoEnderecoCidade
        {
            get { return _sacadoEnderecoCidade; }
            set
            {
                _sacadoEnderecoCidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region SacadoEnderecoCEP
        public string SacadoEnderecoCEP
        {
            get { return _sacadoEnderecoCEP; }
            set
            {
                _sacadoEnderecoCEP = value;
                this._modified = true;
            }
        }
        #endregion
        //Instrução

        #region InstrucaoDescricao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string InstrucaoDescricao
        {
            get { return _instrucaoDescricao; }
            set
            {
                _instrucaoDescricao = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroNossoNumero

        public string NumeroNossoNumero
        {
            get { return _numeroNossoNumero; }
            set
            {
                _numeroNossoNumero = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorBoleto
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal ValorBoleto
        {
            get
            {
                return this._valorBoleto;
            }
            set
            {
                this._valorBoleto = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagEmpresa
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagEmpresa
        {
            get
            {
                return this._flagEmpresa;
            }
            set
            {
                this._flagEmpresa = value;
                this._modified = true;
            }
        }
        #endregion

        #region UrlBoleto
        /// <summary>
		/// Tamanho do campo: 200.
		/// Campo opcional.
		/// </summary>
		public string UrlBoleto
        {
            get
            {
                return this._UrlBoleto;
            }
            set
            {
                this._UrlBoleto = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Boleto_Bancario (Idf_Pagamento, Idf_Banco, Dta_Emissao, Dta_Vencimento, Cedente_Numero_CNPJCPF, Cedente_Nome, Cedente_Agencia, Cedente_DV_Agencia, Cedente_Numero_Conta, Cedente_DVConta, Cedente_Codigo, Sacado_Numero_CNPJCPF, Sacado_Nome, Sacado_Endereco_Logradouro, Sacado_Endereco_Numero, Sacado_Endereco_Complemento, Sacado_Endereco_Bairro, Sacado_Endereco_Cidade, Sacado_Endereco_CEP, Instrucao_Descricao, Num_Nosso_Numero, Valor_Boleto, Flag_Empresa, Url_Boleto) VALUES (@Idf_Pagamento, @Idf_Banco, @Dta_Emissao, @Dta_Vencimento, @Cedente_Numero_CNPJCPF, @Cedente_Nome, @Cedente_Agencia, @Cedente_DV_Agencia, @Cedente_Numero_Conta, @Cedente_DVConta, @Cedente_Codigo, @Sacado_Numero_CNPJCPF, @Sacado_Nome, @Sacado_Endereco_Logradouro, @Sacado_Endereco_Numero, @Sacado_Endereco_Complemento, @Sacado_Endereco_Bairro, @Sacado_Endereco_Cidade, @Sacado_Endereco_CEP, @Instrucao_Descricao, @Num_Nosso_Numero, @Valor_Boleto, @Flag_Empresa, @Url_Boleto);SET @Idf_Boleto_Bancario = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Boleto_Bancario SET Idf_Pagamento = @Idf_Pagamento, Idf_Banco = @Idf_Banco, Dta_Emissao = @Dta_Emissao, Dta_Vencimento = @Dta_Vencimento, Cedente_Numero_CNPJCPF = @Cedente_Numero_CNPJCPF, Cedente_Nome = @Cedente_Nome, Cedente_Agencia = @Cedente_Agencia, Cedente_DV_Agencia = @Cedente_DV_Agencia, Cedente_Numero_Conta = @Cedente_Numero_Conta, Cedente_DVConta = @Cedente_DVConta, Cedente_Codigo = @Cedente_Codigo, Sacado_Numero_CNPJCPF = @Sacado_Numero_CNPJCPF, Sacado_Nome = @Sacado_Nome, Sacado_Endereco_Logradouro = @Sacado_Endereco_Logradouro, Sacado_Endereco_Numero = @Sacado_Endereco_Numero, Sacado_Endereco_Complemento = @Sacado_Endereco_Complemento, Sacado_Endereco_Bairro = @Sacado_Endereco_Bairro, Sacado_Endereco_Cidade = @Sacado_Endereco_Cidade, Sacado_Endereco_CEP = @Sacado_Endereco_CEP, Instrucao_Descricao = @Instrucao_Descricao, Num_Nosso_Numero = @Num_Nosso_Numero, Valor_Boleto = @Valor_Boleto, Flag_Empresa = @Flag_Empresa, Url_Boleto = @Url_Boleto WHERE Idf_Boleto_Bancario = @Idf_Boleto_Bancario";
        private const string SPDELETE = "DELETE FROM BNE_Boleto_Bancario WHERE Idf_Boleto_Bancario = @Idf_Boleto_Bancario";
        private const string SPSELECTID = "SELECT * FROM BNE_Boleto_Bancario WITH(NOLOCK) WHERE Idf_Boleto_Bancario = @Idf_Boleto_Bancario";
        #endregion

        #region Metodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Boleto_Bancario", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Pagamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Banco", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Emissao", SqlDbType.DateTime, 9));
            parms.Add(new SqlParameter("@Dta_Vencimento", SqlDbType.Date, 9));
            parms.Add(new SqlParameter("@Cedente_Numero_CNPJCPF", SqlDbType.VarChar, 20));
            parms.Add(new SqlParameter("@Cedente_Nome", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Cedente_Agencia", SqlDbType.VarChar, 10));
            parms.Add(new SqlParameter("@Cedente_DV_Agencia", SqlDbType.VarChar, 3));
            parms.Add(new SqlParameter("@Cedente_Numero_Conta", SqlDbType.VarChar, 8));
            parms.Add(new SqlParameter("@Cedente_DVConta", SqlDbType.VarChar, 3));
            parms.Add(new SqlParameter("@Cedente_Codigo", SqlDbType.VarChar, 8));
            parms.Add(new SqlParameter("@Sacado_Numero_CNPJCPF", SqlDbType.VarChar, 20));
            parms.Add(new SqlParameter("@Sacado_Nome", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Sacado_Endereco_Logradouro", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Sacado_Endereco_Numero", SqlDbType.VarChar, 15));
            parms.Add(new SqlParameter("@Sacado_Endereco_Complemento", SqlDbType.VarChar, 30));
            parms.Add(new SqlParameter("@Sacado_Endereco_Bairro", SqlDbType.VarChar, 30));
            parms.Add(new SqlParameter("@Sacado_Endereco_Cidade", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Sacado_Endereco_CEP", SqlDbType.VarChar, 8));
            parms.Add(new SqlParameter("@Instrucao_Descricao", SqlDbType.VarChar, 500));
            parms.Add(new SqlParameter("@Num_Nosso_Numero", SqlDbType.VarChar, 20));
            parms.Add(new SqlParameter("@Valor_Boleto", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Flag_Empresa", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Url_Boleto", SqlDbType.VarChar, 200));
            return (parms);
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
            parms[0].Value = this._idBoletoBancario;

            if (this._pagamento != null)
                parms[1].Value = this._pagamento.IdPagamento;
            else
                parms[1].Value = DBNull.Value;

            if (this._banco != null)
                parms[2].Value = this._banco.IdBanco;
            else
                parms[2].Value = DBNull.Value;

            if (this._dataEmissao != null)
                parms[3].Value = this._dataEmissao;
            else
                parms[3].Value = DBNull.Value;


            if (this._dataVencimento.HasValue)
                parms[4].Value = this._dataVencimento;
            else
                parms[4].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._cedenteNumCNPJCPF))
                parms[5].Value = this._cedenteNumCNPJCPF;
            else
                parms[5].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._cedenteNome))
                parms[6].Value = this._cedenteNome;
            else
                parms[6].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._cedenteAgencia))
                parms[7].Value = this._cedenteAgencia;
            else
                parms[7].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._cedenteDVAgencia))
                parms[8].Value = this._cedenteDVAgencia;
            else
                parms[8].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._cedenteNumeroConta))
                parms[9].Value = this._cedenteNumeroConta;
            else
                parms[9].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._cedenteDVConta))
                parms[10].Value = this._cedenteDVConta;
            else
                parms[10].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._cedenteCodigo))
                parms[11].Value = this._cedenteCodigo;
            else
                parms[11].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._sacadoNumCNPJCPF))
                parms[12].Value = this._sacadoNumCNPJCPF;
            else
                parms[12].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._sacadoNome))
                parms[13].Value = this._sacadoNome;
            else
                parms[13].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._sacadoEnderecoLogradouro))
                parms[14].Value = this._sacadoEnderecoLogradouro;
            else
                parms[14].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._sacadoEnderecoNumero))
                parms[15].Value = this._sacadoEnderecoNumero;
            else
                parms[15].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._sacadoEnderecoComplemento))
                parms[16].Value = this._sacadoEnderecoComplemento;
            else
                parms[16].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._sacadoEnderecoBairro))
                parms[17].Value = this._sacadoEnderecoBairro;
            else
                parms[17].Value = DBNull.Value;

            if (this._sacadoEnderecoCidade != null)
                parms[18].Value = this._sacadoEnderecoCidade.IdCidade;
            else
                parms[18].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._sacadoEnderecoCEP))
                parms[19].Value = this._sacadoEnderecoCEP;
            else
                parms[19].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._instrucaoDescricao))
                parms[20].Value = this._instrucaoDescricao;
            else
                parms[20].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._numeroNossoNumero))
                parms[21].Value = this._numeroNossoNumero;
            else
                parms[21].Value = DBNull.Value;

            parms[22].Value = this._valorBoleto;
            parms[23].Value = this._flagEmpresa;

            if (!String.IsNullOrEmpty(this._UrlBoleto))
                parms[24].Value = this._UrlBoleto;
            else
                parms[24].Value = DBNull.Value;

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
        /// Método utilizado para inserir uma instância de BoletoBancario no banco de dados.
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
                        this._idBoletoBancario = Convert.ToInt32(cmd.Parameters["@Idf_Boleto_Bancario"].Value);
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
        /// Método utilizado para inserir uma instância de BoletoBancario no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idBoletoBancario = Convert.ToInt32(cmd.Parameters["@Idf_Boleto_Bancario"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de BoletoBancario no banco de dados.
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
        /// Método utilizado para atualizar uma instância de BoletoBancario no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de BoletoBancario no banco de dados.
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
        /// Método utilizado para salvar uma instância de BoletoBancario no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de BoletoBancario no banco de dados.
        /// </summary>
        /// <param name="idBoletoBancario">Chave do registro.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idBoletoBancario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Boleto_Bancario", SqlDbType.Int, 4));

            parms[0].Value = idBoletoBancario;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de BoletoBancario no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idBoletoBancario">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idBoletoBancario, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Boleto_Bancario", SqlDbType.Int, 4));

            parms[0].Value = idBoletoBancario;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de BoletoBancario no banco de dados.
        /// </summary>
        /// <param name="idBoletoBancario">Lista de chaves.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(List<int> idBoletoBancario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Boleto_Bancario where Idf_Boleto_Bancario in (";

            for (int i = 0; i < idBoletoBancario.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idBoletoBancario[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idBoletoBancario">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idBoletoBancario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Boleto_Bancario", SqlDbType.Int, 4));

            parms[0].Value = idBoletoBancario;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idBoletoBancario">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idBoletoBancario, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Boleto_Bancario", SqlDbType.Int, 4));

            parms[0].Value = idBoletoBancario;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Bol.Idf_Boleto_Bancario, Bol.Idf_Pagamento, Bol.Idf_Banco, Bol.Dta_Emissao, Bol.Dta_Vencimento, Bol.Cedente_Numero_CNPJCPF, Bol.Cedente_Nome, Bol.Cedente_Agencia, Bol.Cedente_DV_Agencia, Bol.Cedente_Numero_Conta, Bol.Cedente_DVConta, Bol.Cedente_Codigo, Bol.Sacado_Numero_CNPJCPF, Bol.Sacado_Nome, Bol.Sacado_Endereco_Logradouro, Bol.Sacado_Endereco_Numero, Bol.Sacado_Endereco_Complemento, Bol.Sacado_Endereco_Bairro, Bol.Sacado_Endereco_Cidade, Bol.Sacado_Endereco_CEP, Bol.Instrucao_Descricao, Bol.Num_Nosso_Numero, Bol.Valor_Boleto, Bol.Flag_Empresa, Bol.Url_Boleto FROM BNE_Boleto_Bancario Bol";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de BoletoBancario a partir do banco de dados.
        /// </summary>
        /// <param name="idBoletoBancario">Chave do registro.</param>
        /// <returns>Instância de BoletoBancario.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static BoletoBancario LoadObject(int idBoletoBancario)
        {
            using (IDataReader dr = LoadDataReader(idBoletoBancario))
            {
                BoletoBancario objBoletoBancario = new BoletoBancario();
                if (SetInstance(dr, objBoletoBancario))
                    return objBoletoBancario;
            }
            throw (new RecordNotFoundException(typeof(BoletoBancario)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de BoletoBancario a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idBoletoBancario">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de BoletoBancario.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static BoletoBancario LoadObject(int idBoletoBancario, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idBoletoBancario, trans))
            {
                BoletoBancario objBoletoBancario = new BoletoBancario();
                if (SetInstance(dr, objBoletoBancario))
                    return objBoletoBancario;
            }
            throw (new RecordNotFoundException(typeof(BoletoBancario)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de BoletoBancario a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idBoletoBancario))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de BoletoBancario a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idBoletoBancario, trans))
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
        /// <param name="objBoletoBancario">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static bool SetInstance(IDataReader dr, BoletoBancario objBoletoBancario)
        {
            try
            {
                if (dr.Read())
                {
                    objBoletoBancario._idBoletoBancario = Convert.ToInt32(dr["Idf_Boleto_Bancario"]);
                    if (dr["Idf_Pagamento"] != DBNull.Value)
                        objBoletoBancario._pagamento = new Pagamento(Convert.ToInt32(dr["Idf_Pagamento"]));
                    if (dr["Idf_Banco"] != DBNull.Value)
                        objBoletoBancario._banco = new Banco(Convert.ToInt32(dr["Idf_Banco"]));
                    if (dr["Dta_Emissao"] != DBNull.Value)
                        objBoletoBancario._dataEmissao = Convert.ToDateTime(dr["Dta_Emissao"]);
                    if (dr["Dta_Vencimento"] != DBNull.Value)
                        objBoletoBancario._dataVencimento = Convert.ToDateTime(dr["Dta_Vencimento"]);
                    if (dr["Cedente_Numero_CNPJCPF"] != DBNull.Value)
                        objBoletoBancario._cedenteNumCNPJCPF = Convert.ToString(dr["Cedente_Numero_CNPJCPF"]);
                    if (dr["Cedente_Nome"] != DBNull.Value)
                        objBoletoBancario._cedenteNome = Convert.ToString(dr["Cedente_Nome"]);
                    if (dr["Cedente_Agencia"] != DBNull.Value)
                        objBoletoBancario._cedenteAgencia = Convert.ToString(dr["Cedente_Agencia"]);
                    if (dr["Cedente_DV_Agencia"] != DBNull.Value)
                        objBoletoBancario._cedenteDVAgencia = Convert.ToString(dr["Cedente_DV_Agencia"]);
                    if (dr["Cedente_Numero_Conta"] != DBNull.Value)
                        objBoletoBancario._cedenteNumeroConta = Convert.ToString(dr["Cedente_Numero_Conta"]);
                    if (dr["Cedente_DVConta"] != DBNull.Value)
                        objBoletoBancario._cedenteDVConta = Convert.ToString(dr["Cedente_DVConta"]);
                    if (dr["Cedente_Codigo"] != DBNull.Value)
                        objBoletoBancario._cedenteCodigo = Convert.ToString(dr["Cedente_Codigo"]);
                    if (dr["Sacado_Numero_CNPJCPF"] != DBNull.Value)
                        objBoletoBancario._sacadoNumCNPJCPF = Convert.ToString(dr["Sacado_Numero_CNPJCPF"]);
                    if (dr["Sacado_Nome"] != DBNull.Value)
                        objBoletoBancario._sacadoNome = Convert.ToString(dr["Sacado_Nome"]);
                    if (dr["Sacado_Endereco_Logradouro"] != DBNull.Value)
                        objBoletoBancario._sacadoEnderecoLogradouro = Convert.ToString(dr["Sacado_Endereco_Logradouro"]);
                    if (dr["Sacado_Endereco_Numero"] != DBNull.Value)
                        objBoletoBancario._sacadoEnderecoNumero = Convert.ToString(dr["Sacado_Endereco_Numero"]);
                    if (dr["Sacado_Endereco_Bairro"] != DBNull.Value)
                        objBoletoBancario._sacadoEnderecoBairro = Convert.ToString(dr["Sacado_Endereco_Bairro"]);
                    if (dr["Sacado_Endereco_Cidade"] != DBNull.Value)
                        objBoletoBancario._sacadoEnderecoCidade = new Cidade(Convert.ToInt32(dr["Sacado_Endereco_Cidade"]));
                    if (dr["Sacado_Endereco_CEP"] != DBNull.Value)
                        objBoletoBancario._sacadoEnderecoCEP = Convert.ToString(dr["Sacado_Endereco_CEP"]);
                    if (dr["Instrucao_Descricao"] != DBNull.Value)
                        objBoletoBancario._instrucaoDescricao = Convert.ToString(dr["Instrucao_Descricao"]);
                    if (dr["Num_Nosso_Numero"] != DBNull.Value)
                        objBoletoBancario._numeroNossoNumero = Convert.ToString(dr["Num_Nosso_Numero"]);
                    if (dr["Valor_Boleto"] != DBNull.Value)
                        objBoletoBancario._valorBoleto = Convert.ToDecimal(dr["Valor_Boleto"]);
                    objBoletoBancario._flagEmpresa = Convert.ToBoolean(dr["Flag_Empresa"]);

                    if (dr["Url_Boleto"] != DBNull.Value)
                        objBoletoBancario._UrlBoleto = Convert.ToString(dr["Url_Boleto"]);

                    objBoletoBancario._persisted = true;
                    objBoletoBancario._modified = false;

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
