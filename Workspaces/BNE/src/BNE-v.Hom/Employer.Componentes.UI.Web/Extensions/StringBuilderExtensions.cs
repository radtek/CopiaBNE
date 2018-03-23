using System;
using System.Text;

namespace Employer.Componentes.UI.Web.Extensions
{
    /// <summary>
    /// Extensões para o StringBuilder
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Adiciona uma linha a um StringBuilder colocando um prefixo "and" na existência
        /// de outros ítens no StringBuilder
        /// </summary>
        /// <param name="builder">O StringBuilder a ser extendido</param>
        /// <param name="value">O valor a ser inserido no StringBuilder</param>
        public static void AppendLineAnd(this StringBuilder builder, String value)
        {
            if (builder.Length == 0)
                builder.AppendLine(value);
            else
                builder.AppendLine(" and " + value);
        }
        /// <summary>
        /// Concatena um valor a um StringBuilder colocando um prefixo "and" na existência
        /// de outros ítens no StringBuilder
        /// </summary>
        /// <param name="builder">O StringBuilder a ser extendido</param>
        /// <param name="value">O valor a ser inserido no StringBuilder</param>
        public static void AppendAnd(this StringBuilder builder, String value)
        {
            if (builder.Length == 0)
                builder.Append(value);
            else
                builder.Append(" and " + value);
        }

        /// <summary>
        /// Adiciona uma linha a um StringBuilder colocando um prefixo "or" na existência
        /// de outros ítens no StringBuilder
        /// </summary>
        /// <param name="builder">O StringBuilder a ser extendido</param>
        /// <param name="value">O valor a ser inserido no StringBuilder</param>
        public static void AppendLineOr(this StringBuilder builder, String value)
        {
            if (builder.Length == 0)
                builder.AppendLine(value);
            else
                builder.AppendLine(" or " + value);
        }
        /// <summary>
        /// Concatena um valor a um StringBuilder colocando um prefixo "or" na existência
        /// de outros ítens no StringBuilder
        /// </summary>
        /// <param name="builder">O StringBuilder a ser extendido</param>
        /// <param name="value">O valor a ser inserido no StringBuilder</param>
        public static void AppendOr(this StringBuilder builder, String value)
        {
            if (builder.Length == 0)
                builder.Append(value);
            else
                builder.Append(" or " + value);
        }
        /// <summary>
        /// Concatena um valor a um StringBuilder colocando uma virgula entre os ítens. 
        /// </summary>
        /// <param name="builder">O StringBuilder a ser extendido</param>
        /// <param name="value">O valor a ser inserido no StringBuilder</param>
        public static void AppendComa(this StringBuilder builder, String value)
        {
            if (builder.Length == 0)
                builder.Append(value);
            else
                builder.Append("," + value);
        }
        /// <summary>
        /// Minimiza um texto excluindo espaços duplos, enters, tabs e finais de linha. Usado para otmização de Javascript.
        /// Obervações:
        /// Esse método utiliza internamente as extensões da classe String definidas em Employer.Componentes.UI.Web.Extensions.StringExtensions
        /// e está sujeito as suas limitações
        /// </summary>
        /// <param name="builder">O StringBuilder a ser extendido</param>
        /// <returns>O texto minimizado</returns>
        public static String Minify(this StringBuilder builder)
        {
            return builder.ToString().Minify();
        }

        /// <summary>
        /// Retorna o texto javasript minimizado contendo as tags script
        /// Esse método utiliza internamente as extensões da classe String definidas em Employer.Componentes.UI.Web.Extensions.StringExtensions
        /// e está sujeito as suas limitações
        /// </summary>
        /// <param name="builder">O StringBuilder a ser extendido</param>
        /// <returns>O texto javascript</returns>
        public static String ToJavascript(this StringBuilder builder)
        {
            return builder.ToString().ToJavascript();
        }
    }
}
