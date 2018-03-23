//-- Data: 28/02/2013 11:38
//-- Autor: Gieyson Stelmak

using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class TransacaoResposta // Tabela: BNE_Transacao_Resposta
    {
        #region [Consultas]

        #region [spAlertaTentativaCompra]
        private const string spAlertaTentativaCompraEmpresa = @"select tr.dta_cadastro, tr.des_resultado_solicitacao_aprovacao, tr.des_codigo_autorizacao, t.Vlr_Documento
, pf.Nme_Pessoa,  f.num_cnpj, f.raz_social, f.num_ddd_comercial, f.num_comercial, ps.des_plano_situacao,
 vendedor.nme_vendedor, plano.des_plano, recorrencia.Parcela_Paga
 from bne.bne_transacao_resposta  tr with(nolock)
join bne.bne_transacao t with(nolock) on t.idf_transacao = tr.idf_transacao
join bne.bne_plano_adquirido pa with(nolock) on pa.idf_plano_adquirido = t.idf_plano_adquirido
join bne.bne_plano_situacao ps with(nolock) on ps.idf_plano_situacao = pa.idf_plano_situacao
join bne.tab_usuario_filial_perfil ufp with(nolock) on ufp.idf_usuario_Filial_perfil = pa.idf_usuario_filial_perfil
join bne.tab_pessoa_Fisica pf with(Nolock) on pf.idf_pessoa_Fisica = ufp.idf_pessoa_Fisica
join bne.tab_filial f with(nolock) on f.idf_Filial = ufp.idf_Filial
join bne.bne_plano plano with(nolock) on plano.idf_plano = pa.idf_plano
outer apply (SELECT TOP 1 v.Nme_Vendedor
  FROM DW_CRM2012.dbo.CRM_Vendedor_Empresa ve WITH (NOLOCK)
       JOIN DW_CRM2012.dbo.CRM_Vendedor v WITH (NOLOCK) ON ve.Num_CPF = v.Num_CPF
	   JOIN BNE.TAB_Pessoa_Fisica pf WITH (NOLOCK) ON v.Num_CPF = pf.Num_CPF
	   LEFT JOIN BNE.TAB_Usuario_Filial_Perfil ufp WITH (NOLOCK) ON pf.Idf_Pessoa_Fisica = ufp.Idf_Pessoa_Fisica 
	   LEFT JOIN BNE.BNE_Usuario_Filial usuf WITH (NOLOCK) ON ufp.Idf_Usuario_Filial_Perfil = usuf.Idf_Usuario_Filial_Perfil
 WHERE GETDATE() BETWEEN ve.Dta_Inicio AND CONVERT(DATE,DATEADD(DAY,1,ve.Dta_Fim))
   AND Num_CNPJ = f.Num_Cnpj) as Vendedor
outer apply ( select count(idf_transacao) as Parcela_Paga from bne.bne_transacao with(nolock) 
   where idf_plano_adquirido = pa.idf_plano_adquirido
    and idf_status_transacao =  3 --Transação Cartão Capturada
	and dta_cadastro < t.dta_cadastro - 27
	) as recorrencia
where tr.flg_transacao_aprovada = 0 
and tr.des_resultado_solicitacao_aprovacao not like 'Refused Cod:%'
and tr.dta_cadastro between @Dta_inicio and @Dta_Fim
order by tr.dta_cadastro asc
";


        #endregion

        #region [spAlertaTentativaCompraPF]
        private const string spAlertaTentativaCompraPF = @"
                            select t.Idf_Transacao, tr.dta_cadastro, tr.des_resultado_solicitacao_aprovacao, tr.des_codigo_autorizacao, t.Vlr_Documento
                            , pf.Nme_Pessoa, ps.des_plano_situacao, plano.des_plano,recorrencia.Parcela_Paga
                             from bne.bne_transacao_resposta  tr with(nolock)
                            join bne.bne_transacao t with(nolock) on t.idf_transacao = tr.idf_transacao
                            join bne.bne_plano_adquirido pa with(nolock) on pa.idf_plano_adquirido = t.idf_plano_adquirido
                            join bne.bne_plano_situacao ps with(nolock) on ps.idf_plano_situacao = pa.idf_plano_situacao
                            join bne.tab_usuario_filial_perfil ufp with(nolock) on ufp.idf_usuario_Filial_perfil = pa.idf_usuario_filial_perfil
                            join bne.tab_pessoa_Fisica pf with(Nolock) on pf.idf_pessoa_Fisica = ufp.idf_pessoa_Fisica
                            join bne.bne_plano plano with(nolock) on plano.idf_plano = pa.idf_plano
                            outer apply ( select count(idf_transacao) as Parcela_Paga from bne.bne_transacao  with(nolock) 
                               where idf_plano_adquirido = pa.idf_plano_adquirido
                                and idf_status_transacao =  3 --Transação Cartão Capturada
	                            and dta_cadastro < t.dta_cadastro - 27
	                            ) as recorrencia
                            where tr.flg_transacao_aprovada = 0  and pa.idf_Filial is null
                            and tr.des_resultado_solicitacao_aprovacao not like 'Refused Cod:%'
                            and tr.dta_cadastro between @Dta_inicio and @Dta_Fim
                            order by tr.dta_cadastro asc
                            ";


        #endregion

        #endregion
        #region SalvarResposta
        public static void SalvarResposta(Transacao objTransacao, bool aprovada, string descricaoResultado, string codigoAutorizacao, string descricaoTransacao, string cartaoMascarado, decimal? numeroSequencial, string comprovanteAdministradora, string nacionalidadeEmissor)
        {
            try
            {
                var objTransacaoResposta = new TransacaoResposta
                {
                    Transacao = objTransacao,
                    FlagTransacaoAprovada = aprovada,
                    DescricaoResultadoSolicitacaoAprovacao = descricaoResultado,
                    DescricaoCodigoAutorizacao = codigoAutorizacao,
                    DescricaoTransacao = descricaoTransacao,
                    DescricaoCartaoMascarado = cartaoMascarado,
                    NumeroSequencialUnico = numeroSequencial,
                    DescricaoComprovanteAdministradora = comprovanteAdministradora,
                    DescricaoNacionalidadeEmissor = nacionalidadeEmissor
                };

                objTransacaoResposta.Save();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region [AlertaTentativaCompra]
        /// <summary>
        /// Usado em robo para enviar carta com as tentativas de compra do dia anterior de PJ e PF
        /// </summary>
        public static void AlertaTentativaCompra()
        {
            List<SqlParameter> parametro = new List<SqlParameter>()
            {
                new SqlParameter{ ParameterName = "@Dta_inicio", SqlDbType = SqlDbType.DateTime, Value  = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(-1).Day,0,0,0)},
                new SqlParameter{ ParameterName = "@Dta_Fim",  SqlDbType = SqlDbType.DateTime, Value =new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(-1).Day,23,59,59)}
            };

            var carta = CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.AlertaTentativaCompra);
            string EmailDestino = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailDestinatarioAlertaTentativaCompraPJ);


            string linhas = "<tr><td>{Dta_Tentativa}</td><td>{Motivo}</td><td>{CNPJ}</td><td>{Razao_Social}</td><td>{Telefone}</td><td>{Vendedor}</td><td>{Plano_}</td><td>{Valor}</td><td>{Plano_Situacao}</td></tr>";
            string linhaCompra =  string.Empty;
            string linhaRecorrencia = string.Empty;
            int totalCompra = 0;
            int totalRecorrencia = 0;

            #region [Pessoa Juridica]

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spAlertaTentativaCompraEmpresa, parametro))
            {

                while (dr.Read())
                {
                    try
                    {
                        if(Convert.ToInt32(dr["Parcela_Paga"]) > 0) { //tentativa de cobrança recorrencia
                        var ParametrosCarta = new
                        {
                            Dta_Tentativa = Convert.ToDateTime(dr["dta_cadastro"]).ToString("dd/MM/yyyy HH:mm"),
                            Motivo = dr["des_resultado_solicitacao_aprovacao"].ToString(),
                            CNPJ = dr["num_cnpj"].ToString(),
                            Valor = dr["Vlr_Documento"].ToString(),
                            Razao_Social = dr["raz_social"].ToString(),
                            Telefone = Helper.FormatarTelefone(dr["num_ddd_comercial"].ToString(), dr["num_comercial"].ToString()),
                            Vendedor = dr["nme_vendedor"].ToString(),
                            Plano_ = dr["des_plano"].ToString(),
                            Plano_Situacao = dr["des_plano_situacao"].ToString()
                        };

                        linhaRecorrencia += FormatObject.ToString(ParametrosCarta, linhas);
                            totalRecorrencia++;
                        }
                        else // tentativa de compra
                        {
                            var ParametrosCarta = new
                            {
                                Dta_Tentativa = Convert.ToDateTime(dr["dta_cadastro"]).ToString("dd/MM/yyyy HH:mm"),
                                Motivo = dr["des_resultado_solicitacao_aprovacao"].ToString(),
                                CNPJ = dr["num_cnpj"].ToString(),
                                Valor = dr["Vlr_Documento"].ToString(),
                                Razao_Social = dr["raz_social"].ToString(),
                                Telefone = Helper.FormatarTelefone(dr["num_ddd_comercial"].ToString(), dr["num_comercial"].ToString()),
                                Vendedor = dr["nme_vendedor"].ToString(),
                                Plano_ = dr["des_plano"].ToString(),
                                Plano_Situacao = dr["des_plano_situacao"].ToString()
                            };

                            linhaCompra += FormatObject.ToString(ParametrosCarta, linhas);
                            totalCompra++;
                        }
                       
                    }
                    catch (Exception)
                    {

                    }
                    
                }

                var mensagem = carta.Conteudo.Replace("{LinhasTableRecorrente}", linhaRecorrencia).Replace("{LinhasTablePrimeiraCompra}", linhaCompra)
                    .Replace("{QuantidadeTentativasDia}", totalCompra.ToString())
                    .Replace("{QuantidadeTentativasRecorrencia}",totalRecorrencia.ToString()).Replace("{Data}", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));

                carta.Assunto = carta.Assunto.Replace("{Data}", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                    .Enviar(carta.Assunto, mensagem, Enumeradores.CartaEmail.AlertaTentativaCompra, "mailson@bne.com.br",
                        EmailDestino);

            }
            #endregion


            #region [Pessoa Fisica]
            //rezar variaveis
            totalCompra = totalRecorrencia = 0; linhaRecorrencia = linhaCompra = string.Empty;
            EmailDestino = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailDestinatarioAlertaTentativaCompraPF);
            linhas = "<tr><td>{Dta_Tentativa}</td><td>{Motivo}</td><td>{Nome}</td><td>{Plano_}</td><td>{Valor}</td><td>{Plano_Situacao}</td></tr>";
            carta = CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.AlertaTentativaCompraPF);

            parametro = new List<SqlParameter>()
            {
                new SqlParameter{ ParameterName = "@Dta_inicio", SqlDbType = SqlDbType.DateTime, Value  = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(-1).Day,0,0,0)},
                new SqlParameter{ ParameterName = "@Dta_Fim",  SqlDbType = SqlDbType.DateTime, Value =new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.AddDays(-1).Day,23,59,59)}
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spAlertaTentativaCompraPF, parametro))
            {

                while (dr.Read())
                {
                    try
                    {
                        if (Convert.ToInt32(dr["Parcela_Paga"]) > 0){ //tentativa de cobrança recorrencia
                            var ParametrosCarta = new
                        {
                            Dta_Tentativa = Convert.ToDateTime(dr["dta_cadastro"]).ToString("dd/MM/yyyy HH:mm:ss.FFF"),
                            Motivo = dr["des_resultado_solicitacao_aprovacao"].ToString(),
                            Valor = dr["Vlr_Documento"].ToString(),
                            Nome = dr["Nme_Pessoa"].ToString(),
                            Plano_ = dr["des_plano"].ToString(),
                            Plano_Situacao = dr["des_plano_situacao"].ToString()
                        };

                        linhaRecorrencia += FormatObject.ToString(ParametrosCarta, linhas);
                        totalRecorrencia++;
                    }
                        else
                        {
                            var ParametrosCarta = new
                            {
                                Dta_Tentativa = Convert.ToDateTime(dr["dta_cadastro"]).ToString("dd/MM/yyyy HH:mm:ss.FFF"),
                                Motivo = dr["des_resultado_solicitacao_aprovacao"].ToString(),
                                Valor = dr["Vlr_Documento"].ToString(),
                                Nome = dr["Nme_Pessoa"].ToString(),
                                Plano_ = dr["des_plano"].ToString(),
                                Plano_Situacao = dr["des_plano_situacao"].ToString()
                            };

                            linhaCompra += FormatObject.ToString(ParametrosCarta, linhas);
                            totalCompra++;
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                   
                }

                var mensagem = carta.Conteudo.Replace("{LinhasTablePrimeiraCompra}", linhaCompra)
                                .Replace("{LinhasTableRecorrente}", linhaRecorrencia)
                                .Replace("{QuantidadeTentativasDia}", totalCompra.ToString())
                                .Replace("{QuantidadeTentativasRecorrencia}", totalRecorrencia.ToString()).Replace("{Data}", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
                carta.Assunto = carta.Assunto.Replace("{Data}", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                    .Enviar(carta.Assunto, mensagem, Enumeradores.CartaEmail.AlertaTentativaCompraPF, "mailson@bne.com.br",
                        EmailDestino);

            }
            #endregion
        }
        #endregion
    }
}