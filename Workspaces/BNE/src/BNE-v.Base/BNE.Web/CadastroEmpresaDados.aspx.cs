using BNE.Auth.Helper;
using BNE.Auth.HttpModules;
using BNE.BLL;
using BNE.BLL.AsyncServices;
using BNE.BLL.Custom;
using BNE.BLL.Security;
using BNE.Bridge;
using BNE.Componentes;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using BNE.Web.Resources;
using Employer.Plataforma.Web.Componentes;
using Microsoft.IdentityModel.Claims;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Enumeradores = BNE.BLL.Enumeradores;
using Parametro = BNE.BLL.Parametro;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;

namespace BNE.Web
{
    public partial class CadastroEmpresaDados : BasePage
    {

        #region Propriedades

        #region CadastroEmpresaDadosIdFilial - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Filial.
        /// </summary>
        public int? CadastroEmpresaDadosIdFilial
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

        #region IdPessoaFisica - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa Fisica.
        /// </summary>
        public int? IdPessoaFisica
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                {
                    int value;
                    if (Int32.TryParse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString(), out value))
                    {
                        return value;
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel2.ToString());
            }
        }
        #endregion

        #region IdOrigem - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Filial Observacao
        /// </summary>
        public int? IdOrigem
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

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera as Permissoes
        /// </summary>
        protected List<int> Permissoes
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value);
            }
        }
        #endregion

        #region CadastroEmpresaDadosDadosReceita - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Filial Observacao
        /// </summary>
        public ControlCNPJReceitaFederal.DadosCNPJReceitaFederal CadastroEmpresaDadosDadosReceita
        {
            get
            {
                if (Session["DadosCNPJReceitaFederal"] != null)
                    return (ControlCNPJReceitaFederal.DadosCNPJReceitaFederal)(Session["DadosCNPJReceitaFederal"]);
                return null;
            }
            set
            {
                Session.Add("DadosCNPJReceitaFederal", value);
            }
        }
        #endregion

        #region LogoWSBytes - Variavel10
        private byte[] LogoWSBytes
        {
            get
            {
                return (byte[])(ViewState[Chave.Temporaria.Variavel10.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel10.ToString(), value);
            }
        }
        #endregion

        #region LogoWSURL - Variavel11
        private string LogoWSURL
        {
            get
            {
                return ViewState[Chave.Temporaria.Variavel11.ToString()].ToString();
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel11.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            //Ajustando a expressao de validacao.
            //txtEmail.ExpressaoValidacao = Configuracao.regexEmail;
            //txtSite.ExpressaoValidacao = Configuracao.regexSite;

            ucModalConfirmacao.eventFechar += ucModalConfirmacao_eventFechar;
            ucConfirmacaoCadastroEmpresa.Fechar += ucConfirmacaoCadastroEmpresa_Fechar;

            ucDadosRepetidosEmpresa.FilialSelecionada += ucDadosRepetidosEmpresa_FilialSelecionada;

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "CadastroEmpresaDados");

            Ajax.Utility.RegisterTypeForAjax(typeof(CadastroEmpresaDados));

            var master = (Principal)Page.Master;

            if (master != null)
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;
        }
        #endregion

        #region ucDadosRepetidosEmpresa_FilialSelecionada
        void ucDadosRepetidosEmpresa_FilialSelecionada(int idFilial)
        {
            LimparCampos();
            CadastroEmpresaDadosIdFilial = IdPessoaFisica = null;
            CadastroEmpresaDadosIdFilial = idFilial;

            UsuarioFilialPerfil objUsuarioFilialPerfil;
            if (UsuarioFilialPerfil.CarregarPorPerfilFilial((int)Enumeradores.Perfil.AcessoEmpresaMaster, CadastroEmpresaDadosIdFilial.Value, out objUsuarioFilialPerfil))
                IdPessoaFisica = objUsuarioFilialPerfil.PessoaFisica.IdPessoaFisica;

            PreencherCampos();
            upDadosBasicos.Update();
            upUsuarioMasterEmpresa.Update();
            upDadosGeraisEmpresa.Update();
            upCartaoCNPJ.Update();
        }
        #endregion

        #region ucModalConfirmacao_eventFechar
        void ucModalConfirmacao_eventFechar()
        {
            FecharModalConfirmacao();
        }
        #endregion

        #region ucConfirmacaoCadastroEmpresa_Fechar
        void ucConfirmacaoCadastroEmpresa_Fechar()
        {
            FecharModalConfirmacao();
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate(btnSalvar.ValidationGroup);
                if (Page.IsValid)
                {
                    Salvar();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnDadosRepetidos_Click
        protected void btnDadosRepetidos_Click(object sender, EventArgs e)
        {
            ucDadosRepetidosEmpresa.IdFilial = this.CadastroEmpresaDadosIdFilial.Value;
            ucDadosRepetidosEmpresa.Inicializar();
            ucDadosRepetidosEmpresa.MostrarModal();
        }
        #endregion

        #region btlExisteLogoWS_Click
        protected void btlExisteLogoWS_Click(object sender, EventArgs e)
        {
            ucFoto.ImageData = LogoWSBytes;
            ucFoto.ImageUrl = LogoWSURL;
            LogoWSBytes = null;
            LogoWSURL = string.Empty;
            btlExisteLogoWS.Visible = false;

            upFoto.Update();
        }
        #endregion

        #region ucFoto_Error
        protected void ucFoto_Error(Exception ex)
        {
            ExibirMensagemErro(ex);
        }
        #endregion

        #region master_LoginEfetuadoSucesso
        protected void master_LoginEfetuadoSucesso()
        {
            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue && IdPessoaFisicaLogada.HasValue)
            {
                if (UsuarioFilialPerfil.PessoaFisicaPossuiPerfilNaFilial(new Filial(CadastroEmpresaDadosIdFilial.Value), new PessoaFisica(base.IdPessoaFisicaLogada.Value), new Perfil((int)Enumeradores.Perfil.AcessoEmpresaMaster)))
                {
                    IdPessoaFisica = base.IdPessoaFisicaLogada.Value;

                    HabilitarCamposDadosEmpresa(true);
                    HabilitarCamposUsuarioMaster(true);

                    PreencherCampos();
                    upUsuarioMasterEmpresa.Update();
                    upDadosGeraisEmpresa.Update();
                    upCartaoCNPJ.Update();

                }
                else
                {
                    ExibirMensagem(MensagemAviso._202403, TipoMensagem.Aviso);
                    ExibirLogin();
                }
            }
            else
            {
                ExibirMensagem(MensagemAviso._202403, TipoMensagem.Aviso);
                ExibirLogin();
            }
        }
        #endregion

        #region txtCNPJ_CnpjEncontrado
        protected void txtCNPJ_CnpjEncontrado(object sender, ControlCNPJReceitaFederal.DadosCNPJReceitaFederal dadoscnpj)
        {
            ValorCNPJAlterado(dadoscnpj);
        }
        #endregion

        #region txtCNPJ_ProblemaComunicacao
        protected void txtCNPJ_ProblemaComunicacao(object sender)
        {
            ValorCNPJAlterado(null);
        }
        #endregion

        #endregion

        #region Métodos

        #region ValorCNPJAlterado
        private void ValorCNPJAlterado(ControlCNPJReceitaFederal.DadosCNPJReceitaFederal dadoscnpj)
        {
            CadastroEmpresaDadosDadosReceita = dadoscnpj;

            int? idFilial = Filial.RecuperarIdentificador(txtCNPJ.numeroCNPJ.Value);
            if (idFilial.HasValue)
            {
                CadastroEmpresaDadosIdFilial = idFilial;

                //Verifica se existe usuario ligado a empresa.
                if (UsuarioFilialPerfil.ExisteUsuarioLigadoFilialcomPerfil(CadastroEmpresaDadosIdFilial.Value, (int)BNE.BLL.Enumeradores.Perfil.AcessoEmpresaMaster))
                {
                    if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue && IdPessoaFisicaLogada.HasValue)
                    {
                        if (UsuarioFilialPerfil.PessoaFisicaPossuiPerfilNaFilial(new Filial(CadastroEmpresaDadosIdFilial.Value), new PessoaFisica(base.IdPessoaFisicaLogada.Value), new Perfil((int)Enumeradores.Perfil.AcessoEmpresaMaster)))
                        {
                            IdPessoaFisica = base.IdPessoaFisicaLogada.Value;

                            HabilitarCamposDadosEmpresa(true);
                            HabilitarCamposUsuarioMaster(true);

                            PreencherCampos();

                            upUsuarioMasterEmpresa.Update();
                            upDadosGeraisEmpresa.Update();
                            upCartaoCNPJ.Update();
                        }
                        else
                        {
                            ExibirMensagem(MensagemAviso._202403, TipoMensagem.Aviso);
                            ExibirLogin();
                        }
                    }
                    else
                    {
                        ExibirMensagem(MensagemAviso._202403, TipoMensagem.Aviso);
                        ExibirLogin();
                    }
                }
                else
                {
                    if (base.IdPessoaFisicaLogada.HasValue)
                        IdPessoaFisica = base.IdPessoaFisicaLogada.Value;

                    HabilitarCamposDadosEmpresa(true);
                    HabilitarCamposUsuarioMaster(true);

                    PreencherCampos();

                    upUsuarioMasterEmpresa.Update();
                    upDadosGeraisEmpresa.Update();
                    upCartaoCNPJ.Update();
                }
            }
            else
            {
                if (CadastroEmpresaDadosDadosReceita != null)
                {
                    if (CadastroEmpresaDadosDadosReceita.Ativa)
                        PreencherDadosReceitaFederal(CadastroEmpresaDadosDadosReceita);
                    else
                    {
                        MostrarMensagemEmpresaInativaReceitaFederal();
                        return;
                    }
                }

                HabilitarCamposDadosEmpresa(true);
                HabilitarCamposUsuarioMaster(true);

                upUsuarioMasterEmpresa.Update();
                upDadosGeraisEmpresa.Update();
                upCartaoCNPJ.Update();
            }
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            if (!ckbAutorizacao.Checked)
            {
                ExibirMensagem("Para salvar seu cadastro é necessário que autorize a publicação de suas vagas no BNE.", TipoMensagem.Erro);
                return;
            }

            //Dados Gerais da Empresa
            string cnpjMatriz = CNPJ.RetornarMatriz(txtCNPJ.numeroCNPJ.ToString());

            PessoaFisica objPessoaFisica;
            Filial objFilial;
            UsuarioFilialPerfil objUsuarioFilialPerfil;
            UsuarioFilial objUsuarioFilial;
            FilialLogo objFilialLogo;

            //Endereco objEndereco;
            bool autorizacaoPublicacaoVagas = ckbAutorizacao.Checked;
            bool contrataEstag = ckbEstagiarios.Checked;
            /*Edição do tipo da origem se a filial tiver origem*/
            Origem objOrigem = null;

            #region Objetos para rastrear alterações de dados cadastrais
            Filial objFilialAntiga = null;
            Endereco objEnderecoAntigo = null;
            PessoaFisica objPessoaFisicaAntigo = null;
            UsuarioFilial objUsuarioFilialAntigo = null;
            Sexo objSexoPessoaFisicaAntigo = null;
            NaturezaJuridica objNaturezaJuridicaAntigo = null;
            CNAESubClasse objCNAESubClasseAntigo = null;
            SituacaoFilial objSituacaoFilialAntigo = null;

            //Para rastrear alteração de dados relacionados aos dados da receita
            if (CadastroEmpresaDadosDadosReceita != null)
            {
                if (!CadastroEmpresaDadosDadosReceita.Ativa)
                {
                    MostrarMensagemEmpresaInativaReceitaFederal();
                    return;
                }

                objFilialAntiga = new Filial
                {
                    RazaoSocial = CadastroEmpresaDadosDadosReceita.RazaoSocial.Trim(),
                    NomeFantasia = CadastroEmpresaDadosDadosReceita.NomeFantasia.Trim(),
                    FlagMatriz = string.IsNullOrEmpty(cnpjMatriz),
                    NumeroCNPJ = txtCNPJ.numeroCNPJ,
                    DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                };

                CNAESubClasse.CarregarPorCodigo(CadastroEmpresaDadosDadosReceita.CNAEPrincipal.Trim(), out objCNAESubClasseAntigo);
                NaturezaJuridica.CarregarPorCodigo(CadastroEmpresaDadosDadosReceita.NaturezaJuridica.Trim(), out objNaturezaJuridicaAntigo);

                objEnderecoAntigo = new Endereco
                {
                    NumeroCEP = CadastroEmpresaDadosDadosReceita.CEP.Trim(),
                    DescricaoLogradouro = CadastroEmpresaDadosDadosReceita.Logradouro.Trim(),
                    NumeroEndereco = CadastroEmpresaDadosDadosReceita.Numero.Trim(),
                    DescricaoComplemento = CadastroEmpresaDadosDadosReceita.Complemento.Trim(),
                    DescricaoBairro = CadastroEmpresaDadosDadosReceita.Bairro.Trim()
                };

                //Salvando a cidade
                Cidade objCidadeEnderecoAntigo;
                if (Cidade.CarregarPorNome(UIHelper.FormatarCidade(CadastroEmpresaDadosDadosReceita.Municipio, CadastroEmpresaDadosDadosReceita.UF), out objCidadeEnderecoAntigo))
                    objEnderecoAntigo.Cidade = objCidadeEnderecoAntigo;
            }

            #endregion


            bool novaFilial = false;
            if (CadastroEmpresaDadosIdFilial.HasValue)
            {
                objFilial = Filial.LoadObject(CadastroEmpresaDadosIdFilial.Value);

                if (objFilial.CNAEPrincipal != null)
                {
                    objFilial.CNAEPrincipal.CompleteObject();
                    objCNAESubClasseAntigo = (CNAESubClasse)objFilial.CNAEPrincipal.Clone();
                }

                if (objFilial.NaturezaJuridica != null)
                {
                    objFilial.NaturezaJuridica.CompleteObject();
                    objNaturezaJuridicaAntigo = (NaturezaJuridica)objFilial.NaturezaJuridica.Clone();
                }

                //Armazenando os dados da filial antiga
                objFilialAntiga = (Filial)objFilial.Clone();
                objFilialAntiga.SituacaoFilial.CompleteObject();
                objSituacaoFilialAntigo = (SituacaoFilial)objFilial.SituacaoFilial.Clone();

                if (objFilial.Endereco != null)
                {
                    objFilial.Endereco.CompleteObject();
                    //Armazenando os dados da filial bne antiga
                    objEnderecoAntigo = (Endereco)objFilial.Endereco.Clone();
                }
                else
                    objFilial.Endereco = new Endereco();

                if (this.IdPessoaFisica.HasValue)
                {
                    if (!UsuarioFilialPerfil.CarregarPorPerfilFilialPessoaFisica((int)Enumeradores.Perfil.AcessoEmpresaMaster, objFilial.IdFilial, this.IdPessoaFisica.Value, out objUsuarioFilialPerfil))
                    {
                        objUsuarioFilialPerfil = new UsuarioFilialPerfil
                            {
                                DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                            };
                        objUsuarioFilial = new UsuarioFilial();
                    }
                    else if (!UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                        objUsuarioFilial = new UsuarioFilial();
                    else
                        objUsuarioFilialAntigo = (UsuarioFilial)objUsuarioFilial.Clone();
                }
                else
                {
                    objUsuarioFilialPerfil = new UsuarioFilialPerfil
                        {
                            DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                        };
                    objUsuarioFilial = new UsuarioFilial();
                }

                if (!FilialLogo.CarregarLogo(objFilial.IdFilial, out objFilialLogo))
                    objFilialLogo = new FilialLogo();
            }
            else
            {
                novaFilial = true;

                objFilial = new Filial
                {
                    DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                    Endereco = new Endereco()
                };
                objUsuarioFilialPerfil = new UsuarioFilialPerfil
                    {
                        DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                    };
                objUsuarioFilial = new UsuarioFilial();
                objFilialLogo = new FilialLogo();
            }

            bool novaPessoa = false;
            if (this.IdPessoaFisica.HasValue)
            {
                objPessoaFisica = PessoaFisica.LoadObject(this.IdPessoaFisica.Value);
                objPessoaFisica.Sexo.CompleteObject();

                objPessoaFisicaAntigo = (PessoaFisica)objPessoaFisica.Clone();
                objSexoPessoaFisicaAntigo = (Sexo)objPessoaFisica.Sexo.Clone();
            }
            else
            {
                novaPessoa = true;
                objPessoaFisica = new PessoaFisica();
            }

            objPessoaFisica.NumeroCPF = txtCPF.Valor;
            objPessoaFisica.DataNascimento = txtDataNascimento.ValorDatetime.Value;
            objPessoaFisica.NomePessoa = txtNome.Valor;
            objPessoaFisica.NomePessoaPesquisa = txtNome.Valor;
            objPessoaFisica.Sexo = Sexo.LoadObject(Convert.ToInt32(rblSexo.SelectedValue));
            objPessoaFisica.NumeroCelular = txtTelefoneCelularMaster.Fone;
            objPessoaFisica.FlagInativo = false;
            objPessoaFisica.NumeroDDDCelular = txtTelefoneCelularMaster.DDD;
            objFilial.DescricaoPaginaFacebook = txtFacebook.Valor;

            if (objPessoaFisica.Deficiencia == null)
                objPessoaFisica.Deficiencia = new Deficiencia(0); //Nenhuma

            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(txtFuncaoExercida.Text, out objFuncao))
            {
                objUsuarioFilial.DescricaoFuncao = txtFuncaoExercida.Text;
                objUsuarioFilial.Funcao = objFuncao;
            }
            else
            {
                objUsuarioFilial.Funcao = null;
                objUsuarioFilial.DescricaoFuncao = txtFuncaoExercida.Text;
            }

            objUsuarioFilial.NumeroDDDComercial = txtTelefoneMaster.DDD;
            objUsuarioFilial.NumeroComercial = txtTelefoneMaster.Fone;
            objUsuarioFilial.EmailComercial = txtEmail.Text;

            objUsuarioFilialPerfil.PessoaFisica = objPessoaFisica;
            objUsuarioFilialPerfil.SenhaUsuarioFilialPerfil = objPessoaFisica.DataNascimento.ToString("ddMMyyyy");
            objUsuarioFilialPerfil.Perfil = new BLL.Perfil((int)BLL.Enumeradores.Perfil.AcessoEmpresaMaster);

            objFilial.FlagMatriz = string.IsNullOrEmpty(cnpjMatriz);
            //Para ser salvo junto com o apelido
            objFilial.RazaoSocial = txtRazaoSocial.Valor;
            objFilial.NomeFantasia = txtNomeFantasia.Valor;
            objFilial.EnderecoSite = txtSite.Text;
            objFilial.NumeroCNPJ = Convert.ToDecimal(txtCNPJ.numeroCNPJ);
            objFilial.NumeroComercial = txtTelefoneComercialEmpresa.Fone;
            objFilial.NumeroDDDComercial = txtTelefoneComercialEmpresa.DDD;

            //Limpando a localização, caso o CEP previamente informado seja diferente do 
            if (!string.IsNullOrEmpty(objFilial.Endereco.NumeroCEP) && !objFilial.Endereco.NumeroCEP.Equals(ucEndereco.CEP))
                objFilial.DescricaoLocalizacao = null;

            //Endereco
            objFilial.Endereco.NumeroCEP = ucEndereco.CEP;
            objFilial.Endereco.DescricaoLogradouro = ucEndereco.Logradouro;
            objFilial.Endereco.NumeroEndereco = ucEndereco.Numero;
            objFilial.Endereco.DescricaoComplemento = ucEndereco.Complemento;
            objFilial.Endereco.DescricaoBairro = ucEndereco.Bairro;

            //Salvando Dados Financeiro ISS
            int idfFilial;

            if (IdFilial.HasValue)
                idfFilial = IdFilial.Value;
            else
                idfFilial = objFilial.IdFilial;

            bool flgISS;
            string textoPersonalizado;
            int idCidade;

            if (Filial.RecuperarInfoISSFlgIss(objFilial.IdFilial, out flgISS, out textoPersonalizado, out idCidade))
            {
                if (flgISS != cbISS.Checked)
                {
                    var flgISSAux = cbISS.Checked ? "1" : "0";

                    ParametroFilial.AtualizarParametroPorFilial(flgISSAux.ToString(), null, idfFilial);
                }
            }
            else
            {
            if (cbISS.Checked)
            {
                var objParametroFilial = new ParametroFilial
                {
                    IdFilial = idfFilial,
                    ValorParametro = "1",
                    FlagInativo = false,
                    IdParametro = 347
                };

                objParametroFilial.Save();
                }
            }

            if (Filial.RecuperarInfoISSTextoPersonalizado(objFilial.IdFilial, out flgISS, out textoPersonalizado, out idCidade))
            {
                if (textoPersonalizado != txtTextoNF.Text)
                    ParametroFilial.AtualizarParametroPorFilial(null, txtTextoNF.Text, idfFilial);
            }
            else
            {
            if (!string.IsNullOrEmpty(txtTextoNF.Text))
            {
                var objParametroFilialAux =(ParametroFilial)null;

                if(ParametroFilial.IsParamentro(348, idfFilial))
                {
                    objParametroFilialAux = ParametroFilial.LoadObject(348, idfFilial);
                    objParametroFilialAux.FlagInativo = false;
                    objParametroFilialAux.ValorParametro = txtTextoNF.Text;
                }else
                {
                    objParametroFilialAux = new ParametroFilial
                    {
                        IdFilial = idfFilial,
                        ValorParametro = txtTextoNF.Text,
                        FlagInativo = false,
                        IdParametro = 348
                    };
                }
                objParametroFilialAux.Save();
            }
            }

            //Salvando a cidade
            Cidade objCidade;
            if (Cidade.CarregarPorNome(ucEndereco.Cidade, out objCidade))
                objFilial.Endereco.Cidade = objCidade;

            CNAESubClasse objCNAESubClasse;
            CNAESubClasse.CarregarPorCodigo(Convert.ToInt32(txtCNAE.Valor).ToString(CultureInfo.CurrentCulture), out objCNAESubClasse);
            objFilial.CNAEPrincipal = objCNAESubClasse;

            NaturezaJuridica objNaturezaJuridica;
            NaturezaJuridica.CarregarPorCodigo(txtNaturezaJuridica.Valor, out objNaturezaJuridica);
            objFilial.NaturezaJuridica = objNaturezaJuridica;

            //Perfil Administrador
            bool empresaAuditada = false;

            // parâmetro null pelo motivo de não ser necessário alterar se o usuário não for administrador           
            bool? usaWebEstagios;
            if (pnlPerfilAdministrador.Visible)
            {
                int quantidadeUsuariosAdicionais = Convert.ToInt32(txtQuantidadeUsuario.Valor) - Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeLimiteUsuarios));
                if (quantidadeUsuariosAdicionais > 0)
                    objFilial.QuantidadeUsuarioAdicional = quantidadeUsuariosAdicionais;
                else
                    objFilial.QuantidadeUsuarioAdicional = null;

                objFilial.SituacaoFilial = SituacaoFilial.LoadObject(Convert.ToInt32(rcbEditarEmpresa.SelectedValue));

                if (objFilial.SituacaoFilial.IdSituacaoFilial.Equals((int)Enumeradores.SituacaoFilial.PublicadoEmpresa))
                    empresaAuditada = true;

                if (IdOrigem.HasValue)
                {
                    objOrigem = Origem.LoadObject(IdOrigem.Value);
                    objOrigem.TipoOrigem = new TipoOrigem(Convert.ToInt32(rcbBancoDados.SelectedValue));
                    objFilial.TipoParceiro = new TipoParceiro(Convert.ToInt32(rcbTipoParceiro.SelectedValue));
                }
                else
                {
                    objFilial.TipoParceiro = null;
                }

                usaWebEstagios = ckbClienteWebEstagios.Checked;


            }
            else
            {
                usaWebEstagios = null;

                if (objFilial.SituacaoFilial == null)
                    objFilial.SituacaoFilial = SituacaoFilial.LoadObject((int)Enumeradores.SituacaoFilial.AguardandoPublicacao);
            }

            objFilial.QuantidadeFuncionarios = Convert.ToInt32(txtNumeroFuncionarios.Valor);
            objFilial.FlagOfereceCursos = ckbCursos.Checked;

            #region LogoreAgradecimentoCandidatura
            if (ucFoto.ImageData != null && !ucFoto.ImageData.Length.Equals(0))
            {
                objFilialLogo.ImagemLogo = ucFoto.ImageData;
                objFilialLogo.FlagInativo = false;
            }
            else
            {
                objFilialLogo.ImagemLogo = null;
                objFilialLogo.FlagInativo = true;
            }
            #endregion Logo

            #region Gravação de histórico de alteração de dados cadastrais
            int? idUsuarioFilialPerfilLogado = null;
            IEnumerable<CompareObject.CompareResult> listaAlteracao = null;
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                idUsuarioFilialPerfilLogado = base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value;

            if (objFilialAntiga != null || objEnderecoAntigo != null || objPessoaFisicaAntigo != null || objUsuarioFilialAntigo != null)
            {
                #region Filial
                List<CompareObject.CompareResult> listaAlteracaoFilial = null;
                if (objFilialAntiga != null && objFilial != null)
                {
                    listaAlteracaoFilial = CompareObject.CompareList(objFilialAntiga, objFilial, new[] { "ApelidoFilial", "CNAEPrincipal", "NaturezaJuridica", "SituacaoFilial", "Endereco" });

                    List<CompareObject.CompareResult> listaAlteracaoCNAE = null;
                    if (objCNAESubClasseAntigo != null && objCNAESubClasse != null)
                        listaAlteracaoCNAE = CompareObject.CompareList(objCNAESubClasseAntigo, objCNAESubClasse, new[] { "IdCNAESubClasse", "DescricaoCNAESubClasse", "IdCNAEClasse" });

                    List<CompareObject.CompareResult> listaAlteracaoNaturezaJuridica = null;
                    if (objNaturezaJuridicaAntigo != null && objNaturezaJuridica != null)
                        listaAlteracaoNaturezaJuridica = CompareObject.CompareList(objNaturezaJuridicaAntigo, objNaturezaJuridica, new[] { "IdNaturezaJuridica", "DescricaoNaturezaJuridica", "FlagInativo", "DataCadastro" });

                    List<CompareObject.CompareResult> listaAlteracaoSituacaoFilial = null;
                    if (objSituacaoFilialAntigo != null && objFilial.SituacaoFilial != null)
                        listaAlteracaoSituacaoFilial = CompareObject.CompareList(objSituacaoFilialAntigo, objFilial.SituacaoFilial, new[] { "IdSituacaoFilial", "FlagInativo", "DataCadastro" });

                    if (listaAlteracaoCNAE != null)
                        listaAlteracaoFilial = listaAlteracaoFilial.Concat(listaAlteracaoCNAE).ToList();

                    if (listaAlteracaoNaturezaJuridica != null)
                        listaAlteracaoFilial = listaAlteracaoFilial.Concat(listaAlteracaoNaturezaJuridica).ToList();

                    if (listaAlteracaoSituacaoFilial != null)
                        listaAlteracaoFilial = listaAlteracaoFilial.Concat(listaAlteracaoSituacaoFilial).ToList();

                }
                #endregion

                #region Endereco
                List<CompareObject.CompareResult> listaAlteracaoFilialEndereco = null;
                if (objEnderecoAntigo != null && objFilial.Endereco != null)
                {
                    //Completar com objCidade em objEnderecoAntigoCompleto para comparacao com objFilial.Endereco só retornar se for diferente
                    Endereco objEnderecoAntigoCompleto = objEnderecoAntigo;
                    objEnderecoAntigoCompleto.Cidade = Cidade.LoadObject(objEnderecoAntigoCompleto.Cidade.IdCidade);

                    listaAlteracaoFilialEndereco = CompareObject.CompareList(objEnderecoAntigoCompleto, objFilial.Endereco);
                }

                #endregion

                #region Pessoa Física
                List<CompareObject.CompareResult> listaAlteracaoPessoaFisica = null;
                if (objPessoaFisicaAntigo != null && objPessoaFisica != null)
                {
                    listaAlteracaoPessoaFisica = CompareObject.CompareList(objPessoaFisicaAntigo, objPessoaFisica, new[] { "NomeCompleto", "NomePessoaPesquisa", "Deficiencia" });

                    List<CompareObject.CompareResult> listaAlteracaoPessoaFisicaSexo = null;
                    if (objSexoPessoaFisicaAntigo != null && objPessoaFisica.Sexo != null)
                    {
                        listaAlteracaoPessoaFisicaSexo = CompareObject.CompareList(objSexoPessoaFisicaAntigo, objPessoaFisica.Sexo, new[] { "IdSexo", "SiglaSexo" });
                    }

                    if (listaAlteracaoPessoaFisicaSexo != null)
                        listaAlteracaoPessoaFisica = listaAlteracaoPessoaFisica.Concat(listaAlteracaoPessoaFisicaSexo).ToList();
                }
                #endregion

                #region Usuario Filial
                List<CompareObject.CompareResult> listaAlteracaoUsuarioFilial = null;
                if (objUsuarioFilialAntigo != null && objUsuarioFilial != null)
                    listaAlteracaoUsuarioFilial = CompareObject.CompareList(objUsuarioFilialAntigo, objUsuarioFilial, new[] { "BNE.BLL.Funcao" });
                #endregion

                listaAlteracao = listaAlteracaoFilial;

                if (listaAlteracaoFilialEndereco != null)
                    listaAlteracao = listaAlteracao != null ? listaAlteracao.Concat(listaAlteracaoFilialEndereco)
                        : listaAlteracaoFilialEndereco;

                if (listaAlteracaoPessoaFisica != null)
                    listaAlteracao = listaAlteracao != null ? listaAlteracao.Concat(listaAlteracaoPessoaFisica)
                        : listaAlteracaoPessoaFisica;

                if (listaAlteracaoUsuarioFilial != null)
                    listaAlteracao = listaAlteracao != null ? listaAlteracao.Concat(listaAlteracaoUsuarioFilial)
                        : listaAlteracaoUsuarioFilial;

                if (listaAlteracao == null)
                    listaAlteracao = new CompareObject.CompareResult[] { }; // inicializa sem nada dentro, só para nao dar excecao no método Filial.SalvarDadosEmpresa()
            }
            #endregion

            List<Vaga> listaVagasParaPublicacao;
            objFilial.SalvarDadosEmpresa(objUsuarioFilialPerfil, objUsuarioFilial, objPessoaFisica, objOrigem, listaAlteracao == null ? null : listaAlteracao.ToList(), idUsuarioFilialPerfilLogado, empresaAuditada, autorizacaoPublicacaoVagas, contrataEstag, usaWebEstagios, objFilialLogo, reCartaApresentacao.Text, reAgradecimentoCandidatura.Text, out listaVagasParaPublicacao);

            if (!pnlPerfilAdministrador.Visible)
            {
                if ((User.Identity == null || !User.Identity.IsAuthenticated)
                || (novaPessoa || novaFilial)
                || ((User.Identity as ClaimsIdentity) ?? new ClaimsIdentity()).GetPessoaFisicaId().GetValueOrDefault() != objPessoaFisica.IdPessoaFisica)
                {
                    LogarUsuarioFormsAuth(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica,
                        objPessoaFisica.CPF, objFilial);
                }
            }

            //Publica todas as vagas
            Task.Factory.StartNew(() => PublicarVagas(listaVagasParaPublicacao));

            if (CadastroEmpresaDadosIdFilial.HasValue)
            {
                ucModalConfirmacao.PreencherCampos(MensagemAviso._24020, MensagemAviso._24021, false);
                ucModalConfirmacao.MostrarModal();
            }
            else
                ucConfirmacaoCadastroEmpresa.MostrarModal();

            CadastroEmpresaDadosIdFilial = objFilial.IdFilial;
            IdPessoaFisica = objPessoaFisica.IdPessoaFisica;

            return;
        }
        #endregion

        #region PublicarVagas
        /// <summary>
        /// Publica as vagas da filial
        /// </summary>
        /// <param name="listaVagasParaPublicacao"></param>
        /// <returns></returns>
        private void PublicarVagas(object listaVagasParaPublicacao)
        {
            foreach (var objVaga in (List<Vaga>)listaVagasParaPublicacao)
            {
                var parametros = new ParametroExecucaoCollection
                    {
                        {"idVaga", "Vaga", objVaga.IdVaga.ToString(CultureInfo.InvariantCulture), objVaga.CodigoVaga},
                        {"EnfileraRastreador", "Deve enfileirar rastreador", "true", "Verdadeiro"}
                    };

                ProcessoAssincrono.IniciarAtividade(
                    TipoAtividade.PublicacaoVaga,
                    PluginsCompatibilidade.CarregarPorMetadata("PublicacaoVaga", "PublicacaoVagaRastreador"),
                    parametros,
                    null,
                    null,
                    null,
                    null,
                    DateTime.Now);
            }
        }
        #endregion

        #region Inicializar
        private void Inicializar()
        {
            AjustarPermissoes();
            LimparCampos();

            string url = String.Format(Configuracao.EnderecoReceitaCartaoCNPJ, txtCNPJ.numeroCNPJ);
            hlReceita.NavigateUrl = url;
            btnVisualizarCNPJ.OnClientClick = string.Format("AbrirPopup('{0}', 600, 800);", url);

            UIHelper.CarregarRadioButtonList(rblSexo, Sexo.Listar());

            if (Permissoes != null && Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.VisualizarInformacoesCadastroEmpresa))
            {
                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.AlterarSituacaoFilial))
                {
                    rcbEditarEmpresa.Enabled = false;
                    rcbEditarEmpresa.ToolTip = @"Apenas usuários do financeiro podem utilizar esta funcionalidade";
                }

                UIHelper.CarregarRadComboBox(rcbEditarEmpresa, SituacaoFilial.Listar(), "Idf_Situacao_Filial", "Des_Situacao_Filial");

                //selecionar no combo por padrão "aguardando publicação"
                rcbEditarEmpresa.SelectedValue = "3";

                Origem objOrigem;
                if (CadastroEmpresaDadosIdFilial.HasValue && Origem.OrigemPorFilial(CadastroEmpresaDadosIdFilial.Value, out objOrigem))
                {
                    IdOrigem = objOrigem.IdOrigem;
                    UIHelper.CarregarRadComboBox(rcbBancoDados, TipoOrigem.Listar(), "Idf_Tipo_Origem", "Des_Tipo_Origem");
                    UIHelper.CarregarRadComboBox(rcbTipoParceiro, TipoParceiro.Listar(), "Idf_Tipo_Parceiro", "Des_Tipo_Parceiro");
                }
                else
                {
                    IdOrigem = null;
                    lblBancoDados.Visible = false;
                    rcbBancoDados.Visible = false;
                    lblTipoParceiro.Visible = false;
                    rcbTipoParceiro.Visible = false;
                }

                btnDadosRepetidos.Visible = true;
                btnVisualizarCNPJ.Visible = true;
            }
            else
            {
                pnlPerfilAdministrador.Visible = false;
                btnDadosRepetidos.Visible = false;
                btnVisualizarCNPJ.Visible = false;
            }

            CarregarParametros();

            ucEndereco.Inicializar(btnSalvar.ValidationGroup);

            //Aplicação de Regra: 
            //- Intervalo da Data de Nascimento. ( 14 anos >= idade <= 80 anos )
            int idadeMinima = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdadeMinima));
            int idadeMaxima = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdadeMaxima));
            DateTime dataAtual = DateTime.Now;
            try
            {
                DateTime dataMinima = Convert.ToDateTime(dataAtual.Day.ToString() + "/" + dataAtual.Month.ToString() + "/" + (dataAtual.Year - idadeMaxima).ToString());
                DateTime dataMaxima = Convert.ToDateTime(dataAtual.Day.ToString() + "/" + dataAtual.Month.ToString() + "/" + (dataAtual.Year - idadeMinima).ToString());
                txtDataNascimento.DataMinima = dataMinima;
                txtDataNascimento.DataMaxima = dataMaxima;
            }
            catch
            {
                txtDataNascimento.DataMinima = DateTime.MinValue;
                txtDataNascimento.DataMaxima = DateTime.MaxValue;
            }
            txtDataNascimento.MensagemErroIntervalo = String.Format(MensagemAviso._100005, idadeMinima, idadeMaxima);

            if (base.IdFilial.HasValue) //Se o usuário já está logado
            {
                CadastroEmpresaDadosIdFilial = base.IdFilial.Value;
                if (UsuarioFilialPerfil.PessoaFisicaPossuiPerfilNaFilial(new Filial(CadastroEmpresaDadosIdFilial.Value), new PessoaFisica(base.IdPessoaFisicaLogada.Value), new Perfil((int)Enumeradores.Perfil.AcessoEmpresaMaster)))
                {
                    IdPessoaFisica = base.IdPessoaFisicaLogada.Value;

                    HabilitarCamposDadosEmpresa(true);
                    HabilitarCamposUsuarioMaster(true);

                    PreencherCampos();
                }
                else
                {
                    ExibirMensagem(MensagemAviso._202403, TipoMensagem.Aviso);
                    ExibirLogin();
                }
            }
            else
            {
                LimparCampos();
                PreencherCampos();

                HabilitarCamposDadosEmpresa(CadastroEmpresaDadosIdFilial.HasValue);
                HabilitarCamposUsuarioMaster(IdPessoaFisica.HasValue);
            }

            txtCNPJ.Focus();
            UIHelper.ValidateFocus(btnSalvar);
        }
        #endregion

        #region CarregarParametros
        private void CarregarParametros()
        {
            try
            {
                var parametros = new List<Enumeradores.Parametro>
                                     {
                                         Enumeradores.Parametro.IntervaloTempoAutoComplete,
                                         Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao,
                                         Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao,
                                         Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteCidade,
                                         Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade
                                     };

                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                aceFuncaoExercida.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            txtCNPJ.ExibirValidadorReceitaFederal = true;
            txtCNPJ.ValidarReceitaFederal = false;
            txtCNPJ.Limpar();
            txtCPF.Valor = String.Empty;
            txtDataNascimento.Valor = String.Empty;
            txtNome.Valor = String.Empty;
            rblSexo.ClearSelection();
            //aceFuncaoExercida.ContextKey = String.Empty;
            txtFuncaoExercida.Text = String.Empty;
            txtTelefoneCelularMaster.Fone = String.Empty;
            txtTelefoneCelularMaster.DDD = String.Empty;
            txtTelefoneMaster.DDD = String.Empty;
            txtTelefoneMaster.Fone = String.Empty;
            //txtEmail.Text = String.Empty;
            //txtSite.Text = String.Empty;
            txtNumeroFuncionarios.Valor = String.Empty;
            ckbCursos.Checked = false;
            ckbEstagiarios.Checked = false;
            ckbClienteWebEstagios.Checked = false;
            txtTelefoneComercialEmpresa.Fone = String.Empty;
            txtTelefoneComercialEmpresa.DDD = String.Empty;
            txtRazaoSocial.Valor = String.Empty;
            txtNomeFantasia.Valor = String.Empty;
            txtCNAE.Valor = String.Empty;
            lblInfoCNAE.Text = String.Empty;
            txtNaturezaJuridica.Valor = String.Empty;
            lblInfoNaturezaJuridica.Text = String.Empty;
            ucEndereco.LimparCampos();
            ckbCursos.Checked = false;
            txtFacebook.Valor = String.Empty;
            ckbAutorizacao.Checked = true;
            reAgradecimentoCandidatura.Content = string.Empty;
            reCartaApresentacao.Content = string.Empty;
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            Filial objFilial = null;
            if (CadastroEmpresaDadosIdFilial.HasValue)
                objFilial = Filial.LoadObject(CadastroEmpresaDadosIdFilial.Value);

            PessoaFisica objPessoaFisica = null;
            if (IdPessoaFisica.HasValue)
                objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);

            if (objFilial != null)
            {
                string url = String.Format(Configuracao.EnderecoReceitaCartaoCNPJ, objFilial.NumeroCNPJ.ToString().PadLeft(14, '0'));
                hlReceita.NavigateUrl = url;
                btnVisualizarCNPJ.OnClientClick = string.Format("AbrirPopup('{0}', 600, 800);", url);

                if (objFilial.NumeroCNPJ.HasValue)
                    txtCNPJ.numeroCNPJ = objFilial.NumeroCNPJ;
                    txtCNPJ.ReadOnly = true;

                //Dados Gerais da Empresa
                txtSite.Text = objFilial.EnderecoSite;

                txtNumeroFuncionarios.Valor = objFilial.QuantidadeFuncionarios.ToString(CultureInfo.CurrentCulture);
                ckbCursos.Checked = objFilial.FlagOfereceCursos;
                txtFacebook.Valor = objFilial.DescricaoPaginaFacebook;

                ParametroFilial autorizoPublicarParamFilial;
                if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.AutorizoBNEPublicarVagas, objFilial, out autorizoPublicarParamFilial))
                    ckbAutorizacao.Checked = Convert.ToBoolean(autorizoPublicarParamFilial.ValorParametro);

                ParametroFilial contrataEstagParamFilial;
                if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.FilialContrataEstagiario, objFilial, out contrataEstagParamFilial))
                    ckbEstagiarios.Checked = Convert.ToBoolean(contrataEstagParamFilial.ValorParametro);

                if (pnlPerfilAdministrador.Visible)
                {
                    ParametroFilial clienteWebEstagio;
                    if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.FilialParceiraWebEstagios, objFilial, out clienteWebEstagio))
                        ckbClienteWebEstagios.Checked = Convert.ToBoolean(clienteWebEstagio.ValorParametro);

                    rcbEditarEmpresa.SelectedValue = objFilial.SituacaoFilial.IdSituacaoFilial.ToString(CultureInfo.CurrentCulture);
                    txtQuantidadeUsuario.Valor =
                        Convert.ToString(Convert.ToInt32(
                            Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeLimiteUsuarios)) +
                                         (objFilial.QuantidadeUsuarioAdicional.HasValue
                                              ? objFilial.QuantidadeUsuarioAdicional.Value
                                              : 0));

                    ParametroFilial objParametroFilialAgradecimento;
                    if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.CartaAgradecimentoCandidatura, objFilial, out objParametroFilialAgradecimento, null))
                        reAgradecimentoCandidatura.Content = objParametroFilialAgradecimento.ValorParametro;

                    ParametroFilial objParametroFilial;
                    if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.CartaApresentacao, objFilial, out objParametroFilial, null))
                        reCartaApresentacao.Content = objParametroFilial.ValorParametro;

                }

                txtTelefoneComercialEmpresa.Fone = objFilial.NumeroComercial;
                txtTelefoneComercialEmpresa.DDD = objFilial.NumeroDDDComercial;

                // Dados do Financeiro
                bool flgISS;
                string textoPersonalizado;
                int idCidade;
                Filial.RecuperarInfoISS(objFilial.IdFilial, out flgISS, out textoPersonalizado, out idCidade);

                cbISS.Checked = flgISS;
                if (!string.IsNullOrEmpty(textoPersonalizado))
                    txtTextoNF.Text = textoPersonalizado;

                //Cartao do CNPJ
                txtRazaoSocial.Valor = objFilial.RazaoSocial;
                txtNomeFantasia.Valor = objFilial.NomeFantasia;

                if (objFilial.CNAEPrincipal != null)
                {
                    objFilial.CNAEPrincipal.CompleteObject();
                    txtCNAE.Valor = objFilial.CNAEPrincipal.CodigoCNAESubClasse;
                    lblInfoCNAE.Text = objFilial.CNAEPrincipal.DescricaoCNAESubClasse;
                }

                if (objFilial.NaturezaJuridica != null)
                {
                    objFilial.NaturezaJuridica.CompleteObject();
                    txtNaturezaJuridica.Valor = objFilial.NaturezaJuridica.CodigoNaturezaJuridica;
                    lblInfoNaturezaJuridica.Text = objFilial.NaturezaJuridica.DescricaoNaturezaJuridica;
                }

                if (objFilial.Endereco != null)
                {
                    objFilial.Endereco.CompleteObject();
                    objFilial.Endereco.Cidade.CompleteObject();

                    string numeroCEP = objFilial.Endereco.NumeroCEP;
                    string descricaoLogradouro = objFilial.Endereco.DescricaoLogradouro;
                    string numeroEndereco = objFilial.Endereco.NumeroEndereco;
                    string descricaoComplemento = objFilial.Endereco.DescricaoComplemento;
                    string nomeBairro = objFilial.Endereco.DescricaoBairro;
                    string nomeCidade = objFilial.Endereco.Cidade.NomeCidade;
                    string siglaEstado = objFilial.Endereco.Cidade.Estado.SiglaEstado;

                    ucEndereco.PreencherCampos(numeroCEP, descricaoLogradouro, numeroEndereco, descricaoComplemento, nomeBairro, nomeCidade, siglaEstado);
                }
                btnSalvar.Visible = true;

                CarregarLogo(objFilial);
            }

            if (objPessoaFisica != null)
            {
                txtCPF.Valor = objPessoaFisica.NumeroCPF;
                txtNome.Valor = objPessoaFisica.NomePessoa;
                txtDataNascimento.ValorDatetime = objPessoaFisica.DataNascimento;

                if (objPessoaFisica.Sexo != null)
                    rblSexo.SelectedValue = objPessoaFisica.Sexo.IdSexo.ToString();

                txtTelefoneCelularMaster.DDD = objPessoaFisica.NumeroDDDCelular;
                txtTelefoneCelularMaster.Fone = objPessoaFisica.NumeroCelular;

                if (objFilial != null)
                {
                    UsuarioFilialPerfil objUsuarioFilialPerfil;
                    if (UsuarioFilialPerfil.CarregarPorPerfilFilialPessoaFisica(Enumeradores.Perfil.AcessoEmpresaMaster.GetHashCode(), objFilial.IdFilial, IdPessoaFisica.Value, out objUsuarioFilialPerfil))
                    {
                        UsuarioFilial objUsuarioFilial;
                        if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                        {
                            if (objUsuarioFilial.Funcao != null)
                            {
                                objUsuarioFilial.Funcao.CompleteObject();
                                txtFuncaoExercida.Text = objUsuarioFilial.Funcao.DescricaoFuncao;
                            }
                            else
                                txtFuncaoExercida.Text = objUsuarioFilial.DescricaoFuncao;

                            txtTelefoneMaster.Fone = objUsuarioFilial.NumeroComercial;
                            txtTelefoneMaster.DDD = objUsuarioFilial.NumeroDDDComercial;
                            txtEmail.Text = objUsuarioFilial.EmailComercial;
                        }

                        objUsuarioFilialPerfil.PessoaFisica.CompleteObject();

                        txtCPF.Valor = objUsuarioFilialPerfil.PessoaFisica.NumeroCPF;
                        txtDataNascimento.ValorDatetime = objUsuarioFilialPerfil.PessoaFisica.DataNascimento;

                        txtTelefoneCelularMaster.Fone = objUsuarioFilialPerfil.PessoaFisica.NumeroCelular;
                        txtTelefoneCelularMaster.DDD = objUsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular;
                    }
                }

                btnSalvar.Enabled = true;
            }

            if (pnlPerfilAdministrador.Visible)
            {
                if (CadastroEmpresaDadosIdFilial.HasValue)
                {
                    ucObservacaoFilial.IdFilial = CadastroEmpresaDadosIdFilial.Value;
                    ucObservacaoFilial.Inicializar();
                }

                if (IdOrigem.HasValue)
                {
                    Origem objOrigem = Origem.LoadObject(IdOrigem.Value);
                    rcbBancoDados.SelectedValue = objOrigem.TipoOrigem.IdTipoOrigem.ToString();
                    if (objFilial.TipoParceiro != null)
                        rcbTipoParceiro.SelectedValue = objFilial.TipoParceiro.IdTipoParceiro.ToString();
                }
            }
        }
        #endregion

        #region CarregarLogo
        /// <summary>
        /// Rotina para carregar a logo da empresa
        /// </summary>
        private void CarregarLogo(Filial objFilial)
        {
            try
            {
                LogoWSBytes = null;
                LogoWSURL = null;
                btlExisteLogoWS.Visible = false;

                byte[] byteArray = FilialLogo.RecuperarArquivo((decimal)objFilial.NumeroCNPJ);

                if (byteArray != null)
                {
                    ucFoto.LimparFoto();
                    ucFoto.ImageData = byteArray;
                    ucFoto.ImageUrl = UIHelper.RetornarUrlLogo(objFilial.CNPJ, Handlers.PessoaJuridicaLogo.OrigemLogo.Local);
                }
                else
                {

                    #region Plataforma
                    try
                    {
                        using (var wsPJ = new WSPessoaJuridica.WSPessoaJuridica())
                        {
                            byteArray = wsPJ.CarregarPessoaJuridicaLogoPrincipalBinario((decimal)objFilial.NumeroCNPJ);
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                    #endregion Plataforma

                    if (byteArray != null)
                    {
                        LogoWSBytes = byteArray;
                        LogoWSURL = UIHelper.RetornarUrlLogo(objFilial.CNPJ, Handlers.PessoaJuridicaLogo.OrigemLogo.Plataforma);
                        btlExisteLogoWS.Visible = true;
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

        #region PreencherDadosReceitaFederal
        private void PreencherDadosReceitaFederal(ControlCNPJReceitaFederal.DadosCNPJReceitaFederal dados)
        {
            txtRazaoSocial.Valor = dados.RazaoSocial.Trim();
            txtNomeFantasia.Valor = dados.NomeFantasia.Trim();
            txtCNAE.Valor = dados.CNAEPrincipal.Trim();
            txtNaturezaJuridica.Valor = dados.NaturezaJuridica.Trim();

            string numeroCEP = dados.CEP.Trim();
            string descricaoLogradouro = dados.Logradouro.Trim();
            string numeroEndereco = dados.Numero.Trim();
            string descricaoComplemento = dados.Complemento.Trim();
            string nomeBairro = dados.Bairro.Trim();
            string nomeCidade = dados.Municipio.Trim();
            string siglaEstado = dados.UF.Trim();

            ucEndereco.PreencherCampos(numeroCEP, descricaoLogradouro, numeroEndereco, descricaoComplemento, nomeBairro, nomeCidade, siglaEstado);
        }
        #endregion

        #region HabilitarCamposUsuarioMaster
        private void HabilitarCamposUsuarioMaster(bool habilitar)
        {
            txtCPF.Enabled = habilitar;
            txtDataNascimento.Enabled = habilitar;
            lblNome.Enabled = txtNome.Enabled = habilitar;
            lblSexo.Enabled = rblSexo.Enabled = habilitar;
            lblFuncaoExercida.Enabled = txtFuncaoExercida.Enabled = habilitar;
            lblCelular.Enabled = txtTelefoneCelularMaster.Enabled = habilitar;
            lblTelefoneMaster.Enabled = txtTelefoneMaster.Enabled = habilitar;
            lblEmail.Enabled = txtEmail.Enabled = habilitar;
        }
        #endregion

        #region HabilitarCamposDadosEmpresa
        private void HabilitarCamposDadosEmpresa(bool habilitar)
        {
            txtCNPJ.ExibirValidadorReceitaFederal = !habilitar;
            hlReceita.Enabled = habilitar;
            //ucFoto.PermitirAlterarFoto = habilitar;
            lblSite.Enabled = txtSite.Enabled = habilitar;
            lblNumeroFuncionarios.Enabled = txtNumeroFuncionarios.Enabled = habilitar;
            lblTelefoneComercialEmpresa.Enabled = txtTelefoneComercialEmpresa.Enabled = habilitar;
            ckbCursos.Enabled = lblMinhaEmpresaCursos.Enabled = habilitar;
            ckbEstagiarios.Enabled = lblEstagioEsquerda.Enabled = lblEstagioDireita.Enabled = habilitar;
            ckbClienteWebEstagios.Enabled = habilitar;
            ckbAutorizacao.Enabled = habilitar;
            //Cartão do cnpj
            lblRazaoSocial.Enabled = txtRazaoSocial.Enabled = habilitar;
            lblNomeFantasia.Enabled = txtNomeFantasia.Enabled = habilitar;
            lblCNAE.Enabled = txtCNAE.Enabled = habilitar;
            lblNaturezaJuridica.Enabled = txtNaturezaJuridica.Enabled = habilitar;
            ucEndereco.HabilitarTodosCampos(habilitar);

            reCartaApresentacao.Enabled = habilitar;
            reAgradecimentoCandidatura.Enabled = habilitar;
        }
        #endregion

        #region txtDataNascimento_ValorAlterado
        protected void txtDataNascimento_ValorAlterado(object sender, EventArgs e)
        {
            ValidarEntradaCPFData();
            txtNome.Focus();
        }
        #endregion

        #region txtCPF_ValorAlterado
        protected void txtCPF_ValorAlterado(object sender, EventArgs e)
        {
            txtDataNascimento.Valor = String.Empty;
            ValidarEntradaCPFData();
            txtDataNascimento.Focus();
        }
        #endregion

        #region LogarUsuario
        private void LogarUsuario(int idFilial, int idPessoaFisica)
        {
            var pf = new PessoaFisica(idPessoaFisica);
            if (!pf.CompleteObject())
            {
                throw new ArgumentOutOfRangeException("idPessoaFisica");
            }


            base.IdFilial.Value = idFilial;
            base.IdPessoaFisicaLogada.Value = idPessoaFisica; 
            
            LogarUsuarioFormsAuth(pf.NomeCompleto, pf.IdPessoaFisica, pf.CPF, new Filial(idFilial));

            UsuarioFilialPerfil objUsuarioFilialPerfil;
            if (UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(idPessoaFisica, idFilial, out objUsuarioFilialPerfil))
                base.IdUsuarioFilialPerfilLogadoEmpresa.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
        }


        private void LogarUsuarioFormsAuth(string nomePessoa, int idPessoaFisica, decimal cpf, Filial filial)
        {
            var identity = BNE.Auth.BNEAutenticacao.LogarCPF(nomePessoa,
                idPessoaFisica, cpf);

            var context = HttpContext.Current ?? Context;
            var auth = new BNEAuthLoginControlEventArgs(identity, context);
            BNE.Auth.AuthEventAggregator.Instance.OnUserEnterSuccessfully(this, auth);

            var modelResult = new BNESessaoLoginModelResult(BNESessaoLoginResultType.OK,
                BNESessaoProfileType.EMPRESA);
            BNE.Bridge.BNELoginProcess.SalvarNovaSessaoBanco(auth, modelResult, filial);
//            BNE.Bridge.BNELoginProcess.RegistrarBLLProcess(auth, modelResult);
        }

        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, Enumeradores.CategoriaPermissao.Administrador);
            }
        }
        #endregion

        #region ValidarEntradaCPFData
        private void ValidarEntradaCPFData()
        {
            if (String.IsNullOrEmpty(txtCPF.Valor) || String.IsNullOrEmpty(txtDataNascimento.Valor) || String.IsNullOrEmpty(txtCNPJ.numeroCNPJ.ToString()))
                return;

            PessoaFisica objPessoaFisica;
            if (PessoaFisica.CarregarPorCPF(Convert.ToDecimal(txtCPF.Valor), out objPessoaFisica))
            {
                if (!txtDataNascimento.ValorDatetime.Equals(objPessoaFisica.DataNascimento))
                {
                    ExibirMensagem(MensagemAviso._100813, TipoMensagem.Aviso);
                    btnSalvar.Enabled = false;
                    return;
                }
                IdPessoaFisica = objPessoaFisica.IdPessoaFisica;

                PreencherCampos();
                btnSalvar.Enabled = true;
                upUsuarioMasterEmpresa.Update();
            }
        }
        #endregion

        #region FecharModalConfirmacao
        private void FecharModalConfirmacao()
        {
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                Redirect("SalaAdministradorEmpresas.aspx");
            else
            {
                LogarUsuario(CadastroEmpresaDadosIdFilial.Value, IdPessoaFisica.Value);
                Redirect("SalaSelecionador.aspx");
            }
        }
        #endregion

        #region MostrarMensagemEmpresaInativaReceitaFederal
        private void MostrarMensagemEmpresaInativaReceitaFederal()
        {
            base.ExibirMensagem("Esta empresa está inativa na Receita Federal.", TipoMensagem.Erro);
        }
        #endregion

        #endregion

        #region AjaxMehods

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

        #region RecuperarCNAE
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string RecuperarCNAE(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return null;

            valor = valor.Trim().TrimStart('0');

            CNAESubClasse objCNAESubClasse;
            if (CNAESubClasse.CarregarPorCodigo(valor, out objCNAESubClasse))
                return objCNAESubClasse.DescricaoCNAESubClasse;
            return null;
        }
        #endregion

        #region ValidarCNAE
        /// <summary>
        /// Validar CNAE
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool ValidarCNAE(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return true;

            valor = valor.Trim().TrimStart('0');

            CNAESubClasse objCNAESubClasse;
            if (CNAESubClasse.CarregarPorCodigo(valor, out objCNAESubClasse))
                return true;
            return false;
        }
        #endregion

        #region RecuperarNaturezaJuridica
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string RecuperarNaturezaJuridica(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return null;

            NaturezaJuridica objNaturezaJuridica;
            if (NaturezaJuridica.CarregarPorCodigo(valor, out objNaturezaJuridica))
                return objNaturezaJuridica.DescricaoNaturezaJuridica;
            return null;
        }
        #endregion

        #region ValidarNaturezaJuridica
        /// <summary>
        /// Validar Natureza Juridica
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool ValidarNaturezaJuridica(string valor)
        {
            valor = valor.Trim();
            if (string.IsNullOrEmpty(valor))
                return true;
            NaturezaJuridica objNaturezaJuridica;
            if (NaturezaJuridica.CarregarPorCodigo(valor, out objNaturezaJuridica))
                return true;
            return false;
        }
        #endregion

        #region ValidarVariosEnderecosPorCEP
        /// <summary>
        /// Validar se o cep informado contém mais de um endereço associado ao mesmo.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarVariosEnderecosPorCEP(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                wsCEP.cepws servico = null;
                var objCep = new wsCEP.CEP
                    {
                        Cep = valor.Replace("-", string.Empty).Trim()
                    };

                int qtdeCepEncontrados = 0;
                try
                {
                    servico = new wsCEP.cepws();

                    ServiceAuth.GerarHashAcessoWS(servico);

                    if (servico.RecuperarQuantidadeEnderecosPorCEP(objCep, ref qtdeCepEncontrados))
                    {
                        if (qtdeCepEncontrados > 1)
                            return true;
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
                finally
                {
                    if (servico != null)
                        servico.Dispose();
                }
            }
            return false;
        }
        #endregion

        #endregion

    }
}