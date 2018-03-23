using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.Sistmars.BLL
{
    public partial class Pessoa
    {
        
        #region Consultas

        private const string SPSELECTPORCPF = @"SELECT * FROM SMA_Pessoa WHERE PES_CPF = @Num_CPF";

        #endregion

        #region CarregarPorCPF
        /// <summary>   
        /// Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <param name="numCpf">Valor de CPF</param>
        /// <returns>Objeto Pessoa</returns>      
        public static bool CarregarPorCPF(decimal numCpf, out Pessoa objPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11));
            parms[0].Value = numCpf;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORCPF, parms))
            {
                objPessoaFisica = new Pessoa();
                if (SetInstance(dr, objPessoaFisica))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objPessoaFisica = null;
            return false;
        }
        #endregion

        #region SalvarDados

        public static Array SalvarDados(decimal CPF, string nome, string cidade, string estado, int CEP, string telefone, string email, DateTime dataNascimento)
        {
            Array ret = Array.CreateInstance(typeof(String), 2);
            string codigoPessoa = string.Empty;
            string dataNasc = string.Empty;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@PES_CPF", SqlDbType.Decimal, 5));
            parms.Add(new SqlParameter("@PES_Nome", SqlDbType.VarChar, 22));
            parms.Add(new SqlParameter("@PES_Cidade", SqlDbType.VarChar, 22));
            parms.Add(new SqlParameter("@PES_Estado", SqlDbType.VarChar, 22));
            parms.Add(new SqlParameter("@PES_CEP", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PES_Telefone", SqlDbType.VarChar, 22));
            parms.Add(new SqlParameter("@PES_Email", SqlDbType.VarChar, 22));
            parms.Add(new SqlParameter("@PES_DataNascimento", SqlDbType.DateTime, 33));

            parms[0].Value = CPF;
            parms[1].Value = nome;
            parms[2].Value = cidade;
            parms[3].Value = estado;
            parms[4].Value = CEP;
            parms[5].Value = telefone;
            parms[6].Value = email;
            parms[7].Value = dataNascimento;

            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "p_CadastrarPessoa", parms);
            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "p_CadastrarPessoa", parms).Tables[0];
            codigoPessoa = dt.Rows[0]["PES_CodigoPessoa"].ToString();
            dataNasc = dt.Rows[0]["PES_DataNascimento"].ToString(); 
            ret.SetValue(codigoPessoa, 0);
            ret.SetValue(dataNasc, 1);
            dt.Dispose();
            return ret;


        }

        #endregion



    }

}
