using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.SalaVip
{
    public partial class JaEnviei : BaseUserControl
    {

        #region Propriedades

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

        }

        #endregion

        #region Delegates

        public delegate void DelegateVerEmpresa(int idFilial,bool FlagConfidencial, int idVaga);
        public event DelegateVerEmpresa EventVerEmpresa;

        public delegate void DelegateImprimir(int idVaga);
        public event DelegateImprimir EventEmprimir;

        #endregion

        #region gvJaEnviei_PageIndexChanged
        protected void gvJaEnviei_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region gvJaEnviei_ItemCommand

        protected void gvJaEnviei_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("VerEmpresa"))
            {
                int idFilial = Convert.ToInt32(gvJaEnviei.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);
                int idVaga = Convert.ToInt32(gvJaEnviei.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);
                bool FlagConfidencial = Convert.ToBoolean(gvJaEnviei.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Img_Empresa_Confidencial_Visible"]);

                if (EventVerEmpresa != null)
                    EventVerEmpresa(idFilial, FlagConfidencial, idVaga);
            }
            else if (e.CommandName.Equals("VerVaga"))
            {
                int idVaga = Convert.ToInt32(gvJaEnviei.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);
                Redirect(Vaga.MontarUrlVaga(idVaga));
            }
            else if (e.CommandName.Equals("Imprimir"))
            {
                int idVaga = Convert.ToInt32(gvJaEnviei.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                if (EventVerEmpresa != null)
                    EventEmprimir(idVaga);
            }
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
            //Carregando a quantidade de itens a ser mostrado em tela
            int quantidadeitens = 6;
            gvJaEnviei.PageSize = quantidadeitens;
            PageIndex = 1;
            CarregarGrid();

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri.ToString();
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros = 8;
            UIHelper.CarregarRadGrid(gvJaEnviei, VagaCandidato.CarregarVagaCandidatadaPorCurriculo(base.IdCurriculo.Value, PageIndex, gvJaEnviei.PageSize, out totalRegistros), totalRegistros);
            upGvJaEnviei.Update();
        }

        #endregion               

        #endregion

    }
}