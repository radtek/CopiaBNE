using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaVipAdquirirMeuPlano : BasePage
    {

        #region Propriedades

        #region UrlOrigem - Variável 1
        /// <summary>
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

        #endregion

        #region Eventos

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), null));
            if (!IsPostBack)
                Inicializar();

            ucModalLogin.Logar += ucModalLogin_Logar;
        }
        #endregion

        #region ucModalLogin_Logar
        void ucModalLogin_Logar(string urlDestino)
        {
            if (base.IdCurriculo.HasValue)
            {
                AjustarPlano();
            }
            else
            {
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
            }
        }
        #endregion

        #region btnContinuar_Click
        protected void btnContinuar_Click(object sender, ImageClickEventArgs e)
        {
            Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.CandidaturaVagas));
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UrlOrigem))
                Redirect(UrlOrigem);
            else
                Redirect("Default.aspx");
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "SalaVipMeuPlano");
            AjustarTituloTela("Meu Plano");

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;

            if (base.IdCurriculo.HasValue)
                AjustarPlano();
            else
            {
                ucModalLogin.Inicializar();
                ucModalLogin.Mostrar();
            }
        }
        #endregion

        #region AjustarPlano
        private void AjustarPlano()
        {
            if (!base.IdCurriculo.HasValue) return;

            var objFuncaoCategoria = FuncaoCategoria.RecuperarCategoriaPorCurriculo(new Curriculo(base.IdCurriculo.Value));

            int idPlanoMensal;
            OrigemFilial objOrigemFilial = null;
            if (base.STC.Value && OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial) && (objOrigemFilial != null && objOrigemFilial.Filial.PossuiSTCUniversitario()))
                idPlanoMensal = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPUniversitarioMensal));
            else
            {
                idPlanoMensal = Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(objFuncaoCategoria);

                decimal mensalSemDesconto = Decimal.Zero;

                List<Enumeradores.Parametro> listaParametros;
                Dictionary<Enumeradores.Parametro, string> parametros;
                switch (objFuncaoCategoria.IdFuncaoCategoria)
                {
                    case (int)Enumeradores.FuncaoCategoria.Apoio:
                        listaParametros = new List<Enumeradores.Parametro>
                        {
                            Enumeradores.Parametro.ValorSemDescontoVIPApoioMensal,
                        };
                        parametros = Parametro.ListarParametros(listaParametros);

                        mensalSemDesconto = Convert.ToDecimal(parametros[Enumeradores.Parametro.ValorSemDescontoVIPApoioMensal]);
                        break;
                    case (int)Enumeradores.FuncaoCategoria.Operacao:
                        listaParametros = new List<Enumeradores.Parametro>
                        {
                            Enumeradores.Parametro.ValorSemDescontoVIPOperacaoMensal,
                        };
                        parametros = Parametro.ListarParametros(listaParametros);

                        mensalSemDesconto = Convert.ToDecimal(parametros[Enumeradores.Parametro.ValorSemDescontoVIPOperacaoMensal]);
                        break;
                    case (int)Enumeradores.FuncaoCategoria.Especialista:
                        listaParametros = new List<Enumeradores.Parametro>
                        {
                            Enumeradores.Parametro.ValorSemDescontoVIPEspecialistaMensal,
                        };
                        parametros = Parametro.ListarParametros(listaParametros);

                        mensalSemDesconto = Convert.ToDecimal(parametros[Enumeradores.Parametro.ValorSemDescontoVIPEspecialistaMensal]);
                        break;
                    case (int)Enumeradores.FuncaoCategoria.Gestao:
                        listaParametros = new List<Enumeradores.Parametro>
                        {
                            Enumeradores.Parametro.ValorSemDescontoVIPGestaoMensal,
                        };
                        parametros = Parametro.ListarParametros(listaParametros);

                        mensalSemDesconto = Convert.ToDecimal(parametros[Enumeradores.Parametro.ValorSemDescontoVIPGestaoMensal]);
                        break;
                }

                litPrecoSemDescontoMensal.Text = mensalSemDesconto.ToString(CultureInfo.CurrentCulture);
            }

            litPrecoDescontoMensal.Text = new Plano(idPlanoMensal).RecuperarValor().ToString(CultureInfo.CurrentCulture);

            upBtiEuQuero.Update();
        }
        #endregion

        #endregion

    }
}
