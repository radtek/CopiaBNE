using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using BNE.BLL;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using BNE.Services.Plugins.PluginsEntrada.Publicacao;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "PublicacaoCurriculo")]
    public class PublicacaoCurriculo : InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idCurriculo = objParametros["idCurriculo"].ValorInt;
            try
            {
                if (idCurriculo.HasValue)
                {
                    if (!AuditarCurriculo(idCurriculo.Value))
                    {
                        var listaMensagem = new List<MensagemPlugin.MensagemEmail> 
                        { 
                            new MensagemPlugin.MensagemEmail 
                            { 
                                Descricao = string.Format("Houve um erro na auditoria do currículo {0}.", idCurriculo), 
                                Assunto = "Publicação automática de currículos", 
                                To = "gieyson@bne.com.br", 
                                From = "ti@bne.com.br"
                            } 
                        };
                        return new MensagemPlugin(this, listaMensagem, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Core.LogError(ex);
            }
            return new MensagemPlugin(this, true);
        }
        #endregion

        #region Métodos

        #region PublicarCurriculo
        public static bool AuditarCurriculo(int idCurriculo)
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var encontrouPalavrasProibidas = false;
            var retorno = true;

            var objCurriculo = Curriculo.LoadObject(idCurriculo);

            PessoaFisicaComplemento objPessoaFisicaComplemento;
            PessoaFisicaComplemento.CarregarPorPessoaFisica(objCurriculo.PessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento);

            //Recuperando todas as regras de publicação
            var listaTodasRegras = Publicacao.RegraPublicacao.RecuperarRegrasPublicacao(Publicacao.TipoPublicacao.Curriculo);
            if (listaTodasRegras.Count > 0)
            {
                var listaPalavrasProibidas = PalavraProibida.ListarPalavrasProibidas();
                var regexPublicacaoCurriculo = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomatica);
                var regexFormatacaoCurriculoCapitalizacao = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomaticaFormatacaoCapitalizacao);
                var regexFormatacaoCurriculoEspaco = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomaticaFormatacaoEspaco);

                var camposParaAplicarRegra = listaTodasRegras.GroupBy(rp => rp.DescricaoCampo).Select(c => c.First()).ToList();
                foreach (var campo in camposParaAplicarRegra)
                {

                    #region Curriculo

                    var propriedadeCurriculo = objCurriculo.GetType().GetProperty(campo.DescricaoCampo);

                    if (propriedadeCurriculo != null && propriedadeCurriculo.CanWrite)
                    {
                        var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadeCurriculo.Name).ToList();
                        var valorCampo = propriedadeCurriculo.GetValue(objCurriculo, null) ?? string.Empty;

                        string texto = valorCampo.ToString();

                        if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadeCurriculo.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas))
                            propriedadeCurriculo.SetValue(objCurriculo, texto, null);
                        else
                            retorno = false;
                    }

                    #endregion

                    #region Pessoa Fisica Complemento

                    if (objPessoaFisicaComplemento != null)
                    {
                        var propriedadeFisicaComplemento = objPessoaFisicaComplemento.GetType().GetProperty(campo.DescricaoCampo);

                        if (propriedadeFisicaComplemento != null && propriedadeFisicaComplemento.CanWrite)
                        {
                            var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propriedadeFisicaComplemento.Name).ToList();
                            var valorCampo = propriedadeFisicaComplemento.GetValue(objPessoaFisicaComplemento, null) ?? string.Empty;

                            string texto = valorCampo.ToString();

                            if (PublicacaoAutomatica.ProcessarTextoCurriculo(ref objCurriculo, ref texto, propriedadeFisicaComplemento.Name, regexPublicacaoCurriculo, regexFormatacaoCurriculoCapitalizacao, regexFormatacaoCurriculoEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas))
                                propriedadeFisicaComplemento.SetValue(objPessoaFisicaComplemento, texto, null);
                            else
                                retorno = false;
                        }
                    }

                    #endregion

                }
            }

            if (encontrouPalavrasProibidas)
            {
                retorno = false;
                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)BLL.Enumeradores.SituacaoCurriculo.Bloqueado);
            }
            else
                objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)BLL.Enumeradores.SituacaoCurriculo.Auditado);

            objCurriculo.Save();

            objCurriculo.AtualizaCurriculoDW();

            if (objPessoaFisicaComplemento != null)
                objPessoaFisicaComplemento.Save();

            stopWatch.Stop();

            PublicacaoAutomatica.SalvarHistoricoCurriculo(objCurriculo, string.Format("Curriculo auditado pela publicação automática. Tempo de auditoria {0}.", stopWatch.Elapsed));

            return retorno;
        }
        #endregion

        #endregion

    }
}
