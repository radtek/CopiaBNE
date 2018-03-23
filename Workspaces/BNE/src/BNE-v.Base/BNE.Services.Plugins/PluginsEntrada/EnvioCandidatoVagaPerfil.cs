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

                var listaSMS = new List<MensagemPlugin.MensagemSMSTanque>();
                var listaEmail = new List<MensagemPlugin.MensagemEmail>();

                #region EnvioEmail Candidatos perfil Plano Venda SMS Email

                var objPlanoAdquiridoDetalhes = PlanoAdquiridoDetalhes.PlanoEnvioSmsEmailVagaLiberado(objVaga, objVaga.Filial);
                if (objPlanoAdquiridoDetalhes != null)
                {
                    CriarMensagens(objVaga, objPlanoAdquiridoDetalhes, out listaSMS, out listaEmail);

                    var emailRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.EmailRemetenteVagaPerfil);

                    objVaga.UsuarioFilialPerfil.CompleteObject();
                    objVaga.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                    List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaCVSEmail = BNE.BLL.Curriculo.RecuperarListaCvsEnvioEmail(objVaga.Cidade.IdCidade, objVaga.Funcao.IdFuncao);
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
                #endregion

                if (listaSMS.Count > 0 || listaEmail.Count > 0)
                {
                    var retorno = new MensagemPlugin(this, false)
                    {
                        ListaSMSTanque = listaSMS,
                        ListaEmail = listaEmail
                    };
                    return retorno;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                Core.LogError(ex);
                throw;
            }
            return new MensagemPlugin(this, false);
        }
        #endregion

        #region Metodos

        #region CriarMensagens
        public static void CriarMensagens(BLL.Vaga objVaga, BLL.PlanoAdquiridoDetalhes objPlanoAdquiridoDetalhes, out List<MensagemPlugin.MensagemSMSTanque> listaSMS, out List<MensagemPlugin.MensagemEmail> listaEmail)
        {

            listaSMS = new List<MensagemPlugin.MensagemSMSTanque>();
            listaEmail = new List<MensagemPlugin.MensagemEmail>();

            #region SMS

            var sms = ConteudoHTML.RecuperaValorConteudo(BNE.BLL.Enumeradores.ConteudoHTML.MensagemSMSCvPerfilVaga);
            sms = sms.Replace("{Funcao}", objVaga.DescricaoFuncao);
            sms = sms.Replace("{Idf_Vaga}", objVaga.IdVaga.ToString());

            // Recupera Cvs para envio de SMS
            List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaCvsSMS = BNE.BLL.Curriculo.RecuperarListaCvsEnvioSMS(objVaga.Cidade.IdCidade, objVaga.Funcao.IdFuncao);

            foreach (var objCurriculo in listaCvsSMS)
            {
                if (string.IsNullOrWhiteSpace(objCurriculo.dddCelular)
                    && string.IsNullOrWhiteSpace(objCurriculo.numeroCelular))
                    continue;

                var mensagemSMS = new MensagemPlugin.MensagemSMSTanque
                {
                    Descricao = sms,
                    IdCurriculo = objCurriculo.idDestinatario,
                    DDDCelular = objCurriculo.dddCelular,
                    NumeroCelular = objCurriculo.numeroCelular,
                    NomePessoa = objCurriculo.nomePessoa
                };

                if (listaSMS.Any(a => a.IdCurriculo == mensagemSMS.IdCurriculo))
                    continue;

                listaSMS.Add(mensagemSMS);

                var objPlanoAdquiridoDetalhesCurriculo = new PlanoAdquiridoDetalhesCurriculo
                {
                    Curriculo = new BLL.Curriculo(objCurriculo.idDestinatario),
                    PlanoAdquiridoDetalhes = objPlanoAdquiridoDetalhes,
                    TipoMensagemCS = new TipoMensagemCS((int)BLL.Enumeradores.TipoMensagem.SMS)
                };
                objPlanoAdquiridoDetalhesCurriculo.Save();
            }

            #endregion

            #region Email

            List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaCVSEmail = BNE.BLL.Curriculo.RecuperarListaCvsEnvioEmail(objVaga.Cidade.IdCidade, objVaga.Funcao.IdFuncao);

            string corpoDefault = BNE.BLL.CartaEmail.RecuperarConteudo(BLL.Enumeradores.CartaEmail.MensagemEmailCvPerfilVaga);

            string assuntoEmailDefault = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.AssuntoVagaPerfil);

            foreach (var objCurriculo in listaCVSEmail)
            {
                if (string.IsNullOrWhiteSpace(objCurriculo.emailPessoa))
                    continue;

                if (BLL.Custom.Validacao.ValidarEmail(objCurriculo.emailPessoa))
                {
                    #region MontarEmail

                    var assuntoEmail = string.Format(assuntoEmailDefault, objCurriculo.nomePessoa);
                    var emailRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.EmailRemetenteVagaPerfil);
                    var mensagem = corpoDefault;

                    string cidade, uf, porteEmpresa, setorEmpresa;

                    BNE.BLL.Filial.RecuperarInformacoesFilialParaCartaCvPerfil(objVaga.Filial.IdFilial, out setorEmpresa, out porteEmpresa, out cidade, out uf);

                    string listaVagas, salaVip, vip, cadastroCurriculo, pesquisaVagas, loginCandidato, quemMeViu, cadastroExperiencias;

                    BNE.BLL.Curriculo.RetornarHashLogarCurriculo(objCurriculo.idDestinatario, out listaVagas, out salaVip, out vip, out quemMeViu, out cadastroCurriculo, out pesquisaVagas, out loginCandidato, out cadastroExperiencias);

                    // Logar candidatos hash links
                    mensagem = mensagem.Replace("{Lista_Vagas}", listaVagas);
                    mensagem = mensagem.Replace("{Sala_Vip}", salaVip);
                    mensagem = mensagem.Replace("{vip}", vip);
                    mensagem = mensagem.Replace("{Quem_Me_Viu}", quemMeViu);
                    mensagem = mensagem.Replace("{Cadastro_Curriculo}", cadastroCurriculo);
                    mensagem = mensagem.Replace("{Pesquisa_Vagas}", pesquisaVagas);
                    mensagem = mensagem.Replace("{login_candidato}", salaVip);

                    mensagem = mensagem.Replace("{Funcao_Vaga}", objVaga.DescricaoFuncao);
                    mensagem = mensagem.Replace("{Qtd_Vaga}", objVaga.QuantidadeVaga.ToString());
                    if (!string.IsNullOrEmpty(objVaga.ValorSalarioPara.ToString()))
                        mensagem = mensagem.Replace("{Salario}", objVaga.ValorSalarioPara.Value.ToString("C2"));
                    else
                        mensagem = mensagem.Replace("{Salario}", "não informado");

                    mensagem = mensagem.Replace("{Cidade}", objVaga.Cidade.NomeCidade);
                    mensagem = mensagem.Replace("{UF}", objVaga.Cidade.Estado.SiglaEstado);

                    mensagem = mensagem.Replace("{Atribuicoes_Vaga}", objVaga.DescricaoAtribuicoes);
                    mensagem = mensagem.Replace("{Nome_Usuario}", objCurriculo.nomePessoa);
                    mensagem = mensagem.Replace("{Idf_Vaga}", objVaga.IdVaga.ToString());

                    if (objVaga.DescricaoRequisito != null)
                        mensagem = mensagem.Replace("{Requisitos_Vaga}", objVaga.DescricaoRequisito);
                    else
                        mensagem = mensagem.Replace("{Requisitos_Vaga}", "não informados");

                    #endregion

                    var msgEmail = new MensagemPlugin.MensagemEmail
                    {
                        Descricao = mensagem,
                        To = objCurriculo.emailPessoa,
                        From = emailRemetente,
                        Assunto = assuntoEmail
                    };

                    if (listaEmail.Any(a => a.To == msgEmail.To))
                        continue;

                    listaEmail.Add(msgEmail);

                    var objPlanoAdquiridoDetalhesCurriculo = new PlanoAdquiridoDetalhesCurriculo
                    {
                        Curriculo = new BLL.Curriculo(objCurriculo.idDestinatario),
                        PlanoAdquiridoDetalhes = objPlanoAdquiridoDetalhes,
                        TipoMensagemCS = new TipoMensagemCS((int)BLL.Enumeradores.TipoMensagem.Email)
                    };
                    objPlanoAdquiridoDetalhesCurriculo.Save();
                }
            }

            #endregion

        }
        #endregion

        #region PopularEmailCandidatosPlanoEnvioSMSVaga
        private void PopularEmailCandidatosPlanoEnvioSMSVaga(List<PessoaFisicaEnvioSMSTanque> listaCvs, out string mensagemEmail)
        {
            string assunto;
            var conteudoEmail = BNE.BLL.CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.CandidatosPlanoLiberadoEmailSMSVaga, out assunto);
            var conteudoCandidatos = BNE.BLL.CartaEmail.RecuperarConteudo(BLL.Enumeradores.CartaEmail.DadosCandidatosPlanoLiberadoEmailSMSVaga);

            StringBuilder corpoEmail = new StringBuilder();

            foreach (var objCurrilo in listaCvs)
            {
                corpoEmail.Append(
                    conteudoCandidatos
                        .Replace("{Nome_Pessoa}", objCurrilo.nomePessoa)
                        .Replace("{Eml_Pessoa}", objCurrilo.emailPessoa)
                        .Replace("{DDD}", objCurrilo.dddCelular)
                        .Replace("{Telefone}", objCurrilo.numeroCelular)
                    );
            }

            string textoEmail = corpoEmail.ToString();
            mensagemEmail = conteudoEmail.Replace("{Candidatos}", textoEmail);
        }
        #endregion

        #endregion

    }
}
