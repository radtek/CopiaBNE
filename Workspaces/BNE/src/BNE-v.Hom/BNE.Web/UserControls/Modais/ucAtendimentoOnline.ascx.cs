using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class ucAtendimentoOnline : BaseUserControl
    {

        #region LocalizadoRodape - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public bool LocalizadoRodape
        {
            get
            {
                return (Boolean)(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion


        #region AjustarSOS
        public void AjustarSOS()
        {
            aSOS.Visible = false;
            aSOS.CausesValidation = false;
            aSOSRodape.Visible = false;
            aSOSRodape.CausesValidation = false;

            if (LocalizadoRodape)
                aSOSRodape.Visible = true;
            else
                aSOS.Visible = true;

            var idAtendimento = "265420800";

            if (IdFilial.HasValue)
                idAtendimento = "248659200";

            litImagem.Text = string.Format("<img src=\"http://cloud.aloweb.com.br/img/0.gif\" border=\"0\" alt=\"Atendimento on line\" id=\"image49939200_{0}\" />", idAtendimento);
            litScript.Text = string.Format("<script type=\"text/javascript\" src=\"http://cloud.aloweb.com.br/img/49939200_{0}.js\"></script>", idAtendimento);

            upSOS.Update();
        }
        #endregion

    }
}