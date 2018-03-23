using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;

namespace BNE.Web.UserControls.Modais
{
    public partial class AguardeProcessando : BaseUserControl
    {
 
            
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region MostrarModal
        public void MostrarModal()
        {
            mpeModalAguarde.Show();
        }
        #endregion


        #region PreencherCampos
    
        public void PreencherCampos(string strTitulo, string strMensagem, bool visualizarCliqueAqui, string nomeBotao)
        {
            lblTitulo.Text = strTitulo;
            lblTexto.Text = strMensagem;
            upConteudo.Update();
        }
        #endregion

   
      
    }
}