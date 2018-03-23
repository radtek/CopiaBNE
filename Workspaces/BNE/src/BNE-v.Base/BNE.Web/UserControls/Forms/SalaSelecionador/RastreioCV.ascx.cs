using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.SalaSelecionador
{

    public partial class RastreioCV : BaseUserControl
    {

        #region Propriedades

        #region IdRastreador - Variavel1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdRastreador
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
                else
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

        #region PageIndex - PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
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

            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;

            Ajax.Utility.RegisterTypeForAjax(typeof(RastreioCV));
        }
        #endregion

        #region gvRastreador_ItemCommand
        protected void gvRastreador_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("VisualizarCurriculo"))
                {
                    base.UrlDestino.Value = "SalaSelecionadorRastreadorCV.aspx";
                    int idRastreador = Convert.ToInt32(gvRastreador.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Rastreador"]);

                    Session.Add(Chave.Temporaria.Variavel8.ToString(), idRastreador);
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                }
                else if (e.CommandName.Equals("EditarRastreador"))
                {
                    int idRastreador = Convert.ToInt32(gvRastreador.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Rastreador"]);

                    Session.Add(Chave.Temporaria.Variavel1.ToString(), idRastreador);
                    Redirect("SalaSelecionadorCadastroRastreadorCV.aspx");
                }
                else if (e.CommandName.Equals("ExcluirRastreador"))
                {
                    int idRastreador = Convert.ToInt32(gvRastreador.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Rastreador"]);

                    IdRastreador = idRastreador;
                    ucConfirmacaoExclusao.Inicializar("Atenção!", "Tem certeza que deseja excluir este registro?!");
                    ucConfirmacaoExclusao.MostrarModal();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region ucConfirmacaoExclusao_Confirmar
        void ucConfirmacaoExclusao_Confirmar()
        {
            ExcluirRastreador(IdRastreador.Value);
            CarregarGrid();
            upGvRastreador.Update();
        }
        #endregion

        #region ExcluirRastreador
        private void ExcluirRastreador(int idRastreador)
        {
            Rastreador objRastreador = Rastreador.LoadObject(idRastreador);
            objRastreador.FlagInativo = true;
            objRastreador.Save();
        }
        #endregion

        #region gvRastreador_PageIndexChanged
        protected void gvRastreador_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (base.STC.Value)
                Redirect("SiteTrabalheConoscoMenu.aspx");
            else
                Redirect("SalaSelecionador.aspx");
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {


            //Carregando a quantidade de itens a ser mostrado em tela
            int quantidadeitens = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaSalaSelecionador, null));
            gvRastreador.PageSize = quantidadeitens;
            PageIndex = 1;
            CarregarGrid();
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros;
            UIHelper.CarregarRadGrid(gvRastreador, Rastreador.ListarRastreadorFilialDT(base.IdFilial.Value, PageIndex, gvRastreador.PageSize, out totalRegistros), totalRegistros);
        }
        #endregion

        #endregion

    }
}