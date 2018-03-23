//-- Data: 04/10/2016 16:02
//-- Autor: Marty Sroka

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class InformativoVip // Tabela: BNE_Informativo_Vip
    {
        #region Atributos
        private int _idInformativoVip;
        private string _descricaoInformativoVip;
        private TipoInformativoVip _tipoInformativoVip;
        private string _valorInformativoVip;
        private bool _flagInativo;
        private string _descricaoAssuntoInformativoVip;
        private string _descricaoMensagemSMS;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdInformativoVip
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdInformativoVip
        {
            get
            {
                return this._idInformativoVip;
            }
        }
        #endregion

        #region DescricaoInformativoVip
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoInformativoVip
        {
            get
            {
                return this._descricaoInformativoVip;
            }
            set
            {
                this._descricaoInformativoVip = value;
                this._modified = true;
            }
        }
        #endregion

        #region TipoInformativoVip
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public TipoInformativoVip TipoInformativoVip
        {
            get
            {
                return this._tipoInformativoVip;
            }
            set
            {
                this._tipoInformativoVip = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorInformativoVip
        /// <summary>
        /// Tamanho do campo: -1.
        /// Campo obrigatório.
        /// </summary>
        public string ValorInformativoVip
        {
            get
            {
                return this._valorInformativoVip;
            }
            set
            {
                this._valorInformativoVip = value;
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

        #region DescricaoAssuntoInformativoVip
        /// <summary>
        /// Tamanho do campo: 80.
        /// Campo opcional.
        /// </summary>
        public string DescricaoAssuntoInformativoVip
        {
            get
            {
                return this._descricaoAssuntoInformativoVip;
            }
            set
            {
                this._descricaoAssuntoInformativoVip = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoMensagemSMS
        /// <summary>
        /// Tamanho do campo: 160.
        /// Campo opcional.
        /// </summary>
        public string DescricaoMensagemSMS
        {
            get
            {
                return this._descricaoMensagemSMS;
            }
            set
            {
                this._descricaoMensagemSMS = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public InformativoVip()
        {
        }
        public InformativoVip(int idInformativoVip)
        {
            this._idInformativoVip = idInformativoVip;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Informativo_Vip (Des_Informativo_Vip, Idf_Tipo_Informativo_Vip, Vlr_Informativo_Vip, Flg_Inativo, Des_Assunto_Informativo_Vip, Des_Mensagem_SMS) VALUES (@Des_Informativo_Vip, @Idf_Tipo_Informativo_Vip, @Vlr_Informativo_Vip, @Flg_Inativo, @Des_Assunto_Informativo_Vip, @Des_Mensagem_SMS);SET @Idf_Informativo_Vip = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Informativo_Vip SET Des_Informativo_Vip = @Des_Informativo_Vip, Idf_Tipo_Informativo_Vip = @Idf_Tipo_Informativo_Vip, Vlr_Informativo_Vip = @Vlr_Informativo_Vip, Flg_Inativo = @Flg_Inativo, Des_Assunto_Informativo_Vip = @Des_Assunto_Informativo_Vip, Des_Mensagem_SMS = @Des_Mensagem_SMS WHERE Idf_Informativo_Vip = @Idf_Informativo_Vip";
        private const string SPDELETE = "DELETE FROM BNE_Informativo_Vip WHERE Idf_Informativo_Vip = @Idf_Informativo_Vip";
        private const string SPSELECTID = "SELECT * FROM BNE_Informativo_Vip WITH(NOLOCK) WHERE Idf_Informativo_Vip = @Idf_Informativo_Vip";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Marty Sroka</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Informativo_Vip", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Informativo_Vip", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Idf_Tipo_Informativo_Vip", SqlDbType.Int, 2));
            parms.Add(new SqlParameter("@Vlr_Informativo_Vip", SqlDbType.VarChar));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Des_Assunto_Informativo_Vip", SqlDbType.VarChar, 80));
            parms.Add(new SqlParameter("@Des_Mensagem_SMS", SqlDbType.VarChar, 160));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Marty Sroka</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idInformativoVip;
            parms[1].Value = this._descricaoInformativoVip;
            parms[2].Value = this._tipoInformativoVip.IdTipoInformativoVip;
            parms[3].Value = this._valorInformativoVip;
            parms[4].Value = this._flagInativo;

            if (!String.IsNullOrEmpty(this._descricaoAssuntoInformativoVip))
                parms[5].Value = this._descricaoAssuntoInformativoVip;
            else
                parms[5].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoMensagemSMS))
                parms[6].Value = this._descricaoMensagemSMS;
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
        /// Método utilizado para inserir uma instância de InformativoVip no banco de dados.
        /// </summary>
        /// <remarks>Marty Sroka</remarks>
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
                        this._idInformativoVip = Convert.ToInt32(cmd.Parameters["@Idf_Informativo_Vip"].Value);
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
        /// Método utilizado para inserir uma instância de InformativoVip no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Marty Sroka</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idInformativoVip = Convert.ToInt32(cmd.Parameters["@Idf_Informativo_Vip"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de InformativoVip no banco de dados.
        /// </summary>
        /// <remarks>Marty Sroka</remarks>
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
        /// Método utilizado para atualizar uma instância de InformativoVip no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Marty Sroka</remarks>
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
        /// Método utilizado para salvar uma instância de InformativoVip no banco de dados.
        /// </summary>
        /// <remarks>Marty Sroka</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de InformativoVip no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Marty Sroka</remarks>
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
        /// Método utilizado para excluir uma instância de InformativoVip no banco de dados.
        /// </summary>
        /// <param name="idInformativoVip">Chave do registro.</param>
        /// <remarks>Marty Sroka</remarks>
        public static void Delete(int idInformativoVip)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Informativo_Vip", SqlDbType.Int, 4));

            parms[0].Value = idInformativoVip;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de InformativoVip no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idInformativoVip">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Marty Sroka</remarks>
        public static void Delete(int idInformativoVip, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Informativo_Vip", SqlDbType.Int, 4));

            parms[0].Value = idInformativoVip;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de InformativoVip no banco de dados.
        /// </summary>
        /// <param name="idInformativoVip">Lista de chaves.</param>
        /// <remarks>Marty Sroka</remarks>
        public static void Delete(List<int> idInformativoVip)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Informativo_Vip where Idf_Informativo_Vip in (";

            for (int i = 0; i < idInformativoVip.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idInformativoVip[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idInformativoVip">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Marty Sroka</remarks>
        private static IDataReader LoadDataReader(int idInformativoVip)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Informativo_Vip", SqlDbType.Int, 4));

            parms[0].Value = idInformativoVip;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idInformativoVip">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Marty Sroka</remarks>
        private static IDataReader LoadDataReader(int idInformativoVip, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Informativo_Vip", SqlDbType.Int, 4));

            parms[0].Value = idInformativoVip;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Inf.Idf_Informativo_Vip, Inf.Des_Informativo_Vip, Inf.Idf_Tipo_Informativo_Vip, Inf.Vlr_Informativo_Vip, Inf.Flg_Inativo, Inf.Des_Assunto_Informativo_Vip, Inf.Des_Mensagem_SMS FROM BNE_Informativo_Vip Inf";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de InformativoVip a partir do banco de dados.
        /// </summary>
        /// <param name="idInformativoVip">Chave do registro.</param>
        /// <returns>Instância de InformativoVip.</returns>
        /// <remarks>Marty Sroka</remarks>
        public static InformativoVip LoadObject(int idInformativoVip)
        {
            using (IDataReader dr = LoadDataReader(idInformativoVip))
            {
                InformativoVip objInformativoVip = new InformativoVip();
                if (SetInstance(dr, objInformativoVip))
                    return objInformativoVip;
            }
            throw (new RecordNotFoundException(typeof(InformativoVip)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de InformativoVip a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idInformativoVip">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de InformativoVip.</returns>
        /// <remarks>Marty Sroka</remarks>
        public static InformativoVip LoadObject(int idInformativoVip, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idInformativoVip, trans))
            {
                InformativoVip objInformativoVip = new InformativoVip();
                if (SetInstance(dr, objInformativoVip))
                    return objInformativoVip;
            }
            throw (new RecordNotFoundException(typeof(InformativoVip)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de InformativoVip a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Marty Sroka</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idInformativoVip))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de InformativoVip a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Marty Sroka</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idInformativoVip, trans))
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
        /// <param name="objInformativoVip">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Marty Sroka</remarks>
        private static bool SetInstance(IDataReader dr, InformativoVip objInformativoVip, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objInformativoVip._idInformativoVip = Convert.ToInt32(dr["Idf_Informativo_Vip"]);
                    objInformativoVip._descricaoInformativoVip = Convert.ToString(dr["Des_Informativo_Vip"]);
                    objInformativoVip._tipoInformativoVip = new TipoInformativoVip(Convert.ToInt16(dr["Idf_Tipo_Informativo_Vip"]));
                    objInformativoVip._valorInformativoVip = Convert.ToString(dr["Vlr_Informativo_Vip"]);
                    objInformativoVip._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Des_Assunto_Informativo_Vip"] != DBNull.Value)
                        objInformativoVip._descricaoAssuntoInformativoVip = Convert.ToString(dr["Des_Assunto_Informativo_Vip"]);
                    if (dr["Des_Mensagem_SMS"] != DBNull.Value)
                        objInformativoVip._descricaoMensagemSMS = Convert.ToString(dr["Des_Mensagem_SMS"]);

                    objInformativoVip._persisted = true;
                    objInformativoVip._modified = false;

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