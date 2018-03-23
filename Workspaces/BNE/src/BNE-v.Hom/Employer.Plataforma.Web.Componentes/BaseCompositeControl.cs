using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Employer.Plataforma.Web.Componentes
{
    public class BaseCompositeControl : CompositeControl
    {
        // Define se registra ou não os JavaScripts referentes ao comportamento do controle
        // Foi adicionado devido ao comportamento padrão o quel está registrando diversas vezes,
        // muitas delas desnecessárias, causando uma lentidão no carregamento de diversas instancias
        // Eduardo Ordine
        private Boolean _loadScripts = true;

        #region LoadScripts

        /// <summary>
        /// Define se registra ou não os JavaScripts referentes ao comportamento do controle
        /// Foi adicionado devido ao comportamento padrão o quel está registrando diversas vezes,
        /// muitas delas desnecessárias, causando uma lentidão no carregamento de diversas instancias
        /// </summary>
        [Category("Employer - Valor Decimal"),
         DisplayName("LoadScripts"),
         DefaultValue("true")]
        public bool LoadScripts
        {
            get
            {
                return _loadScripts;
            }
            set
            {
                _loadScripts = value;
            }
        }

        #endregion
    }
}
