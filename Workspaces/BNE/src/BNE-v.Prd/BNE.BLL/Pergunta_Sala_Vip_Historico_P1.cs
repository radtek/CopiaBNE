//-- Data: 25/08/2014 15:47
//-- Autor: Fabiano Charan

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PerguntaSalaVipHistorico // Tabela: BNE.Pergunta_Sala_Vip_Historico
    {
        #region Atributos
        private int _idPerguntaSalaVipHistorico;
        private int _idPerguntaSalaVip;
        private int _idPessoaFisica;
        private string _valorResposta;
        private DateTime _dataExibicao;
        private DateTime? _dataResposta;
        private bool _flg_Confirmado;


        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPerguntaSalaVipHistorico
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int IdPerguntaSalaVipHistorico
        {
            get
            {
                return this._idPerguntaSalaVipHistorico;
            }
        }
        #endregion

        #region IdPerguntaSalaVip
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int IdPerguntaSalaVip
        {
            get
            {
                return this._idPerguntaSalaVip;
            }
            set
            {
                this._idPerguntaSalaVip = value;
                this._modified = true;
            }
        }
        #endregion

        #region IdPessoaFisica
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int IdPessoaFisica
        {
            get
            {
                return this._idPessoaFisica;
            }
            set
            {
                this._idPessoaFisica = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorResposta
        /// <summary>
        /// Tamanho do campo: 2000.
        /// </summary>
        public string ValorResposta
        {
            get
            {
                return this._valorResposta;
            }
            set
            {
                this._valorResposta = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataExibicao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataExibicao
        {
            get
            {
                return this._dataExibicao;
            }
        }
        #endregion

        #region DataResposta
        /// <summary>
        /// Campo NÃO obrigatório.
        /// </summary>
        public DateTime? DataResposta
        {
            get
            {
                return this._dataResposta;
            }
            set {
               this._dataResposta = value;
            }
        }
        #endregion

        #region Flg_Confirmado

        public bool Flg_Confirmado
        {
            get { return this._flg_Confirmado; }
            set { this._flg_Confirmado = value; }
        }

        #endregion

        #endregion

        #region Construtores
        public PerguntaSalaVipHistorico()
        {
        }
        public PerguntaSalaVipHistorico(int idPerguntaSalaVipHistorico)
        {
            this._idPerguntaSalaVipHistorico = idPerguntaSalaVipHistorico;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPUPDATE = "UPDATE BNE.BNE_Pergunta_Sala_Vip_Historico SET Vlr_Resposta = @Vlr_Resposta, Dta_Resposta = @Dta_Resposta, Flg_Confirmado = @Flg_Confirmado WHERE Idf_Pergunta_Sala_Vip_Historico = @Idf_Pergunta_Sala_Vip_Historico";
        private const string SPDELETE = "DELETE FROM BNE.BNE_Pergunta_Sala_Vip_Historico WHERE Idf_Pergunta_Sala_Vip_Historico = @Idf_Pergunta_Sala_Vip_Historico";
        private const string SPSELECTID = "SELECT * FROM BNE.BNE_Pergunta_Sala_Vip_Historico WHERE Idf_Pergunta_Sala_Vip_Historico = @Idf_Pergunta_Sala_Vip_Historico";
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
            parms.Add(new SqlParameter("@Idf_Pergunta_Sala_Vip_Historico", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Pergunta_Sala_Vip", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Exibicao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Resposta", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Vlr_Resposta", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Flg_Confirmado", SqlDbType.Bit));


            return (parms);
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de PerguntaSalaVipHistorico no banco de dados.
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
        /// Método utilizado para atualizar uma instância de PerguntaSalaVipHistorico no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de PerguntaSalaVipHistorico no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        public int Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();

            return this._idPerguntaSalaVipHistorico;
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de PerguntaSalaVipHistorico no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de PerguntaSalaVipHistorico no banco de dados.
        /// </summary>
        /// <param name="idTipoContato">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idTipoContato)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pergunta_Sala_Vip_Historico", SqlDbType.Int, 4));

            parms[0].Value = idTipoContato;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PerguntaSalaVipHistorico no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idTipoContato">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idTipoContato, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pergunta_Sala_Vip_Historico", SqlDbType.Int, 4));

            parms[0].Value = idTipoContato;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de PerguntaSalaVipHistorico no banco de dados.
        /// </summary>
        /// <param name="idPerguntaSalaVipHistorico">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idPerguntaSalaVipHistorico)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from plataforma.BNE_Pergunta_Sala_Vip_Historico where Idf_Pergunta_Sala_Vip_Historico in (";

            for (int i = 0; i < idPerguntaSalaVipHistorico.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPerguntaSalaVipHistorico[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPerguntaSalaVipHistorico">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPerguntaSalaVipHistorico)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pergunta_Sala_Vip_Historico", SqlDbType.Int, 4));

            parms[0].Value = idPerguntaSalaVipHistorico;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPerguntaSalaVipHistorico">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPerguntaSalaVipHistorico, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pergunta_Sala_Vip_Historico", SqlDbType.Int, 4));

            parms[0].Value = idPerguntaSalaVipHistorico;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Tip.Idf_Pergunta_Sala_Vip_Historico,Tip.Idf_Pergunta_Sala_Vip,Tip.Idf_PessoaFisica, Tip.Vlr_Resposta, Tip.Dta_Resposta, Tip.Dta_Exibicao,Tipo.Flg_Confirmado FROM plataforma.BNE_Pergunta_Sala_Vip_Historico Tip";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PerguntaSalaVipHistorico a partir do banco de dados.
        /// </summary>
        /// <param name="idTipoContato">Chave do registro.</param>
        /// <returns>Instância de PerguntaSalaVipHistorico.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PerguntaSalaVipHistorico LoadObject(int idPerguntaSalaVipHistorico)
        {
            using (IDataReader dr = LoadDataReader(idPerguntaSalaVipHistorico))
            {
                PerguntaSalaVipHistorico objPerguntaSalaVipHistorico = new PerguntaSalaVipHistorico();
                if (SetInstance(dr, objPerguntaSalaVipHistorico))
                    return objPerguntaSalaVipHistorico;
            }
            throw (new RecordNotFoundException(typeof(PerguntaSalaVipHistorico)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PerguntaSalaVipHistorico a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idTipoContato">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PerguntaSalaVipHistorico.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PerguntaSalaVipHistorico LoadObject(int idPerguntaSalaVipHistorico, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPerguntaSalaVipHistorico, trans))
            {
                PerguntaSalaVipHistorico objPerguntaSalaVipHistorico = new PerguntaSalaVipHistorico();
                if (SetInstance(dr, objPerguntaSalaVipHistorico))
                    return objPerguntaSalaVipHistorico;
            }
            throw (new RecordNotFoundException(typeof(PerguntaSalaVipHistorico)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PerguntaSalaVipHistorico a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPerguntaSalaVipHistorico))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PerguntaSalaVipHistorico a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPerguntaSalaVipHistorico, trans))
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
        /// <param name="objPerguntaSalaVipHistorico">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, PerguntaSalaVipHistorico objPerguntaSalaVipHistorico)
        {
            try
            {
                if (dr.Read())
                {
                    objPerguntaSalaVipHistorico._idPerguntaSalaVipHistorico = Convert.ToInt32(dr["Idf_Pergunta_Sala_Vip_Historico"]);
                    objPerguntaSalaVipHistorico._idPerguntaSalaVip = Convert.ToInt32(dr["Idf_Pergunta_Sala_Vip"]);
                    objPerguntaSalaVipHistorico._idPessoaFisica = Convert.ToInt32(dr["Idf_Pessoa_Fisica"]);
                    objPerguntaSalaVipHistorico._dataExibicao = Convert.ToDateTime(dr["Dta_Exibicao"]);
                    if (dr["Dta_Resposta"] != DBNull.Value)
                    objPerguntaSalaVipHistorico._dataResposta = Convert.ToDateTime(dr["Dta_Resposta"]);
                    objPerguntaSalaVipHistorico.ValorResposta = Convert.ToString(dr["Vlr_Resposta"]);
                    objPerguntaSalaVipHistorico.Flg_Confirmado = Convert.ToBoolean(dr["Flg_Confirmado"]);


                    objPerguntaSalaVipHistorico._persisted = true;
                    objPerguntaSalaVipHistorico._modified = false;

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