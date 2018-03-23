using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.Promocoes.PeixeUrbano
{
    public partial class PeixeUrbano : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region [Eventos]

        #region [btnValidar_Click]
        protected void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                divCodigoInvalido.Visible = false;
                divCodigoUtilizado.Visible = false;
                CodigoDesconto objCodigoDesconto;

                if (!CodigoDesconto.CarregarPorCodigo(txtCodigo.Text, out objCodigoDesconto) 
                    || !objCodigoDesconto.Parceiro.IdParceiro.Equals((int)BNE.BLL.Enumeradores.CodigoDescontoParceiro.PeixeUrbano))
                    divCodigoInvalido.Visible = true;
                else
                {
                    if (objCodigoDesconto.JaUtilizado())
                        divCodigoUtilizado.Visible = true;
                    else
                    {
                        base.PagamentoIdCodigoDesconto.Value = objCodigoDesconto.IdCodigoDesconto;
                        mdConfirmacao.Show();
                    }
                }
                upConteudo.Update();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Validar Codigo Peixe Urbano");
                base.ExibirMensagem("Ocorreu um erro tente novamente mais tarde.", TipoMensagem.Erro);
            }
        }
        #endregion

        #region [btnConcluir_Click]
        protected void btnConcluir_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null) + "?utm_source=bne&utm_medium=peixeurbano&utm_campaign=cupom-peixe-urbano");
        }
        #endregion

        #endregion
       
       

    }
}