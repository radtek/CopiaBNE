//-- Data: 03/07/2015 14:15
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PlanoAdquiridoContrato // Tabela: BNE_Plano_Adquirido_Contrato
    {
        #region Atributos
        private int _idPlanoAdquiridoContrato;
        private PlanoAdquirido _planoAdquirido;
        private TemplateContrato _templateContrato;
        private string _descricaoContrato;
        private DateTime _dataCadastro;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPlanoAdquiridoContrato
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPlanoAdquiridoContrato
        {
            get
            {
                return this._idPlanoAdquiridoContrato;
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

        #region TemplateContrato
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public TemplateContrato TemplateContrato
        {
            get
            {
                return this._templateContrato;
            }
            set
            {
                this._templateContrato = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoContrato
        /// <summary>
        /// Tamanho do campo: -1.
        /// Campo opcional.
        /// </summary>
        public string DescricaoContrato
        {
            get
            {
                return this._descricaoContrato;
            }
            set
            {
                this._descricaoContrato = value;
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
        public PlanoAdquiridoContrato()
        {
        }
        public PlanoAdquiridoContrato(int idPlanoAdquiridoContrato)
        {
            this._idPlanoAdquiridoContrato = idPlanoAdquiridoContrato;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Plano_Adquirido_Contrato (Idf_Plano_Adquirido, Idf_Template_Contrato, Des_Contrato, Dta_Cadastro) VALUES (@Idf_Plano_Adquirido, @Idf_Template_Contrato, @Des_Contrato, @Dta_Cadastro);SET @Idf_Plano_Adquirido_Contrato = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Plano_Adquirido_Contrato SET Idf_Plano_Adquirido = @Idf_Plano_Adquirido, Idf_Template_Contrato = @Idf_Template_Contrato, Des_Contrato = @Des_Contrato, Dta_Cadastro = @Dta_Cadastro WHERE Idf_Plano_Adquirido_Contrato = @Idf_Plano_Adquirido_Contrato";
        private const string SPDELETE = "DELETE FROM BNE_Plano_Adquirido_Contrato WHERE Idf_Plano_Adquirido_Contrato = @Idf_Plano_Adquirido_Contrato";
        private const string SPSELECTID = "SELECT * FROM BNE_Plano_Adquirido_Contrato WITH(NOLOCK) WHERE Idf_Plano_Adquirido_Contrato = @Idf_Plano_Adquirido_Contrato";
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
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Template_Contrato", SqlDbType.Int, 2));
            parms.Add(new SqlParameter("@Des_Contrato", SqlDbType.VarChar));
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
            parms[0].Value = this._idPlanoAdquiridoContrato;
            parms[1].Value = this._planoAdquirido.IdPlanoAdquirido;

            if (this._templateContrato != null)
                parms[2].Value = this._templateContrato.IdTemplateContrato;
            else
                parms[2].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoContrato))
                parms[3].Value = this._descricaoContrato;
            else
                parms[3].Value = DBNull.Value;


            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[4].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de PlanoAdquiridoContrato no banco de dados.
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
                        this._idPlanoAdquiridoContrato = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Adquirido_Contrato"].Value);
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
        /// Método utilizado para inserir uma instância de PlanoAdquiridoContrato no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPlanoAdquiridoContrato = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Adquirido_Contrato"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de PlanoAdquiridoContrato no banco de dados.
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
        /// Método utilizado para atualizar uma instância de PlanoAdquiridoContrato no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de PlanoAdquiridoContrato no banco de dados.
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
        /// Método utilizado para salvar uma instância de PlanoAdquiridoContrato no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de PlanoAdquiridoContrato no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoContrato">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlanoAdquiridoContrato)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoContrato;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PlanoAdquiridoContrato no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoContrato">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlanoAdquiridoContrato, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoContrato;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de PlanoAdquiridoContrato no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoContrato">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idPlanoAdquiridoContrato)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Plano_Adquirido_Contrato where Idf_Plano_Adquirido_Contrato in (";

            for (int i = 0; i < idPlanoAdquiridoContrato.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPlanoAdquiridoContrato[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoContrato">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlanoAdquiridoContrato)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoContrato;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoContrato">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlanoAdquiridoContrato, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoContrato;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Adquirido_Contrato, Pla.Idf_Plano_Adquirido, Pla.Idf_Template_Contrato, Pla.Des_Contrato, Pla.Dta_Cadastro FROM BNE_Plano_Adquirido_Contrato Pla";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoAdquiridoContrato a partir do banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoContrato">Chave do registro.</param>
        /// <returns>Instância de PlanoAdquiridoContrato.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PlanoAdquiridoContrato LoadObject(int idPlanoAdquiridoContrato)
        {
            using (IDataReader dr = LoadDataReader(idPlanoAdquiridoContrato))
            {
                PlanoAdquiridoContrato objPlanoAdquiridoContrato = new PlanoAdquiridoContrato();
                if (SetInstance(dr, objPlanoAdquiridoContrato))
                    return objPlanoAdquiridoContrato;
            }
            throw (new RecordNotFoundException(typeof(PlanoAdquiridoContrato)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoAdquiridoContrato a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoContrato">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PlanoAdquiridoContrato.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PlanoAdquiridoContrato LoadObject(int idPlanoAdquiridoContrato, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPlanoAdquiridoContrato, trans))
            {
                PlanoAdquiridoContrato objPlanoAdquiridoContrato = new PlanoAdquiridoContrato();
                if (SetInstance(dr, objPlanoAdquiridoContrato))
                    return objPlanoAdquiridoContrato;
            }
            throw (new RecordNotFoundException(typeof(PlanoAdquiridoContrato)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoAdquiridoContrato a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoAdquiridoContrato))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoAdquiridoContrato a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoAdquiridoContrato, trans))
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
        /// <param name="objPlanoAdquiridoContrato">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, PlanoAdquiridoContrato objPlanoAdquiridoContrato)
        {
            try
            {
                if (dr.Read())
                {
                    objPlanoAdquiridoContrato._idPlanoAdquiridoContrato = Convert.ToInt32(dr["Idf_Plano_Adquirido_Contrato"]);
                    objPlanoAdquiridoContrato._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
                    if (dr["Idf_Template_Contrato"] != DBNull.Value)
                        objPlanoAdquiridoContrato._templateContrato = new TemplateContrato(Convert.ToInt16(dr["Idf_Template_Contrato"]));
                    if (dr["Des_Contrato"] != DBNull.Value)
                        objPlanoAdquiridoContrato._descricaoContrato = Convert.ToString(dr["Des_Contrato"]);
                    objPlanoAdquiridoContrato._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                    objPlanoAdquiridoContrato._persisted = true;
                    objPlanoAdquiridoContrato._modified = false;

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