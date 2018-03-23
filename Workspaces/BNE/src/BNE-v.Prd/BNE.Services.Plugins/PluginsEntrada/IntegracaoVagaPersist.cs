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
using BNE.BLL.Custom.Vaga;
using BNE.BLL.Custom;
using System.Globalization;
using System.Text.RegularExpressions;

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
        public IPluginResult DoExecute(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            return this.DoExecuteTask(objParametros, objAnexos);
        }

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

                //Trata Vinculos, cada vaga tera somente um tipo vinculo respeitando a regra de sobreposição Estagio - Efetivo - Primeiro vinculo existente
                oVagaIntegracao = TrataTiposVinculo(oVagaIntegracao);

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

                decimal? vlrSalarioDe = null;
                if (!String.IsNullOrEmpty(objParametros["vlrSalarioDe"].Valor))
                    vlrSalarioDe = decimal.Parse(objParametros["vlrSalarioDe"].Valor, new CultureInfo("pt-BR"));

                decimal? vlrSalarioPara = null;
                if (!String.IsNullOrEmpty(objParametros["vlrSalarioPara"].Valor))
                    vlrSalarioPara = decimal.Parse(objParametros["vlrSalarioPara"].Valor, new CultureInfo("pt-BR"));

                #endregion

                VagaIntegracao oVagaIntegracao = new VagaIntegracao();

                oVagaIntegracao.CodigoVagaIntegrador = codigo;
                oVagaIntegracao.VagaParaDeficiente = vagaIntegracao_vagaParaDeficiente;
                oVagaIntegracao.DeficienciaImportada = desDeficiencia;

                //Tratamento alteração descrição Vendedora por Vendedor
                if (descricao.ToLower().Contains("vendedor"))
                {
                    descricao = Regex.Replace(descricao.ToLower(), @"vendedor[ae]s?", "Vendedor");
                }

                //Carrega o objeto da vaga com base nos atributos recebidos
                oVagaIntegracao.Vaga = new BLL.Vaga
                {
                    CodigoVaga = codigo,
                    EmailVaga = email,
                    DescricaoAtribuicoes = descricao,
                    DescricaoDisponibilidades = disponibilidades,
                    DescricaoTiposVinculo = tiposVinculo,
                    NomeEmpresa = empresa,
                    QuantidadeVaga = short.Parse(QtdeVaga.ToString()),
                    ValorSalarioDe = vlrSalarioDe,
                    ValorSalarioPara = vlrSalarioPara
                };

                //Carrega Datas
                try
                {
                    if (!string.IsNullOrEmpty(dataAbertura))
                        oVagaIntegracao.Vaga.DataAbertura = Convert.ToDateTime(dataAbertura, CultureInfo.GetCultureInfo("pt-BR"));

                    if (!string.IsNullOrEmpty(dataPrazo))
                        oVagaIntegracao.Vaga.DataPrazo = Convert.ToDateTime(dataPrazo, CultureInfo.GetCultureInfo("pt-BR"));
                }
                catch(Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "ConverteVagaToVagaIntegracao(). Data abertura: " + dataAbertura + " Data prazo: " + dataPrazo);
                    throw;
                }

                //Carrega Cidade, caso tenha sido informado
                if (!string.IsNullOrEmpty(desCidade))
                {
                    IntegracaoVaga oIntegracaoVaga = new IntegracaoVaga();
                    oVagaIntegracao.Vaga.Cidade = Helper.DetectaCidade(desCidade); ;
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
            }
        }
        #endregion CarregaObjetoVagaIntegracao

        #region Trata tipo Vinculo
        public VagaIntegracao TrataTiposVinculo(VagaIntegracao objVaga)
        {
            string mensagem = string.Empty;
            string vinculoRecebido = objVaga.Vaga.DescricaoTiposVinculo;

            try
            {
                if (!String.IsNullOrEmpty(objVaga.Vaga.DescricaoTiposVinculo))
                {
                    var tiposVinculo = objVaga.Vaga.DescricaoTiposVinculo;
                    String[] contratos = tiposVinculo.Split(';');
                    objVaga.TiposVinculo = new List<VagaTipoVinculo>();

                    try
                    {
                        foreach (string contrato in contratos)
                        {
                            try
                            {
                                string vinculo = Regex.Replace(contrato, @"[,\.\/:\(\)\\\[\]\{\}\-\+\*].+", "");

                                //switch para vinculos inexistentes
                                switch(vinculo.ToLower())
                                {
                                    case "estagiario":
                                    case "estagiário":
                                    case "estag":

                                        vinculo = "Estágio";
                                        break;

                                    case "qualquer":
                                    case "intern":
                                    case "clt":

                                        vinculo = "Efetivo";
                                        break;
                                }

                                objVaga.TiposVinculo.Add(new VagaTipoVinculo { TipoVinculo = TipoVinculo.CarregarPorDescricao(vinculo.Trim()) });
                            }
                            catch (Exception ex)
                            {
                                mensagem = ex + " Tipo Vinculo original: " + contrato;
                                throw;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Erro foreach contratos: " + mensagem + " - " + tiposVinculo);
                        throw;
                    }

                    if (objVaga.TiposVinculo.Count > 0)
                    {
                        try
                        {
                            // seeta vinculo estagio caso exista o tipo vinculo
                            if (objVaga.TiposVinculo.Exists(x => x.TipoVinculo.DescricaoTipoVinculo.Equals(BLL.Enumeradores.TipoVinculo.Estágio.ToString())))
                            {
                                objVaga.TiposVinculo.Clear();
                                objVaga.TiposVinculo.Add(new VagaTipoVinculo { TipoVinculo = TipoVinculo.CarregarPorDescricao(BLL.Enumeradores.TipoVinculo.Estágio.ToString()) });
                                objVaga.Vaga.DescricaoTiposVinculo = objVaga.TiposVinculo.Select(v => v.TipoVinculo.DescricaoTipoVinculo).First();
                            }
                            // se não existir estagio seta efetivo
                            else if (objVaga.TiposVinculo.Exists(x => x.TipoVinculo.DescricaoTipoVinculo.Equals(BLL.Enumeradores.TipoVinculo.Efetivo.ToString())))
                            {
                                objVaga.TiposVinculo.Clear();
                                objVaga.TiposVinculo.Add(new VagaTipoVinculo { TipoVinculo = TipoVinculo.CarregarPorDescricao(BLL.Enumeradores.TipoVinculo.Efetivo.ToString()) });
                                objVaga.Vaga.DescricaoTiposVinculo = objVaga.TiposVinculo.Select(v => v.TipoVinculo.DescricaoTipoVinculo).First();

                            }
                            else if (objVaga.TiposVinculo.Count > 0)
                            {
                                // caso não exista estagio e efetivo seta o primeiro vinculo
                                var primeiroVinculo = objVaga.TiposVinculo.First();
                                objVaga.TiposVinculo.Clear();
                                objVaga.TiposVinculo.Add(primeiroVinculo);
                                objVaga.Vaga.DescricaoTiposVinculo = objVaga.TiposVinculo.Select(v => v.TipoVinculo.DescricaoTipoVinculo).First();

                            }
                        }
                        catch (Exception ex)
                        {
                            mensagem = ex + " TrataTipoVinculo(). Erro ao verificar existencia tipo vinculo vaga --> " + tiposVinculo;
                            throw;
                        }
                       
                    }
                    else
                    {
                        objVaga.TiposVinculo.Add(new VagaTipoVinculo { TipoVinculo = TipoVinculo.LoadObject((int)BLL.Enumeradores.TipoVinculo.Efetivo) });
                        objVaga.Vaga.DescricaoTiposVinculo = objVaga.TiposVinculo.Select(v => v.TipoVinculo.DescricaoTipoVinculo).First();
                    }
                }
                else
                {
                    //caso não exista tipo vinculo seta o tipo vinculo como "Efetivo"
                    objVaga.TiposVinculo = new List<VagaTipoVinculo>();
                    objVaga.TiposVinculo.Add(new VagaTipoVinculo { TipoVinculo = TipoVinculo.LoadObject((int)BLL.Enumeradores.TipoVinculo.Efetivo) });
                    objVaga.Vaga.DescricaoTiposVinculo = objVaga.TiposVinculo.Select(v => v.TipoVinculo.DescricaoTipoVinculo).First();

                }

                return objVaga;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "TrataTipoVinculo(). Erro ao tratar tipo vinculo: " + vinculoRecebido + " " + mensagem);
                throw;
            }
        }

        #endregion

        #endregion
    }
}
