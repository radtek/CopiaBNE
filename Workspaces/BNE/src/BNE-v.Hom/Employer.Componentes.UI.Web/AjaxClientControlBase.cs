using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Classe abstrata para trabalhar com a biblioteca de Client Script do ScriptManager para componentes compostos sem datasource.<br/>
    /// Ela registra no uma biblioteca base em javascript que é usada em todos os componentes ajax.<br/>
    /// Para usar a classe implemente os métodos GetScriptDescriptors e GetScriptReferences seguindo o <see href="http://msdn.microsoft.com/pt-br/library/bb386450(v=vs.110).aspx">padrão da microsoft</see>. 
    /// </summary>
    public abstract class AjaxClientControlBase : CompositeControl , IScriptControl
    {
        #region Atributos
        private ScriptManager _Sm = null;
        #endregion        

        #region Propriedades
        /// <summary>
        /// Referência para instância do ScriptManager atual
        /// </summary>
        protected ScriptManager Sm
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

        #region SetScriptDescriptors
        /// <summary>
        /// Registra os descritores de script
        /// </summary>
        /// <param name="descriptor">O Descritor de script</param>
        protected virtual void SetScriptDescriptors(ScriptControlDescriptor descriptor)
        {
        }
        #endregion 

        #region SetScriptReferences
        /// <summary>
        /// Registra as referências de script
        /// </summary>
        /// <param name="references">Uma coleção de referências de script</param>
        protected virtual void SetScriptReferences(IList<ScriptReference> references)
        {
            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.AjaxClientControlBase.js";

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
            }

            base.OnPreRender(e);
        }
        #endregion 

        #region Render
        /// <summary>
        /// Renderiza o controle para o browser
        /// </summary>
        /// <param name="writer">O HtmlTextWriter usado na operação</param>
        protected override void Render(HtmlTextWriter writer)
        {

            if (!this.DesignMode)
            {
                // Registra os descritores de script
                this.Sm.RegisterScriptDescriptors(this);
            }

            base.Render(writer);
        }
        #endregion

        #region GetScriptDescriptors
        /// <summary>
        /// Retorna os descritores de script
        /// </summary>
        /// <returns>Coleção dos descritores de script</returns>
        public abstract IEnumerable<ScriptDescriptor> GetScriptDescriptors();
        #endregion 

        #region GetScriptReferences
        /// <summary>
        /// Retorna as referencias de script
        /// </summary>
        /// <returns>Coleção de referências de script</returns>
        public abstract IEnumerable<ScriptReference> GetScriptReferences();
        #endregion

        #endregion
    }
}
