//-- Data: 25/07/2014 11:14
//-- Autor: Lennon Vidal

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class AllinCicloVida // Tabela: BNE_Allin_Ciclo_Vida
    {
        #region Atributos
        private int _idAllinCicloVida;
        private TipoGatilho _tipoGatilho;
        private string _IdentificadorCicloAllin;
        private string _descricaoEvento;
        private bool _flagAceitaRepeticao;
        private bool _flagInativo;
        private string _descricaoGoogleUtm;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdAllinCicloVida
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int IdAllinCicloVida
        {
            get
            {
                return this._idAllinCicloVida;
            }
            set
            {
                this._idAllinCicloVida = value;
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

        #region IdentificadorCicloAllin
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo obrigatório.
        /// </summary>
        public string IdentificadorCicloAllin
        {
            get
            {
                return this._IdentificadorCicloAllin;
            }
            set
            {
                this._IdentificadorCicloAllin = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoEvento
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoEvento
        {
            get
            {
                return this._descricaoEvento;
            }
            set
            {
                this._descricaoEvento = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagAceitaRepeticao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagAceitaRepeticao
        {
            get
            {
                return this._flagAceitaRepeticao;
            }
            set
            {
                this._flagAceitaRepeticao = value;
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
        public AllinCicloVida()
        {
        }
        public AllinCicloVida(int idAllinCicloVida)
        {
            this._idAllinCicloVida = idAllinCicloVida;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Allin_Ciclo_Vida (Idf_Allin_Ciclo_Vida, Idf_Tipo_Gatilho, Identificador_Ciclo_Allin, Des_Evento, Flg_Aceita_Repeticao, Flg_Inativo, Des_Google_Utm) VALUES (@Idf_Allin_Ciclo_Vida, @Idf_Tipo_Gatilho, @Identificador_Ciclo_Allin, @Des_Evento, @Flg_Aceita_Repeticao, @Flg_Inativo, @Des_Google_Utm);";
        private const string SPUPDATE = "UPDATE BNE_Allin_Ciclo_Vida SET Idf_Tipo_Gatilho = @Idf_Tipo_Gatilho, Identificador_Ciclo_Allin = @Identificador_Ciclo_Allin, Des_Evento = @Des_Evento, Flg_Aceita_Repeticao = @Flg_Aceita_Repeticao, Flg_Inativo = @Flg_Inativo, Des_Google_Utm = @Des_Google_Utm WHERE Idf_Allin_Ciclo_Vida = @Idf_Allin_Ciclo_Vida";
        private const string SPDELETE = "DELETE FROM BNE_Allin_Ciclo_Vida WHERE Idf_Allin_Ciclo_Vida = @Idf_Allin_Ciclo_Vida";
        private const string SPSELECTID = "SELECT * FROM BNE_Allin_Ciclo_Vida WITH(NOLOCK) WHERE Idf_Allin_Ciclo_Vida = @Idf_Allin_Ciclo_Vida";
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
            parms.Add(new SqlParameter("@Idf_Allin_Ciclo_Vida", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Gatilho", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Identificador_Ciclo_Allin", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Des_Evento", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Flg_Aceita_Repeticao", SqlDbType.Bit, 1));
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
            parms[0].Value = this._idAllinCicloVida;

            if (this._tipoGatilho != null)
                parms[1].Value = this._tipoGatilho.IdTipoGatilho;
            else
                parms[1].Value = DBNull.Value;

            parms[2].Value = this._IdentificadorCicloAllin;
            parms[3].Value = this._descricaoEvento;
            parms[4].Value = this._flagAceitaRepeticao;
            parms[5].Value = this._flagInativo;

            if (!String.IsNullOrEmpty(this._descricaoGoogleUtm))
                parms[6].Value = this._descricaoGoogleUtm;
            else
                parms[6].Value = DBNull.Value;


            if (!this._persisted)
            {
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de AllinCicloVida no banco de dados.
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
        /// Método utilizado para inserir uma instância de AllinCicloVida no banco de dados, dentro de uma transação.
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
        /// Método utilizado para atualizar uma instância de AllinCicloVida no banco de dados.
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
        /// Método utilizado para atualizar uma instância de AllinCicloVida no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de AllinCicloVida no banco de dados.
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
        /// Método utilizado para salvar uma instância de AllinCicloVida no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de AllinCicloVida no banco de dados.
        /// </summary>
        /// <param name="idAllinCicloVida">Chave do registro.</param>
        /// <remarks>Lennon Vidal</remarks>
        public static void Delete(int idAllinCicloVida)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Allin_Ciclo_Vida", SqlDbType.Int, 4));

            parms[0].Value = idAllinCicloVida;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de AllinCicloVida no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAllinCicloVida">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Lennon Vidal</remarks>
        public static void Delete(int idAllinCicloVida, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Allin_Ciclo_Vida", SqlDbType.Int, 4));

            parms[0].Value = idAllinCicloVida;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de AllinCicloVida no banco de dados.
        /// </summary>
        /// <param name="idAllinCicloVida">Lista de chaves.</param>
        /// <remarks>Lennon Vidal</remarks>
        public static void Delete(List<int> idAllinCicloVida)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Allin_Ciclo_Vida where Idf_Allin_Ciclo_Vida in (";

            for (int i = 0; i < idAllinCicloVida.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idAllinCicloVida[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idAllinCicloVida">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private static IDataReader LoadDataReader(int idAllinCicloVida)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Allin_Ciclo_Vida", SqlDbType.Int, 4));

            parms[0].Value = idAllinCicloVida;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAllinCicloVida">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private static IDataReader LoadDataReader(int idAllinCicloVida, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Allin_Ciclo_Vida", SqlDbType.Int, 4));

            parms[0].Value = idAllinCicloVida;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, All.Idf_Allin_Ciclo_Vida, All.Idf_Tipo_Gatilho, All.Identificador_Ciclo_Allin, All.Des_Evento, All.Flg_Aceita_Repeticao, All.Flg_Inativo, All.Des_Google_Utm FROM BNE_Allin_Ciclo_Vida All";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de AllinCicloVida a partir do banco de dados.
        /// </summary>
        /// <param name="idAllinCicloVida">Chave do registro.</param>
        /// <returns>Instância de AllinCicloVida.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public static AllinCicloVida LoadObject(int idAllinCicloVida)
        {
            using (IDataReader dr = LoadDataReader(idAllinCicloVida))
            {
                AllinCicloVida objAllinCicloVida = new AllinCicloVida();
                if (SetInstance(dr, objAllinCicloVida))
                    return objAllinCicloVida;
            }
            throw (new RecordNotFoundException(typeof(AllinCicloVida)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de AllinCicloVida a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idAllinCicloVida">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de AllinCicloVida.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public static AllinCicloVida LoadObject(int idAllinCicloVida, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idAllinCicloVida, trans))
            {
                AllinCicloVida objAllinCicloVida = new AllinCicloVida();
                if (SetInstance(dr, objAllinCicloVida))
                    return objAllinCicloVida;
            }
            throw (new RecordNotFoundException(typeof(AllinCicloVida)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de AllinCicloVida a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idAllinCicloVida))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de AllinCicloVida a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idAllinCicloVida, trans))
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
        /// <param name="objAllinCicloVida">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private static bool SetInstance(IDataReader dr, AllinCicloVida objAllinCicloVida)
        {
            try
            {
                if (dr.Read())
                {
                    objAllinCicloVida._idAllinCicloVida = Convert.ToInt32(dr["Idf_Allin_Ciclo_Vida"]);
                    if (dr["Idf_Tipo_Gatilho"] != DBNull.Value)
                        objAllinCicloVida._tipoGatilho = new TipoGatilho(Convert.ToInt32(dr["Idf_Tipo_Gatilho"]));
                    objAllinCicloVida._IdentificadorCicloAllin = Convert.ToString(dr["Identificador_Ciclo_Allin"]);
                    objAllinCicloVida._descricaoEvento = Convert.ToString(dr["Des_Evento"]);
                    objAllinCicloVida._flagAceitaRepeticao = Convert.ToBoolean(dr["Flg_Aceita_Repeticao"]);
                    objAllinCicloVida._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (HasColumn(dr, "Des_Google_Utm"))
                        if (dr["Des_Google_Utm"] != DBNull.Value)
                            objAllinCicloVida._descricaoGoogleUtm = Convert.ToString(dr["Des_Google_Utm"]);

                    objAllinCicloVida._persisted = true;
                    objAllinCicloVida._modified = false;

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