//-- Data: 14/07/2014 15:42
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CodigoDesconto // Tabela: BNE_Codigo_Desconto
    {
        #region Atributos
        private int _idCodigoDesconto;
        private string _descricaoCodigoDesconto;
        private StatusCodigoDesconto _statusCodigoDesconto;
        private Parceiro _parceiro;
        private TipoCodigoDesconto _tipoCodigoDesconto;
        private DateTime _dataCadastro;
        private DateTime? _dataAlteracao;
        private DateTime? _dataUtilizacao;
        private DateTime? _dataValidadeInicio;
        private DateTime? _dataValidadeFim;
        private string _descricaoIdentificacaoCodigo;
        private UsuarioFilialPerfil _usuarioFilialPerfil;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdCodigoDesconto
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdCodigoDesconto
        {
            get
            {
                return this._idCodigoDesconto;
            }
        }
        #endregion

        #region DescricaoCodigoDesconto
        /// <summary>
        /// Tamanho do campo: 200.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoCodigoDesconto
        {
            get
            {
                return this._descricaoCodigoDesconto;
            }
            set
            {
                this._descricaoCodigoDesconto = value;
                this._modified = true;
            }
        }
        #endregion

        #region StatusCodigoDesconto
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public StatusCodigoDesconto StatusCodigoDesconto
        {
            get
            {
                return this._statusCodigoDesconto;
            }
            set
            {
                this._statusCodigoDesconto = value;
                this._modified = true;
            }
        }
        #endregion

        #region Parceiro
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Parceiro Parceiro
        {
            get
            {
                return this._parceiro;
            }
            set
            {
                this._parceiro = value;
                this._modified = true;
            }
        }
        #endregion

        #region TipoCodigoDesconto
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public TipoCodigoDesconto TipoCodigoDesconto
        {
            get
            {
                return this._tipoCodigoDesconto;
            }
            set
            {
                this._tipoCodigoDesconto = value;
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

        #region DataAlteracao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataAlteracao
        {
            get
            {
                return this._dataAlteracao;
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

        #region DataValidadeInicio
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataValidadeInicio
        {
            get
            {
                return this._dataValidadeInicio;
            }
            set
            {
                this._dataValidadeInicio = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataValidadeFim
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataValidadeFim
        {
            get
            {
                return this._dataValidadeFim;
            }
            set
            {
                this._dataValidadeFim = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoIdentificacaoCodigo
        /// <summary>
        /// Tamanho do campo: 200.
        /// Campo opcional.
        /// </summary>
        public string DescricaoIdentificacaoCodigo
        {
            get
            {
                return this._descricaoIdentificacaoCodigo;
            }
            set
            {
                this._descricaoIdentificacaoCodigo = value;
                this._modified = true;
            }
        }
        #endregion

        #region UsuarioFilialPerfil
        /// <summary>
        /// Campo opcional.
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

        #endregion

        #region Construtores
        public CodigoDesconto()
        {
        }
        public CodigoDesconto(int idCodigoDesconto)
        {
            this._idCodigoDesconto = idCodigoDesconto;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Codigo_Desconto (Des_Codigo_Desconto, Idf_Status_Codigo_Desconto, Idf_Parceiro, Idf_Tipo_Codigo_Desconto, Dta_Cadastro, Dta_Alteracao, Dta_Utilizacao, Dta_Validade_Inicio, Dta_Validade_Fim, Des_Identificacao_Codigo, Idf_Usuario_Filial_Perfil) VALUES (@Des_Codigo_Desconto, @Idf_Status_Codigo_Desconto, @Idf_Parceiro, @Idf_Tipo_Codigo_Desconto, @Dta_Cadastro, @Dta_Alteracao, @Dta_Utilizacao, @Dta_Validade_Inicio, @Dta_Validade_Fim, @Des_Identificacao_Codigo, @Idf_Usuario_Filial_Perfil);SET @Idf_Codigo_Desconto = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Codigo_Desconto SET Des_Codigo_Desconto = @Des_Codigo_Desconto, Idf_Status_Codigo_Desconto = @Idf_Status_Codigo_Desconto, Idf_Parceiro = @Idf_Parceiro, Idf_Tipo_Codigo_Desconto = @Idf_Tipo_Codigo_Desconto, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Dta_Utilizacao = @Dta_Utilizacao, Dta_Validade_Inicio = @Dta_Validade_Inicio, Dta_Validade_Fim = @Dta_Validade_Fim, Des_Identificacao_Codigo = @Des_Identificacao_Codigo, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil WHERE Idf_Codigo_Desconto = @Idf_Codigo_Desconto";
        private const string SPDELETE = "DELETE FROM BNE_Codigo_Desconto WHERE Idf_Codigo_Desconto = @Idf_Codigo_Desconto";
        private const string SPSELECTID = "SELECT * FROM BNE_Codigo_Desconto with(nolock)  WHERE Idf_Codigo_Desconto = @Idf_Codigo_Desconto";
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
            parms.Add(new SqlParameter("@Idf_Codigo_Desconto", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Codigo_Desconto", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Idf_Status_Codigo_Desconto", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Parceiro", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Utilizacao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Validade_Inicio", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Validade_Fim", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Des_Identificacao_Codigo", SqlDbType.VarChar, 200));
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
            parms[0].Value = this._idCodigoDesconto;
            parms[1].Value = this._descricaoCodigoDesconto;
            parms[2].Value = this._statusCodigoDesconto.IdStatusCodigoDesconto;

            if (this._parceiro != null)
                parms[3].Value = this._parceiro.IdParceiro;
            else
                parms[3].Value = DBNull.Value;


            if (this._tipoCodigoDesconto != null)
                parms[4].Value = this._tipoCodigoDesconto.IdTipoCodigoDesconto;
            else
                parms[4].Value = DBNull.Value;


            if (this._dataUtilizacao.HasValue)
                parms[7].Value = this._dataUtilizacao;
            else
                parms[7].Value = DBNull.Value;


            if (this._dataValidadeInicio.HasValue)
                parms[8].Value = this._dataValidadeInicio;
            else
                parms[8].Value = DBNull.Value;


            if (this._dataValidadeFim.HasValue)
                parms[9].Value = this._dataValidadeFim;
            else
                parms[9].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoIdentificacaoCodigo))
                parms[10].Value = this._descricaoIdentificacaoCodigo;
            else
                parms[10].Value = DBNull.Value;


            if (this._usuarioFilialPerfil != null)
                parms[11].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            else
                parms[11].Value = DBNull.Value;


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
            this._dataAlteracao = DateTime.Now;
            parms[6].Value = this._dataAlteracao;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de CodigoDesconto no banco de dados.
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
                        this._idCodigoDesconto = Convert.ToInt32(cmd.Parameters["@Idf_Codigo_Desconto"].Value);
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
        /// Método utilizado para inserir uma instância de CodigoDesconto no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idCodigoDesconto = Convert.ToInt32(cmd.Parameters["@Idf_Codigo_Desconto"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de CodigoDesconto no banco de dados.
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
        /// Método utilizado para atualizar uma instância de CodigoDesconto no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de CodigoDesconto no banco de dados.
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
        /// Método utilizado para salvar uma instância de CodigoDesconto no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de CodigoDesconto no banco de dados.
        /// </summary>
        /// <param name="idCodigoDesconto">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCodigoDesconto)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Codigo_Desconto", SqlDbType.Int, 4));

            parms[0].Value = idCodigoDesconto;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de CodigoDesconto no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCodigoDesconto">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCodigoDesconto, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Codigo_Desconto", SqlDbType.Int, 4));

            parms[0].Value = idCodigoDesconto;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de CodigoDesconto no banco de dados.
        /// </summary>
        /// <param name="idCodigoDesconto">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idCodigoDesconto)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Codigo_Desconto where Idf_Codigo_Desconto in (";

            for (int i = 0; i < idCodigoDesconto.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idCodigoDesconto[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCodigoDesconto">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCodigoDesconto)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Codigo_Desconto", SqlDbType.Int, 4));

            parms[0].Value = idCodigoDesconto;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCodigoDesconto">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCodigoDesconto, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Codigo_Desconto", SqlDbType.Int, 4));

            parms[0].Value = idCodigoDesconto;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cod.Idf_Codigo_Desconto, Cod.Des_Codigo_Desconto, Cod.Idf_Status_Codigo_Desconto, Cod.Idf_Parceiro, Cod.Idf_Tipo_Codigo_Desconto, Cod.Dta_Cadastro, Cod.Dta_Alteracao, Cod.Dta_Utilizacao, Cod.Dta_Validade_Inicio, Cod.Dta_Validade_Fim, Cod.Des_Identificacao_Codigo, Cod.Idf_Usuario_Filial_Perfil FROM BNE_Codigo_Desconto Cod";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de CodigoDesconto a partir do banco de dados.
        /// </summary>
        /// <param name="idCodigoDesconto">Chave do registro.</param>
        /// <returns>Instância de CodigoDesconto.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CodigoDesconto LoadObject(int idCodigoDesconto)
        {
            using (IDataReader dr = LoadDataReader(idCodigoDesconto))
            {
                CodigoDesconto objCodigoDesconto = new CodigoDesconto();
                if (SetInstance(dr, objCodigoDesconto))
                    return objCodigoDesconto;
            }
            throw (new RecordNotFoundException(typeof(CodigoDesconto)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de CodigoDesconto a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCodigoDesconto">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de CodigoDesconto.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CodigoDesconto LoadObject(int idCodigoDesconto, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idCodigoDesconto, trans))
            {
                CodigoDesconto objCodigoDesconto = new CodigoDesconto();
                if (SetInstance(dr, objCodigoDesconto))
                    return objCodigoDesconto;
            }
            throw (new RecordNotFoundException(typeof(CodigoDesconto)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de CodigoDesconto a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idCodigoDesconto))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de CodigoDesconto a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idCodigoDesconto, trans))
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
        /// <param name="objCodigoDesconto">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, CodigoDesconto objCodigoDesconto)
        {
            try
            {
                if (dr.Read())
                {
                    objCodigoDesconto._idCodigoDesconto = Convert.ToInt32(dr["Idf_Codigo_Desconto"]);
                    objCodigoDesconto._descricaoCodigoDesconto = Convert.ToString(dr["Des_Codigo_Desconto"]);
                    objCodigoDesconto._statusCodigoDesconto = new StatusCodigoDesconto(Convert.ToInt32(dr["Idf_Status_Codigo_Desconto"]));
                    if (dr["Idf_Parceiro"] != DBNull.Value)
                        objCodigoDesconto._parceiro = new Parceiro(Convert.ToInt32(dr["Idf_Parceiro"]));
                    if (dr["Idf_Tipo_Codigo_Desconto"] != DBNull.Value)
                        objCodigoDesconto._tipoCodigoDesconto = new TipoCodigoDesconto(Convert.ToInt32(dr["Idf_Tipo_Codigo_Desconto"]));
                    objCodigoDesconto._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Dta_Alteracao"] != DBNull.Value)
                        objCodigoDesconto._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
                    if (dr["Dta_Utilizacao"] != DBNull.Value)
                        objCodigoDesconto._dataUtilizacao = Convert.ToDateTime(dr["Dta_Utilizacao"]);
                    if (dr["Dta_Validade_Inicio"] != DBNull.Value)
                        objCodigoDesconto._dataValidadeInicio = Convert.ToDateTime(dr["Dta_Validade_Inicio"]);
                    if (dr["Dta_Validade_Fim"] != DBNull.Value)
                        objCodigoDesconto._dataValidadeFim = Convert.ToDateTime(dr["Dta_Validade_Fim"]);
                    if (dr["Des_Identificacao_Codigo"] != DBNull.Value)
                        objCodigoDesconto._descricaoIdentificacaoCodigo = Convert.ToString(dr["Des_Identificacao_Codigo"]);
                    if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                        objCodigoDesconto._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));

                    objCodigoDesconto._persisted = true;
                    objCodigoDesconto._modified = false;

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