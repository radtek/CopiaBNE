using System;
using System.Text;
using System.Web.Mvc;

namespace BNE.Web.Vagas.Code.Helpers
{
    public static class MetatagHelper
    {

        #region MetatagType
        public enum MetatagType
        {
            /// <summary>
            /// Used for Title meta tag.
            /// </summary>
            Title,

            /// <summary>
            /// Used for Description and keyword meta tag.
            /// </summary>
            MetaData,

            /// <summary>
            /// Used for noindex, nofollow meta tag.
            /// </summary>
            Robots
        }
        #endregion

        #region Public Methods

        public static MvcMetaTag MetaTag(this HtmlHelper helper, MetatagType metaType)
        {
            return new MvcMetaTag(helper, metaType);
        }

        #endregion

        #region MvcMetaTag
        public sealed class MvcMetaTag
        {
            #region Constructors and Destructors

            public MvcMetaTag(HtmlHelper helper, MetatagType metaType)
            {
                this.Helper = helper;
                this.MetaType = metaType;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets or sets Helper.
            /// </summary>
            private HtmlHelper Helper { get; set; }

            /// <summary>
            /// Gets or sets MetaType.
            /// </summary>
            private MetatagType MetaType { get; set; }

            #endregion

            #region Public Methods

            public MvcHtmlString Render()
            {
                var sb = new StringBuilder();
                if (this.Helper.ViewContext.RouteData.DataTokens.ContainsKey("area"))
                {
                    sb.Append(this.Helper.ViewContext.RequestContext.RouteData.DataTokens["area"].ToString().ToLower());
                    sb.Append('_');
                }
                sb.Append(this.Helper.ViewContext.RequestContext.RouteData.Values["controller"].ToString().ToLower());
                if (this.Helper.ViewContext.RequestContext.RouteData.Values["action"].ToString().ToLower() != "index")
                {
                    sb.Append('_');
                    sb.Append(this.Helper.ViewContext.RequestContext.RouteData.Values["action"].ToString().ToLower());
                }

                var meta = new StringBuilder();
                switch (this.MetaType)
                {
                    case MetatagType.MetaData:
                        meta.AppendLine(String.Format("<meta name=\"description\" content=\"{0}\" />", this.Helper.ViewData["Description"] ?? string.Empty));
                        meta.AppendLine(String.Format("<meta name=\"keywords\" content=\"{0}\" />", this.Helper.ViewData["Keywords"] ?? string.Empty));
                        break;
                    case MetatagType.Robots:
                        meta.AppendLine(String.Format("<meta name=\"robots\" content=\"{0}\" />", this.Helper.ViewData["Robots"] ?? string.Empty));
                        break;
                    case MetatagType.Title:
                        meta.AppendLine((string)(this.Helper.ViewData["Title"] ?? string.Empty));
                        break;
                }

                return new MvcHtmlString(meta.ToString());
            }

            #endregion
        }
        #endregion

    }
}