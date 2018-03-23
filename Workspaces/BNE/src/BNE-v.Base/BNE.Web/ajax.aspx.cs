using System;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.BLL;
using BNE.BLL.Custom.Email;

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
                            .Enviar(assunto, mensagem, emailRemetente, emailDestinatario);
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
            if ((!string.IsNullOrEmpty(dddCelular) && !string.IsNullOrEmpty(celular)) || !string.IsNullOrEmpty(email))
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
                            DataNascimento = dn,
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
                        .Enviar(assunto, mensagem, emailRemetente, emailDestinatario);
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
        
    }

}