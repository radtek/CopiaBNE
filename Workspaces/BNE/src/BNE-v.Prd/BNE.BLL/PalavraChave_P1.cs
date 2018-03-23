//TODO: OLD - Não estava sendo usado no projeto, comentado para realizar exclusão da tabela

////-- Data: 08/02/2012 15:30
////-- Autor: Gieyson Stelmak

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using BNE.EL;

//namespace BNE.BLL
//{
//    public partial class PalavraChave // Tabela: BNE_Palavra_Chave
//    {
//        #region Atributos
//        private int _idPalavraChave;
//        private string _descricaoPalavraChave;
//        private DateTime _dataCadastro;
//        private bool _flagInativo;

//        private bool _persisted;
//        private bool _modified;
//        #endregion

//        #region Propriedades

//        #region IdPalavraChave
//        /// <summary>
//        /// Campo obrigatório.
//        /// Campo auto-numerado.
//        /// </summary>
//        public int IdPalavraChave
//        {
//            get
//            {
//                return this._idPalavraChave;
//            }
//        }
//        #endregion 

//        #region DescricaoPalavraChave
//        /// <summary>
//        /// Tamanho do campo: 30.
//        /// Campo obrigatório.
//        /// </summary>
//        public string DescricaoPalavraChave
//        {
//            get
//            {
//                return this._descricaoPalavraChave;
//            }
//            set
//            {
//                this._descricaoPalavraChave = value;
//                this._modified = true;
//            }
//        }
//        #endregion 

//        #region DataCadastro
//        /// <summary>
//        /// Campo obrigatório.
//        /// </summary>
//        public DateTime DataCadastro
//        {
//            get
//            {
//                return this._dataCadastro;
//            }
//        }
//        #endregion 

//        #region FlagInativo
//        /// <summary>
//        /// Campo obrigatório.
//        /// </summary>
//        public bool FlagInativo
//        {
//            get
//            {
//                return this._flagInativo;
//            }
//            set
//            {
//                this._flagInativo = value;
//                this._modified = true;
//            }
//        }
//        #endregion 

//        #endregion

//        #region Construtores
//        public PalavraChave()
//        {
//        }
//        public PalavraChave(int idPalavraChave)
//        {
//            this._idPalavraChave = idPalavraChave;
//            this._persisted = true;
//        }
//        #endregion

//        #region Consultas
//        private const string SPINSERT = "INSERT INTO BNE_Palavra_Chave (Des_Palavra_Chave, Dta_Cadastro, Flg_Inativo) VALUES (@Des_Palavra_Chave, @Dta_Cadastro, @Flg_Inativo);SET @Idf_Palavra_Chave = SCOPE_IDENTITY();";
//        private const string SPUPDATE = "UPDATE BNE_Palavra_Chave SET Des_Palavra_Chave = @Des_Palavra_Chave, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo WHERE Idf_Palavra_Chave = @Idf_Palavra_Chave";
//        private const string SPDELETE = "DELETE FROM BNE_Palavra_Chave WHERE Idf_Palavra_Chave = @Idf_Palavra_Chave";
//        private const string SPSELECTID = "SELECT * FROM BNE_Palavra_Chave WHERE Idf_Palavra_Chave = @Idf_Palavra_Chave";
//        #endregion

//        #region Métodos

//        #region GetParameters
//        /// <summary>
//        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
//        /// </summary>
//        /// <returns>Lista de parâmetros SQL.</returns>
//        /// <remarks>Gieyson Stelmak</remarks>
//        private List<SqlParameter> GetParameters()
//        {
//            List<SqlParameter> parms = new List<SqlParameter>();
//            parms.Add(new SqlParameter("@Idf_Palavra_Chave", SqlDbType.Int, 4));
//            parms.Add(new SqlParameter("@Des_Palavra_Chave", SqlDbType.VarChar, 30));
//            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
//            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
//            return(parms);
//        }
//        #endregion

