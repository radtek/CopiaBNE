//-- Data: 18/09/2013 18:15
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class Campanha // Tabela: BNE_Campanha
    {
        //#region Atributos
        //private int _idCampanha;
        //private CelularSelecionador _celularSelecionador;
        //private string _nomeCampanha;
        //private DateTime _dataCadastro;
        //private DateTime? _dataEnvio;
        //private string _descricaoMensagem;

        //private bool _persisted;
        //private bool _modified;
        //#endregion

        //#region Propriedades

        //#region IdCampanha
        ///// <summary>
        ///// Campo obrigatório.
        ///// Campo auto-numerado.
        ///// </summary>
        //public int IdCampanha
        //{
        //    get
        //    {
        //        return this._idCampanha;
        //    }
        //}
        //#endregion

        //#region CelularSelecionador
        ///// <summary>
        ///// Campo obrigatório.
        ///// </summary>
        //public CelularSelecionador CelularSelecionador
        //{
        //    get
        //    {
        //        return this._celularSelecionador;
        //    }
        //    set
        //    {
        //        this._celularSelecionador = value;
        //        this._modified = true;
        //    }
        //}
        //#endregion

        //#region NomeCampanha
        ///// <summary>
        ///// Tamanho do campo: 200.
        ///// Campo obrigatório.
        ///// </summary>
        //public string NomeCampanha
        //{
        //    get
        //    {
        //        return this._nomeCampanha;
        //    }
        //    set
        //    {
        //        this._nomeCampanha = value;
        //        this._modified = true;
        //    }
        //}
        //#endregion

        //#region DataCadastro
        ///// <summary>
        ///// Campo obrigatório.
        ///// </summary>
        //public DateTime DataCadastro
        //{
        //    get
        //    {
        //        return this._dataCadastro;
        //    }
        //}
        //#endregion

        //#region DataEnvio
        ///// <summary>
        ///// Campo opcional.
        ///// </summary>
        //public DateTime? DataEnvio
        //{
        //    get
        //    {
        //        return this._dataEnvio;
        //    }
        //    set
        //    {
        //        this._dataEnvio = value;
        //        this._modified = true;
        //    }
        //}
        //#endregion

        //#region DescricaoMensagem
        ///// <summary>
        ///// Tamanho do campo: 200.
        ///// Campo obrigatório.
        ///// </summary>
        //public string DescricaoMensagem
        //{
        //    get
        //    {
        //        return this._descricaoMensagem;
        //    }
        //    set
        //    {
        //        this._descricaoMensagem = value;
        //        this._modified = true;
        //    }
        //}
        //#endregion

        //#endregion

        //#region Construtores
        //public Campanha()
        //{
        //}
        //public Campanha(int idCampanha)
        //{
        //    this._idCampanha = idCampanha;
        //    this._persisted = true;
        //}
        //#endregion

        //#region Consultas
        //private const string SPINSERT = "INSERT INTO BNE_Campanha (Idf_Celular_Selecionador, Nme_Campanha, Dta_Cadastro, Dta_Envio, Des_Mensagem) VALUES (@Idf_Celular_Selecionador, @Nme_Campanha, @Dta_Cadastro, @Dta_Envio, @Des_Mensagem);SET @Idf_Campanha = SCOPE_IDENTITY();";
        //private const string SPUPDATE = "UPDATE BNE_Campanha SET Idf_Celular_Selecionador = @Idf_Celular_Selecionador, Nme_Campanha = @Nme_Campanha, Dta_Cadastro = @Dta_Cadastro, Dta_Envio = @Dta_Envio, Des_Mensagem = @Des_Mensagem WHERE Idf_Campanha = @Idf_Campanha";
        //private const string SPDELETE = "DELETE FROM BNE_Campanha WHERE Idf_Campanha = @Idf_Campanha";
        //private const string SPSELECTID = "SELECT * FROM BNE_Campanha WHERE Idf_Campanha = @Idf_Campanha";
        //#endregion

        //#region Métodos

        //#region GetParameters
        ///// <summary>
        ///// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        ///// </summary>
        ///// <returns>Lista de parâmetros SQL.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private List<SqlParameter> GetParameters()
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Idf_Campanha", SqlDbType.Int, 4));
        //    parms.Add(new SqlParameter("@Idf_Celular_Selecionador", SqlDbType.Int, 4));
        //    parms.Add(new SqlParameter("@Nme_Campanha", SqlDbType.VarChar, 200));
        //    parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
        //    parms.Add(new SqlParameter("@Dta_Envio", SqlDbType.DateTime, 8));
        //    parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar, 200));
        //    return (parms);
        //}
        //#endregion

        //#region SetParameters
        ///// <summary>
        ///// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        ///// </summary>
        ///// <param name="parms">Lista de parâmetros SQL.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private void SetParameters(List<SqlParameter> parms)
        //{
        //    parms[0].Value = this._idCampanha;
        //    parms[1].Value = this._celularSelecionador.IdCelularSelecionador;
        //    parms[2].Value = this._nomeCampanha;

        //    if (this._dataEnvio.HasValue)
        //        parms[4].Value = this._dataEnvio;
        //    else
        //        parms[4].Value = DBNull.Value;

        //    parms[5].Value = this._descricaoMensagem;

        //    if (!this._persisted)
        //    {
        //        parms[0].Direction = ParameterDirection.Output;
        //        this._dataCadastro = DateTime.Now;
        //    }
        //    else
        //    {
        //        parms[0].Direction = ParameterDirection.Input;
        //    }
        //    parms[3].Value = this._dataCadastro;
        //}
        //#endregion

        //#region Insert
        ///// <summary>
        ///// Método utilizado para inserir uma instância de Campanha no banco de dados.
        ///// </summary>
        ///// <remarks>Gieyson Stelmak</remarks>
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
        //                this._idCampanha = Convert.ToInt32(cmd.Parameters["@Idf_Campanha"].Value);
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
        ///// Método utilizado para inserir uma instância de Campanha no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private void Insert(SqlTransaction trans)
        //{
        //    List<SqlParameter> parms = GetParameters();
        //    SetParameters(parms);
        //    SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
        //    this._idCampanha = Convert.ToInt32(cmd.Parameters["@Idf_Campanha"].Value);
        //    cmd.Parameters.Clear();
        //    this._persisted = true;
        //    this._modified = false;
        //}
        //#endregion

        //#region Update
        ///// <summary>
        ///// Método utilizado para atualizar uma instância de Campanha no banco de dados.
        ///// </summary>
        ///// <remarks>Gieyson Stelmak</remarks>
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
        ///// Método utilizado para atualizar uma instância de Campanha no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
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
        ///// Método utilizado para salvar uma instância de Campanha no banco de dados.
        ///// </summary>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public void Save()
        //{
        //    if (!this._persisted)
        //        this.Insert();
        //    else
        //        this.Update();
        //}
        ///// <summary>
        ///// Método utilizado para salvar uma instância de Campanha no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
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
        ///// Método utilizado para excluir uma instância de Campanha no banco de dados.
        ///// </summary>
        ///// <param name="idCampanha">Chave do registro.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static void Delete(int idCampanha)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Idf_Campanha", SqlDbType.Int, 4));

        //    parms[0].Value = idCampanha;

        //    DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        //}
        ///// <summary>
        ///// Método utilizado para excluir uma instância de Campanha no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="idCampanha">Chave do registro.</param>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static void Delete(int idCampanha, SqlTransaction trans)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Idf_Campanha", SqlDbType.Int, 4));

        //    parms[0].Value = idCampanha;

        //    DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        //}
        ///// <summary>
        ///// Método utilizado para excluir uma lista de Campanha no banco de dados.
        ///// </summary>
        ///// <param name="idCampanha">Lista de chaves.</param>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static void Delete(List<int> idCampanha)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    string query = "delete from BNE_Campanha where Idf_Campanha in (";

        //    for (int i = 0; i < idCampanha.Count; i++)
        //    {
        //        string nomeParametro = "@parm" + i.ToString();

        //        if (i > 0)
        //        {
        //            query += ", ";
        //        }
        //        query += nomeParametro;
        //        parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
        //        parms[i].Value = idCampanha[i];
        //    }

        //    query += ")";

        //    DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        //}
        //#endregion

        //#region LoadDataReader
        ///// <summary>
        ///// Método utilizado por retornar as colunas de um registro no banco de dados.
        ///// </summary>
        ///// <param name="idCampanha">Chave do registro.</param>
        ///// <returns>Cursor de leitura do banco de dados.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private static IDataReader LoadDataReader(int idCampanha)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Idf_Campanha", SqlDbType.Int, 4));

        //    parms[0].Value = idCampanha;

        //    return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        //}
        ///// <summary>
        ///// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="idCampanha">Chave do registro.</param>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <returns>Cursor de leitura do banco de dados.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private static IDataReader LoadDataReader(int idCampanha, SqlTransaction trans)
        //{
        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Idf_Campanha", SqlDbType.Int, 4));

        //    parms[0].Value = idCampanha;

        //    return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
        //}
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

        //    string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cam.Idf_Campanha, Cam.Idf_Celular_Selecionador, Cam.Nme_Campanha, Cam.Dta_Cadastro, Cam.Dta_Envio, Cam.Des_Mensagem FROM BNE_Campanha Cam";
        //    string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
        //    SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

        //    totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
        //    return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        //}
        //#endregion

        //#region LoadObject
        ///// <summary>
        ///// Método utilizado para retornar uma instância de Campanha a partir do banco de dados.
        ///// </summary>
        ///// <param name="idCampanha">Chave do registro.</param>
        ///// <returns>Instância de Campanha.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static Campanha LoadObject(int idCampanha)
        //{
        //    using (IDataReader dr = LoadDataReader(idCampanha))
        //    {
        //        Campanha objCampanha = new Campanha();
        //        if (SetInstance(dr, objCampanha))
        //            return objCampanha;
        //    }
        //    throw (new RecordNotFoundException(typeof(Campanha)));
        //}
        ///// <summary>
        ///// Método utilizado para retornar uma instância de Campanha a partir do banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="idCampanha">Chave do registro.</param>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <returns>Instância de Campanha.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public static Campanha LoadObject(int idCampanha, SqlTransaction trans)
        //{
        //    using (IDataReader dr = LoadDataReader(idCampanha, trans))
        //    {
        //        Campanha objCampanha = new Campanha();
        //        if (SetInstance(dr, objCampanha))
        //            return objCampanha;
        //    }
        //    throw (new RecordNotFoundException(typeof(Campanha)));
        //}
        //#endregion

        //#region CompleteObject
        ///// <summary>
        ///// Método utilizado para completar uma instância de Campanha a partir do banco de dados.
        ///// </summary>
        ///// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public bool CompleteObject()
        //{
        //    using (IDataReader dr = LoadDataReader(this._idCampanha))
        //    {
        //        return SetInstance(dr, this);
        //    }
        //}
        ///// <summary>
        ///// Método utilizado para completar uma instância de Campanha a partir do banco de dados, dentro de uma transação.
        ///// </summary>
        ///// <param name="trans">Transação existente no banco de dados.</param>
        ///// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //public bool CompleteObject(SqlTransaction trans)
        //{
        //    using (IDataReader dr = LoadDataReader(this._idCampanha, trans))
        //    {
        //        return SetInstance(dr, this);
        //    }
        //}
        //#endregion

        //#region SetInstance
        ///// <summary>
        ///// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        ///// </summary>
        ///// <param name="dr">Cursor de leitura do banco de dados.</param>
        ///// <param name="objCampanha">Instância a ser manipulada.</param>
        ///// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        ///// <remarks>Gieyson Stelmak</remarks>
        //private static bool SetInstance(IDataReader dr, Campanha objCampanha)
        //{
        //    try
        //    {
        //        if (dr.Read())
        //        {
        //            objCampanha._idCampanha = Convert.ToInt32(dr["Idf_Campanha"]);
        //            objCampanha._celularSelecionador = new CelularSelecionador(Convert.ToInt32(dr["Idf_Celular_Selecionador"]));
        //            objCampanha._nomeCampanha = Convert.ToString(dr["Nme_Campanha"]);
        //            objCampanha._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
        //            if (dr["Dta_Envio"] != DBNull.Value)
        //                objCampanha._dataEnvio = Convert.ToDateTime(dr["Dta_Envio"]);
        //            objCampanha._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);

        //            objCampanha._persisted = true;
        //            objCampanha._modified = false;

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
        //        dr.Dispose();
        //    }
        //}
        //#endregion

        //#endregion
    }
}