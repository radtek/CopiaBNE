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
        public static void CadastroCurriculo_SalvarDadosContato(string cpf, string dataNascimento, string nome, string sexo, string funcao1, string ano1, string mes1, string funcao2, string ano2, string mes2, string funcao3, string ano3, string mes3, string dddCelular, string celular, string email, string nomeCidade, string pretensao)
        {
            //Só envia se foi informado: celular ou email
            if ((!string.IsNullOrEmpty(dddCelular) && !string.IsNullOrEmpty(celular) || !string.IsNullOrEmpty(email)) && cpf != "undefined")
            {
                int idPessoaFisica;
                if (!PessoaFisica.ExistePessoaFisica(cpf, out idPessoaFisica))
                {

                    string assunto;
                    string templateMensagem = CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.DesistenciaCadastroCurriculo, out assunto);

                    string dataNascimentoAux = string.Empty;
                    DateTime dn;
                    if (DateTime.TryParse(dataNascimento, out dn))
                        dataNascimentoAux = dn.ToShortDateString();

                    var parametros = new
                    {
                        CPF = cpf,
                        DataNascimento = dataNascimentoAux,
                        DDDCelular = dddCelular,
                        Celular = celular,
                        Email = email,
                        Nome = nome,
                        Sexo = sexo,
                        NomeCidade = nomeCidade,
                        Funcao1 = funcao1,
                        Ano1 = ano1,
                        Mes1 = mes1,
                        Funcao2 = funcao2,
                        Ano2 = ano2,
                        Mes2 = mes2,
                        Funcao3 = funcao3,
                        Ano3 = ano3,
                        Mes3 = mes3,
                        Pretensao = pretensao
                    };

                    try
                    {
                        decimal numeroCpf;
                        Decimal.TryParse(cpf.Replace(".", "").Replace("-", ""), out numeroCpf);

                        var objDesistenteCadastro = new DesistenteCadastro
                        {
                            NumeroCPF = numeroCpf,
                            DataNascimento = dn == default(DateTime) ? (DateTime?)null : dn,
                            EmailDesistenteCadastro = email,
                            NomeDesistenteCadastro = nome,
                            NumeroDDDCelular = dddCelular,
                            NumeroCelular = celular,
                            Origem = new Origem((int)BLL.Enumeradores.Origem.BNE)
                        };

                        objDesistenteCadastro.Save();
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }

                    string mensagem = parametros.ToString(templateMensagem);

                    string emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens);
                    string emailDestinatario = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailDestinoContatosCurriculo);

                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(assunto, mensagem, BLL.Enumeradores.CartaEmail.DesistenciaCadastroCurriculo, emailRemetente, emailDestinatario);
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
            var listCand = VagaCandidato.VagasCandidatosInfoPorCurriculo(idc, false, page, pagesize, out totalRegistros, DtaInicio,DtaFim);
            var objRetorno = new AjaxDTO();
            objRetorno.Candidaturas = new List<CandidaturasI>();
            foreach (DataRow item in listCand.Rows)
            {
                CandidaturasI obj = new CandidaturasI();
                obj.DataCadastro =Convert.ToDateTime(item["Dta_Cadastro"]).ToString("dd/MM/yyyy HH:mm");
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
        public static EmpresasQueVisualizaram  EmpresaVisualizaram(int idc, int page, int pagesize, DateTime? DtaInicio = null, DateTime? DtaFim = null)
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

    }

   

}




