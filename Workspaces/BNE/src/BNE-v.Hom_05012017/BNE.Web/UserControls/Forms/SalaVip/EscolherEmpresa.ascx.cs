using System;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Web.UI.HtmlControls;

namespace BNE.Web.UserControls.Forms.SalaVip
{
    public partial class EscolherEmpresa : BaseUserControl
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
                
                return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion


        #region IdIndexManipular - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        private int? IdIndexManipular
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

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            ucVerDadosEmpresa.Candidatar += ucVerDadosEmpresa_Candidatar;
            ucModalConfirmacao.ModalConfirmada += ucModalConfirmacao_ModalConfirmada;
            ucIndicarEmpresa.Indicar += ucIndicarEmpresa_Indicar;
        }
        #endregion

        #region ucModalConfirmacao_ModalConfirmada
        void ucModalConfirmacao_ModalConfirmada()
        {
            if (IdIndexManipular.HasValue)
                AjustarPanelButtonCandidatarSucesso();
        }
        #endregion

        #region ucIndicarEmpresa_Indicar
        void ucIndicarEmpresa_Indicar()
        {
            ucModalConfirmacao.PreencherCampos("Confirmação de Indicação", "Obrigado pela indicação! <br>Nossa equipe entrará em contato com a empresa para inclusão no BNE. ", String.Empty, false);
            ucModalConfirmacao.MostrarModal();
        }
        #endregion

        #region gvEscolherEmpresa_PageIndexChanged
        protected void gvEscolherEmpresa_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGridEscolherEmpresa();
        }
        #endregion

        #region gvEscolherEmpresa_ItemCommand
        protected void gvEscolherEmpresa_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Candidatar"))
            {
                var objCurriculo = new Curriculo(base.IdCurriculo.Value);
                
                if (objCurriculo.VIP())
                {
                    int idFilial = Convert.ToInt32(gvEscolherEmpresa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);
                    IdIndexManipular = e.Item.ItemIndex;

                    ucVerDadosEmpresa.IdFilial = idFilial;
                    ucVerDadosEmpresa.MostrarModal();
                }
                else
                    Redirect(UIHelper.RecuperarCaminhoProdutoVIP(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null), Enumeradores.VantagensVIP.EscolherEmpresa));
            }
            else if (e.CommandName.Equals("Bloquear"))
            {
                var idFilial = gvEscolherEmpresa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"];
                btnConfirmar.CommandArgument = idFilial.ToString();
                lblNomeEmpresa.Text = gvEscolherEmpresa.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Nme_Empresa"].ToString();
                pnlConfirmarBloqueio.Visible = true;
                pnlconfirmarDesbloqueio.Visible = false;
                upConfirmacaoExclusao.Update();
                mpeConfirmacaoExclusao.Show();
            }
        }
        #endregion

        #region btnFiltrar_Click
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            gvEscolherEmpresa.CurrentPageIndex = 0;
            CarregarGridEscolherEmpresa();
        }
        #endregion

        #region ucVerDadosEmpresa_Candidatar
        void ucVerDadosEmpresa_Candidatar()
        {
            try
            {
                IntencaoFilial objIntencaoFilial;
                if (!IntencaoFilial.CarregarPorFilialCurriculo(base.IdCurriculo.Value, ucVerDadosEmpresa.IdFilial, out objIntencaoFilial))
                {
                    objIntencaoFilial = new IntencaoFilial
                        {
                            Curriculo = new Curriculo(base.IdCurriculo.Value), 
                            Filial = new Filial(ucVerDadosEmpresa.IdFilial)
                        };
                }
                objIntencaoFilial.FlagInativo = false;
                objIntencaoFilial.Save();
                ucVerDadosEmpresa.FecharModal();
                ucModalConfirmacao.PreencherCampos("Confirmação de Envio", "Notificação enviada com sucesso!", "Havendo interesse a própria empresa fará<br /> contato com você, sem intermediários.", false);
                ucModalConfirmacao.MostrarModal();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
            CarregarGridEscolherEmpresa();
            upGvEscolherEmpresa.Update();
        }
        #endregion

        #region btnNaoAcheiEmpresa_Click
        protected void btnNaoAcheiEmpresa_Click(object sender, EventArgs e)
        {
            ucIndicarEmpresa.MostrarModal();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            PageIndex = 1;
            //Carregando a quantidade de itens a ser mostrado em tela
            gvEscolherEmpresa.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeItensPorPaginaSalaVip));
            UIHelper.CarregarDropDownList(ddlMotivoBloqueio, CurriculoNaoVisivelFilial.ListarMotivoBloqueio(), new ListItem("Selecione", "0"));
            CarregarGridEscolherEmpresa();
            CarregarGridEmpresaBloqueda();
        }
        #endregion

        #region CarregarGrid
        private void CarregarGridEscolherEmpresa()
        {
            if (base.IdCurriculo.HasValue)
            {
                int totalRegistros;
                string busca = tbxFiltroBusca.Text;
                UIHelper.CarregarRadGrid(gvEscolherEmpresa, Filial.ListarFilialPorRazaoRamoCidade(busca, base.IdCurriculo.Value, PageIndex, gvEscolherEmpresa.PageSize, out totalRegistros), totalRegistros);
            }
            else
            {
                base.ExibirLogin();
            }
        }
        #endregion

        #region CarregarGrid
        private void CarregarGridEmpresaBloqueda()
        {
            if (base.IdCurriculo.HasValue)
            {
                UIHelper.CarregarRadGrid(gvEmpresaBloqueada, CurriculoNaoVisivelFilial.ListaFilialBloqueadaUsuario(base.IdCurriculo.Value));
            }
            else
            {
                base.ExibirLogin();
            }
        }
        #endregion

        #region AjustarPanelButtonCandidatarSucesso
        private void AjustarPanelButtonCandidatarSucesso()
        {
            if (IdIndexManipular.HasValue)
            {
                GridDataItem gvDi = gvEscolherEmpresa.Items[IdIndexManipular.Value];

                var ib = (LinkButton)gvDi.FindControl("btiCandidatar");
                ib.Visible = false;
                var i = (HtmlGenericControl)gvDi.FindControl("spanJaEnviei");
                i.Visible = true;
                
                upGvEscolherEmpresa.Update();

                IdIndexManipular = null;
            }
        }
        #endregion

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            int idFilial = Convert.ToInt32(btnConfirmar.CommandArgument);
            CurriculoNaoVisivelFilial objBloquear = new CurriculoNaoVisivelFilial();
            objBloquear.MotivoBloqueio = Convert.ToInt32(ddlMotivoBloqueio.SelectedValue);
            objBloquear.Curriculo = new Curriculo(base.IdCurriculo.Value);
            objBloquear.Filial = new Filial(idFilial);
            if(Convert.ToInt32(ddlMotivoBloqueio.SelectedValue) == (int)Enumeradores.MotivoBloqueio.Outros)
                objBloquear.DescricaoMotivoCurriculoNaoVisivelFilial = txtMotivoBloqueio.Text;

            objBloquear.Save();
            //limpa campo
            txtMotivoBloqueio.Text = string.Empty; ddlMotivoBloqueio.SelectedValue = "0";
            mpeConfirmacaoExclusao.Hide();
            CarregarGridEscolherEmpresa();
            CarregarGridEmpresaBloqueda();
            upGvEscolherEmpresa.Update();
            upGvEmpresaBloqueada.Update();
            ucModalConfirmacao.PreencherCampos("", "Bloqueada com sucesso!", false,false);
            ucModalConfirmacao.MostrarModal();
        }
        #endregion

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            mpeConfirmacaoExclusao.Hide();
        }

        protected void gvEmpresaBloqueada_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Desbloquear"))
            {
                lblNomeEmpresaDesbloquear.Text = gvEmpresaBloqueada.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Nme_Empresa"].ToString();
                btnDesbloquear.CommandArgument = gvEmpresaBloqueada.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"].ToString();
                pnlConfirmarBloqueio.Visible = false;
                pnlconfirmarDesbloqueio.Visible = true;
                upConfirmacaoExclusao.Update();
                mpeConfirmacaoExclusao.Show();
            }
        }


        protected void ddlMotivoBloqueio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlMotivoBloqueio.SelectedValue) == (int)Enumeradores.MotivoBloqueio.Outros)
            {
                txtMotivoBloqueio.Visible = true;
                rfvMotivo.Enabled = true;
            }
            else
            {
                txtMotivoBloqueio.Visible = false;
                rfvMotivo.Enabled = false;
            }

        }

        protected void btnDesbloquear_Click(object sender, EventArgs e)
        {
            int idfilial = Convert.ToInt32(btnDesbloquear.CommandArgument);
            CurriculoNaoVisivelFilial.Delete(idfilial, base.IdCurriculo.Value);

            //ATIVAR AS CANDIDADUTAS DA EMPRESA
            CarregarGridEscolherEmpresa();
            CarregarGridEmpresaBloqueda();
            upGvEscolherEmpresa.Update();
            upGvEmpresaBloqueada.Update();
            mpeConfirmacaoExclusao.Hide();
            ucModalConfirmacao.PreencherCampos("", "Desbloqueada com sucesso!",false,false);
            ucModalConfirmacao.MostrarModal();
            
        }
    }
}