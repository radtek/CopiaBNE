using BNE.BLL.Common;
using BNE.BLL.Common.Helpers;
using System;

namespace BNE.BLL.NotificacoesVip.Notificacoes
{
    internal class MediaSalarial : INotificacaoCV
    {
        public bool Notificar(DTO.Curriculo curriculo, short idNotificacao, int idCiclo)
        {
            try
            {
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.criado);
              
                if (!string.IsNullOrEmpty(curriculo.email))
                {
                    var mensagem = string.Empty;
                    var emailRemetente = "atendimento@bne.com.br";
                    var objCarta =  CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.MediaSalarial);
                    var assunto = string.Empty;

                    //Url Login Sala VIP
                    var link = LoginAutomatico.GerarHashAcessoLogin(curriculo.numeroCPF,
                        curriculo.dataNascimento,
                        "/" + Rota.RecuperarURLRota(BNE.BLL.Enumeradores.RouteCollection.SalaVIP));

                    //Faz o replace dos parametros do assunto da carta
                    assunto = objCarta.Assunto.Replace("{funcao}",curriculo.funcao);

                    // Faz o replace dos parâmetros da carta
                    mensagem = objCarta.Conteudo.Replace("{Primeiro_Nome}", Formatting.RetornarPrimeiroNome(curriculo.nome))
                                                .Replace("{funcao}", curriculo.funcao)
                                                .Replace("{link}", link)
                                                .Replace("{cidade}",$"{curriculo.cidade}/{curriculo.siglaEstado}");

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
                        var desMensagemSMS = CartaSMS.RecuperaValorConteudo(BLL.Enumeradores.CartaSMS.MediaSalarial);                 
                        var sms = new Uteis.SMS(curriculo.nome, curriculo.numeroCelular.Value, desMensagemSMS, curriculo.idCurriculo);
                        sms.Enviar();
                    }
                }

                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Sucesso);

                return true;
            }
            catch (Exception ex)
            {
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Erro);
                EL.GerenciadorException.GravarExcecao(ex, "Falha na notificação de 'Media Salarial' para o CV " + curriculo.idCurriculo);
                return false;
            }
        }
    }
}
