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
        private string usuario;
        private string senha;
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

        #region Usuario
        /// <summary>
        /// 
        /// </summary>
        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        #endregion

        #region Senha
        /// <summary>
        /// 
        /// </summary>
        public string Senha
        {
            get { return senha; }
            set { senha = value; }
        }
        #endregion
        
        #endregion
    }
}