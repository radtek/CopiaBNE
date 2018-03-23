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

        public string Erro { get; set; }
        public string Data_Cadastro { get; set; }
        public string NomeSistema { get; set; }
        public string ValorNota { get; set; }


        private const string SP_Lista_NotasComErro = @"
DECLARE @tabelaDados TABLE
(
	Nme_Sistema VARCHAR(200),
	Dta_Cadastro DATETIME,
	num_transacao VARCHAR(200),
	num_nfe INT,
	Vlr_Total VARCHAR(200), 
	Erro XML, 
	cod_verificacao VARCHAR(200)
	 
)
DECLARE @Erro2 XML
INSERT INTO @tabelaDados
        ( Nme_Sistema ,
          Dta_Cadastro ,
          num_transacao ,
          num_nfe ,
          Vlr_Total, 
		  Erro,
		  cod_verificacao
        )
SELECT sis.Nme_Sistema,
 ige.Dta_Cadastro ,
            ige.num_transacao ,
            fnfe.num_nfe ,
			fnfe.Vlr_Total,
			REPLACE(REPLACE(fnfe.Des_Mensagem_Erro,'''', ''),'<?xml version=1.0 encoding=iso-8859-1?>',''), 
			fnfe.Cod_Verificacao
    FROM    [EMPVW5205\PRDST].PLATAFORMA_EMPLOYER.employer.fat_integracao_gestao ige WITH(NOLOCK)
            JOIN [EMPVW5205\PRDST].PLATAFORMA_EMPLOYER.employer.fat_fatura_nfe  fnfe WITH(NOLOCK) ON fnfe.idf_fatura = ige.idf_fatura
            JOIN [EMPVW5205\PRDST].PLATAFORMA_EMPLOYER.plataforma.TAB_Sistema sis WITH(NOLOCK)  ON ige.Idf_Sistema = sis.Idf_Sistema
    WHERE   1 = 1
	AND ige.num_transacao IN ({ID})
	
	ORDER BY 1 desc

	IF EXISTS( SELECT * FROM @tabelaDados WHERE Erro IS NOT NULL )
	BEGIN 
		SET @Erro2 = (SELECT Erro FROM @tabelaDados WHERE Erro IS NOT NULL)
		UPDATE @tabelaDados SET Erro = @Erro2.value('(retorno/mensagem)[1]', 'varchar(200)')
	END 
		

SELECT Nme_Sistema ,
       Dta_Cadastro ,
       num_transacao ,
       num_nfe ,
       Vlr_Total ,
        ISNULL(Erro, '') AS Erro,
	   cod_verificacao
 FROM  @tabelaDados
";

        private const string SP_Lista_Notas =
        @"SELECT  ige.Dta_Cadastro ,
            ige.num_transacao ,
            fnfe.num_nfe ,
            fnfe.cod_verificacao
    FROM    [Empvw5205\PRDST].PLATAFORMA_EMPLOYER.employer.fat_integracao_gestao ige
            JOIN [Empvw5205\PRDST].PLATAFORMA_EMPLOYER.employer.fat_fatura_nfe fnfe ON fnfe.idf_fatura = ige.idf_fatura
            JOIN [Empvw5205\PRDST].PLATAFORMA_EMPLOYER.plataforma.TAB_Sistema sis ON ige.Idf_Sistema = sis.Idf_Sistema
    WHERE   1 = 1
            AND ige.num_transacao = '{ID}'";

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

        public static List<NotaFiscal> ObterNotas(List<string> DesIdentificador)
        {

            if (DesIdentificador.Count == 0)
                return new List<NotaFiscal>();
            List<NotaFiscal> lstRetorno = new List<NotaFiscal>();

            using (var conn = new SqlConnection(DataAccessLayer.CONN_PLATAFORMA))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    foreach (var item in DesIdentificador)
                    {

                        using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SP_Lista_Notas.Replace("{ID}", item), null))
                        {
                            while (dr.Read())
                            {
                                try
                                {
                                    if (string.IsNullOrEmpty(dr["num_nfe"].ToString()))
                                        continue;

                                    NotaFiscal nf = new NotaFiscal
                                    {
                                        Transacao = dr["num_transacao"].ToString().Trim(),
                                        NumeroNotaFiscal = Convert.ToInt32(dr["num_nfe"].ToString().Trim()),
                                        CodigoVerificacao = dr["cod_verificacao"].ToString().Trim()
                                    };

                                    lstRetorno.Add(nf);
                                }
                                catch { }
                            }

                            if (!dr.IsClosed)
                                dr.Close();

                            dr.Dispose();
                        }
                    }

                    conn.Close();
                }

            }
            return lstRetorno;
        }

        public static new List<NotaFiscal> VerificadorDeNotas(string identificador)
        {
            List<NotaFiscal> lstRetorno = new List<NotaFiscal>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_Lista_NotasComErro.Replace("{ID}", identificador), null, DataAccessLayer.CONN_PLATAFORMA))
            {
                while (dr.Read())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(dr["Nme_Sistema"].ToString()))
                            continue;

                        NotaFiscal nf = new NotaFiscal
                        {
                            Transacao = dr["num_transacao"].ToString().Trim(),
                            NumeroNotaFiscal = dr["num_nfe"] != DBNull.Value ? Convert.ToInt32(dr["num_nfe"]) : 0,
                            CodigoVerificacao = dr["cod_verificacao"].ToString().Trim(),
                            Erro = dr["Erro"] != DBNull.Value ? dr["Erro"].ToString() : "",
                            Data_Cadastro = dr["Dta_Cadastro"] != DBNull.Value ? dr["Dta_Cadastro"].ToString() : "",
                            NomeSistema = dr["Nme_Sistema"].ToString(),
                            ValorNota = dr["Vlr_Total"].ToString(),

                        };

                        lstRetorno.Add(nf);
                    }
                    catch (Exception ex)
                    {
                        return new List<NotaFiscal>();
                    }
                }

                if (!dr.IsClosed)
                    dr.Close();

                dr.Dispose();
            }
            return lstRetorno;
        }


    }
}
