using System;

namespace BNE.Componentes
{
    public class ControlImageButton : System.Web.UI.WebControls.ImageButton
    {

        #region ImageUrlDisabled
        public String ImageUrlDisabled
        {
            get
            {
                return Convert.ToString(ViewState["ImageButtonDisabled"]);
            }
            set
            {
                ViewState["ImageButtonDisabled"] = value;
            }
        }
        #endregion

        #region ImageUrl
        public override string ImageUrl
        {
            get
            {
                if (!IsEnabled)
                {
                    if (!String.IsNullOrEmpty(ImageUrlDisabled))
                        return ImageUrlDisabled;
                }

                return base.ImageUrl;
            }
            set
            {   
                base.ImageUrl = value;
            }
        }
        #endregion

    }
}
