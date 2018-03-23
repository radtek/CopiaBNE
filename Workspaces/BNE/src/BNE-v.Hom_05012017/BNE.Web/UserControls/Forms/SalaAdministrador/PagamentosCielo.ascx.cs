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
using System.Text.RegularExpressions;
using System.Collections;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class PagamentosCielo : BaseUserControl
    {
        private const string sql = @"DECLARE @Dados_Cielo TABLE
    (
      codAut VARCHAR(6) ,
      dta_Cadastro DATE ,
      des_identificador VARCHAR(20)
    )

INSERT  INTO @Dados_Cielo
        ( codAut ,
          dta_Cadastro ,
          des_identificador
        )
        SELECT  C.value('codAut[1]', 'VARCHAR(6)') AS ID ,
                CONVERT(DATE, C.value('dtaCad[1]', 'VARCHAR(10)'), 103) AS DTA_CAD ,
                C.value('desIdent[1]', 'VARCHAR(20)') AS IDENT
        FROM    @autorizacao.nodes('node') AS T ( C )

SELECT DISTINCT
        pag.Des_Identificador COLLATE Latin1_General_CI_AS AS Des_Codigo_Autorizacao ,
        t.Idf_Transacao AS Idf_Transacao ,
        'BNE' AS Sistema ,
        pl.Des_Plano COLLATE Latin1_General_CI_AS AS Descrição_Plano ,
        ISNULL(f.Raz_Social, pf.Nme_Pessoa) COLLATE Latin1_General_CI_AS AS Nome_Razão_Social ,
        ISNULL(f.Num_CNPJ, pf.Num_CPF) AS CPF_CNPJ ,
        tr.Des_Cartao_Mascarado COLLATE Latin1_General_CI_AS AS Cartão ,
        t.Vlr_Documento AS Valor_Plano ,
        pag.Num_Nota_Fiscal COLLATE Latin1_General_CI_AS AS Nota_Fiscal ,
        pag.Url_Nota_Fiscal COLLATE Latin1_General_CI_AS AS Url_Nota_Fiscal 
FROM    BNE.BNE_Transacao t WITH ( NOLOCK )
        JOIN @Dados_Cielo dc ON dc.des_identificador = t.Des_Transacao
        JOIN BNE_IMP.BNE.BNE_Pagamento pag WITH ( NOLOCK ) ON t.Idf_Pagamento = pag.Idf_Pagamento
        LEFT JOIN BNE_IMP.BNE.BNE_Transacao_Resposta tr WITH ( NOLOCK ) ON t.Idf_Transacao = tr.Idf_Transacao
        JOIN BNE_IMP.BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK ) ON t.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
        JOIN BNE_IMP.BNE.BNE_Plano pl WITH ( NOLOCK ) ON pa.Idf_Plano = pl.Idf_Plano
        LEFT JOIN BNE_IMP.BNE.TAB_Filial f WITH ( NOLOCK ) ON pa.Idf_Filial = f.Idf_Filial
        LEFT JOIN BNE_IMP.BNE.TAB_Usuario_Filial_Perfil ufp WITH ( NOLOCK ) ON pa.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
        LEFT JOIN BNE_IMP.BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
WHERE
		idf_Pagamento_Situacao = 2
 GROUP BY
		  pag.Des_Identificador COLLATE Latin1_General_CI_AS,
        t.Idf_Transacao ,
        pl.Des_Plano COLLATE Latin1_General_CI_AS ,
        ISNULL(f.Raz_Social, pf.Nme_Pessoa) COLLATE Latin1_General_CI_AS,
        ISNULL(f.Num_CNPJ, pf.Num_CPF),
        tr.Des_Cartao_Mascarado COLLATE Latin1_General_CI_AS ,
        t.Vlr_Documento,
        pag.Num_Nota_Fiscal COLLATE Latin1_General_CI_AS ,
        pag.Url_Nota_Fiscal COLLATE Latin1_General_CI_AS  
UNION ALL
SELECT DISTINCT trc.Des_Codigo_Autorizacao COLLATE Latin1_General_CI_AS AS Des_Codigo_Autorizacao ,
        t.Idf_Transacao AS Idf_Transacao ,
        'SINE' AS Sistema ,
        tip.Des_Produto COLLATE Latin1_General_CI_AS AS Descrição_Plano ,
        u.Nme_Usuario COLLATE Latin1_General_CI_AS AS Nome_Razão_Social ,
        u.Num_CPF AS CPF_CNPJ ,
        trc.Des_Cartao_Mascarado COLLATE Latin1_General_CI_AS AS Cartão ,
        t.Vlr_Transacao AS Valor_Plano ,
        t.Num_Nota_Fiscal COLLATE Latin1_General_CI_AS AS Nota_Fiscal ,
        t.Url_Nota_Fiscal COLLATE Latin1_General_CI_AS AS Url_Nota_Fiscal
