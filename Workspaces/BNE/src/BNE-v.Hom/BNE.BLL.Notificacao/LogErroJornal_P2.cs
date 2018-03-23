//-- Data: 03/08/2016 17:54
//-- Autor: Gieyson Stelmak

namespace BNE.BLL.Notificacao
{
    public partial class LogErroJornal // Tabela: alerta.log_erro_jornal
    {
        public static void Logar(string mensagem, string codigoCurriculos, string codigoVagas)
        {
            var objLogErroJornal = new LogErroJornal
            {
                DescricaoMensagem = mensagem,
                IdfsCvs = codigoCurriculos,
                IdfsVagas = codigoVagas,
            };

            objLogErroJornal.Save();
        }
    }
}