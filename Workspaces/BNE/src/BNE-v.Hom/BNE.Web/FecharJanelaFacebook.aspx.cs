using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;

namespace BNE.Web
{
    public partial class FecharJanelaFacebook : Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            Inicializar();
        }
        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            EmiteJavascriptParaFecharJanela(ExistePostIdFacebook());
        }
        #endregion

        #region ExistePostIdFacebook
        private bool ExistePostIdFacebook()
        {
            return Request.QueryString["post_id"] != null;
        }
        #endregion

        #region EmiteHtmlParaFecharJanela
        private void EmiteJavascriptParaFecharJanela(bool sucesso)
        {
            Response.Clear();
            Response.Write(@"
<script type=""text/javascript"">");
            if (sucesso)
                Response.Write(@"
    opener.mostrarSucessoFacebook();");
            Response.Write(@"
    window.open('javascript:window.open("""", ""_self"", """");window.close();', '_self');
</script>
");
            Response.End();
        }
        #endregion
        #endregion
    }
}