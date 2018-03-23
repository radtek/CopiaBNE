using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class PagamentosPagarMe : BaseUserControl
    {
        private const string sql = @"

DECLARE @Dados_PagarMe TABLE
    (
      StatusPagamento VARCHAR(200) ,
      ID VARCHAR(100) ,
      Nome VARCHAR(100) ,
      NumerodoCartao VARCHAR(100) ,
      Valor DECIMAL(10, 2) ,
      ValorEstornado DECIMAL(10, 2) NULL
    );

DECLARE @DadosRetorno TABLE
    (
      StatusPagamento VARCHAR(100) ,
      Des_Codigo_Autorizacao VARCHAR(50) ,
      Sistema VARCHAR(20) ,
      Descrição_Plano VARCHAR(50) ,
      Nome_Razão_Social VARCHAR(200) ,
      CPF_CNPJ VARCHAR(200) ,
      Cartão VARCHAR(200) ,
      Valor_Plano DECIMAL(10, 2) ,
      Nota_Fiscal VARCHAR(200) ,
      Url_Nota_Fiscal VARCHAR(200) ,
      Data VARCHAR(20)
    );

INSERT  INTO @Dados_PagarMe
        ( StatusPagamento ,
          ID ,
          Nome ,
          NumerodoCartao 
        )
        SELECT  C.value('StatusPagamento[1]', 'VARCHAR(100)') AS StatusPagamento ,
                C.value('ID[1]', 'VARCHAR(100)') AS ID ,
                c.value('Nome[1]', 'VARCHAR(200)') AS NOME ,
                c.value('NumerodoCartao[1]', 'VARCHAR(20)') AS CARTAO
        FROM    @autorizacao.nodes('node') AS T ( C );

INSERT  INTO @DadosRetorno
        ( StatusPagamento ,
          Des_Codigo_Autorizacao ,
          Sistema ,
          Descrição_Plano ,
          Nome_Razão_Social ,
          CPF_CNPJ ,
          Cartão ,
          Valor_Plano ,
          Nota_Fiscal ,
          Url_Nota_Fiscal		  
		  
        )
        SELECT DISTINCT
                dc.StatusPagamento AS StatusNota ,
                pag.Des_Identificador COLLATE Latin1_General_CI_AS AS Des_Codigo_Autorizacao ,
                --t.Idf_Transacao AS Idf_Transacao ,
                'BNE' AS Sistema ,
                pl.Des_Plano COLLATE Latin1_General_CI_AS AS Descrição_Plano ,
                ISNULL(f.Raz_Social, pf.Nme_Pessoa) COLLATE Latin1_General_CI_AS AS Nome_Razão_Social ,
                ISNULL(f.Num_CNPJ, pf.Num_CPF) AS CPF_CNPJ ,
                tr.Des_Cartao_Mascarado COLLATE Latin1_General_CI_AS AS Cartão ,
                t.Vlr_Documento AS Valor_Plano ,
                pag.Num_Nota_Fiscal COLLATE Latin1_General_CI_AS AS Nota_Fiscal ,
                pag.Url_Nota_Fiscal COLLATE Latin1_General_CI_AS AS Url_Nota_Fiscal
        FROM    BNE.BNE_Transacao t WITH ( NOLOCK )
                JOIN @Dados_PagarMe dc ON dc.ID = t.Des_Transacao
                JOIN BNE.BNE_Pagamento pag WITH ( NOLOCK ) ON t.Idf_Pagamento = pag.Idf_Pagamento
                JOIN BNE.BNE_Transacao_Resposta tr WITH ( NOLOCK ) ON t.Idf_Transacao = tr.Idf_Transacao
                LEFT JOIN BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK ) ON t.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                JOIN BNE.BNE_Plano pl WITH ( NOLOCK ) ON pa.Idf_Plano = pl.Idf_Plano
                LEFT JOIN BNE.TAB_Filial f WITH ( NOLOCK ) ON pa.Idf_Filial = f.Idf_Filial
                LEFT JOIN BNE.TAB_Usuario_Filial_Perfil ufp WITH ( NOLOCK ) ON pa.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
                LEFT JOIN BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        GROUP BY dc.StatusPagamento ,
                pag.Des_Identificador COLLATE Latin1_General_CI_AS ,
                t.Idf_Transacao ,
                pl.Des_Plano COLLATE Latin1_General_CI_AS ,
                ISNULL(f.Raz_Social, pf.Nme_Pessoa) COLLATE Latin1_General_CI_AS ,
                ISNULL(f.Num_CNPJ, pf.Num_CPF) ,
                tr.Des_Cartao_Mascarado COLLATE Latin1_General_CI_AS ,
                t.Vlr_Documento ,
                pag.Num_Nota_Fiscal COLLATE Latin1_General_CI_AS ,
                pag.Url_Nota_Fiscal COLLATE Latin1_General_CI_AS;
       
