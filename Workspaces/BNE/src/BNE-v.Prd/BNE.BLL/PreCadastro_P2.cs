using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PreCadastro
    {
        #region Consultas

        #region SpVerificaCadastro

        private const string SpVerificaCadastro = @"Select idf_Pessoa_fisica from tab_pessoa_fisica with(nolock) 
                    where eml_Pessoa = @Email";

        #endregion

        #endregion

        #region Métodos

        #region Verifica se existe cadastro

        /// <summary>
        ///     Se ja existir no bne um cadastro com o email e nome cadastrado na popup é inativado da table do preCadastro
        /// </summary>
        /// <param name="email"></param>
        /// <param name="nome"></param>
        public static bool VerificaCadastro(string email, int idfOrigem)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Email", SqlDbType = SqlDbType.VarChar, Value = email},
            };
            // se ja possuir email e nome igual cadastrados no bne é inativado o pre cadastro pra não receber mais jornal do preCadastro
            if (DataAccessLayer.ExecuteScalar(CommandType.Text, SpVerificaCadastro, parms) != null)
            {
                Delete(email, idfOrigem);
                return true;
            }
            else
                return false;
        }
        #endregion

        #endregion
    }
}