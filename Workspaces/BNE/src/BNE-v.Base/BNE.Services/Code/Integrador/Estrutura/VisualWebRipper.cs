using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNE.BLL;
using System.Net;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;
using BNE.BLL.Custom;
using BNE.Services.Resources;

namespace BNE.Services.Code.Integrador.Estrutura
{
    public class VisualWebRipper
    {
        private XmlDocument xml = new XmlDocument();
        private List<SubstituicaoIntegracao> lstSubstituicoes;
        private String xmlPath = String.Empty;

        public IEnumerable<VagaIntegracao> RecuperarVagas(BLL.Integrador objIntegrador)
        {
            xmlPath = objIntegrador.GetValorParametro(BLL.Enumeradores.Parametro.Integracao_Url_Integracao);
            var sr = new StreamReader(xmlPath);
            try
            {
                string retorno = sr.ReadToEnd();
                lstSubstituicoes = SubstituicaoIntegracao.ListarSubstituicoesDeIntegrador(objIntegrador);
                xml.LoadXml(retorno);
                return MapearRetorno();
            }
            finally
            {
                sr.Close();
            }
        }

        private IEnumerable<BLL.VagaIntegracao> MapearRetorno()
        {
            XmlNodeList vagasList = xml.SelectNodes("//columns");

            MD5 md5Hash = MD5.Create();

            foreach (XmlNode vaga in vagasList)
            {
                VagaIntegracao objVagaIntegracao = new VagaIntegracao();
                objVagaIntegracao.Vaga = new Vaga();
                
                XmlNodeList camposVagaList = vaga.ChildNodes;
                
                //Detectando código da vaga do integrador
                string codigoVaga = string.Empty;
                foreach (XmlNode campoVaga in camposVagaList)
                {
                    if (campoVaga.Attributes["name"].Value.ToLower().Trim() == "codigo")
                    {
                        codigoVaga = campoVaga.InnerText;
                        break;
                    }
                }

                //Se não detectou código da vaga, grava exceção e continua a leitura das vagas
                if (String.IsNullOrEmpty(codigoVaga))
                {
                    Exception ex = new Exception("Código de vaga de intregração não especificado.");
                    EL.GerenciadorException.GravarExcecao(ex, String.Format("URL: {0}", xmlPath));
                    continue;
                }

                objVagaIntegracao.CodigoVagaIntegrador = codigoVaga;
                objVagaIntegracao.Vaga.CodigoVaga = codigoVaga;

                foreach (XmlNode campoVaga in camposVagaList)
                {
                    //Verificando se a vaga foi colocada como inativa
                    if (campoVaga.Attributes["Inativa"] != null &&
                        !String.IsNullOrEmpty(campoVaga.Attributes["Inativa"].Value) &&
                        campoVaga.Attributes["Inativa"].Value.ToLower() == "sim")
                    {
                        continue;
                    }

                    //Definindo valores default
                    if (objVagaIntegracao.Vaga.QuantidadeVaga == null)
                    {
                        objVagaIntegracao.Vaga.QuantidadeVaga = 1;
                    }

                    String valor = campoVaga.InnerText.Trim();
                    switch (campoVaga.Attributes["name"].Value.ToLower())
                    {
                        case "funcao":
                        case "Função":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Funcao);
                            objVagaIntegracao.FuncaoImportada = valor;
                            Funcao objFuncao;
                            if (Funcao.CarregarPorDescricao(valor, out objFuncao))
                            {
                                objVagaIntegracao.Vaga.Funcao = objFuncao;
                                objVagaIntegracao.Vaga.DescricaoFuncao = objFuncao.DescricaoFuncao;
                            }
                            else
                            {
                                objVagaIntegracao.Vaga.DescricaoFuncao = valor;
                            }
                            
                            break;
                        case "cidade":
                            objVagaIntegracao.CidadeImportada = valor;
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Cidade);
                            Cidade objCidade;
                            if (Cidade.CarregarPorNome(valor, out objCidade))
                            {
                                objVagaIntegracao.Vaga.Cidade = objCidade;
                            }
                            break;
                        case "escolaridade":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Escolaridade);
                            Escolaridade objEscolaridade;
                            Escolaridade.CarregarPorNome(valor, out objEscolaridade);
                            objVagaIntegracao.Vaga.Escolaridade = objEscolaridade;
                            break;
                        case "salario_de":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.SalarioDe);
                            if (Regex.IsMatch(valor,"[0-9.,]{5,}"))
                            {
                                objVagaIntegracao.Vaga.ValorSalarioDe = DetectarSalario(Regex.Match(valor, "[0-9.,]{5,}").Value);
                            }
                            break;
                        case "salario_ate":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.SalarioAte);
                            MatchCollection matches = Regex.Matches(valor, "[0-9.,]{5,}");
                            if (matches.Count > 0)
                            {
                                objVagaIntegracao.Vaga.ValorSalarioPara = DetectarSalario(matches[matches.Count-1].Value);
                            }
                            break;
                        case "beneficios":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Benefícios);
                            objVagaIntegracao.Vaga.DescricaoBeneficio = valor;
                            break;
                        case "idade_de":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.IdadeDe);
                            Int16 IdadeDe;
                            if (Int16.TryParse(valor, out IdadeDe))
                            {
                                objVagaIntegracao.Vaga.NumeroIdadeMinima = IdadeDe;
                            }
                            break;
                        case "idade_ate":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.IdadeAte);
                            Int16 IdadeAte;
                            if (Int16.TryParse(valor, out IdadeAte))
                            {
                                objVagaIntegracao.Vaga.NumeroIdadeMaxima = IdadeAte;
                            }
                            break;
                        case "sexo":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Sexo);
                            //Se string contiver homem, homens, masc ou maculino, considera sexo como sendo masculino
                            if (Regex.IsMatch(valor, "(home(m|ns){1}|masc(ulino){0,1})"))
                            {
                                if (!Regex.IsMatch(valor, "(mulher(es){0,1}|fem(inino){0,1})"))
                                {
                                    objVagaIntegracao.Vaga.Sexo = Sexo.LoadObject((int)BNE.BLL.Enumeradores.Sexo.Masculino);
                                }
                            }
                            else if (Regex.IsMatch(valor, "(mulher(es){0,1}|fem(inino){0,1})"))
                            {
                                objVagaIntegracao.Vaga.Sexo = Sexo.LoadObject((int)BNE.BLL.Enumeradores.Sexo.Feminino);
                            }
                            break;
                        case "numero_vagas":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.NumeroVagas);
                            Int16 NumeroVagas;
                            if (Int16.TryParse(valor.Replace(" ", ""), out NumeroVagas))
                            {
                                objVagaIntegracao.Vaga.QuantidadeVaga = NumeroVagas;
                            }
                            break;
                        case "disponibilidade":
                            if (String.IsNullOrEmpty(campoVaga.InnerText))
                                break;

                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Disponibilidade);
                            objVagaIntegracao.Vaga.DescricaoDisponibilidades = valor;
                            String[] disponibilidades = valor.Split(';');
                            objVagaIntegracao.Disponibilidades = new List<VagaDisponibilidade>();

                            foreach (string disponibilidade in disponibilidades)
                            {
                                try
                                {
                                    objVagaIntegracao.Disponibilidades.Add(new VagaDisponibilidade{ Disponibilidade = Disponibilidade.CarregarPorDescricao(disponibilidade) });
                                }
                                catch (Exception ex)
                                {
                                    EL.GerenciadorException.GravarExcecao(ex, String.Format("URL: {0} Codigo Vaga: {1}", xmlPath, objVagaIntegracao.CodigoVagaIntegrador));
                                }
                            }
                            break;
                        case "contrato":
                            if (String.IsNullOrEmpty(campoVaga.InnerText))
                                break;

                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Contrato);
                            objVagaIntegracao.Vaga.DescricaoTiposVinculo = valor;
                            String[] contratos = valor.Split(';');
                            objVagaIntegracao.TiposVinculo = new List<VagaTipoVinculo>();

                            foreach (string contrato in contratos)
                            {
                                try
                                {
                                    objVagaIntegracao.TiposVinculo.Add(new VagaTipoVinculo { TipoVinculo = TipoVinculo.CarregarPorDescricao(contrato) });
                                }
                                catch (Exception ex)
                                {
                                    EL.GerenciadorException.GravarExcecao(ex, String.Format("URL: {0} Codigo Vaga: {1}", xmlPath, objVagaIntegracao.CodigoVagaIntegrador));
                                }
                            }
                            break;
                        case "requisitos":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Requisitos);
                            objVagaIntegracao.Vaga.DescricaoRequisito = valor.Trim();
                            break;
                        case "atribuicoes":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Atribuicoes);
                            objVagaIntegracao.Vaga.DescricaoAtribuicoes = valor.Trim();
                            break;
                        case "deficiencia":
                            try
                            {
                                valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Deficiencia).Trim();
                                if (!String.IsNullOrEmpty(valor))
                                {
                                    objVagaIntegracao.Vaga.Deficiencia = Deficiencia.CarregarPorDescricao(valor);
                                }
                            }
                            catch (Exception ex)
                            {
                                EL.GerenciadorException.GravarExcecao(ex, String.Format("URL: {0} Codigo Vaga: {1}", xmlPath, objVagaIntegracao.CodigoVagaIntegrador));
                            }
                            break;
                        case "nome_fantasia":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.NomeFantasia);
                            objVagaIntegracao.Vaga.NomeEmpresa = valor.Trim();
                            break;
                        case "telefone":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Telefone);
                            objVagaIntegracao.Vaga.NumeroTelefone = valor.Trim();
                            break;
                        case "ddd_telefone":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.DDDTelefone);
                            objVagaIntegracao.Vaga.NumeroDDD = valor.Trim();
                            break;
                        case "confidencial":
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Confidencial);
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
                            valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.EmailRetorno);
                            objVagaIntegracao.Vaga.EmailVaga = valor.Trim().ToLower();
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
        /// Método para aplicar as regras de substituição nos campos da vaga importada.
        /// </summary>
        /// <param name="lstSubstituicoes">Lista com as substituições a serem efetuadas</param>
        /// <param name="objVagaIntegracao">Objeto vaga onde as substituições serão aplicadas</param>
        private String AplicarSubstituicoes(String descricao, BNE.BLL.Enumeradores.CampoIntegrador campo)
        {
            //Lista de substituições para todos os campos
            List<SubstituicaoIntegracao> lstSubstituicoesTodos = lstSubstituicoes.Where(s => s.RegraSubstituicaoIntegracao == null ||
                                                                                             s.RegraSubstituicaoIntegracao.CampoIntegrador == null).ToList();
            foreach (var objSubstituicao in lstSubstituicoesTodos)
            {
                descricao = objSubstituicao.AplicarSubstituicao(descricao);
            }

            //Lista de substituições para o campo
            List<SubstituicaoIntegracao> lstSubstituicoesCampo = lstSubstituicoes.Where(s => s.RegraSubstituicaoIntegracao != null &&
                                                                                             s.RegraSubstituicaoIntegracao.CampoIntegrador == campo).ToList();
            foreach (var objSubstituicao in lstSubstituicoesCampo)
            {
                descricao = objSubstituicao.AplicarSubstituicao(descricao);
            }

            return descricao;
        }
        #endregion

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        static Decimal? DetectarSalario(string salario)
        {
            Int32? vlrDecimais = null;

            //Detectando valor das casas decimais
            Regex rCasasDecimais = new Regex("(?<=[.,])[0-9]{0,2}$");
            if (rCasasDecimais.IsMatch(salario))
            {
                string decimais = rCasasDecimais.Match(salario).Value;
                if (decimais.Length < 2)
                {
                    decimais += "0";
                }
                vlrDecimais = Int32.Parse(decimais);
                salario = rCasasDecimais.Replace(salario, "");
            }

            //Retirando todos os caracteres não numéricos
            salario = Regex.Replace(salario, "[^0-9]", "");

            //vlrDecimais não foi detectado pela pontuação
            if (vlrDecimais == null)
            {
                if (salario.Length >= 5)
                {
                    //Se o salário tem mais de 5 posições, a pontuação de casas decimais não foi indicada
                    vlrDecimais = Int32.Parse(salario.Substring(salario.Length-2, 2));
                    salario = salario.Substring(0, salario.Length - 2);
                }
                else
                {
                    //Se o comprimento for menor que 5, as casas decimais não foram indicadas
                    vlrDecimais = 0;
                }
            }

            Decimal salarioDetectado;
            if (Decimal.TryParse(salario, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, new CultureInfo("pt-BR"), out salarioDetectado))
            {
                salarioDetectado += (Decimal)vlrDecimais.Value / 100;
                return salarioDetectado;
            }
            return null;
        }
    }
}
