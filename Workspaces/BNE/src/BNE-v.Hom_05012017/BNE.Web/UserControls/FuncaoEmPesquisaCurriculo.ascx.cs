using BNE.BLL;
using BNE.EL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace BNE.Web.UserControls
{
    public partial class FuncaoEmPesquisaCurriculo : BaseUserControl
    {

        public enum TipoFoco
        {
            Estagiario,
            Funcao,
            Aprendiz
        }


        #region [ Properties / Events ]
        public event EventHandler FuncaoReset;
        public event EventHandler FuncaoValida;
        public event EventHandler FuncaoInvalida;
        public event EventHandler TextChanged;
        public event EventHandler FuncoesAlteradas;

        protected virtual void OnFuncaoReset()
        {
            EventHandler handler = FuncaoReset;
            if (handler != null) handler(this, EventArgs.Empty);
        }
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

        protected virtual void OnTextChanged()
        {
            EventHandler handler = TextChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnFuncoesAlteradas()
        {
            EventHandler handler = FuncoesAlteradas;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public string OnClientFuncaoLostFocus { get; set; }
        public string OnClientFuncaoChange { get; set; }

        public string ValidationGroup { get; set; }
        public bool Obrigatorio { get; set; }

        public bool CheckBoxEstagiarioSelecionado
        {
            get { return this.chbEstagiario.Checked; }

        }

        public bool CheckBoxAprendizSelecionado
        {
            get { return this.chbAprendiz.Checked; }
        }

        #region Funcoes - Variavel1
        /// <summary>
        /// Propriedade que armazena e recupera o datatable de funcoes
        /// </summary>
        public DataTable Funcoes
        {
            get
            {
                return (DataTable)ViewState[Chave.Temporaria.Variavel1.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                OnFuncoesAlteradas();
            }
        }
        #endregion

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
                case TipoFoco.Estagiario:
                    this.chbEstagiario.Focus();
                    break;
                case TipoFoco.Aprendiz:
                    this.chbAprendiz.Focus();
                    break;
                case TipoFoco.Funcao:
                    if (Page.IsPostBack)
                    {
                        this.txtFuncaoPretendida.Focus();
                    }
                    else
                    {
                        AjaxControlToolkit.Utility.SetFocusOnLoad(txtFuncaoPretendida);
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
            if (!string.IsNullOrWhiteSpace(OnClientFuncaoChange))
            {
                txtFuncaoPretendida.Attributes["OnChange"] += OnClientFuncaoChange;
            }

            if (!string.IsNullOrWhiteSpace(OnClientFuncaoLostFocus))
            {
                txtFuncaoPretendida.Attributes["OnBlur"] += OnClientFuncaoLostFocus;
            }

            this.rfFuncao.Enabled = Obrigatorio;
            this.rfFuncao.Visible = Obrigatorio;
            if (!Obrigatorio)
            {
                this.txtFuncaoPretendida.CssClass = "textbox_padrao";
            }
        }

        #endregion

        public void InicializarDataTableFuncoes()
        {
            Funcoes = new DataTable();

            Funcoes.Columns.Add(new DataColumn("IdFuncao", typeof(Int32)));
            Funcoes.Columns.Add(new DataColumn("DescricaoFuncao", typeof(String)));
            Funcoes.Columns.Add(new DataColumn("class", typeof(String)));
            Funcoes.Columns.Add(new DataColumn("Ativa", typeof(Boolean)));
            Funcoes.Columns.Add(new DataColumn("Novo", typeof(Boolean)));
            Funcoes.Columns.Add(new DataColumn("Similar", typeof(Boolean)));
        }

        public void LimparCampos()
        {
            InicializarDataTableFuncoes();
            chbEstagiario.Checked = false;
            chbAprendiz.Checked = false;
            txtFuncaoPretendida.Text = string.Empty;
            rptFuncoes.DataBind();
        }

        public void SetFuncoes(List<Funcao> listaFuncoes)
        {
            listaFuncoes.ForEach(x => AdicionarFuncoes(x));
            CarregarRepeater();
        }


        protected void cvFuncaoAnunciarVaga_OnServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!ValidarFuncao(txtFuncaoPretendida.Text))
            {
                args.IsValid = false;
                cvFuncaoAnunciarVaga.ErrorMessage = @"Função Inválida";
                OnFuncaoInvalida();
                return;
            }

            if (!ValidarFuncaoLimitacaoPorContrato(txtFuncaoPretendida.Text))
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

        public void SetEstagiario(bool isChecked)
        {
            this.chbEstagiario.Checked = isChecked;
        }

        public void SetAprendiz(bool isChecked)
        {
            this.chbAprendiz.Checked = isChecked;
        }

        #region AdicionarFuncoes

        /// <summary>
        /// Adiciona uma nova função ou ativa uma existente
        /// </summary>
        /// <param name="funcao">A Função selecionada</param>
        /// <param name="isSimilar">Se é uma função similar a outra já selecionada</param>
        private void AdicionarFuncoes(Funcao funcao, bool isSimilar = false, bool similaresPretendidas = false)
        {
            var jaExiste = false;

            foreach (DataRow item in Funcoes.Rows)
            {
                if (item["IdFuncao"].ToString() == funcao.IdFuncao.ToString())
                {
                    jaExiste = true;

                    if (item["class"].ToString().Contains("liEsconder"))
                    {
                        item["class"] = "liActive";
                        item["Ativa"] = true;
                    }
                }
                else
                {
                    item["class"] = item["class"].ToString().Replace("liInactive", "liActive");
                }
            }
            if (jaExiste)
                return;

            var dr = Funcoes.NewRow();
            dr["IdFuncao"] = funcao.IdFuncao;
            dr["DescricaoFuncao"] = funcao.DescricaoFuncao;
            if (similaresPretendidas | isSimilar)
                dr["class"] = "liActive liSimilar";
            else
                dr["class"] = "liActive";
            dr["Ativa"] = true;
            dr["Novo"] = true;
            dr["Similar"] = isSimilar;

            Funcoes.Rows.Add(dr);
            OnFuncoesAlteradas();
        }
        #endregion

        #region CarregarRepeater
        /// <summary>
        /// Bind no repeater
        /// </summary>
        private void CarregarRepeater()
        {
            rptFuncoes.DataSource = Funcoes;
            rptFuncoes.DataBind();
        }
        #endregion

        #region OnTextChanged
        protected void OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                Funcao funcao = null;
                if (string.IsNullOrEmpty(FuncoesSel.Value) || FuncoesSel.Value.Equals("null"))
                {
                    var textBox = (sender) as TextBox;
                    if (textBox != null)
                        funcao = Funcao.CarregarPorDescricao(textBox.Text);
                    if (funcao == null)
                        return;
                }
                else
                {
                    try
                    {
                        funcao = Funcao.LoadObject(Convert.ToInt32(FuncoesSel.Value));
                    }
                    catch (RecordNotFoundException)
                    {
                        funcao = new Funcao(Convert.ToInt32(FuncoesSel.Value));
                    }
                }
                if (funcao != null)
                {
                    if (Funcoes.Rows.Count < 3)
                    {
                        AdicionarFuncoes(funcao, false);

                        if (Funcoes.Rows.Count > 0)
                            CarregarRepeater();
                    }
                    else
                        base.ExibirMensagem("É permitido escolher no máximo 3 funções", TipoMensagem.Aviso);
                }
                else
                {
                    throw new RecordNotFoundException(typeof(Funcao));
                }
            }
            catch (RecordNotFoundException rex)
            {
                base.ExibirMensagem("Não foi possível gerar Alerta! " + rex.Message, TipoMensagem.Erro, false);
            }
            finally
            {
                var textBox = sender as TextBox;
                if (textBox != null) textBox.Text = "";
            }
        }
        #endregion

        #region repeater_ItemCommand
        protected void repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //Inativa um elemento dado um comando
            var nmRow = "IdFuncao";
            foreach (DataRow row in Funcoes.Rows)
            {
                if (row[nmRow].ToString() == e.CommandArgument.ToString())
                {
                    row.Delete();
                    OnFuncoesAlteradas();
                    break;
                }
            }
            CarregarRepeater();
        }
        #endregion

    }
}