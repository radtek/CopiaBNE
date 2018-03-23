using BNE.BLL.AsyncServices;
using BNE.BLL.Custom.Email;
using BNE.BLL.DTO;
using BNE.BLL.Enumeradores;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using BNE.BLL.Common;
using BNE.BLL.Custom.Solr.Buffer;

namespace BNE.BLL.Custom.EnvioMensagens
{
    public class EnvioMensagens
    {
        #region Metodos Publicos

        #region CriarFilaProcessamentoMensagens
        public static void CriarFilaProcessamentoMensagens(UsuarioFilialPerfil objUsuarioFilialPerfil, Filial objFilial, List<int> listCurriculo, string desMensagemSMS, string desMensagemEmail, bool enviarSMS, bool enviarEmail, DateTime? dtaAgendamento)
        {
            try
            {
                //Cria itens de campanha para serem processados via assincrono
                CampanhaMensagem objCampanhaMensagem = new CampanhaMensagem();
                objCampanhaMensagem.DataDisparo = DateTime.Now;

                objCampanhaMensagem.FlagEnviaEmail = enviarEmail;
                if (enviarEmail)
                    objCampanhaMensagem.DescricaomensagemEmail = desMensagemEmail;

                objCampanhaMensagem.FlagEnviaSMS = enviarSMS;
                if (enviarSMS)
                    objCampanhaMensagem.DescricaomensagemSMS = desMensagemSMS;

                objCampanhaMensagem.UsuarioFilialPerfil = objUsuarioFilialPerfil;
                objCampanhaMensagem.Save();

                CampanhaMensagemEnvios objCampanhaMensagemEnvios;
                DataTable dtCampanhasMensagem = null;

                foreach (int cv in listCurriculo)
                {
                    objCampanhaMensagemEnvios = new CampanhaMensagemEnvios();
                    objCampanhaMensagemEnvios.Curriculo = new Curriculo(cv);
                    objCampanhaMensagemEnvios.CampanhaMensagem = objCampanhaMensagem;
                    objCampanhaMensagemEnvios.DataAgendamento = dtaAgendamento;
                    objCampanhaMensagemEnvios.AddBulkTable(ref dtCampanhasMensagem);
                }

                CampanhaMensagemEnvios.SaveBulkTable(dtCampanhasMensagem);

                //Criar fila no assincrono
                var parametros = new ParametroExecucaoCollection
                            {
                                {"idCampanhaMensagem", "idCampanhaMensagem", objCampanhaMensagem.IdCampanhaMensagem.ToString(), objCampanhaMensagem.IdCampanhaMensagem.ToString()}
                            };

                ProcessoAssincrono.IniciarAtividade(AsyncServices.Enumeradores.TipoAtividade.CampanhaMensagem, parametros);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "BNE - Falha ao criar campanha de mensagens");
            }
        }
        #endregion

