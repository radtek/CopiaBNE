using BNE.BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.NotificacoesVip.Notificacoes
{
    internal class Atualizacao : INotificacaoCV
    {
        public bool Notificar(DTO.Curriculo curriculo, short idNotificacao, int idCiclo)
        {
            try
            {
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.criado);

                InformativoVip informativoParaEnvio = InformativoVip.RetornarProximoInformativoCVPorTipo(curriculo.idCurriculo, (int)Enumeradores.TipoInformativoVip.AtualizacaoCV);

                if (informativoParaEnvio != null)
                {
                    //Efetua o envio do informativo da vez
                    var objUsuarioFilialPerfil = new UsuarioFilialPerfil(curriculo.idUsuarioFilialPerfil);
                    var objCurriculo = new Curriculo(curriculo.idCurriculo);

                    var assunto = string.Empty;
                    var mensagem = string.Empty;
                    var emailDestinatario = curriculo.email;
                    var emailRemetente = "atendimento@bne.com.br";
                    var link = LoginAutomatico.GerarHashAcessoLogin(curriculo.numeroCPF, curriculo.dataNascimento, "/" + Rota.RecuperarURLRota(BNE.BLL.Enumeradores.RouteCollection.CadastroCurriculoDados));

                    assunto = informativoParaEnvio.DescricaoAssuntoInformativoVip;
                    assunto = assunto.Replace("{Nome}", curriculo.nome);

                    mensagem = informativoParaEnvio.ValorInformativoVip;
                    mensagem = mensagem.Replace("{Nome}", curriculo.nome);
                    mensagem = mensagem.Replace("{link}", link);

                    var email = new Uteis.Email(objCurriculo, objUsuarioFilialPerfil, assunto, mensagem, emailRemetente, emailDestinatario);
                    if (!email.Enviar())
                    {
                        NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Erro, "Falha no envio do email.");
                        return false;
                    }

                    if (curriculo.numeroCelular.HasValue)
                    {
                        var desMensagemSMS = informativoParaEnvio.DescricaoMensagemSMS;
                        desMensagemSMS = desMensagemSMS.Replace("{Nome}", curriculo.nome);
                        var sms = new Uteis.SMS(curriculo.nome, curriculo.numeroCelular.Value, desMensagemSMS, curriculo.idCurriculo);
                        sms.Enviar();
                    }

                    NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Sucesso);

                    InformativoVipCurriculo infoVipCv = new InformativoVipCurriculo() { Curriculo = new Curriculo(curriculo.idCurriculo), InformativoVip = informativoParaEnvio };
                    infoVipCv.Save();
                }
                else
                    NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_SemDados);

                return true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha na notificação de Atualização para o CV " + curriculo.idCurriculo);
                return false;
            }
        }
    }
}
