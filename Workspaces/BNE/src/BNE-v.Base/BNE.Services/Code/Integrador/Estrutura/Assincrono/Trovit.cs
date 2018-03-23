using BNE.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace BNE.Services.Code.Integrador.Estrutura.Assincrono
{
    public class Trovit
    {
        public class Response
        {
            public Response(BLL.Integrador objIntegrador)
            {
                this.objIntegrador = objIntegrador;
                BLL.UsuarioFilialPerfil.CarregarUsuarioEmpresaPorFilial(objIntegrador.Filial.IdFilial, out objUsuarioFilialPerfil);
                if (objUsuarioFilialPerfil == null)
                {
                    throw new Exception(String.Format("Nenhum UsuarioFilialPerfil encontrado para a filial id = {0}", objIntegrador.Filial.IdFilial));
                }
                BLL.UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial);
                if (objUsuarioFilialPerfil == null)
                {
                    throw new Exception(String.Format("Nenhum UsuarioFilial encontrado para o UsuarioFilialPerfil = {0}", objUsuarioFilialPerfil.IdUsuarioFilialPerfil));
                }
            }
            private BLL.Integrador objIntegrador;
            private BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
            private BLL.UsuarioFilial objUsuarioFilial;
            public XmlDocument xml { get; set; }

            /// <summary>
            /// Metodo mapeia as vagas recuperadas no XML para objetos VagaIntegracao.
            /// Se a vaga ja esta cadastrada, retorna o objeto atual do banco.
            /// </summary>
            /// <returns>Lista de VagaIntegracao</returns>
            private IEnumerable<VagaIntegracao> MapearRetorno()
            {
                XmlNodeList elemList = xml.GetElementsByTagName("ad");
                for (int i = 0; i < elemList.Count; i++)
                {
                    string DescricaoCidade = elemList[i].SelectSingleNode("city").InnerText.Trim() + "/" + elemList[i].SelectSingleNode("region").InnerText.Trim();
                    string DescricaoFuncao = elemList[i].SelectSingleNode("title").InnerText.Trim();                       

                    /* Tratando a string recebida na tag Salary]
                     * Será recebido o padrao "R$ <Salario Inicial> a <Salario Final>"
                     */
                    //1 - Retirando os caracteres nao numericos e que nao sejam espacos (para evitar juntar a string em um numero so)
                    //2 - Retira os espacos em branco do inicio e fim da string
                    //3 - Faz um split pelo primeiro caracter nao numerico
                    Decimal? ValorSalarioDe = null;
                    Decimal? ValorSalarioPara = null;
                    try
                    {
                        string[] salarios = Regex.Split(Regex.Replace(elemList[i].SelectSingleNode("salary").InnerText.Trim(), "[^0-9, ]", "").Trim(), "[^0-9,]");
                        Decimal converteSalario;
                        if (salarios.Count() >= 1 && Decimal.TryParse(salarios[0], out converteSalario))
                        {
                            ValorSalarioDe = converteSalario;
                        }
                        if (salarios.Count() == 3 && Decimal.TryParse(salarios[2], out converteSalario))
                        {
                            ValorSalarioPara = converteSalario;
                        }
                    }
                    catch (Exception ex)
                    {
                        string customMessage = string.Format("URL: {0}, Vaga de integração: {0}", objIntegrador.GetValorParametro(BLL.Enumeradores.Parametro.Integracao_Url_Integracao), elemList[i].SelectSingleNode("id").InnerText.Trim());
                        EL.GerenciadorException.GravarExcecao(ex, customMessage);
                    }

                    yield return new VagaIntegracao
                    {
                        Integrador = this.objIntegrador,
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
                            FlagVagaRapida = false,
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
                            ValorSalarioPara = ValorSalarioPara,
                        }
                    };
                }
            }

            /// <summary>
            /// Lê o XML do endereço presente na propriedade UrlIntegracao;
            /// </summary>
            /// <returns>Lista de objetos VagaIntegracao com as vagas presentes no xml.</returns>
            public IEnumerable<VagaIntegracao> RecuperarVagas()
            {
                String urlIntegracao = objIntegrador.GetValorParametro(BLL.Enumeradores.Parametro.Integracao_Url_Integracao);
                try
                {
                    //Tentando obter o XML diretamente
                    XmlTextReader reader = new XmlTextReader(urlIntegracao);
                    xml = new XmlDocument();
                    xml.Load(reader);
                }
                catch
                {
                    //Se não conseguir obter o xml diretamente, tenta pegar a string e converter para xml.
                    var webRequest = WebRequest.Create(urlIntegracao);
                    var webResponse = webRequest.GetResponse();
                    StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.Default);
                    try
                    {
                        string retorno = sr.ReadToEnd();
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
                    return this.MapearRetorno();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
