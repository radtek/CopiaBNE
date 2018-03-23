//-- Data: 19/01/2016 11:13
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class RastreadorCurriculoIdioma // Tabela: BNE_Rastreador_Curriculo_Idioma
    {
        #region Atributos
        private int _idRastreadorIdioma;
        private RastreadorCurriculo _rastreadorCurriculo;
        private Idioma _idioma;
        private NivelIdioma _nivelIdioma;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdRastreadorIdioma
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdRastreadorIdioma
        {
            get
            {
                return this._idRastreadorIdioma;
            }
        }
        #endregion

        #region RastreadorCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public RastreadorCurriculo RastreadorCurriculo
        {
            get
            {
                return this._rastreadorCurriculo;
            }
            set
            {
                this._rastreadorCurriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region Idioma
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Idioma Idioma
        {
            get
            {
                return this._idioma;
            }
            set
            {
                this._idioma = value;
                this._modified = true;
            }
        }
        #endregion

        #region NivelIdioma
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public NivelIdioma NivelIdioma
        {
            get
            {
                return this._nivelIdioma;
            }
            set
            {
                this._nivelIdioma = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public RastreadorCurriculoIdioma()
        {
        }
        public RastreadorCurriculoIdioma(int idRastreadorIdioma)
        {
            this._idRastreadorIdioma = idRastreadorIdioma;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Rastreador_Curriculo_Idioma (Idf_Rastreador_Curriculo, Idf_Idioma, Idf_Nivel_Idioma) VALUES (@Idf_Rastreador_Curriculo, @Idf_Idioma, @Idf_Nivel_Idioma);SET @Idf_Rastreador_Idioma = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Rastreador_Curriculo_Idioma SET Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo, Idf_Idioma = @Idf_Idioma, Idf_Nivel_Idioma = @Idf_Nivel_Idioma WHERE Idf_Rastreador_Idioma = @Idf_Rastreador_Idioma";
        private const string SPDELETE = "DELETE FROM BNE_Rastreador_Curriculo_Idioma WHERE Idf_Rastreador_Idioma = @Idf_Rastreador_Idioma";
        private const string SPSELECTID = "SELECT * FROM BNE_Rastreador_Curriculo_Idioma WITH(NOLOCK) WHERE Idf_Rastreador_Idioma = @Idf_Rastreador_Idioma";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>mailson</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Rastreador_Idioma", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Rastreador_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Idioma", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Nivel_Idioma", SqlDbType.Int, 4));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>mailson</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idRastreadorIdioma;
            parms[1].Value = this._rastreadorCurriculo.IdRastreadorCurriculo;
            parms[2].Value = this._idioma.IdIdioma;

            if (this._nivelIdioma != null)
                parms[3].Value = this._nivelIdioma.IdNivelIdioma;
            else
                parms[3].Value = DBNull.Value;


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
        /// Método utilizado para inserir uma instância de RastreadorCurriculoIdioma no banco de dados.
        /// </summary>
        /// <remarks>mailson</remarks>
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
                        this._idRastreadorIdioma = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Idioma"].Value);
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
        /// Método utilizado para inserir uma instância de RastreadorCurriculoIdioma no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idRastreadorIdioma = Convert.ToInt32(cmd.Parameters["@Idf_Rastreador_Idioma"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de RastreadorCurriculoIdioma no banco de dados.
        /// </summary>
        /// <remarks>mailson</remarks>
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
        /// Método utilizado para atualizar uma instância de RastreadorCurriculoIdioma no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
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
        /// Método utilizado para salvar uma instância de RastreadorCurriculoIdioma no banco de dados.
        /// </summary>
        /// <remarks>mailson</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de RastreadorCurriculoIdioma no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
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
        /// Método utilizado para excluir uma instância de RastreadorCurriculoIdioma no banco de dados.
        /// </summary>
        /// <param name="idRastreadorIdioma">Chave do registro.</param>
        /// <remarks>mailson</remarks>
        public static void Delete(int idRastreadorIdioma)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Rastreador_Idioma", SqlDbType.Int, 4));

            parms[0].Value = idRastreadorIdioma;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de RastreadorCurriculoIdioma no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idRastreadorIdioma">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
        public static void Delete(int idRastreadorIdioma, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Rastreador_Idioma", SqlDbType.Int, 4));

            parms[0].Value = idRastreadorIdioma;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de RastreadorCurriculoIdioma no banco de dados.
        /// </summary>
        /// <param name="idRastreadorIdioma">Lista de chaves.</param>
        /// <remarks>mailson</remarks>
        public static void Delete(List<int> idRastreadorIdioma)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Rastreador_Curriculo_Idioma where Idf_Rastreador_Idioma in (";

            for (int i = 0; i < idRastreadorIdioma.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idRastreadorIdioma[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idRastreadorIdioma">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>mailson</remarks>
        private static IDataReader LoadDataReader(int idRastreadorIdioma)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Rastreador_Idioma", SqlDbType.Int, 4));

            parms[0].Value = idRastreadorIdioma;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idRastreadorIdioma">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>mailson</remarks>
        private static IDataReader LoadDataReader(int idRastreadorIdioma, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Rastreador_Idioma", SqlDbType.Int, 4));

            parms[0].Value = idRastreadorIdioma;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ras.Idf_Rastreador_Idioma, Ras.Idf_Rastreador_Curriculo, Ras.Idf_Idioma, Ras.Idf_Nivel_Idioma FROM BNE_Rastreador_Curriculo_Idioma Ras";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de RastreadorCurriculoIdioma a partir do banco de dados.
        /// </summary>
        /// <param name="idRastreadorIdioma">Chave do registro.</param>
        /// <returns>Instância de RastreadorCurriculoIdioma.</returns>
        /// <remarks>mailson</remarks>
        public static RastreadorCurriculoIdioma LoadObject(int idRastreadorIdioma)
        {
            using (IDataReader dr = LoadDataReader(idRastreadorIdioma))
            {
                RastreadorCurriculoIdioma objRastreadorCurriculoIdioma = new RastreadorCurriculoIdioma();
                if (SetInstance(dr, objRastreadorCurriculoIdioma))
                    return objRastreadorCurriculoIdioma;
            }
            throw (new RecordNotFoundException(typeof(RastreadorCurriculoIdioma)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de RastreadorCurriculoIdioma a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idRastreadorIdioma">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de RastreadorCurriculoIdioma.</returns>
        /// <remarks>mailson</remarks>
        public static RastreadorCurriculoIdioma LoadObject(int idRastreadorIdioma, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idRastreadorIdioma, trans))
            {
                RastreadorCurriculoIdioma objRastreadorCurriculoIdioma = new RastreadorCurriculoIdioma();
                if (SetInstance(dr, objRastreadorCurriculoIdioma))
                    return objRastreadorCurriculoIdioma;
            }
            throw (new RecordNotFoundException(typeof(RastreadorCurriculoIdioma)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de RastreadorCurriculoIdioma a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>mailson</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idRastreadorIdioma))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de RastreadorCurriculoIdioma a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>mailson</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idRastreadorIdioma, trans))
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
        /// <param name="objRastreadorCurriculoIdioma">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>mailson</remarks>
        private static bool SetInstance(IDataReader dr, RastreadorCurriculoIdioma objRastreadorCurriculoIdioma, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objRastreadorCurriculoIdioma._idRastreadorIdioma = Convert.ToInt32(dr["Idf_Rastreador_Idioma"]);
                    objRastreadorCurriculoIdioma._rastreadorCurriculo = new RastreadorCurriculo(Convert.ToInt32(dr["Idf_Rastreador_Curriculo"]));
                    objRastreadorCurriculoIdioma._idioma = new Idioma(Convert.ToInt32(dr["Idf_Idioma"]));
                    if (dr["Idf_Nivel_Idioma"] != DBNull.Value)
                        objRastreadorCurriculoIdioma._nivelIdioma = new NivelIdioma(Convert.ToInt32(dr["Idf_Nivel_Idioma"]));

                    objRastreadorCurriculoIdioma._persisted = true;
                    objRastreadorCurriculoIdioma._modified = false;

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