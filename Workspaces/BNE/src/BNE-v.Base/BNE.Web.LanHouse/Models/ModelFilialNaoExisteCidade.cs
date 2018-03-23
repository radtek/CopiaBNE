using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.LanHouse.Models
{
    public class ModelFilialNaoExisteCidade
    {
        public string Diretorio { get; private set; }

        public ModelFilialNaoExisteCidade(string diretorio)
        {
            Diretorio = diretorio;
        }
    }
}