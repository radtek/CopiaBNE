using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Models
{
    public class CampanhaModel
    {
        public List<CampanhaEmpresaModel> ListaEmpresa { get; set; }
        public string cnpjs { get; set; }
        public string cnjpsJson { get; set; }
        
    }

    public class CampanhaEmpresaModel
    {
        public decimal num_cnpj { get; set; }
        public string Raz_Social { get; set; }
        public string Eml_Filial { get; set; }
        public string Nme_Usuario { get; set; }
        public string Nme_Usuario_Primeiro { get; set; }
    }
}