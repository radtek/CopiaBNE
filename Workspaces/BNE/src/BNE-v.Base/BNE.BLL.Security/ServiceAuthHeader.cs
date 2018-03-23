using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Services.Protocols;

namespace BNE.BLL.Security
{
    public class ServiceAuthHeader : SoapHeader
    {
        #region Atributos
        private string numcpf;
        private string hash;
        #endregion

        #region Propriedades

        #region NumeroCPF
        /// <summary>
        /// 
        /// </summary>
        public string NumeroCPF
        {
            get { return numcpf; }
            set { numcpf = value; }
        }

        #endregion

        #region CodHash
        /// <summary>
        /// 
        /// </summary>
        public string CodHash
        {
            get { return hash; }
            set { hash = value; }
        }
        #endregion
        
        #endregion
    }
}