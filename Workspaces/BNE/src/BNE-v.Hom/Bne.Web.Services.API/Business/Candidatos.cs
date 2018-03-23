using Bne.Web.Services.API.DTO;
using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.Business
{
    public static class Candidatos
    {

        private const int TOTAL_POR_PAGINA = 50;

        #region QRY_SELECT_PAGINATION
        private const string QRY_SELECT_PAGINATION = @"  
        SELECT  COUNT(vc.Idf_Curriculo),CONVERT(INT, CEILING(CAST(COUNT(vc.Idf_Curriculo) AS FLOAT) / @TotalPaginas))  AS TotalPages,
        ((@Pagina - 1) * @TotalPaginas) + 1 AS Inicio_Busca,
         (@Pagina * @TotalPaginas) + 1 AS Fim_Busca
	     FROM BNE.BNE_Vaga_Candidato vc 
        INNER JOIN BNE.BNE_Vaga vg WITH (NOLOCK) ON vg.Idf_Vaga = vc.Idf_Vaga
	    WHERE vc.Flg_Inativo = 0 AND vc.Idf_Vaga = @Idf_Vaga AND vg.Idf_Filial = @Idf_Filial";
        #endregion


        #region QRY_SELECT_CANDIDATOS
        private const string QRY_SELECT_CANDIDATOS = @"  
        SELECT * FROM ( SELECT ROW_NUMBER() OVER ( ORDER BY vc.Dta_Cadastro DESC ) AS RowNum,
        vc.Dta_Cadastro AS Dta_Candidatura,
        cu.Flg_VIP AS Flg_VIP,
        vc.Idf_Curriculo, 
        CASE CHARINDEX(' ', pf.Nme_Pessoa, 1)
                WHEN 0 THEN pf.Nme_Pessoa
                ELSE SUBSTRING(pf.Nme_Pessoa, 1, CHARINDEX(' ', pf.Nme_Pessoa, 1) - 1) 
        END AS Nme_Pessoa,
        sx.Sig_Sexo, ec.Des_Estado_Civil,
        CASE 
	        WHEN pf.Dta_Nascimento  IS NULL THEN NULL
            WHEN pf.Dta_Nascimento  IS NOT NULL THEN (DATEDIFF(HOUR, pf.Dta_Nascimento , GETDATE()) / 8766) 
        END AS Num_Idade, es.Des_Abreviada, cu.Vlr_Pretensao_Salarial, en.Des_Bairro, ci.Nme_Cidade,
        ci.Sig_Estado,
            (SELECT fu.Des_Funcao + '; ' AS [text()]
            From BNE.BNE_Funcao_Pretendida fp
	        LEFT JOIN plataforma.TAB_Funcao fu ON fp.Idf_Funcao = fu.Idf_Funcao
            Where fp.Idf_Curriculo = cu.Idf_Curriculo
            For XML PATH (''))  AS Des_Funcao,
        (SELECT SUM(fp.Qtd_Experiencia) FROM  BNE.BNE_Funcao_Pretendida fp Where fp.Idf_Curriculo = cu.Idf_Curriculo)  AS Qtd_Experiencia,
        ch.Des_Categoria_Habilitacao
        FROM BNE.BNE_Vaga_Candidato vc WITH (NOLOCK) INNER JOIN BNE.BNE_Curriculo cu WITH (NOLOCK) ON cu.Idf_Curriculo = vc.Idf_Curriculo
        INNER JOIN BNE.BNE_Vaga vg WITH (NOLOCK) ON vg.Idf_Vaga = vc.Idf_Vaga
        INNER JOIN BNE.TAB_Pessoa_Fisica pf WITH (NOLOCK) ON pf.Idf_Pessoa_Fisica = cu.Idf_Pessoa_Fisica
        LEFT JOIN plataforma.TAB_Sexo sx WITH (NOLOCK) ON pf.Idf_Sexo = sx.Idf_Sexo 
        LEFT JOIN plataforma.TAB_Estado_Civil ec WITH (NOLOCK) ON pf.Idf_Estado_Civil = ec.Idf_Estado_Civil
        LEFT JOIN plataforma.TAB_Escolaridade es WITH (NOLOCK) ON pf.Idf_Escolaridade = es.Idf_Escolaridade
        LEFT JOIN BNE.TAB_Endereco en WITH (NOLOCK) ON pf.Idf_Endereco = en.Idf_Endereco
        INNER JOIN plataforma.TAB_Cidade ci WITH (NOLOCK) ON en.Idf_Cidade = ci.Idf_Cidade
        LEFT JOIN BNE.TAB_Pessoa_Fisica_Complemento co WITH (NOLOCK) ON co.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        LEFT JOIN plataforma.TAB_Categoria_Habilitacao ch WITH (NOLOCK) ON ch.Idf_Categoria_Habilitacao = co.Idf_Categoria_Habilitacao
        WHERE vc.Flg_Inativo = 0 AND vc.Idf_Vaga = @Idf_Vaga AND vg.Idf_Filial = @Idf_Filial) AS CONSULTA WHERE RowNum >= @Inicio_Busca AND RowNum < @Fim_Busca";
        #endregion

        #region QRY_SELECT_CANDIDATOS_POR_DATA
        private const string QRY_SELECT_CANDIDATOS_POR_DATA = @"  
        SELECT	vc.Dta_Cadastro AS Dta_Candidatura,
		cu.Flg_VIP AS Flg_VIP,
        vc.Idf_Curriculo, 
        CASE CHARINDEX(' ', pf.Nme_Pessoa, 1)
                WHEN 0 THEN pf.Nme_Pessoa
                ELSE SUBSTRING(pf.Nme_Pessoa, 1, CHARINDEX(' ', pf.Nme_Pessoa, 1) - 1) 
        END AS Nme_Pessoa,
        sx.Sig_Sexo, ec.Des_Estado_Civil,
        CASE 
	        WHEN pf.Dta_Nascimento  IS NULL THEN NULL
            WHEN pf.Dta_Nascimento  IS NOT NULL THEN (DATEDIFF(HOUR, pf.Dta_Nascimento , GETDATE()) / 8766) 
        END AS Num_Idade, es.Des_Abreviada, cu.Vlr_Pretensao_Salarial, en.Des_Bairro, ci.Nme_Cidade,
        ci.Sig_Estado,
            (SELECT fu.Des_Funcao + '; ' AS [text()]
            From BNE.BNE_Funcao_Pretendida fp
	        LEFT JOIN plataforma.TAB_Funcao fu ON fp.Idf_Funcao = fu.Idf_Funcao
            Where fp.Idf_Curriculo = cu.Idf_Curriculo
            For XML PATH (''))  AS Des_Funcao,
        (SELECT SUM(fp.Qtd_Experiencia) FROM  BNE.BNE_Funcao_Pretendida fp Where fp.Idf_Curriculo = cu.Idf_Curriculo)  AS Qtd_Experiencia,
        ch.Des_Categoria_Habilitacao
        FROM BNE.BNE_Vaga_Candidato vc WITH (NOLOCK) INNER JOIN BNE.BNE_Curriculo cu WITH (NOLOCK) ON cu.Idf_Curriculo = vc.Idf_Curriculo
        INNER JOIN BNE.BNE_Vaga vg WITH (NOLOCK) ON vg.Idf_Vaga = vc.Idf_Vaga
        INNER JOIN BNE.TAB_Pessoa_Fisica pf WITH (NOLOCK) ON pf.Idf_Pessoa_Fisica = cu.Idf_Pessoa_Fisica
        LEFT JOIN plataforma.TAB_Sexo sx WITH (NOLOCK) ON pf.Idf_Sexo = sx.Idf_Sexo 
        LEFT JOIN plataforma.TAB_Estado_Civil ec WITH (NOLOCK) ON pf.Idf_Estado_Civil = ec.Idf_Estado_Civil
        LEFT JOIN plataforma.TAB_Escolaridade es WITH (NOLOCK) ON pf.Idf_Escolaridade = es.Idf_Escolaridade
        LEFT JOIN BNE.TAB_Endereco en WITH (NOLOCK) ON pf.Idf_Endereco = en.Idf_Endereco
        INNER JOIN plataforma.TAB_Cidade ci WITH (NOLOCK) ON en.Idf_Cidade = ci.Idf_Cidade
        LEFT JOIN BNE.TAB_Pessoa_Fisica_Complemento co WITH (NOLOCK) ON co.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
        LEFT JOIN plataforma.TAB_Categoria_Habilitacao ch WITH (NOLOCK) ON ch.Idf_Categoria_Habilitacao = co.Idf_Categoria_Habilitacao
        WHERE vc.Dta_Cadastro > @Dta_Cadastro 
		AND vc.Flg_Inativo = 0 
		AND vc.Idf_Vaga = @Idf_Vaga 
		AND vg.Idf_Filial = @Idf_Filial";
        #endregion

        public static List<DTO.Curriculo> ObterCandidatos(int idf_vaga, UsuarioFilialPerfil objUsuarioFilialPerfil, int pagina, bool curriculoCompleto, out int total_registros, out int total_paginas)
        {

            var curriculos = new List<DTO.Curriculo>();
            total_registros = 0;
            total_paginas = 0;

            if (string.IsNullOrEmpty(objUsuarioFilialPerfil.Filial.CNPJ))
                if (!objUsuarioFilialPerfil.Filial.CompleteObject())
                    return curriculos;

            var parmsPagination = new List<SqlParameter>
            {
                new SqlParameter("@TotalPaginas", SqlDbType.Int) {  Value = TOTAL_POR_PAGINA },
                new SqlParameter("@Pagina", SqlDbType.Int) { Value = pagina},
                new SqlParameter("@Idf_Vaga", SqlDbType.Int) { Value = idf_vaga },
                new SqlParameter("@Idf_Filial", SqlDbType.Int) { Value = objUsuarioFilialPerfil.Filial.IdFilial }
            };

            using (IDataReader drPagination = DataAccessLayer.ExecuteReader(CommandType.Text, QRY_SELECT_PAGINATION, parmsPagination))
            {
                if (!drPagination.Read()) return curriculos;

                total_registros = drPagination.GetInt32(0);
                total_paginas = drPagination.GetInt32(1);
                var inicioBusca = drPagination.GetInt32(2);
                var fimBusca = drPagination.GetInt32(3);

                if (pagina <= total_paginas)
                {
                    var parms = new List<SqlParameter>
                    {
                        new SqlParameter("@Idf_Vaga", SqlDbType.Int) {  Value = idf_vaga },
                        new SqlParameter("@Inicio_Busca", SqlDbType.Int) { Value = inicioBusca},
                        new SqlParameter("@Fim_Busca", SqlDbType.Int) { Value = fimBusca},
                        new SqlParameter("@Idf_Filial", SqlDbType.Int) { Value = objUsuarioFilialPerfil.Filial.IdFilial}
                    };

                    using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, QRY_SELECT_CANDIDATOS, parms))
                    {
                        while (dr.Read())
                        {
                            if (!curriculoCompleto)
                            {
                                curriculos.Add(new DTO.MiniCurriculo(dr));
                            }
                            else
                            {
                                var cv = Curriculos.VerDadosCompleto(dr.GetInt32(dr.GetOrdinal("Idf_Curriculo")),
                                    objUsuarioFilialPerfil.Filial, objUsuarioFilialPerfil);
                                if (dr["Dta_Candidatura"] != null)
                                    cv.DataHoraCandidatura = Convert.ToDateTime(dr["Dta_Candidatura"]);
                                curriculos.Add(cv);
                            }
                        }
                    }
                }
            }
            return curriculos;
        }

        public static List<DTO.Curriculo> ObterCandidatos(int idfVaga, UsuarioFilialPerfil objUsuarioFilialPerfil, DateTime data, bool curriculoCompleto)
        {

            var curriculos = new List<DTO.Curriculo>();

            if (string.IsNullOrEmpty(objUsuarioFilialPerfil.Filial.CNPJ))
                if (!objUsuarioFilialPerfil.Filial.CompleteObject())
                    return curriculos;


            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Vaga", SqlDbType.Int) {  Value = idfVaga },
                new SqlParameter("@Idf_Filial", SqlDbType.Int) { Value = objUsuarioFilialPerfil.Filial.IdFilial},
                new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime) { Value = data }
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, QRY_SELECT_CANDIDATOS_POR_DATA, parms))
            {
                while (dr.Read())
                {
                    if (!curriculoCompleto)
                    {
                        curriculos.Add(new DTO.MiniCurriculo(dr));
                    }
                    else
                    {
                        var cv = Curriculos.VerDadosCompleto(dr.GetInt32(dr.GetOrdinal("Idf_Curriculo")),
                            objUsuarioFilialPerfil.Filial, objUsuarioFilialPerfil);
                        if (dr["Dta_Candidatura"] != null)
                            cv.DataHoraCandidatura = Convert.ToDateTime(dr["Dta_Candidatura"]);
                        curriculos.Add(cv);
                    }
                }
            }
            return curriculos;
        }

    }
}