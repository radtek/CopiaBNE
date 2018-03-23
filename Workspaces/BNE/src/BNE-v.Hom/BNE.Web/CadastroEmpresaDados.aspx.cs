using BNE.Auth.Helper;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Bridge;
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
using BNE.Auth.EventArgs;
using Enumeradores = BNE.BLL.Enumeradores;
using Parametro = BNE.BLL.Parametro;
using BNE.BLL.Assincronos;
using BNE.CEP;
using BNE.Componentes.EL;
using System.Threading;

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
            ucModalConfirmacao.eventVoltar += ucModalConfirmacao_eventVoltar;

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
                EL.GerenciadorException.GravarExcecao(ex, "Salvar empresa usando usuario interno.");
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
            LogoWSBytes = null;
            btlExisteLogoWS.Visible = false;

            upFoto.Update();
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

        #endregion

        #region Métodos

        #region Salvar
        private void Salvar()
        {
            try
            {
                if (!ckbAutorizacao.Checked)
                {
                    ExibirMensagem("Para salvar seu cadastro é necessário que autorize a publicação de suas vagas no BNE.", TipoMensagem.Erro);
                    return;
                }

                //Dados Gerais da Empresa
                string cnpjMatriz = CNPJ.RetornarMatriz(txtCNPJ.Valor);

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
                TipoParceiro objTipoParceiroAntigo = null;
                ParametroFilial objParametroFilialISSAntigo = null;
                ParametroFilial objParametroFilialTPAntigo = null;
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

                    if (objFilial.TipoParceiro != null)
                    {
                        objFilialAntiga.TipoParceiro.CompleteObject();
                        objTipoParceiroAntigo = (TipoParceiro)objFilial.TipoParceiro.Clone();
                    }

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
                                DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                                FlagUsuarioResponsavel = true
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
                            DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                            FlagUsuarioResponsavel = true
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
                        DescricaoIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                        FlagUsuarioResponsavel = true
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
                    objUsuarioFilial.Funcao = objFuncao;
                    objUsuarioFilial.DescricaoFuncao = null;
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
                objFilial.NumeroCNPJ = Convert.ToDecimal(txtCNPJ.Valor);
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

                //TODO: Corrigir esse código
                //Salvando Dados Financeiro ISS
                int idfFilial;

                if (IdFilial.HasValue)
                    idfFilial = IdFilial.Value;
                else
                    idfFilial = objFilial.IdFilial;

                bool flgISS;
                string textoPersonalizado;
                int idCidade;

                var objParametroFilialAuxIss = (ParametroFilial)null;

                if (Filial.RecuperarInfoISSFlgIss(objFilial.IdFilial, out flgISS, out textoPersonalizado, out idCidade))
                {


                    if (flgISS != cbISS.Checked)
                    {
                        var flgISSAux = cbISS.Checked ? "1" : "0";
                        ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.Flg_Iss, new Filial(idfFilial), out objParametroFilialAuxIss);
                        objParametroFilialISSAntigo = objParametroFilialAuxIss.Clone() as ParametroFilial;
                        objParametroFilialAuxIss.ValorParametro = flgISSAux;

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

                var objParametroFilialAuxTP = (ParametroFilial)null;

                if (Filial.RecuperarInfoISSTextoPersonalizado(objFilial.IdFilial, out flgISS, out textoPersonalizado, out idCidade))
                {
                    if (textoPersonalizado != txtTextoNF.Text)
                    {
                        ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.Descricao_Nf, new Filial(idfFilial), out objParametroFilialAuxTP);

                        objParametroFilialTPAntigo = objParametroFilialAuxTP.Clone() as ParametroFilial;
                        objParametroFilialAuxTP.ValorParametro = txtTextoNF.Text;

                        ParametroFilial.AtualizarParametroPorFilial(null, txtTextoNF.Text, idfFilial);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtTextoNF.Text))
                    {
                        var objParametroFilialAux = (ParametroFilial)null;

                        if (ParametroFilial.IsParamentro(348, idfFilial))
                        {
                            objParametroFilialAux = ParametroFilial.LoadObject(348, idfFilial);
                            objParametroFilialAux.FlagInativo = false;
                            objParametroFilialAux.ValorParametro = txtTextoNF.Text;
                        }
                        else
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
                CNAESubClasse.CarregarPorCodigo(txtCNAE.Valor, out objCNAESubClasse);
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
                        listaAlteracaoFilial = CompareObject.CompareList(objFilialAntiga, objFilial, new[] { "ApelidoFilial", "CNAEPrincipal", "NaturezaJuridica", "SituacaoFilial", "Endereco", "TipoParceiro" });

                        List<CompareObject.CompareResult> listaAlteracaoCNAE = null;
                        if (objCNAESubClasseAntigo != null && objCNAESubClasse != null)
                            listaAlteracaoCNAE = CompareObject.CompareList(objCNAESubClasseAntigo, objCNAESubClasse, new[] { "IdCNAESubClasse", "DescricaoCNAESubClasse", "IdCNAEClasse" });

                        List<CompareObject.CompareResult> listaAlteracaoNaturezaJuridica = null;
                        if (objNaturezaJuridicaAntigo != null && objNaturezaJuridica != null)
                            listaAlteracaoNaturezaJuridica = CompareObject.CompareList(objNaturezaJuridicaAntigo, objNaturezaJuridica, new[] { "IdNaturezaJuridica", "DescricaoNaturezaJuridica", "FlagInativo", "DataCadastro" });

                        List<CompareObject.CompareResult> listaAlteracaoSituacaoFilial = null;
                        if (objSituacaoFilialAntigo != null && objFilial.SituacaoFilial != null)
                            listaAlteracaoSituacaoFilial = CompareObject.CompareList(objSituacaoFilialAntigo, objFilial.SituacaoFilial, new[] { "IdSituacaoFilial", "FlagInativo", "DataCadastro" });

                        List<CompareObject.CompareResult> listaAlteracaoTipoParceiro = null;
                        if (objTipoParceiroAntigo != null && objFilial.TipoParceiro != null)
                            listaAlteracaoTipoParceiro = CompareObject.CompareList(objTipoParceiroAntigo, objFilial.TipoParceiro, new[] { "IdTipoParceiro", "FlagInativo", "DataCadastro" });

                        if (listaAlteracaoCNAE != null)
                            listaAlteracaoFilial = listaAlteracaoFilial.Concat(listaAlteracaoCNAE).ToList();

                        if (listaAlteracaoNaturezaJuridica != null)
                            listaAlteracaoFilial = listaAlteracaoFilial.Concat(listaAlteracaoNaturezaJuridica).ToList();

                        if (listaAlteracaoSituacaoFilial != null)
                            listaAlteracaoFilial = listaAlteracaoFilial.Concat(listaAlteracaoSituacaoFilial).ToList();

                        if (listaAlteracaoTipoParceiro != null)
                            listaAlteracaoFilial = listaAlteracaoFilial.Concat(listaAlteracaoTipoParceiro).ToList();

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
                        LogarUsuarioFormsAuth(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objFilial);
                    }
                }

                #region [Colocar/Remover os e-mail das vagas no bronquinha do SINE]
                        Dictionary<int, string> dic = new Dictionary<int, string>();
                        if (!objFilialAntiga.EmpresaBloqueada() && objFilial.EmpresaBloqueada())
                        {
                            dic.Add(objFilial.IdFilial, "Bloqueado pela tela de dados da empresa - BNE ");
                            new Thread(new ParameterizedThreadStart(Filial.ColocarEmailVagasBronquinha)).Start(dic);
                        }
                        else if (objFilialAntiga.EmpresaBloqueada() && !objFilial.EmpresaBloqueada())
                        {
                            dic.Add(objFilial.IdFilial, "Empresa Desblqqueada pela tela de dados da empresa - BNE ");
                            new Thread(new ParameterizedThreadStart(Filial.RemoverEmailVagasBronquinha)).Start(dic);
                        }
                #endregion


                //Publica todas as vagas
                if (!objFilial.EmpresaBloqueada())
                 Task.Factory.StartNew(() => PublicarVagas(listaVagasParaPublicacao));


                Task.Factory.StartNew(() => CelularSelecionador.HabilitarDesabilitarUsuarios(objFilial));

                if (CadastroEmpresaDadosIdFilial.HasValue)
                {
                    ucModalConfirmacao.PreencherCampos(MensagemAviso._24020, MensagemAviso._24021, false, true);
                    ucModalConfirmacao.MostrarModal();

                    CadastroEmpresaDadosIdFilial = objFilial.IdFilial;
                    IdPessoaFisica = objPessoaFisica.IdPessoaFisica;

                    string descricao = string.Empty;


                    //Paramentro Filial
                    if (objParametroFilialISSAntigo != null)
                    {
                        MensagemAssincronoLogCRM[] msgAssLogIss = MensagemAssincronoLogCRM.DiffProperties(objParametroFilialISSAntigo, objParametroFilialAuxIss, objParametroFilialISSAntigo.GetType());
                        for (int i = 0; i < msgAssLogIss.Length; i++)
                        {
                            if (msgAssLogIss[i].NewValue.Equals("1"))
                            {
                                msgAssLogIss[i].NewValue = "Sim";
                                msgAssLogIss[i].OldValue = "Não";
                            }
                            else
                            {
                                msgAssLogIss[i].NewValue = "Não";
                                msgAssLogIss[i].OldValue = "Sim";
                            }
                            msgAssLogIss[i].Property = "Tem ISS?";
                        }
                        descricao += MensagemAssincronoLogCRM.StringListaDeItensModificados(msgAssLogIss);
                    }

                    if (objParametroFilialTPAntigo != null)
                    {
                        MensagemAssincronoLogCRM[] msgAssLogTp = MensagemAssincronoLogCRM.DiffProperties(objParametroFilialTPAntigo, objParametroFilialAuxTP, objParametroFilialTPAntigo.GetType());
                        for (int i = 0; i < msgAssLogTp.Length; i++)
                        {
                            msgAssLogTp[i].Property = "Texto Personalizado Nota";
                        }
                        descricao += MensagemAssincronoLogCRM.StringListaDeItensModificados(msgAssLogTp);
                    }

                    if (!string.IsNullOrEmpty(descricao))
                    {
                        UsuarioFilialPerfil ufp = null;
                        if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                            ufp = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);
                        else if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                            ufp = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                        else
                            ufp = objUsuarioFilialPerfil;

                        MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM("Tela Cadastro Empresa: <br/><br/>" + descricao, objFilial, ufp, null);
                    }

                }
                else
                    ucConfirmacaoCadastroEmpresa.MostrarModal();



                return;
            }
            catch (Exception)
            {
                throw;
            }
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
                objVaga.Publicar();
            }
        }
        #endregion

        #region Inicializar
        private void Inicializar()
        {
            AjustarPermissoes();
            LimparCampos();

            string url = String.Format(Configuracao.EnderecoReceitaCartaoCNPJ, txtCNPJ.Valor);
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

                UIHelper.CarregarRadComboBox(rcbEditarEmpresa, SituacaoFilial.Listar());

                //selecionar no combo por padrão "aguardando publicação"
                rcbEditarEmpresa.SelectedValue = "3";

                IdOrigem = null;
                lblBancoDados.Visible = rcbBancoDados.Visible = lblTipoParceiro.Visible = rcbTipoParceiro.Visible = false;

                if (CadastroEmpresaDadosIdFilial.HasValue)
                {
                    IdOrigem = OrigemFilial.RecuperarIdOrigemPorFilial(new Filial(CadastroEmpresaDadosIdFilial.Value));
                    if (IdOrigem.HasValue)
                    {
                        UIHelper.CarregarRadComboBox(rcbBancoDados, TipoOrigem.Listar(), "Idf_Tipo_Origem", "Des_Tipo_Origem");
                        UIHelper.CarregarRadComboBox(rcbTipoParceiro, TipoParceiro.Listar(), "Idf_Tipo_Parceiro", "Des_Tipo_Parceiro");
                        lblBancoDados.Visible = rcbBancoDados.Visible = lblTipoParceiro.Visible = rcbTipoParceiro.Visible = true;
                    }
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
                    SetarObrigatoriedadeCamposCNAEeNJ(true);

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
                SetarObrigatoriedadeCamposCNAEeNJ(true);
            }

            txtCNPJ.Focus();
            UIHelper.ValidateFocus(btnSalvar);
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            txtCNPJ.Valor = string.Empty;
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
                    txtCNPJ.Valor = objFilial.CNPJ;

                txtCNPJ.ReadOnly = true;

                //Dados Gerais da Empresa
                txtSite.Text = objFilial.EnderecoSite;

                txtNumeroFuncionarios.Valor = objFilial.QuantidadeFuncionarios.ToString(CultureInfo.CurrentCulture);
                ckbCursos.Checked = objFilial.FlagOfereceCursos;
                txtFacebook.Valor = objFilial.DescricaoPaginaFacebook;

                var parametrosFilial = ParametroFilial.ListarParametros(objFilial, new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.AutorizoBNEPublicarVagas,
                    Enumeradores.Parametro.FilialContrataEstagiario,
                    Enumeradores.Parametro.FilialParceiraWebEstagios,
                    Enumeradores.Parametro.CartaAgradecimentoCandidatura,
                    Enumeradores.Parametro.CartaApresentacao,
                    Enumeradores.Parametro.Flg_Iss,
                    Enumeradores.Parametro.Descricao_Nf
                });

                ckbAutorizacao.Checked = !string.IsNullOrWhiteSpace(parametrosFilial[Enumeradores.Parametro.AutorizoBNEPublicarVagas]) && Convert.ToBoolean(parametrosFilial[Enumeradores.Parametro.AutorizoBNEPublicarVagas]);
                ckbEstagiarios.Checked = !string.IsNullOrWhiteSpace(parametrosFilial[Enumeradores.Parametro.FilialContrataEstagiario]) && Convert.ToBoolean(parametrosFilial[Enumeradores.Parametro.FilialContrataEstagiario]);

                if (pnlPerfilAdministrador.Visible)
                {
                    ckbClienteWebEstagios.Checked = !string.IsNullOrWhiteSpace(parametrosFilial[Enumeradores.Parametro.FilialParceiraWebEstagios]) && Convert.ToBoolean(parametrosFilial[Enumeradores.Parametro.FilialParceiraWebEstagios]);

                    rcbEditarEmpresa.SelectedValue = objFilial.SituacaoFilial.IdSituacaoFilial.ToString(CultureInfo.CurrentCulture);
                    txtQuantidadeUsuario.Valor = Convert.ToString(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeLimiteUsuarios)) + (objFilial.QuantidadeUsuarioAdicional ?? 0));

                    reAgradecimentoCandidatura.Content = parametrosFilial[Enumeradores.Parametro.CartaAgradecimentoCandidatura];
                    reCartaApresentacao.Content = parametrosFilial[Enumeradores.Parametro.CartaApresentacao];
                }

                txtTelefoneComercialEmpresa.Fone = objFilial.NumeroComercial;
                txtTelefoneComercialEmpresa.DDD = objFilial.NumeroDDDComercial;

                cbISS.Checked = !string.IsNullOrWhiteSpace(parametrosFilial[Enumeradores.Parametro.Flg_Iss]) && Convert.ToBoolean(Convert.ToInt16(parametrosFilial[Enumeradores.Parametro.Flg_Iss]));

                if (!string.IsNullOrWhiteSpace(parametrosFilial[Enumeradores.Parametro.Descricao_Nf]))
                    txtTextoNF.Text = parametrosFilial[Enumeradores.Parametro.Descricao_Nf];

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

                        txtCPF.Valor = objPessoaFisica.NumeroCPF;
                        txtDataNascimento.ValorDatetime = objPessoaFisica.DataNascimento;

                        txtTelefoneCelularMaster.Fone = objPessoaFisica.NumeroCelular;
                        txtTelefoneCelularMaster.DDD = objPessoaFisica.NumeroDDDCelular;
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

                SetarObrigatoriedadeCamposCNAEeNJ(false);
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
                btlExisteLogoWS.Visible = false;

                byte[] byteArray = FilialLogo.RecuperarArquivo((decimal)objFilial.NumeroCNPJ);

                if (byteArray != null)
                {
                    ucFoto.LimparFoto();
                    ucFoto.ImageData = byteArray;
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
            if (!pnlPerfilAdministrador.Visible)
            {
                txtCNPJ.Enabled = !habilitar;
            }
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

        #region SetarObrigatoriedadeCamposCNAEeNJ
        private void SetarObrigatoriedadeCamposCNAEeNJ(bool obrigatorio)
        {
            txtCNAE.Obrigatorio = obrigatorio;
            txtNaturezaJuridica.Obrigatorio = obrigatorio;
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

            LogarUsuarioFormsAuth(pf.NomeCompleto, pf.IdPessoaFisica, pf.CPF, pf.DataNascimento, new Filial(idFilial));

            UsuarioFilialPerfil objUsuarioFilialPerfil;
            if (UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(idPessoaFisica, idFilial, out objUsuarioFilialPerfil))
                base.IdUsuarioFilialPerfilLogadoEmpresa.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
        }


        private void LogarUsuarioFormsAuth(string nomePessoa, int idPessoaFisica, decimal cpf, DateTime dataNascimento, Filial filial)
        {
            var identity = BNE.Auth.BNEAutenticacao.LogarCPF(nomePessoa, idPessoaFisica, cpf, dataNascimento);

            var context = HttpContext.Current ?? Context;
            var auth = new BNEAuthLoginControlEventArgs(identity, context);
            BNE.Auth.AuthEventAggregator.Instance.OnUserEnterSuccessfully(this, auth);

            var modelResult = new BNESessaoLoginModelResult(BNESessaoLoginResultType.OK,
                BNESessaoProfileType.EMPRESA);
            BNE.Bridge.BNELoginProcess.SalvarNovaSessaoBanco(auth, modelResult, filial);
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
            if (String.IsNullOrEmpty(txtCPF.Valor) || String.IsNullOrEmpty(txtDataNascimento.Valor) || String.IsNullOrEmpty(txtCNPJ.Valor))
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
            if (!base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                //    Redirect("SalaAdministradorEmpresas.aspx");
                //else
                //{
                LogarUsuario(CadastroEmpresaDadosIdFilial.Value, IdPessoaFisica.Value);
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaSelecionador.ToString(), null));
            }
        }
        #endregion

        #region ucModalConfirmacao_eventVoltar

        void ucModalConfirmacao_eventVoltar()
        {
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                Redirect("SalaAdministradorEmpresas.aspx");
            else
            {
                LogarUsuario(CadastroEmpresaDadosIdFilial.Value, IdPessoaFisica.Value);
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaSelecionador.ToString(), null));
            }
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

            valor = valor.Trim();

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

            //valor = valor.Trim().TrimStart('0');

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
                try
                {
                    int qtdeCepEncontrados = 0;
                    var objCep = new Cep
                    {
                        sCep = valor.Replace("-", string.Empty).Trim()
                    };

                    qtdeCepEncontrados = Cep.ContaConsulta(objCep);

                    if (qtdeCepEncontrados > 1)
                        return true;

                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Falha na busca de CEP");
                }
            }
            return false;
        }
        #endregion

        #endregion

    }
}