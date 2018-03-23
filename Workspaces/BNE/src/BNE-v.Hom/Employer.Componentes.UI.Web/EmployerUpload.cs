using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente de upload de arquivo.<br/>
    /// Este componente tem um FileUpload e um validador embutido.
    /// </summary>
    public class EmployerUpload : ControlBaseValidator
    {
        #region atributos
        private Panel _PainelContainer = new Panel();
        private FileUpload _FileUpload = new FileUpload();
        private TextBox _txtCampoVal = new TextBox();
        private CustomValidator _cvValor = new CustomValidator();
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

        #region Campos do FileUpload
        /// <summary>
        /// Indica se tem arquivo informado
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public bool HasFile
        {
            get
            {
                return _FileUpload.HasFile;
            }
        }

        /// <summary>
        /// Nome do arquivo
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string FileName { get { return _FileUpload.FileName; } }

        /// <summary>
        /// Obtém um array de bytes do arquivo
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public byte[] FileBytes { get { return _FileUpload.FileBytes; } }

        /// <summary>
        /// Obtém um Stream de bytes do arquivo
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Stream FileContent { get { return _FileUpload.FileContent; } }

        /// <summary>
        /// Obtém o objeto System.Web.HttpPostedFile de um arquivo
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public HttpPostedFile PostedFile { get { return _FileUpload.PostedFile; } }

        /// <summary>
        /// Tamanho de lagura
        /// </summary>
        [DefaultValue(typeof(Unit), "")]
        public override Unit Width 
        {
            get { return _FileUpload.Width; }
            set { _FileUpload.Width = value; }
        }

        /// <summary>
        /// Tamanho de altura
        /// </summary>
        [DefaultValue(typeof(Unit), "")]
        public override Unit Height
        {
            get { return _FileUpload.Height; }
            set { _FileUpload.Height = value; }
        }
        #endregion

        #region Campos validação
        /// <summary>
        /// colocar as extenções seperadas por virgula.
        /// Ex.: .exe,.pdf
        /// </summary>
        [Browsable(true)]        
        public string ExtensoesArquivos
        {
            get
            {
                return this.ViewState["ExtensoesArquivos"] as string;
            }
            set
            {
                this.ViewState["ExtensoesArquivos"] = value;

                if (string.IsNullOrEmpty(value))
                    RegexValidacao = "";
                else
                {
                    string seperador = @"$|.+\.";
                    RegexValidacao = @".+\." + value.Replace(",", seperador);
                    if (RegexValidacao.EndsWith(seperador))
                        RegexValidacao = RegexValidacao.Remove(RegexValidacao.LastIndexOf(seperador) - seperador.Length);
                    RegexValidacao += "$";
                }
            }
        }

        private string RegexValidacao
        {
            get { return this.ViewState["ExtensoesArquivos"] as string; }
            set { this.ViewState["ExtensoesArquivos"] = value; }
        }
        #endregion

        #region css
        /// <summary>
        /// Css aplicado no input de arquivo
        /// </summary>
        [CssClassProperty]
        [DefaultValue("")]
        public override string CssClass
        {
            get
            {
                return _FileUpload.CssClass;
            }
            set
            {
                _FileUpload.CssClass = value;
            }
        }       
        #endregion

        #endregion

        #region Métodos

        #region override
        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _cvValor.ServerValidate += new ServerValidateEventHandler(_cvValor_ServerValidate);            
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
            InicializarTextBox();
            InicializarValidator();
            InicializarPainel();
            Controls.Add(CampoTexto);
            InicializarFileUpload();
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
        }

        /// <summary>
        /// Inicializa o validador
        /// </summary>
        private new void InicializarValidator()
        {
            base.InicializarValidator();
            ValidadorTexto.ErrorMessage = String.Empty;
            _cvValor.Display = ValidatorDisplay.Dynamic;
            _cvValor.EnableClientScript = true;
            _cvValor.ID = "cvValor";
            _cvValor.ControlToValidate = this.CampoTexto.ID;
            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.EmployerUpload.ValidarTextBox";
        }
        #endregion

        #region Script
        /// <inheritdoc/>
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.EmployerUpload", this.ClientID);

            this.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("RegexValidacao", this.RegexValidacao);

            return new ScriptControlDescriptor[] { descriptor };
        }

        /// <inheritdoc/>
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.EmployerUpload.js";
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

            if (!String.IsNullOrEmpty(this.Text) && !string.IsNullOrEmpty(this.RegexValidacao))
            {
                Regex rx = new Regex(this.RegexValidacao, RegexOptions.IgnoreCase);

                args.IsValid = rx.IsMatch(this.Text);
                this._cvValor.IsValid = args.IsValid;
                this.ValidadorTexto.ErrorMessage = MensagemErroFormato;
            }
        }
        #endregion       

        #region SaveAs
        /// <summary>
        /// Salva o arquivo 
        /// </summary>
        /// <param name="filename"></param>
        public void SaveAs(string filename)
        {
            _FileUpload.SaveAs(filename);
        }
        #endregion

        #endregion
    }
}
