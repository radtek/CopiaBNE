using System;
using BNE.BLL.Security;
using BNE.Web.Code;
using BNE.CEP;

namespace BNE.Web.UserControls
{
    public partial class ucEndereco : BaseUserControl
    {

        #region Propriedades

        public string CEP { get { return txtCEP.Valor; } }
        public string Logradouro { get { return txtLogradouroEndereco.Text; } }
        public string Numero { get { return txtNumero.Text; } }
        public string Complemento { get { return txtComplementoEndereco.Text; } }
        public string Bairro { get { return txtBairroEndereco.Text; } }
        public string Cidade { get { return txtCidadeEndereco.Text; } }

        #endregion

        #region Métodos

        #region HabilitarCamposEndereco
        private void HabilitarCamposEndereco(bool habilitar)
        {
            lblLogradouro.Enabled = txtLogradouroEndereco.Enabled = habilitar;
            lblBairro.Enabled = txtBairroEndereco.Enabled = habilitar;
            lblCidade.Enabled = txtCidadeEndereco.Enabled = habilitar;
        }
        #endregion

        #region HabilitarTodosCampos
        public void HabilitarTodosCampos(bool habilitar)
        {
            lblCEP.Enabled = txtCEP.Enabled = habilitar;
            lblLogradouro.Enabled = txtLogradouroEndereco.Enabled = habilitar;
            lblNumero.Enabled = txtNumero.Enabled = habilitar;
            lblComplemento.Enabled = txtComplementoEndereco.Enabled = habilitar;
            lblBairro.Enabled = txtBairroEndereco.Enabled = habilitar;
            lblCidade.Enabled = txtCidadeEndereco.Enabled = habilitar;
        }
        #endregion

        #region ValidarVariosEnderecosPorCEP
        /// <summary>
        /// Validar se o cep informado contém mais de um endereço associado ao mesmo.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarVariosEnderecosPorCEP(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                try
                {
                    int qtdeCepEncontrados = 0;

                    var objCep = new Cep
                    {
                        sCep = valor.Replace("-", string.Empty).Trim()
                    };

                    qtdeCepEncontrados = Cep.ContaConsulta(objCep);

                    if (qtdeCepEncontrados > 1)
                    {
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Falha na busca de CEP");
                }
            }
            return false;
        }
        #endregion

        #region Inicializar
        public void Inicializar(string validationGroup)
        {
            AjustarValidationGroup(validationGroup);
            HabilitarCamposEndereco(ValidarVariosEnderecosPorCEP(txtCEP.Valor));
        }
        #endregion

        #region PreencherCampos
        public void PreencherCampos(string numeroCEP, string descricaoLogradouro, string numeroEndereco, string descricaoComplemento, string nomeBairro, string nomeCidade, string siglaEstado)
        {
            txtCEP.Valor = numeroCEP;
            txtLogradouroEndereco.Text = descricaoLogradouro;
            txtNumero.Text = numeroEndereco;
            txtComplementoEndereco.Text = descricaoComplemento;
            txtCidadeEndereco.Text = String.Format("{0}/{1}", nomeCidade, siglaEstado);
            txtBairroEndereco.Text = nomeBairro;
        }
        #endregion

        #region LimparCampos
        public void LimparCampos()
        {
            txtCEP.Valor = txtLogradouroEndereco.Text = txtNumero.Text = txtComplementoEndereco.Text = txtCidadeEndereco.Text = txtBairroEndereco.Text = string.Empty;
        }
        #endregion

        #region AjustarValidationGroup
        public void AjustarValidationGroup(string validationGroup)
        {
            txtCEP.ValidationGroup = rfvLogradouroEndereco.ValidationGroup = rfvNumero.ValidationGroup = rfvCidadeEndereco.ValidationGroup = rfvBairroEndereco.ValidationGroup = validationGroup;
            upTxtLogradouroEndereco.Update();
        }
        #endregion

        #endregion

        #region Eventos

        #region Delegates
        public delegate void OnCEPValorAlterado();
        public event OnCEPValorAlterado ValorAlterado;
        #endregion

