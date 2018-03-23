using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.DTO;

namespace BNE.Web
{
    public partial class LoginComercialEmpresa : BasePage
    {

        #region Propriedades

        #region UrlOrigem - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        private string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel1.ToString()]).ToString();

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

        #region MensagemErro - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        private string MensagemErro
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel3.ToString()]).ToString();

                return null;
            }
        }
        #endregion

        #region BoolAtualizarCadastroEmpresa - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        private bool BoolAtualizarCadastroEmpresa
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return Convert.ToBoolean((ViewState[Chave.Temporaria.Variavel4.ToString()]));

                return false;
            }
        }
        #endregion

        #region DestinoLogin - Variável 5
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        private Enumeradores.LoginEmpresaDestino DestinoLogin
        {
            get
            {
                if (RouteData.Values.Count > 0 && RouteData.Values["Destino"] != null)
                {
                    Enumeradores.LoginEmpresaDestino enumDestino;
                    if (Enum.TryParse(RouteData.Values["Destino"].ToString(), true, out enumDestino))
                        return enumDestino;
                }

                return Enumeradores.LoginEmpresaDestino.Campanha;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

            ucLogin.Cancelar += ucLogin_Cancelado;
            ucLogin.Logar += ucLogin_Logar;
            ucLogin.DestinoBotaoCadastrar = UserControls.Login.DestinoCadastrar.CadastroEmpresa;

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "LoginComercialEmpresa");
        }
        #endregion

        #region ucLogin_Cancelado
        void ucLogin_Cancelado()
        {
            Redirect(UrlOrigem);
        }
        #endregion

        #region ucLogin_Logar
        void ucLogin_Logar(string urlDestino)
        {
            //Se o cpf não existe na base de dados, redirecionar para o cadastro de empresa.
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                if (!base.IdFilial.HasValue)
                {
                    Session.Add(Chave.Temporaria.Variavel3.ToString(), MensagemAviso._101001);
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), null));
                }
                else
                {
                    if (DestinoLogin.Equals(Enumeradores.LoginEmpresaDestino.Cadastro) && BoolAtualizarCadastroEmpresa)
                    {
                        Session.Add(Chave.Temporaria.Variavel1.ToString(), base.IdFilial.Value);
                        Session.Add(Chave.Temporaria.Variavel2.ToString(), base.IdPessoaFisicaLogada.Value);
                    }

                    Redirect(RecuperarDestino());
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.AtualizarDadosEmpresa.ToString(), null));
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            #region Serviço em Destaque

            string nomeServicoDestaque;
            string textoServicoDestaque;
            string HtmlServicoDestaque;
            switch (DestinoLogin)
            {
                //Atualizar Conta
                case Enumeradores.LoginEmpresaDestino.Cadastro:
                    nomeServicoDestaque = "Atualizar Conta";
                    textoServicoDestaque = "<p>Mantenha os dados da sua conta atualizados, melhorando a imagem de sua empresa para os candidatos e aumentando o número de interessados em suas vagas.</p>";
                    HtmlServicoDestaque = "<i class='fa fa-building-o ico-gg'></i>";
                    pnlServicoAtualizarEmpresa.Visible = false;
                    break;
                //Divulgar Vagas
                case Enumeradores.LoginEmpresaDestino.AnunciarVaga:
                    nomeServicoDestaque = "Divulgar Vagas";
                    textoServicoDestaque = "<p>A divulgação ocorre no site do BNE, redes sociais, sites de busca e parceiros com mais de 300 mil visualizações por dia! Conte também com anuncio ilimitado de vagas e gerenciamento de currículos recebidos.</p>";
                    HtmlServicoDestaque = "<i class='fa fa-newspaper-o ico-gg'></i>";
                    pnlServicoAnunciarVagas.Visible = false;
                    break;
                //CVs Recebidos
                case Enumeradores.LoginEmpresaDestino.VagasAnunciadas:
                case Enumeradores.LoginEmpresaDestino.CurriculosRecebidos:
                    nomeServicoDestaque = "Currículos Recebidos";
                    textoServicoDestaque = "<p>Você administra os currículos de candidatos inscritos nas suas vagas, com funcionalidades simples e ágeis de recrutamento.</p>";
                    HtmlServicoDestaque = "<i class='fa fa-paste ico-gg'></i>";
                    pnlServicoCVsRecebidos.Visible = false;
                    break;
                //Pesquisar CVs
                case Enumeradores.LoginEmpresaDestino.PesquisaCurriculo:
                    //nomeServicoDestaque = "Pesquisar CVs";
                    nomeServicoDestaque = "Pesquisa de Currículos";
                    textoServicoDestaque = "<p>Encontre através de pesquisas completas e assertivas os candidatos mais atualizados do mercado! São mais de 15 mil novos currículos diariamente.</p>";
                    HtmlServicoDestaque = "<p class='fa-stack ico-gg'><i class='fa fa-file-o fa-stack-2x'></i><i class='fa fa-search fa-stack-1x'></i></p>";
                    pnlServicoPesquisarCVs.Visible = false;
                    break;
                //Sala Selecionadora
                case Enumeradores.LoginEmpresaDestino.SalaSelecionador:
                    nomeServicoDestaque = "SMS da Selecionadora";
                    textoServicoDestaque = "<p>Recrute seus candidatos com agilidade e organização, utilizando o SMS da Selecionadora! Com ele você pode enviar SMS para vários candidatos e receber os retornos na tela do seu computador, em modelo de chat.</p>";
                    HtmlServicoDestaque = "<div style=\"margin-top: 24px;margin-right: 10px;\"><a href=\"#\" onclick=\"AbrirModalVideoSMSSelecionadora(true);\"><img src=\"/img/SalaSelecionadora/btn-video-salaselecionadora-new.png\" /></a></div>";
                    pnlServicoSalaSelecionadora.Visible = false;
                    break;
                //Site Trabalhe Conosco Grátis
                case Enumeradores.LoginEmpresaDestino.STC:
                    nomeServicoDestaque = "Site Trabalhe Conosco";
                    textoServicoDestaque = "<p>Sua empresa divulga vagas disponíveis e recebe currículos de interessados a partir do seu próprio site! Com um sistema personalizado e exclusivo para conectar em seu site, no link 'trabalhe conosco'.</p>";
                    HtmlServicoDestaque = "<i class='fa fa-database ico-gg'></i>";
                    pnlServicoSiteTrabalheConosco.Visible = false;
                    break;
                default:
                    nomeServicoDestaque = "Campanha de Recrutamento";
                    textoServicoDestaque = "<p>Solicite online a convocação em massa de candidatos operacionais e de apoio e tenha retorno imediato e assertivo, agilizando o fechamento de suas vagas.</p>";
                    HtmlServicoDestaque = "<i class='fa fa-bullhorn ico-gg'></i>";
                    pnlServicoCompreCV.Visible = false;
                    break;
            }

            //Define as informações do serviço em destaque
            litIconeGrandeServicoDestaque.Text = HtmlServicoDestaque;

            litNomeServicoDestaque.Text = nomeServicoDestaque;
            litDescricaoServicoDestaque.Text = textoServicoDestaque;

            hlPesquisaCurriculo.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.PesquisaCurriculo });
            hlAnunciarVaga.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.AnunciarVaga });
            hlSalaSelecionadora.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.SalaSelecionador });
            hlCampanha.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.Campanha });
            hlSTC.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.STC });
            hlAtualizarEmpresa.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.Cadastro });
            hlCurriculosRecebidos.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.CurriculosRecebidos });
            #endregion

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;

            //Quando o usuário candidato tentar se logar pelo Login Comercial Empresa, irá mostra a mensagem abaixo
            if (!string.IsNullOrEmpty(MensagemErro))
                ExibirMensagem(MensagemErro, TipoMensagem.Aviso);

            ucLogin.Inicializar();
        }
        #endregion

        #region RecuperarDestino
        private string RecuperarDestino()
        {

            switch (DestinoLogin)
            {
                case Enumeradores.LoginEmpresaDestino.Cadastro:
                    BLL.Filial objFilial = new BLL.Filial(base.IdFilial.Value);

                    var retorno = EmpresaBloqueada(objFilial) ? "~/Principal.aspx" : GetRouteUrl(Enumeradores.RouteCollection.AtualizarDadosEmpresa.ToString(), null);
                    return retorno;
                case Enumeradores.LoginEmpresaDestino.AnunciarVaga:
                    return GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null);
                case Enumeradores.LoginEmpresaDestino.VagasAnunciadas:
                    return GetRouteUrl(Enumeradores.RouteCollection.VagasAnunciadas.ToString(), null);
                case Enumeradores.LoginEmpresaDestino.PesquisaCurriculo:
                    return GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculoAvancada.ToString(), null);
                case Enumeradores.LoginEmpresaDestino.SalaSelecionador:
                    return GetRouteUrl(Enumeradores.RouteCollection.SalaSelecionador.ToString(), null);
                case Enumeradores.LoginEmpresaDestino.STC:
                    return GetRouteUrl(Enumeradores.RouteCollection.ConfiguracaoSTC.ToString(), null);
                case Enumeradores.LoginEmpresaDestino.CompreCV:
                    return GetRouteUrl(Enumeradores.RouteCollection.ProdutoCIA.ToString(), null);
                case Enumeradores.LoginEmpresaDestino.Campanha:
                    return GetRouteUrl(Enumeradores.RouteCollection.CampanhaRecrutamento.ToString(), null);

            }
            return string.Empty;
        }
        #endregion

        #endregion

    }
}