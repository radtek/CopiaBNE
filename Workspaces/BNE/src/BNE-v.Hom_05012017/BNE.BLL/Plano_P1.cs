//-- Data: 16/03/2016 10:36
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class Plano // Tabela: BNE_Plano
    {
        #region Atributos
        private int _idPlano;
        private string _descricaoPlano;
        private int _quantidadeDiasValidade;
        private int _quantidadeSMS;
        private int _quantidadeVisualizacao;
        private DateTime _dataCadastro;
        private bool _flagInativo;
        private decimal _valorBase;
        private decimal _valorDe;
        private DateTime? _dataInicio;
        private DateTime? _dataFinal;
        private PlanoTipo _planoTipo;
        private PlanoFormaPagamento _planoFormaPagamento;
        private int _quantidadeParcela;
        private int _valorDescontoMaximo;
        private int? _quantidadeSMSMaxima;
        private decimal? _valorBaseMinimo;
        private int? _quantidadePrazoBoletoMaxima;
        private bool _flagBoletoRegistrado;
        private TipoContrato _tipoContrato;
        private bool _flagEnviarContrato;
        private bool _flagHabilitaVendaPersonalizada;
        private bool _flagLiberaUsuariosTanque;
        private bool _flagIlimitado;
        private bool _flagRecorrente;
        private Int16 _quantidadeCampanha;
        private bool _flgBoletoRecorrente;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPlano
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPlano
        {
            get
            {
                return this._idPlano;
            }
        }
        #endregion

        #region QuantidadeDiasValidade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int QuantidadeDiasValidade
        {
            get
            {
                return this._quantidadeDiasValidade;
            }
            set
            {
                this._quantidadeDiasValidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeSMS
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int QuantidadeSMS
        {
            get
            {
                return this._quantidadeSMS;
            }
            set
            {
                this._quantidadeSMS = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeVisualizacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int QuantidadeVisualizacao
        {
            get
            {
                return this._quantidadeVisualizacao;
            }
            set
            {
                this._quantidadeVisualizacao = value;
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

        #region ValorBase
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public decimal ValorBase
        {
            get
            {
                return this._valorBase;
            }
            set
            {
                this._valorBase = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorDe
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public decimal ValorDe
        {
            get
            {
                return this._valorDe;
            }
            set
            {
                this._valorDe = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataInicio
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataInicio
        {
            get
            {
                return this._dataInicio;
            }
            set
            {
                this._dataInicio = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataFinal
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataFinal
        {
            get
            {
                return this._dataFinal;
            }
            set
            {
                this._dataFinal = value;
                this._modified = true;
            }
        }
        #endregion

        #region PlanoTipo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PlanoTipo PlanoTipo
        {
            get
            {
                return this._planoTipo;
            }
            set
            {
                this._planoTipo = value;
                this._modified = true;
            }
        }
        #endregion

        #region PlanoFormaPagamento
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PlanoFormaPagamento PlanoFormaPagamento
        {
            get
            {
                return this._planoFormaPagamento;
            }
            set
            {
                this._planoFormaPagamento = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeParcela
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int QuantidadeParcela
        {
            get
            {
                return this._quantidadeParcela;
            }
            set
            {
                this._quantidadeParcela = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorDescontoMaximo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int ValorDescontoMaximo
        {
            get
            {
                return this._valorDescontoMaximo;
            }
            set
            {
                this._valorDescontoMaximo = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeSMSMaxima
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? QuantidadeSMSMaxima
        {
            get
            {
                return this._quantidadeSMSMaxima;
            }
            set
            {
                this._quantidadeSMSMaxima = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorBaseMinimo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? ValorBaseMinimo
        {
            get
            {
                return this._valorBaseMinimo;
            }
            set
            {
                this._valorBaseMinimo = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadePrazoBoletoMaxima
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? QuantidadePrazoBoletoMaxima
        {
            get
            {
                return this._quantidadePrazoBoletoMaxima;
            }
            set
            {
                this._quantidadePrazoBoletoMaxima = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagBoletoRegistrado
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagBoletoRegistrado
        {
            get
            {
                return this._flagBoletoRegistrado;
            }
            set
            {
                this._flagBoletoRegistrado = value;
                this._modified = true;
            }
        }
        #endregion

        #region TipoContrato
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public TipoContrato TipoContrato
        {
            get
            {
                return this._tipoContrato;
            }
            set
            {
                this._tipoContrato = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagEnviarContrato
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagEnviarContrato
        {
            get
            {
                return this._flagEnviarContrato;
            }
            set
            {
                this._flagEnviarContrato = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagHabilitaVendaPersonalizada
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagHabilitaVendaPersonalizada
        {
            get
            {
                return this._flagHabilitaVendaPersonalizada;
            }
            set
            {
                this._flagHabilitaVendaPersonalizada = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagLiberaUsuariosTanque
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagLiberaUsuariosTanque
        {
            get
            {
                return this._flagLiberaUsuariosTanque;
            }
            set
            {
                this._flagLiberaUsuariosTanque = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagIlimitado
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagIlimitado
        {
            get
            {
                return this._flagIlimitado;
            }
            set
            {
                this._flagIlimitado = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagRecorrente
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagRecorrente
        {
            get
            {
                return this._flagRecorrente;
            }
            set
            {
                this._flagRecorrente = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeCampanha
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Int16 QuantidadeCampanha
        {
            get
            {
                return this._quantidadeCampanha;
            }
            set
            {
                this._quantidadeCampanha = value;
                this._modified = true;
            }
        }
        #endregion

        public bool FlagBoletoRecorrente {
            get {
                return this._flgBoletoRecorrente;
            }
            set {
                this._flgBoletoRecorrente = value;
                this._modified = true;
            }
        }

        #endregion

        #region Construtores
        public Plano()
        {
        }
        public Plano(int idPlano)
        {
            this._idPlano = idPlano;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Plano (Des_Plano, Qtd_Dias_Validade, Qtd_SMS, Qtd_Visualizacao, Dta_Cadastro, Flg_Inativo, Vlr_Base, Dta_Inicio, Dta_Final, Idf_Plano_Tipo, Idf_Plano_Forma_Pagamento, Qtd_Parcela, Vlr_Desconto_Maximo, Qtd_SMS_Maxima, Vlr_Base_Minimo, Qtd_Prazo_Boleto_Maxima, Flg_Boleto_Registrado, Idf_Tipo_Contrato, Flg_Enviar_Contrato, Flg_Habilita_Venda_Personalizada, Flg_Libera_Usuarios_Tanque, Flg_Ilimitado, Flg_Recorrente, Qtd_Campanha) VALUES (@Des_Plano, @Qtd_Dias_Validade, @Qtd_SMS, @Qtd_Visualizacao, @Dta_Cadastro, @Flg_Inativo, @Vlr_Base, @Dta_Inicio, @Dta_Final, @Idf_Plano_Tipo, @Idf_Plano_Forma_Pagamento, @Qtd_Parcela, @Vlr_Desconto_Maximo, @Qtd_SMS_Maxima, @Vlr_Base_Minimo, @Qtd_Prazo_Boleto_Maxima, @Flg_Boleto_Registrado, @Idf_Tipo_Contrato, @Flg_Enviar_Contrato, @Flg_Habilita_Venda_Personalizada, @Flg_Libera_Usuarios_Tanque, @Flg_Ilimitado, @Flg_Recorrente, @Qtd_Campanha);SET @Idf_Plano = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Plano SET Des_Plano = @Des_Plano, Qtd_Dias_Validade = @Qtd_Dias_Validade, Qtd_SMS = @Qtd_SMS, Qtd_Visualizacao = @Qtd_Visualizacao, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Vlr_Base = @Vlr_Base, Dta_Inicio = @Dta_Inicio, Dta_Final = @Dta_Final, Idf_Plano_Tipo = @Idf_Plano_Tipo, Idf_Plano_Forma_Pagamento = @Idf_Plano_Forma_Pagamento, Qtd_Parcela = @Qtd_Parcela, Vlr_Desconto_Maximo = @Vlr_Desconto_Maximo, Qtd_SMS_Maxima = @Qtd_SMS_Maxima, Vlr_Base_Minimo = @Vlr_Base_Minimo, Qtd_Prazo_Boleto_Maxima = @Qtd_Prazo_Boleto_Maxima, Flg_Boleto_Registrado = @Flg_Boleto_Registrado, Idf_Tipo_Contrato = @Idf_Tipo_Contrato, Flg_Enviar_Contrato = @Flg_Enviar_Contrato, Flg_Habilita_Venda_Personalizada = @Flg_Habilita_Venda_Personalizada, Flg_Libera_Usuarios_Tanque = @Flg_Libera_Usuarios_Tanque, Flg_Ilimitado = @Flg_Ilimitado, Flg_Recorrente = @Flg_Recorrente, Qtd_Campanha = @Qtd_Campanha WHERE Idf_Plano = @Idf_Plano";
        private const string SPDELETE = "DELETE FROM BNE_Plano WHERE Idf_Plano = @Idf_Plano";
        private const string SPSELECTID = "SELECT * FROM BNE_Plano WITH(NOLOCK) WHERE Idf_Plano = @Idf_Plano";
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
            parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Plano", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Qtd_Dias_Validade", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_SMS", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_Visualizacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Vlr_Base", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Dta_Inicio", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Final", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Plano_Tipo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Plano_Forma_Pagamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_Parcela", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Vlr_Desconto_Maximo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_SMS_Maxima", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Vlr_Base_Minimo", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Qtd_Prazo_Boleto_Maxima", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Boleto_Registrado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Tipo_Contrato", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Enviar_Contrato", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Habilita_Venda_Personalizada", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Libera_Usuarios_Tanque", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Ilimitado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Recorrente", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Qtd_Campanha", SqlDbType.Int, 1));
            parms.Add(new SqlParameter("@Vlr_De", SqlDbType.Decimal, 9));
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
            parms[0].Value = this._idPlano;
            parms[1].Value = this._descricaoPlano;
            parms[2].Value = this._quantidadeDiasValidade;
            parms[3].Value = this._quantidadeSMS;
            parms[4].Value = this._quantidadeVisualizacao;
            parms[6].Value = this._flagInativo;
            parms[7].Value = this._valorBase;
            


            if (this._dataInicio.HasValue)
                parms[8].Value = this._dataInicio;
            else
                parms[8].Value = DBNull.Value;


            if (this._dataFinal.HasValue)
                parms[9].Value = this._dataFinal;
            else
                parms[9].Value = DBNull.Value;

            parms[10].Value = this._planoTipo.IdPlanoTipo;
            parms[11].Value = this._planoFormaPagamento.IdPlanoFormaPagamento;
            parms[12].Value = this._quantidadeParcela;
            parms[13].Value = this._valorDescontoMaximo;

            if (this._quantidadeSMSMaxima.HasValue)
                parms[14].Value = this._quantidadeSMSMaxima;
            else
                parms[14].Value = DBNull.Value;


            if (this._valorBaseMinimo.HasValue)
                parms[15].Value = this._valorBaseMinimo;
            else
                parms[15].Value = DBNull.Value;


            if (this._quantidadePrazoBoletoMaxima.HasValue)
                parms[16].Value = this._quantidadePrazoBoletoMaxima;
            else
                parms[16].Value = DBNull.Value;

            parms[17].Value = this._flagBoletoRegistrado;

            if (this._tipoContrato != null)
                parms[18].Value = this._tipoContrato.IdTipoContrato;
            else
                parms[18].Value = DBNull.Value;

            parms[19].Value = this._flagEnviarContrato;
            parms[20].Value = this._flagHabilitaVendaPersonalizada;
            parms[21].Value = this._flagLiberaUsuariosTanque;
            parms[22].Value = this._flagIlimitado;
            parms[23].Value = this._flagRecorrente;
            parms[24].Value = this._quantidadeCampanha;
            parms[25].Value = this._valorDe;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[5].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de Plano no banco de dados.
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
                        this._idPlano = Convert.ToInt32(cmd.Parameters["@Idf_Plano"].Value);
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
        /// Método utilizado para inserir uma instância de Plano no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPlano = Convert.ToInt32(cmd.Parameters["@Idf_Plano"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Plano no banco de dados.
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
        /// Método utilizado para atualizar uma instância de Plano no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de Plano no banco de dados.
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
        /// Método utilizado para salvar uma instância de Plano no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de Plano no banco de dados.
        /// </summary>
        /// <param name="idPlano">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlano)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));

            parms[0].Value = idPlano;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de Plano no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlano">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlano, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));

            parms[0].Value = idPlano;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de Plano no banco de dados.
        /// </summary>
        /// <param name="idPlano">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idPlano)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Plano where Idf_Plano in (";

            for (int i = 0; i < idPlano.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPlano[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPlano">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlano)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));

            parms[0].Value = idPlano;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlano">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlano, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano", SqlDbType.Int, 4));

            parms[0].Value = idPlano;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano, Pla.Des_Plano, Pla.Qtd_Dias_Validade, Pla.Qtd_SMS, Pla.Qtd_Visualizacao, Pla.Dta_Cadastro, Pla.Flg_Inativo, Pla.Vlr_Base, Pla.Dta_Inicio, Pla.Dta_Final, Pla.Idf_Plano_Tipo, Pla.Idf_Plano_Forma_Pagamento, Pla.Qtd_Parcela, Pla.Vlr_Desconto_Maximo, Pla.Qtd_SMS_Maxima, Pla.Vlr_Base_Minimo, Pla.Qtd_Prazo_Boleto_Maxima, Pla.Flg_Boleto_Registrado, Pla.Idf_Tipo_Contrato, Pla.Flg_Enviar_Contrato, Pla.Flg_Habilita_Venda_Personalizada, Pla.Flg_Libera_Usuarios_Tanque, Pla.Flg_Ilimitado, Pla.Flg_Recorrente, Pla.Qtd_Campanha FROM BNE_Plano Pla";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Plano a partir do banco de dados.
        /// </summary>
        /// <param name="idPlano">Chave do registro.</param>
        /// <returns>Instância de Plano.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Plano LoadObject(int idPlano)
        {
            using (IDataReader dr = LoadDataReader(idPlano))
            {
                Plano objPlano = new Plano();
                if (SetInstance(dr, objPlano))
                    return objPlano;
            }
            throw (new RecordNotFoundException(typeof(Plano)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Plano a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlano">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Plano.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Plano LoadObject(int idPlano, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPlano, trans))
            {
                Plano objPlano = new Plano();
                if (SetInstance(dr, objPlano))
                    return objPlano;
            }
            throw (new RecordNotFoundException(typeof(Plano)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Plano a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPlano))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Plano a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPlano, trans))
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
        /// <param name="objPlano">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool SetInstance(IDataReader dr, Plano objPlano, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objPlano._idPlano = Convert.ToInt32(dr["Idf_Plano"]);
                    objPlano._descricaoPlano = Convert.ToString(dr["Des_Plano"]);
                    objPlano._quantidadeDiasValidade = Convert.ToInt32(dr["Qtd_Dias_Validade"]);
                    objPlano._quantidadeSMS = Convert.ToInt32(dr["Qtd_SMS"]);
                    objPlano._quantidadeVisualizacao = Convert.ToInt32(dr["Qtd_Visualizacao"]);
                    objPlano._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objPlano._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objPlano._valorBase = Convert.ToDecimal(dr["Vlr_Base"]);
                    if (dr["Dta_Inicio"] != DBNull.Value)
                        objPlano._dataInicio = Convert.ToDateTime(dr["Dta_Inicio"]);
                    if (dr["Dta_Final"] != DBNull.Value)
                        objPlano._dataFinal = Convert.ToDateTime(dr["Dta_Final"]);
                    objPlano._planoTipo = new PlanoTipo(Convert.ToInt32(dr["Idf_Plano_Tipo"]));
                    objPlano._planoFormaPagamento = new PlanoFormaPagamento(Convert.ToInt32(dr["Idf_Plano_Forma_Pagamento"]));
                    objPlano._quantidadeParcela = Convert.ToInt32(dr["Qtd_Parcela"]);
                    objPlano._valorDescontoMaximo = Convert.ToInt32(dr["Vlr_Desconto_Maximo"]);
                    if (dr["Qtd_SMS_Maxima"] != DBNull.Value)
                        objPlano._quantidadeSMSMaxima = Convert.ToInt32(dr["Qtd_SMS_Maxima"]);
                    if (dr["Vlr_Base_Minimo"] != DBNull.Value)
                        objPlano._valorBaseMinimo = Convert.ToDecimal(dr["Vlr_Base_Minimo"]);
                    if (dr["Qtd_Prazo_Boleto_Maxima"] != DBNull.Value)
                        objPlano._quantidadePrazoBoletoMaxima = Convert.ToInt32(dr["Qtd_Prazo_Boleto_Maxima"]);
                    objPlano._flagBoletoRegistrado = Convert.ToBoolean(dr["Flg_Boleto_Registrado"]);
                    if (dr["Idf_Tipo_Contrato"] != DBNull.Value)
                        objPlano._tipoContrato = new TipoContrato(Convert.ToInt32(dr["Idf_Tipo_Contrato"]));
                    objPlano._flagEnviarContrato = Convert.ToBoolean(dr["Flg_Enviar_Contrato"]);
                    objPlano._flagHabilitaVendaPersonalizada = Convert.ToBoolean(dr["Flg_Habilita_Venda_Personalizada"]);
                    objPlano._flagLiberaUsuariosTanque = Convert.ToBoolean(dr["Flg_Libera_Usuarios_Tanque"]);
                    objPlano._flagIlimitado = Convert.ToBoolean(dr["Flg_Ilimitado"]);
                    objPlano._flagRecorrente = Convert.ToBoolean(dr["Flg_Recorrente"]);
                    objPlano._quantidadeCampanha = Convert.ToInt16(dr["Qtd_Campanha"]);
                    if (dr["Vlr_De"] != DBNull.Value)
                        objPlano._valorDe = Convert.ToDecimal(dr["Vlr_De"]);
                    if (dr["flg_boleto_recorrente"] != DBNull.Value)
                        objPlano.FlagBoletoRecorrente = Convert.ToBoolean(dr["flg_boleto_recorrente"]);
                    
                    objPlano._persisted = true;
                    objPlano._modified = false;

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