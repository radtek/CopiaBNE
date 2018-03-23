using System;
using BNE.BLL;
using BNE.Web.Code;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.SalaSelecionador
{
    public partial class Dados : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Inicializar();
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

                //TODO: Melhorar código
                PlanoAdquirido objPlanoAdquirido;
                PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(new Filial(base.IdFilial.Value), (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido);

                if (objPlanoAdquirido != null)
                    lblNomeEmpresaValor.Text = String.Format("{0} ({1})", dtoFilial.NomeFantasia, objPlanoAdquirido.Plano.DescricaoPlano);
                else
                    lblNomeEmpresaValor.Text = dtoFilial.NomeFantasia;
            }
        }
        #endregion

        #endregion

    }
}