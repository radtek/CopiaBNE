using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using BNE.BLL;
using BNE.EL;
using CampoIntegrador = BNE.BLL.Enumeradores.CampoIntegrador;
using Parametro = BNE.BLL.Enumeradores.Parametro;

namespace BNE.Services.Code.Integrador.Estrutura
{
    public class Bne
    {
        private BLL.Custom.IntegrationObjects.Bne deserializedBNE;
        private List<SubstituicaoIntegracao> lstSubstituicoes;
        private string xmlPath = string.Empty;

        public IEnumerable<VagaIntegracao> RecuperarVagas(BLL.Integrador objIntegrador)
        {
            xmlPath = objIntegrador.GetValorParametro(Parametro.Integracao_Url_Integracao);

            var uri = new Uri(xmlPath);
            StreamReader sr;

            //Verificando se o caminho passado é um arquivo
            if (uri.IsFile)
            {
                sr = new StreamReader(xmlPath);
            }
            else
            {
                //Se não é arquivo, faz um WebRequest
                var webRequest = WebRequest.Create(xmlPath);
                var webResponse = webRequest.GetResponse();
                sr = new StreamReader(webResponse.GetResponseStream());
            }

            try
            {
                deserializedBNE = BLL.Custom.IntegrationObjects.Bne.Deserialize(sr);
                lstSubstituicoes = SubstituicaoIntegracao.ListarSubstituicoesDeIntegrador(objIntegrador);
                return MapearRetorno();
            }
            finally
            {
                sr.Close();
            }
        }

