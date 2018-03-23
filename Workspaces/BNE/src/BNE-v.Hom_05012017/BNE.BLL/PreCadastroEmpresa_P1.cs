using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class PreCadastroEmpresa
    {
        #region Atributos
        private int _idPreCadastroEmpresa;
        private string _nome;
        private string _email;
        private bool _flagInativo;

        private DateTime _dataCadastro;
        private DateTime _dataEnvio;

        private bool _persisted;
        private bool _modified;

        private int _idOrigemPreCadastro;

        #endregion

        #region Propriedades

        #region idPreCadastroEmpresa
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int idPreCadastroEmpresa
        {
            get
            {
                return this._idPreCadastroEmpresa;
            }
            set
            {
                this._idPreCadastroEmpresa = value;
                this._modified = true;
            }
        }
        #endregion

        #region nome
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string nome
        {
            get
            {
                return this._nome;
            }
            set
            {
                this._nome = value;
                this._modified = true;
            }
        }
        #endregion

        #region Email
        /// <summary>
        /// Tamanho do campo 1000.
        /// </summary>
        public string email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
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

        #region DataEnvio
        public DateTime DataEnvio
        {
            get
            {
                return this._dataEnvio;
            }
            set
            {
                this._dataEnvio = value;
                this._modified = true;
            }
        }
        #endregion

        #region idOrigemPreCadastro
        public int idOrigemPreCadastro
        {
            get
            {
                return this._idOrigemPreCadastro;
            }
            set
            {
                this._idOrigemPreCadastro = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public PreCadastroEmpresa()
        {
        }
        public PreCadastroEmpresa(int idPreCadastroEmpresa)
        {
            this._idPreCadastroEmpresa = idPreCadastroEmpresa;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE.BNE_Pre_Cadastro_Empresa (Nme_Pessoa, Eml_Pessoa, Flg_Inativo, Dta_Cadastro, idf_Origem_Pre_Cadastro ) VALUES (@Nme_Pessoa, @Eml_Pessoa, @Flg_Inativo, @Dta_Cadastro, @idf_Origem_Pre_Cadastro);";
        private const string SPUPDATE = "UPDATE BNE.BNE_Pre_Cadastro_Empresa SET Nme_Pessoa = @Nme_Pessoa, Eml_Pessoa = @Eml_Pessoa, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, idf_Origem_Pre_Cadastro = @idf_Origem_Pre_Cadastro WHERE Eml_Pessoa = @Eml_Pessoa and Nme_Pessoa = @Nme_Pessoa";
       // private const string SPDELETE = "UPDATE BNE.BNE_Pre_Cadastro SET flg_inativo = 1 WHERE Eml_Pessoa = @Eml_Pessoa and Nme_Pessoa = @Nme_Pessoa";
       // private const string SPSELECTID = "SELECT * FROM plataforma.TAB_Motivo_Rescisao WITH(NOLOCK) WHERE Idf_Motivo_Rescisao = @Idf_Motivo_Rescisao";
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
            parms.Add(new SqlParameter("@idf_Pre_Cadastro_Empresa", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@idf_Origem_Pre_Cadastro", SqlDbType.Int, 4));
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
            parms[0].Value = this._idPreCadastroEmpresa;
            parms[1].Value = this._nome;
            parms[2].Value = this._email;
            parms[3].Value = this._flagInativo;

            if (!this._persisted)
            {
                this._dataCadastro = DateTime.Now;
            }
            parms[4].Value = this._dataCadastro;
            parms[5].Value = this._idOrigemPreCadastro;

        }
        #endregion


        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de MotivoRescisao no banco de dados.
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
        /// Método utilizado para inserir uma instância de MotivoRescisao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
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
        /// Método utilizado para atualizar uma instância de MotivoRescisao no banco de dados.
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
        /// Método utilizado para atualizar uma instância de MotivoRescisao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de MotivoRescisao no banco de dados.
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
        /// Método utilizado para salvar uma instância de MotivoRescisao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de MotivoRescisao no banco de dados.
        /// </summary>
        /// <param name="idMotivoRescisao">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        //public static void Delete(String EmlPessoa, String NmePessoa)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100));
        //    parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 100));

        //    parms[0].Value = EmlPessoa;
        //    parms[1].Value = NmePessoa;

        //    DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        //}
        ///// <summary>
        ///// Método utilizado para excluir uma instância de MotivoRescisao no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="idMotivoRescisao">Chave do registro.</param>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static void Delete(int idMotivoRescisao, SqlTransaction trans)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Idf_Motivo_Rescisao", SqlDbType.Int, 4));

        //    parms[0].Value = idMotivoRescisao;

        //    DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        //}
        ///// <summary>
        ///// Método utilizado para excluir uma lista de MotivoRescisao no banco de dados.
        ///// </summary>
        ///// <param name="idMotivoRescisao">Lista de chaves.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static void Delete(List<int> idMotivoRescisao)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    string query = "delete from plataforma.TAB_Motivo_Rescisao where Idf_Motivo_Rescisao in (";

        //    for (int i = 0; i < idMotivoRescisao.Count; i++)
        //    {
        //        string nomeParametro = "@parm" + i.ToString();

        //        if (i > 0)
        //        {
        //            query += ", ";
        //        }
        //        query += nomeParametro;
        //        parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
        //        parms[i].Value = idMotivoRescisao[i];
        //    }

        //    query += ")";

        //    DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        //}
        //#endregion

        //#region LoadDataReader
        ///// <summary>
        ///// Método utilizado por retornar as colunas de um registro no banco de dados.
        ///// </summary>
        ///// <param name="idMotivoRescisao">Chave do registro.</param>
        ///// <returns>Cursor de leitura do banco de dados.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private static IDataReader LoadDataReader(int idMotivoRescisao)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Idf_Motivo_Rescisao", SqlDbType.Int, 4));

        //    parms[0].Value = idMotivoRescisao;

        //    return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        //}
        ///// <summary>
        ///// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="idMotivoRescisao">Chave do registro.</param>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <returns>Cursor de leitura do banco de dados.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private static IDataReader LoadDataReader(int idMotivoRescisao, SqlTransaction trans)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Idf_Motivo_Rescisao", SqlDbType.Int, 4));

        //    parms[0].Value = idMotivoRescisao;

        //    return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
        //}
        ///// <summary>
        ///// Método utilizado por retornar uma consulta paginada do banco de dados.
        ///// </summary>
        ///// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
        ///// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
        ///// <param name="paginaCorrente">Número da página que será exibida.</param>
        ///// <param name="tamanhoPagina">Quantidade de itens em cada página.</param>
        ///// <param name="totalRegistros">Quantidade total de registros na tabela.</param>
        ///// <returns>Cursor de leitura do banco de dados.</returns>
        //public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        //{
        //    int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
        //    int fim = paginaCorrente * tamanhoPagina;

        //    string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Mot.Idf_Motivo_Rescisao, Mot.Des_Motivo_Rescisao, Mot.Flg_Inativo, Mot.Dta_Cadastro, Mot.Sig_Causa_Afastamento, Mot.Sig_Codigo_Afastamento FROM plataforma.TAB_Motivo_Rescisao Mot";
        //    string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
        //    SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

        //    totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
        //    return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        //}
        //#endregion

        //#region LoadObject
        ///// <summary>
        ///// Método utilizado para retornar uma instância de MotivoRescisao a partir do banco de dados.
        ///// </summary>
        ///// <param name="idMotivoRescisao">Chave do registro.</param>
        ///// <returns>Instância de MotivoRescisao.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static MotivoRescisao LoadObject(int idMotivoRescisao)
        //{
        //    using (IDataReader dr = LoadDataReader(idMotivoRescisao))
        //    {
        //        MotivoRescisao objMotivoRescisao = new MotivoRescisao();
        //        // if (SetInstance(dr, objMotivoRescisao))
        //        return objMotivoRescisao;
        //    }
        //    throw (new RecordNotFoundException(typeof(MotivoRescisao)));
        //}
        ///// <summary>
        ///// Método utilizado para retornar uma instância de MotivoRescisao a partir do banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="idMotivoRescisao">Chave do registro.</param>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <returns>Instância de MotivoRescisao.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static MotivoRescisao LoadObject(int idMotivoRescisao, SqlTransaction trans)
        //{
        //    using (IDataReader dr = LoadDataReader(idMotivoRescisao, trans))
        //    {
        //        MotivoRescisao objMotivoRescisao = new MotivoRescisao();
        //        // if (SetInstance(dr, objMotivoRescisao))
        //        return objMotivoRescisao;
        //    }
        //    throw (new RecordNotFoundException(typeof(MotivoRescisao)));
        //}
        //#endregion

        //#region CompleteObject
        ///// <summary>
        ///// Método utilizado para completar uma instância de MotivoRescisao a partir do banco de dados.
        ///// </summary>
        ///// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public bool CompleteObject()
        //{
        //    using (IDataReader dr = LoadDataReader(this._idPreCadastroEmpresa))
        //    {
        //        return SetInstance(dr, this);
        //    }
        //}
        ///// <summary>
        ///// Método utilizado para completar uma instância de MotivoRescisao a partir do banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public bool CompleteObject(SqlTransaction trans)
        //{
        //    using (IDataReader dr = LoadDataReader(this._idPreCadastroEmpresa, trans))
        //    {
        //        return SetInstance(dr, this);
        //    }
        //}
        //#endregion

        //#region SetInstance
        ///// <summary>
        ///// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        ///// </summary>
        ///// <param name="dr">Cursor de leitura do banco de dados.</param>
        ///// <param name="objPreCadastro">Instância a ser manipulada.</param>
        ///// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private static bool SetInstance(IDataReader dr, PreCadastro objPreCadastro)
        //{
        //    try
        //    {
        //        if (dr.Read())
        //        {
        //            //objPreCadastro._idMotivoRescisao = Convert.ToInt32(dr["Idf_Motivo_Rescisao"]);
        //            //objPreCadastro._descricaoMotivoRescisao = Convert.ToString(dr["Des_Motivo_Rescisao"]);
        //            //objPreCadastro._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
        //            //objPreCadastro._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
        //            //if (dr["Sig_Causa_Afastamento"] != DBNull.Value)
        //            //    objPreCadastro._siglaCausaAfastamento = Convert.ToString(dr["Sig_Causa_Afastamento"]);
        //            //if (dr["Sig_Codigo_Afastamento"] != DBNull.Value)
        //            //    objPreCadastro._siglaCodigoAfastamento = Convert.ToString(dr["Sig_Codigo_Afastamento"]);

        //            //objPreCadastro._persisted = true;
        //            //objPreCadastro._modified = false;

        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        dr.Dispose();
        //    }
        //}
        #endregion

        #endregion
    }
}


