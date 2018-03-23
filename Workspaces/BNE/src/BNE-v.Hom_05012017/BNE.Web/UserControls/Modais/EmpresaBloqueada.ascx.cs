using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web.UserControls.Modais
{
    public partial class EmpresaBloqueada : System.Web.UI.UserControl
    {
        #region Metodos

        #region MostrarModal
        public void MostrarModal()
        {
            pnlBloqueada.Visible = true;
            mpeEmpresaBloqueada.Show();
        }

        public void MostrarModalForaHorario()
        {
            pnlAuditoria.Visible = true;
            mpeEmpresaBloqueada.Show();
        }
        #endregion

        #endregion
    }
}