SELECT  StatusPagamento ,
        Des_Codigo_Autorizacao ,
        Sistema ,
        Descrição_Plano ,
        Nome_Razão_Social ,
        CPF_CNPJ ,
        Cartão ,
        Valor_Plano ,
        Nota_Fiscal ,
        Url_Nota_Fiscal
FROM    @DadosRetorno;
        ";


        #region [sqlBoleto]
        private const string sqlBoleto = @"
DECLARE @Dados_PagarMe TABLE
    (
      StatusPagamento VARCHAR(200) ,
      ID VARCHAR(100) ,
      Nome VARCHAR(100) ,
      Documento VARCHAR(100) ,
      Valor DECIMAL(10, 2) ,
      ValorEstornado DECIMAL(10, 2) NULL
    );

DECLARE @DadosRetorno TABLE
    (
        Des_Codigo_Autorizacao VARCHAR(50) ,
     -- Idf_Transacao INT ,
      Sistema VARCHAR(20) ,
      Descrição_Plano VARCHAR(50) ,
      Nome_Razão_Social VARCHAR(200) ,
      CPF_CNPJ VARCHAR(200) ,
      Valor_Plano DECIMAL(10, 2) ,
      Nota_Fiscal VARCHAR(200) ,
      Url_Nota_Fiscal VARCHAR(200) ,
	  Codigo_Barra varchar(300)
    );

INSERT  INTO @Dados_PagarMe
        ( StatusPagamento ,
          ID ,
          Nome ,
		  Valor
        )
        SELECT  C.value('StatusPagamento[1]', 'VARCHAR(100)') AS StatusPagamento ,
                C.value('ID[1]', 'VARCHAR(100)') AS ID ,
                c.value('Nome[1]', 'VARCHAR(200)') AS NOME ,
				  c.value('Valor[1]', 'DECIMAL(10,2)') AS VALOR 
        FROM    @autorizacao.nodes('node') AS T ( C );

