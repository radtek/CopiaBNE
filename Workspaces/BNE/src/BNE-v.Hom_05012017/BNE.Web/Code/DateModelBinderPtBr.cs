using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace BNE.Web
{
    public class DateModelBinderPtBr : IModelBinder
    {
        private readonly Func<ControllerContext, bool> _applicable;
        public DateModelBinderPtBr(Func<ControllerContext, bool> applicable)
        {
            if (applicable == null)
                throw new NullReferenceException("applicable");
            this._applicable = applicable;
        }
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var str = controllerContext.HttpContext.Request.QueryString[bindingContext.ModelName];
            if (string.IsNullOrEmpty(str))
                return null;

            if (!_applicable(controllerContext))
                return str;

            if (str.Length != 8 &&
                str.Length != 10)
                return default(DateTime);

            DateTime date;
            if (DateTime.TryParseExact(str, "dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-br"), System.Globalization.DateTimeStyles.None, out date) ||
                DateTime.TryParseExact(str, "dd/MM/yy", CultureInfo.GetCultureInfo("pt-br"), System.Globalization.DateTimeStyles.None, out date))
            {
                return date;
            }
            return default(DateTime);

        }
    }
}
