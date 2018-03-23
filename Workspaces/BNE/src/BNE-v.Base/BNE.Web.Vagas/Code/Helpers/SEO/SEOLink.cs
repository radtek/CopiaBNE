using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BNE.Web.Vagas.Code.Helpers.SEO
{
    public class SEOLink
    {
        private RelAttibuteValues _rel = RelAttibuteValues.none;

        public string Descricao { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public RelAttibuteValues Rel { get; set; }

        public MvcHtmlString GetLink()
        {
            if (_rel == RelAttibuteValues.none)
                return new MvcHtmlString(String.Format("<a href=\"{0}\" title=\"{1}\">{2}</a>", URL, Title, Descricao));

            return new MvcHtmlString(String.Format("<a href=\"{0}\" title=\"{1}\" rel=\"{2}\">{3}</a>", URL, Title, _rel.ToString(), Descricao));
        }
    }
}