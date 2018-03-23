using BNE.BLL;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using BNE.BLL.DTO;
using System.Text;
using System.Linq;
using BNE.BLL.Custom;
using TipoVinculo = BNE.BLL.Enumeradores.TipoVinculo;
using BNE.BLL.Mensagem.DTO;
using BNE.BLL.Mensagem;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "EnvioCandidatoVagaPerfil")]
    public class EnvioCandidatoVagaPerfil : InputPlugin
    {
        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var paramIdVaga = objParametros["idVaga"];

            if (paramIdVaga == null || !paramIdVaga.ValorInt.HasValue)
                throw new ArgumentNullException("paramIdVaga");

            var idVaga = paramIdVaga.ValorInt.Value;

            if (idVaga <= 0)
                throw new ArgumentOutOfRangeException("paramIdVaga");

            try
            {
                var objVaga = BNE.BLL.Vaga.LoadObject(idVaga);

                objVaga.Funcao.CompleteObject();
                objVaga.Cidade.CompleteObject();
                objVaga.Filial.CompleteObject();

                var loteSMS = new MensagemPlugin.MensagemSMSTanque();
                var listaEmail = new List<MensagemPlugin.MensagemEmail>();

                #region EnvioEmail Candidatos perfil Plano Venda SMS Email

                var objPlanoAdquiridoDetalhes = PlanoAdquiridoDetalhes.PlanoEnvioSmsEmailVagaLiberado(objVaga, objVaga.Filial);
                if (objPlanoAdquiridoDetalhes != null)
                {
                    //Monta a lista de envio com base no plano comprado
                    CriarMensagens(objVaga, out loteSMS, out listaEmail, objPlanoAdquiridoDetalhes);

                    var emailRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.EmailRemetenteVagaPerfil);

                    objVaga.UsuarioFilialPerfil.CompleteObject();
                    objVaga.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                    List<DestinatarioSMS> listaCVSEmail = BNE.BLL.Curriculo.RecuperarListaCvsEnvio(objVaga.Cidade.IdCidade, objVaga.Funcao.IdFuncao, VagaTipoVinculo.PossuiVinculo(objVaga, TipoVinculo.Estágio));
                    string mensagem;
                    PopularEmailCandidatosPlanoEnvioSMSVaga(listaCVSEmail, out mensagem);

                    UsuarioFilial objUsuarioFilial;
                    UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objVaga.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial);

                    var msgEmail = new MensagemPlugin.MensagemEmail
                    {
                        Descricao = mensagem,
                        To = objUsuarioFilial.EmailComercial,
                        From = emailRemetente,
                        Assunto = "Convocação realizada com sucesso, confira os dados."
                    };

                    listaEmail.Add(msgEmail);

                    // Encerra Plano Adquirido SmsEmailVaga
                    objPlanoAdquiridoDetalhes.PlanoAdquirido.CompleteObject();
                    objPlanoAdquiridoDetalhes.PlanoAdquirido.PlanoSituacao = new PlanoSituacao((int)BNE.BLL.Enumeradores.PlanoSituacao.Encerrado);
                    objPlanoAdquiridoDetalhes.PlanoAdquirido.Save();
                }
                else
                {
                    //Monta a lista de envio de empresas com plano ativo
                    CriarMensagens(objVaga, out loteSMS, out listaEmail);
                }
                #endregion

                if (loteSMS.mensagens.Count > 0 || listaEmail.Count > 0)
                {
                    var retorno = new MensagemPlugin(this, false)
                    {
                        ListaSMSTanque = new List<MensagemPlugin.MensagemSMSTanque> { loteSMS },
                        ListaEmail = listaEmail
                    };
                    return retorno;
                }

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "EnvioCandidatoVagaPerfil -> DoExecuteTask. Vaga: " + idVaga);
                Core.LogError(ex);
                throw;
            }
            return new MensagemPlugin(this, false);
        }
        #endregion

        #region Metodos

        #region CriarMensagens
        public static void CriarMensagens(BLL.Vaga objVaga, out MensagemPlugin.MensagemSMSTanque LoteSMSTanque, out List<MensagemPlugin.MensagemEmail> listaEmail, BLL.PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes = null)
        {
            try
            {
                LoteSMSTanque = new MensagemPlugin.MensagemSMSTanque();
                listaEmail = new List<MensagemPlugin.MensagemEmail>();

                // Recupera Cvs para envio de SMS/E-mail
                var listaCvs = BNE.BLL.Curriculo.RecuperarListaCvsEnvio(objVaga.Cidade.IdCidade, objVaga.Funcao.IdFuncao, VagaTipoVinculo.PossuiVinculo(objVaga, TipoVinculo.Estágio));

                #region SMS

                var templateSMS = new CampanhaTanque().GetTextoCampanha(BLL.Mensagem.Enumeradores.CampanhaTanque.EnvioCandVagaPerfil);
                var sms = templateSMS.mensagem;
                sms = sms.Replace("{Funcao}", objVaga.DescricaoFuncao);
                sms = sms.Replace("{Cidade}", Helper.FormatarCidade(objVaga.Cidade.NomeCidade, objVaga.Cidade.Estado.SiglaEstado));
                sms = sms.Replace("{Idf_Vaga}", objVaga.IdVaga.ToString());
                sms = sms.Replace("{CodVaga}", objVaga.CodigoVaga);
                LoteSMSTanque.idVaga = objVaga.IdVaga;
                LoteSMSTanque.desFuncao = objVaga.DescricaoFuncao;
                LoteSMSTanque.IdUsuarioOrigem = objVaga.UsuarioFilialPerfil.IdUsuarioFilialPerfil.ToString();
                LoteSMSTanque.idCampanha = (int)BLL.Mensagem.Enumeradores.CampanhaTanque.EnvioCandVagaPerfil;

                foreach (var objCurriculo in listaCvs)
                {
                    #region [SMS]
                    try
                    {
                        //Vaga para pcd não enviar sms task 28660
                        if (objVaga.Deficiencia == null || objVaga.Deficiencia.IdDeficiencia.Equals((int)BLL.Enumeradores.Deficiencia.Nenhuma))
                        {
                            if (string.IsNullOrWhiteSpace(objCurriculo.DDDCelular)
                                && string.IsNullOrWhiteSpace(objCurriculo.NumeroCelular))
                                continue;

                    if (LoteSMSTanque.mensagens.Any(a => a.IdCurriculo == objCurriculo.IdDestinatario))
                        continue;
                    string newSms = sms;
                    newSms = newSms.Replace("{Link}", BLL.Vaga.MontarLinkVagaSMS(objVaga.IdVaga, BLL.Curriculo.LoadObject(objCurriculo.IdDestinatario), "?utm_source=candidatarvaga&utm_medium=SMS&utm_campaign=candidatovagaperfilSMS"));
                    newSms = newSms.Replace("{Nome}", PessoaFisica.RetornarPrimeiroNome(objCurriculo.NomePessoa));
                    var mensagemSMS = new MensagemPlugin.MensagemSMS
                    {
                        Descricao = newSms,
                        IdCurriculo = objCurriculo.IdDestinatario,
                        idMensagemCampanha = templateSMS.id,
                        DDDCelular = objCurriculo.DDDCelular,
                        NumeroCelular = objCurriculo.NumeroCelular,
                        NomePessoa = objCurriculo.NomePessoa
                    };

                            LoteSMSTanque.mensagens.Add(mensagemSMS);

                            if (objPlanoAdquiridoDetalhes != null)
                            {
                                var objPlanoAdquiridoDetalhesCurriculo = new PlanoAdquiridoDetalhesCurriculo
                                {
                                    Curriculo = new BLL.Curriculo(objCurriculo.IdDestinatario),
                                    PlanoAdquiridoDetalhes = objPlanoAdquiridoDetalhes,
                                    TipoMensagemCS = new TipoMensagemCS((int)BLL.Enumeradores.TipoMensagem.SMS)
                                };
                                objPlanoAdquiridoDetalhesCurriculo.Save();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, String.Format("EnvioCandidatoVagaPerfil -> Erro ao gerar sms para envio vaga: {0} cv: {1}", objVaga.IdVaga, objCurriculo.IdDestinatario));
                    }
                    #endregion

                    #region [E-mail]
                    try
                    {
                        if (string.IsNullOrWhiteSpace(objCurriculo.Email))
                            continue;

                        if (BLL.Custom.Validacao.ValidarEmail(objCurriculo.Email))
                        {
                            #region MontarEmail

                            var emailRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.EmailRemetenteVagaPerfil);

                            string cidade, uf, porteEmpresa, setorEmpresa;

                            BNE.BLL.Filial.RecuperarInformacoesFilialParaCartaCvPerfil(objVaga.Filial.IdFilial, out setorEmpresa, out porteEmpresa, out cidade, out uf);

                            string listaVagas, salaVip, vip, cadastroCurriculo, pesquisaVagas, loginCandidato, quemMeViu, cadastroExperiencias;

                            BNE.BLL.Curriculo.RetornarHashLogarCurriculo(objCurriculo.CPF, objCurriculo.DataNascimento, out listaVagas, out salaVip, out vip, out quemMeViu, out cadastroCurriculo, out pesquisaVagas, out loginCandidato, out cadastroExperiencias);

                            objVaga.Funcao.AreaBNE.CompleteObject();
                            var urlVaga = Helper.RemoverAcentos($"/vaga-de-emprego-na-area-{objVaga.Funcao.AreaBNE.DescricaoAreaBNE}-em-{objVaga.Cidade.NomeCidade}-{objVaga.Cidade.Estado.SiglaEstado}/{objVaga.DescricaoFuncao}/{objVaga.IdVaga}".Replace(" ","-").ToLower());
                            string linkVaga = LoginAutomatico.GerarHashAcessoLogin(objCurriculo.CPF, objCurriculo.DataNascimento, urlVaga);
                            #endregion

                            string assunto;
                            var mensagem = Cartas.Email.MensagemEmailCvPerfilVaga(objVaga.DescricaoFuncao, linkVaga,
                                objVaga.IdVaga, objVaga.QuantidadeVaga, objVaga.ValorSalarioDe, objVaga.ValorSalarioPara, objVaga.Cidade.NomeCidade, objVaga.Cidade.Estado.SiglaEstado, objVaga.DescricaoAtribuicoes, objVaga.DescricaoRequisito, objCurriculo.NomePessoa, listaVagas, salaVip, vip, quemMeViu, cadastroCurriculo, pesquisaVagas, loginCandidato, out assunto);

                            var msgEmail = new MensagemPlugin.MensagemEmail
                            {
                                Assunto = assunto,
                                Descricao = mensagem,
                                To = objCurriculo.Email,
                                From = emailRemetente
                            };

                            if (listaEmail.Any(a => a.To == msgEmail.To))
                                continue;

                            listaEmail.Add(msgEmail);

                            if (objPlanoAdquiridoDetalhes != null)
                            {
                                var objPlanoAdquiridoDetalhesCurriculo = new PlanoAdquiridoDetalhesCurriculo
                                {
                                    Curriculo = new BLL.Curriculo(objCurriculo.IdDestinatario),
                                    PlanoAdquiridoDetalhes = objPlanoAdquiridoDetalhes,
                                    TipoMensagemCS = new TipoMensagemCS((int)BLL.Enumeradores.TipoMensagem.Email)
                                };
                                objPlanoAdquiridoDetalhesCurriculo.Save();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, String.Format("EnvioCandidatoVagaPerfil - > Envio de e-mail Vaga: {0}, cv: {1} ", objVaga.IdVaga, objCurriculo.IdDestinatario));
                    }
                    #endregion
                }

                #endregion
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "EnvioCandidatoVagaPerfil -> Falha ao criar mensagem. Vaga: " + objVaga.IdVaga);
                throw;
            }
        }
        #endregion

        #region PopularEmailCandidatosPlanoEnvioSMSVaga
        private void PopularEmailCandidatosPlanoEnvioSMSVaga(List<DestinatarioSMS> listaCvs, out string mensagemEmail)
        {
            string assunto;
            var conteudoEmail = BNE.BLL.CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.CandidatosPlanoLiberadoEmailSMSVaga, out assunto);
            var conteudoCandidatos = BNE.BLL.CartaEmail.RecuperarConteudo(BLL.Enumeradores.CartaEmail.DadosCandidatosPlanoLiberadoEmailSMSVaga);

            StringBuilder corpoEmail = new StringBuilder();

            foreach (var objCurrilo in listaCvs)
            {
                corpoEmail.Append(
                    conteudoCandidatos
                        .Replace("{Nome_Pessoa}", objCurrilo.NomePessoa)
                        .Replace("{Eml_Pessoa}", objCurrilo.Email)
                        .Replace("{DDD}", objCurrilo.DDDCelular)
                        .Replace("{Telefone}", objCurrilo.NumeroCelular)
                    );
            }

            string textoEmail = corpoEmail.ToString();
            mensagemEmail = conteudoEmail.Replace("{Candidatos}", textoEmail);
        }
        #endregion

        #endregion

    }
}
