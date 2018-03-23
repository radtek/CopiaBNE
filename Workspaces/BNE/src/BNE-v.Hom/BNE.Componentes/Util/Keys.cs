namespace BNE.Componentes.Util
{
    internal class Keys
    {

        #region General
        public enum General
        {
            AutoPostBack,
            Required,
            ToolTipTitle,
            ToolTipText,
            Title,
            Text,
            TargetContolID,
            ReadOnly,
            SelectedValue,
            Value,
            ImageUrl,
            ThumbUrl,
            //ThumbDir,
            ImageData,
            MaxWidth,
            MaxHeight,
            AllowSelectOnType,
            AllowClientBehavior,
            ShowOnMouseover
        }
        #endregion

        #region Paging
        public enum Paging
        {
            PageSize
        }
        #endregion

        #region LookupControl
        public enum LookupControl
        {
            DataTextField,
            SearchTextField,
            DataValueField,
            DataParentValueField,
            DialogType,
            DataValueTextField,
            RequestDelay,
            RequestMinCharacterCount
        }
        #endregion

        #region Permissions
        public enum Permissions
        {
            Permissions,
            PermissionsWithCategory,
            Idf_Filial,
            Idf_Usuario
        }
        #endregion

        #region Stylesheet
        public enum Stylesheet
        {
            CssClassLabel,
            CssClassHover,
            CssClassFocus,
            CssClassError
        }
        #endregion

        #region Images
        public enum Images
        {
            AspectRatio,
            InitialSelection,
            InvalidInitialSelection,
            InvalidAspectRatio,
            InvalidFormat,
            ImageWidth,
            ImageHeight,
            MinAcceptedWidth,
            MinAcceptedHeight,
            InvalidMinimumImageSizeMessage,
            Percentual
        }
        #endregion

        #region Css
        public enum Css
        {
            EmployerPanel,
            EmployerComboCheckbox,
            EmployerLabelNormal,
            EmployerLabelRequired,
            EmployerButton,
            EmployerButtonHover,
            EmployerTextBox,
            EmployerTextBoxFocused,
            EmployerTextBoxError,
            EmployerValidator,
            EmployerAvisoLookup
        }
        #endregion

    }
}