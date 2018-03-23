using System;

namespace BNE.PessoaJuridica.ApplicationService.PessoaJuridica.View
{
    public class CadastroEmpresaView
    {
        public string LinkAcesso { get; set; }
        public string LinkPlanoFree { get; set; }
        public string LinkPlanoPago { get; set; }
        public bool Click2Call { get; set; }
        public string NumeroTelefone { get; set; }
        public string Nome { get; set; }
        public bool EmpresaJaCadastrada { get; set; }
        public decimal ValorPlanoPagoDe { get; set; }
        public decimal ValorPlanoPagoPara { get; set; }
        public Exception Error { get; set; }
    }
}