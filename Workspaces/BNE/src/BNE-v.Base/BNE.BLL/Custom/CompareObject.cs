using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BNE.BLL.Custom
{
    public static class CompareObject
    {
        public static string ToStringNullSafe(this object obj)
        {
            return obj != null ? obj.ToString().Trim() : String.Empty;
        }
        public static bool Compare<T>(T a, T b, params string[] ignore)
        {
            int count = a.GetType().GetProperties().Count();
            for (int i = 0; i < count; i++)
            {
                string aa = a.GetType().GetProperties()[i].GetValue(a, null).ToStringNullSafe();
                string bb = b.GetType().GetProperties()[i].GetValue(b, null).ToStringNullSafe();
                if (aa != bb && ignore.Where(x => x == a.GetType().GetProperties()[i].Name).Count() == 0)
                    return false;
            }
            return true;
        }
        public static List<CompareResult> CompareList<T>(T a, T b, params string[] ignore)
        {
            var list = new List<CompareResult>();
            int count = a.GetType().GetProperties().Count();
            for (int i = 0; i < count; i++)
            {

                string aa = a.GetType().GetProperties()[i].GetValue(a, null).ToStringNullSafe();
                string bb = b.GetType().GetProperties()[i].GetValue(b, null).ToStringNullSafe();
                if (aa != bb && ignore.Where(x => x == a.GetType().GetProperties()[i].Name).Count() == 0)
                {
                    if (aa.Equals("False"))
                        aa = "Falso";

                    if (bb.Equals("False"))
                        bb = "Falso";

                    if (aa.Equals("True"))
                        aa = "Verdadeiro";

                    if (bb.Equals("True"))
                        bb = "Verdadeiro";

                    list.Add(new CompareResult(a.GetType().GetProperties()[i], aa, bb));
                }
            }
            return list;
        }

        public struct CompareResult
        {
            public readonly MemberInfo Member;
            public readonly object Value1, Value2;
            public CompareResult(MemberInfo member, object value1, object value2)
            {
                Member = member;
                Value1 = value1;
                Value2 = value2;
            }
            public override string ToString()
            {
                return Member.Name + ": " + Value1 + (Value1.Equals(Value2) ? " == " : " -> ") + Value2;
            }
        }
    }
}