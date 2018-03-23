//-- Data: 05/07/2012 11:45
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class EmailDestinatario // Tabela: BNE_Email_Destinatario
    {
        #region Atributos
        private int _idEmailDestinatario;
        private string _descricaoEmail;
        private DateTime _dataCadastro;
        private DateTime _dataAlteracao;
        private bool _flagInativo;
        private UsuarioFilialPerfil _usuarioGerador;
        private string _nomePessoa;
        private string _numeroDDDTelefone;
        private string _numeroTelefone;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdEmailDestinatario
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdEmailDestinatario
        {
            get
            {
                return this._idEmailDestinatario;
            }
        }
        #endregion

        #region DescricaoEmail
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoEmail
        {
            get
            {
                return this._descricaoEmail;
            }
            set
            {
                this._descricaoEmail = value;
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
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataAlteracao
        {
            get
            {
                return this._dataAlteracao;
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

        #region UsuarioGerador
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public UsuarioFilialPerfil UsuarioGerador
        {
            get
            {
                return this._usuarioGerador;
            }
        }
        #endregion

        #region NomePessoa
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo obrigatório.
        /// </summary>
        public string NomePessoa
        {
            get
            {
                return this._nomePessoa;
            }
            set
            {
                this._nomePessoa = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDDDTelefone
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string NumeroDDDTelefone
        {
            get
            {
                return this._numeroDDDTelefone;
            }
            set
            {
                this._numeroDDDTelefone = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroTelefone
        /// <summary>
        /// Tamanho do campo: 10.
        /// Campo opcional.
        /// </summary>
        public string NumeroTelefone
        {
            get
            {
                return this._numeroTelefone;
            }
            set
            {
                this._numeroTelefone = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public EmailDestinatario()
        {
        }
        public EmailDestinatario(int idEmailDestinatario)
        {
            this._idEmailDestinatario = idEmailDestinatario;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Email_Destinatario (Des_Email, Dta_Cadastro, Dta_Alteracao, Flg_Inativo, Idf_Usuario_Gerador, Nme_Pessoa, Num_DDD_Telefone, Num_Telefone) VALUES (@Des_Email, @Dta_Cadastro, @Dta_Alteracao, @Flg_Inativo, @Idf_Usuario_Gerador, @Nme_Pessoa, @Num_DDD_Telefone, @Num_Telefone);SET @Idf_Email_Destinatario = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Email_Destinatario SET Des_Email = @Des_Email, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Inativo = @Flg_Inativo, Idf_Usuario_Gerador = @Idf_Usuario_Gerador, Nme_Pessoa = @Nme_Pessoa, Num_DDD_Telefone = @Num_DDD_Telefone, Num_Telefone = @Num_Telefone WHERE Idf_Email_Destinatario = @Idf_Email_Destinatario";
        //private const string SPDELETE = "UPDATE BNE_Email_Destinatario SET Idf_Usuario_Gerador = @Idf_Usuario_Gerador WHERE Idf_Email_Destinatario = @Idf_Email_Destinatario ; DELETE FROM BNE_Email_Destinatario WHERE Idf_Email_Destinatario = @Idf_Email_Destinatario";
        private const string SPDELETE = "DELETE FROM BNE_Email_Destinatario WHERE Idf_Email_Destinatario = @Idf_Email_Destinatario";        
        private const string SPSELECTID = "SELECT * FROM BNE_Email_Destinatario WHERE Idf_Email_Destinatario = @Idf_Email_Destinatario";
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
            parms.Add(new SqlParameter("@Idf_Email_Destinatario", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Email", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Usuario_Gerador", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Num_DDD_Telefone", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Telefone", SqlDbType.Char, 10));
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
            parms[0].Value = this._idEmailDestinatario;
            parms[1].Value = this._descricaoEmail;
            parms[4].Value = this._flagInativo;
            parms[6].Value = this._nomePessoa;

            if (!String.IsNullOrEmpty(this._numeroDDDTelefone))
                parms[7].Value = this._numeroDDDTelefone;
            else
                parms[7].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroTelefone))
                parms[8].Value = this._numeroTelefone;
            else
                parms[8].Value = DBNull.Value;


            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[2].Value = this._dataCadastro;
            this._dataAlteracao = DateTime.Now;
            parms[3].Value = this._dataAlteracao;

            //Problemas gerador de classes
            if (this._usuarioGerador != null)
                parms[5].Value = this._usuarioGerador.IdUsuarioFilialPerfil;
            else
                parms[5].Value = DBNull.Value;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de EmailDestinatario no banco de dados.
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
                        this._idEmailDestinatario = Convert.ToInt32(cmd.Parameters["@Idf_Email_Destinatario"].Value);
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
        /// Método utilizado para inserir uma instância de EmailDestinatario no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idEmailDestinatario = Convert.ToInt32(cmd.Parameters["@Idf_Email_Destinatario"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de EmailDestinatario no banco de dados.
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
        /// Método utilizado para atualizar uma instância de EmailDestinatario no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de EmailDestinatario no banco de dados.
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
        /// Método utilizado para salvar uma instância de EmailDestinatario no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de EmailDestinatario no banco de dados.
        /// </summary>
        /// <param name="idEmailDestinatario">Chave do registro.</param>0
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idEmailDestinatario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Email_Destinatario", SqlDbType.Int, 4));

            parms[0].Value = idEmailDestinatario;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de EmailDestinatario no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idEmailDestinatario">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idEmailDestinatario, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Email_Destinatario", SqlDbType.Int, 4));

            parms[0].Value = idEmailDestinatario;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de EmailDestinatario no banco de dados.
        /// </summary>
        /// <param name="idEmailDestinatario">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idEmailDestinatario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Email_Destinatario where Idf_Email_Destinatario in (";

            for (int i = 0; i < idEmailDestinatario.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idEmailDestinatario[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idEmailDestinatario">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idEmailDestinatario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Email_Destinatario", SqlDbType.Int, 4));

            parms[0].Value = idEmailDestinatario;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idEmailDestinatario">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idEmailDestinatario, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Email_Destinatario", SqlDbType.Int, 4));

            parms[0].Value = idEmailDestinatario;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Ema.Idf_Email_Destinatario, Ema.Des_Email, Ema.Dta_Cadastro, Ema.Dta_Alteracao, Ema.Flg_Inativo, Ema.Idf_Usuario_Gerador, Ema.Nme_Pessoa, Ema.Num_DDD_Telefone, Ema.Num_Telefone FROM BNE_Email_Destinatario Ema";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de EmailDestinatario a partir do banco de dados.
        /// </summary>
        /// <param name="idEmailDestinatario">Chave do registro.</param>
        /// <returns>Instância de EmailDestinatario.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static EmailDestinatario LoadObject(int idEmailDestinatario)
        {
            using (IDataReader dr = LoadDataReader(idEmailDestinatario))
            {
                EmailDestinatario objEmailDestinatario = new EmailDestinatario();
                if (SetInstance(dr, objEmailDestinatario))
                    return objEmailDestinatario;
            }
            throw (new RecordNotFoundException(typeof(EmailDestinatario)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de EmailDestinatario a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idEmailDestinatario">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de EmailDestinatario.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static EmailDestinatario LoadObject(int idEmailDestinatario, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idEmailDestinatario, trans))
            {
                EmailDestinatario objEmailDestinatario = new EmailDestinatario();
                if (SetInstance(dr, objEmailDestinatario))
                    return objEmailDestinatario;
            }
            throw (new RecordNotFoundException(typeof(EmailDestinatario)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de EmailDestinatario a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idEmailDestinatario))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de EmailDestinatario a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idEmailDestinatario, trans))
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
        /// <param name="objEmailDestinatario">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, EmailDestinatario objEmailDestinatario)
        {
            try
            {
                if (dr.Read())
                {
                    objEmailDestinatario._idEmailDestinatario = Convert.ToInt32(dr["Idf_Email_Destinatario"]);
                    objEmailDestinatario._descricaoEmail = Convert.ToString(dr["Des_Email"]);
                    objEmailDestinatario._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objEmailDestinatario._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
                    objEmailDestinatario._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objEmailDestinatario._usuarioGerador = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Gerador"]));
                    objEmailDestinatario._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
                    if (dr["Num_DDD_Telefone"] != DBNull.Value)
                        objEmailDestinatario._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
                    if (dr["Num_Telefone"] != DBNull.Value)
                        objEmailDestinatario._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);

                    objEmailDestinatario._persisted = true;
                    objEmailDestinatario._modified = false;

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