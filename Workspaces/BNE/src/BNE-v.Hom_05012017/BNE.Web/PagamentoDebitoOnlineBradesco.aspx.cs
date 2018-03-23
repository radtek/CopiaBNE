using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace BNE.Web
{
    public partial class PagamentoDebitoOnlineBradesco : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string xml = string.Empty;

            switch (Request["TransId"])
            {
                case "getTransfer":
                    xml = string.Format(Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.XMLPagamentoTransferenciaDebitoOnlineBradesco)
                                             , Request["orderid"]
                                             , Request["descritivo"]
                                             , Request["quantidade"]
                                             , Request["unidade"]
                                             , Request["valor"]
                                             , Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.agenciaDebitoOnlineBradesco)
                                             , Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.contaDebitoOnlineBradesco)
                                             , Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.AssinaturaDebitoOnlineBradesco));
                    break;

                case "putAuth":
                    if (Request["if"].ToString().ToLower() == "bradesco")
                    {
                        string msgErro = string.Empty;
                        Transacao objTransacao = Transacao.LoadObject(Convert.ToInt32(Request["numOrder"]));

                        using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    if (Request["cod"] != "0" || string.IsNullOrEmpty(Request["Assinatura"]))//Falha
                                    {
                                        msgErro = falha(Request["cod"]);
                                        objTransacao.CancelamentoDebitoOnline(msgErro, trans);
                                        xml = "<ERRO>";
                                    }
                                    else
                                    {
                                        //Altualizando os status
                                        objTransacao.DescricaoTransacao = Request["Protocolo"];
                                        objTransacao.StatusTransacao.IdStatusTransacao =(int)BNE.BLL.Enumeradores.StatusTransacao.Realizada;
                                        objTransacao.Save(trans);

                                        objTransacao.Pagamento.CompleteObject();
                                        objTransacao.Pagamento.Liberar(trans, DateTime.Now);
                                        xml = "<PUT_AUTH_OK>";
                                    }
                                    trans.Commit();
                                }
                                catch (Exception ex)
                                {
                                    trans.Rollback();
                                    EL.GerenciadorException.GravarExcecao(ex);
                                    throw;
                                }
                                finally
                                {
                                    conn.Close();
                                }
                            }
                        }
                    }

                    break;
            }
            Response.ContentType = "text/xml";
            Response.ContentEncoding = Encoding.UTF8;
            Response.Write(xml);
            Response.End();
        }


        #region FALHA
        private string falha(string situacao)
        {
            XElement xml = XElement.Load("~/xml/erros_debito_online_bradesco.xml");
            XElement erro = xml.Elements().Where((XAttribute => xml.Attribute("codigo").Value.Equals(situacao))).First();
            return string.Concat(xml.Attribute("codigo").Value, string.IsNullOrEmpty(erro.Attribute("origem").Value) ? "" : " - " + erro.Attribute("origem").Value," - ", erro.Attribute("descricao").Value);
        }
        #endregion
    }
}