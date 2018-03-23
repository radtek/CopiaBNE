using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Code;
using System.Data;
using Telerik.Web.UI;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class BoletoAvulsoPJ : BaseUserControl
    {
        #region Propriedades

        #region PageIndex - PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            PageIndex = 1;
            gvBoletoAvulsoPJ.PageSize = 6;
            CarregarGrid();
        }


        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros = 8;
            UIHelper.CarregarRadGrid(gvBoletoAvulsoPJ, RetornaEdicaoCVTemporario(), totalRegistros);
        }

        /// <summary>
        /// Criado apenas para aplicação do design, remover no desenvolvimento
        /// </summary>
        /// <returns></returns>
        public DataTable RetornaEdicaoCVTemporario()
        {

            DataTable tb = new DataTable();

            tb.Columns.Add(new DataColumn("Num_Parcela"));
            tb.Columns.Add(new DataColumn("Nme_Situacao"));
            tb.Columns.Add(new DataColumn("Dta_Enviado"));
            tb.Columns.Add(new DataColumn("Dta_Vencimento"));
            tb.Columns.Add(new DataColumn("Des_NossoNumero"));
            tb.Columns.Add(new DataColumn("Vlr_Parcela"));
            tb.Columns.Add(new DataColumn("Nme_Enviado"));


            DataRow row = tb.NewRow();
            row[0] = "1 Parcela";
            row[1] = "Pago";
            row[2] = "24/04/2010";
            row[3] = "30/03/2011";
            row[4] = "41558";
            row[5] = "150,00";
            row[6] = "tortato@bne.com.br";
            tb.Rows.Add(row);

            row = tb.NewRow();
            row[0] = "2 Parcela";
            row[1] = "Pago";
            row[2] = "24/04/2010";
            row[3] = "30/03/2011";
            row[4] = "41558";
            row[5] = "150,00";
            row[6] = "tortato@bne.com.br";
            tb.Rows.Add(row);

            row = tb.NewRow();
            row[0] = "3 Parcela";
            row[1] = "Pago";
            row[2] = "24/04/2010";
            row[3] = "30/03/2011";
            row[4] = "41558";
            row[5] = "150,00";
            row[6] = "tortato@bne.com.br";
            tb.Rows.Add(row);

            row = tb.NewRow();
            row[0] = "4 Parcela";
            row[1] = "Pago";
            row[2] = "24/04/2010";
            row[3] = "30/03/2011";
            row[4] = "41558";
            row[5] = "150,00";
            row[6] = "tortato@bne.com.br";
            tb.Rows.Add(row);

            row = tb.NewRow();
            row[0] = "5 Parcela";
            row[1] = "Pago";
            row[2] = "24/04/2010";
            row[3] = "30/03/2011";
            row[4] = "41558";
            row[5] = "150,00";
            row[6] = "tortato@bne.com.br";
            tb.Rows.Add(row);

            row = tb.NewRow();
            row[0] = "6 Parcela";
            row[1] = "Pago";
            row[2] = "24/04/2010";
            row[3] = "30/03/2011";
            row[4] = "41558";
            row[5] = "150,00";
            row[6] = "tortato@bne.com.br";
            tb.Rows.Add(row);

            row = tb.NewRow();
            row[0] = "7 Parcela";
            row[1] = "Pago";
            row[2] = "24/04/2010";
            row[3] = "30/03/2011";
            row[4] = "41558";
            row[5] = "150,00";
            row[6] = "tortato@bne.com.br";
            tb.Rows.Add(row);

            row = tb.NewRow();
            row[0] = "8 Parcela";
            row[1] = "Pago";
            row[2] = "24/04/2010";
            row[3] = "30/03/2011";
            row[4] = "41558";
            row[5] = "150,00";
            row[6] = "tortato@bne.com.br";
            tb.Rows.Add(row);

            row = tb.NewRow();
            row[0] = "9 Parcela";
            row[1] = "Pago";
            row[2] = "24/04/2010";
            row[3] = "30/03/2011";
            row[4] = "41558";
            row[5] = "150,00";
            row[6] = "tortato@bne.com.br";
            tb.Rows.Add(row);


            return tb;
        }

        #endregion


        #region gvBoletoAvulsoPJ_PageIndexChanged
        protected void gvBoletoAvulsoPJ_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion
    }
}