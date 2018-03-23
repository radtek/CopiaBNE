using System;

namespace BNE.Web.Vagas.Models
{
    [Serializable]
    public class ExperienciaProfissional
    {
        public int Identificador { get; set; }
        public string RazSocial {get; set; }
        public string DesAtividade { get; set; }
        public DateTime DtaCadastro { get; set; }
        public int IdPerguntaHistoricoExp { get; set; }

        public int IdentificadorVaga { get; set; }


    }
}