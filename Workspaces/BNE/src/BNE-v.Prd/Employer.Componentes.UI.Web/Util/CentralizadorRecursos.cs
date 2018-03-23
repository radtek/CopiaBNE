using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Employer.Componentes.UI.Web.Util
{
    /// <summary>
    /// Classe centralizadora de recursos
    /// </summary>
    public static class CentralizadorRecursos
    {
        #region Properties
        #region BasePath
        /// <summary>
        /// Caminho base para os recursos
        /// </summary>
        private static String BasePath
        {
            get
            {
                //return "http://static.employer.com.br/";
                return "http://dev.webfopag.com.br/";
            }
        }
        #endregion 
        #endregion

        #region Methods
        #region GetImage
        // /webfopag/AcessoNegado72x72.gif
        /// <summary>
        /// Mapeia uma imagem no centralizador de recursos
        /// </summary>
        /// <param name="path">O caminho relativo para a imagem</param>
        /// <returns>O caminho absoluto da imagem</returns>
        public static String GetImage(String path)
        {
            String newPath = Path.Combine(BasePath, String.Format("/img{0}", path));
            return newPath;
        }
        #endregion 
        #endregion
    }
}
