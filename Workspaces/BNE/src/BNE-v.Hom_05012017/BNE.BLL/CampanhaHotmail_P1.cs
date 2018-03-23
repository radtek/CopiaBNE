//-- Data: 18/04/2016 16:51
//-- Autor: mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
	public partial class CampanhaHotmail // Tabela: TAB_Campanha_Hotmail
	{
        #region Atributos
        private decimal _numeroCPF;
        private DateTime? _dataNascimento;
        private string _emailPessoa;
        private int _idSituacaoCurriculo;
        private int _idCurriculo;
        private DateTime _dataAtualizacao;
        private bool? _flagEnviado;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region NumeroCPF
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public decimal NumeroCPF
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

        #region EmailPessoa
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string EmailPessoa
        {
            get
            {
                return this._emailPessoa;
            }
            set
            {
                this._emailPessoa = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdSituacaoCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int IdSituacaoCurriculo
        {
            get
            {
                return this._idSituacaoCurriculo;
            }
            set
            {
                this._idSituacaoCurriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int IdCurriculo
        {
            get
            {
                return this._idCurriculo;
            }
            set
            {
                this._idCurriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAtualizacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataAtualizacao
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

        #region FlagEnviado
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagEnviado
        {
            get
            {
                return this._flagEnviado;
            }
            set
            {
                this._flagEnviado = value;
                this._modified = true;
            }
        }
        #endregion 

        #endregion

        //#region Construtores
        //public CampanhaHotmail()
        //{
        //}
		
        //#endregion

        //#region Consultas
        //private const string SPINSERT = "INSERT INTO TAB_Campanha_Hotmail (Num_CPF, Dta_Nascimento, Eml_Pessoa, Idf_Situacao_Curriculo, Idf_Curriculo, Dta_Atualizacao, Flg_Enviado) VALUES (@Num_CPF, @Dta_Nascimento, @Eml_Pessoa, @Idf_Situacao_Curriculo, @Idf_Curriculo, @Dta_Atualizacao, @Flg_Enviado);";
        //private const string SPUPDATE = "UPDATE TAB_Campanha_Hotmail SET Num_CPF = @Num_CPF, Dta_Nascimento = @Dta_Nascimento, Eml_Pessoa = @Eml_Pessoa, Idf_Situacao_Curriculo = @Idf_Situacao_Curriculo, Idf_Curriculo = @Idf_Curriculo, Dta_Atualizacao = @Dta_Atualizacao, Flg_Enviado = @Flg_Enviado";
        //private const string SPDELETE = "DELETE FROM TAB_Campanha_Hotmail";
        //private const string SPSELECTID = "SELECT * FROM TAB_Campanha_Hotmail WITH(NOLOCK) ";
        //#endregion

        //#region Métodos

        //#region GetParameters
        ///// <summary>
        ///// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        ///// </summary>
        ///// <returns>Lista de parâmetros SQL.</returns>
        ///// <remarks>mailson</remarks>
        //private List<SqlParameter> GetParameters()
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
        //    parms.Add(new SqlParameter("@Dta_Nascimento", SqlDbType.DateTime, 3));
        //    parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100));
        //    parms.Add(new SqlParameter("@Idf_Situacao_Curriculo", SqlDbType.Int, 4));
        //    parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
        //    parms.Add(new SqlParameter("@Dta_Atualizacao", SqlDbType.DateTime, 8));
        //    parms.Add(new SqlParameter("@Flg_Enviado", SqlDbType.Bit, 1));
        //    return(parms);
        //}
        //#endregion

        //#region SetParameters
        ///// <summary>
        ///// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        ///// </summary>
        ///// <param name="parms">Lista de parâmetros SQL.</param>
        ///// <remarks>mailson</remarks>
        //private void SetParameters(List<SqlParameter> parms)
        //{
        //    parms[0].Value = this._numeroCPF;

        //    if (this._dataNascimento.HasValue)
        //        parms[1].Value = this._dataNascimento;
        //    else
        //        parms[1].Value = DBNull.Value;


        //    if (!String.IsNullOrEmpty(this._emailPessoa))
        //        parms[2].Value = this._emailPessoa;
        //    else
        //        parms[2].Value = DBNull.Value;

        //    parms[3].Value = this._idSituacaoCurriculo;
        //    parms[4].Value = this._idCurriculo;
        //    parms[5].Value = this._dataAtualizacao;

        //    if (this._flagEnviado.HasValue)
        //        parms[6].Value = this._flagEnviado;
        //    else
        //        parms[6].Value = DBNull.Value;


        //    if (!this._persisted)
        //    {
        //    }
        //}
        //#endregion

        //#region Insert
        ///// <summary>
        ///// Método utilizado para inserir uma instância de CampanhaHotmail no banco de dados.
        ///// </summary>
        ///// <remarks>mailson</remarks>
        //private void Insert()
        //{
        //    List<SqlParameter> parms = GetParameters();
        //    SetParameters(parms);

        //    using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
        //    {
        //        conn.Open();
        //        using (SqlTransaction trans = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
        //                cmd.Parameters.Clear();
        //                this._persisted = true;
        //                this._modified = false;
        //                trans.Commit();
        //            }
        //            catch
        //            {
        //                trans.Rollback();
        //                throw;
        //            }
        //        }
        //    }
        //}
        ///// <summary>
        ///// Método utilizado para inserir uma instância de CampanhaHotmail no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>mailson</remarks>
        //private void Insert(SqlTransaction trans)
        //{
        //    List<SqlParameter> parms = GetParameters();
        //    SetParameters(parms);
        //    SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
        //    cmd.Parameters.Clear();
        //    this._persisted = true;
        //    this._modified = false;
        //}
        //#endregion

        //#region Update
        ///// <summary>
        ///// Método utilizado para atualizar uma instância de CampanhaHotmail no banco de dados.
        ///// </summary>
        ///// <remarks>mailson</remarks>
        //private void Update()
        //{
        //    if (this._modified)
        //    {
        //        List<SqlParameter> parms = GetParameters();
        //        SetParameters(parms);
        //        DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATE, parms);
        //        this._modified = false;
        //    }
        //}
        ///// <summary>
        ///// Método utilizado para atualizar uma instância de CampanhaHotmail no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>mailson</remarks>
        //private void Update(SqlTransaction trans)
        //{
        //    if (this._modified)
        //    {
        //        List<SqlParameter> parms = GetParameters();
        //        SetParameters(parms);
        //        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
        //        this._modified = false;
        //    }
        //}
        //#endregion

        //#region Save
        ///// <summary>
        ///// Método utilizado para salvar uma instância de CampanhaHotmail no banco de dados.
        ///// </summary>
        ///// <remarks>mailson</remarks>
        //public void Save()
        //{
        //    if (!this._persisted)
        //        this.Insert();
        //    else
        //        this.Update();
        //}
        ///// <summary>
        ///// Método utilizado para salvar uma instância de CampanhaHotmail no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>mailson</remarks>
        //public void Save(SqlTransaction trans)
        //{
        //    if (!this._persisted)
        //        this.Insert(trans);
        //    else
        //        this.Update(trans);
        //}
        //#endregion

        //#region Delete
        ///// <summary>
        ///// Método utilizado para excluir uma instância de CampanhaHotmail no banco de dados.
        ///// </summary>
        ///// <remarks>mailson</remarks>
        //public static void Delete()
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();


        //    DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        //}
        ///// <summary>
        ///// Método utilizado para excluir uma instância de CampanhaHotmail no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>mailson</remarks>
        ////public static void Delete(, SqlTransaction trans)
        ////{
        ////    List<SqlParameter> parms = new List<SqlParameter>();


        ////    DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        ////}
        ////#endregion

        //#region LoadDataReader
        ///// <summary>
        ///// Método utilizado por retornar as colunas de um registro no banco de dados.
        ///// </summary>
        ///// <returns>Cursor de leitura do banco de dados.</returns>
        ///// <remarks>mailson</remarks>
        //private static IDataReader LoadDataReader()
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();


        //    return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        //}
        ///// <summary>
        ///// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <returns>Cursor de leitura do banco de dados.</returns>
        ///// <remarks>mailson</remarks>
        ////private static IDataReader LoadDataReader(, SqlTransaction trans)
        ////{
        ////    List<SqlParameter> parms = new List<SqlParameter>();


        ////    return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
        ////}
        ///// <summary>
        ///// Método utilizado por retornar uma consulta paginada do banco de dados.
        ///// </summary>
        ///// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
        ///// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
        ///// <param name="paginaCorrente">Número da página que será exibida.</param>
        ///// <param name="tamanhoPagina">Quantidade de itens em cada página.</param>
        ///// <param name="totalRegistros">Quantidade total de registros na tabela.</param>
        ///// <returns>Cursor de leitura do banco de dados.</returns>
        //public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        //{
        //    int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
        //    int fim = paginaCorrente * tamanhoPagina;

        //    string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cam.Num_CPF, Cam.Dta_Nascimento, Cam.Eml_Pessoa, Cam.Idf_Situacao_Curriculo, Cam.Idf_Curriculo, Cam.Dta_Atualizacao, Cam.Flg_Enviado FROM TAB_Campanha_Hotmail Cam";
        //    string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
        //    SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

        //    totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
        //    return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        //}
        //#endregion

        ////#region LoadObject
        /////// <summary>
        /////// Método utilizado para retornar uma instância de CampanhaHotmail a partir do banco de dados.
        /////// </summary>
        /////// <returns>Instância de CampanhaHotmail.</returns>
        /////// <remarks>mailson</remarks>
        ////public static CampanhaHotmail LoadObject()
        ////{
        ////    using (IDataReader dr = LoadDataReader())
        ////    {
        ////        CampanhaHotmail objCampanhaHotmail = new CampanhaHotmail();
        ////        if (SetInstance(dr, objCampanhaHotmail))
        ////            return objCampanhaHotmail;
        ////    }
        ////    throw (new RecordNotFoundException(typeof(CampanhaHotmail)));
        ////}
        /////// <summary>
        /////// Método utilizado para retornar uma instância de CampanhaHotmail a partir do banco de dados, dentro de uma transação.
        /////// </summary>
        /////// <param name="trans">Transação existente no banco de dados.</param>
        /////// <returns>Instância de CampanhaHotmail.</returns>
        /////// <remarks>mailson</remarks>
        ////public static CampanhaHotmail LoadObject(, SqlTransaction trans)
        ////{
        ////    using (IDataReader dr = LoadDataReader(, trans))
        ////    {
        ////        CampanhaHotmail objCampanhaHotmail = new CampanhaHotmail();
        ////        if (SetInstance(dr, objCampanhaHotmail))
        ////            return objCampanhaHotmail;
        ////    }
        ////    throw (new RecordNotFoundException(typeof(CampanhaHotmail)));
        ////}
        ////#endregion

        ////#region CompleteObject
        /////// <summary>
        /////// Método utilizado para completar uma instância de CampanhaHotmail a partir do banco de dados.
        /////// </summary>
        /////// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /////// <remarks>mailson</remarks>
        ////public bool CompleteObject()
        ////{
        ////    using (IDataReader dr = LoadDataReader())
        ////    {
        ////        return SetInstance(dr, this);
        ////    }
        ////}
        /////// <summary>
        /////// Método utilizado para completar uma instância de CampanhaHotmail a partir do banco de dados, dentro de uma transação.
        /////// </summary>
        /////// <param name="trans">Transação existente no banco de dados.</param>
        /////// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /////// <remarks>mailson</remarks>
        ////public bool CompleteObject(SqlTransaction trans)
        ////{
        ////    using (IDataReader dr = LoadDataReader(, trans))
        ////    {
        ////        return SetInstance(dr, this);
        ////    }
        ////}
        ////#endregion

        //#region SetInstance
        ///// <summary>
        ///// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        ///// </summary>
        ///// <param name="dr">Cursor de leitura do banco de dados.</param>
        ///// <param name="objCampanhaHotmail">Instância a ser manipulada.</param>
        ///// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        ///// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        ///// <remarks>mailson</remarks>
        //private static bool SetInstance(IDataReader dr, CampanhaHotmail objCampanhaHotmail, bool dispose = true)
        //{
        //    try
        //    {
        //        if (dr.Read())
        //        {
        //            objCampanhaHotmail._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
        //            if (dr["Dta_Nascimento"] != DBNull.Value)
        //                objCampanhaHotmail._dataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
        //            if (dr["Eml_Pessoa"] != DBNull.Value)
        //                objCampanhaHotmail._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
        //            objCampanhaHotmail._idSituacaoCurriculo = Convert.ToInt32(dr["Idf_Situacao_Curriculo"]);
        //            objCampanhaHotmail._idCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
        //            objCampanhaHotmail._dataAtualizacao = Convert.ToDateTime(dr["Dta_Atualizacao"]);
        //            if (dr["Flg_Enviado"] != DBNull.Value)
        //                objCampanhaHotmail._flagEnviado = Convert.ToBoolean(dr["Flg_Enviado"]);

        //            objCampanhaHotmail._persisted = true;
        //            objCampanhaHotmail._modified = false;

        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch 
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (dispose)
        //            dr.Dispose();
        //    }
        //}
        //#endregion

        //#endregion
        //#endregion
    }
}
        