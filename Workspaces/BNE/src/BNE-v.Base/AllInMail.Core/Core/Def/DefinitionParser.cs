﻿using AllInMail.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllInMail.Core
{
    public class DefPosParser : DefinitionParser<string>
    {
        public DefPosParser(Func<string, string> parser)
            : base(parser)
        {

        }

    }
    public class DefinitionParser<T> : DefPreParser
    {
        public DefinitionParser(Func<T, string> parser)
            : base(typeof(T), GenericToObjectFunc(parser))
        {

        }

        private static Func<object, string> GenericToObjectFunc(Func<T, string> parser)
        {
            return (arg =>
                {
                    return  parser(arg.ConvertOrDefaultOrEmpty<T>());
                });
        }

    }
    public class DefPreParser
    {
        private readonly Func<object, string> _func;
        public DefPreParser(Type target, Func<object, string> func)
        {
            this.TargetType = target;
            this._func = func;
        }
        public Type TargetType { get; set; }

        public string DoParse(object value)
        {
            return _func(value);
        }

    }
}