        #region EnviarCampanhaCVs
        /// <summary>
        ///     Envia campanha de e-mails e/ou SMS (tanque OU tww)
        /// </summary>
        /// <returns>
        ///     TRUE: O envio de mensagens ocorreu, para 1 ou mais destinatários informados. Pode retornar a variável mensagemObservacoes
        ///     FALSE: Nenhuma mensagem de e-mail e/ou sms foi enviada - retorna a variável mensagemErro
        /// </returns>
        public static bool EnviarMensagemCV(CampanhaMensagem objCampanhaMensagem, List<CampanhaMensagemEnvios> lstCampanhaMensagemEnvios, out string mensagemRetorno, bool processoAssincrono = false, int? idMensagemCampanha = null, int? idCampanhaTanque = null)
        {
            try
            {
                ProcessamentoEnvioMensagens objProcessamento = new ProcessamentoEnvioMensagens();

                //Inicializa variaveis de retorno do método
                mensagemRetorno = string.Empty;

                //Carrega objeto filial
                objCampanhaMensagem.UsuarioFilialPerfil.CompleteObject();
                Filial objFilial = Filial.LoadObject(objCampanhaMensagem.UsuarioFilialPerfil.Filial.IdFilial);

                objProcessamento.DesMensagemEmail = objCampanhaMensagem.DescricaomensagemEmail;
                objProcessamento.DesMensagemSMS = objCampanhaMensagem.DescricaomensagemSMS;
                objProcessamento.EnviarEmail = objCampanhaMensagem.FlagEnviaEmail;
                objProcessamento.EnviarSMS = objCampanhaMensagem.FlagEnviaSMS;
                objProcessamento.Filial = objFilial;
                objProcessamento.UsuarioFilialPerfil = objCampanhaMensagem.UsuarioFilialPerfil;
                objProcessamento.idCampanhaTanque = idCampanhaTanque;
                objProcessamento.idMensagemCampanha = idMensagemCampanha;
                objProcessamento.EmpresaPossuiPlano = objFilial.PossuiPlanoAtivo();
                objProcessamento.processamentoAssincrono = processoAssincrono;

                //Validação Tanque
                if (objProcessamento.EnviarSMS)
                    objProcessamento.UsaSMSTanque = CelularSelecionador.VerificaCelularEstaLiberadoParaTanque(objCampanhaMensagem.UsuarioFilialPerfil.IdUsuarioFilialPerfil); //Valida se o usuário está ativo no tanque, caso positivo, o envio e validações serão por la

                //Busca Email Comercial
                UsuarioFilial objUsuarioFilial;
                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objCampanhaMensagem.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                    objProcessamento.EmailComercial = objUsuarioFilial.EmailComercial;


                //Validações gerais
                if (objProcessamento.EnviarEmail && !objProcessamento.EnviarSMS && string.IsNullOrEmpty(objProcessamento.EmailComercial))
                {
                    mensagemRetorno = "Não é possível realizar o envio do(s) e-mail(s). Motivo: não existe nenhum e-mail comercial configurado para o usuário. Verifique.";
                    objCampanhaMensagem.DescricaoObsErroEnvio = "Não é possível realizar o envio do(s) e-mail(s). Motivo: não existe nenhum e-mail comercial configurado para o usuário.";
                    objCampanhaMensagem.DataFinalizacao = DateTime.Now;
                    objCampanhaMensagem.Save();
                    return false;
                }

                //Carrega Destinatarios
                var destinatarios = ListaDestinatarios(lstCampanhaMensagemEnvios.Select(x => x.Curriculo.IdCurriculo).ToList()).ToList();
                if (destinatarios.Count == 0)
                {
                    mensagemRetorno = "Nenhum destinatário válido foi encontrado. Verifique.";
                    objCampanhaMensagem.DescricaoObsErroEnvio = "Nenhum destinatário válido foi encontrado.";
                    objCampanhaMensagem.DataFinalizacao = DateTime.Now;
                    objCampanhaMensagem.Save();
                    return false;
                }
                objProcessamento.QtdCandidatosSelecionados = destinatarios.Count;


                CampanhaMensagemEnvios objCampanhaMensagemEnvios;
                //Percorrer a lista de CVs para envio
                foreach (var destinatario in destinatarios)
                {
                    //Carrega o destinatario da campanha
                    objCampanhaMensagemEnvios = new CampanhaMensagemEnvios();
                    objCampanhaMensagemEnvios = lstCampanhaMensagemEnvios.Where(x => x.Curriculo.IdCurriculo == destinatario.IdCurriculo).First();

                    using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                if (destinatario.VIP)
                                {
                                    ProcessaEnvioVIP(objProcessamento, destinatario, objCampanhaMensagemEnvios, trans);
                                }
                                else
                                {
                                    ProcessaEnvioNaoVIP(objProcessamento, destinatario, objCampanhaMensagemEnvios, trans);
                                }

                                objCampanhaMensagemEnvios.DataProcessamento = DateTime.Now;
                                objCampanhaMensagemEnvios.Save(trans);

                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                EL.GerenciadorException.GravarExcecao(ex,
                                    "Falha no processamento de envio das mensagens por e-mail e sms para um destinatário. CV: " + destinatario.IdCurriculo +
                                    " Enviar SMS: " + objProcessamento.EnviarSMS.ToString() +
                                    " Enviar Email: " + objProcessamento.EnviarEmail.ToString());
                                trans.Rollback();


                                objCampanhaMensagemEnvios.DataProcessamento = DateTime.Now;
                                objCampanhaMensagemEnvios.DescricaoObsErroEnvio += "Ocorreu algum erro no processamento do envio.";
                                objCampanhaMensagemEnvios.Save();

                            }
                        }
                    }
                }

                //Finaliza a campanha
                objCampanhaMensagem.DataFinalizacao = DateTime.Now;
                objCampanhaMensagem.Save();

                if (processoAssincrono)
                    return true;

                return ValidarEnvioMensagens(objProcessamento, out mensagemRetorno);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha no processamento de envio das mensagens por e-mail e sms.");
                mensagemRetorno = "Falha no envio das mensagens. Por favor, tente novamente.";
                return false;
            }
        }
        #endregion

        #endregion


        #region Métodos Privados

        #region ListaDestinatarios
        protected static IEnumerable<CelularSelecionadorDestinatario> ListaDestinatarios(List<int> listaCurriculos)
        {
            var strListCVs = String.Join(",", listaCurriculos);
            return BLL.Curriculo.CarregarObjetoDestinatariosEnvioMensagens(strListCVs);
        }
        #endregion


        #region ProcessaEnvioVIP
        protected static void ProcessaEnvioVIP(ProcessamentoEnvioMensagens objProcessamento, CelularSelecionadorDestinatario destinatario, CampanhaMensagemEnvios objCampanhaMensagemEnvios, SqlTransaction trans)
        {
            if (VerificaVisualizacao30Dias(objProcessamento, destinatario, trans))
            {
                EnviarSemDescontarSaldo(objProcessamento, destinatario, objCampanhaMensagemEnvios, trans);
            }
            else
            {
                if (objProcessamento.EmpresaPossuiPlano)
                {
                    if (objProcessamento.Filial.SaldoVisualizacao(trans) > 0)
                    {
                        EnviarDescontandoSaldo(objProcessamento, destinatario, objCampanhaMensagemEnvios, trans);
                    }
                    else
                    {
                        //Não envia
                        objCampanhaMensagemEnvios.FlagEnviouEmail = false;
                        objCampanhaMensagemEnvios.FlagEnviouSMS = false;
                        objCampanhaMensagemEnvios.DescricaoObsErroEnvio = "Empresa sem saldo de visualização. Nenhum envio realizado";
                        return;
                    }
                }
                else
                {
                    if (VerificaRegraChupaVIP(objProcessamento, trans))
                    {
                        if (EnviarSemDescontarSaldo(objProcessamento, destinatario, objCampanhaMensagemEnvios, trans))
                        {
                            CurriculoQuemMeViu.SalvarQuemMeViuSite(objProcessamento.Filial, new Curriculo(destinatario.IdCurriculo), objProcessamento.UsuarioFilialPerfil.IdUsuarioFilialPerfil);
                            CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objProcessamento.Filial, objProcessamento.UsuarioFilialPerfil, new Curriculo(destinatario.IdCurriculo), destinatario.VIP, true, BNE.Common.Helper.RecuperarIP());
                        }
                    }
                    else
                    {
                        //Não envia
                        objCampanhaMensagemEnvios.FlagEnviouEmail = false;
                        objCampanhaMensagemEnvios.FlagEnviouSMS = false;
                        objCampanhaMensagemEnvios.DescricaoObsErroEnvio = "Empresa sem plano. Nenhum envio realizado";
                        return;
                    }
                }
            }
        }
        #endregion

        #region ProcessaEnvioNaoVIP
        protected static void ProcessaEnvioNaoVIP(ProcessamentoEnvioMensagens objProcessamento, CelularSelecionadorDestinatario destinatario, CampanhaMensagemEnvios objCampanhaMensagemEnvios, SqlTransaction trans)
        {
            if (!objProcessamento.EmpresaPossuiPlano)
            {
                //Não envia
                objCampanhaMensagemEnvios.FlagEnviouEmail = false;
                objCampanhaMensagemEnvios.FlagEnviouSMS = false;
                objCampanhaMensagemEnvios.DescricaoObsErroEnvio = "Empresa sem plano. Nenhum envio realizado";
                return;
            }


            if (VerificaVisualizacao30Dias(objProcessamento, destinatario, trans))
            {
                EnviarSemDescontarSaldo(objProcessamento, destinatario, objCampanhaMensagemEnvios, trans);
            }
            else
            {
                //Armazana o obj do plano quantidade para ajustar quantidades.
                if (objProcessamento.Filial.SaldoVisualizacao(trans) > 0)
                {
                    EnviarDescontandoSaldo(objProcessamento, destinatario, objCampanhaMensagemEnvios, trans);
                }
                else
                {
                    //Não Envia
                    objCampanhaMensagemEnvios.FlagEnviouEmail = false;
                    objCampanhaMensagemEnvios.FlagEnviouSMS = false;
                    objCampanhaMensagemEnvios.DescricaoObsErroEnvio = "Empresa sem saldo de visualização. Nenhum envio realizado";
                    return;
                }
            }
        }
        #endregion

        #region VerificaVisualizacao30Dias
        protected static bool VerificaVisualizacao30Dias(ProcessamentoEnvioMensagens objProcessamento, CelularSelecionadorDestinatario destinatario, SqlTransaction trans)
        {
            return CurriculoVisualizacao.VerificarVisualizacaoCV(new Curriculo(destinatario.IdCurriculo), objProcessamento.Filial, trans);
        }
        #endregion

        #region VerificaRegraChupaVIP
        protected static bool VerificaRegraChupaVIP(ProcessamentoEnvioMensagens obProcessamento, SqlTransaction trans)
        {
            return obProcessamento.Filial.EmpresaSemPlanoPodeVisualizarCurriculo(1, trans);
        }
        #endregion

        #region EnviarDescontandoSaldo
        protected static void EnviarDescontandoSaldo(ProcessamentoEnvioMensagens objProcessamento, CelularSelecionadorDestinatario destinatario, CampanhaMensagemEnvios objCampanhaMensagemEnvios, SqlTransaction trans)
        {
            bool enviouEmail = false;
            bool enviouSMS = false;
            string mensagemErroEmail = string.Empty;
            string mensagemErroSMS = string.Empty;

            if (objProcessamento.EnviarEmail)
            {
                if (!String.IsNullOrEmpty(destinatario.Email))
                    enviouEmail = EnviarEmail(objProcessamento, destinatario, trans, out mensagemErroEmail);
                else // Task 28678 - Enviar SMS pra o candidato sem email.
                    EnvioSMSParaEmailNull(destinatario, trans);
            }

            if (objProcessamento.EnviarSMS)
            {
                if (objProcessamento.UsaSMSTanque)
                    enviouSMS = EnviarSMSTanque(objProcessamento, destinatario, objCampanhaMensagemEnvios, trans, out mensagemErroSMS);
                else
                    enviouSMS = EnviarSMSTWW(objProcessamento, destinatario, trans, out mensagemErroSMS);
            }

            objCampanhaMensagemEnvios.FlagEnviouEmail = enviouEmail;
            if (objProcessamento.EnviarEmail && !enviouEmail)
                objCampanhaMensagemEnvios.DescricaoObsErroEnvio += "Falha no envio de e-mail: " + mensagemErroEmail + ".";

            objCampanhaMensagemEnvios.FlagEnviouSMS = enviouSMS;
            if (objProcessamento.EnviarSMS && !enviouSMS)
                objCampanhaMensagemEnvios.DescricaoObsErroEnvio += "Falha no envio de SMS: " + mensagemErroSMS + ".";

            TratarVisualizacoesCV(objProcessamento, destinatario, enviouEmail, enviouSMS, trans);
        }
        #endregion

        #region EnviaMensagemTanque
        protected static bool EnviaMensagemTanque(CelularSelecionadorDestinatario destinatario, CampanhaMensagemEnvios objCampanhaMensagemEnvios, out string erroOuObservacao, ProcessamentoEnvioMensagens objProcessamento)
        {
            var dto = new BNETanqueService.InReceberMensagem();
            dto.cu = objProcessamento.UsuarioFilialPerfil.IdUsuarioFilialPerfil.ToString(CultureInfo.InvariantCulture);
            dto.e = true;

            if (objProcessamento.idCampanhaTanque != null) //Se o tipo de campanha foi informado, envia junto com a conversa para o tanque
                dto.conversa = new BNETanqueService.ConversaDTO() { campanha = objProcessamento.idCampanhaTanque };

            BNETanqueService.Mensagem mensagem = new BNETanqueService.Mensagem()
            {
                ci = destinatario.IdCurriculo.ToString(CultureInfo.InvariantCulture),
                dcm = DateTime.Now,
                dm = new { nome = destinatario.Nome }.ToString(objProcessamento.DesMensagemSMS),
                nc = Convert.ToDecimal(destinatario.NumeroDDDCelular + destinatario.NumeroCelular),
                np = destinatario.Nome,
                imc = objProcessamento.idMensagemCampanha,
                dta = objCampanhaMensagemEnvios.DataAgendamento

            };

            BNETanqueService.Mensagem[] mensagens = new BNETanqueService.Mensagem[1];
            mensagens[0] = mensagem;
            dto.l = mensagens;

            BNETanqueService.OutReceberMensagem envioMensagemResult;
            using (var client = new BNETanqueService.AppClient())
            {
                envioMensagemResult = client.ReceberMensagem(dto);
            }

            if (envioMensagemResult == null)
            {
                EL.GerenciadorException.GravarExcecao(new NullReferenceException("envioMensagemResult"));
                erroOuObservacao = "Falha na comunicação com o serviço de envio de SMS, contate administrador.";
                return false;
            }

            if (envioMensagemResult.l != null && envioMensagemResult.l.Count() > 0)
            {
                int idCurriculo = Convert.ToInt32(mensagem.ci);

                //Tratamento para o chat da selecionadora com cada CV (quando o processamento não é assincrono)
                if (!objProcessamento.processamentoAssincrono)
                {
                    var res = envioMensagemResult.l.ElementAtOrDefault(0);
                    var pair = BNE.Chat.Core.ChatService.Instance.ChatStore.AddPrivateMessage(idCurriculo,
                                                                        objProcessamento.UsuarioFilialPerfil.IdUsuarioFilialPerfil,
                                                                        res == 0
                                                                            ? Guid.NewGuid().ToString()
                                                                            : res.ToString(CultureInfo.InvariantCulture),
                                                                        true);
                    if (pair.Value != null)
                    {
                        pair.Value.StatusTypeValue = -2;
                        pair.Value.MessageContent = mensagem.dm;
                    }
                }

                //Atualiza o CV para constar o envio de SMS
                BufferAtualizacaoSMSCurriculo.Update(new Curriculo(idCurriculo));
            }

            if (!string.IsNullOrEmpty(envioMensagemResult.me))
            {
                erroOuObservacao = envioMensagemResult.me;
                return false;
            }

            if (envioMensagemResult.qtdDisp > 0)
            {
                erroOuObservacao = string.Empty;
                return true;
            }

            //Trata resposta pois ocorreu um agendamento de mensagens
            if (1 <= (envioMensagemResult.qtdDisp * -1))
            {
                erroOuObservacao = "A mensagem via SMS foi agendada para envio amanhã. Atenção! A cota diária de SMS já foi excedida.";
                return true;
            }

            var totalEnviado = 1 - (envioMensagemResult.qtdDisp * -1);
            if (envioMensagemResult.qtdAgendadaAmanha > 0)
            {
                erroOuObservacao =
                    string.Format(
                        "A mensagem via SMS foi enviada com sucesso. Observação: A cota diária de SMS foi excedida, {0} mensagens estão agendadas para serem enviadas amanhã.",
                        envioMensagemResult.qtdAgendadaAmanha);
            }
            else
            {
                erroOuObservacao = "A mensagem via SMS foi enviada com sucesso. Observação: A cota diária de SMS foi excedida.";
            }
            return true;
        }
        #endregion

        #region TratarVisualizacoesCV
        protected static void TratarVisualizacoesCV(ProcessamentoEnvioMensagens objProcessamento, CelularSelecionadorDestinatario destinatario, bool enviouEmail, bool enviouSMS, SqlTransaction trans)
        {
            if (enviouEmail || enviouSMS)
            {
                var objCurriculo = new Curriculo(destinatario.IdCurriculo);

                var objCurriculoVisualizacao = new CurriculoVisualizacao
                {
                    Filial = objProcessamento.Filial,
                    Curriculo = objCurriculo,
                    DataVisualizacao = DateTime.Now
                };
                objCurriculoVisualizacao.Save(trans);

                CurriculoQuemMeViu.SalvarQuemMeViuSite(objProcessamento.Filial, new Curriculo(destinatario.IdCurriculo), objProcessamento.UsuarioFilialPerfil.IdUsuarioFilialPerfil);
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objProcessamento.Filial, objProcessamento.UsuarioFilialPerfil, objCurriculo, destinatario.VIP, true, BNE.Common.Helper.RecuperarIP());

                if (enviouSMS)
                    objProcessamento.Filial.DescontarSMS(trans);

                objProcessamento.Filial.DescontarVisualizacao(trans);
            }
        }
        #endregion

        #region EnvioSMSParaEmailNull
        /// <summary>
        /// Envia sms para quem não tem email 
        /// </summary>
        /// <param name="objProcessamento"></param>
        /// <param name="destinatario"></param>
        /// <param name="tras"></param>
        private static void EnvioSMSParaEmailNull(CelularSelecionadorDestinatario destinatario, SqlTransaction tras)
        {
            var DesMensagemSMS = CartaSMS.RecuperaValorConteudo(Enumeradores.CartaSMS.EmailNull);
            Curriculo oCurriculo = new Curriculo(destinatario.IdCurriculo);
            PessoaFisica oPessoa = PessoaFisica.LoadObject(destinatario.IdPessoaFisica);
            UsuarioFilialPerfil oUsuarioFilial = new UsuarioFilialPerfil(destinatario.IdUsuarioFilialPerfil);
            ParametroCurriculo oParametroC = null;
            try
            {
                oParametroC = ParametroCurriculo.LoadObject((int)Enumeradores.Parametro.EnvioSMSSemEmailDias, destinatario.IdCurriculo);
            }
            catch (Exception ex)
            {
            }

            if (oParametroC != null)
            {// ja possuir registro verificar a data de atualização pra não enviar novamente no mesmo dia(parametro)
                if (oParametroC.DataAlteracao.Value.AddDays(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.EnvioSMSSemEmailDias))) <= DateTime.Now.Date)
                {
                    int idMensagem = MensagemCS.EnviaSMSTanque(oCurriculo, oPessoa, oUsuarioFilial, oUsuarioFilial, DesMensagemSMS, destinatario.NumeroDDDCelular, destinatario.NumeroCelular,
                    Enumeradores.UsuarioSistemaTanque.EnviaSMSCadSemEmail, null);
                    if (idMensagem > 0)
                        oParametroC.Save();
                }
            }
            else
            {
                int idMensagem = MensagemCS.EnviaSMSTanque(oCurriculo, oPessoa, oUsuarioFilial, oUsuarioFilial, DesMensagemSMS, destinatario.NumeroDDDCelular, destinatario.NumeroCelular,
                   Enumeradores.UsuarioSistemaTanque.EnviaSMSCadSemEmail, null);
                if (idMensagem > 0)
                {
                    oParametroC = new ParametroCurriculo();
                    oParametroC.Parametro = Parametro.LoadObject((int)Enumeradores.Parametro.EnvioSMSSemEmailDias);
                    oParametroC.Curriculo = oCurriculo;
                    oParametroC.Save();
                }

            }

        }
        #endregion

        #region EnviarSemDescontarSaldo
        protected static bool EnviarSemDescontarSaldo(ProcessamentoEnvioMensagens objProcessamento, CelularSelecionadorDestinatario destinatario, CampanhaMensagemEnvios objCampanhaMensagemEnvios, SqlTransaction trans)
        {
            bool enviouEmail = false;
            bool enviouSMS = false;
            string mensagemErroEmail = string.Empty;
            string mensagemErroSMS = string.Empty;

            if (objProcessamento.EnviarEmail)
            {
                if (!String.IsNullOrEmpty(destinatario.Email))
                    enviouEmail = EnviarEmail(objProcessamento, destinatario, trans, out mensagemErroEmail);
                else // Task 28678 - Enviar SMS pra o candidato sem email.
                    EnvioSMSParaEmailNull(destinatario, trans);
            }

            if (objProcessamento.EnviarSMS)
            {
                if (objProcessamento.UsaSMSTanque)
                    enviouSMS = EnviarSMSTanque(objProcessamento, destinatario, objCampanhaMensagemEnvios, trans, out mensagemErroSMS);
                else
                    enviouSMS = EnviarSMSTWW(objProcessamento, destinatario, trans, out mensagemErroSMS);
            }

            objCampanhaMensagemEnvios.FlagEnviouEmail = enviouEmail;
            if (objProcessamento.EnviarEmail && !enviouEmail)
                objCampanhaMensagemEnvios.DescricaoObsErroEnvio += "Falha no envio de e-mail: " + mensagemErroEmail + ".";

            objCampanhaMensagemEnvios.FlagEnviouSMS = enviouSMS;
            if (objProcessamento.EnviarSMS && !enviouSMS)
                objCampanhaMensagemEnvios.DescricaoObsErroEnvio += "Falha no envio de SMS: " + mensagemErroSMS + ".";

            return enviouEmail || enviouSMS;
        }
        #endregion

        #region EnviarSMSTWW
        protected static bool EnviarSMSTWW(ProcessamentoEnvioMensagens objProcessamento, CelularSelecionadorDestinatario destinatario, SqlTransaction trans, out string erroOuObservacao)
        {
            erroOuObservacao = string.Empty;

            if (!objProcessamento.EmpresaPossuiPlano)
            {
                erroOuObservacao = "Empresa não possui plano para realizar envio de SMS";
                return false;
            }

            if (string.IsNullOrEmpty(destinatario.NumeroCelular) || string.IsNullOrEmpty(destinatario.NumeroDDDCelular))
            {
                erroOuObservacao = "Número de celular e/ou DDD inválidos.";
                return false;
            }

            var saldoSMS = objProcessamento.Filial.SaldoSMS(trans);
            if (saldoSMS <= 0)
            {
                erroOuObservacao = "Saldo insuficiente para envio de SMS";
                return false;
            }

            MensagemCS.SalvarSMS(new Curriculo(destinatario.IdCurriculo), objProcessamento.UsuarioFilialPerfil, new UsuarioFilialPerfil(destinatario.IdUsuarioFilialPerfil), objProcessamento.DesMensagemSMS, destinatario.NumeroDDDCelular, destinatario.NumeroCelular, trans);
            BufferAtualizacaoSMSCurriculo.Update(new Curriculo(destinatario.IdCurriculo));
            objProcessamento.SMSEnviados.Add(new DtoEnvioSMS() { IdCVDestinatario = destinatario.IdCurriculo, CelDestinatario = Convert.ToDecimal(destinatario.NumeroDDDCelular + destinatario.NumeroDDDCelular), NomeDestinatario = destinatario.Nome });
            return true;

        }
        #endregion

        #region EnviarSMSTanque
        protected static bool EnviarSMSTanque(ProcessamentoEnvioMensagens objProcessamento, CelularSelecionadorDestinatario destinatario, CampanhaMensagemEnvios objCampanhaMensagemEnvios, SqlTransaction trans, out string erroOuObservacao)
        {
            erroOuObservacao = string.Empty;

            if (objProcessamento.CotaDiaEncerrada)//Sem cota, não realiza mais envios no dia atual
            {
                erroOuObservacao = "Cota do dia encerrada para envio de SMS";
                return false;
            }

            if (string.IsNullOrEmpty(destinatario.NumeroCelular) || string.IsNullOrEmpty(destinatario.NumeroDDDCelular))
            {
                erroOuObservacao = "Número de celular e/ou DDD inválidos.";
                return false;
            }

            //Caso o retorno seja positivo, enviou todas as mensagens ou agendou algumas para o próximo dia
            //Se retornar false deu erro, se retornar true e a variavel de erro ou obs estiver vazia enviou, caso contrario ou agendou ou acabou a cota
            bool enviouMensagem = false;
            enviouMensagem = EnviaMensagemTanque(destinatario, objCampanhaMensagemEnvios, out erroOuObservacao, objProcessamento);

            if (enviouMensagem)
                objProcessamento.SMSEnviados.Add(new DtoEnvioSMS() { IdCVDestinatario = destinatario.IdCurriculo, CelDestinatario = Convert.ToDecimal(destinatario.NumeroDDDCelular + destinatario.NumeroDDDCelular), NomeDestinatario = destinatario.Nome });

            if (enviouMensagem && !string.IsNullOrEmpty(erroOuObservacao))
            {
                //Envio efetuado mas a cota foi encerrada
                objProcessamento.CotaDiaEncerrada = true;
            }
            return enviouMensagem;
        }
        #endregion

        #region ValidaCelular
        protected static bool ValidaCelular(CelularSelecionadorDestinatario destinatario)
        {
            if (!String.IsNullOrWhiteSpace(destinatario.NumeroDDDCelular) &&
                !String.IsNullOrWhiteSpace(destinatario.NumeroCelular) &&
                destinatario.NumeroDDDCelular.Trim().All(char.IsDigit) &&
                destinatario.NumeroCelular.Trim().All(char.IsDigit))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region EnviarEmail
        protected static bool EnviarEmail(ProcessamentoEnvioMensagens objProcessamento, CelularSelecionadorDestinatario destinatario, SqlTransaction trans, out string mensagemErro)
        {
            mensagemErro = string.Empty;
            if (string.IsNullOrEmpty(destinatario.Email) || string.IsNullOrEmpty(objProcessamento.EmailComercial))
            {
                mensagemErro = "Destinatário ou usuário sem e-mail.";
                return false;
            }

            if (!MensagemCS.PodeEnviarMensagem(objProcessamento.UsuarioFilialPerfil.IdUsuarioFilialPerfil, (int)TipoMensagem.Email, destinatario.IdUsuarioFilialPerfil))
            {
                mensagemErro = "Envio repetido identificado.";
                return false;
            }

            if (EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(new Curriculo(destinatario.IdCurriculo), objProcessamento.UsuarioFilialPerfil, new UsuarioFilialPerfil(destinatario.IdUsuarioFilialPerfil), string.Format("Mensagem enviada por {0}", objProcessamento.Filial.NomeFantasia), objProcessamento.DesMensagemEmail, null, objProcessamento.EmailComercial, destinatario.Email, trans))
            {
                objProcessamento.EmailsEnviados.Add(new DtoEnvioEmail() { IdCVDestinatario = destinatario.IdCurriculo, EmailDestinatario = destinatario.Email, NomeDestinatario = destinatario.Nome });
                return true;
            }
            else
            {
                mensagemErro = "E-mail do destinatário inválido.";
                return false;
            }
        }
        #endregion

        #region ValidarEnvioMensagens
        protected static bool ValidarEnvioMensagens(ProcessamentoEnvioMensagens objProcessamento, out string retornoValidacao)
        {
            retornoValidacao = string.Empty;
            return ValidaEnvios(objProcessamento, out retornoValidacao);
        }

        /// <summary>
        ///     Tratamento para exibição de mensagem em qualquer tipo de falha
        ///     Retonos:
        ///         TRUE: caso o envio ocorreu, com ou sem falhas
        ///         FALSE: caso nenhuma mensagem tenha sido enviada
        /// </summary>
        protected static bool ValidaEnvios(ProcessamentoEnvioMensagens objProcessamento, out string mensagemRetorno)
        {
            mensagemRetorno = string.Empty;

            //Tratamento de envios zerados
            #region Zero envio de email e sms, na tentativa de envio de email e de SMS
            //Tentativa de envio de sms e e-mail, sem sucesso nos dois casos
            if (objProcessamento.EnviarSMS && objProcessamento.EnviarEmail)
                if (objProcessamento.SMSEnviados.Count == 0 && objProcessamento.EmailsEnviados.Count == 0)
                {
                    mensagemRetorno = "Nenhuma mensagem foi enviada por SMS e e-mail. Motivo: falta de saldo ou destinatário(s) inválido(s). Verifique.";
                    return false;
                }
            #endregion

            #region Zero envio de email ou de sms, na tentativa de envio de email e de SMS
            //Tentativa de envio de sms e e-mail, sem sucesso no envio de email
            if (objProcessamento.EnviarSMS && objProcessamento.EnviarEmail)
                if (objProcessamento.SMSEnviados.Count > 0 && objProcessamento.EmailsEnviados.Count == 0)
                {
                    mensagemRetorno = "Mensagens SMS enviadas. Nenhuma mensagem foi enviada por e-mail. Verifique.";
                    return true;
                }

            //Tentativa de envio de sms e e-mail, sem sucesso no envio de SMS
            if (objProcessamento.EnviarSMS && objProcessamento.EnviarEmail)
                if (objProcessamento.SMSEnviados.Count == 0 && objProcessamento.EmailsEnviados.Count > 0)
                {
                    mensagemRetorno = "Mensagens de e-mail enviadas. Nenhuma mensagem foi enviada por SMS. Motivo: falta de saldo ou destinatário(s) inválido(s). Verifique.";
                    return true;
                }
            #endregion

            #region Zero envio de SMS, na tentativa de envio de SMS apenas
            //Tentativa de envio de SMS, sem sucesso
            if (objProcessamento.EnviarSMS && !objProcessamento.EnviarEmail)
                if (objProcessamento.SMSEnviados.Count == 0)
                {
                    mensagemRetorno = "Nenhum SMS foi enviado. Motivo: falta de saldo ou destinatário(s) inválido(s). Verifique.";
                    return false;
                }
            #endregion

            #region Zero envio de Email, na tentativa de envio de Email apenas
            //Tentativa de envio de e-mail, sem sucesso
            if (!objProcessamento.EnviarSMS && objProcessamento.EnviarEmail)
                if (objProcessamento.EmailsEnviados.Count == 0)
                {
                    mensagemRetorno = "Nenhum e-mail foi enviado. Motivo: falta de saldo ou destinatário(s) inválido(s). Verifique.";
                    return false;
                }
            #endregion


            //Se houve o envio de algo, trata as perdas se houverem... (apenas envio em massa)
            if (objProcessamento.QtdCandidatosSelecionados > 1)
            {
                #region Envio de SMS e de Email
                if (objProcessamento.EnviarEmail && objProcessamento.EnviarSMS)
                {
                    if (objProcessamento.QtdCandidatosSelecionados > objProcessamento.SMSEnviados.Count || objProcessamento.QtdCandidatosSelecionados > objProcessamento.EmailsEnviados.Count)
                    {
                        mensagemRetorno = "Mensagens enviadas com sucesso! Observação: Algumas mensagens não foram enviadas por falta de saldo ou alguns destinatários não são válidos para o envio. Verifique.";
                        return true;
                    }
                }
                #endregion

                #region Envio de SMS apenas
                if (!objProcessamento.EnviarEmail && objProcessamento.EnviarSMS)
                {
                    if (objProcessamento.QtdCandidatosSelecionados > objProcessamento.SMSEnviados.Count)
                    {
                        mensagemRetorno = "Mensagens enviadas com sucesso! Observação: Algumas mensagens por SMS não foram enviadas por falta de saldo ou alguns destinatários não são válidos para o envio. Verifique.";
                        return true;
                    }
                }
                #endregion

                #region Envio de Email apenas
                if (objProcessamento.EnviarEmail && !objProcessamento.EnviarSMS)
                {
                    if (objProcessamento.QtdCandidatosSelecionados > objProcessamento.EmailsEnviados.Count)
                    {
                        mensagemRetorno = "Mensagens enviadas com sucesso! Observação: Algumas mensagens por e-mail não foram enviadas por falta de saldo ou alguns destinatários não são válidos para o envio. Verifique.";
                        return true;
                    }
                }
                #endregion
            }
            return true;
        }
        #endregion

        #endregion
    }
}
