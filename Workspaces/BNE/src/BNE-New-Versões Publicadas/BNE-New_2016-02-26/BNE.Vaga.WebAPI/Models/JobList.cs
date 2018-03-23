using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Vaga.WebAPI.Models
{
    public class JobList
    {
        /// <summary>
        /// Página da lista
        /// </summary>
        public int Page;
    }

    public class Pagination<TObject> where TObject : class
    {
        /// <summary>
        /// Página da lista
        /// </summary>
        public int Page;

        public List<TObject> Results;
    }
}