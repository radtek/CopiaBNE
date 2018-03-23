using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.Erros
{
    public partial class Erros : Page
    {

        #region Propriedades

        #region ListArquivos - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera os arquivos
        /// </summary>
        public List<string> ListArquivos
        {
            get
            {
                return (List<string>)(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region CarregarGrid
        private void CarregarGrid()
        {
            DataTable dt = null;
            try
            {
                dt = new DataTable();
                dt.Columns.Add("arquivo");
                dt.Columns.Add("Data");
                dt.Columns.Add("Usuario");
                dt.Columns.Add("Codigo");
                dt.Columns.Add("Mensagem");
                dt.Columns.Add("StackTrace");
                dt.Columns.Add("InnerException");
                dt.Columns.Add("HelpLink");
                dt.Columns.Add("Source");

                foreach (string arquivo in ListArquivos)
                {
                    string texto = File.ReadAllText(arquivo);
                    string[] exception = texto.Split('*');

                    DataRow dr = dt.NewRow();
                    dr["arquivo"] = arquivo;

                    if (exception.Length >= 9)
                    {
                        dr["Data"] = exception[1];
                        dr["Usuario"] = exception[2];
                        dr["Codigo"] = exception[3];
                        dr["Mensagem"] = exception[4];
                        dr["StackTrace"] = exception[5];
                        dr["InnerException"] = exception[6];
                        dr["HelpLink"] = exception[7];
                        dr["Source"] = exception[8];
                    }
                    dt.Rows.Add(dr);
                }
                gvErros.DataSource = dt;
                gvErros.DataBind();
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            lblQuantidadeRegistros.Text = String.Format("({0}) arquivos", gvErros.Items.Count);

            upErros.Update();
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListArquivos = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "logErroBNE\\").ToList<string>();
                CarregarGrid();
            }
        }
        #endregion

        #region gvErros_DeleteCommand
        protected void gvErros_DeleteCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            string arquivo = gvErros.MasterTableView.DataKeyValues[e.Item.ItemIndex]["arquivo"].ToString();
            File.Delete(arquivo);
            ListArquivos.Remove(arquivo);
            CarregarGrid();
        }
        #endregion

        #region btnLimparPasta_Click
        protected void btnLimparPasta_Click(object sender, EventArgs e)
        {
            foreach (string arquivo in ListArquivos)
                File.Delete(arquivo);

            ListArquivos = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "logErroBNE\\").ToList<string>();
            CarregarGrid();
        }
        #endregion

        #endregion

    }
}
