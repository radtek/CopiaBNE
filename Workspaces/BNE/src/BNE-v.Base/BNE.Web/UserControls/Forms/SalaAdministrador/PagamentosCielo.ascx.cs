using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Globalization;
using BNE.BLL;
using System.Data.SqlClient;
using System.Data;
using BNE.Web.Code;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class PagamentosCielo : BaseUserControl
    {
        private const string sql = @"declare @Dados_Cielo TABLE(codAut VARCHAR(6),dta_Cadastro DATE)

INSERT INTO @Dados_Cielo
        ( codAut,dta_Cadastro)
SELECT 
    C.value('codAut[1]','varchar(6)') as ID,
	CONVERT(DATE,C.value('dtaCad[1]','VARCHAR(10)'),103) as DTA_CAD
	     
FROM 
    @autorizacao.nodes('node') as T(C)

SELECT  tr.Des_Codigo_Autorizacao collate Latin1_General_CI_AS AS Des_Codigo_Autorizacao,
        t.Idf_Transacao AS Idf_Transacao,
        'BNE' AS Sistema ,
        pl.Des_Plano collate Latin1_General_CI_AS  AS Descrição_Plano,
        ISNULL(f.Raz_Social, pf.Nme_Pessoa) collate Latin1_General_CI_AS AS Nome_Razão_Social,
        ISNULL(f.Num_CNPJ, pf.Num_CPF) AS CPF_CNPJ,
        tr.Des_Cartao_Mascarado collate Latin1_General_CI_AS AS Cartão,
        t.Vlr_Documento AS Valor_Plano ,
        pag.Num_Nota_Fiscal AS Nota_Fiscal,
        pag.Url_Nota_Fiscal AS Url_Nota_Fiscal ,
        tr.Dta_Cadastro AS Data_Pagamento
FROM    BNE.BNE_Transacao t --OUTER APPLY (SELECT 
--				MAX(Idf_Transacao) AS Idf_Transacao
--			FROM 
--				BNE_IMP.BNE.BNE_Transacao_Resposta 
--			WHERE 
--				tr.Des_Codigo_Autorizacao IN (SELECT codAut FROM #tmpCodAut)) AS transacao_resposta
        LEFT JOIN BNE_IMP.BNE.BNE_Transacao_Resposta tr WITH ( NOLOCK ) ON t.Idf_Transacao = tr.Idf_Transacao
        INNER JOIN @Dados_Cielo dc ON dc.codAut = tr.Des_Codigo_Autorizacao
                                      AND ( dc.dta_Cadastro = CAST(tr.Dta_Cadastro AS DATE) )
        LEFT JOIN BNE_IMP.BNE.BNE_Pagamento pag WITH ( NOLOCK ) ON t.Des_Transacao = pag.Des_Identificador
        JOIN BNE_IMP.BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK ) ON t.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
        JOIN BNE_IMP.BNE.BNE_Plano pl WITH ( NOLOCK ) ON pa.Idf_Plano = pl.Idf_Plano
        LEFT JOIN BNE_IMP.BNE.TAB_Filial f WITH ( NOLOCK ) ON pa.Idf_Filial = f.Idf_Filial
        LEFT JOIN BNE_IMP.BNE.TAB_Usuario_Filial_Perfil ufp WITH ( NOLOCK ) ON pa.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
        LEFT JOIN BNE_IMP.BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
UNION ALL
SELECT  trc.Des_Codigo_Autorizacao collate Latin1_General_CI_AS AS Des_Codigo_Autorizacao,
        t.Idf_Transacao AS Idf_Transacao ,
        'SINE' AS Sistema,
        tip.Des_Produto collate Latin1_General_CI_AS AS Descrição_Plano ,
        u.Nme_Usuario collate Latin1_General_CI_AS AS Nome_Razão_Social ,
        u.Num_CPF AS CPF_CNPJ ,
        trc.Des_Cartao_Mascarado collate Latin1_General_CI_AS AS Cartão,
        t.Vlr_Transacao AS  Valor_Plano,
		--'','',
        nota.num_nfe  AS  Nota_Fiscal,
        CONCAT('https://nfe.prefeitura.sp.gov.br/contribuinte/notaprint.aspx?ccm=40954625&nf=',
               nota.num_nfe, '&cod=', nota.cod_verificacao) AS Url_Nota_Fiscal, 
        trc.Dta_Cadastro AS Data_Pagamento
FROM    SINE_PRD.pagamento.PAG_Transacao t WITH ( NOLOCK )
        JOIN SINE_PRD.pagamento.PAG_Transacao_Cartao_Credito tcc WITH ( NOLOCK ) ON t.Idf_Transacao = tcc.Idf_Transacao
        JOIN SINE_PRD.pagamento.PAG_Transacao_Resposta_Cartao trc WITH ( NOLOCK ) ON tcc.Idf_Cartao_Credito = trc.Idf_Cartao_Credito
        JOIN @Dados_Cielo dc ON ( dc.codAut COLLATE Latin1_General_CI_AS ) = trc.Des_Codigo_Autorizacao
                                AND ( dc.dta_Cadastro = CAST(trc.Dta_Cadastro AS DATE) )
        LEFT JOIN SINE_PRD.SINE.SIN_Transacao_Produto tp WITH ( NOLOCK ) ON t.Idf_Transacao = tp.Idf_Transacao
        LEFT JOIN SINE_PRD.SINE.SIN_Produto p WITH ( NOLOCK ) ON tp.Idf_Produto = p.Idf_Produto
        LEFT JOIN SINE_PRD.SINE.TAB_Preco_Produto pp WITH ( NOLOCK ) ON p.Idf_Preco_Produto = pp.Idf_Preco_Produto
        LEFT JOIN SINE_PRD.SINE.TAB_Tipo_Produto tip WITH ( NOLOCK ) ON tip.Idf_Tipo_Produto = pp.Idf_Tipo_Produto
        LEFT JOIN SINE_PRD.SINE.SIN_Vaga v WITH ( NOLOCK ) ON p.Idf_Vaga = v.Idf_Vaga
        LEFT JOIN SINE_PRD.SINE.SIN_Usuario u WITH ( NOLOCK ) ON v.Idf_Usuario = u.Idf_Usuario
        OUTER APPLY ( SELECT    fnfe.num_nfe ,
                                fnfe.cod_verificacao
                      FROM      [EMPVW0365\PRD].PLATAFORMA_EMPLOYER.employer.fat_integracao_gestao ige
                                WITH ( NOLOCK )
                                LEFT JOIN [EMPVW0365\PRD].PLATAFORMA_EMPLOYER.employer.fat_fatura_nfe fnfe
                                WITH ( NOLOCK ) ON fnfe.idf_fatura = ige.idf_fatura
                                LEFT JOIN [EMPVW0365\PRD].PLATAFORMA_EMPLOYER.plataforma.TAB_Sistema sis
                                WITH ( NOLOCK ) ON ige.Idf_Sistema = sis.Idf_Sistema
                      WHERE     ige.Num_Transacao = trc.Des_Codigo_Autorizacao COLLATE Latin1_General_CI_AI
                    ) AS nota
UNION ALL
SELECT  tr.Des_Codigo_Autorizacao collate Latin1_General_CI_AS AS Des_Codigo_Autorizacao,
        t.Idf_Transacao AS Sistema,
        'Salario BR' AS Descrição_Plano,
        '' AS Descrição_Plano,
        ISNULL(f.Raz_Social, pf.Nme_Pessoa) collate Latin1_General_CI_AS AS Nome_Razão_Social,
        ISNULL(f.Num_CNPJ, pf.Num_CPF) AS CPF_CNPJ,
        tr.Des_Cartao_Mascarado collate Latin1_General_CI_AS AS Cartão,
        t.Vlr_Documento AS Valor_Plano,
		--'','',
        nota.num_nfe AS Nota_Fiscal,
        CONCAT('https://nfe.prefeitura.sp.gov.br/contribuinte/notaprint.aspx?ccm=40954625&nf=',
               nota.num_nfe, '&cod=', nota.cod_verificacao) AS Url_Nota_Fiscal,
        tr.Dta_Cadastro AS Dta_Cadastro
FROM    [SALARIOBR_PRD].[salariobr].[SBR_Pagamento] pag WITH ( NOLOCK )
        JOIN [SALARIOBR_PRD].salariobr.SBR_Transacao_Resposta tr WITH ( NOLOCK ) ON pag.Des_Identificador = tr.Des_Transacao
        JOIN @Dados_Cielo dc ON dc.codAut = tr.Des_Codigo_Autorizacao
                                AND ( dc.dta_Cadastro = CAST(tr.Dta_Cadastro AS DATE) )
        JOIN [SALARIOBR_PRD].salariobr.SBR_Transacao t WITH ( NOLOCK ) ON tr.Idf_Transacao = t.Idf_Transacao
        JOIN [SALARIOBR_PRD].salariobr.SBR_Venda v WITH ( NOLOCK ) ON pag.Idf_Venda = v.Idf_Venda
        JOIN SALARIOBR_PRD.salariobr.SBR_Pacote_Adquirido pa WITH ( NOLOCK ) ON v.Idf_Venda = pa.Idf_Venda
        LEFT JOIN SALARIOBR_PRD.salariobr.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON pa.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        LEFT JOIN SALARIOBR_PRD.salariobr.TAB_Filial f WITH ( NOLOCK ) ON pa.Idf_Filial = f.Idf_Filial
        OUTER APPLY ( SELECT    fnfe.num_nfe ,
                                fnfe.cod_verificacao
                      FROM      [EMPVW0365\PRD].PLATAFORMA_EMPLOYER.employer.fat_integracao_gestao ige
                                WITH ( NOLOCK )
                                LEFT JOIN [EMPVW0365\PRD].PLATAFORMA_EMPLOYER.employer.fat_fatura_nfe fnfe
                                WITH ( NOLOCK ) ON fnfe.idf_fatura = ige.idf_fatura
                                LEFT JOIN [EMPVW0365\PRD].PLATAFORMA_EMPLOYER.plataforma.TAB_Sistema sis
                                WITH ( NOLOCK ) ON ige.Idf_Sistema = sis.Idf_Sistema
                      WHERE     ige.Num_Transacao = tr.Des_Transacao COLLATE Latin1_General_CI_AI
                    ) AS nota";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region btEnviar_Click
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            if (fCsvCielo.HasFile)
            {
                StreamReader sr = new StreamReader(new MemoryStream(fCsvCielo.FileBytes));
                string linha;
                string xml = string.Empty;
                while (sr.Peek() >= 0)
                {
                    linha = sr.ReadLine().Replace("\"","");
                    string[] itens = linha.Split(';');
                    DateTime result;
                    if (!DateTime.TryParseExact(itens[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                        continue;
                    xml += String.Join("", "<node><codAut>" + itens[6].PadLeft(6, '0') + "</codAut><dtaCad>" + result.ToString() + "</dtaCad></node>");
                }
                if (xml.IsEmpty()) base.ExibirMensagem("Não Existe informações no arquivo!",Code.Enumeradores.TipoMensagem.Aviso);
                else
                {
                    List<SqlParameter> parms = new List<SqlParameter>();
                    parms.Add(new SqlParameter("@autorizacao", SqlDbType.Xml));
                    parms[0].Value = xml;

                    gvPagamentosCielo.DataSource = DataAccessLayer.ExecuteReader(CommandType.Text, sql, parms);
                    gvPagamentosCielo.DataBind();

                    upPagamentos.Update();

                }
                
            }
        }
        #endregion
    }
}