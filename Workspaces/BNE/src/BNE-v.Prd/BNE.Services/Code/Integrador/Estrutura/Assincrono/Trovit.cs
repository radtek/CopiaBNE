using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BNE.BLL;
using BNE.EL;
using Parametro = BNE.BLL.Enumeradores.Parametro;

namespace BNE.Services.Code.Integrador.Estrutura.Assincrono
{
    public class Trovit
    {
        public class Response
        {
            private readonly BLL.Integrador objIntegrador;
            private readonly UsuarioFilial objUsuarioFilial;
            private readonly UsuarioFilialPerfil objUsuarioFilialPerfil;
            public XmlDocument xml { get; set; }

            public Response(BLL.Integrador objIntegrador)
            {
                this.objIntegrador = objIntegrador;
                UsuarioFilialPerfil.CarregarUsuarioEmpresaPorFilial(objIntegrador.Filial.IdFilial,
                    out objUsuarioFilialPerfil);
                if (objUsuarioFilialPerfil == null)
                {
                    throw new Exception(string.Format("Nenhum UsuarioFilialPerfil encontrado para a filial id = {0}",
                        objIntegrador.Filial.IdFilial));
                }
                UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil,
                    out objUsuarioFilial);
                if (objUsuarioFilialPerfil == null)
                {
                    throw new Exception(string.Format(
                        "Nenhum UsuarioFilial encontrado para o UsuarioFilialPerfil = {0}",
                        objUsuarioFilialPerfil.IdUsuarioFilialPerfil));
                }
            }

            /// <summary>
            ///     Metodo mapeia as vagas recuperadas no XML para objetos VagaIntegracao.
            ///     Se a vaga ja esta cadastrada, retorna o objeto atual do banco.
            /// </summary>
            /// <returns>Lista de VagaIntegracao</returns>
            private IEnumerable<VagaIntegracao> MapearRetorno()
            {
                var elemList = xml.GetElementsByTagName("ad");
                for (var i = 0; i < elemList.Count; i++)
                {
                    var DescricaoCidade = elemList[i].SelectSingleNode("city").InnerText.Trim() + "/" +
                                          elemList[i].SelectSingleNode("region").InnerText.Trim();
                    var DescricaoFuncao = elemList[i].SelectSingleNode("title").InnerText.Trim();

                    /* Tratando a string recebida na tag Salary]
                     * Será recebido o padrao "R$ <Salario Inicial> a <Salario Final>"
                     */
                    //1 - Retirando os caracteres nao numericos e que nao sejam espacos (para evitar juntar a string em um numero so)
                    //2 - Retira os espacos em branco do inicio e fim da string
                    //3 - Faz um split pelo primeiro caracter nao numerico
                    decimal? ValorSalarioDe = null;
                    decimal? ValorSalarioPara = null;
                    try
                    {
                        var salarios =
                            Regex.Split(
                                Regex.Replace(elemList[i].SelectSingleNode("salary").InnerText.Trim(), "[^0-9, ]", "")
                                    .Trim(), "[^0-9,]");
                        decimal converteSalario;
                        if (salarios.Count() >= 1 && decimal.TryParse(salarios[0], out converteSalario))
                        {
                            ValorSalarioDe = converteSalario;
                        }
                        if (salarios.Count() == 3 && decimal.TryParse(salarios[2], out converteSalario))
                        {
                            ValorSalarioPara = converteSalario;
                        }
                    }
                    catch (Exception ex)
                    {
                        var customMessage = string.Format("URL: {0}, Vaga de integração: {0}",
                            objIntegrador.GetValorParametro(Parametro.Integracao_Url_Integracao),
                            elemList[i].SelectSingleNode("id").InnerText.Trim());
                        GerenciadorException.GravarExcecao(ex, customMessage);
                    }

                    yield return new VagaIntegracao
                    {
                        Integrador = objIntegrador,
                        CodigoVagaIntegrador = elemList[i].SelectSingleNode("id").InnerText.Trim(),
                        FlagInativo = false,
                        CidadeImportada = DescricaoCidade,
                        FuncaoImportada = DescricaoFuncao,
                        Vaga = new Vaga
                        {
                            ValorSalarioDe = ValorSalarioDe,
                            //Mesmo e-mail comercial do UsuarioFilial
                            EmailVaga = objUsuarioFilial.EmailComercial,
                            DescricaoRequisito = @elemList[i].SelectSingleNode("content").InnerText.Trim() + '\n' +
                                                 elemList[i].SelectSingleNode("studies").InnerText.Trim() + '\n' +
                                                 elemList[i].SelectSingleNode("contract").InnerText.Trim(),
                            QuantidadeVaga = 1,
                            NomeEmpresa = objIntegrador.Filial.RazaoSocial,
                            FlagInativo = false,
                            Filial = objIntegrador.Filial,
                            FlagConfidencial = false,
                            UsuarioFilialPerfil = objUsuarioFilialPerfil,
                            Escolaridade = null,
                            NumeroIdadeMinima = null,
                            NumeroIdadeMaxima = null,
                            Sexo = null,
                            DescricaoBeneficio = null,
                            DescricaoAtribuicoes = null,
                            NumeroDDD = null,
                            NumeroTelefone = null,
                            FlagReceberCadaCV = false,
                            FlagReceberTodosCV = false,
                            DescricaoFuncao = DescricaoFuncao,
                            FlagAuditada = false,
                            FlagBNERecomenda = false,
                            FlagVagaArquivada = false,
                            FlagVagaMassa = false,
                            Origem = new Origem(1),
                            FlagLiberada = false,
                            Deficiencia = null,
                            DataAuditoria = null,
                            ValorSalarioPara = ValorSalarioPara
                        }
                    };
                }
            }

            /// <summary>
            ///     Lê o XML do endereço presente na propriedade UrlIntegracao;
            /// </summary>
            /// <returns>Lista de objetos VagaIntegracao com as vagas presentes no xml.</returns>
            public IEnumerable<VagaIntegracao> RecuperarVagas()
            {
                var urlIntegracao = objIntegrador.GetValorParametro(Parametro.Integracao_Url_Integracao);
                try
                {
                    //Tentando obter o XML diretamente
                    var reader = new XmlTextReader(urlIntegracao);
                    xml = new XmlDocument();
                    xml.Load(reader);
                }
                catch
                {
                    //Se não conseguir obter o xml diretamente, tenta pegar a string e converter para xml.
                    var webRequest = WebRequest.Create(urlIntegracao);
                    var webResponse = webRequest.GetResponse();
                    var sr = new StreamReader(webResponse.GetResponseStream(), Encoding.Default);
                    try
                    {
                        var retorno = sr.ReadToEnd();
                        xml = new XmlDocument();
                        xml.LoadXml(retorno);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        sr.Close();
                    }
                }

                try
                {
                    return MapearRetorno();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}