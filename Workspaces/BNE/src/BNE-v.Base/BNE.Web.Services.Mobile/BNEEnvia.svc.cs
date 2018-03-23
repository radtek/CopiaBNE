using System;
using System.Collections.Generic;
using System.Web;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Services.Mobile.DTO.BNEEnvia;
using BNE.Web.Services.Mobile.Enum;

namespace BNE.Web.Services.Mobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BNEEnviaApp" in code, svc and config file together.
    public class BNEEnvia : IBNEEnvia
    {

        #region Propriedades
        public HttpRequest Request
        {
            get
            {
                if (HttpContext.Current != null)
                    return HttpContext.Current.Request;
                return null;
            }
        }
        #endregion

        #region Métodos

        #region ValidaIMEi
        public RetornoValidaIMEI ValidaIMEI(EntradaValidaIMEI validaIMEI)
        {
            var retorno = new RetornoValidaIMEI();
            try
            {
                if (!Celular.VerificaIMEIEstaCadastrado(validaIMEI.IMEI))
                    retorno.Status = (int)StatusBNEEnvia.CelularNaoCadastrado;
                else
                {
                    if (!CelularSelecionador.VerificaCelularEstaLiberado(validaIMEI.IMEI))
                        retorno.Status = (int)StatusBNEEnvia.CelularBloqueado;
                    else
                    {
                        var objCelularSelecionadora = CelularSelecionador.RecuperarCelularSelecionador(validaIMEI.IMEI);
                        if (objCelularSelecionadora != null)
                        {
                            objCelularSelecionadora.Celular.AtualizarToken(validaIMEI.TokenGCM);

                            retorno.Nome = objCelularSelecionadora.RecuperarNomeSelecionador();
                            retorno.Status = (int)StatusBNEEnvia.Ok;
                            retorno.DataAtual = DateTime.Now.ToString("s");
                            retorno.Cota = Int32.Parse(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.BNEEnviaQuantidadeLimiteSMSDiario));
                            retorno.EnviaCopia = Properties.Settings.Default.EnviaCopia;
                            retorno.Versao = Properties.Settings.Default.Versao;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Status = (int)StatusBNEEnvia.Erro;
                EL.GerenciadorException.GravarExcecao(ex);
            }
            return retorno;
        }
        #endregion

        #region VerificaCampanhas
        public RetornoVerificaCampanhas VerificaCampanhas(EntradaVerificaCampanhas verificaCampanhas)
        {
            var retorno = new RetornoVerificaCampanhas { IdCampanha = 0, Telefones = new List<Telefone>() };
            try
            {
                Campanha objCampanhaParaEnviar = null;
                if (verificaCampanhas.idCampanha != 0)
                {
                    objCampanhaParaEnviar = Campanha.RecuperaCampanhaAEnviarIdCampanha(verificaCampanhas.IMEI, verificaCampanhas.idCampanha);
                }
                else
                {
                    objCampanhaParaEnviar = Campanha.RecuperaCampanhaAEnviar(verificaCampanhas.IMEI);
                }
                if (objCampanhaParaEnviar != null)
                {
                    retorno.IdCampanha = objCampanhaParaEnviar.IdCampanha;
                    retorno.nome = objCampanhaParaEnviar.NomeCampanha;
                    retorno.Mensagem = BoundaryHelper.EliminarCaracteres(objCampanhaParaEnviar.DescricaoMensagem);
                    var listaTelefones = CampanhaCurriculo.RecuperarListaCurriculosPorCampanha(objCampanhaParaEnviar);
                    foreach (var objCurriculos in listaTelefones)
                    {
                        retorno.Telefones.Add(new Telefone { Nome = objCurriculos.NomePessoa, NumeroTelefone = string.Concat(objCurriculos.NumeroDDDCelular, objCurriculos.NumeroCelular) });
                    }
                    objCampanhaParaEnviar.ConfirmarEnvio();
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
            return retorno;
        }
        #endregion

        #region ConfirmaCampanhaEnviada
        public RetornoConfirmaCampanhaEnviada ConfirmaCampanhaEnviada(EntradaConfirmaCampanhaEnviada confirmaCampanhaEnviada)
        {
            var retorno = new RetornoConfirmaCampanhaEnviada();
            try
            {
                var objCampanha = new Campanha(confirmaCampanhaEnviada.IdCampanha);
                if (objCampanha.ConfirmarEnvio())
                    retorno.Status = (int)StatusBNEEnvia.Ok;
                else
                    retorno.Status = (int)StatusBNEEnvia.Erro;
            }
            catch (Exception ex)
            {
                retorno.Status = (int)StatusBNEEnvia.Erro;

                EL.GerenciadorException.GravarExcecao(ex);
            }
            return retorno;
        }
        #endregion

        #region ReceberSMSConversacao
        [Obsolete]
        public void ReceberSMSConversacao(string Mensagem, decimal NumeroCelular, string CodigoUsuario)
        {
            throw new InvalidOperationException("obsoleto");

            //var objMensagem = new MensagemCS
            //    {
            //        TipoMensagemCS = new TipoMensagemCS((int)Enumeradores.TipoMensagem.SMS),
            //        StatusMensagemCS = new StatusMensagemCS((int)Enumeradores.StatusMensagem.Naoenviado),
            //        DescricaoMensagem = Mensagem,
            //        DescricaoEmailRemetente = " ",
            //        NumeroDDDCelular = NumeroCelular.ToString().Substring(0, 2),
            //        NumeroCelular = NumeroCelular.ToString().Substring(2, 8),
            //        FlagInativo = false,
            //        IdSistema = (int)Enumeradores.Sistema.BNE,
            //        IdCentroServico = (int)Enumeradores.CentroServico.BNE,
            //        FlagLido = false
            //    };

            //objMensagem.Save();

        }
        #endregion

        #endregion

    }
}
