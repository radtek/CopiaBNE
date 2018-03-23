using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.Data.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]

namespace Employer.Plataforma.Web.Componentes
{
	[ToolboxData("<{0}:Data runat=server Obrigatorio=False MostrarCalendario=true ></{0}:Data>")]
	public class Data : BaseCompositeControl
	{
		#region Atributos

		private TextBox _txtValor;
		private CustomValidator _cvValor;
		private CalendarExtender _extValor;
		private Boolean _renderUpLevel;
		private HiddenField _hfDataMaxima;
		private HiddenField _hfDataMinima;

		#endregion

		#region Propriedades

		#region SetFocusOnError
		/// <summary>
		/// Define se o campo é SetFocusOnError.
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("SetFocusOnError")
		]
		public bool SetFocusOnError
		{
			get
			{
				return _cvValor.SetFocusOnError;
			}
			set
			{
				_cvValor.SetFocusOnError = value;
			}
		}

		#endregion

		#region ReadOnly
		/// <summary>
		/// Define se o campo é ReadOnly.
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("ReadOnly")
		]
		public bool ReadOnly
		{
			get
			{
				return _txtValor.ReadOnly;
			}
			set
			{
				_txtValor.ReadOnly = value;
			}
		}

		#endregion

		#region Validation Group
		/// <summary>
		/// ValidationGroup do CustomValidator
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("Validation Group")
		]
		public string ValidationGroup
		{
			get
			{
				return _cvValor.ValidationGroup;
			}
			set
			{
				_cvValor.ValidationGroup = value;
			}
		}
		#endregion

		#region Custom Validator - Mensagem Erro
		/// <summary>
		/// Mensagem de Erro do CustomValidator
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("Mensagem Erro Formato"),
			Localizable(true)
		]
		public string MensagemErroFormato
		{
			get
			{
				return _cvValor.ErrorMessage;
			}
			set
			{
				_cvValor.ErrorMessage = value;
			}
		}
		#endregion

		#region Required Field - Mensagem Erro
		/// <summary>
		/// Mensagem de Erro Obrigatório
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("Mensagem Erro Obrigatorio"),
			Localizable(true)
		]
		public string MensagemErroObrigatorio
		{
			get
			{
				if (ViewState["MensagemErroObrigatorio"] == null)
					return string.Empty;
				return ViewState["MensagemErroObrigatorio"].ToString();
			}
			set
			{
				ViewState["MensagemErroObrigatorio"] = value;
			}
		}

		#endregion

		#region Range - Mensagem Erro
		/// <summary>
		/// Mensagem de Erro Obrigatório
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("Mensagem Erro Intervalo"),
			Localizable(true)
		]
		public string MensagemErroIntervalo
		{
			get
			{
				if (ViewState["MensagemErroIntervalo"] == null)
					return string.Empty;
				return ViewState["MensagemErroIntervalo"].ToString();
			}
			set
			{
				ViewState["MensagemErroIntervalo"] = value;
			}
		}

		#endregion

		#region Obrigatorio
		/// <summary>
		/// Define se o campo é obrigatório.
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("Obrigatorio")
		]
		public bool Obrigatorio
		{
			get
			{
				if (ViewState["Obrigatorio"] == null)
					return false;
				return Convert.ToBoolean(ViewState["Obrigatorio"]);
			}
			set
			{
				ViewState["Obrigatorio"] = value;
			}
		}
		#endregion

		#region MostrarCalendario
		/// <summary>
		/// Mostra o calendário no campo de Data
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("Mostrar Calendário")
		]
		public bool MostrarCalendario
		{
			get
			{
				return _extValor.Enabled;
			}
			set
			{
				_extValor.Enabled = value;
			}
		}
		#endregion

		#region Data Mínima
		/// <summary>
		/// Define um valor mínimo de data.
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("Data Mínima")
		]
		public DateTime DataMinima
		{
			get
			{
				CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");

				//Limitação no Javascript
				DateTime dtDefault = Convert.ToDateTime(new DateTime(1001, 1, 1).ToString("dd/MM/yyyy"), culture);
				if (ViewState["DataMinima"] == null)
					return dtDefault;
				DateTime dtMim = Convert.ToDateTime(ViewState["DataMinima"], culture);
				if (dtMim < dtDefault)
					return dtDefault;
				else
					return dtMim;
			}
			set
			{
				ViewState["DataMinima"] = value.ToString("dd/MM/yyyy");
			}
		}
		#endregion

		#region Data Máxima
		/// <summary>
		/// Define uma data máxima.
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("Data Máxima")
		]
		public DateTime DataMaxima
		{
			get
			{
				CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");

				if (ViewState["DataMaxima"] == null)
					return Convert.ToDateTime(DateTime.MaxValue.ToString("dd/MM/yyyy"), culture);
				return Convert.ToDateTime(ViewState["DataMaxima"], culture);
			}
			set
			{
				ViewState["DataMaxima"] = value.ToString("dd/MM/yyyy");
			}
		}

		#endregion

		#region TextBox - CssClass
		/// <summary>
		/// CssClass do TextBox
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("CssClass TextBox")
		]
		public string CssClassTextBox
		{
			get
			{
				return _txtValor.CssClass;
			}
			set
			{
				_txtValor.CssClass = value;
			}
		}
		#endregion

		#region Calendar - CssClass
		/// <summary>
		/// CssClass do Calendar
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("CssClass Calendar")
		]
		public string CssClassCalendar
		{
			get
			{
				return _extValor.CssClass;
			}
			set
			{
				_extValor.CssClass = value;
			}
		}
		#endregion

		#region CustomValidator - CssClass
		/// <summary>
		/// CssClass do CustomValidator
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("CssClass CustomValidator")
		]
		public string CssClassCustomValidatorExpression
		{
			get
			{
				return _cvValor.CssClass;
			}
			set
			{
				_cvValor.CssClass = value;
			}
		}
		#endregion

		#region Valor
		/// <summary>
		/// Recupera ou Seta o valor do componente
		/// </summary>
		[Browsable(false)]
		public string Valor
		{
			get
			{
                _txtValor.Text = AplicarMascara(_txtValor.Text); //BUG #Em alguns casos foi identificado que ocorre post sem as "/", provocando que o formato seja inválido.
                if (ValidarFormato(_txtValor.Text))
					return AplicarMascara(_txtValor.Text);
				return string.Empty;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					value = value.Trim();
					if (ValidarFormato(value))
						value = AplicarMascara(value);
				}
				_txtValor.Text = value;
			}
		}
		#endregion

		#region ValorDateTime
		/// <summary>
		/// Recupera ou Seta o valor do componente
		/// </summary>
		[Browsable(false)]
		public DateTime? ValorDatetime
		{
			get
			{
                _txtValor.Text = AplicarMascara(_txtValor.Text); //BUG #Em alguns casos foi identificado que ocorre post sem as "/", provocando que o formato seja inválido.
				if (ValidarFormato(_txtValor.Text))
				{
					int ano = Convert.ToInt32(_txtValor.Text.Split('/')[2]);
					int mes = Convert.ToInt32(_txtValor.Text.Split('/')[1]);
					int dia = Convert.ToInt32(_txtValor.Text.Split('/')[0]);
					return new DateTime(ano, mes, dia);
				}
				return null;
			}
			set
			{
				if (!value.HasValue)
					_txtValor.Text = string.Empty;
				else
				{
					if (ValidarFormato(value.Value.ToString("dd/MM/yyyy")))
						_txtValor.Text = AplicarMascara(value.Value.ToString("dd/MM/yyyy"));
				}
			}
		}
		#endregion

		#region ValorAlteradoClient
		/// <summary>
		/// Função javascript que deve ser executada ao alterar o valor.
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("ValorAlteradoClient")
		]
		public string ValorAlteradoClient
		{
			get;
			set;
		}
		#endregion

		#region OnBlurClient
		/// <summary>
		/// Função javascript que deve ser executada ao alterar o valor.
		/// </summary>
		[
			Category("Employer - Data"),
			DisplayName("OnBlurClient")
		]
		public string OnBlurClient
		{
			get;
			set;
		}
		#endregion

		#endregion

		#region Construtores
		/// <summary>
		/// Construtor da Classe Data
		/// </summary>
		public Data()
		{
			_txtValor = new TextBox();
			_cvValor = new CustomValidator();
			_extValor = new CalendarExtender();
			this._hfDataMaxima = new HiddenField();
			this._hfDataMinima = new HiddenField();
		}

		#endregion

		#region Métodos

		#region DetermineRenderUpLevel
		/// <summary>
		/// DetermineRenderUpLevel
		/// </summary>
		/// <returns></returns>
		protected virtual bool DetermineRenderUpLevel()
		{
			HttpBrowserCapabilities browser = Page.Request.Browser;
			if (browser.W3CDomVersion.Major >= 1
				&& browser.EcmaScriptVersion.CompareTo(new Version(1, 2)) >= 0)
				return true;
			return false;
		}
		#endregion

		#region AplicarAcoesComponente
		/// <summary>
		/// Método que instância o objeto de argumentos e registra a função
		/// valor alterado para ser invocada no Client.
		/// </summary>
		private void AplicarAcoesComponente()
		{
			Args args = new Args();
			args.NomeCampoValor = _txtValor.ClientID;
			List<string> nomeValidadores = new List<string>();
			nomeValidadores.Add(_cvValor.ClientID);
			args.Validadores = nomeValidadores.ToArray();

			_txtValor.Attributes.Add("OnPaste", "return false;");
			_txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, getDataSize(this));");
			_txtValor.Attributes.Add("OnKeyPress", "return ApenasNumerosData(this, event, '" + DateTime.Now.ToString("dd/MM/yyyy") + "');");
			_txtValor.Attributes.Add("OnFocus", "RemoverMascaraControle(this, '(^(0[1-9]|[12][0-9]|3[01])[/.](0[1-9]|1[012])[/.](\\\\d{4})$)|(\\\\d{8})');");

			_txtValor.Attributes.Add("OnBlur", "AplicarMascaraData(this, '" + DateTime.Now.ToString("dd/MM/yyyy") + "');");
			_txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

			if (!String.IsNullOrEmpty(ValorAlteradoClient))
				_txtValor.Attributes["OnBlur"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");";
			if (ValorAlterado != null)
				_txtValor.Attributes["OnBlur"] += "DoPostBack(this," + new JSONReflector(args) + ");";

			_txtValor.Attributes.Add("OnChange", "AplicarMascaraData(this, '" + DateTime.Now.ToString("dd/MM/yyyy") + "');");
			if (!String.IsNullOrEmpty(ValorAlteradoClient))
				_txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");";
			if (ValorAlterado != null)
				_txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";


			if (!String.IsNullOrEmpty(OnBlurClient))
				_txtValor.Attributes["OnBlur"] += OnBlurClient + "(" + new JSONReflector(args) + ");";

			// Adiciona postback no evento on blur no firefox quando o Text estiver vazio.
			if (ValorAlterado != null && String.IsNullOrEmpty(this._txtValor.Text) && "Firefox".Equals(this.Page.Request.Browser.Browser, StringComparison.OrdinalIgnoreCase))
			{
				_txtValor.Attributes["OnBlur"] += "DoPostBack(this," + new JSONReflector(args) + ");";
			}
		}
		#endregion

		#region InicializarCalendarExtender
		/// <summary>
		/// Método de configuração do Calendar Extender
		/// </summary>
		private void InicializarCalendarExtender()
		{
			_extValor.ID = "calendarExtender";
			_extValor.TargetControlID = _txtValor.ID;
			_extValor.Format = "dd/MM/yyyy";
		}
		#endregion

		#region InicializarText
		/// <summary>
		/// Método que configura o TextBox de Data
		/// </summary>
		protected virtual void InicializarTextBox()
		{
			_txtValor.ID = "txtValor";
			_txtValor.Columns = 10;
			_txtValor.MaxLength = 10;
			_txtValor.AutoCompleteType = AutoCompleteType.Disabled;
		}
		#endregion

		#region InicializarHiddenFields
		/// <summary>
		/// Iniciliza as instâncias dos hidden fields
		/// </summary>
		protected virtual void InicializarHiddenFields()
		{
			// Data mínima
			this._hfDataMinima.EnableViewState = false;
			this._hfDataMinima.ID = "hfDataMinima";
			// Data máxima
			this._hfDataMaxima.EnableViewState = false;
			this._hfDataMaxima.ID = "hfDataMaxima";
		}
		#endregion

		#region InicializarCustomValidator
		/// <summary>
		/// Método que seta as configurações da propriedades do Custom Validator do campo Data.
		/// </summary>
		protected void InicializarCustomValidator()
		{
			_cvValor.ID = "cvValor";
			_cvValor.ForeColor = Color.Empty;
			_cvValor.ControlToValidate = _txtValor.ID;
			_cvValor.ClientValidationFunction = "ValidarData";
			_cvValor.Display = ValidatorDisplay.Dynamic;
			_cvValor.ValidateEmptyText = true;
			//_cvValor.ErrorMessage = "Erro";
			_cvValor.ServerValidate += new ServerValidateEventHandler(_cvValor_ServerValidate);
		}
		#endregion

		#region AplicarMascara
		/// <summary>
		/// Método que aplica a data formatada com máscara no campo Data.
		/// </summary>
		/// <param name="data">Data informada pelo usuário que passou por processo de validação.</param>
		/// <returns>Afirma que a data é valida.</returns>
		private string AplicarMascara(string data)
		{
			if (data.Length.Equals(8))
				return string.Format("{0}/{1}/{2}", data.Substring(0, 2), data.Substring(2, 2), data.Substring(4)).Replace("//", "/");

            return data;
		}
		#endregion

		#region ValidarFormato
		/// <summary>
		/// Validar o formato do valor desejado
		/// </summary>
		/// <param name="data">Valor desejado a validar</param>
		/// <returns>'True' se valor é válido, caso contrário 'False'</returns>
		private static bool ValidarFormato(string data)
		{
			data = data.Trim();
			DateTime dt;
			if (DateTime.TryParse(data, new CultureInfo("pt-BR"), DateTimeStyles.None, out dt))
				return true;
			return false;
		}
		#endregion

		#region AjaxMethods
		[Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
		public static string DataAtual()
		{
			return DateTime.Now.ToString("dd/MM/yyyy");
		}

		#endregion

		#endregion

		#region Eventos

		#region _cvValor_ServerValidate
		void _cvValor_ServerValidate(object source, ServerValidateEventArgs args)
		{
			DateTime dtTemp = new DateTime(1001, 1, 1);

			args.IsValid = DateTime.TryParse(_txtValor.Text, out dtTemp);

			if (this.Obrigatorio)
				args.IsValid = DateTime.TryParse(_txtValor.Text, out dtTemp);
			else
			{
				args.IsValid = true;
			}

			// Validação de range de data
			if (args.IsValid)
			{
				if (!String.IsNullOrEmpty(_txtValor.Text))
				{
					if (this.DataMaxima != DateTime.MaxValue)
						args.IsValid = (dtTemp <= this.DataMaxima);

					if (this.DataMinima != DateTime.MinValue)
						args.IsValid = args.IsValid && (dtTemp >= this.DataMinima);
				}
			}
		}
		#endregion

		#region AddAttributesToRender
		/// <summary>
		/// AddAttributesToRender
		/// </summary>
		/// <param name="writer"></param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);

			if (_renderUpLevel)
			{
				writer.AddAttribute("Obrigatorio", Obrigatorio ? "1" : "");
				writer.AddAttribute("MensagemErroObrigatorio", MensagemErroObrigatorio);
				writer.AddAttribute("MensagemErroFormato", MensagemErroFormato);
				writer.AddAttribute("MensagemErroIntervalo", MensagemErroIntervalo);
			}
		}
		#endregion

		#region ValorAlterado
		/// <summary>
		/// Evento ao alterar valor
		/// </summary>
		public event EventHandler ValorAlterado;
		#endregion

		#region OnInit
		/// <summary>
		/// Evento de inicialização do componente.
		/// </summary>
		/// <param name="e">Argumento necessário de inicialização.</param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (!Page.ClientScript.IsClientScriptIncludeRegistered("Data.js"))
				Page.ClientScript.RegisterClientScriptInclude("Data.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.Data.js"));

			if (!Page.ClientScript.IsClientScriptIncludeRegistered("GeralControle.js"))
				Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));

			Page.Culture = "pt-BR";
		}
		#endregion

		#region CreateChildControls
		protected override void CreateChildControls()
		{
			// Métodos de Configuração das Propriedades dos Componentes
			InicializarTextBox();
			InicializarCustomValidator();
			InicializarCalendarExtender();
			InicializarHiddenFields();

			//Adicionar Controles no WebForm 
			Panel pnlValidador = new Panel();
			pnlValidador.ID = "pnlValidador";
			pnlValidador.Controls.Add(_cvValor);
			Controls.Add(pnlValidador);
			Controls.Add(_txtValor); //Campo Texto de Data
			if (_extValor.Enabled)
				Controls.Add(_extValor); //Calendar Extender

			this.Controls.Add(_hfDataMinima);
			this.Controls.Add(_hfDataMaxima);

			AplicarAcoesComponente();
			base.CreateChildControls();
		}
		#endregion

		#region OnPreRender
		/// <summary>
		/// Rederizar os componentes
		/// </summary>
		/// <param name="e">Argumento do evento</param>
		protected override void OnPreRender(EventArgs e)
		{
			_renderUpLevel = DetermineRenderUpLevel();
			base.OnPreRender(e);

			string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
			if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(_txtValor.ClientID))
				ValorAlterado(_txtValor, null);

			// Valores dos hidden field
			this._hfDataMinima.Value = DataMinima.ToString("dd/MM/yyyy");
			this._hfDataMaxima.Value = DataMaxima.ToString("dd/MM/yyyy");
		}
		#endregion

		#region OnLoad
		/// <summary>
		/// Evento executado quando o componente é carregado.
		/// </summary>
		/// <param name="e">Argumento do evento</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Ajax.Utility.RegisterTypeForAjax(typeof(Data));
		}
		#endregion
		#endregion
	}
}