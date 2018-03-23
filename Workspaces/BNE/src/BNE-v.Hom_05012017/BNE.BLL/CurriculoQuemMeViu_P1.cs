//-- Data: 27/05/2016 09:48
//-- Autor: mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CurriculoQuemMeViu // Tabela: BNE_Curriculo_Quem_Me_Viu
    {
        #region Atributos
        private Int64 _idCurriculoQuemMeViu;
        private Curriculo _curriculo;
        private DateTime _dataQuemMeViu;
        private Filial _filial;
        private bool _flagInativo;
        private OrigemCurriculoQuemMeViu _origemCurriculoQuemMeViu;
        private UsuarioFilialPerfil _usuarioFilialPerfil;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdCurriculoQuemMeViu
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public Int64 IdCurriculoQuemMeViu
        {
            get
            {
                return this._idCurriculoQuemMeViu;
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

        #region DataQuemMeViu
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataQuemMeViu
        {
            get
            {
                return this._dataQuemMeViu;
            }
            set
            {
                this._dataQuemMeViu = value;
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

        #region OrigemCurriculoQuemMeViu
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public OrigemCurriculoQuemMeViu OrigemCurriculoQuemMeViu
        {
            get
            {
                return this._origemCurriculoQuemMeViu;
            }
            set
            {
                this._origemCurriculoQuemMeViu = value;
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
        public CurriculoQuemMeViu()
        {
        }
        public CurriculoQuemMeViu(Int64 idCurriculoQuemMeViu)
        {
            this._idCurriculoQuemMeViu = idCurriculoQuemMeViu;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Curriculo_Quem_Me_Viu (Idf_Curriculo, Dta_Quem_Me_Viu, Idf_Filial, Flg_Inativo, Idf_Origem_Curriculo_Quem_Me_Viu, Idf_Usuario_Filial_Perfil) VALUES (@Idf_Curriculo, @Dta_Quem_Me_Viu, @Idf_Filial, @Flg_Inativo, @Idf_Origem_Curriculo_Quem_Me_Viu, @Idf_Usuario_Filial_Perfil);SET @Idf_Curriculo_Quem_Me_Viu = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Curriculo_Quem_Me_Viu SET Idf_Curriculo = @Idf_Curriculo, Dta_Quem_Me_Viu = @Dta_Quem_Me_Viu, Idf_Filial = @Idf_Filial, Flg_Inativo = @Flg_Inativo, Idf_Origem_Curriculo_Quem_Me_Viu = @Idf_Origem_Curriculo_Quem_Me_Viu, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil WHERE Idf_Curriculo_Quem_Me_Viu = @Idf_Curriculo_Quem_Me_Viu";
        private const string SPDELETE = "DELETE FROM BNE_Curriculo_Quem_Me_Viu WHERE Idf_Curriculo_Quem_Me_Viu = @Idf_Curriculo_Quem_Me_Viu";
        private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Quem_Me_Viu WITH(NOLOCK) WHERE Idf_Curriculo_Quem_Me_Viu = @Idf_Curriculo_Quem_Me_Viu";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>mailson</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Quem_Me_Viu", SqlDbType.BigInt, 8));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Quem_Me_Viu", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Origem_Curriculo_Quem_Me_Viu", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>mailson</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idCurriculoQuemMeViu;
            parms[1].Value = this._curriculo.IdCurriculo;
            parms[2].Value = this._dataQuemMeViu;
            parms[3].Value = this._filial.IdFilial;
            parms[4].Value = this._flagInativo;

            if (this._origemCurriculoQuemMeViu != null)
                parms[5].Value = this._origemCurriculoQuemMeViu.IdOrigemCurriculoQuemMeViu;
            else
                parms[5].Value = DBNull.Value;


            if (this._usuarioFilialPerfil != null)
                parms[6].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            else
                parms[6].Value = DBNull.Value;


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
        /// Método utilizado para inserir uma instância de CurriculoQuemMeViu no banco de dados.
        /// </summary>
        /// <remarks>mailson</remarks>
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
                        this._idCurriculoQuemMeViu = Convert.ToInt64(cmd.Parameters["@Idf_Curriculo_Quem_Me_Viu"].Value);
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
        /// Método utilizado para inserir uma instância de CurriculoQuemMeViu no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idCurriculoQuemMeViu = Convert.ToInt64(cmd.Parameters["@Idf_Curriculo_Quem_Me_Viu"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de CurriculoQuemMeViu no banco de dados.
        /// </summary>
        /// <remarks>mailson</remarks>
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
        /// Método utilizado para atualizar uma instância de CurriculoQuemMeViu no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
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
        /// Método utilizado para salvar uma instância de CurriculoQuemMeViu no banco de dados.
        /// </summary>
        /// <remarks>mailson</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de CurriculoQuemMeViu no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
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
        /// Método utilizado para excluir uma instância de CurriculoQuemMeViu no banco de dados.
        /// </summary>
        /// <param name="idCurriculoQuemMeViu">Chave do registro.</param>
        /// <remarks>mailson</remarks>
        public static void Delete(Int64 idCurriculoQuemMeViu)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Quem_Me_Viu", SqlDbType.BigInt, 8));

            parms[0].Value = idCurriculoQuemMeViu;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de CurriculoQuemMeViu no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoQuemMeViu">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
        public static void Delete(Int64 idCurriculoQuemMeViu, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Quem_Me_Viu", SqlDbType.BigInt, 8));

            parms[0].Value = idCurriculoQuemMeViu;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de CurriculoQuemMeViu no banco de dados.
        /// </summary>
        /// <param name="idCurriculoQuemMeViu">Lista de chaves.</param>
        /// <remarks>mailson</remarks>
        public static void Delete(List<Int64> idCurriculoQuemMeViu)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Curriculo_Quem_Me_Viu where Idf_Curriculo_Quem_Me_Viu in (";

            for (int i = 0; i < idCurriculoQuemMeViu.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.BigInt, 8));
                parms[i].Value = idCurriculoQuemMeViu[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCurriculoQuemMeViu">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>mailson</remarks>
        private static IDataReader LoadDataReader(Int64 idCurriculoQuemMeViu)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Quem_Me_Viu", SqlDbType.BigInt, 8));

            parms[0].Value = idCurriculoQuemMeViu;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoQuemMeViu">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>mailson</remarks>
        private static IDataReader LoadDataReader(Int64 idCurriculoQuemMeViu, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Quem_Me_Viu", SqlDbType.BigInt, 8));

            parms[0].Value = idCurriculoQuemMeViu;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Quem_Me_Viu, Cur.Idf_Curriculo, Cur.Dta_Quem_Me_Viu, Cur.Idf_Filial, Cur.Flg_Inativo, Cur.Idf_Origem_Curriculo_Quem_Me_Viu, Cur.Idf_Usuario_Filial_Perfil FROM BNE_Curriculo_Quem_Me_Viu Cur";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de CurriculoQuemMeViu a partir do banco de dados.
        /// </summary>
        /// <param name="idCurriculoQuemMeViu">Chave do registro.</param>
        /// <returns>Instância de CurriculoQuemMeViu.</returns>
        /// <remarks>mailson</remarks>
        public static CurriculoQuemMeViu LoadObject(Int64 idCurriculoQuemMeViu)
        {
            using (IDataReader dr = LoadDataReader(idCurriculoQuemMeViu))
            {
                CurriculoQuemMeViu objCurriculoQuemMeViu = new CurriculoQuemMeViu();
                if (SetInstance(dr, objCurriculoQuemMeViu))
                    return objCurriculoQuemMeViu;
            }
            throw (new RecordNotFoundException(typeof(CurriculoQuemMeViu)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de CurriculoQuemMeViu a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoQuemMeViu">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de CurriculoQuemMeViu.</returns>
        /// <remarks>mailson</remarks>
        public static CurriculoQuemMeViu LoadObject(Int64 idCurriculoQuemMeViu, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idCurriculoQuemMeViu, trans))
            {
                CurriculoQuemMeViu objCurriculoQuemMeViu = new CurriculoQuemMeViu();
                if (SetInstance(dr, objCurriculoQuemMeViu))
                    return objCurriculoQuemMeViu;
            }
            throw (new RecordNotFoundException(typeof(CurriculoQuemMeViu)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de CurriculoQuemMeViu a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>mailson</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idCurriculoQuemMeViu))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de CurriculoQuemMeViu a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>mailson</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idCurriculoQuemMeViu, trans))
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
        /// <param name="objCurriculoQuemMeViu">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>mailson</remarks>
        private static bool SetInstance(IDataReader dr, CurriculoQuemMeViu objCurriculoQuemMeViu, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objCurriculoQuemMeViu._idCurriculoQuemMeViu = Convert.ToInt64(dr["Idf_Curriculo_Quem_Me_Viu"]);
                    objCurriculoQuemMeViu._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    objCurriculoQuemMeViu._dataQuemMeViu = Convert.ToDateTime(dr["Dta_Quem_Me_Viu"]);
                    objCurriculoQuemMeViu._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                    objCurriculoQuemMeViu._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Idf_Origem_Curriculo_Quem_Me_Viu"] != DBNull.Value)
                        objCurriculoQuemMeViu._origemCurriculoQuemMeViu = new OrigemCurriculoQuemMeViu(Convert.ToInt32(dr["Idf_Origem_Curriculo_Quem_Me_Viu"]));
                    if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                        objCurriculoQuemMeViu._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));

                    objCurriculoQuemMeViu._persisted = true;
                    objCurriculoQuemMeViu._modified = false;

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