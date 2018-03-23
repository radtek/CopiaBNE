namespace Employer.Componentes.UI.Web.Util
{
    #pragma warning disable 1591
    public class Keys
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
            ThumbDir,
            MaxWidth,
            MaxHeight,
            AllowSelectOnType,
            AllowClientBehavior            
        }
        #endregion 

        public enum Paging
        {
            PageSize
        }

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
            InvalidMinimumImageSizeMessage
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