using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using BNE.BLL;
using BNE.BLL.AsyncServices;
using BNE.BLL.Integracoes.Facebook;
using BNE.Services.Base.ProcessosAssincronos;
using Newtonsoft.Json;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Parametro = BNE.BLL.Parametro;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;
using System.Configuration;
using System.Linq;

namespace BNE.Web.UserControls
{
    public partial class Login : BaseUserControl
    {

        #region Propriedades

        #region DadosInvalidos - Variavel1
        /// <summary>
        /// Propriedade que faz a verificação dos dados informados em tela
        /// </summary>
        public bool DadosInvalidos
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel1.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }

        #endregion

        #region AutenticadoAD - Variavel4
        /// <summary>
        /// Propriedade que verifica se o usuário autenticou no AD
        /// </summary>
        public bool AutenticadoAD
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel4.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
            }
        }
        #endregion

        #region LoginComFacebook
        public bool LoginComFacebook
        {
            set { pnlLoginFacebook.Visible = value; }
        }
        #endregion

        #region RedirecionarParaVagasBNE - Variavel2
        /// <summary>
        /// Propriedade armazena se o login deve redirecionar para o projeto de vagas ou não, para aproveitar as regras de login
        /// </summary>
        public bool RedirectDiretoVagasBNE
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel2.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }

        #endregion

        #region RedirectURL - Variavel3
        /// <summary>
        /// Propriedade armazena se o login deve redirecionar para uma url ou não, para aproveitar as regras de login
        /// </summary>
        public string RedirectUrl
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel3.ToString()].ToString();
                return string.Empty;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
            }
        }
        #endregion

        #region DestinoBotaoCadastrar - Variavel5
        /// <summary>
        /// Propriedade armazena o destino do botão cadastrar
        /// </summary>
        public DestinoCadastrar DestinoBotaoCadastrar
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                {
                    DestinoCadastrar enumDestino;
                    if (Enum.TryParse(ViewState[Chave.Temporaria.Variavel5.ToString()].ToString(), true, out enumDestino))
                        return enumDestino;
                }

                return DestinoCadastrar.CadastroCurriculo;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
            }
        }

        public bool RedirectDiretoVip
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel6.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
            }
        }
        #endregion

        #region DefaultRedirectParamArg
        public string DefaultRedirectParamArg
        {
            get
            {
                return ConfigurationManager.AppSettings["aspnet:FormsAuthReturnUrlVar"] ?? "ReturnUrl";
            }
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void DelegateCancelarLogin();
        public event DelegateCancelarLogin Cancelar;
        public delegate void logar(string urlDestino);
        public event logar Logar;
        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ucModalTrocarEmpresa.EmpresaSelecionada += ucModalTrocarEmpresa_EmpresaSelecionada;

            string parameter = Request["__EVENTARGUMENT"];
            if (parameter == "facebook")
                btnEntrarFacebook_Click(sender, null);


            Ajax.Utility.RegisterTypeForAjax(typeof(Login));
        }
        #endregion

        #region ucModalTrocarEmpresa_EmpresaSelecionada
        void ucModalTrocarEmpresa_EmpresaSelecionada(string urlDestino)
        {
            if (Logar != null)
                Logar(urlDestino);
        }
        #endregion

        #region btnEntrar_Click
        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCPF.Valor) && txtDataNascimento.ValorDateTime.HasValue)
                        Validar(); 
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnCancelar_Click
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (Cancelar != null)
                Cancelar();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            Inicializar(false, false, TentarUsarEnderecoDeRedirecionamento());
        }

        private string TentarUsarEnderecoDeRedirecionamento()
        {
            var req = HttpContext.Current != null ? HttpContext.Current.Request : Request;

            var redirectInfo = req[DefaultRedirectParamArg];
            if (string.IsNullOrWhiteSpace(redirectInfo))
            {
                redirectInfo = req["RedirectUrl"] ?? string.Empty;
            }

            return redirectInfo;
        }

        public void Inicializar(bool redirectDiretoVagasBNE, bool redirectDiretoNovoVip, string redirectUrl)
        {
            RedirectDiretoVagasBNE = redirectDiretoVagasBNE;
            RedirectDiretoVip = redirectDiretoNovoVip;
            RedirectUrl = redirectUrl;

            //Setando validation groups
            txtCPF.ValidationGroup = btnEntrar.ClientID;
            txtDataNascimento.ValidationGroup = btnEntrar.ClientID;
            btnEntrar.ValidationGroup = btnEntrar.ClientID;

            LimparCampos();

            //pnlInformacaoAutenticacao.Visible = false;
            AutenticadoAD = false;
            pnlInformacaoAD.Visible = false;

            txtCPF.Focus();

            upCpf.Update();
            upDataNascimento.Update();
            upAvisoAutenticacao.Update();
            upInformacaoAD.Update();
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            txtCPF.Valor = String.Empty;
            txtDataNascimento.ValorDateTime = null;
        }
        #endregion

        #region Validar
        private void Validar()
        {
            if (txtDataNascimento.ValorDateTime.HasValue)
            {
                PessoaFisica objPessoaFisica;
                if (PessoaFisica.CarregarPorCPF(Convert.ToDecimal(txtCPF.Valor), out objPessoaFisica))
                {
                    //Se a data informada for diferente do cadastro mostra mensagem para ir para o SOSRH.
                    if (!txtDataNascimento.ValorDateTime.Equals(objPessoaFisica.DataNascimento))
                    {
                        //Os dados foram informados incorretamente
                        DadosInvalidos = true;
                        AjustaTelaDadosInvalidos();
                    }
                    else
                    {
                        ValidarPessoaFisica(objPessoaFisica);
                    }
                }
                else //Se a pessoa fisica não existir no banco de dados
                {
                    OnLogar(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
                }
            }
            else
                txtDataNascimento.Focus();
        }
        #endregion

        #region ValidarPessoaFisica
        private void ValidarPessoaFisica(PessoaFisica objPessoaFisica)
        {
            // Processo de LOGIN UTILIZADO de forma semelhante também no:
            // - BasePage.aspx, no EntrarController (neste mesmo projeto, BNE WEB), 
            // - No Filtro de login do VAGAS.BNE 
            // - No BNE.Auth (RecuperarSessão/RestaurarSessão)
            // - Talvez no Novo VIP

            //Os dados foram informados corretamente
            DadosInvalidos = false;

            //Se o usuário for inativo mostrar mensagem especifica.
            if (Convert.ToBoolean(objPessoaFisica.FlagInativo))
                ExibirMensagem(MensagemAviso._202406, TipoMensagem.Aviso);
            else
            {
                //Busca qtos perfis esse usuário tem desbloquados													  
                if (UsuarioFilialPerfil.PossuiPerfilAtivo(objPessoaFisica))
                {
                    int quantidadeEmpresa = UsuarioFilialPerfil.QuantidadeUsuarioEmpresa(objPessoaFisica);

                    if (quantidadeEmpresa != 0) //Se o usuário tiver empresa relacionada.
                    {
                        //Verifica se o usuário é interno para ser redirecionado para a tela de manutenção do sistema
                        int idUsuarioFilialPerfil, idPerfil;
                        if (PessoaFisica.VerificaPessoaFisicaUsuarioInterno(objPessoaFisica.IdPessoaFisica, out idUsuarioFilialPerfil, out idPerfil))
                            ValidaUsuarioInterno(idUsuarioFilialPerfil, idPerfil, objPessoaFisica);
                        else if (base.STC.Value)
                        {
                            OrigemFilial objOrigemFilial;
                            if (OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial))
                            {
                                UsuarioFilialPerfil objUsuarioFilialPerfil;
                                if (UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(objPessoaFisica.IdPessoaFisica, objOrigemFilial.Filial.IdFilial, out objUsuarioFilialPerfil))
                                {
                                    if (!objUsuarioFilialPerfil.FlagInativo)
                                    {
                                        //Gravar origem do acesso
                                        GravarOrigemAcesso(objUsuarioFilialPerfil.IdUsuarioFilialPerfil);

                                        LogarPessoaFisica(objPessoaFisica);
                                        base.IdFilial.Value = objUsuarioFilialPerfil.Filial.IdFilial;
                                        base.IdUsuarioFilialPerfilLogadoEmpresa.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                                        BNE.Auth.BNEAutenticacao.LogarCPF(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, ckMantenhaLogado != null ? ckMantenhaLogado.Checked : false);

                                        if (RedirectDiretoVagasBNE)
                                            RedirecionarParaVagas(RedirectUrl);
                                        else if (RedirectDiretoVip)
                                            RedirecionarParaNovoVip(RedirectUrl);
                                        else
                                        {
                                            if (string.IsNullOrWhiteSpace(RedirectUrl))
                                            {
                                                OnLogar("SiteTrabalheConoscoMenu.aspx");
                                            }
                                            else
                                            {
                                                OnLogar(RedirectUrl);
                                            }
                                        }
                                    }
                                    else
                                        ExibirMensagem(MensagemAviso._202406, TipoMensagem.Aviso);
                                }
                                else
                                {
                                    DadosInvalidos = true;
                                    AjustaTelaDadosInvalidos();
                                }
                            }
                            else
                            {
                                DadosInvalidos = true;
                                AjustaTelaDadosInvalidos();
                            }
                        }
                        else
                            CarregarUsuarioEmpresa(quantidadeEmpresa, objPessoaFisica);
                    }
                    else
                    {
                        //Verifica se o usuário é interno para ser redirecionado para a tela de manutenção do sistema
                        int idUsuarioFilialPerfil, idPerfil;
                        if (PessoaFisica.VerificaPessoaFisicaUsuarioInterno(objPessoaFisica.IdPessoaFisica, out idUsuarioFilialPerfil, out idPerfil))
                            ValidaUsuarioInterno(idUsuarioFilialPerfil, idPerfil, objPessoaFisica);
                        else
                            CarregarUsuarioCandidato(objPessoaFisica);
                    }
                }
                else
                    ExibirMensagem(MensagemAviso._202406, TipoMensagem.Aviso);
            }
        }

        private void OnLogar(string url)
        {
            if (Logar != null)
                Logar(url);
        }
        #endregion

        #region RedirecionarParaVagas
        private void RedirecionarParaVagas(string redirectURL)
        {
            var mainUrlAccessor = new Func<string>(() => BNE.BLL.Custom.Helper.RecuperarURLVagas());
            RedirecionarCustomizado(redirectURL, mainUrlAccessor);
        }
        #endregion

        #region RedirecionarParaVip
        private void RedirecionarParaNovoVip(string redirectURL)
        {
            var mainUrlAccessor = new Func<string>(() => BNE.BLL.Custom.Helper.RecuperarURLNovoVip());
            RedirecionarCustomizado(redirectURL, mainUrlAccessor);
        }
        #endregion

        private void RedirecionarCustomizado(string redirectURL, Func<string> mainUrlAccessor)
        {
            if (redirectURL.StartsWith("http", StringComparison.OrdinalIgnoreCase) ||
                redirectURL.StartsWith("ww", StringComparison.OrdinalIgnoreCase) ||
                redirectURL.IndexOf(".bne.com.br", StringComparison.OrdinalIgnoreCase) > -1)
            {
                Redirect(redirectURL);
                return;
            }

            if (redirectURL.StartsWith("~"))
                redirectURL = new string(redirectURL.Skip(1).ToArray());
            else if (redirectURL.StartsWith("%7E"))
                redirectURL = new string(redirectURL.Skip(3).ToArray());

            var mainUrl = mainUrlAccessor();

            string urlResult;
            if (mainUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                urlResult = mainUrl + redirectURL;
            else
                urlResult = string.Concat("http://", mainUrl, redirectURL);

            Redirect(urlResult);
        }

        #region ValidarAD
        private void ValidarAD(PessoaFisica objPessoaFisica)
        {
            //string usuario = txtNomeUsuario.Valor;
            string senha = txtSenhaUsuario.Valor.Trim();

            //if (!AutenticadoAD && (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(senha)))
            //    AutenticadoAD = BLL.Custom.ActiveDirectory.VerificarUsuario(usuario, senha);

            if (!AutenticadoAD && !string.IsNullOrEmpty(senha))
                AutenticadoAD = PageHelper.ValidarSenhaUsuario(objPessoaFisica.IdPessoaFisica, senha);
        }
        #endregion

        #region ValidaUsuarioInterno
        private void ValidaUsuarioInterno(int idUsuarioFilialPerfil, int idPerfil, PessoaFisica objPessoaFisica)
        {
            ValidarAD(objPessoaFisica);

            if (AutenticadoAD)
            {
                //Guarda o IdUsuarioFilialPerfil e o IdPerfilSession do Usuário interno em váriavel global
                base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value = idUsuarioFilialPerfil;
                base.IdPerfil.Value = idPerfil;
                LogarPessoaFisica(objPessoaFisica);

                BNE.Auth.BNEAutenticacao.LogarCPF(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, ckMantenhaLogado != null ? ckMantenhaLogado.Checked : false);
                Redirect("~/SalaAdministrador.aspx");

            }
            else
            {
                pnlInformacaoAD.Visible = true;
                //txtNomeUsuario.ValidationGroup = btnEntrar.ValidationGroup;
                txtSenhaUsuario.ValidationGroup = btnEntrar.ValidationGroup;
                HabilitaBotaoEntrar(true);
            }
        }
        #endregion

        #region CarregarUsuarioEmpresa
        /// <summary>
        /// Inicializa em tela as configurações de um Usuário Empresa.
        /// </summary>
        /// <param name="quantidadeEmpresa">Quantidade de empresas ligadas ao CPF</param>
        /// <param name="objPessoaFisica">Pessoa Física</param>
        private void CarregarUsuarioEmpresa(int quantidadeEmpresa, PessoaFisica objPessoaFisica)
        {
            Curriculo objCurriculo;
            if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
            {
                if (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.Bloqueado) || objCurriculo.FlagInativo)
                    ExibirMensagem(MensagemAviso._103703, TipoMensagem.Erro, true);
                else
                {
                    BNE.Auth.BNEAutenticacao.LogarCandidato(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.IdCurriculo, ckMantenhaLogado != null ? ckMantenhaLogado.Checked : false);

                    UsuarioFilialPerfil objUsuarioFilialPerfil;
                    if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objUsuarioFilialPerfil))
                        base.IdUsuarioFilialPerfilLogadoCandidato.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                    base.IdCurriculo.Value = objCurriculo.IdCurriculo;
                    LogarPessoaFisica(objPessoaFisica);
                }
            }
            else
            {
                BNE.Auth.BNEAutenticacao.LogarCPF(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, ckMantenhaLogado != null ? ckMantenhaLogado.Checked : false);
            }

            if (quantidadeEmpresa > 1)
            {
                Usuario objUsuario;
                if (Usuario.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuario))
                {
                    LogarPessoaFisica(objPessoaFisica);

                    if (objUsuario.UltimaFilialLogada != null)
                    {
                        //Verifica se o usuário filial perfil está inativo para a IDUltimaFilialLogada
                        if (UsuarioFilialPerfil.VerificaUsuarioFilialPorUltimaEmpresaLogada(objPessoaFisica.IdPessoaFisica, objUsuario.UltimaFilialLogada.IdFilial))
                        {
                            VerificarLoginEmpresa(objPessoaFisica.IdPessoaFisica, objUsuario.UltimaFilialLogada.IdFilial);

                            base.IdFilial.Value = objUsuario.UltimaFilialLogada.IdFilial;

                            //Carrega o usuário filial perfil pela pessoa física e filial
                            UsuarioFilialPerfil objUsuarioFilialPerfil;
                            if (UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(objPessoaFisica.IdPessoaFisica, base.IdFilial.Value, out objUsuarioFilialPerfil))
                                base.IdUsuarioFilialPerfilLogadoEmpresa.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                            if (RedirectDiretoVagasBNE)
                                RedirecionarParaVagas(RedirectUrl);
                            else if (RedirectDiretoVip)
                                RedirecionarParaNovoVip(RedirectUrl);
                            else
                            {
                                if (string.IsNullOrWhiteSpace(RedirectUrl))
                                {
                                    OnLogar(GetRouteUrl(Enumeradores.RouteCollection.SalaSelecionador.ToString(), null));
                                }
                                else
                                {
                                    OnLogar(RedirectUrl);
                                }
                            }
                        }
                        else
                        {
                            objUsuario.UltimaFilialLogada = null;
                            objUsuario.Save();
                            MostrarModalSelecaoEmpresa();
                        }
                    }
                    else
                        MostrarModalSelecaoEmpresa();
                }

                HabilitaBotaoEntrar(true);
            }
            else
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (UsuarioFilialPerfil.CarregarUsuarioEmpresaPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil))
                {
                    VerificarLoginEmpresa(objPessoaFisica.IdPessoaFisica, objUsuarioFilialPerfil.Filial.IdFilial);

                    base.IdUsuarioFilialPerfilLogadoEmpresa.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
                    base.IdFilial.Value = objUsuarioFilialPerfil.Filial.IdFilial;

                    LogarPessoaFisica(objPessoaFisica);

                    if (RedirectDiretoVagasBNE)
                        RedirecionarParaVagas(RedirectUrl);
                    else if (RedirectDiretoVip)
                        RedirecionarParaNovoVip(RedirectUrl);
                    else
                    {
                        if (string.IsNullOrWhiteSpace(RedirectUrl))
                        {
                            OnLogar("~/SalaSelecionador.aspx");
                        }
                        else
                        {
                            OnLogar(RedirectUrl);
                        }
                    }
                }
            }
        }
        #endregion

        #region CarregarUsuarioCandidato
        private void CarregarUsuarioCandidato(PessoaFisica objPessoaFisica)
        {
            // Processo de LOGIN UTILIZADO de forma semelhante também no:
            // - BasePage.aspx no EntrarController (neste mesmo projeto), 
            // - No Filtro de login do VAGAS.BNE 
            // - No BNE.Auth (RecuperarSessão/RestaurarSessão)

            Curriculo objCurriculo;
            
            if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
            {
                //para saber se desbloqeuio o e-mail ao final do processo
                var idSituacaoCV = objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo;

                //Task 43574 - Não logar currículo nessa situação.
                if (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.ExclusaoLogica))
                {
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
                }
                /*Caso o curriculo do usuário tenha sido excluido por ele, ativa novamente após login*/
                if (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.Hibernado) && objCurriculo.FlagInativo)
                {
                    objCurriculo.FlagInativo = false;
                    objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoPublicacao);
                }
       
                //Verifica se o curriculo está bloqueado
                if (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.Bloqueado) || objCurriculo.FlagInativo)
                    ExibirMensagem(MensagemAviso._103703, TipoMensagem.Erro, true);
                else
                {
                    base.IdCurriculo.Value = objCurriculo.IdCurriculo;
                    LogarPessoaFisica(objPessoaFisica);
                    BNE.Auth.BNEAutenticacao.LogarCandidato(objPessoaFisica.NomeCompleto, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.IdCurriculo, ckMantenhaLogado != null ? ckMantenhaLogado.Checked : false);

                    //@reinaldo: Grava a origem do acesso sempre após logar
                    UsuarioFilialPerfil objUsuarioFilialPerfil = null;
                    if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objUsuarioFilialPerfil))
                        GravarOrigemAcesso(objUsuarioFilialPerfil.IdUsuarioFilialPerfil);
                    //@reinaldo: fim

                    if (CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, new Origem(base.IdOrigem.Value)))
                    {
                       
                            BoasVindasNovamente(objPessoaFisica); //Envia email para candidato.

                            objCurriculo.Salvar();
                            if (idSituacaoCV.Equals((int)Enumeradores.SituacaoCurriculo.Hibernado))
                                EmailSituacao.DesbloquearEmail(objPessoaFisica.IdPessoaFisica, objPessoaFisica.EmailPessoa);

                            #region Publicação Currículo

                            try
                            {
                                var enfileiraPublicacao = Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.EnfileiraPublicacaoAutomaticaCurriculo));
                                if (enfileiraPublicacao)
                                {
                                    var parametros = new ParametroExecucaoCollection
                                    {
                                        {"idCurriculo", "Curriculo", objCurriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture), objCurriculo.IdCurriculo.ToString(CultureInfo.InvariantCulture)}
                                    };

                                    ProcessoAssincrono.IniciarAtividade(TipoAtividade.PublicacaoCurriculo, parametros);
                                }
                            }
                            catch (Exception ex)
                            {
                                EL.GerenciadorException.GravarExcecao(ex);
                            }

                            #endregion

                            //Guardar IdUsuarioFilialPerfil em variável global conforme requisito
                            if (objUsuarioFilialPerfil != null)
                            {
                                //Gravar no cookie para não aparecer mais a popup
                                GravarCookiePreCadastro("PreCadastro", "Logado");

                                base.IdUsuarioFilialPerfilLogadoCandidato.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
                                base.IdFilial.Clear();

                                if (RedirectDiretoVagasBNE)
                                    RedirecionarParaVagas(RedirectUrl);
                                else if (RedirectDiretoVip)
                                    RedirecionarParaNovoVip(RedirectUrl);
                                else
                                {
                                    if (string.IsNullOrWhiteSpace(RedirectUrl))
                                    {
                                        OnLogar(GetRouteUrl(Enumeradores.RouteCollection.SalaVIP.ToString(), null));
                                    }
                                    else
                                    {
                                        OnLogar(RedirectUrl);
                                    }
                                }
                            }
                            else
                                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
                        
                    }
                    else
                    {
                        //Caso a pessoa tenha curriculo, mas o curriculo não seja da origem BNE
                        Session.Add(Chave.Temporaria.Variavel1.ToString(), objPessoaFisica.IdPessoaFisica);
                        if (base.STC.Value && Logar != null)
                        {
                            Logar(RedirectUrl);
                        }
                        else
                        {
                            Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
                        }
                    }
                }
            }
            else //Se o usuário não existir
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
        }
        #endregion

        #region GravarOrigemAcesso
        private void GravarOrigemAcesso(int idUsuarioFilialPerfil)
        {
            try
            {
                string strOrigemUrlReferr = Session["OrigemHTTP_REFERER"] != null ? Session["OrigemHTTP_REFERER"].ToString() : "";
                string strOrigemQuery = Session["OrigemQUERY_STRING"] != null ? Session["OrigemQUERY_STRING"].ToString() : "";
                string strOrigemUtmSource = Session["OrigemUtmSource"] != null ? Session["OrigemUtmSource"].ToString() : "";
                string strOrigemUtmMedium = Session["OrigemUtmMedium"] != null ? Session["OrigemUtmMedium"].ToString() : "";
                string strOrigemUtmCampaign = Session["OrigemUtmCampaign"] != null ? Session["OrigemUtmCampaign"].ToString() : "";
                string strOrigemUtmTerm = Session["OrigemUtmTerm"] != null ? Session["OrigemUtmTerm"].ToString() : "";
                string strOrigemUtmKeyWords = Session["OrigemQUERY_STRING"] != null ? Session["OrigemQUERY_STRING"].ToString() : "";

                if (string.IsNullOrEmpty(strOrigemUtmSource) && !string.IsNullOrEmpty(strOrigemUrlReferr))
                {
                    string[] Origem = BLL.Custom.Helper.TratarUrlOrigem(strOrigemUrlReferr);

                    if (Origem != null)
                    {
                        strOrigemUrlReferr = Origem[0];
                        strOrigemUtmKeyWords = Origem[1];
                    }
                }

                UsuarioFilialPerfil.GravarDadosOrigemAcesso(idUsuarioFilialPerfil,
                                strOrigemUrlReferr,
                                strOrigemQuery,
                                strOrigemUtmSource,
                                strOrigemUtmMedium,
                                strOrigemUtmCampaign,
                                strOrigemUtmTerm,
                                strOrigemUtmKeyWords);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao Tratar Url Origem");
            }
        }

        #endregion

        #region LogarPessoaFisica
        private void LogarPessoaFisica(PessoaFisica objPessoaFisica)
        {
            base.IdPessoaFisicaLogada.Value = objPessoaFisica.IdPessoaFisica;
            PageHelper.AtualizarSessaoUsuario(objPessoaFisica.IdPessoaFisica, Session.SessionID);

            if (HttpContext.Current.Session["DadosFacebookPessoa"] != null) //Facebook
                GravarCookieAcesso(objPessoaFisica);
            else if (ckMantenhaLogado.Checked) //Checou para salvar em seus dados
                GravarCookieAcesso(objPessoaFisica);

            GravarCookieLoginVagas(objPessoaFisica);
        }
        #endregion

        #region MostrarModalSelecaoEmpresav
        private void MostrarModalSelecaoEmpresa()
        {
            ucModalTrocarEmpresa.Inicializar();
            ucModalTrocarEmpresa.MostrarModal();
        }
        #endregion

        #region AjustaTelaDadosInvalidos
        /// <summary>
        /// Metodo que ajusta a tela quando o usuário informa dados incorretos
        /// </summary>
        public void AjustaTelaDadosInvalidos()
        {
            HabilitaBotaoEntrar(false);
            ExibirMensagem(MensagemAviso._103711, TipoMensagem.Aviso);
        }
        #endregion

        #region HabilitaBotaoEntrar
        public void HabilitaBotaoEntrar(bool habilitar)
        {
            //btnEntrar.Enabled = habilitar;
            //upBtnEntrar.Update();
        }
        #endregion

        #region btlCadastrar_Click
        protected void btlCadastrar_Click(object sender, EventArgs e)
        {
            switch (DestinoBotaoCadastrar)
            {
                case DestinoCadastrar.CadastroCurriculo:
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
                    break;
                case DestinoCadastrar.CadastroEmpresa:
                    Redirect("/cadastro-de-empresa-gratis");
                    break;
            }
        }
        #endregion

        #region btnEntrarFacebook_Click
        protected void btnEntrarFacebook_Click(object sender, ImageClickEventArgs e)
        {
            if (HttpContext.Current.Session["DadosFacebookPessoa"] != null)
                ValidarPessoaFisica(PessoaFisica.LoadObject(Convert.ToInt32(HttpContext.Current.Session["DadosFacebookPessoa"])));
        }
        #endregion

        #endregion

        #region Ajax Methods

        #region ValidarFacebook
        /// <summary>
        /// Validar id do facebook
        /// </summary>
        /// <param name="idFacebook"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool ValidarFacebook(string idFacebook)
        {
            try
            {
                PessoaFisicaRedeSocial objPessoaFisicaRedeSocial;
                if (PessoaFisicaRedeSocial.CarregarPorCodigoRedeSocial(idFacebook, BNE.BLL.Enumeradores.RedeSocial.FaceBook, out objPessoaFisicaRedeSocial))
                {
                    HttpContext.Current.Session["DadosFacebookPessoa"] = objPessoaFisicaRedeSocial.PessoaFisica.IdPessoaFisica;
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha ao recuperar dados do Facebook");
                throw;
            }
        }
        #endregion

        #region ArmazenarDadosFacebook
        /// <summary>
        /// Validar id do facebook
        /// </summary>
        /// <param name="jsonFacebook">JSON com todos os dados do Facebook</param>
        /// <param name="jsonFriendsFacebook">JSON com todos os amigos do Facebook</param>
        /// <param name="jsonMePicture">JSON com a url da foto do candidato</param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void ArmazenarDadosFacebook(string jsonFacebook, string jsonFriendsFacebook, string jsonMePicture)
        {
            try
            {
                var dadosFacebook = JsonConvert.DeserializeObject<ProfileFacebook.DadosFacebook>(jsonFacebook);
                var dadosFoto = JsonConvert.DeserializeObject<ProfileFacebook.FotoFacebook>(jsonMePicture);

                HttpContext.Current.Session["DadosFacebook"] = dadosFacebook;
                HttpContext.Current.Session["DadosFotoFacebook"] = dadosFoto;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha ao recuperar dados do Facebook");
                throw;
            }
        }
        #endregion

        #endregion

        public enum DestinoCadastrar
        {
            CadastroEmpresa,
            CadastroCurriculo
        }

        #region EnviarBoasVindasNovamente
        /// <summary>
        /// Envia E-mail para o candidato que voltar a acessar o site.
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <remarks>Ramalho</remarks>
        public void BoasVindasNovamente(PessoaFisica objPessoaFisica)
        {
            int numDias = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DataEnvioBoasVindasNovamente));
            Curriculo objCurriculo;
            Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);


            if (objCurriculo == null)
                return;

            //Se a data de Atualização foi a mais de X dias envia email de Boas Vindas Novamente
            if (objCurriculo.DataAtualizacao < DateTime.Now.AddDays(-numDias))
            {
                if (BLL.Custom.Validacao.ValidarEmail(objPessoaFisica.EmailPessoa)) // se o email for válido.
                {
                    string emailRemetente = Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail);

                    UsuarioFilialPerfil objPerfilDestinatario = null;
                    UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objPessoaFisica, out objPerfilDestinatario);
                    Estatistica _estatisticas = Estatistica.Estatisticas;

                    CartaEmail objcarta = CartaEmail.LoadObject(Convert.ToInt32(BNE.BLL.Enumeradores.CartaEmail.BoasVindasNovamente));

                    objcarta.DescricaoAssunto = objcarta.DescricaoAssunto.Replace("{nomeCandidato}", objPessoaFisica.NomeCompleto);

                    var carta = CartaEmail.RecuperarConteudo(BLL.Enumeradores.CartaEmail.BoasVindasNovamente);

                    string listaVagas, salaVip, vip, cadastroCurriculo, quemMeViu, pesquisaVagas, loginCandidato, cadastroExperiencias;
                    Curriculo.RetornarHashLogarCurriculo(objPessoaFisica.CPF, objPessoaFisica.DataNascimento, out listaVagas, out salaVip, out vip, out quemMeViu, out cadastroCurriculo, out pesquisaVagas, out loginCandidato, out cadastroExperiencias);


                    carta = carta.Replace("{nomeCandidato}", objPessoaFisica.NomeCompleto);
                    carta = carta.Replace("{login_candidato}", vip);
                    carta = carta.Replace("{login_candidato}", salaVip);
                    carta = carta.Replace("{Quantidade_empresas}", _estatisticas.QuantidadeEmpresa.ToString());
                    carta = carta.Replace("{Quantidade_vagas}", _estatisticas.QuantidadeVaga.ToString());
                    carta = carta.Replace("{nomeCandidato}", objPessoaFisica.NomeCompleto);
                    carta = carta.Replace("{vip}", vip);
                    carta = carta.Replace("{Quem_Me_Viu}", quemMeViu);
                    carta = carta.Replace("{Sala_Vip}", salaVip);
                    carta = carta.Replace("{Pesquisa_Vagas}", pesquisaVagas);
                    carta = carta.Replace("{Cadastro_Curriculo}", cadastroCurriculo);

                    MensagemSistema objMensagem = new MensagemSistema();

                    objMensagem.DescricaoMensagemSistema = carta;

                    //Enviar E-mail para o candidato
                    MensagemCS.SalvarEmail(objCurriculo, null, objPerfilDestinatario, null, objcarta.DescricaoAssunto, objMensagem.DescricaoMensagemSistema, BLL.Enumeradores.CartaEmail.BoasVindasNovamente,
                    emailRemetente, objPessoaFisica.EmailPessoa, null, null, null);
                }

            }



        }

        #endregion EnviarBoasVindasNovamente
    }
}