//        #region SetParameters
//        /// <summary>
//        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
//        /// </summary>
//        /// <param name="parms">Lista de parâmetros SQL.</param>
//        /// <remarks>Gieyson Stelmak</remarks>
//        private void SetParameters(List<SqlParameter> parms)
//        {
//            parms[0].Value = this._idPalavraChave;
//            parms[1].Value = this._descricaoPalavraChave;
//            parms[3].Value = this._flagInativo;

//            if (!this._persisted)
//            {
//                parms[0].Direction = ParameterDirection.Output;
//                this._dataCadastro = DateTime.Now;
//            }
//            else
//            {
//                parms[0].Direction = ParameterDirection.Input;
//            }
//            parms[2].Value = this._dataCadastro;
//        }
//        #endregion

//        #region Insert
//        /// <summary>
//        /// Método utilizado para inserir uma instância de PalavraChave no banco de dados.
//        /// </summary>
//        /// <remarks>Gieyson Stelmak</remarks>
//        private void Insert()
//        {
//            List<SqlParameter> parms = GetParameters();
//            SetParameters(parms);

//            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
//            {
//                conn.Open();
//                using (SqlTransaction trans = conn.BeginTransaction())
//                {
//                    try
//                    {
//                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
//                        this._idPalavraChave = Convert.ToInt32(cmd.Parameters["@Idf_Palavra_Chave"].Value);
//                        cmd.Parameters.Clear();
//                        this._persisted = true;
//                        this._modified = false;
//                        trans.Commit();
//                    }
//                    catch
//                    {
//                        trans.Rollback();
//                        throw;
//                    }
//                }
//            }
//        }
//        /// <summary>
//        /// Método utilizado para inserir uma instância de PalavraChave no banco de dados, dentro de uma transação.
//        /// </summary>
//        /// <param name="trans">Transação existente no banco de dados.</param>
//        /// <remarks>Gieyson Stelmak</remarks>
//        private void Insert(SqlTransaction trans)
//        {
//            List<SqlParameter> parms = GetParameters();
//            SetParameters(parms);
//            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
//            this._idPalavraChave = Convert.ToInt32(cmd.Parameters["@Idf_Palavra_Chave"].Value);
//            cmd.Parameters.Clear();
//            this._persisted = true;
//            this._modified = false;
//        }
//        #endregion

//        #region Update
//        /// <summary>
//        /// Método utilizado para atualizar uma instância de PalavraChave no banco de dados.
//        /// </summary>
//        /// <remarks>Gieyson Stelmak</remarks>
//        private void Update()
//        {
//            if (this._modified)
//            {
//                List<SqlParameter> parms = GetParameters();
//                SetParameters(parms);
//                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATE, parms);
//                this._modified = false;
//            }
//        }
//        /// <summary>
//        /// Método utilizado para atualizar uma instância de PalavraChave no banco de dados, dentro de uma transação.
//        /// </summary>
//        /// <param name="trans">Transação existente no banco de dados.</param>
//        /// <remarks>Gieyson Stelmak</remarks>
//        private void Update(SqlTransaction trans)
//        {
//            if (this._modified)
//            {
//                List<SqlParameter> parms = GetParameters();
//                SetParameters(parms);
//                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
//                this._modified = false;
//            }
//        }
//        #endregion

//        #region Save
//        /// <summary>
//        /// Método utilizado para salvar uma instância de PalavraChave no banco de dados.
//        /// </summary>
//        /// <remarks>Gieyson Stelmak</remarks>
//        public void Save()
//        {
//            if (!this._persisted)
//                this.Insert();
//            else
//                this.Update();
//        }
//        /// <summary>
//        /// Método utilizado para salvar uma instância de PalavraChave no banco de dados, dentro de uma transação.
//        /// </summary>
//        /// <param name="trans">Transação existente no banco de dados.</param>
//        /// <remarks>Gieyson Stelmak</remarks>
//        public void Save(SqlTransaction trans)
//        {
//            if (!this._persisted)
//                this.Insert(trans);
//            else
//                this.Update(trans);
//        }
//        #endregion

