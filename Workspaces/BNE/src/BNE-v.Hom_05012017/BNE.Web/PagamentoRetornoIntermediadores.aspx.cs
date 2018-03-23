using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web
{
    public partial class PagamentoRetornoIntermediadores : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RouteData.Values["Intermediador"] != null)
            {
                Transacao objTransacao = null;

                #region PagSeguro
                if (RouteData.Values["Intermediador"].ToString() == IntermediadorPagamento.PagSeguro.ToString())
                {
                    base.PagamentoFormaPagamento.Value = (int)BNE.BLL.Enumeradores.TipoPagamento.PagSeguro;
                    if (RouteData.Values["IdfPlanoAdquirido"] != null)
                    {
                        if (RouteData.Values["IdfPlanoAdquirido"].ToString().ToLower() == "notificacao")
                        {
                            string notificationType = Request.Form["notificationType"];
                            string notificationCode = Request.Form["notificationCode"];

                            if (notificationType == "transaction" && !String.IsNullOrEmpty(notificationCode))
                            {
                                Transacao.AtualizarSituacaoPagSeguro(notificationCode);
                            }

                            return;
                        }
                        else
                        {
                            string idTransaction = Request.QueryString["id_transacao"];
                            Transacao.AtualizarSituacaoPagSeguro(idTransaction, out objTransacao);
                        }
                    }
                }
                #endregion PagSeguro

                #region PayPal
                if (RouteData.Values["Intermediador"].ToString() == IntermediadorPagamento.PayPal.ToString())
                {
                    base.PagamentoFormaPagamento.Value = (int)BNE.BLL.Enumeradores.TipoPagamento.PayPal;
                    string token = Request.QueryString["token"];
                    if (!String.IsNullOrEmpty(token))
                    {
                        Transacao.AtualizarSituacaoPayPal(token, out objTransacao);
                    }
                    else
                    {
                        int idf_transacao;
                        if (Int32.TryParse(Request["custom"], out idf_transacao) && idf_transacao > 0)
                        {
                            Transacao.AtualizarSituacaoTransacaoPayPal(idf_transacao, Request["payment_status"].ToString(), out objTransacao);
                        }
                        else
                        {
                            EL.GerenciadorException.GravarExcecao(new Exception("Não foi possível reconhecer a transação PayPal da notificação"));
                        }
                    }
                    
                }
                #endregion PayPal


                #region RedirecionamentoParaConfirmação
                int IdfPlanoAdquirido;
                if (RouteData.Values["IdfPlanoAdquirido"] != null && Int32.TryParse(RouteData.Values["IdfPlanoAdquirido"].ToString(), out IdfPlanoAdquirido) && objTransacao != null)
                {
                    PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IdfPlanoAdquirido);
                    base.PagamentoValorPago.Value = objTransacao.ValorDocumento;
                    base.PagamentoIdentificadorPlanoAdquirido.Value = Convert.ToInt32(RouteData.Values["IdfPlanoAdquirido"]);

                    if (objTransacao.PlanoAdquirido.Plano == null)
                    {
                        objTransacao.PlanoAdquirido.CompleteObject();
                    }
                    if (objTransacao.PlanoAdquirido.Plano == null || objTransacao.PlanoAdquirido.Plano.PlanoTipo == null)
                    {
                        objTransacao.PlanoAdquirido.Plano.CompleteObject();
                        objTransacao.PlanoAdquirido.Plano.PlanoTipo.CompleteObject();
                    }

                    if (!base.IdUsuarioFilialPerfilLogadoCandidato.HasValue &&
                        !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue &&
                        !base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                    {
                        if (objTransacao.PlanoAdquirido.UsuarioFilialPerfil == null)
                            objTransacao.PlanoAdquirido.CompleteObject();
                        if (objTransacao.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica == null)
                            objTransacao.PlanoAdquirido.UsuarioFilialPerfil.CompleteObject();

                        this.LogarAutomaticoPessoaFisica(objTransacao.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica);
                    }

                    if (objTransacao.PlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo == (int)BLL.Enumeradores.PlanoTipo.PessoaFisica)
                    {
                        Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamento.ToString(), null));
                    }
                    Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamentoCIA.ToString(), null));
                }
                #endregion
            }
        }
    }
}