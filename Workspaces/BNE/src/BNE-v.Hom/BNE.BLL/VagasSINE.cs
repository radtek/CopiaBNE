using BNE.BLL.AsyncServices;
using BNE.BLL.Common.Sitemap;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.DTO;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace BNE.BLL
{
    public class VagasSINE
    {
        #region Spselectprojetossemrodar
        private const string Spselectprojetossemrodar = @"
      
       SELECT 
        Idf_Origem_Importacao, Des_Origem_Importacao, Des_Endereco
        FROM SINE_PRD.SINE.TAB_Origem_Importacao OI WITH(NOLOCK)
        OUTER APPLY
        (
        SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
        WHERE V.Dta_Importacao BETWEEN DATEADD(DAY, -10, GETDATE()) AND GETDATE()
        AND V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
        AND Idf_Situacao_Vaga IN ( 2, 3 )
        ) ultimos10Dias
        WHERE oi.Flg_Inativo = 0
        AND ultimos10Dias.total <= 10

        ";
        #endregion

        #region Spselecttotalprojetossemrodar
        private const string Spselecttotalprojetossemrodar = @"
      
        SELECT 
        COUNT(*) as 'totalGeral'
        FROM SINE_PRD.SINE.TAB_Origem_Importacao OI WITH(NOLOCK)
        OUTER APPLY
        (
        SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
        WHERE V.Dta_Importacao BETWEEN DATEADD(DAY, -10, GETDATE()) AND GETDATE()
        AND V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
        AND Idf_Situacao_Vaga IN ( 2, 3 )
        ) ultimos10Dias
        WHERE oi.Flg_Inativo = 0
        AND ultimos10Dias.total <= 10

        ";
        #endregion

        #region Spselectimportacaovagas
        private const string Spselectimportacaovagas = @"
        SELECT * FROM (
        SELECT 
        oi.Des_Endereco,
        oi.Idf_Origem_Importacao, 
        oi.Des_Origem_Importacao,
        hoje.total AS 'Hoje',
        ultimos10Dias.total AS 'Ultimos 10 dias',
        ultimos30Dias.total AS 'Ultimos 30 dias',
        ultimos60Dias.total AS 'Ultimos 60 dias'
        FROM SINE_PRD.SINE.TAB_Origem_Importacao OI WITH(NOLOCK)
        OUTER APPLY
        (
	        SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
	        WHERE V.Dta_Importacao BETWEEN DATEADD(DAY, -2, GETDATE()) AND GETDATE()
	        AND V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
	        AND Idf_Situacao_Vaga IN ( 2, 3 )
        ) hoje
        OUTER APPLY
        (
	        SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
	        WHERE V.Dta_Importacao BETWEEN DATEADD(DAY, -10, GETDATE()) AND GETDATE()
	        AND V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
	        AND Idf_Situacao_Vaga IN ( 2, 3 )
        ) ultimos10Dias
        OUTER APPLY
        (
	        SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
	        WHERE V.Dta_Importacao BETWEEN DATEADD(DAY, -30, GETDATE()) AND GETDATE()
	        AND V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
	        AND Idf_Situacao_Vaga IN ( 2, 3 )
        ) ultimos30Dias
        OUTER APPLY
        (
	        SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
	        
	        WHERE V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
	        AND Idf_Situacao_Vaga IN ( 2, 3 )
        ) ultimos60Dias
        WHERE oi.Flg_Inativo = 0 AND ultimos10Dias.total <= 0) AS t
        ORDER BY t.[Ultimos 60 dias] DESC

        ";
        #endregion

        #region Spselecttotalvagasimportadas
        private const string Spselecttotalvagasimportadas = @"
  DECLARE @dataAtual DATE = CAST(GETDATE() AS date)


   SELECT * FROM (
SELECT 
SUM(hoje.total) AS 'Hoje',
SUM(ultimos10Dias.total) AS 'Ultimos 10 dias',
SUM(ultimos30Dias.total) AS 'Ultimos 30 dias',
SUM(ultimos60Dias.total) AS 'Ultimos 60 dias',
SUM(ontem.total) AS 'Ontem'

FROM SINE_PRD.SINE.TAB_Origem_Importacao OI WITH(NOLOCK)
OUTER APPLY
(
	SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
	WHERE V.Dta_Importacao BETWEEN DATEADD(DAY, -1, @dataAtual) AND @dataAtual
	AND V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
	AND Idf_Situacao_Vaga IN ( 2, 3 )
) ontem
OUTER APPLY
(
	SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
	WHERE V.Dta_Importacao BETWEEN  @dataAtual AND DATEADD(DAY, +1, @dataAtual)
	AND V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
	AND Idf_Situacao_Vaga IN ( 2, 3 )
) hoje
OUTER APPLY
(
	SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
	WHERE V.Dta_Importacao BETWEEN DATEADD(DAY, -10, @dataAtual) AND @dataAtual
	AND V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
	AND Idf_Situacao_Vaga IN ( 2, 3 )
) ultimos10Dias
OUTER APPLY
(
	SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
	WHERE V.Dta_Importacao BETWEEN DATEADD(DAY, -30, @dataAtual) AND @dataAtual
	AND V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
	AND Idf_Situacao_Vaga IN ( 2, 3 )
) ultimos30Dias
OUTER APPLY
(
	SELECT COUNT(V.idf_vaga) AS 'total' FROM SINE_PRD.SINE.SIN_Vaga v WITH(NOLOCK)
	WHERE V.Dta_Importacao BETWEEN DATEADD(DAY, -60, @dataAtual) AND @dataAtual
	AND V.Idf_Origem_Importacao = OI.Idf_Origem_Importacao
	AND Idf_Situacao_Vaga IN ( 2, 3 )
) ultimos60Dias
WHERE oi.Flg_Inativo = 0) AS t
ORDER BY t.[Ultimos 10 dias] DESC, t.[Ultimos 30 dias] DESC, t.[Ultimos 60 dias] DESC

        ";
        #endregion

        #region Spselectstatusprojetos
        private const string Spselectstatusprojetos = @"
        SELECT  IIF(total>10,'Ativos','Com problema') Obs INTO #tabfinal
FROM    ( SELECT    oi.Des_origem_importacao ,
                    COUNT(V.idf_vaga) AS 'total'
          FROM      SINE_PRD.SINE.TAB_Origem_Importacao oi WITH ( NOLOCK ) 
                    LEFT JOIN SINE_PRD.SINE.SIN_Vaga v WITH ( NOLOCK ) ON v.Idf_Origem_Importacao = oi.Idf_Origem_Importacao AND V.Dta_Importacao BETWEEN DATEADD(DAY, -10, GETDATE())
                                     AND     GETDATE() AND Idf_Situacao_Vaga IN ( 2, 3 )
          WHERE     oi.Flg_Inativo = 0
          GROUP BY  oi.Des_origem_importacao
        ) ori
		

SELECT Obs, COUNT(*) Quantidade, (SELECT COUNT(*) FROM #tabfinal) Total FROM #tabfinal GROUP BY Obs
DROP TABLE #tabfinal


        ";
        #endregion

        public static List<ProjetosParados> ListarStatusProjetos()
        {
            List<ProjetosParados> projetos = new List<ProjetosParados>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectstatusprojetos, null))
            {
                while (dr.Read())
                    projetos.Add(new ProjetosParados { Obs = dr["Obs"].ToString(), Quantidade = Convert.ToInt32(dr["Quantidade"]), Total = Convert.ToInt32(dr["Total"]) });

            }

            return projetos;
        }

        public static List<ProjetosParados> ListarTotalVagasImportadas()
        {
            List<ProjetosParados> projetos = new List<ProjetosParados>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselecttotalvagasimportadas, null))
            {
                while (dr.Read())
                    projetos.Add(new ProjetosParados { Hoje = dr["Hoje"].ToString(), DezDias = dr["Ultimos 10 dias"].ToString(), TrintaDias = dr["Ultimos 30 dias"].ToString(), SessentaDias = dr["Ultimos 60 dias"].ToString(), Ontem = dr["Ontem"].ToString() });

            }

            return projetos;
        }

        public static int ListarTotalProjetosSemRodar()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselecttotalprojetossemrodar, null))
            {
                while (dr.Read())
                    return Convert.ToInt32(dr["totalGeral"]);
            }

            return 0;
        }

        public static List<ProjetosParados> ListarProjetosSemRodar()
        {
            List<ProjetosParados> projetos = new List<ProjetosParados>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectprojetossemrodar, null))
            {
                while (dr.Read())
                    projetos.Add(new ProjetosParados { Idf_Origem_Importacao = Convert.ToInt32(dr["Idf_Origem_Importacao"]), Des_Origem_Importacao = dr["Des_Origem_Importacao"].ToString(), Des_Endereco = dr["Des_Endereco"].ToString() });
            }

            return projetos;
        }

        public static List<ProjetosParados> ListarProjetosQtdVagas()
        {
            List<ProjetosParados> projetos = new List<ProjetosParados>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectimportacaovagas, null))
            {
                while (dr.Read())
                    projetos.Add(new ProjetosParados { Idf_Origem_Importacao = Convert.ToInt32(dr["Idf_Origem_Importacao"]), Des_Origem_Importacao = dr["Des_Origem_Importacao"].ToString(), Des_Endereco = dr["Des_Endereco"].ToString(), Hoje = dr["Hoje"].ToString(), DezDias = dr["Ultimos 10 dias"].ToString(), TrintaDias = dr["Ultimos 30 dias"].ToString(), SessentaDias = dr["Ultimos 60 dias"].ToString() });
            }

            return projetos;
        }
    }

    public class ProjetosParados
    {
        CultureInfo ptBR = CultureInfo.CreateSpecificCulture("pt-BR");

        private decimal _hoje;
        private decimal _ontem;
        private decimal _dezDias;
        private decimal _trintaDias;
        private decimal _sessentaDias;

        public int Idf_Origem_Importacao { get; set; }
        public string Des_Origem_Importacao { get; set; }
        public string Des_Endereco { get; set; }
        
        public string Hoje {
            get { return String.Format(ptBR, "{0:0,0}", _hoje); }
            set { _hoje = Convert.ToDecimal(value); } 
        }

        public string DezDias
        {
            get { return String.Format(ptBR, "{0:0,0}", _dezDias); }
            set { _dezDias = Convert.ToDecimal(value); }
        }

        public string Ontem
        {
            get { return String.Format(ptBR, "{0:0,0}", _ontem); }
            set { _ontem = Convert.ToDecimal(value); }
        }

        public string TrintaDias
        {
            get { return String.Format(ptBR, "{0:0,0}", _trintaDias); }
            set { _trintaDias = Convert.ToDecimal(value); }
        }

        public string SessentaDias
        {
            get { return String.Format(ptBR, "{0:0,0}", _sessentaDias); }
            set { _sessentaDias = Convert.ToDecimal(value); }
        }
        
        public string Obs { get; set; }
        
        public int Quantidade { get; set; }
        
        public int Total { get; set; }
    }
}
