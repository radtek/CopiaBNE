using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Integracoes.WFat
{
    public static class ExtensionNotaFiscal
    {
        private const string SP_Nota =
        @"SELECT  ige.Dta_Cadastro ,
            ige.num_transacao ,
            fnfe.num_nfe ,
            fnfe.cod_verificacao
    FROM    [Empvw5205\PRD].PLATAFORMA_EMPLOYER.employer.fat_integracao_gestao ige
            JOIN [Empvw5205\PRD].PLATAFORMA_EMPLOYER.employer.fat_fatura_nfe fnfe ON fnfe.idf_fatura = ige.idf_fatura
            JOIN [Empvw5205\PRD].PLATAFORMA_EMPLOYER.plataforma.TAB_Sistema sis ON ige.Idf_Sistema = sis.Idf_Sistema
    WHERE   1 = 1
            AND ige.num_transacao = @DesIdentificador";

        #region EntensionNotaFiscal
        public static NotaFiscal ObterNota(this NotaFiscal objNotaFiscal, string DesIdentificador)
        {
            var parms = new List<SqlParameter> 
                {
                    new SqlParameter{ ParameterName = "@DesIdentificador", SqlDbType = SqlDbType.VarChar, Value = DesIdentificador}
                };


            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_Nota, parms, DataAccessLayer.CONN_PLATAFORMA))
            {
                while (dr.Read())
                {
                    try
                    {
                        NotaFiscal nf = new NotaFiscal();
                        nf.Transacao = dr["num_transacao"].ToString().Trim();
                        nf.NumeroNotaFiscal = Convert.ToInt32(dr["num_nfe"].ToString().Trim());
                        nf.CodigoVerificacao = dr["cod_verificacao"].ToString().Trim();
                        return nf;
                    }
                    catch { }
                }

                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }
            return null;

        }
        #endregion
    }
}
