using System;
using System.Collections.Generic;
using System.Web.UI;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI.WebControls;
using BNE.Componentes.Base; 

namespace BNE.Componentes
{
    /// <summary>
    /// Campo texto formatado para datas
    /// </summary>
    public class DataTextBox : ControlBaseTextBox
    {
        #region Atributos
        private readonly CustomValidator _cvValor = new CustomValidator();
        private readonly Panel _pnlValidador = new Panel();

        #endregion

        #region Propriedades

        #region Internas

        #region ValidadorTexto
        /// <summary>
        /// Sobrecar da propriedade para uso na clase ControlBaseTextBox
        /// </summary>
        protected override BaseValidator ValidadorTexto
        {
            get { return _cvValor; }
        }
        #endregion

        #region PanelValidador
        /// <inheritdoc />
        protected override Panel PanelValidador
        {
            get { return _pnlValidador; }
        }
        #endregion

        #endregion

        #region Validador
        #region DataMinima
        /// <summary>
        /// Define a data mínima do campo.
        /// </summary>
        [Category("Validador"), DisplayName("Valor Mínimo")]
        public DateTime? DataMinima
        {
            get
            {
                return (DateTime?) ViewState["ValorMinimo"];
            }
            set { ViewState["ValorMinimo"] = value; }
        }
        #endregion

        #region CampoDataMinima
        /// <summary>
        /// Define um campo de valor mínimo do campo.
        /// </summary>
        [Category("Validador"), DisplayName("Campo Valor Mínimo")]
        public string CampoDataMinima
        {
            get
            {
                return (string) ViewState["CampoDataMinima"];
            }
            set { ViewState["CampoDataMinima"] = value; }
        }
        #endregion

        #region DataMaxima
        /// <summary>
        /// Define a data maxima do campo.
        /// </summary>
        [Category("Validador"), DisplayName("Valor Maxima")]
        public DateTime? DataMaxima
        {
            get
            {
                return (DateTime?) ViewState["DataMaxima"];
            }
            set { ViewState["DataMaxima"] = value; }
        }
        #endregion

        #region CampoDataMaxima
        /// <summary>
        /// Define um campo de valor maximo do campo.
        /// </summary>
        [Category("Validador"), DisplayName("Campo Data Maxima")]
        public string CampoDataMaxima
        {
            get
            {
                return (string) ViewState["CampoDataMaxima"];
            }
            set { ViewState["CampoDataMaxima"] = value; }
        }
        #endregion

        #endregion

        #region Data
        /// <summary>
        /// Propriedade para setar ou receber valor do Campo Valor Decimal.
        /// </summary>
        [Category("Data"), Browsable(true), Bindable(true)]
        public DateTime? ValorDateTime
        {
            get
            {
                DateTime vlr;
                if (DateTime.TryParse(CampoTexto.Text, out vlr))
                    return vlr;
                return null;
            }
            set
            {
                if (value.HasValue)
                    CampoTexto.Text = FomataDatetime(value.Value);
                else
                    CampoTexto.Text = string.Empty;
            }
        }

        /// <summary>
        /// Mostra o calendário no campo de Data
        /// </summary>
        [Category("Data"), DisplayName("Mostrar Calendário")]
        public bool MostrarCalendario
        {
            get
            {
                if (ViewState["MostrarCalendario"] == null)
                    return false;
                return Convert.ToBoolean(ViewState["MostrarCalendario"]);
            }
            set
            {
                ViewState["MostrarCalendario"] = value;
            }
        }
        #endregion

        #region ImagemBotaoUrl
        public String ImagemBotaoUrl
        {
            get
            {
                if (ViewState["ImagemBotaoUrl"] == null)
                    return Page.ClientScript.GetWebResourceUrl(typeof(DataTextBox),
                        "BNE.Componentes.Content.Imagens.botao_calendar.png");

                return Page.ResolveUrl(Convert.ToString(ViewState["ImagemBotaoUrl"]));
            }
            set
            {
                ViewState["ImagemBotaoUrl"] = value;
            }
        }
        #endregion 

