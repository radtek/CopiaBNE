//-- Data: 06/06/2014 16:39
//-- Autor: Lennon Vidal

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class ConversasAtivas // Tabela: BNE_Conversas_Ativas
    {
        #region Atributos
        private int _idConversaAtiva;
        private Curriculo _curriculo;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private bool _flagMensagemPendente;
        private bool? _flagArmazenado;
        private DateTime? _dataUltimaAtualizacao;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdConversaAtiva
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdConversaAtiva
        {
            get
            {
                return this._idConversaAtiva;
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

        #region FlagMensagemPendente
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagMensagemPendente
        {
            get
            {
                return this._flagMensagemPendente;
            }
            set
            {
                this._flagMensagemPendente = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagArmazenado
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagArmazenado
        {
            get
            {
                return this._flagArmazenado;
            }
            set
            {
                this._flagArmazenado = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataUltimaAtualizacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataUltimaAtualizacao
        {
            get
            {
                return this._dataUltimaAtualizacao;
            }
            set
            {
                this._dataUltimaAtualizacao = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public ConversasAtivas()
        {
        }
        public ConversasAtivas(int idConversaAtiva)
        {
            this._idConversaAtiva = idConversaAtiva;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Conversas_Ativas (Idf_Curriculo, Idf_Usuario_Filial_Perfil, Flg_Mensagem_Pendente, Flg_Armazenado, Dta_Ultima_Atualizacao) VALUES (@Idf_Curriculo, @Idf_Usuario_Filial_Perfil, @Flg_Mensagem_Pendente, @Flg_Armazenado, @Dta_Ultima_Atualizacao);SET @Idf_Conversa_Ativa = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Conversas_Ativas SET Idf_Curriculo = @Idf_Curriculo, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Flg_Mensagem_Pendente = @Flg_Mensagem_Pendente, Flg_Armazenado = @Flg_Armazenado, Dta_Ultima_Atualizacao = @Dta_Ultima_Atualizacao WHERE Idf_Conversa_Ativa = @Idf_Conversa_Ativa";
        private const string SPDELETE = "DELETE FROM BNE_Conversas_Ativas WHERE Idf_Conversa_Ativa = @Idf_Conversa_Ativa";
        private const string SPSELECTID = "SELECT * FROM BNE_Conversas_Ativas WITH(NOLOCK) WHERE Idf_Conversa_Ativa = @Idf_Conversa_Ativa";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Conversa_Ativa", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Mensagem_Pendente", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Armazenado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Dta_Ultima_Atualizacao", SqlDbType.DateTime, 8));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Lennon Vidal</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idConversaAtiva;
            parms[1].Value = this._curriculo.IdCurriculo;
            parms[2].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            parms[3].Value = this._flagMensagemPendente;

            if (this._flagArmazenado.HasValue)
                parms[4].Value = this._flagArmazenado;
            else
                parms[4].Value = DBNull.Value;


            if (this._dataUltimaAtualizacao.HasValue)
                parms[5].Value = this._dataUltimaAtualizacao;
            else
                parms[5].Value = DBNull.Value;


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
        /// Método utilizado para inserir uma instância de ConversasAtivas no banco de dados.
        /// </summary>
        /// <remarks>Lennon Vidal</remarks>
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
                        this._idConversaAtiva = Convert.ToInt32(cmd.Parameters["@Idf_Conversa_Ativa"].Value);
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
        /// Método utilizado para inserir uma instância de ConversasAtivas no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Lennon Vidal</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idConversaAtiva = Convert.ToInt32(cmd.Parameters["@Idf_Conversa_Ativa"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de ConversasAtivas no banco de dados.
        /// </summary>
        /// <remarks>Lennon Vidal</remarks>
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
        /// Método utilizado para atualizar uma instância de ConversasAtivas no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Lennon Vidal</remarks>
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
        /// Método utilizado para salvar uma instância de ConversasAtivas no banco de dados.
        /// </summary>
        /// <remarks>Lennon Vidal</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de ConversasAtivas no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Lennon Vidal</remarks>
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
        /// Método utilizado para excluir uma instância de ConversasAtivas no banco de dados.
        /// </summary>
        /// <param name="idConversaAtiva">Chave do registro.</param>
        /// <remarks>Lennon Vidal</remarks>
        public static void Delete(int idConversaAtiva)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Conversa_Ativa", SqlDbType.Int, 4));

            parms[0].Value = idConversaAtiva;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de ConversasAtivas no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idConversaAtiva">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Lennon Vidal</remarks>
        public static void Delete(int idConversaAtiva, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Conversa_Ativa", SqlDbType.Int, 4));

            parms[0].Value = idConversaAtiva;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de ConversasAtivas no banco de dados.
        /// </summary>
        /// <param name="idConversaAtiva">Lista de chaves.</param>
        /// <remarks>Lennon Vidal</remarks>
        public static void Delete(List<int> idConversaAtiva)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Conversas_Ativas where Idf_Conversa_Ativa in (";

            for (int i = 0; i < idConversaAtiva.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idConversaAtiva[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idConversaAtiva">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private static IDataReader LoadDataReader(int idConversaAtiva)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Conversa_Ativa", SqlDbType.Int, 4));

            parms[0].Value = idConversaAtiva;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idConversaAtiva">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private static IDataReader LoadDataReader(int idConversaAtiva, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Conversa_Ativa", SqlDbType.Int, 4));

            parms[0].Value = idConversaAtiva;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Con.Idf_Conversa_Ativa, Con.Idf_Curriculo, Con.Idf_Usuario_Filial_Perfil, Con.Flg_Mensagem_Pendente, Con.Flg_Armazenado, Con.Dta_Ultima_Atualizacao FROM BNE_Conversas_Ativas Con";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de ConversasAtivas a partir do banco de dados.
        /// </summary>
        /// <param name="idConversaAtiva">Chave do registro.</param>
        /// <returns>Instância de ConversasAtivas.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public static ConversasAtivas LoadObject(int idConversaAtiva)
        {
            using (IDataReader dr = LoadDataReader(idConversaAtiva))
            {
                ConversasAtivas objConversasAtivas = new ConversasAtivas();
                if (SetInstance(dr, objConversasAtivas))
                    return objConversasAtivas;
            }
            throw (new RecordNotFoundException(typeof(ConversasAtivas)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de ConversasAtivas a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idConversaAtiva">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de ConversasAtivas.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public static ConversasAtivas LoadObject(int idConversaAtiva, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idConversaAtiva, trans))
            {
                ConversasAtivas objConversasAtivas = new ConversasAtivas();
                if (SetInstance(dr, objConversasAtivas))
                    return objConversasAtivas;
            }
            throw (new RecordNotFoundException(typeof(ConversasAtivas)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de ConversasAtivas a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idConversaAtiva))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de ConversasAtivas a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idConversaAtiva, trans))
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
        /// <param name="objConversasAtivas">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Lennon Vidal</remarks>
        private static bool SetInstance(IDataReader dr, ConversasAtivas objConversasAtivas)
        {
            try
            {
                if (dr.Read())
                {
                    objConversasAtivas._idConversaAtiva = Convert.ToInt32(dr["Idf_Conversa_Ativa"]);
                    objConversasAtivas._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    objConversasAtivas._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    objConversasAtivas._flagMensagemPendente = Convert.ToBoolean(dr["Flg_Mensagem_Pendente"]);
                    if (dr["Flg_Armazenado"] != DBNull.Value)
                        objConversasAtivas._flagArmazenado = Convert.ToBoolean(dr["Flg_Armazenado"]);
                    if (dr["Dta_Ultima_Atualizacao"] != DBNull.Value)
                        objConversasAtivas._dataUltimaAtualizacao = Convert.ToDateTime(dr["Dta_Ultima_Atualizacao"]);

                    objConversasAtivas._persisted = true;
                    objConversasAtivas._modified = false;

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