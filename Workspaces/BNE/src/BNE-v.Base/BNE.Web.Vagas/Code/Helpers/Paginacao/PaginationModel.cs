﻿using System.Collections.Generic;
using System.Web.Mvc.Ajax;

namespace BNE.Web.Vagas.Code.Helpers.Paginacao
{
    public class PaginationModel
    {
        public int PageSize { get; internal set; }
        public int CurrentPage { get; internal set; }
        public int PageCount { get; internal set; }
        public int TotalItemCount { get; internal set; }
        public IList<PaginationLink> PaginationLinks { get; private set; }
        public AjaxOptions AjaxOptions { get; internal set; }
        public PagerOptions Options { get; internal set; }

        public PaginationModel()
        {
            PaginationLinks = new List<PaginationLink>();
            AjaxOptions = null;
            Options = null;
        }
    }

    public class PaginationLink
    {
        public bool Active { get; set; }
        public bool IsCurrent { get; set; }
        public int? PageIndex { get; set; }
        public string DisplayText { get; set; }
        public string Url { get; set; }
        public bool IsSpacer { get; set; }
        public bool IsInput { get; set; }
        public string Href { get; set; }
        public string Relation { get; set; }
        public string TitleText { get; set; }
        public string Class { get; set; }
    }
}