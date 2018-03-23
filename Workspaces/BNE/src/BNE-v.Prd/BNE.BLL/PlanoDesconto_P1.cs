//-- Data: 30/11/2017 15:15
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PlanoDesconto // Tabela: BNE_Plano_Desconto
    {
        #region Atributos
        private int _idPlanoDesconto;
        private PlanoAdquirido _planoAdquirido;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private DateTime _dataCadastro;
        private bool _flagInativo;
        private decimal? _valorDesconto;
        private DateTime? _dataUtilizacao;
        private PlanoParcela _planoParcela;
        private DateTime? _dataAtualizacao;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPlanoDesconto
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPlanoDesconto
        {
            get
            {
                return this._idPlanoDesconto;
            }
        }
        #endregion

        #region PlanoAdquirido
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PlanoAdquirido PlanoAdquirido
        {
            get
            {
                return this._planoAdquirido;
            }
            set
            {
                this._planoAdquirido = value;
                this._modified = true;
            }
        }
        #endregion

        #region UsuarioFilialPerfil
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
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

        #region ValorDesconto
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? ValorDesconto
        {
            get
            {
                return this._valorDesconto;
            }
            set
            {
                this._valorDesconto = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataUtilizacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataUtilizacao
        {
            get
            {
                return this._dataUtilizacao;
            }
            set
            {
                this._dataUtilizacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region PlanoParcela
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public PlanoParcela PlanoParcela
        {
            get
            {
                return this._planoParcela;
            }
            set
            {
                this._planoParcela = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAtualizacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataAtualizacao
        {
            get
            {
                return this._dataAtualizacao;
            }
            set
            {
                this._dataAtualizacao = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public PlanoDesconto()
        {
        }
        public PlanoDesconto(int idPlanoDesconto)
        {
            this._idPlanoDesconto = idPlanoDesconto;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Plano_Desconto (Idf_Plano_Adquirido, Idf_Usuario_Filial_Perfil, Dta_Cadastro, Flg_Inativo, Vlr_Desconto, Dta_Utilizacao, Idf_Plano_Parcela, Dta_Atualizacao) VALUES (@Idf_Plano_Adquirido, @Idf_Usuario_Filial_Perfil, @Dta_Cadastro, @Flg_Inativo, @Vlr_Desconto, @Dta_Utilizacao, @Idf_Plano_Parcela, @Dta_Atualizacao);SET @Idf_Plano_Desconto = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Plano_Desconto SET Idf_Plano_Adquirido = @Idf_Plano_Adquirido, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Vlr_Desconto = @Vlr_Desconto, Dta_Utilizacao = @Dta_Utilizacao, Idf_Plano_Parcela = @Idf_Plano_Parcela, Dta_Atualizacao = @Dta_Atualizacao WHERE Idf_Plano_Desconto = @Idf_Plano_Desconto";
        private const string SPDELETE = "DELETE FROM BNE_Plano_Desconto WHERE Idf_Plano_Desconto = @Idf_Plano_Desconto";
        private const string SPSELECTID = "SELECT * FROM BNE_Plano_Desconto WITH(NOLOCK) WHERE Idf_Plano_Desconto = @Idf_Plano_Desconto";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Mailson</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Desconto", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Vlr_Desconto", SqlDbType.Decimal, 5));
            parms.Add(new SqlParameter("@Dta_Utilizacao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Plano_Parcela", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Atualizacao", SqlDbType.DateTime, 8));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Mailson</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idPlanoDesconto;
            parms[1].Value = this._planoAdquirido.IdPlanoAdquirido;
            parms[2].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            parms[4].Value = this._flagInativo;

            if (this._valorDesconto.HasValue)
                parms[5].Value = this._valorDesconto;
            else
                parms[5].Value = DBNull.Value;


            if (this._dataUtilizacao.HasValue)
                parms[6].Value = this._dataUtilizacao;
            else
                parms[6].Value = DBNull.Value;


            if (this._planoParcela != null)
                parms[7].Value = this._planoParcela.IdPlanoParcela;
            else
                parms[7].Value = DBNull.Value;


            if (this._dataAtualizacao.HasValue)
                parms[8].Value = this._dataAtualizacao;
            else
                parms[8].Value = DateTime.Now;


            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[3].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de PlanoDesconto no banco de dados.
        /// </summary>
        /// <remarks>Mailson</remarks>
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
                        this._idPlanoDesconto = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Desconto"].Value);
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
        /// Método utilizado para inserir uma instância de PlanoDesconto no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPlanoDesconto = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Desconto"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de PlanoDesconto no banco de dados.
        /// </summary>
        /// <remarks>Mailson</remarks>
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
        /// Método utilizado para atualizar uma instância de PlanoDesconto no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
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
        /// Método utilizado para salvar uma instância de PlanoDesconto no banco de dados.
        /// </summary>
        /// <remarks>Mailson</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de PlanoDesconto no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
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
        /// Método utilizado para excluir uma instância de PlanoDesconto no banco de dados.
        /// </summary>
        /// <param name="idPlanoDesconto">Chave do registro.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(int idPlanoDesconto)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Desconto", SqlDbType.Int, 4));

            parms[0].Value = idPlanoDesconto;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PlanoDesconto no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoDesconto">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(int idPlanoDesconto, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Desconto", SqlDbType.Int, 4));

            parms[0].Value = idPlanoDesconto;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de PlanoDesconto no banco de dados.
        /// </summary>
        /// <param name="idPlanoDesconto">Lista de chaves.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(List<int> idPlanoDesconto)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Plano_Desconto where Idf_Plano_Desconto in (";

            for (int i = 0; i < idPlanoDesconto.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPlanoDesconto[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPlanoDesconto">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Mailson</remarks>
        private static IDataReader LoadDataReader(int idPlanoDesconto)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Desconto", SqlDbType.Int, 4));

            parms[0].Value = idPlanoDesconto;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoDesconto">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Mailson</remarks>
        private static IDataReader LoadDataReader(int idPlanoDesconto, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Desconto", SqlDbType.Int, 4));

            parms[0].Value = idPlanoDesconto;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Desconto, Pla.Idf_Plano_Adquirido, Pla.Idf_Usuario_Filial_Perfil, Pla.Dta_Cadastro, Pla.Flg_Inativo, Pla.Vlr_Desconto, Pla.Dta_Utilizacao, Pla.Idf_Plano_Parcela, Pla.Dta_Atualizacao FROM BNE_Plano_Desconto Pla";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoDesconto a partir do banco de dados.
        /// </summary>
        /// <param name="idPlanoDesconto">Chave do registro.</param>
        /// <returns>Instância de PlanoDesconto.</returns>
        /// <remarks>Mailson</remarks>
        public static PlanoDesconto LoadObject(int idPlanoDesconto)
        {
            using (IDataReader dr = LoadDataReader(idPlanoDesconto))
            {
                PlanoDesconto objPlanoDesconto = new PlanoDesconto();
                if (SetInstance(dr, objPlanoDesconto))
                    return objPlanoDesconto;
            }
            throw (new RecordNotFoundException(typeof(PlanoDesconto)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoDesconto a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoDesconto">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PlanoDesconto.</returns>
        /// <remarks>Mailson</remarks>
        public static PlanoDesconto LoadObject(int idPlanoDesconto, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPlanoDesconto, trans))
            {
                PlanoDesconto objPlanoDesconto = new PlanoDesconto();
                if (SetInstance(dr, objPlanoDesconto))
                    return objPlanoDesconto;
            }
            throw (new RecordNotFoundException(typeof(PlanoDesconto)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoDesconto a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoDesconto))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoDesconto a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoDesconto, trans))
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
        /// <param name="objPlanoDesconto">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        private static bool SetInstance(IDataReader dr, PlanoDesconto objPlanoDesconto, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objPlanoDesconto._idPlanoDesconto = Convert.ToInt32(dr["Idf_Plano_Desconto"]);
                    objPlanoDesconto._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
                    objPlanoDesconto._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    objPlanoDesconto._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objPlanoDesconto._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Vlr_Desconto"] != DBNull.Value)
                        objPlanoDesconto._valorDesconto = Convert.ToDecimal(dr["Vlr_Desconto"]);
                    if (dr["Dta_Utilizacao"] != DBNull.Value)
                        objPlanoDesconto._dataUtilizacao = Convert.ToDateTime(dr["Dta_Utilizacao"]);
                    if (dr["Idf_Plano_Parcela"] != DBNull.Value)
                        objPlanoDesconto._planoParcela = new PlanoParcela(Convert.ToInt32(dr["Idf_Plano_Parcela"]));
                    if (dr["Dta_Atualizacao"] != DBNull.Value)
                        objPlanoDesconto._dataAtualizacao = Convert.ToDateTime(dr["Dta_Atualizacao"]);

                    objPlanoDesconto._persisted = true;
                    objPlanoDesconto._modified = false;

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