        #region SelecionarFeriados
        public bool SelecionarFeriados
        {
            get
            {
                if (ViewState["SelecionarFeriados"] == null)
                    return true;
                return Convert.ToBoolean(ViewState["SelecionarFeriados"]);
            }
            set
            {
                ViewState["SelecionarFeriados"] = value;
            }
        }
        #endregion 

        #region CssClassFeriado
        public String CssClassFeriado
        {
            get
            {
                return Convert.ToString(ViewState["CssClassFeriado"]);
            }
            set
            {
                ViewState["CssClassFeriado"] = value;
            }
        }
        #endregion 

        #region UrlUpdateFeriados
        /// <summary>
        /// Url chamada para atualizar a lista de feriados.
        /// </summary>
        public string UrlUpdateFeriados
        {
            private get{
                return ViewState["UrlUpdateFeriados"] != null ? ViewState["UrlUpdateFeriados"].ToString() : "";
            }
            set{
                ViewState["UrlUpdateFeriados"] = value;
            }
        }
        #endregion

        #endregion

        #region Inicializar Controles

        #region InicializarCustomValidator
        /// <summary>
        /// Inicializa o validador
        /// </summary>
        private void InicializarCustomValidator()
        {
            InicializarValidator();

            _cvValor.ValidateEmptyText = true;
            _cvValor.ClientValidationFunction = "BNE.Componentes.DataTextBox.ValidarTextBox"; 
        }
        #endregion

        #region InicializarBotaoExibirCalendario
        private void InicializarBotaoExibirCalendario()
        {

        }
        #endregion

        #endregion

        #region AjaxMethods
        /// <summary>
        /// Método Ajax que retorna a data do servidor
        /// </summary>
        /// <param name="sCultura">A cultura na qual a data será formatada</param>
        /// <returns></returns>
        [Ajax.AjaxMethod]
        public static string DataAtual(string sCultura)
        {
            CultureInfo cult = new CultureInfo(sCultura);
            return DateTime.Now.ToString(GetMascaraSemilonga(cult));
        }
        #endregion

        #region Métodos

        #region CreateChildControls
        /// <inheritdoc />
        protected override void CreateChildControls()
        {
            InicializarTextBox();
            InicializarBotaoExibirCalendario();
            InicializarCustomValidator();
            InicializarPainel();

            CampoTexto.Columns = 10;
            CampoTexto.MaxLength = 10;
            Controls.Add(CampoTexto);

            base.CreateChildControls();
        }
        #endregion

        #region OnInit
        /// <inheritdoc />
        protected override void OnInit(EventArgs e)
        {
            _cvValor.ServerValidate += _cvValor_ServerValidate;
            base.OnInit(e);
        }
        #endregion

        #region OnLoad
        /// <inheritdoc />
        protected override void OnLoad(EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(DataTextBox));           
            base.OnLoad(e);
        }
        #endregion

