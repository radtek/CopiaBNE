using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web
{
    public partial class SalaAdministradorVisualizadoCV : BasePage
    {

        #region IdFilial - Variavel 6
        /// <summary>
        /// Propriedade que armazena e recupera o IdEmpresa
        /// </summary>
        public int? IdFilial
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel6.ToString()].ToString());
                return null;
            }

        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
        }

        public void Inicializar()
        {
            ucVisualizadoCV.IdFilial = IdFilial;
            AjustarTituloTela("CVs Visualizados");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "VisualizadoCV");
        }
    }
}