using BNE.BLL;
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

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idVaga = objParametros["idVaga"].ValorInt;
            try
            {
                var dispararRastreador = objParametros["EnfileraRastreador"].ValorBool;

                if (idVaga != null)
                {
                    var objVaga = BLL.Vaga.LoadObject(idVaga.Value);
                    if (PublicarVaga(ref objVaga))
                    {
                        if (dispararRastreador && (objVaga.Deficiencia == null || (objVaga.Deficiencia != null && objVaga.Deficiencia.IdDeficiencia.Equals(0))) && !objVaga.Funcao.IdFuncao.Equals((int)BLL.Enumeradores.Funcao.Estagiario))
                            return new RastreadorVagaPlugin(this, objVaga.IdVaga, objVaga.CodigoVaga, false);
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

            var jaAuditada = objVaga.FlagAuditada;
            var retorno = true;

            //Recuperando todas as regras de publicação
            var listaTodasRegras = Publicacao.RegraPublicacao.RecuperarRegrasPublicacao(Publicacao.TipoPublicacao.Vaga);
            var encontrouPalavrasProibidas = false;
            if (listaTodasRegras.Count > 0)
            {
                var listaPalavrasProibidas = PalavraProibida.ListarPalavrasProibidas();
                var regexPublicacaoVaga = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomatica);
                var regexFormatacaoVagaCapitalizacao = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomaticaFormatacaoCapitalizacao);
                var regexFormatacaoVagaEspaco = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.RegexPublicacaoAutomaticaFormatacaoEspaco);

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
            objVaga.Save();

            if (retorno)
                objVaga.DispararPluginEnvioCandidatoVagaPerfil();

            stopWatch.Stop();

            PublicacaoAutomatica.SalvarHistoricoVaga(objVaga, string.Format("Vaga auditada pela publicação automática. Tempo de auditoria {0}.", stopWatch.Elapsed));

            objVaga.Cidade.CompleteObject();
            objVaga.Funcao.CompleteObject();
            objVaga.Filial.CompleteObject();

            return retorno;
        }
        #endregion

        #endregion

    }

}
