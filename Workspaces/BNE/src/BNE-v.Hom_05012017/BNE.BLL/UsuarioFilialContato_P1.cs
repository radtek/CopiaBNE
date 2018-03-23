//-- Data: 17/03/2016 17:42
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class UsuarioFilialContato // Tabela: BNE_Usuario_Filial_Contato
    {
        #region Atributos
        private int _idUsuarioFilialContato;
        private string _nomeContato;
        private Funcao _funcao;
        private string _descricaoFuncao;
        private Filial _filial;
        private string _emailContato;
        private string _DDDTelefone;
        private string _numeroTelefone;
        private DateTime _dataCadastro;
        private bool _flagInativo;
        private decimal? _numeroCPF;
        private DateTime? _dataNascimento;
        private Sexo _sexo;
        private string _numeroRamal;
        private string _numeroDDDCelular;
        private string _numeroCelular;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdUsuarioFilialContato
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdUsuarioFilialContato
        {
            get
            {
                return this._idUsuarioFilialContato;
            }
        }
        #endregion

        #region NomeContato
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string NomeContato
        {
            get
            {
                return this._nomeContato;
            }
            set
            {
                this._nomeContato = value;
                this._modified = true;
            }
        }
        #endregion

        #region Funcao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Funcao Funcao
        {
            get
            {
                return this._funcao;
            }
            set
            {
                this._funcao = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoFuncao
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo opcional.
        /// </summary>
        public string DescricaoFuncao
        {
            get
            {
                return this._descricaoFuncao;
            }
            set
            {
                this._descricaoFuncao = value;
                this._modified = true;
            }
        }
        #endregion

        #region Filial
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Filial Filial
        {
            get
            {
                return this._filial;
            }
            set
            {
                this._filial = value;
                this._modified = true;
            }
        }
        #endregion

        #region EmailContato
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string EmailContato
        {
            get
            {
                return this._emailContato;
            }
            set
            {
                this._emailContato = value;
                this._modified = true;
            }
        }
        #endregion

        #region DDDTelefone
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string DDDTelefone
        {
            get
            {
                return this._DDDTelefone;
            }
            set
            {
                this._DDDTelefone = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroTelefone
        /// <summary>
        /// Tamanho do campo: 9.
        /// Campo opcional.
        /// </summary>
        public string NumeroTelefone
        {
            get
            {
                return this._numeroTelefone;
            }
            set
            {
                this._numeroTelefone = value;
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

        #region NumeroCPF
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? NumeroCPF
        {
            get
            {
                return this._numeroCPF;
            }
            set
            {
                this._numeroCPF = value;
                this._modified = true;
            }
        }
        #endregion 

        #region DataNascimento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataNascimento
        {
            get
            {
                return this._dataNascimento;
            }
            set
            {
                this._dataNascimento = value;
                this._modified = true;
            }
        }
        #endregion

        #region Sexo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Sexo Sexo
        {
            get
            {
                return this._sexo;
            }
            set
            {
                this._sexo = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroRamal
        /// <summary>
        /// Tamanho do campo: 4.
        /// Campo opcional.
        /// </summary>
        public string NumeroRamal
        {
            get
            {
                return this._numeroRamal;
            }
            set
            {
                this._numeroRamal = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDDDCelular
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string NumeroDDDCelular
        {
            get
            {
                return this._numeroDDDCelular;
            }
            set
            {
                this._numeroDDDCelular = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroCelular
        /// <summary>
        /// Tamanho do campo: 10.
        /// Campo opcional.
        /// </summary>
        public string NumeroCelular
        {
            get
            {
                return this._numeroCelular;
            }
            set
            {
                this._numeroCelular = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public UsuarioFilialContato()
        {
        }
        public UsuarioFilialContato(int idUsuarioFilialContato)
        {
            this._idUsuarioFilialContato = idUsuarioFilialContato;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Usuario_Filial_Contato (Nme_Contato, Idf_Funcao, Des_Funcao, Idf_Filial, Eml_Contato, DDD_Telefone, Num_Telefone, Dta_Cadastro, Flg_Inativo, Num_CPF, Dta_Nascimento, Idf_Sexo, Num_Ramal, Num_DDD_Celular, Num_Celular) VALUES (@Nme_Contato, @Idf_Funcao, @Des_Funcao, @Idf_Filial, @Eml_Contato, @DDD_Telefone, @Num_Telefone, @Dta_Cadastro, @Flg_Inativo, @Num_CPF, @Dta_Nascimento, @Idf_Sexo, @Num_Ramal, @Num_DDD_Celular, @Num_Celular);SET @Idf_Usuario_Filial_Contato = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Usuario_Filial_Contato SET Nme_Contato = @Nme_Contato, Idf_Funcao = @Idf_Funcao, Des_Funcao = @Des_Funcao, Idf_Filial = @Idf_Filial, Eml_Contato = @Eml_Contato, DDD_Telefone = @DDD_Telefone, Num_Telefone = @Num_Telefone, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Num_CPF = @Num_CPF, Dta_Nascimento = @Dta_Nascimento, Idf_Sexo = @Idf_Sexo, Num_Ramal = @Num_Ramal, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular WHERE Idf_Usuario_Filial_Contato = @Idf_Usuario_Filial_Contato";
        private const string SPDELETE = "DELETE FROM BNE_Usuario_Filial_Contato WHERE Idf_Usuario_Filial_Contato = @Idf_Usuario_Filial_Contato";
        private const string SPSELECTID = "SELECT * FROM BNE_Usuario_Filial_Contato WITH(NOLOCK) WHERE Idf_Usuario_Filial_Contato = @Idf_Usuario_Filial_Contato";
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
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Contato", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Nme_Contato", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Funcao", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Eml_Contato", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@DDD_Telefone", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Telefone", SqlDbType.VarChar, 9));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Dta_Nascimento", SqlDbType.DateTime, 3));
            parms.Add(new SqlParameter("@Idf_Sexo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Ramal", SqlDbType.Char, 4));
            parms.Add(new SqlParameter("@Num_DDD_Celular", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Celular", SqlDbType.Char, 10));
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
            parms[0].Value = this._idUsuarioFilialContato;

            if (!String.IsNullOrEmpty(this._nomeContato))
                parms[1].Value = this._nomeContato;
            else
                parms[1].Value = DBNull.Value;


            if (this._funcao != null)
                parms[2].Value = this._funcao.IdFuncao;
            else
                parms[2].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoFuncao))
                parms[3].Value = this._descricaoFuncao;
            else
                parms[3].Value = DBNull.Value;

            parms[4].Value = this._filial.IdFilial;

            if (!String.IsNullOrEmpty(this._emailContato))
                parms[5].Value = this._emailContato;
            else
                parms[5].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._DDDTelefone))
                parms[6].Value = this._DDDTelefone;
            else
                parms[6].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroTelefone))
                parms[7].Value = this._numeroTelefone;
            else
                parms[7].Value = DBNull.Value;

            parms[9].Value = this._flagInativo;

            if (this._numeroCPF.HasValue)
                parms[10].Value = this._numeroCPF;
            else
                parms[10].Value = DBNull.Value;


            if (this._dataNascimento.HasValue)
                parms[11].Value = this._dataNascimento;
            else
                parms[11].Value = DBNull.Value;


            if (this._sexo != null)
                parms[12].Value = this._sexo.IdSexo;
            else
                parms[12].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroRamal))
                parms[13].Value = this._numeroRamal;
            else
                parms[13].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroDDDCelular))
                parms[14].Value = this._numeroDDDCelular;
            else
                parms[14].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroCelular))
                parms[15].Value = this._numeroCelular;
            else
                parms[15].Value = DBNull.Value;


            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[8].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de UsuarioFilialContato no banco de dados.
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
                        this._idUsuarioFilialContato = Convert.ToInt32(cmd.Parameters["@Idf_Usuario_Filial_Contato"].Value);
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
        /// Método utilizado para inserir uma instância de UsuarioFilialContato no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idUsuarioFilialContato = Convert.ToInt32(cmd.Parameters["@Idf_Usuario_Filial_Contato"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de UsuarioFilialContato no banco de dados.
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
        /// Método utilizado para atualizar uma instância de UsuarioFilialContato no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de UsuarioFilialContato no banco de dados.
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
        /// Método utilizado para salvar uma instância de UsuarioFilialContato no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de UsuarioFilialContato no banco de dados.
        /// </summary>
        /// <param name="idUsuarioFilialContato">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idUsuarioFilialContato)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Contato", SqlDbType.Int, 4));

            parms[0].Value = idUsuarioFilialContato;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de UsuarioFilialContato no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idUsuarioFilialContato">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idUsuarioFilialContato, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Contato", SqlDbType.Int, 4));

            parms[0].Value = idUsuarioFilialContato;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de UsuarioFilialContato no banco de dados.
        /// </summary>
        /// <param name="idUsuarioFilialContato">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idUsuarioFilialContato)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Usuario_Filial_Contato where Idf_Usuario_Filial_Contato in (";

            for (int i = 0; i < idUsuarioFilialContato.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idUsuarioFilialContato[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idUsuarioFilialContato">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idUsuarioFilialContato)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Contato", SqlDbType.Int, 4));

            parms[0].Value = idUsuarioFilialContato;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idUsuarioFilialContato">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idUsuarioFilialContato, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Contato", SqlDbType.Int, 4));

            parms[0].Value = idUsuarioFilialContato;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Usu.Idf_Usuario_Filial_Contato, Usu.Nme_Contato, Usu.Idf_Funcao, Usu.Des_Funcao, Usu.Idf_Filial, Usu.Eml_Contato, Usu.DDD_Telefone, Usu.Num_Telefone, Usu.Dta_Cadastro, Usu.Flg_Inativo, Usu.Num_CPF, Usu.Dta_Nascimento, Usu.Idf_Sexo, Usu.Num_Ramal, Usu.Num_DDD_Celular, Usu.Num_Celular FROM BNE_Usuario_Filial_Contato Usu";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de UsuarioFilialContato a partir do banco de dados.
        /// </summary>
        /// <param name="idUsuarioFilialContato">Chave do registro.</param>
        /// <returns>Instância de UsuarioFilialContato.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static UsuarioFilialContato LoadObject(int idUsuarioFilialContato)
        {
            using (IDataReader dr = LoadDataReader(idUsuarioFilialContato))
            {
                UsuarioFilialContato objUsuarioFilialContato = new UsuarioFilialContato();
                if (SetInstance(dr, objUsuarioFilialContato))
                    return objUsuarioFilialContato;
            }
            throw (new RecordNotFoundException(typeof(UsuarioFilialContato)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de UsuarioFilialContato a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idUsuarioFilialContato">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de UsuarioFilialContato.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static UsuarioFilialContato LoadObject(int idUsuarioFilialContato, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idUsuarioFilialContato, trans))
            {
                UsuarioFilialContato objUsuarioFilialContato = new UsuarioFilialContato();
                if (SetInstance(dr, objUsuarioFilialContato))
                    return objUsuarioFilialContato;
            }
            throw (new RecordNotFoundException(typeof(UsuarioFilialContato)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de UsuarioFilialContato a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idUsuarioFilialContato))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de UsuarioFilialContato a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idUsuarioFilialContato, trans))
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
        /// <param name="objUsuarioFilialContato">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, UsuarioFilialContato objUsuarioFilialContato, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objUsuarioFilialContato._idUsuarioFilialContato = Convert.ToInt32(dr["Idf_Usuario_Filial_Contato"]);
                    if (dr["Nme_Contato"] != DBNull.Value)
                        objUsuarioFilialContato._nomeContato = Convert.ToString(dr["Nme_Contato"]);
                    if (dr["Idf_Funcao"] != DBNull.Value)
                        objUsuarioFilialContato._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
                    if (dr["Des_Funcao"] != DBNull.Value)
                        objUsuarioFilialContato._descricaoFuncao = Convert.ToString(dr["Des_Funcao"]);
                    objUsuarioFilialContato._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                    if (dr["Eml_Contato"] != DBNull.Value)
                        objUsuarioFilialContato._emailContato = Convert.ToString(dr["Eml_Contato"]);
                    if (dr["DDD_Telefone"] != DBNull.Value)
                        objUsuarioFilialContato._DDDTelefone = Convert.ToString(dr["DDD_Telefone"]);
                    if (dr["Num_Telefone"] != DBNull.Value)
                        objUsuarioFilialContato._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
                    objUsuarioFilialContato._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objUsuarioFilialContato._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Num_CPF"] != DBNull.Value)
                        objUsuarioFilialContato._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
                    if (dr["Dta_Nascimento"] != DBNull.Value)
                        objUsuarioFilialContato._dataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
                    if (dr["Idf_Sexo"] != DBNull.Value)
                        objUsuarioFilialContato._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
                    if (dr["Num_Ramal"] != DBNull.Value)
                        objUsuarioFilialContato._numeroRamal = Convert.ToString(dr["Num_Ramal"]);
                    if (dr["Num_DDD_Celular"] != DBNull.Value)
                        objUsuarioFilialContato._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
                    if (dr["Num_Celular"] != DBNull.Value)
                        objUsuarioFilialContato._numeroCelular = Convert.ToString(dr["Num_Celular"]);

                    objUsuarioFilialContato._persisted = true;
                    objUsuarioFilialContato._modified = false;

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