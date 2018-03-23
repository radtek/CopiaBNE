using System;
using System.Web.UI;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls
{
    public partial class ucPaginacao : UserControl
    {

        #region Eventos e Delegates
        public delegate void delegatePaginacao();
        public event delegatePaginacao MudaPagina;
        #endregion

        #region Propriedades
        /// <summary>
        /// Propriedade relativa à propriedade CssClass do pnlUcPaginacao
        /// </summary>
        public string CssClass
        {
            get
            {
                return this.pnlUCPaginacao.CssClass;
            }
            set
            {
                this.pnlUCPaginacao.CssClass = value;
            }
        }
        /// <summary>
        /// Propriedade que indica o total de resultados que seriam retornados.
        /// </summary>
        public int TotalResultados
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel1.ToString()] = value;
            }
        }
        /// <summary>
        /// Propriedade que indica a página do grid que está sendo visualizada.
        /// </summary>
        public int PaginaCorrente
        {
            get
            {
                if (Convert.ToInt32(ViewState[Chave.Temporaria.Variavel2.ToString()]) < 1)
                {
                    ViewState[Chave.Temporaria.Variavel2.ToString()] = 1;
                }
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel2.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel2.ToString()] = value;
            }
        }
        /// <summary>
        /// Indica o total de páginas que o grid contém.
        /// </summary>
        public int TotalPaginas
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel3.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel3.ToString()] = value;
            }
        }
        /// <summary>
        /// Indica o número de itens que devem ser exibidos em uma página do grid.
        /// </summary>
        public int TamanhoPagina
        {
            get
            {
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel4.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel4.ToString()] = value;
            }
        }
        #endregion

        #region Eventos
        #region UserControl
        /// <summary>
        /// Método executado quando o user control é carregado.
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PaginaCorrente = 1;
            }
        }
        #endregion

        #region btiPrimeira
        /// <summary>
        /// Event handler executado quando o botão btiPrimeira é clicado.
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>
        protected void btiPrimeira_Click(object sender, ImageClickEventArgs e)
        {
            PaginaCorrente = 1;
            this.AtualizarPaginacao();
        }
        #endregion

        #region btiAnterior
        /// <summary>
        /// Event handler executado quando o botão btiAnterior é clicado.
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>
        protected void btiAnterior_Click(object sender, ImageClickEventArgs e)
        {
            PaginaCorrente--;
            this.AtualizarPaginacao();
        }
        #endregion

        #region btiProxima
        /// <summary>
        /// Event handler executado quando o botão btiProxima é clicado.
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>
        protected void btiProxima_Click(object sender, ImageClickEventArgs e)
        {
            PaginaCorrente++;
            this.AtualizarPaginacao();
        }
        #endregion

        #region btiUltima
        /// <summary>
        /// Event handler executado quando o botão btiUltima é clicado.
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>
        protected void btiUltima_Click(object sender, ImageClickEventArgs e)
        {
            PaginaCorrente = TotalPaginas;
            this.AtualizarPaginacao();
        }
        #endregion

        #region btnAtualizarPagina
        /// <summary>
        /// Event handler executado quando o botão btnAtualizarPagina é clicado.
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>
        protected void btnAtualizarPagina_Click(object sender, EventArgs e)
        {
            int paginaCorrente = 0;
            if (int.TryParse(txtNovaPagina.Text, out paginaCorrente))
            {
                this.PaginaCorrente = paginaCorrente;
                this.AtualizarPaginacao();
            }
            txtNovaPagina.Text = null;
        }
        #endregion
        #endregion

        #region Métodos
        /// <summary>
        /// Ajusta os botões de navegação pelo Grid, verificando quais devem estar habilidados ou desabilidados.
        /// </summary>
        private void AjustarBotoesPaginacao()
        {
            if (TotalResultados == 0)
            {
                btiPrimeira.Enabled = false;
                btiAnterior.Enabled = false;
                btiProxima.Enabled = false;
                btiUltima.Enabled = false;
                txtNovaPagina.Enabled = false;
                btnAtualizarPagina.Enabled = false;
                return;
            }
            if (PaginaCorrente == 1)
            {
                btiPrimeira.Enabled = false;
                btiAnterior.Enabled = false;
                if (TotalPaginas > 1)
                {
                    txtNovaPagina.Enabled = true;
                    btnAtualizarPagina.Enabled = true;
                    btiProxima.Enabled = true;
                    btiUltima.Enabled = true;
                }
                else
                {
                    txtNovaPagina.Enabled = false;
                    btnAtualizarPagina.Enabled = false;
                    btiProxima.Enabled = false;
                    btiUltima.Enabled = false;
                }
            }
            else if (PaginaCorrente == TotalPaginas)
            {
                txtNovaPagina.Enabled = true;
                btnAtualizarPagina.Enabled = true;
                btiPrimeira.Enabled = true;
                btiAnterior.Enabled = true;
                btiProxima.Enabled = false;
                btiUltima.Enabled = false;
            }
            else
            {
                txtNovaPagina.Enabled = true;
                btnAtualizarPagina.Enabled = true;
                btiPrimeira.Enabled = true;
                btiAnterior.Enabled = true;
                btiProxima.Enabled = true;
                btiUltima.Enabled = true;
            }
        }
        /// <summary>
        /// Método que atualiza a barra de paginação devido a algum evento de navegação pela paginação
        /// ou mudança no estado do grid.
        /// </summary>
        private void AtualizarPaginacao()
        {
            if (this.MudaPagina != null)
            {
                MudaPagina.Invoke();
            }
        }
        /// <summary>
        /// Método responsável por ajustar a barra de paginação para o estado atual do Grid.
        /// </summary>
        public void AjustarPaginacao()
        {
            if (TamanhoPagina != 0)
            {
                this.TotalPaginas = TotalResultados / TamanhoPagina;
                if (TotalResultados % TamanhoPagina != 0)
                {
                    TotalPaginas++;
                }
                //if (this.TotalPaginas > 0)
                //{
                //    this.vdNovaPagina.ValorMinimo = 1;
                //    this.vdNovaPagina.ValorMaximo = this.TotalPaginas;
                //}
                //else
                //{
                //    this.vdNovaPagina.ValorMinimo = 0;
                //    this.vdNovaPagina.ValorMaximo = 0;
                //}

                if (PaginaCorrente > TotalPaginas && !TotalPaginas.Equals(0))
                {
                    PaginaCorrente--;
                    if (MudaPagina != null)
                    {
                        this.MudaPagina.Invoke();
                        return;
                    }
                }

                this.AjustarBotoesPaginacao();

                if (TotalResultados > 0)
                    lblEstadoPaginacao.Text = string.Format("Exibindo página {0} de {1}", this.PaginaCorrente, this.TotalPaginas);
                else
                    lblEstadoPaginacao.Text = string.Format("Exibindo página 0 de 0");

                //if(TotalResultados <= TamanhoPagina)
                //{
                //    pnlUCPaginacao.Visible = false;
                //}
            }
        }

        #region LimparPaginação
        public void LimparPaginacao()
        {
            lblEstadoPaginacao.Text = String.Empty;
            btiPrimeira.Enabled = false;
            btiAnterior.Enabled = false;
            btiProxima.Enabled = false;
            btiUltima.Enabled = false;
            txtNovaPagina.Enabled = false;
            btnAtualizarPagina.Enabled = false;

        }
        #endregion


        #endregion

    }
}