using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using BNE.Services.Plugins.PluginsEntrada.Publicacao;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "PublicacaoVaga")]
    public class PublicacaoVaga : InputPlugin
    {

        public IPluginResult DoExecute(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            return DoExecuteTask(objParametros, objAnexos);
        }

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idVaga = objParametros["idVaga"].ValorInt;
            var novoCadastro = objParametros["novoCadastro"].ValorBool;
            try
            {
                if (idVaga != null)
                {
                    var objVaga = BLL.Vaga.LoadObject(idVaga.Value);
                    if (PublicarVaga(ref objVaga))
                    {
                        if (novoCadastro)
                        {
                            objVaga.FinalizarPublicacaoNovaVaga();
                        }

                        return new MensagemPlugin(this, true);
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

        #region PublicarVaga
        public static bool PublicarVaga(ref BLL.Vaga objVaga)
        {
            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var retorno = true;

            //Recuperando todas as regras de publicação
            var listaTodasRegras = Publicacao.RegraPublicacao.RecuperarRegrasPublicacao(Publicacao.TipoPublicacao.Vaga);
            var encontrouPalavrasProibidas = false;
            if (listaTodasRegras.Count > 0)
            {
                var listaPalavrasProibidas = PalavraProibida.ListarPalavrasProibidas();
                var regexPublicacaoVaga = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomatica);
                var regexFormatacaoVagaCapitalizacao = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomaticaFormatacaoCapitalizacao);
                var regexFormatacaoVagaEspaco = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomaticaFormatacaoEspaco);

                var camposParaAplicarRegra = listaTodasRegras.GroupBy(rp => rp.DescricaoCampo).Select(c => c.First()).ToList();
                List<VagaPergunta> listaPerguntas = VagaPergunta.RecuperarListaPerguntas(objVaga.IdVaga);

                foreach (var campo in camposParaAplicarRegra)
                {
                    var propertyInfo = objVaga.GetType().GetProperty(campo.DescricaoCampo);

                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propertyInfo.Name).ToList();

                        var valorCampo = propertyInfo.GetValue(objVaga, null) ?? string.Empty;

                        string texto = valorCampo.ToString();

                        if (PublicacaoAutomatica.ProcessarTextoVaga(ref objVaga, ref texto, propertyInfo.Name, regexPublicacaoVaga, regexFormatacaoVagaCapitalizacao, regexFormatacaoVagaEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas))
                            propertyInfo.SetValue(objVaga, texto, null);
                        else
                            retorno = false;
                    }
                    else if (listaPerguntas.Any())
                    {

                        propertyInfo = listaPerguntas.First().GetType().GetProperty(campo.DescricaoCampo);
                        if (propertyInfo != null && propertyInfo.CanWrite)
                        {

                            var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propertyInfo.Name).ToList();

                            foreach (var item in listaPerguntas)
                            {
                                var valorCampo = propertyInfo.GetValue(item, null) ?? string.Empty;

                                string texto = valorCampo.ToString();

                                if (PublicacaoAutomatica.ProcessarTextoVaga(ref objVaga, ref texto, propertyInfo.Name, regexPublicacaoVaga, regexFormatacaoVagaCapitalizacao, regexFormatacaoVagaEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas))
                                    propertyInfo.SetValue(item, texto, null);
                                else
                                    retorno = false;
                                item.Save();
                            }

                        }
                    }
                }
            }

            if (objVaga.SalarioEstaAcimaMedia())
            {
                retorno = false;
                PublicacaoAutomatica.SalvarHistoricoVaga(objVaga, "Salário de ou salário para acima da média!");
            }
            else
            {
                if (encontrouPalavrasProibidas)
                    retorno = false;
                else
                {
                    objVaga.FlagAuditada = true;
                    objVaga.DataAuditoria = DateTime.Now;
                }
            }
            objVaga.Save(null,BLL.Enumeradores.VagaLog.PluginPublicacaoVaga);

            if (retorno)
            {
                BufferAtualizacaoVaga.UpdateVaga(objVaga); //Update da vaga no SOLR
                objVaga.DispararPluginEnvioCandidatoVagaPerfil();
            }

            stopWatch.Stop();

            PublicacaoAutomatica.SalvarHistoricoVaga(objVaga, $"Vaga auditada pela publicação automática. Tempo de auditoria {stopWatch.Elapsed}.");

            objVaga.Cidade.CompleteObject();
            objVaga.Funcao.CompleteObject();
            objVaga.Filial.CompleteObject();

            return retorno;
        }
        public static void ValidarTermosVaga(BLL.Vaga objVaga)
        {
            var retorno = true;

            string desAtribuicoesOriginal = objVaga.DescricaoAtribuicoes;
            string desRequisitosOriginal = objVaga.DescricaoRequisito ?? "";

            //Recuperando todas as regras de publicação
            var listaTodasRegras = Publicacao.RegraPublicacao.RecuperarRegrasTermosVagas();
            var encontrouPalavrasProibidas = false;
            if (listaTodasRegras.Count > 0)
            {
                var listaPalavrasProibidas = PalavraProibida.ListarPalavrasProibidas();
                var regexPublicacaoVaga = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomatica);
                var regexFormatacaoVagaCapitalizacao = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomaticaFormatacaoCapitalizacao);
                var regexFormatacaoVagaEspaco = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomaticaFormatacaoEspaco);

                var camposParaAplicarRegra = listaTodasRegras.GroupBy(rp => rp.DescricaoCampo).Select(c => c.First()).ToList();

                foreach (var campo in camposParaAplicarRegra)
                {
                    var propertyInfo = objVaga.GetType().GetProperty(campo.DescricaoCampo);

                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        var listaRegras = listaTodasRegras.Where(rp => rp.DescricaoCampo == "" || rp.DescricaoCampo == propertyInfo.Name).ToList();

                        var valorCampo = propertyInfo.GetValue(objVaga, null) ?? string.Empty;

                        string texto = valorCampo.ToString();

                        if (PublicacaoAutomatica.ProcessarTextoVaga(ref objVaga, ref texto, propertyInfo.Name, regexPublicacaoVaga, regexFormatacaoVagaCapitalizacao, regexFormatacaoVagaEspaco, listaRegras, listaPalavrasProibidas, ref encontrouPalavrasProibidas))
                            propertyInfo.SetValue(objVaga, texto, null);
                        else
                            retorno = false;
                    }
                }
            }

            BLL.Vaga.InserirLogProcessamentoVaga(objVaga.IdVaga);

            if (desAtribuicoesOriginal == objVaga.DescricaoAtribuicoes && desRequisitosOriginal == objVaga.DescricaoRequisito)
            {
                //Não houve mudança na vaga
                return;
            }

            objVaga.Save(null, BLL.Enumeradores.VagaLog.PluginPublicacaoVaga_ValidarTermosVaga);

            if (retorno)
                BufferAtualizacaoVaga.UpdateVaga(objVaga); //Update da vaga no SOLR

            PublicacaoAutomatica.SalvarHistoricoVaga(objVaga, "Vaga auditada pelo processamento de validação de vagas.");
        }
        #endregion

        #endregion

    }

}
