using System;
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

        public bool Enable
        {
            set { this.txtFuncaoAnunciarVaga.Enabled = value; }
        }
        public ListItem[] TipoContratoItens
        {
            get
            {
                if ((this.rblContrato.Items ?? new ListItemCollection()).Count == 0)
                    CarregarCheckBoxList();

                return (this.rblContrato.Items ?? new ListItemCollection()).OfType<ListItem>().ToArray();
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
                    this.rblContrato.Focus();
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
            if (this.rblContrato.Items != null && this.rblContrato.Items.Count > 0)
                return;

            UIHelper.CarregarRadioButtonList(rblContrato, TipoVinculo.ListarTipoVinculo());
            
            foreach (var item in rblContrato.Items.OfType<ListItem>())
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(item.Text, "Efetivo"))
                    item.Selected = true;
            }
        }

        #endregion

        public void LimparCampos()
        {
            txtFuncaoAnunciarVaga.Text = string.Empty;

            foreach (ListItem item in TipoContratoItens)
            {
                item.Selected = false;
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


        //Inativado
        //public void SetTipoVinculoChecked(string listItemValue, bool isChecked)
        //{
        //    foreach (var item in TipoContratoItens)
        //    {
        //        if (StringComparer.OrdinalIgnoreCase.Equals(listItemValue, item.Value))
        //        {
        //            item.Selected = isChecked;
        //        }
        //    }
        //}


        public delegate void TemEstagioEventHandler(bool tem_estagio);
        public event TemEstagioEventHandler MarcouEstagio;

        protected void rblContrato_SelectedIndexChanged(object sender, EventArgs e)
        {
            TemEstagioEventHandler handler = MarcouEstagio;
            if (handler != null)
                handler(this.TemEstagio);
        }

        public bool TemEstagio
        {
            get
            {
                return (this.TipoContratoItens.Where(obj => (obj.Selected && (obj.Value == "4" || obj.Value == "1"))).Count() > 0);
            }
        }

    }
}