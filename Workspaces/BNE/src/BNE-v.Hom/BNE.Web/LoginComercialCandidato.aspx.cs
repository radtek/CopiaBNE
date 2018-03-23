using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class LoginComercialCandidato : BasePage
    {

        #region Propriedades

        #region UrlOrigem - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        public string UrlOrigem
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

        #region UrlDestino - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        public string UrlDestino
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel2.ToString()]).ToString();
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

        #region MensagemErro - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        public string MensagemErro
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel3.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel3.ToString());
            }
        }
        #endregion

        #region BoolAtualizarCadastroCurriculo - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        public bool BoolAtualizarCadastroCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return Convert.ToBoolean((ViewState[Chave.Temporaria.Variavel4.ToString()]));
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
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
            ucLogin.DestinoBotaoCadastrar = UserControls.Login.DestinoCadastrar.CadastroCurriculo;

            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "LoginComercialCandidato");
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
            //Se o cpf não existe na base de dados, redirecionar para o cadastro de currículo
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                //Se a pessoa tem currículo cadastrado
                if (base.IdCurriculo.HasValue)
                {
                    //Verifica se existe uma UrlDestinoSession Setado para fazer o redirect
                    if (!string.IsNullOrEmpty(UrlDestino))
                    {
                        //Verifica se o destino é a atualizacao de currículo
                        if (UrlDestino.Equals("/cadastro-de-curriculo-gratis") && BoolAtualizarCadastroCurriculo)
                            Session.Add(Chave.Temporaria.Variavel1.ToString(), base.IdPessoaFisicaLogada.Value);

                        Redirect(UrlDestino);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(UrlDestino))
                            Redirect(UrlDestino);
                        else
                            Redirect(urlDestino);
                    }
                }
                else
                {
                    Session.Add(Chave.Temporaria.Variavel3.ToString(), MensagemAviso._24029);
                    Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
                }
            }
            else
                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["UrlDestino"]))
                UrlDestino = Request.QueryString["UrlDestino"];

            #region Serviço em Destaque

            string nomeServicoDestaque;
            string textoServicoDestaque;
            string ConteudoIcone;
            switch (UrlDestino)
            {
                //Quem Me Viu?
                case "/quem-me-viu-vip":
                    nomeServicoDestaque = "Quem me Viu?";
                    textoServicoDestaque = "<p>Conheça em tempo real as empresas que visualizaram seu currículo. Um relatório completo com nome da empresa, dia e hora em que seu currículo foi acessado.</p>";
                    ConteudoIcone = "<i class=\"fa fa-eye ico-gg\"></i>"; //"~/img/login/icone_grande_quem_me_viu.png"; Dougras, trocar aqui
                    pnlServicoQuemMeViu.Visible = false;
                    break;
                //Sala VIP
                case "/sala-vip":
                    nomeServicoDestaque = "Sala VIP";
                    textoServicoDestaque = "<p>Um espaço construído especialmente para atender as suas necessidades de colocação no mercado de trabalho.  Você contará com rastreador de vagas, quem me viu, escolher empresa, mensagens, já enviei e meu plano.</p>";
                    ConteudoIcone = "<i class=\"fa fa-key ico-gg\"></i>"; //"~/img/login/icone_grande_sala_vip.png"; Dougras, trocar aqui
                    pnlServicoSalaVIP.Visible = false;
                    break;
                //Atualizar Currículo
                case "/cadastro-de-curriculo-gratis":
                    nomeServicoDestaque = "Atualizar Currículo";
                    textoServicoDestaque = "<p>Mais de 600 pessoas por dia são chamadas para entrevista através de SMS pelo BNE. Mantenha o seu currículo atualizado e faça parte desta estatística.</p>";
                    ConteudoIcone = "<span class='fa-stack fa-lg ico-gg'><i class='fa fa-file-o fa-stack-2x'></i><i class='fa fa-refresh fa-stack-1x'></i></span>"; //"~/img/login/icone_grande_atualizar_curriculo.png"; Dougras, trocar aqui
                    pnlServicoAtualizarCurriculo.Visible = false;
                    break;
                //VIP - Default
                default:
                    nomeServicoDestaque = "Atendimento Diferenciado";
                    textoServicoDestaque = "<p>Com o serviço VIP seu currículo sempre estará no topo da lista, sendo um candidato especial você terá preferência na visualização das empresas. Seja você também um fura fila!</p>";
                    ConteudoIcone = "<i class=\"fa fa-key ico-gg\"></i>"; //"~/img/login/icone_grande_vip.png"; Dougras, trocar aqui
                    pnlServicoVIP.Visible = false;
                    break;
            }

            //Define as informações do serviço em destaque
            
            litIconeGrandeServicoDestaque.Text = ConteudoIcone;
            litNomeServicoDestaque.Text = nomeServicoDestaque;
            litDescricaoServicoDestaque.Text = textoServicoDestaque;

            #endregion

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;


            //Quando o usuário candidato tentar se logar pelo Login Comercial Empresa, irá mostra a mensagem abaixo
            if (!string.IsNullOrEmpty(MensagemErro))
                ExibirMensagem(MensagemErro, TipoMensagem.Aviso);

            ucLogin.Inicializar();
        }
        #endregion

        #endregion

    }
}