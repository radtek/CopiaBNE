using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web
{
    public partial class SalaVipEscolherEmpresa : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "Escolher Empresa");
        }

        public void Inicializar()
        {
            AjustarTituloTela("Escolher Empresa");
            PropriedadeAjustarTopoUsuarioCandidato(true);
        }
    }
}