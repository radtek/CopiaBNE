//-- Data: 22/03/2014 17:23
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class EmpresaHome // Tabela: BNE_Empresa_Home
    {
        #region Atributos
        private Filial _filial;
        private string _descricaoNomeURL;
        private string _descricaoCaminhoImagem;
        private bool _flagInativo;
        private DateTime _dataCadastro;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

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

        #region DescricaoNomeURL
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoNomeURL
        {
            get
            {
                return this._descricaoNomeURL;
            }
            set
            {
                this._descricaoNomeURL = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoCaminhoImagem
        /// <summary>
        /// Tamanho do campo: 300.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoCaminhoImagem
        {
            get
            {
                return this._descricaoCaminhoImagem;
            }
            set
            {
                this._descricaoCaminhoImagem = value;
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

        #endregion

        #region Construtores
        public EmpresaHome()
        {
        }
        public EmpresaHome(Filial filial)
        {
            this._filial = filial;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Empresa_Home (Idf_Filial, Des_Nome_URL, Des_Caminho_Imagem, Flg_Inativo, Dta_Cadastro) VALUES (@Idf_Filial, @Des_Nome_URL, @Des_Caminho_Imagem, @Flg_Inativo, @Dta_Cadastro);";
        private const string SPUPDATE = "UPDATE BNE_Empresa_Home SET Des_Nome_URL = @Des_Nome_URL, Des_Caminho_Imagem = @Des_Caminho_Imagem, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Filial = @Idf_Filial";
        private const string SPDELETE = "DELETE FROM BNE_Empresa_Home WHERE Idf_Filial = @Idf_Filial";
        private const string SPSELECTID = "SELECT * FROM BNE_Empresa_Home WITH(NOLOCK) WHERE Idf_Filial = @Idf_Filial";
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
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Nome_URL", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Des_Caminho_Imagem", SqlDbType.VarChar, 300));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
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
            parms[0].Value = this._filial.IdFilial;
            parms[1].Value = this._descricaoNomeURL;
            parms[2].Value = this._descricaoCaminhoImagem;
            parms[3].Value = this._flagInativo;

            if (!this._persisted)
            {
                this._dataCadastro = DateTime.Now;
            }
            parms[4].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de EmpresaHome no banco de dados.
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
        /// Método utilizado para inserir uma instância de EmpresaHome no banco de dados, dentro de uma transação.
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
        /// Método utilizado para atualizar uma instância de EmpresaHome no banco de dados.
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
        /// Método utilizado para atualizar uma instância de EmpresaHome no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de EmpresaHome no banco de dados.
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
        /// Método utilizado para salvar uma instância de EmpresaHome no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de EmpresaHome no banco de dados.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = idFilial;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de EmpresaHome no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idFilial, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = idFilial;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de EmpresaHome no banco de dados.
        /// </summary>
        /// <param name="filial">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<Filial> filial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Empresa_Home where Idf_Filial in (";

            for (int i = 0; i < filial.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = filial[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idFilial)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = idFilial;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idFilial, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));

            parms[0].Value = idFilial;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Emp.Idf_Filial, Emp.Des_Nome_URL, Emp.Des_Caminho_Imagem, Emp.Flg_Inativo, Emp.Dta_Cadastro FROM BNE_Empresa_Home Emp";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de EmpresaHome a partir do banco de dados.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <returns>Instância de EmpresaHome.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static EmpresaHome LoadObject(int idFilial)
        {
            using (IDataReader dr = LoadDataReader(idFilial))
            {
                EmpresaHome objEmpresaHome = new EmpresaHome();
                if (SetInstance(dr, objEmpresaHome))
                    return objEmpresaHome;
            }
            throw (new RecordNotFoundException(typeof(EmpresaHome)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de EmpresaHome a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilial">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de EmpresaHome.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static EmpresaHome LoadObject(int idFilial, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idFilial, trans))
            {
                EmpresaHome objEmpresaHome = new EmpresaHome();
                if (SetInstance(dr, objEmpresaHome))
                    return objEmpresaHome;
            }
            throw (new RecordNotFoundException(typeof(EmpresaHome)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de EmpresaHome a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._filial.IdFilial))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de EmpresaHome a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._filial.IdFilial, trans))
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
        /// <param name="objEmpresaHome">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, EmpresaHome objEmpresaHome)
        {
            try
            {
                if (dr.Read())
                {
                    objEmpresaHome._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                    objEmpresaHome._descricaoNomeURL = Convert.ToString(dr["Des_Nome_URL"]);
                    objEmpresaHome._descricaoCaminhoImagem = Convert.ToString(dr["Des_Caminho_Imagem"]);
                    objEmpresaHome._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objEmpresaHome._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                    objEmpresaHome._persisted = true;
                    objEmpresaHome._modified = false;

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