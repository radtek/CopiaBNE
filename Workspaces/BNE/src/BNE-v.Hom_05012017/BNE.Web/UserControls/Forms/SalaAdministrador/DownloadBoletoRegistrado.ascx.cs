using BNE.BLL;
using BNE.BLL.Enumeradores;
using BNE.Web.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class DownloadBoletoRegidtrado : BaseUserControl
    {

        #region Consultas
        private string SQL_COMBO_BOX = @"SELECT  ta.Dsc_Tipo_Arquivo AS NOME_BANCO_OPERACAO,
                                                 ta.Idf_Tipo_Arquivo AS COD_BANCO_OPERACAO
                                         FROM    TAB_Tipo_Arquivo AS ta
                                         WHERE   ta.Flg_Remessa = 1";

        #endregion

        #region Propriedades

        protected DataTable dtCBX
        {
            set
            {
                if (value == null)
                    ViewState["dt"] = new DataTable();
                else
                    ViewState["dt"] = value;
            }
            get
            {
                return (DataTable)ViewState["dt"];
            }
        }
        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtCBX = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(SQL_COMBO_BOX, DataAccessLayer.CONN_STRING);
                da.Fill(dtCBX);

                RadComboBoxPaymentMethod.Items.Add(new RadComboBoxItem("Selecione...", "000"));
                foreach (DataRow dr in dtCBX.Rows)
                    RadComboBoxPaymentMethod.Items.Add(new RadComboBoxItem(dr["NOME_BANCO_OPERACAO"].ToString(), dr["COD_BANCO_OPERACAO"].ToString()));
                RadComboBoxPaymentMethod.DataBind();
            }
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Método para gerar os Arquivos de BoletoRegistrado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gerarArquivo_Click(object sender, EventArgs e)
        {
            Arquivo retorno = null;
            string nomeArqs = string.Empty;

            if (RadComboBoxPaymentMethod.SelectedValue.ToString() != "000")
            {
                switch (Convert.ToInt16(RadComboBoxPaymentMethod.SelectedValue))
                {
                    case ((int)BLL.Enumeradores.TipoArquivo.RemessaRegistroBoletos):
                        retorno = Arquivo.GerarArquivo(BLL.Enumeradores.TipoArquivo.RemessaRegistroBoletos);
                        break;
                    case ((int)BLL.Enumeradores.TipoArquivo.RemessaDebitoBB):
                        retorno = Arquivo.GerarArquivo(BLL.Enumeradores.TipoArquivo.RemessaDebitoBB);
                        break;
                    case ((int) BLL.Enumeradores.TipoArquivo.RemessaDebitoHSBC):
                        retorno = Arquivo.GerarArquivo(BLL.Enumeradores.TipoArquivo.RemessaDebitoHSBC); 
                        break;
                }
                nomeArqs = retorno != null ? retorno.NomeArquivo : "";
                retornoArquivoDownload(retorno, nomeArqs);
            }
            else
            {
                base.ExibirMensagem("Selecione um item para download!", BNE.Web.Code.Enumeradores.TipoMensagem.Aviso, false);
            }

        }

        private void retornoArquivoDownload(Arquivo retorno, string nomeArq)
        {
            if (retorno == null)
            {
                base.ExibirMensagem("Não existem arquivos para serem baixados!", BNE.Web.Code.Enumeradores.TipoMensagem.Aviso, false);
            }

            else
            {
                Response.Clear();
                Response.ClearHeaders();

                Response.AddHeader("Content-Length", retorno.GetString().Length.ToString());
                Response.ContentType = "text/plain";
                Response.AppendHeader("content-disposition", "attachment;filename=\"" + nomeArq);
                Response.Write(retorno.GetString());
                Response.End();
            }
        }
        #endregion
    }
}