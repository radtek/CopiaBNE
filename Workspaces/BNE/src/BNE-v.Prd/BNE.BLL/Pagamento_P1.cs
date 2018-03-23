//-- Data: 01/10/2014 15:13
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using System.ComponentModel.DataAnnotations;

namespace BNE.BLL
{
    public partial class Pagamento // Tabela: BNE_Pagamento
    {
        #region Atributos
        private int _idPagamento;
        private Filial _filial;
        private TipoPagamento _tipoPagamento;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private DateTime? _dataEmissao;
        private DateTime? _dataVencimento;
        private string _descricaoIdentificador;
        private string _descricaoDescricao;
        private decimal _valorPagamento;
        private DateTime _dataCadastro;
        private bool _flagAvulso;
        private bool _flagInativo;
        private PagamentoSituacao _pagamentoSituacao;
        private Operadora _operadora;
        private PlanoParcela _planoParcela;
        private UsuarioFilialPerfil _usuarioGerador;
        private string _codigoGuid;
        private AdicionalPlano _adicionalPlano;
        private string _numeroNotaFiscal;
        private CodigoDesconto _codigoDesconto;
        private bool _flagbaixado;
        private string _UrlNotaFiscal;
        private bool _flagNotaEnviada;
        private string _desOrdemDeCompra;
        private Boolean _flgJuros;
        private decimal _vlrJuros;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPagamento
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPagamento
        {
            get
            {
                return this._idPagamento;
            }
        }
        #endregion

