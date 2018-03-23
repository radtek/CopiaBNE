using System.Web.Mvc;

namespace BNE.Components.Web
{
    public static class ControlInput
    {

        /// <summary>
        /// Builds the type of the input using an MVC TagBuilder.
        /// </summary>
        /// <param name="name">The name of the Input type.</param>
        /// <param name="inputType">Type of the input.</param>
        /// <returns>A Tagbuilder.</returns>
        internal static TagBuilder BuildInputType(string name, string inputType)
        {
            TagBuilder builder = new TagBuilder("input");
            builder.MergeAttribute("type", inputType);
            if (name != null)
            {
                builder.MergeAttribute("name", name);
            }

            return builder;
        }

    }
}
