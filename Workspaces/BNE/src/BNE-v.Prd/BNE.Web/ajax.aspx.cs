using System;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.BLL.Common;
using System.Net;
using System.IO;
using BNE.BLL.DTO.SINE;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;
using BNE.Web.Code.DTO;
using BNE.BLL.Notificacao;
using BNE.Web.Handlers;

namespace BNE.Web
{

    public partial class ajax : Page
    {

        #region CadastroEmpresa_SalvarDadosContato
        /// <summary>
        /// CadastroEmpresa_SalvarDadosContato
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static void CadastroEmpresa_SalvarDadosContato(string cnpj, string cpf, string dataNascimento, string nome, string sexo, string funcao, string dddCelular, string celular, string dddTelefone, string telefone, string emailComercial, string site, string numeroFuncionario, string dataFundacao, string periodoInicial, string periodoFinal, string dddComercial, string telefoneComercial, string dddFax, string numeroFax, string ofereceCurso, string razaoSocial, string nomeFantasia, string cnae, string nj, string cep, string endereco, string numero, string complemento, string bairro, string nomeCidade)
        {
            //Só envia se foi informado: celular ou telefone comercial ou email comercial ou telefone da empresa.
            if ((!string.IsNullOrEmpty(dddCelular) && !string.IsNullOrEmpty(celular)) || (!string.IsNullOrEmpty(dddTelefone) && !string.IsNullOrEmpty(telefone)) || (!string.IsNullOrEmpty(dddComercial) && !string.IsNullOrEmpty(telefoneComercial)) || !string.IsNullOrEmpty(emailComercial))
            {
                if (!cnpj.Equals("undefined") && !Filial.ExisteFilial(cnpj))
                {
                    string emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens);
                    string emailDestinatario = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailDestinoContatosEmpresa);

                    if (!string.IsNullOrWhiteSpace(emailDestinatario))
                    {

                        string assunto;
                        string templateMensagem = CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.DesistenciaCadastroEmpresa, out assunto);

                        string dataNascimentoAux = string.Empty;
                        DateTime dn;
                        if (DateTime.TryParse(dataNascimento, out dn))
                            dataNascimentoAux = dn.ToShortDateString();

                        string dataFundacaoAux = string.Empty;
                        DateTime df;
                        if (DateTime.TryParse(dataFundacao, out df))
                            dataFundacaoAux = df.ToShortDateString();

                        var parametros = new
                        {
                            CNPJ = cnpj,
                            CPF = cpf,
                            DataNascimento = dataNascimentoAux,
                            Nome = nome,
                            Sexo = sexo,
                            Funcao = funcao,
                            DDDCelular = dddCelular,
                            Celular = celular,
                            DDDTelefone = dddTelefone,
                            Telefone = telefone,
                            EmailComercial = emailComercial,
                            Site = site,
                            NumeroFuncionario = numeroFuncionario,
                            DataFundacao = dataFundacaoAux,
                            PeriodoInicial = periodoInicial,
                            PeriodoFinal = periodoFinal,
                            DDDComercial = dddComercial,
                            TelefoneComercial = telefoneComercial,
                            DDDFax = dddFax,
                            NumeroFax = numeroFax,
                            OfereceCurso = ofereceCurso,
                            RazaoSocial = razaoSocial,
                            NomeFantasia = nomeFantasia,
                            CNAE = cnae,
                            NaturezaJuridica = nj,
                            CEP = cep,
                            Endereco = endereco,
                            Numero = numero,
                            Complemento = complemento,
                            Bairro = bairro,
                            NomeCidade = nomeCidade
                        };

                        try
                        {
                            decimal numeroCpf;
                            Decimal.TryParse(cpf.Replace(".", "").Replace("-", ""), out numeroCpf);

                            decimal numeroCnpj;
                            Decimal.TryParse(cnpj.Replace(".", "").Replace("-", ""), out numeroCnpj);

                            var objDesistenteCadastro = new DesistenteCadastro
                            {
                                NumeroCNPJ = numeroCnpj,
                                NumeroCPF = numeroCpf,
                                DataNascimento = dn,
                                EmailDesistenteCadastro = emailComercial,
                                NomeDesistenteCadastro = nome,
                                NumeroDDDCelular = dddCelular,
                                NumeroCelular = celular,
                                NumeroDDDTelefone = dddTelefone,
                                NumeroTelefone = telefone,
                                Origem = new Origem((int)BLL.Enumeradores.Origem.BNE)
                            };

                            objDesistenteCadastro.Save();
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex);
                        }

