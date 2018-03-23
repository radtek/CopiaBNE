using System;
using System.Collections.Generic;
using System.Linq;
using BNE.BLL;
using BNE.EL;
using BNE.Web.Code;
using Parametro = BNE.BLL.Enumeradores.Parametro;
using BNE.Web.Master;

namespace BNE.Web
{
    public partial class CampanhaRecrutamento : BasePage
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            var master = (Principal)Page.Master;

            if (master != null)
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;

        }
        #endregion

        #region btnDispararCampanha_Click
        protected void btnDispararCampanha_Click(object sender, EventArgs e)
        {
            FluxoDispararCampanha();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            var parametro = BLL.Parametro.RecuperaValorParametro(Parametro.CampanhaRecrutamentoQuantidadeCurriculoEscolhaTela);
            var dicionario = parametro.Contains(',') ? parametro.Split(',').ToDictionary(v => v, v => v) : new Dictionary<string, string> { { parametro, parametro } };

            UIHelper.CarregarRadComboBox(rcbQuantidadeCurriculos, dicionario);
            UIHelper.CarregarRadComboBox(rcbTipoRetorno, BLL.TipoRetornoCampanhaRecrutamento.Listar());
            //if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            //{
            //    string telefone = string.Empty;
            //    //Recuperar o telefone da ultima campanha
            //    string ddd = BLL.CampanhaRecrutamento.GetTelefoneUltimaCampanhaEnviada(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out telefone);
            //    if (!String.IsNullOrEmpty(ddd))
            //    {
            //        txtTelefone.DDD = ddd;
            //        txtTelefone.Fone = telefone;
            //        return;
            //    }
            //    UsuarioFilial objUsuarioFilial;
            //    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
            //        objUsuarioFilial.UsuarioFilialPerfil.CompleteObject();
                
            //    txtTelefone.DDD = objUsuarioFilial.NumeroDDDComercial;
            //    txtTelefone.Fone = objUsuarioFilial.NumeroComercial;

            //}
        }
        #endregion

        #region PreencherCamposSaldo
        private void PreencherCamposSaldo()
        {
            if (base.IdFilial.HasValue)
            {
                litSaldo.Text = new Filial(base.IdFilial.Value).SaldoCampanha().ToString();
                pnlSaldo.Visible = true;
            }
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            rcbTipoRetorno.SelectedIndex = rcbQuantidadeCurriculos.SelectedIndex = 0;
            txtFuncao.Text = txtCidade.Text = string.Empty;
            txtSalario.Valor = null;
        }
        #endregion

        #region FluxoDispararCampanha
        private void FluxoDispararCampanha()
        {
            try
            {
                Filial objFilial = null;

                if(base.IdFilial.HasValue){
                    objFilial = new Filial(base.IdFilial.Value);
                }
               
                if (!base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                {
                    ExibirLogin();
                    return;
                }
                else if (EmpresaBloqueada(objFilial))
                {
                    Redirect("~/Principal.aspx");
                    return;
                }

                switch (objFilial.SituacaoFilial.IdSituacaoFilial)
                {
                    case (int)BNE.BLL.Enumeradores.SituacaoFilial.Bloqueado:
                    case (int)BNE.BLL.Enumeradores.SituacaoFilial.PreCadastro:
                    case (int)BNE.BLL.Enumeradores.SituacaoFilial.FaltaDadosReceita:
                        base.ExibirMensagem("Não foi possível criar campanha!", Code.Enumeradores.TipoMensagem.Aviso);
                        return;
                }
              
                var objCampanhaRecrutamento = new BLL.CampanhaRecrutamento
                {
                    QuantidadeRetorno = Convert.ToInt16(rcbQuantidadeCurriculos.SelectedValue),
                    TipoRetornoCampanhaRecrutamento = new BLL.TipoRetornoCampanhaRecrutamento(Convert.ToInt16(rcbTipoRetorno.SelectedValue)),
                };
                //, Convert.ToDecimal(txtTelefone.DDD), Convert.ToDecimal(txtTelefone.Fone),
                objCampanhaRecrutamento.Salvar(txtFuncao.Text, txtCidade.Text, txtSalario.Valor.Value, new BLL.Filial(base.IdFilial.Value), new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value));
                base.ExibirMensagemConfirmacao("Sucesso", "Campanha enviada com sucesso!", false);
                LimparCampos();
            }
            catch (Exception ex)
            {
                base.ExibirMensagem(ex.Message, Code.Enumeradores.TipoMensagem.Aviso);
                GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region master_LoginEfetuadoSucesso
        protected void master_LoginEfetuadoSucesso()
        {
            PreencherCamposSaldo();
            FluxoDispararCampanha();
        }
        #endregion

        #endregion

    }
}