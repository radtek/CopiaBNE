using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Core.Exceptions
{
    public class SqlExceptionsHandler
    {
        public static Exception TratarExcecao(Exception ex)
        {
            Exception innerException = ex;
            while (innerException.GetType() != typeof(SqlException))
            {
                if (innerException.InnerException == null)
                    return ex;
                innerException = innerException.InnerException;
            }

            SqlException sqlEx = (SqlException)innerException;
            if (sqlEx.Errors.Count > 0) // Assume the interesting stuff is in the first error
            {
                switch (sqlEx.Errors[0].Number)
                {
                    case 547: // Foreign Key violation
                        return new ForeingKeyException("Some helpful description", innerException);
                    case 2601: // Primary key violation
                        return new PrimaryKeyException("Some other helpful description", innerException);
                    default:
                        return ex;
                }
            }
            return ex;
        }
    }
}
