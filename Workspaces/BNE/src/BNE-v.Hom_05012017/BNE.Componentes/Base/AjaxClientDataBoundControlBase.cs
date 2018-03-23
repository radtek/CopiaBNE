using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace BNE.Componentes.Base
{
    /// <summary>
    /// Classe base para os controle ajax ligados a dados
    /// </summary>
    public abstract class AjaxClientDataBoundControlBase : CompositeDataBoundControl, IScriptControl
    {
        #region Atributos
        private bool _RegisterScriptControl = false;
        private ScriptManager _Sm = null;
        #endregion
        
        #region Propriedades
        /// <summary>
        /// Instância do ScriptManager atual
        /// </summary>
        private ScriptManager Sm
        {
            get
            {
                if (_Sm == null)
                    _Sm = ScriptManager.GetCurrent(this.Page);
                if (_Sm == null)
                    throw new System.Exception("É necessário ter um ScriptManager ou RadScriptManager registrado na página para poder prosseguir");
                return _Sm;
            }
        }
        #endregion

        #region Métodos

        #region CreateChildControls
        /// <summary>
        /// Cria os controles filhos de forma que sejam preenchidos por dados de uma fonte
        /// </summary>
        /// <param name="dataSource">A fonte de dados</param>
        /// <param name="dataBinding">Verdadeiro se estiver durante a fase de databind</param>
        /// <returns>Quantidade de registros criados</returns>
        abstract protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding);
        #endregion 

        #region SetScriptDescriptors
        /// <inheritdoc />
        protected virtual void SetScriptDescriptors(ScriptControlDescriptor descriptor)
        {
        }
        #endregion 

        #region SetScriptReferences
        /// <inheritdoc />
        protected virtual void SetScriptReferences(IList<ScriptReference> references)
        {
            ScriptReference reference = new ScriptReference();
            reference.Assembly = "BNE.Componentes";
            reference.Name = "BNE.Componentes.Content.js.AjaxClientDataBoundControlBase.js";

            references.Add(reference);
        }
        #endregion 

        #region OnPreRender
        /// <summary>
        /// Fase de pré-renderização
        /// </summary>        
        protected override void OnPreRender(EventArgs e)
        {
            // Registra o controle caso não esteja em tempo de design
            if (!this.DesignMode)
            {
                this.Sm.RegisterScriptControl(this);
                _RegisterScriptControl = true;
            }

            base.OnPreRender(e);
        }
        #endregion 

        #region Render
        /// <inheritdoc />
        protected override void Render(HtmlTextWriter writer)
        {

            if (!this.DesignMode && _RegisterScriptControl)
            {
                // Registra os descritores de script
                this.Sm.RegisterScriptDescriptors(this);
            }

            base.Render(writer);
        }
        #endregion 

        #region GetScriptDescriptors
        /// <inheritdoc />
        public abstract IEnumerable<ScriptDescriptor> GetScriptDescriptors();
        #endregion 

        #region GetScriptReferences
        /// <inheritdoc />
        public abstract IEnumerable<ScriptReference> GetScriptReferences();
        #endregion

        #endregion
    }
}