FROM    SINE_PRD.pagamento.PAG_Transacao t WITH ( NOLOCK )
        JOIN SINE_PRD.pagamento.PAG_Transacao_Cartao_Credito tcc WITH ( NOLOCK ) ON t.Idf_Transacao = tcc.Idf_Transacao
        JOIN SINE_PRD.pagamento.PAG_Transacao_Resposta_Cartao trc WITH ( NOLOCK ) ON tcc.Idf_Cartao_Credito = trc.Idf_Cartao_Credito
        JOIN @Dados_Cielo dc ON ((dc.des_identificador COLLATE Latin1_General_CI_AS) = trc.Des_Codigo_Autorizacao )
        LEFT JOIN SINE_PRD.SINE.SIN_Transacao_Produto tp WITH ( NOLOCK ) ON t.Idf_Transacao = tp.Idf_Transacao
        LEFT JOIN SINE_PRD.SINE.SIN_Produto p WITH ( NOLOCK ) ON tp.Idf_Produto = p.Idf_Produto
        LEFT JOIN SINE_PRD.SINE.TAB_Preco_Produto pp WITH ( NOLOCK ) ON p.Idf_Preco_Produto = pp.Idf_Preco_Produto
        LEFT JOIN SINE_PRD.SINE.TAB_Tipo_Produto tip WITH ( NOLOCK ) ON tip.Idf_Tipo_Produto = pp.Idf_Tipo_Produto
        LEFT JOIN SINE_PRD.SINE.SIN_Vaga v WITH ( NOLOCK ) ON p.Idf_Vaga = v.Idf_Vaga
        LEFT JOIN SINE_PRD.SINE.SIN_Usuario u WITH ( NOLOCK ) ON v.Idf_Usuario = u.Idf_Usuario
WHERE
        Idf_Status_Transacao = 2
GROUP BY
		trc.Des_Codigo_Autorizacao COLLATE Latin1_General_CI_AS ,
        t.Idf_Transacao ,
        tip.Des_Produto COLLATE Latin1_General_CI_AS ,
        u.Nme_Usuario COLLATE Latin1_General_CI_AS  ,
        u.Num_CPF ,
        trc.Des_Cartao_Mascarado COLLATE Latin1_General_CI_AS  ,
        t.Vlr_Transacao  ,
        t.Num_Nota_Fiscal COLLATE Latin1_General_CI_AS  ,
        t.Url_Nota_Fiscal COLLATE Latin1_General_CI_AS  

UNION ALL
SELECT  DISTINCT tr.Des_Codigo_Autorizacao COLLATE Latin1_General_CI_AS AS Des_Codigo_Autorizacao ,
        t.Idf_Transacao AS Sistema ,
        'Salario BR' AS Descrição_Plano ,
        '' AS Descrição_Plano ,
        ISNULL(f.Raz_Social, pf.Nme_Pessoa) COLLATE Latin1_General_CI_AS AS Nome_Razão_Social ,
        ISNULL(f.Num_CNPJ, pf.Num_CPF) AS CPF_CNPJ ,
        tr.Des_Cartao_Mascarado COLLATE Latin1_General_CI_AS AS Cartão ,
        t.Vlr_Documento AS Valor_Plano ,
        pag.Num_Nota COLLATE Latin1_General_CI_AS AS Nota_Fiscal ,
        CONCAT('https://nfe.prefeitura.sp.gov.br/contribuinte/notaprint.aspx?ccm=40954625&nf=',
               pag.Num_Nota, '&cod=', pag.Cod_Verificador) COLLATE Latin1_General_CI_AS AS Url_Nota_Fiscal
FROM    [SALARIOBR_PRD].[salariobr].[SBR_Pagamento] pag WITH ( NOLOCK )
        JOIN [SALARIOBR_PRD].salariobr.SBR_Transacao_Resposta tr WITH ( NOLOCK ) ON pag.Des_Identificador = tr.Des_Transacao
        JOIN @Dados_Cielo dc ON (dc.des_identificador = tr.Des_Codigo_Autorizacao )
        JOIN [SALARIOBR_PRD].salariobr.SBR_Transacao t WITH ( NOLOCK ) ON tr.Idf_Transacao = t.Idf_Transacao
        JOIN [SALARIOBR_PRD].salariobr.SBR_Venda v WITH ( NOLOCK ) ON pag.Idf_Venda = v.Idf_Venda
        JOIN SALARIOBR_PRD.salariobr.SBR_Pacote_Adquirido pa WITH ( NOLOCK ) ON v.Idf_Venda = pa.Idf_Venda
        LEFT JOIN SALARIOBR_PRD.salariobr.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON pa.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        LEFT JOIN SALARIOBR_PRD.salariobr.TAB_Filial f WITH ( NOLOCK ) ON pa.Idf_Filial = f.Idf_Filial
WHERE
        Idf_Pagamento_Situacao = 2
GROUP BY
		tr.Des_Codigo_Autorizacao COLLATE Latin1_General_CI_AS ,
        t.Idf_Transacao,
        ISNULL(f.Raz_Social, pf.Nme_Pessoa) COLLATE Latin1_General_CI_AS ,
        ISNULL(f.Num_CNPJ, pf.Num_CPF) ,
        tr.Des_Cartao_Mascarado COLLATE Latin1_General_CI_AS ,
        t.Vlr_Documento  ,
        pag.Num_Nota COLLATE Latin1_General_CI_AS ,
        CONCAT('https://nfe.prefeitura.sp.gov.br/contribuinte/notaprint.aspx?ccm=40954625&nf=',
               pag.Num_Nota, '&cod=', pag.Cod_Verificador) COLLATE Latin1_General_CI_AS  ";

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
                    if (!DateTime.TryParseExact(itens[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) || (Convert.ToDecimal(itens[7]) < 0))
                        continue;
                    xml += String.Join("", "<node><codAut>" + itens[6].PadLeft(6, '0') + "</codAut><dtaCad>" + result.ToString() + "</dtaCad><desIdent>" + Regex.Replace(itens[4], "[^0-9a-zA-Z]+", "") + "</desIdent></node>");
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