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

                    upPagamentos.Update();

                }
            }
        }

    }
}