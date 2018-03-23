//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.Logger.Exceptions;

namespace BNE.Mensagem.AsyncServices.BLL
{
    public partial class Atividade // Tabela: TAB_Atividade
    {
        #region Atributos
        private int _idAtividade;
        private PluginsCompatibilidade _pluginsCompatibilidade;
        private StatusAtividade _statusAtividade;
        private DateTime _dataCadastro;
        private string _descricaoErro;
        private DateTime? _dataExecucao;
        private TipoAtividadeSistema _tipoAtividadeSistema;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdAtividade
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdAtividade
        {
            get
            {
                return this._idAtividade;
            }
        }
        #endregion

        #region PluginsCompatibilidade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PluginsCompatibilidade PluginsCompatibilidade
        {
            get
            {
                return this._pluginsCompatibilidade;
            }
            set
            {
                this._pluginsCompatibilidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region StatusAtividade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public StatusAtividade StatusAtividade
        {
            get
            {
                return this._statusAtividade;
            }
            set
            {
                this._statusAtividade = value;
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

        #region DescricaoErro
        /// <summary>
        /// Tamanho do campo: 16.
        /// Campo opcional.
        /// </summary>
        public string DescricaoErro
        {
            get
            {
                return this._descricaoErro;
            }
            set
            {
                this._descricaoErro = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataExecucao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataExecucao
        {
            get
            {
                return this._dataExecucao;
            }
            set
            {
                this._dataExecucao = value;
                this._modified = true;
            }
        }
        #endregion

        #region TipoAtividadeSistema
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public TipoAtividadeSistema TipoAtividadeSistema
        {
            get
            {
                return this._tipoAtividadeSistema;
            }
            set
            {
                this._tipoAtividadeSistema = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public Atividade()
        {
        }
        public Atividade(int idAtividade)
        {
            this._idAtividade = idAtividade;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO TAB_Atividade (Idf_Plugins_Compatibilidade, Idf_Status_Atividade, Des_Parametros_Entrada, Des_Parametros_Saida, Dta_Cadastro,Des_Erro, Dta_Execucao, Idf_Tipo_Atividade_Sistema) VALUES (@Idf_Plugins_Compatibilidade, @Idf_Status_Atividade, @Des_Parametros_Entrada, @Des_Parametros_Saida, @Dta_Cadastro, @Des_Erro, @Dta_Execucao, @Idf_Tipo_Atividade_Sistema);SET @Idf_Atividade = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE TAB_Atividade SET Idf_Plugins_Compatibilidade = @Idf_Plugins_Compatibilidade, Idf_Status_Atividade = @Idf_Status_Atividade, Des_Parametros_Entrada = @Des_Parametros_Entrada, Des_Parametros_Saida = @Des_Parametros_Saida, Dta_Cadastro = @Dta_Cadastro, Des_Erro = @Des_Erro, Dta_Execucao = @Dta_Execucao, Idf_Tipo_Atividade_Sistema = @Idf_Tipo_Atividade_Sistema WHERE Idf_Atividade = @Idf_Atividade";
        private const string SPDELETE = "DELETE FROM TAB_Atividade WHERE Idf_Atividade = @Idf_Atividade";
        #endregion

        #region Métodos

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de Atividade no banco de dados.
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
                        this._idAtividade = Convert.ToInt32(cmd.Parameters["@Idf_Atividade"].Value);
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
        /// Método utilizado para inserir uma instância de Atividade no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idAtividade = Convert.ToInt32(cmd.Parameters["@Idf_Atividade"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Atividade no banco de dados.
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
        /// Método utilizado para atualizar uma instância de Atividade no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de Atividade no banco de dados.
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
        /// Método utilizado para salvar uma instância de Atividade no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de Atividade no banco de dados.
        /// </summary>
        /// <param name="idAtividade">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idAtividade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Atividade", SqlDbType.Int, 4));

            parms[0].Value = idAtividade;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de Atividade no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAtividade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idAtividade, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Atividade", SqlDbType.Int, 4));

            parms[0].Value = idAtividade;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de Atividade no banco de dados.
        /// </summary>
        /// <param name="idAtividade">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idAtividade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from TAB_Atividade where Idf_Atividade in (";

            for (int i = 0; i < idAtividade.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idAtividade[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idAtividade">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idAtividade)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Atividade", SqlDbType.Int, 4));

            parms[0].Value = idAtividade;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAtividade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idAtividade, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Atividade", SqlDbType.Int, 4));

            parms[0].Value = idAtividade;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ati.Idf_Atividade, Ati.Idf_Plugins_Compatibilidade, Ati.Idf_Status_Atividade, Ati.Idf_Usuario_Gerador, Ati.Des_Parametros_Entrada, Ati.Des_Parametros_Saida, Ati.Dta_Cadastro, Ati.Des_Caminho_Arquivo_Upload, Ati.Des_Caminho_Arquivo_Gerado, Ati.Des_Erro, Ati.Dta_Agendamento, Ati.Dta_Execucao,  Ati.Idf_Tipo_Saida, Ati.Idf_Filial FROM TAB_Atividade Ati";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Atividade a partir do banco de dados.
        /// </summary>
        /// <param name="idAtividade">Chave do registro.</param>
        /// <returns>Instância de Atividade.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Atividade LoadObject(int idAtividade)
        {
            using (IDataReader dr = LoadDataReader(idAtividade))
            {
                Atividade objAtividade = new Atividade();
                if (SetInstance(dr, objAtividade))
                    return objAtividade;
            }
            throw (new RecordNotFoundException(typeof(Atividade)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Atividade a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAtividade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Atividade.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Atividade LoadObject(int idAtividade, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idAtividade, trans))
            {
                Atividade objAtividade = new Atividade();
                if (SetInstance(dr, objAtividade))
                    return objAtividade;
            }
            throw (new RecordNotFoundException(typeof(Atividade)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Atividade a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idAtividade))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Atividade a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idAtividade, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #endregion
    }
}