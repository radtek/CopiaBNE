using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

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

                return Enumeradores.LoginEmpresaDestino.CompreCV;
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
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroEmpresaDados.ToString(), null));
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
                //Atualizar Empresa
                case Enumeradores.LoginEmpresaDestino.Cadastro:
                    nomeServicoDestaque = "Atualizar Empresa";
                    textoServicoDestaque = "<p>Mantenha os dados da sua empresa sempre atualizados, assim você potencializará a captação dos talentos interessados em suas vagas.</p>";
                    HtmlServicoDestaque = "<i class='fa fa-building-o ico-gg'></i>";
                    pnlServicoAtualizarEmpresa.Visible = false;
                    break;
                //Compre CVs
                case Enumeradores.LoginEmpresaDestino.AnunciarVaga:
                    nomeServicoDestaque = "Publicar Vagas";
                    textoServicoDestaque = "<p>Suas vagas disponíveis para todos candidatos cadastrados no BNE e encaminhadas por e-mail e sms para aqueles que estão no perfíl. Este serviço é ilimitado para empresas cadastradas. Divulgue agora mesmo suas vagas.</p>";
                    HtmlServicoDestaque = "<i class='fa fa-bullhorn ico-gg'></i>";
                    pnlServicoAnunciarVagas.Visible = false;
                    break;
                //CVs Recebidos
                case Enumeradores.LoginEmpresaDestino.VagasAnunciadas:
                case Enumeradores.LoginEmpresaDestino.CurriculosRecebidos:
                    nomeServicoDestaque = "CVs Recebidos";
                    textoServicoDestaque = "<p>Receba CVs dos candidatos interessados em um único lugar, pré triados como dentro ou fora do perfil, de acordo com os requisitos de sua vaga, você não perde tempo e ainda economiza com convites para entrevistas por SMS.</p>";
                    HtmlServicoDestaque = "<i class='fa fa-files-o ico-gg'></i>";
                    pnlServicoCVsRecebidos.Visible = false;
                    break;
                //Pesquisar CVs
                case Enumeradores.LoginEmpresaDestino.PesquisaCurriculo:
                    nomeServicoDestaque = "Pesquisar CVs";
                    textoServicoDestaque = "<p>Encontre em um banco com mais de 5 milhões de currículos o perfil ideal para suas vagas, contando com diversos filtros que permitirão exatidão em seu resultado e novidades como comparar currículo.</p>";
                    HtmlServicoDestaque = "<p class='fa-stack ico-gg'><i class='fa fa-file-o fa-stack-2x'></i><i class='fa fa-search fa-stack-1x'></i></p>";
                    pnlServicoPesquisarCVs.Visible = false;
                    break;
                //Sala Selecionadora
                case Enumeradores.LoginEmpresaDestino.SalaSelecionador:
                    nomeServicoDestaque = "Sala Selecionadora";
                    textoServicoDestaque = "<p>Um espaço construído especialmente para atender as necessidades de um processo seletivo. Você selecionadora contará com anúncio de vagas, rastreador de currículos, R1, meu plano e exclusivo banco de currículos.</p>";
                    HtmlServicoDestaque = "<i class='fa fa-key ico-gg'></i>";
                    pnlServicoSalaSelecionadora.Visible = false;
                    break;
                //Site Trabalhe Conosco Grátis
                case Enumeradores.LoginEmpresaDestino.STC:
                    nomeServicoDestaque = "Exclusivo Banco de Currículos";
                    textoServicoDestaque = "<p>Adquira gratuitamente um endereço na web exclusivo e personalizado para sua empresa, assim os candidatos interessados em suas vagas poderão cadastrar o currículo e se candidatar às suas vagas.</p>";
                    HtmlServicoDestaque = "<i class='fa fa-database ico-gg'></i>";
                    pnlServicoSiteTrabalheConosco.Visible = false;
                    break;
                //Anunciar Vagas - Default
                //case "AnunciarVaga.aspx":
                default:
                    nomeServicoDestaque = "Compre CVs";
                    textoServicoDestaque = "<p>Acesse mais de 3 milhões de currículos sem limite com a facilidade de diversos filtros, economia na comunicação por SMS e recrutamento em um dia útil. Não perca mais dinheiro com sites de retorno duvidoso.</p>";
                    HtmlServicoDestaque = "<p class='fa-stack ico-gg'><i class='fa fa-file-o fa-stack-2x'></i><i class='fa fa-usd fa-stack-1x'></i></p>";
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
            hlCompreCV.NavigateUrl = GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.CompreCV });
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
                    return GetRouteUrl(Enumeradores.RouteCollection.CadastroEmpresaDados.ToString(), null);
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
            }
            return string.Empty;
        }
        #endregion

        #endregion

    }
}