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

        public static List<CurriculoCurtoDTO> ObterCandidatos(int idf_vaga, decimal num_cnpj, int pagina, out int total_registros, out int total_paginas)
        {

            List<CurriculoCurtoDTO> curriculos = new List<CurriculoCurtoDTO>();
            total_registros = 0;
            total_paginas = 0;
            int inicio_busca = 0;
            int fim_busca = 0;

            Filial filial;
            if (!Filial.CarregarPorCnpj(num_cnpj, out filial)) return curriculos;


            var parmsPagination = new List<SqlParameter>
            {
                new SqlParameter("@TotalPaginas", SqlDbType.Int) {  Value = TOTAL_POR_PAGINA },
                new SqlParameter("@Pagina", SqlDbType.Int) { Value = pagina},
                new SqlParameter("@Idf_Vaga", SqlDbType.Int) { Value = idf_vaga },
                new SqlParameter("@Idf_Filial", SqlDbType.Int) { Value = filial.IdFilial }
            };

            using (IDataReader drPagination = DataAccessLayer.ExecuteReader(CommandType.Text, QRY_SELECT_PAGINATION, parmsPagination))
            {
                if (drPagination.Read())
                {
                    total_registros =  drPagination.GetInt32(0);
                    total_paginas = drPagination.GetInt32(1);
                    inicio_busca = drPagination.GetInt32(2); 
                    fim_busca = drPagination.GetInt32(3);

                    if (pagina <= total_paginas)
                    {
                        var parms = new List<SqlParameter>
                        {
                            new SqlParameter("@Idf_Vaga", SqlDbType.Int) {  Value = idf_vaga },
                            new SqlParameter("@Inicio_Busca", SqlDbType.Int) { Value = inicio_busca},
                            new SqlParameter("@Fim_Busca", SqlDbType.Int) { Value = fim_busca},
                            new SqlParameter("@Idf_Filial", SqlDbType.Int) { Value = filial.IdFilial}
                        };

                        using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, QRY_SELECT_CANDIDATOS, parms))
                        {
                            while(dr.Read())
                            {
                                curriculos.Add(new CurriculoCurtoDTO()
                                {
                                    Bairro = dr.IsDBNull(dr.GetOrdinal("Des_Bairro")) ? "" : dr.GetString(dr.GetOrdinal("Des_Bairro")),
                                    Carteira = dr.IsDBNull(dr.GetOrdinal("Des_Categoria_Habilitacao")) ? "" : dr.GetString(dr.GetOrdinal("Des_Categoria_Habilitacao")),
                                    Cidade = dr.IsDBNull(dr.GetOrdinal("Nme_Cidade")) ? "" : dr.GetString(dr.GetOrdinal("Nme_Cidade")),
                                    Escolaridade = dr.IsDBNull(dr.GetOrdinal("Des_Abreviada")) ? "" : dr.GetString(dr.GetOrdinal("Des_Abreviada")),
                                    Estado = dr.IsDBNull(dr.GetOrdinal("Sig_Estado")) ? "" : dr.GetString(dr.GetOrdinal("Sig_Estado")),
                                    EstadoCivil = dr.IsDBNull(dr.GetOrdinal("Des_Estado_Civil")) ? "" : dr.GetString(dr.GetOrdinal("Des_Estado_Civil")),
                                    Experiencia = dr.IsDBNull(dr.GetOrdinal("Qtd_Experiencia")) ? "0 m" : dr.GetInt32(dr.GetOrdinal("Qtd_Experiencia")).ToString() + " m",
                                    Funcoes = dr.IsDBNull(dr.GetOrdinal("Des_Funcao")) ? "" : dr.GetString(dr.GetOrdinal("Des_Funcao")),
                                    Idade = dr.IsDBNull(dr.GetOrdinal("Num_Idade")) ? 0 : dr.GetInt32(dr.GetOrdinal("Num_Idade")),
                                    IDCurriculo = dr.IsDBNull(dr.GetOrdinal("Idf_Curriculo")) ? 0 : dr.GetInt32(dr.GetOrdinal("Idf_Curriculo")),
                                    Nome = dr.IsDBNull(dr.GetOrdinal("Nme_Pessoa")) ? "" : dr.GetString(dr.GetOrdinal("Nme_Pessoa")),
                                    Pretensao = dr.IsDBNull(dr.GetOrdinal("Vlr_Pretensao_Salarial")) ? 0m : dr.GetDecimal(dr.GetOrdinal("Vlr_Pretensao_Salarial")),
                                    Sexo = dr.IsDBNull(dr.GetOrdinal("Sig_Sexo")) ? "" : dr.GetString(dr.GetOrdinal("Sig_Sexo")),
                                    Vip = dr["Flg_Vip"] == DBNull.Value ? false : Convert.ToBoolean(dr["Flg_Vip"])
                                });
                            }
                        }
                    }
                }
            }
            return curriculos; 
        }
    }
}