//-- Data: 19/01/2015 14:48
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class Transacao // Tabela: BNE_Transacao
    {
        #region Atributos
        private int _idTransacao;
        private DateTime _dataCadastro;
        private PlanoAdquirido _planoAdquirido;
        private TipoPagamento _tipoPagamento;
        private decimal _valorDocumento;
        private string _numeroCartaoCredito;
        private string _numeroMesValidadeCartaoCredito;
        private string _numeroAnoValidadeCartaoCredito;
        private string _numeroCodigoVerificadorCartaoCredito;
        private string _descricaoIPComprador;
        private Operadora _operadora;
        private int? _numeroDiaAgendamento;
        private int? _numeroMesesAgendamento;
        private int? _numeroTentativasNaoAprovadoAgendamento;
        private int? _numeroDiasEntreTentativasAgendamento;
        private Banco _banco;
        private string _descricaoAgenciaDebito;
        private string _descricaoContaCorrenteDebito;
        private string _nomeTitularContaCorrenteDebito;
        private decimal? _numeroCPFTitularContaCorrenteDebito;
        private string _descricaoTransacao;
        private string _descricaoMensagemCaptura;
        private StatusTransacao _statusTransacao;
        private decimal? _numeroCNPJTitularContaCorrenteDebito;
        private Pagamento _pagamento;
        private string _numeroVersaoCriptografia;
        private string _desCodigoToken;
        private string _nomeCartaoCredito;
        private int _gerenciadoraTransacao;
        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdTransacao
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdTransacao
        {
            get
            {
                return this._idTransacao;
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

        #region PlanoAdquirido
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public PlanoAdquirido PlanoAdquirido
        {
            get
            {
                return this._planoAdquirido;
            }
            set
            {
                this._planoAdquirido = value;
                this._modified = true;
            }
        }
        #endregion

        #region TipoPagamento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public TipoPagamento TipoPagamento
        {
            get
            {
                return this._tipoPagamento;
            }
            set
            {
                this._tipoPagamento = value;
                this._modified = true;
            }
        }
        #endregion

        public int GerenciadoraTransacao
        {
            get { return this._gerenciadoraTransacao; }
            set
            {
                this._gerenciadoraTransacao = value;
                this._modified = true;
            }
        }

        #region ValorDocumento
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public decimal ValorDocumento
        {
            get
            {
                return this._valorDocumento;
            }
            set
            {
                this._valorDocumento = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoIPComprador
        /// <summary>
        /// Tamanho do campo: 15.
        /// Campo opcional.
        /// </summary>
        public string DescricaoIPComprador
        {
            get
            {
                return this._descricaoIPComprador;
            }
            set
            {
                this._descricaoIPComprador = value;
                this._modified = true;
            }
        }
        #endregion

        #region Operadora
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Operadora Operadora
        {
            get
            {
                return this._operadora;
            }
            set
            {
                this._operadora = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDiaAgendamento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? NumeroDiaAgendamento
        {
            get
            {
                return this._numeroDiaAgendamento;
            }
            set
            {
                this._numeroDiaAgendamento = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroMesesAgendamento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? NumeroMesesAgendamento
        {
            get
            {
                return this._numeroMesesAgendamento;
            }
            set
            {
                this._numeroMesesAgendamento = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroTentativasNaoAprovadoAgendamento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? NumeroTentativasNaoAprovadoAgendamento
        {
            get
            {
                return this._numeroTentativasNaoAprovadoAgendamento;
            }
            set
            {
                this._numeroTentativasNaoAprovadoAgendamento = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDiasEntreTentativasAgendamento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? NumeroDiasEntreTentativasAgendamento
        {
            get
            {
                return this._numeroDiasEntreTentativasAgendamento;
            }
            set
            {
                this._numeroDiasEntreTentativasAgendamento = value;
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

        #region DescricaoAgenciaDebito
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo opcional.
        /// </summary>
        public string DescricaoAgenciaDebito
        {
            get
            {
                return this._descricaoAgenciaDebito;
            }
            set
            {
                this._descricaoAgenciaDebito = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoContaCorrenteDebito
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo opcional.
        /// </summary>
        public string DescricaoContaCorrenteDebito
        {
            get
            {
                return this._descricaoContaCorrenteDebito;
            }
            set
            {
                this._descricaoContaCorrenteDebito = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomeTitularContaCorrenteDebito
        /// <summary>
        /// Tamanho do campo: 40.
        /// Campo opcional.
        /// </summary>
        public string NomeTitularContaCorrenteDebito
        {
            get
            {
                return this._nomeTitularContaCorrenteDebito;
            }
            set
            {
                this._nomeTitularContaCorrenteDebito = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroCPFTitularContaCorrenteDebito
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? NumeroCPFTitularContaCorrenteDebito
        {
            get
            {
                return this._numeroCPFTitularContaCorrenteDebito;
            }
            set
            {
                this._numeroCPFTitularContaCorrenteDebito = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoTransacao
        /// <summary>
        /// Tamanho do campo: 50.
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

        #region DescricaoMensagemCaptura
        /// <summary>
        /// Tamanho do campo: 200.
        /// Campo opcional.
        /// </summary>
        public string DescricaoMensagemCaptura
        {
            get
            {
                return this._descricaoMensagemCaptura;
            }
            set
            {
                this._descricaoMensagemCaptura = value;
                this._modified = true;
            }
        }
        #endregion

        #region StatusTransacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public StatusTransacao StatusTransacao
        {
            get
            {
                return this._statusTransacao;
            }
            set
            {
                this._statusTransacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroCNPJTitularContaCorrenteDebito
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? NumeroCNPJTitularContaCorrenteDebito
        {
            get
            {
                return this._numeroCNPJTitularContaCorrenteDebito;
            }
            set
            {
                this._numeroCNPJTitularContaCorrenteDebito = value;
                this._modified = true;
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

        #region NumeroVersaoCriptografia
        /// <summary>
        /// Tamanho do campo: 10.
        /// Campo opcional.
        /// </summary>
        public string NumeroVersaoCriptografia
        {
            get
            {
                return this._numeroVersaoCriptografia;
            }
            set
            {
                this._numeroVersaoCriptografia = value;
                this._modified = true;
            }
        }
        #endregion

        #region DesCodigoToken
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string DesCodigoToken
        {
            get
            {
                return this._desCodigoToken;
            }
            set
            {
                this._desCodigoToken = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public Transacao()
        {
        }
        public Transacao(int idTransacao)
        {
            this._idTransacao = idTransacao;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Transacao (Dta_Cadastro, Idf_Plano_Adquirido, Idf_Tipo_Pagamento, Vlr_Documento, Num_Cartao_Credito, Num_Mes_Validade_Cartao_Credito, Num_Ano_Validade_Cartao_Credito, Num_Codigo_Verificador_Cartao_Credito, Des_IP_Comprador, Idf_Operadora, Num_Dia_Agendamento, Num_Meses_Agendamento, Num_Tentativas_Nao_Aprovado_Agendamento, Num_Dias_Entre_Tentativas_Agendamento, Idf_Banco, Des_Agencia_Debito, Des_Conta_Corrente_Debito, Nme_Titular_Conta_Corrente_Debito, Num_CPF_Titular_Conta_Corrente_Debito, Des_Transacao, Des_Mensagem_Captura, Idf_Status_Transacao, Num_CNPJ_Titular_Conta_Corrente_Debito, Idf_Pagamento, Num_Versao_Criptografia, Des_Codigo_Token, Nome_Cartao_Credito, Gerenciadora_Transacao) VALUES (@Dta_Cadastro, @Idf_Plano_Adquirido, @Idf_Tipo_Pagamento, @Vlr_Documento, @Num_Cartao_Credito, @Num_Mes_Validade_Cartao_Credito, @Num_Ano_Validade_Cartao_Credito, @Num_Codigo_Verificador_Cartao_Credito, @Des_IP_Comprador, @Idf_Operadora, @Num_Dia_Agendamento, @Num_Meses_Agendamento, @Num_Tentativas_Nao_Aprovado_Agendamento, @Num_Dias_Entre_Tentativas_Agendamento, @Idf_Banco, @Des_Agencia_Debito, @Des_Conta_Corrente_Debito, @Nme_Titular_Conta_Corrente_Debito, @Num_CPF_Titular_Conta_Corrente_Debito, @Des_Transacao, @Des_Mensagem_Captura, @Idf_Status_Transacao, @Num_CNPJ_Titular_Conta_Corrente_Debito, @Idf_Pagamento, @Num_Versao_Criptografia, @Des_Codigo_Token, @Nome_Cartao_Credito, @Gerenciadora_Transacao);SET @Idf_Transacao = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Transacao SET Dta_Cadastro = @Dta_Cadastro, Idf_Plano_Adquirido = @Idf_Plano_Adquirido, Idf_Tipo_Pagamento = @Idf_Tipo_Pagamento, Vlr_Documento = @Vlr_Documento, Num_Cartao_Credito = @Num_Cartao_Credito, Num_Mes_Validade_Cartao_Credito = @Num_Mes_Validade_Cartao_Credito, Num_Ano_Validade_Cartao_Credito = @Num_Ano_Validade_Cartao_Credito, Num_Codigo_Verificador_Cartao_Credito = @Num_Codigo_Verificador_Cartao_Credito, Des_IP_Comprador = @Des_IP_Comprador, Idf_Operadora = @Idf_Operadora, Num_Dia_Agendamento = @Num_Dia_Agendamento, Num_Meses_Agendamento = @Num_Meses_Agendamento, Num_Tentativas_Nao_Aprovado_Agendamento = @Num_Tentativas_Nao_Aprovado_Agendamento, Num_Dias_Entre_Tentativas_Agendamento = @Num_Dias_Entre_Tentativas_Agendamento, Idf_Banco = @Idf_Banco, Des_Agencia_Debito = @Des_Agencia_Debito, Des_Conta_Corrente_Debito = @Des_Conta_Corrente_Debito, Nme_Titular_Conta_Corrente_Debito = @Nme_Titular_Conta_Corrente_Debito, Num_CPF_Titular_Conta_Corrente_Debito = @Num_CPF_Titular_Conta_Corrente_Debito, Des_Transacao = @Des_Transacao, Des_Mensagem_Captura = @Des_Mensagem_Captura, Idf_Status_Transacao = @Idf_Status_Transacao, Num_CNPJ_Titular_Conta_Corrente_Debito = @Num_CNPJ_Titular_Conta_Corrente_Debito, Idf_Pagamento = @Idf_Pagamento, Num_Versao_Criptografia = @Num_Versao_Criptografia, Des_Codigo_Token = @Des_Codigo_Token, Nome_Cartao_Credito = @Nome_Cartao_Credito, Gerenciadora_Transacao = @Gerenciadora_Transacao  WHERE Idf_Transacao = @Idf_Transacao";
        private const string SPDELETE = "DELETE FROM BNE_Transacao WHERE Idf_Transacao = @Idf_Transacao";
        private const string SPSELECTID = "SELECT * FROM BNE_Transacao WITH(NOLOCK) WHERE Idf_Transacao = @Idf_Transacao";
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
            parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Pagamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Vlr_Documento", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Num_Cartao_Credito", SqlDbType.VarChar, 88));
            parms.Add(new SqlParameter("@Num_Mes_Validade_Cartao_Credito", SqlDbType.VarChar, 64));
            parms.Add(new SqlParameter("@Num_Ano_Validade_Cartao_Credito", SqlDbType.VarChar, 64));
            parms.Add(new SqlParameter("@Num_Codigo_Verificador_Cartao_Credito", SqlDbType.VarChar, 64));
            parms.Add(new SqlParameter("@Des_IP_Comprador", SqlDbType.VarChar, 15));
            parms.Add(new SqlParameter("@Idf_Operadora", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Dia_Agendamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Meses_Agendamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Tentativas_Nao_Aprovado_Agendamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Dias_Entre_Tentativas_Agendamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Banco", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Agencia_Debito", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Des_Conta_Corrente_Debito", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Nme_Titular_Conta_Corrente_Debito", SqlDbType.VarChar, 40));
            parms.Add(new SqlParameter("@Num_CPF_Titular_Conta_Corrente_Debito", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Des_Transacao", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Des_Mensagem_Captura", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Idf_Status_Transacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_CNPJ_Titular_Conta_Corrente_Debito", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Idf_Pagamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Versao_Criptografia", SqlDbType.VarChar, 10));
            parms.Add(new SqlParameter("@Des_Codigo_Token", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Nome_Cartao_Credito", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Gerenciadora_Transacao", SqlDbType.Int, 2));

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
            parms[0].Value = this._idTransacao;

            if (this._planoAdquirido != null)
                parms[2].Value = this._planoAdquirido.IdPlanoAdquirido;
            else
                parms[2].Value = DBNull.Value;


            if (this._tipoPagamento != null)
                parms[3].Value = this._tipoPagamento.IdTipoPagamento;
            else
                parms[3].Value = DBNull.Value;

            parms[4].Value = this._valorDocumento;

            if (!String.IsNullOrEmpty(this._numeroCartaoCredito))
                parms[5].Value = this._numeroCartaoCredito;
            else
                parms[5].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroMesValidadeCartaoCredito))
                parms[6].Value = this._numeroMesValidadeCartaoCredito;
            else
                parms[6].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroAnoValidadeCartaoCredito))
                parms[7].Value = this._numeroAnoValidadeCartaoCredito;
            else
                parms[7].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroCodigoVerificadorCartaoCredito))
                parms[8].Value = this._numeroCodigoVerificadorCartaoCredito;
            else
                parms[8].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoIPComprador))
                parms[9].Value = this._descricaoIPComprador;
            else
                parms[9].Value = DBNull.Value;


            if (this._operadora != null)
                parms[10].Value = this._operadora.IdOperadora;
            else
                parms[10].Value = DBNull.Value;


            if (this._numeroDiaAgendamento.HasValue)
                parms[11].Value = this._numeroDiaAgendamento;
            else
                parms[11].Value = DBNull.Value;


            if (this._numeroMesesAgendamento.HasValue)
                parms[12].Value = this._numeroMesesAgendamento;
            else
                parms[12].Value = DBNull.Value;


            if (this._numeroTentativasNaoAprovadoAgendamento.HasValue)
                parms[13].Value = this._numeroTentativasNaoAprovadoAgendamento;
            else
                parms[13].Value = DBNull.Value;


            if (this._numeroDiasEntreTentativasAgendamento.HasValue)
                parms[14].Value = this._numeroDiasEntreTentativasAgendamento;
            else
                parms[14].Value = DBNull.Value;


            if (this._banco != null)
                parms[15].Value = this._banco.IdBanco;
            else
                parms[15].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoAgenciaDebito))
                parms[16].Value = this._descricaoAgenciaDebito;
            else
                parms[16].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoContaCorrenteDebito))
                parms[17].Value = this._descricaoContaCorrenteDebito;
            else
                parms[17].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._nomeTitularContaCorrenteDebito))
                parms[18].Value = this._nomeTitularContaCorrenteDebito;
            else
                parms[18].Value = DBNull.Value;


            if (this._numeroCPFTitularContaCorrenteDebito.HasValue)
                parms[19].Value = this._numeroCPFTitularContaCorrenteDebito;
            else
                parms[19].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoTransacao))
                parms[20].Value = this._descricaoTransacao;
            else
                parms[20].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoMensagemCaptura))
                parms[21].Value = this._descricaoMensagemCaptura;
            else
                parms[21].Value = DBNull.Value;

            parms[22].Value = this._statusTransacao.IdStatusTransacao;

            if (this._numeroCNPJTitularContaCorrenteDebito.HasValue)
                parms[23].Value = this._numeroCNPJTitularContaCorrenteDebito;
            else
                parms[23].Value = DBNull.Value;


            if (this._pagamento != null)
                parms[24].Value = this._pagamento.IdPagamento;
            else
                parms[24].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroVersaoCriptografia))
                parms[25].Value = this._numeroVersaoCriptografia;
            else
                parms[25].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._desCodigoToken))
                parms[26].Value = this._desCodigoToken;
            else
                parms[26].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(this._nomeCartaoCredito))
                parms[27].Value = this._nomeCartaoCredito;
            else
                parms[27].Value = DBNull.Value;

            if (this._gerenciadoraTransacao != null)
                parms[28].Value = this._gerenciadoraTransacao;
            else
                parms[28].Value = DBNull.Value;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[1].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de Transacao no banco de dados.
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
                        this._idTransacao = Convert.ToInt32(cmd.Parameters["@Idf_Transacao"].Value);
                        cmd.Parameters.Clear();
                        this._persisted = true;
                        this._modified = false;
                        trans.Commit();
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// Método utilizado para inserir uma instância de Transacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idTransacao = Convert.ToInt32(cmd.Parameters["@Idf_Transacao"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Transacao no banco de dados.
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
        /// Método utilizado para atualizar uma instância de Transacao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de Transacao no banco de dados.
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
        /// Método utilizado para salvar uma instância de Transacao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de Transacao no banco de dados.
        /// </summary>
        /// <param name="idTransacao">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idTransacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));

            parms[0].Value = idTransacao;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de Transacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idTransacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idTransacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));

            parms[0].Value = idTransacao;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de Transacao no banco de dados.
        /// </summary>
        /// <param name="idTransacao">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idTransacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Transacao where Idf_Transacao in (";

            for (int i = 0; i < idTransacao.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idTransacao[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idTransacao">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idTransacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));

            parms[0].Value = idTransacao;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idTransacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idTransacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));

            parms[0].Value = idTransacao;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tra.Idf_Transacao, Tra.Dta_Cadastro, Tra.Idf_Plano_Adquirido, Tra.Idf_Tipo_Pagamento, Tra.Vlr_Documento, Tra.Num_Cartao_Credito, Tra.Num_Mes_Validade_Cartao_Credito, Tra.Num_Ano_Validade_Cartao_Credito, Tra.Num_Codigo_Verificador_Cartao_Credito, Tra.Des_IP_Comprador, Tra.Idf_Operadora, Tra.Num_Dia_Agendamento, Tra.Num_Meses_Agendamento, Tra.Num_Tentativas_Nao_Aprovado_Agendamento, Tra.Num_Dias_Entre_Tentativas_Agendamento, Tra.Idf_Banco, Tra.Des_Agencia_Debito, Tra.Des_Conta_Corrente_Debito, Tra.Nme_Titular_Conta_Corrente_Debito, Tra.Num_CPF_Titular_Conta_Corrente_Debito, Tra.Des_Transacao, Tra.Des_Mensagem_Captura, Tra.Idf_Status_Transacao, Tra.Num_CNPJ_Titular_Conta_Corrente_Debito, Tra.Idf_Pagamento, Tra.Num_Versao_Criptografia, Tra.Nome_Cartao_Credito, Tra.Gerenciadora_Transacao FROM BNE_Transacao Tra";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Transacao a partir do banco de dados.
        /// </summary>
        /// <param name="idTransacao">Chave do registro.</param>
        /// <returns>Instância de Transacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Transacao LoadObject(int idTransacao)
        {
            using (IDataReader dr = LoadDataReader(idTransacao))
            {
                Transacao objTransacao = new Transacao();
                if (SetInstance(dr, objTransacao))
                    return objTransacao;
            }
            throw (new RecordNotFoundException(typeof(Transacao)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Transacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idTransacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Transacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Transacao LoadObject(int idTransacao, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idTransacao, trans))
            {
                Transacao objTransacao = new Transacao();
                if (SetInstance(dr, objTransacao))
                    return objTransacao;
            }
            throw (new RecordNotFoundException(typeof(Transacao)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Transacao a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idTransacao))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Transacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idTransacao, trans))
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
        /// <param name="objTransacao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, Transacao objTransacao)
        {
            try
            {
                if (dr.Read())
                {
                    objTransacao._idTransacao = Convert.ToInt32(dr["Idf_Transacao"]);
                    objTransacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Idf_Plano_Adquirido"] != DBNull.Value)
                        objTransacao._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
                    if (dr["Idf_Tipo_Pagamento"] != DBNull.Value)
                        objTransacao._tipoPagamento = new TipoPagamento(Convert.ToInt32(dr["Idf_Tipo_Pagamento"]));
                    objTransacao._valorDocumento = Convert.ToDecimal(dr["Vlr_Documento"]);
                    if (dr["Num_Cartao_Credito"] != DBNull.Value)
                        objTransacao._numeroCartaoCredito = Convert.ToString(dr["Num_Cartao_Credito"]);
                    if (dr["Num_Mes_Validade_Cartao_Credito"] != DBNull.Value)
                        objTransacao._numeroMesValidadeCartaoCredito = Convert.ToString(dr["Num_Mes_Validade_Cartao_Credito"]);
                    if (dr["Num_Ano_Validade_Cartao_Credito"] != DBNull.Value)
                        objTransacao._numeroAnoValidadeCartaoCredito = Convert.ToString(dr["Num_Ano_Validade_Cartao_Credito"]);
                    if (dr["Num_Codigo_Verificador_Cartao_Credito"] != DBNull.Value)
                        objTransacao._numeroCodigoVerificadorCartaoCredito = Convert.ToString(dr["Num_Codigo_Verificador_Cartao_Credito"]);
                    if (dr["Des_IP_Comprador"] != DBNull.Value)
                        objTransacao._descricaoIPComprador = Convert.ToString(dr["Des_IP_Comprador"]);
                    if (dr["Idf_Operadora"] != DBNull.Value)
                        objTransacao._operadora = new Operadora(Convert.ToInt32(dr["Idf_Operadora"]));
                    if (dr["Num_Dia_Agendamento"] != DBNull.Value)
                        objTransacao._numeroDiaAgendamento = Convert.ToInt32(dr["Num_Dia_Agendamento"]);
                    if (dr["Num_Meses_Agendamento"] != DBNull.Value)
                        objTransacao._numeroMesesAgendamento = Convert.ToInt32(dr["Num_Meses_Agendamento"]);
                    if (dr["Num_Tentativas_Nao_Aprovado_Agendamento"] != DBNull.Value)
                        objTransacao._numeroTentativasNaoAprovadoAgendamento = Convert.ToInt32(dr["Num_Tentativas_Nao_Aprovado_Agendamento"]);
                    if (dr["Num_Dias_Entre_Tentativas_Agendamento"] != DBNull.Value)
                        objTransacao._numeroDiasEntreTentativasAgendamento = Convert.ToInt32(dr["Num_Dias_Entre_Tentativas_Agendamento"]);
                    if (dr["Idf_Banco"] != DBNull.Value)
                        objTransacao._banco = new Banco(Convert.ToInt32(dr["Idf_Banco"]));
                    if (dr["Des_Agencia_Debito"] != DBNull.Value)
                        objTransacao._descricaoAgenciaDebito = Convert.ToString(dr["Des_Agencia_Debito"]);
                    if (dr["Des_Conta_Corrente_Debito"] != DBNull.Value)
                        objTransacao._descricaoContaCorrenteDebito = Convert.ToString(dr["Des_Conta_Corrente_Debito"]);
                    if (dr["Nme_Titular_Conta_Corrente_Debito"] != DBNull.Value)
                        objTransacao._nomeTitularContaCorrenteDebito = Convert.ToString(dr["Nme_Titular_Conta_Corrente_Debito"]);
                    if (dr["Num_CPF_Titular_Conta_Corrente_Debito"] != DBNull.Value)
                        objTransacao._numeroCPFTitularContaCorrenteDebito = Convert.ToDecimal(dr["Num_CPF_Titular_Conta_Corrente_Debito"]);
                    if (dr["Des_Transacao"] != DBNull.Value)
                        objTransacao._descricaoTransacao = Convert.ToString(dr["Des_Transacao"]);
                    if (dr["Des_Mensagem_Captura"] != DBNull.Value)
                        objTransacao._descricaoMensagemCaptura = Convert.ToString(dr["Des_Mensagem_Captura"]);
                    objTransacao._statusTransacao = new StatusTransacao(Convert.ToInt32(dr["Idf_Status_Transacao"]));
                    if (dr["Num_CNPJ_Titular_Conta_Corrente_Debito"] != DBNull.Value)
                        objTransacao._numeroCNPJTitularContaCorrenteDebito = Convert.ToDecimal(dr["Num_CNPJ_Titular_Conta_Corrente_Debito"]);
                    if (dr["Idf_Pagamento"] != DBNull.Value)
                        objTransacao._pagamento = new Pagamento(Convert.ToInt32(dr["Idf_Pagamento"]));
                    if (dr["Num_Versao_Criptografia"] != DBNull.Value)
                        objTransacao._numeroVersaoCriptografia = Convert.ToString(dr["Num_Versao_Criptografia"]);
                    if (dr["Des_Codigo_Token"] != DBNull.Value)
                        objTransacao._desCodigoToken = Convert.ToString(dr["Des_Codigo_Token"]);
                    if (dr["Nome_Cartao_Credito"] != DBNull.Value)
                    {
                        objTransacao._nomeCartaoCredito = Convert.ToString(dr["Nome_Cartao_Credito"]);
                    }
                    if (dr["gerenciadora_Transacao"] != DBNull.Value)
                    {
                        objTransacao._gerenciadoraTransacao = Convert.ToInt32(dr["gerenciadora_Transacao"]);
                    }



                    objTransacao._persisted = true;
                    objTransacao._modified = false;

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