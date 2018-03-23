using System;
using BNE.BLL;
using BNE.EL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class EmpresaBloqueadaAceiteContrato : BaseUserControl
    {

        #region Metodos

        #region MostrarModal
        public void MostrarModal()
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                var objFilial = new Filial(base.IdFilial.Value);

                if (/*objUsuarioFilialPerfil.Perfil.IdPerfil == (int)BLL.Enumeradores.Perfil.AcessoEmpresaMaster &&*/ PlanoAdquirido.ExistePlanoAdquiridoPrecisandoAceiteContrato(objUsuarioFilialPerfil))
                {
                    PlanoAdquirido objPlanoAdquirido;                    
                    if (PlanoAdquirido.CarregarPlanoVigentePessoaJuridicaPorSituacao(objFilial, new PlanoSituacao((int)BNE.BLL.Enumeradores.PlanoSituacao.Liberado), out objPlanoAdquirido))
                    {
                        pnlContrato.Visible = true;
                        objPlanoAdquirido.Plano.CompleteObject();
                        if (objPlanoAdquirido.FlagRecorrente || objPlanoAdquirido.Plano.FlagRecorrente)
                            litContrato.Text = objPlanoAdquirido.ContratoPlanoRecorrenteCia(objPlanoAdquirido);
                        else
                            litContrato.Text = objPlanoAdquirido.Contrato();
                    }
                }
                else
                {
                    UIHelper.CarregarRepeater(rptUsuariosMaster, UsuarioFilialPerfil.ListarNomeUsuariosFilial(objFilial, 3));
                    pnlAviso.Visible = true;
                }
            }
            mpeEmpresaBloqueadaAceiteContrato.Show();
        }
        #endregion

        #endregion

        #region Eventos

        #region btnTenteNovamente_Click
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ckbAceitoContrato.Checked)
                {
                    PlanoAdquirido objPlanoAdquirido;
                    if (PlanoAdquirido.CarregarPlanoVigentePessoaJuridicaPorSituacao(new Filial(base.IdFilial.Value), new PlanoSituacao((int)BNE.BLL.Enumeradores.PlanoSituacao.Liberado), out objPlanoAdquirido))
                        PlanoAdquiridoContrato.SalvarAceite(objPlanoAdquirido, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value));
                }
                else
                {
                    base.ExibirMensagem("É necessário que aceite os termos de contrato !", TipoMensagem.Aviso, false);
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region btnOk_Click
        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                BNE.Auth.BNEAutenticacao.DeslogarPadrao();
                LimparSession();
                Redirect("~/");
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #endregion

    }
}