//        #region Delete
//        /// <summary>
//        /// Método utilizado para excluir uma instância de PalavraChave no banco de dados.
//        /// </summary>
//        /// <param name="idPalavraChave">Chave do registro.</param>
//        /// <remarks>Gieyson Stelmak</remarks>
//        public static void Delete(int idPalavraChave)
//        {
//            List<SqlParameter> parms = new List<SqlParameter>();
//            parms.Add(new SqlParameter("@Idf_Palavra_Chave", SqlDbType.Int, 4));

//            parms[0].Value = idPalavraChave;

//            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
//        }
//        /// <summary>
//        /// Método utilizado para excluir uma instância de PalavraChave no banco de dados, dentro de uma transação.
//        /// </summary>
//        /// <param name="idPalavraChave">Chave do registro.</param>
//        /// <param name="trans">Transação existente no banco de dados.</param>
//        /// <remarks>Gieyson Stelmak</remarks>
//        public static void Delete(int idPalavraChave, SqlTransaction trans)
//        {
//            List<SqlParameter> parms = new List<SqlParameter>();
//            parms.Add(new SqlParameter("@Idf_Palavra_Chave", SqlDbType.Int, 4));

//            parms[0].Value = idPalavraChave;

//            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
//        }
//        /// <summary>
//        /// Método utilizado para excluir uma lista de PalavraChave no banco de dados.
//        /// </summary>
//        /// <param name="idPalavraChave">Lista de chaves.</param>
//        /// <remarks>Gieyson Stelmak</remarks>
//        public static void Delete(List<int> idPalavraChave)
//        {
//            List<SqlParameter> parms = new List<SqlParameter>();
//            string query = "delete from BNE_Palavra_Chave where Idf_Palavra_Chave in (";

//            for (int i = 0; i < idPalavraChave.Count; i++)
//            {
//                string nomeParametro = "@parm" + i.ToString();

//                if (i > 0)
//                {
//                    query += ", ";
//                }
//                query += nomeParametro;
//                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
//                parms[i].Value = idPalavraChave[i];
//            }

//            query += ")";

//            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
//        }
//        #endregion

//        #region LoadDataReader
//        /// <summary>
//        /// Método utilizado por retornar as colunas de um registro no banco de dados.
//        /// </summary>
//        /// <param name="idPalavraChave">Chave do registro.</param>
//        /// <returns>Cursor de leitura do banco de dados.</returns>
//        /// <remarks>Gieyson Stelmak</remarks>
//        private static IDataReader LoadDataReader(int idPalavraChave)
//        {
//            List<SqlParameter> parms = new List<SqlParameter>();
//            parms.Add(new SqlParameter("@Idf_Palavra_Chave", SqlDbType.Int, 4));

//            parms[0].Value = idPalavraChave;

//            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
//        }
//        /// <summary>
//        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
//        /// </summary>
//        /// <param name="idPalavraChave">Chave do registro.</param>
//        /// <param name="trans">Transação existente no banco de dados.</param>
//        /// <returns>Cursor de leitura do banco de dados.</returns>
//        /// <remarks>Gieyson Stelmak</remarks>
//        private static IDataReader LoadDataReader(int idPalavraChave, SqlTransaction trans)
//        {
//            List<SqlParameter> parms = new List<SqlParameter>();
//            parms.Add(new SqlParameter("@Idf_Palavra_Chave", SqlDbType.Int, 4));

//            parms[0].Value = idPalavraChave;

//            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
//        }
//        /// <summary>
//        /// Método utilizado por retornar uma consulta paginada do banco de dados.
//        /// </summary>
//        /// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
//        /// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
//        /// <param name="paginaCorrente">Número da página que será exibida.</param>
//        /// <param name="tamanhoPagina">Quantidade de itens em cada página.</param>
//        /// <param name="totalRegistros">Quantidade total de registros na tabela.</param>
//        /// <returns>Cursor de leitura do banco de dados.</returns>
//        public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
//        {
//            int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
//            int fim = paginaCorrente * tamanhoPagina;

