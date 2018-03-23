using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Custom
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
                
                if (parm.Value != null || parm.Value != DBNull.Value)
                {
                    if (parm.SqlDbType == SqlDbType.VarChar ||
                        parm.SqlDbType == SqlDbType.DateTime ||
                        parm.SqlDbType == SqlDbType.Char ||
                        parm.SqlDbType == SqlDbType.SmallDateTime
                        )
                        retorno += "'" + parm.Value.ToString() + "'";
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
        #endregion
    }
}
