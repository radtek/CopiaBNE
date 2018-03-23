using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using System;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;

namespace BNE.Web
{
    public partial class R1Produto : BasePage
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "R1Produto");
            Ajax.Utility.RegisterTypeForAjax(typeof(R1Produto));

            var master = (Principal)Page.Master;

            if (master != null)
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;

            revEmail.ValidationExpression = Resources.Configuracao.regexEmail;

            if (!IsPostBack)
            {
                UIHelper.CarregarRadComboBox(rcbDisponibilidade, Disponibilidade.Listar());
                UIHelper.CarregarRadComboBox(rcbEscolaridade, Escolaridade.Listar());
                UIHelper.CarregarRadComboBox(rcbSexo, Sexo.Listar(), new RadComboBoxItem("Indiferente", "-1"));
                UIHelper.CarregarRadComboBox(rcbHabilitacao, CategoriaHabilitacao.Listar(), "Idf_Categoria_Habilitacao", "Des_Categoria_Habilitacao", new RadComboBoxItem("Indiferente", "-1"));
                UIHelper.CarregarRadComboBox(rcbTipoVinculo, TipoVinculo.ListarTipoVinculo());

                rcbEscolaridade.SelectedValue = ((int)BLL.Enumeradores.Escolaridade.EnsinoFundamentalIncompleto).ToString();
            }
        }
        #endregion

        #region btlSolicitar_Click
        protected void btlSolicitar_Click(object sender, EventArgs e)
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue || base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                Solicitar();
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirModal", "AbrirModal();", true);
        }
        #endregion

        #region btlOK_Click
        protected void btlOK_Click(object sender, EventArgs e)
        {
            Solicitar();
        }
        #endregion

        #region btlJaSouCadastrado_Click
        protected void btlJaSouCadastrado_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "FecharModal", "FecharModal();", true);
            ExibirLogin();
        }
        #endregion

        #region master_LoginEfetuadoSucesso
        protected void master_LoginEfetuadoSucesso()
        {
            Solicitar();
        }
        #endregion

        #endregion

        #region Metodos

        #region Solicitar
        private void Solicitar()
        {
            try
            {
                Funcao objFuncao = Funcao.CarregarPorDescricao(txtFuncao.Text);
                if (objFuncao == null)
                    ExibirMensagem("É necessário informar uma função!", TipoMensagem.Aviso);
                else
                {
                    Cidade objCidade;
                    Cidade.CarregarPorNome(txtCidade.Text, out objCidade);
                    if (objCidade == null)
                        ExibirMensagem("É necessário informar uma cidade!", TipoMensagem.Aviso);
                    else
                    {

                        #region Usuário Filial Perfil
                        UsuarioFilialPerfil objUsuarioFilialPerfil = null;
                        if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                            objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                        else if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                            objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoCandidato.Value);
                        #endregion

                        #region Sexo
                        Sexo objSexo = null;
                        if (!rcbSexo.SelectedValue.Equals("-1"))
                            objSexo = Sexo.LoadObject(Convert.ToInt32(rcbSexo.SelectedValue));
                        #endregion

                        #region Escolaridade
                        Escolaridade objEscolaridade = Escolaridade.LoadObject(Convert.ToInt32(rcbEscolaridade.SelectedValue));
                        #endregion

                        #region Categoria Habilitação
                        CategoriaHabilitacao objCategoriaHabilitacao = null;
                        if (!rcbHabilitacao.SelectedValue.Equals("-1"))
                            objCategoriaHabilitacao = CategoriaHabilitacao.LoadObject(Convert.ToInt32(rcbHabilitacao.SelectedValue));
                        #endregion

                        #region Tipo Vinculo
                        TipoVinculo objTipoVinculo = null;
                        if (!rcbHabilitacao.SelectedValue.Equals("-1"))
                            objTipoVinculo = TipoVinculo.LoadObject(Convert.ToInt32(rcbTipoVinculo.SelectedValue));
                        #endregion

                        #region Disponibilidade
                        var listaDisponibilidades = rcbDisponibilidade.GetCheckedItems().Select(item => Disponibilidade.LoadObject(Convert.ToInt32(item.Value)));
                        #endregion

                        short? idadeMinima = null;
                        short? idadeMaxima = null;
                        short dummy;

                        if (Int16.TryParse(txtIdadeMinima.Valor, out dummy))
                            idadeMinima = dummy;

                        if (Int16.TryParse(txtIdadeMaxima.Valor, out dummy))
                            idadeMaxima = dummy;

                        SolicitacaoR1.Solicitar(txtNome.Text, txtTelefone.DDD, txtTelefone.Fone, txtEmail.Text, objFuncao, objCidade, txtAtribuicoes.Text, idadeMinima, idadeMaxima, objEscolaridade, txtSalarioDe.Valor, txtSalarioAte.Valor, Convert.ToInt16(txtNumeroVagas.Valor), txtBeneficios.Text, objSexo, listaDisponibilidades.ToList(), objCategoriaHabilitacao, txtInformacoesAdicionais.Text, txtBairro.Text, objTipoVinculo, objUsuarioFilialPerfil);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "FecharModal", "FecharModal();", true);
                        base.ExibirMensagemConfirmacao("Sucesso", "Obrigado! Sua solicitação foi enviada com sucesso, aguarde nosso contato.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                ExibirMensagem("Houve um erro ao processar sua solicitação!", TipoMensagem.Erro);
            }
        }
        #endregion

        #region RecuperarJobFuncao
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string RecuperarJobFuncao(string valor)
        {
            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(valor, out objFuncao))
            {
                if (!string.IsNullOrEmpty(objFuncao.DescricaoJob))
                    return objFuncao.DescricaoJob;
                return String.Empty;
            }
            return String.Empty;
        }
        #endregion

        #endregion

    }
}