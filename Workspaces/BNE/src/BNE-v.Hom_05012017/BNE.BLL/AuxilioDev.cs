using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace BNE.BLL
{
    public class AuxilioDev
    {
        #region CarregarParametroTesteQuery
        public static string CarregarParametroTesteQuery(List<SqlParameter> parms)
        {
            string retorno = "";

            foreach (SqlParameter parm in parms)
            {
                retorno += "DECLARE ";
                retorno += parm.ToString() + " ";
                retorno += parm.SqlDbType.ToString();

                if (parm.SqlDbType == SqlDbType.VarChar ||
                    parm.SqlDbType == SqlDbType.Char
                    )
                    retorno += "(" + parm.Size.ToString() + ")";

                retorno += Environment.NewLine;

                retorno += "SET ";
                retorno += parm.ToString() + " = ";

                if (parm.Value != null &&
                    parm.Value != DBNull.Value)
                {
                    if (parm.SqlDbType == SqlDbType.VarChar ||
                        parm.SqlDbType == SqlDbType.Char)
                        retorno += "'" + parm.Value.ToString() + "'";
                    else if (parm.SqlDbType == SqlDbType.DateTime ||
                             parm.SqlDbType == SqlDbType.SmallDateTime)
                    {
                        DateTime date = DateTime.Parse(parm.Value.ToString());
                        retorno += "'" + date.Year.ToString() + "-" +
                            (date.Month < 10 ? "0" + date.Month.ToString() : date.Month.ToString()) + "-" +
                            (date.Day < 10 ? "0" + date.Day.ToString() : date.Day.ToString()) + "'";
                    }
                    else if (parm.SqlDbType == SqlDbType.Bit)
                        retorno += parm.Value.ToString().Equals("True") ? "1" : "0";
                    else
                        retorno += parm.Value.ToString();
                }
                else
                    retorno += "NULL";

                retorno += Environment.NewLine;
                retorno += Environment.NewLine;
            }

            return retorno;
        }      

        public static IValidator GetValidatorInvalid(Page objPage)
        {
            foreach (IValidator objVal in objPage.Validators)
            {
                if (objVal.IsValid == false)
                    return objVal;
            }
            return null;
        }
        #endregion
    }
}
