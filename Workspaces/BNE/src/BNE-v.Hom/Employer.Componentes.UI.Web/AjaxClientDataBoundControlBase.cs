using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Configuration;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Classe abstrata para trabalhar com a biblioteca de Client Script do ScriptManager para componentes compostos com datasource.<br/>
    /// Ela registra no uma biblioteca base em javascript que é usada em todos os componentes ajax.<br/>
    /// Para usar a classe implemente os métodos GetScriptDescriptors e GetScriptReferences seguindo o <see href="http://msdn.microsoft.com/pt-br/library/bb386450(v=vs.110).aspx">padrão da microsoft</see>
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

        #region ModoRenderizacao
        /// <summary>
        /// Padrão de renderização do componente
        /// </summary>
        public ModoRenderizacaoEnum ModoRenderizacao
        {
            get
            {
                var type = typeof(ModoRenderizacaoEnum);
                var value = ConfigurationManager.AppSettings["ModoRenderizacao"];
                var retValue = string.IsNullOrEmpty(value) ? false : Enum.IsDefined(type, value);
                return retValue ? (ModoRenderizacaoEnum)Enum.Parse(type, value) : ModoRenderizacaoEnum.Padrao;
            }
        }
        #endregion

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
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.AjaxClientDataBoundControlBase.js";

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
