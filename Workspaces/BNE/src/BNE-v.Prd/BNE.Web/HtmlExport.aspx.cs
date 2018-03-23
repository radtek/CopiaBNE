using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using System.Data;
using BNE.BLL;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace BNE.Web
{
    public partial class HtmlExport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Variáveis Globais
            ///Gera a string que vai 'imprimir' o excel
            StringBuilder sb = new StringBuilder();

            ///Obtem o nome do relatório e de acordo com o nome verifica qual relatorio gerar
            string relatorio = Request.QueryString["rel"];

            ///Obtém o DataTable para geração do Excel
            DataTable dt = null;

            #endregion

            #region Verificacao e Obtenção de Dados
            if (string.IsNullOrEmpty(relatorio))
            {
                relatorio = "Relatorio";
            }

            switch (relatorio)
            {
                case "NovasEmpresas":

                    #region Informações de Estilos e abertura da Tabela Html

                    ///Isso pode mudar de acordo com a estilização desejada pelo usuário
                    sb.Append("<style type='text/css'>.num {mso-number-format:General;}.text{mso-number-format:'\\@';/*force text*/}</style>");
                    sb.Append("<font style='font-size:12.0pt; font-family:Calibri;'></br></br></br>");
                    sb.Append("<table border='1' bgColor='#ffffff' borderColor='#000000' cellspacing='0' cellpadding='0'");
                    sb.Append("style='font-size:12.0pt; font-family:Calibri; background:white;'><tr style='background:#99AABB'>");

                    #endregion

                    string dtIni, dtFim;

                    dtIni = Request.QueryString["dtIn"];
                    dtFim = Request.QueryString["dtFn"];

                    dt = Filial.ListarFiliaisDataCadastro(DateTime.Parse(dtIni), DateTime.Parse(dtFim));

                    break;
                default:
                    break;
            }
            #endregion

            #region Inicio da Geração do arquivo

            ///Se a consulta retornou resultados...
            if (dt != null)
            {
                #region MontaResponse

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                
                ///TO DO: Pode ser alterado para gerar outros tipos de arquivo como doc, csv... 
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                ///

                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + relatorio+".xls");
                HttpContext.Current.Response.Charset = "ISO-8859-1";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                
                #endregion

                #region Inicio da impressão de conteúdo

                ///Criação de Cabeçalhos
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sb.Append("<td><b>");
                    sb.Append(dt.Columns[j].ColumnName);
                    sb.Append("</b></td>");
                }

                sb.Append("</tr>");

                if (dt.Rows.Count > 0)
                {
                    ///Criação do Conteúdo Excel
                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (i == 3)
                                sb.Append("<td class='num'>");
                            else
                                sb.Append("<td class='text'>");
                            sb.Append(row[i].ToString());
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }
                }
                else
                {
                    sb.Append("<tr>");
                    sb.Append("<td class='text' colspan='"+dt.Columns.Count+"'>Não há informações no período informado</td>");
                    sb.Append("</tr>");
                }
                

                #endregion

                #region Finaliza a tabela Html e Envia o conteúdo

                sb.Append("</table></font>");
                HttpContext.Current.Response.Write(sb.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();

                #endregion
            }
            else
            {
                #region MontaResponse

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;

                ///TO DO: Pode ser alterado para gerar outros tipos de arquivo como doc, csv... 
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                ///

                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + relatorio + ".xls");
                HttpContext.Current.Response.Charset = "ISO-8859-1";
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

                sb.Append("<td><b>");
                sb.Append("Período inválido ou Erro no Processamento");
                sb.Append("</b></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td class='text' colspan='3'>Não há informações no período informado</td>");
                sb.Append("</tr>");

                sb.Append("</table></font>");
                HttpContext.Current.Response.Write(sb.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                #endregion
            }

            #endregion
        }
    }    
}