using System;
using System.Web.UI;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls.Forms.Mensagens
{
    public partial class MensagensRecebidas : BaseUserControl
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

        #region IdUsuarioFilialPerfil - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o IdUsuarioFilialPerfil
        /// </summary>
        public int? IdUsuarioFilialPerfil
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
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

        #region DesPesquisa - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public string DesPesquisa
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel2.ToString()].ToString();
                return string.Empty;
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

        #region EnumMensagemSalaOrigem - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public MensagemSalaOrigem EnumMensagemSalaOrigem
        {
            get
            {
                return (MensagemSalaOrigem)(ViewState[Chave.Temporaria.Variavel3.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
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

        public delegate void delegateCorpoMensagensRecebidas(int idMensagemCS, int pageIndexGridMensagem, TipoMensagemSala tipoMensagem, string desPesquisa);
        public event delegateCorpoMensagensRecebidas EventCorpoMensagensRecebidas;

        #endregion

        #region gvMensagensRecebidas_ItemCommand

        protected void gvMensagensRecebidas_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("RowClick"))
            {
                int idMensagemCS = Convert.ToInt32(gvMensagensRecebidas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Mensagem_CS"].ToString());

                if (EventCorpoMensagensRecebidas != null)
                    EventCorpoMensagensRecebidas(idMensagemCS, PageIndex, TipoMensagemSala.MensagensRecebidas, DesPesquisa);

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

        #region gvMensagensRecebidas_PageIndexChanged

        protected void gvMensagensRecebidas_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
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
                upMensagensRecebidas.Update();
            }
        }

        #endregion

        #endregion

        #region Metodos

        #region Inicializar

        public void Inicializar(int idUsuarioFilialPerfilLogado)
        {
            IdUsuarioFilialPerfil = idUsuarioFilialPerfilLogado;

            gvMensagensRecebidas.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaMensagens));
            PageIndex = 1;
            CarregarGrid();
        }

        #endregion

        #region CarregarGrid
        public void CarregarGrid()
        {
            int totalRegistros;

            bool usuarioCandidato = EnumMensagemSalaOrigem.Equals(MensagemSalaOrigem.SalaVip);

            UIHelper.CarregarRadGrid(gvMensagensRecebidas, MensagemCS.CarregarMensagensRecebidas(IdUsuarioFilialPerfil.Value, PageIndex, gvMensagensRecebidas.PageSize, DesPesquisa, usuarioCandidato, out totalRegistros), totalRegistros);
            upMensagensRecebidas.Update();
        }
        #endregion

        #region CarregarCampo

        public void CarregarCampo()
        {
            txtPesquisar.Text = DesPesquisa;
            upMensagensRecebidas.Update();
        }

        #endregion

        #region LimparCampo

        public void LimparCampo()
        {
            txtPesquisar.Text = string.Empty;
            upMensagensRecebidas.Update();
        }

        #endregion

        #endregion

    }
}