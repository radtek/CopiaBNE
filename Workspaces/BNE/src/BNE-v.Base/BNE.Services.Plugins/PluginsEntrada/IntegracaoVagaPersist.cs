using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;
using BNE.BLL;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;

namespace BNE.Services.Plugins.PluginsEntrada
{

    #region Help

    /*
     *      FILA QUE RECEBE OS ATRIBUTOS DA VAGA ATIVA CADASTRADA NO SINE (VIA SITE OU IMPORTACAO) E ENVIA PARA O BANCO DE DADOS DO BNE.
     */

    #endregion

    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "IntegracaoVaga")]
    public class IntegracaoVagaPersist : InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            try
            {
                #region Parametros Mensagem
                var idOrigemImportacao = objParametros["OrigemImportacao"].ValorInt;
                var idIntegrador = objParametros["Integrador"].ValorInt;
                #endregion

                BLL.Custom.Vaga.IntegracaoVaga oIntegradorVaga = new BLL.Custom.Vaga.IntegracaoVaga();

                //Carrega a origem da vaga
                Origem oOrigemImportacao = new Origem();
                oOrigemImportacao = Origem.LoadObject(idOrigemImportacao.Value);

                //Carrega o integrador da vaga
                Integrador oIntegrador = new Integrador();
                oIntegrador = Integrador.LoadObject(idIntegrador.Value);

                //Carrega o objeto VagaIntegracao
                VagaIntegracao oVagaIntegracao = CarregaObjetoVagaIntegracao(objParametros);

                if (oVagaIntegracao == null)
                    return new MensagemPlugin(this, true); //Não carregou o obj vaga integração, então não processa a vaga

                
                //Realiza o Processamento da Vaga com o BD
                bool retornoProcessamento = oIntegradorVaga.ProcessaVaga(oVagaIntegracao, oOrigemImportacao, oIntegrador);

                return new MensagemPlugin(this, true);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "appBNE - Plugin IntegracaoVagaPersist");
                throw;
            }
            finally
            {
                
            }
        }
        #endregion

        #region Métodos

        #region CarregaObjetoVagaIntegracao
        protected VagaIntegracao CarregaObjetoVagaIntegracao(ParametroExecucaoCollection objParametros)
        {
            try
            {
                //Recebe os atributos da Vaga Integracao / Vaga
                #region Parametros Mensagem

                //Geral
                var codigo = objParametros["codigo"].Valor;

                //VagaIntegracao
                var vagaIntegracao_vagaParaDeficiente = objParametros["vagaIntegracao_vagaParaDeficiente"].ValorBool;

                //Vaga
                var email = objParametros["email"].Valor;
                var desCidade = objParametros["desCidade"].Valor;
                var dataAbertura = objParametros["dataAbertura"].Valor;
                var dataPrazo = objParametros["dataPrazo"].Valor;
                var desFuncao = objParametros["desFuncao"].Valor;
                var descricao = objParametros["descricao"].Valor;
                var empresa = objParametros["empresa"].Valor;
                var disponibilidades = objParametros["disponibilidades"].Valor;
                var tiposVinculo = objParametros["tiposVinculo"].Valor;
                var desEscolaridade = objParametros["desEscolaridade"].Valor;
                var desDeficiencia = objParametros["desDeficiencia"].Valor;
                var QtdeVaga = objParametros["qtdeVaga"].ValorInt;
                #endregion

                VagaIntegracao oVagaIntegracao = new VagaIntegracao();

                oVagaIntegracao.CodigoVagaIntegrador = codigo;
                oVagaIntegracao.VagaParaDeficiente = vagaIntegracao_vagaParaDeficiente;
                oVagaIntegracao.DeficienciaImportada = desDeficiencia;

                //Carrega o objeto da vaga com base nos atributos recebidos
                oVagaIntegracao.Vaga = new BLL.Vaga
                {
                    CodigoVaga = codigo,
                    EmailVaga = email,
                    DescricaoAtribuicoes = descricao,
                    DescricaoDisponibilidades = disponibilidades,
                    DescricaoTiposVinculo = tiposVinculo,
                    NomeEmpresa = empresa,
                    QuantidadeVaga = short.Parse(QtdeVaga.ToString())
                };

                //Carrega Datas
                if (!string.IsNullOrEmpty(dataAbertura))
                    oVagaIntegracao.Vaga.DataAbertura = Convert.ToDateTime(dataAbertura);

                if (!string.IsNullOrEmpty(dataPrazo))
                    oVagaIntegracao.Vaga.DataPrazo = Convert.ToDateTime(dataPrazo);


                //Carrega Cidade, caso tenha sido informado
                if (!string.IsNullOrEmpty(desCidade))
                {
                    Cidade oCidade;
                    Cidade.CarregarPorNome(desCidade, out oCidade);
                    oVagaIntegracao.Vaga.Cidade = oCidade;
                }
                else
                    return null;

                //Carrega Funcao, caso tenha sido informado
                if (!string.IsNullOrEmpty(desFuncao))
                {
                    oVagaIntegracao.Vaga.Funcao = Funcao.CarregarPorDescricao(desFuncao);
                    oVagaIntegracao.Vaga.DescricaoFuncao = desFuncao;
                    oVagaIntegracao.FuncaoImportada = desFuncao;
                }
                else
                {
                    return null;
                }

                return oVagaIntegracao;
            }
            catch (Exception ex)
            {
                throw;
                return null;
            }
        }
        #endregion CarregaObjetoVagaIntegracao

        #endregion
    }
}
