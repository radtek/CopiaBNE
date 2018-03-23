using System;
using System.Text;
using BNE.BLL;
using BNE.Web.Code;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class ImprimirProposta : BasePage
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        private void Inicializar()
        {
            if (base.IdFilial.HasValue)
            {
                var sb = new StringBuilder();

                sb.Append(ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ConteudoTelaProposta));

                //Plano objPlano = Plano.LoadObject(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CodigoPlanoEmpresa)));
                //sb.Replace("#ValorProposta#", objPlano.ValorBase.ToString(CultureInfo.CurrentCulture));

                Filial objFilial = Filial.LoadObject(base.IdFilial.Value);
                sb.Replace("#NomeEmpresa#", objFilial.NomeFantasia);

                lblProposta.Text = sb.ToString();
            }
            else
                Redirect("Default.aspx");
        }
        #endregion

        #endregion

    }
}