INSERT  INTO @DadosRetorno
        (  Des_Codigo_Autorizacao ,
         -- Idf_Transacao ,
          Sistema ,
          Descrição_Plano ,
          Nome_Razão_Social ,
          CPF_CNPJ ,
          Valor_Plano ,
          Nota_Fiscal ,
          Url_Nota_Fiscal,
		  Codigo_Barra
        )
        SELECT DISTINCT
                pag.Des_Identificador COLLATE Latin1_General_CI_AS AS Des_Codigo_Autorizacao ,
                'BNE' AS Sistema ,
                pl.Des_Plano COLLATE Latin1_General_CI_AS AS Descrição_Plano ,
                ISNULL(f.Raz_Social, pf.Nme_Pessoa) COLLATE Latin1_General_CI_AS AS Nome_Razão_Social ,
                ISNULL(f.Num_CNPJ, pf.Num_CPF) AS CPF_CNPJ ,
                pag.vlr_pagamento AS Valor_Plano ,
                pag.Num_Nota_Fiscal COLLATE Latin1_General_CI_AS AS Nota_Fiscal ,
                pag.Url_Nota_Fiscal COLLATE Latin1_General_CI_AS AS Url_Nota_Fiscal,
				pag.des_descricao COLLATE Latin1_General_CI_AS
        FROM    BNE_IMP.BNE.BNE_Pagamento pag WITH ( NOLOCK )
                JOIN @Dados_PagarMe dc ON dc.Id = pag.Des_Identificador
				join bne.bne_plano_parcela pp with(nolock) on pp.idf_plano_parcela = pag.idf_plano_parcela
                LEFT JOIN BNE_IMP.BNE.BNE_Plano_Adquirido pa WITH ( NOLOCK ) ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                JOIN BNE_IMP.BNE.BNE_Plano pl WITH ( NOLOCK ) ON pa.Idf_Plano = pl.Idf_Plano
                LEFT JOIN BNE_IMP.BNE.TAB_Filial f WITH ( NOLOCK ) ON pa.Idf_Filial = f.Idf_Filial
                LEFT JOIN BNE_IMP.BNE.TAB_Usuario_Filial_Perfil ufp WITH ( NOLOCK ) ON pa.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
                LEFT JOIN BNE_IMP.BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON ufp.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        --WHERE   pag.idf_pagamento_situacao <> 3 --cancelado
        GROUP BY pag.Des_Identificador COLLATE Latin1_General_CI_AS ,
                pl.Des_Plano COLLATE Latin1_General_CI_AS ,
                ISNULL(f.Raz_Social, pf.Nme_Pessoa) COLLATE Latin1_General_CI_AS ,
                ISNULL(f.Num_CNPJ, pf.Num_CPF) ,
                pag.vlr_pagamento ,
                pag.Num_Nota_Fiscal COLLATE Latin1_General_CI_AS ,
                pag.Url_Nota_Fiscal COLLATE Latin1_General_CI_AS,
				pag.des_descricao COLLATE Latin1_General_CI_AS
        UNION ALL
        SELECT DISTINCT
                tbb.Des_Identificador COLLATE Latin1_General_CI_AS AS Des_Codigo_Autorizacao ,
                --t.Idf_Transacao AS Idf_Transacao ,
                'SINE' AS Sistema ,
                tip.Des_Produto COLLATE Latin1_General_CI_AS AS Descrição_Plano ,
                u.Nme_Usuario COLLATE Latin1_General_CI_AS AS Nome_Razão_Social ,
                u.Num_CPF AS CPF_CNPJ ,
                t.Vlr_Transacao AS Valor_Plano ,
                t.Num_Nota_Fiscal COLLATE Latin1_General_CI_AS AS Nota_Fiscal ,
                t.Url_Nota_Fiscal COLLATE Latin1_General_CI_AS AS Url_Nota_Fiscal,
				tbb.Des_Descricao COLLATE Latin1_General_CI_AS
        FROM    SINE_PRD.pagamento.PAG_Transacao t WITH ( NOLOCK )
                JOIN SINE_PRD.pagamento.PAG_Transacao_Boleto tbb WITH ( NOLOCK ) ON t.Idf_Transacao = tbb.Idf_Transacao
                JOIN @Dados_PagarMe dc ON ( ( dc.Id COLLATE Latin1_General_CI_AS ) = tbb.des_identificador )
                LEFT JOIN SINE_PRD.SINE.SIN_Transacao_Produto tp WITH ( NOLOCK ) ON t.Idf_Transacao = tp.Idf_Transacao
                LEFT JOIN SINE_PRD.SINE.SIN_Produto p WITH ( NOLOCK ) ON tp.Idf_Produto = p.Idf_Produto
                LEFT JOIN SINE_PRD.SINE.TAB_Preco_Produto pp WITH ( NOLOCK ) ON p.Idf_Preco_Produto = pp.Idf_Preco_Produto
                LEFT JOIN SINE_PRD.SINE.TAB_Tipo_Produto tip WITH ( NOLOCK ) ON tip.Idf_Tipo_Produto = pp.Idf_Tipo_Produto
                LEFT JOIN SINE_PRD.SINE.SIN_Vaga v WITH ( NOLOCK ) ON p.Idf_Vaga = v.Idf_Vaga
                LEFT JOIN SINE_PRD.SINE.SIN_Usuario u WITH ( NOLOCK ) ON v.Idf_Usuario = u.Idf_Usuario
                INNER JOIN SINE_PRD.pagamento.TAB_Status_Transacao st ON st.Idf_Status_Transacao = t.Idf_Status_Transacao
        --WHERE   t.Idf_Status_Transacao = 2
        GROUP BY tbb.des_identificador COLLATE Latin1_General_CI_AS ,
                t.Idf_Transacao ,
                tip.Des_Produto COLLATE Latin1_General_CI_AS ,
                u.Nme_Usuario COLLATE Latin1_General_CI_AS ,
                u.Num_CPF ,
                t.Vlr_Transacao ,
                t.Num_Nota_Fiscal COLLATE Latin1_General_CI_AS ,
                t.Url_Nota_Fiscal COLLATE Latin1_General_CI_AS ,
                st.Des_Status_Transacao COLLATE Latin1_General_CI_AS,
				tbb.Des_Descricao COLLATE Latin1_General_CI_AS
        UNION ALL
        SELECT  DISTINCT
                pag.Des_Identificador COLLATE Latin1_General_CI_AS AS Des_Codigo_Autorizacao ,
                --t.Idf_Transacao AS Sistema ,
                'Salario BR' AS Descrição_Plano ,
                '' AS Descrição_Plano ,
                ISNULL(f.Raz_Social, pf.Nme_Pessoa) COLLATE Latin1_General_CI_AS AS Nome_Razão_Social ,
                ISNULL(f.Num_CNPJ, pf.Num_CPF) AS CPF_CNPJ ,
                v.Vlr_Total AS Valor_Plano ,
                pag.Num_Nota COLLATE Latin1_General_CI_AS AS Nota_Fiscal ,
                CONCAT('https://www.nfs-e.net/datacenter/include/nfw/nfw_imprime_notas.php?codauten=',
                       pag.Cod_Verificador) COLLATE Latin1_General_CI_AS AS Url_Nota_Fiscal,
					   pag.Boleto_Codigo_Barra COLLATE Latin1_General_CI_AS
        FROM    [SALARIOBR_PRD].[salariobr].[SBR_Pagamento] pag WITH ( NOLOCK )
                JOIN @Dados_PagarMe dc ON ( dc.Id = pag.Des_Identificador )
                --JOIN [SALARIOBR_PRD].salariobr.SBR_Transacao t WITH ( NOLOCK ) ON tr.Idf_Transacao = t.Idf_Transacao
                JOIN [SALARIOBR_PRD].salariobr.SBR_Venda v WITH ( NOLOCK ) ON pag.Idf_Venda = v.Idf_Venda
                JOIN SALARIOBR_PRD.salariobr.SBR_Pacote_Adquirido pa WITH ( NOLOCK ) ON v.Idf_Venda = pa.Idf_Venda
                LEFT JOIN SALARIOBR_PRD.salariobr.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON pa.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
                LEFT JOIN SALARIOBR_PRD.salariobr.TAB_Filial f WITH ( NOLOCK ) ON pa.Idf_Filial = f.Idf_Filial
        --WHERE   Idf_Pagamento_Situacao = 2
        GROUP BY pag.Des_Identificador COLLATE Latin1_General_CI_AS ,
                pag.Des_Identificador ,
                ISNULL(f.Raz_Social, pf.Nme_Pessoa) COLLATE Latin1_General_CI_AS ,
                ISNULL(f.Num_CNPJ, pf.Num_CPF) ,
                v.Vlr_Total ,
                pag.Num_Nota COLLATE Latin1_General_CI_AS ,
                CONCAT('https://www.nfs-e.net/datacenter/include/nfw/nfw_imprime_notas.php?codauten=',
                       pag.Cod_Verificador) COLLATE Latin1_General_CI_AS,
					     pag.Boleto_Codigo_Barra COLLATE Latin1_General_CI_AS

	
			   
