//-- Data: 20/10/2014 10:32
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PlanoAdquiridoDetalhes // Tabela: BNE_Plano_Adquirido_Detalhes
    {
        #region Atributos
        private int _idPlanoAdquiridoDetalhes;
        private PlanoAdquirido _planoAdquirido;
        private bool _flagNotaFiscal;
        private string _nomeResPlanoAdquirido;
        private Funcao _funcao;
        private string _numeroResDDDTelefone;
        private string _numeroResTelefone;
        private string _emailEnvioBoleto;
        private string _descricaoObservacao;
        private Vaga _vaga;
        private Filial _filialGestora;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPlanoAdquiridoDetalhes
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPlanoAdquiridoDetalhes
        {
            get
            {
                return this._idPlanoAdquiridoDetalhes;
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

        #region FlagNotaFiscal
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagNotaFiscal
        {
            get
            {
                return this._flagNotaFiscal;
            }
            set
            {
                this._flagNotaFiscal = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomeResPlanoAdquirido
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string NomeResPlanoAdquirido
        {
            get
            {
                return this._nomeResPlanoAdquirido;
            }
            set
            {
                this._nomeResPlanoAdquirido = value;
                this._modified = true;
            }
        }
        #endregion

        #region Funcao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Funcao Funcao
        {
            get
            {
                return this._funcao;
            }
            set
            {
                this._funcao = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroResDDDTelefone
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo obrigatório.
        /// </summary>
        public string NumeroResDDDTelefone
        {
            get
            {
                return this._numeroResDDDTelefone;
            }
            set
            {
                this._numeroResDDDTelefone = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroResTelefone
        /// <summary>
        /// Tamanho do campo: 10.
        /// Campo obrigatório.
        /// </summary>
        public string NumeroResTelefone
        {
            get
            {
                return this._numeroResTelefone;
            }
            set
            {
                this._numeroResTelefone = value;
                this._modified = true;
            }
        }
        #endregion

        #region EmailEnvioBoleto
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string EmailEnvioBoleto
        {
            get
            {
                return this._emailEnvioBoleto;
            }
            set
            {
                this._emailEnvioBoleto = value;
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

        #region Vaga
        /// <summary>
        /// Campo opcional.
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

        #region FilialGestora
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Filial FilialGestora
        {
            get
            {
                return this._filialGestora;
            }
            set
            {
                this._filialGestora = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public PlanoAdquiridoDetalhes()
        {
        }
        public PlanoAdquiridoDetalhes(int idPlanoAdquiridoDetalhes)
        {
            this._idPlanoAdquiridoDetalhes = idPlanoAdquiridoDetalhes;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Plano_Adquirido_Detalhes (Idf_Plano_Adquirido, Flg_Nota_Fiscal, Nme_Res_Plano_Adquirido, Idf_Funcao, Num_Res_DDD_Telefone, Num_Res_Telefone, Eml_Envio_Boleto, Des_Observacao, Idf_Vaga, Idf_Filial_Gestora) VALUES (@Idf_Plano_Adquirido, @Flg_Nota_Fiscal, @Nme_Res_Plano_Adquirido, @Idf_Funcao, @Num_Res_DDD_Telefone, @Num_Res_Telefone, @Eml_Envio_Boleto, @Des_Observacao, @Idf_Vaga, @Idf_Filial_Gestora);SET @Idf_Plano_Adquirido_Detalhes = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Plano_Adquirido_Detalhes SET Idf_Plano_Adquirido = @Idf_Plano_Adquirido, Flg_Nota_Fiscal = @Flg_Nota_Fiscal, Nme_Res_Plano_Adquirido = @Nme_Res_Plano_Adquirido, Idf_Funcao = @Idf_Funcao, Num_Res_DDD_Telefone = @Num_Res_DDD_Telefone, Num_Res_Telefone = @Num_Res_Telefone, Eml_Envio_Boleto = @Eml_Envio_Boleto, Des_Observacao = @Des_Observacao, Idf_Vaga = @Idf_Vaga, Idf_Filial_Gestora = @Idf_Filial_Gestora WHERE Idf_Plano_Adquirido_Detalhes = @Idf_Plano_Adquirido_Detalhes";
        private const string SPDELETE = "DELETE FROM BNE_Plano_Adquirido_Detalhes WHERE Idf_Plano_Adquirido_Detalhes = @Idf_Plano_Adquirido_Detalhes";
        private const string SPSELECTID = "SELECT * FROM BNE_Plano_Adquirido_Detalhes WITH(NOLOCK) WHERE Idf_Plano_Adquirido_Detalhes = @Idf_Plano_Adquirido_Detalhes";
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
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Detalhes", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Nota_Fiscal", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Nme_Res_Plano_Adquirido", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Res_DDD_Telefone", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Res_Telefone", SqlDbType.Char, 10));
            parms.Add(new SqlParameter("@Eml_Envio_Boleto", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Des_Observacao", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial_Gestora", SqlDbType.Int, 4));
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
            parms[0].Value = this._idPlanoAdquiridoDetalhes;
            parms[1].Value = this._planoAdquirido.IdPlanoAdquirido;
            parms[2].Value = this._flagNotaFiscal;
            parms[3].Value = this._nomeResPlanoAdquirido;

            if (this._funcao != null)
                parms[4].Value = this._funcao.IdFuncao;
            else
                parms[4].Value = DBNull.Value;

            parms[5].Value = this._numeroResDDDTelefone;
            parms[6].Value = this._numeroResTelefone;
            parms[7].Value = this._emailEnvioBoleto;

            if (!String.IsNullOrEmpty(this._descricaoObservacao))
                parms[8].Value = this._descricaoObservacao;
            else
                parms[8].Value = DBNull.Value;


            if (this._vaga != null)
                parms[9].Value = this._vaga.IdVaga;
            else
                parms[9].Value = DBNull.Value;


            if (this._filialGestora != null)
                parms[10].Value = this._filialGestora.IdFilial;
            else
                parms[10].Value = DBNull.Value;


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
        /// Método utilizado para inserir uma instância de PlanoAdquiridoDetalhes no banco de dados.
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
                        this._idPlanoAdquiridoDetalhes = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Adquirido_Detalhes"].Value);
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
        /// Método utilizado para inserir uma instância de PlanoAdquiridoDetalhes no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPlanoAdquiridoDetalhes = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Adquirido_Detalhes"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de PlanoAdquiridoDetalhes no banco de dados.
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
        /// Método utilizado para atualizar uma instância de PlanoAdquiridoDetalhes no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de PlanoAdquiridoDetalhes no banco de dados.
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
        /// Método utilizado para salvar uma instância de PlanoAdquiridoDetalhes no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de PlanoAdquiridoDetalhes no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idPlanoAdquiridoDetalhes)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Detalhes", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoDetalhes;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PlanoAdquiridoDetalhes no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idPlanoAdquiridoDetalhes, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Detalhes", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoDetalhes;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de PlanoAdquiridoDetalhes no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Lista de chaves.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(List<int> idPlanoAdquiridoDetalhes)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Plano_Adquirido_Detalhes where Idf_Plano_Adquirido_Detalhes in (";

            for (int i = 0; i < idPlanoAdquiridoDetalhes.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPlanoAdquiridoDetalhes[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idPlanoAdquiridoDetalhes)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Detalhes", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoDetalhes;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idPlanoAdquiridoDetalhes, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Detalhes", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoDetalhes;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Adquirido_Detalhes, Pla.Idf_Plano_Adquirido, Pla.Flg_Nota_Fiscal, Pla.Nme_Res_Plano_Adquirido, Pla.Idf_Funcao, Pla.Num_Res_DDD_Telefone, Pla.Num_Res_Telefone, Pla.Eml_Envio_Boleto, Pla.Des_Observacao, Pla.Idf_Vaga, Pla.Idf_Filial_Gestora FROM BNE_Plano_Adquirido_Detalhes Pla";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoAdquiridoDetalhes a partir do banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <returns>Instância de PlanoAdquiridoDetalhes.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static PlanoAdquiridoDetalhes LoadObject(int idPlanoAdquiridoDetalhes)
        {
            using (IDataReader dr = LoadDataReader(idPlanoAdquiridoDetalhes))
            {
                PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes = new PlanoAdquiridoDetalhes();
                if (SetInstance(dr, objPlanoAdquiridoDetalhes))
                    return objPlanoAdquiridoDetalhes;
            }
            throw (new RecordNotFoundException(typeof(PlanoAdquiridoDetalhes)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoAdquiridoDetalhes a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoDetalhes">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PlanoAdquiridoDetalhes.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static PlanoAdquiridoDetalhes LoadObject(int idPlanoAdquiridoDetalhes, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPlanoAdquiridoDetalhes, trans))
            {
                PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes = new PlanoAdquiridoDetalhes();
                if (SetInstance(dr, objPlanoAdquiridoDetalhes))
                    return objPlanoAdquiridoDetalhes;
            }
            throw (new RecordNotFoundException(typeof(PlanoAdquiridoDetalhes)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoAdquiridoDetalhes a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoAdquiridoDetalhes))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoAdquiridoDetalhes a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoAdquiridoDetalhes, trans))
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
        /// <param name="objPlanoAdquiridoDetalhes">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static bool SetInstance(IDataReader dr, PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes)
        {
            try
            {
                if (dr.Read())
                {
                    objPlanoAdquiridoDetalhes._idPlanoAdquiridoDetalhes = Convert.ToInt32(dr["Idf_Plano_Adquirido_Detalhes"]);
                    objPlanoAdquiridoDetalhes._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
                    objPlanoAdquiridoDetalhes._flagNotaFiscal = Convert.ToBoolean(dr["Flg_Nota_Fiscal"]);
                    objPlanoAdquiridoDetalhes._nomeResPlanoAdquirido = Convert.ToString(dr["Nme_Res_Plano_Adquirido"]);
                    if (dr["Idf_Funcao"] != DBNull.Value)
                        objPlanoAdquiridoDetalhes._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
                    objPlanoAdquiridoDetalhes._numeroResDDDTelefone = Convert.ToString(dr["Num_Res_DDD_Telefone"]);
                    objPlanoAdquiridoDetalhes._numeroResTelefone = Convert.ToString(dr["Num_Res_Telefone"]);
                    objPlanoAdquiridoDetalhes._emailEnvioBoleto = Convert.ToString(dr["Eml_Envio_Boleto"]);
                    if (dr["Des_Observacao"] != DBNull.Value)
                        objPlanoAdquiridoDetalhes._descricaoObservacao = Convert.ToString(dr["Des_Observacao"]);
                    if (dr["Idf_Vaga"] != DBNull.Value)
                        objPlanoAdquiridoDetalhes._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
                    if (dr["Idf_Filial_Gestora"] != DBNull.Value)
                        objPlanoAdquiridoDetalhes._filialGestora = new Filial(Convert.ToInt32(dr["Idf_Filial_Gestora"]));

                    objPlanoAdquiridoDetalhes._persisted = true;
                    objPlanoAdquiridoDetalhes._modified = false;

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