//-- Data: 25/07/2014 11:14
//-- Autor: Lennon Vidal

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class AllinTransacional // Tabela: BNE_Allin_Transacional
    {
        #region Atributos
        private int _idAllinTransacao;
        private TipoGatilho _tipoGatilho;
        private string _descricaoAssunto;
        private string _emailRemetente;
        private string _emailResposta;
        private string _IdentificadorHtmlAllin;
        private string _nomeRemetente;
        private bool _flagAgendar;
        private decimal? _quantidadeDiasDisparo;
        private TimeSpan? _horaDisparo;
        private bool _flagInativo;
        private string _descricaoGoogleUtm;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdAllinTransacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int IdAllinTransacao
        {
            get
            {
                return this._idAllinTransacao;
            }
            set
            {
                this._idAllinTransacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region TipoGatilho
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public TipoGatilho TipoGatilho
        {
            get
            {
                return this._tipoGatilho;
            }
            set
            {
                this._tipoGatilho = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoAssunto
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoAssunto
        {
            get
            {
                return this._descricaoAssunto;
            }
            set
            {
                this._descricaoAssunto = value;
                this._modified = true;
            }
        }
        #endregion

        #region EmailRemetente
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string EmailRemetente
        {
            get
            {
                return this._emailRemetente;
            }
            set
            {
                this._emailRemetente = value;
                this._modified = true;
            }
        }
        #endregion

        #region EmailResposta
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string EmailResposta
        {
            get
            {
                return this._emailResposta;
            }
            set
            {
                this._emailResposta = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdentificadorHtmlAllin
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string IdentificadorHtmlAllin
        {
            get
            {
                return this._IdentificadorHtmlAllin;
            }
            set
            {
                this._IdentificadorHtmlAllin = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomeRemetente
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string NomeRemetente
        {
            get
            {
                return this._nomeRemetente;
            }
            set
            {
                this._nomeRemetente = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagAgendar
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagAgendar
        {
            get
            {
                return this._flagAgendar;
            }
            set
            {
                this._flagAgendar = value;
                this._modified = true;
            }
        }
        #endregion

        #region QuantidadeDiasDisparo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? QuantidadeDiasDisparo
        {
            get
            {
                return this._quantidadeDiasDisparo;
            }
            set
            {
                this._quantidadeDiasDisparo = value;
                this._modified = true;
            }
        }
        #endregion

        #region HoraDisparo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public TimeSpan? HoraDisparo
        {
            get
            {
                return this._horaDisparo;
            }
            set
            {
                this._horaDisparo = value;
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

        #region DescricaoGoogleUtm
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string DescricaoGoogleUtm
        {
            get
            {
                return this._descricaoGoogleUtm;
            }
            set
            {
                this._descricaoGoogleUtm = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public AllinTransacional()
        {
        }
        public AllinTransacional(int idAllinTransacao)
        {
            this._idAllinTransacao = idAllinTransacao;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Allin_Transacional (Idf_Allin_Transacao, Idf_Tipo_Gatilho, Des_Assunto, Eml_Remetente, Eml_Resposta, Identificador_Html_Allin, Nme_Remetente, Flg_Agendar, Qtd_Dias_Disparo, Hora_Disparo, Flg_Inativo, Des_Google_Utm) VALUES (@Idf_Allin_Transacao, @Idf_Tipo_Gatilho, @Des_Assunto, @Eml_Remetente, @Eml_Resposta, @Identificador_Html_Allin, @Nme_Remetente, @Flg_Agendar, @Qtd_Dias_Disparo, @Hora_Disparo, @Flg_Inativo, @Des_Google_Utm);";
        private const string SPUPDATE = "UPDATE BNE_Allin_Transacional SET Idf_Tipo_Gatilho = @Idf_Tipo_Gatilho, Des_Assunto = @Des_Assunto, Eml_Remetente = @Eml_Remetente, Eml_Resposta = @Eml_Resposta, Identificador_Html_Allin = @Identificador_Html_Allin, Nme_Remetente = @Nme_Remetente, Flg_Agendar = @Flg_Agendar, Qtd_Dias_Disparo = @Qtd_Dias_Disparo, Hora_Disparo = @Hora_Disparo, Flg_Inativo = @Flg_Inativo, Des_Google_Utm = @Des_Google_Utm WHERE Idf_Allin_Transacao = @Idf_Allin_Transacao";
        private const string SPDELETE = "DELETE FROM BNE_Allin_Transacional WHERE Idf_Allin_Transacao = @Idf_Allin_Transacao";
        private const string SPSELECTID = "SELECT * FROM BNE_Allin_Transacional WITH(NOLOCK) WHERE Idf_Allin_Transacao = @Idf_Allin_Transacao";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Allin_Transacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Gatilho", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Assunto", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Eml_Remetente", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Eml_Resposta", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Identificador_Html_Allin", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Nme_Remetente", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Flg_Agendar", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Qtd_Dias_Disparo", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Hora_Disparo", SqlDbType.Time, 5));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Des_Google_Utm", SqlDbType.VarChar, 100));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Lennon Vidal</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idAllinTransacao;

            if (this._tipoGatilho != null)
                parms[1].Value = this._tipoGatilho.IdTipoGatilho;
            else
                parms[1].Value = DBNull.Value;

            parms[2].Value = this._descricaoAssunto;
            parms[3].Value = this._emailRemetente;
            parms[4].Value = this._emailResposta;
            parms[5].Value = this._IdentificadorHtmlAllin;
            parms[6].Value = this._nomeRemetente;
            parms[7].Value = this._flagAgendar;

            if (this._quantidadeDiasDisparo.HasValue)
                parms[8].Value = this._quantidadeDiasDisparo;
            else
                parms[8].Value = DBNull.Value;


            if (this._horaDisparo.HasValue)
                parms[9].Value = this._horaDisparo;
            else
                parms[9].Value = DBNull.Value;

            parms[10].Value = this._flagInativo;

            if (!String.IsNullOrEmpty(this._descricaoGoogleUtm))
                parms[11].Value = this._descricaoGoogleUtm;
            else
                parms[11].Value = DBNull.Value;


            if (!this._persisted)
            {
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de AllinTransacional no banco de dados.
        /// </summary>
        /// <remarks>Lennon Vidal</remarks>
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
        /// Método utilizado para inserir uma instância de AllinTransacional no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Lennon Vidal</remarks>
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
        /// Método utilizado para atualizar uma instância de AllinTransacional no banco de dados.
        /// </summary>
        /// <remarks>Lennon Vidal</remarks>
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
        /// Método utilizado para atualizar uma instância de AllinTransacional no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Lennon Vidal</remarks>
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
        /// Método utilizado para salvar uma instância de AllinTransacional no banco de dados.
        /// </summary>
        /// <remarks>Lennon Vidal</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de AllinTransacional no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Lennon Vidal</remarks>
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
        /// Método utilizado para excluir uma instância de AllinTransacional no banco de dados.
        /// </summary>
        /// <param name="idAllinTransacao">Chave do registro.</param>
        /// <remarks>Lennon Vidal</remarks>
        public static void Delete(int idAllinTransacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Allin_Transacao", SqlDbType.Int, 4));

            parms[0].Value = idAllinTransacao;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de AllinTransacional no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAllinTransacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Lennon Vidal</remarks>
        public static void Delete(int idAllinTransacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Allin_Transacao", SqlDbType.Int, 4));

            parms[0].Value = idAllinTransacao;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de AllinTransacional no banco de dados.
        /// </summary>
        /// <param name="idAllinTransacao">Lista de chaves.</param>
        /// <remarks>Lennon Vidal</remarks>
        public static void Delete(List<int> idAllinTransacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Allin_Transacional where Idf_Allin_Transacao in (";

            for (int i = 0; i < idAllinTransacao.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idAllinTransacao[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idAllinTransacao">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private static IDataReader LoadDataReader(int idAllinTransacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Allin_Transacao", SqlDbType.Int, 4));

            parms[0].Value = idAllinTransacao;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAllinTransacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private static IDataReader LoadDataReader(int idAllinTransacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Allin_Transacao", SqlDbType.Int, 4));

            parms[0].Value = idAllinTransacao;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, All.Idf_Allin_Transacao, All.Idf_Tipo_Gatilho, All.Des_Assunto, All.Eml_Remetente, All.Eml_Resposta, All.Identificador_Html_Allin, All.Nme_Remetente, All.Flg_Agendar, All.Qtd_Dias_Disparo, All.Hora_Disparo, All.Flg_Inativo, All.Des_Google_Utm FROM BNE_Allin_Transacional All";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de AllinTransacional a partir do banco de dados.
        /// </summary>
        /// <param name="idAllinTransacao">Chave do registro.</param>
        /// <returns>Instância de AllinTransacional.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public static AllinTransacional LoadObject(int idAllinTransacao)
        {
            using (IDataReader dr = LoadDataReader(idAllinTransacao))
            {
                AllinTransacional objAllinTransacional = new AllinTransacional();
                if (SetInstance(dr, objAllinTransacional))
                    return objAllinTransacional;
            }
            throw (new RecordNotFoundException(typeof(AllinTransacional)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de AllinTransacional a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAllinTransacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de AllinTransacional.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public static AllinTransacional LoadObject(int idAllinTransacao, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idAllinTransacao, trans))
            {
                AllinTransacional objAllinTransacional = new AllinTransacional();
                if (SetInstance(dr, objAllinTransacional))
                    return objAllinTransacional;
            }
            throw (new RecordNotFoundException(typeof(AllinTransacional)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de AllinTransacional a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idAllinTransacao))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de AllinTransacional a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idAllinTransacao, trans))
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
        /// <param name="objAllinTransacional">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private static bool SetInstance(IDataReader dr, AllinTransacional objAllinTransacional)
        {
            try
            {
                if (dr.Read())
                {
                    objAllinTransacional._idAllinTransacao = Convert.ToInt32(dr["Idf_Allin_Transacao"]);
                    if (dr["Idf_Tipo_Gatilho"] != DBNull.Value)
                        objAllinTransacional._tipoGatilho = new TipoGatilho(Convert.ToInt32(dr["Idf_Tipo_Gatilho"]));
                    objAllinTransacional._descricaoAssunto = Convert.ToString(dr["Des_Assunto"]);
                    objAllinTransacional._emailRemetente = Convert.ToString(dr["Eml_Remetente"]);
                    objAllinTransacional._emailResposta = Convert.ToString(dr["Eml_Resposta"]);
                    objAllinTransacional._IdentificadorHtmlAllin = Convert.ToString(dr["Identificador_Html_Allin"]);
                    objAllinTransacional._nomeRemetente = Convert.ToString(dr["Nme_Remetente"]);
                    objAllinTransacional._flagAgendar = Convert.ToBoolean(dr["Flg_Agendar"]);
                    if (dr["Qtd_Dias_Disparo"] != DBNull.Value)
                        objAllinTransacional._quantidadeDiasDisparo = Convert.ToDecimal(dr["Qtd_Dias_Disparo"]);
                    if (dr["Hora_Disparo"] != DBNull.Value)
                        objAllinTransacional._horaDisparo = dr["Hora_Disparo"] is TimeSpan ? (TimeSpan)dr["Hora_Disparo"] :
                                                                                             dr["Hora_Disparo"] is int || dr["Hora_Disparo"] is long
                                                                                                    ? new TimeSpan(Convert.ToInt64(dr["Hora_Disparo"]))
                                                                                                    : TimeSpan.Parse(dr["Hora_Disparo"].ToString());
                    objAllinTransacional._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Des_Google_Utm"] != DBNull.Value)
                        objAllinTransacional._descricaoGoogleUtm = Convert.ToString(dr["Des_Google_Utm"]);

                    objAllinTransacional._persisted = true;
                    objAllinTransacional._modified = false;

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