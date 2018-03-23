//-- Data: 20/01/2015 16:57
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class DesistenteCadastro // Tabela: BNE_Desistente_Cadastro
    {
        #region Atributos
        private int _idDesistenteCadastro;
        private decimal? _numeroCPF;
        private decimal? _numeroCNPJ;
        private string _numeroDDDTelefone;
        private string _numeroTelefone;
        private DateTime? _dataNascimento;
        private string _numeroDDDCelular;
        private string _numeroCelular;
        private string _nomeDesistenteCadastro;
        private string _emailDesistenteCadastro;
        private DateTime _dataCadastro;
        private Origem _origem;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdDesistenteCadastro
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdDesistenteCadastro
        {
            get
            {
                return this._idDesistenteCadastro;
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

        #region NumeroCNPJ
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? NumeroCNPJ
        {
            get
            {
                return this._numeroCNPJ;
            }
            set
            {
                this._numeroCNPJ = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDDDTelefone
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string NumeroDDDTelefone
        {
            get
            {
                return this._numeroDDDTelefone;
            }
            set
            {
                this._numeroDDDTelefone = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroTelefone
        /// <summary>
        /// Tamanho do campo: 10.
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

        #region NomeDesistenteCadastro
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string NomeDesistenteCadastro
        {
            get
            {
                return this._nomeDesistenteCadastro;
            }
            set
            {
                this._nomeDesistenteCadastro = value;
                this._modified = true;
            }
        }
        #endregion

        #region EmailDesistenteCadastro
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string EmailDesistenteCadastro
        {
            get
            {
                return this._emailDesistenteCadastro;
            }
            set
            {
                this._emailDesistenteCadastro = value;
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

        #region Origem
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Origem Origem
        {
            get
            {
                return this._origem;
            }
            set
            {
                this._origem = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public DesistenteCadastro()
        {
        }
        public DesistenteCadastro(int idDesistenteCadastro)
        {
            this._idDesistenteCadastro = idDesistenteCadastro;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Desistente_Cadastro (Num_CPF, Num_CNPJ, Num_DDD_Telefone, Num_Telefone, Dta_Nascimento, Num_DDD_Celular, Num_Celular, Nme_Desistente_Cadastro, Eml_Desistente_Cadastro, Dta_Cadastro, Idf_Origem) VALUES (@Num_CPF, @Num_CNPJ, @Num_DDD_Telefone, @Num_Telefone, @Dta_Nascimento, @Num_DDD_Celular, @Num_Celular, @Nme_Desistente_Cadastro, @Eml_Desistente_Cadastro, @Dta_Cadastro, @Idf_Origem);SET @Idf_Desistente_Cadastro = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Desistente_Cadastro SET Num_CPF = @Num_CPF, Num_CNPJ = @Num_CNPJ, Num_DDD_Telefone = @Num_DDD_Telefone, Num_Telefone = @Num_Telefone, Dta_Nascimento = @Dta_Nascimento, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular, Nme_Desistente_Cadastro = @Nme_Desistente_Cadastro, Eml_Desistente_Cadastro = @Eml_Desistente_Cadastro, Dta_Cadastro = @Dta_Cadastro, Idf_Origem = @Idf_Origem WHERE Idf_Desistente_Cadastro = @Idf_Desistente_Cadastro";
        private const string SPDELETE = "DELETE FROM BNE_Desistente_Cadastro WHERE Idf_Desistente_Cadastro = @Idf_Desistente_Cadastro";
        private const string SPSELECTID = "SELECT * FROM BNE_Desistente_Cadastro WITH(NOLOCK) WHERE Idf_Desistente_Cadastro = @Idf_Desistente_Cadastro";
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
            parms.Add(new SqlParameter("@Idf_Desistente_Cadastro", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Num_CNPJ", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Num_DDD_Telefone", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Telefone", SqlDbType.Char, 10));
            parms.Add(new SqlParameter("@Dta_Nascimento", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Num_DDD_Celular", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Celular", SqlDbType.Char, 10));
            parms.Add(new SqlParameter("@Nme_Desistente_Cadastro", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Eml_Desistente_Cadastro", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4));
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
            parms[0].Value = this._idDesistenteCadastro;

            if (this._numeroCPF.HasValue)
                parms[1].Value = this._numeroCPF;
            else
                parms[1].Value = DBNull.Value;


            if (this._numeroCNPJ.HasValue)
                parms[2].Value = this._numeroCNPJ;
            else
                parms[2].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroDDDTelefone))
                parms[3].Value = this._numeroDDDTelefone;
            else
                parms[3].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroTelefone))
                parms[4].Value = this._numeroTelefone;
            else
                parms[4].Value = DBNull.Value;


            if (this._dataNascimento.HasValue)
                parms[5].Value = this._dataNascimento;
            else
                parms[5].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroDDDCelular))
                parms[6].Value = this._numeroDDDCelular;
            else
                parms[6].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroCelular))
                parms[7].Value = this._numeroCelular;
            else
                parms[7].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._nomeDesistenteCadastro))
                parms[8].Value = this._nomeDesistenteCadastro;
            else
                parms[8].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._emailDesistenteCadastro))
                parms[9].Value = this._emailDesistenteCadastro;
            else
                parms[9].Value = DBNull.Value;

            parms[11].Value = this._origem.IdOrigem;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[10].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de DesistenteCadastro no banco de dados.
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
                        this._idDesistenteCadastro = Convert.ToInt32(cmd.Parameters["@Idf_Desistente_Cadastro"].Value);
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
        /// Método utilizado para inserir uma instância de DesistenteCadastro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idDesistenteCadastro = Convert.ToInt32(cmd.Parameters["@Idf_Desistente_Cadastro"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de DesistenteCadastro no banco de dados.
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
        /// Método utilizado para atualizar uma instância de DesistenteCadastro no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de DesistenteCadastro no banco de dados.
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
        /// Método utilizado para salvar uma instância de DesistenteCadastro no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de DesistenteCadastro no banco de dados.
        /// </summary>
        /// <param name="idDesistenteCadastro">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idDesistenteCadastro)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Desistente_Cadastro", SqlDbType.Int, 4));

            parms[0].Value = idDesistenteCadastro;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de DesistenteCadastro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idDesistenteCadastro">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idDesistenteCadastro, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Desistente_Cadastro", SqlDbType.Int, 4));

            parms[0].Value = idDesistenteCadastro;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de DesistenteCadastro no banco de dados.
        /// </summary>
        /// <param name="idDesistenteCadastro">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idDesistenteCadastro)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Desistente_Cadastro where Idf_Desistente_Cadastro in (";

            for (int i = 0; i < idDesistenteCadastro.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idDesistenteCadastro[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idDesistenteCadastro">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idDesistenteCadastro)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Desistente_Cadastro", SqlDbType.Int, 4));

            parms[0].Value = idDesistenteCadastro;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idDesistenteCadastro">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idDesistenteCadastro, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Desistente_Cadastro", SqlDbType.Int, 4));

            parms[0].Value = idDesistenteCadastro;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Des.Idf_Desistente_Cadastro, Des.Num_CPF, Des.Num_CNPJ, Des.Num_DDD_Telefone, Des.Num_Telefone, Des.Dta_Nascimento, Des.Num_DDD_Celular, Des.Num_Celular, Des.Nme_Desistente_Cadastro, Des.Eml_Desistente_Cadastro, Des.Dta_Cadastro, Des.Idf_Origem FROM BNE_Desistente_Cadastro Des";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de DesistenteCadastro a partir do banco de dados.
        /// </summary>
        /// <param name="idDesistenteCadastro">Chave do registro.</param>
        /// <returns>Instância de DesistenteCadastro.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DesistenteCadastro LoadObject(int idDesistenteCadastro)
        {
            using (IDataReader dr = LoadDataReader(idDesistenteCadastro))
            {
                DesistenteCadastro objDesistenteCadastro = new DesistenteCadastro();
                if (SetInstance(dr, objDesistenteCadastro))
                    return objDesistenteCadastro;
            }
            throw (new RecordNotFoundException(typeof(DesistenteCadastro)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de DesistenteCadastro a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idDesistenteCadastro">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de DesistenteCadastro.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DesistenteCadastro LoadObject(int idDesistenteCadastro, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idDesistenteCadastro, trans))
            {
                DesistenteCadastro objDesistenteCadastro = new DesistenteCadastro();
                if (SetInstance(dr, objDesistenteCadastro))
                    return objDesistenteCadastro;
            }
            throw (new RecordNotFoundException(typeof(DesistenteCadastro)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de DesistenteCadastro a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idDesistenteCadastro))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de DesistenteCadastro a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idDesistenteCadastro, trans))
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
        /// <param name="objDesistenteCadastro">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, DesistenteCadastro objDesistenteCadastro)
        {
            try
            {
                if (dr.Read())
                {
                    objDesistenteCadastro._idDesistenteCadastro = Convert.ToInt32(dr["Idf_Desistente_Cadastro"]);
                    if (dr["Num_CPF"] != DBNull.Value)
                        objDesistenteCadastro._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
                    if (dr["Num_CNPJ"] != DBNull.Value)
                        objDesistenteCadastro._numeroCNPJ = Convert.ToDecimal(dr["Num_CNPJ"]);
                    if (dr["Num_DDD_Telefone"] != DBNull.Value)
                        objDesistenteCadastro._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
                    if (dr["Num_Telefone"] != DBNull.Value)
                        objDesistenteCadastro._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
                    if (dr["Dta_Nascimento"] != DBNull.Value)
                        objDesistenteCadastro._dataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
                    if (dr["Num_DDD_Celular"] != DBNull.Value)
                        objDesistenteCadastro._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
                    if (dr["Num_Celular"] != DBNull.Value)
                        objDesistenteCadastro._numeroCelular = Convert.ToString(dr["Num_Celular"]);
                    if (dr["Nme_Desistente_Cadastro"] != DBNull.Value)
                        objDesistenteCadastro._nomeDesistenteCadastro = Convert.ToString(dr["Nme_Desistente_Cadastro"]);
                    if (dr["Eml_Desistente_Cadastro"] != DBNull.Value)
                        objDesistenteCadastro._emailDesistenteCadastro = Convert.ToString(dr["Eml_Desistente_Cadastro"]);
                    objDesistenteCadastro._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objDesistenteCadastro._origem = new Origem(Convert.ToInt32(dr["Idf_Origem"]));

                    objDesistenteCadastro._persisted = true;
                    objDesistenteCadastro._modified = false;

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