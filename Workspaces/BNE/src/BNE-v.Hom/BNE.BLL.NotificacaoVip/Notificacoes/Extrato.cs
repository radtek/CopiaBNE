using System;
using System.Collections.Generic;
using System.Linq;
using BNE.BLL.Common;
using System.IO;

namespace BNE.BLL.NotificacoesVip.Notificacoes
{
    internal class Extrato : INotificacaoCV
    {
        #region Variaveis
        private DTO.ExtratoCurriculo _extrato;
        private DTO.Curriculo _dtoCurriculo;
        private int _idCiclo;
        #endregion

        #region Notificar
        public bool Notificar(DTO.Curriculo curriculo, short idNotificacao, int idCiclo)
        {
            try
            {
                GravaLog(null, "Grava notificação do Vip -> " + curriculo.idCurriculo);
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.criado);

                if (string.IsNullOrEmpty(curriculo.email))
                {
                    NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_SemDados, "CV sem e-mail.");
                    return false;
                }

                _dtoCurriculo = curriculo;
                _idCiclo = idCiclo;

                var dt = Curriculo.CarregarExtratoCurriculo(_dtoCurriculo.idCurriculo);

                GravaLog(null, "Vai buscar extrato do Vip -> " + curriculo.idCurriculo);
                _extrato = DTO.ExtratoCurriculo.RetortarExtratoCurriculo(dt, _dtoCurriculo);
                GravaLog(null, "Retornou extrato do Vip -> " + curriculo.idCurriculo);

                if (_extrato.ValoresExtrato.Count <= 0)
                {
                    GravaLog(null, "Grava notificação de sem dados -> " + curriculo.idCurriculo);
                    NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_SemDados);
                    return true;
                }

