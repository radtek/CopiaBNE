using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.EL;
using CampoIntegrador = BNE.BLL.Enumeradores.CampoIntegrador;
using Parametro = BNE.BLL.Enumeradores.Parametro;

namespace BNE.Services.Code.Integrador.Estrutura.Assincrono
{
    public class Ripper
    {
        private List<SubstituicaoIntegracao> lstSubstituicoes;
        private readonly XmlDocument xml = new XmlDocument();
        private string xmlPath = string.Empty;

        public IEnumerable<VagaIntegracao> RecuperarVagas(BLL.Integrador objIntegrador)
        {
            xmlPath = objIntegrador.GetValorParametro(Parametro.Integracao_Url_Integracao);

            var sr = new StreamReader(xmlPath);
            try
            {
                var retorno = sr.ReadToEnd();
                lstSubstituicoes = SubstituicaoIntegracao.ListarSubstituicoesDeIntegrador(objIntegrador);
                xml.LoadXml(retorno);
                return MapearRetorno();
            }
            finally
            {
                sr.Close();
            }
        }

        private IEnumerable<VagaIntegracao> MapearRetorno()
        {
            var vagasList = xml.SelectNodes("//columns");

            var md5Hash = MD5.Create();

            foreach (XmlNode vaga in vagasList)
            {
                var objVagaIntegracao = new VagaIntegracao();
                objVagaIntegracao.Vaga = new Vaga();

                var camposVagaList = vaga.ChildNodes;

                //Detectando código da vaga do integrador
                var codigoVaga = string.Empty;
                foreach (XmlNode campoVaga in camposVagaList)
                {
                    if (campoVaga.Attributes["name"].Value.ToLower().Trim() == "codigo")
                    {
                        codigoVaga = campoVaga.InnerText;
                        break;
                    }
                }

                //Se não detectou código da vaga, grava exceção e continua a leitura das vagas
                if (string.IsNullOrEmpty(codigoVaga))
                {
                    var ex = new Exception("Código de vaga de intregração não especificado.");
                    GerenciadorException.GravarExcecao(ex, string.Format("URL: {0}", xmlPath));
                    continue;
                }

                objVagaIntegracao.CodigoVagaIntegrador = codigoVaga;
                objVagaIntegracao.Vaga.CodigoVaga = codigoVaga;

                foreach (XmlNode campoVaga in camposVagaList)
                {
                    //Verificando se a vaga foi colocada como inativa
                    if (campoVaga.Attributes["Inativa"] != null &&
                        !string.IsNullOrEmpty(campoVaga.Attributes["Inativa"].Value) &&
                        campoVaga.Attributes["Inativa"].Value.ToLower() == "sim")
                    {
                        continue;
                    }

                    //Definindo valores default
                    if (objVagaIntegracao.Vaga.QuantidadeVaga == null)
                    {
                        objVagaIntegracao.Vaga.QuantidadeVaga = 1;
                    }

                    var valor = campoVaga.InnerText.Trim();
                    switch (campoVaga.Attributes["name"].Value.ToLower())
                    {
                        case "funcao":
                        case "Função":
                            objVagaIntegracao.FuncaoImportada = valor;
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Funcao);
                            break;
                        case "cidade":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Cidade);
                            objVagaIntegracao.CidadeImportada = valor;
                            break;
                        case "escolaridade":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Escolaridade);
                            objVagaIntegracao.EscolaridadeImportada = valor;
                            break;
                        case "salario_de":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.SalarioDe);
                            objVagaIntegracao.Vaga.ValorSalarioDe = DetectarSalario(valor);
                            break;
                        case "salario_ate":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.SalarioAte);
                            objVagaIntegracao.Vaga.ValorSalarioPara = DetectarSalario(valor);
                            break;
                        case "beneficios":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Benefícios);
                            objVagaIntegracao.Vaga.DescricaoBeneficio = valor;
                            break;
                        case "idade_de":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.IdadeDe);
                            short IdadeDe;
                            if (short.TryParse(valor, out IdadeDe))
                            {
                                objVagaIntegracao.Vaga.NumeroIdadeMinima = IdadeDe;
                            }
                            break;
                        case "idade_ate":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.IdadeAte);
                            short IdadeAte;
                            if (short.TryParse(valor, out IdadeAte))
                            {
                                objVagaIntegracao.Vaga.NumeroIdadeMaxima = IdadeAte;
                            }
                            break;
                        case "sexo":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Sexo);
                            //Se string contiver homem, homens, masc ou maculino, considera sexo como sendo masculino
                            if (Regex.IsMatch(valor, "(home(m|ns){1}|masc(ulino){0,1})"))
                            {
                                if (!Regex.IsMatch(valor, "(mulher(es){0,1}|fem(inino){0,1})"))
                                {
                                    objVagaIntegracao.Vaga.Sexo = Sexo.LoadObject((int) BLL.Enumeradores.Sexo.Masculino);
                                }
                            }
                            else if (Regex.IsMatch(valor, "(mulher(es){0,1}|fem(inino){0,1})"))
                            {
                                objVagaIntegracao.Vaga.Sexo = Sexo.LoadObject((int) BLL.Enumeradores.Sexo.Feminino);
                            }
                            break;
                        case "numero_vagas":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.NumeroVagas);
                            short NumeroVagas;
                            if (short.TryParse(valor.Replace(" ", ""), out NumeroVagas))
                            {
                                objVagaIntegracao.Vaga.QuantidadeVaga = NumeroVagas;
                            }
                            break;
                        case "disponibilidade":
                            if (string.IsNullOrEmpty(campoVaga.InnerText))
                                break;

                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Disponibilidade);
                            objVagaIntegracao.Vaga.DescricaoDisponibilidades = valor;

                            break;
                        case "contrato":
                            if (string.IsNullOrEmpty(campoVaga.InnerText))
                                break;

                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Contrato);
                            objVagaIntegracao.Vaga.DescricaoTiposVinculo = valor;

                            break;
                        case "requisitos":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Requisitos);
                            objVagaIntegracao.Vaga.DescricaoRequisito = valor.Trim();
                            break;
                        case "atribuicoes":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Atribuicoes);
                            objVagaIntegracao.Vaga.DescricaoAtribuicoes = valor.Trim();
                            break;
                        case "deficiencia":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Deficiencia).Trim();
                            objVagaIntegracao.DeficienciaImportada = valor;
                            break;
                        case "nome_fantasia":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.NomeFantasia);
                            objVagaIntegracao.Vaga.NomeEmpresa = valor.Trim();
                            break;
                        case "telefone":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Telefone);
                            objVagaIntegracao.Vaga.NumeroTelefone = valor.Trim();
                            break;
                        case "ddd_telefone":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.DDDTelefone);
                            objVagaIntegracao.Vaga.NumeroDDD = valor.Trim();
                            break;
                        case "confidencial":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.Confidencial);
                            if (valor.Trim().ToLower() == "sim" ||
                                valor.Trim().ToLower() == "1" ||
                                valor.Trim().ToLower() == "s")
                            {
                                objVagaIntegracao.Vaga.FlagConfidencial = true;
                            }
                            else
                            {
                                objVagaIntegracao.Vaga.FlagConfidencial = false;
                            }
                            break;
                        case "email_retorno":
                            valor = AplicarSubstituicoes(valor, CampoIntegrador.EmailRetorno);
                            objVagaIntegracao.Vaga.EmailVaga = Helper.RemoverCaracteresBrancos(valor.Trim().ToLower());

                            break;
                        case "deficiente":
                            if (valor.ToLower() == "sim")
                            {
                                objVagaIntegracao.VagaParaDeficiente = true;
                                objVagaIntegracao.Vaga.FlagDeficiencia = true;
                            }
                            break;
                        default:
                            //vagaImportacao.DescricaoVaga += String.Format("<BR/><b>{0}</b>:{1}", campoVaga.Attributes["name"].Value.Trim(), campoVaga.InnerText.Trim());
                            break;
                    }
                }

                //Se existe uma deficiencia indicada, a FlagDeficiencia é setada para true.
                if (objVagaIntegracao.Vaga.Deficiencia != null && objVagaIntegracao.Vaga.Deficiencia.IdDeficiencia > 0)
                {
                    objVagaIntegracao.VagaParaDeficiente = true;
                    objVagaIntegracao.Vaga.FlagDeficiencia = true;
                }

                yield return objVagaIntegracao;
            }
        }

        #region AplicarSubstituicoes
        /// <summary>
        ///     Método para aplicar as regras de substituição nos campos da vaga importada.
        /// </summary>
        /// <param name="lstSubstituicoes">Lista com as substituições a serem efetuadas</param>
        /// <param name="objVagaIntegracao">Objeto vaga onde as substituições serão aplicadas</param>
        private string AplicarSubstituicoes(string descricao, CampoIntegrador campo)
        {
            //Lista de substituições para todos os campos
            var lstSubstituicoesTodos = lstSubstituicoes.Where(s => s.RegraSubstituicaoIntegracao == null ||
                                                                    s.RegraSubstituicaoIntegracao.CampoIntegrador ==
                                                                    null).ToList();
            foreach (var objSubstituicao in lstSubstituicoesTodos)
            {
                descricao = objSubstituicao.AplicarSubstituicao(descricao);
            }

            //Lista de substituições para o campo
            var lstSubstituicoesCampo = lstSubstituicoes.Where(s => s.RegraSubstituicaoIntegracao != null &&
                                                                    s.RegraSubstituicaoIntegracao.CampoIntegrador ==
                                                                    campo).ToList();
            foreach (var objSubstituicao in lstSubstituicoesCampo)
            {
                descricao = objSubstituicao.AplicarSubstituicao(descricao);
            }

            return descricao;
        }
        #endregion

        #region DetectarSalario
        private static decimal? DetectarSalario(string strVlrSalario)
        {
            try
            {
                decimal Salario;
                strVlrSalario = Regex.Replace(strVlrSalario, @"(R\$|r\$)", "");
                CultureInfo providere = new CultureInfo("pt-BR");

                //Verifica se possui decimais
                var decimaisEncontrados = Regex.Match(strVlrSalario, @"([\.|\,]\d{1,2})?$");

                if (decimaisEncontrados.Length > 0) //numero quebrado
                {
                    decimal vlrDecimal = decimal.Parse(decimaisEncontrados.ToString().Replace(".", ","), providere);
                    strVlrSalario = strVlrSalario.Replace(decimaisEncontrados.ToString(), "");
                    strVlrSalario = strVlrSalario.Replace(".", "");
                    strVlrSalario = strVlrSalario.Replace(",", "");

                    Salario = decimal.Parse(strVlrSalario) + vlrDecimal;
                }
                else //numero inteiro
                {
                    strVlrSalario = strVlrSalario.Replace(".", "");
                    strVlrSalario = strVlrSalario.Replace(",", "");
                    Salario = decimal.Parse(strVlrSalario);
                }

                if (Salario > 35000)
                    Salario = Salario / 100;

                return Salario;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

    }
}