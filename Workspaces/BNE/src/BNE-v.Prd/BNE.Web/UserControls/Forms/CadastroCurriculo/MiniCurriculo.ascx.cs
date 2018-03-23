using BNE.Auth.Helper;
using BNE.BLL;
using BNE.BLL.Integracoes.Facebook;
using BNE.Bridge;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using JSONSharp;
using Microsoft.IdentityModel.Claims;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Auth.EventArgs;
using BNE.Componentes.EL;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Web.Routing;
using BNE.Auth;

namespace BNE.Web.UserControls.Forms.CadastroCurriculo
{
    public partial class MiniCurriculo : BaseUserControl
    {

        #region Propriedades

        #region EstadoManutencao

        /// <summary>
        /// Propriedade que armazena e recupera um boolean indicando se o user controls está em estado de manutenção
        /// </summary>
        public bool EstadoManutencao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel5.ToString()]);

                return false;
            }
            set { ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value); }
        }

        #endregion

        #region IdPessoaFisica - Variável 1

        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdPessoaFisica
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());
            }
        }

        #endregion

        #region CadastroBloqueado - Variável 5

        /// <summary>
        /// Propriedade utilizada para armazenar se o curriculo está bloquado ou não, para que, ao usuario clicar em salvar, impedir o fluxo e mostrar mensagem específica.
        /// </summary>
        private bool CadastroBloqueado
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel5.ToString()]);

                return false;
            }
            set { ViewState[Chave.Temporaria.Variavel5.ToString()] = value; }
        }

        #endregion

        #region IdCurriculoSession - Variável 6

        /// <summary>
        /// Propriedade que armazena e recupera o ID do Curriculo
        /// </summary>
        public int? IdCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel6.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel6.ToString());
            }
        }

        #endregion

        #region EnumSituacaoCurriculo

        /// <summary>
        /// Propriedade que armazena e recupera um enum situação curriculo setado pelo usuário administrador no curriculo completo
        /// </summary>
        public Enumeradores.SituacaoCurriculo? EnumSituacaoCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return (Enumeradores.SituacaoCurriculo)(ViewState[Chave.Temporaria.Variavel7.ToString()]);

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel7.ToString());
            }
        }

        #endregion

        #region FotoWSBytes - Variavel10

        private byte[] FotoWSBytes
        {
            get { return (byte[])(ViewState[Chave.Temporaria.Variavel10.ToString()]); }
            set { ViewState.Add(Chave.Temporaria.Variavel10.ToString(), value); }
        }

        #endregion

        #region IdFormacao - Variável 12

        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdFormacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel12.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel12.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel12.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel12.ToString());
            }
        }
        #endregion

        #region dsFormacao - Variável 13

        /// <summary>
        /// Propriedade que armazena e recuperaa descrição da formação
        /// </summary>
        public string DsFormacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel13.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel13.ToString()].ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel13.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel13.ToString());
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar

        /// <summary>
        /// Método responsável por carregar os controles e definir as propriedades
        /// iniciais dos mesmos.
        /// </summary>
        private void Inicializar()
        {
            if (EstadoManutencao)
            {
                pnlBotoes.Visible = false;
                pnlAbas.Visible = false;
                litTitulo.Text = "Mini Currículo";
            }

            //Vindo da carta SMSSemRetorno
            if (!String.IsNullOrEmpty(Page.Request.QueryString["revisar"]) && IdPessoaFisica.HasValue)
            {
                Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoRevisao.ToString(), null));
            }

            UIHelper.CarregarDropDownList(ddlEscolaridade, Escolaridade.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarRadioButtonList(rblSexo, Sexo.Listar());
            UIHelper.CarregarDropDownList(ddlAt, DeParaEmail.ListaAt(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlSituacao, SituacaoFormacao.Listar(), new ListItem("Selecione", "0"));
            ddlAt.Items.Add(new ListItem("Outros", "null"));
            upRblSexo.Update();

            if (base.STC.ValueOrDefault)
            {
                pnlPoliticaPrivacidade.Visible = false;
            }

            //dados vindos do link "Terminar Cadastro" do jornal de vagas do pre cadastro
            if (!String.IsNullOrEmpty(Page.Request.QueryString["eml"]))
            {
                try
                {
                    txtNome.Valor = Page.Request.QueryString["nme"].Replace("-", " ");
                    TratamentoEmail(Page.Request.QueryString["eml"]);
                    txtCidadeMini.Text = Page.Request.QueryString["cid"].Replace("-", "/");
                    txtFuncaoPretendida1.Text = Page.Request.QueryString["fun"];
                }
                catch (Exception)
                {
                }

            }
            CarregarParametros();
            avisoSalarionivalido.InnerText =
                string.Format("Sua pretensão salarial deve ser maior que o Salário Mínimo Nacional R$ {0}",
                    hdfValorSalarioMinimo.Value);

            //Aplicação de Regra: 
            //- Intervalo da Data de Nascimento. ( 14 anos >= idade <= 80 anos )
            int idadeMinima = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdadeMinima));
            int idadeMaxima = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdadeMaxima));
            try
            {
                DateTime dataAtual = DateTime.Now;
                DateTime dataMinima =
                    Convert.ToDateTime(dataAtual.Day.ToString(CultureInfo.CurrentCulture) + "/" +
                                       dataAtual.Month.ToString(CultureInfo.CurrentCulture) + "/" +
                                       (dataAtual.Year - idadeMaxima).ToString(CultureInfo.CurrentCulture));
                DateTime dataMaxima =
                    Convert.ToDateTime(dataAtual.Day.ToString(CultureInfo.CurrentCulture) + "/" +
                                       dataAtual.Month.ToString(CultureInfo.CurrentCulture) + "/" +
                                       (dataAtual.Year - idadeMinima).ToString(CultureInfo.CurrentCulture));
                txtDataDeNascimento.DataMinima = dataMinima;
                txtDataDeNascimento.DataMaxima = dataMaxima;
            }
            catch
            {
                txtDataDeNascimento.DataMinima = DateTime.MinValue;
                txtDataDeNascimento.DataMaxima = DateTime.MaxValue;
            }
            txtDataDeNascimento.MensagemErroFormato = String.Format(MensagemAviso._100005, idadeMinima, idadeMaxima);

            txtFuncaoPretendida1.Attributes["OnChange"] += "FuncaoPretendida_OnChange(this)";
            txtFuncaoPretendida1.Attributes["OnBlur"] += "FuncaoPretendida_OnBlur()";
            ddlEscolaridade.Attributes["OnChange"] += "FuncaoEscolaridade_OnChange()";
            txtCidadeMini.Attributes["OnBlur"] += string.Format("setTimeout(\"ValidatorValidate($get('{0}'))\",500);", cvCidadeMini.ClientID);


            LimparCampos();
            PreencherCampos();

            btlGestao.Visible = base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue;

            var parametros = new
            {
                msgFaixaSalarial = MensagemAviso._202001
            };

            UIHelper.ValidateFocus(btnSalvarCurriculo);
            UIHelper.ValidateFocus(btlDadosPessoais);
            UIHelper.ValidateFocus(btlFormacaoCursos);
            UIHelper.ValidateFocus(btlDadosComplementares);

            //Foco inicial
            SetarFoco(txtCPF);
        }

        #endregion

        #region ValidarDataNascimento
        private bool ValidarDataNascimento()
        {
            return (txtDataDeNascimento.Text == "");
        }

        #endregion

        #region ValidarCadastroExistente

        private bool ValidarCadastroExistente()
        {
            PessoaFisica objPessoaFisica;

            if (PessoaFisica.CarregarPorCPF(Convert.ToDecimal(this.txtCPF.Valor), out objPessoaFisica))
            {
                //if (objPessoaFisica.DataNascimento.Equals(txtDataDeNascimento.ValorDateTime))
                //return true;

                return true;
                //return false;
            }

            return true;
        }

        #endregion

        #region ValidarCPFDataNascimento

        private void ValidarCPFDataNascimento()
        {
            CadastroBloqueado = false;
            if (txtDataDeNascimento.ValorDateTime.HasValue && !String.IsNullOrEmpty(txtCPF.Valor))
            {
                try
                {
                    int idPessoaFisica;
                    if (PessoaFisica.ExistePessoaFisica(txtCPF.Valor, out idPessoaFisica))
                    {
                        if (PessoaFisica.ValidarCPFDataNascimento(txtCPF.Valor, txtDataDeNascimento.ValorDateTime.Value))
                        {
                            IdPessoaFisica = idPessoaFisica;

                            int idCurriculo;
                            if (Curriculo.ExisteCurriculo(new PessoaFisica(IdPessoaFisica.Value), out idCurriculo))
                            {
                                var objSituacaoCurriculo = new Curriculo(idCurriculo).RecuperarSituacao();
                                //Se a situação do Currículo estiver bloqueado mostrar mensagem específica
                                if (
                                    objSituacaoCurriculo.IdSituacaoCurriculo.Equals(
                                        (int)Enumeradores.SituacaoCurriculo.Bloqueado) ||
                                    objSituacaoCurriculo.IdSituacaoCurriculo.Equals(
                                        (int)Enumeradores.SituacaoCurriculo.Cancelado))
                                {
                                    CadastroBloqueado = true;
                                    ExibirMensagem(MensagemAviso._103703, TipoMensagem.Erro);
                                    return;
                                }

                                bool origem = CurriculoOrigem.ExisteCurriculoNaOrigem(new Curriculo(idCurriculo),
                                    new Origem(base.IdOrigem.Value));

                                //Task: 43574 - Não logar curriculo nessa situação // mas ao cadastrar fazer update nos dados.
                                if (objSituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.ExclusaoLogica))
                                {
                                    EnumSituacaoCurriculo = Enumeradores.SituacaoCurriculo.ExclusaoLogica;
                                    return;
                                }
                                if (objSituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.Hibernado))
                                {//Curriculos no excluidos pelo proprio candidato(hibernado) ao se logar ativa de volta o curriculo
                                    Curriculo objCv = Curriculo.LoadObject(idCurriculo);
                                    objCv.FlagInativo = false;
                                    objCv.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoPublicacao);
                                    objCv.Save();
                                    objCv.PessoaFisica.CompleteObject();
                                    EmailSituacao.DesbloquearEmail(objCv.PessoaFisica.IdPessoaFisica, objCv.PessoaFisica.EmailPessoa);

                                }
                                LogarUsuarioCandidato(idPessoaFisica, idCurriculo);

                                LogarUsuarioCurriculo(new PessoaFisica(idPessoaFisica).RecuperarNomePessoa(),
                                    idPessoaFisica, idCurriculo,
                                    Convert.ToDecimal(new string(txtCPF.Valor.Where(char.IsNumber).ToArray())), txtDataDeNascimento.ValorDateTime.Value);

                                ExibirMensagem(origem ? MensagemAviso._100109 : MensagemAviso._100112, TipoMensagem.Erro);
                            }

                            PreencherCampos();
                            ComprouVipPeixeUrbano();

                        }
                    }
                }
                catch (Exception ex)
                {
                    base.ExibirMensagemErro(ex);
                }
            }
            else if (!txtDataDeNascimento.ValorDateTime.HasValue)
            {
                txtDataDeNascimento.Focus();
            }
            else
            {
                txtCPF.Focus();
            }
        }

        private void LogarUsuarioCurriculo(string nome, int idPessoaFisica, int idCurriculo, decimal cpf, DateTime dataNascimento)
        {
            var identity = BNE.Auth.BNEAutenticacao.LogarCandidato(nome, idPessoaFisica, cpf, dataNascimento, idCurriculo);

            var context = HttpContext.Current ?? Context;
            var auth = new BNEAuthLoginControlEventArgs(identity, context);
            BNE.Auth.AuthEventAggregator.Instance.OnUserEnterSuccessfully(this, auth);

            var modelResult = new BNESessaoLoginModelResult(BNESessaoLoginResultType.OK,
                BNESessaoProfileType.CANDIDATO);
            BNE.Bridge.BNELoginProcess.SalvarNovaSessaoBanco(auth, modelResult);
            //BNE.Bridge.BNELoginProcess.RegistrarBLLProcess(auth, modelResult);
        }

        #endregion

        #region PreencherCampos

        /// <summary>
        /// Método responsável por preencher os campos do formulário.
        /// </summary>
        private void PreencherCampos()
        {
            //Tsk 56043 - Se usuario de empresa logado tentar acessar a tela de cadastro de curriculo derrubar a sessão dele.
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                BNEAutenticacao.DeslogarPadrao();
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
            }
            if (IdPessoaFisica.HasValue)
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);

                CelularChecked.Visible = objPessoaFisica.FlagCelularConfirmado;

                txtCPF.Valor = objPessoaFisica.NumeroCPF;
                txtDataDeNascimento.ValorDateTime = objPessoaFisica.DataNascimento;

                txtTelefoneCelular.DDD = objPessoaFisica.NumeroDDDCelular;
                txtTelefoneCelular.Fone = objPessoaFisica.NumeroCelular;

                txtNome.Valor = objPessoaFisica.NomePessoa;
                if (objPessoaFisica.FlagWhatsApp.HasValue)
                    cbWhats.Checked = objPessoaFisica.FlagWhatsApp.Value;

                if (!string.IsNullOrEmpty(objPessoaFisica.EmailPessoa) && BLL.Custom.Validacao.ValidarEmail(objPessoaFisica.EmailPessoa))
                    TratamentoEmail(objPessoaFisica.EmailPessoa);
                else
                {//não tem e-mail - campo não obrigatório
                    rfvAt.Enabled = !string.IsNullOrWhiteSpace(txtEmail.Text);
                    rfvEmail.Enabled = !string.IsNullOrWhiteSpace(txtEmail.Text);
                    rfvDdlAt.Enabled = !string.IsNullOrWhiteSpace(txtEmail.Text);
                }

                if (objPessoaFisica.Sexo != null)
                {
                    rblSexo.SelectedValue = objPessoaFisica.Sexo.IdSexo.ToString(CultureInfo.CurrentCulture);
                    upRblSexo.Update();
                }

                if (objPessoaFisica.Endereco != null)
                {
                    objPessoaFisica.Endereco.CompleteObject();
                    objPessoaFisica.Endereco.Cidade.CompleteObject();
                    txtCidadeMini.Text = String.Format("{0}/{1}", objPessoaFisica.Endereco.Cidade.NomeCidade,
                        objPessoaFisica.Endereco.Cidade.Estado.SiglaEstado);
                }
                else if (objPessoaFisica.Cidade != null)
                {
                    objPessoaFisica.Cidade.CompleteObject();
                    txtCidadeMini.Text = String.Format("{0}/{1}", objPessoaFisica.Cidade.NomeCidade, objPessoaFisica.Cidade.Estado.SiglaEstado);
                }

                Curriculo objCurriculo;
                if (Curriculo.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objCurriculo))
                {
                    List<FuncaoPretendida> listFuncoesPretendidas =
                        FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);

                    PreencherCamposFuncoesPretendidas(listFuncoesPretendidas);


                    if (objCurriculo.ValorPretensaoSalarial.HasValue)
                        txtPretensaoSalarial.Text = objCurriculo.ValorPretensaoSalarial.Value.ToString();
                }

                //Campos que devem estar desabilitados com a tela em modo de edição.
                txtCPF.ReadOnly = true;
                //txtDataDeNascimento.ReadOnly = true;

                if (objPessoaFisica.Escolaridade != null)
                {
                    ddlEscolaridade.SelectedValue = objPessoaFisica.Escolaridade.IdEscolaridade.ToString();

                    //para escolaridade incompleta mostrar DIV aceita estagio
                    int idEscolaridadeCv = objPessoaFisica.Escolaridade.IdEscolaridade;

                    if (idEscolaridadeCv == 6 || idEscolaridadeCv == 8 || idEscolaridadeCv == 10 ||
                        idEscolaridadeCv == 11)
                    {
                        divAceitaEstagio.Style["display"] = "block";
                        PreeencherParametroAceitaEstagio(objCurriculo);
                    }

                    if (idEscolaridadeCv == 8 || idEscolaridadeCv == 10 || idEscolaridadeCv == 11)
                    {
                        divLinhaTituloCurso.Style["display"] = "block";
                    }
                    PreencherFormacao(objPessoaFisica);

                }

                CarregarFoto(objPessoaFisica);

                PreencherPretensaoSalarial();

                cbPoliticaPrivacidade.Checked = objCurriculo.DataAceitePoliticaPrivacidade.HasValue;
                if (objCurriculo.DataAceitePoliticaPrivacidade.HasValue)
                {
                    cbPoliticaPrivacidade.Attributes.Add("disabled", "disabled");
                }

                //if (!objPessoaFisica.FlagCelularConfirmado)
                //    ValidacaoCelular();

                upnAbaMiniCurriculoDados.Update();
            }
            else
            {

                CelularChecked.Visible = false;

                if (Session["DadosFacebook"] != null)
                {
                    var dadosFacebook = (ProfileFacebook.DadosFacebook)Session["DadosFacebook"];
                    var dadosFotoFacebook = (ProfileFacebook.FotoFacebook)Session["DadosFotoFacebook"];

                    txtNome.Valor = dadosFacebook.Nome;
                    txtDataDeNascimento.ValorDateTime = dadosFacebook.DataNascimento;
                    txtEmail.Text = dadosFacebook.Email;
                    txtCidadeMini.Text = dadosFacebook.Cidade;
                    rblSexo.SelectedValue = dadosFacebook.Sexo.ToString(CultureInfo.InvariantCulture);
                    txtFuncaoPretendida1.Text = dadosFacebook.UltimaFuncao;

                    if (dadosFacebook.education != null)
                    {
                        var escolaridade =
                            dadosFacebook.education.Where(a => a.IdEscolaridade != null)
                                .OrderBy(a => a.IdEscolaridade.Value).FirstOrDefault();

                        if (escolaridade != null)
                        {
                            ddlEscolaridade.SelectedValue =
                                escolaridade.IdEscolaridade.Value.ToString(CultureInfo.InvariantCulture);
                        }
                    }
                    if (dadosFotoFacebook != null && dadosFotoFacebook.data != null)
                    {
                        using (var wc = new System.Net.WebClient())
                        {
                            ucFoto.ImageData = wc.DownloadData(dadosFotoFacebook.data.url);
                        }
                    }
                }
            }

            var funcoesPretendidass = new List<FuncaoPretendida>();
            SalvarFuncoesPretendidas(funcoesPretendidass);

            if (IdPessoaFisica.HasValue)
            {
                string mensagemValidador;
                bool var_teste = false;
                if (!ValidarPrimeiraFuncao(funcoesPretendidass, out var_teste, out mensagemValidador))
                {
                    cvFuncaoPretendida.IsValid = false;
                    cvFuncaoPretendida.ErrorMessage = mensagemValidador;
                }
            }
        }


        #region PreencherUltimoCurso

        private void PreencherFormacao(PessoaFisica objPessoaFisica)
        {
            try
            {
                // Formacao objFormacao = Formacao.LoadObject(IdFormacao.Value);

                Formacao objFormacao;
                Formacao.CarregarMaiorFormacaoPorPessoaFisica(objPessoaFisica, out objFormacao);

                //Valida Formaçao
                if (objFormacao != null)
                {
                    if (objFormacao.IdFormacao != null)
                        IdFormacao = objFormacao.IdFormacao;

                    if (objFormacao.Curso != null)
                    {
                        objFormacao.Curso.CompleteObject();
                        if (objFormacao.Curso.DescricaoCurso != null)
                        {
                            txtTituloCurso.Text = objFormacao.Curso != null ? objFormacao.Curso.DescricaoCurso : string.Empty;
                            ddlSituacao.SelectedValue = objFormacao.SituacaoFormacao != null ? objFormacao.SituacaoFormacao.IdSituacaoFormacao.ToString() : "0";
                            txtPeriodo.Valor = objFormacao.NumeroPeriodo.ToString();
                            DsFormacao = objFormacao.Curso != null ? objFormacao.Curso.DescricaoCurso.ToString() : string.Empty;
                        }
                    }
                    else
                    {
                        if (objFormacao.DescricaoCurso != null)
                        {
                            txtTituloCurso.Text = objFormacao.DescricaoCurso;
                            DsFormacao = objFormacao.DescricaoCurso.ToString();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }

        #endregion

        #region PreencherParametroAceitaEstagio

        /// <summary>
        /// Rotina para verificar se o candidato aceita estagio
        /// </summary>
        /// <param name="objCurriculo"></param>
        private void PreeencherParametroAceitaEstagio(Curriculo objCurriculo)
        {
            ParametroCurriculo aceitaEstagParamCurriculo;
            if (ParametroCurriculo.CarregarParametroPorCurriculo(Enumeradores.Parametro.CurriculoAceitaEstagio, objCurriculo, out aceitaEstagParamCurriculo, null))
                ckbAceitaEstagio.Checked = Convert.ToBoolean(aceitaEstagParamCurriculo.ValorParametro);
        }

        #endregion

        #region ValidacaoCelular
        public void ValidacaoCelular(int? IdPessoaFisica = null)
        {
            if (!string.IsNullOrWhiteSpace(txtTelefoneCelular.DDD) && !string.IsNullOrWhiteSpace(txtTelefoneCelular.Fone))
            {
                int Id = 0;

                if (this.IdPessoaFisica.HasValue)
                    Id = this.IdPessoaFisica.Value;
                else if (IdPessoaFisica.HasValue)
                    Id = IdPessoaFisica.Value;

                if (Id > 0)
                {
                    PessoaFisica pf = new PessoaFisica(Id);
                    CelularChecked.Visible = false;
                    if (pf.EnviarCodigoAtivacaoCelular(txtTelefoneCelular.DDD, txtTelefoneCelular.Fone))
                        base.MostrarMensagemSmsEnviado.Value = "Um SMS de confirmação foi enviado para o número de celular informado.";
                }
            }
        }
        #endregion ValidacaoCelular

        #region CarregarFoto
        /// <summary>
        /// Rotina para carregar a foto do candidato
        /// </summary>
        private void CarregarFoto(PessoaFisica objPessoaFisica)
        {
            try
            {
                FotoWSBytes = null;
                btlExisteFotoWS.Visible = false;

                byte[] byteArray = PessoaFisicaFoto.RecuperarArquivo(objPessoaFisica.CPF);

                if (byteArray != null)
                {
                    ucFoto.LimparFoto();
                    ucFoto.ImageData = byteArray;
                }
                else
                {
                    #region Sou Eu Mesmo
                    try
                    {
                        using (var client = new IntegracaoSouEuMesmo.IntegracaoClient())
                        {
                            byteArray = client.RetornoFotoPessoaFisica(objPessoaFisica.CPF);
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                    #endregion

                    if (byteArray != null)
                    {
                        FotoWSBytes = byteArray;
                        btlExisteFotoWS.Visible = true;
                    }
                }
                upFoto.Update();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region PreencherCamposFuncoesPretendidas
        private void PreencherCamposFuncoesPretendidas(List<FuncaoPretendida> listFuncoesPretendidas)
        {
            if (listFuncoesPretendidas.Count >= 1)
            {
                //Calculando quantidade de experiência
                FuncaoPretendida objFuncaoPretendida = listFuncoesPretendidas[0];

                //Preenchendo campos
                txtAnoExperiencia1.Valor = objFuncaoPretendida.AnosExperiencia.ToString();
                txtMesExperiencia1.Valor = objFuncaoPretendida.MesesExperiencia.ToString();
                txtFuncaoPretendida1.Text = objFuncaoPretendida.Funcao != null ? objFuncaoPretendida.Funcao.DescricaoFuncao : objFuncaoPretendida.DescricaoFuncaoPretendida;
            }

            if (listFuncoesPretendidas.Count >= 2)
            {
                //Calculando quantidade de experiência
                FuncaoPretendida objFuncaoPretendida = listFuncoesPretendidas[1];

                txtAnoExperiencia2.Valor = objFuncaoPretendida.AnosExperiencia.ToString();
                txtMesExperiencia2.Valor = objFuncaoPretendida.MesesExperiencia.ToString();
                txtFuncaoPretendida2.Text = objFuncaoPretendida.Funcao != null ? objFuncaoPretendida.Funcao.DescricaoFuncao : objFuncaoPretendida.DescricaoFuncaoPretendida;
            }

            if (listFuncoesPretendidas.Count.Equals(3))
            {
                //Calculando quantidade de experiência
                FuncaoPretendida objFuncaoPretendida = listFuncoesPretendidas[2];

                txtAnoExperiencia3.Valor = objFuncaoPretendida.AnosExperiencia.ToString();
                txtMesExperiencia3.Valor = objFuncaoPretendida.MesesExperiencia.ToString();
                txtFuncaoPretendida3.Text = objFuncaoPretendida.Funcao != null ? objFuncaoPretendida.Funcao.DescricaoFuncao : objFuncaoPretendida.DescricaoFuncaoPretendida;
            }
        }
        #endregion

        #region PreencherPretensaoSalarial
        private void PreencherPretensaoSalarial()
        {
            string pesquisa = PesquisarMediaSalarial(txtFuncaoPretendida1.Text, txtCidadeMini.Text);

            if (!string.IsNullOrEmpty(pesquisa))
            {
                faixa_salarial.InnerText = string.Format(MensagemAviso._202001, pesquisa.Split(';')[0], pesquisa.Split(';')[1]);
                faixa_salarial.Style["display"] = "block";
            }
            else
            {
                faixa_salarial.InnerText = string.Empty;
                faixa_salarial.Style["display"] = "none";
            }
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            rblSexo.ClearSelection();
            upRblSexo.Update();
            ddlEscolaridade.SelectedValue = "0";
            ddlEscolaridade.SelectedIndex = 0;
        }
        #endregion

        #region Salvar
        /// <summary>
        /// Método responsável por salvar os dados da tela.        
        /// </summary>
        public bool Salvar(out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (txtPretensaoSalarial.Text == "")
            {
                mensagemErro = string.Format("Por favor, preencha a pretensão salarial");
                return false;
            }
            else
            {
                var salarioMinimo = Convert.ToDecimal(Parametro.RecuperaValorParametro(Enumeradores.Parametro.SalarioMinimoNacional));

                if (Convert.ToDecimal(txtPretensaoSalarial.Text) < salarioMinimo)
                {
                    mensagemErro = string.Format("Sua pretensão salarial deve ser maior que o Salário Mínimo Nacional R$ {0}", salarioMinimo);
                    return false;
                }
            }

            PessoaFisica objPessoaFisica;
            Curriculo objCurriculo;
            UsuarioFilialPerfil objUsuarioFilialPerfil;
            PessoaFisicaComplemento objPessoaFisicaComplemento;
            PessoaFisicaFoto objPessoaFisicaFoto;
            bool novoUsuario = false;

            //Se não foi identificada a pessoa física que está sendo editada, validamos cpf e data para edição.         
            bool novoCurriculo = false;
            int idPessoaFisica;
            PessoaFisica.ExistePessoaFisica(txtCPF.Valor, out idPessoaFisica);

            if (IdPessoaFisica.HasValue && IdPessoaFisica.Value == idPessoaFisica)
            {
                if (PessoaFisica.ExistePessoaFisica(txtCPF.Valor, out idPessoaFisica))
                {
                    if (PessoaFisica.ValidarCPFDataNascimento(txtCPF.Valor, txtDataDeNascimento.ValorDateTime.Value))
                    {
                        IdPessoaFisica = idPessoaFisica;
                    }
                    else
                    {
                        mensagemErro = "CPF já cadastrado e a data de nascimento não confere. Por favor entre em contato com o atendimento 0800 41 2400.";
                        return false;
                    }
                }
                else
                {
                    IdPessoaFisica = null;
                }
            }
            else if (idPessoaFisica > 0)
            {
                if (PessoaFisica.ValidarCPFDataNascimento(txtCPF.Valor, txtDataDeNascimento.ValorDateTime.Value))
                {
                    IdPessoaFisica = idPessoaFisica;
                }
                else
                {
                    mensagemErro = "CPF já cadastrado e a data de nascimento não confere. Por favor entre em contato com o atendimento 0800 41 2400.";
                    return false;
                }
            }
            else
            {
                novoCurriculo = true;
                IdPessoaFisica = null;
            }


            string emailCadastrado = String.Empty;
            if (IdPessoaFisica.HasValue)
            {
                objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);

                emailCadastrado = objPessoaFisica.EmailPessoa;
                if (!PessoaFisicaFoto.CarregarFoto(objPessoaFisica.IdPessoaFisica, out objPessoaFisicaFoto))
                    objPessoaFisicaFoto = new PessoaFisicaFoto();

                if (objPessoaFisica.Endereco != null)
                    objPessoaFisica.Endereco.CompleteObject();
                else
                    objPessoaFisica.Endereco = new Endereco();

                if (!PessoaFisicaComplemento.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objPessoaFisicaComplemento))
                    objPessoaFisicaComplemento = new PessoaFisicaComplemento();

                if (!Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                {
                    objCurriculo = new Curriculo();
                    novoCurriculo = true;
                }

                if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(objPessoaFisica, out objUsuarioFilialPerfil))
                {
                    objUsuarioFilialPerfil = new UsuarioFilialPerfil
                    {
                        Perfil = new Perfil((int)Enumeradores.Perfil.AcessoNaoVIP)
                    };
                }
            }
            else
            {
                novoUsuario = true;
                novoCurriculo = true;

                objPessoaFisica = new PessoaFisica();
                objCurriculo = new Curriculo();
                objPessoaFisica.Endereco = new Endereco();
                objPessoaFisicaComplemento = new PessoaFisicaComplemento();
                objPessoaFisicaFoto = new PessoaFisicaFoto();
                objUsuarioFilialPerfil = new UsuarioFilialPerfil
                {
                    Perfil = new Perfil((int)Enumeradores.Perfil.AcessoNaoVIP)
                };
            }

            var listFuncoesPretendidas = new List<FuncaoPretendida>();
            var marcar_flag_aceita_estagio = false;
            SalvarFuncoesPretendidas(listFuncoesPretendidas);
            //Validação da função pretendida sobre as funções.
            cvFuncaoPretendida.IsValid = true;
            string validadorMensagem;
            if (!ValidarPrimeiraFuncao(listFuncoesPretendidas, out marcar_flag_aceita_estagio, out validadorMensagem))
            {
                cvFuncaoPretendida.IsValid = false;
                cvFuncaoPretendida.ErrorMessage = validadorMensagem;
                mensagemErro = "Digite por extenso a função e selecione a opção desejada, cada campo aceitará apenas UMA função. Ex: Auxiliar Administrativo";
                return false;
            }

            if (!ValidarEscolaridade())
            {
                rfvEscolaridade.IsValid = false;
                mensagemErro = "É necessário selecionar a sua escolaridade.";
                return false;
            }

            //Pessoa Física
            objPessoaFisica.NumeroCPF = txtCPF.Valor;
            objPessoaFisica.DataNascimento = txtDataDeNascimento.ValorDateTime.Value;
            objPessoaFisica.NomePessoa = UIHelper.AjustarString(txtNome.Valor);
            objPessoaFisica.NomePessoaPesquisa = UIHelper.RemoverAcentos(txtNome.Valor);
            objPessoaFisica.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));
            objPessoaFisica.FlagWhatsApp = cbWhats.Checked;

            if (!string.IsNullOrWhiteSpace(rblSexo.SelectedValue))
                objPessoaFisica.Sexo = new Sexo(Convert.ToInt32(rblSexo.SelectedValue));

            objPessoaFisica.NumeroDDDCelular = txtTelefoneCelular.DDD;
            objPessoaFisica.NumeroCelular = txtTelefoneCelular.Fone;
            objPessoaFisica.FlagInativo = false;
            objPessoaFisica.DescricaoIP = objCurriculo.DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (!string.IsNullOrEmpty(txtEmail.Text) || (!String.IsNullOrEmpty(txtAT.Text) || (ddlAt.SelectedItem.Text != "Selecione" && ddlAt.SelectedItem.Text != "Outros")))
            {
                if (!String.IsNullOrEmpty(txtAT.Text))
                    objPessoaFisica.EmailPessoa = string.Format("{0}@{1}", txtEmail.Text.Replace("@", ""), txtAT.Text.Trim());
                else
                    objPessoaFisica.EmailPessoa = string.Format("{0}@{1}", txtEmail.Text.Replace("@", ""), (ddlAt.SelectedItem.Text == "Selecione" ? "" : ddlAt.SelectedItem.Text));

                if (!BLL.Custom.Validacao.ValidarEmail(objPessoaFisica.EmailPessoa))
                {
                    mensagemErro = string.Format("E-mail incorreto");
                    return false;
                }
            }
            else
            {
                objPessoaFisica.EmailPessoa = string.Empty;
            }



            //Endereco
            Cidade objCidade;

            if (Cidade.CarregarPorNome(txtCidadeMini.Text, out objCidade))
                objPessoaFisica.Endereco.Cidade = objCidade;

            //De acordo com o bug 11720
            objPessoaFisica.Cidade = objCidade;

            //Currículo 
            objCurriculo.ValorPretensaoSalarial = (txtPretensaoSalarial.Text != "" ? Convert.ToDecimal(txtPretensaoSalarial.Text) : 0);
            if (!objCurriculo.DataAceitePoliticaPrivacidade.HasValue && cbPoliticaPrivacidade.Checked)
            {
                objCurriculo.DataAceitePoliticaPrivacidade = DateTime.Now;
            }

            //Só deve salvar passar uma origem, se o usuário logado não for interno, do contrário os currículos que estão no RHOffice virão para o BNE
            //Isso porque, a parte administrativa não é sensibilizada pela Origem (STC) dos dados
            Origem objOrigem = null;
            if (!base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                objOrigem = new Origem(base.IdOrigem.Value);

            #region Foto
            if (ucFoto.ImageData != null && !ucFoto.ImageData.Length.Equals(0))
            {
                objPessoaFisicaFoto.ImagemPessoa = ucFoto.ImageData;
                objPessoaFisicaFoto.FlagInativo = false;
            }
            else
            {
                objPessoaFisicaFoto.ImagemPessoa = null;
                objPessoaFisicaFoto.FlagInativo = true;
            }
            #endregion

            var dadosFacebook = (ProfileFacebook.DadosFacebook)Session["DadosFacebook"];

            #region Boas Vindas Novamente
            //Se a data de Atualização foi a mais de 90 dias envia email de Boas Vindas Novamente
            if (ValidarCadastroExistente())
            {
                BNE.Web.UserControls.Login login = new Login();

                login.BoasVindasNovamente(objPessoaFisica);
            }
            #endregion Boas Vindas Novamente

            if (objCurriculo.SituacaoCurriculo != null)
                EnumSituacaoCurriculo = (Enumeradores.SituacaoCurriculo)objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo;
            //Salvar
            objCurriculo.SalvarMiniCurriculo(objPessoaFisica,
                listFuncoesPretendidas,
                objOrigem,
                null,
                SalvarFormacao(),
                objUsuarioFilialPerfil,
                objPessoaFisicaComplemento,
                EnumSituacaoCurriculo,
                objPessoaFisicaFoto,
                dadosFacebook);

            //Email diferente do cadastrado salva na curriculo observação
            if (!novoCurriculo && !objPessoaFisica.EmailPessoa.Equals(emailCadastrado))
            {
                try
                {
                    if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                        CurriculoObservacao.SalvarCRM(string.Format("E-mail alterado de {0} para {1}", emailCadastrado, objPessoaFisica.EmailPessoa), objCurriculo, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);
                    else
                        CurriculoObservacao.SalvarCRM(string.Format("E-mail alterado de {0} para {1}", emailCadastrado, objPessoaFisica.EmailPessoa), objCurriculo, objUsuarioFilialPerfil, null);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Erro no salvar alteração do email no CRM - tela do minicurriculo");
                }

            }


            SalvarFlagAceitaEstagio(objCurriculo, marcar_flag_aceita_estagio);

            //Todo incluir curso
            // SalvarFormacao(objPessoaFisica);

            if (novoUsuario)
            {
                if (!base.STC.Value) //Se é um cadastro novo sem pessoa física e não é dentro de um STC
                    Session.Add(Chave.Permanente.MostrarModalDegustacaoCandidatura.ToString(), true); //Adiciona na session um propriedade para mostrar a modal de degustação)

                LogarUsuarioCurriculo(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objCurriculo.IdCurriculo, Convert.ToDecimal(new string(txtCPF.Valor.Where(char.IsNumber).ToArray())), objPessoaFisica.DataNascimento);
            }
            else
            {
                if (novoCurriculo ||
                    Context.User.Identity == null ||
                    !Context.User.Identity.IsAuthenticated ||
                    ((Context.User.Identity as ClaimsIdentity ?? new ClaimsIdentity()).GetPessoaFisicaId().GetValueOrDefault() != objPessoaFisica.IdPessoaFisica))
                {
                    LogarUsuarioCurriculo(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objCurriculo.IdCurriculo, Convert.ToDecimal(new string(txtCPF.Valor.Where(char.IsNumber).ToArray())), objPessoaFisica.DataNascimento);
                }
            }

            //Ajustando o ID da Pessoa Física
            IdPessoaFisica = objPessoaFisica.IdPessoaFisica;

            //Ajustando o ID do Curriculo da Pessoa Fisica
            IdCurriculo = objCurriculo.IdCurriculo;


            #region Validação Celular
            if (!objPessoaFisica.FlagCelularConfirmado)
            {
                ValidacaoCelular(objPessoaFisica.IdPessoaFisica);
            }
            #endregion

            Handlers.PessoaFisicaFoto.RemoverFotoCache(objPessoaFisica.NumeroCPF);

            return true;
        }

        private Formacao SalvarFormacao()
        {
            Formacao objFormacao = null;
            try
            {
                objFormacao = IdFormacao.HasValue ? Formacao.LoadObject(IdFormacao.Value) : new Formacao();

                //TODO: VALDECIR - PERSISTENCIA - CARREGAR ULTIMA FORMACAO DO CV
                //Formação acima - salvar novo
                //Formação ensino medio para baixo apagar, quando adicionado tecnologo para cima.
                //

                switch (Convert.ToInt16(ddlEscolaridade.SelectedValue))
                {
                    case (int)BNE.BLL.Enumeradores.Escolaridade.EnsinoFundamentalIncompleto:
                    case (int)BNE.BLL.Enumeradores.Escolaridade.EnsinoFundamentalCompleto:
                    case (int)BNE.BLL.Enumeradores.Escolaridade.EnsinoMedioIncompleto:
                    case (int)BNE.BLL.Enumeradores.Escolaridade.EnsinoMedioCompleto:
                    case (int)BNE.BLL.Enumeradores.Escolaridade.TecnicoPosMedioIncompleto:
                    case (int)BNE.BLL.Enumeradores.Escolaridade.TecnicoPosMedioCompleto:
                        #region [Ensino Medio]
                        objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));
                        #endregion
                        break;
                    case (int)BNE.BLL.Enumeradores.Escolaridade.TecnologoIncompleto:
                    case (int)BNE.BLL.Enumeradores.Escolaridade.SuperiorIncompleto:
                        #region [Ensino que aceita estagio]

                        if (IdFormacao.HasValue)
                        {

                            if (!objFormacao.Escolaridade.IdEscolaridade.Equals(Convert.ToInt32(ddlEscolaridade.SelectedValue)))
                                objFormacao = new Formacao();

                        }
                        else
                            objFormacao = new Formacao();

                        Curso objCurso;
                        if (Curso.CarregarPorNome(txtTituloCurso.Text, out objCurso))
                        {
                            objFormacao.Curso = objCurso;
                            objFormacao.DescricaoCurso = String.Empty;

                        }
                        else
                        {
                            objFormacao.Curso = null;
                            objFormacao.DescricaoCurso = txtTituloCurso.Text;
                        }
                        if (!String.IsNullOrEmpty(txtPeriodo.Valor))
                            objFormacao.NumeroPeriodo = Convert.ToInt16(txtPeriodo.Valor);
                        if (ddlSituacao.SelectedValue != "0")
                            objFormacao.SituacaoFormacao = new SituacaoFormacao(Convert.ToInt16(ddlSituacao.SelectedValue));
                        objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));
                        //objFormacao.Save();

                        DsFormacao = txtTituloCurso.Text;
                        #endregion
                        break;
                    case (int)BNE.BLL.Enumeradores.Escolaridade.TecnologoCompleto:
                        #region [Tecnologo Completo]
                        if (IdFormacao.HasValue &&
                            objFormacao.Escolaridade.IdEscolaridade.Equals((int)BLL.Enumeradores.Escolaridade.TecnologoIncompleto))
                            objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));
                        else if (IdFormacao.HasValue &&
                              !objFormacao.Escolaridade.IdEscolaridade.Equals((int)BLL.Enumeradores.Escolaridade.TecnologoCompleto))
                        {
                            objFormacao = new Formacao();
                            objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));
                        }
                        else
                            objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));

                        #endregion
                        break;
                    case (int)BNE.BLL.Enumeradores.Escolaridade.SuperiorCompleto:
                        #region [Superiror Completo]
                        if (IdFormacao.HasValue &&
                            objFormacao.Escolaridade.IdEscolaridade.Equals((int)BLL.Enumeradores.Escolaridade.SuperiorIncompleto))
                            objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));
                        else if (IdFormacao.HasValue &&
                          !objFormacao.Escolaridade.IdEscolaridade.Equals((int)BLL.Enumeradores.Escolaridade.SuperiorCompleto))
                        {
                            objFormacao = new Formacao();
                            objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));
                        }
                        else
                            objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));
                        #endregion
                        break;
                    case (int)BNE.BLL.Enumeradores.Escolaridade.PosGraduacaoEspecializacao:
                    case (int)BNE.BLL.Enumeradores.Escolaridade.Mestrado:
                    case (int)BNE.BLL.Enumeradores.Escolaridade.Doutorado:
                    case (int)BNE.BLL.Enumeradores.Escolaridade.PosDoutorado:
                        #region [Ensino PosGraduacao para cima]
                        if (IdFormacao.HasValue && objFormacao.Escolaridade.IdEscolaridade.Equals(Convert.ToInt32(ddlEscolaridade.SelectedValue)))
                            objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));
                        else
                        {
                            objFormacao = new Formacao();
                            objFormacao.Escolaridade = new Escolaridade(Convert.ToInt32(ddlEscolaridade.SelectedValue));
                        }
                        #endregion
                        break;
                    case (int)BNE.BLL.Enumeradores.Escolaridade.OutrosCursos:
                        break;
                    default:

                        break;
                }
                return objFormacao;

            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
                return objFormacao;
            }
        }

        private void SalvarFlagAceitaEstagio(Curriculo objCurriculo, bool aceita_estagio)
        {
            try
            {
                bool aceitaEstag = ckbAceitaEstagio.Checked;
                aceitaEstag = (aceita_estagio) ? true : aceitaEstag;

                ParametroCurriculo aceitaEstagParamCurriculo;

                if (ParametroCurriculo.CarregarParametroPorCurriculo(Enumeradores.Parametro.CurriculoAceitaEstagio, objCurriculo, out aceitaEstagParamCurriculo, null))
                {
                    aceitaEstagParamCurriculo.ValorParametro = aceitaEstag.ToString();
                }
                else
                {
                    aceitaEstagParamCurriculo = new ParametroCurriculo
                    {
                        Curriculo = objCurriculo,
                        Parametro = new Parametro((int)Enumeradores.Parametro.CurriculoAceitaEstagio),
                        ValorParametro = aceitaEstag.ToString()
                    };
                }
                aceitaEstagParamCurriculo.Save();

            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }

        private bool ValidarEscolaridade()
        {
            if (ddlEscolaridade.SelectedIndex == 0)
            {
                return false;
            }

            if (ddlEscolaridade.SelectedValue == null)
            {
                return false;
            }

            int escolaridadeId;
            if (Int32.TryParse(ddlEscolaridade.SelectedValue, out escolaridadeId))
            {
                if (escolaridadeId > 0)
                    return true;
            }
            return false;
        }
        #endregion

        #region SalvarFuncoesPretendidas
        private void SalvarFuncoesPretendidas(List<FuncaoPretendida> listFuncoesPretendidas)
        {
            short? expAnos, expMeses;

            if (!String.IsNullOrEmpty(txtFuncaoPretendida1.Text))
            {
                expAnos = expMeses = 0;

                var objFuncaoPretendida = new FuncaoPretendida();

                if (!String.IsNullOrEmpty(txtAnoExperiencia1.Valor))
                    expAnos = Convert.ToInt16(txtAnoExperiencia1.Valor);

                if (!String.IsNullOrEmpty(txtMesExperiencia1.Valor))
                    expMeses = Convert.ToInt16(txtMesExperiencia1.Valor);

                objFuncaoPretendida.QuantidadeExperiencia = Convert.ToInt16((expAnos.Value * 12) + expMeses.Value);

                Funcao objFuncao;
                FuncaoErroSinonimo objFuncaoErroSinonimo;
                if (Funcao.CarregarPorDescricao(txtFuncaoPretendida1.Text, out objFuncao))
                {
                    objFuncaoPretendida.Funcao = objFuncao;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                }
                else if (FuncaoErroSinonimo.CarregarPorDescricao(txtFuncaoPretendida1.Text, out objFuncaoErroSinonimo))
                {
                    objFuncaoPretendida.Funcao = objFuncaoErroSinonimo.Funcao;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                }
                else
                {
                    objFuncaoPretendida.Funcao = null;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = txtFuncaoPretendida1.Text;
                }

                listFuncoesPretendidas.Add(objFuncaoPretendida);
            }

            if (!String.IsNullOrEmpty(txtFuncaoPretendida2.Text))
            {
                expAnos = expMeses = 0;

                var objFuncaoPretendida = new FuncaoPretendida();

                if (!String.IsNullOrEmpty(txtAnoExperiencia2.Valor))
                    expAnos = Convert.ToInt16(txtAnoExperiencia2.Valor);

                if (!String.IsNullOrEmpty(txtMesExperiencia2.Valor))
                    expMeses = Convert.ToInt16(txtMesExperiencia2.Valor);

                objFuncaoPretendida.QuantidadeExperiencia = Convert.ToInt16((expAnos.Value * 12) + expMeses.Value);

                Funcao objFuncao;
                FuncaoErroSinonimo objFuncaoErroSinonimo;
                if (Funcao.CarregarPorDescricao(txtFuncaoPretendida2.Text, out objFuncao))
                {
                    objFuncaoPretendida.Funcao = objFuncao;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                }
                else if (FuncaoErroSinonimo.CarregarPorDescricao(txtFuncaoPretendida2.Text, out objFuncaoErroSinonimo))
                {
                    objFuncaoPretendida.Funcao = objFuncaoErroSinonimo.Funcao;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                }
                else
                {
                    objFuncaoPretendida.Funcao = null;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = txtFuncaoPretendida2.Text;
                }

                listFuncoesPretendidas.Add(objFuncaoPretendida);
            }

            if (!String.IsNullOrEmpty(txtFuncaoPretendida3.Text))
            {
                expAnos = expMeses = 0;

                var objFuncaoPretendida = new FuncaoPretendida();

                if (!String.IsNullOrEmpty(txtAnoExperiencia3.Valor))
                    expAnos = Convert.ToInt16(txtAnoExperiencia3.Valor);

                if (!String.IsNullOrEmpty(txtMesExperiencia3.Valor))
                    expMeses = Convert.ToInt16(txtMesExperiencia3.Valor);

                objFuncaoPretendida.QuantidadeExperiencia = Convert.ToInt16((expAnos.Value * 12) + expMeses.Value);

                Funcao objFuncao;
                FuncaoErroSinonimo objFuncaoErroSinonimo;
                if (Funcao.CarregarPorDescricao(txtFuncaoPretendida3.Text, out objFuncao))
                {
                    objFuncaoPretendida.Funcao = objFuncao;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                }
                else if (FuncaoErroSinonimo.CarregarPorDescricao(txtFuncaoPretendida3.Text, out objFuncaoErroSinonimo))
                {
                    objFuncaoPretendida.Funcao = objFuncaoErroSinonimo.Funcao;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = String.Empty;
                }
                else
                {
                    objFuncaoPretendida.Funcao = null;
                    objFuncaoPretendida.DescricaoFuncaoPretendida = txtFuncaoPretendida3.Text;
                }

                listFuncoesPretendidas.Add(objFuncaoPretendida);
            }


            //Verifica se após 
            //if (listFuncoesPretendidas.Count.Equals(2))
            //{
            //    //Se as duas funções pretendidas tiverem a mesma função
            //    if ((listFuncoesPretendidas[0] != null && listFuncoesPretendidas[0].Funcao != null) && (listFuncoesPretendidas[1] != null && listFuncoesPretendidas[1].Funcao != null))
            //    {
            //        if (listFuncoesPretendidas[0].Funcao.IdFuncao.Equals(listFuncoesPretendidas[1].Funcao.IdFuncao))
            //            listFuncoesPretendidas[1] = null;
            //    }
            //}

            //if (listFuncoesPretendidas.Count.Equals(3))
            //{
            //    //Se as duas funções pretendidas tiverem a mesma função, comparando a primeira com a segunda
            //    if ((listFuncoesPretendidas[0] != null && listFuncoesPretendidas[0].Funcao != null) && (listFuncoesPretendidas[1] != null && listFuncoesPretendidas[1].Funcao != null))
            //    {
            //        if (listFuncoesPretendidas[0].Funcao.IdFuncao.Equals(listFuncoesPretendidas[1].Funcao.IdFuncao))
            //            listFuncoesPretendidas[1] = null;
            //    }

            //    //Se as duas funções pretendidas tiverem a mesma função, comparando a primeira com a terceira
            //    if ((listFuncoesPretendidas[0] != null && listFuncoesPretendidas[0].Funcao != null) && (listFuncoesPretendidas[2] != null && listFuncoesPretendidas[2].Funcao != null))
            //    {
            //        if (listFuncoesPretendidas[0].Funcao.IdFuncao.Equals(listFuncoesPretendidas[2].Funcao.IdFuncao))
            //            listFuncoesPretendidas[2] = null;
            //    }

            //    //Se as duas funções pretendidas tiverem a mesma função, e compara a segunda com a terceira
            //    if ((listFuncoesPretendidas[1] != null && listFuncoesPretendidas[1].Funcao != null) && (listFuncoesPretendidas[2] != null && listFuncoesPretendidas[2].Funcao != null))
            //    {
            //        if (listFuncoesPretendidas[1].Funcao.IdFuncao.Equals(listFuncoesPretendidas[2].Funcao.IdFuncao))
            //            listFuncoesPretendidas[2] = null;
            //    }
            //}
        }
        #endregion

        #region VerificarFuncoesPretendidas
        /// <summary>
        /// Método responsável por verificar se funções informadas na tela são iguais
        /// </summary>
        /// <returns></returns>
        private bool VerificarFuncoesPretendidas()
        {
            if (txtFuncaoPretendida1.Text.Equals(txtFuncaoPretendida2.Text))
                return false;
            if (txtFuncaoPretendida1.Text.Equals(txtFuncaoPretendida3.Text) && !string.IsNullOrEmpty(txtFuncaoPretendida3.Text))
                return false;
            if (txtFuncaoPretendida2.Text.Equals(txtFuncaoPretendida3.Text) && !string.IsNullOrEmpty(txtFuncaoPretendida3.Text))
                return false;

            return true;
        }
        #endregion

        #region ValidarPrimeiraFuncao
        private bool ValidarPrimeiraFuncao(List<FuncaoPretendida> listFuncoesPretendidas, out bool aceita_estagio, out string validadorMensagem)
        {
            aceita_estagio = false;
            if (listFuncoesPretendidas.Count > 0)
            {
                FuncaoPretendida objFuncaoPretendida = listFuncoesPretendidas[0];
                if (objFuncaoPretendida == null
                    || objFuncaoPretendida.Funcao == null)
                {
                    validadorMensagem = "Função Inválida";
                    return false;
                }

                if (objFuncaoPretendida.Funcao.DescricaoFuncao != null && objFuncaoPretendida.Funcao.DescricaoFuncao.Equals("Aprendiz"))
                {
                    validadorMensagem = "Função Indisponível";
                    return false;
                }



                foreach (var fp in listFuncoesPretendidas)
                {
                    if (fp.Funcao != null)
                    {
                        fp.Funcao.CompleteObject();
                        if (fp.Funcao.DescricaoFuncao.Equals("Estagiário", StringComparison.OrdinalIgnoreCase))
                        {
                            aceita_estagio = true;
                            break;
                        }
                    }
                    else
                    {
                        if (fp.DescricaoFuncaoPretendida.Equals("Estagiário", StringComparison.OrdinalIgnoreCase))
                        {
                            aceita_estagio = true;
                            break;
                        }
                    }
                }


            }

            validadorMensagem = string.Empty;
            return true;
        }
        #endregion

        #region LogarUsuarioCandidato
        /// <summary>
        /// Metodo responsável por logar a pessoa que está cadastrando o currículo
        /// </summary>
        /// <param name="idPessoaFisica">IdPessoaFisica do Currículo</param>
        /// <param name="idCurriculo">IdCurriculoSession da pessoa que está cadastrando</param>
        private void LogarUsuarioCandidato(int idPessoaFisica, int idCurriculo)
        {
            base.IdPessoaFisicaLogada.Value = idPessoaFisica;
            base.IdCurriculo.Value = idCurriculo;

            UsuarioFilialPerfil objUsuarioFilialPerfil;
            if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(new PessoaFisica(idPessoaFisica), out objUsuarioFilialPerfil))
                base.IdUsuarioFilialPerfilLogadoCandidato.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

            PageHelper.AtualizarSessaoUsuario(idPessoaFisica, Session.SessionID);
            AjustarLogin();

            GravarCookieLoginVagas(PessoaFisica.LoadObject(idPessoaFisica));
        }
        #endregion

        #endregion


        #region [ComprouVipPeixeUrbano]
        /// <summary>
        /// Utilizou codigo do peixe urbano libera o vip
        /// </summary>
        private void ComprouVipPeixeUrbano()
        {
            var idCodigoDesconto = new BasePage().PagamentoIdCodigoDesconto;
            if (base.IdCurriculo.HasValue && idCodigoDesconto.HasValue)
            {
                CodigoDesconto objCodigoDesconto = CodigoDesconto.LoadObject(idCodigoDesconto.Value);
                if (!objCodigoDesconto.JaUtilizado() && objCodigoDesconto.Parceiro.IdParceiro.Equals((int)Enumeradores.CodigoDescontoParceiro.PeixeUrbano))
                {
                    string erro;

                    if (BLL.Pagamento.ConcederDescontoIntegral(
                           new Curriculo(base.IdCurriculo.Value),
                           new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoCandidato.Value),
                           new Plano(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoTrimestralPeixeUrbano))),
                           objCodigoDesconto,
                           out erro))
                    {
                        Session.Remove(Chave.Permanente.MostrarModalDegustacaoCandidatura.ToString());
                        new BasePage().LimparSessionPagamento();

                    }
                }

            }
        }
        #endregion


        #region [TratamentoEmail]

        private void TratamentoEmail(String Email)
        {
            string At;
            txtEmail.Text = UIHelper.TratarEmail(Email.ToLower(), out At);
            At = At.Trim();
            if (!DeParaEmail.ExistsDomainAt(At))
            {
                txtAT.Visible = true;
                ddlAt.Visible = false;
                txtAT.Visible = true;
                // rfvAt.Enabled = true;
                // revTxtAt.Enabled = true;
                ddlAt.Visible = false;
                txtAT.Text = At;
            }
            else
                ddlAt.SelectedValue = ddlAt.Items.FindByText(At).Value;
        }
        #endregion

        #endregion

        #region Eventos

        #region Page
        /// <summary>
        /// Evento executado a Carregar a página.
        /// Em sua primeira inicilização, o evento é responsável por
        /// executar os métodos de inicialização da página.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

            if (!base.STC.Value || (base.STC.Value && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue))
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "MiniCurriculo");
            else
                InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "MiniCurriculo");

            // revEmail.ValidationExpression = Configuracao.regexEmail;


            Ajax.Utility.RegisterTypeForAjax(typeof(MiniCurriculo));
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var parametros = new
            {
                msgFaixaSalarial = MensagemAviso._202001
            };
            ScriptManager.RegisterStartupScript(this, GetType(), "InicializarPage_PreRender", "javaScript:InicializarAutoCompleteMiniCurriculo(" + new JSONReflector(parametros) + ");", true);
            ScriptManager.RegisterStartupScript(upFuncaoPretendida1, upFuncaoPretendida1.GetType(), "FuncaoPretendida_OnChange", "javaScript:FuncaoPretendida_OnChange();", true);
            ScriptManager.RegisterStartupScript(upFuncaoPretendida1, upFuncaoPretendida1.GetType(), "FuncaoPretendida_OnBlur", "javaScript:FuncaoPretendida_OnBlur();", true);
        }
        #endregion


        #region txtDataDeNascimento
        /// <summary>
        /// Responsável por verificar junto com o valor do cpf se a pessoa já se cadastrou.
        /// Caso os valores já existam na base de dados, carrega o formulário em modo de edição.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtDataDeNascimento_ValorAlterado(object sender, EventArgs e)
        {
            ValidarCPFDataNascimento();
        }
        #endregion

        #region txtCPF_ValorAlterado
        /// <summary>
        /// Responsável por verificar junto com o valor do cpf se a pessoa já se cadastrou.
        /// Caso os valores já existam na base de dados, carrega o formulário em modo de edição.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCPF_ValorAlterado(object sender, EventArgs e)
        {
            ValidarCPFDataNascimento();
        }
        #endregion

        #region CarregarParametros
        /// <summary>
        /// Carrega os parâmetros iniciais da aba de dados gerais.
        /// </summary>
        private void CarregarParametros()
        {
            try
            {
                var parametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.SalarioMinimoNacional
                    };

                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                hdfValorSalarioMinimo.Value = valoresParametros[Enumeradores.Parametro.SalarioMinimoNacional];
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnSalvarCurriculo
        /// <summary>
        /// Responsável por executar as ações nescessárias para salvar um currículo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalvarCurriculo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarDataNascimento())
                {
                    if (!CadastroBloqueado)
                    {
                        if (ValidarCadastroExistente())
                        {
                            if (VerificarFuncoesPretendidas())
                            {
                                string mensagemErro;
                                if (Salvar(out mensagemErro))
                                {
                                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;
                                    LogarUsuarioCandidato(IdPessoaFisica.Value, IdCurriculo.Value);
                                    //Gravar no cookie para não aparecer mais a popup
                                    GravarCookiePreCadastro("PreCadastro", "Logado");
                                    ComprouVipPeixeUrbano();
                                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoDados.ToString(), null));
                                }
                                else
                                {
                                    ExibirMensagem(mensagemErro, TipoMensagem.Erro);
                                }
                            }
                            else
                            {
                                ExibirMensagem("Erro! Função já cadastrada no currículo", TipoMensagem.Erro);
                            }
                        }
                        else
                        {
                            ExibirMensagem(MensagemAviso._100108, TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        ExibirMensagem("Cadastro bloqueado, procure nosso atendimento on-line", TipoMensagem.Erro);
                    }
                }
                else
                {
                    ExibirMensagem("Por favor preencha a Data de nascimento.", TipoMensagem.Erro);
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlDadosPessoais
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlDadosPessoais_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CadastroBloqueado)
                {
                    string mensagemErro;
                    if (Salvar(out mensagemErro))
                    {
                        ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
                        LogarUsuarioCandidato(IdPessoaFisica.Value, IdCurriculo.Value);

                        if (IdPessoaFisica.HasValue)
                        {
                            Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;
                            //Gravar no cookie para não aparecer mais a popup
                            GravarCookiePreCadastro("PreCadastro", "Logado");
                        }

                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoDados.ToString(), null));
                    }
                    else
                        ExibirMensagem(mensagemErro, TipoMensagem.Erro);
                }
                else
                {
                    ExibirMensagem("Cadastro bloqueado, procure nosso atendimento on-line", TipoMensagem.Erro);
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao Salvar Mini Curriculo");
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlFormacaoCursos_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlFormacaoCursos_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CadastroBloqueado)
                {
                    string mensagemErro;
                    if (Salvar(out mensagemErro))
                    {
                        ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                        LogarUsuarioCandidato(IdPessoaFisica.Value, IdCurriculo.Value);

                        if (IdPessoaFisica.HasValue)
                            Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoFormacao.ToString(), null));
                    }
                    else
                    {
                        ExibirMensagem(mensagemErro, TipoMensagem.Erro);
                    }
                }
                else
                {
                    ExibirMensagem("Cadastro bloqueado, procure nosso atendimento on-line", TipoMensagem.Erro);
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlDadosComplementares_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlDadosComplementares_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CadastroBloqueado)
                {
                    string mensagemErro;
                    if (Salvar(out mensagemErro))
                    {
                        ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
                        LogarUsuarioCandidato(IdPessoaFisica.Value, IdCurriculo.Value);

                        if (IdPessoaFisica.HasValue)
                            Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoComplementar.ToString(), null));
                    }
                    else
                        ExibirMensagem(mensagemErro, TipoMensagem.Erro);
                }
                else
                {
                    ExibirMensagem("Cadastro bloqueado, procure nosso atendimento on-line", TipoMensagem.Erro);
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlRevisaoDados_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlRevisaoDados_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CadastroBloqueado)
                {
                    string mensagemErro;
                    if (Salvar(out mensagemErro))
                    {
                        ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
                        LogarUsuarioCandidato(IdPessoaFisica.Value, IdCurriculo.Value);

                        if (IdPessoaFisica.HasValue)
                            Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoRevisao.ToString(), null));
                    }
                    else
                        ExibirMensagem(mensagemErro, TipoMensagem.Erro);
                }
                else
                {
                    ExibirMensagem("Cadastro bloqueado, procure nosso atendimento on-line", TipoMensagem.Erro);
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlGestao_Click
        /// <summary>
        /// habilitar aba Gestao
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlGestao_Click(object sender, EventArgs e)
        {
            try
            {
                string mensagemErro;
                if (Salvar(out mensagemErro))
                {
                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                    if (IdPessoaFisica.HasValue)
                        Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                    Redirect("~/CadastroCurriculoGestao.aspx");
                }
                else
                {
                    ExibirMensagem(mensagemErro, TipoMensagem.Erro);
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnAbrirModalCep_Click
        protected void btnAbrirModalCep_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region ucFoto_Error
        protected void ucFoto_Error(Exception ex)
        {
            if (ex is ImageSlicerException)
            {
                ExibirMensagem(ex.Message, TipoMensagem.Erro);
            }
            else
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnExisteFotoWS_Click
        protected void btlExisteFotoWS_Click(object sender, EventArgs e)
        {
            ucFoto.ImageData = FotoWSBytes;
            FotoWSBytes = null;
            btlExisteFotoWS.Visible = false;

            upFoto.Update();
        }
        #endregion

        #region txtTelefoneCelular_OnValorAlteradoFone
        protected void txtTelefoneCelular_OnValorAlteradoFone(object sender, EventArgs e)
        {
            //ValidacaoCelular();
        }
        #endregion txtTelefoneCelular_OnValorAlteradoFone

        #region ddlAt_SelectedIndexChanged
        protected void ddlAt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAt.SelectedItem.Text == "Outros")
            {
                #region [Validação]
                //rfvAt.Enabled = true;
                // revTxtAt.Enabled = true;
                #endregion

                txtAT.Visible = true;
                ddlAt.Visible = false;
                txtAT.Focus();
                upEmail.Update();
            }
            else if (ddlAt.SelectedItem.Text != "Selecione")
                rfvEmail.Enabled = true;
            else
                rfvEmail.Enabled = false;

            ScriptManager.RegisterStartupScript(upEmail, upEmail.GetType(), "blockAt", "blockAt();", true);
        }
        #endregion

        #region txtAT_TextChanged
        protected void txtAT_TextChanged(object sender, EventArgs e)
        {
            rfvAt.Enabled = !string.IsNullOrWhiteSpace(txtAT.Text);
            rfvEmail.Enabled = !string.IsNullOrWhiteSpace(txtAT.Text);
            rfvDdlAt.Enabled = !string.IsNullOrWhiteSpace(txtAT.Text);

            if (String.IsNullOrEmpty(this.txtAT.Text))
                return;
            VerificaHotmail();

        }
        #endregion

        #region VerificaHotmail
        private void VerificaHotmail()
        {
            string[] lista = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailProblema).Split(',');
            foreach (string email in lista)
            {
                if (txtAT.Text.ToLower().Contains(email))
                    base.ExibirMensagemAlerta(string.Empty, String.Format("<b>Atenção!</b><br>O <b>{0}</b> não entrega vagas de emprego e convocações de entrevista para seus usuários. <br>Cadastre outro e-mail para garantir que receberá nossos comunicados.", email));
            }
            ScriptManager.RegisterStartupScript(upEmail, upEmail.GetType(), "blockAt", "blockAt();", true);
        }
        #endregion

        #region txtEmail_OnTextChanged
        protected void txtEmail_OnTextChanged(object sender, EventArgs e)
        {
            rfvAt.Enabled = !string.IsNullOrWhiteSpace(txtEmail.Text);
            rfvEmail.Enabled = !string.IsNullOrWhiteSpace(txtEmail.Text);
            rfvDdlAt.Enabled = !string.IsNullOrWhiteSpace(txtEmail.Text);
            VerificaHotmail();
        }
        #endregion

        #endregion

        #region AjaxMethods

        #region ValidarCidade
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Cidade objCidade;
            return Cidade.CarregarPorNome(valor, out objCidade);
        }
        #endregion

        #region PesquisarMediaSalarial
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string PesquisarMediaSalarial(string desFuncao, string cidadeEstado)
        {
            //TODO: Comentado até ser inserida a matriz nova de função
            /*
            desFuncao = desFuncao.Trim();
            cidadeEstado = cidadeEstado.Trim();
            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(desFuncao, out objFuncao))
            {
                Cidade objCidade;
                if (Cidade.CarregarPorNome(cidadeEstado, out objCidade))
                {
                    var pesquisa = BLL.Custom.SalarioBr.PesquisaSalarial.EfetuarPesquisa(objFuncao, objCidade.Estado);

                    if (pesquisa != null && (pesquisa.ValorTrainee != Decimal.Zero && pesquisa.ValorMaster != Decimal.Zero))
                        return String.Format("{0};{1}", pesquisa.ValorTrainee.ToString("N2"), pesquisa.ValorMaster.ToString("N2"));
                }
            }
            */
            return String.Empty;
        }
        #endregion

        #region ValidarFuncao
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Funcao objFuncao;
            return Funcao.CarregarPorDescricao(valor, out objFuncao);
        }
        #endregion

        #endregion


    }
}