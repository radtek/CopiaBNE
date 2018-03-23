using APIGateway.ModelValidation.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.ModelValidation.Common
{
    public class IsUnique : ValidationAttribute
    {
        public IsUnique(){ }

        public IsUnique(string tableName, string fieldName)
        {
            this.TableName = tableName;
            this.FieldName = fieldName;
        }


        public string TableName { get; private set; }
        public string FieldName { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var className = String.IsNullOrEmpty(TableName) ? validationContext.ObjectType.Name.Split('.').Last() : TableName;
            var propertyName = String.IsNullOrEmpty(FieldName) ? validationContext.MemberName : FieldName;
            var parameterName = string.Format("@{0}", propertyName);

            string cmd = string.Format("SELECT COUNT(*) FROM {0} WHERE {1}={2}", className, propertyName, parameterName);
            List<System.Data.SqlClient.SqlParameter> cmdParms = new List<System.Data.SqlClient.SqlParameter>(){new System.Data.SqlClient.SqlParameter(parameterName, value)};

            int count = Convert.ToInt32(DataAccessLayer.ExecuteScalar(System.Data.CommandType.Text, cmd, cmdParms));
            if (count > 0)
            {
                if (String.IsNullOrEmpty(this.ErrorMessage))
                    return new ValidationResult(string.Format("Já existe um registro com o '{0}' igual a '{1}'", propertyName, value),
                                new List<string>() { propertyName });
                else
                    return new ValidationResult(this.ErrorMessage);
            }

            return null;
        }
    }
}