        #region _cvValor_ServerValidate
        /// <summary>
        /// Evento disparado pela validação de servidor do custom validate
        /// </summary>
        /// <param name="source">O objeto que disparou o evento</param>
        /// <param name="args">Os argumentos do evento</param>
        private void _cvValor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (this.Obrigatorio)
            {
                args.IsValid = this.ValorDateTime.HasValue &&
                (
                   (this.DataMinima.HasValue == false || this.ValorDateTime.Value >= this.DataMinima.Value) &&
                   (this.DataMaxima.HasValue == false || this.ValorDateTime.Value <= this.DataMaxima.Value)
                );
            }
        }
        #endregion

        #region GetDataConvertida
        //public static object[][] ConverterData(ListaFeriados objferiados)
        //{
        //    if (objferiados.RetornarFeriados() != null && objferiados.RetornarFeriados().Count > 0)
        //    {
        //        object[][] dataConvertida = new object[0][];
        //        int contador = 0;
        //        //foreach (KeyValuePair<DateTime, Employer.Plataforma.BLL.Enumeradores.TipoFeriado> linha in objferiados)
        //        foreach (var linha in objferiados.RetornarFeriados())
        //        {
        //            Array.Resize(ref dataConvertida, dataConvertida.Count() + linha.Value.Count);
        //            foreach (var item in linha.Value) {
        //                dataConvertida[contador] = new object[] { linha.Key.Day, linha.Key.Month, linha.Key.Year, item.Key.GetHashCode(), item.Key.ToString(), item.Value };
        //                contador++;
        //            }
        //        }
        //        return dataConvertida;
        //    }
        //    return new object[0][];
        //}
        #endregion

        #endregion

        #region GetMascaraSemilonga
        /// <summary>
        /// Retorna a máscara semi-longa na cultura atual
        /// </summary>
        /// <returns>A máscara semi-loonga</returns>
        public static string GetMascaraSemilonga()
        {
            return GetMascaraSemilonga(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Retorna a máscara semi-longa na cultura parametrizada
        /// </summary>
        /// <param name="cult">A cultura na qual deseja-se a máscara</param>
        /// <returns>A máscara semi-loonga</returns>
        public static string GetMascaraSemilonga(CultureInfo cult)
        {
            DateTimeFormatInfo formato = cult.DateTimeFormat;

            string sMascara = formato.ShortDatePattern;
            //Data muito curta
            if (sMascara.Contains("dd") == false)
                sMascara = sMascara.Replace("d", "dd");

            //Data muito curta
            if (sMascara.Contains("MM") == false)
                sMascara = sMascara.Replace("M", "MM");
            return sMascara;
        }
        #endregion

        #region FomataDatetime
        /// <summary>
        /// Formata a data e hora parametrizada
        /// </summary>
        /// <param name="objData">A data/hora parametrizada</param>
        /// <returns>Uma string contendo a data/hora formatada</returns>
        public string FomataDatetime(DateTime objData)
        {
            return objData.ToString(GetMascaraSemilonga());
        }
        #endregion

        #region SetPropertyById
        /// <summary>
        /// Define uma propriedade a partir de um id de controle
        /// </summary>
        /// <param name="sProperty">A propriedade</param>
        /// <param name="sId">O id</param>
        /// <param name="descriptor">O descritor de scripts</param>
        private void SetPropertyById(string sProperty, string sId, ScriptControlDescriptor descriptor)
        {
            if (string.IsNullOrEmpty(sId) == false)
            {
                Control objCtrl = this.Parent.FindControl(sId);
                if (objCtrl is ControlBaseTextBox)
                    descriptor.AddProperty(sProperty, ((DataTextBox)objCtrl).CampoTexto.ClientID);
                else if (objCtrl != null)
                    descriptor.AddProperty(sProperty, objCtrl.ClientID);
                else
                    descriptor.AddProperty(sProperty, sId);
            }
        }
        #endregion

        #region GetScriptDescriptors
        /// <summary>
        /// Registra as propriedades para o objeto em javascript
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("BNE.Componentes.DataTextBox", this.ClientID);

            this.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("DataMinima", this.DataMinima);
            descriptor.AddProperty("DataMaxima", this.DataMaxima);
            descriptor.AddProperty("MostrarCalendario", this.MostrarCalendario);
            descriptor.AddProperty("ImagemBotaoUrl", this.ImagemBotaoUrl);

            descriptor.AddProperty("Feriados", new int[0][]);
            descriptor.AddProperty("CssClassFeriado", CssClassFeriado);
            descriptor.AddProperty("SelecionarFeriados", SelecionarFeriados);
            descriptor.AddProperty("UrlUpdateFeriados", this.UrlUpdateFeriados);
            
            SetPropertyById("CampoDataMinima", CampoDataMinima, descriptor);
            SetPropertyById("CampoDataMaxima", CampoDataMaxima, descriptor);

            //Conficuração de internacionalização
            DateTimeFormatInfo formato = CultureInfo.CurrentCulture.DateTimeFormat;
            descriptor.AddProperty("Mascara", GetMascaraSemilonga());
            descriptor.AddProperty("DateSeparator", formato.DateSeparator);
            descriptor.AddProperty("Cultura", CultureInfo.CurrentCulture.Name);

            return new[] { descriptor };
        }
        #endregion

        #region GetScriptReferences
        /// <summary>
        /// Retorna as referências das bibiotecas em javascript
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();

            reference.Assembly = "BNE.Componentes";
            reference.Name = "BNE.Componentes.Content.js.DataTextBox.js";
            references.Add(reference);


            return references;
        }
        #endregion
    }
}
