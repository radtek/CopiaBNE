using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Employer.Componentes.UI.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Configuration;
using Employer.Componentes.UI.Web.Interface;
using System.Drawing.Design;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Classe base para o localizar com treeview
    /// </summary>
    public abstract class LocalizarTreeViewBase : CompositeControl, IRequiredField, IMensagemErro
    {
        #region Atributos
        ControlAutoCompleteBase _AutoComplete = new ControlAutoCompleteBase();
        ControlModalBuscaTreeView _ModalBusca = new ControlModalBuscaTreeView();
        HiddenField _hdf = new HiddenField();
        #endregion

        #region Propriedades

        #region ModalBusca
        /// <summary>
        /// A janela modal de consulta
        /// </summary>
        public ControlModalBuscaTreeView ModalBusca
        {
            get 
            {
                EnsureChildControls();
                return _ModalBusca; 
            }
        }
        #endregion

        #region AutoComplete
        /// <summary>
        /// O campo de auto complete
        /// </summary>
        public ControlAutoCompleteBase AutoComplete
        {
            get 
            {
                EnsureChildControls();
                return _AutoComplete; 
            }
        }
        #endregion

        #region AutoWidth
        /// <summary>
        /// A largura do campo de auto complete
        /// </summary>
        public Unit AutoWidth
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.Width; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.Width = value; 
            }
        }
        #endregion

        #region AutoHeight
        /// <summary>
        /// A altura do campo de auto complete
        /// </summary>
        public Unit AutoHeight
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.Height; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.Height = value; 
            }
        }
        #endregion

        #region AutoColumns
        /// <summary>
        /// A quantidade de colunas do campo de auto complete
        /// </summary>
        public int AutoColumns
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.Columns; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.Columns = value; 
            }
        }
        #endregion

        #region ModalWidth
        /// <summary>
        /// A largura da modal de consulta
        /// </summary>
        public Unit ModalWidth
        {
            get 
            {
                EnsureChildControls();
                return ModalBusca.Width; 
            }
            set 
            {
                EnsureChildControls();
                ModalBusca.Width = value; 
            }
        }
        #endregion

        #region ModalHeight
        /// <summary>
        /// A altura da modal de consulta
        /// </summary>
        public Unit ModalHeight
        {
            get 
            {
                EnsureChildControls();
                return ModalBusca.Height; 
            }
            set 
            {
                EnsureChildControls();
                ModalBusca.Height = value; 
            }
        }
        #endregion

        #region ModalBuscaWidth
        /// <summary>
        /// A largura do campo texto da modal de consulta
        /// </summary>
        public Unit ModalBuscaWidth
        {
            get 
            {
                EnsureChildControls();
                return ModalBusca.BuscaWidth; 
            }
            set 
            {
                EnsureChildControls();
                ModalBusca.BuscaWidth = value; 
            }
        }
        #endregion

        #region Cadastro
        /// <summary>
        /// Endereço da tela de cadastro.
        /// </summary>
        public string CadastroUrl
        {
            get { return this.AutoComplete.CadastroUrl; }
            set { this.AutoComplete.CadastroUrl = value; }
        }

        /// <summary>
        /// Nome do parâmetro de sessão para passar o valor informado no campo texto do autocomplete para e tela de cadastro.
        /// </summary>
        public string CadastroParametroSessao
        {
            get { return this.AutoComplete.CadastroParametroSessao; }
            set { this.AutoComplete.CadastroParametroSessao = value; }
        }

        /// <summary>
        /// Nome do parâmetro da url para passar o valor informado no campo texto do autocomplete para e tela de cadastro.
        /// </summary>
        public string CadastroParametroUrl
        {
            get { return this.AutoComplete.CadastroParametroUrl; }
            set { this.AutoComplete.CadastroParametroUrl = value; }
        }

        /// <summary>
        /// Nome do parâmetro de sessão para passar do código informado no autocomplete para a tela de cadastro.
        /// </summary>
        public string CadastroParametroSessaoCodigo
        {
            get { return this.AutoComplete.CadastroParametroSessaoCodigo; }
            set { this.AutoComplete.CadastroParametroSessaoCodigo = value; }
        }

        /// <summary>
        /// Nome do parâmetro da url para passar do código informado no autocomplete para a tela de cadastro.
        /// </summary>
        public string CadastroParametroUrlCodigo
        {
            get { return this.AutoComplete.CadastroParametroUrlCodigo; }
            set { this.AutoComplete.CadastroParametroUrlCodigo = value; }
        }

        /// <summary>
        /// Caminho da imagem para o botão que ativa a modal de busca
        /// </summary>
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [Bindable(true)]
        public string ImagemBotaoCadastro
        {
            get { return this.AutoComplete.CadastroBotao.ImageUrl; }
            set { this.AutoComplete.CadastroBotao.ImageUrl = value; }
        }

        /// <summary>
        /// Classe Css do botão que ativa a modal de busca
        /// </summary>
        public string CssBotaoCadastro
        {
            get { return this.AutoComplete.CadastroBotao.CssClass; }
            set { this.AutoComplete.CadastroBotao.CssClass = value; }
        }
        #endregion

        #region Texto
        /// <summary>
        /// O texto atualmente no auto complete
        /// </summary>
        public string Texto
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.Text; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.Text = value; 
            }
        }
        #endregion

        #region LabelVisivel
        /// <summary>
        /// Define a visibilidade do label do auto complete
        /// </summary>
        public bool LabelVisivel
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.LabelVisivel; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.LabelVisivel = value; 
            }
        }
        #endregion

        #region LabelTexto
        /// <summary>
        /// Define o texto do label do auto complete
        /// </summary>
        public string LabelTexto
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.LabelTexto; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.LabelTexto = value; 
            }
        }
        #endregion

        #region Obrigatorio
        /// <summary>
        /// Define se o campo é obrigatório ou não
        /// </summary>
        public bool Obrigatorio
        {
            get
            {
                EnsureChildControls();
                return _AutoComplete.Obrigatorio;
            }
            set
            {
                EnsureChildControls();
                _AutoComplete.Obrigatorio = value;
            }
        }
        #endregion

        #region ValidationGroup
        /// <summary>
        /// Define o grupo de validação
        /// </summary>
        public string ValidationGroup
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.ValidationGroup; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.ValidationGroup = value; 
            }
        }
        #endregion

        #region ValorAlteradoClient
        /// <summary>
        /// Evento javascript disparado quando o conteúdo do controle muda
        /// </summary>
        public string ValorAlteradoClient
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.ValorAlteradoClient; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.ValorAlteradoClient = value; 
            }
        }
        #endregion

        #region ValorAlterado
        /// <summary>
        /// Evento de valor alterado
        /// </summary>
        public event EventHandler ValorAlterado
        {
            add { AutoComplete.ValorAlterado += value; }
            remove { AutoComplete.ValorAlterado -= value; }
        }
        #endregion

        #region AutoCustomServerValidate
        /// <summary>
        /// Evento disparado pelo custom validator
        /// </summary>
        public event ServerValidateEventHandler AutoCustomServerValidate
        {
            add { AutoComplete.CustomServerValidate += value; }
            remove { AutoComplete.CustomServerValidate -= value; }
        }
        #endregion

        #region AutoCustomClientValidate
        /// <summary>
        /// Evento cliente disparado pelo custom validator
        /// </summary>
        public string AutoCustomClientValidate
        {
            get { return AutoComplete.CustomClientValidate; }
            set { AutoComplete.CustomClientValidate = value; }
        }
        #endregion

        #region SkinIDModalBusca
        /// <summary>
        /// ID do Skin da modal de pesquisa
        /// </summary>
        [Browsable(true)]
        [Themeable(true)]
        public string SkinIDModalBusca
        {
            get { return ModalBusca.SkinID; }
            set { ModalBusca.SkinID = value; }
        }
        #endregion
        
        #region SkinIDAutoComplete
        /// <summary>
        /// ID do Skin do auto complete
        /// </summary>
        [Browsable(true)]
        [Themeable(true)]
        public string SkinIDAutoComplete
        {
            get { return AutoComplete.SkinID; }
            set { AutoComplete.SkinID = value; }
        }
        #endregion

        #region GridSkinID
        /// <summary>
        /// ID do Skin da grid de pesquisa
        /// </summary>
        [Browsable(true)]
        [Themeable(true)]
        public string GridSkinID
        {
            get { return ModalBusca.SkinID; }
            set { ModalBusca.SkinID = value; }
        }
        #endregion

        #region Enabled
        /// <inheritdoc />
        public override bool Enabled
        {
            get
            {
                EnsureChildControls();
                return _AutoComplete.Enabled;
            }
            set
            {
                EnsureChildControls();
                base.Enabled = value;
                _AutoComplete.Enabled = value;
            }
        }
        #endregion

        #region BotaoBuscaVisivel
        /// <summary>
        /// Define a visibilidade do botão de busca
        /// </summary>
        public Boolean BotaoBuscaVisivel
        {
            get 
            {
                EnsureChildControls();
                return _AutoComplete.BotaoBuscaVisivel; 
            }
            set 
            {
                EnsureChildControls();
                this._AutoComplete.BotaoBuscaVisivel = value; 
            }
        }
        #endregion

        #region TipoApresentacaoPesquisa
        /// <summary>
        /// Define qual é o modo de apresentação da modal de pesquisa
        /// Tabela: Modo de exibição em tabela
        /// Estrutura: Modo de exibição em árvore
        /// </summary>
        public Employer.Componentes.UI.Web.ControlModalBuscaTreeView.ModosExibicao TipoApresentacaoPesquisa
        {
            get
            {
                EnsureChildControls();
                return _ModalBusca.TipoApresentacaoPesquisa;
            }
            set
            {
                EnsureChildControls();
                _ModalBusca.TipoApresentacaoPesquisa = value;
            }
        }
        #endregion

        #region TipoApresentacaoEstrutura
        /// <summary>
        /// Quando no modo estrutura define se a estrutura será apresentada:
        /// Fechada: Nós retraídos
        /// Aberta: Nós abertos
        /// </summary>
        public Employer.Componentes.UI.Web.ControlModalBuscaTreeView.ExibicaoEstrutura TipoApresentacaoEstrutura
        {
            get
            {
                EnsureChildControls();
                return _ModalBusca.TipoApresentacaoEstrutura;
            }
            set
            {
                EnsureChildControls();
                _ModalBusca.TipoApresentacaoEstrutura = value;
            }
        }
        #endregion

        #region MensagemErroFormato
        /// <summary>
        /// Define a mensagem de erro de formato inválido
        /// </summary>
        public String MensagemErroFormato
        {
            get
            {
                EnsureChildControls();
                return this.AutoComplete.MensagemErroFormato;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.MensagemErroFormato = value;
            }
        }
        #endregion

        #region MensagemErroObrigatorio
        /// <summary>
        /// Define a mensagem de erro de campo obrigatório
        /// </summary>
        public String MensagemErroObrigatorio
        {
            get
            {
                EnsureChildControls();
                return this.AutoComplete.MensagemErroObrigatorio;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.MensagemErroObrigatorio = value;
            }
        }
        #endregion

        #region MensagemErroFormatoSummary
        /// <summary>
        /// Define a mensagem de erro de formato inválido
        /// </summary>
        public String MensagemErroFormatoSummary
        {
            get
            {
                EnsureChildControls();
                return this.AutoComplete.MensagemErroFormatoSummary;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.MensagemErroFormatoSummary = value;
            }
        }
        #endregion

        #region MensagemErroObrigatorioSummary
        /// <summary>
        /// Define a mensagem de erro de campo obrigatório
        /// </summary>
        public String MensagemErroObrigatorioSummary
        {
            get
            {
                EnsureChildControls();
                return this.AutoComplete.MensagemErroObrigatorioSummary;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.MensagemErroObrigatorioSummary = value;
            }
        }
        #endregion

        #region MensagemErroInvalidoSummary
        /// <summary>
        /// Sem funcionalidade. Obrigado a implementar por causa da Interface.
        /// </summary>
        public string MensagemErroInvalidoSummary
        {
            get
            {
                EnsureChildControls();
                return this.AutoComplete.MensagemErroInvalidoSummary;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.MensagemErroInvalidoSummary = value;
            }
        }
        #endregion

        #region TabIndex
        /// <inheritdoc/>
        public override short TabIndex
        {
            get
            {
                EnsureChildControls();
                return _AutoComplete.TabIndex;
            }
            set
            {
                EnsureChildControls();
                _AutoComplete.TabIndex = value;
            }
        }
        #endregion

        #endregion

        #region Metodos

        #region Focus
        /// <inheritdoc />
        public override void Focus()
        {
            AutoComplete.Focus();
        }
        #endregion

        #region ExecutarValorAlterado
        /// <summary>
        /// Dispara o evento de valor alterado
        /// </summary>
        public void ExecutarValorAlterado()
        {
            this.AutoComplete.ExecutarValorAlterado();

        }
        #endregion

        #region LimparCampos
        /// <summary>
        /// Limpa o campo do auto complete
        /// </summary>
        public void LimparCampos()
        {
            AutoComplete.Codigo = string.Empty;
            AutoComplete.Text = string.Empty;
        }
        #endregion

        #region CreateChildControls
        /// <inheritdoc />
        protected override void CreateChildControls()
        {
            _AutoComplete.ID = "AutoComplete";            
            this.Controls.Add(_AutoComplete);

            _ModalBusca.ID = "ModalBusca";
            this.Controls.Add(_ModalBusca);

            _hdf.ID = "hdnModal";
            this.Controls.Add(_hdf);

            base.CreateChildControls();
        }
        #endregion

        #region GetKey
        /// <summary>
        /// Retorna a campo chave a partir de um objeto da fonte de dados
        /// </summary>
        /// <param name="DataItem">O objeto da fonte de dados</param>
        /// <returns>Um objeto representado o campo chave na fonte de dados</returns>
        public abstract object GetKey(object DataItem);
        #endregion

        #region GetValue
        /// <summary>
        /// Retorna o campo valor a partir de um objeto da fonte de dados
        /// </summary>
        /// <param name="DataItem">O objeto da fonte de dados</param>
        /// <returns>Um objeto representando o campo valor na fonte de dados</returns>
        public abstract object GetValue(object DataItem);
        #endregion

        #region LoadByKey
        /// <summary>
        /// Carrega um registro a partir do código informado ao autocomplete
        /// </summary>
        protected abstract void LoadByKey();
        #endregion

        #region OnInit
        /// <inheritdoc />
        protected override void OnInit(EventArgs e)
        {
            _ModalBusca.AtualizaLinhasGrid += new GridViewRowEventHandler(_ModalBusca_AtualizaLinhasGrid);
            _AutoComplete.CarregarPorCodigo += new EventHandler(_AutoComplete_CarregarPorCodigo);
            _ModalBusca.AtualizaNo += new ControlModalBuscaTreeView.HandlerAtualizaNodes(_ModalBusca_NoSelecionado);
            _AutoComplete.BotaoBusca.Click += new ImageClickEventHandler(BotaoBusca_Click);            

            base.OnInit(e);
        }
        #endregion

        /// <inheritdoc/>
        protected override void OnPreRender(EventArgs e)
        {
            ModalBusca.TargetControlID = _hdf.ID;

            _AutoComplete.IdModalAjax = _ModalBusca.Modal.ClientID;
            _AutoComplete.IdModalEstado = _ModalBusca.IdModalEstado;

            base.OnPreRender(e);
        }

        #region BotaoBusca_Click
        /// <summary>
        /// Tratador do evento disparado quando o usuário clica no botão que exibe a modal.  
        /// </summary>
        /// <param name="sender">O objeto que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void BotaoBusca_Click(object sender, ImageClickEventArgs e)
        {
            _ModalBusca.Show();
        }
        #endregion

        #region _ModalBusca_NoSelecionado
        /// <summary>
        /// Evento disparado quando um nó é selecionado na árvore
        /// </summary>
        /// <param name="dataKey">O objeto da fonte de dados</param>
        void _ModalBusca_NoSelecionado(object dataKey)
        {
            //_AutoComplete.Codigo = Convert.ToString(dataKey);                
            TreeNode node = (TreeNode)dataKey;

            node.NavigateUrl = string.Format("javascript:$find('{0}').CarregarPorCodigo('{1}','{2}')",
                    _AutoComplete.ClientID,
                    GetKey(node.DataItem), GetValue(node.DataItem));

        }
        #endregion

        #region _AutoComplete_CarregarPorCodigo
        /// <summary>
        /// Tratador do evento disparado disparado ao informar o código do ítem a ser carregado.  
        /// </summary>
        /// <param name="sender">O objeto que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _AutoComplete_CarregarPorCodigo(object sender, EventArgs e)
        {
            this.LoadByKey();
        }
        #endregion

        #region _ModalBusca_AtualizaLinhasGrid
        /// <summary>
        /// Tratador do evento disparado quando está atualizando as linhas da grid.  
        /// </summary>
        /// <param name="sender">O objeto que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _ModalBusca_AtualizaLinhasGrid(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                object oKey = this.GetKey(e.Row.DataItem);
                string sValue = System.Web.HttpUtility.HtmlEncode(this.GetValue(e.Row.DataItem).ToString());

                foreach (TableCell c in e.Row.Cells)
                {
                    c.Attributes["onclick"] = string.Format("javascript:$find('{0}').CarregarPorCodigo('{1}','{2}')",
                    _AutoComplete.ClientID,
                    oKey, sValue);
                }
            }
        }
        #endregion

        #endregion
    }
}
