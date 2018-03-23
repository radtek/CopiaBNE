//-- Data: 19/07/2013 10:48
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class VagaIntegracao // Tabela: BNE_Vaga_Integracao
    {
        #region Atributos
        private int _idVagaIntegracao;
        private Vaga _vaga;
        private Integrador _integrador;
        private string _codigoVagaIntegrador;
        private bool _flagInativo;
        private bool _flagEnviadaParaAuditoria;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdVagaIntegracao
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdVagaIntegracao
        {
            get
            {
                return this._idVagaIntegracao;
            }
        }
        #endregion

        #region Vaga
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Vaga Vaga
        {
            get
            {
                return this._vaga;
            }
            set
            {
                this._vaga = value;
                this._modified = true;
            }
        }
        #endregion

        #region Integrador
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Integrador Integrador
        {
            get
            {
                return this._integrador;
            }
            set
            {
                this._integrador = value;
                this._modified = true;
            }
        }
        #endregion

        #region CodigoVagaIntegrador
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo obrigatório.
        /// </summary>
        public string CodigoVagaIntegrador
        {
            get
            {
                return this._codigoVagaIntegrador;
            }
            set
            {
                this._codigoVagaIntegrador = value;
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

        #region FlagEnviadaParaAuditoria
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagEnviadaParaAuditoria
        {
            get
            {
                return this._flagEnviadaParaAuditoria;
            }
            set
            {
                this._flagEnviadaParaAuditoria = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public VagaIntegracao()
        {
        }
        public VagaIntegracao(int idVagaIntegracao)
        {
            this._idVagaIntegracao = idVagaIntegracao;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Vaga_Integracao (Idf_Vaga, Idf_Integrador, Cod_Vaga_Integrador, Flg_Inativo, Flg_Enviada_Para_Auditoria) VALUES (@Idf_Vaga, @Idf_Integrador, @Cod_Vaga_Integrador, @Flg_Inativo, @Flg_Enviada_Para_Auditoria);SET @Idf_Vaga_Integracao = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Vaga_Integracao SET Idf_Vaga = @Idf_Vaga, Idf_Integrador = @Idf_Integrador, Cod_Vaga_Integrador = @Cod_Vaga_Integrador, Flg_Inativo = @Flg_Inativo, Flg_Enviada_Para_Auditoria = @Flg_Enviada_Para_Auditoria WHERE Idf_Vaga_Integracao = @Idf_Vaga_Integracao";
        private const string SPDELETE = "DELETE FROM BNE_Vaga_Integracao WHERE Idf_Vaga_Integracao = @Idf_Vaga_Integracao";
        private const string SPSELECTID = "SELECT * FROM BNE_Vaga_Integracao WITH(NOLOCK) WHERE Idf_Vaga_Integracao = @Idf_Vaga_Integracao";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga_Integracao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Integrador", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Cod_Vaga_Integrador", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Enviada_Para_Auditoria", SqlDbType.Bit, 1));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Francisco Ribas</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idVagaIntegracao;
            parms[1].Value = this._vaga.IdVaga;
            parms[2].Value = this._integrador.IdIntegrador;
            parms[3].Value = this._codigoVagaIntegrador;
            parms[4].Value = this._flagInativo;
            parms[5].Value = this._flagEnviadaParaAuditoria;

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
        /// Método utilizado para inserir uma instância de VagaIntegracao no banco de dados.
        /// </summary>
        /// <remarks>Francisco Ribas</remarks>
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
                        this._idVagaIntegracao = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Integracao"].Value);
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
        /// Método utilizado para inserir uma instância de VagaIntegracao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idVagaIntegracao = Convert.ToInt32(cmd.Parameters["@Idf_Vaga_Integracao"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de VagaIntegracao no banco de dados.
        /// </summary>
        /// <remarks>Francisco Ribas</remarks>
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
        /// Método utilizado para atualizar uma instância de VagaIntegracao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
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
        /// Método utilizado para salvar uma instância de VagaIntegracao no banco de dados.
        /// </summary>
        /// <remarks>Francisco Ribas</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de VagaIntegracao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
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
        /// Método utilizado para excluir uma instância de VagaIntegracao no banco de dados.
        /// </summary>
        /// <param name="idVagaIntegracao">Chave do registro.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idVagaIntegracao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga_Integracao", SqlDbType.Int, 4));

            parms[0].Value = idVagaIntegracao;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de VagaIntegracao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idVagaIntegracao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idVagaIntegracao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga_Integracao", SqlDbType.Int, 4));

            parms[0].Value = idVagaIntegracao;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de VagaIntegracao no banco de dados.
        /// </summary>
        /// <param name="idVagaIntegracao">Lista de chaves.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(List<int> idVagaIntegracao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Vaga_Integracao where Idf_Vaga_Integracao in (";

            for (int i = 0; i < idVagaIntegracao.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idVagaIntegracao[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idVagaIntegracao">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idVagaIntegracao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga_Integracao", SqlDbType.Int, 4));

            parms[0].Value = idVagaIntegracao;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idVagaIntegracao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idVagaIntegracao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga_Integracao", SqlDbType.Int, 4));

            parms[0].Value = idVagaIntegracao;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Vag.Idf_Vaga_Integracao, Vag.Idf_Vaga, Vag.Idf_Integrador, Vag.Cod_Vaga_Integrador, Vag.Flg_Inativo, Vag.Flg_Enviada_Para_Auditoria FROM BNE_Vaga_Integracao Vag";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de VagaIntegracao a partir do banco de dados.
        /// </summary>
        /// <param name="idVagaIntegracao">Chave do registro.</param>
        /// <returns>Instância de VagaIntegracao.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static VagaIntegracao LoadObject(int idVagaIntegracao)
        {
            using (IDataReader dr = LoadDataReader(idVagaIntegracao))
            {
                VagaIntegracao objVagaIntegracao = new VagaIntegracao();
                if (SetInstance(dr, objVagaIntegracao))
                    return objVagaIntegracao;
            }
            throw (new RecordNotFoundException(typeof(VagaIntegracao)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de VagaIntegracao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idVagaIntegracao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de VagaIntegracao.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static VagaIntegracao LoadObject(int idVagaIntegracao, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idVagaIntegracao, trans))
            {
                VagaIntegracao objVagaIntegracao = new VagaIntegracao();
                if (SetInstance(dr, objVagaIntegracao))
                    return objVagaIntegracao;
            }
            throw (new RecordNotFoundException(typeof(VagaIntegracao)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de VagaIntegracao a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idVagaIntegracao))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de VagaIntegracao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idVagaIntegracao, trans))
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
        /// <param name="objVagaIntegracao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static bool SetInstance(IDataReader dr, VagaIntegracao objVagaIntegracao)
        {
            try
            {
                if (dr.Read())
                {
                    objVagaIntegracao._idVagaIntegracao = Convert.ToInt32(dr["Idf_Vaga_Integracao"]);
                    objVagaIntegracao._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
                    objVagaIntegracao._integrador = new Integrador(Convert.ToInt32(dr["Idf_Integrador"]));
                    objVagaIntegracao._codigoVagaIntegrador = Convert.ToString(dr["Cod_Vaga_Integrador"]);
                    objVagaIntegracao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objVagaIntegracao._flagEnviadaParaAuditoria = Convert.ToBoolean(dr["Flg_Enviada_Para_Auditoria"]);

                    objVagaIntegracao._persisted = true;
                    objVagaIntegracao._modified = false;

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