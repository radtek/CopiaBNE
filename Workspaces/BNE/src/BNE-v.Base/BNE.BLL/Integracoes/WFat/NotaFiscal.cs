using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL.Integracoes.WFat
{
    public class NotaFiscal
    {
        public string Transacao { get; set; }
        public int NumeroNotaFiscal { get; set; }
        public string CodigoVerificacao { get; set; }
        public string Link
        {
            get
            {
                return String.Format("https://nfe.prefeitura.sp.gov.br/contribuinte/notaprint.aspx?ccm=40954625&nf={0}&cod={1}", NumeroNotaFiscal, CodigoVerificacao);
            }
        }

        private const string SP_Lista_Notas = 
        @"SELECT
	        ige.Dta_Cadastro,
	        ige.num_transacao,
	        fnfe.num_nfe,
	        fnfe.cod_verificacao
        from employer.fat_integracao_gestao ige
        join employer.fat_fatura_nfe fnfe on fnfe.idf_fatura=ige.idf_fatura
        JOIN plataforma.TAB_Sistema sis ON ige.Idf_Sistema = sis.Idf_Sistema
        where 1=1
        --AND sis.Idf_Sistema = 9
        --ORDER BY ige.Dta_Cadastro
        and ige.num_transacao IN ({id})
        ORDER BY Num_NFE;";

        public static List<NotaFiscal> ObterNotasNaoImportadas()
        {
            List<NotaFiscal> lstRetorno = new List<NotaFiscal>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "BNE_SP_NOTAS_GERADAS_NAO_IMPORTADAS", null))
            {
                while (dr.Read())
                {
                    try
                    {
                        NotaFiscal nf = new NotaFiscal();
                        nf.Transacao = dr["num_transacao"].ToString().Trim();
                        nf.NumeroNotaFiscal = Convert.ToInt32(dr["num_nfe"].ToString().Trim());
                        nf.CodigoVerificacao = dr["cod_verificacao"].ToString().Trim();

                        lstRetorno.Add(nf);
                    }
                    catch { }
                }

                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }

            return lstRetorno;
        }

        public static List<NotaFiscal> ObterNotas (List<BLL.Pagamento> LstPagamentos){
            List<NotaFiscal> lstRetorno = new List<NotaFiscal>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_Lista_Notas, null, DataAccessLayer.CONN_PLATAFORMA))
            {
                while (dr.Read())
                {
                    try
                    {
                        NotaFiscal nf = new NotaFiscal();
                        nf.Transacao = dr["num_transacao"].ToString().Trim();
                        nf.NumeroNotaFiscal = Convert.ToInt32(dr["num_nfe"].ToString().Trim());
                        nf.CodigoVerificacao = dr["cod_verificacao"].ToString().Trim();
                    }
                    catch { }
                }
               
                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }

            return lstRetorno;
        }
    }
}
