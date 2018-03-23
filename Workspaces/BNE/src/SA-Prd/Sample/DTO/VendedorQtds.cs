using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class VendedorQtds
    {
        public int qtdFinaldePrazo { get; set; }

        public int qtdClientesOnline { get; set; }

        public int qtdClientesnaCarteira { get; set; }

        public int qtdClientesSemPlano { get; set; }

        public int qtdClientesComPlano { get; set; }

        public int qtdNovosClientes { get; set; }

    }
}