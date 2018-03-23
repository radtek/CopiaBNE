using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Employer.Componentes.UI.Web.Extensions
{
    /// <summary>
    /// Classe utilitária para criar de forma mais fácil a estrutura html dos componentes.
    /// </summary>
    #pragma warning disable 1591
    public static class PanelExtensions
    {    
        public static void AdicionarLinhaPadraoLista(this System.Web.UI.WebControls.Panel instance, String label, Control controle,
            List<HtmlGenericControl> lsLinha, List<HtmlGenericControl> lsContainerRelatorio)
        {
            AdicionarLinhaPadrao(instance, label, controle, string.Empty, string.Empty, lsLinha, lsContainerRelatorio);
        }

        public static void AdicionarLinhaPadrao(this System.Web.UI.WebControls.Panel instance,  String label, Control controle, 
            String cssClassLinha, String cssClassContainerCampo,
            List<HtmlGenericControl> lsLinha = null, List<HtmlGenericControl> lsContainerRelatorio = null)
        {
            instance.AdicionarLinhaPadrao(
                new Label() { Text = label, AssociatedControlID = controle.ID },
                controle, cssClassLinha, cssClassContainerCampo, lsLinha, lsContainerRelatorio);
        }


        public static void AdicionarLinhaPadrao(this System.Web.UI.WebControls.Panel instance, Label objLabel, Control controle, 
            String cssClassLinha, String cssClassContainerCampo,
            List<HtmlGenericControl> lsLinha = null, List<HtmlGenericControl> lsContainerRelatorio = null)
        {
            HtmlGenericControl divLinhaRelatorio = new HtmlGenericControl("div");
            divLinhaRelatorio.Attributes["class"] = cssClassLinha;
            divLinhaRelatorio.Controls.Add(objLabel);
            if (lsLinha != null)
                lsLinha.Add(divLinhaRelatorio);

            HtmlGenericControl divContainerRelatorio = new HtmlGenericControl("div");
            divContainerRelatorio.Attributes["class"] = cssClassContainerCampo;
            divContainerRelatorio.Controls.Add(controle);
            if (lsContainerRelatorio != null)
                lsContainerRelatorio.Add(divContainerRelatorio);

            divLinhaRelatorio.Controls.Add(divContainerRelatorio);

            instance.Controls.Add(divLinhaRelatorio);
        }

        /// <summary>
        /// Adiciona uma linha contendo label, controle-controle seguindo a estrutura <div><label><div><controle /><controle /></div></label></div>
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="objLabel"></param>
        /// <param name="controleEsquerda"></param>
        /// <param name="controleDireita"></param>
        /// <param name="cssClassLinha"></param>
        /// <param name="cssClassContainerCampo"></param>
        public static void AdicionarLinhaPadrao(this System.Web.UI.WebControls.Panel instance, Label objLabel, Control controleEsquerda, Control controleDireita, String cssClassLinha, String cssClassContainerCampo)
        {
            instance.Controls.Add(objLabel);
            if (!String.IsNullOrEmpty(cssClassLinha))
                instance.Attributes["class"] = cssClassLinha;

            HtmlGenericControl divLinhaRelatorio = new HtmlGenericControl("div");
            divLinhaRelatorio.Controls.Add(controleEsquerda);
            divLinhaRelatorio.Controls.Add(controleDireita);
            
            if (!String.IsNullOrEmpty(cssClassContainerCampo))
                divLinhaRelatorio.Attributes["class"] = cssClassContainerCampo;

            instance.Controls.Add(divLinhaRelatorio);
        }
    }
}
