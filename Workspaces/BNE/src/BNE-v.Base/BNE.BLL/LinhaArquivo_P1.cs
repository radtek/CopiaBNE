//-- Data: 16/06/2014 15:44
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class LinhaArquivo // Tabela: BNE_Linha_Arquivo
    {
        #region Atributos
        private int _idLinhaArquivo;
        private Arquivo _arquivo;
        private Transacao _transacao;
        private int _numeroLinha;
        private CobrancaBoleto _cobrancaBoleto;
        private TipoLinhaArquivo _tipoLinhaArquivo;
        private string _descricaoConteudo;
        private string _descricaoMensagemLiberacao;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdLinhaArquivo
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdLinhaArquivo
        {
            get
            {
                return this._idLinhaArquivo;
            }
        }
        #endregion

        #region Arquivo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Arquivo Arquivo
        {
            get
            {
                return this._arquivo;
            }
            set
            {
                this._arquivo = value;
                this._modified = true;
            }
        }
        #endregion

        #region Transacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Transacao Transacao
        {
            get
            {
                return this._transacao;
            }
            set
            {
                this._transacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroLinha
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int NumeroLinha
        {
            get
            {
                return this._numeroLinha;
            }
            set
            {
                this._numeroLinha = value;
                this._modified = true;
            }
        }
        #endregion

        #region CobrancaBoleto
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public CobrancaBoleto CobrancaBoleto
        {
            get
            {
                return this._cobrancaBoleto;
            }
            set
            {
                this._cobrancaBoleto = value;
                this._modified = true;
            }
        }
        #endregion

        #region TipoLinhaArquivo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public TipoLinhaArquivo TipoLinhaArquivo
        {
            get
            {
                return this._tipoLinhaArquivo;
            }
            set
            {
                this._tipoLinhaArquivo = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoMensagemLiberacao
        /// <summary>
        /// Tamanho do campo: 500.
        /// Campo opcional.
        /// </summary>
        public string DescricaoMensagemLiberacao
        {
            get
            {
                return this._descricaoMensagemLiberacao;
            }
            set
            {
                this._descricaoMensagemLiberacao = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public LinhaArquivo()
        {
        }
        public LinhaArquivo(int idLinhaArquivo)
        {
            this._idLinhaArquivo = idLinhaArquivo;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Linha_Arquivo (Idf_Arquivo, Idf_Transacao, Num_Linha, Idf_Cobranca_Boleto, Idf_Tipo_Linha_Arquivo, Des_Conteudo, Des_Mensagem_Liberacao) VALUES (@Idf_Arquivo, @Idf_Transacao, @Num_Linha, @Idf_Cobranca_Boleto, @Idf_Tipo_Linha_Arquivo, @Des_Conteudo, @Des_Mensagem_Liberacao);SET @Idf_Linha_Arquivo = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Linha_Arquivo SET Idf_Arquivo = @Idf_Arquivo, Idf_Transacao = @Idf_Transacao, Num_Linha = @Num_Linha, Idf_Cobranca_Boleto = @Idf_Cobranca_Boleto, Idf_Tipo_Linha_Arquivo = @Idf_Tipo_Linha_Arquivo, Des_Conteudo = @Des_Conteudo, Des_Mensagem_Liberacao = @Des_Mensagem_Liberacao WHERE Idf_Linha_Arquivo = @Idf_Linha_Arquivo";
        private const string SPDELETE = "DELETE FROM BNE_Linha_Arquivo WHERE Idf_Linha_Arquivo = @Idf_Linha_Arquivo";
        private const string SPSELECTID = "SELECT * FROM BNE_Linha_Arquivo WITH(NOLOCK) WHERE Idf_Linha_Arquivo = @Idf_Linha_Arquivo";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Linha_Arquivo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Arquivo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Linha", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Cobranca_Boleto", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Linha_Arquivo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Conteudo", SqlDbType.VarChar, 500));
            parms.Add(new SqlParameter("@Des_Mensagem_Liberacao", SqlDbType.VarChar, 500));
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
            parms[0].Value = this._idLinhaArquivo;
            parms[1].Value = this._arquivo.IdArquivo;

            if (this._transacao != null)
                parms[2].Value = this._transacao.IdTransacao;
            else
                parms[2].Value = DBNull.Value;

            parms[3].Value = this._numeroLinha;

            if (this._cobrancaBoleto != null)
                parms[4].Value = this._cobrancaBoleto.IdCobrancaBoleto;
            else
                parms[4].Value = DBNull.Value;

            parms[5].Value = this._tipoLinhaArquivo.IdTipoLinhaArquivo;
            parms[6].Value = this._descricaoConteudo;

            if (!String.IsNullOrEmpty(this._descricaoMensagemLiberacao))
                parms[7].Value = this._descricaoMensagemLiberacao;
            else
                parms[7].Value = DBNull.Value;


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
        /// Método utilizado para inserir uma instância de LinhaArquivo no banco de dados.
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
                        this._idLinhaArquivo = Convert.ToInt32(cmd.Parameters["@Idf_Linha_Arquivo"].Value);
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
        /// Método utilizado para inserir uma instância de LinhaArquivo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idLinhaArquivo = Convert.ToInt32(cmd.Parameters["@Idf_Linha_Arquivo"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de LinhaArquivo no banco de dados.
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
        /// Método utilizado para atualizar uma instância de LinhaArquivo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de LinhaArquivo no banco de dados.
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
        /// Método utilizado para salvar uma instância de LinhaArquivo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de LinhaArquivo no banco de dados.
        /// </summary>
        /// <param name="idLinhaArquivo">Chave do registro.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idLinhaArquivo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Linha_Arquivo", SqlDbType.Int, 4));

            parms[0].Value = idLinhaArquivo;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de LinhaArquivo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idLinhaArquivo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idLinhaArquivo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Linha_Arquivo", SqlDbType.Int, 4));

            parms[0].Value = idLinhaArquivo;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de LinhaArquivo no banco de dados.
        /// </summary>
        /// <param name="idLinhaArquivo">Lista de chaves.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(List<int> idLinhaArquivo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Linha_Arquivo where Idf_Linha_Arquivo in (";

            for (int i = 0; i < idLinhaArquivo.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idLinhaArquivo[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idLinhaArquivo">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idLinhaArquivo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Linha_Arquivo", SqlDbType.Int, 4));

            parms[0].Value = idLinhaArquivo;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idLinhaArquivo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idLinhaArquivo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Linha_Arquivo", SqlDbType.Int, 4));

            parms[0].Value = idLinhaArquivo;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Lin.Idf_Linha_Arquivo, Lin.Idf_Arquivo, Lin.Idf_Transacao, Lin.Num_Linha, Lin.Idf_Cobranca_Boleto, Lin.Idf_Tipo_Linha_Arquivo, Lin.Des_Conteudo, Lin.Des_Mensagem_Liberacao FROM BNE_Linha_Arquivo Lin";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de LinhaArquivo a partir do banco de dados.
        /// </summary>
        /// <param name="idLinhaArquivo">Chave do registro.</param>
        /// <returns>Instância de LinhaArquivo.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static LinhaArquivo LoadObject(int idLinhaArquivo)
        {
            using (IDataReader dr = LoadDataReader(idLinhaArquivo))
            {
                LinhaArquivo objLinhaArquivo = new LinhaArquivo();
                if (SetInstance(dr, objLinhaArquivo))
                    return objLinhaArquivo;
            }
            throw (new RecordNotFoundException(typeof(LinhaArquivo)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de LinhaArquivo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idLinhaArquivo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de LinhaArquivo.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static LinhaArquivo LoadObject(int idLinhaArquivo, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idLinhaArquivo, trans))
            {
                LinhaArquivo objLinhaArquivo = new LinhaArquivo();
                if (SetInstance(dr, objLinhaArquivo))
                    return objLinhaArquivo;
            }
            throw (new RecordNotFoundException(typeof(LinhaArquivo)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de LinhaArquivo a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idLinhaArquivo))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de LinhaArquivo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idLinhaArquivo, trans))
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
        /// <param name="objLinhaArquivo">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static bool SetInstance(IDataReader dr, LinhaArquivo objLinhaArquivo)
        {
            try
            {
                if (dr.Read())
                {
                    objLinhaArquivo._idLinhaArquivo = Convert.ToInt32(dr["Idf_Linha_Arquivo"]);
                    objLinhaArquivo._arquivo = new Arquivo(Convert.ToInt32(dr["Idf_Arquivo"]));
                    if (dr["Idf_Transacao"] != DBNull.Value)
                        objLinhaArquivo._transacao = new Transacao(Convert.ToInt32(dr["Idf_Transacao"]));
                    objLinhaArquivo._numeroLinha = Convert.ToInt32(dr["Num_Linha"]);
                    if (dr["Idf_Cobranca_Boleto"] != DBNull.Value)
                        objLinhaArquivo._cobrancaBoleto = new CobrancaBoleto(Convert.ToInt32(dr["Idf_Cobranca_Boleto"]));
                    objLinhaArquivo._tipoLinhaArquivo = new TipoLinhaArquivo(Convert.ToInt32(dr["Idf_Tipo_Linha_Arquivo"]));
                    objLinhaArquivo._descricaoConteudo = Convert.ToString(dr["Des_Conteudo"]);
                    if (dr["Des_Mensagem_Liberacao"] != DBNull.Value)
                        objLinhaArquivo._descricaoMensagemLiberacao = Convert.ToString(dr["Des_Mensagem_Liberacao"]);

                    objLinhaArquivo._persisted = true;
                    objLinhaArquivo._modified = false;

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