        private IEnumerable<VagaIntegracao> MapearRetorno()
        {
            foreach (var vaga in deserializedBNE.Vagas)
            {
                var objVagaIntegracao = new VagaIntegracao();
                objVagaIntegracao.Vaga = new Vaga();


                objVagaIntegracao.CodigoVagaIntegrador = vaga._codigo;
                objVagaIntegracao.Vaga.CodigoVaga = vaga._codigo;

                //Verificando se a vaga foi colocada como inativa
                if (vaga.Inativa)
                {
                    continue;
                }

                var valor = string.Empty;

                valor = AplicarSubstituicoes(vaga._funcao, CampoIntegrador.Funcao);
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

                valor = vaga.Cidade._nome + "/" + vaga.Cidade._estado;
                objVagaIntegracao.CidadeImportada = valor;
                valor = AplicarSubstituicoes(valor, CampoIntegrador.Cidade);
                Cidade objCidade;
                if (Cidade.CarregarPorNome(valor, out objCidade))
                {
                    objVagaIntegracao.Vaga.Cidade = objCidade;
                }

                if (!string.IsNullOrEmpty(vaga._escolaridade))
                {
                    valor = AplicarSubstituicoes(vaga._escolaridade, CampoIntegrador.Escolaridade);
                    Escolaridade objEscolaridade;
                    Escolaridade.CarregarPorNome(valor, out objEscolaridade);
                    objVagaIntegracao.Vaga.Escolaridade = objEscolaridade;
                }

                if (vaga.SalarioDe > 0)
                {
                    objVagaIntegracao.Vaga.ValorSalarioDe = vaga.SalarioDe;
                }

                if (vaga.SalarioAte > 0)
                {
                    objVagaIntegracao.Vaga.ValorSalarioPara = vaga.SalarioAte;
                }

                valor = vaga._beneficios;
                valor = AplicarSubstituicoes(valor, CampoIntegrador.Benefícios);
                objVagaIntegracao.Vaga.DescricaoBeneficio = valor;

                if (vaga.IdadeDe > 0)
                {
                    objVagaIntegracao.Vaga.NumeroIdadeMinima = Convert.ToInt16(vaga.IdadeDe);
                }

                if (vaga.IdadeAte > 0)
                {
                    objVagaIntegracao.Vaga.NumeroIdadeMaxima = Convert.ToInt16(vaga.IdadeAte);
                }

                if (!string.IsNullOrEmpty(vaga._sexo))
                {
                    valor = AplicarSubstituicoes(vaga._sexo, CampoIntegrador.Sexo);
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
                }

                if (vaga.NumeroVagas > 0)
                {
                    objVagaIntegracao.Vaga.QuantidadeVaga = Convert.ToInt16(vaga.NumeroVagas);
                }
                else
                {
                    objVagaIntegracao.Vaga.QuantidadeVaga = 1;
                }

                if (!string.IsNullOrEmpty(vaga._disponibilidade))
                {
                    valor = AplicarSubstituicoes(vaga._disponibilidade, CampoIntegrador.Disponibilidade);
                    objVagaIntegracao.Vaga.DescricaoDisponibilidades = valor;
                    var disponibilidades = valor.Split(';');
                    objVagaIntegracao.Disponibilidades = new List<VagaDisponibilidade>();

                    foreach (var disponibilidade in disponibilidades)
                    {
                        try
                        {
                            objVagaIntegracao.Disponibilidades.Add(new VagaDisponibilidade
                            {
                                Disponibilidade = Disponibilidade.CarregarPorDescricao(disponibilidade)
                            });
                        }
                        catch (Exception ex)
                        {
                            GerenciadorException.GravarExcecao(ex,
                                string.Format("URL: {0} Codigo Vaga: {1}", xmlPath,
                                    objVagaIntegracao.CodigoVagaIntegrador));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(vaga._contrato))
                {
                    valor = AplicarSubstituicoes(vaga._contrato, CampoIntegrador.Contrato);
                    objVagaIntegracao.Vaga.DescricaoTiposVinculo = valor;
                    var contratos = valor.Split(';');
                    objVagaIntegracao.TiposVinculo = new List<VagaTipoVinculo>();

                    foreach (var contrato in contratos)
                    {
                        try
                        {
                            objVagaIntegracao.TiposVinculo.Add(new VagaTipoVinculo
                            {
                                TipoVinculo = TipoVinculo.CarregarPorDescricao(contrato)
                            });
                        }
                        catch (Exception ex)
                        {
                            GerenciadorException.GravarExcecao(ex,
                                string.Format("URL: {0} Codigo Vaga: {1}", xmlPath,
                                    objVagaIntegracao.CodigoVagaIntegrador));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(vaga._requisitos))
                {
                    valor = AplicarSubstituicoes(vaga._requisitos, CampoIntegrador.Requisitos);
                    objVagaIntegracao.Vaga.DescricaoRequisito = valor.Trim();
                }

                if (!string.IsNullOrEmpty(vaga._atribuicoes))
                {
                    valor = AplicarSubstituicoes(vaga._atribuicoes, CampoIntegrador.Atribuicoes);
                    objVagaIntegracao.Vaga.DescricaoAtribuicoes = valor.Trim();
                }

                if (!string.IsNullOrEmpty(vaga._deficiencia))
                {
                    try
                    {
                        valor = AplicarSubstituicoes(vaga._deficiencia, CampoIntegrador.Deficiencia).Trim();
                        if (!string.IsNullOrEmpty(valor))
                        {
                            objVagaIntegracao.Vaga.Deficiencia = Deficiencia.CarregarPorDescricao(valor);
                        }
                    }
                    catch (Exception ex)
                    {
                        GerenciadorException.GravarExcecao(ex,
                            string.Format("URL: {0} Codigo Vaga: {1}", xmlPath, objVagaIntegracao.CodigoVagaIntegrador));
                    }
                }

                if (!string.IsNullOrEmpty(vaga._empresa))
                {
                    valor = AplicarSubstituicoes(vaga._empresa, CampoIntegrador.NomeFantasia);
                    objVagaIntegracao.Vaga.NomeEmpresa = valor.Trim();
                }

                if (vaga.TelefoneRetorno != null && vaga.TelefoneRetorno.DDD > 0 &&
                    !string.IsNullOrEmpty(vaga.TelefoneRetorno._numero))
                {
                    valor = AplicarSubstituicoes(vaga.TelefoneRetorno._numero, CampoIntegrador.Telefone);
                    objVagaIntegracao.Vaga.NumeroTelefone = valor.Trim();

                    valor = AplicarSubstituicoes(vaga.TelefoneRetorno.DDD.ToString(), CampoIntegrador.DDDTelefone);
                    objVagaIntegracao.Vaga.NumeroDDD = valor.Trim();
                }

                objVagaIntegracao.Vaga.FlagConfidencial = vaga.Confidencial;

                if (!string.IsNullOrEmpty(vaga._emailRetorno))
                {
                    valor = AplicarSubstituicoes(vaga._emailRetorno, CampoIntegrador.EmailRetorno);
                    objVagaIntegracao.Vaga.EmailVaga = valor.Trim().ToLower();
                }

                //Se existe uma deficiencia indicada, a FlagDeficiencia é setada para true.
                if (!string.IsNullOrEmpty(vaga._deficiencia) ||
                    (objVagaIntegracao.Vaga.Deficiencia != null && objVagaIntegracao.Vaga.Deficiencia.IdDeficiencia > 0))
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

        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        private static decimal? DetectarSalario(string salario)
        {
            int? vlrDecimais = null;

            //Detectando valor das casas decimais
            var rCasasDecimais = new Regex("(?<=[.,])[0-9]{0,2}$");
            if (rCasasDecimais.IsMatch(salario))
            {
                var decimais = rCasasDecimais.Match(salario).Value;
                if (decimais.Length < 2)
                {
                    decimais += "0";
                }
                vlrDecimais = int.Parse(decimais);
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
                    vlrDecimais = int.Parse(salario.Substring(salario.Length - 2, 2));
                    salario = salario.Substring(0, salario.Length - 2);
                }
                else
                {
                    //Se o comprimento for menor que 5, as casas decimais não foram indicadas
                    vlrDecimais = 0;
                }
            }

            decimal salarioDetectado;
            if (decimal.TryParse(salario, NumberStyles.Number | NumberStyles.AllowCurrencySymbol,
                new CultureInfo("pt-BR"), out salarioDetectado))
            {
                salarioDetectado += (decimal) vlrDecimais.Value/100;
                return salarioDetectado;
            }
            return null;
        }
    }
}