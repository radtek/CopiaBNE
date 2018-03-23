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
    public class Bne
    {
        private List<SubstituicaoIntegracao> lstSubstituicoes;
        BNE.BLL.Custom.IntegrationObjects.Bne deserializedBNE;
        private String xmlPath = String.Empty;

        public IEnumerable<VagaIntegracao> RecuperarVagas(BLL.Integrador objIntegrador)
        {
            xmlPath = objIntegrador.GetValorParametro(BLL.Enumeradores.Parametro.Integracao_Url_Integracao);

            Uri uri = new Uri(xmlPath);
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
                deserializedBNE = BNE.BLL.Custom.IntegrationObjects.Bne.Deserialize(sr);
                lstSubstituicoes = SubstituicaoIntegracao.ListarSubstituicoesDeIntegrador(objIntegrador);
                return MapearRetorno();
            }
            finally
            {
                sr.Close();
            }
        }

        private IEnumerable<BLL.VagaIntegracao> MapearRetorno()
        {
            foreach (BNE.BLL.Custom.IntegrationObjects.VagaBne vaga in deserializedBNE.Vagas)
            {
                VagaIntegracao objVagaIntegracao = new VagaIntegracao();
                objVagaIntegracao.Vaga = new Vaga();
                              

                objVagaIntegracao.CodigoVagaIntegrador = vaga._codigo;
                objVagaIntegracao.Vaga.CodigoVaga = vaga._codigo;

                //Verificando se a vaga foi colocada como inativa
                if (vaga.Inativa)
                {
                    continue;
                }

                String valor = String.Empty;
                
                valor = AplicarSubstituicoes(vaga._funcao, BLL.Enumeradores.CampoIntegrador.Funcao);
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
                valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Cidade);
                Cidade objCidade;
                if (Cidade.CarregarPorNome(valor, out objCidade))
                {
                    objVagaIntegracao.Vaga.Cidade = objCidade;
                }

                if (!String.IsNullOrEmpty(vaga._escolaridade))
                {
                    valor = AplicarSubstituicoes(vaga._escolaridade, BLL.Enumeradores.CampoIntegrador.Escolaridade);
                    Escolaridade objEscolaridade;
                    Escolaridade.CarregarPorNome(valor, out objEscolaridade);
                    objVagaIntegracao.Vaga.Escolaridade = objEscolaridade;
                }

                if(vaga.SalarioDe > 0){
                    objVagaIntegracao.Vaga.ValorSalarioDe = vaga.SalarioDe;
                }

                if(vaga.SalarioAte > 0){
                    objVagaIntegracao.Vaga.ValorSalarioPara = vaga.SalarioAte;
                }

                valor = vaga._beneficios;
                valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Benefícios);
                objVagaIntegracao.Vaga.DescricaoBeneficio = valor;

                if (vaga.IdadeDe > 0)
	            {
                    objVagaIntegracao.Vaga.NumeroIdadeMinima = Convert.ToInt16(vaga.IdadeDe);
	            }
                
                if (vaga.IdadeAte > 0)
	            {
                    objVagaIntegracao.Vaga.NumeroIdadeMaxima = Convert.ToInt16(vaga.IdadeAte);
	            }

                if (!String.IsNullOrEmpty(vaga._sexo))
	            {
                    valor = AplicarSubstituicoes(vaga._sexo, BLL.Enumeradores.CampoIntegrador.Sexo);
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
	            }
                    
                if (vaga.NumeroVagas > 0)
	            {
                    objVagaIntegracao.Vaga.QuantidadeVaga = Convert.ToInt16(vaga.NumeroVagas);
	            }else{
                    objVagaIntegracao.Vaga.QuantidadeVaga = 1;
                }

                if (!String.IsNullOrEmpty(vaga._disponibilidade))
                {
                    valor = AplicarSubstituicoes(vaga._disponibilidade, BLL.Enumeradores.CampoIntegrador.Disponibilidade);
                    objVagaIntegracao.Vaga.DescricaoDisponibilidades = valor;
                    String[] disponibilidades = valor.Split(';');
                    objVagaIntegracao.Disponibilidades = new List<VagaDisponibilidade>();

                    foreach (string disponibilidade in disponibilidades)
                    {
                        try
                        {
                            objVagaIntegracao.Disponibilidades.Add(new VagaDisponibilidade { Disponibilidade = Disponibilidade.CarregarPorDescricao(disponibilidade) });
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex, String.Format("URL: {0} Codigo Vaga: {1}", xmlPath, objVagaIntegracao.CodigoVagaIntegrador));
                        }
                    }
                }

                if (!String.IsNullOrEmpty(vaga._contrato))
                {
                    valor = AplicarSubstituicoes(vaga._contrato, BLL.Enumeradores.CampoIntegrador.Contrato);
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
                }

                if (!String.IsNullOrEmpty(vaga._requisitos))
                {
                    valor = AplicarSubstituicoes(vaga._requisitos, BLL.Enumeradores.CampoIntegrador.Requisitos);
                    objVagaIntegracao.Vaga.DescricaoRequisito = valor.Trim();
                }

                if (!String.IsNullOrEmpty(vaga._atribuicoes))
                {
                    valor = AplicarSubstituicoes(vaga._atribuicoes, BLL.Enumeradores.CampoIntegrador.Atribuicoes);
                    objVagaIntegracao.Vaga.DescricaoAtribuicoes = valor.Trim();
                }

                if (!String.IsNullOrEmpty(vaga._deficiencia))
                {
                    try
                    {
                        valor = AplicarSubstituicoes(vaga._deficiencia, BLL.Enumeradores.CampoIntegrador.Deficiencia).Trim();
                        if (!String.IsNullOrEmpty(valor))
                        {
                            objVagaIntegracao.Vaga.Deficiencia = Deficiencia.CarregarPorDescricao(valor);
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, String.Format("URL: {0} Codigo Vaga: {1}", xmlPath, objVagaIntegracao.CodigoVagaIntegrador));
                    }
                }

                if (!String.IsNullOrEmpty(vaga._empresa))
                {
                    valor = AplicarSubstituicoes(vaga._empresa, BLL.Enumeradores.CampoIntegrador.NomeFantasia);
                    objVagaIntegracao.Vaga.NomeEmpresa = valor.Trim();
                }
                     
                if (vaga.TelefoneRetorno != null && vaga.TelefoneRetorno.DDD > 0 && !String.IsNullOrEmpty(vaga.TelefoneRetorno._numero))
	            {
                    valor = AplicarSubstituicoes(vaga.TelefoneRetorno._numero, BLL.Enumeradores.CampoIntegrador.Telefone);
                    objVagaIntegracao.Vaga.NumeroTelefone = valor.Trim();

                    valor = AplicarSubstituicoes(vaga.TelefoneRetorno.DDD.ToString(), BLL.Enumeradores.CampoIntegrador.DDDTelefone);
                    objVagaIntegracao.Vaga.NumeroDDD = valor.Trim();
                }
                        
                objVagaIntegracao.Vaga.FlagConfidencial = vaga.Confidencial;

                if (!String.IsNullOrEmpty(vaga._emailRetorno))
                {
                    valor = AplicarSubstituicoes(vaga._emailRetorno, BLL.Enumeradores.CampoIntegrador.EmailRetorno);
                    objVagaIntegracao.Vaga.EmailVaga = valor.Trim().ToLower();
                }
                
                //Se existe uma deficiencia indicada, a FlagDeficiencia é setada para true.
                if (!String.IsNullOrEmpty(vaga._deficiencia) || (objVagaIntegracao.Vaga.Deficiencia != null && objVagaIntegracao.Vaga.Deficiencia.IdDeficiencia > 0))
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
