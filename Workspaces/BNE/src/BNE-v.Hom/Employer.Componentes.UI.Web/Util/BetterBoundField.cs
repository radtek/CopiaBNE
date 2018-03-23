// -----------------------------------------------------------------------
// <copyright file="BetterBoundField.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Employer.Componentes.UI.Web.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;
    using System.Web.UI;

    /// <summary>
    /// Classe de BoundField que realiza bind em mais de um nível.<br/>
    /// Ex.: Caso seja passado no DataField uma propriedade x.pto.yyy ele iré chamar x depois pto de x e por fim yyy de pto
    /// </summary>
    public class BetterBoundField : BoundField
    {
        /// <inheritdoc/>
        protected override object GetValue(Control controlContainer)
        {
            if (DataField.Contains("."))
            {
                var component = DataBinder.GetDataItem(controlContainer);
                return DataBinder.Eval(component, DataField);
            }
            return base.GetValue(controlContainer);
        }
    }
}
