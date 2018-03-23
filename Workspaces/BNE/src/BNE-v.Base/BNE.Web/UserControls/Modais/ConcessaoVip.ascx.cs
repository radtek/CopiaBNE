using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using BNE.BLL;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.EL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;

namespace BNE.Web.UserControls.Modais
{
    public partial class ConcessaoVip : BaseUserControl
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

        #region IDPlanoAdquirido - IDPlanoAdquirido
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int IDPlanoAdquirido
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void fechar(string Mensagem);
        public event fechar Fechar;

        #endregion

        #region Eventos

        #region Page_Load
        /// <summary>
        /// Método executado quando a página é carregada
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(ConcessaoVip));
        }
        #endregion

        #region btiFechar_Click

        protected void btiFechar_Click(object sender, EventArgs e)
        {
            mpeConcessaoVip.Hide();
        }

        #endregion

        #region btnLiberar_Click

        protected void btnLiberar_Click(object sender, EventArgs e)
        {

            Curriculo objCurriculo = Curriculo.LoadObject(IDCurriculo);
            if (PlanoAdquirido.ConcederPlanoPF(objCurriculo, Plano.LoadObject(Convert.ToInt32(rcbPlanos.SelectedValue))))
            {

                if (Fechar != null)
                    Fechar("Plano concedido com sucesso");

                //base.ExibirMensagem("Plano concedido com sucesso", TipoMensagem.Aviso);
            }
            else
                base.ExibirMensagem("Erro ao conceder o plano", TipoMensagem.Erro);



        }

        #endregion

        #region btnCancelar_Click

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IDPlanoAdquirido);

            if (objPlanoAdquirido.CancelarPlanoVip())
            {
                if (Fechar != null)
                    Fechar("Plano cancelado com sucesso");
                //base.ExibirMensagem("Plano cancelado com sucesso", TipoMensagem.Aviso);
            }
            else
                base.ExibirMensagem("Erro ao cancelar o plano", TipoMensagem.Erro);


        }

        #endregion

        #endregion

        #region Métodos

        #region MostrarModal
        public void MostrarModal()
        {
            mpeConcessaoVip.Show();
        }
        #endregion

        #region EsconderModal
        public void EsconderModal()
        {
            mpeConcessaoVip.Hide();
            upConcessaoVip.Update();
        }
        #endregion

        #region InicializarPlanoAtual

        public void InicializarPlanoAtual(int idCurriculo, string NomeCandidato,string PlanoAtual,int IdfPlanoAdquirido)
        {
            IDCurriculo = idCurriculo;
            IDPlanoAdquirido = IdfPlanoAdquirido;

            lblNomeCandidatoCancelar.Text = NomeCandidato;
            lblPlanoVIPAtual.Text = PlanoAtual;
            pnlCancelarVIP.Visible = true;
            pnlLiberarVIP.Visible = false;

            upConcessaoVip.Update();
        }

        #endregion

        #region InicializarCandidato

        public void InicializarCandidato(int idCurriculo, string NomeCandidato)
        {
            IDCurriculo = idCurriculo;

            lblNomeCandidatoLiberar.Text = NomeCandidato;
            UIHelper.CarregarRadComboBox(rcbPlanos, Plano.ListarPorTipo(Enumeradores.PlanoTipo.PessoaFisica), "Idf_Plano", "Des_Plano", new RadComboBoxItem("Selecione", "0"));

            pnlCancelarVIP.Visible = false;
            pnlLiberarVIP.Visible = true;
            
            upConcessaoVip.Update();
        }

        #endregion

        #endregion

    }
}