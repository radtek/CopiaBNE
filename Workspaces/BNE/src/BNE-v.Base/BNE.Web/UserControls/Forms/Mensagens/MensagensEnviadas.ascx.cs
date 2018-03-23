using System;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.Mensagens
{
    public partial class MensagensEnviadas : BaseUserControl
    {
        #region Propriedades

        #region PageIndex - PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());
                
                return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

        #region DesPesquisa - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public string DesPesquisa
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel1.ToString()].ToString();
                return string.Empty;
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

        #region Delegates

        public delegate void delegateCorpoMensagensEnviadas(int idMensagemCS, int pageIndexGridMensagem, TipoMensagemSala tipoMensagem, string desPesquisa);
        public event delegateCorpoMensagensEnviadas EventCorpoMensagensEnviadas;

        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region gvMensagensEnviadas_ItemCommand

        protected void gvMensagensEnviadas_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("RowClick"))
            {
                int idMensagemCS = Convert.ToInt32(gvMensagensEnviadas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Mensagem_CS"].ToString());

                if (EventCorpoMensagensEnviadas != null)
                    EventCorpoMensagensEnviadas(idMensagemCS, PageIndex, TipoMensagemSala.MensagensEnviadas, DesPesquisa);

                //Atualiza Flag Mensagem Lida
                var objMensagemCS = new MensagemCS();
                objMensagemCS = MensagemCS.LoadObject(idMensagemCS);
                if (objMensagemCS.FlagLido == false)
                {
                    objMensagemCS.FlagLido = true;
                    objMensagemCS.Save();
                }   
            }
        }

        #endregion

        #region gvMensagensEnviadas_PageIndexChanged

        protected void gvMensagensEnviadas_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }

        #endregion

        #region btiPesquisar_Click

        protected void btiPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPesquisar.Text))
            {
                DesPesquisa = txtPesquisar.Text;
                CarregarGrid();
            }
        }

        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            gvMensagensEnviadas.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaMensagens));
            PageIndex = 1;
            CarregarGrid();
        }
        #endregion

        #region CarregarGrid
        public void CarregarGrid()
        {
            int totalRegistros;

            UIHelper.CarregarRadGrid(gvMensagensEnviadas, MensagemCS.CarregarMensagensEnviadas(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, PageIndex, gvMensagensEnviadas.PageSize, DesPesquisa, out totalRegistros), totalRegistros);
            upMensagensEnviadas.Update();
        }
        #endregion

        #region LimparCampo

        public void LimparCampo()
        {
            txtPesquisar.Text = string.Empty;
            upMensagensEnviadas.Update();
        }

        #endregion

        #region CarregarCampo

        public void CarregarCampo()
        {
            txtPesquisar.Text = DesPesquisa;
            upMensagensEnviadas.Update();
        }

        #endregion

        #endregion
    }
}