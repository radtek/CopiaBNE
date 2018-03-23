using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Web.Caching;

namespace BNE.Web.UserControls
{
    public partial class ucEstatistica : System.Web.UI.UserControl
    {

        #region Propriedades

        #region Estatistica - Cache
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected Estatistica Estatistica
        {
            get
            {
                if (Cache["Estatistica"] != null)
                    return (Estatistica)Cache["Estatistica"];
                return null;
            }
            set
            {
                if (value != null)
                {
                    DateTime dataAtualizar = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 05, 00).AddDays(1);
                    Cache.Insert("Estatistica",value, null, dataAtualizar, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                AjustarEstatistica();
        }
        #endregion

        #endregion

        #region Métodos

        #region AjustarEstatistica
        public void AjustarEstatistica()
        {
            Estatistica objEstatistica = RetornarEstatistica();

            decimal totalCurriculo = objEstatistica.QuantidadeCurriculo;
            decimal totalParametroContador = Convert.ToDecimal(Parametro.RecuperaValorParametro(Enumeradores.Parametro.ContadorCurriculo));
            decimal total = totalCurriculo + totalParametroContador;
            lblNumeroCurriculos.Text = total.ToString("N0");
            lblNumeroVagas.Text = Convert.ToDecimal(objEstatistica.QuantidadeVaga).ToString("N0");
            lblNumeroEmpresa.Text = Convert.ToDecimal(objEstatistica.QuantidadeEmpresa).ToString("N0");
        }
        #endregion

        #region RetornarEstatistica
        private Estatistica RetornarEstatistica()
        {
            if (Estatistica == null)
                Estatistica = Estatistica.RecuperarEstatistica();

            return Estatistica;
        }
        #endregion

        #endregion
        
    }
}