        #region Filial
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Filial Filial
        {
            get
            {
                return this._filial;
            }
            set
            {
                this._filial = value;
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

        #region UsuarioFilialPerfil
        /// <summary>
        /// Campo obrigatório.
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

        #region DataEmissao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataEmissao
        {
            get
            {
                return this._dataEmissao;
            }
            set
            {
                this._dataEmissao = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoIdentificador
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo opcional.
        /// </summary>
        public string DescricaoIdentificador
        {
            get
            {
                return this._descricaoIdentificador;
            }
            set
            {
                this._descricaoIdentificador = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoDescricao
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        /// 
        [Display(Name = "IgnoreData")]
        public string DescricaoDescricao
        {
            get
            {
                return this._descricaoDescricao;
            }
            set
            {
                this._descricaoDescricao = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorPagamento
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public decimal ValorPagamento
        {
            get
            {
                return this._valorPagamento;
            }
            set
            {
                this._valorPagamento = value;
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

        #region FlagAvulso
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagAvulso
        {
            get
            {
                return this._flagAvulso;
            }
            set
            {
                this._flagAvulso = value;
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

        #region PagamentoSituacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PagamentoSituacao PagamentoSituacao
        {
            get
            {
                return this._pagamentoSituacao;
            }
            set
            {
                this._pagamentoSituacao = value;
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

        #region PlanoParcela
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public PlanoParcela PlanoParcela
        {
            get
            {
                return this._planoParcela;
            }
            set
            {
                this._planoParcela = value;
                this._modified = true;
            }
        }
        #endregion

        #region UsuarioGerador
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public UsuarioFilialPerfil UsuarioGerador
        {
            get
            {
                return this._usuarioGerador;
            }
            set
            {
                this._usuarioGerador = value;
                this._modified = true;
            }
        }
        #endregion

        #region CodigoGuid
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string CodigoGuid
        {
            get
            {
                return this._codigoGuid;
            }
            set
            {
                this._codigoGuid = value;
                this._modified = true;
            }
        }
        #endregion

        #region AdicionalPlano
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public AdicionalPlano AdicionalPlano
        {
            get
            {
                return this._adicionalPlano;
            }
            set
            {
                this._adicionalPlano = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroNotaFiscal
        /// <summary>
        /// Tamanho do campo: 10.
        /// Campo opcional.
        /// </summary>
        public string NumeroNotaFiscal
        {
            get
            {
                return this._numeroNotaFiscal;
            }
            set
            {
                this._numeroNotaFiscal = value;
                this._modified = true;
            }
        }
        #endregion

        #region CodigoDesconto
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public CodigoDesconto CodigoDesconto
        {
            get
            {
                return this._codigoDesconto;
            }
            set
            {
                this._codigoDesconto = value;
                this._modified = true;
            }
        }
        #endregion

        #region Flagbaixado
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagBaixado
        {
            get
            {
                return this._flagbaixado;
            }
            set
            {
                this._flagbaixado = value;
                this._modified = true;
            }
        }
        #endregion

        #region UrlNotaFiscal
        /// <summary>
        /// Tamanho do campo: 500.
        /// Campo opcional.
        /// </summary>
        public string UrlNotaFiscal
        {
            get
            {
                return this._UrlNotaFiscal;
            }
            set
            {
                this._UrlNotaFiscal = value;
                this._modified = true;
            }
        }
        #endregion

        #region Flagbaixado
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNotaEnviada
        {
            get
            {
                return this._flagNotaEnviada;
            }
            set
            {
                this._flagNotaEnviada = value;
                this._modified = true;
            }
        }
        #endregion

        #region OrdemDeCompra
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        /// 
        [Display(Name = "Ordem de Compra")]
        public string DesOrdemDeCompra
        {
            get
            {
                return this._desOrdemDeCompra;
            }
            set
            {
                this._desOrdemDeCompra = value;
                this._modified = true;
            }
        }
        #endregion

        #region Juros
        /// <summary>
        /// Indica cobrança de Juros na parcela
        /// </summary>
        [Display(Name = "Juros")]
        public Boolean FlagJuros {
            get { return _flgJuros; }
            set {
                _flgJuros = value;
                _modified = true;
                   
            }
        }
        #endregion


        #region ValorOriginal
        /// <summary>
        /// Indica o valor que esta sendo pago de Jutos
        /// </summary>
        [Display(Name = "Valor Juros")]
        public Decimal ValorJuros {
            get { return _vlrJuros; }
            set {
                _vlrJuros = value;
                _modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public Pagamento()
        {
        }
        public Pagamento(int idPagamento)
        {
            this._idPagamento = idPagamento;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Pagamento(    Idf_Filial,    Idf_Tipo_Pagamento,    Idf_Usuario_Filial_Perfil,    Dta_Emissao,    Dta_Vencimento,    Des_Identificador,    Des_Descricao,    Vlr_Pagamento,    Dta_Cadastro,    Flg_Avulso,    Flg_Inativo,    Idf_Pagamento_Situacao,    Idf_Operadora,    Idf_Plano_Parcela,    IDF_Usuario_Gerador,    Cod_Guid,    Idf_Adicional_Plano,    Num_Nota_Fiscal,    Idf_Codigo_Desconto,    Flg_baixado,    Url_Nota_Fiscal,    Flg_Nota_Enviada,    Des_Ordem_De_Compra,    Flg_juros, 	Vlr_Juros)VALUES(@Idf_Filial, @Idf_Tipo_Pagamento, @Idf_Usuario_Filial_Perfil, @Dta_Emissao, @Dta_Vencimento, @Des_Identificador, @Des_Descricao, @Vlr_Pagamento, @Dta_Cadastro, @Flg_Avulso, @Flg_Inativo, @Idf_Pagamento_Situacao, @Idf_Operadora, @Idf_Plano_Parcela, @IDF_Usuario_Gerador, @Cod_Guid, @Idf_Adicional_Plano, @Num_Nota_Fiscal, @Idf_Codigo_Desconto, @Flg_baixado, @Url_Nota_Fiscal, @Flg_Nota_Enviada, @Des_Ordem_De_Compra, @Flg_juros, @Vlr_Juros);SET @Idf_Pagamento = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Pagamento SET Idf_Filial = @Idf_Filial,    Idf_Tipo_Pagamento = @Idf_Tipo_Pagamento,    Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil,    Dta_Emissao = @Dta_Emissao,    Dta_Vencimento = @Dta_Vencimento,    Des_Identificador = @Des_Identificador,    Des_Descricao = @Des_Descricao,    Vlr_Pagamento = @Vlr_Pagamento,    Dta_Cadastro = @Dta_Cadastro,    Flg_Avulso = @Flg_Avulso,    Flg_Inativo = @Flg_Inativo,    Idf_Pagamento_Situacao = @Idf_Pagamento_Situacao,    Idf_Operadora = @Idf_Operadora,    Idf_Plano_Parcela = @Idf_Plano_Parcela,    IDF_Usuario_Gerador = @IDF_Usuario_Gerador,    Cod_Guid = @Cod_Guid,    Idf_Adicional_Plano = @Idf_Adicional_Plano,    Num_Nota_Fiscal = @Num_Nota_Fiscal,    Idf_Codigo_Desconto = @Idf_Codigo_Desconto,    Flg_baixado = @Flg_baixado,    Url_Nota_Fiscal = @Url_Nota_Fiscal,    Flg_Nota_Enviada = @Flg_Nota_Enviada,    Des_Ordem_De_Compra = @Des_Ordem_De_Compra,    Flg_juros = @Flg_juros, 	Vlr_Juros = @Vlr_Juros WHERE Idf_Pagamento = @Idf_Pagamento;";
        private const string SPDELETE = "UPDATE BNE_Pagamento SET Idf_Usuario_Gerador = @Idf_Usuario_Gerador WHERE Idf_Pagamento = @Idf_Pagamento ; DELETE FROM BNE_Pagamento WHERE Idf_Pagamento = @Idf_Pagamento";
        private const string SPSELECTID = "SELECT * FROM BNE_Pagamento WITH(NOLOCK) WHERE Idf_Pagamento = @Idf_Pagamento";
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
            parms.Add(new SqlParameter("@Idf_Pagamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Pagamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Emissao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Vencimento", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Des_Identificador", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Des_Descricao", SqlDbType.VarChar, 500));
            parms.Add(new SqlParameter("@Vlr_Pagamento", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Avulso", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Pagamento_Situacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Operadora", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Plano_Parcela", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@IDF_Usuario_Gerador", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Cod_Guid", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Idf_Adicional_Plano", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Nota_Fiscal", SqlDbType.VarChar, 10));
            parms.Add(new SqlParameter("@Idf_Codigo_Desconto", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_baixado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Url_Nota_Fiscal", SqlDbType.VarChar, 500));
            parms.Add(new SqlParameter("@Flg_Nota_Enviada", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Des_Ordem_De_Compra", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Flg_juros", SqlDbType.Bit,1));
            parms.Add(new SqlParameter("@Vlr_Juros", SqlDbType.Decimal, 9));

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
            parms[0].Value = this._idPagamento;

            if (this._filial != null)
                parms[1].Value = this._filial.IdFilial;
            else
                parms[1].Value = DBNull.Value;


            if (this._tipoPagamento != null)
                parms[2].Value = this._tipoPagamento.IdTipoPagamento;
            else
                parms[2].Value = DBNull.Value;

            parms[3].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;

            if (this._dataEmissao.HasValue)
                parms[4].Value = this._dataEmissao;
            else
                parms[4].Value = DBNull.Value;


            if (this._dataVencimento.HasValue)
                parms[5].Value = this._dataVencimento;
            else
                parms[5].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoIdentificador))
                parms[6].Value = this._descricaoIdentificador;
            else
                parms[6].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoDescricao))
                parms[7].Value = this._descricaoDescricao;
            else
                parms[7].Value = DBNull.Value;

            parms[8].Value = this._valorPagamento;
            parms[10].Value = this._flagAvulso;
            parms[11].Value = this._flagInativo;
            parms[12].Value = this._pagamentoSituacao.IdPagamentoSituacao;

            if (this._operadora != null)
                parms[13].Value = this._operadora.IdOperadora;
            else
                parms[13].Value = DBNull.Value;


            if (this._planoParcela != null)
                parms[14].Value = this._planoParcela.IdPlanoParcela;
            else
                parms[14].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._codigoGuid))
                parms[16].Value = this._codigoGuid;
            else
                parms[16].Value = DBNull.Value;


            if (this._adicionalPlano != null)
                parms[17].Value = this._adicionalPlano.IdAdicionalPlano;
            else
                parms[17].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroNotaFiscal))
                parms[18].Value = this._numeroNotaFiscal;
            else
                parms[18].Value = DBNull.Value;


            if (this._codigoDesconto != null)
                parms[19].Value = this._codigoDesconto.IdCodigoDesconto;
            else
                parms[19].Value = DBNull.Value;

            parms[20].Value = this._flagbaixado;

            if (!String.IsNullOrEmpty(this._UrlNotaFiscal))
                parms[21].Value = this._UrlNotaFiscal;
            else
                parms[21].Value = DBNull.Value;

            parms[22].Value = this._flagNotaEnviada;

            if (!String.IsNullOrEmpty(this._desOrdemDeCompra))
                parms[23].Value = this._desOrdemDeCompra;
            else
                parms[23].Value = DBNull.Value;

            parms[24].Value = this._flgJuros;

            parms[25].Value = this._vlrJuros;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[9].Value = this._dataCadastro;
            if (this._usuarioGerador != null)
                parms[15].Value = this._usuarioGerador.IdUsuarioFilialPerfil;
            else
                parms[15].Value = DBNull.Value;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de Pagamento no banco de dados.
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
                        this._idPagamento = Convert.ToInt32(cmd.Parameters["@Idf_Pagamento"].Value);
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
        /// Método utilizado para inserir uma instância de Pagamento no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPagamento = Convert.ToInt32(cmd.Parameters["@Idf_Pagamento"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Pagamento no banco de dados.
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
        /// Método utilizado para atualizar uma instância de Pagamento no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de Pagamento no banco de dados.
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
        /// Método utilizado para salvar uma instância de Pagamento no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de Pagamento no banco de dados.
        /// </summary>
        /// <param name="idPagamento">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPagamento)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pagamento", SqlDbType.Int, 4));

            parms[0].Value = idPagamento;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de Pagamento no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPagamento">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPagamento, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pagamento", SqlDbType.Int, 4));

            parms[0].Value = idPagamento;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de Pagamento no banco de dados.
        /// </summary>
        /// <param name="idPagamento">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idPagamento)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Pagamento where Idf_Pagamento in (";

            for (int i = 0; i < idPagamento.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPagamento[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPagamento">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPagamento)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pagamento", SqlDbType.Int, 4));

            parms[0].Value = idPagamento;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPagamento">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPagamento, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pagamento", SqlDbType.Int, 4));

            parms[0].Value = idPagamento;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pag.Idf_Pagamento, Pag.Idf_Filial, Pag.Idf_Tipo_Pagamento, Pag.Idf_Usuario_Filial_Perfil, Pag.Dta_Emissao, Pag.Dta_Vencimento, Pag.Des_Identificador, Pag.Des_Descricao, Pag.Vlr_Pagamento, Pag.Dta_Cadastro, Pag.Flg_Avulso, Pag.Flg_Inativo, Pag.Idf_Pagamento_Situacao, Pag.Idf_Operadora, Pag.Idf_Plano_Parcela, Pag.IDF_Usuario_Gerador, Pag.Cod_Guid, Pag.Idf_Adicional_Plano, Pag.Num_Nota_Fiscal, Pag.Idf_Codigo_Desconto, Pag.Flg_baixado, Pag.Url_Nota_Fiscal, Pag.Flg_Nota_Enviada FROM BNE_Pagamento Pag";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Pagamento a partir do banco de dados.
        /// </summary>
        /// <param name="idPagamento">Chave do registro.</param>
        /// <returns>Instância de Pagamento.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Pagamento LoadObject(int idPagamento)
        {
            using (IDataReader dr = LoadDataReader(idPagamento))
            {
                Pagamento objPagamento = new Pagamento();
                if (SetInstance(dr, objPagamento))
                    return objPagamento;
            }
            throw (new RecordNotFoundException(typeof(Pagamento)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Pagamento a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPagamento">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Pagamento.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Pagamento LoadObject(int idPagamento, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPagamento, trans))
            {
                Pagamento objPagamento = new Pagamento();
                if (SetInstance(dr, objPagamento))
                    return objPagamento;
            }
            throw (new RecordNotFoundException(typeof(Pagamento)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Pagamento a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPagamento))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Pagamento a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPagamento, trans))
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
        /// <param name="objPagamento">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, Pagamento objPagamento)
        {
            try
            {
                if (dr.Read())
                {
                    objPagamento._idPagamento = Convert.ToInt32(dr["Idf_Pagamento"]);

                    if (dr["Idf_Filial"] != DBNull.Value)
                        objPagamento._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                    if (dr["Idf_Tipo_Pagamento"] != DBNull.Value)
                        objPagamento._tipoPagamento = new TipoPagamento(Convert.ToInt32(dr["Idf_Tipo_Pagamento"]));
                    objPagamento._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    if (dr["Dta_Emissao"] != DBNull.Value)
                        objPagamento._dataEmissao = Convert.ToDateTime(dr["Dta_Emissao"]);
                    if (dr["Dta_Vencimento"] != DBNull.Value)
                        objPagamento._dataVencimento = Convert.ToDateTime(dr["Dta_Vencimento"]);
                    if (dr["Des_Identificador"] != DBNull.Value)
                        objPagamento._descricaoIdentificador = Convert.ToString(dr["Des_Identificador"]);
                    if (dr["Des_Descricao"] != DBNull.Value)
                        objPagamento._descricaoDescricao = Convert.ToString(dr["Des_Descricao"]);
                    objPagamento._valorPagamento = Convert.ToDecimal(dr["Vlr_Pagamento"]);
                    objPagamento._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objPagamento._flagAvulso = Convert.ToBoolean(dr["Flg_Avulso"]);
                    objPagamento._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objPagamento._flagNotaEnviada = Convert.ToBoolean(dr["Flg_Nota_Enviada"]);
                    objPagamento._pagamentoSituacao = new PagamentoSituacao(Convert.ToInt32(dr["Idf_Pagamento_Situacao"]));
                    if (dr["Idf_Operadora"] != DBNull.Value)
                        objPagamento._operadora = new Operadora(Convert.ToInt32(dr["Idf_Operadora"]));
                    if (dr["Idf_Plano_Parcela"] != DBNull.Value)
                        objPagamento._planoParcela = new PlanoParcela(Convert.ToInt32(dr["Idf_Plano_Parcela"]));
                    if (dr["IDF_Usuario_Gerador"] != DBNull.Value)
                        objPagamento._usuarioGerador = new UsuarioFilialPerfil(Convert.ToInt32(dr["IDF_Usuario_Gerador"]));
                    if (dr["Cod_Guid"] != DBNull.Value)
                        objPagamento._codigoGuid = Convert.ToString(dr["Cod_Guid"]);
                    if (dr["Idf_Adicional_Plano"] != DBNull.Value)
                        objPagamento._adicionalPlano = new AdicionalPlano(Convert.ToInt32(dr["Idf_Adicional_Plano"]));
                    if (dr["Num_Nota_Fiscal"] != DBNull.Value)
                        objPagamento._numeroNotaFiscal = Convert.ToString(dr["Num_Nota_Fiscal"]);
                    if (dr["Idf_Codigo_Desconto"] != DBNull.Value)
                        objPagamento._codigoDesconto = new CodigoDesconto(Convert.ToInt32(dr["Idf_Codigo_Desconto"]));
                    objPagamento._flagbaixado = Convert.ToBoolean(dr["Flg_baixado"]);
                    if (dr["Url_Nota_Fiscal"] != DBNull.Value)
                        objPagamento._UrlNotaFiscal = Convert.ToString(dr["Url_Nota_Fiscal"]);
                    if (dr["Des_Ordem_De_Compra"] != DBNull.Value)
                        objPagamento._desOrdemDeCompra = Convert.ToString(dr["Des_Ordem_De_Compra"]);
                    objPagamento._flgJuros = Convert.ToBoolean(dr["Flg_juros"] == DBNull.Value ? false : dr["Flg_juros"]);
                    objPagamento._vlrJuros = Convert.ToDecimal(dr["Vlr_Juros"] == DBNull.Value ? 0 : dr["Vlr_Juros"]);

                    objPagamento._persisted = true;
                    objPagamento._modified = false;

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