using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;

namespace BNE.Web.UserControls.Forms.Mensagens
{
    public partial class MensagensEnviadas : BaseUserControl
    {
        #region Propriedades
        
        #region DesPesquisa - Variável 1
        /// <summary>
        ///     Propriedade que armazena e recupera o ID
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

        #region TotalRegistros

        private int totalRegistros=0;
        
        #endregion
        #endregion

        #region Delegates

        public delegate void DelegateCorpoMensagensEnviadas(int idMensagemCS, TipoMensagemSala tipoMensagem, string desPesquisa);
        public event DelegateCorpoMensagensEnviadas EventCorpoMensagensEnviadas;

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #endregion

        #region gvMensagensEnviadas_ItemCommand
        protected void gvMensagensEnviadas_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("RowClick") || e.CommandName.Equals("MostrarModal"))
            {
                var idMensagemCS = Convert.ToInt32(gvMensagensEnviadas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Mensagem_CS"].ToString());

                if (idMensagemCS > 0)
                {
                    if (EventCorpoMensagensEnviadas != null)
                        EventCorpoMensagensEnviadas(idMensagemCS, TipoMensagemSala.MensagensEnviadas, DesPesquisa);

                    new MensagemCS(idMensagemCS).Ler();
                }
            }
        }
        #endregion

        #region gvMensagensEnviadas_PageIndexChanged
        protected void gvMensagensEnviadas_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvMensagensEnviadas.CurrentPageIndex = e.NewPageIndex;
            CarregarGrid();
        }
        #endregion

        #region btiPesquisar_Click
        protected void btiPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            DesPesquisa = txtPesquisar.Text;
            gvMensagensEnviadas.CurrentPageIndex = 0;
            CarregarGrid();
            
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            gvMensagensEnviadas.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.QuantidadeItensPorPaginaMensagens));
            CarregarGrid();
        }
        #endregion

        #region CarregarGrid
        public void CarregarGrid()
        {
            UIHelper.CarregarRadGrid(gvMensagensEnviadas, MensagemCS.CarregarMensagensEnviadas(IdUsuarioFilialPerfilLogadoEmpresa.Value, gvMensagensEnviadas.CurrentPageIndex, gvMensagensEnviadas.PageSize, DesPesquisa, out totalRegistros), totalRegistros);
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
        
        #region GetTotalMensagem
        public int GetTotalMensagem()
        {
            return totalRegistros;
        }
        #endregion

        #endregion

    }
}