        #region Page_Load
        /// <summary>
        /// Método executado quando a página é carregada
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ucBuscarCEP.LogradouroSelecionado += ucBuscarCEP_LogradouroSelecionado;
            ucBuscarCEP.Cancelar += ucBuscarCEP_Cancelar;
        }
        #endregion

        #region txtCEP_ValorAlterado
        /// <summary>
        /// Responsável por habilitar a modal de busca de CEP com os vários endereços
        /// vinculados ao CEP informado (caso o CEP informado esteja configurado para vários endereços).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCEP_ValorAlterado(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCEP.Valor) && ValidarVariosEnderecosPorCEP(txtCEP.Valor))
            {
                txtCEP.DisplayMensagemErroWS = "none";

                ucBuscarCEP.CarregarPorCep(txtCEP.Valor);
            }
            else
            {
                int qtdeCepEncontrados = 0;
                try
                {
                    var objCep = new Cep
                    {
                        sCep = txtCEP.Valor.Replace("-", string.Empty).Trim()
                    };

                    qtdeCepEncontrados = Cep.ContaConsulta(objCep);

                    if (qtdeCepEncontrados == 0)
                    {
                        HabilitarCamposEndereco(true);

                        txtCEP.DisplayMensagemErroWS = "block";
                        txtCEP.MensagemErroWS = "CEP inexistente";
                        txtCEP.Focus();
                    }
                    else
                    {
                        Cep.CompletarCEP(ref objCep);
                        HabilitarCamposEndereco(false);

                        //Em alguns casos há um CEP mas não há um Logradouro descrito para o mesmo.
                        if (string.IsNullOrEmpty(objCep.Logradouro))
                        {
                            txtLogradouroEndereco.Text = string.Empty;
                            lblLogradouro.Enabled = txtLogradouroEndereco.Enabled = true;
                            HabilitarCamposEndereco(true);
                        }
                        else
                        {
                            txtLogradouroEndereco.Text = objCep.Logradouro;
                            lblLogradouro.Enabled = txtLogradouroEndereco.Enabled = false;
                        }

                        txtCidadeEndereco.Text = String.Format("{0}/{1}", objCep.Cidade, objCep.Estado);
                        txtBairroEndereco.Text = objCep.Bairro;
                        txtCEP.DisplayMensagemErroWS = "none";
                        txtNumero.Focus();

                        if (ValorAlterado != null)
                            ValorAlterado();
                    }
                        
                    upTxtCEP.Update();
                    upTxtLogradouroEndereco.Update();
                    upTxtBairroEndereco.Update();
                    upTxtCidadeEndereco.Update();
                }
                catch (Exception ex)
                {
                    HabilitarCamposEndereco(true);
                    txtCEP.DisplayMensagemErroWS = "block";
                    txtCEP.MensagemErroWS = "CEP inexistente";
                    txtCEP.Focus();

                    EL.GerenciadorException.GravarExcecao(ex, "Falha na busca de CEP");
                }
            }
        }
        #endregion

        #region ucBuscarCEP_VoltarFoco
        protected void ucBuscarCEP_VoltarFoco()
        {
            txtCEP.Focus();
        }
        #endregion

        #region ucBuscarCEP_Cancelar
        void ucBuscarCEP_Cancelar()
        {
            HabilitarCamposEndereco(ValidarVariosEnderecosPorCEP(txtCEP.Valor));
        }
        #endregion

        #region ucBuscarCEP_LogradouroSelecionado
        /// <summary>
        /// Ação a realizar ao encontrar cep pesquisado
        /// </summary>
        /// <param name="cep">cep encontrado</param>
        /// <param name="logradouro">logradouro selecionado</param>
        void ucBuscarCEP_LogradouroSelecionado(string cep, string logradouro)
        {
            txtCidadeEndereco.Text = string.Empty;
            txtLogradouroEndereco.Text = string.Empty;
            txtBairroEndereco.Text = string.Empty;

            HabilitarCamposEndereco(true);

            if (!string.IsNullOrEmpty(cep))
            {
                try
                {
                    var objCep = new Cep
                    {
                        sCep = cep,
                        Logradouro = Server.HtmlDecode(logradouro)
                    };

                    Cep.CompletarCEP(ref objCep);

                    txtCEP.Valor = objCep.sCep;
                    txtCidadeEndereco.Text = String.Format("{0}/{1}", objCep.Cidade, objCep.Estado);
                    txtLogradouroEndereco.Text = objCep.Logradouro;
                    txtBairroEndereco.Text = objCep.Bairro;
                }
                catch(Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Falha na busca de CEP");
                }

                if (string.IsNullOrEmpty(txtLogradouroEndereco.Text))
                    HabilitarCamposEndereco(true);
            }

            txtNumero.Focus();
            txtCEP.DisplayMensagemErroWS = "none";

            upTxtCEP.Update();
            upTxtLogradouroEndereco.Update();
            upTxtBairroEndereco.Update();
            upTxtCidadeEndereco.Update();
        }
        #endregion

        #endregion

    }
}