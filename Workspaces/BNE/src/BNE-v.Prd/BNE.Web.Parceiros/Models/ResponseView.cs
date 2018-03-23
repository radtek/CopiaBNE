using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Parceiros.Models
{
    public class ResponseView
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public Cadastro Model { get; set; }
    }
}