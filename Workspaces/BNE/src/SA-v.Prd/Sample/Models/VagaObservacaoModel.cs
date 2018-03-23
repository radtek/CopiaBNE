using AdminLTE_Application;
using System.Collections.Generic;

namespace Sample.Models
{
    public class VagaObservacaoModel
    {
        public string Observacao { get; set; }
        public AdminLTE_Application.VWVagas vaga { get; set; }
        public IEnumerable<VWVagaObservacao> listaObservacao { get; set; }

    }
}