SELECT  DISTINCT dr.Url_Nota_Fiscal,
		ISNULL(dr.Des_Codigo_Autorizacao, dc.Id) COLLATE Latin1_General_CI_AS AS Des_Codigo_Autorizacao ,
        --dr.Idf_Transacao ,
        dr.Sistema ,
        dr.Descrição_Plano ,
        dr.Nome_Razão_Social ,
        dr.CPF_CNPJ ,
        dc.valor AS Valor_Plano ,
        --ISNULL(dr.Valor_Plano, dc.valor * ( -1 )) AS Valor_Plano ,
        dr.Nota_Fiscal ,
        dc.StatusPagamento ,
        dc.valor,
		dr.Codigo_Barra
		--dc.vinculo AS codAut
		
FROM    @DadosRetorno dr
        FULL JOIN @Dados_PagarMe dc ON dc.Id = dr.Des_Codigo_Autorizacao
WHERE   dc.valor IS NOT NULL
";
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
       
        protected void btEnviar_Click(object sender, EventArgs e)
        {
            if (fCsvPagarMe.HasFile)
            {
                StreamReader sr = new StreamReader(new MemoryStream(fCsvPagarMe.FileBytes));
                string linha;
                string xml = string.Empty;
                while (sr.Peek() >= 0)
                {
                    linha = sr.ReadLine().Replace("\"", "");
                    string[] itens = linha.Split(',');
                    DateTime result;
                    if (itens[0].Equals("Status") || itens.Count() == 1)
                        continue;

                    xml += String.Join("",
                        "<node><StatusPagamento>" + itens[0] + "</StatusPagamento><ID>" + itens[1] + "</ID><Data>" + Convert.ToDateTime(itens[2]) +
                        "</Data><Nome>" +
                        itens[3] + "</Nome><NumerodoCartao>" + itens[5] + "</NumerodoCartao><Valor>" +
                        itens[11].ToString() + "</Valor><ValorEstornado>" + itens[12].ToString().Replace("-","") + "</ValorEstornado> </node>");

                }
                if (xml.IsEmpty()) base.ExibirMensagem("Não Existe informações no arquivo!", Code.Enumeradores.TipoMensagem.Aviso);
                else
                {
                    List<SqlParameter> parms = new List<SqlParameter>();
                    parms.Add(new SqlParameter("@autorizacao", SqlDbType.Xml));
                    parms[0].Value = xml;

                    var resultado = DataAccessLayer.ExecuteReader(CommandType.Text, sql, parms);

                    gvPagamentosPagarMe.DataSource = resultado;
                    gvPagamentosPagarMe.DataBind();
                 
                    pnlBoleto.Visible = false;
                    pnlCartao.Visible = true;
                    upPagamentos.Update();

                }
            }
        }

        protected void btEnviarBoleto_Click(object sender, EventArgs e)
        {
            if (fuCvsBoleto.HasFile)
            {
                StreamReader sr = new StreamReader(new MemoryStream(fuCvsBoleto.FileBytes));
                string linha;
                string xml = string.Empty;
                Regex regex = new Regex("((?![\"]).)([,]+)");
                while (sr.Peek() >= 0)
                {
                    linha =  regex.Replace(sr.ReadLine(), " ");
                    linha = linha.Replace("\"", "");
                    string[] itens = linha.Split(',');
                    DateTime result;
                    if (itens[0].Equals("Status") || itens.Count() == 1)
                        continue;

                    xml += String.Join("",
                        "<node><StatusPagamento>" + itens[0] + "</StatusPagamento><ID>" + itens[1] + "</ID><Data>" + Convert.ToDateTime(itens[2]) +
                        "</Data><Nome>" +
                        itens[5] + "</Nome><Documento>" + itens[8] + "</Documento><Valor>" +
                        itens[12].ToString() + "</Valor><ValorEstornado>" + itens[14].ToString().Replace("-", "") + "</ValorEstornado> </node>");

                }
                if (xml.IsEmpty()) base.ExibirMensagem("Não Existe informações no arquivo!", Code.Enumeradores.TipoMensagem.Aviso);
                else
                {
                    xml = xml.Replace("&", " ").Replace("'", " ");
                    List<SqlParameter> parms = new List<SqlParameter>();
                    parms.Add(new SqlParameter("@autorizacao", SqlDbType.Xml));
                    parms[0].Value = xml;

                    var resultado = DataAccessLayer.ExecuteReader(CommandType.Text, sqlBoleto, parms);

                    gvPagamentosBoleto.DataSource = resultado;

                    gvPagamentosBoleto.DataBind();

                    pnlBoleto.Visible = true;
                    pnlCartao.Visible = false;
                    upPagamentos.Update();
                   
                }
            }
        }
    }
}