                GravaLog(null, "Vai enviar email -> " + curriculo.idCurriculo);
                if(!EnviarEmailCurriculo())
                {
                    NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Erro);
                    GravaLog(null, "Erro -> Extrato -> Notificar. idCurriculo: " + _dtoCurriculo.idCurriculo + " Não enviou email.");
                    return false;
                }

                GravaLog(null, "Enviou email -> " + curriculo.idCurriculo);
                GravaLog(null, "Grava notificação de conclusao -> " + curriculo.idCurriculo);
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Sucesso);
                return true;
            }
            catch (Exception ex)
            {
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Erro, ex.Message.Truncate(50));
                GravaLog(ex, "Erro -> Extrato -> Notificar. idCurriculo: " + _dtoCurriculo.idCurriculo + " Erro:" + ex.Message);
                return false;
            }
        }
        #endregion

        #region EnviarEmailCurriculo
        private bool EnviarEmailCurriculo()
        {
            try
            {
                var objUsuarioFilialPerfil = new UsuarioFilialPerfil(_dtoCurriculo.idUsuarioFilialPerfil);
                var objCurriculo = new Curriculo(_dtoCurriculo.idCurriculo);

                var assunto = string.Empty;
                var mensagem = string.Empty;
                var emailDestinatario = _dtoCurriculo.email;
                var emailRemetente = "atendimento@bne.com.br";
                var objCarta = CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.ExtratoVip);

                assunto = objCarta.Assunto;
                mensagem = MontarHtmlMensagem(objCarta);

                if (string.IsNullOrEmpty(mensagem))
                {
                    GravaLog(null, "HTML para envio não gerado");
                    return false;
                }

                var email = new Uteis.Email(objCurriculo, objUsuarioFilialPerfil, assunto, mensagem, emailRemetente, emailDestinatario, BLL.Enumeradores.CartaEmail.ExtratoVip);
                if (!email.Enviar())
                    return false;

                if (_dtoCurriculo.numeroCelular.HasValue)
                {
                    var desMensagemSMS = CartaSMS.RecuperaValorConteudo(BLL.Enumeradores.CartaSMS.VipExtrato);
                    desMensagemSMS = desMensagemSMS.Replace("{Nome}", _dtoCurriculo.nome);
                    var sms = new Uteis.SMS(_dtoCurriculo.nome, _dtoCurriculo.numeroCelular.Value, desMensagemSMS, _dtoCurriculo.idCurriculo);
                    sms.Enviar();
                }

                return true;
            }
            catch (Exception ex)
            {
                GravaLog(ex, "Erro -> Extrato -> EnviarEmailCurriculo. idCurriculo: " + _dtoCurriculo.idCurriculo + " Erro: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region MontarHtmlMensagem
        private string MontarHtmlMensagem(BLL.DTO.CartaEmail objCarta)
        {
            try
            {
                var html = objCarta.Conteudo;

                var qtdLinhasNecessarias = _extrato.ValoresExtrato.Count;

                var LinhaGrupoAzul = 1;
                var LinhaGrupoCinza = 8;
                var indexTemplate = 0;
                var nomeLinha = string.Empty;

                foreach (var extrato in _extrato.ValoresExtrato)
                {
                    if(extrato.grupo == DTO.Grupo.azul)
                    {
                        nomeLinha = "{linha" + LinhaGrupoAzul + "}";
                        html = html.Replace(nomeLinha, CriarLinha(_extrato.ValoresExtrato[indexTemplate]));
                        indexTemplate++;
                        LinhaGrupoAzul++;
                    }
                    else
                    {
                        nomeLinha = "{linha" + LinhaGrupoCinza + "}";
                        html = html.Replace(nomeLinha, CriarLinha(_extrato.ValoresExtrato[indexTemplate]));
                        indexTemplate++;
                        LinhaGrupoCinza++;
                    }
                }

                //Tratar ver vagas
                if (_extrato.QtdVagasNoPerfil > 0 && _extrato.QtdVagasNoPerfilMes > 0)
                {
                    string templateVerVagas = _extrato.templateVerVagas;
                    templateVerVagas = templateVerVagas.Replace("{QtdVagasNoPerfil}", _extrato.QtdVagasNoPerfil.ToString());
                    templateVerVagas = templateVerVagas.Replace("{QtdVagasNoPerfilMes}", _extrato.QtdVagasNoPerfilMes.ToString());

                    //Montagem URL vagas no perfil
                    var linkMaisVagas = string.Format("/vagas-de-emprego-para-{0}-em-{1}-{2}", _dtoCurriculo.funcao.NormalizarURL(), _dtoCurriculo.cidade.NormalizarURL(), _dtoCurriculo.siglaEstado);
                    var url_vagas_no_perfil = LoginAutomatico.GerarUrl(_dtoCurriculo.numeroCPF, _dtoCurriculo.dataNascimento, linkMaisVagas);
                    templateVerVagas = templateVerVagas.Replace("{UrlVagasNoPerfil}", url_vagas_no_perfil);

                    html = html.Replace("{templateVerVagas}", templateVerVagas);
                }
                else
                {
                    html = html.Replace("{templateVerVagas}", "");
                }

                //Tratar campos fixos
                html = html.Replace("{NomeCurriculo}", _extrato.NomeCurriculo);
                
                LimparLinhasNaoUsadas(ref html);

                return html;
            }
            catch (Exception ex)
            {
                GravaLog(ex, "Extrato -> MontarHtmlMensagem. idCurriculo: " + _dtoCurriculo.idCurriculo);
                return "";
            }
        }
        #endregion

        #region CriarLinha
        private string CriarLinha(DTO.ExtratoTemplate extratoTemplate)
        {
            string linha = _extrato.templateLinha;

            if(extratoTemplate.nome == "QtdQuemMeviu") //Caso específico, não mostra quantidade do mes
            {
                linha = linha.Replace("(últimos 30 dias)", ""); 
                linha = linha.Replace("{descricaoTotal}", "");
                linha = linha.Replace("{prefixoQtdTotal}", "");
                linha = linha.Replace("{valorTotal}", "");
                linha = linha.Replace("{titulo}", extratoTemplate.titulo);
                linha = linha.Replace("{valorMes}", extratoTemplate.valorTotal.ToString());
            }
            else
            {
                linha = linha.Replace("{descricaoTotal}", extratoTemplate.sufixoQtdTotal == "" ? extratoTemplate.titulo.ToLower() : extratoTemplate.sufixoQtdTotal);
                linha = linha.Replace("{prefixoQtdTotal}", extratoTemplate.prefixoQtdTotal);
                linha = linha.Replace("{valorTotal}", extratoTemplate.valorTotal.ToString());
                linha = linha.Replace("{titulo}", extratoTemplate.titulo);
                linha = linha.Replace("{valorMes}", extratoTemplate.valorMes.ToString());
            }

            linha = linha.Replace("{imagem}", extratoTemplate.imagem);
            linha = linha.Replace("{corFundo}", extratoTemplate.corFundo);
            linha = linha.Replace("{corTextoMes}", extratoTemplate.corTextoMes);
            linha = linha.Replace("{corTextoTotal}", extratoTemplate.corTextoTotal);

            return linha;
        }
        #endregion

        #region LimparLinhasNaoUsadas
        private void LimparLinhasNaoUsadas(ref string html)
        {
            html = html.Replace("{linha10}", "");
            html = html.Replace("{linha9}", "");
            html = html.Replace("{linha8}", "");
            html = html.Replace("{linha7}", "");
            html = html.Replace("{linha6}", "");
            html = html.Replace("{linha5}", "");
            html = html.Replace("{linha4}", "");
            html = html.Replace("{linha3}", "");
            html = html.Replace("{linha2}", "");
            html = html.Replace("{linha1}", "");
        }
        #endregion

        #region GravaLogText
        private static void GravaLog(Exception ex, string customMsg)
        {
            using (StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "log.txt"))
                sw.WriteLine(DateTime.Now + "   Mensagem:   " + customMsg);

            if (ex != null)
                EL.GerenciadorException.GravarExcecao(ex, "NotificacoesVip -> " + customMsg);
        }
        #endregion
    }
}
