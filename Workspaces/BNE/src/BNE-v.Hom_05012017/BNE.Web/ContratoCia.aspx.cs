using System;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web
{
    public partial class ContratoCia : Page
    {
        #region Propriedades

        #region IdPlanoAdquirido
        /// <summary>
        ///     Propriedade que armazena e recupera o valor pago pelo plano
        /// </summary>
        public int? IdPlanoAdquirido
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return int.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());

                return null;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Inicializar();
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            if (IdPlanoAdquirido.HasValue)
                litContrato.Text = PlanoAdquirido.LoadObject(IdPlanoAdquirido.Value).Contrato();
        }
        #endregion

        #endregion
    }
}