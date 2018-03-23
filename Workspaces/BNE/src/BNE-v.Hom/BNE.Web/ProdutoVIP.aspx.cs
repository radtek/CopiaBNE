using System;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class ProdutoVIP : BasePage
    {

        #region Propriedades

        #region IdVantagem - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa Fisica.
        /// </summary>
        public int IdVantagem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
                return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = (Principal)Page.Master;

            if (master != null)
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;

            if (!IsPostBack)
                Inicializar();
        }
        #endregion

        #region btnContinuar_Click
        protected void btnContinuar_Click(object sender, ImageClickEventArgs e)
        {
            if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                ProcessoCompra();
            else
                ExibirLogin();
        }
        #endregion

        #region master_LoginEfetuadoSucesso
        protected void master_LoginEfetuadoSucesso()
        {
            ProcessoCompra();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            IdVantagem = (int)Enumeradores.VantagensVIP.CandidaturaVagas;

            if (!string.IsNullOrEmpty(Request.QueryString["vantagem"]))
            {
                var vantagem = Request.QueryString["vantagem"];

                Enumeradores.VantagensVIP enumVantagem;
                if (Enum.TryParse(vantagem, true, out enumVantagem))
                {
                    IdVantagem = (int)enumVantagem;
                    ScriptManager.RegisterStartupScript(this, GetType(), "AssociarSeta", "javaScript:associarSeta('" + IdVantagem + "');", true);
                }
            }

            container_vantagem.InnerHtml = GetVantagens(IdVantagem).html;

            hlVagas.Attributes["OnClick"] += "return false;";
            hlVagas.Attributes["data-vantagem"] += (int)Enumeradores.VantagensVIP.CandidaturaVagas;
            hlVagas.NavigateUrl = UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.CandidaturaVagas);

            hlQuemMeViu.Attributes["OnClick"] += "return false;";
            hlQuemMeViu.Attributes["data-vantagem"] += (int)Enumeradores.VantagensVIP.QuemMeViu;
            hlQuemMeViu.NavigateUrl = UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.QuemMeViu);

            hlEscolherEmpresas.Attributes["OnClick"] += "return false;";
            hlEscolherEmpresas.Attributes["data-vantagem"] += (int)Enumeradores.VantagensVIP.EscolherEmpresa;
            hlEscolherEmpresas.NavigateUrl = UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.EscolherEmpresa);

            hlTopoLista.Attributes["OnClick"] += "return false;";
            hlTopoLista.Attributes["data-vantagem"] += (int)Enumeradores.VantagensVIP.TopoListaPesquisas;
            hlTopoLista.NavigateUrl = UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.TopoListaPesquisas);

            hlDadosEmpresa.Attributes["OnClick"] += "return false;";
            hlDadosEmpresa.Attributes["data-vantagem"] += (int)Enumeradores.VantagensVIP.AcessoDadosEmpresas;
            hlDadosEmpresa.NavigateUrl = UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.AcessoDadosEmpresas);

            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, GetType().ToString());
        }
        #endregion

        #endregion

        #region AjaxMethods

        #region GetVantagens
        [WebMethod]
        [ScriptMethod]
        public static HTML GetVantagens(int idVantagem)
        {
            string retorno;

            var vantagem = (Enumeradores.VantagensVIP)idVantagem;
            switch (vantagem)
            {
                case Enumeradores.VantagensVIP.QuemMeViu:
                    retorno = @"<div class='container_imagem quem_me_viu'><div class='texto_quem_me_viu'>Receba em tempo real via<br/> <b>SMS</b> e <b>E-MAIL</b>: <p></p> - As empresas que buscam<br/> &nbsp; currículos como o seu. <br/> - Mensagens de entrevista. <br/> - Lista de todas as empresas<br/> &nbsp;que acabaram de visualizar<br/> &nbsp;seu currículo. <br/> - Toda comunicação que seja<br/> &nbsp;preciso fazer com você.</div></div>";
                    break;
                case Enumeradores.VantagensVIP.EscolherEmpresa:
                    retorno = @"<div class='container_imagem escolher_empresa'><div class='texto_escolher_empresa'>Escolha onde deseja trabalhar, <br/> <b>as melhores empresas</b> estão<br/> no <b>BNE</b>. <br/><br/> <b>Acesso à</b>: endereços, contatos e <br/> quantidade de vagas anunciadas<br/> das empresas.</div></div>";
                    break;
                case Enumeradores.VantagensVIP.TopoListaPesquisas:
                    retorno = @"<div class='container_imagem topo_lista'><div class='texto_topo_lista'>Esteja sempre em <b>EVIDÊNCIA</b>,<br/> no topo da lista dos<br/> pesquisadores. <p></p> Seu nome sempre <b>entre os<br/> primeiros lugares</b> nas buscas<br/> das melhores empresas.<br/></div></div>";
                    break;
                case Enumeradores.VantagensVIP.AcessoDadosEmpresas:
                    retorno = @"<div class='container_imagem acesso_dados'><div class='texto_acesso_dados'>Conheça as empresas que<br/> estão anunciando vagas e/ou que<br/> <b>acabaram de visualizar seu<br/> currículo</b>, assim como,<br/> <b>a quantidade de funcionários</b> e<br/> <b>quantas vagas anunciadas</b>.<br/></div></div>";
                    break;
                default:
                    retorno = @"<div class='container_imagem quero_candidatar'><div class='texto_quero_candidatar'>Você tem acesso livre a todas as<br /> vagas anunciadas<b> por mais de<br /> 80 mil empresas</b> no site do <b>BNE</b>. <br /> <br /> além disto, receba <b>todos os dias</b> <br /> <b>as vagas mais recentes</b> pelo<br /> nosso jornal.</div></div>";
                    break;
            }

            return new HTML
                {
                    html = retorno
                };
        }
        #endregion

        #region ProcessoCompra
        private void ProcessoCompra()
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.EscolhaPlano.ToString(), null));
        }
        #endregion

        #endregion

        #region HTML
        public struct HTML
        {
            public string html { get; set; }
        }
        #endregion

    }
}