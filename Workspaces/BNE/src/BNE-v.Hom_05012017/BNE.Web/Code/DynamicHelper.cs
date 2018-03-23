using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.CSharp.RuntimeBinder;

namespace BNE.Web.Code
{
    public static class DynamicHelper
    {
        private static readonly Dictionary<KeyValuePair<Type, string>, CallSite<Func<CallSite, object, object>>>
            GetValueCache = new Dictionary<KeyValuePair<Type, string>, CallSite<Func<CallSite, object, object>>>();

        private static readonly Dictionary<KeyValuePair<Type, string>, CallSite<Func<CallSite, object, object, object>>>
            SetValueCache = new Dictionary<KeyValuePair<Type, string>, CallSite<Func<CallSite, object, object, object>>>();

        public static object GetValue(object dyn, string propName)
        {
            return InternalGetValue(dyn, propName);
        }

        private static object InternalGetValue(object dyn, string propName, bool tentarNovamenteAcessoNaPropriedade = true)
        {
            var getterSite = GetOrAdd(GetValueCache, new KeyValuePair<Type, string>(dyn.GetType(), propName),
                                      fact => CallSite<Func<CallSite, object, object>>.Create(
                                          Binder.GetMember(CSharpBinderFlags.None,
                                                           fact.Value,
                                                           fact.Key,
                                                           new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }
                                              )));

            if (!tentarNovamenteAcessoNaPropriedade)
                return getterSite.Target(getterSite, dyn);

            try
            {
                return getterSite.Target(getterSite, dyn);
            }
            catch (RuntimeBinderException)
            {
                if (propName.Any(obj => !char.IsLower(obj)))
                    return InternalGetValue(dyn, propName.ToLower(), false);

                throw;
            }
        }

        public static void SetValue(object dyn, string propName, object val)
        {
            var setterSite = GetOrAdd(SetValueCache, new KeyValuePair<Type, string>(dyn.GetType(), propName), fact => CallSite<Func<CallSite, object, object, object>>.Create(
                Binder.SetMember(CSharpBinderFlags.None,
                    fact.Value,
                    fact.Key,
                    new[]
                    {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant |
                                                  CSharpArgumentInfoFlags.UseCompileTimeType, null)
                    }
                    )));

            setterSite.Target(setterSite, dyn, val);
        }

        private static TValue GetOrAdd<TKey, TValue>(IDictionary<TKey, TValue> dictionary,
            TKey key, Func<TKey, TValue> valueCreator)
        {
            TValue value;
            if (!dictionary.TryGetValue(key, out value))
            {
                value = valueCreator(key);
                dictionary.Add(key, value);
            }
            return value;
        }

    }
}