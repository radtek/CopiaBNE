using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public partial class AlertaCurriculosAgenda // Tabela alerta.Tab_Alerta_Curriculos_Agenda_Semanal
    {
        #region Atributos

        public int _idCurriculo { get; set; }
        public int _idDiaDaSemana { get; set; }
        public DateTime _dataCadastro { get; set; }
        private bool _flagInativo;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdCurriculo
        public int IdCurriculo 
        { 
            get
            {
                return _idCurriculo;
            }
            set
            {
                this._idCurriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdDiaDaSemana
        public int IdDiaDaSemana
        {
            get
            {
                return _idDiaDaSemana;
            }
            set
            {
                this._idDiaDaSemana = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataCadastro
        public DateTime DataCadastro
        {
            get
            {
                return this._dataCadastro;
            }
            set
            {
                this._dataCadastro = value;
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

        #endregion

        #region Construtores

        public AlertaCurriculosAgenda()
        {

        }
        public AlertaCurriculosAgenda(int idCurriculo, int idDiaDaSemana, DateTime dataCadastro)
        {
            this._idCurriculo = idCurriculo;
            this._idDiaDaSemana = idDiaDaSemana;
            this._dataCadastro = dataCadastro;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO alerta.Tab_Alerta_Curriculos_Agenda_Semanal (Idf_Curriculo, Idf_Dia_Da_Semana, Dta_Cadastro, Flg_Inativo) VALUES (@Idf_Curriculo, @Idf_Dia_Da_Semana, @Dta_Cadastro, @Flg_Inativo);";
        private const string SPUPDATE = "UPDATE alerta.Tab_Alerta_Curriculos_Agenda_Semanal SET Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Dia_Da_Semana = @Idf_Dia_Da_Semana";
        private const string SPDELETE = "update alerta.Tab_Alerta_Curriculos_Agenda_Semanal set flg_inativo = 1 WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Dia_Da_Semana = @Idf_Dia_Da_Semana";
        private const string SPSELECTID = "SELECT * FROM alerta.Tab_Alerta_Curriculos_Agenda_Semanal WITH(NOLOCK) WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Dia_Da_Semana = @Idf_Dia_Da_Semana";
        #endregion

        #region Métodos
        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Dia_Da_Semana", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idCurriculo;
            parms[1].Value = this._idDiaDaSemana;
            parms[2].Value = this._dataCadastro;
            parms[3].Value = this._flagInativo;
            if (!this._persisted)
            {
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de AlertaCurriculoAgenda no banco de dados.
        /// </summary>
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
        /// Método utilizado para inserir uma instância de AlertaCurriculoAgenda no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
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
        /// Método utilizado para atualizar uma instância de AlertaCurriculoAgenda no banco de dados.
        /// </summary>
        private void Update()
        {
            if (this._modified)
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

                            DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPUPDATE, parms);
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
        }

        /// <summary>
        /// Método utilizado para atualizar uma instância de AlertaCurriculoAgenda no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
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
        /// Método utilizado para salvar uma instância de AlertaCurriculoAgenda no banco de dados.
        /// </summary>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de AlertaCurriculoAgenda no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
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
        /// Método utilizado para excluir uma instância de AlertaCurriculoAgenda no banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idDiaDaSemana">Chave do registro.</param>
        public static void Delete(int idCurriculo, int idDiaDaSemana)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Dia_Da_Semana", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;
            parms[1].Value = idDiaDaSemana;

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                SqlTransaction trans;
                using (trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }

                }
                conn.Close();
                conn.Dispose();
            }

        }
        
        /// <summary>
        /// Método utilizado para excluir uma instância de AlertaCurriculoAgenda no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idDiaDaSemana">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        public static void Delete(int idCurriculo,int idDiaDaSemana, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Dia_Da_Semana", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;
            parms[1].Value = idDiaDaSemana;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do currículo.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        private static IDataReader LoadDataReader(int idCurriculo, int idDiaDaSemana)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Dia_Da_Semana", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;
            parms[1].Value = idDiaDaSemana;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms, DataAccessLayer.CONN_STRING);
        }

        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculo">Chave do currículo.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        private static IDataReader LoadDataReader(int idCurriculo, int idDiaDaSemana, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Dia_Da_Semana", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;
            parms[1].Value = idDiaDaSemana;


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
        //public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        //{
        //    int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
        //    int fim = paginaCorrente * tamanhoPagina;

        //    string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, bAl.Idf_Funcao, bAl.Idf_Curriculo, bAl.Des_Funcao, bAl.Flg_Similar, bAl.Flg_Inativo FROM alerta.Tab_Alerta_Funcoes bAl";
        //    string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
        //    SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

        //    totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
        //    return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null, DataAccessLayer.CONN_STRING);
        //}
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de AlertaCurriculoAgenda a partir do banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <returns>Instância de AlertaCurriculoAgenda.</returns>
        public static AlertaCurriculosAgenda LoadObject(int idCurriculo, int idDiaDaSemana)
        {
            using (IDataReader dr = LoadDataReader(idCurriculo, idDiaDaSemana))
            {
                AlertaCurriculosAgenda objAlertaCurriculosAgenda = new AlertaCurriculosAgenda();
                if (SetInstance(dr, objAlertaCurriculosAgenda))
                    return objAlertaCurriculosAgenda;
            }
            return null; //throw(new RecordNotFoundException(typeof(AlertaCurriculosAgenda)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de AlertaCurriculoAgenda a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de AlertaFuncoes.</returns>
        public static AlertaCurriculosAgenda LoadObject(int idCurriculo, int idDiaDaSemana, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idCurriculo, idDiaDaSemana, trans))
            {
                AlertaCurriculosAgenda objAlertaCurriculosAgenda = new AlertaCurriculosAgenda();
                if (SetInstance(dr, objAlertaCurriculosAgenda))
                    return objAlertaCurriculosAgenda;
            }
            return null; //throw (new RecordNotFoundException(typeof(AlertaCurriculosAgenda)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de AlertaFuncoes a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idCurriculo, this._idDiaDaSemana))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de AlertaCurriculosAgenda a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idCurriculo, this._idDiaDaSemana, trans))
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
        /// <param name="objAlertaCurriculosAgenda">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        private static bool SetInstance(IDataReader dr, AlertaCurriculosAgenda objAlertaCurriculosAgenda)
        {
            try
            {
                if (dr.Read())
                {
                    objAlertaCurriculosAgenda._idCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
                    objAlertaCurriculosAgenda._idDiaDaSemana = Convert.ToInt32(dr["Idf_Dia_Da_Semana"]);
                    objAlertaCurriculosAgenda._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objAlertaCurriculosAgenda._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

                    objAlertaCurriculosAgenda._persisted = true;
                    objAlertaCurriculosAgenda._modified = false;

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
