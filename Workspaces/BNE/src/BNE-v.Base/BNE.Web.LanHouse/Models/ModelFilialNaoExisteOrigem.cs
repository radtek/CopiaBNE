using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.LanHouse.Models
{
    public class ModelFilialNaoExisteOrigem
    {
        public ModelFilialNaoExisteOrigem(string diretorio)
        {
            Diretorio = diretorio;
        }

        public string Diretorio { get; private set; }
    }
}