using System.Data;
using AdminLTE_Application;
using System.Linq;
using System;

namespace Sample.BLL
{
    public class Parametro
    {

        #region [RecuperarParametro]
        /// <summary>
        /// Retona o parametro do DB
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public static string RecuperarParametro(Enumerador.Parametro parametro)
        {
            AdminLTE_Application.dbParametro retorno;
            using (var context = new Model())
            {
                retorno = (from s in context.tbParametro select s).Where(s => s.Id_Parametro.Equals((Int16)parametro)).FirstOrDefault();
            }

            return retorno.Valor;
        }
        #endregion
    }
}