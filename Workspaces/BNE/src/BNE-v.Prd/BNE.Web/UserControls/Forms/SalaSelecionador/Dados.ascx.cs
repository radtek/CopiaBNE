using System;
using BNE.BLL;
using BNE.Web.Code;

namespace BNE.Web.UserControls.Forms.SalaSelecionador
{
    public partial class Dados : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Inicializar();
                hlAtualizarEmpresa.NavigateUrl = GetRouteUrl(BLL.Enumeradores.RouteCollection.AtualizarDadosEmpresa.ToString(), null);
            }
        }

        #region Métodos

        #region Inicializar
        /// <summary>
        /// Método utilizado para para preenchimento de componentes, funções de foco e navegação
        /// </summary>
        private void Inicializar()
        {
            PreencherCampos();
        }
        #endregion

        #region PreencherCampos
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        private void PreencherCampos()
        {
            if (base.IdFilial.HasValue && base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue && base.IdPessoaFisicaLogada.HasValue)
            {
                var dtoFilial = Filial.CarregarDTO(base.IdFilial.Value);

                UCLogoFilial1.Cnpj = dtoFilial.NumeroCNPJ;
                lblNomeEmpresaValor.Text = dtoFilial.NomeFantasia;
            }
        }
        #endregion

        #endregion

    }
}