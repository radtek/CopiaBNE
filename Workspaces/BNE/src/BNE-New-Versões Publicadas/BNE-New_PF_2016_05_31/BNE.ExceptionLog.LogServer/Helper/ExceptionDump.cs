using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BNE.ExceptionLog.Model;
using Exception = System.Exception;

namespace BNE.ExceptionLog.LogServer.Helper
{
    public static class ExceptionDump
    {

        public static string DumpException(this TraceLog ex)
        {
            if (ex == null)
                return "Exception is null.";

            var allMsg = new StringBuilder();
            int count = 0;
            TraceLog genericException = ex;

            var avoidsStackOverflow = new List<TraceLog>();
            try
            {
                do
                {
                    var partialMsg = new StringBuilder();
                    count++;
                    partialMsg.Append("[");
                    partialMsg.Append(count);
                    partialMsg.Append("]");

                    if (count > 1)
                        partialMsg.Append(" InnerException Type: ");
                    else
                        partialMsg.Append(" Exception Type: ");

                    partialMsg.Append(genericException.GetType());

                    if (!string.IsNullOrEmpty(genericException.Message))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("Message: ");
                        partialMsg.Append(genericException.Message);
                    }

                    if (!string.IsNullOrEmpty(genericException.Source))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("Source: ");
                        partialMsg.Append(genericException.Source);
                    }

                    if (!string.IsNullOrEmpty(genericException.StackTrace))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("StackTrace: ");
                        partialMsg.Append(genericException.StackTrace);
                    }


                    if (genericException.TargetSite != null)
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("TargetSite: name=");
                        partialMsg.Append(genericException.TargetSite.Name);

                        partialMsg.Append(", type=");
                        partialMsg.Append(genericException.TargetSite.ReflectedType);

                        partialMsg.Append(", assembly=");
                        partialMsg.Append(genericException.TargetSite.ReflectedType.Assembly);
                    }

                    if (!string.IsNullOrEmpty(genericException.HelpLink))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("HelpLink: ");
                        partialMsg.Append(genericException.HelpLink);
                    }

                    if (genericException.Data != null && genericException.Data.Count > 0)
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("Data [");
                        partialMsg.Append(genericException.Data.Count);
                        partialMsg.Append("]: ");
                        partialMsg.AppendLine();

                        foreach (DictionaryEntry item in genericException.Data)
                        {
                            try
                            {
                                partialMsg.AppendFormat("Key='{0}' | Value='{1}'", item.Key, item.Value.ToString());
                                partialMsg.AppendLine();
                            }
                            catch
                            {

                            }
                        }
                    }

                    Type exceptionType = genericException.GetType();
                    while (exceptionType != typeof(Exception) && exceptionType != null && (exceptionType.IsValueType || exceptionType.IsPrimitive))
                    {
                        var otherProperties = exceptionType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        if (otherProperties.Length > 0)
                        {

                            bool writeHeader = false;
                            foreach (var item in otherProperties)
                            {
                                if (!item.CanRead)
                                    continue;

                                var getMethod = item.GetGetMethod(false);
                                if (getMethod == null || getMethod.GetBaseDefinition() != getMethod)
                                    continue;

                                if (!writeHeader)
                                {
                                    partialMsg.AppendLine();
                                    partialMsg.AppendLine();
                                    partialMsg.Append("Other Information:");
                                    partialMsg.AppendLine();
                                    writeHeader = true;
                                }

                                partialMsg.AppendFormat("   PropertyName='{0}'", item.Name).AppendLine();
                                partialMsg.AppendFormat("   Value='{0}'", item.GetValue(genericException, null).ToString());
                                partialMsg.AppendLine();
                            }
                        }

                        exceptionType = exceptionType.BaseType;
                    }

                    if (count != 1)
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                    }
                    allMsg.Insert(0, partialMsg.ToString());

                    if (avoidsStackOverflow.Any(obj => obj == genericException))
                        break;

                    avoidsStackOverflow.Add(genericException);
                    genericException = genericException.InnerException;


                } while (genericException != null);
            }
            catch
            {

            }

            return allMsg.ToString();
        }

    }
}
