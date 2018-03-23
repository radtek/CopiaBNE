using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;

namespace BNE.Web.UserControls
{
    public partial class ContratoFuncao : BaseUserControl
    {
        public enum TipoFoco
        {
            TipoContrato,
            Funcao
        }

        public enum TipoUso
        {
            Candidato,
            Empresa
        }

        #region [ Properties / Events ]
        public event EventHandler FuncaoReset;

        protected virtual void OnFuncaoReset()
        {
            EventHandler handler = FuncaoReset;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public event EventHandler FuncaoValida;

        public event EventHandler FuncaoInvalida;

        protected virtual void OnFuncaoInvalida()
        {
            EventHandler handler = FuncaoInvalida;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnFuncaoValida()
        {
            EventHandler handler = FuncaoValida;
            if (handler != null) handler(this, EventArgs.Empty);

        }

        public string OnClientFuncaoLostFocus { get; set; }
        public string OnClientFuncaoChange { get; set; }

        public string ValidationGroup { get; set; }
        public bool Obrigatorio { get; set; }

        public string FuncaoDesc
        {
            get
            {
                return this.txtFuncaoAnunciarVaga.Text;
            }
        }
        public ListItem[] TipoContratoItens
        {
            get
            {
                if ((this.chblContrato.Items ?? new ListItemCollection()).Count == 0)
                    CarregarCheckBoxList();

                return (this.chblContrato.Items ?? new ListItemCollection()).OfType<ListItem>().ToArray();
            }
        }

        public TipoUso ImagemPara { get; set; }
        #endregion

        #region [ Load ]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

            Ajax.Utility.RegisterTypeForAjax(typeof(ContratoFuncao));
        }

        public void SetFocus(TipoFoco foco)
        {
            switch (foco)
            {
                case TipoFoco.TipoContrato:
                    this.chblContrato.Focus();
                    break;
                case TipoFoco.Funcao:
                    if (Page.IsPostBack)
                    {
                        this.txtFuncaoAnunciarVaga.Focus();
                    }
                    else
                    {
                        AjaxControlToolkit.Utility.SetFocusOnLoad(txtFuncaoAnunciarVaga);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("foco");
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!string.IsNullOrEmpty(ValidationGroup))
            {
                this.cvFuncaoAnunciarVaga.ValidationGroup = ValidationGroup;

                if (Obrigatorio)
                    this.rfFuncao.ValidationGroup = ValidationGroup;
            }
        }

        private void Inicializar()
        {
            if (base.STC.Value)
            {
                aceFuncao.ContextKey = base.IdOrigem.Value.ToString(CultureInfo.CurrentCulture);
            }

            CarregarCheckBoxList();

            if (!string.IsNullOrWhiteSpace(OnClientFuncaoChange))
            {
                txtFuncaoAnunciarVaga.Attributes["OnChange"] += OnClientFuncaoChange;
            }

            if (!string.IsNullOrWhiteSpace(OnClientFuncaoLostFocus))
            {
                txtFuncaoAnunciarVaga.Attributes["OnBlur"] += OnClientFuncaoLostFocus;
            }

            this.rfFuncao.Enabled = Obrigatorio;
            this.rfFuncao.Visible = Obrigatorio;
            if (!Obrigatorio)
            {
                this.txtFuncaoAnunciarVaga.CssClass = "textbox_padrao";
            }


            //TODO Valdecir 29/09/2014
            if (ImagemPara == TipoUso.Candidato)
            {
                imgBoxContrato.ImageUrl = "~/img/aviso_estag_candidato.png";
            }
            /*else
            {
                imgBoxContrato.ImageUrl = "~/img/aviso_estag_empresa.png";
            }*/
        }

        private void CarregarCheckBoxList()
        {
            if (this.chblContrato.Items != null && this.chblContrato.Items.Count > 0)
                return;

            UIHelper.CarregarCheckBoxList(chblContrato, TipoVinculo.ListarTipoVinculo());
            foreach (var item in chblContrato.Items.OfType<ListItem>())
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(item.Text, "Efetivo"))
                    item.Selected = true;
            }
        }

