using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.curriculo
{
    public partial class Curriculo : BasePage
    {

        #region Propriedades

        #region IdCurriculoVisualizacaoCurriculo - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int IdCurriculoVisualizacaoCurriculo
        {
            get
            {
                return Convert.ToInt32(RouteData.Values["Identificador"]);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, true, "Curriculo");
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            ucVisualizacaoCurriculo.Inicializar(IdCurriculoVisualizacaoCurriculo);
        }
        #endregion

        #endregion

    }
}