//            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pal.Idf_Palavra_Chave, Pal.Des_Palavra_Chave, Pal.Dta_Cadastro, Pal.Flg_Inativo FROM BNE_Palavra_Chave Pal";
//            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
//            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

//            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
//            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
//        }
//        #endregion

//        #region LoadObject
//        /// <summary>
//        /// Método utilizado para retornar uma instância de PalavraChave a partir do banco de dados.
//        /// </summary>
//        /// <param name="idPalavraChave">Chave do registro.</param>
//        /// <returns>Instância de PalavraChave.</returns>
//        /// <remarks>Gieyson Stelmak</remarks>
//        public static PalavraChave LoadObject(int idPalavraChave)
//        {
//            using (IDataReader dr = LoadDataReader(idPalavraChave))
//            {
//                PalavraChave objPalavraChave = new PalavraChave();
//                if (SetInstance(dr, objPalavraChave))
//                    return objPalavraChave;
//            }
//            throw (new RecordNotFoundException(typeof(PalavraChave)));
//        }
//        /// <summary>
//        /// Método utilizado para retornar uma instância de PalavraChave a partir do banco de dados, dentro de uma transação.
//        /// </summary>
//        /// <param name="idPalavraChave">Chave do registro.</param>
//        /// <param name="trans">Transação existente no banco de dados.</param>
//        /// <returns>Instância de PalavraChave.</returns>
//        /// <remarks>Gieyson Stelmak</remarks>
//        public static PalavraChave LoadObject(int idPalavraChave, SqlTransaction trans)
//        {
//            using (IDataReader dr = LoadDataReader(idPalavraChave, trans))
//            {
//                PalavraChave objPalavraChave = new PalavraChave();
//                if (SetInstance(dr, objPalavraChave))
//                    return objPalavraChave;
//            }
//            throw (new RecordNotFoundException(typeof(PalavraChave)));
//        }
//        #endregion

//        #region CompleteObject
//        /// <summary>
//        /// Método utilizado para completar uma instância de PalavraChave a partir do banco de dados.
//        /// </summary>
//        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
//        /// <remarks>Gieyson Stelmak</remarks>
//        public bool CompleteObject()
//        {
//            using (IDataReader dr = LoadDataReader(this._idPalavraChave))
//            {
//                return SetInstance(dr, this);
//            }
//        }
//        /// <summary>
//        /// Método utilizado para completar uma instância de PalavraChave a partir do banco de dados, dentro de uma transação.
//        /// </summary>
//        /// <param name="trans">Transação existente no banco de dados.</param>
//        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
//        /// <remarks>Gieyson Stelmak</remarks>
//        public bool CompleteObject(SqlTransaction trans)
//        {
//            using (IDataReader dr = LoadDataReader(this._idPalavraChave, trans))
//            {
//                return SetInstance(dr, this);
//            }
//        }
//        #endregion

//        #region SetInstance
//        /// <summary>
//        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
//        /// </summary>
//        /// <param name="dr">Cursor de leitura do banco de dados.</param>
//        /// <param name="objPalavraChave">Instância a ser manipulada.</param>
//        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
//        /// <remarks>Gieyson Stelmak</remarks>
//        private static bool SetInstance(IDataReader dr, PalavraChave objPalavraChave)
//        {
//            try
//            {
//                if (dr.Read())
//                {
//                    objPalavraChave._idPalavraChave = Convert.ToInt32(dr["Idf_Palavra_Chave"]);
//                    objPalavraChave._descricaoPalavraChave = Convert.ToString(dr["Des_Palavra_Chave"]);
//                    objPalavraChave._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
//                    objPalavraChave._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

//                    objPalavraChave._persisted = true;
//                    objPalavraChave._modified = false;

//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch 
//            {
//                throw;
//            }
//            finally
//            {
//                dr.Dispose();
//            }
//        }
//        #endregion

//        #endregion
//    }
//}