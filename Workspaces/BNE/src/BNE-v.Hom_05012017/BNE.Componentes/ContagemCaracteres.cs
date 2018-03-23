using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Componentes
{
    /// <summary>
    /// Campo texto formatado para datas
    /// </summary>
    public class ContagemCaracteres : WebControl, IScriptControl
    {

        private readonly Literal _label = new Literal();
        private ScriptManager _sm;

        #region Propriedades

        #region ControlToValidate
        /// <summary>
        /// Define o controle que será efetuada a contagem
        /// </summary>
        [DisplayName("ControlToValidate")]
        public string ControlToValidate
        {
            get
            {
                return (string)ViewState["ControlToValidate"] ?? string.Empty;
            }
            set
            {
                ViewState["ControlToValidate"] = value;
            }
        }
        #endregion

        #region MaxLength
        /// <summary>
        /// Define a quantidade maxima de caracteres aceitos
        /// </summary>
        [DisplayName("MaxLength")]
        public int MaxLength
        {
            get
            {
                if (ViewState["MaxLength"] == null)
                    return 0;
                return (int)ViewState["MaxLength"];
            }
            set { ViewState["MaxLength"] = value; }
        }
        #endregion

        #region CssClassLabel
        public String CssClassLabel
        {
            set
            {
                CssClass = value;
            }
        }
        #endregion

        #region PatternText
        public String PatternText
        {
            get
            {
                if (ViewState["PatternText"] == null)
                    return "Faltam {0} caracteres";
                return (string)ViewState["PatternText"];
            }
            set { ViewState["PatternText"] = value; }
        }
        #endregion

        #endregion

        #region Métodos

        #region CreateChildControls
        /// <inheritdoc />
        protected override void CreateChildControls()
        {
            InicializarLabel();

            base.CreateChildControls();
        }
        #endregion

        #region InicializarLabel
        /// <summary>
        /// Método de configuração das propriedades do campo label
        /// </summary>
        protected void InicializarLabel()
        {
            _label.Text = string.Format(PatternText, MaxLength);
            Controls.Add(_label);
        }
        #endregion

        #endregion

        #region OnPreRender
        /// <summary>
        /// Fase de pré-renderização
        /// </summary>        
        protected override void OnPreRender(EventArgs e)
        {
            if (!DesignMode)
            {
                // Pega o script manager da página atual
                _sm = ScriptManager.GetCurrent(Page);
                if (_sm == null)
                    throw new HttpException("É necessário ter um ScriptManager ou RadScriptManager registrado na página para poder prosseguir");

                _sm.RegisterScriptControl(this);
            }

            base.OnPreRender(e);
        }
        #endregion

        #region Render
        /// <summary>
        /// Renderiza o controle
        /// </summary>
        /// <param name="writer">O renderizador da página</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (!DesignMode)
                _sm.RegisterScriptDescriptors(this);

            base.Render(writer);
        }
        #endregion

        public IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var descriptor = new ScriptControlDescriptor("BNE.Componentes.ContagemCaracteres", ClientID);

            descriptor.AddProperty("ControlToValidate", GetControlRenderID(ControlToValidate));
            descriptor.AddProperty("MaxLength", MaxLength);
            descriptor.AddProperty("PatternText", PatternText);
            descriptor.AddProperty("Label", ClientID);

            yield return descriptor;
        }

        protected string GetControlRenderID(string name)
        {

            // get the control using the relative name
            Control c = FindControl(name);
            if (c == null)
            {
                Debug.Fail("We should have already checked for the presence of this");
                return String.Empty;
            }
            return c.ClientID;
        }

        public IEnumerable<ScriptReference> GetScriptReferences()
        {
            var reference = new ScriptReference { Assembly = "BNE.Componentes", Name = "BNE.Componentes.Content.js.ContagemCaracteres.js" };

            yield return reference;
        }
    }
}
