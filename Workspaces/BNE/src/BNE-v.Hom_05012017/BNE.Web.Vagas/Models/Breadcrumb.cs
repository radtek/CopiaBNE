using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BNE.Web.Vagas.Models
{
    public class Breadcrumb
    {
        public bool exibeBreadcrumb { get; set; }
        public string links { get; set; }
        public string nomePagina { get; set; }
    }
}
