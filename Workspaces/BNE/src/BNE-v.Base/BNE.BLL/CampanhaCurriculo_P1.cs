//-- Data: 20/09/2013 15:06
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CampanhaCurriculo // Tabela: BNE_Campanha_Curriculo
    {
        #region Atributos
        private Curriculo _curriculo;
        private Campanha _campanha;
        private string _nomePessoa;
        private string _numeroDDDCelular;
        private string _numeroCelular;
        private DateTime _dataCadastro;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

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

        #region Campanha
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Campanha Campanha
        {
            get
            {
                return this._campanha;
            }
            set
            {
                this._campanha = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomePessoa
        /// <summary>
        /// Tamanho do campo: 500.
        /// Campo obrigatório.
        /// </summary>
        public string NomePessoa
        {
            get
            {
                return this._nomePessoa;
            }
            set
            {
                this._nomePessoa = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDDDCelular
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo obrigatório.
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
        /// Tamanho do campo: 9.
        /// Campo obrigatório.
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
        public CampanhaCurriculo()
        {
        }
        public CampanhaCurriculo(Curriculo curriculo, Campanha campanha)
        {
            this._curriculo = curriculo;
            this._campanha = campanha;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Campanha_Curriculo (Idf_Curriculo, Idf_Campanha, Nme_Pessoa, Num_DDD_Celular, Num_Celular, Dta_Cadastro) VALUES (@Idf_Curriculo, @Idf_Campanha, @Nme_Pessoa, @Num_DDD_Celular, @Num_Celular, @Dta_Cadastro);";
        private const string SPUPDATE = "UPDATE BNE_Campanha_Curriculo SET Nme_Pessoa = @Nme_Pessoa, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Campanha = @Idf_Campanha";
        private const string SPDELETE = "DELETE FROM BNE_Campanha_Curriculo WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Campanha = @Idf_Campanha";
        private const string SPSELECTID = "SELECT * FROM BNE_Campanha_Curriculo WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Campanha = @Idf_Campanha";
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
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Campanha", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 500));
            parms.Add(new SqlParameter("@Num_DDD_Celular", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Celular", SqlDbType.VarChar, 9));
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
            parms[0].Value = this._curriculo.IdCurriculo;
            parms[1].Value = this._campanha.IdCampanha;
            parms[2].Value = this._nomePessoa;
            parms[3].Value = this._numeroDDDCelular;
            parms[4].Value = this._numeroCelular;

            if (!this._persisted)
            {
                this._dataCadastro = DateTime.Now;
            }
            parms[5].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de CampanhaCurriculo no banco de dados.
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
        /// Método utilizado para inserir uma instância de CampanhaCurriculo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para atualizar uma instância de CampanhaCurriculo no banco de dados.
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
        /// Método utilizado para atualizar uma instância de CampanhaCurriculo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de CampanhaCurriculo no banco de dados.
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
        /// Método utilizado para salvar uma instância de CampanhaCurriculo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de CampanhaCurriculo no banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idCampanha">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCurriculo, int idCampanha)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Campanha", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;
            parms[1].Value = idCampanha;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de CampanhaCurriculo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idCampanha">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCurriculo, int idCampanha, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Campanha", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;
            parms[1].Value = idCampanha;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idCampanha">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCurriculo, int idCampanha)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Campanha", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;
            parms[1].Value = idCampanha;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idCampanha">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCurriculo, int idCampanha, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Campanha", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;
            parms[1].Value = idCampanha;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cam.Idf_Curriculo, Cam.Idf_Campanha, Cam.Nme_Pessoa, Cam.Num_DDD_Celular, Cam.Num_Celular, Cam.Dta_Cadastro FROM BNE_Campanha_Curriculo Cam";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de CampanhaCurriculo a partir do banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idCampanha">Chave do registro.</param>
        /// <returns>Instância de CampanhaCurriculo.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CampanhaCurriculo LoadObject(int idCurriculo, int idCampanha)
        {
            using (IDataReader dr = LoadDataReader(idCurriculo, idCampanha))
            {
                CampanhaCurriculo objCampanhaCurriculo = new CampanhaCurriculo();
                if (SetInstance(dr, objCampanhaCurriculo))
                    return objCampanhaCurriculo;
            }
            throw (new RecordNotFoundException(typeof(CampanhaCurriculo)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de CampanhaCurriculo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idCampanha">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de CampanhaCurriculo.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CampanhaCurriculo LoadObject(int idCurriculo, int idCampanha, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idCurriculo, idCampanha, trans))
            {
                CampanhaCurriculo objCampanhaCurriculo = new CampanhaCurriculo();
                if (SetInstance(dr, objCampanhaCurriculo))
                    return objCampanhaCurriculo;
            }
            throw (new RecordNotFoundException(typeof(CampanhaCurriculo)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de CampanhaCurriculo a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._curriculo.IdCurriculo, this._campanha.IdCampanha))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de CampanhaCurriculo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._curriculo.IdCurriculo, this._campanha.IdCampanha, trans))
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
        /// <param name="objCampanhaCurriculo">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, CampanhaCurriculo objCampanhaCurriculo)
        {
            try
            {
                if (dr.Read())
                {
                    objCampanhaCurriculo._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    objCampanhaCurriculo._campanha = new Campanha(Convert.ToInt32(dr["Idf_Campanha"]));
                    objCampanhaCurriculo._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
                    objCampanhaCurriculo._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
                    objCampanhaCurriculo._numeroCelular = Convert.ToString(dr["Num_Celular"]);
                    objCampanhaCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                    objCampanhaCurriculo._persisted = true;
                    objCampanhaCurriculo._modified = false;

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