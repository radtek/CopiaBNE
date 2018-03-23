using BNE.BLL.Common;
using System;

namespace BNE.BLL.NotificacoesVip.Notificacoes
{
    internal class VoceNaFrente : INotificacaoCV
    {
        #region Notificar
        public bool Notificar(DTO.Curriculo curriculo, short idNotificacao, int idCiclo)
        {
            try
            {
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.criado);

                //Efetua o envio do informativo da vez
                var objUsuarioFilialPerfil = new UsuarioFilialPerfil(curriculo.idUsuarioFilialPerfil);
                var objCurriculo = new Curriculo(curriculo.idCurriculo);

                var numPessoasNaFrente = Curriculo.CarregarQtdCvNaFrente(curriculo.idCurriculo);
                var qtdPessoasNaFrente = numPessoasNaFrente > 0 ? numPessoasNaFrente.ToString() : "várias";

                var assunto = string.Empty;
                var mensagem = string.Empty;
                var emailDestinatario = curriculo.email;
                var emailRemetente = "atendimento@bne.com.br";
                var objCarta = CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.VipVoceNaFrenteDePessoas);
                var link = LoginAutomatico.GerarHashAcessoLogin(curriculo.numeroCPF, curriculo.dataNascimento, "/" + Rota.RecuperarURLRota(BNE.BLL.Enumeradores.RouteCollection.SalaVIP));

                assunto = objCarta.Assunto;
                assunto = assunto.Replace("{QtdPessoasNaFrente}", qtdPessoasNaFrente);

                mensagem = objCarta.Conteudo.Replace("{Primeiro_Nome}", curriculo.nome);
                mensagem = mensagem.Replace("{QtdPessoasNaFrente}", qtdPessoasNaFrente);
                mensagem = mensagem.Replace("{link}", link);

                var email = new Uteis.Email(objCurriculo, objUsuarioFilialPerfil, assunto, mensagem, emailRemetente, emailDestinatario);
                if (!email.Enviar())
                {
                    NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Erro, "Falha no envio do email.");
                    return false;
                }

                if (curriculo.numeroCelular.HasValue)
                {
                    var desMensagemSMS = CartaSMS.RecuperaValorConteudo(BLL.Enumeradores.CartaSMS.VipVoceNaFrente);
                    desMensagemSMS = desMensagemSMS.Replace("{Nome}", curriculo.nome);
                    desMensagemSMS = desMensagemSMS.Replace("{QtdPessoasNaFrente}", qtdPessoasNaFrente);
                    var sms = new Uteis.SMS(curriculo.nome, curriculo.numeroCelular.Value, desMensagemSMS, curriculo.idCurriculo);
                    sms.Enviar();
                }

                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Sucesso);
                return true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha na notificação de 'voce na frente' para o CV " + curriculo.idCurriculo);
                return false;
            }
        }
        #endregion
    }
}
