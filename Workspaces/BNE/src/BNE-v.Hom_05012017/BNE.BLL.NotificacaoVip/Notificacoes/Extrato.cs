using BNE.BLL.Custom.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                EnviarEmailCurriculo();
                GravaLog(null, "Enviou email -> " + curriculo.idCurriculo);

                GravaLog(null, "Grava notificação de conclusao -> " + curriculo.idCurriculo);
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Sucesso);
                return true;
            }
            catch (Exception ex)
            {
                NotificacaoVipCurriculo.GravarNotificacao(curriculo.idCurriculo, idNotificacao, (short)Enumeradores.StatusExecucaoNotificacaoVip.finalizado_Erro, ex.Message.Truncate(50));
                GravaLog(ex, "Extrato -> Notificar. idCurriculo: " + _dtoCurriculo.idCurriculo);
                return false;
            }
        }
        #endregion

        #region EnviarEmailCurriculo
        private void EnviarEmailCurriculo()
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
                    return;
                }

                var email = new Uteis.Email(objCurriculo, objUsuarioFilialPerfil, assunto, mensagem, emailRemetente, emailDestinatario, BLL.Enumeradores.CartaEmail.ExtratoVip);
                email.Enviar();

                if (_dtoCurriculo.numeroCelular.HasValue)
                {
                    var desMensagemSMS = _dtoCurriculo.nome + ", seu extrato do VIP BNE foi encaminhado por email.";
                    var sms = new Uteis.SMS(_dtoCurriculo.nome, _dtoCurriculo.numeroCelular.Value, desMensagemSMS, _dtoCurriculo.idCurriculo);
                    sms.Enviar();
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex, "Extrato -> EnviarEmailCurriculo. idCurriculo: " + _dtoCurriculo.idCurriculo);
            }
        }
        #endregion

        #region MontarHtmlMensagem
        private string MontarHtmlMensagem(BLL.DTO.CartaEmail objCarta)
        {
            try
            {
                var html = objCarta.Conteudo;

                bool exibicaoPar = _extrato.ValoresExtrato.Count % 2 == 0;
                
                var qtdLinhasGrupo1 = 0;
                var qtdLinhasGrupo2 = 0;
                var exibirUltimoItemSozinho = false;
                var qtdLinhasNecessarias = 0;

                qtdLinhasNecessarias = _extrato.ValoresExtrato.Count / 2;

                if (exibicaoPar){
                    qtdLinhasGrupo1 = qtdLinhasNecessarias / 2;
                    qtdLinhasGrupo2 = qtdLinhasNecessarias - qtdLinhasGrupo1;
                }
                else
                {
                    qtdLinhasNecessarias++;
                    qtdLinhasGrupo1 = qtdLinhasNecessarias / 2;
                    qtdLinhasGrupo2 = qtdLinhasNecessarias - qtdLinhasGrupo1;
                    exibirUltimoItemSozinho = true;
                }

                List<string> linhasGrupo1 = Enumerable.Repeat(_extrato.templateLinha, qtdLinhasGrupo1).ToList();
                List<string> linhasGrupo2 = Enumerable.Repeat(_extrato.templateLinha, qtdLinhasGrupo2).ToList();

                //Percorer e preencher as linhas
                int idLinha = 1;
                int indexColuna = 0;
                string nomeLinha;
                string templateLinha;
                foreach (string linha in linhasGrupo1)
                {
                    nomeLinha = "{linha" + idLinha + "}";
                    templateLinha = _extrato.templateLinha;

                    templateLinha = templateLinha.Replace("{ColunaEsquerda}", CriarColuna(_extrato.ValoresExtrato[indexColuna]));
                    indexColuna++;

                    templateLinha = templateLinha.Replace("{ColunaDireita}", CriarColuna(_extrato.ValoresExtrato[indexColuna]));
                    indexColuna++;

                    html = html.Replace(nomeLinha, templateLinha);

                    idLinha++;
                }

                idLinha = 5; //Inicia grupo 2

                foreach (string linha in linhasGrupo2)
                {
                    nomeLinha = "{linha" + idLinha + "}";
                    templateLinha = _extrato.templateLinha;

                    if (exibirUltimoItemSozinho && indexColuna == _extrato.ValoresExtrato.Count - 1) //Ultimo item e impar
                        templateLinha = templateLinha.Replace("{ColunaEsquerda}", CriarColuna(_extrato.ValoresExtrato[indexColuna], true));
                    else
                        templateLinha = templateLinha.Replace("{ColunaEsquerda}", CriarColuna(_extrato.ValoresExtrato[indexColuna]));

                    indexColuna++;

                    if (indexColuna >= _extrato.ValoresExtrato.Count)
                    {
                        templateLinha = templateLinha.Replace("{ColunaDireita}", "");
                        html = html.Replace(nomeLinha, templateLinha);
                        break;
                    }

                    templateLinha = templateLinha.Replace("{ColunaDireita}", CriarColuna(_extrato.ValoresExtrato[indexColuna]));
                    indexColuna++;

                    html = html.Replace(nomeLinha, templateLinha);

                    idLinha++;
                }

                //Tratar ver vagas
                if (_extrato.QtdVagasNoPerfil > 0)
                {
                    string templateVerVagas = _extrato.templateVerVagas;
                    templateVerVagas = templateVerVagas.Replace("{QtdVagasNoPerfil}", _extrato.QtdVagasNoPerfil.ToString());


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

        #region CriarColuna
        private string CriarColuna(DTO.ExtratoTemplate extratoTemplate, bool dobrarTamanho = false)
        {
            string coluna = _extrato.templateColuna;

            coluna = coluna.Replace("{corFundo}", extratoTemplate.corFundo);
            coluna = coluna.Replace("{corTexto}", extratoTemplate.corTexto);
            coluna = coluna.Replace("{titulo}", extratoTemplate.titulo);
            coluna = coluna.Replace("{valor}", extratoTemplate.valor.ToString());
            coluna = coluna.Replace("{imagem}", extratoTemplate.imagem);

            if(dobrarTamanho)
                coluna = coluna.Replace("{tamanho}", (extratoTemplate.tamanho * 2).ToString());
            else
                coluna = coluna.Replace("{tamanho}", (extratoTemplate.tamanho).ToString());

            return coluna;
        }
        #endregion

        #region LimparLinhasNaoUsadas
        private void LimparLinhasNaoUsadas(ref string html)
        {
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
            {
                sw.WriteLine(DateTime.Now + "   Mensagem:   " + customMsg);

            }

            if (ex != null)
                EL.GerenciadorException.GravarExcecao(ex, "NotificacoesVip -> " + customMsg);
        }
        #endregion
    }
}
