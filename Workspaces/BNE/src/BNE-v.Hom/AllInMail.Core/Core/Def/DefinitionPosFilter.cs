using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AllInMail.Core;
using AllInMail.Helper;

namespace AllInMail
{
    public class DefFilterDefault<TArg> : DefFilterInit<TArg, string>
    {
        protected DefFilterDefault()
        {

        }

        public DefFilterDefault(Func<TArg, string> func)
            : base(func)
        {
        }

        public DefFilterDefault(string prop, Func<TArg, string> func)
            : base(prop, func)
        {


        }

        public override bool SafeConversion
        {
            get { return true; }
        }

        protected override TArg ConvertArgBeforeFilter(object value)
        {
            return value.ConvertOrDefault<TArg>();
        }
    }

    public class DefPosFilter : DefFilterDefault<string>
    {
        protected DefPosFilter()
        {

        }

        public DefPosFilter(Func<string, string> func)
            : base(func)
        {
        }

        public DefPosFilter(string prop, Func<string, string> func)
            : base(prop, func)
        {

        }

        public DefPosFilter Apply(Func<string, string> other)
        {
            base.Override(other);
            return this;
        }
    }

    public class DefPosFilterFor<T> : DefPosFilter
    {
        public DefPosFilterFor(Func<string> propertyNameAccessor, Func<string, string> func)
            : base(func)
        {
            this.For(propertyNameAccessor);
        }

        public DefPosFilterFor(Expression<Func<T, object>> propertyNameAccessor, Func<string, string> func)
            : base(func)
        {
            this.For(() => Helper.ExtGen.GetMemName<T>(propertyNameAccessor));
        }
        public DefPosFilterFor(Expression<Func<T, object>> propertyNameAccessor)
        {
            this.For(() => Helper.ExtGen.GetMemName<T>(propertyNameAccessor));
        }
        public DefPosFilterFor(string prop)
        {
            this.For(() => prop);
        }
        public DefPosFilterFor(string prop, Func<string, string> func)
            : base(prop, func)
        {

        }

        public DefPosFilterFor(Func<string, string> func)
            : base(func)
        {

        }

        public DefPosFilterFor<T> For(Expression<Func<T, object>> propertyNameAccessor)
        {
            this.For(() => Helper.ExtGen.GetMemName(propertyNameAccessor));
            return this;
        }
    }

    public static class DefinitionPosFilterHelper
    {
        public static DefPosFilterFor<T> UseTooWith<T>(this DefPosFilterFor<T> current, Func<string, string> other)
        {
            var copy = current.CopyWith(other);
            return new DefPosFilterFor<T>(() => current.TargetProperty, (a) => copy.DoFilter(current.DoFilter(a)));
        }

        public static DefPosFilterFor<T> UseWith<T>(this DefPosFilterFor<T> current, Func<string, string> other)
        {
            current.Apply(other);
            return current;
        }

    }
}