        #endregion

        public void SetAceFuncaoContextKey(string newValue)
        {
            aceFuncao.ContextKey = newValue;
        }

        public void LimparCampos()
        {
            txtFuncaoAnunciarVaga.Text = string.Empty;

            foreach (ListItem item in TipoContratoItens)
            {
                item.Selected = false;
            }
        }

        public void CarregarParametros()
        {
            try
            {
                var parametros = new List<BNE.BLL.Enumeradores.Parametro>
                    {
                        BNE.BLL.Enumeradores.Parametro.IntervaloTempoAutoComplete,
                        BNE.BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao,
                        BNE.BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao
                    };

                Dictionary<BNE.BLL.Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                aceFuncao.CompletionInterval = Convert.ToInt32(valoresParametros[BNE.BLL.Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncao.CompletionSetCount = Convert.ToInt32(valoresParametros[BNE.BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncao.MinimumPrefixLength = Convert.ToInt32(valoresParametros[BNE.BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }

        public void SetFuncao(string funcao)
        {
            this.txtFuncaoAnunciarVaga.Text = funcao;
        }

        protected void txtFuncaoAnunciarVaga_TextChanged(object sender, EventArgs e)
        {
            if (this.Obrigatorio)
            {
                rfFuncao.Validate();
            }

            if (string.IsNullOrWhiteSpace(txtFuncaoAnunciarVaga.Text))
            {
                OnFuncaoReset();
                return;
            }

            cvFuncaoAnunciarVaga.Validate();
            if (!cvFuncaoAnunciarVaga.IsValid)
            {
                txtFuncaoAnunciarVaga.Attributes.Add("onfocus", "this.select()");
            }
        }

        protected void cvFuncaoAnunciarVaga_OnServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!ValidarFuncao(txtFuncaoAnunciarVaga.Text))
            {
                args.IsValid = false;
                cvFuncaoAnunciarVaga.ErrorMessage = @"Função Inválida";
                OnFuncaoInvalida();
                return;
            }

            if (!ValidarFuncaoLimitacaoPorContrato(txtFuncaoAnunciarVaga.Text))
            {
                args.IsValid = false;
                //cvFuncaoAnunciarVaga.ErrorMessage = @"Função indisponível, favor utilizar o tipo de contrato</br>&nbsp;&nbsp;ao especificar uma vaga para Aprendiz ou Estagiário";
                cvFuncaoAnunciarVaga.ErrorMessage = @"Função Indisponível";
                OnFuncaoInvalida();
                return;
            }

            OnFuncaoValida();
        }

        #region ValidarFuncao
        /// <summary>
        /// Validar funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            int? idOrigem = null;
            if (base.IdOrigem.HasValue)
                idOrigem = base.IdOrigem.Value;

            return Funcao.ValidarFuncaoPorOrigem(idOrigem, valor);
        }

        /// <summary>
        /// Validar função limitação de função genérica, feito p/ integração web estágios
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool ValidarFuncaoLimitacaoPorContrato(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return true;

            valor = valor.Trim();

            return Funcao.ValidarFuncaoLimitacaoIntegracaoWebEstagios(valor);
        }
        #endregion

        public override void Dispose()
        {
            base.Dispose();
            FuncaoValida = null;
            FuncaoInvalida = null;
            FuncaoReset = null;
        }

        public void AtualizarValidationGroup(string validationGroup)
        {
            this.rfFuncao.ValidationGroup = validationGroup;
            this.cvFuncaoAnunciarVaga.ValidationGroup = validationGroup;
        }

        public void SetTipoVinculoChecked(string listItemValue, bool isChecked)
        {
            foreach (var item in TipoContratoItens)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(listItemValue, item.Value))
                {
                    item.Selected = isChecked;
                }
            }
        }
    }
}