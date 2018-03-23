//-- Data: 08/02/2018 12:39
//-- Autor: Mailson

using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class LogEnvioCandidatura // Tabela: BNE_Log_Envio_Candidatura
    {
        #region [Consultas]
        private const string spAtualizaEtapaProcessamento = @"update BNE.BNE_Log_Envio_Candidatura 
                                    set Dta_Processamento_Candidatura_Envio = getdate()
                                    where Idf_Log_Envio_Candidatura = @Idf_Log_Envio_Candidatura
                                    ";
        private const string spAtualizaEtapaAnalise = @"update BNE.BNE_Log_Envio_Candidatura 
                                    set Dta_Analise_CV_Envio = getdate()
                                    where Idf_Log_Envio_Candidatura = @Idf_Log_Envio_Candidatura";

        private const string spAtualizaEtapaEnvio = @"update BNE.BNE_Log_Envio_Candidatura 
                                    set Dta_Envio_CV_Envio = getdate()
                                    where Idf_Log_Envio_Candidatura = @Idf_Log_Envio_Candidatura";

        #region [spVagaCandidatura]
        private const string spVagaCandidatura = @"select * from bne.bne_log_envio_candidatura with(nolock)
                        where Idf_Vaga_Candidato = @Idf_Vaga_Candidato";
        #endregion

        #region [spMontarEmail]
        private const string spMontarEmail = @"select pf.Nme_Pessoa, pf.Num_CPF, pf.Dta_Nascimento,
                             vag.Des_Atribuicoes, cv.Flg_VIP, f.raz_social,
                                        cid.Nme_Cidade,
			                            cid.Sig_Estado, funcao.Des_Funcao, pf.Eml_Pessoa  from bne.BNE_Log_Envio_Candidatura vc with(nolock)
                                    join bne.bne_curriculo cv with(nolock) on cv.idf_curriculo = vc.idf_curriculo
                                    join bne.tab_pessoa_Fisica pf with(nolock) on pf.idf_pessoa_Fisica = cv.Idf_Pessoa_Fisica
                                    join bne.bne_vaga vag with(nolock) on vag.idf_vaga = vc.idf_vaga
                                    join plataforma.tab_cidade cid with(nolock) on cid.Idf_Cidade = vag.Idf_Cidade 
		                            join bne.tab_filial f with(nolock) on f.idf_Filial = vag.idf_Filial
		                            join plataforma.tab_funcao funcao witH(nolocK) on funcao.idf_funcao = vag.idf_Funcao
                                where Idf_Vaga_Candidato = @Idf_vaga_Candidato";
        #endregion
        #endregion


        #region [AtualizarEnvioEmail]
        public static void AtualizarEnvioEmail(Enumeradores.CartaEmail carta, int idfLog)
        {
            var parametro = new List<SqlParameter>()
            {
                new SqlParameter{ParameterName = "@Idf_Log_Envio_Candidatura", SqlDbType = SqlDbType.Int, Value = idfLog}
            };

            switch (carta)
            {
                case Enumeradores.CartaEmail.CandEtapaProcessamento:
                    DataAccessLayer.ExecuteNonQuery(CommandType.Text, spAtualizaEtapaProcessamento, parametro);
                    break;
                case Enumeradores.CartaEmail.CandEtapaAnalise:
                    DataAccessLayer.ExecuteNonQuery(CommandType.Text, spAtualizaEtapaAnalise, parametro);
                    break;
                case Enumeradores.CartaEmail.CandEtapaEnvio:
                    DataAccessLayer.ExecuteNonQuery(CommandType.Text, spAtualizaEtapaEnvio, parametro);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region [AtulizarDatasEtapasCandidatura]
        /// <summary>
        /// Que não de merda depois que a tabela estiver muito cheia. #oremos
        /// </summary>
        /// <param name="Idf_Vaga_Candidatura"></param>
        public static void AtulizarDatasEtapasCandidatura(int Idf_Vaga_Candidatura, DateTime? DataVisualizacao)
        {
            List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter{ParameterName = "@Idf_Vaga_Candidato", SqlDbType = SqlDbType.Int, Value = Idf_Vaga_Candidatura}
            };
            var objLog = new LogEnvioCandidatura();
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spVagaCandidatura, parametros))
            {
                SetInstance(dr, objLog);

                #region [Envio Email de visualizacao]
                if (DataVisualizacao.HasValue && !objLog.DataEnvioCV.HasValue)
                {//Envia o e-mail de visualização
                    var email = string.Empty;
                    var objCarta = CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.CandCurriculoVisualizado);
                    var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ContaPadraoEnvioEmail);
                    var assunto = objCarta.Assunto;
                    var html = MontarEmail(objCarta.Conteudo, ref assunto, objLog.VagaCandidato.IdVagaCandidato, out  email);
                    //é trocado os passao em cima da hora ferra com toda a logica.
                    objLog.DataEnvioCV = DataVisualizacao.Value;
                    objLog.DataEnvioCVEnvio = DateTime.Now;
                 
                    if (!string.IsNullOrEmpty(email))
                    {
                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                                     .Enviar(assunto, html, BLL.Enumeradores.CartaEmail.CandCurriculoVisualizado, emailRemetente, email);
                     
                    }
                    if (objLog.DataProcessamentoCandidatura > DataVisualizacao.Value)
                    {
                        objLog.DataProcessamentoCandidatura = DataVisualizacao.Value;
                        objLog.DataAnaliseCV = DataVisualizacao.Value;
                    }
                    else if (objLog.DataAnaliseCV > DataVisualizacao.Value)
                        objLog.DataAnaliseCV = DataVisualizacao.Value;

                    objLog.Save();
                }
                #endregion

                

              

            }
        }
        #endregion

        #region RecuperarEmailPf
        /// <summary>
        /// Monta as cartae enviadas a cada etapa da candidatura
        /// </summary>
        /// <param name="html"></param>
        /// <param name="idCurriculo"></param>
        /// <param name="idVaga"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string MontarEmail(string html, ref string assunto, int idf_vaga_candidato, out string email)
        {
            email = string.Empty;
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga_Candidato", SqlDbType = SqlDbType.Int, Size = 4, Value = idf_vaga_candidato }

                };
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spMontarEmail, parms))
            {
                if (dr.Read())
                {
                    var link = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(Convert.ToDecimal(dr["Num_Cpf"]), Convert.ToDateTime(dr["Dta_Nascimento"]), "/sala-vip-ja-enviei");
                    link = $"{Helper.RecuperarURLAmbiente()}/logar/{link}";

                    html = html.Replace("{LinkJaEnviei}", link).Replace("{Funcao}", dr["Des_Funcao"].ToString())
                        .Replace("{NomeCompleto}", dr["Nme_Pessoa"].ToString())
                        .Replace("{Cidade}", Helper.FormatarCidade(dr["Nme_Cidade"].ToString(), dr["Sig_Estado"].ToString()))
                        .Replace("{Descricao}", dr["Des_Atribuicoes"].ToString())
                        .Replace("{Nome}", Helper.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString()))
                        .Replace("{NomeEmpresa}", Convert.ToBoolean(dr["Flg_Vip"]) ? dr["Raz_Social"].ToString() : "responsável pela vaga")
                        .Replace("{Funcao}", dr["Des_Funcao"].ToString());

                    assunto = assunto.Replace("{Funcao}", dr["Des_Funcao"].ToString());
                    email = dr["Eml_Pessoa"].ToString();
                }
            }

            return html;
        }
        #endregion
    }
}