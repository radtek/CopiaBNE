using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanHouse.Business.DTO
{
    public class FilialOrigemFilial
    {
        public int IdFilial { get; set; }
        public int IdFilialOrigem { get; set; }
        public string NomeFantasia { get; set; }
        public decimal? Cnpj { get; set; }
        public string Diretorio { get; set; }
        public string GeoLocalizacao { get; set; }
        public byte[] logoLan { get; set; }
    }
}