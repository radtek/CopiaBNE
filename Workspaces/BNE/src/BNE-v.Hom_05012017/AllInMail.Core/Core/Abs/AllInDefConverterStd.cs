using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AllInMail.Helper;

namespace AllInMail.Core
{
    public abstract class AllInDefConverterStd<T> : AllInDefConverterBase<T>
    {
        readonly Lazy<DefPosFilter[]> _posFilters;
        readonly Lazy<DefPreParser[]> _posParsers;
        readonly Lazy<PropertyInfo[]> _modelProperties;
        readonly Lazy<DefPreParser[]> _preParsers;
        readonly Lazy<IDefFilter[]> _initFilters;
        readonly Lazy<IDefFilter[]> _preFilters;

        public AllInDefConverterStd()
        {
            _initFilters = new Lazy<IDefFilter[]>(BuildInitFilters);
            _preParsers = new Lazy<DefPreParser[]>(BuildPreParsers);
            _preFilters = new Lazy<IDefFilter[]>(BuildPreFilters);
            _posFilters = new Lazy<DefPosFilter[]>(BuildPosFilters);
            _posParsers = new Lazy<DefPreParser[]>(BuildPosParsers);
            _modelProperties = new Lazy<PropertyInfo[]>(BuildModelProperties);
        }

        protected abstract IEnumerable<IDefFilter> InitTreatmentFilters();
        protected abstract IEnumerable<DefPreParser> PreParsers();
        protected abstract IEnumerable<DefFilterInit<object, string>> IntermediateFilters();
        protected abstract IEnumerable<DefPosParser> PosParsers();
        protected abstract IEnumerable<DefPosFilter> PosFilters();

        public override string Parse(T model)
        {
            var fields = GetDefiniedFields();

            var allProperties = _modelProperties.Value;
            var initFilters = _initFilters.Value;
            var preParsers = _preParsers.Value;
            var preFilters = _preFilters.Value;
            var posParsers = _posParsers.Value;
            var posFilters = _posFilters.Value;

            StringBuilder builder = new StringBuilder();
            bool started = false;
            foreach (var item in fields)
            {
                if (started)
                    builder.Append(Delimiter);

                started = true;

                var prop = allProperties.BinarySearchFirst(b => b.Name, item, StringComparer.OrdinalIgnoreCase) ??
                                                            allProperties.BinarySearchFirst(b => b.Name.Replace("_", " "), item.Replace("_", ""), StringComparer.OrdinalIgnoreCase);
                if (prop == null)
                    throw new NullReferenceException(string.Format("prop='{0}' not found", item));

                var value = prop.GetValue(model, null);

                if (value == null)
                    continue;

                value = ApplyBehaviors(initFilters, preParsers, preFilters, posParsers, posFilters, prop.Name, value);
                builder.Append(value);
            }
            return builder.ToString();
        }

        private static System.Reflection.PropertyInfo[] BuildModelProperties()
        {
            var allProperties = typeof(T).GetProperties().OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase).ToArray();
            return allProperties;
        }

        private DefPosFilter[] BuildPosFilters()
        {
            var posFilters = PosFilters().OrderBy(a => a.TargetProperty, StringComparer.OrdinalIgnoreCase).ToArray();
            return posFilters;
        }

        private DefPosParser[] BuildPosParsers()
        {
            var posParsers = PosParsers().ToArray();
            return posParsers;
        }

        private IDefFilter[] BuildPreFilters()
        {
            var preFilters = IntermediateFilters().OrderBy(a => a.TargetProperty, StringComparer.OrdinalIgnoreCase).ToArray();
            return preFilters;
        }

        private DefPreParser[] BuildPreParsers()
        {
            var preParsers = PreParsers().OrderBy(a => a.TargetType.FullName, StringComparer.OrdinalIgnoreCase).ToArray();
            return preParsers;
        }

        private IDefFilter[] BuildInitFilters()
        {
            var initFilters = InitTreatmentFilters().OrderBy(a => a.TargetProperty, StringComparer.OrdinalIgnoreCase).ToArray();
            return initFilters;
        }

        private object ApplyBehaviors(IDefFilter[] initFilter, DefPreParser[] preParsers, IDefFilter[] preFilters, DefPreParser[] posParsers, DefPosFilter[] posFilters, string propertyName, object value)
        {
            var toInitFilter = initFilter.BinarySearchMany(a => a.TargetProperty, propertyName, StringComparer.OrdinalIgnoreCase);
            foreach (var filter in toInitFilter)
            {
                value = filter.DoFilter(value);
            }

            var toPreParse = preParsers.BinarySearchMany(a => a.TargetType.FullName, value.GetType().FullName);
            foreach (var parser in toPreParse)
            {
                value = parser.DoParse(value);
            }

            var toPreFilter = preFilters.BinarySearchMany(b => b.TargetProperty, propertyName, StringComparer.OrdinalIgnoreCase);
            foreach (var filter in toPreFilter)
            {
                value = filter.DoFilter(value);
            }

            if (!(value is string))
            {
                value = value.ConvertOrDefault<string>();
            }

            foreach (var parser in posParsers)
            {
                value = parser.DoParse(value);
            }

            var toPosFilter = posFilters.BinarySearchMany(b => b.TargetProperty, propertyName, StringComparer.OrdinalIgnoreCase);
            foreach (var filter in toPosFilter)
            {
                value = filter.DoFilter((string)value);
            }

            return value;
        }
    }
}
