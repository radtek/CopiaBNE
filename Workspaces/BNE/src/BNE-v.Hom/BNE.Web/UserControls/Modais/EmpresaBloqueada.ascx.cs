
namespace BNE.Web.UserControls.Modais
{
    public partial class EmpresaBloqueada : System.Web.UI.UserControl
    {
        #region Metodos

        #region MostrarModal
        public void MostrarModal()
        {
            pnlBloqueada.Visible = true;
            upConteudoBloqueio.Update();
            mpeEmpresaBloqueada.Show();
        }

        public void MostrarModalForaHorario()
        {
            pnlAuditoria.Visible = true;
            upConteudoBloqueio.Update();
            mpeEmpresaBloqueada.Show();
        }
        #endregion

        #endregion
    }
}