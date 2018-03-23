//-- Data: 27/04/2010 13:53
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CurriculoClassificacao // Tabela: BNE_Curriculo_Classificacao
    {
        #region Atributos
        private int _idCurriculoClassificacao;
        private Curriculo _curriculo;
        private Filial _filial;
        private string _descricaoObservacao;
        private bool _flagInativo;
        private DateTime _dataCadastro;
        private Avaliacao _avaliacao;
        private UsuarioFilialPerfil _usuarioFilialPerfil;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdCurriculoClassificacao
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdCurriculoClassificacao
        {
            get
            {
                return this._idCurriculoClassificacao;
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

        #region DescricaoObservacao
        /// <summary>
        /// Tamanho do campo: 2000.
        /// Campo opcional.
        /// </summary>
        public string DescricaoObservacao
        {
            get
            {
                return this._descricaoObservacao;
            }
            set
            {
                this._descricaoObservacao = value;
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

        #region Avaliacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Avaliacao Avaliacao
        {
            get
            {
                return this._avaliacao;
            }
            set
            {
                this._avaliacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region UsuarioFilialPerfil
        public UsuarioFilialPerfil UsuarioFilialPerfil
        {
            get
            {
                return this._usuarioFilialPerfil;
            }
            set
            {
                this._usuarioFilialPerfil = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public CurriculoClassificacao()
        {
        }
        public CurriculoClassificacao(int idCurriculoClassificacao)
        {
            this._idCurriculoClassificacao = idCurriculoClassificacao;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Curriculo_Classificacao (Idf_Curriculo, Idf_Filial, Des_Observacao, Flg_Inativo, Dta_Cadastro, Idf_Avaliacao,Idf_Usuario_Filial_Perfil) VALUES (@Idf_Curriculo, @Idf_Filial, @Des_Observacao, @Flg_Inativo, @Dta_Cadastro, @Idf_Avaliacao,@Idf_Usuario_Filial_Perfil);SET @Idf_Curriculo_Classificacao = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Curriculo_Classificacao SET Idf_Curriculo = @Idf_Curriculo, Idf_Filial = @Idf_Filial, Des_Observacao = @Des_Observacao, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Idf_Avaliacao = @Idf_Avaliacao, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil WHERE Idf_Curriculo_Classificacao = @Idf_Curriculo_Classificacao";
        private const string SPDELETE = "DELETE FROM BNE_Curriculo_Classificacao WHERE Idf_Curriculo_Classificacao = @Idf_Curriculo_Classificacao";
        private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Classificacao WHERE Idf_Curriculo_Classificacao = @Idf_Curriculo_Classificacao";
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
            parms.Add(new SqlParameter("@Idf_Curriculo_Classificacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Observacao", SqlDbType.Char, 2000));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Avaliacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
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
            parms[0].Value = this._idCurriculoClassificacao;
            parms[1].Value = this._curriculo.IdCurriculo;
            parms[2].Value = this._filial.IdFilial;

            if (!String.IsNullOrEmpty(this._descricaoObservacao))
                parms[3].Value = this._descricaoObservacao;
            else
                parms[3].Value = DBNull.Value;

            parms[4].Value = this._flagInativo;
            parms[6].Value = this._avaliacao.IdAvaliacao;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[5].Value = this._dataCadastro;

            parms[7].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de CurriculoClassificacao no banco de dados.
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
                        this._idCurriculoClassificacao = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Classificacao"].Value);
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
        /// Método utilizado para inserir uma instância de CurriculoClassificacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idCurriculoClassificacao = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Classificacao"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de CurriculoClassificacao no banco de dados.
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
        /// Método utilizado para atualizar uma instância de CurriculoClassificacao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de CurriculoClassificacao no banco de dados.
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
        /// Método utilizado para salvar uma instância de CurriculoClassificacao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de CurriculoClassificacao no banco de dados.
        /// </summary>
        /// <param name="idCurriculoClassificacao">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCurriculoClassificacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Classificacao", SqlDbType.Int, 4));

            parms[0].Value = idCurriculoClassificacao;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de CurriculoClassificacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoClassificacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCurriculoClassificacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Classificacao", SqlDbType.Int, 4));

            parms[0].Value = idCurriculoClassificacao;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de CurriculoClassificacao no banco de dados.
        /// </summary>
        /// <param name="idCurriculoClassificacao">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idCurriculoClassificacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Curriculo_Classificacao where Idf_Curriculo_Classificacao in (";

            for (int i = 0; i < idCurriculoClassificacao.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idCurriculoClassificacao[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCurriculoClassificacao">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCurriculoClassificacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Classificacao", SqlDbType.Int, 4));

            parms[0].Value = idCurriculoClassificacao;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoClassificacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCurriculoClassificacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Classificacao", SqlDbType.Int, 4));

            parms[0].Value = idCurriculoClassificacao;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Classificacao, Cur.Idf_Curriculo, Cur.Idf_Filial, Cur.Des_Observacao, Cur.Flg_Inativo, Cur.Dta_Cadastro, Cur.Idf_Avaliacao FROM BNE_Curriculo_Classificacao Cur";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de CurriculoClassificacao a partir do banco de dados.
        /// </summary>
        /// <param name="idCurriculoClassificacao">Chave do registro.</param>
        /// <returns>Instância de CurriculoClassificacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CurriculoClassificacao LoadObject(int idCurriculoClassificacao)
        {
            using (IDataReader dr = LoadDataReader(idCurriculoClassificacao))
            {
                CurriculoClassificacao objCurriculoClassificacao = new CurriculoClassificacao();
                if (SetInstance(dr, objCurriculoClassificacao))
                    return objCurriculoClassificacao;
            }
            throw (new RecordNotFoundException(typeof(CurriculoClassificacao)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de CurriculoClassificacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoClassificacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de CurriculoClassificacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CurriculoClassificacao LoadObject(int idCurriculoClassificacao, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idCurriculoClassificacao, trans))
            {
                CurriculoClassificacao objCurriculoClassificacao = new CurriculoClassificacao();
                if (SetInstance(dr, objCurriculoClassificacao))
                    return objCurriculoClassificacao;
            }
            throw (new RecordNotFoundException(typeof(CurriculoClassificacao)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de CurriculoClassificacao a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idCurriculoClassificacao))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de CurriculoClassificacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idCurriculoClassificacao, trans))
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
        /// <param name="objCurriculoClassificacao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, CurriculoClassificacao objCurriculoClassificacao)
        {
            try
            {
                if (dr.Read())
                {
                    objCurriculoClassificacao._idCurriculoClassificacao = Convert.ToInt32(dr["Idf_Curriculo_Classificacao"]);
                    objCurriculoClassificacao._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    objCurriculoClassificacao._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                    if (dr["Des_Observacao"] != DBNull.Value)
                        objCurriculoClassificacao._descricaoObservacao = Convert.ToString(dr["Des_Observacao"]);
                    objCurriculoClassificacao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objCurriculoClassificacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objCurriculoClassificacao._avaliacao = new Avaliacao(Convert.ToInt32(dr["Idf_Avaliacao"]));
                    if(dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                        objCurriculoClassificacao._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));

                    objCurriculoClassificacao._persisted = true;
                    objCurriculoClassificacao._modified = false;

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