using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Ajax;
using BNE.BLL.Enumeradores;
using BNE.Web.Code;
using Newtonsoft.Json;
using Funcao = BNE.BLL.Funcao;

namespace BNE.Web.UserControls
{
    public partial class ContratoFuncao : BaseUserControl
    {
        public delegate void TemEstagioEventHandler(bool tem_estagio);

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

        public bool UmCurso { get; set; }

        public bool TemEstagio
        {
            get { return TipoContratoItens.Any(obj => obj.Selected && Convert.ToInt32(obj.Value) == Convert.ToInt32(TipoVinculo.Estágio)); }
        }

        public bool TemAprendiz
        {
            get { return TipoContratoItens.Any(obj => obj.Selected && Convert.ToInt32(obj.Value) == Convert.ToInt32(TipoVinculo.Aprendiz)); }
        }

        public void LimparCampos()
        {
            txtFuncaoAnunciarVaga.Text = string.Empty;

            foreach (var item in TipoContratoItens)
            {
                item.Selected = false;
            }
        }

        public void SetFuncao(string funcao)
        {
            txtFuncaoAnunciarVaga.Text = funcao;
        }

        protected void txtFuncaoAnunciarVaga_TextChanged(object sender, EventArgs e)
        {
            if (Obrigatorio)
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
                cvFuncaoAnunciarVaga.ErrorMessage = @"Função Indisponível";
                OnFuncaoInvalida();
                return;
            }

            OnFuncaoValida();
        }

        public override void Dispose()
        {
            base.Dispose();
            FuncaoValida = null;
            FuncaoInvalida = null;
            FuncaoReset = null;
        }

        public void AtualizarValidationGroup(string validationGroup)
        {
            rfFuncao.ValidationGroup = validationGroup;
            cvFuncaoAnunciarVaga.ValidationGroup = validationGroup;
        }

        public event TemEstagioEventHandler MarcouEstagio;
        public event TemEstagioEventHandler MarcouAprendiz;

        protected void rblContrato_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Resolvendo função da vaga
            if (TemEstagio)
            {
                txtFuncaoAnunciarVaga.Text = "Estagiário";
            }
            else if (TemAprendiz)
            {
                txtFuncaoAnunciarVaga.Text = "Aprendiz";
            }
            else
            {
                txtFuncaoAnunciarVaga.Text = string.Empty;
            }

            AjustarPaineisEstagio();

            upPanelFuncaoCursos.Update();
            //Esgario ou aprendiz, selecionam curso
            MarcouEstagio?.Invoke(TemEstagio ? TemEstagio : TemAprendiz);
        }

        private void AjustarPaineisEstagio()
        {
            var temEstagio = TemEstagio ? TemEstagio : TemAprendiz;

            if (UmCurso) //pesquisa avançada de vaga aceitar apenas um curso na pesquisa
            {
                btnCurso.Visible = false;
            }

            // Ajustando visualização
            divCursos.Visible = divCursosAceitos.Visible = temEstagio;
            divFuncao.Visible = !temEstagio;

            upPanelFuncaoCursos.Update();
        }

        #region [ Properties / Events ]

        public event EventHandler FuncaoReset;

        protected virtual void OnFuncaoReset()
        {
            var handler = FuncaoReset;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler FuncaoValida;

        public event EventHandler FuncaoInvalida;

        protected virtual void OnFuncaoInvalida()
        {
            var handler = FuncaoInvalida;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnFuncaoValida()
        {
            var handler = FuncaoValida;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public string OnClientFuncaoLostFocus { get; set; }
        public string OnClientFuncaoChange { get; set; }

        public string ValidationGroup { get; set; }
        public bool Obrigatorio { get; set; }

        public string CursoDescricao
        {
            get { return txtCursoAnunciarVaga.Text; }
        }

        public string FuncaoDesc
        {
            get { return txtFuncaoAnunciarVaga.Text; }
        }

        public List<string> DescricoesCursos
        {
            get
            {
                var cursos = JsonConvert.DeserializeObject<List<string>>(hdfListaCursos.Value);
                if (!string.IsNullOrEmpty(CursoDescricao))
                {
                    cursos.Add(CursoDescricao);
                }
                return cursos;
            }
            set { hdfListaCursos.Value = JsonConvert.SerializeObject(value); }
        }

        public bool Enable
        {
            set
            {
                rblContrato.Enabled = value;
                txtFuncaoAnunciarVaga.Enabled = value;
            }
        }

        public bool EnableTipoVinculo
        {
            set { rblContrato.Enabled = value; }
        }

        public ListItem[] TipoContratoItens
        {
            get
            {
                if ((rblContrato.Items ?? new ListItemCollection()).Count == 0)
                {
                    CarregarCheckBoxList();
                }

                return (rblContrato.Items ?? new ListItemCollection()).OfType<ListItem>().ToArray();
            }
        }

        public TipoUso ImagemPara { get; set; }

        #endregion

        #region [ Load ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Inicializar();
            }

            Utility.RegisterTypeForAjax(typeof(ContratoFuncao));
        }

        public void SetFocus(TipoFoco foco)
        {
            switch (foco)
            {
                case TipoFoco.TipoContrato:
                    rblContrato.Focus();
                    break;
                case TipoFoco.Funcao:
                    if (Page.IsPostBack)
                    {
                        txtFuncaoAnunciarVaga.Focus();
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
                cvFuncaoAnunciarVaga.ValidationGroup = ValidationGroup;

                if (Obrigatorio)
                {
                    rfFuncao.ValidationGroup = ValidationGroup;
                }
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

            rfFuncao.Enabled = Obrigatorio;
            rfFuncao.Visible = Obrigatorio;
            if (!Obrigatorio)
            {
                txtFuncaoAnunciarVaga.CssClass = "textbox_padrao";
            }

            // se a lista de cursos ainda não foi inicializada
            if (string.IsNullOrEmpty(hdfListaCursos.Value))
            {
                hdfListaCursos.Value = JsonConvert.SerializeObject(new List<string>());
            }

            AjustarPaineisEstagio();
        }

        private void CarregarCheckBoxList()
        {
            if (rblContrato.Items.Count > 0)
            {
                return;
            }

            UIHelper.CarregarRadioButtonList(rblContrato, BLL.TipoVinculo.ListarTipoVinculo());

            foreach (var item in rblContrato.Items.OfType<ListItem>())
            {
                item.Selected = Convert.ToInt32(item.Value) == Convert.ToInt32(TipoVinculo.Efetivo);
            }
        }

        #endregion

        #region ValidarFuncao

        /// <summary>
        ///     Validar funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [AjaxMethod(HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
            {
                return true;
            }

            int? idOrigem = null;
            if (IdOrigem.HasValue)
            {
                idOrigem = IdOrigem.Value;
            }

            return Funcao.ValidarFuncaoPorOrigem(idOrigem, valor);
        }

        /// <summary>
        ///     Validar função limitação de função genérica, feito p/ integração web estágios
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [AjaxMethod(HttpSessionStateRequirement.Read)]
        public bool ValidarFuncaoLimitacaoPorContrato(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return true;
            }

            valor = valor.Trim();

            return Funcao.ValidarFuncaoLimitacaoIntegracaoWebEstagios(valor);
        }

        #endregion
    }
}