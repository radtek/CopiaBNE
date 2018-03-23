//-- Data: 04/05/2015 17:24
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PlanoAdquiridoDetalhesCurriculo // Tabela: BNE_Plano_Adquirido_Detalhes_Curriculo
    {
        #region Atributos
        private PlanoAdquiridoDetalhes _planoAdquiridoDetalhes;
        private Curriculo _curriculo;
        private TipoMensagemCS _tipoMensagemCS;
        private DateTime _dataCadastro;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region PlanoAdquiridoDetalhes
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PlanoAdquiridoDetalhes PlanoAdquiridoDetalhes
        {
            get
            {
                return this._planoAdquiridoDetalhes;
            }
            set
            {
                this._planoAdquiridoDetalhes = value;
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
        public PlanoAdquiridoDetalhesCurriculo()
        {
        }
        public PlanoAdquiridoDetalhesCurriculo(PlanoAdquiridoDetalhes planoAdquiridoDetalhes, Curriculo curriculo, TipoMensagemCS tipoMensagemCS)
        {
            this._planoAdquiridoDetalhes = planoAdquiridoDetalhes;
            this._curriculo = curriculo;
            this._tipoMensagemCS = tipoMensagemCS;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Plano_Adquirido_Detalhes_Curriculo (Idf_Plano_Adquirido_Detalhes, Idf_Curriculo, Idf_Tipo_Mensagem_CS, Dta_Cadastro) VALUES (@Idf_Plano_Adquirido_Detalhes, @Idf_Curriculo, @Idf_Tipo_Mensagem_CS, @Dta_Cadastro);";
        private const string SPUPDATE = "UPDATE BNE_Plano_Adquirido_Detalhes_Curriculo SET Dta_Cadastro = @Dta_Cadastro WHERE Idf_Plano_Adquirido_Detalhes = @Idf_Plano_Adquirido_Detalhes AND Idf_Curriculo = @Idf_Curriculo AND Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS";
        private const string SPDELETE = "DELETE FROM BNE_Plano_Adquirido_Detalhes_Curriculo WHERE Idf_Plano_Adquirido_Detalhes = @Idf_Plano_Adquirido_Detalhes AND Idf_Curriculo = @Idf_Curriculo AND Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS";
        private const string SPSELECTID = "SELECT * FROM BNE_Plano_Adquirido_Detalhes_Curriculo WITH(NOLOCK) WHERE Idf_Plano_Adquirido_Detalhes = @Idf_Plano_Adquirido_Detalhes AND Idf_Curriculo = @Idf_Curriculo AND Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS";
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
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Detalhes", SqlDbType.Int, 4));
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
            parms[0].Value = this._planoAdquiridoDetalhes.IdPlanoAdquiridoDetalhes;
            parms[1].Value = this._curriculo.IdCurriculo;
            parms[2].Value = this._tipoMensagemCS.IdTipoMensagemCS;

            if (!this._persisted)
            {
                this._dataCadastro = DateTime.Now;
            }
            parms[3].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de PlanoAdquiridoDetalhesCurriculo no banco de dados.
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
        /// Método utilizado para inserir uma instância de PlanoAdquiridoDetalhesCurriculo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para atualizar uma instância de PlanoAdquiridoDetalhesCurriculo no banco de dados.
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
        /// Método utilizado para atualizar uma instância de PlanoAdquiridoDetalhesCurriculo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de PlanoAdquiridoDetalhesCurriculo no banco de dados.
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
        /// Método utilizado para salvar uma instância de PlanoAdquiridoDetalhesCurriculo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de PlanoAdquiridoDetalhesCurriculo no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idTipoMensagemCS">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlanoAdquiridoDetalhes, int idCurriculo, int idTipoMensagemCS)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Detalhes", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoDetalhes;
            parms[1].Value = idCurriculo;
            parms[2].Value = idTipoMensagemCS;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PlanoAdquiridoDetalhesCurriculo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idTipoMensagemCS">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlanoAdquiridoDetalhes, int idCurriculo, int idTipoMensagemCS, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Detalhes", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoDetalhes;
            parms[1].Value = idCurriculo;
            parms[2].Value = idTipoMensagemCS;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idTipoMensagemCS">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlanoAdquiridoDetalhes, int idCurriculo, int idTipoMensagemCS)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Detalhes", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoDetalhes;
            parms[1].Value = idCurriculo;
            parms[2].Value = idTipoMensagemCS;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idTipoMensagemCS">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlanoAdquiridoDetalhes, int idCurriculo, int idTipoMensagemCS, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Detalhes", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Mensagem_CS", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoDetalhes;
            parms[1].Value = idCurriculo;
            parms[2].Value = idTipoMensagemCS;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Adquirido_Detalhes, Pla.Idf_Curriculo, Pla.Idf_Tipo_Mensagem_CS, Pla.Dta_Cadastro FROM BNE_Plano_Adquirido_Detalhes_Curriculo Pla";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoAdquiridoDetalhesCurriculo a partir do banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idTipoMensagemCS">Chave do registro.</param>
        /// <returns>Instância de PlanoAdquiridoDetalhesCurriculo.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PlanoAdquiridoDetalhesCurriculo LoadObject(int idPlanoAdquiridoDetalhes, int idCurriculo, int idTipoMensagemCS)
        {
            using (IDataReader dr = LoadDataReader(idPlanoAdquiridoDetalhes, idCurriculo, idTipoMensagemCS))
            {
                PlanoAdquiridoDetalhesCurriculo objPlanoAdquiridoDetalhesCurriculo = new PlanoAdquiridoDetalhesCurriculo();
                if (SetInstance(dr, objPlanoAdquiridoDetalhesCurriculo))
                    return objPlanoAdquiridoDetalhesCurriculo;
            }
            throw (new RecordNotFoundException(typeof(PlanoAdquiridoDetalhesCurriculo)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoAdquiridoDetalhesCurriculo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="idTipoMensagemCS">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PlanoAdquiridoDetalhesCurriculo.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PlanoAdquiridoDetalhesCurriculo LoadObject(int idPlanoAdquiridoDetalhes, int idCurriculo, int idTipoMensagemCS, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPlanoAdquiridoDetalhes, idCurriculo, idTipoMensagemCS, trans))
            {
                PlanoAdquiridoDetalhesCurriculo objPlanoAdquiridoDetalhesCurriculo = new PlanoAdquiridoDetalhesCurriculo();
                if (SetInstance(dr, objPlanoAdquiridoDetalhesCurriculo))
                    return objPlanoAdquiridoDetalhesCurriculo;
            }
            throw (new RecordNotFoundException(typeof(PlanoAdquiridoDetalhesCurriculo)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoAdquiridoDetalhesCurriculo a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._planoAdquiridoDetalhes.IdPlanoAdquiridoDetalhes, this._curriculo.IdCurriculo, this._tipoMensagemCS.IdTipoMensagemCS))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoAdquiridoDetalhesCurriculo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._planoAdquiridoDetalhes.IdPlanoAdquiridoDetalhes, this._curriculo.IdCurriculo, this._tipoMensagemCS.IdTipoMensagemCS, trans))
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
        /// <param name="objPlanoAdquiridoDetalhesCurriculo">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, PlanoAdquiridoDetalhesCurriculo objPlanoAdquiridoDetalhesCurriculo)
        {
            try
            {
                if (dr.Read())
                {
                    objPlanoAdquiridoDetalhesCurriculo._planoAdquiridoDetalhes = new PlanoAdquiridoDetalhes(Convert.ToInt32(dr["Idf_Plano_Adquirido_Detalhes"]));
                    objPlanoAdquiridoDetalhesCurriculo._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    objPlanoAdquiridoDetalhesCurriculo._tipoMensagemCS = new TipoMensagemCS(Convert.ToInt32(dr["Idf_Tipo_Mensagem_CS"]));
                    objPlanoAdquiridoDetalhesCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                    objPlanoAdquiridoDetalhesCurriculo._persisted = true;
                    objPlanoAdquiridoDetalhesCurriculo._modified = false;

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