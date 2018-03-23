using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class PerguntaSalaVipHistorico
    {

        #region Consultas

        #region SPINSERT
        private const string SPINSERT = "INSERT INTO BNE.BNE_Pergunta_Sala_Vip_Historico (Idf_Pergunta_Sala_Vip,Idf_Pessoa_Fisica,Dta_Exibicao,Dta_Resposta,Vlr_Resposta,Flg_Confirmado) VALUES (@Idf_Pergunta_Sala_Vip, @Idf_Pessoa_Fisica, @Dta_Exibicao,@Dta_Resposta,@Vlr_Resposta,@Flg_Confirmado);SET @Idf_Pergunta_Sala_Vip_Historico = SCOPE_IDENTITY();";
        #endregion

        #region SPSELECTPERGUNTASALAVIPPARAEXIBIRAOCANDIDATO
        /// <summary>
        /// Retornar a pergunta que será exibida ao candidato
        /// </summary>
        private const string SPSELECTPERGUNTASALAVIPPARAEXIBIRAOCANDIDATO = @"
        SELECT TOP 1
            pergunta.Des_Pergunta_Sala_Vip
	        FROM BNE.BNE_Pergunta_Sala_Vip_Historico AS PergHistorico  WITH (NOLOCK)
			    JOIN BNE.BNE_Pergunta_Sala_Vip AS pergunta WITH (NOLOCK) ON PergHistorico.Idf_Pergunta_Sala_Vip = pergunta.Idf_Pergunta_Sala_Vip
		            WHERE Idf_Pessoa_Fisica=@Idf_Pessoa_Fisica AND Dta_Resposta IS NULL
                        ORDER BY PergHistorico.Idf_Pergunta_Sala_Vip_Historico DESC, Dta_Exibicao DESC";

        #endregion

        #region SPSELECTIDPROXIMAPERGUNTASALAVIPPARAEXIBIRAOCANDIDATO
        /// <summary>
        /// Retornar o ID da próxima pergunta que será exibida ao candidato
        /// </summary>
        private const string SPSELECTIDPROXIMAPERGUNTASALAVIPPARAEXIBIRAOCANDIDATO = @"
        SELECT TOP 1
            pergunta.Idf_Pergunta_Sala_Vip
	        FROM BNE.BNE_Pergunta_Sala_Vip_Historico AS PergHistorico  WITH (NOLOCK)
			    JOIN BNE.BNE_Pergunta_Sala_Vip AS pergunta WITH (NOLOCK) ON PergHistorico.Idf_Pergunta_Sala_Vip = pergunta.Idf_Pergunta_Sala_Vip
		            WHERE Idf_Pessoa_Fisica=@Idf_Pessoa_Fisica AND Dta_Resposta IS NOT NULL
                        ORDER BY PergHistorico.Idf_Pergunta_Sala_Vip_Historico DESC, Dta_Exibicao DESC";

        #endregion

        private const string SPSELECTDATAULTIMAPERGUNTAHISTORICO = @"
        SELECT TOP 1
        Dta_Resposta
	        FROM BNE.BNE_Pergunta_Sala_Vip_Historico AS PergHistorico  WITH (NOLOCK)
		        WHERE Idf_Pessoa_Fisica=@Idf_Pessoa_Fisica AND Idf_Pergunta_Sala_Vip=@Idf_Pergunta_Sala_Vip
        ORDER BY Dta_Resposta DESC";

        #endregion


        #region Metodos

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idPerguntaSalaVipHistorico;
            parms[1].Value = this.IdPerguntaSalaVip;
            parms[2].Value = this._idPessoaFisica;
            parms[3].Value = this._dataExibicao;

            if (this._dataResposta.HasValue)
                parms[4].Value = this.DataResposta;
            else
                parms[4].Value = DBNull.Value;

            parms[5].Value = this._valorResposta;
            parms[6].Value = this._flg_Confirmado;

            if (!this._persisted)
            {
                this._dataExibicao = DateTime.Now;
            }
            parms[3].Value = this._dataExibicao;

            if (!_persisted)
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
        /// Método utilizado para inserir uma instância de PerguntaSalaVipHistorico no banco de dados.
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
                        _idPerguntaSalaVipHistorico = Convert.ToInt32(cmd.Parameters["@Idf_Pergunta_Sala_Vip_Historico"].Value);
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
        /// Método utilizado para inserir uma instância de PerguntaSalaVipHistorico no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            _idPerguntaSalaVipHistorico = Convert.ToInt32(cmd.Parameters["@Idf_Pergunta_Sala_Vip_Historico"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region CarregarPerguntaSalaVipExibirAoCandidato
        public static string CarregarPerguntaSalaVipExibirAoCandidato(int idPessoaFisica)
        {
            string retorno = string.Empty;
            var parms = new List<SqlParameter> { new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4) };
            parms[0].Value = idPessoaFisica;

            //retorno = Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTPERGUNTASALAVIPPARAEXIBIRAOCANDIDATO, parms));
            //return (retorno != "" ? retorno : "PerguntarSobreCelular");

            retorno = Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTIDPROXIMAPERGUNTASALAVIPPARAEXIBIRAOCANDIDATO, parms));
            return (retorno != "" ? retorno : "0");
            
        }
        #endregion

        #region CarregarPerguntaSalaVipExibirAoCandidato
        public static DateTime CarregarPerguntaHistoricoUltimaResposta(int idPessoaFisica, Enum tipoPergunta)
        {
            string retorno = "";
            var parms = new List<SqlParameter> { 
                new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4),
                new SqlParameter("@Idf_Pergunta_Sala_Vip", SqlDbType.Int, 4) 
            };

            parms[0].Value = idPessoaFisica;
            parms[1].Value = Convert.ToInt16(tipoPergunta);

            retorno = Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTDATAULTIMAPERGUNTAHISTORICO, parms));

            return (retorno != "" ? Convert.ToDateTime(retorno) : DateTime.MinValue);
        }
        #endregion

        #region SalvarHistoricoPerguntaExibicao

        /// <summary>
        /// Salvar o historico de exibição da pergunta
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <param name="tipoPergunta"></param>
        public static int SalvarHistoricoPerguntaExibicao(int idPessoaFisica, Enum tipoPergunta)
        {
            BLL.PerguntaSalaVipHistorico historico = new PerguntaSalaVipHistorico();

            historico.IdPerguntaSalaVip = Convert.ToInt16(tipoPergunta);
            historico.IdPessoaFisica = idPessoaFisica;
            historico.ValorResposta = "";
            historico.Flg_Confirmado = false;

            return historico.Save();
        }
        #endregion

        #region SalvarHistoricoPerguntaResposta

        /// <summary>
        /// Salvar a resposta da pergunta
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="flgConfirmado"></param>
        public static void SalvarHistoricoPerguntaResposta(int? IdPerguntaSalaVipHistorico, string valor, bool flgConfirmado)
        {
            if (IdPerguntaSalaVipHistorico.HasValue)
            {
                PerguntaSalaVipHistorico respostaHistorico = PerguntaSalaVipHistorico.LoadObject(IdPerguntaSalaVipHistorico.Value);

                respostaHistorico.DataResposta = DateTime.Now;
                respostaHistorico.Flg_Confirmado = flgConfirmado;
                respostaHistorico.ValorResposta = valor == null ? "": valor;

                respostaHistorico.Save();
            }
        }

        #endregion

        #endregion
    }
}
