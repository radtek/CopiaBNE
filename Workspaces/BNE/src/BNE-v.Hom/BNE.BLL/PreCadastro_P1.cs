using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PreCadastro
    {
        #region Atributos
        private int _idPreCadastro;
        private string _nome;
        private string _email;
        private int _idFuncao;
        private int _idCidade;
        private bool _flagInativo;

        private DateTime _dataCadastro;
        private DateTime _dataEnvio;

        private bool _persisted;
        private bool _modified;

        private int _idOrigemPreCadastro;

        #endregion

        #region Propriedades

        #region idPreCadastro
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int idPreCadastro
        {
            get
            {
                return this._idPreCadastro;
            }
            set
            {
                this._idPreCadastro = value;
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

        #region idCidade
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int idCidade
        {
            get
            {
                return this._idCidade;
            }
            set
            {
                this._idCidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region idFuncao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int idFuncao
        {
            get
            {
                return this._idFuncao;
            }
            set
            {
                this._idFuncao = value;
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
        public PreCadastro()
        {
        }
        public PreCadastro(int idPreCadastro)
        {
            this._idPreCadastro = idPreCadastro;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE.BNE_Pre_Cadastro (Nme_Pessoa, Eml_Pessoa,idf_Funcao,idf_Cidade, Flg_Inativo, Dta_Cadastro, idf_Origem_Pre_Cadastro ) VALUES (@Nme_Pessoa, @Eml_Pessoa, @Idf_Funcao, @Idf_Cidade, @Flg_Inativo, @Dta_Cadastro, @idf_Origem_Pre_Cadastro); SET @idf_Pre_Cadastro = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE.BNE_Pre_Cadastro SET Nme_Pessoa = @Nme_Pessoa, Eml_Pessoa = @Eml_Pessoa, idf_Funcao = @idf_Funcao, idf_Cidade = @idf_Cidade, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, idf_Origem_Pre_Cadastro = @idf_Origem_Pre_Cadastro WHERE Eml_Pessoa = @Eml_Pessoa and Nme_Pessoa = @Nme_Pessoa";
        private const string SPDELETE = "UPDATE BNE.BNE_Pre_Cadastro SET flg_inativo = 1 WHERE Eml_Pessoa = @Eml_Pessoa and idf_Origem_Pre_Cadastro = @idf_Origem";
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
            parms.Add(new SqlParameter("@idf_Pre_Cadastro", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@idf_Funcao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@idf_Cidade", SqlDbType.Int, 4));
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
            parms[0].Value = this._idPreCadastro;
            parms[0].Direction = ParameterDirection.InputOutput;

            parms[1].Value = this._nome;
            parms[2].Value = this._email;

            if (this._idFuncao > 0)
                parms[3].Value = this._idFuncao;
            else
                parms[3].Value = DBNull.Value;
            if (this._idCidade > 0)
                parms[4].Value = this._idCidade;
            else
                parms[4].Value = DBNull.Value;

            parms[5].Value = this._flagInativo;

            if (!this._persisted)
            {
                this._dataCadastro = DateTime.Now;
            }
            parms[6].Value = this._dataCadastro;
            parms[7].Value = this._idOrigemPreCadastro;

            

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
                        this._idPreCadastro = Convert.ToInt32(cmd.Parameters["@idf_Pre_Cadastro"].Value);
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
        /// 
        /// </summary>
        /// <param name="">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(String EmlPessoa, int idOrigem)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@idf_Origem", SqlDbType.Int, 4));

            parms[0].Value = EmlPessoa;
            parms[1].Value = idOrigem;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        #endregion

        #endregion
    }
}


