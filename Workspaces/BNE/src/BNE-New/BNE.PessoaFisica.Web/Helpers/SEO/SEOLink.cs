using System;
using System.Web.Mvc;

namespace BNE.PessoaFisica.Web.Helpers.SEO
{
    public sealed class SEOLink
    {
        private RelAttibuteValues _rel = RelAttibuteValues.none;

        public string Descricao { get; private set; }
        public string URL { get; private set; }
        public string Title { get; private set; }
        public RelAttibuteValues Rel { get; set; }

        public SEOLink() { }
        public SEOLink(string descricao, string title, string url)
        {
            Descricao = descricao;
            URL = url;
            Title = title;
        }

        public MvcHtmlString GetLink()
        {
            if (_rel == RelAttibuteValues.none)
                return new MvcHtmlString(String.Format("<a href=\"{0}\" title=\"{1}\" class='link'>{2}</a>", URL, Title.Replace("\"", "'"), Descricao.Replace("\"", "'")));

            return new MvcHtmlString(String.Format("<a href=\"{0}\" title=\"{1}\" rel=\"{2}\" class=\"link\">{3}</a>", URL, Title.Replace("\"", "'"), _rel.ToString(), Descricao.Replace("\"", "'")));
        }
    }
}