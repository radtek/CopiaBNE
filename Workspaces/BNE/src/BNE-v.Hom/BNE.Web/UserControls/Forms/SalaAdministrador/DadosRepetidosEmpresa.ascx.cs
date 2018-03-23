using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Web.UI;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class DadosRepetidosEmpresa : BaseUserControl
    {

        #region Propriedades

        #region PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());

                return 1;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

        #region IdFilial
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int IdFilial
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void delegateFilialSelecionada(int idFilial);
        public event delegateFilialSelecionada FilialSelecionada;
        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {}
        #endregion

        #region gvDadosRepetidos_ItemCommand
        protected void gvDadosRepetidos_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Selecionar"))
            {
                int idFilial = Convert.ToInt32(gvDadosRepetidos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);

                if (FilialSelecionada != null)
                    FilialSelecionada(idFilial);

                FecharModal();
            }
        }
        #endregion

        #region gvDadosRepetidos_PageIndexChanged
        protected void gvDadosRepetidos_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, ImageClickEventArgs e)
        {
            FecharModal();
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            //Setando propriedades da radgrid
            gvDadosRepetidos.PageSize = 12;
            CarregarGrid();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal()
        {
            mpeDadosRepetidos.Show();
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeDadosRepetidos.Hide();
        }
        #endregion

        #region CarregarGrid
        public void CarregarGrid()
        {
            int totalRegistros;
            UIHelper.CarregarRadGrid(gvDadosRepetidos, Filial.ListarFiliaisDadosRepetidos(IdFilial, PageIndex, gvDadosRepetidos.PageSize, out totalRegistros), totalRegistros);
            upDadosRepetidos.Update();
        }
        #endregion

        #endregion
    }
}