using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.Enumeradores;
using BNE.Web.Code;

namespace BNE.Web
{
    public partial class ImprimirCurriculo : BasePage
    {

        #region Propriedades

        #region IdCurriculoImpressaoCurriculo - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int IdCurriculoImpressaoCurriculo
        {
            get
            {
                return Convert.ToInt32(RouteData.Values["Identificador"]);
            }
        }
        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((base.IdCurriculo.HasValue && base.IdCurriculo.Value.Equals(IdCurriculoImpressaoCurriculo)) || base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                    litCurriculo.Text = new Curriculo(IdCurriculoImpressaoCurriculo).RecuperarHTMLCurriculo(true, true, FormatoVisualizacaoImpressao.Candidato);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Imprimir", "javascript:setTimeout('window.print()', 600);", true);
        }
    }
}