using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Web.Script.Serialization;

[assembly: WebResource("Employer.Componentes.UI.Web.Content.js.jquery.uploadify.js", "text/javascript")]
[assembly: WebResource("Employer.Componentes.UI.Web.Content.js.EmployerMultipleUpload.js", "text/javascript")]
[assembly: WebResource("Employer.Componentes.UI.Web.Content.Styles.uploadify.css", "text/css")]
[assembly: WebResource("Employer.Componentes.UI.Web.Content.swf.uploadify.swf", "application/x-shockwave-flash")]
[assembly: WebResource("Employer.Componentes.UI.Web.Content.Images.cancel.png", "image/png")]

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente que realizar upload de vários arquivos em paralelo.<br/>
    /// Neste componente existe também uma grid com a lista dos arquivos que o usuário subiu para o servidor.<br/>
    /// </summary>
    public class EmployerMultipleUpload : ControlBaseValidator
    {
        /// <summary>
        /// Delegate usado para informar erro ocorridos no Upload
        /// </summary>
        /// <param name="sErros"></param>
        public delegate void ErrosNoUploadHandler(IList<ErroUpload> sErros);

        /// <summary>
        /// Evento disparado ao terminar o upload antes de atulizar a grid.
        /// </summary>
        public event EventHandler UploadCompletoAntesAtualizarGrid;

        /// <summary>
        /// Evento disparado ao terminar o upload depois de atulizar a grid.
        /// </summary>
        public event EventHandler UploadCompletoDepoisAtualizarGrid;

        /// <summary>
        /// Evento disparado quando ocorrer erros no upload
        /// </summary>
        public event ErrosNoUploadHandler ErrosNoUpload;

        #region atributos
        private Panel _PainelContainer = new Panel();
        private FileUpload _FileUpload = new FileUpload();
        private TextBox _txtCampoVal = new TextBox();
        private CustomValidator _cvValor = new CustomValidator();
        private Button _btnAtualizarGrid = new Button();
        private Button _btnErroUpload = new Button();
        private EmployerGrid _GridArquivos = new EmployerGrid();
        private HiddenField _hdnErros = new HiddenField();
        private UpdatePanel _upGrid = new UpdatePanel
        {
            ID = "upGrid",
            UpdateMode = UpdatePanelUpdateMode.Conditional
        };
        #endregion

        #region Propriedades

        #region Propriedades do ControlBaseValidator
        /// <inheritdoc/>
        protected override TextBox CampoTexto
        {
            get { return _txtCampoVal; }
        }

        /// <inheritdoc/>
        protected override BaseValidator ValidadorTexto
        {
            get { return _cvValor; }
        }

        /// <inheritdoc/>
        protected override System.Web.UI.WebControls.Panel PanelValidador
        {
            get { return _PainelContainer; }
        }

        /// <inheritdoc/>
        public override string ValidationGroup
        {
            get
            {
                return base.ValidationGroup;
            }
            set
            {
                this._btnAtualizarGrid.ValidationGroup = 
                base.ValidationGroup = value;
            }
        }

        /// <inheritdoc/>
        public override bool CausesValidation
        {
            get
            {
                return base.CausesValidation;
            }
            set
            {
                this._btnAtualizarGrid.CausesValidation = 
                base.CausesValidation = value;
            }
        }

        #region Obrigatorio
        /// <summary>
        /// Define se o campo é obrigatório.
        /// </summary>
        [Category("Validador"), DisplayName("Obrigatório")]
        public override bool Obrigatorio
        {
            get { return base.Obrigatorio; }
            set
            {
                base.Obrigatorio = value;
                _cvValor.ValidateEmptyText = value;
            }
        }
        #endregion

        #endregion

        #region Campos validação
        /// <summary>
        /// Lista de extenções permitidas para upload.<br/>
        /// <code>*.exe;*.pdf</code>
        /// </summary>
        [Browsable(true)]
        public string ExtensoesArquivos
        {
            get { return this.ViewState["ExtensoesArquivos"] as string; }
            set { this.ViewState["ExtensoesArquivos"] = value; }
        }
        #endregion

        /// <summary>
        /// A descrição dos arquivos selecionáveis.<br/>
        /// Esta seqüência aparece na caixa de diálogo de arquivos de navegação no tipo de arquivo suspenso.
        /// </summary>
        [Browsable(true)]
        public string DesExtensoesArquivos
        {
            get { return this.ViewState["DesExtensoesArquivos"] as string; }
            set { this.ViewState["DesExtensoesArquivos"] = value; }
        }

        /// <summary>
        /// Imagem do botão de upload
        /// </summary>
        [DefaultValue("")]
        [UrlProperty]
        [Browsable(true)]
        public string ImagemBotao
        {
            get { return this.ViewState["ImagemBotao"] as string; }
            set { this.ViewState["ImagemBotao"] = value; }
        }

        /// <summary>
        /// Texto do botão de upload
        /// </summary>
        public string TextoBotao
        {
            get {
                string textoBotao = this.ViewState["TextoBotao"] as string;
                if (string.IsNullOrEmpty(textoBotao))
                    return "Fazer Upload";
                return textoBotao; 
            }
            set { this.ViewState["TextoBotao"] = value; }
        }

        private Guid? IdArquivos
        {
            get { return this.ViewState["IdArquivos"] as Guid?; }
            set { this.ViewState["IdArquivos"] = value; }
        }

        private DirectoryInfo PastaUpload
        {
            get
            {
                DirectoryInfo divTemp = new DirectoryInfo(this.Page.Server.MapPath("~/Temp"));
                if (!divTemp.Exists)
                    divTemp.Create();
                return divTemp;
            }
        }

        #region css

        /// <summary>
        /// Css aplicado no input de arquivo
        /// </summary>
        [CssClassProperty]
        [DefaultValue("")]
        public string CssBotao
        {
            get { return this.ViewState["CssBotao"] as string; }
            set { this.ViewState["CssBotao"] = value; }
        }
        #endregion

        /// <summary>
        /// Lista dos arquivos updados
        /// </summary>
        public FileInfo[] Arquivos
        {
            get
            {
                return PastaUpload.GetFiles(string.Format("Arquivo_{0}*", IdArquivos));
            }
        }

        /// <summary>
        /// Indica se a grid com a lista de arquivos está visível.
        /// </summary>
        public bool GridArquivosCompletosVisivel
        {
            get { EnsureChildControls(); return _GridArquivos.Visible; }
            set { EnsureChildControls(); _GridArquivos.Visible = value; }
        }

        /// <summary>
        /// Url do handler de upload
        /// </summary>
        public string CaminhoUpload
        {
            get { EnsureChildControls(); return ViewState["CaminhoUpload"] == null ? ConfigurationManager.AppSettings["CaminhoUploadHandler"] : (string) ViewState["CaminhoUpload"]; }
            set { EnsureChildControls(); ViewState["CaminhoUpload"] = value; }
        }

        /// <summary>
        /// Tamanho limite para upload do arquivo em B, KB, MB ou GB.<br/>
        /// Ex.: 100KB
        /// </summary>
        public string TamanhoLimiteArquivo
        {
            get { EnsureChildControls(); return ViewState["TamanhoLimiteArquivo"] == null ? "0KB" : (string) ViewState["TamanhoLimiteArquivo"]; }
            set { EnsureChildControls(); ViewState["TamanhoLimiteArquivo"] = value; }
        }

        /// <summary>
        /// Mensagem de Erro quando o usuário tenta informar um arquivo maior que o limite configurado na propriedade TamanhoLimiteArquivo.<br/>
        /// Formatação do texto: {0} nome do arquivo {1} tamanho do arquivo.<br/>
        /// Menságem padrão: O arquivo {0} excede o limite de tamanho ({1}).
        /// </summary>
        public string MensagemErroTamanhoLimiteArquivo
        {
            get { EnsureChildControls(); return ViewState["MensagemErroTamanhoLimiteArquivo"] == null ?
                "O arquivo {0} excede o limite de tamanho ({1}) ." : (string)ViewState["MensagemErroTamanhoLimiteArquivo"];
            }
            set { EnsureChildControls(); ViewState["MensagemErroTamanhoLimiteArquivo"] = value; }
        }

        /// <summary>
        /// Mensagem de Erro quando o usuário tenta passar um arquivo em branco.<br/>
        /// Formatação do texto: {0} nome do arquivo.<br/>
        /// Menságem padrão: O upload do arquivo \"{0}\" não foi realizado porque ele está vazio.
        /// </summary>
        public string MensagemErroArquivoEmBranco
        {
            get
            {
                EnsureChildControls(); return ViewState["MensagemErroArquivoEmBranco"] == null ?
              "O upload do arquivo \"{0}\" não foi realizado porque ele está vazio." : (string)ViewState["MensagemErroArquivoEmBranco"];
            }
            set { EnsureChildControls(); ViewState["MensagemErroArquivoEmBranco"] = value; }
        }

        /// <summary>
        /// Define quantos arquivos podem ser upados ao mesmo tempo.<br/>
        /// Valor padrão 999
        /// </summary>
        public int TamanhoFilaDeUpload
        {
            get { EnsureChildControls(); return ViewState["TamanhoQtdLimiteArquivos"] == null ? 999 : (int)ViewState["TamanhoQtdLimiteArquivos"]; }
            set { EnsureChildControls(); ViewState["TamanhoQtdLimiteArquivos"] = value; }
        }

        /// <summary>
        /// Erro quando o usuário informa um número maior de arquivos configurado em TamanhoFilaDeUpload.
        /// </summary>
        public string MensagemErroQtdLimiteArquivos
        {
            get { EnsureChildControls(); return ViewState["MensagemErroQtdLimiteArquivos"] == null ?
                "O número de arquivos selecionados excede o limite de tamanho da fila de upload ({0})" : (string)ViewState["MensagemErroQtdLimiteArquivos"];
            }
            set { EnsureChildControls(); ViewState["MensagemErroQtdLimiteArquivos"] = value; }
        }
        #endregion

        #region Métodos

        private void AtualizarGrid()
        {
            if (!_GridArquivos.Visible)
                return;

            DataTable tbArquivos = new DataTable();
            tbArquivos.Columns.Add("Arquivo", typeof(string));
            tbArquivos.Columns.Add("NomeCompleto", typeof(string));

            foreach (var arquivo in this.Arquivos)
            {
                var linha = tbArquivos.NewRow();
                linha["NomeCompleto"] = arquivo.Name;

                var ponto = arquivo.Name.LastIndexOf('.');
                var extensao = arquivo.Extension;

                var nome = arquivo.Name.Substring(0, ponto);
                var tag = "NomeAmigavel_";
                var n = nome.IndexOf(tag);
                nome = nome.Substring(n + tag.Length) + extensao;
                linha["Arquivo"] = nome;

                tbArquivos.Rows.Add(linha);
            }


            _GridArquivos.DataSource = tbArquivos;
            _GridArquivos.DataBind();
            _upGrid.Update();
        }

        /// <summary>
        /// Apaga todos os arquivo upados
        /// </summary>
        public void LimparArquivos()
        {
            var arquivos = Arquivos;
            for (int i = 0; arquivos.Length > i; i++)
                arquivos[i].Delete();

            this._txtCampoVal.Text = string.Empty;

            AtualizarGrid();
        }

        #region override
        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            

            if (!IdArquivos.HasValue)
                IdArquivos = Guid.NewGuid();

            string styleSheet = "Employer.Componentes.UI.Web.Content.Styles.uploadify.css";
            string pathCss = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), styleSheet);
            if (this.Page.Header.FindControl(styleSheet) == null)
            {
                HtmlLink cssLink = new HtmlLink();
                cssLink.ID = styleSheet;
                cssLink.Href = pathCss;
                cssLink.Attributes.Add("rel", "stylesheet");
                cssLink.Attributes.Add("type", "text/css");
                this.Page.Header.Controls.Add(cssLink);
            }

            if (string.IsNullOrEmpty(this.CssClass))
                this.CssClass = "uploadify-container";
        }

        /// <inheritdoc/>
        protected override void OnInit(EventArgs e)
        {
            _cvValor.ServerValidate += new ServerValidateEventHandler(_cvValor_ServerValidate);
            _btnAtualizarGrid.Click += new EventHandler(_btnAtualizarGrid_Click);
            _btnErroUpload.Click += new EventHandler(_btnErroUpload_Click);
            _GridArquivos.RowDataBound += new GridViewRowEventHandler(_GridArquivos_RowDataBound);
            _GridArquivos.RowCreated += new GridViewRowEventHandler(_GridArquivos_RowCreated);

            base.OnInit(e);
        }

        /// <inheritdoc/>
        protected override void OnPreRender(EventArgs e)
        {
            if (_FileUpload.HasFile)
                _txtCampoVal.Text = string.Empty;
            base.OnPreRender(e);
        }

        #region CreateChildControls
        /// <summary>
        /// Cria os controles filhos
        /// </summary>
        protected override void CreateChildControls()
        {
            if (DesignMode)
            {
                Controls.Add(new Literal { Text = "Componente EmployerMultipleUpload" });
                return;
            }

            InicializarTextBox();
            InicializarValidator();
            InicializarPainel();

            Controls.Add(CampoTexto);
            Controls.Add(_hdnErros);            

            InicializarFileUpload();
            InicializarGridArquivos();

            base.CreateChildControls();
        }

        private void InicializarFileUpload()
        {
            _FileUpload.ID = "CampoArquivo";
            Controls.Add(_FileUpload);
        }

        private void InicializarTextBox()
        {
            _txtCampoVal.ID = "txtCampoVal";
            _txtCampoVal.Style[HtmlTextWriterStyle.Display] = "none";

            _hdnErros.ID = "hdnErros";
            //_hdnErros.EnableViewState = false;
        }

        private void InicializarGridArquivos()
        {
            _btnAtualizarGrid.ID = "btnAtualizarGrid";
            _btnAtualizarGrid.Style[HtmlTextWriterStyle.Display] = "none";
            
            _btnErroUpload.ID = "btnErroUpload";
            _btnErroUpload.Style[HtmlTextWriterStyle.Display] = "none";
            _btnErroUpload.CausesValidation = false;

            _upGrid.ContentTemplateContainer.Controls.Add(_btnAtualizarGrid);
            _upGrid.ContentTemplateContainer.Controls.Add(_btnErroUpload);

            _GridArquivos.ID = "GridArquivos";
            _GridArquivos.AutoGenerateColumns = false;
            _GridArquivos.Columns.Add(new BoundField
            {
                DataField = "Arquivo",
                HeaderText = "Arquivo"
            });

            _GridArquivos.Columns.Add(new TemplateField
            {
                HeaderText = "Excluir",
                ItemTemplate = new ColunaAcao()
            });
            
            _upGrid.ContentTemplateContainer.Controls.Add(_GridArquivos);

            Controls.Add(_upGrid); 
        }

        void _btnErroUpload_Click(object sender, EventArgs e)
        {
            if (ErrosNoUpload != null && !string.IsNullOrEmpty(_hdnErros.Value))
            {
                var js = new JavaScriptSerializer();
                var erros = js.Deserialize<IList<ErroUpload>>(_hdnErros.Value);

                ErrosNoUpload(erros);
            }
            _hdnErros.Value = string.Empty;
        }

        void _btnAtualizarGrid_Click(object sender, EventArgs e)
        {
            _btnErroUpload_Click(sender, e);

            if (UploadCompletoAntesAtualizarGrid != null)
                UploadCompletoAntesAtualizarGrid(sender, e);

            AtualizarGrid();

            if (UploadCompletoDepoisAtualizarGrid != null)
                UploadCompletoDepoisAtualizarGrid(sender, e);
        }

        void _GridArquivos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnExcluir = e.Row.FindControl("btnExcluir") as ImageButton;

                var url = this.Page.ClientScript.GetWebResourceUrl(
                    this.GetType(), "Employer.Componentes.UI.Web.Content.Images.ico_excluir_16x16.gif");

                btnExcluir.ImageUrl = url;
                btnExcluir.Click += new ImageClickEventHandler(btnExcluir_Click);
            }
        }

        void btnExcluir_Click(object sender, ImageClickEventArgs e)
        {
            var btnExcluir = sender as ImageButton;
            var nomeArquivo = btnExcluir.CommandArgument;

            var arquivo = this.Arquivos.FirstOrDefault(a => a.Name.Equals(nomeArquivo, StringComparison.InvariantCultureIgnoreCase));
            if (arquivo.Exists)
                arquivo.Delete();

            AtualizarGrid();
        }

        void _GridArquivos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var linha = e.Row.DataItem as DataRowView;

                ImageButton btnExcluir = e.Row.FindControl("btnExcluir") as ImageButton;
                btnExcluir.CommandArgument = linha["NomeCompleto"].ToString();
            }
        }

        /// <summary>
        /// Inicializa o validador
        /// </summary>
        protected new void InicializarValidator()
        {
            base.InicializarValidator();
            ValidadorTexto.ErrorMessage = String.Empty;
            _cvValor.Display = ValidatorDisplay.Dynamic;
            _cvValor.EnableClientScript = true;
            _cvValor.ID = "cvValor";
            _cvValor.ControlToValidate = this.CampoTexto.ID;
            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.EmployerMultipleUpload.ValidarTextBox";
        }
        #endregion

        #region SubClasses
        /// <inheritdoc/>
        public class ColunaAcao : ITemplate
        {
            /// <inheritdoc/>
            public void InstantiateIn(Control container)
            {
                ImageButton btnExcluir = new ImageButton
                {
                    //ImageUrl = url,
                    ID = "btnExcluir",
                };

                container.Controls.Add(btnExcluir);
            }
        }

        /// <summary>
        /// Objeto utilizado para informar erros de upload
        /// </summary>
        [Serializable]
        public class ErroUpload
        {
            /// <summary>
            /// Menságem de erro
            /// </summary>
            public string Mensagem { get; set; }

            /// <summary>
            /// Código do erro
            /// </summary>
            public int Codigo { get; set; }
        }
        #endregion

        #region Script
        /// <inheritdoc/>
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {   
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.EmployerMultipleUpload", this.ClientID);

            this.SetScriptDescriptors(descriptor);

            //descriptor.AddProperty("RegexValidacao", this.RegexValidacao);
            descriptor.AddProperty("CaminhoSWF",
                this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Employer.Componentes.UI.Web.Content.swf.uploadify.swf"));
                //"http://localhost:53482/swf/uploadify.swf");
            descriptor.AddProperty("ImgBotaoCancelar",
                this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Employer.Componentes.UI.Web.Content.Images.cancel.png"));

            descriptor.AddProperty("CaminhoUpload", CaminhoUpload);
            descriptor.AddProperty("IdArquivos", IdArquivos.Value.ToString());
            descriptor.AddProperty("ExtensoesArquivos", ExtensoesArquivos);
            descriptor.AddProperty("DesExtensoesArquivos", DesExtensoesArquivos);
            descriptor.AddProperty("ImgBotao",  string.IsNullOrEmpty(this.ImagemBotao) ? string.Empty : this.ResolveUrl(this.ImagemBotao));
            descriptor.AddProperty("CssBotao", this.CssBotao);
            descriptor.AddProperty("TextoBotao", this.TextoBotao);
            descriptor.AddProperty("TamanhoLimiteArquivo", this.TamanhoLimiteArquivo);
            descriptor.AddProperty("MensagemErroTamanhoLimiteArquivo", this.MensagemErroTamanhoLimiteArquivo);
            descriptor.AddProperty("MensagemErroArquivoEmBranco", this.MensagemErroArquivoEmBranco);
            descriptor.AddProperty("TamanhoQtdLimiteArquivos", this.TamanhoFilaDeUpload);
            descriptor.AddProperty("MensagemErroQtdLimiteArquivos", this.MensagemErroQtdLimiteArquivos);
            descriptor.AddProperty("ImplementaEventoDeErro", this.ErrosNoUpload != null);

            return new ScriptControlDescriptor[] { descriptor };
        }

        /// <inheritdoc/>
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            references.Add(new ScriptReference
                {
                    Assembly = "Employer.Componentes.UI.Web",
                    Name = "Employer.Componentes.UI.Web.Content.js.jquery.uploadify.js"
                });

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.EmployerMultipleUpload.js";
            references.Add(reference);

            return references;
        }
        #endregion
        #endregion

        #region _cvValor_ServerValidate
        /// <summary>
        /// Dispara a validação server-side do validador
        /// </summary>
        /// <param name="source">O controle que disparou a ação</param>
        /// <param name="args">Os argumentos do evento</param>
        private void _cvValor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (this.Obrigatorio)
            {
                args.IsValid = !String.IsNullOrEmpty(this.Text);
                this._cvValor.IsValid = args.IsValid;
                this.ValidadorTexto.ErrorMessage = this.MensagemErroObrigatorio;
            }
        }
        #endregion

        #endregion
    }
}

