using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllInMail.Helper;
using System.Linq.Expressions;

namespace AllInMail.Core
{

    public class DefPreFilterBase<T> : DefFilterInit<T, T>
    {
        public DefPreFilterBase(Func<T, T> func)
            : base(func)
        {

        }

        public DefPreFilterBase(string propName)
            : base(propName)
        {

        }
        public DefPreFilterBase(string propName, Func<T, T> func)
            : base(propName, func)
        {

        }

        public override bool SafeConversion
        {
            get
            {
                return true;
            }
        }

        protected override T ConvertArgBeforeFilter(object value)
        {
            return value.ConvertOrDefault<T>();
        }
    }

    public class DefPreFilterBaseFor<TObj, TFilter> : DefPreFilterBase<TFilter>
    {

        public DefPreFilterBaseFor(Expression<Func<TObj, object>> propName)
            : base(ExtGen.GetMemName(propName))
        {

        }
        public DefPreFilterBaseFor(Func<TFilter, TFilter> func)
            : base(func)
        {

        }

        public DefPreFilterBaseFor(string propName, Func<TFilter, TFilter> func)
            : base(propName, func)
        {

        }

        public DefPreFilterBaseFor(Expression<Func<TObj, object>> propName, Func<TFilter, TFilter> func)
            : base(ExtGen.GetMemName(propName), func)
        {

        }

    }


    public class DefPreFilterDefault : DefFilterInit<object, string>
    {
        public DefPreFilterDefault(Func<object, string> func)
            : base(func)
        {

        }
        public DefPreFilterDefault(string propName)
            : base(propName)
        {

        }
        public DefPreFilterDefault(string propName, Func<object, string> func)
            : base(propName, func)
        {

        }

        public override bool SafeConversion
        {
            get
            {
                return true;
            }
        }



        protected override object ConvertArgBeforeFilter(object value)
        {
            return value;
        }

    }


    public class DefPreFilterDefaultFor<TObj> : DefPreFilterDefault
    {
        public DefPreFilterDefaultFor(Func<object, string> func)
            : base(func)
        {

        }

        public DefPreFilterDefaultFor(Expression<Func<TObj, object>> propName)
            : base(ExtGen.GetMemName(propName))
        {

        }

        public DefPreFilterDefaultFor(Expression<Func<TObj, object>> propName, Func<object, string> func)
            : this(ExtGen.GetMemName(propName), func)
        {

        }

        public DefPreFilterDefaultFor(string propName, Func<object, string> func)
            : base(propName, func)
        {

        }

        //private static Func<object, string> GenericToObjectToStringFunc(Func<T, T> parser)
        //{
        //    return arg =>
        //    {
        //        var argValue = arg.ConvertOrDefault<T>();
        //        var parseResult = parser(argValue);
        //        if (parseResult == null)
        //            return string.Empty;

        //        return parseResult.ToString();
        //    };
        //}

    }


}
