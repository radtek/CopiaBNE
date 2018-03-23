//-- Data: 02/07/2015 10:38
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PlanoAdquiridoContratoUsuario // Tabela: BNE_Plano_Adquirido_Contrato_Usuario
    {
        #region Atributos
        private int _idPlanoAdquiridoContratoUsuario;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private DateTime _dataCadastro;
        private PlanoAdquiridoContrato _planoAdquiridoContrato;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPlanoAdquiridoContratoUsuario
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPlanoAdquiridoContratoUsuario
        {
            get
            {
                return this._idPlanoAdquiridoContratoUsuario;
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

        #region PlanoAdquiridoContrato
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PlanoAdquiridoContrato PlanoAdquiridoContrato
        {
            get
            {
                return this._planoAdquiridoContrato;
            }
            set
            {
                this._planoAdquiridoContrato = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public PlanoAdquiridoContratoUsuario()
        {
        }
        public PlanoAdquiridoContratoUsuario(int idPlanoAdquiridoContratoUsuario)
        {
            this._idPlanoAdquiridoContratoUsuario = idPlanoAdquiridoContratoUsuario;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Plano_Adquirido_Contrato_Usuario (Idf_Usuario_Filial_Perfil, Dta_Cadastro, Idf_Plano_Adquirido_Contrato) VALUES (@Idf_Usuario_Filial_Perfil, @Dta_Cadastro, @Idf_Plano_Adquirido_Contrato);SET @Idf_Plano_Adquirido_Contrato_Usuario = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Plano_Adquirido_Contrato_Usuario SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Dta_Cadastro = @Dta_Cadastro, Idf_Plano_Adquirido_Contrato = @Idf_Plano_Adquirido_Contrato WHERE Idf_Plano_Adquirido_Contrato_Usuario = @Idf_Plano_Adquirido_Contrato_Usuario";
        private const string SPDELETE = "DELETE FROM BNE_Plano_Adquirido_Contrato_Usuario WHERE Idf_Plano_Adquirido_Contrato_Usuario = @Idf_Plano_Adquirido_Contrato_Usuario";
        private const string SPSELECTID = "SELECT * FROM BNE_Plano_Adquirido_Contrato_Usuario WITH(NOLOCK) WHERE Idf_Plano_Adquirido_Contrato_Usuario = @Idf_Plano_Adquirido_Contrato_Usuario";
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
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato_Usuario", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato", SqlDbType.Int, 4));
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
            parms[0].Value = this._idPlanoAdquiridoContratoUsuario;
            parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            parms[3].Value = this._planoAdquiridoContrato.IdPlanoAdquiridoContrato;

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
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de PlanoAdquiridoContratoUsuario no banco de dados.
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
                        this._idPlanoAdquiridoContratoUsuario = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Adquirido_Contrato_Usuario"].Value);
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
        /// Método utilizado para inserir uma instância de PlanoAdquiridoContratoUsuario no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPlanoAdquiridoContratoUsuario = Convert.ToInt32(cmd.Parameters["@Idf_Plano_Adquirido_Contrato_Usuario"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de PlanoAdquiridoContratoUsuario no banco de dados.
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
        /// Método utilizado para atualizar uma instância de PlanoAdquiridoContratoUsuario no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de PlanoAdquiridoContratoUsuario no banco de dados.
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
        /// Método utilizado para salvar uma instância de PlanoAdquiridoContratoUsuario no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de PlanoAdquiridoContratoUsuario no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoContratoUsuario">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlanoAdquiridoContratoUsuario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato_Usuario", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoContratoUsuario;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PlanoAdquiridoContratoUsuario no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoContratoUsuario">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPlanoAdquiridoContratoUsuario, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato_Usuario", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoContratoUsuario;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de PlanoAdquiridoContratoUsuario no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoContratoUsuario">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idPlanoAdquiridoContratoUsuario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Plano_Adquirido_Contrato_Usuario where Idf_Plano_Adquirido_Contrato_Usuario in (";

            for (int i = 0; i < idPlanoAdquiridoContratoUsuario.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPlanoAdquiridoContratoUsuario[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoContratoUsuario">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlanoAdquiridoContratoUsuario)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato_Usuario", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoContratoUsuario;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoContratoUsuario">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPlanoAdquiridoContratoUsuario, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido_Contrato_Usuario", SqlDbType.Int, 4));

            parms[0].Value = idPlanoAdquiridoContratoUsuario;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pla.Idf_Plano_Adquirido_Contrato_Usuario, Pla.Idf_Usuario_Filial_Perfil, Pla.Dta_Cadastro, Pla.Idf_Plano_Adquirido_Contrato FROM BNE_Plano_Adquirido_Contrato_Usuario Pla";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoAdquiridoContratoUsuario a partir do banco de dados.
        /// </summary>
        /// <param name="idPlanoAdquiridoContratoUsuario">Chave do registro.</param>
        /// <returns>Instância de PlanoAdquiridoContratoUsuario.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PlanoAdquiridoContratoUsuario LoadObject(int idPlanoAdquiridoContratoUsuario)
        {
            using (IDataReader dr = LoadDataReader(idPlanoAdquiridoContratoUsuario))
            {
                PlanoAdquiridoContratoUsuario objPlanoAdquiridoContratoUsuario = new PlanoAdquiridoContratoUsuario();
                if (SetInstance(dr, objPlanoAdquiridoContratoUsuario))
                    return objPlanoAdquiridoContratoUsuario;
            }
            throw (new RecordNotFoundException(typeof(PlanoAdquiridoContratoUsuario)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PlanoAdquiridoContratoUsuario a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPlanoAdquiridoContratoUsuario">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PlanoAdquiridoContratoUsuario.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PlanoAdquiridoContratoUsuario LoadObject(int idPlanoAdquiridoContratoUsuario, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPlanoAdquiridoContratoUsuario, trans))
            {
                PlanoAdquiridoContratoUsuario objPlanoAdquiridoContratoUsuario = new PlanoAdquiridoContratoUsuario();
                if (SetInstance(dr, objPlanoAdquiridoContratoUsuario))
                    return objPlanoAdquiridoContratoUsuario;
            }
            throw (new RecordNotFoundException(typeof(PlanoAdquiridoContratoUsuario)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoAdquiridoContratoUsuario a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoAdquiridoContratoUsuario))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PlanoAdquiridoContratoUsuario a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPlanoAdquiridoContratoUsuario, trans))
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
        /// <param name="objPlanoAdquiridoContratoUsuario">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, PlanoAdquiridoContratoUsuario objPlanoAdquiridoContratoUsuario)
        {
            try
            {
                if (dr.Read())
                {
                    objPlanoAdquiridoContratoUsuario._idPlanoAdquiridoContratoUsuario = Convert.ToInt32(dr["Idf_Plano_Adquirido_Contrato_Usuario"]);
                    objPlanoAdquiridoContratoUsuario._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    objPlanoAdquiridoContratoUsuario._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objPlanoAdquiridoContratoUsuario._planoAdquiridoContrato = new PlanoAdquiridoContrato(Convert.ToInt32(dr["Idf_Plano_Adquirido_Contrato"]));

                    objPlanoAdquiridoContratoUsuario._persisted = true;
                    objPlanoAdquiridoContratoUsuario._modified = false;

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