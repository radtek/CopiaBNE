using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Collections.Specialized;

namespace BNE.Web.Vagas.Code.Helpers.Paginacao
{
    public class Pager : IHtmlString
    {
        private readonly HtmlHelper htmlHelper;
        private readonly int pageSize;
        private readonly int currentPage;
        private readonly int totalItemCount;
        private readonly PagerOptions pagerOptions;
        private readonly NameValueCollection QueryString;

        public Pager(HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, NameValueCollection QueryString)
        {
            this.htmlHelper = htmlHelper;
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.totalItemCount = totalItemCount;
            this.pagerOptions = new PagerOptions();
            this.QueryString = QueryString;
        }

        public Pager Options(Action<PagerOptionsBuilder> buildOptions)
        {
            buildOptions(new PagerOptionsBuilder(this.pagerOptions));
            return this;
        }

        public virtual PaginationModel BuildPaginationModel(Func<int, string> generateUrl)
        {
            var pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            var model = new PaginationModel { PageSize = this.pageSize, CurrentPage = this.currentPage, TotalItemCount = this.totalItemCount, PageCount = pageCount };

			if (currentPage > 1)
			{
				//First
				model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = 0, DisplayText = "<i class='current fa fa-fast-backward'></i>", Url = generateUrl(1), IsInput = false, Relation = "nofollow", TitleText = "Primeira Página", Class = "input" });

