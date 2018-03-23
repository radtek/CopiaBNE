//-- Data: 22/07/2015 14:21
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CampanhaRecrutamentoCurriculo // Tabela: BNE_Campanha_Recrutamento_Curriculo
    {
        #region Atributos
        private int _idCampanhaRecrutamentoCurriculo;
        private CampanhaRecrutamento _campanhaRecrutamento;
        private Curriculo _curriculo;
        private TipoMensagemCS _tipoMensagemCS;
        private DateTime _dataCadastro;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdCampanhaRecrutamentoCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdCampanhaRecrutamentoCurriculo
        {
            get
            {
                return this._idCampanhaRecrutamentoCurriculo;
            }
        }
        #endregion

        #region CampanhaRecrutamento
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public CampanhaRecrutamento CampanhaRecrutamento
        {
            get
            {
                return this._campanhaRecrutamento;
            }
            set
            {
                this._campanhaRecrutamento = value;
                this._modified = true;
            }
        }
        #endregion

        #region Curriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Curriculo Curriculo
        {
            get
            {
                return this._curriculo;
            }
            set
            {
                this._curriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region TipoMensagemCS
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public TipoMensagemCS TipoMensagemCS
        {
            get
            {
                return this._tipoMensagemCS;
            }
            set
            {
                this._tipoMensagemCS = value;
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

        #endregion

        #region Construtores
        public CampanhaRecrutamentoCurriculo()
        {
        }
        public CampanhaRecrutamentoCurriculo(int idCampanhaRecrutamentoCurriculo)
        {
            this._idCampanhaRecrutamentoCurriculo = idCampanhaRecrutamentoCurriculo;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Campanha_Recrutamento_Curriculo (Idf_Campanha_Recrutamento, Idf_Curriculo, Idf_Tipo_Mensagem_CS, Dta_Cadastro) VALUES (@Idf_Campanha_Recrutamento, @Idf_Curriculo, @Idf_Tipo_Mensagem_CS, @Dta_Cadastro);SET @Idf_Campanha_Recrutamento_Curriculo = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Campanha_Recrutamento_Curriculo SET Idf_Campanha_Recrutamento = @Idf_Campanha_Recrutamento, Idf_Curriculo = @Idf_Curriculo, Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Campanha_Recrutamento_Curriculo = @Idf_Campanha_Recrutamento_Curriculo";
        private const string SPDELETE = "DELETE FROM BNE_Campanha_Recrutamento_Curriculo WHERE Idf_Campanha_Recrutamento_Curriculo = @Idf_Campanha_Recrutamento_Curriculo";
        private const string SPSELECTID = "SELECT * FROM BNE_Campanha_Recrutamento_Curriculo WITH(NOLOCK) WHERE Idf_Campanha_Recrutamento_Curriculo = @Idf_Campanha_Recrutamento_Curriculo";
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
            parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
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
            parms[0].Value = this._idCampanhaRecrutamentoCurriculo;
            parms[1].Value = this._campanhaRecrutamento.IdCampanhaRecrutamento;
            parms[2].Value = this._curriculo.IdCurriculo;
            parms[3].Value = this._tipoMensagemCS.IdTipoMensagemCS;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[4].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de CampanhaRecrutamentoCurriculo no banco de dados.
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
                        this._idCampanhaRecrutamentoCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Campanha_Recrutamento_Curriculo"].Value);
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
        /// Método utilizado para inserir uma instância de CampanhaRecrutamentoCurriculo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idCampanhaRecrutamentoCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Campanha_Recrutamento_Curriculo"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de CampanhaRecrutamentoCurriculo no banco de dados.
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
        /// Método utilizado para atualizar uma instância de CampanhaRecrutamentoCurriculo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de CampanhaRecrutamentoCurriculo no banco de dados.
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
        /// Método utilizado para salvar uma instância de CampanhaRecrutamentoCurriculo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de CampanhaRecrutamentoCurriculo no banco de dados.
        /// </summary>
        /// <param name="idCampanhaRecrutamentoCurriculo">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCampanhaRecrutamentoCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaRecrutamentoCurriculo;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de CampanhaRecrutamentoCurriculo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCampanhaRecrutamentoCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCampanhaRecrutamentoCurriculo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaRecrutamentoCurriculo;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de CampanhaRecrutamentoCurriculo no banco de dados.
        /// </summary>
        /// <param name="idCampanhaRecrutamentoCurriculo">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idCampanhaRecrutamentoCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Campanha_Recrutamento_Curriculo where Idf_Campanha_Recrutamento_Curriculo in (";

            for (int i = 0; i < idCampanhaRecrutamentoCurriculo.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idCampanhaRecrutamentoCurriculo[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCampanhaRecrutamentoCurriculo">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCampanhaRecrutamentoCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaRecrutamentoCurriculo;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCampanhaRecrutamentoCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCampanhaRecrutamentoCurriculo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Recrutamento_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaRecrutamentoCurriculo;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cam.Idf_Campanha_Recrutamento_Curriculo, Cam.Idf_Campanha_Recrutamento, Cam.Idf_Curriculo, Cam.Idf_Tipo_Mensagem_CS, Cam.Dta_Cadastro FROM BNE_Campanha_Recrutamento_Curriculo Cam";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de CampanhaRecrutamentoCurriculo a partir do banco de dados.
        /// </summary>
        /// <param name="idCampanhaRecrutamentoCurriculo">Chave do registro.</param>
        /// <returns>Instância de CampanhaRecrutamentoCurriculo.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CampanhaRecrutamentoCurriculo LoadObject(int idCampanhaRecrutamentoCurriculo)
        {
            using (IDataReader dr = LoadDataReader(idCampanhaRecrutamentoCurriculo))
            {
                CampanhaRecrutamentoCurriculo objCampanhaRecrutamentoCurriculo = new CampanhaRecrutamentoCurriculo();
                if (SetInstance(dr, objCampanhaRecrutamentoCurriculo))
                    return objCampanhaRecrutamentoCurriculo;
            }
            throw (new RecordNotFoundException(typeof(CampanhaRecrutamentoCurriculo)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de CampanhaRecrutamentoCurriculo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCampanhaRecrutamentoCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de CampanhaRecrutamentoCurriculo.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CampanhaRecrutamentoCurriculo LoadObject(int idCampanhaRecrutamentoCurriculo, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idCampanhaRecrutamentoCurriculo, trans))
            {
                CampanhaRecrutamentoCurriculo objCampanhaRecrutamentoCurriculo = new CampanhaRecrutamentoCurriculo();
                if (SetInstance(dr, objCampanhaRecrutamentoCurriculo))
                    return objCampanhaRecrutamentoCurriculo;
            }
            throw (new RecordNotFoundException(typeof(CampanhaRecrutamentoCurriculo)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de CampanhaRecrutamentoCurriculo a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idCampanhaRecrutamentoCurriculo))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de CampanhaRecrutamentoCurriculo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idCampanhaRecrutamentoCurriculo, trans))
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
        /// <param name="objCampanhaRecrutamentoCurriculo">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, CampanhaRecrutamentoCurriculo objCampanhaRecrutamentoCurriculo, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objCampanhaRecrutamentoCurriculo._idCampanhaRecrutamentoCurriculo = Convert.ToInt32(dr["Idf_Campanha_Recrutamento_Curriculo"]);
                    objCampanhaRecrutamentoCurriculo._campanhaRecrutamento = new CampanhaRecrutamento(Convert.ToInt32(dr["Idf_Campanha_Recrutamento"]));
                    objCampanhaRecrutamentoCurriculo._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    objCampanhaRecrutamentoCurriculo._tipoMensagemCS = new TipoMensagemCS(Convert.ToInt32(dr["Idf_Tipo_Mensagem_CS"]));
                    objCampanhaRecrutamentoCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                    objCampanhaRecrutamentoCurriculo._persisted = true;
                    objCampanhaRecrutamentoCurriculo._modified = false;

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