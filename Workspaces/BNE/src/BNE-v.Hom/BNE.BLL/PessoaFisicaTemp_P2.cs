using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PessoaFisicaTemp // Tabela: TAB_Pessoa_Fisica_Temp
    {
        #region Propriedades

        #region NumeroCPF
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public string NumeroCPF
        {
            get
            {
                if (this._numeroCPF <= 0)
                    return string.Empty;
                else
                    return this._numeroCPF.ToString().PadLeft(11, '0').Insert(3, ".").Insert(7, ".").Insert(11, "-");
            }
            set
            {
                this._numeroCPF = Convert.ToDecimal(value);
                this._modified = true;
            }
        }
        #endregion

        #endregion

        private const string SPSELECTPORCPF = "SELECT * FROM TAB_Pessoa_Fisica_Temp WHERE Num_Cpf = @Num_Cpf";

        #region CarregarPorCPF
        public static bool CarregarPorCPF(decimal numCpf, out PessoaFisicaTemp objPessoaFisicaTemp)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11));
            parms[0].Value = numCpf;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORCPF, parms))
            {
                objPessoaFisicaTemp = new PessoaFisicaTemp();
                if (SetInstance(dr, objPessoaFisicaTemp))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objPessoaFisicaTemp = null;
            return false;
        }
        #endregion

    }
}