                        string mensagem = parametros.ToString(templateMensagem);

                        EmailSenderFactory
                            .Create(TipoEnviadorEmail.Fila)
                            .Enviar(assunto, mensagem, BLL.Enumeradores.CartaEmail.DesistenciaCadastroEmpresa, emailRemetente, emailDestinatario);
                    }
                }
            }
        }
        #endregion

        #region CadastroCurriculo_SalvarDadosContato
        /// <summary>
        /// CadastroCurriculo_SalvarDadosContato
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static void CadastroCurriculo_SalvarDadosContato(string cpf, string dataNascimento, string nome,
            string sexo, string funcao1, string ano1, string mes1, string funcao2, string ano2, string mes2,
            string funcao3, string ano3, string mes3, string dddCelular, string celular, string email, string nomeCidade,
            string pretensao, bool termosDeUso, bool whatsApp, int escolaridade, string escolaridadest)
        {
            //Só envia se foi informado: celular ou email
            //  if ((!string.IsNullOrEmpty(dddCelular) && !string.IsNullOrEmpty(celular) || !string.IsNullOrEmpty(email)) && cpf != "undefined")
            //  {
            if (pretensao.Equals("undefined"))
                pretensao = string.Empty;
            if (cpf.Equals("undefined"))
                cpf = string.Empty;

            int idPessoaFisica;
            if (!PessoaFisica.ExistePessoaFisica(cpf, out idPessoaFisica))
            {

                //string assunto;
                //string templateMensagem = CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.DesistenciaCadastroCurriculo, out assunto);

                //string dataNascimentoAux = string.Empty;
                //DateTime dn;
                //if (DateTime.TryParse(dataNascimento, out dn))
                //    dataNascimentoAux = dn.ToShortDateString();

                //var parametros = new
                //{
                //    CPF = cpf,
                //    DataNascimento = dataNascimentoAux,
                //    DDDCelular = dddCelular,
                //    Celular = celular,
                //    Email = email,
                //    Nome = nome,
                //    Sexo = sexo,
                //    NomeCidade = nomeCidade,
                //    Funcao1 = funcao1,
                //    Ano1 = ano1,
                //    Mes1 = mes1,
                //    Funcao2 = funcao2,
                //    Ano2 = ano2,
                //    Mes2 = mes2,
                //    Funcao3 = funcao3,
                //    Ano3 = ano3,
                //    Mes3 = mes3,
                //    Pretensao = pretensao,
                //    TermosDeUso = termosDeUso,
                //    whatsApp = whatsApp,
                //    Escolaridade = escolaridadest
                //};

                // salvar com o salario minimo
                if (!string.IsNullOrEmpty(dataNascimento)
                    && !string.IsNullOrEmpty(dataNascimento)
                    && !string.IsNullOrEmpty(nome)
                    && !string.IsNullOrEmpty(nomeCidade)
                    && !string.IsNullOrEmpty(funcao1)
                    && escolaridade > 0
                    && Helper.ValidarCPF(cpf)
                    && !string.IsNullOrEmpty(sexo))
                {
                    #region [Salvar Mini Curriculo]
                    try
                    {
                        string emailCadastrado = String.Empty;

                        PessoaFisica objPessoaFisica = new PessoaFisica();
                        Curriculo objCurriculo = new Curriculo();
                        objPessoaFisica.Endereco = new Endereco();
                        PessoaFisicaComplemento objPessoaFisicaComplemento = new PessoaFisicaComplemento();
                        BLL.PessoaFisicaFoto objPessoaFisicaFoto = new BLL.PessoaFisicaFoto();
                        UsuarioFilialPerfil objUsuarioFilialPerfil = new UsuarioFilialPerfil
                        {
                            Perfil = new Perfil((int)BLL.Enumeradores.Perfil.AcessoNaoVIP)
                        };


                        var listFuncoesPretendidas = new List<FuncaoPretendida>();

                        #region [Funcao Pretendida]
                        string[] exFuncao = new string[] { funcao1, funcao2, funcao3 };
                        string[] exFuncaoAno = new string[] { ano1, ano2, ano3 };
                        string[] exFuncaoMes = new string[] { mes1, mes2, mes3 };

                        for (int i = 0; i < exFuncao.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(exFuncao[i]))
                            {
                                short? expAnos, expMeses;
                                expAnos = expMeses = 0;

                                var objFuncaoPretendida = new FuncaoPretendida();

                                if (!String.IsNullOrEmpty(exFuncaoAno[i]))
                                    expAnos = Convert.ToInt16(exFuncaoAno[i]);

                                if (!String.IsNullOrEmpty(exFuncaoMes[i]))
                                    expMeses = Convert.ToInt16(exFuncaoMes[i]);


                                objFuncaoPretendida.QuantidadeExperiencia = Convert.ToInt16((expAnos.Value * 12) + expMeses.Value);

                                Funcao objFuncao;
                                FuncaoErroSinonimo objFuncaoErroSinonimo;
                                if (Funcao.CarregarPorDescricao(exFuncao[0], out objFuncao))
                                {
                                    objFuncaoPretendida.Funcao = objFuncao;
                                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                                }
                                else if (FuncaoErroSinonimo.CarregarPorDescricao(exFuncao[0], out objFuncaoErroSinonimo))
                                {
                                    objFuncaoPretendida.Funcao = objFuncaoErroSinonimo.Funcao;
                                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                                }
                                else
                                {
                                    objFuncaoPretendida.Funcao = null;
                                    objFuncaoPretendida.DescricaoFuncaoPretendida = exFuncao[0];
                                }

                                listFuncoesPretendidas.Add(objFuncaoPretendida);
                            }

                        }

                        #endregion

                        //Validação da função pretendida sobre as funções.

                        //Pessoa Física
                        cpf = Helper.LimparMascaraCPFCNPJ(cpf);
                        objPessoaFisica.CPF = Convert.ToDecimal(cpf);
                        objPessoaFisica.DataNascimento = Convert.ToDateTime(dataNascimento);
                        objPessoaFisica.NomePessoa = UIHelper.AjustarString(nome);
                        objPessoaFisica.NomePessoaPesquisa = UIHelper.RemoverAcentos(nome);
                        if (escolaridade > 0)
                            objPessoaFisica.Escolaridade = new Escolaridade(Convert.ToInt32(escolaridade));
                        objPessoaFisica.FlagWhatsApp = whatsApp;

                        if (!string.IsNullOrWhiteSpace(sexo))
                            objPessoaFisica.Sexo = new Sexo(Convert.ToInt32(BLL.Enumeradores.Sexo.Masculino.Equals(sexo) ? (int)BLL.Enumeradores.Sexo.Masculino : (int)BLL.Enumeradores.Sexo.Feminino));

                        objPessoaFisica.NumeroDDDCelular = dddCelular;
                        objPessoaFisica.NumeroCelular = celular;
                        objPessoaFisica.FlagInativo = false;

                        objPessoaFisica.DescricaoIP = objCurriculo.DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                        if (!string.IsNullOrEmpty(email) && BLL.Custom.Validacao.ValidarEmail(email))
                            objPessoaFisica.EmailPessoa = email.Trim();


                        //Endereco
                        Cidade objCidade;

                        if (Cidade.CarregarPorNome(nomeCidade, out objCidade))
                            objPessoaFisica.Endereco.Cidade = objCidade;

                        //Currículo 
                        var salarioMinimo = Convert.ToDecimal(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.SalarioMinimoNacional));
                        if (string.IsNullOrEmpty(pretensao) || Convert.ToDecimal(pretensao) < salarioMinimo)
                            objCurriculo.ValorPretensaoSalarial = salarioMinimo;

                        if (termosDeUso)
                        {
                            objCurriculo.DataAceitePoliticaPrivacidade = DateTime.Now;
                        }

                        Origem objOrigem = new Origem(1);//bne
                        Formacao objFormacao = null;
                        if (escolaridade > 0)
                        {
                            objFormacao = new Formacao();
                            objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(escolaridade));
                        }

                        //Salvar
                        objCurriculo.SalvarMiniCurriculo(objPessoaFisica,
                            listFuncoesPretendidas,
                            objOrigem,
                            null,
                            objFormacao,
                            objUsuarioFilialPerfil,
                            objPessoaFisicaComplemento,
                            BLL.Enumeradores.SituacaoCurriculo.AguardandoPublicacao,
                            objPessoaFisicaFoto,
                            null);
                    }
                    catch (Exception ex)
                    {

                    }


                    #endregion
                }//salvar pre curriculo
                else if (!string.IsNullOrEmpty(nome)
                    && !string.IsNullOrEmpty(email))
                {
                    #region [Salvar Pre Curriculo]
                    try
                    {
                        if (BLL.Custom.Validacao.ValidarEmail(email) && !PreCadastro.VerificaCadastro(email, (int)BLL.Enumeradores.OrigemPreCadastro.MiniCurriculo))
                        {
                            PreCadastro oPre = new PreCadastro();
                            oPre.nome = nome.Trim();
                            oPre.email = email.Trim();
                            if (!string.IsNullOrEmpty(nomeCidade))
                            {
                                Cidade objCidade;
                                Cidade.CarregarPorNome(nomeCidade, out objCidade);
                                oPre.idCidade = objCidade.IdCidade;

                            }
                            if (!string.IsNullOrEmpty(funcao1))
                            {
                                Funcao objFuncao;
                                Funcao.CarregarPorDescricao(funcao1, out objFuncao);
                                oPre.idFuncao = objFuncao.IdFuncao;

                            }
                            oPre.idOrigemPreCadastro = (int)BNE.BLL.Enumeradores.OrigemPreCadastro.MiniCurriculo;
                            oPre.Save();
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                    #endregion
                    // }
                    //else//Envia o e-mail
                    //{
                    //    try
                    //    {
                    //        decimal numeroCpf;
                    //        Decimal.TryParse(cpf.Replace(".", "").Replace("-", ""), out numeroCpf);

                    //        var objDesistenteCadastro = new DesistenteCadastro
                    //        {
                    //            NumeroCPF = numeroCpf,
                    //            DataNascimento = dn == default(DateTime) ? (DateTime?)null : dn,
                    //            EmailDesistenteCadastro = email,
                    //            NomeDesistenteCadastro = nome,
                    //            NumeroDDDCelular = dddCelular,
                    //            NumeroCelular = celular,
                    //            Origem = new Origem((int)BLL.Enumeradores.Origem.BNE)
                    //        };

                    //        objDesistenteCadastro.Save();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        EL.GerenciadorException.GravarExcecao(ex);
                    //    }

                    //    string mensagem = parametros.ToString(templateMensagem);

                    //    string emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens);
                    //    string emailDestinatario = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailDestinoContatosCurriculo);

                    //    EmailSenderFactory
                    //        .Create(TipoEnviadorEmail.Fila)
                    //        .Enviar(assunto, mensagem, BLL.Enumeradores.CartaEmail.DesistenciaCadastroCurriculo, emailRemetente, emailDestinatario);
                    //}
                }
            }
        }
        #endregion



        #region PesquisaVaga_VisualizacaoVaga
        /// <summary>
        /// PesquisaVaga_VisualizacaoVaga
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public static void PesquisaVaga_VisualizacaoVaga(int i)
        {
            try
            {
                var idCurriculo = HttpContext.Current.Session["BNE.Common.Session.SessionVariable`1[System.Int32]IdCurriculo"];
                var objVaga = new BLL.Vaga(i);
                if (idCurriculo != null)
                {
                    var objCurriculo = new Curriculo(Convert.ToInt32(idCurriculo));
                    VagaVisualizada.SalvarVisualizacaoVaga(objVaga, objCurriculo);
                }
                else
                    VagaVisualizada.SalvarVisualizacaoVaga(objVaga);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        [WebMethod]
        public static bool RemoveEmailList(int id, int idufp)
        {
            try
            {
                EmailEnvioCurriculo.Delete(id, idufp);
                return true;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, String.Format("Metodo RemoveEmailList: idf_email_envio_curriculo {0} idUfp {1} ", id, idufp));
                return false;
            }

        }
        #region ListarSugestEmailUsuarioFilial
        [WebMethod]
        public static string[] ListarSugestEmailUsuarioFilial(string prefixText, int count, string contextKey)
        {
            try
            {
                if (!String.IsNullOrEmpty(contextKey))
                {
                    string[] pesqui = prefixText.Split(';', ':', ',');
                    if (pesqui[pesqui.Length - 1].Length > 0)
                        return UIHelper.RecuperarItensAutoComplete(EmailEnvioCurriculo.ListarSugestEmailUsuarioFilial(pesqui[pesqui.Length - 1].Trim(), count, Convert.ToInt32(contextKey)));
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                string message = "A sentença informada foi: " + prefixText;
                EL.GerenciadorException.GravarExcecao(ex, message);
                return null;
            }
        }

        #endregion

        [WebMethod]
        public static void SalarioBR(int IdPF, int Idf)
        {
            try
            {
                BLL.Custom.SalarioBr.SalarioBR.EnviarPropaganda(IdPF, Idf);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro enviar propaganda do SalarioBR");
            }
        }

        [WebMethod]
        public static List<Funcao.FuncaoSinonimoSugest> SugestaoFuncao(string funcao)
        {
            Funcao objFuncao = new Funcao();
            if (Funcao.CarregarPorDescricao(funcao, out objFuncao))
                return Funcao.SugesteFuncaoSinonimo(objFuncao.IdFuncao);
            else
                return null;
        }


        [WebMethod]
        public static bool AnexoSine(int curriculo)
        {
            Curriculo cv = new Curriculo(curriculo);
            /*caso o curriculo tenha origem sine*/
            if (cv.CurriculoOrigemSine())
            {

                string sURL;

                cv.CompleteObject();
                cv.PessoaFisica.CompleteObject();

#if DEBUG
                sURL = String.Format("http://localhost:51899/v1.0/User/ArquivoExiste?cpf={0}", cv.PessoaFisica.CPF);
#else
                    string api_target = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.SineApi);
                    sURL = String.Format("{0}/User/ArquivoExiste?cpf={1}", api_target, cv.PessoaFisica.CPF);
#endif

                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create(sURL);
                try
                {
                    using (var response = (HttpWebResponse)wrGETURL.GetResponse())
                    {
                        StreamReader objReader = new StreamReader(response.GetResponseStream());
                        ArquivoExisteResult objResult = new JavaScriptSerializer().Deserialize<ArquivoExisteResult>(objReader.ReadToEnd());
                        response.Close();
                        if (!objResult.Message)
                            return false;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }

        [WebMethod]
        public static SistemaFolhaPgto.PesquisaFolha PesquisaFolha(int Filial)
        {
            return SistemaFolhaPgto.FolhaPesquisa(Filial);
        }

        [WebMethod]
        public static bool SavePesquisaFolha(int ufp, int folhaPgto, string outro)
        {
            try
            {
                SistemaFolhaPgtoPesquisa objPesquisa = new SistemaFolhaPgtoPesquisa();
                objPesquisa.UsuarioFilialPerfil = new UsuarioFilialPerfil(ufp);

                if (folhaPgto > 0)
                {
                    objPesquisa.FlagResposta = true;
                    objPesquisa.SistemaFolhaPgto = new SistemaFolhaPgto(Convert.ToInt16(folhaPgto));
                    objPesquisa.DescricaoSistemaFolhaPgtoPesquisa = outro;
                }
                else//respondeu não a pesquisa
                    objPesquisa.FlagResposta = false;


                objPesquisa.Save();

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, $"Erro ao salvar pesquisa {ufp}");
                return false;
            }
            return true;
        }

        #region [CidadeEstadoZona]
        [WebMethod]
        public static string CidadeEstadoZona(string Cidade)
        {
            CidadeEstadoZonaResult result = new CidadeEstadoZonaResult();
            if (!string.IsNullOrEmpty(Cidade))
            {
                var cidadeEstado = Cidade.Split('/');
                if (cidadeEstado.Length == 2)
                {
                    try
                    {
                        var objEstado = Estado.CarregarPorSiglaEstado(cidadeEstado[1]);

                        if (objEstado.SiglaEstado.Equals("SP"))
                        {
                            var url = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.UrlAPICEP) + "/api/bairro/getbycidade/{cidade}/{estado}".Replace("{cidade}", cidadeEstado[0]).Replace("{estado}", objEstado.SiglaEstado);

                            var request = WebRequest.Create(url);
                            using (var response = request.GetResponse())
                            {
                                using (var sr = new StreamReader(response.GetResponseStream()))
                                {
                                    var retorno = sr.ReadToEnd();
                                    result = JsonConvert.DeserializeObject<CidadeEstadoZonaResult>(retorno);
                                }
                            }
                        }
                        result.SiglaEstado = objEstado.SiglaEstado;
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Erro ao consultar os bairros na api de cep");
                    }

                }
            }

            return JsonConvert.SerializeObject(result);

        }

        [Serializable]
        private class CidadeEstadoZonaResult
        {
            public string SiglaEstado { get; set; }
            public ZonaLeste[] ZonaLeste { get; set; }
            public ZonaOeste[] ZonaOeste { get; set; }
            public ZonaNorte[] ZonaNorte { get; set; }
            public ZonaSul[] ZonaSul { get; set; }
            public Centro[] Centro { get; set; }

        }

        public class ZonaLeste
        {
            public int ID { get; set; }
            public string Nome { get; set; }
        }

        public class ZonaOeste
        {
            public int ID { get; set; }
            public string Nome { get; set; }
        }

        public class ZonaNorte
        {
            public int ID { get; set; }
            public string Nome { get; set; }
        }

        public class ZonaSul
        {
            public int ID { get; set; }
            public string Nome { get; set; }
        }

        public class Centro
        {
            public int ID { get; set; }
            public string Nome { get; set; }
        }

        #endregion

        #region [CarregarCandidaturas]
        [WebMethod]
        public static AjaxDTO CarregarCandidaturas(int idc, int page, int pagesize, DateTime? DtaInicio = null, DateTime? DtaFim = null)
        {
            int totalRegistros;
            var listCand = VagaCandidato.VagasCandidatosInfoPorCurriculo(idc, false, page, pagesize, out totalRegistros, DtaInicio, DtaFim);
            var objRetorno = new AjaxDTO();
            objRetorno.Candidaturas = new List<CandidaturasI>();
            foreach (DataRow item in listCand.Rows)
            {
                CandidaturasI obj = new CandidaturasI();
                obj.DataCadastro = Convert.ToDateTime(item["Dta_Cadastro"]).ToString("dd/MM/yyyy HH:mm");
                obj.Funcao = item["Des_Funcao"].ToString();
                obj.Cidade = Helper.FormatarCidade(item["nme_cidade"].ToString(), item["Sig_Estado"].ToString());
                obj.codVaga = item["cod_Vaga"].ToString();
                obj.linkVaga = Vaga.MontarUrlVaga(Convert.ToInt32(item["Idf_Vaga"]));
                obj.Status = item["statusvaga"].ToString();
                objRetorno.Candidaturas.Add(obj);

            }

            objRetorno.Page = page;
            objRetorno.PageSize = pagesize;
            objRetorno.TotalRegistros = totalRegistros;


            return objRetorno;
        }
        #endregion

        #region [EmpresaVisualizaram]
        [WebMethod]
        public static EmpresasQueVisualizaram EmpresaVisualizaram(int idc, int page, int pagesize, DateTime? DtaInicio = null, DateTime? DtaFim = null)
        {
            var retorno = new EmpresasQueVisualizaram();
            try
            {
                int totalRegistros;

                var listVisu = CurriculoQuemMeViu.RecuperarQuemMeViuAdministrador(idc, page, pagesize, out totalRegistros, DtaInicio, DtaFim);
                retorno.TotalRegistros = totalRegistros;
                retorno.Visualizacoes = new List<Code.DTO.EmpresaVisualizaram>();
                foreach (DataRow item in listVisu.Rows)
                {
                    var obj = new EmpresaVisualizaram();
                    obj.Nome_Empresa = item["Raz_Social"].ToString();
                    obj.Data_Visualizacao = Convert.ToDateTime(item["Dta_Quem_Me_Viu"]).ToString("dd/MM/yyyy HH:mm");
                    retorno.Visualizacoes.Add(obj);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao consultar as informações de visualizacoes o curriculo - Administrador");
            }
            return retorno;

        }
        #endregion

        #region [EnvioCurriculoParaEmpresa]
        [WebMethod]
        public static EnvioCvEmpresaInfo EnvioCurriculoParaEmpresa(int idc, int page, int pagesize, DateTime? DtaInicio = null, DateTime? DtaFim = null)
        {
            var retorno = new EnvioCvEmpresaInfo();

            try
            {
                int totalRegistro;
                var listIntencao = IntencaoFilial.CarregarPorCurriculo(idc, page, pagesize, out totalRegistro, DtaInicio, DtaFim);
                retorno.TotalRegistros = totalRegistro;
                retorno.EnvioEmpresa = new List<EnvioCvEmpresa>();
                foreach (DataRow item in listIntencao.Rows)
                {
                    var obj = new EnvioCvEmpresa();
                    obj.Nome_Empresa = item["Raz_Social"].ToString();
                    obj.Data_Envio = Convert.ToDateTime(item["Dta_Cadastro"]).ToString("dd/MM/yyyy HH:mm");
                    retorno.EnvioEmpresa.Add(obj);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro metodo EnvioCurriculoParaEmpresa");
            }
            return retorno;

        }
        #endregion

        #region [AlertaVagas]
        [WebMethod]
        public static AlertaVagasDTO AlertaVagas(int idc)
        {
            var retorno = new AlertaVagasDTO();

            try
            {
                #region [Dias]
                var dtDias = AlertaCurriculosAgenda.ListarDiasDaSemana(idc);
                retorno.Dias = new List<string>();

                foreach (DataRow item in dtDias.Rows)
                    retorno.Dias.Add(item["des_dia_da_semana"].ToString());
                #endregion

                #region [Função]
                var dtFuncoes = AlertaFuncoes.ListarFuncoesAlertaCurriculo(idc);
                retorno.Funcoes = new List<string>();
                foreach (DataRow item in dtFuncoes.Rows)
                    retorno.Funcoes.Add(item["Des_Funcao"].ToString());
                #endregion

                #region [Cidade]
                var dtCidades = AlertaCidades.ListarCidadesAlertaCurriculo(idc);
                retorno.Cidades = new List<string>();

                foreach (DataRow item in dtCidades.Rows)
                    retorno.Cidades.Add(Helper.FormatarCidade(item["Nme_Cidade"].ToString(), item["Sig_Estado"].ToString()));
                #endregion

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "AlertaVagas DTO - admin");
            }

            return retorno;

        }
        #endregion

        #region [Ja Enviei]
        [WebMethod]
        public static List<JaEnvieiDTO> JaEnviei(string idc, int pag, int pagsize)
        {
            var listJaEnviei = new List<JaEnvieiDTO>();
            int totalRegistros = 0;
            try
            {
                var idcDescript = Helper.Descriptografa(idc);
                var dtCandidaturas = VagaCandidato.CarregarVagaCandidatadaPorCurriculoJaEnviei(Convert.ToInt32(idcDescript), pag, pagsize, out totalRegistros);

                foreach (DataRow item in dtCandidaturas.Rows)
                {
                    var obj = new JaEnvieiDTO();
                    obj.NomeEmpresa = Convert.ToBoolean(item["Flg_Vip"]) ? item["Raz_Social"].ToString() : String.Empty;
                    obj.Funcao = item["Des_Funcao"].ToString();
                    obj.DataCandidatura = Convert.ToDateTime(item["Dta_Cadastro"]).ToString("dd/MM/yyyy");
                    obj.HoraCandidatura = Convert.ToDateTime(item["Dta_Cadastro"]).ToString("HH:mm");
                    obj.Oportunidade = Convert.ToBoolean(item["Flg_Vaga_Arquivada"]);
                    obj.Inativo = Convert.ToBoolean(item["Flg_Inativo"]);
                    obj.PCD = item["Flg_Deficiencia"] != DBNull.Value ? Convert.ToBoolean(item["Flg_Deficiencia"]) : false;
                    obj.TotalRegistros = totalRegistros;
                    obj.Etapas = new List<EtapaCandidaturaDTO>();

                    if (item["Dta_Analise_CV"] != DBNull.Value)
                    {//Atenção na ordem.
                        if (item["Dta_Visualizacao"] != DBNull.Value)
                            obj.Etapas.Add(new EtapaCandidaturaDTO("Visualizado", Convert.ToDateTime(item["Dta_Visualizacao"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(item["Dta_Visualizacao"]).ToString("HH:mm")));


                        //if (Convert.ToDateTime(item["Dta_Envio_CV"]) <= DateTime.Now) {

                        //    //obj.Etapas.Add(new EtapaCandidaturaDTO("Enviado", Convert.ToDateTime(item["Dta_Envio_CV"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(item["Dta_Envio_CV"]).ToString("HH:mm")));
                        //    obj.Etapas.Add(new EtapaCandidaturaDTO("Analise", Convert.ToDateTime(item["Dta_Analise_CV"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(item["Dta_Analise_CV"]).ToString("HH:mm")));
                        //    obj.Etapas.Add(new EtapaCandidaturaDTO("Processamento", Convert.ToDateTime(item["Dta_Processamento_Candidatura"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(item["Dta_Processamento_Candidatura"]).ToString("HH:mm")));

                        //}
                        obj.Etapas.Add(new EtapaCandidaturaDTO("Analise", Convert.ToDateTime(item["Dta_Analise_CV"]) <= DateTime.Now ? Convert.ToDateTime(item["Dta_Analise_CV"]).ToString("dd/MM/yyyy") : string.Empty, Convert.ToDateTime(item["Dta_Analise_CV"]) <= DateTime.Now ? Convert.ToDateTime(item["Dta_Analise_CV"]).ToString("HH:mm") : string.Empty));
                        obj.Etapas.Add(new EtapaCandidaturaDTO("Processamento", Convert.ToDateTime(item["Dta_Processamento_Candidatura"]) <= DateTime.Now ? Convert.ToDateTime(item["Dta_Processamento_Candidatura"]).ToString("dd/MM/yyyy") : string.Empty, Convert.ToDateTime(item["Dta_Processamento_Candidatura"]) <= DateTime.Now ? Convert.ToDateTime(item["Dta_Processamento_Candidatura"]).ToString("HH:mm") : string.Empty));
                        obj.Etapas.Add(new EtapaCandidaturaDTO("Candidatura", Convert.ToDateTime(item["Dta_Cadastro"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(item["Dta_Cadastro"]).ToString("HH:mm")));
                    }
                    else
                        obj.Etapas.Add(new EtapaCandidaturaDTO("Analise", Convert.ToDateTime(item["Dta_Cadastro"]).ToString("dd/MM/yyyy"), Convert.ToDateTime(item["Dta_Cadastro"]).ToString("HH:mm")));

                    listJaEnviei.Add(obj);
                }
            }
            catch (Exception)
            {
            }
         

            return listJaEnviei;
        }
        #endregion

        #region [Quem me Viu]
        [WebMethod]
        public static List<QuemMeViuDTO> QuemMeViu(int idc, int pag, int pagsize)
        {
            List<QuemMeViuDTO> lista = new List<QuemMeViuDTO>();

            int totalRegistros;
            var dtQuemMeViu = CurriculoQuemMeViu.RecuperarQuemMeViuSite(idc, pag, pagsize, out totalRegistros);

            foreach (DataRow item in dtQuemMeViu.Rows)
            {
                try
                {
                    QuemMeViuDTO obj = new QuemMeViuDTO();
                    obj.IdFilial = Convert.ToInt32(item["Idf_Filial"]);
                    obj.vip = Convert.ToBoolean(item["Flg_VIP"]);
                    obj.RazSocial = obj.vip && item["Vlr_Parametro"] == DBNull.Value ? item["Raz_Social"].ToString() : "Confidencial";
                    obj.DataQuemMeViu = Convert.ToDateTime(item["Dta_Quem_Me_Viu"]).ToString("dd/MM/yyyy");
                    obj.HoraQuemMeViu = Convert.ToDateTime(item["Dta_Quem_Me_Viu"]).ToString("HH:mm");
                    obj.Cidade = item["Nme_Cidade"].ToString();
                    obj.SigEstado = item["Sig_Estado"].ToString();
                    obj.QtdFuncionario = Convert.ToInt32(item["Qtd_Funcionarios"]);
                    obj.DataCadastro = Convert.ToDateTime(item["Dta_Cadastro"]).ToString("dd/MM/yyyy");
                    obj.Telefone = obj.vip && item["Vlr_Parametro"] == DBNull.Value ? Helper.FormatarTelefone(item["num_ddd_comercial"].ToString(), item["num_comercial"].ToString()) : "Confidencial";
                    obj.TotalVagas = Convert.ToInt32(item["TotalVagas"]);
                    obj.TotalVisualizacoes = Convert.ToInt32(item["VisualizacaoEmpresa"]);
                    obj.AreaEmpresa = item["Des_Area_BNE"].ToString();
                    obj.TotalRegistros = totalRegistros;
                    obj.UrlImg = obj.vip && item["Vlr_Parametro"] == DBNull.Value ? UIHelper.RetornarUrlLogo(item["Num_cnpj"].ToString(), PessoaJuridicaLogo.OrigemLogo.Local) : UIHelper.RetornarUrlLogo("2", PessoaJuridicaLogo.OrigemLogo.Local);//cnpj 2 só para não trazer a logo da empresa
                    obj.Bairro = obj.vip && item["Vlr_Parametro"] == DBNull.Value ? item["Des_Bairro"].ToString() : "Confidencial";
                    obj.LinkVagas = obj.vip && item["Vlr_Parametro"] == DBNull.Value ? Helper.RecuperarURLVagasEmpresa(item["Raz_Social"].ToString(), Convert.ToInt32(item["Idf_Filial"])) : "Confidencial";
                    lista.Add(obj);
                }
                catch (Exception)
                {
                }
                //img.ImageUrl = "/img/cv_email/logo_bne_cv_tr.png";

            }
            return lista;
        }
        #endregion
    }



}




