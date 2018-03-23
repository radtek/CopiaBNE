using System;
using System.Globalization;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Code.Session;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class FormaPagamento : BasePage
    {

        #region Propriedades

        #region IdConteudoHTMLFormaPagamento
        /// <summary>
        /// Propriedade que armazena e recupera o IdConteudoHTML
        /// </summary>
        protected int? IdConteudoHTMLFormaPagamento
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdConteudoHTML.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdConteudoHTML.ToString()].ToString());

                if (Session[Chave.Temporaria.IdConteudoHTML.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdConteudoHTML.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdConteudoHTML.ToString(), value);
                Session.Add(Chave.Temporaria.IdConteudoHTML.ToString(), value);
            }
        }
        #endregion

        #region IdPlanoFormaPagamento
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        protected int? IdPlanoFormaPagamento
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlano.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlano.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlano.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlano.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlano.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlano.ToString(), value);
            }
        }
        #endregion

        #region IdPlanoNormalFormaPagamento
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano Extendido
        /// </summary>
        protected int? IdPlanoNormalFormaPagamento
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlanoNormal.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlanoNormal.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlanoNormal.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlanoNormal.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlanoNormal.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlanoNormal.ToString(), value);
            }
        }
        #endregion

        #region IdPlanoExtendidoFormaPagamento
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano Extendido
        /// </summary>
        protected int? IdPlanoExtendidoFormaPagamento
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlanoExtendido.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlanoExtendido.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlanoExtendido.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlanoExtendido.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlanoExtendido.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlanoExtendido.ToString(), value);
            }
        }
        #endregion

        #region CodigoVaga
        /// <summary>
        /// Propriedade que armazena e recupera o CodigoVaga
        /// </summary>
        protected string CodigoVaga
        {
            get
            {
                if (ViewState[Chave.Temporaria.CodigoVaga.ToString()] != null)
                    return ViewState[Chave.Temporaria.CodigoVaga.ToString()].ToString();

                if (Session[Chave.Temporaria.CodigoVaga.ToString()] != null)
                    return Session[Chave.Temporaria.CodigoVaga.ToString()].ToString();

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.CodigoVaga.ToString(), value);
                Session.Add(Chave.Temporaria.CodigoVaga.ToString(), value);
            }
        }
        #endregion

        #region UrlOrigemFormaPagamento
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        public string UrlOrigemFormaPagamento
        {
            get
            {
                if (Session["UrlOrigem"] != null)
                    return Session["UrlOrigem"].ToString();
                return null;
            }
            set
            {
                if (value != null)
                    Session.Add("UrlOrigem", value);
                else
                    Session.Remove("UrlOrigem");
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

            if (!base.STC.Value || (base.STC.Value && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue))
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "FormaPagamento");
            else
                InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "FormaPagamento");

            Ajax.Utility.RegisterTypeForAjax(typeof(FormaPagamento));
        }
        #endregion

        #region BtnVoltarClick
        protected void BtnVoltarClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UrlOrigemFormaPagamento) && !UrlOrigemFormaPagamento.Contains("DepositoBancario.aspx") && !UrlOrigemFormaPagamento.Contains("BoletoBancario.aspx"))
                Redirect(UrlOrigemFormaPagamento);
            else
                Redirect("Default.aspx");
        }
        #endregion

        #region RblPlanoCheckedChanged
        protected void RblPlanoCheckedChanged(object sender, EventArgs e)
        {
            AjustarPlanoSelecionado();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarURLOrigem();

            if (IdPlanoFormaPagamento.HasValue)
            {
                Plano objPlano = Plano.LoadObject(IdPlanoFormaPagamento.Value);

                ucFormaPagamento.IdConteudoHTMLFormaPagamento = IdConteudoHTMLFormaPagamento;
                ucFormaPagamento.IdPlanoExtendidoFormaPagamento = IdPlanoExtendidoFormaPagamento;
                ucFormaPagamento.IdPlanoNormal = IdPlanoNormalFormaPagamento;

                if (!IdPlanoNormalFormaPagamento.HasValue)
                    IdPlanoNormalFormaPagamento = IdPlanoFormaPagamento;

                if (objPlano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
                {
                    //rblPlano1.Checked = true;

                    //AjustarPlanoSelecionado();

                    if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                    {
                        UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoCandidato.Value);

                        if (!PlanoAdquirido.ExistePlanoAdquirido(base.IdUsuarioFilialPerfilLogadoCandidato.Value))
                            PlanoAdquirido.CriarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, objPlano);
                    }

                }
                else if (objPlano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaJuridica)
                {
                    UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                    Filial objFilial = Filial.LoadObject(base.IdFilial.Value);

                    if (!PlanoAdquirido.ExistePlanoAdquiridoPorFilial(objFilial))
                        PlanoAdquirido.CriarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, objPlano);

                    ucFormaPagamento.IdPlano = IdPlanoFormaPagamento;
                }

                ucFormaPagamento.Inicializar();
            }

            PreencherCampos();

            AjustarTituloTela("Pagamento");
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            ConteudoHTML objConteudoHTML = ConteudoHTML.LoadObject((int)IdConteudoHTMLFormaPagamento);
            Plano objPlano = Plano.LoadObject((int)IdPlanoNormalFormaPagamento);

            if (objPlano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
            {
                var objFuncaoCategoria = FuncaoCategoria.RecuperarCategoriaPorCurriculo(new Curriculo(base.IdCurriculo.Value));
                imgCampanha.Visible = true;
                switch (objFuncaoCategoria.IdFuncaoCategoria)
                {
                    case (int)Enumeradores.FuncaoCategoria.Apoio:
                    case (int)Enumeradores.FuncaoCategoria.Operacao:
                        imgCampanha.ImageUrl = "/img/campanha/pagamento/pagamento_apoio_operacional.png";
                        break;
                    case (int)Enumeradores.FuncaoCategoria.Especialista:
                        imgCampanha.ImageUrl = "/img/campanha/pagamento/pagamento_especialista.png";
                        break;
                    case (int)Enumeradores.FuncaoCategoria.Gestao:
                        imgCampanha.ImageUrl = "/img/campanha/pagamento/pagamento_gestao.png";
                        break;
                }

                //lblTextoVantagens.Text = objConteudoHTML.ValorConteudo;
                litTextoVantagens.Text = objConteudoHTML.ValorConteudo;


                pnlOpcoesPlanoVIP.Visible = true;

                //Plano objPlanoExtendido = Plano.LoadObject((int)IdPlanoExtendidoFormaPagamento);
                //rblPlano1.Text = string.Format("{0} ({1})", objPlano.DescricaoPlano, objPlano.ValorBase.ToString("C"));
                //rblPlano2.Text = string.Format("{0} ({1})", objPlanoExtendido.DescricaoPlano, objPlanoExtendido.ValorBase.ToString("C"));

                if (!string.IsNullOrEmpty(CodigoVaga))
                {
                    pnlCandidaturaVagas.Visible = true;

                    litNome.Text = new PessoaFisica(base.IdPessoaFisicaLogada.Value).PrimeiroNome;
                    litProtocolo.Text = Vaga.RetornarProtocolo(base.IdCurriculo.Value, CodigoVaga);

                    //Mensagem complementar
                    litMensagemPagamento.Visible = true;
                }
            }
            else
            {
                //lblTextoVantagens.Text = string.Format(objConteudoHTML.ValorConteudo, objPlano.ValorBase.ToString(CultureInfo.CurrentCulture));
                litTextoVantagens.Text = string.Format(objConteudoHTML.ValorConteudo, objPlano.ValorBase.ToString(CultureInfo.CurrentCulture));
            }
        }
        #endregion

        #region AjustarURLOrigem
        /// <summary>
        /// Método responsável por identificar a url que chamou a página atual.
        /// </summary>
        private void AjustarURLOrigem()
        {
            if (Request.UrlReferrer != null)
            {
                if (string.IsNullOrEmpty(UrlOrigemFormaPagamento))
                    UrlOrigemFormaPagamento = Request.UrlReferrer.AbsoluteUri;
            }
        }
        #endregion

        #region AjustarPlanoSelecionado
        private void AjustarPlanoSelecionado()
        {
            ucFormaPagamento.IdPlano = IdPlanoFormaPagamento = rblPlano1.Checked ? IdPlanoNormalFormaPagamento : IdPlanoExtendidoFormaPagamento;
        }
        #endregion

        #endregion

    }
}