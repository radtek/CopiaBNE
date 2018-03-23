using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;

namespace BNE.Web.UserControls.Modais
{
    public partial class ucAssociarCurriculoVaga : BaseUserControl
    {

        #region Propriedades

        #region IdCurriculo - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID do currículo
        /// </summary>
        public int IdCurriculo
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

        #region Eventos

        #region btiAssociar_Click
        protected void btiAssociar_Click(object sender, ImageClickEventArgs e)
        {
            mpeAVC.Show();
            CarregarGrid();
        }
        #endregion

        #region btnAssociar_Click
        protected void btnAssociar_Click(object sender, EventArgs e)
        {
            mpeAVC.Show();
            CarregarGrid();
        }
        #endregion

        #region GvVagas

        #region gvVagas_PageIndexChanged
        protected void gvVagas_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvVagas.CurrentPageIndex = e.NewPageIndex;
            CarregarGrid();
        }
        #endregion

        #region gvVagas_ItemCommand
        protected void gvVagas_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Associar"))
                {
                    int idVaga = Convert.ToInt32(gvVagas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Vaga"]);

                    VagaCandidato.AssociarCurriculoVaga(new Curriculo(IdCurriculo), new Vaga(idVaga));

                    CarregarGrid();

                    ExibirMensagem("Currículo associado com sucesso!", TipoMensagem.Aviso);
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

        #region btnFiltrarVaga_Click
        protected void btnFiltrarVaga_Click(object sender, EventArgs e)
        {
            gvVagas.CurrentPageIndex = 0;
            CarregarGrid();
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar(int idCurriculo)
        {
            IdCurriculo = idCurriculo;
            CarregarComboFuncoes();

            ccFiltrarVagasFuncao.CheckAllItems();

            CarregarGrid();
            mpeAVC.Show();
        }
        #endregion

        #region CarregarGrid
        public void CarregarGrid()
        {
            int totalRegistros;
            DataTable dt = VagaCandidato.ListarVagasFilial(IdCurriculo, base.IdFilial.Value, base.STC.Value ? base.IdOrigem.Value : (int?)null, gvVagas.CurrentPageIndex, gvVagas.PageSize, RecuperarListFuncoes(), out totalRegistros);
            UIHelper.CarregarRadGrid(gvVagas, dt, totalRegistros);

            upGvVagas.Update();
        }
        #endregion

        #region CarregarComboFuncoes
        public void CarregarComboFuncoes()
        {
            UIHelper.CarregarRadComboBox(ccFiltrarVagasFuncao, Vaga.ListarFuncoesVagasFilial(base.IdFilial.Value, true), "Idf_Funcao", "Des_Funcao");

            upStatusFuncao.Update();
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeAVC.Hide();
        }
        #endregion

        #region RecuperarListFuncoes
        private List<int> RecuperarListFuncoes()
        {
            return ccFiltrarVagasFuncao.GetCheckedItems().Select(item => Convert.ToInt32(item.Value)).ToList();
        }
        #endregion

        #endregion

    }

}