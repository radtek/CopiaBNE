using System;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class Empresas : BaseUserControl
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            gvEmpresas.PageSize = 6;
            if (!IsPostBack)
                CarregarGrid();

            ucBloquearCandidato.Fechar += ucBloquearCandidato_Fechar;
        }
        #endregion

        #region ucBloquearCandidato_Fechar
        private void ucBloquearCandidato_Fechar(string Mensagem)
        {
            ucBloquearCandidato.EsconderModal();
            ExibirMensagemConfirmacao("Confirmação", Mensagem, false);
            CarregarGrid();
            upGvEmpresas.Update();
        }
        #endregion

        #region CarregarGrid
        public void CarregarGrid()
        {
            int totalRegistros;
            UIHelper.CarregarRadGrid(gvEmpresas, Filial.SalaAdministradoraListar(string.IsNullOrEmpty(tbxFiltroBusca.Text), tbxFiltroBusca.Text, gvEmpresas.CurrentPageIndex, gvEmpresas.PageSize, out totalRegistros), totalRegistros);
        }
        #endregion

        #region Eventos

        #region gvEmpresas_PageIndexChanged
        protected void gvEmpresas_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvEmpresas.CurrentPageIndex = e.NewPageIndex;
            CarregarGrid();
        }
        #endregion

        #region btnFiltrar_Click
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            gvEmpresas.CurrentPageIndex = 0;
            CarregarGrid();
        }
        #endregion

        #region gvEmpresas_ItemCommand
        protected void gvEmpresas_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("EditarEmpresa"))
            {
                var idFilial = Convert.ToInt32(gvEmpresas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);
                Session.Add(Chave.Temporaria.Variavel1.ToString(), idFilial);

                var idPessoaFisica = UsuarioFilialPerfil.RecuperarIdentificadorPessoaFisicaPrimeiroMaster(idFilial);
                if (idPessoaFisica.HasValue)
                    Session.Add(Chave.Temporaria.Variavel2.ToString(), idPessoaFisica.Value);

                Redirect(GetRouteUrl(RouteCollection.AtualizarDadosEmpresa.ToString(), null));
            }
            else if (e.CommandName.Equals("VisualisaCurriculos"))
            {
                var idEmpresa = Convert.ToInt32(gvEmpresas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);
                Session.Add(Chave.Temporaria.Variavel6.ToString(), idEmpresa);
                Redirect("SalaAdministradorVisualizadoCV.aspx");
            }
            else if (e.CommandName.Equals("SiteTrabalheConosco"))
            {
                var idFilial = Convert.ToInt32(gvEmpresas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);

                IdFilial.Value = idFilial;
                Redirect("~/SiteTrabalheConoscoCriacao.aspx");
            }
            else if (e.CommandName.Equals("VagasAnunciadas"))
            {
                var idFilial = Convert.ToInt32(gvEmpresas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);
                Redirect(Helper.RecuperarURLVagasEmpresa(((Label) e.Item.FindControl("lblNomeEmpresa")).Text, idFilial));
            }
            else if (e.CommandName.Equals("BronquinhaBloquear"))
            {
                ucBloquearCandidato.InicializarBloquear(null, Convert.ToInt32(e.CommandArgument), ((Label) e.Item.FindControl("lblNomeEmpresa")).Text, string.Empty);
                ucBloquearCandidato.MostrarModal();
            }
            else if (e.CommandName.Equals("BronquinhaBloqueado"))
            {
                ucBloquearCandidato.InicializarBloqueado(null, Convert.ToInt32(e.CommandArgument), ((Label) e.Item.FindControl("lblNomeEmpresa")).Text);
                ucBloquearCandidato.MostrarModal();
            }
            else if (e.CommandName.Equals("Usuario"))
            {
                var idFilial = Convert.ToInt32(gvEmpresas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);
                Session.Add(Chave.Temporaria.Variavel1.ToString(), idFilial);
                Redirect("CadastroEmpresaUsuario.aspx");
            }
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect("SalaAdministrador.aspx");
        }
        #endregion

        #endregion
    }
}