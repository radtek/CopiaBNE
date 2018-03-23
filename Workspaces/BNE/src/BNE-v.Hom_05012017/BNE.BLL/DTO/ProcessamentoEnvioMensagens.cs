using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.DTO
{
    public class ProcessamentoEnvioMensagens
    {
        public ProcessamentoEnvioMensagens()
        {
            EmailsEnviados = new List<DtoEnvioEmail>();
            SMSEnviados = new List<DtoEnvioSMS>();
        }

        public BLL.Filial Filial { get; set; }
        public UsuarioFilialPerfil UsuarioFilialPerfil { get; set; }
        public int? idMensagemCampanha { get; set; }
        public int? idCampanhaTanque { get; set; }
        public bool CotaDiaEncerrada { get; set; }
        public bool EnviarEmail { get; set; }
        public bool EnviarSMS { get; set; }
        public bool EmpresaPossuiPlano { get; set; }
        public bool UsaSMSTanque { get; set; }
        public int QtdCandidatosSelecionados { get; set; }
        public string EmailComercial { get; set; }
        public string DesMensagemEmail { get; set; }
        public string DesMensagemSMS { get; set; }
        public bool processamentoAssincrono { get; set; }
        public List<DtoEnvioEmail> EmailsEnviados { get; set; }
        public List<DtoEnvioSMS> SMSEnviados { get; set; }
    }

    public class DtoEnvioBase
    {
        public int IdCVDestinatario { get; set; }
        public string NomeDestinatario { get; set; }
    }

    public class DtoEnvioEmail : DtoEnvioBase
    {
        public string EmailDestinatario { get; set; }
    }

    public class DtoEnvioSMS : DtoEnvioBase
    {
        public decimal CelDestinatario { get; set; }
    }
}
