using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code;
using BNE.BLL;
using BNE.Web.Code.Enumeradores;
using Telerik.Web.UI;

namespace BNE.Web.UserControls.Modais
{
    public partial class ExportarCurriculo : BaseUserControl
    {

        #region Propriedades

        #region IDCurriculo - IDCurriculo
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int IDCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion





        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Delegates
        public delegate void fechar(string Mensagem);
        public event fechar Fechar;

        #endregion

        #region btiFechar_Click

        protected void btiFechar_Click(object sender, EventArgs e)
        {
            mpeExportarCurriculo.Hide();
        }

        #endregion

        #region btnExportarCurriculo_Click
        protected void btnExportarCurriculo_Click(object sender, EventArgs e)
        {

            try
            {
                CurriculoOrigem objCurriculoOrigem = new CurriculoOrigem();

                objCurriculoOrigem.Curriculo = new Curriculo(IDCurriculo);
                objCurriculoOrigem.Origem = new Origem(Convert.ToInt32(rcbExportarCurriculo.SelectedValue));

                objCurriculoOrigem.Save();
                if (Fechar != null)
                    Fechar("Currículo exportado com sucesso!");

            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex, "Erro ao exportar o currículo.");
            } 
        }
        #endregion

        #region EsconderModal
        public void EsconderModal()
        {
            mpeExportarCurriculo.Hide();
            upExportarCurriculo.Update();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal()
        {
            mpeExportarCurriculo.Show();
        }
        #endregion

        #region Inicializar
        public void Inicializar(int idCurriculo, string NomeCurriculo)
        {
            IDCurriculo = idCurriculo;
            lblNomeCandidato.Text = NomeCurriculo;
            UIHelper.CarregarRadComboBox(rcbExportarCurriculo, OrigemFilial.CarregarOrigemNaoVinculadaAoCurriculo(idCurriculo), "Idf_Origem", "Nme_Fantasia", new RadComboBoxItem("Selecione", "0"));
            upExportarCurriculo.Update();
        }
        #endregion
    }
}