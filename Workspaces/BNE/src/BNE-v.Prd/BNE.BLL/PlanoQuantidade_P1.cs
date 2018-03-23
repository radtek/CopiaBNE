//-- Data: 16/03/2016 10:36
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using System.ComponentModel.DataAnnotations;

namespace BNE.BLL
{
    public partial class PlanoQuantidade // Tabela: BNE_Plano_Quantidade
    {
        #region Atributos
        private int _idPlanoQuantidade;
        private DateTime _dataInicioQuantidade;
        private DateTime _dataFimQuantidade;
        private int _quantidadeSMS;
        private int _quantidadeVisualizacao;
        private int _quantidadeSMSUtilizado;
        private int _quantidadeVisualizacaoUtilizado;
        private bool _flagInativo;
        private DateTime _dataCadastro;
        private DateTime? _dataAlteracao;
        private PlanoAdquirido _planoAdquirido;
        private Int16 _quantidadeCampanha;
        private Int16 _quantidadeCampanhaUtilizado;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPlanoQuantidade
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPlanoQuantidade
        {
            get
            {
                return this._idPlanoQuantidade;
            }
        }
        #endregion

        #region DataInicioQuantidade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataInicioQuantidade
        {
            get
            {
                return this._dataInicioQuantidade;
            }
            set
            {
                this._dataInicioQuantidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataFimQuantidade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataFimQuantidade
        {
            get
            {
                return this._dataFimQuantidade;
            }
            set
            {
                this._dataFimQuantidade = value;
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

        #region QuantidadeSMSUtilizado
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int QuantidadeSMSUtilizado
        {
            get
            {
                return this._quantidadeSMSUtilizado;
            }
            set
            {
                this._quantidadeSMSUtilizado = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeVisualizacaoUtilizado
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int QuantidadeVisualizacaoUtilizado
        {
            get
            {
                return this._quantidadeVisualizacaoUtilizado;
            }
            set
            {
                this._quantidadeVisualizacaoUtilizado = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagInativo
        /// <summary>
        /// Campo opcional.
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
        /// Campo opcional.
        /// </summary>
        /// 
        [Display(Name = "IgnoreData")]
        public DateTime? DataAlteracao
        {
            get
            {
                return this._dataAlteracao;
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

        #region QuantidadeCampanhaUtilizado
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Int16 QuantidadeCampanhaUtilizado
        {
            get
            {
                return this._quantidadeCampanhaUtilizado;
            }
            set
            {
                this._quantidadeCampanhaUtilizado = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public PlanoQuantidade()
        {
        }
        public PlanoQuantidade(int idPlanoQuantidade)
        {
            this._idPlanoQuantidade = idPlanoQuantidade;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Plano_Quantidade (Dta_Inicio_Quantidade, Dta_Fim_Quantidade, Qtd_SMS, Qtd_Visualizacao, Qtd_SMS_Utilizado, Qtd_Visualizacao_Utilizado, Flg_Inativo, Dta_Cadastro, Dta_Alteracao, Idf_Plano_Adquirido, Qtd_Campanha, Qtd_Campanha_Utilizado) VALUES (@Dta_Inicio_Quantidade, @Dta_Fim_Quantidade, @Qtd_SMS, @Qtd_Visualizacao, @Qtd_SMS_Utilizado, @Qtd_Visualizacao_Utilizado, @Flg_Inativo, @Dta_Cadastro, @Dta_Alteracao, @Idf_Plano_Adquirido, @Qtd_Campanha, @Qtd_Campanha_Utilizado);SET @Idf_Plano_Quantidade = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Plano_Quantidade SET Dta_Inicio_Quantidade = @Dta_Inicio_Quantidade, Dta_Fim_Quantidade = @Dta_Fim_Quantidade, Qtd_SMS = @Qtd_SMS, Qtd_Visualizacao = @Qtd_Visualizacao, Qtd_SMS_Utilizado = @Qtd_SMS_Utilizado, Qtd_Visualizacao_Utilizado = @Qtd_Visualizacao_Utilizado, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Idf_Plano_Adquirido = @Idf_Plano_Adquirido, Qtd_Campanha = @Qtd_Campanha, Qtd_Campanha_Utilizado = @Qtd_Campanha_Utilizado WHERE Idf_Plano_Quantidade = @Idf_Plano_Quantidade";
        private const string SPDELETE = "DELETE FROM BNE_Plano_Quantidade WHERE Idf_Plano_Quantidade = @Idf_Plano_Quantidade";
        private const string SPSELECTID = "SELECT * FROM BNE_Plano_Quantidade WITH(NOLOCK) WHERE Idf_Plano_Quantidade = @Idf_Plano_Quantidade";
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
            parms.Add(new SqlParameter("@Idf_Plano_Quantidade", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Inicio_Quantidade", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Fim_Quantidade", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Qtd_SMS", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_Visualizacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_SMS_Utilizado", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_Visualizacao_Utilizado", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Qtd_Campanha", SqlDbType.Int, 1));
            parms.Add(new SqlParameter("@Qtd_Campanha_Utilizado", SqlDbType.Int, 1));
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
            parms[0].Value = this._idPlanoQuantidade;
            parms[1].Value = this._dataInicioQuantidade;
            parms[2].Value = this._dataFimQuantidade;
            parms[3].Value = this._quantidadeSMS;
            parms[4].Value = this._quantidadeVisualizacao;
            parms[5].Value = this._quantidadeSMSUtilizado;
            parms[6].Value = this._quantidadeVisualizacaoUtilizado;
            parms[7].Value = this._flagInativo;

            if (this._planoAdquirido != null)
                parms[10].Value = this._planoAdquirido.IdPlanoAdquirido;
            else
                parms[10].Value = DBNull.Value;

            parms[11].Value = this._quantidadeCampanha;
            parms[12].Value = this._quantidadeCampanhaUtilizado;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[8].Value = this._dataCadastro;
            this._dataAlteracao = DateTime.Now;
            parms[9].Value = this._dataAlteracao;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de PlanoQuantidade no banco de dados.
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
                        this._idPlanoQuantidade = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Quantidade"].Value);
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
        /// Método utilizado para inserir uma instância de PlanoQuantidade no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPlanoQuantidade = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Quantidade"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de PlanoQuantidade no banco de dados.
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
        /// Método utilizado para atualizar uma instância de PlanoQuantidade no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de PlanoQuantidade no banco de dados.
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
        /// Método utilizado para salvar uma instância de PlanoQuantidade no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de PlanoQuantidade no banco de dados.
        /// </summary>
        /// <param name="idPlanoQuantidade">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlanoQuantidade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Quantidade", SqlDbType.Int, 4));

            parms[0].Value = idPlanoQuantidade;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PlanoQuantidade no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoQuantidade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlanoQuantidade, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Quantidade", SqlDbType.Int, 4));

            parms[0].Value = idPlanoQuantidade;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de PlanoQuantidade no banco de dados.
        /// </summary>
        /// <param name="idPlanoQuantidade">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idPlanoQuantidade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Plano_Quantidade where Idf_Plano_Quantidade in (";

            for (int i = 0; i < idPlanoQuantidade.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPlanoQuantidade[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPlanoQuantidade">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlanoQuantidade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Quantidade", SqlDbType.Int, 4));

            parms[0].Value = idPlanoQuantidade;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoQuantidade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlanoQuantidade, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Quantidade", SqlDbType.Int, 4));

            parms[0].Value = idPlanoQuantidade;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Quantidade, Pla.Dta_Inicio_Quantidade, Pla.Dta_Fim_Quantidade, Pla.Qtd_SMS, Pla.Qtd_Visualizacao, Pla.Qtd_SMS_Utilizado, Pla.Qtd_Visualizacao_Utilizado, Pla.Flg_Inativo, Pla.Dta_Cadastro, Pla.Dta_Alteracao, Pla.Idf_Plano_Adquirido, Pla.Qtd_Campanha, Pla.Qtd_Campanha_Utilizado FROM BNE_Plano_Quantidade Pla";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoQuantidade a partir do banco de dados.
        /// </summary>
        /// <param name="idPlanoQuantidade">Chave do registro.</param>
        /// <returns>Instância de PlanoQuantidade.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PlanoQuantidade LoadObject(int idPlanoQuantidade)
        {
            using (IDataReader dr = LoadDataReader(idPlanoQuantidade))
            {
                PlanoQuantidade objPlanoQuantidade = new PlanoQuantidade();
                if (SetInstance(dr, objPlanoQuantidade))
                    return objPlanoQuantidade;
            }
            throw (new RecordNotFoundException(typeof(PlanoQuantidade)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoQuantidade a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoQuantidade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PlanoQuantidade.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PlanoQuantidade LoadObject(int idPlanoQuantidade, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPlanoQuantidade, trans))
            {
                PlanoQuantidade objPlanoQuantidade = new PlanoQuantidade();
                if (SetInstance(dr, objPlanoQuantidade))
                    return objPlanoQuantidade;
            }
            throw (new RecordNotFoundException(typeof(PlanoQuantidade)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoQuantidade a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoQuantidade))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoQuantidade a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoQuantidade, trans))
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
        /// <param name="objPlanoQuantidade">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, PlanoQuantidade objPlanoQuantidade, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objPlanoQuantidade._idPlanoQuantidade = Convert.ToInt32(dr["Idf_Plano_Quantidade"]);
                    objPlanoQuantidade._dataInicioQuantidade = Convert.ToDateTime(dr["Dta_Inicio_Quantidade"]);
                    objPlanoQuantidade._dataFimQuantidade = Convert.ToDateTime(dr["Dta_Fim_Quantidade"]);
                    objPlanoQuantidade._quantidadeSMS = Convert.ToInt32(dr["Qtd_SMS"]);
                    objPlanoQuantidade._quantidadeVisualizacao = Convert.ToInt32(dr["Qtd_Visualizacao"]);
                    objPlanoQuantidade._quantidadeSMSUtilizado = Convert.ToInt32(dr["Qtd_SMS_Utilizado"]);
                    objPlanoQuantidade._quantidadeVisualizacaoUtilizado = Convert.ToInt32(dr["Qtd_Visualizacao_Utilizado"]);
                    if (dr["Flg_Inativo"] != DBNull.Value)
                        objPlanoQuantidade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objPlanoQuantidade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Dta_Alteracao"] != DBNull.Value)
                        objPlanoQuantidade._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
                    if (dr["Idf_Plano_Adquirido"] != DBNull.Value)
                        objPlanoQuantidade._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
                    objPlanoQuantidade._quantidadeCampanha = Convert.ToInt16(dr["Qtd_Campanha"]);
                    objPlanoQuantidade._quantidadeCampanhaUtilizado = Convert.ToInt16(dr["Qtd_Campanha_Utilizado"]);

                    objPlanoQuantidade._persisted = true;
                    objPlanoQuantidade._modified = false;

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