using AdminLTE_Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Models.Empresa
{
    public class GridEmpresaComPlano
    {
        public string sortOrder { get; set; }
        public string currentFilter { get; set; }
        public string SearchString { get; set; }
        public int? page { get; set; }
        public string orderby { get; set; }
        public int PageSize { get; set; }
        public int currentFilterSituacao { get; set; }
        public string CurrentSort { get; set; }
        public string PageNumber { get; set; }
        public List<string> listSearchOrder { get; set; }
        public PagedList.IPagedList<VW_EMPRESA_COM_PLANO> Empresas { get; set; }

    }
}