				//Previous
				model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = currentPage - 1, DisplayText = "<i class='current fa fa-backward'></i>", IsInput = false, Url = generateUrl(currentPage - 1), Relation = "prev", TitleText = "Página Anterior", Class = "input" });
			}

            var start = 1;
            var end = pageCount;
            var nrOfPagesToDisplay = this.pagerOptions.MaxNrOfPages;

            if (pageCount > nrOfPagesToDisplay)
            {
                var middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
                var below = (currentPage - middle);
                var above = (currentPage + middle);

                if (below < 2)
                {
                    above = nrOfPagesToDisplay;
                    below = 1;
                }
                else if (above > (pageCount - 2))
                {
                    above = pageCount;
                    below = (pageCount - nrOfPagesToDisplay + 1);
                }

                start = below;
                end = above;
            }

            if (start > 1)
            {
                model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = 1, DisplayText = "1", Url = generateUrl(1) });
                if (start > 3)
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = 2, DisplayText = "2", Url = generateUrl(2) });
                }
                if (start > 2)
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = false, DisplayText = "...", IsSpacer = true });
                }
            }

            for (var i = start; i <= end; i++)
            {
                if (i == currentPage || (currentPage <= 0 && i == 0))
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = i, IsCurrent = true, DisplayText = i.ToString(), Relation = "nofollow", Class = "pagina" });
                }
                else
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = i, DisplayText = i.ToString(), Url = generateUrl(i), Relation = "nofollow", Class = "pagina" });
                }
            }
            if (end < pageCount)
            {
                if (end < pageCount - 1)
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = false, DisplayText = "...", IsSpacer = true });
                }
                if (pageCount - 2 > end)
                {
                    model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = pageCount - 1, DisplayText = (pageCount - 1).ToString(), Url = generateUrl(pageCount - 1) });
                }

                model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = pageCount, DisplayText = pageCount.ToString(), Url = generateUrl(pageCount) });
            }
            
            if (currentPage < pageCount)
            {
                // Next
                model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = currentPage + 1, IsInput = false, DisplayText = "<i class='current fa fa-forward'></i>", Url = generateUrl(currentPage + 1), Relation = "nofollow", TitleText = "Próxima Página", Class = "input" });
                // Last
                model.PaginationLinks.Add(new PaginationLink { Active = true, PageIndex = pageCount - 1, IsInput = false, DisplayText = "<i class='current fa fa-fast-forward'></i>", Url = generateUrl(pageCount), Relation = "nofollow", TitleText = "Última Página", Class = "input" });

            }

            // AjaxOptions
            if (pagerOptions.AjaxOptions != null)
            {
                model.AjaxOptions = pagerOptions.AjaxOptions;
            }

            model.Options = pagerOptions;
            return model;
        }

        public virtual string ToHtmlString()
        {
            var model = BuildPaginationModel(GeneratePageUrl);

            if (!String.IsNullOrEmpty(this.pagerOptions.DisplayTemplate))
            {
                var templatePath = string.Format("DisplayTemplates/{0}", this.pagerOptions.DisplayTemplate);
                return htmlHelper.Partial(templatePath, model).ToHtmlString();
            }
            else
            {
                var sb = new StringBuilder();

                foreach (var paginationLink in model.PaginationLinks)
                {
                    if (paginationLink.Active)
                    {
                        if (paginationLink.IsCurrent)
                        {
                            sb.AppendFormat("<span class=\"current\">{0}</span>", paginationLink.DisplayText);
                        }
                        else if (!paginationLink.PageIndex.HasValue)
                        {
                            sb.AppendFormat(paginationLink.DisplayText);
                        }
                        else
                        {
                            var linkBuilder = new StringBuilder();

                            linkBuilder = new StringBuilder("<a");
                            if (pagerOptions.AjaxOptions != null)
                                foreach (var ajaxOption in pagerOptions.AjaxOptions.ToUnobtrusiveHtmlAttributes())
                                    linkBuilder.AppendFormat(" {0}=\"{1}\"", ajaxOption.Key, ajaxOption.Value);

                            if (paginationLink.IsInput)
                            {
                                linkBuilder.AppendFormat(" href=\"{0}\" title=\"{1}\" rel=\"{2}\" class=\"{3}\">{4}</a>", paginationLink.Url, paginationLink.TitleText, paginationLink.Relation, paginationLink.Class, string.Format("<img src=\"{0}\" alt=\"{1}\">", paginationLink.Href, paginationLink.TitleText)); //string.Format("<input type=\"image\" src=\"{0}\">", paginationLink.Href));
                            }
                            else
                            {
                                linkBuilder.AppendFormat(" href=\"{0}\" title=\"{1}\" rel=\"{2}\" class=\"{3}\">{4}</a>", paginationLink.Url, paginationLink.TitleText, paginationLink.Relation, paginationLink.Class, paginationLink.DisplayText);
                            }

                            sb.Append(linkBuilder.ToString());
                        }
                    }
                    else
                    {
                        if (!paginationLink.IsSpacer)
                        {
                            if (paginationLink.IsInput)
                                sb.AppendFormat("<span class=\"disabled\">{0}</span>", string.Format("<span>{0}</span>", paginationLink.Href, paginationLink.TitleText)); //string.Format("<input type=\"image\" src=\"{0}\" style=\"cursor:default;\">", paginationLink.Href));
                            else
                                sb.AppendFormat("<span class=\"disabled\">{0}</span>", paginationLink.DisplayText);
                        }
                        else
                        {
                            sb.Append(paginationLink.DisplayText);
                        }
                    }
                }
                return sb.ToString();
            }
        }

        protected virtual string GeneratePageUrl(int pageNumber)
        {
            var viewContext = this.htmlHelper.ViewContext;
            var routeDataValues = viewContext.RequestContext.RouteData.Values;
            RouteValueDictionary pageLinkValueDictionary;
            // Avoid canonical errors when pageNumber is equal to 1.
            if (pageNumber == 1 && !this.pagerOptions.AlwaysAddFirstPageNumber)
            {
                pageLinkValueDictionary = new RouteValueDictionary(this.pagerOptions.RouteValues);
                if (routeDataValues.ContainsKey(this.pagerOptions.PageRouteValueKey))
                {
                    routeDataValues.Remove(this.pagerOptions.PageRouteValueKey);
                }
            }
            else
            {
                pageLinkValueDictionary = new RouteValueDictionary(this.pagerOptions.RouteValues);
            }

            // To be sure we get the right route, ensure the controller and action are specified.
            if (!pageLinkValueDictionary.ContainsKey("controller") && routeDataValues.ContainsKey("controller"))
            {
                pageLinkValueDictionary.Add("controller", routeDataValues["controller"]);
            }
            if (!pageLinkValueDictionary.ContainsKey("action") && routeDataValues.ContainsKey("action"))
            {
                pageLinkValueDictionary.Add("action", routeDataValues["action"]);
            }

            // Fix the dictionary if there are arrays in it.
            pageLinkValueDictionary = pageLinkValueDictionary.FixListRouteDataValues();

            // 'Render' virtual path.
            var virtualPathForArea = RouteTable.Routes.GetVirtualPathForArea(viewContext.RequestContext, pageLinkValueDictionary);
#if !DEBUG
            //não gerar links com vaga de emprego duplicados em prd
            virtualPathForArea.VirtualPath = virtualPathForArea.VirtualPath.Replace("/vagas-de-emprego","");
#endif

            virtualPathForArea.VirtualPath = virtualPathForArea.VirtualPath.Replace(' ', '-');

            // Adding 'pagina' to query string
            virtualPathForArea.VirtualPath += "?pagina=" + pageNumber.ToString();

            foreach (string key in QueryString)
	        {
                if (key != "pagina")
                    virtualPathForArea.VirtualPath += "&" + key + "=" + QueryString[key];
	        }

            return virtualPathForArea == null ? null : virtualPathForArea.VirtualPath;
        }
    }
}