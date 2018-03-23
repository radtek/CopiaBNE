//-- Data: 14/03/2011 09:07
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using System.ComponentModel.DataAnnotations;

namespace BNE.BLL
{
    public partial class PlanoParcela // Tabela: BNE_Plano_Parcela
    {
        #region Atributos
        private int _idPlanoParcela;
        private PlanoAdquirido _planoAdquirido;
        private DateTime _dataCadastro;
        private DateTime? _dataPagamento;
        private decimal _valorParcela;
        private bool _flagInativo;
        private PlanoParcelaSituacao _planoParcelaSituacao;
        private int? _numeroDesconto;
        private int _quantidadeSMSTotal;
        private int? _quantidadeSMSLiberada;
        private DateTime? _dataEmissaoNotaAntecipada;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPlanoParcela
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPlanoParcela
        {
            get
            {
                return this._idPlanoParcela;
            }
        }
        #endregion

        #region PlanoAdquirido
        /// <summary>
        /// Campo obrigatório.
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

        #region DataPagamento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataPagamento
        {
            get
            {
                return this._dataPagamento;
            }
            set
            {
                this._dataPagamento = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorParcela
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public decimal ValorParcela
        {
            get
            {
                return this._valorParcela;
            }
            set
            {
                this._valorParcela = value;
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

        #region PlanoParcelaSituacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PlanoParcelaSituacao PlanoParcelaSituacao
        {
            get
            {
                return this._planoParcelaSituacao;
            }
            set
            {
                this._planoParcelaSituacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDesconto
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? NumeroDesconto
        {
            get
            {
                return this._numeroDesconto;
            }
            set
            {
                this._numeroDesconto = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeSMSTotal
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int QuantidadeSMSTotal
        {
            get
            {
                return this._quantidadeSMSTotal;
            }
            set
            {
                this._quantidadeSMSTotal = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeSMSLiberada
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public int? QuantidadeSMSLiberada
        {
            get
            {
                return this._quantidadeSMSLiberada;
            }
            set
            {
                this._quantidadeSMSLiberada = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataEmissaoNotaAntecipada
        /// <summary>
        /// Campo opcional.
        /// </summary>
        [Display(Name = "Data de Emissão da Nota Antecipada")]
        public DateTime? DataEmissaoNotaAntecipada
        {
            get
            {
                return this._dataEmissaoNotaAntecipada;
            }
            set
            {
                this._dataEmissaoNotaAntecipada = value;
                this._modified = true;
            }
        }
        #endregion
        
        #endregion

        #region Construtores
        public PlanoParcela()
        {
        }
        public PlanoParcela(int idPlanoParcela)
        {
            this._idPlanoParcela = idPlanoParcela;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Plano_Parcela (Idf_Plano_Adquirido, Dta_Cadastro, Dta_Pagamento, Vlr_Parcela, Flg_Inativo, Idf_Plano_Parcela_Situacao, Num_Desconto, Qtd_SMS_Total, Qtd_SMS_Liberada,Dta_Emissao_Nota_Antecipada) VALUES (@Idf_Plano_Adquirido, @Dta_Cadastro, @Dta_Pagamento, @Vlr_Parcela, @Flg_Inativo, @Idf_Plano_Parcela_Situacao, @Num_Desconto, @Qtd_SMS_Total, @Qtd_SMS_Liberada,@Dta_Emissao_Nota_Antecipada);SET @Idf_Plano_Parcela = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Plano_Parcela SET Idf_Plano_Adquirido = @Idf_Plano_Adquirido, Dta_Cadastro = @Dta_Cadastro, Dta_Pagamento = @Dta_Pagamento, Vlr_Parcela = @Vlr_Parcela, Flg_Inativo = @Flg_Inativo, Idf_Plano_Parcela_Situacao = @Idf_Plano_Parcela_Situacao, Num_Desconto = @Num_Desconto, Qtd_SMS_Total = @Qtd_SMS_Total, Qtd_SMS_Liberada = @Qtd_SMS_Liberada, Dta_Emissao_Nota_Antecipada = @Dta_Emissao_Nota_Antecipada WHERE Idf_Plano_Parcela = @Idf_Plano_Parcela";
        private const string SPDELETE = "DELETE FROM BNE_Plano_Parcela WHERE Idf_Plano_Parcela = @Idf_Plano_Parcela";
        private const string SPSELECTID = "SELECT * FROM BNE_Plano_Parcela WHERE Idf_Plano_Parcela = @Idf_Plano_Parcela";
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
            parms.Add(new SqlParameter("@Idf_Plano_Parcela", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Pagamento", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Vlr_Parcela", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Plano_Parcela_Situacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Desconto", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_SMS_Total", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_SMS_Liberada", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Emissao_Nota_Antecipada", SqlDbType.DateTime, 8));
            
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
            parms[0].Value = this._idPlanoParcela;
            parms[1].Value = this._planoAdquirido.IdPlanoAdquirido;

            if (this._dataPagamento.HasValue)
                parms[3].Value = this._dataPagamento;
            else
                parms[3].Value = DBNull.Value;

            parms[4].Value = this._valorParcela;
            parms[5].Value = this._flagInativo;
            parms[6].Value = this._planoParcelaSituacao.IdPlanoParcelaSituacao;

            if (this._numeroDesconto.HasValue)
                parms[7].Value = this._numeroDesconto;
            else
                parms[7].Value = DBNull.Value;

            parms[8].Value = this._quantidadeSMSTotal;
            if (this._quantidadeSMSLiberada.HasValue)
                parms[9].Value = this._quantidadeSMSLiberada;
            else
                parms[9].Value = DBNull.Value;

            if (this._dataEmissaoNotaAntecipada.HasValue)
                parms[10].Value = this._dataEmissaoNotaAntecipada;
            else
                parms[10].Value = DBNull.Value;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[2].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de PlanoParcela no banco de dados.
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
                        this._idPlanoParcela = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Parcela"].Value);
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
        /// Método utilizado para inserir uma instância de PlanoParcela no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPlanoParcela = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Parcela"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de PlanoParcela no banco de dados.
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
        /// Método utilizado para atualizar uma instância de PlanoParcela no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de PlanoParcela no banco de dados.
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
        /// Método utilizado para salvar uma instância de PlanoParcela no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de PlanoParcela no banco de dados.
        /// </summary>
        /// <param name="idPlanoParcela">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlanoParcela)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Parcela", SqlDbType.Int, 4));

            parms[0].Value = idPlanoParcela;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PlanoParcela no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoParcela">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlanoParcela, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Parcela", SqlDbType.Int, 4));

            parms[0].Value = idPlanoParcela;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de PlanoParcela no banco de dados.
        /// </summary>
        /// <param name="idPlanoParcela">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idPlanoParcela)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Plano_Parcela where Idf_Plano_Parcela in (";

            for (int i = 0; i < idPlanoParcela.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPlanoParcela[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPlanoParcela">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlanoParcela)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Parcela", SqlDbType.Int, 4));

            parms[0].Value = idPlanoParcela;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoParcela">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlanoParcela, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Parcela", SqlDbType.Int, 4));

            parms[0].Value = idPlanoParcela;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Parcela, Pla.Idf_Plano_Adquirido, Pla.Dta_Cadastro, Pla.Dta_Pagamento, Pla.Vlr_Parcela, Pla.Flg_Inativo, Pla.Idf_Plano_Parcela_Situacao, Pla.Num_Desconto, Pla.Qtd_SMS_Total, Pla.Qtd_SMS_Liberada, Pla.Dta_Emissao_Nota_Antecipada FROM BNE_Plano_Parcela Pla";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoParcela a partir do banco de dados.
        /// </summary>
        /// <param name="idPlanoParcela">Chave do registro.</param>
        /// <returns>Instância de PlanoParcela.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PlanoParcela LoadObject(int idPlanoParcela)
        {
            using (IDataReader dr = LoadDataReader(idPlanoParcela))
            {
                PlanoParcela objPlanoParcela = new PlanoParcela();
                if (SetInstance(dr, objPlanoParcela))
                    return objPlanoParcela;
            }
            throw (new RecordNotFoundException(typeof(PlanoParcela)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoParcela a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoParcela">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PlanoParcela.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PlanoParcela LoadObject(int idPlanoParcela, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPlanoParcela, trans))
            {
                PlanoParcela objPlanoParcela = new PlanoParcela();
                if (SetInstance(dr, objPlanoParcela))
                    return objPlanoParcela;
            }
            throw (new RecordNotFoundException(typeof(PlanoParcela)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoParcela a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoParcela))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoParcela a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoParcela, trans))
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
        /// <param name="objPlanoParcela">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, PlanoParcela objPlanoParcela)
        {
            try
            {
                if (dr.Read())
                {
                    objPlanoParcela._idPlanoParcela = Convert.ToInt32(dr["Idf_Plano_Parcela"]);
                    objPlanoParcela._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
                    objPlanoParcela._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Dta_Pagamento"] != DBNull.Value)
                        objPlanoParcela._dataPagamento = Convert.ToDateTime(dr["Dta_Pagamento"]);
                    objPlanoParcela._valorParcela = Convert.ToDecimal(dr["Vlr_Parcela"]);
                    objPlanoParcela._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objPlanoParcela._planoParcelaSituacao = new PlanoParcelaSituacao(Convert.ToInt32(dr["Idf_Plano_Parcela_Situacao"]));
                    if (dr["Num_Desconto"] != DBNull.Value)
                        objPlanoParcela._numeroDesconto = Convert.ToInt32(dr["Num_Desconto"]);
                    objPlanoParcela._quantidadeSMSTotal = Convert.ToInt32(dr["Qtd_SMS_Total"]);
                    if (dr["Qtd_SMS_Liberada"] != DBNull.Value)
                        objPlanoParcela._quantidadeSMSLiberada = Convert.ToInt32(dr["Qtd_SMS_Liberada"]);

                    objPlanoParcela._persisted = true;
                    objPlanoParcela._modified = false;
                    if (dr["Dta_Emissao_Nota_Antecipada"] != DBNull.Value)
                        objPlanoParcela._dataEmissaoNotaAntecipada = Convert.ToDateTime(dr["Dta_Emissao_Nota_Antecipada"]);
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