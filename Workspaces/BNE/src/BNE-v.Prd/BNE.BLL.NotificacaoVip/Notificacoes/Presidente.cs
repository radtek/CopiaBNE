using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BNE.BLL.NotificacoesVip.DTO;
using BNE.BLL.Common.Helpers;

namespace BNE.BLL.NotificacoesVip.Notificacoes
{
    internal class Presidente : INotificacaoCV
    {
        public bool Notificar(DTO.Curriculo curriculo, short idNotificacao, int idCiclo)
        {
            try
            {
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.criado);

                if(string.IsNullOrEmpty(curriculo.email))
                {
                    NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_SemDados, "CV sem e-mail.");
                    return false;
                }

                var primeiroNome = Common.Helpers.Formatting.RetornarPrimeiroNome(curriculo.nome);
                var mensagem = string.Empty;
                var emailRemetente = "presidente@bne.com.br";
                var objCarta = CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.EmailPresidente);

                //Faz o replace dos parametros do assunto da carta
                var assunto = objCarta.Assunto;

                // Faz o replace dos parâmetros da carta
                mensagem = objCarta.Conteudo.Replace("{Primeiro_Nome}", primeiroNome)
                    .Replace("{Prezado}", curriculo.IdSexo == (int)Enumeradores.Sexo.Masculino ? "Prezado" : "Prezada");

                var onjUsuarioFilialPerfil = new UsuarioFilialPerfil(curriculo.idUsuarioFilialPerfil);
                var objCurriculo = new Curriculo(curriculo.idCurriculo);

                //Envio e-mail
                var email = new Uteis.Email(objCurriculo, onjUsuarioFilialPerfil, assunto, mensagem, emailRemetente, curriculo.email);
                if (!email.Enviar())
                {
                    NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Erro, "Falha no envio do email.");
                    return false;
                }

                //Envio sms
                if (curriculo.numeroCelular.HasValue)
                {
                    var desMensagemSMS = CartaSMS.RecuperaValorConteudo(BLL.Enumeradores.CartaSMS.SmsPresidente);
                    desMensagemSMS = desMensagemSMS.Replace("{Nome}", primeiroNome);

                    var sms = new Uteis.SMS(curriculo.nome, curriculo.numeroCelular.Value, desMensagemSMS, curriculo.idCurriculo);
                    sms.Enviar();
                }

                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Sucesso);

                return true;
            }
            catch (Exception ex)
            {
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Erro);
                EL.GerenciadorException.GravarExcecao(ex, "Falha na notificação do 'Presidente' para o CV " + curriculo.idCurriculo);
                return false;
            }
          
        }
    }
}
