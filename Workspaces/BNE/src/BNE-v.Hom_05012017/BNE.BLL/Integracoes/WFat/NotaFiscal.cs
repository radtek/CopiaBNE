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
                return String.Format("http://www.nfs-e.net/datacenter/include/nfw/nfw_imprime_notas.php?codauten={0}", CodigoVerificacao);
            }
        }

        private const string SP_Lista_Notas =
        @"SELECT  ige.Dta_Cadastro ,
            ige.num_transacao ,
            fnfe.num_nfe ,
            fnfe.cod_verificacao
    FROM    [Empvw5205\PRD].PLATAFORMA_EMPLOYER.employer.fat_integracao_gestao ige
            JOIN [Empvw5205\PRD].PLATAFORMA_EMPLOYER.employer.fat_fatura_nfe fnfe ON fnfe.idf_fatura = ige.idf_fatura
            JOIN [Empvw5205\PRD].PLATAFORMA_EMPLOYER.plataforma.TAB_Sistema sis ON ige.Idf_Sistema = sis.Idf_Sistema
    WHERE   1 = 1
            AND ige.num_transacao IN ({ID})";

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

        public static List<NotaFiscal> ObterNotas (List<string> DesIdentificador){

            List<NotaFiscal> lstRetorno = new List<NotaFiscal>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_Lista_Notas.Replace("{ID}", "'" + string.Join("','", DesIdentificador.ToArray()) + "'"), null, DataAccessLayer.CONN_PLATAFORMA))
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
    }
}
