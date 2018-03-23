//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using BNE.BLL.Common.Sitemap;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.DTO;
using BNE.BLL.Enumeradores;
using BNE.BLL.Integracoes.Facebook;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using BNE.BLL.Common;
using BNE.BLL.Custom.Solr.Buffer;
using BNE.Solr;
using BNE.CEP;
using BNE.BLL.Notificacao;
using BNE.EL;
using BNE.BLL.Mensagem.DTO;
using BNE.Domain.Events.CrossDomainEvents;
using BNE.Domain.Events.Handler;

namespace BNE.BLL
{
    public partial class Curriculo : ICloneable // Tabela: BNE_Curriculo
    {

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas

        #region Spselectporpessoafisica
        private const string Spselectporpessoafisica = @"
		SELECT  *
		FROM    BNE_Curriculo WITH(NOLOCK)
		WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region Spselectorigem
        private const string Spselectorigem = "SELECT Idf_Curriculo_Origem FROM BNE_Curriculo_Origem WITH(NOLOCK) WHERE Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region Spcurriculoenviomensagemsemanal
        private const string Spcurriculoenviomensagemsemanal = @"
		SELECT	C.Idf_Curriculo ,
				PF.Num_DDD_Celular ,
				PF.Num_Celular ,
                PF.Nme_Pessoa
		FROM    BNE.BNE_Curriculo C WITH(NOLOCK)
				INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
				INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				INNER JOIN BNE.TAB_Perfil P WITH(NOLOCK) ON UFP.Idf_Perfil = P.Idf_Perfil AND P.Idf_Tipo_Perfil = 1
				INNER JOIN BNE_Plano_Adquirido PA WITH(NOLOCK) ON UFP.Idf_Usuario_Filial_Perfil = PA.Idf_Usuario_Filial_Perfil  
		WHERE   C.Flg_Vip = 1
				AND PF.Num_Celular IS NOT NULL
				AND PF.Num_DDD_Celular IS NOT NULL
				AND (	
					C.Idf_Situacao_Curriculo <> 6 /* Bloqueado */
					AND
					C.Idf_Situacao_Curriculo <> 7 /* Cancelado */
					AND
					C.Idf_Situacao_Curriculo <> 8 /* Invis�vel */
                    AND
                    C.Idf_Situacao_Curriculo <> 12 /* Exclusão Lógica */
					)
				AND PA.Idf_Plano_Situacao = 1 
				AND CONVERT(VARCHAR, PA.Dta_Inicio_Plano, 112) < CONVERT(VARCHAR, GETDATE() - 7, 112)
		";
        #endregion

        #region Spcurriculoenviomensagemsemanalnaocandidatou
        private const string Spcurriculoenviomensagemsemanalnaocandidatou = @"
		SELECT	C.Idf_Curriculo ,
				PF.Num_DDD_Celular ,
				PF.Num_Celular ,
                PF.Nme_Pessoa
		FROM    BNE.BNE_Curriculo C WITH(NOLOCK)
				INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
				INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				INNER JOIN BNE.TAB_Perfil P WITH(NOLOCK) ON UFP.Idf_Perfil = P.Idf_Perfil AND P.Idf_Tipo_Perfil = 1
				INNER JOIN BNE_Plano_Adquirido PA WITH(NOLOCK) ON UFP.Idf_Usuario_Filial_Perfil = PA.Idf_Usuario_Filial_Perfil  
				OUTER APPLY(SELECT TOP 1 * FROM BNE.BNE_Vaga_Candidato VC WITH(NOLOCK) WHERE VC.Idf_Curriculo = C.Idf_Curriculo) AS VC
		WHERE   C.Flg_Vip = 1
				AND PF.Num_Celular IS NOT NULL
				AND PF.Num_DDD_Celular IS NOT NULL
				AND (	
					C.Idf_Situacao_Curriculo <> 6 /* Bloqueado */
					AND
					C.Idf_Situacao_Curriculo <> 7 /* Cancelado */
					AND
					C.Idf_Situacao_Curriculo <> 8 /* Invis�vel */
                    AND
                    C.Idf_Situacao_Curriculo <> 12 /* Exclusão Lógica */
					)
				AND PA.Idf_Plano_Situacao = 1 
				AND CONVERT(VARCHAR, PA.Dta_Inicio_Plano, 112) < CONVERT(VARCHAR, GETDATE() - 7, 112)
				AND VC.Idf_Vaga_Candidato IS NULL
		";
        #endregion

        #region Spcurriculominicurriculoescolaridade
        private const string Spcurriculominicurriculoescolaridade = @"
		SELECT  TOP 1 Idf_Formacao ,
                Idf_Grau_Escolaridade ,
                CASE Esc.Idf_Grau_Escolaridade
                  WHEN 3 --Superior 
                       THEN 'Graduação:'
                  WHEN 4 --Pós-Gradua��o
                       THEN 'Pós-Graduação:'
                END AS Des_Grau_Escolaridade ,
                Des_BNE ,
                ISNULL(Cur.Des_Curso, ISNULL(F.Des_Curso, Des_BNE)) AS Des_Escolaridade ,
                ISNULL(Fon.Sig_Fonte, ISNULL(F.Des_Fonte, '')) AS Des_Escolaridade_Fonte ,
                CONVERT(VARCHAR, ISNULL(', ' + CONVERT(VARCHAR, F.Ano_Conclusao), '')) AS Dta_Conclusao
        FROM    BNE.BNE_Curriculo cv WITH ( NOLOCK )
                INNER JOIN BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK ) ON cv.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
                INNER JOIN BNE.BNE_Formacao f WITH ( NOLOCK ) ON pf.Idf_Pessoa_Fisica = f.Idf_Pessoa_Fisica
		        INNER JOIN plataforma.TAB_Escolaridade Esc WITH ( NOLOCK ) ON f.Idf_Escolaridade = Esc.Idf_Escolaridade
                LEFT  JOIN BNE.TAB_Fonte Fon WITH ( NOLOCK ) ON f.Idf_Fonte = Fon.Idf_Fonte
                LEFT  JOIN BNE.TAB_Curso Cur WITH ( NOLOCK ) ON f.Idf_Curso = Cur.Idf_Curso        
        WHERE   CV.Idf_Curriculo = @Idf_Curriculo
		        AND PF.Idf_Escolaridade = esc.Idf_Escolaridade
     	        AND F.Flg_Inativo = 0
		        AND Idf_Grau_Escolaridade <> 5
        ORDER BY  F.Dta_Alteracao DESC ,
                  F.Dta_Cadastro DESC
				";
        #endregion

        #region Spcurriculominicurriculoexperiencia
        private const string Spcurriculominicurriculoexperiencia = @"  SELECT TOP 5
																	C.Idf_Curriculo ,
																	EPSub.Raz_Social ,
																	Dta_Admissao ,                                                                    
																	Dta_Demissao,
																	ISNULL(Des_Funcao,Des_Funcao_Exercida) AS Des_Funcao ,
																	Des_Atividade,

																CASE 
																	WHEN EPSub.Dta_Demissao IS NULL THEN 1
																	WHEN EPSub.Dta_Demissao IS NOT NULL THEN 2
																END AS 'Ordem'
															 FROM   BNE.BNE_Experiencia_Profissional EPSub WITH(NOLOCK)
																	LEFT JOIN plataforma.TAB_Funcao FSub WITH(NOLOCK) ON EPSub.Idf_Funcao = FSub.Idf_Funcao
																	INNER JOIN BNE.BNE_Curriculo C WITH(NOLOCK) ON EPSub.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
															 WHERE  C.Idf_Curriculo = @Idf_Curriculo
																	AND EPSub.Flg_Inativo = 0
																	 ORDER BY Ordem ASC, EPSub.Dta_Demissao DESC, EPSub.Dta_Admissao DESC";
        #endregion

        #region Spcurriculominicurriculofuncaopretendida
        private const string Spcurriculominicurriculofuncaopretendida = @"  
        SELECT  TOP 3 ISNULL(F.Des_Funcao, FP.Des_Funcao_Pretendida) AS Des_Funcao, Qtd_Experiencia
        FROM    BNE_Funcao_Pretendida FP WITH(NOLOCK) 
                LEFT JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON FP.Idf_Funcao = F.Idf_Funcao
        WHERE   Idf_Curriculo = @Idf_Curriculo
        ORDER BY Idf_Funcao_Pretendida ASC ";
        #endregion

        #region Spselectinformacoescurriculo
        private const string Spselectinformacoescurriculo = @"
			  DECLARE @SQL VARCHAR(MAX)
			  SET @SQL = '
			  SELECT    C.Idf_Curriculo , 
						PF.Num_CPF ,
						PF.Nme_Pessoa ,
						FLOOR(DATEDIFF(MONTH, PF.Dta_Nascimento, GETDATE()) / 12) AS Num_Idade ,
						EC.Des_Estado_Civil ,
						( SELECT TOP 1
									Esc1.Des_BNE
						  FROM      BNE.BNE_Formacao F1 ( NOLOCK )
									JOIN plataforma.TAB_Escolaridade Esc1 ( NOLOCK ) ON Esc1.Idf_Escolaridade = F1.Idf_Escolaridade
						  WHERE     F1.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
									AND Esc1.Idf_Escolaridade <> 18 --Outros cursos
						  ORDER BY  Esc1.Seq_Peso DESC
						) AS Des_Escolaridade ,
						Cid.Nme_Cidade ,
						Cid.Sig_Estado ,
						Ende.Des_Bairro ,
						CH.Des_Categoria_Habilitacao ,
						( SELECT TOP 1
									TV1.Des_Tipo_Veiculo
						  FROM      BNE.TAB_Pessoa_Fisica_Veiculo PFV1 ( NOLOCK )
									JOIN plataforma.TAB_Tipo_Veiculo TV1 ( NOLOCK ) ON PFV1.Idf_Tipo_Veiculo = TV1.Idf_Tipo_Veiculo
						  WHERE     PFV1.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
						  ORDER BY  PFV1.Dta_Cadastro DESC
						) AS Des_Tipo_Veiculo ,
						PF.Flg_Possui_Dependentes ,                        
						case PFC.Flg_Filhos when 1 then ''Sim''
											when 0 then ''Não''
											end as Flg_Filhos,
						D.Des_Deficiencia ,
						C.Vlr_Pretensao_Salarial ,
						C.Vlr_Ultimo_Salario ,
						( SELECT    FU.Des_Funcao + '';''
						  FROM      BNE.BNE_Funcao_Pretendida (NOLOCK) FPSub
									INNER JOIN plataforma.TAB_Funcao (NOLOCK) FU ON FPSub.Idf_Funcao = FU.Idf_Funcao
						  WHERE     FPSub.Idf_Curriculo = C.Idf_Curriculo
						  ORDER BY  FPSub.Dta_Cadastro DESC
						FOR
						  XML PATH('''')
						) AS Des_Funcao ,
						( SELECT    Descricao_Experiencia
						  FROM      ( SELECT    ROW_NUMBER() OVER ( ORDER BY EPSub.Dta_Cadastro DESC, EPSub.Idf_Experiencia_Profissional DESC ) AS Row_ID ,
												EPSub.Dta_Cadastro ,
												EPSub.Raz_Social + ''/'' + ISNULL(Des_Funcao,
																		  Des_Funcao_Exercida)
												+ ''/''
												+ +CASE WHEN CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12) > 0
														THEN +CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12)
															 + '' ano''
															 + CASE WHEN CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12) > 1
																	THEN +''s''
																	ELSE ''''
															   END
														ELSE ''''
												   END
												+ CASE WHEN DATEDIFF(MONTH, Dta_Admissao,
																	 ISNULL(Dta_Demissao,
																		  GETDATE()))
															- ( 12 * ( DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ) > 0
															AND DATEDIFF(MONTH,
																		 Dta_Admissao,
																		 ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 > 0
													   THEN +'' e ''
													   ELSE +''''
												  END
												+ CASE WHEN DATEDIFF(MONTH, Dta_Admissao,
																	 ISNULL(Dta_Demissao,
																		  GETDATE()))
															- ( 12 * ( DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ) > 0
													   THEN +CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE()))
															- ( 12 * ( DATEDIFF(month,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ))
															+ '' mes''
															+ CASE WHEN DATEDIFF(month,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE()))
																		- ( 12
																		  * ( DATEDIFF(month,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ) > 1
																   THEN ''es''
																   ELSE ''''
															  END
													   ELSE ''''
												  END AS Descricao_Experiencia
									  FROM      BNE.BNE_Experiencia_Profissional EPSub
												LEFT JOIN plataforma.TAB_Funcao FSub ON EPSub.Idf_Funcao = FSub.Idf_Funcao
									  WHERE     EPSub.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
												AND EPSub.Flg_Inativo = 0
									) AS temp1
						  WHERE     Row_ID = 1
						) AS Des_Ultima_Experiencia ,
						( SELECT    Descricao_Experiencia
						  FROM      ( SELECT    ROW_NUMBER() OVER ( ORDER BY EPSub.Dta_Cadastro DESC, EPSub.Idf_Experiencia_Profissional DESC ) AS Row_ID ,
												EPSub.Dta_Cadastro ,
												EPSub.Raz_Social + ''/'' + ISNULL(Des_Funcao,
																		  Des_Funcao_Exercida)
												+ ''/''
												+ +CASE WHEN CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12) > 0
														THEN +CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12)
															 + '' ano''
															 + CASE WHEN CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12) > 1
																	THEN +''s''
																	ELSE ''''
															   END
														ELSE ''''
												   END
												+ CASE WHEN DATEDIFF(MONTH, Dta_Admissao,
																	 ISNULL(Dta_Demissao,
																		  GETDATE()))
															- ( 12 * ( DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ) > 0
															AND DATEDIFF(MONTH,
																		 Dta_Admissao,
																		 ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 > 0
													   THEN +'' e ''
													   ELSE +''''
												  END
												+ CASE WHEN DATEDIFF(MONTH, Dta_Admissao,
																	 ISNULL(Dta_Demissao,
																		  GETDATE()))
															- ( 12 * ( DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ) > 0
													   THEN +CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE()))
															- ( 12 * ( DATEDIFF(month,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ))
															+ '' mes''
															+ CASE WHEN DATEDIFF(month,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE()))
																		- ( 12
																		  * ( DATEDIFF(month,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ) > 1
																   THEN ''es''
																   ELSE ''''
															  END
													   ELSE ''''
												  END AS Descricao_Experiencia
									  FROM      BNE.BNE_Experiencia_Profissional EPSub
												LEFT JOIN plataforma.TAB_Funcao FSub ON EPSub.Idf_Funcao = FSub.Idf_Funcao
									  WHERE     EPSub.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
												AND EPSub.Flg_Inativo = 0
									) AS temp1
						  WHERE     Row_ID = 2
						) AS Des_Penultima_Experiencia ,
						( SELECT    Descricao_Experiencia
						  FROM      ( SELECT    ROW_NUMBER() OVER ( ORDER BY EPSub.Dta_Cadastro DESC, EPSub.Idf_Experiencia_Profissional DESC ) AS Row_ID ,
												EPSub.Dta_Cadastro ,
												EPSub.Raz_Social + ''/'' + ISNULL(Des_Funcao,
																		  Des_Funcao_Exercida)
												+ ''/''
												+ +CASE WHEN CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12) > 0
														THEN +CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12)
															 + '' ano''
															 + CASE WHEN CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12) > 1
																	THEN +''s''
																	ELSE ''''
															   END
														ELSE ''''
												   END
												+ CASE WHEN DATEDIFF(MONTH, Dta_Admissao,
																	 ISNULL(Dta_Demissao,
																		  GETDATE()))
															- ( 12 * ( DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ) > 0
															AND DATEDIFF(MONTH,
																		 Dta_Admissao,
																		 ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 > 0
													   THEN +'' e ''
													   ELSE +''''
												  END
												+ CASE WHEN DATEDIFF(MONTH, Dta_Admissao,
																	 ISNULL(Dta_Demissao,
																		  GETDATE()))
															- ( 12 * ( DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ) > 0
													   THEN +CONVERT(NVARCHAR, DATEDIFF(MONTH,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE()))
															- ( 12 * ( DATEDIFF(month,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ))
															+ '' mes''
															+ CASE WHEN DATEDIFF(month,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE()))
																		- ( 12
																		  * ( DATEDIFF(month,
																		  Dta_Admissao,
																		  ISNULL(Dta_Demissao,
																		  GETDATE())) / 12 ) ) > 1
																   THEN ''es''
																   ELSE ''''
															  END
													   ELSE ''''
												  END AS Descricao_Experiencia
									  FROM      BNE.BNE_Experiencia_Profissional EPSub
												LEFT JOIN plataforma.TAB_Funcao FSub ON EPSub.Idf_Funcao = FSub.Idf_Funcao
									  WHERE     EPSub.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
												AND EPSub.Flg_Inativo = 0
									) AS temp1
						  WHERE     Row_ID = 3
						) AS Des_Antepenultima_Experiencia
			  FROM      BNE.BNE_Curriculo (NOLOCK) C
						INNER JOIN bne.TAB_Pessoa_Fisica (NOLOCK) PF ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
						LEFT JOIN plataforma.TAB_Estado_Civil (NOLOCK) EC ON PF.Idf_Estado_Civil = EC.Idf_Estado_Civil
						LEFT JOIN BNE.TAB_Endereco Ende ( NOLOCK ) ON PF.Idf_Endereco = Ende.Idf_Endereco
						LEFT JOIN plataforma.TAB_Cidade Cid ( NOLOCK ) ON Ende.Idf_Cidade = Cid.Idf_Cidade
						LEFT JOIN BNE.TAB_Pessoa_Fisica_Complemento PFC ( NOLOCK ) ON PF.Idf_Pessoa_Fisica = PFC.Idf_Pessoa_Fisica
						LEFT JOIN plataforma.TAB_Categoria_Habilitacao CH ( NOLOCK ) ON PFC.Idf_Categoria_Habilitacao = CH.Idf_Categoria_Habilitacao
						LEFT JOIN plataforma.TAB_Deficiencia D ( NOLOCK ) ON PF.Idf_Deficiencia = D.Idf_Deficiencia
			  WHERE C.Idf_Curriculo IN ( ' + @ids + ')'

				 IF(@Campo_Ordenacao IS NOT NULL AND @Tipo_Ordenacao IS NOT NULL)
					SET @SQL = @SQL + ' ORDER BY ' + @Campo_Ordenacao + ' ' + @Tipo_Ordenacao


			  EXECUTE (@SQL)";
        #endregion

        #region Spcurriculodesatualizados
        private const string Spcurriculodesatualizados = @" 
		SELECT C.Idf_Curriculo, PF.Eml_Pessoa, UFP.Idf_Usuario_Filial_Perfil, PF.Nme_Pessoa, PF.Num_DDD_Celular, PF.Num_Celular,
        PF.Num_cpf, PF.Dta_Nascimento
		FROM BNE.BNE_Curriculo C WITH(NOLOCK)
			INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica 
			INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
			INNER JOIN BNE.TAB_Perfil P WITH(NOLOCK) ON UFP.Idf_Perfil = P.Idf_Perfil
		WHERE
			C.Flg_Inativo = 0
			AND (DATEDIFF(DAY,C.Dta_Atualizacao,GETDATE()) >=33)
			AND PF.Eml_Pessoa IS NOT NULL
			AND P.Idf_Tipo_Perfil = 1 /* Candidato */
			AND (	
				C.Idf_Situacao_Curriculo <> 6 /* Bloqueado */
				AND
				C.Idf_Situacao_Curriculo <> 7 /* Cancelado */
				AND
				C.Idf_Situacao_Curriculo <> 8 /* Invis�vel */
                AND
                C.Idf_Situacao_Curriculo <> 12 /* Exclusão Lógica */
				)";
        #endregion

        #region Spcurriculosforadafaixa
        private const string Spcurriculosforadafaixa = @"
		SELECT  *
		FROM    ( SELECT    F.IDF_FUNCAO ,
							F.DES_FUNCAO ,
							C.IDF_CURRICULO ,
							PF.NME_PESSOA ,
							PF.NUM_CPF ,
							PF.EML_PESSOA ,
							C.VLR_PRETENSAO_SALARIAL ,
							UFP.IDF_USUARIO_FILIAL_PERFIL ,
							CASE WHEN PSEstado.Num_Amostra > 5
								 THEN PSEstado.Vlr_Media * 0.5
								 ELSE PSBrasil.Vlr_Media * 0.5
							END AS Vlr_Minimo ,
							CASE WHEN PSEstado.Num_Amostra > 5
								 THEN PSEstado.Vlr_Media * 2
								 ELSE PSBrasil.Vlr_Media * 2
							END AS Vlr_Maximo
				  FROM      BNE_Curriculo C WITH ( NOLOCK )
							INNER JOIN TAB_Pessoa_Fisica PF WITH ( NOLOCK ) ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
							INNER JOIN TAB_Endereco E WITH ( NOLOCK ) ON E.Idf_Endereco = PF.Idf_Endereco
							INNER JOIN plataforma.TAB_Cidade Cid WITH ( NOLOCK ) ON E.Idf_Cidade = Cid.Idf_Cidade
							INNER JOIN plataforma.TAB_Estado Est WITH ( NOLOCK ) ON Est.Sig_Estado = Cid.Sig_Estado
							CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
							CROSS APPLY ( SELECT TOP 1 * FROM BNE_Curriculo_Origem CO WITH(NOLOCK) WHERE CO.Idf_Curriculo = C.Idf_Curriculo AND CO.Idf_Origem = 1 /* BNE */ ) AS CO
							INNER JOIN plataforma.TAB_FUNCAO F WITH ( NOLOCK ) ON F.IDF_FUNCAO = fp.IDF_FUNCAO
							LEFT JOIN TAB_Pesquisa_Salarial PSEstado WITH(NOLOCK) ON PSEstado.Idf_Funcao = FP.Idf_Funcao AND PSEstado.Idf_Estado = Est.Idf_Estado
							LEFT JOIN TAB_Pesquisa_Salarial PSBrasil WITH(NOLOCK) ON PSBrasil.Idf_Funcao = FP.Idf_Funcao AND PSBrasil.Idf_Estado IS NULL
							LEFT JOIN TAB_USUARIO_FILIAL_PERFIL UFP WITH(NOLOCK) ON UFP.IDF_PESSOA_FISICA = PF.IDF_PESSOA_FISICA AND UFP.FLG_INATIVO = 0 AND UFP.IDF_PERFIL IN ( 2, 3 )
				  WHERE     PF.EML_PESSOA IS NOT NULL
							AND PF.Eml_Pessoa <> ''
							AND C.Idf_Situacao_Curriculo IN ( 1, 2, 3, 4, 9, 10 )
							AND F.Idf_Funcao_Categoria = @Idf_Funcao_Categoria
				) AS temp
		WHERE   Vlr_Pretensao_Salarial < Vlr_Minimo
				OR Vlr_Pretensao_Salarial > Vlr_Maximo
		";

        #endregion

        #region Spselectcurriculosfiltro
        private const string Spselectcurriculosfiltro = @"
		DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )

        SET @iSelect = '
        SELECT	ROW_NUMBER() OVER (ORDER BY CONVERT(VARCHAR, C.Dta_Cadastro) DESC) AS RowID,
		        C.Idf_Curriculo ,
		        PF.Idf_Pessoa_Fisica ,
		        PF.Num_CPF ,
		        PF.Nme_Pessoa ,
                PF.Eml_Pessoa,
                PF.Idf_Email_Situacao_Bloqueio,
		        (
			        CASE WHEN C.Idf_Situacao_Curriculo IS NULL
				        THEN 6 ELSE C.Idf_Situacao_Curriculo 
			        END
		        ) AS Idf_Situacao_Curriculo,
		        CONVERT(VARCHAR, PF.Dta_Nascimento, 103) AS Dta_Nascimento ,
		        ISNULL(CONVERT(VARCHAR,PA.Dta_Fim_Plano,103) , '' - '') AS Validade_Vip,
		        C.Flg_VIP,
		        PA.Idf_Plano_Adquirido,
		        PA.Des_Plano,
                Cid.Nme_Cidade,
                Cid.Sig_Estado,
                F.Des_Funcao
        FROM	BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK)
		        JOIN BNE.BNE_CURRICULO C WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
                LEFT JOIN BNE.TAB_Endereco Ende ( NOLOCK ) ON PF.Idf_Endereco = Ende.Idf_Endereco
				LEFT JOIN plataforma.TAB_Cidade Cid ( NOLOCK ) ON Ende.Idf_Cidade = Cid.Idf_Cidade
		        OUTER APPLY 
		        (	
			        SELECT  top 1 P.Des_Plano, PA.Idf_Plano_Adquirido, PA.Dta_Fim_Plano
			        FROM	BNE.BNE_Plano_Adquirido PA WITH(NOLOCK)
					        INNER JOIN BNE.BNE_Plano P WITH(NOLOCK) ON P.Idf_Plano = PA.Idf_Plano
					        INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil AND UFP.Idf_Filial IS NULL
			        WHERE   PA.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
					        AND	PA.Idf_Plano_Situacao = 1
					        AND UFP.Idf_Pessoa_Fisica= PF.Idf_Pessoa_Fisica
			        ORDER BY Idf_Plano_adquirido DESC
		        ) AS PA
                OUTER APPLY
                (   SELECT    TOP 1 FU.Des_Funcao 
				    FROM      BNE.BNE_Funcao_Pretendida (NOLOCK) FPSub
							  INNER JOIN plataforma.TAB_Funcao (NOLOCK) FU ON FPSub.Idf_Funcao = FU.Idf_Funcao
					WHERE     FPSub.Idf_Curriculo = C.Idf_Curriculo
				    ORDER BY  FPSub.Dta_Cadastro DESC
				) AS F
        WHERE	PF.Flg_Inativo=0'

        IF (@Num_CPF IS NOT NULL)
	        SET @iSelect = @iSelect + ' AND PF.Num_CPF = ' + CONVERT(VARCHAR, @Num_CPF) + ''

        IF (@Eml_Pessoa IS NOT NULL)
	        SET @iSelect = @iSelect + ' AND CONTAINS(PF.Eml_pessoa , ''' + @Eml_Pessoa + ''')'
	        
	    IF (@Nome IS NOT NULL)
	        SET @iSelect = @iSelect + ' AND CONTAINS(PF.Nme_Pessoa, ''' + bne.BNE_BuscaMontaFT(@Nome) + ''')'

        IF (@Idf_Curriculo IS NOT NULL)
	        SET @iSelect = @iSelect + ' AND C.Idf_Curriculo = ' + CONVERT(VARCHAR, @Idf_Curriculo) + ' '

        IF (@Telefone IS NOT NULL)
	        SET @iSelect = @iSelect + ' AND (PF.Num_Celular LIKE ''' + @Telefone + ''' OR PF.Num_Telefone LIKE ''' + @Telefone + ''')'

        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";

        #endregion

        #region Spcurriculoenviomensagemboasvindasstc
        private const string Spcurriculoenviomensagemboasvindasstc = @"
		SELECT  C.Idf_Curriculo ,
				UFP.Idf_Usuario_Filial_Perfil ,
				PF.Eml_Pessoa ,
				OFi.Des_Mensagem_Candidato AS Mensagem,
				'Boas vindas ' + PF.Nme_Pessoa AS Mensagem_Assunto
		FROM    BNE_Curriculo (NOLOCK) C
				INNER JOIN BNE_Curriculo_Origem (NOLOCK) CO ON c.Idf_Curriculo = CO.Idf_Curriculo
				INNER JOIN TAB_Origem_Filial (NOLOCK) OFi ON CO.Idf_Origem = OFi.Idf_Origem
				INNER JOIN TAB_Pessoa_Fisica (NOLOCK) PF ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				INNER JOIN TAB_Usuario_Filial_Perfil (NOLOCK) UFP ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
		WHERE   CONVERT(VARCHAR, C.Dta_Cadastro, 112) = CONVERT(VARCHAR, GETDATE(), 112)
				AND PF.Eml_Pessoa IS NOT NULL";

        #endregion

        #region SprecuperarcurriculossiteMapOLD
        private const string SprecuperarcurriculossiteMapOLD = @"
        /* TODOS OS CV's */
		SELECT  C.Idf_Curriculo ,
				PF.Nme_Pessoa ,
				F.Des_Funcao ,
				CI.Nme_Cidade ,
				CI.Sig_Estado
		FROM    BNE.BNE_Curriculo C WITH (NOLOCK)
				INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH (NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
				INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON F.Idf_Funcao = FP.Idf_Funcao
				INNER JOIN plataforma.TAB_Cidade CI WITH (NOLOCK) ON PF.Idf_Cidade = CI.Idf_Cidade
        /* CV's POR Função */
        UNION
        SELECT  NULL AS Idf_Curriculo ,
				NULL AS Nme_Pessoa ,
				F.Des_Funcao ,
				NULL AS Nme_Cidade ,
				NULL AS Sig_Estado
		FROM    BNE.BNE_Curriculo C WITH (NOLOCK)
				INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH (NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
				INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON F.Idf_Funcao = FP.Idf_Funcao
				INNER JOIN plataforma.TAB_Cidade CI WITH (NOLOCK) ON PF.Idf_Cidade = CI.Idf_Cidade
        GROUP BY F.Des_Funcao
        /* CV's POR CIDADE */
        UNION
        SELECT  NULL AS Idf_Curriculo ,
				NULL AS Nme_Pessoa ,
				NULL AS Des_Funcao ,
				CI.Nme_Cidade ,
				CI.Sig_Estado
		FROM    BNE.BNE_Curriculo C WITH (NOLOCK)
				INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH (NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
				INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON F.Idf_Funcao = FP.Idf_Funcao
				INNER JOIN plataforma.TAB_Cidade CI WITH (NOLOCK) ON PF.Idf_Cidade = CI.Idf_Cidade
        GROUP BY CI.Nme_Cidade , CI.Sig_Estado
        /* CV's POR Função E CIDADE */
        UNION
        SELECT  NULL AS Idf_Curriculo ,
				NULL AS Nme_Pessoa ,
				F.Des_Funcao ,
				CI.Nme_Cidade ,
				CI.Sig_Estado
		FROM    BNE.BNE_Curriculo C WITH (NOLOCK)
				INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH (NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
				INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON F.Idf_Funcao = FP.Idf_Funcao
				INNER JOIN plataforma.TAB_Cidade CI WITH (NOLOCK) ON PF.Idf_Cidade = CI.Idf_Cidade
        GROUP BY F.Des_Funcao , CI.Nme_Cidade , CI.Sig_Estado
        ORDER BY C.Idf_Curriculo ASC, F.Des_Funcao , CI.Nme_Cidade , CI.Sig_Estado
		";
        #endregion

        #region SP_CV_SITEMAP
        private const string SP_CV_SITEMAP = @"
        /* TODOS OS CV's */
		SELECT  C.Idf_Curriculo ,
				PF.Nme_Pessoa ,
				F.Des_Funcao ,
				CI.Nme_Cidade ,
				CI.Sig_Estado
		FROM    BNE.BNE_Curriculo C WITH (NOLOCK)
				INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH (NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
				INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON F.Idf_Funcao = FP.Idf_Funcao
				INNER JOIN plataforma.TAB_Cidade CI WITH (NOLOCK) ON PF.Idf_Cidade = CI.Idf_Cidade";
        #endregion

        #region SP_CV_FUNCAO_SITEMAP
        private const string SP_CV_FUNCAO_SITEMAP = @"
        /* CV's POR Função */
        SELECT  NULL AS Idf_Curriculo ,
				NULL AS Nme_Pessoa ,
				F.Des_Funcao ,
				NULL AS Nme_Cidade ,
				NULL AS Sig_Estado
		FROM    BNE.BNE_Curriculo C WITH (NOLOCK)
				INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH (NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
				INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON F.Idf_Funcao = FP.Idf_Funcao
				INNER JOIN plataforma.TAB_Cidade CI WITH (NOLOCK) ON PF.Idf_Cidade = CI.Idf_Cidade
        GROUP BY F.Des_Funcao";
        #endregion

        #region SP_CV_CIDADE_SITEMAP
        private const string SP_CV_CIDADE_SITEMAP = @"
        /* CV's POR CIDADE */
        SELECT  NULL AS Idf_Curriculo ,
				NULL AS Nme_Pessoa ,
				NULL AS Des_Funcao ,
				CI.Nme_Cidade ,
				CI.Sig_Estado
		FROM    BNE.BNE_Curriculo C WITH (NOLOCK)
				INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH (NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
				INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON F.Idf_Funcao = FP.Idf_Funcao
				INNER JOIN plataforma.TAB_Cidade CI WITH (NOLOCK) ON PF.Idf_Cidade = CI.Idf_Cidade
        GROUP BY CI.Nme_Cidade , CI.Sig_Estado";
        #endregion

        #region SP_CV_FUNCAO_CIDADE_SITEMAP
        private const string SP_CV_FUNCAO_CIDADE_SITEMAP = @"
        /* CV's POR Função E CIDADE */
        SELECT  NULL AS Idf_Curriculo ,
				NULL AS Nme_Pessoa ,
				F.Des_Funcao ,
				CI.Nme_Cidade ,
				CI.Sig_Estado
		FROM    BNE.BNE_Curriculo C WITH (NOLOCK)
				INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH (NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
				CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
				INNER JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON F.Idf_Funcao = FP.Idf_Funcao
				INNER JOIN plataforma.TAB_Cidade CI WITH (NOLOCK) ON PF.Idf_Cidade = CI.Idf_Cidade
        GROUP BY F.Des_Funcao , CI.Nme_Cidade , CI.Sig_Estado
        ORDER BY F.Des_Funcao , CI.Nme_Cidade , CI.Sig_Estado";
        #endregion

        #region Sprecuperarcurriculositemap
        private const string Sprecuperarcurriculositemap = @"
        SELECT  C.Idf_Curriculo ,
                PF.Nme_Pessoa ,
                COALESCE(F.Des_Funcao, FP.Des_Funcao_Pretendida, 'Sem função informada') [Des_Funcao] ,
                CI.Nme_Cidade ,
                CI.Sig_Estado
        FROM    BNE.BNE_Curriculo C WITH ( NOLOCK )
                JOIN BNE.TAB_Pessoa_Fisica PF WITH ( NOLOCK ) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                JOIN BNE.TAB_Endereco E WITH ( NOLOCK ) ON E.Idf_Endereco = PF.Idf_Endereco
                JOIN plataforma.TAB_Cidade CI WITH ( NOLOCK ) ON E.Idf_Cidade = CI.Idf_Cidade
                CROSS APPLY ( SELECT TOP 1
                                        Idf_Funcao ,
                                        Des_Funcao_Pretendida
                              FROM      BNE.BNE_Funcao_Pretendida FP WITH ( NOLOCK )
                              WHERE     C.Idf_Curriculo = FP.Idf_Curriculo
                              ORDER BY  FP.Idf_Funcao_Pretendida
                            ) AS FP
                LEFT JOIN plataforma.TAB_Funcao F WITH ( NOLOCK ) ON F.Idf_Funcao = FP.Idf_Funcao
        WHERE C.Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region SpRetonarCurriculosParaInativar
        private const string SpRetonarCurriculosParaInativar = @"
		SELECT   C.Idf_Curriculo, PF.Eml_Pessoa, UFP.Idf_Usuario_Filial_Perfil, PF.Nme_Pessoa, pf.num_cpf, 
				pf.dta_nascimento
		FROM    BNE.BNE_Curriculo C WITH(NOLOCK)
                INNER JOIN BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                INNER JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
			    INNER JOIN BNE.TAB_Perfil P WITH(NOLOCK) ON UFP.Idf_Perfil = P.Idf_Perfil
		WHERE   DATEDIFF(DAY,C.Dta_Atualizacao,GETDATE()) > @QuantidadeDiasInativar
			    AND P.Idf_Tipo_Perfil = 1 /* Candidato */
			    AND (	
				    C.Idf_Situacao_Curriculo <> 6 /* Bloqueado */
				    AND
				    C.Idf_Situacao_Curriculo <> 7 /* Cancelado */
				    AND
				    C.Idf_Situacao_Curriculo <> 8 /* Invis�vel */
                    AND
                    C.Idf_Situacao_Curriculo <> 12 /* Exclusão Lógica */
				    )";
        #endregion

        #region SpAtualizarSituacaoCurriculo
        private const string SpAtualizarSituacaoCurriculo = "UPDATE BNE.BNE_Curriculo SET Idf_Situacao_Curriculo = @Idf_Situacao_Curriculo WHERE Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region Spselectidfcurriculoporidfpessoafisica
        private const string Spselectidfcurriculoporidfpessoafisica = @"
        SELECT  Idf_Curriculo 
        FROM    TAB_Pessoa_Fisica PF WITH(NOLOCK)
                INNER JOIN BNE_Curriculo C WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        WHERE   PF.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region Spselectvip
        private const string Spselectvip = @"
        SELECT  Flg_VIP 
        FROM    BNE_Curriculo C
        WHERE   C.Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region Spexistecurriculoporpessoafisica
        private const string Spexistecurriculoporpessoafisica = @"
        SELECT  Idf_Curriculo
        FROM    BNE_Curriculo C
        WHERE   C.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
        ";
        #endregion

        #region Spselectsituacaocurriculo
        private const string Spselectsituacaocurriculo = @"
        SELECT  Idf_Situacao_Curriculo 
        FROM    BNE_Curriculo C
        WHERE   C.Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region Spselectdataatualizacaocurriculo
        private const string Spselectdataatualizacaocurriculo = @"
        SELECT  Dta_Atualizacao
        FROM    BNE_Curriculo C
        WHERE   C.Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region SpquantidadesemLocalizacao
        private const string SpquantidadesemLocalizacao = @"
        SELECT  COUNT(*) 
        FROM    BNE_Curriculo C WITH(NOLOCK) 
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                INNER JOIN TAB_Endereco E WITH(NOLOCK) ON PF.Idf_Endereco = E.Idf_Endereco
                INNER JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON Cid.Idf_Cidade = E.Idf_Cidade
        WHERE   C.Des_Localizacao IS NULL
                AND C.Flg_Inativo = 0
        ";
        #endregion

        #region SprecuperarsemLocalizacao
        private const string SprecuperarsemLocalizacao = @"
        SELECT  TOP(@Limite) C.Idf_Curriculo, E.Num_CEP, E.Des_Logradouro, E.Num_Endereco, Cid.Nme_Cidade, Cid.Sig_Estado, C.Dta_Atualizacao
        FROM    BNE_Curriculo C WITH(NOLOCK) 
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                INNER JOIN TAB_Endereco E WITH(NOLOCK) ON PF.Idf_Endereco = E.Idf_Endereco
                INNER JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON Cid.Idf_Cidade = E.Idf_Cidade
        WHERE   C.Des_Localizacao IS NULL
                AND C.Flg_Inativo = 0
                AND (@Dta_Atualizacao IS NULL OR C.Dta_Atualizacao < @Dta_Atualizacao)
        ORDER BY Dta_Atualizacao DESC
        ";
        #endregion

        #region SpAtualizarLocalizacao
        private const string SpAtualizarLocalizacao = "UPDATE BNE_Curriculo SET Des_Localizacao = @Des_Localizacao, Dta_Atualizacao_Geolocalizacao = GETDATE() WHERE Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region SpListaCvsEnvioSMSVaga
        private const string SpListaCvsEnvioSMSVaga = @"
        select top 50 cv.*
        from bne.bne_curriculo cv with(nolock)
	    cross apply(
		    select top 1 fp.Idf_Funcao
		    from bne.BNE_Funcao_Pretendida fp with(nolock)
		    where fp.Idf_Curriculo = cv.Idf_Curriculo
	    ) as funcao_pretendida
	    join bne.TAB_Pessoa_Fisica pf with(nolock) on cv.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
	    cross apply(
		    select top 1 ex.Idf_Funcao
		    from bne.bne_experiencia_profissional ex with(nolock)
		    where ex.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
	    ) as experiencia_profissional
	    join bne.TAB_Endereco en with(nolock) on pf.Idf_Endereco = en.Idf_Endereco
        where en.Idf_Cidade = @Idf_Cidade
	        and ( experiencia_profissional.Idf_Funcao = @Idf_Funcao
	        or funcao_pretendida.Idf_Funcao = @Idf_Funcao )
	        and cv.Idf_Situacao_Curriculo not in (6,7,8)
        order by cv.Dta_Atualizacao desc
        ";
        #endregion

        #region SpListaCvsEnvioEmailVaga
        private const string SpListaCvsEnvioEmailVaga = @"
        select top 100 cv.*
        from bne.bne_curriculo cv with(nolock)
	    cross apply(
		    select top 1 fp.Idf_Funcao
		    from bne.BNE_Funcao_Pretendida fp with(nolock)
		    where fp.Idf_Curriculo = cv.Idf_Curriculo
	    ) as funcao_pretendida
	    join bne.TAB_Pessoa_Fisica pf with(nolock) on cv.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
	    cross apply(
		    select top 1 ex.Idf_Funcao
		    from bne.bne_experiencia_profissional ex with(nolock)
		    where ex.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
	    ) as experiencia_profissional
	    join bne.TAB_Endereco en with(nolock) on pf.Idf_Endereco = en.Idf_Endereco
        where en.Idf_Cidade = @Idf_Cidade
	        and ( experiencia_profissional.Idf_Funcao = @Idf_Funcao
	        or funcao_pretendida.Idf_Funcao = @Idf_Funcao )
	        and cv.Idf_Situacao_Curriculo not in (6,7,8)
        order by cv.Dta_Atualizacao desc
        ";
        #endregion

        #region SpRecuperarHashLogarCurriculo
        private const string SpRecuperarHashLogarCurriculo = @"
        select [BNE].[BNE_BuscaUrlAcesso](@Idf_Curriculo,@pagina) as 'hash'
        ";
        #endregion

        #region spListarBronquinha
        public const string SPListarMotivosBronquinha = @"
                SELECT Idf_Curriculo_Correcao,Des_Correcao,Dta_Cadastro FROM BNE.BNE_Curriculo_Correcao 
                WHERE Idf_Curriculo =@Idf_Curriculo order by Dta_Cadastro DESC";
        #endregion


        #region SP_VerificaCurriculoOrigemSINE
        private const string SP_VerificaCurriculoOrigemSINE = @" SELECT COUNT(*) FROM BNE.BNE_Curriculo_Origem WITH(NOLOCK) WHERE Idf_Curriculo = @Idf_Curriculo and Idf_Origem = @Idf_Origem";
        #endregion

        #endregion

        #region Métodos

        #region CarregarPorPessoaFisica
        /// <summary>
        /// M�todo respons�vel por carregar uma instancia de curr�culo atrav�s do
        /// identificar de uma pessoa f�sica.
        /// </summary>
        /// <param name="idPessoaFisica">Identificador da Pessoa F�sica</param>
        /// <param name="objCurriculo">Inst�ncia de Curr�culo</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPessoaFisica(int idPessoaFisica, out Curriculo objCurriculo)
        {
            return CarregarPorPessoaFisica(null, idPessoaFisica, out objCurriculo);
        }
        /// <summary>
        /// M�todo respons�vel por carregar uma instancia de curr�culo atrav�s do
        /// identificar de uma pessoa f�sica.
        /// </summary>
        /// <param name="trans">Transa��o</param>
        /// <param name="idPessoaFisica">Identificador da Pessoa F�sica</param>
        /// <param name="objCurriculo">Inst�ncia de Curr�culo</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPessoaFisica(SqlTransaction trans, int idPessoaFisica, out Curriculo objCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4)
                };
            parms[0].Value = idPessoaFisica;

            IDataReader dr;

            if (trans == null)
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporpessoafisica, parms);
            else
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectporpessoafisica, parms);

            objCurriculo = new Curriculo();
            if (SetInstance(dr, objCurriculo))
            {
                if (!dr.IsClosed)
                    dr.Close();

                return true;
            }

            if (!dr.IsClosed)
                dr.Close();

            objCurriculo = null;
            return false;
        }
        #endregion

        #region Salvar
        /// <summary>
        /// M�todo respons�vel por salvar um curr�culo na primeira 
        /// tela de cadastro de curr�culo.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        public void Salvar()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // Somente atualiza de a diferença for maior que um dia
                        if ((DateTime.Now - DataAtualizacao).TotalDays >= 1)
                            DataAtualizacao = DateTime.Now; //task 7367

                        //Pessoa F�sica
                        PessoaFisica.Save(trans);

                        //Curriculo
                        Save(trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// M�todo respons�vel por salvar um curr�culo na primeira 
        /// tela de cadastro de curr�culo.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        public void Salvar(CurriculoPublicacao objCurriculoPublicacao, CurriculoAuditoria objCurriculoAuditoria)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (objCurriculoPublicacao != null)
                            objCurriculoPublicacao.Save(trans);

                        //Curriculo
                        Save(trans);

                        if (objCurriculoPublicacao != null)
                        {
                            if (SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.Publicado))
                            {
                                UsuarioFilialPerfil objUsuarioFilialPerfil;
                                if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(PessoaFisica, trans, out objUsuarioFilialPerfil))
                                {
                                    if (!String.IsNullOrEmpty(PessoaFisica.EmailPessoa)) //S� envia mensagem caso o usu�rio possua e-mail
                                    {
                                        string emailRemetenteSistema = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

                                        string assunto;
                                        var template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ConfirmacaoPublicacao, out assunto);
                                        var parametros = new
                                        {
                                            NomeCandidato = PessoaFisica.NomeCompleto,
                                            CodigoCurriculo = IdCurriculo
                                        };
                                        string mensagemEmail = parametros.ToString(template);

                                        EmailSenderFactory
                                            .Create(TipoEnviadorEmail.Fila)
                                            .Enviar(this, null, objUsuarioFilialPerfil, assunto, mensagemEmail, Enumeradores.CartaEmail.ConfirmacaoPublicacao, emailRemetenteSistema, PessoaFisica.EmailPessoa, trans);
                                    }
                                }
                            }
                        }

                        if (objCurriculoAuditoria != null)
                            objCurriculoAuditoria.Save(trans);

                        trans.Commit();

                        //AlertaCurriculos.OnAlterarCurriculo(this);
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region [ Pega Origem ]
        public bool OrigemBNE()
        {
            const string origemSql = @"SELECT TOP 1 1 FROM BNE_Curriculo_Origem WITH(NOLOCK) WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Origem = @Idf_Origem";
            var sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4) { Value = this.IdCurriculo });
            sqlParams.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4) { Value = (int)Enumeradores.Origem.BNE });

            var res = DataAccessLayer.ExecuteScalar(CommandType.Text, origemSql, sqlParams);

            int value;
            if (res == DBNull.Value ||
                res == null ||
                !int.TryParse(res.ToString(), out value) || value != 1)
                return false;

            return true;
        }
        #endregion

        #region SalvarMiniCurriculo

        /// <summary>
        /// M�todo respons�vel por salvar um curr�culo na primeira 
        /// tela de cadastro de curr�culo.
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <param name="listFuncaoPretendida"></param>
        /// <param name="objOrigem"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objPessoaFisicaComplemento"></param>
        /// <param name="situacaoCurriculo"></param>
        /// <param name="objOrigemSecundaria">Fornece uma segunda origem para o currículo, utilizado quando o currículo vem da LANHOUSE, onde ele precisa ganhar Origem da Filial e da LANHOUSE</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public void SalvarMiniCurriculo(PessoaFisica objPessoaFisica, List<FuncaoPretendida> listFuncaoPretendida,
            Origem objOrigem, string cartaBoasVindas, Formacao objFormacao, UsuarioFilialPerfil objUsuarioFilialPerfil,
            PessoaFisicaComplemento objPessoaFisicaComplemento, Enumeradores.SituacaoCurriculo? situacaoCurriculo,
            PessoaFisicaFoto objPessoaFisicaFoto, ProfileFacebook.DadosFacebook dadosFacebook, bool celularValidado = false,
            Origem objOrigemSecundaria = null)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                bool novoCadastro;
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SalvarMiniCurriculo(
                            trans,
                            out novoCadastro,
                            objPessoaFisica,
                            listFuncaoPretendida,
                            objOrigem,
                            cartaBoasVindas,
                            objFormacao,
                            objUsuarioFilialPerfil,
                            objPessoaFisicaComplemento,
                            situacaoCurriculo,
                            objPessoaFisicaFoto,
                            dadosFacebook,
                            celularValidado,
                            objOrigemSecundaria);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                //Task 45613
                if (novoCadastro)
                    this.AdicionarAtividadeAssincronoSalvarAlertasCurriculo(this.PessoaFisica.EmailPessoa, this.IdCurriculo);

            }
        }

        /// <summary>
        /// M�todo respons�vel por salvar um curr�culo na primeira 
        /// tela de cadastro de curr�culo.
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <param name="listFuncaoPretendida"></param>
        /// <param name="objOrigem"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="objPessoaFisicaComplemento"></param>
        /// <param name="situacaoCurriculo"></param>
        /// <param name="objOrigemSecundaria">Fornece uma segunda origem para o currículo, utilizado quando o currículo vem da LANHOUSE, onde ele precisa ganhar Origem da Filial e da LANHOUSE</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public void SalvarMiniCurriculo(SqlTransaction trans, out bool novoCadastro, PessoaFisica objPessoaFisica, List<FuncaoPretendida> listFuncaoPretendida,
            Origem objOrigem, string cartaBoasVindas, Formacao objFormacao, UsuarioFilialPerfil objUsuarioFilialPerfil,
            PessoaFisicaComplemento objPessoaFisicaComplemento, Enumeradores.SituacaoCurriculo? situacaoCurriculo,
            PessoaFisicaFoto objPessoaFisicaFoto, ProfileFacebook.DadosFacebook dadosFacebook, bool celularValidado = false,
            Origem objOrigemSecundaria = null)
        {

            novoCadastro = !this._persisted;

            #region Validação Celular
            //Invalida os celulares repetidos da base, setar pessoa física como confirmada
            if (celularValidado)
                objPessoaFisica.FlagCelularConfirmado = true;
            #endregion Validação Celular

            //Pessoa F?sica
            objPessoaFisica.Endereco.Save(trans);
            objPessoaFisica.Save(trans);

            if (objFormacao != null)
            {
                //salvar a formação depois 
                objFormacao.PessoaFisica = objPessoaFisica;
                objFormacao.Save(trans);
                //Validando escolaridade, se não existir a escolaridade escolhida
                if (!Formacao.CarregarPorPessoaFisicaEscolaridade(trans, objPessoaFisica.IdPessoaFisica, objPessoaFisica.Escolaridade.IdEscolaridade, out objFormacao))
                {
                    objFormacao = new Formacao
                    {
                        Escolaridade = objPessoaFisica.Escolaridade,
                        PessoaFisica = objPessoaFisica
                    };
                    objFormacao.Save(trans);
                }
            }
            //Foto
            if (objPessoaFisicaFoto != null)
            {
                if (objPessoaFisicaFoto.ImagemPessoa == null)
                {
                    objPessoaFisicaFoto.ImagemPessoa = new byte[0];
                    objPessoaFisicaFoto.FlagInativo = true;
                }
                else
                    objPessoaFisicaFoto.FlagInativo = false;

                objPessoaFisicaFoto.PessoaFisica = objPessoaFisica;
                objPessoaFisicaFoto.Save(trans);
            }

            //objUsuarioFilialPerfil
            objUsuarioFilialPerfil.FlagInativo = false;
            objUsuarioFilialPerfil.PessoaFisica = objPessoaFisica;
            objUsuarioFilialPerfil.SenhaUsuarioFilialPerfil = objPessoaFisica.DataNascimento.ToString("ddMMyyyy");
            objUsuarioFilialPerfil.Save(trans);

            //Curr�culo
            PessoaFisica = objPessoaFisica;
            var dataAtualizacao = DateTime.Now;
            DataModificacaoCV = dataAtualizacao;
            DataAtualizacao = dataAtualizacao;

            //Complemento
            objPessoaFisicaComplemento.PessoaFisica = objPessoaFisica;
            objPessoaFisicaComplemento.Save(trans);

            FlagFinalSemana = false;

            DefinirSituacaoCurriculo(trans, situacaoCurriculo);
            DefinirTipoCurriculo(trans);
            Save(trans);

            #region Validação Celular
            //Invalida os celulares repetidos da base, limpar pessoas físicas que informaram este celular como contato
            if (celularValidado)
                PessoaFisica.CelularValidado(this, objPessoaFisica.NumeroDDDCelular, objPessoaFisica.NumeroCelular, trans);
            #endregion Validação Celular

            #region Carta de Validação de E-mail
            if (novoCadastro)
            {
                ParametroCurriculo objParametroCurriculo;
                if (!ParametroCurriculo.CarregarParametroPorCurriculo(Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, this, out objParametroCurriculo, trans))
                {
                    // mant�m as candidaturas gr�tis para os clientes do STC sem VIP
                    objParametroCurriculo = new ParametroCurriculo
                    {
                        Curriculo = this,
                        Parametro = new Parametro((int)Enumeradores.Parametro.QuantidadeCandidaturaDegustacao),
                        ValorParametro = Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, trans)
                    };

                    objParametroCurriculo.Save(trans);
                }
                if (!String.IsNullOrEmpty(PessoaFisica.EmailPessoa))
                {
                    string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);
                    string urlSite = string.Concat("http://", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente));

                    //deleta ta tabela pre cadastro caso tiver.
                    PreCadastro.Delete(PessoaFisica.EmailPessoa, (int)OrigemPreCadastro.PoPup);

                    var carta = CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.ValidacaoEmail);

                    string assuntoValidacaoEmail = carta.Assunto;
                    string templateValidacaoEmail = carta.Conteudo;

                    var parametrosValidacaoEmail = new
                    {
                        Link = string.Format("{0}/{1}?codigo={2}", urlSite, Rota.RecuperarURLRota(RouteCollection.ConfirmacaoEmail), this.PessoaFisica.ValidacaoEmailGerarCodigo(trans)),
                        NomeCandidato = this.PessoaFisica.PrimeiroNome,
                        cpf = this.PessoaFisica.CPF.ToString(),
                        dataNascimento = this.PessoaFisica.DataNascimento.ToString("dd/MM/yyyy")
                    };
                    string mensagemValidacaoEmail = parametrosValidacaoEmail.ToString(templateValidacaoEmail);

                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(this, null, objUsuarioFilialPerfil, assuntoValidacaoEmail, mensagemValidacaoEmail, Enumeradores.CartaEmail.ValidacaoEmail, emailRemetente, PessoaFisica.EmailPessoa, trans);
                }
            }
            #endregion

            #region Vincula??o na Origem e Envio da Carta de Boas-Vindas
            if (objOrigem != null) //S? existe origem se n?o ? um usu?rio interno logado.
            {
                //Se n?o existe o curriculo nesta origem, o curr?culo recebe a origem
                if (!CurriculoOrigem.ExisteCurriculoNaOrigem(this, objOrigem, trans))
                {
                    var objCurriculoOrigem = new CurriculoOrigem
                    {
                        Origem = objOrigem,
                        Curriculo = this
                    };
                    objCurriculoOrigem.Save(trans);

                    //Caso o cadastro esteja sendo feito no BNE
                    if (!objOrigem.IdOrigem.Equals((int)Enumeradores.Origem.BNE))
                    { // Se for um STC
                        OrigemFilial objOrigemFilial;
                        if (OrigemFilial.CarregarPorOrigem(objOrigem.IdOrigem, out objOrigemFilial))
                        {
                            objOrigemFilial.Filial.CompleteObject();

                            if (objOrigemFilial.Filial.PossuiSTCUniversitario())
                            {
                                // concede logo o plano VIP para os clientes do STC com VIP
                                PlanoAdquirido.ConcederPlanoPF(this, new Plano(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPUniversitarioGratis))), trans);
                            }

                            //Se o candidato informou o Email
                            if (!String.IsNullOrEmpty(PessoaFisica.EmailPessoa))
                            {
                                string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

                                if (!string.IsNullOrEmpty(cartaBoasVindas)) // Se tem uma carta de boas vindas (utilizado na lan house
                                {
                                    string templateAssuntoBoasVindas;
                                    string templateBoasVindas = CartaEmail.RetornarConteudoEmpresa(Enumeradores.CartaEmail.CadastroCurriculoSTC, (decimal)objOrigemFilial.Filial.NumeroCNPJ, objOrigemFilial.Filial.NomeFantasia, out templateAssuntoBoasVindas);

                                    var parametros = new
                                    {
                                        Mensagem = cartaBoasVindas,
                                        NomeFantasia = objOrigemFilial.Filial.NomeFantasia
                                    };

                                    string assuntoBoasVindas = parametros.ToString(templateAssuntoBoasVindas);
                                    string mensagemBoasVindas = parametros.ToString(templateBoasVindas);

                                    EmailSenderFactory
                                        .Create(TipoEnviadorEmail.Fila)
                                        .Enviar(this, null, objUsuarioFilialPerfil, assuntoBoasVindas, mensagemBoasVindas, Enumeradores.CartaEmail.CadastroCurriculoSTC, emailRemetente, PessoaFisica.EmailPessoa, trans);
                                }
                                else
                                {
                                    if (String.IsNullOrEmpty(objOrigemFilial.DescricaoMensagemCandidato)) //Se não tem mensagem configurada, manda a padr�o
                                    {
                                        string templateAssunto;
                                        string templateBoasVindas;

                                        if (objOrigemFilial.Filial.PossuiSTCUniversitario())
                                            templateBoasVindas = CartaEmail.RetornarConteudoVIPUniversitario(Enumeradores.CartaEmail.CadastroCurriculoVIPUniversitarioPadrao, (decimal)objOrigemFilial.Filial.NumeroCNPJ, objOrigemFilial.Filial.NomeFantasia, out templateAssunto);
                                        else
                                            templateBoasVindas = CartaEmail.RetornarConteudoEmpresa(Enumeradores.CartaEmail.CadastroCurriculoSTCPadrao, (decimal)objOrigemFilial.Filial.NumeroCNPJ, objOrigemFilial.Filial.NomeFantasia, out templateAssunto);

                                        var parametros = new
                                        {
                                            NomeCandidato = PessoaFisica.NomeCompleto,
                                            NomeFantasia = objOrigemFilial.Filial.NomeFantasia,
                                            Genero = PessoaFisica.Sexo.IdSexo.Equals((int)Enumeradores.Sexo.Masculino) ? "o" : "a"
                                        };

                                        string assuntoBoasVindas = parametros.ToString(templateAssunto);
                                        string mensagemBoasVindas = parametros.ToString(templateBoasVindas);

                                        EmailSenderFactory
                                            .Create(TipoEnviadorEmail.Fila)
                                            .Enviar(this, null, objUsuarioFilialPerfil, assuntoBoasVindas, mensagemBoasVindas, Enumeradores.CartaEmail.CadastroCurriculoSTCPadrao, emailRemetente, PessoaFisica.EmailPessoa, trans);
                                    }
                                    else //Manda a mensagem da empresa.
                                    {
                                        string templateAssuntoBoasVindas;
                                        string templateBoasVindas;

                                        if (objOrigemFilial.Filial.PossuiSTCUniversitario())
                                            templateBoasVindas = CartaEmail.RetornarConteudoVIPUniversitario(Enumeradores.CartaEmail.CadastroCurriculoSTC, (decimal)objOrigemFilial.Filial.NumeroCNPJ, objOrigemFilial.Filial.NomeFantasia, out templateAssuntoBoasVindas);
                                        else
                                            templateBoasVindas = CartaEmail.RetornarConteudoEmpresa(Enumeradores.CartaEmail.CadastroCurriculoSTC, (decimal)objOrigemFilial.Filial.NumeroCNPJ, objOrigemFilial.Filial.NomeFantasia, out templateAssuntoBoasVindas);

                                        //Buscar dados usuario master
                                        string nomeUsuarioMaster, emailComercial, enderecoFilial = string.Empty;
                                        objOrigemFilial.Filial.CarregarDadosUsuarioMaster(out nomeUsuarioMaster, out emailComercial, out enderecoFilial);

                                        string mensagemBoasVindas = templateBoasVindas.Replace("{Mensagem}", objOrigemFilial.DescricaoMensagemCandidato);

                                        var parametros = new
                                        {
                                            NomeFantasia = objOrigemFilial.Filial.NomeFantasia,
                                            NomeCandidato = PessoaFisica.NomeCompleto,
                                            EmailFilial = emailComercial,
                                            NomeContatoFilial = nomeUsuarioMaster,
                                            EnderecoFilial = enderecoFilial
                                        };

                                        mensagemBoasVindas = parametros.ToString(mensagemBoasVindas);
                                        string assuntoBoasVindas = parametros.ToString(templateAssuntoBoasVindas);

                                        EmailSenderFactory
                                            .Create(TipoEnviadorEmail.Fila)
                                            .Enviar(this, null, objUsuarioFilialPerfil, assuntoBoasVindas, mensagemBoasVindas, Enumeradores.CartaEmail.CadastroCurriculoSTC, emailRemetente, PessoaFisica.EmailPessoa, trans);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (objOrigemSecundaria != null) //Se existe uma origem secundária
            {
                if (!CurriculoOrigem.ExisteCurriculoNaOrigem(this, objOrigemSecundaria, trans))
                {
                    var objCurriculoOrigem = new CurriculoOrigem
                    {
                        Origem = objOrigemSecundaria,
                        Curriculo = this
                    };
                    objCurriculoOrigem.Save(trans);
                }
            }
            #endregion

            FuncaoPretendida.DeleteFuncaoPretendidaPorCurriculo(IdCurriculo, trans);

            foreach (var objFuncaoPretendida in listFuncaoPretendida)
            {
                objFuncaoPretendida.Curriculo = this;
                objFuncaoPretendida.Save(trans);
            }

            #region Facebook
            //Se foi passado o codigo identificador do Facebook
            if (dadosFacebook != null && !string.IsNullOrWhiteSpace(dadosFacebook.CodigoFacebook))
            {
                PessoaFisicaRedeSocial objPessoaFisicaRedeSocial;
                if (!PessoaFisicaRedeSocial.CarregarPorPessoaFisicaRedeSocial(objPessoaFisica.IdPessoaFisica, (int)Enumeradores.RedeSocial.FaceBook, out objPessoaFisicaRedeSocial))
                {
                    objPessoaFisicaRedeSocial = new PessoaFisicaRedeSocial
                    {
                        PessoaFisica = objPessoaFisica,
                        FlagInativo = false,
                        CodigoIdentificador = objPessoaFisica.EmailPessoa,
                        RedeSocialCS = new RedeSocialCS((int)Enumeradores.RedeSocial.FaceBook)
                    };
                }
                objPessoaFisicaRedeSocial.CodigoInternoRedeSocial = dadosFacebook.CodigoFacebook;
                objPessoaFisicaRedeSocial.Save(trans);
                if (novoCadastro)
                    SalvarDadosFacebook(dadosFacebook, objPessoaFisica, this, trans);
            }
            #endregion


        }

        #endregion

        #region SalvarDadosFacebook
        private void SalvarDadosFacebook(ProfileFacebook.DadosFacebook dadosFacebook, PessoaFisica objPessoaFisica, Curriculo objCurriculo, SqlTransaction trans)
        {
            if (dadosFacebook.IdEstadoCivil.HasValue)
                objPessoaFisica.EstadoCivil = new EstadoCivil(dadosFacebook.IdEstadoCivil.Value);

            #region Experi�ncias
            if (dadosFacebook.work != null)
            {
                foreach (var work in dadosFacebook.work)
                {
                    try
                    {
                        var objExperienciaProfissional = new ExperienciaProfissional
                        {
                            PessoaFisica = objPessoaFisica,
                            RazaoSocial = work.RazaoSocial,
                            DescricaoAtividade = work.DescricaoAtividade,
                            DataAdmissao = work.DataAdmissao,
                            DataDemissao = work.DataDemissao,
                            FlagImportado = true
                        };

                        if (!string.IsNullOrWhiteSpace(work.Funcao))
                        {
                            Funcao objFuncao;
                            if (Funcao.CarregarPorDescricao(work.Funcao, out objFuncao, trans))
                            {
                                objExperienciaProfissional.Funcao = objFuncao;
                                objExperienciaProfissional.DescricaoFuncaoExercida = String.Empty;
                            }
                            else
                            {
                                objExperienciaProfissional.Funcao = null;
                                objExperienciaProfissional.DescricaoFuncaoExercida = work.Funcao;
                            }
                        }

                        objExperienciaProfissional.Save(trans);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, string.Concat("Falha ao importar Experiência Profissional da Pessoa Física ", objPessoaFisica.IdPessoaFisica, " data de Admissão ", work.DataAdmissao));
                    }
                }
            }
            #endregion

            #region Forma��o
            if (dadosFacebook.education != null)
            {
                foreach (var education in dadosFacebook.education)
                {
                    try
                    {
                        var objFormacao = new Formacao()
                        {
                            PessoaFisica = objPessoaFisica,
                            Escolaridade = education.IdEscolaridade.HasValue ? new Escolaridade((int)education.IdEscolaridade) : null,
                        };

                        if (!String.IsNullOrEmpty(education.NomeInstituicao))
                        {
                            Fonte objFonte;
                            if (Fonte.CarregarPorNome(education.NomeInstituicao, out objFonte))
                            {
                                objFormacao.Fonte = objFonte;
                                objFormacao.DescricaoFonte = String.Empty;
                            }
                            else
                            {
                                objFormacao.Fonte = null;
                                objFormacao.DescricaoFonte = education.NomeInstituicao;
                            }
                        }
                        else
                            objFormacao.Fonte = null;

                        if (!String.IsNullOrEmpty(education.NomeCurso))
                        {
                            Curso objCurso;
                            if (Curso.CarregarPorNome(education.NomeCurso, out objCurso))
                            {
                                objFormacao.Curso = objCurso;
                                objFormacao.DescricaoCurso = String.Empty;
                            }
                            else
                            {
                                objFormacao.Curso = null;
                                objFormacao.DescricaoCurso = education.NomeCurso;
                            }
                        }
                        else
                            objFormacao.Curso = null;

                        objFormacao.Save(trans);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, string.Concat("Falha ao importar Experiência Profissional da Pessoa Física", objPessoaFisica.IdPessoaFisica));
                    }
                }
            }
            #endregion

        }
        #endregion

        #region SalvarDadosPessoais
        public void SalvarDadosPessoais(PessoaFisica objPessoaFisica,
            PessoaFisicaComplemento objPessoaFisicaComplemento,
            Contato objContatoTelefone,
            Contato objContatoCelular,
            List<PessoaFisicaRedeSocial> listRedeSocial,
            ExperienciaProfissional objExperienciaProfissional1,
            ExperienciaProfissional objExperienciaProfissional2,
            ExperienciaProfissional objExperienciaProfissional3,
            ExperienciaProfissional objExperienciaProfissional4,
            ExperienciaProfissional objExperienciaProfissional5,
            ExperienciaProfissional objExperienciaProfissional6,
            ExperienciaProfissional objExperienciaProfissional7,
            ExperienciaProfissional objExperienciaProfissional8,
            ExperienciaProfissional objExperienciaProfissional9,
            ExperienciaProfissional objExperienciaProfissional10,
            Enumeradores.SituacaoCurriculo? situacaoCurriculo = null)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SalvarDadosPessoais(
                            trans,
                            objPessoaFisica,
                            objPessoaFisicaComplemento,
                            objContatoTelefone,
                            objContatoCelular,
                            listRedeSocial,
                            objExperienciaProfissional1,
                            objExperienciaProfissional2,
                            objExperienciaProfissional3,
                            objExperienciaProfissional4,
                            objExperienciaProfissional5,
                            objExperienciaProfissional6,
                            objExperienciaProfissional7,
                            objExperienciaProfissional8,
                            objExperienciaProfissional9,
                            objExperienciaProfissional10,
                            situacaoCurriculo);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }

                conn.Close();
            }
        }

        public void SalvarDadosPessoais(
            SqlTransaction trans,
            PessoaFisica objPessoaFisica,
            PessoaFisicaComplemento objPessoaFisicaComplemento,
            Contato objContatoTelefone,
            Contato objContatoCelular,
            List<PessoaFisicaRedeSocial> listRedeSocial,
            ExperienciaProfissional objExperienciaProfissional1,
            ExperienciaProfissional objExperienciaProfissional2,
            ExperienciaProfissional objExperienciaProfissional3,
            ExperienciaProfissional objExperienciaProfissional4,
            ExperienciaProfissional objExperienciaProfissional5,
            ExperienciaProfissional objExperienciaProfissional6,
            ExperienciaProfissional objExperienciaProfissional7,
            ExperienciaProfissional objExperienciaProfissional8,
            ExperienciaProfissional objExperienciaProfissional9,
            ExperienciaProfissional objExperienciaProfissional10,
            Enumeradores.SituacaoCurriculo? situacaoCurriculo = null)
        {

            try
            {
                //Pessoa Física
                objPessoaFisica.Endereco.Save(trans);
                objPessoaFisica.Save(trans);

                //Currículo
                var dataAtualizacao = DateTime.Now;
                DataModificacaoCV = dataAtualizacao;
                DataAtualizacao = dataAtualizacao;
                PessoaFisica = objPessoaFisica;

                //Definindo valores default para evitar erro.
                DefinirSituacaoCurriculo(trans, situacaoCurriculo);
                DefinirTipoCurriculo(trans);

                Save(trans);

                //Complemento
                if (objPessoaFisicaComplemento != null)
                {
                    objPessoaFisicaComplemento.PessoaFisica = objPessoaFisica;
                    objPessoaFisicaComplemento.Save(trans);

                    //Contato
                    if (objContatoTelefone != null)
                    {
                        objContatoTelefone.PessoaFisicaComplemento = objPessoaFisicaComplemento;
                        objContatoTelefone.Save(trans);
                    }
                    if (objContatoCelular != null)
                    {
                        objContatoCelular.PessoaFisicaComplemento = objPessoaFisicaComplemento;
                        objContatoCelular.Save(trans);
                    }
                }

                //Experiencia Profissional
                if (objExperienciaProfissional1 != null)
                {
                    objExperienciaProfissional1.PessoaFisica = objPessoaFisica;
                    objExperienciaProfissional1.Save(trans);
                }
                if (objExperienciaProfissional2 != null)
                {
                    objExperienciaProfissional2.PessoaFisica = objPessoaFisica;
                    objExperienciaProfissional2.Save(trans);
                }
                if (objExperienciaProfissional3 != null)
                {
                    objExperienciaProfissional3.PessoaFisica = objPessoaFisica;
                    objExperienciaProfissional3.Save(trans);
                }
                if (objExperienciaProfissional4 != null)
                {
                    objExperienciaProfissional4.PessoaFisica = objPessoaFisica;
                    objExperienciaProfissional4.Save(trans);
                }
                if (objExperienciaProfissional5 != null)
                {
                    objExperienciaProfissional5.PessoaFisica = objPessoaFisica;
                    objExperienciaProfissional5.Save(trans);
                }
                if (objExperienciaProfissional6 != null)
                {
                    objExperienciaProfissional6.PessoaFisica = objPessoaFisica;
                    objExperienciaProfissional6.Save(trans);
                }
                if (objExperienciaProfissional7 != null)
                {
                    objExperienciaProfissional7.PessoaFisica = objPessoaFisica;
                    objExperienciaProfissional7.Save(trans);
                }
                if (objExperienciaProfissional8 != null)
                {
                    objExperienciaProfissional8.PessoaFisica = objPessoaFisica;
                    objExperienciaProfissional8.Save(trans);
                }
                if (objExperienciaProfissional9 != null)
                {
                    objExperienciaProfissional9.PessoaFisica = objPessoaFisica;
                    objExperienciaProfissional9.Save(trans);
                }
                if (objExperienciaProfissional10 != null)
                {
                    objExperienciaProfissional10.PessoaFisica = objPessoaFisica;
                    objExperienciaProfissional10.Save(trans);
                }

                //AlertaCurriculos.OnAlterarCurriculo(this);
            }
            catch
            {
                throw;
            }

        }

        #endregion

        #region SalvarDadosComplementares
        public void SalvarDadosComplementares(PessoaFisica objPessoaFisica, PessoaFisicaComplemento objPessoaFisicaComplemento, List<CurriculoDisponibilidade> lstCurriculoDisponibilidade, Enumeradores.SituacaoCurriculo? situacaoCurriculo)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Pessoa F�sica
                        objPessoaFisica.Endereco.Save(trans);
                        objPessoaFisica.Save(trans);

                        //Curr�culo
                        var dataAtualizacao = DateTime.Now;
                        DataModificacaoCV = dataAtualizacao;
                        DataAtualizacao = dataAtualizacao;
                        PessoaFisica = objPessoaFisica;

                        DefinirSituacaoCurriculo(trans, situacaoCurriculo);
                        DefinirTipoCurriculo(trans);
                        Save(trans);

                        //Complemento
                        objPessoaFisicaComplemento.PessoaFisica = objPessoaFisica;
                        objPessoaFisicaComplemento.Save(trans);

                        //CurriculoDisponibilidade
                        CurriculoDisponibilidade.DeletePorCurriculo(IdCurriculo, trans);
                        foreach (CurriculoDisponibilidade objCurriculoDisponibilidade in lstCurriculoDisponibilidade)
                        {
                            objCurriculoDisponibilidade.Save(trans);
                        }

                        trans.Commit();

                        //AlertaCurriculos.OnAlterarCurriculo(this);
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region SalvarFormacaoCursos
        public void SalvarFormacaoCursos(Enumeradores.SituacaoCurriculo? situacaoCurriculo = null)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Formacao objFormacao;
                        if (Formacao.CarregarMaiorFormacaoPorPessoaFisica(PessoaFisica, out objFormacao, trans))
                        {
                            PessoaFisica.CompleteObject(trans);
                            if (PessoaFisica.Escolaridade == null ||
                                PessoaFisica.Escolaridade.IdEscolaridade < objFormacao.Escolaridade.IdEscolaridade)
                            {
                                PessoaFisica.Escolaridade = objFormacao.Escolaridade;
                                PessoaFisica.Save(trans);
                            }
                        }

                        //Curr�culo
                        DefinirSituacaoCurriculo(trans, situacaoCurriculo);
                        DefinirTipoCurriculo(trans);
                        var dataAtualizacao = DateTime.Now;
                        DataModificacaoCV = dataAtualizacao;
                        DataAtualizacao = dataAtualizacao;

                        Save(trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region SalvarCurriculoCompleto
        /// <summary>
        /// M�todo respons�vel por salvar um curr�culo completo
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        public void SalvarCurriculoCompleto(PessoaFisica objPessoaFisica, PessoaFisicaComplemento objPessoaFisicaComplemento, List<CurriculoDisponibilidade> lstCurriculoDisponibilidade, Contato objContatoTelefone, Contato objContatoCelular, List<FuncaoPretendida> listFuncaoPretendida, List<int?> listIdFuncaoPretendida, CurriculoOrigem objCurriculoOrigem, UsuarioFilialPerfil objUsuarioFilialPerfil, List<PessoaFisicaRedeSocial> listRedeSocial, PessoaFisicaFoto objPessoaFisicaFoto, Enumeradores.SituacaoCurriculo? situacaoCurriculo = null)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Pessoa F�sica
                        objPessoaFisica.Endereco.Save(trans);
                        objPessoaFisica.Save(trans);

                        //Foto
                        if (objPessoaFisicaFoto.ImagemPessoa != null)
                        {
                            objPessoaFisicaFoto.PessoaFisica = objPessoaFisica;
                            objPessoaFisicaFoto.Save(trans);
                        }

                        //objUsuarioFilialPerfil
                        objUsuarioFilialPerfil.PessoaFisica = objPessoaFisica;
                        objUsuarioFilialPerfil.SenhaUsuarioFilialPerfil = objPessoaFisica.DataNascimento.ToString("ddMMyyyy");
                        objUsuarioFilialPerfil.Save(trans);

                        //Curr�culo
                        PessoaFisica = objPessoaFisica;
                        var dataAtualizacao = DateTime.Now;
                        DataModificacaoCV = dataAtualizacao;
                        DataAtualizacao = dataAtualizacao;

                        //Complemento
                        objPessoaFisicaComplemento.PessoaFisica = objPessoaFisica;
                        objPessoaFisicaComplemento.Save(trans);

                        Save(trans);

                        //objPessoaFisica.DeleteRedesSociais(trans);
                        //foreach (PessoaFisicaRedeSocial objPessoaFisicaRedeSocial in listRedeSocial)
                        //{
                        //    var objPessoaFisicaRedeSocialNew = new PessoaFisicaRedeSocial
                        //    {
                        //        CodigoIdentificador = objPessoaFisicaRedeSocial.CodigoIdentificador,
                        //        PessoaFisica = objPessoaFisicaRedeSocial.PessoaFisica,
                        //        RedeSocialCS = objPessoaFisicaRedeSocial.RedeSocialCS,
                        //        FlagInativo = false
                        //    };
                        //    objPessoaFisicaRedeSocialNew.Save(trans);
                        //}

                        //Origem
                        if (objCurriculoOrigem != null)
                        {
                            objCurriculoOrigem.Curriculo = this;
                            objCurriculoOrigem.Save(trans);
                        }

                        //[Obsolete("Obtado por n�o utiliza��o/disponibiliza��o.")]
                        //CurriculoTipoVinculo.DeletePorCurriculo(IdCurriculo, trans);
                        //foreach (var item in listCurTipoVinculo ?? new List<CurriculoTipoVinculo>())
                        //{
                        //    item.IdCurriculo = IdCurriculo;
                        //    item.Save(trans);
                        //}

                        #region FuncaoPretendida

                        FuncaoPretendida objFuncaoPretendida1 = listFuncaoPretendida[0];
                        FuncaoPretendida objFuncaoPretendida2 = listFuncaoPretendida[1];
                        FuncaoPretendida objFuncaoPretendida3 = listFuncaoPretendida[2];

                        //? preciso deletar todas as fun??es pretendidas existentes do curriculo
                        //para n?o entrar em conflito com a Ak da Tabela BNE_Curriculo
                        //FuncaoPretendida.DeleteFuncaoPretendidaPorCurriculo(this.IdCurriculo, trans);

                        if (objFuncaoPretendida1 != null)
                        {
                            objFuncaoPretendida1.Curriculo = this;
                            objFuncaoPretendida1.Save(trans);
                        }
                        else if (listIdFuncaoPretendida[0].HasValue)
                            FuncaoPretendida.Delete(listIdFuncaoPretendida[0].Value, trans);

                        if (objFuncaoPretendida2 != null)
                        {
                            objFuncaoPretendida2.Curriculo = this;
                            objFuncaoPretendida2.Save(trans);
                        }
                        else if (listIdFuncaoPretendida[1].HasValue)
                            FuncaoPretendida.Delete(listIdFuncaoPretendida[1].Value, trans);

                        if (objFuncaoPretendida3 != null)
                        {
                            objFuncaoPretendida3.Curriculo = this;
                            objFuncaoPretendida3.Save(trans);
                        }
                        else if (listIdFuncaoPretendida[2].HasValue)
                            FuncaoPretendida.Delete(listIdFuncaoPretendida[2].Value, trans);

                        #endregion

                        //CurriculoDisponibilidade
                        CurriculoDisponibilidade.DeletePorCurriculo(IdCurriculo, trans);
                        //Seta tudo pra false pra atualizar
                        FlagManha = FlagTarde = FlagNoite = FlagFinalSemana = false;
                        foreach (CurriculoDisponibilidade objCurriculoDisponibilidade in lstCurriculoDisponibilidade)
                        {
                            objCurriculoDisponibilidade.Save(trans);
                            switch (objCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade)
                            {
                                case 1:
                                    FlagManha = true;
                                    break;
                                case 2:
                                    FlagTarde = true;
                                    break;
                                case 3:
                                    FlagNoite = true;
                                    break;
                                case 4:
                                    FlagFinalSemana = true;
                                    break;
                                case 5:
                                    FlagFinalSemana = true;
                                    break;
                            }
                        }

                        //Contato
                        if (objContatoTelefone != null)
                        {
                            objContatoTelefone.PessoaFisicaComplemento = objPessoaFisicaComplemento;
                            objContatoTelefone.Save(trans);
                        }
                        if (objContatoCelular != null)
                        {
                            objContatoCelular.PessoaFisicaComplemento = objPessoaFisicaComplemento;
                            objContatoCelular.Save(trans);
                        }

                        DefinirSituacaoCurriculo(trans, situacaoCurriculo);
                        DefinirTipoCurriculo(trans);
                        Save(trans);

                        trans.Commit();

                        //AlertaCurriculos.OnAlterarCurriculo(this);

                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region RecuperarListCurriculoOrigem
        /// <summary>
        /// M�todo respons�vel por retornar uma lista com as origens de um curriculo
        /// </summary>
        /// <returns></returns>
        public List<CurriculoOrigem> RecuperarListCurriculoOrigem()
        {
            var listCurriculoOrigem = new List<CurriculoOrigem>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = _idCurriculo }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectorigem, parms))
            {
                while (dr.Read())
                {
                    listCurriculoOrigem.Add(CurriculoOrigem.LoadObject(Convert.ToInt32(dr[0])));
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return listCurriculoOrigem;
        }
        #endregion

        #region CurriculoOrigemSine
        /// <summary>
        /// Verificar se o Curriculo é de origem SINE
        /// </summary>
        /// <returns>verdadeiro caso o curriculo tenha vindo do SINE</returns>
        public bool CurriculoOrigemSine()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = _idCurriculo },
                    new SqlParameter { ParameterName = "@Idf_Origem", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)Enumeradores.Origem.SINE }
                };



            if (Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SP_VerificaCurriculoOrigemSINE, parms)) > 0)
                return true;

            return false;
        }
        #endregion

        #region DefinirTipoCurriculo
        /// <summary>
        /// M�todo utilizado para definir o tipo de curriculo de acordo com os dados informados
        /// </summary>
        private void DefinirTipoCurriculo(SqlTransaction trans)
        {
            TipoCurriculo = new TipoCurriculo((int)Enumeradores.TipoCurriculo.PreCurriculo);

            if (PessoaFisica != null)
            {
                bool curriculoCompleto;
                bool curriculoMini = curriculoCompleto = true;

                //Com os dois parametros abaixo conseguimos ajustar se um curriculo � incompleto ou apenas mini
                const int quantidadeValidacoesCompleto = 6; //Define quantas valida��es s�o feitas para descobrir se um curriculo � completo.
                int quantidadeValidacoesNegadas = 0; //Ajustadno quantas valida��es do completo foram negadas

                if (String.IsNullOrEmpty(PessoaFisica.NumeroCPF))
                    curriculoMini = false;
                if (String.IsNullOrEmpty(PessoaFisica.DataNascimento.ToShortDateString()))
                    curriculoMini = false;
                if (String.IsNullOrEmpty(PessoaFisica.NumeroDDDCelular))
                    curriculoMini = false;
                if (String.IsNullOrEmpty(PessoaFisica.NumeroCelular))
                    curriculoMini = false;

                if (PessoaFisica.Sexo == null)
                    curriculoMini = false;
                if (PessoaFisica.Endereco == null)
                    curriculoMini = false;
                else
                {
                    if (CidadeEndereco == null)
                        curriculoMini = false;

                    if (String.IsNullOrEmpty(PessoaFisica.Endereco.NumeroCEP))
                    {
                        curriculoCompleto = false;
                        quantidadeValidacoesNegadas++;
                    }

                    if (String.IsNullOrEmpty(PessoaFisica.Endereco.DescricaoLogradouro))
                    {
                        curriculoCompleto = false;
                        quantidadeValidacoesNegadas++;
                    }

                    if (String.IsNullOrEmpty(PessoaFisica.Endereco.NumeroEndereco))
                    {
                        curriculoCompleto = false;
                        quantidadeValidacoesNegadas++;
                    }

                    if (String.IsNullOrEmpty(PessoaFisica.Endereco.DescricaoBairro))
                    {
                        curriculoCompleto = false;
                        quantidadeValidacoesNegadas++;
                    }

                    if (PessoaFisica.EstadoCivil == null)
                    {
                        curriculoCompleto = false;
                        quantidadeValidacoesNegadas++;
                    }

                    if (!Formacao.ExisteFormacaoInformada(PessoaFisica.IdPessoaFisica, trans))
                    {
                        curriculoCompleto = false;
                        quantidadeValidacoesNegadas++;
                    }
                }

                if (FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(this, trans).Count.Equals(0))
                    curriculoMini = false;

                if (curriculoMini && curriculoCompleto)
                    TipoCurriculo = new TipoCurriculo((int)Enumeradores.TipoCurriculo.Completo);
                else if (curriculoMini && !curriculoCompleto && !quantidadeValidacoesCompleto.Equals(quantidadeValidacoesNegadas))
                    TipoCurriculo = new TipoCurriculo((int)Enumeradores.TipoCurriculo.Incompleto);
                else if (curriculoMini)
                    TipoCurriculo = new TipoCurriculo((int)Enumeradores.TipoCurriculo.Mini);
                else
                    TipoCurriculo = new TipoCurriculo((int)Enumeradores.TipoCurriculo.PreCurriculo);
            }

        }
        #endregion

        #region DefinirSituacaoCurriculo
        /// <summary>
        /// M�todo utilizado para definir o status de curriculo de acordo com os dados informados
        /// </summary>
        private void DefinirSituacaoCurriculo(SqlTransaction trans, Enumeradores.SituacaoCurriculo? enumSituacao = null)
        {
            if (!enumSituacao.HasValue)
            {

                bool boolComCritica = false;
                var palavrasProibidas = PalavraProibida.ListarPalavrasProibidas();

                #region Valida��o de Pessoa F�sica

                PessoaFisica.CompleteObject(trans);

                #region CPF
                if (PessoaFisica.NumeroCPF.Equals("0") || !Validacao.ValidarCPF(PessoaFisica.NumeroCPF))
                    boolComCritica = true;
                #endregion

                #region Data Nascimento

                int idadeMinima = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdadeMinima));
                int idadeMaxima = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdadeMaxima));
                DateTime dataAtual = DateTime.Now;

                DateTime dataMinima;
                DateTime dataMaxima;

                try
                {
                    dataMinima = Convert.ToDateTime(dataAtual.Day.ToString(CultureInfo.CurrentCulture) + "/" + dataAtual.Month.ToString(CultureInfo.CurrentCulture) + "/" + (dataAtual.Year - idadeMaxima).ToString(CultureInfo.CurrentCulture));
                    dataMaxima = Convert.ToDateTime(dataAtual.Day.ToString(CultureInfo.CurrentCulture) + "/" + dataAtual.Month.ToString(CultureInfo.CurrentCulture) + "/" + (dataAtual.Year - idadeMinima).ToString(CultureInfo.CurrentCulture));
                }
                catch
                {
                    dataMinima = DateTime.MinValue;
                    dataMaxima = DateTime.MaxValue;
                }

                if (PessoaFisica.DataNascimento > DateTime.Today || PessoaFisica.DataNascimento < dataMinima || PessoaFisica.DataNascimento > dataMaxima)
                    boolComCritica = true;

                #endregion

                #region Celular
                if (!String.IsNullOrEmpty(PessoaFisica.NumeroDDDCelular) && (PessoaFisica.NumeroDDDCelular.Equals("0") || Convert.ToInt32(PessoaFisica.NumeroDDDCelular) < 11))
                    boolComCritica = true;

                if (!String.IsNullOrEmpty(PessoaFisica.NumeroCelular) && (PessoaFisica.NumeroCelular.Equals("0") || Convert.ToInt32(PessoaFisica.NumeroCelular.Substring(0, 1)) <= 5 || !PessoaFisica.NumeroCelular.Trim().Length.Equals(8)))
                    boolComCritica = true;

                #endregion

                #region Nome
                foreach (string nome in PessoaFisica.NomePessoa.Split(' '))
                {
                    foreach (string palavra in palavrasProibidas)
                    {
                        if (nome.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            boolComCritica = true;
                    }
                }
                #endregion

                #endregion

                #region Endereco

                if (PessoaFisica.Endereco != null)
                {

                    PessoaFisica.Endereco.CompleteObject(trans);

                    #region Logradouro

                    if (!String.IsNullOrEmpty(PessoaFisica.Endereco.DescricaoLogradouro))
                    {
                        foreach (string logradouro in PessoaFisica.Endereco.DescricaoLogradouro.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (logradouro.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }

                    #endregion

                    #region Complemento

                    if (!String.IsNullOrEmpty(PessoaFisica.Endereco.DescricaoComplemento))
                    {
                        foreach (string complemento in PessoaFisica.Endereco.DescricaoComplemento.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (complemento.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }

                    #endregion

                    #region Bairro

                    if (!String.IsNullOrEmpty(PessoaFisica.Endereco.DescricaoBairro))
                    {
                        foreach (string bairro in PessoaFisica.Endereco.DescricaoBairro.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (bairro.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }

                    #endregion
                }

                #endregion

                #region Curriculo

                #region Observa��o

                if (!String.IsNullOrEmpty(ObservacaoCurriculo))
                {
                    foreach (string observacao in ObservacaoCurriculo.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (observacao.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                boolComCritica = true;
                        }
                    }
                }

                #endregion

                #endregion

                #region Função Pretendida

                List<FuncaoPretendida> listFuncao = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(this, trans);
                foreach (FuncaoPretendida objFuncaoPretendida in listFuncao)
                {
                    if (objFuncaoPretendida.Funcao == null)
                        boolComCritica = true;

                    if (!objFuncaoPretendida.QuantidadeExperiencia.HasValue || (objFuncaoPretendida.QuantidadeExperiencia.HasValue && objFuncaoPretendida.QuantidadeExperiencia <= 0))
                        boolComCritica = true;
                }

                #endregion

                #region Contato

                #region Telefone Recado
                Contato objContatoTelefone;
                if (Contato.CarregarPorPessoaFisicaTipoContato(PessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoFixo, out objContatoTelefone, trans))
                {

                    if (objContatoTelefone.NomeContato != null)
                    {
                        foreach (string nomeContato in objContatoTelefone.NomeContato.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (nomeContato.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }
                }
                #endregion

                #region Celular Recado
                Contato objContatoCelular;
                if (Contato.CarregarPorPessoaFisicaTipoContato(PessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoCelular, out objContatoCelular, trans))
                {

                    if (objContatoCelular.NomeContato != null)
                    {
                        foreach (string nomeContato in objContatoCelular.NomeContato.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (nomeContato.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }
                }
                #endregion

                #endregion

                #region Experi�ncia Profissional

                foreach (int i in PessoaFisica.RecuperarExperienciaProfissional(trans))
                {
                    ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(i, trans);

                    #region Nome Empresa

                    if (!string.IsNullOrEmpty(objExperienciaProfissional.RazaoSocial))
                    {
                        foreach (string nomeEmpresa in objExperienciaProfissional.RazaoSocial.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (nomeEmpresa.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }

                    #endregion

                    #region Função Exercida

                    if (!String.IsNullOrEmpty(objExperienciaProfissional.DescricaoFuncaoExercida))
                    {
                        foreach (string funcao in objExperienciaProfissional.DescricaoFuncaoExercida.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (funcao.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }

                    #endregion

                    #region Atividades Exercida
                    if (!string.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    {
                        foreach (string atividade in objExperienciaProfissional.DescricaoAtividade.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (atividade.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }
                    #endregion

                }

                #endregion

                #region Forma��o
                foreach (Formacao objFormacao in Formacao.ListarFormacaoList(PessoaFisica.IdPessoaFisica, false))
                {

                    #region Fonte
                    if (!String.IsNullOrEmpty(objFormacao.DescricaoFonte))
                    {
                        foreach (string nomeFonte in objFormacao.DescricaoFonte.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (nomeFonte.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }
                    #endregion

                    #region Curso
                    if (!String.IsNullOrEmpty(objFormacao.DescricaoCurso))
                    {
                        foreach (string descricaoCurso in objFormacao.DescricaoCurso.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (descricaoCurso.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }
                    #endregion

                    #region Endereco
                    if (!String.IsNullOrEmpty(objFormacao.DescricaoEndereco))
                    {
                        string[] descricaoEndereco = objFormacao.DescricaoEndereco.Split(' ');

                        foreach (string endereco in descricaoEndereco)
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (endereco.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }
                    #endregion

                }
                #endregion

                #region Forma��o - Cursos Complementares
                foreach (Formacao objFormacao in Formacao.ListarFormacaoList(PessoaFisica.IdPessoaFisica, true))
                {

                    #region Fonte
                    if (!String.IsNullOrEmpty(objFormacao.DescricaoFonte))
                    {
                        foreach (string nome in objFormacao.DescricaoFonte.Split(' '))
                        {
                            foreach (string palavras in palavrasProibidas)
                            {
                                if (nome.Trim().ToLower().Equals(palavras.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }
                    #endregion

                    #region Curso
                    if (!String.IsNullOrEmpty(objFormacao.DescricaoCurso))
                    {
                        foreach (string descricaoCurso in objFormacao.DescricaoCurso.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (descricaoCurso.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }
                    #endregion

                    #region Endereco
                    if (!String.IsNullOrEmpty(objFormacao.DescricaoEndereco))
                    {
                        foreach (string descricaoEndereco in objFormacao.DescricaoEndereco.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (descricaoEndereco.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }
                    #endregion

                }
                #endregion

                #region Ve�culo
                foreach (PessoaFisicaVeiculo objPessoaFisicaVeiculo in PessoaFisicaVeiculo.ListarPessoaFisicaVeiculo(PessoaFisica.IdPessoaFisica))
                {
                    objPessoaFisicaVeiculo.TipoVeiculo.CompleteObject(trans);

                    if (!String.IsNullOrEmpty(objPessoaFisicaVeiculo.DescricaoModelo))
                    {
                        foreach (string modelo in objPessoaFisicaVeiculo.DescricaoModelo.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (modelo.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    boolComCritica = true;
                            }
                        }
                    }
                }
                #endregion

                if (boolComCritica)
                    SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.ComCritica);
                else
                    SituacaoCurriculo = FlagVIP ? new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoRevisaoVIP) : new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoPublicacao);
            }
            else
                SituacaoCurriculo = new SituacaoCurriculo((int)enumSituacao);
        }
        #endregion

        #region RetornarInformacoesCurriculos
        /// <summary>
        /// M�todo que retorna todas as informa��es de um curr�culo
        /// </summary>
        /// <param name="listIdCurriculo"></param>
        /// <param name="campoOrdenacao"> </param>
        /// <param name="tipoOrdenacao"> </param>
        /// <returns></returns>
        public static DataTable RetornarInformacoesCurriculos(List<int> listIdCurriculo, string campoOrdenacao, string tipoOrdenacao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@ids", SqlDbType.VarChar, 1600),
                    new SqlParameter("@Campo_Ordenacao", SqlDbType.VarChar, 800),
                    new SqlParameter("@Tipo_Ordenacao", SqlDbType.VarChar, 800)
                };

            parms[0].Value = String.Join(",", listIdCurriculo.Select(item => item.ToString(CultureInfo.CurrentCulture)).ToArray());

            if (!string.IsNullOrEmpty(campoOrdenacao) && !string.IsNullOrEmpty(tipoOrdenacao))
            {
                parms[1].Value = campoOrdenacao;
                parms[2].Value = tipoOrdenacao;
            }
            else
            {
                parms[1].Value = DBNull.Value;
                parms[2].Value = DBNull.Value;
            }

            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectinformacoescurriculo, parms))
                {
                    dt = new DataTable();
                    dt.Load(dr);
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }
        #endregion

        #region InativarCurriculosDesatualizados
        public static void InativarCurriculosDesatualizados()
        {
            var curriculosParaInativar = ListarCurriculosParaInativar();

            if (curriculosParaInativar.Rows.Count > 0)
            {
                string assunto;
                string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ConteudoEmailCurriculoInativado, out assunto);
                string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailRemetenteAtualizarCV);

                foreach (DataRow dr in curriculosParaInativar.Rows)
                {
                    using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                    {
                        conn.Open();

                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                int idCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
                                string nomePessoa = dr["Nme_Pessoa"].ToString();
                                string emailPessoa = dr["Eml_Pessoa"].ToString();
                                var objUsuarioFilialPerfilDes = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                                var objCurriculo = new Curriculo(idCurriculo);

                                objCurriculo.AlterarSituacao(new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Invisivel), trans);

                                if (!string.IsNullOrEmpty(emailPessoa))
                                {
                                    var url = "http://www.bne.com.br/logar/{0}";
#if DEBUG
                                    url = "http://localhost:2000/logar/{0}";
#endif
                                    url = String.Format(url, Custom.LoginAutomatico.GerarHashAcessoLogin(Convert.ToDecimal(dr["num_cpf"]), Convert.ToDateTime(dr["dta_nascimento"]), "/cadastro-de-curriculo-gratis"));
                                    var parametros = new
                                    {
                                        NomePessoa = nomePessoa,
                                        AcessoLink = url

                                    };
                                    string mensagem = parametros.ToString(template);

                                    EmailSenderFactory
                                        .Create(TipoEnviadorEmail.Fila)
                                        .Enviar(objCurriculo, null, objUsuarioFilialPerfilDes, assunto, mensagem, Enumeradores.CartaEmail.ConteudoEmailCurriculoInativado, emailRemetente, emailPessoa, trans);
                                }

                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                EL.GerenciadorException.GravarExcecao(ex);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region RecuperarCriticas
        public static DataTable RecuperarCriticas(int idCurriculo, string mensagemPadraoCritica)
        {

            #region Objetos
            List<String> palavrasProibidas = PalavraProibida.ListarPalavrasProibidas();
            Curriculo objCurriculo = LoadObject(idCurriculo);
            objCurriculo.PessoaFisica.CompleteObject();
            #endregion

            #region DataTable
            var dt = new DataTable();
            dt.Columns.Add("Des_Campo_Erro");
            dt.Columns.Add("Des_Critica");
            dt.Columns.Add("Des_Erro");
            dt.Columns.Add("Nme_Pagina_Redirect");
            dt.Columns.Add("Nme_Campo_Foco");
            #endregion

            #region CPF
            if (String.IsNullOrEmpty(objCurriculo.PessoaFisica.NumeroCPF) || (!String.IsNullOrEmpty(objCurriculo.PessoaFisica.NumeroCPF) && !Validacao.ValidarCPF(objCurriculo.PessoaFisica.NumeroCPF)))
            {
                DataRow dr = dt.NewRow();
                dr["Des_Campo_Erro"] = "CPF";
                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "CPF do Candidato");
                dr["Des_Erro"] = "CPF não informado ou inconsistente.";
                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoMini.aspx";
                dr["Nme_Campo_Foco"] = "txtCPF";
                dt.Rows.Add(dr);
            }
            #endregion

            #region Data de Nascimento
            if (String.IsNullOrEmpty(objCurriculo.PessoaFisica.DataNascimento.ToString(CultureInfo.CurrentCulture)))
            {
                DataRow dr = dt.NewRow();
                dr["Des_Campo_Erro"] = "Data de Nascimento";
                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Data de Nascimento do Candidato");
                dr["Des_Erro"] = "Data de Nascimento não informada.";
                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoMini.aspx";
                dr["Nme_Campo_Foco"] = "txtDataDeNascimento";
                dt.Rows.Add(dr);
            }
            #endregion

            #region Nome
            if (String.IsNullOrEmpty(objCurriculo.PessoaFisica.NomePessoa))
            {
                DataRow dr = dt.NewRow();
                dr["Des_Campo_Erro"] = "Nome";
                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Nome do Candidato");
                dr["Des_Erro"] = "Nome não informado.";
                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoMini.aspx";
                dr["Nme_Campo_Foco"] = "txtNome";
                dt.Rows.Add(dr);
            }
            else
            {
                foreach (string nome in objCurriculo.PessoaFisica.NomePessoa.Split(' '))
                {
                    foreach (string palavra in palavrasProibidas)
                    {
                        if (nome.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                        {
                            DataRow dr = dt.NewRow();
                            dr["Des_Campo_Erro"] = "Nome";
                            dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Nome do Candidato");
                            dr["Des_Erro"] = String.Format("Palavra '{0}' proibida no Nome do Candidato.", palavra);
                            dr["Nme_Pagina_Redirect"] = "CadastroCurriculoMini.aspx";
                            dr["Nme_Campo_Foco"] = "txtNome";
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }

            #endregion

            #region NomeRecado

            //Contato Celular
            Contato objContato;
            if (Contato.CarregarPorPessoaFisicaTipoContato(objCurriculo.PessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoCelular, out objContato, null))
            {
                if (objContato.NomeContato != null)
                {
                    foreach (string contatoCelular in objContato.NomeContato.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (contatoCelular.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = "Nome Contato Celular";
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Palavra Proibida");
                                dr["Des_Erro"] = string.Format("Palavra '{0}' proibida no Nome Contato Celular.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                                dr["Nme_Campo_Foco"] = "txtCelularRecadoFalarCom";
                                dt.Rows.Add(dr);
                                break;
                            }
                        }
                    }
                }
            }

            //Contato Telefone
            if (Contato.CarregarPorPessoaFisicaTipoContato(objCurriculo.PessoaFisica.IdPessoaFisica, (int)Enumeradores.TipoContato.RecadoFixo, out objContato, null))
            {
                if (objContato.NomeContato != null)
                {
                    foreach (string contatoTelefone in objContato.NomeContato.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (contatoTelefone.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = "Nome Contato Telefone";
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Palavra Proibida");
                                dr["Des_Erro"] = string.Format("Palavra '{0}' proibida no Nome Contato Telefone.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                                dr["Nme_Campo_Foco"] = "txtTelefoneRecadoFalarCom";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
            }

            #endregion

            #region PretensaoSalarial
            List<FuncaoPretendida> lstFuncaoPretendida = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);

            if (lstFuncaoPretendida.Count > 0)
            {
                if (objCurriculo.PessoaFisica.Endereco != null && lstFuncaoPretendida[0].Funcao != null)
                {
                    //TODO: Comentado at� ser inserida a matriz nova de Função
                    /*
                    objCurriculo.PessoaFisica.Endereco.CompleteObject();
                    objCurriculo.PessoaFisica.Endereco.Cidade.CompleteObject();
                    lstFuncaoPretendida[0].Funcao.CompleteObject();

                    var pesquisa = Custom.SalarioBr.PesquisaSalarial.EfetuarPesquisa(lstFuncaoPretendida[0].Funcao, objCurriculo.PessoaFisica.Endereco.Cidade.Estado);

                    if (pesquisa != null)
                    { 
                        decimal valorMaximo = pesquisa.ValorMaster;
                        decimal valorMinimo = pesquisa.ValorTrainee;

                        if (objCurriculo.ValorPretensaoSalarial < valorMinimo || objCurriculo.ValorPretensaoSalarial > valorMaximo)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Des_Campo_Erro"] = "Pretens�o Sal�rial";
                            dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Pretens�o Sal�rial do Candidato");

                            dr["Des_Erro"] = string.Format("Faixa salarial para a Função {0}: R$ {1} at� R$ {2}. A Pretens�o Sal�rial informada � {3}.", lstFuncaoPretendida[0].Funcao.DescricaoFuncao, valorMinimo,
                                valorMaximo, objCurriculo.ValorPretensaoSalarial);

                            dr["Nme_Pagina_Redirect"] = "CadastroCurriculoMini.aspx";
                            dr["Nme_Campo_Foco"] = "txtPretensaoSalarial";
                            dt.Rows.Add(dr);
                        }
                    }
                    */
                }
            }
            #endregion

            #region DDD Telefone Celular
            if (String.IsNullOrEmpty(objCurriculo.PessoaFisica.NumeroDDDCelular))
            {
                DataRow dr = dt.NewRow();
                dr["Des_Campo_Erro"] = "DDD Celular";
                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "DDD do Celular do Candidato");
                dr["Des_Erro"] = "DDD Celular não informado.";
                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                dr["Nme_Campo_Foco"] = "txtTelefoneCelular";
                dt.Rows.Add(dr);
            }
            #endregion

            #region N�mero Telefone Celular
            if (String.IsNullOrEmpty(objCurriculo.PessoaFisica.NumeroCelular))
            {
                DataRow dr = dt.NewRow();
                dr["Des_Campo_Erro"] = "Número Celular";
                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Número do Celular do Candidato");
                dr["Des_Erro"] = "Número Celular não informado.";
                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                dr["Nme_Campo_Foco"] = "txtTelefoneCelular";
                dt.Rows.Add(dr);
            }
            #endregion

            #region Logradouro/Bairro/Cidade/CEP
            if (objCurriculo.PessoaFisica.Endereco != null)
            {
                objCurriculo.PessoaFisica.Endereco.CompleteObject();

                #region Logradouro
                if (!String.IsNullOrEmpty(objCurriculo.PessoaFisica.Endereco.DescricaoLogradouro))
                {
                    foreach (string logradouro in objCurriculo.PessoaFisica.Endereco.DescricaoLogradouro.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (logradouro.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = "Logradouro";
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Logradouro do Endereço do Candidato");
                                dr["Des_Erro"] = String.Format("Palavra '{0}' proibida no Logradouro do Endereço do Candidato.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                                dr["Nme_Campo_Foco"] = "txtLogradouro";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }

                #endregion

                #region Complemento
                if (!String.IsNullOrEmpty(objCurriculo.PessoaFisica.Endereco.DescricaoComplemento))
                {
                    foreach (string logradouro in objCurriculo.PessoaFisica.Endereco.DescricaoComplemento.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (logradouro.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = "Complemento";
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Complemento do Endereço do Candidato");
                                dr["Des_Erro"] = String.Format("Palavra '{0}' proibida no Complemento do Endereço do Candidato.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                                dr["Nme_Campo_Foco"] = "txtComplemento";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }

                #endregion

                #region Bairro
                if (!String.IsNullOrEmpty(objCurriculo.PessoaFisica.Endereco.DescricaoBairro))
                {
                    foreach (string bairro in objCurriculo.PessoaFisica.Endereco.DescricaoBairro.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (bairro.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = "Bairro";
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Bairro do Endereço do Candidato");
                                dr["Des_Erro"] = String.Format("Palavra '{0}' proibida no Bairro do Endereço do Candidato.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                                dr["Nme_Campo_Foco"] = "txtBairro";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }

                #endregion

                #region CEP
                try
                {
                    var objCep = new Cep
                    {
                        sCep = objCurriculo.PessoaFisica.Endereco.NumeroCEP
                    };

                    var qtdeCepEncontrado = Cep.ContaConsulta(objCep);
                    {
                        if (qtdeCepEncontrado == 0)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Des_Campo_Erro"] = "CEP";
                            dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "CEP do Endereço do Candidato");
                            dr["Des_Erro"] = string.Format("CEP {0} fora da tabela.", objCurriculo.PessoaFisica.Endereco.NumeroCEP);
                            dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                            dr["Nme_Campo_Foco"] = "txtCep";
                            dt.Rows.Add(dr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
                #endregion

            }
            #endregion

            #region Observa��o

            if (!String.IsNullOrEmpty(objCurriculo.ObservacaoCurriculo))
            {
                foreach (string observacao in objCurriculo.ObservacaoCurriculo.Split(' '))
                {
                    foreach (string palavra in palavrasProibidas)
                    {
                        if (observacao.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                        {
                            DataRow dr = dt.NewRow();
                            dr["Des_Campo_Erro"] = "Observa��o";
                            dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Observação do Currículo do Candidato");
                            dr["Des_Erro"] = String.Format("Palavra '{0}' proibida na Observação do Currículo do Candidato.", palavra);
                            dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                            dr["Nme_Campo_Foco"] = "txtObservacoes";
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }

            #endregion

            #region Função Pretendida

            List<FuncaoPretendida> listFuncao = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);
            int contadorFuncao = 0;
            foreach (FuncaoPretendida objFuncaoPretendida in listFuncao)
            {
                contadorFuncao++;
                if (objFuncaoPretendida.Funcao == null)
                {
                    DataRow dr = dt.NewRow();
                    dr["Des_Campo_Erro"] = "Função Pretendida";
                    dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Função do Candidato");
                    dr["Des_Erro"] = string.Format("A Função Pretendida {0} não foi encontrada na tabela de funções.", objFuncaoPretendida.DescricaoFuncaoPretendida);
                    dr["Nme_Pagina_Redirect"] = "CadastroCurriculoMini.aspx";
                    dr["Nme_Campo_Foco"] = "txtFuncaoPretendida";
                    dt.Rows.Add(dr);

                    if (!string.IsNullOrEmpty(objFuncaoPretendida.DescricaoFuncaoPretendida))
                    {
                        string[] funcaoPretendida = objFuncaoPretendida.DescricaoFuncaoPretendida.Split(' ');

                        foreach (string t in funcaoPretendida)
                        {
                            foreach (string t1 in palavrasProibidas)
                            {
                                if (t.Trim().ToLower().Equals(t1.Trim().ToLower()))
                                {
                                    dr = dt.NewRow();
                                    dr["Des_Campo_Erro"] = "Complemento";
                                    dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Função Pretendida do Candidato");
                                    dr["Des_Erro"] = String.Format("Palavra '{0}' proibida na função pretendida do Candidato.", t1);
                                    dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                                    dr["Nme_Campo_Foco"] = "txtFuncaoPretendida" + contadorFuncao;
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            List<int> lstExperenciaProfessional = objCurriculo.PessoaFisica.RecuperarExperienciaProfissional(null);

            #region Experi�ncia Profissional
            foreach (int i in lstExperenciaProfessional)
            {

                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(i);

                if (String.IsNullOrEmpty(objExperienciaProfissional.RazaoSocial))
                {
                    #region Nome Empresa não informada

                    DataRow dr = dt.NewRow();
                    dr["Des_Campo_Erro"] = String.Format("Nome da Empresa {0}", i);
                    dr["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Nome da Empresa {0} do Candidato", i));
                    dr["Des_Erro"] = String.Format("Nome da Empresa {0} não informada.", i);
                    dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                    dr["Nme_Campo_Foco"] = "txtEmpresa";
                    dt.Rows.Add(dr);

                    #endregion
                }
                else
                {
                    #region Razao Social
                    foreach (string nome in objExperienciaProfissional.RazaoSocial.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (nome.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = String.Format("Nome da Empresa {0}", i);
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Nome da Empresa {0} do Candidato", i));
                                dr["Des_Erro"] = String.Format("Palavra '{0}' proibida no Nome da Empresa.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                                dr["Nme_Campo_Foco"] = "txtEmpresa";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                    #endregion

                    #region Função Exercida

                    if (objExperienciaProfissional.Funcao == null)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Des_Campo_Erro"] = String.Format("Função Exercida {0}", i);
                        dr["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Função Exercida da Empresa {0} do Candidato", i));
                        dr["Des_Erro"] = "Função válida não informada.";
                        dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                        dr["Nme_Campo_Foco"] = "txtFuncaoExercida";
                        dt.Rows.Add(dr);

                        if (!string.IsNullOrWhiteSpace(objExperienciaProfissional.DescricaoFuncaoExercida))
                        {
                            foreach (string funcao in objExperienciaProfissional.DescricaoFuncaoExercida.Split(' '))
                            {
                                foreach (string palavra in palavrasProibidas)
                                {
                                    if (funcao.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    {
                                        DataRow drFuncao = dt.NewRow();
                                        drFuncao["Des_Campo_Erro"] = String.Format("Função Exercida {0}", i);
                                        drFuncao["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Função Exercida da Empresa {0} do Candidato", i));
                                        drFuncao["Des_Erro"] = String.Format("Palavra '{0}' proibida na Função Exercida", palavra);
                                        drFuncao["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                                        drFuncao["Nme_Campo_Foco"] = "txtFuncaoExercida";
                                        dt.Rows.Add(drFuncao);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Atividades Exercida

                    if (String.IsNullOrEmpty(objExperienciaProfissional.DescricaoAtividade))
                    {
                        DataRow dr = dt.NewRow();
                        dr["Des_Campo_Erro"] = String.Format("Atividade Exercida {0}", i);
                        dr["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Atividade Exercida da Empresa {0} do Candidato", i));
                        dr["Des_Erro"] = "Atividade Exercida não informada.";
                        dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                        dr["Nme_Campo_Foco"] = "txtAtividadeExercida";
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        foreach (string atividade in objExperienciaProfissional.DescricaoAtividade.Split(' '))
                        {
                            foreach (string palavra in palavrasProibidas)
                            {
                                if (atividade.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["Des_Campo_Erro"] = String.Format("Atividade Exercida {0}", i);
                                    dr["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Atividade Exercida da Empresa {0} do Candidato", i));
                                    dr["Des_Erro"] = String.Format("Palavra '{0}' proibida na Atividade Exercida.", palavra);
                                    dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                                    dr["Nme_Campo_Foco"] = "txtAtividadeExercida";
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                    }

                    #endregion

                    #region Data Admissão
                    if (String.IsNullOrEmpty(objExperienciaProfissional.DataAdmissao.ToString(CultureInfo.CurrentCulture)) || (!String.IsNullOrEmpty(objExperienciaProfissional.DataAdmissao.ToString(CultureInfo.CurrentCulture)) && !Validacao.ValidarFormatoData(objExperienciaProfissional.DataAdmissao.ToString(CultureInfo.CurrentCulture))))
                    {
                        DataRow dr = dt.NewRow();
                        dr["Des_Campo_Erro"] = String.Format("Data Admissão {0}", i);
                        dr["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Data Admissão da Empresa {0} do Candidato", i));
                        dr["Des_Erro"] = "Data Admissão não informada ou inconsistente.";
                        dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                        dr["Nme_Campo_Foco"] = "txtDataAdmissao";
                        dt.Rows.Add(dr);
                    }
                    #endregion

                    #region Data Demissão
                    if (!String.IsNullOrEmpty(objExperienciaProfissional.DataDemissao.ToString()) && !Validacao.ValidarFormatoData(objExperienciaProfissional.DataDemissao.ToString()))
                    {
                        DataRow dr = dt.NewRow();
                        dr["Des_Campo_Erro"] = String.Format("Data Demissão {0}", i);
                        dr["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Data Demissão da Empresa {0} do Candidato", i));
                        dr["Des_Erro"] = "Data Demissão inconsistente.";
                        dr["Nme_Pagina_Redirect"] = "CadastroCurriculoDados.aspx";
                        dr["Nme_Campo_Foco"] = "txtDataDemissao";
                        dt.Rows.Add(dr);
                    }
                    #endregion
                }
            }
            #endregion

            #region Forma��o
            foreach (Formacao objFormacao in Formacao.ListarFormacaoList(objCurriculo.PessoaFisica.IdPessoaFisica, false))
            {
                objFormacao.Escolaridade.CompleteObject();

                #region Fonte
                if (!String.IsNullOrEmpty(objFormacao.DescricaoFonte))
                {
                    foreach (string nomeFonte in objFormacao.DescricaoFonte.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (nomeFonte.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = "Nome da Fonte";
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Nome da Fonte");
                                dr["Des_Erro"] = String.Format("Palavra proibida '{0}' no Nome da Fonte.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoFormacao.aspx";
                                dr["Nme_Campo_Foco"] = "txtInstituicao";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }

                #endregion

                #region Curso
                if (!String.IsNullOrEmpty(objFormacao.DescricaoCurso))
                {
                    //se for P�s gradua��o, Mestrado, Doutorado, P�s Doutorado ou Aperfei�oamento não valida
                    if (objFormacao.Escolaridade.IdEscolaridade.Equals((int)Enumeradores.Escolaridade.TecnologoIncompleto) ||
                        objFormacao.Escolaridade.IdEscolaridade.Equals((int)Enumeradores.Escolaridade.TecnologoCompleto) ||
                        objFormacao.Escolaridade.IdEscolaridade.Equals((int)Enumeradores.Escolaridade.SuperiorIncompleto) ||
                        objFormacao.Escolaridade.IdEscolaridade.Equals((int)Enumeradores.Escolaridade.SuperiorCompleto)
                        )
                    {
                        //Verifica se a Descrição do curso est� na tabela de cursos
                        if (!Curso.VerificaCursoPorDescricao(objFormacao.DescricaoCurso))
                        {
                            DataRow dr = dt.NewRow();
                            dr["Des_Campo_Erro"] = "Descrição do Curso";
                            dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Descrição da Fonte");
                            dr["Des_Erro"] = string.Format("Descrição do Curso '{0}' não foi encontrado na Tabela de Cursos.", objFormacao.DescricaoCurso);
                            dr["Nme_Pagina_Redirect"] = "CadastroCurriculoFormacao.aspx";
                            dr["Nme_Campo_Foco"] = "txtTituloCurso";
                            dt.Rows.Add(dr);

                            foreach (string descricaoCurso in objFormacao.DescricaoCurso.Split(' '))
                            {
                                foreach (string palavra in palavrasProibidas)
                                {
                                    if (descricaoCurso.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                                    {
                                        dr = dt.NewRow();
                                        dr["Des_Campo_Erro"] = "Descrição do Curso";
                                        dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Descrição da Fonte");
                                        dr["Des_Erro"] = String.Format("Palavra '{0}' proibida na Descrição da Fonte.", palavra);
                                        dr["Nme_Pagina_Redirect"] = "CadastroCurriculoFormacao.aspx";
                                        dr["Nme_Campo_Foco"] = "txtTituloCurso";
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Endereco
                if (!String.IsNullOrEmpty(objFormacao.DescricaoEndereco))
                {
                    foreach (string descricaoEndereco in objFormacao.DescricaoEndereco.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (descricaoEndereco.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = "Descrição do Endereço";
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Descrição do Endereço");
                                dr["Des_Erro"] = String.Format("Palavra '{0}' proibida na Descrição do Endereço.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoFormacao.aspx";
                                dr["Nme_Campo_Foco"] = "txtCidade";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
                #endregion

            }
            #endregion

            #region Ve�culo
            foreach (PessoaFisicaVeiculo objPessoaFisicaVeiculo in PessoaFisicaVeiculo.ListarPessoaFisicaVeiculo(objCurriculo.PessoaFisica.IdPessoaFisica))
            {
                objPessoaFisicaVeiculo.TipoVeiculo.CompleteObject();

                #region Modelo
                if (String.IsNullOrEmpty(objPessoaFisicaVeiculo.DescricaoModelo))
                {
                    DataRow dr = dt.NewRow();
                    dr["Des_Campo_Erro"] = "Modelo";
                    dr["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Modelo do Veículo {0} do Candidato", objPessoaFisicaVeiculo.TipoVeiculo.DescricaoTipoVeiculo));
                    dr["Des_Erro"] = "Modelo não informado.";
                    dr["Nme_Pagina_Redirect"] = "CadastroCurriculoComplementar.aspx";
                    dr["Nme_Campo_Foco"] = "txtModelo";
                    dt.Rows.Add(dr);
                }
                else
                {
                    foreach (string modelo in objPessoaFisicaVeiculo.DescricaoModelo.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (modelo.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = "Modelo";
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Modelo do Veículo {0} do Candidato", objPessoaFisicaVeiculo.TipoVeiculo.DescricaoTipoVeiculo));
                                dr["Des_Erro"] = String.Format("Palavra '{0}' proibida no Modelo do Veículo do Candidato.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoComplementar.aspx";
                                dr["Nme_Campo_Foco"] = "txtModelo";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
                #endregion

                #region Ano
                if (String.IsNullOrEmpty(objPessoaFisicaVeiculo.AnoVeiculo.ToString(CultureInfo.CurrentCulture)))
                {
                    DataRow dr = dt.NewRow();
                    dr["Des_Campo_Erro"] = "Ano";
                    dr["Des_Critica"] = String.Format(mensagemPadraoCritica, String.Format("Ano do Veículo {0} do Candidato", objPessoaFisicaVeiculo.TipoVeiculo.DescricaoTipoVeiculo));
                    dr["Des_Erro"] = "Ano não informado.";
                    dr["Nme_Pagina_Redirect"] = "CadastroCurriculoComplementar.aspx";
                    dr["Nme_Campo_Foco"] = "txtAnoVeiculo";
                    dt.Rows.Add(dr);
                }
                #endregion

            }
            #endregion

            #region PessoaFisicaComplemento

            PessoaFisicaComplemento objPessoaFisicaComplemento;
            if (PessoaFisicaComplemento.CarregarPorPessoaFisica(objCurriculo.PessoaFisica.IdPessoaFisica, out objPessoaFisicaComplemento))
            {
                #region DescricaoComplementoDeficiencia

                if (!String.IsNullOrEmpty(objPessoaFisicaComplemento.DescricaoComplementoDeficiencia))
                {
                    foreach (string complementoDeficiencia in objPessoaFisicaComplemento.DescricaoComplementoDeficiencia.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (complementoDeficiencia.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = "Complemento Defici�ncia";
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Complemento Deficiência");
                                dr["Des_Erro"] = String.Format("Palavra '{0}' proibida no Complemento Deficiência.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoComplementar.aspx";
                                dr["Nme_Campo_Foco"] = "txtComplementoDeficiencia";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }

                #endregion

                #region DescricaoConhecimento
                if (!String.IsNullOrEmpty(objPessoaFisicaComplemento.DescricaoConhecimento))
                {
                    foreach (string conhecimento in objPessoaFisicaComplemento.DescricaoConhecimento.Split(' '))
                    {
                        foreach (string palavra in palavrasProibidas)
                        {
                            if (conhecimento.Trim().ToLower().Equals(palavra.Trim().ToLower()))
                            {
                                DataRow dr = dt.NewRow();
                                dr["Des_Campo_Erro"] = "Outros Conhecimentos";
                                dr["Des_Critica"] = String.Format(mensagemPadraoCritica, "Outros Conhecimentos");
                                dr["Des_Erro"] = String.Format("Palavra '{0}' proibida no Complemento Deficiência.", palavra);
                                dr["Nme_Pagina_Redirect"] = "CadastroCurriculoComplementar.aspx";
                                dr["Nme_Campo_Foco"] = "txtOutrosConhecimentos";
                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
                #endregion
            }

            #endregion

            return dt;
        }
        #endregion

        #region ListarCurriculosEnvioMensagemSemanal
        /// <summary>
        /// M�todo utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarCurriculosEnvioMensagemSemanal()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spcurriculoenviomensagemsemanal, null).Tables[0];
        }
        #endregion

        #region ListarCurriculosEnvioMensagemSemanalNaoCandidatou
        /// <summary>
        /// M�todo utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarCurriculosEnvioMensagemSemanalNaoCandidatou()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spcurriculoenviomensagemsemanalnaocandidatou, null).Tables[0];
        }
        #endregion

        #region ListarCurriculosEnvioEmailBoasVindasSiteTrabalheConosco
        /// <summary>
        /// M�todo utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarCurriculosEnvioEmailBoasVindasSiteTrabalheConosco()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spcurriculoenviomensagemboasvindasstc, null).Tables[0];
        }
        #endregion

        #region ListarCurriculosDesatualizados
        /// <summary>
        /// M�todo utilizado por retornar um datatable com o Codigo Identificador de um curr�culo e o e-mail para envio de uma mensagem 
        /// para alertar o candidato da necessidade de atualizar o curr�culo
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static IEnumerable<CurriculoDesatualizado> ListarCurriculosDesatualizados()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spcurriculodesatualizados, null))
            {
                while (dr.Read())
                {
                    var objCurriculoDesatualizado = new CurriculoDesatualizado
                    {
                        CPF = Convert.ToDecimal(dr["Num_cpf"]),
                        NomePessoa = dr["Nme_Pessoa"].ToString(),
                        DataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]),
                        IdUsuarioFilialPerfil = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]),
                        Id = Convert.ToInt32(dr["Idf_Curriculo"]),
                        Email = dr["Eml_Pessoa"].ToString()
                    };

                    yield return objCurriculoDesatualizado;
                }
            }
        }
        #endregion

        #region ListarCurriculosParaInativar
        /// <summary>
        /// M�todo utilizado por retornar um datatable com o Codigo Identificador de um curr�culo e o e-mail para envio de uma mensagem 
        /// para alertar o candidato da necessidade de atualizar o curr�culo
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarCurriculosParaInativar()
        {
            int quantidade = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDiasInativarCurriculo));
            var parms = new List<SqlParameter>
                        {
                            new SqlParameter { ParameterName = "@QuantidadeDiasInativar", SqlDbType = SqlDbType.Int, Size = 4, Value = quantidade }
                        };
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SpRetonarCurriculosParaInativar, parms).Tables[0];
        }
        #endregion

        #region LsitarMotivosBronquinha
        /// <summary>
        /// Listar os motivos do bronquinha na tela.
        /// </summary>
        /// <returns></returns>
        public static DataTable LsitarMotivosBronquinha(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                        {
                            new SqlParameter {ParameterName="@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size =4, Value = idCurriculo}
                        };
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPListarMotivosBronquinha, parms).Tables[0];
        }
        #endregion

        #region ListaCurriculoForaDaFaixaSalarial
        /// <summary>
        /// M�todo que retorna todos os curr�culos fora da Faixa Salarial e que não entrar na amostragem da Pesquisa
        /// </summary>
        /// <returns></returns>
        public static DataTable ListaCurriculoForaDaFaixaSalarial(Enumeradores.FuncaoCategoria funcaoCategoria)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Funcao_Categoria", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)funcaoCategoria }
                };

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spcurriculosforadafaixa, parms).Tables[0];
        }
        #endregion

        #region Integra��o WebFoPag

        #region WebFoPag Velha

        #region SalvarIntegracao
        public void SalvarIntegracao(DateTime dtaAlteracao)
        {
            if (!_persisted)
                InsertIntegracao(dtaAlteracao);
            else
                UpdateIntegracao(dtaAlteracao);
        }
        #endregion

        #region InsertIntegracao
        private void InsertIntegracao(DateTime dtaAlteracao)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DefinirSituacaoCurriculo(trans);

                        if (PessoaFisica != null && PessoaFisica.Endereco != null && PessoaFisica.Endereco.Cidade != null)
                            CidadeEndereco = PessoaFisica.Endereco.Cidade;

                        List<SqlParameter> parms = GetParameters();
                        SetParameters(parms);

                        //sincroniza a Data de Altera��o do Registro para evitar trashing de sincronismo.
                        parms.Find(ParametroDataAlteracao).Value = dtaAlteracao;

                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
                        _idCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo"].Value);

                        #region TAB_Usuario_Filial_Perfil
                        //Criar o Usuario Filial Perfil do Candidato
                        UsuarioFilialPerfil objUsuarioFilialPerfil;
                        if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(PessoaFisica, trans, out objUsuarioFilialPerfil))
                        {
                            objUsuarioFilialPerfil = new UsuarioFilialPerfil
                            {
                                PessoaFisica = PessoaFisica,
                                SenhaUsuarioFilialPerfil = PessoaFisica.DataNascimento.ToString("ddMMyyyy"),
                                Perfil = new Perfil((int)Enumeradores.Perfil.AcessoNaoVIP),
                                DescricaoIP = "integracao"
                            };
                        }
                        objUsuarioFilialPerfil.FlagInativo = false;
                        objUsuarioFilialPerfil.Save(trans);
                        #endregion

                        //Atribui a Origem BNE (1) para o candidato
                        var objCurOrigem = new CurriculoOrigem
                        {
                            Curriculo = this,
                            Origem = new Origem((int)Enumeradores.Origem.BNE)
                        };
                        objCurOrigem.Save(trans);

                        cmd.Parameters.Clear();
                        _persisted = true;
                        _modified = false;

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region UpdateIntegracao
        private void UpdateIntegracao(DateTime dtaAlteracao)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (PessoaFisica != null && PessoaFisica.Endereco != null && PessoaFisica.Endereco.Cidade != null)
                            CidadeEndereco = PessoaFisica.Endereco.Cidade;

                        List<SqlParameter> parms = GetParameters();
                        SetParameters(parms);

                        //sincroniza a Data de Altera��o do Registro para evitar trashing de sincronismo.
                        parms.Find(ParametroDataAlteracao).Value = dtaAlteracao;

                        #region TAB_Usuario_Filial_Perfil
                        //Criar o Usuario Filial Perfil do Candidato
                        UsuarioFilialPerfil objUsuarioFilialPerfil;
                        if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(PessoaFisica, trans, out objUsuarioFilialPerfil))
                        {
                            objUsuarioFilialPerfil = new UsuarioFilialPerfil
                            {
                                PessoaFisica = PessoaFisica,
                                SenhaUsuarioFilialPerfil = PessoaFisica.DataNascimento.ToString("ddMMyyyy"),
                                Perfil = new Perfil((int)Enumeradores.Perfil.AcessoNaoVIP),
                                DescricaoIP = "integracao"
                            };
                        }
                        objUsuarioFilialPerfil.FlagInativo = false;
                        objUsuarioFilialPerfil.Save(trans);
                        #endregion

                        var objOrigem = new Origem((int)Enumeradores.Origem.BNE);
                        if (!CurriculoOrigem.ExisteCurriculoNaOrigem(this, objOrigem, trans))
                        {
                            var objCurriculoOrigem = new CurriculoOrigem
                            {
                                Curriculo = this,
                                Origem = objOrigem
                            };
                            objCurriculoOrigem.Save(trans);
                        }

                        DefinirSituacaoCurriculo(trans);

                        if (PessoaFisica != null && PessoaFisica.Endereco != null && PessoaFisica.Endereco.Cidade != null)
                            CidadeEndereco = PessoaFisica.Endereco.Cidade;

                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                        _modified = false;

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region ParametroDataAlteracao
        private static bool ParametroDataAlteracao(SqlParameter parm)
        {
            if (parm.ParameterName == "@Dta_Atualizacao")
                return true;

            return false;
        }
        #endregion

        #endregion

        #region WebFoPag Nova

        #region SalvarIntegracao
        public void SalvarIntegracao(Integracao objIntegracao, PessoaFisicaComplemento objPessoaFisicaComplemento, PessoaFisicaVeiculo objPessoaFisicaVeiculo, ExperienciaProfissional objExperienciaProfissional, bool liberarVIP, bool atualizarCurriculoSolr = true)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        if (PessoaFisica != null && PessoaFisica.Endereco != null && PessoaFisica.Endereco.Cidade != null)
                            CidadeEndereco = PessoaFisica.Endereco.Cidade;

                        if (string.IsNullOrEmpty(PessoaFisica.NumeroCelular))
                            this.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Hibernado);

                        if (PessoaFisica.Endereco.Cidade != null)
                            PessoaFisica.Endereco.Save(trans);
                        else
                            PessoaFisica.Endereco = null;

                        PessoaFisica.Save(trans);
                        Save(trans);

                        FuncaoPretendida objFuncaoPretendida = null;
                        FuncaoPretendida.SalvarFuncaoPretendidaIntegracao(objExperienciaProfissional, this, trans, out objFuncaoPretendida);

                        if (objPessoaFisicaComplemento != null)
                            objPessoaFisicaComplemento.Save(trans);

                        if (objPessoaFisicaVeiculo != null)
                            objPessoaFisicaVeiculo.Save(trans);

                        if (objExperienciaProfissional != null)
                            objExperienciaProfissional.Save(trans);

                        if (objFuncaoPretendida != null)
                            objFuncaoPretendida.Save(trans);

                        #region TAB_Usuario_Filial_Perfil
                        //Criar o Usuario Filial Perfil do Candidato
                        UsuarioFilialPerfil objUsuarioFilialPerfil;
                        if (!UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(PessoaFisica, trans, out objUsuarioFilialPerfil))
                        {
                            objUsuarioFilialPerfil = new UsuarioFilialPerfil
                            {
                                PessoaFisica = PessoaFisica,
                                SenhaUsuarioFilialPerfil = PessoaFisica.DataNascimento.ToString("ddMMyyyy"),
                                Perfil = new Perfil((int)Enumeradores.Perfil.AcessoNaoVIP),
                                DescricaoIP = "Integração WF"
                            };
                        }
                        objUsuarioFilialPerfil.FlagInativo = false;
                        objUsuarioFilialPerfil.Save(trans);
                        #endregion

                        #region Criar uma nova forma��o para a Pessoa F�sica
                        if (this.PessoaFisica.Escolaridade != null)
                        {
                            Formacao objFormacao;
                            if (!Formacao.CarregarPorPessoaFisicaEscolaridade(trans, this.PessoaFisica.IdPessoaFisica, this.PessoaFisica.Escolaridade.IdEscolaridade, out objFormacao))
                            {
                                objFormacao = new Formacao();
                                objFormacao.PessoaFisica = this.PessoaFisica;
                                objFormacao.Escolaridade = this.PessoaFisica.Escolaridade;
                                objFormacao.FlagNacional = true;
                                objFormacao.FlagInativo = false;
                                objFormacao.Save(trans);
                            }
                        }
                        #endregion

                        #region LiberarVIP
                        if (liberarVIP)
                        {
                            var idPlano = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CodigoPlanoVIPWebfopag));
                            if (!PlanoAdquirido.ExistePlanoAdquiridoCandidato(this, Enumeradores.PlanoSituacao.Liberado, trans))
                                PlanoAdquirido.ConcederPlanoPF(this, new Plano(idPlano), trans);
                        }
                        #endregion

                        #region Origem no BNE
                        var objOrigem = new Origem((int)Enumeradores.Origem.BNE);
                        if (!CurriculoOrigem.ExisteCurriculoNaOrigem(this, objOrigem, trans))
                        {
                            var objCurriculoOrigem = new CurriculoOrigem
                            {
                                Curriculo = this,
                                Origem = objOrigem
                            };
                            objCurriculoOrigem.Save(trans);
                        }
                        #endregion

                        #region [Salvar no crm caso for inativado]
                        if (this.FlagInativo && objIntegracao.MotivoRescisao != null)
                        {
                            objIntegracao.MotivoRescisao.CompleteObject();
                            var objCurriculoCorrecao = new CurriculoCorrecao
                            {
                                Curriculo = this,
                                DescricaoCorrecao = $"Curriculo bloqueado por integração Webfopag. Motivo: {objIntegracao.MotivoRescisao.DescricaoMotivoRescisao} Causa: {objIntegracao.MotivoRescisao.SiglaCausaAfastamento}",
                                FlagInativo = false,
                                UsuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdUsuarioFilialPerfilAuditoriaCurriculo))),
                                FlagCorrigido = false
                            };
                            objCurriculoCorrecao.Save(trans);
                        }
                        #endregion

                        trans.Commit();

                        if (atualizarCurriculoSolr)
                            BufferAtualizacaoCurriculo.Update(this);

                        if (this.FlagInativo || this.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)Enumeradores.SituacaoCurriculo.Hibernado))
                            ExcluirCVSolr(this.IdCurriculo, PessoaFisica.EmailPessoa);

                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region Dados Mini Curriculo PesquisaCurriculo.aspx

        #region RecuperarEscolaridadeMiniCurriculo
        /// <summary>
        /// M�todo utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>IDataReader</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable RecuperarEscolaridadeMiniCurriculo(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spcurriculominicurriculoescolaridade, parms).Tables[0];
        }
        #endregion

        #region RecuperarExperienciaMiniCurriculo
        /// <summary>
        /// M?todo utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>IDataReader</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable RecuperarExperienciaMiniCurriculo(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spcurriculominicurriculoexperiencia, parms).Tables[0];
        }
        #endregion

        #region RecuperarFuncaoPretendidaMiniCurriculo
        /// <summary>
        /// M?todo utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>IDataReader</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable RecuperarFuncaoPretendidaMiniCurriculo(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Spcurriculominicurriculofuncaopretendida, parms).Tables[0];
        }
        #endregion

        #endregion

        #region CarregarCurriculosPorFiltro
        /// <summary>
        /// Retorna curr?culos por Codigo, CPF, Telefone, Email ou Nome
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="paginaCorrente"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable CarregarCurriculosPorFiltro(string filtro, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            filtro = filtro.Trim();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentPage", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaCorrente } ,
                    new SqlParameter { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina }
                };

            var sqlParamCPF = new SqlParameter("@Num_CPF", SqlDbType.Decimal, 11);
            var sqlParamTelefone = new SqlParameter("@Telefone", SqlDbType.Char, 9);
            var sqlParamEmail = new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100);
            var sqlParamNome = new SqlParameter("@Nome", SqlDbType.VarChar, 120);
            var sqlParamIdCurriculo = new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4);
            sqlParamCPF.Value = DBNull.Value;
            sqlParamTelefone.Value = DBNull.Value;
            sqlParamEmail.Value = DBNull.Value;
            sqlParamNome.Value = DBNull.Value;
            sqlParamIdCurriculo.Value = DBNull.Value;



            foreach (var item in filtro)
            {
                if (!String.IsNullOrEmpty(filtro))
                {
                    if (Regex.IsMatch(filtro, Validacao.RegexEmail))
                    {
                        sqlParamEmail.Value = filtro;
                        break;
                    }
                    else if (Regex.IsMatch(filtro, @"^\d{11}$") || Regex.IsMatch(filtro, Validacao.RegexCPF))
                    {
                        var oRegEx = new Regex(@"[.\\/-]");
                        sqlParamCPF.Value = oRegEx.Replace(filtro, string.Empty);
                        break;
                    }
                    else
                    {
                        if (Regex.IsMatch(filtro, @"^\d{3,4}.-\d{4}$"))
                        {
                            sqlParamTelefone.Value = filtro.Replace("-", string.Empty);
                            break;
                        }
                        //else if (Regex.IsMatch(filtro, @"[.,\d*,-]")) //Sen?o verifica se ? um n?mero com os caracteres - ou .
                        //{
                        //    sqlParamTelefone.Value = filtro.Replace("-", String.Empty);
                        //    break;
                        //}

                        Int32 aux;
                        if (Int32.TryParse(filtro, out aux))
                        {
                            sqlParamIdCurriculo.Value = aux;
                            break;
                        }
                        else
                        { // Sen?o ? texto
                            sqlParamNome.Value = filtro;
                            break;
                        }
                    }
                }
                break;
            }


            parms.Add(sqlParamCPF);
            parms.Add(sqlParamTelefone);
            parms.Add(sqlParamEmail);
            parms.Add(sqlParamNome);
            parms.Add(sqlParamIdCurriculo);

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcurriculosfiltro, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }
        #endregion

        #region BloquearCurriculo
        /// <summary>
        /// Altera o status do curr?culo para bloqueado e salva o motivo na tabela de corre??o
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="motivo"></param>
        /// <param name="objUsuarioFilialPerfil">Usuário que está efetuando o bloqueio</param>
        /// <returns></returns>
        public static bool BloquearCurriculo(int idCurriculo, string motivo, UsuarioFilialPerfil objUsuarioFilialPerfil = null)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Curriculo objCurriculo = LoadObject(idCurriculo);

                        UsuarioFilialPerfil objUsuarioFilialCurriculo;
                        if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivo(objCurriculo.PessoaFisica, trans, out objUsuarioFilialCurriculo))
                        {
                            objUsuarioFilialCurriculo.FlagInativo = true;
                            objUsuarioFilialCurriculo.Save(trans);
                        }

                        var objCurriculoCorrecao = new CurriculoCorrecao
                        {
                            Curriculo = objCurriculo,
                            DescricaoCorrecao = motivo,
                            FlagCorrigido = false,
                            FlagInativo = false,
                            UsuarioFilialPerfil = objUsuarioFilialPerfil ?? objUsuarioFilialCurriculo
                        };

                        objCurriculoCorrecao.Save(trans);

                        objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Bloqueado);
                        objCurriculo.FlagInativo = true;
                        objCurriculo.Save(trans);

                        trans.Commit();

                        if (objCurriculo.PessoaFisica == null || string.IsNullOrEmpty(objCurriculo.PessoaFisica.EmailPessoa))
                            objCurriculo.PessoaFisica.CompleteObject();

                        //Atualizar CV no SOLR
                        //BufferAtualizacaoCurriculo.Update(objCurriculo);
                        ExcluirCVSolr(objCurriculo.IdCurriculo, objCurriculo.PessoaFisica.EmailPessoa);

                        //Não enviar mais e-mail para a pessoa
                        EmailSituacao.BloquearEmail(objCurriculo.PessoaFisica.IdPessoaFisica, objCurriculo.PessoaFisica.EmailPessoa, $"Currículo Bloqueado: {motivo} - Bloqueado por idf_usuario_filial_perfil: { (objUsuarioFilialPerfil != null ? objUsuarioFilialPerfil.IdUsuarioFilialPerfil.ToString() : string.Empty)}");


                        if (!motivo.Contains("Bloqueado no Sine."))
                            InativarCVSine(objCurriculo, motivo);

                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region HibernarCurriculo
        /// <summary>
        /// Altera o status do currículo para Hibernado
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static bool HibernarCurriculo(int idCurriculo, int? usuariofilial, List<string> motivos = null)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Curriculo objCurriculo = LoadObject(idCurriculo, trans);

                        objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.Hibernado);
                        objCurriculo.FlagInativo = true;
                        objCurriculo.Save(trans);
                        if (usuariofilial.HasValue && motivos != null && motivos.Count() > 0)
                        {
                            CurriculoCorrecao objCurriculoCorrecao = new CurriculoCorrecao()
                            {
                                Curriculo = objCurriculo,
                                UsuarioFilialPerfil = new UsuarioFilialPerfil(usuariofilial.Value),
                                DescricaoCorrecao = $"Excluiu o cv pelos motivos: { string.Join(", ", motivos)}",
                                FlagCorrigido = false
                            };
                            objCurriculoCorrecao.Save(trans);
                        }


                        if (objCurriculo.PessoaFisica == null || string.IsNullOrEmpty(objCurriculo.PessoaFisica.EmailPessoa))
                            objCurriculo.PessoaFisica.CompleteObject(trans);

                        trans.Commit();

                        //Excluir CV no SOLR
                        ExcluirCVSolr(objCurriculo.IdCurriculo, objCurriculo.PessoaFisica.EmailPessoa);

                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region DesbloquearCurriculo
        public static bool DesbloquearCurriculo(int idCurriculo, string motivo, int idUsuarioFilialPerfil)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Curriculo objCurriculo = LoadObject(idCurriculo);

                        var objCurriculoCorrecao = new CurriculoCorrecao
                        {
                            Curriculo = objCurriculo,
                            DescricaoCorrecao = motivo,
                            FlagCorrigido = false,
                            FlagInativo = false,
                            UsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(idUsuarioFilialPerfil)
                        };

                        objCurriculoCorrecao.Save(trans);

                        UsuarioFilialPerfil objUsuarioFilialCurriculo;
                        if (UsuarioFilialPerfil.CarregarUsuarioFilialPerfilCandidatoAtivoEInativo(objCurriculo.PessoaFisica, trans, out objUsuarioFilialCurriculo))
                        {
                            objUsuarioFilialCurriculo.FlagInativo = false;
                            objUsuarioFilialCurriculo.Save(trans);
                        }

                        objCurriculo.FlagInativo = false;

                        if (objCurriculo.FlagVIP)
                            objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoRevisaoVIP);
                        else
                            objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoPublicacao);

                        objCurriculo.Save(trans);

                        trans.Commit();

                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        #endregion

        #region AlterarSituacao
        /// <summary>
        /// Altera o status do curr?culo.
        /// </summary>
        /// <param name="objSituacaoCurriculo"></param>
        /// <param name="trans"> </param>
        /// <returns></returns>
        public void AlterarSituacao(SituacaoCurriculo objSituacaoCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = IdCurriculo } ,
                    new SqlParameter { ParameterName = "@Idf_Situacao_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objSituacaoCurriculo.IdSituacaoCurriculo }
                };

            if (trans != null)
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SpAtualizarSituacaoCurriculo, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarSituacaoCurriculo, parms);
        }
        #endregion

        #region RecuperarCurriculosSiteMap
        public static IEnumerable<CurriculoSitemap> RecuperarCurriculosSiteMap()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_CV_SITEMAP, null))
            {
                while (dr.Read())
                {
                    yield return new CurriculoSitemap
                    {
                        IdfCurriculo = dr["Idf_Curriculo"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Curriculo"]) : (int?)null,
                        NomePessoa = dr["Nme_Pessoa"].ToString(),
                        DescricaoFuncao = dr["Des_Funcao"].ToString(),
                        NomeCidade = dr["Nme_Cidade"].ToString(),
                        SiglaEstado = dr["Sig_Estado"].ToString()
                    };
                }
            }
        }
        #endregion

        #region RecuperarCurriculosPorFuncaoSiteMap
        public static IEnumerable<CurriculoSitemap> RecuperarCurriculosPorFuncaoSiteMap()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_CV_FUNCAO_SITEMAP, null))
            {
                while (dr.Read())
                {
                    yield return new CurriculoSitemap
                    {
                        IdfCurriculo = dr["Idf_Curriculo"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Curriculo"]) : (int?)null,
                        NomePessoa = dr["Nme_Pessoa"].ToString(),
                        DescricaoFuncao = dr["Des_Funcao"].ToString(),
                        NomeCidade = dr["Nme_Cidade"].ToString(),
                        SiglaEstado = dr["Sig_Estado"].ToString()
                    };
                }
            }
        }
        #endregion

        #region RecuperarCurriculosPorCidadeSiteMap
        public static IEnumerable<CurriculoSitemap> RecuperarCurriculosPorCidadeSiteMap()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_CV_CIDADE_SITEMAP, null))
            {
                while (dr.Read())
                {
                    yield return new CurriculoSitemap
                    {
                        IdfCurriculo = dr["Idf_Curriculo"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Curriculo"]) : (int?)null,
                        NomePessoa = dr["Nme_Pessoa"].ToString(),
                        DescricaoFuncao = dr["Des_Funcao"].ToString(),
                        NomeCidade = dr["Nme_Cidade"].ToString(),
                        SiglaEstado = dr["Sig_Estado"].ToString()
                    };
                }
            }
        }
        #endregion

        #region RecuperarCurriculosPorFuncaoCidadeSiteMap
        public static IEnumerable<CurriculoSitemap> RecuperarCurriculosPorFuncaoCidadeSiteMap()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_CV_FUNCAO_CIDADE_SITEMAP, null))
            {
                while (dr.Read())
                {
                    yield return new CurriculoSitemap
                    {
                        IdfCurriculo = dr["Idf_Curriculo"] != DBNull.Value ? Convert.ToInt32(dr["Idf_Curriculo"]) : (int?)null,
                        NomePessoa = dr["Nme_Pessoa"].ToString(),
                        DescricaoFuncao = dr["Des_Funcao"].ToString(),
                        NomeCidade = dr["Nme_Cidade"].ToString(),
                        SiglaEstado = dr["Sig_Estado"].ToString()
                    };
                }
            }
        }
        #endregion

        #region RecuperarCurriculoSitemap
        public static CurriculoSitemap RecuperarCurriculoSitemap(Curriculo objCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo }
                };

            CurriculoSitemap objCurriculoSitemap = null;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarcurriculositemap, parms))
            {
                if (dr.Read())
                {
                    objCurriculoSitemap = new CurriculoSitemap
                    {
                        IdfCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]),
                        NomePessoa = dr["Nme_Pessoa"].ToString(),
                        DescricaoFuncao = dr["Des_Funcao"].ToString(),
                        NomeCidade = dr["Nme_Cidade"].ToString(),
                        SiglaEstado = dr["Sig_Estado"].ToString()
                    };
                }
            }
            return objCurriculoSitemap;
        }
        #endregion

        #region Insert
        /// <summary>
        /// M?todo utilizado para inserir uma inst?ncia de Curriculo no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert()
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (PessoaFisica != null && PessoaFisica.Endereco != null && PessoaFisica.Endereco.Cidade != null)
                            CidadeEndereco = PessoaFisica.Endereco.Cidade;

                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
                        this._idCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo"].Value);
                        cmd.Parameters.Clear();
                        this._persisted = true;
                        this._modified = false;

                        trans.Commit();

                        BufferAtualizacaoCurriculo.Update(this);
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// M?todo utilizado para inserir uma inst?ncia de Curriculo no banco de dados, dentro de uma transa??o.
        /// </summary>
        /// <param name="trans">Transa??o existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            if (PessoaFisica != null && PessoaFisica.Endereco != null && PessoaFisica.Endereco.Cidade != null)
                CidadeEndereco = PessoaFisica.Endereco.Cidade;

            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo"].Value);
            cmd.Parameters.Clear();

            this._persisted = true;
            this._modified = false;

            BufferAtualizacaoCurriculo.Update(this);
        }
        #endregion

        #region Update
        /// <summary>
        /// M?todo utilizado para atualizar uma inst?ncia de Curriculo no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Update()
        {
            if (this._modified)
            {
                if (PessoaFisica != null && PessoaFisica.Endereco != null && PessoaFisica.Endereco.Cidade != null)
                    CidadeEndereco = PessoaFisica.Endereco.Cidade;

                if (_modifiedFields.Count <= 0)
                {
                    this._modified = false;
                    return;
                }

                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);

                var sqlUpdate = "UPDATE BNE_Curriculo SET ";
                for (int i = 0; i < _modifiedFields.Count; i++)
                {
                    sqlUpdate += _modifiedFields[i] + " = @" + _modifiedFields[i];
                    if (i != _modifiedFields.Count - 1)
                        sqlUpdate += ",";
                }
                sqlUpdate += " WHERE Idf_Curriculo = @Idf_Curriculo";

                DataAccessLayer.ExecuteNonQuery(CommandType.Text, sqlUpdate, parms);
                this._modified = false;

                BufferAtualizacaoCurriculo.Update(this);
            }
        }
        /// <summary>
        /// M?todo utilizado para atualizar uma inst?ncia de Curriculo no banco de dados, dentro de uma transa??o.
        /// </summary>
        /// <param name="trans">Transa??o existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Update(SqlTransaction trans)
        {
            if (this._modified)
            {
                if (PessoaFisica != null && PessoaFisica.Endereco != null && PessoaFisica.Endereco.Cidade != null)
                    CidadeEndereco = PessoaFisica.Endereco.Cidade;

                if (_modifiedFields.Count <= 0)
                {
                    this._modified = false;
                    return;
                }

                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);

                var sqlUpdate = "UPDATE BNE_Curriculo SET ";
                for (int i = 0; i < _modifiedFields.Count; i++)
                {
                    sqlUpdate += _modifiedFields[i] + " = @" + _modifiedFields[i];
                    if (i != _modifiedFields.Count - 1)
                        sqlUpdate += ",";
                }
                sqlUpdate += " WHERE Idf_Curriculo = @Idf_Curriculo";

                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, sqlUpdate, parms);
                this._modified = false;

                BufferAtualizacaoCurriculo.Update(this);
            }
        }
        #endregion

        #region RecuperarIdPorPessoaFisica
        /// <summary>
        /// M?todo respons?vel por carregar o c?digo identificador de um curr?culo atrav?s do c?digo identificador da pessoa f?sica
        /// </summary>
        /// <param name="objPessoaFisica">PessoaFisica</param>
        /// <returns></returns>
        public static int RecuperarIdPorPessoaFisica(PessoaFisica objPessoaFisica, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica }
                };

            if (trans == null)
            {
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectidfcurriculoporidfpessoafisica, parms));
            }

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spselectidfcurriculoporidfpessoafisica, parms));

        }
        #endregion

        #region VIP
        public bool VIP()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = _idCurriculo }
                };

            return Convert.ToBoolean(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectvip, parms));
        }
        #endregion

        #region ExisteCurriculo
        /// <summary>
        /// Verifica se existe pessoa fisica cadastrada com o cpf informado
        /// </summary>
        /// <param name="objPessoaFisica"></param>
        /// <param name="idCurriculo"></param>
        /// <returns>Bolean</returns>
        public static bool ExisteCurriculo(PessoaFisica objPessoaFisica, out int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica }
                };
            Object retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, Spexistecurriculoporpessoafisica, parms);

            if (retorno != DBNull.Value)
            {
                idCurriculo = Convert.ToInt32(retorno);
                if (idCurriculo <= 0)
                    return false;

                return true;
            }

            idCurriculo = 0;
            return false;
        }
        #endregion

        #region RecuperarSituacao
        public SituacaoCurriculo RecuperarSituacao()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = _idCurriculo }
                };

            return new SituacaoCurriculo(Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectsituacaocurriculo, parms)));
        }
        #endregion

        #region RecuperarDataAtualizacao
        public DateTime RecuperarDataAtualizacao()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = _idCurriculo }
                };

            return Convert.ToDateTime(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectdataatualizacaocurriculo, parms));
        }
        #endregion

        #region DTO

        #region DestinatarioEnvioMensagens
        public static List<CelularSelecionadorDestinatario> CarregarObjetoDestinatariosEnvioMensagens(string strListCVs)
        {
            List<CelularSelecionadorDestinatario> lstDestinatario = new List<CelularSelecionadorDestinatario>();
            CelularSelecionadorDestinatario objDestinatario;

            using (IDataReader dr = RetornarDataReaderDestinatariosEnvioMensagem(strListCVs))
            {
                while (dr.Read())
                {
                    objDestinatario = new CelularSelecionadorDestinatario();
                    objDestinatario.IdCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
                    objDestinatario.IdPessoaFisica = Convert.ToInt32(dr["Idf_Pessoa_Fisica"]);
                    objDestinatario.IdUsuarioFilialPerfil = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]);
                    objDestinatario.Nome = PessoaFisica.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString());
                    objDestinatario.VIP = Convert.ToBoolean(dr["Flg_VIP"]);
                    objDestinatario.NumeroDDDCelular = dr["Num_DDD_Celular"].ToString();
                    objDestinatario.NumeroCelular = dr["Num_Celular"].ToString();
                    objDestinatario.Email = dr["Eml_Pessoa"].ToString();
                    lstDestinatario.Add(objDestinatario);
                }
            }
            return lstDestinatario;
        }
        private static IDataReader RetornarDataReaderDestinatariosEnvioMensagem(string strListCVs)
        {
            string SpDestinatariosEnvioMensagem = @"
                SELECT cv.Idf_Curriculo,
		                pf.Idf_Pessoa_Fisica,
		                ufp.idf_usuario_filial_perfil,
		                pf.Nme_Pessoa,
		                cv.Flg_VIP,
		                pf.Num_DDD_Celular,
		                pf.Num_Celular,
		                pf.Eml_Pessoa		
                FROM BNE.BNE_Curriculo cv WITH(NOLOCK)
                JOIN BNE.TAB_Pessoa_Fisica pf WITH(NOLOCK) ON cv.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
                JOIN TAB_Usuario_Filial_Perfil ufp WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica AND (Idf_Perfil = 2 OR Idf_Perfil = 3)
                WHERE Idf_Curriculo in (@strListCVs)
            ";
            SpDestinatariosEnvioMensagem = SpDestinatariosEnvioMensagem.Replace("@strListCVs", strListCVs);

            return DataAccessLayer.ExecuteReader(CommandType.Text, SpDestinatariosEnvioMensagem, null);
        }

        #endregion

        #region CarregarCurriculoDTO
        /// <summary>
        /// M?todo utilizado para retornar uma inst?ncia completa de Vaga a partir do banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <returns>DTO de curr?culo</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DTO.Curriculo CarregarCurriculoDTO(int idCurriculo)
        {
            return CarregarCurriculoDTO(idCurriculo, DadosCurriculo.Basico);
        }

        /// <summary>
        /// M?todo utilizado para retornar uma inst?ncia completa de Vaga a partir do banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="dadosCurriculo">Flag enumarator para identificar o que deve ser carregado</param>
        /// <returns>DTO de currículo</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DTO.Curriculo CarregarCurriculoDTO(int idCurriculo, DadosCurriculo dadosCurriculo)
        {
            var retorno = false;
            var objCurriculo = new DTO.Curriculo();
            using (IDataReader dr = RetornarDataReaderDTO(idCurriculo))
            {
                if (SetInstanceDTO(dr, objCurriculo))
                    retorno = true;
            }

            if ((dadosCurriculo & DadosCurriculo.FuncoesPretendidas) == DadosCurriculo.FuncoesPretendidas)
            {
                using (IDataReader dr = RetornarDataReaderFuncaoPretendidaDTO(idCurriculo))
                {
                    objCurriculo.FuncoesPretendidas = new List<DTO.FuncaoPretendida>();
                    while (dr.Read())
                    {
                        objCurriculo.FuncoesPretendidas.Add(new DTO.FuncaoPretendida
                        {
                            NomeFuncaoPretendida = dr["Des_Funcao"].ToString(),
                            QuantidadeExperiencia = dr["Qtd_Experiencia"] != DBNull.Value ? Convert.ToInt16(dr["Qtd_Experiencia"]) : (Int16?)null
                        });
                    }
                }
            }

            if ((dadosCurriculo & DadosCurriculo.ExperienciasProfissionais) == DadosCurriculo.ExperienciasProfissionais)
            {
                using (IDataReader dr = RetornarDataReaderExperienciaDTO(idCurriculo))
                {
                    objCurriculo.Experiencias = new List<DTO.ExperienciaProfissional>();
                    while (dr.Read())
                    {
                        objCurriculo.Experiencias.Add(new DTO.ExperienciaProfissional
                        {
                            DescricaoAtividade = dr["Des_Atividade"].ToString(),
                            RazaoSocial = dr["Raz_Social"].ToString(),
                            //sTempo = dr["Des_Tempo"].ToString(),
                            AreaBNE = dr["Des_Area_BNE"].ToString(),
                            DataAdmissao = Convert.ToDateTime(dr["Dta_Admissao"]),
                            DataDemissao = dr["Dta_Demissao"] != DBNull.Value ? Convert.ToDateTime(dr["Dta_Demissao"]) : (DateTime?)null,
                            Funcao = dr["Des_Funcao"].ToString(),
                            VlrSalario = dr["Vlr_Salario"] != DBNull.Value ? Convert.ToDecimal(dr["Vlr_Salario"].ToString()) : (decimal?)null
                        });
                    }
                }
            }

            if ((dadosCurriculo & DadosCurriculo.Idiomas) == DadosCurriculo.Idiomas)
            {
                using (IDataReader dr = RetornarDataReaderIdiomaDTO(idCurriculo))
                {
                    objCurriculo.Idiomas = new List<DTO.Idioma>();
                    while (dr.Read())
                    {
                        objCurriculo.Idiomas.Add(new DTO.Idioma { DescricaoIdioma = dr["Des_Idioma"].ToString(), NivelIdioma = dr["Des_Nivel_Idioma"].ToString() });
                    }
                }
            }

            if ((dadosCurriculo & DadosCurriculo.Formacoes) == DadosCurriculo.Formacoes)
            {
                using (IDataReader dr = RetornarDataReaderFormacaoDTO(idCurriculo))
                {
                    objCurriculo.Formacoes = new List<DTO.Formacao>();
                    while (dr.Read())
                    {
                        objCurriculo.Formacoes.Add(new DTO.Formacao
                        {
                            DescricaoFormacao = dr["Des_BNE"].ToString(),
                            DescricaoCurso = dr["Des_Curso"].ToString(),
                            NomeFonte = dr["Nme_Fonte"].ToString(),
                            SiglaFonte = dr["Sig_Fonte"].ToString(),
                            SituacaoFormacao = dr["Des_Situacao_Formacao"].ToString(),
                            AnoConclusao = Convert.IsDBNull(dr["Ano_Conclusao"]) ? null : new short?(Convert.ToInt16(dr["Ano_Conclusao"])),
                            Periodo = dr["Num_Periodo"].ToString()
                        });
                    }
                }
            }

            if ((dadosCurriculo & DadosCurriculo.Cursos) == DadosCurriculo.Cursos)
            {
                using (IDataReader dr = RetornarDataReaderCursoDTO(idCurriculo))
                {
                    objCurriculo.Cursos = new List<DTO.Formacao>();
                    while (dr.Read())
                    {
                        objCurriculo.Cursos.Add(new DTO.Formacao
                        {
                            DescricaoFormacao = dr["Des_BNE"].ToString(),
                            DescricaoCurso = dr["Des_Curso"].ToString(),
                            NomeFonte = dr["Nme_Fonte"].ToString(),
                            SiglaFonte = dr["Sig_Fonte"].ToString(),
                            SituacaoFormacao = dr["Des_Situacao_Formacao"].ToString(),
                            AnoConclusao = Convert.IsDBNull(dr["Ano_Conclusao"]) ? null : new short?(Convert.ToInt16(dr["Ano_Conclusao"])),
                            Periodo = dr["Num_Periodo"].ToString()
                        });
                    }
                }
            }

            if ((dadosCurriculo & DadosCurriculo.Veiculos) == DadosCurriculo.Veiculos)
            {
                using (IDataReader dr = RetornarDataReaderVeiculoDTO(idCurriculo))
                {
                    objCurriculo.Veiculos = new List<DTO.Veiculo>();
                    while (dr.Read())
                    {
                        objCurriculo.Veiculos.Add(new DTO.Veiculo { Ano = dr["Ano_Veiculo"].ToString(), Modelo = dr["Des_Modelo"].ToString(), Tipo = dr["Des_Tipo_Veiculo"].ToString() });
                    }
                }
            }

            if ((dadosCurriculo & DadosCurriculo.DisponibilidadesMudarCidade) == DadosCurriculo.DisponibilidadesMudarCidade)
            {
                using (IDataReader dr = RetornarDataReaderDisponibilidadeCidadeDTO(idCurriculo))
                {
                    objCurriculo.DisponibilidadesMorarEm = new List<DTO.DisponibilidadeMorarEm>();
                    while (dr.Read())
                    {
                        objCurriculo.DisponibilidadesMorarEm.Add(new DTO.DisponibilidadeMorarEm
                        {
                            Descricao = dr["Des_Disponibilidade"].ToString()
                        });
                    }
                }
            }

            if ((dadosCurriculo & DadosCurriculo.DisponibilidadesTrabalho) == DadosCurriculo.DisponibilidadesTrabalho)
            {
                using (IDataReader dr = RetornarDataReaderDisponibilidadeTrabalhoDTO(idCurriculo))
                {
                    objCurriculo.DisponibilidadesTrabalho = new List<DTO.DisponibilidadeTrabalho>();
                    while (dr.Read())
                    {
                        objCurriculo.DisponibilidadesTrabalho.Add(new DTO.DisponibilidadeTrabalho
                        {
                            Descricao = dr["Des_Disponibilidade"].ToString()
                        });
                    }
                }
            }

            if (retorno)
                return objCurriculo;

            return null;
        }

        public static IEnumerable<int> BuscarCurriculosIdModificacaoExportacao(DateTime fromDate, int idfCurriculoMin, int take, int skip, SqlTransaction trans)
        {
            bool toOpen = trans == null || trans.Connection == null || trans.Connection.State == ConnectionState.Closed || trans.Connection.State == ConnectionState.Broken;
            SqlConnection conn = null;
            try
            {
                if (toOpen)
                {
                    conn = new SqlConnection(DataAccessLayer.CONN_STRING);
                    conn.Open();
                    trans = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
                }

                if (fromDate < (DateTime)SqlDateTime.MinValue)
                    fromDate = (DateTime)SqlDateTime.MinValue;

                const string sqlToFormat = @"
                SELECT TOP ({0}) 
                    [Project1].[Idf_Curriculo] AS [Idf_Curriculo]
                    FROM ( SELECT [Project1].[Idf_Curriculo] AS [Idf_Curriculo], row_number() OVER (ORDER BY [Project1].[Idf_Curriculo] ASC) AS [row_number]
                            FROM ( SELECT 
                            [Extent1].[Idf_Curriculo] AS [Idf_Curriculo]
                            FROM [BNE].[BNE_Curriculo] AS [Extent1] with(nolock)
                            WHERE ([Extent1].[Flg_Inativo] = 0) AND (([Extent1].[Dta_Cadastro] > @Update_Date) OR ([Extent1].[Dta_Modificacao_CV] > @Update_Date)) AND ([Extent1].[Idf_Curriculo] > @Idf_Curriculo)
                        )  AS [Project1]
                    )  AS [Project1]
                    WHERE [Project1].[row_number] > {1}
                    ORDER BY [Project1].[Idf_Curriculo] ASC
                ";

                var sql = string.Format(sqlToFormat, take, skip);

                var list = new List<SqlParameter>
                {
                      new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idfCurriculoMin },
                      new SqlParameter { ParameterName = "@Update_Date", SqlDbType = SqlDbType.DateTime, Size = 8, Value = fromDate }
                };

                using (var reader = DataAccessLayer.ExecuteReader(trans, CommandType.Text, sql, list))
                {
                    while (reader.Read())
                    {
                        yield return Convert.ToInt32(reader["Idf_Curriculo"]);
                    }
                }
            }
            finally
            {
                if (toOpen)
                {
                    if (trans != null)
                        trans.Dispose();
                    if (conn != null)
                        conn.Dispose();
                }
            }

        }

        private static IDataReader RetornarDataUltimoPlanoVip(int pessoaFisicaId, SqlTransaction trans = null)
        {
            const string spUltimoPlano = @"
                            SELECT TOP 1 
	                        PA.Dta_Inicio_Plano , 
	                        PA.Dta_Fim_Plano 
                        FROM    BNE_Plano_Adquirido PA WITH(NOLOCK)
                        INNER JOIN BNE_Plano_Situacao PS WITH(NOLOCK) ON PA.Idf_Plano_Situacao = PS.Idf_Plano_Situacao
                        INNER JOIN BNE_Plano P WITH(NOLOCK) ON PA.Idf_Plano = P.Idf_Plano
                        WHERE   PA.Idf_Filial IS NULL
                            AND Idf_Usuario_Filial_Perfil IN (
		                        SELECT Idf_Usuario_Filial_Perfil FROM TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) WHERE UFP.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica)
		                        ORDER BY  PA.Dta_Fim_Plano DESC
                                ";

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = pessoaFisicaId }
                };

            if (trans != null)
                return DataAccessLayer.ExecuteReader(trans, CommandType.Text, spUltimoPlano, parms);

            return DataAccessLayer.ExecuteReader(CommandType.Text, spUltimoPlano, parms);
        }
        #endregion

        #region RetornarDataReaderDTO
        public static IDataReader RetornarDataReaderDTO(int idCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            #region spselectdadoscurriculo
            const string spselectdadoscurriculo = @"
                SELECT  C.Idf_Curriculo ,
                        PF.Idf_Pessoa_Fisica ,
                        PF.Num_CPF ,
                        PF.Nme_Pessoa ,
                        PF.Num_DDD_Celular ,
                        PF.Num_Celular,
                        PF.Num_DDD_Telefone ,
                        PF.Num_Telefone ,
                        PF.Dta_Nascimento ,
                        C.Dta_Atualizacao ,
                        C.Flg_VIP ,
                        PF.Eml_Pessoa ,
                        E.Num_CEP ,
                        E.Des_Logradouro ,
                        E.Num_Endereco ,
                        E.Des_Complemento ,
                        E.Des_Bairro ,
                        Cid.Nme_Cidade ,
                        Cid.Sig_Estado ,
                        Est.Nme_Estado ,
                        C.Vlr_Pretensao_Salarial ,
                        F.Des_Funcao ,
                        BNE.SF_Calcula_Idade_Em_Anos(PF.Dta_Nascimento) AS Num_Idade,
                        CASE 
                            WHEN Formacao.Des_Curso IS NULL
                            THEN Formacao.Des_BNE
                            ELSE Formacao.Des_BNE + ' em ' + Formacao.Des_Curso
                        END AS Des_Formacao,
                        Formacao.Des_Abreviada,
                        S.Des_Sexo ,
                        EC.Des_Estado_Civil ,
                        CH.Des_Categoria_Habilitacao,
                        PFC.Num_Peso ,
                        PFC.Num_Altura ,
                        R.Des_Raca ,
                        C.Obs_Curriculo ,
                        PFC.Des_Conhecimento ,
                        PFC.Flg_Filhos ,
                        PFC.Flg_Viagem ,
                        C.Vlr_Ultimo_Salario ,
                        CC.Num_DDD_Celular AS Num_DDD_Celular_Contato ,
                        CC.Num_Celular AS Num_Celular_Contato ,
                        CC.Nme_Contato AS Nme_Celular_Contato ,
                        CT.Num_DDD_Telefone AS Num_DDD_Telefone_Contato ,
                        CT.Num_Telefone AS Num_Telefone_Contato ,
                        CT.Nme_Contato AS Nme_Telefone_Contato ,
                        D.Des_Deficiencia,
                        OC.Nme_Operadora_Celular,
                        OC.Des_URL_Logo,
                        C.Idf_Situacao_Curriculo,
                        C.Flg_Inativo,
                        UFP.Idf_Usuario_Filial_Perfil,
                        pf.Flg_WhatsApp
                FROM    BNE_Curriculo C WITH(NOLOCK)
                        INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                        INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON UFP.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica AND (Idf_Perfil = 2 OR Idf_Perfil = 3)
                        INNER JOIN plataforma.TAB_Sexo S WITH(NOLOCK) ON PF.Idf_Sexo = S.Idf_Sexo
				        OUTER APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo AND Idf_Funcao IS NOT NULL ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
        				LEFT JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON F.Idf_Funcao = FP.Idf_Funcao
                        LEFT JOIN TAB_Endereco E WITH(NOLOCK) ON PF.Idf_Endereco = E.Idf_Endereco
                        LEFT JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON E.Idf_Cidade = Cid.Idf_Cidade
                        LEFT JOIN plataforma.TAB_Estado Est WITH(NOLOCK) ON Cid.Sig_Estado = Est.Sig_Estado
				        OUTER APPLY ( SELECT TOP 1 E.Des_BNE, F.Des_Curso, E.Des_Abreviada FROM BNE_Formacao F WITH(NOLOCK) INNER JOIN plataforma.TAB_Escolaridade E WITH(NOLOCK) ON F.Idf_Escolaridade = E.Idf_Escolaridade WHERE F.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica AND F.Flg_Inativo = 0 AND F.Idf_Escolaridade <> 18 /* Diferente de Aperfeicamento */ ORDER BY F.Idf_Escolaridade DESC ) AS Formacao
                        LEFT JOIN plataforma.TAB_Estado_Civil EC WITH(NOLOCK) ON PF.Idf_Estado_Civil = EC.Idf_Estado_Civil
                        LEFT JOIN TAB_Pessoa_Fisica_Complemento PFC WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = PFC.Idf_Pessoa_Fisica
                        LEFT JOIN plataforma.TAB_Categoria_Habilitacao CH WITH(NOLOCK) ON PFC.Idf_Categoria_Habilitacao = CH.Idf_Categoria_Habilitacao
                        LEFT JOIN plataforma.TAB_Raca R WITH(NOLOCK) ON PF.Idf_Raca = R.Idf_Raca 
                        LEFT JOIN plataforma.TAB_Deficiencia D WITH(NOLOCK) ON PF.Idf_Deficiencia = D.Idf_Deficiencia AND PF.Idf_Deficiencia <> 0 /* Nenhuma */
                        LEFT JOIN TAB_Contato CC WITH(NOLOCK) ON CC.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica AND CC.Idf_Tipo_Contato = 8 /* Recado Celular */
                        LEFT JOIN TAB_Contato CT WITH(NOLOCK) ON CT.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica AND CT.Idf_Tipo_Contato = 7 /* Recado Fixo */
                        LEFT JOIN plataforma.TAB_Operadora_Celular OC WITH(NOLOCK) ON PF.Idf_Operadora_Celular = OC.Idf_Operadora_Celular
                WHERE   C.Idf_Curriculo = @Idf_Curriculo
            ";
            #endregion

            if (trans != null)
                return DataAccessLayer.ExecuteReader(trans, CommandType.Text, spselectdadoscurriculo, parms);

            return DataAccessLayer.ExecuteReader(CommandType.Text, spselectdadoscurriculo, parms);
        }

        public static IDataReader RetornarDataReaderDTOComDatasMaisInfo(int idCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            #region spselectdadoscurriculo
            const string spselectdadoscurriculo = @"
                  SELECT  C.Idf_Curriculo ,
                        PF.Idf_Pessoa_Fisica ,
                        PF.Num_CPF ,
                        PF.Nme_Pessoa ,
                        PF.Num_DDD_Celular ,
                        PF.Num_Celular,
                        PF.Num_DDD_Telefone ,
                        PF.Num_Telefone ,
                        PF.Dta_Nascimento ,
                        C.Dta_Cadastro ,
                        C.Dta_Atualizacao ,
                        C.Dta_Modificacao_CV ,
                        C.Flg_VIP ,
                        PF.Eml_Pessoa ,
                        E.Num_CEP ,
                        E.Des_Logradouro ,
                        E.Num_Endereco ,
                        E.Des_Complemento ,
                        E.Des_Bairro ,
                        Cid.Nme_Cidade ,
                        Cid.Sig_Estado ,
                        Est.Nme_Estado ,
                        C.Vlr_Pretensao_Salarial ,
                        F.Des_Funcao ,
                        BNE.SF_Calcula_Idade_Em_Anos(PF.Dta_Nascimento) AS Num_Idade,
                        CASE 
                            WHEN Formacao.Des_Curso IS NULL
                            THEN Formacao.Des_BNE
                            ELSE Formacao.Des_BNE + ' em ' + Formacao.Des_Curso
                        END AS Des_Formacao,
                        Formacao.Des_Abreviada,
                        S.Des_Sexo ,
                        EC.Des_Estado_Civil ,
                        CH.Des_Categoria_Habilitacao,
                        PFC.Num_Peso ,
                        PFC.Num_Altura ,
                        R.Des_Raca ,
                        C.Obs_Curriculo ,
                        PFC.Des_Conhecimento ,
                        PFC.Flg_Filhos ,
                        PFC.Flg_Viagem ,
                        C.Vlr_Ultimo_Salario ,
                        CC.Num_DDD_Celular AS Num_DDD_Celular_Contato ,
                        CC.Num_Celular AS Num_Celular_Contato ,
                        CC.Nme_Contato AS Nme_Celular_Contato ,
                        CT.Num_DDD_Telefone AS Num_DDD_Telefone_Contato ,
                        CT.Num_Telefone AS Num_Telefone_Contato ,
                        CT.Nme_Contato AS Nme_Telefone_Contato ,
                        D.Des_Deficiencia,
                        OC.Nme_Operadora_Celular,
                        OC.Des_URL_Logo,
						TC.Des_Tipo_Curriculo,
						SC.Des_Situacao_Curriculo
                FROM    BNE_Curriculo C WITH(NOLOCK)
                        INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
                        INNER JOIN plataforma.TAB_Sexo S WITH(NOLOCK) ON PF.Idf_Sexo = S.Idf_Sexo
				        CROSS APPLY ( SELECT TOP 1 IDF_FUNCAO FROM BNE_Funcao_Pretendida FP WITH ( NOLOCK ) WHERE C.Idf_Curriculo = FP.Idf_Curriculo AND Idf_Funcao IS NOT NULL ORDER BY FP.Idf_Funcao_Pretendida ) AS FP
        				LEFT JOIN plataforma.TAB_Funcao F WITH (NOLOCK) ON F.Idf_Funcao = FP.Idf_Funcao
                        LEFT JOIN TAB_Endereco E WITH(NOLOCK) ON PF.Idf_Endereco = E.Idf_Endereco
                        LEFT JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON E.Idf_Cidade = Cid.Idf_Cidade
                        LEFT JOIN plataforma.TAB_Estado Est WITH(NOLOCK) ON Cid.Sig_Estado = Est.Sig_Estado
				        OUTER APPLY ( SELECT TOP 1 E.Des_BNE, F.Des_Curso, E.Des_Abreviada FROM BNE_Formacao F WITH(NOLOCK) INNER JOIN plataforma.TAB_Escolaridade E WITH(NOLOCK) ON F.Idf_Escolaridade = E.Idf_Escolaridade WHERE F.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica AND F.Flg_Inativo = 0 AND F.Idf_Escolaridade <> 18 /* Diferente de Aperfeicamento */ ORDER BY F.Idf_Escolaridade DESC ) AS Formacao
                        LEFT JOIN plataforma.TAB_Estado_Civil EC WITH(NOLOCK) ON PF.Idf_Estado_Civil = EC.Idf_Estado_Civil
                        LEFT JOIN TAB_Pessoa_Fisica_Complemento PFC WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = PFC.Idf_Pessoa_Fisica
                        LEFT JOIN plataforma.TAB_Categoria_Habilitacao CH WITH(NOLOCK) ON PFC.Idf_Categoria_Habilitacao = CH.Idf_Categoria_Habilitacao
                        LEFT JOIN plataforma.TAB_Raca R WITH(NOLOCK) ON PF.Idf_Raca = R.Idf_Raca 
                        LEFT JOIN plataforma.TAB_Deficiencia D WITH(NOLOCK) ON PF.Idf_Deficiencia = D.Idf_Deficiencia AND PF.Idf_Deficiencia <> 0 /* Nenhuma */
                        LEFT JOIN TAB_Contato CC WITH(NOLOCK) ON CC.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica AND CC.Idf_Tipo_Contato = 8 /* Recado Celular */
                        LEFT JOIN TAB_Contato CT WITH(NOLOCK) ON CT.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica AND CT.Idf_Tipo_Contato = 7 /* Recado Fixo */
                        LEFT JOIN plataforma.TAB_Operadora_Celular OC WITH(NOLOCK) ON PF.Idf_Operadora_Celular = OC.Idf_Operadora_Celular
						LEFT JOIN BNE_Tipo_Curriculo TC WITH (NOLOCK) ON TC.Idf_Tipo_Curriculo = C.Idf_Tipo_Curriculo
						LEFT JOIN BNE_Situacao_Curriculo SC WITH (NOLOCK) ON SC.Idf_Situacao_Curriculo = C.Idf_Situacao_Curriculo
                WHERE   C.Idf_Curriculo = @Idf_Curriculo
            ";
            #endregion

            if (trans != null)
                return DataAccessLayer.ExecuteReader(trans, CommandType.Text, spselectdadoscurriculo, parms);

            return DataAccessLayer.ExecuteReader(CommandType.Text, spselectdadoscurriculo, parms);
        }
        #endregion

        #region RetornarDataReaderFuncaoPretendidaDTO
        private static IDataReader RetornarDataReaderFuncaoPretendidaDTO(int idCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            const string Spselectnomefuncoesporcurriculo = @"
            SELECT  TOP 3 ISNULL(F.Des_Funcao, FP.Des_Funcao_Pretendida) AS Des_Funcao, Qtd_Experiencia
            FROM    BNE_Funcao_Pretendida FP WITH(NOLOCK) 
                    LEFT JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON FP.Idf_Funcao = F.Idf_Funcao
            WHERE   Idf_Curriculo = @Idf_Curriculo
            ORDER BY Idf_Funcao_Pretendida ASC 
            ";

            if (trans == null)
                return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectnomefuncoesporcurriculo, parms);

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectnomefuncoesporcurriculo, parms);
        }
        #endregion

        #region RetornarDataReaderExperienciaDTO
        private static IDataReader RetornarDataReaderExperienciaDTO(int idCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            const string Spselectexperienciaporcurriculo = @"
            SELECT TOP 5
                EP.Raz_Social ,
                EP.Des_Atividade ,
                EP.Dta_Admissao ,
                EP.Dta_Demissao ,
                EP.Vlr_Salario,
                AB.Des_Area_BNE ,
                ISNULL(F.Des_Funcao, EP.Des_Funcao_Exercida) AS Des_Funcao ,
                CASE WHEN EP.Dta_Demissao IS NULL THEN 1
                     WHEN EP.Dta_Demissao IS NOT NULL THEN 2
                END AS 'Ordem'
            FROM
                BNE_Experiencia_Profissional EP WITH ( NOLOCK )
                INNER JOIN BNE_Curriculo C WITH ( NOLOCK ) ON EP.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
                INNER JOIN plataforma.TAB_Area_BNE AB WITH ( NOLOCK ) ON EP.Idf_Area_BNE = AB.Idf_Area_BNE
                LEFT JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON F.Idf_Funcao = EP.Idf_Funcao
            WHERE
                C.Idf_Curriculo = @Idf_Curriculo
                AND EP.Flg_Inativo = 0
            ORDER BY
                Ordem ASC ,
                EP.Dta_Demissao DESC ,
                EP.Dta_Admissao DESC
            ";

            if (trans == null)
                return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectexperienciaporcurriculo, parms);

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectexperienciaporcurriculo, parms);
        }
        #endregion

        #region RetornarDataReaderIdiomaDTO
        private static IDataReader RetornarDataReaderIdiomaDTO(int idCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            const string Spselectidiomaporcurriculo = @"
            SELECT  Des_Idioma, Des_Nivel_Idioma
            FROM    TAB_Pessoa_fisica_Idioma PFI WITH(NOLOCK)
                    INNER JOIN TAB_Idioma I WITH(NOLOCK) ON PFI.Idf_Idioma = I.Idf_Idioma
                    INNER JOIN BNE_Curriculo C WITH(NOLOCK) ON PFI.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
                    INNER JOIN TAB_Nivel_Idioma NI WITH(NOLOCK) ON NI.Idf_Nivel_Idioma = PFI.Idf_Nivel_Idioma
            WHERE   C.Idf_Curriculo = @Idf_Curriculo
                    AND PFI.Flg_Inativo = 0            
            ";

            if (trans == null)
                return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectidiomaporcurriculo, parms);

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectidiomaporcurriculo, parms);
        }
        #endregion

        #region RetornarDataReaderVeiculoDTO
        private static IDataReader RetornarDataReaderVeiculoDTO(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            const string Spselectveiculoporcurriculo = @"
            SELECT  PFV.Ano_Veiculo, PFV.Des_Modelo, TV.Des_Tipo_Veiculo
            FROM    TAB_Pessoa_Fisica_Veiculo PFV WITH(NOLOCK)
                    INNER JOIN BNE_Curriculo C WITH(NOLOCK) ON PFV.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
                    INNER JOIN plataforma.TAB_Tipo_Veiculo TV WITH(NOLOCK) ON PFV.Idf_Tipo_Veiculo = TV.Idf_Tipo_Veiculo
            WHERE   C.Idf_Curriculo = @Idf_Curriculo
                    AND PFV.Flg_Inativo = 0
            ";

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectveiculoporcurriculo, parms);
        }
        #endregion

        #region RetornarDataReaderDisponibilidadeCidadeDTO
        private static IDataReader RetornarDataReaderDisponibilidadeCidadeDTO(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            const string Spselectdisponibilidadecidadecurriculo = @"
            SELECT  CI.Nme_Cidade + '/' + CI.Sig_Estado AS Des_Disponibilidade
            FROM    BNE_Curriculo_Disponibilidade_Cidade AS CDC WITH(NOLOCK)
                    INNER JOIN plataforma.TAB_Cidade AS CI WITH(NOLOCK) ON CDC.Idf_Cidade = CI.Idf_Cidade 
            WHERE   Idf_Curriculo = @Idf_Curriculo 
                    AND CDC.Flg_Inativo = 0
            ";

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectdisponibilidadecidadecurriculo, parms);
        }
        #endregion

        #region RetornarDataReaderDisponibilidadeTrabalhoDTO
        private static IDataReader RetornarDataReaderDisponibilidadeTrabalhoDTO(int idCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            const string Spselectdisponibilidadetrabalhoporcurriculo = @"
            SELECT Des_Disponibilidade
            FROM    BNE_Curriculo_Disponibilidade CD WITH(NOLOCK)
                    INNER JOIN Tab_Disponibilidade D WITH(NOLOCK) ON CD.Idf_Disponibilidade = D.Idf_Disponibilidade
            WHERE   CD.Idf_Curriculo = @Idf_Curriculo
            ";

            if (trans == null)
                return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectdisponibilidadetrabalhoporcurriculo, parms);

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectdisponibilidadetrabalhoporcurriculo, parms);
        }

        //[Obsolete("Obtado por n�o utiliza��o/disponibiliza��o.")]
        //        private static IDataReader RetornarDataReaderTipoVinculo(int idCurriculo)
        //        {
        //            var parms = new List<SqlParameter>
        //                {
        //                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
        //                };

        //            const string spSelectTipoVinculoPorCurriculo = @"
        //            SELECT vinc.Des_Tipo_Vinculo FROM BNE_Tipo_Vinculo as vinc
        //            INNER JOIN BNE_Curriculo_Tipo_Vinculo as rel
        //            ON rel.Idf_Tipo_Vinculo = vinc.Idf_Tipo_Vinculo
        //            WHERE rel.Idf_Curriculo = @Idf_Curriculo
        //            ORDER BY vinc.Cod_Grau_Tipo_Vinculo
        //            ";

        //            return DataAccessLayer.ExecuteReader(CommandType.Text, spSelectTipoVinculoPorCurriculo, parms);
        //        }
        #endregion

        #region RetornarDataReaderFormacaoDTO
        private static IDataReader RetornarDataReaderFormacaoDTO(int idCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            const string Spselectformacaoporcurriculo = @"
            SELECT  E.Des_BNE ,
                    ISNULL(C.Des_Curso , F.Des_Curso) AS Des_Curso ,
                    ISNULL(FT.Nme_Fonte, F.Des_Fonte) AS Nme_Fonte ,
                    FT.Sig_Fonte ,
                    SF.Des_Situacao_Formacao,
                    F.Ano_Conclusao,
                    F.Num_Periodo
            FROM    BNE_Formacao F
                    INNER JOIN BNE_Curriculo Cur WITH(NOLOCK) ON F.Idf_Pessoa_Fisica = Cur.Idf_Pessoa_Fisica
                    LEFT JOIN plataforma.TAB_Escolaridade E ON F.Idf_Escolaridade = E.Idf_Escolaridade
                    LEFT JOIN TAB_Curso C ON F.Idf_Curso = C.Idf_Curso
                    LEFT JOIN TAB_Fonte FT ON F.Idf_Fonte = FT.Idf_Fonte
                    LEFT JOIN BNE_Situacao_Formacao SF ON F.Idf_Situacao_Formacao = SF.Idf_Situacao_Formacao
            WHERE   Cur.Idf_Curriculo = @Idf_Curriculo 
                    AND F.Flg_Inativo = 0 
                    AND E.Flg_BNE = 1         
                    AND F.Idf_Escolaridade <> 18
            ORDER BY F.Idf_Escolaridade DESC, F.Ano_Conclusao DESC
            ";

            if (trans == null)
                return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectformacaoporcurriculo, parms);

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectformacaoporcurriculo, parms);
        }
        #endregion

        #region RetornarDataReaderCursoDTO
        private static IDataReader RetornarDataReaderCursoDTO(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            const string Spselectcursoporcurriculo = @"
            SELECT  E.Des_BNE ,
                    ISNULL(C.Des_Curso , F.Des_Curso) AS Des_Curso ,
                    ISNULL(FT.Nme_Fonte, F.Des_Fonte) AS Nme_Fonte ,
                    FT.Sig_Fonte ,
                    SF.Des_Situacao_Formacao,
                    F.Ano_Conclusao,
                    F.Num_Periodo
            FROM    BNE_Formacao F
                    INNER JOIN BNE_Curriculo Cur WITH(NOLOCK) ON F.Idf_Pessoa_Fisica = Cur.Idf_Pessoa_Fisica
                    LEFT JOIN plataforma.TAB_Escolaridade E ON F.Idf_Escolaridade = E.Idf_Escolaridade
                    LEFT JOIN TAB_Curso C ON F.Idf_Curso = C.Idf_Curso
                    LEFT JOIN TAB_Fonte FT ON F.Idf_Fonte = FT.Idf_Fonte
                    LEFT JOIN BNE_Situacao_Formacao SF ON F.Idf_Situacao_Formacao = SF.Idf_Situacao_Formacao
            WHERE   Cur.Idf_Curriculo = @Idf_Curriculo 
                    AND F.Flg_Inativo = 0 
                    AND E.Flg_BNE = 1   
                    AND F.Idf_Escolaridade = 18
            ORDER BY F.Idf_Escolaridade DESC, F.Ano_Conclusao DESC
            ";

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcursoporcurriculo, parms);
        }
        #endregion

        #region SetInstanceDTO
        /// <summary>
        /// M?todo auxiliar utilizado pelos m?todos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objCurriculo">Inst?ncia a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a opera??o foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceDTO(IDataReader dr, DTO.Curriculo objCurriculo)
        {
            try
            {
                if (dr.Read())
                {
                    objCurriculo.IdCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
                    objCurriculo.IdPessoaFisica = Convert.ToInt32(dr["Idf_Pessoa_Fisica"]);
                    objCurriculo.IdUsuarioFilialPerfil = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]);

                    if (dr["Num_CPF"] != DBNull.Value)
                        objCurriculo.NumeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
                    if (dr["Nme_Pessoa"] != DBNull.Value)
                        objCurriculo.NomeCompleto = dr["Nme_Pessoa"].ToString();
                    if (dr["Flg_VIP"] != DBNull.Value)
                        objCurriculo.VIP = Convert.ToBoolean(dr["Flg_VIP"]);
                    if (dr["Flg_Inativo"] != DBNull.Value)
                        objCurriculo.Inativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Idf_Situacao_Curriculo"] != DBNull.Value)
                        objCurriculo.Bloqueado = (Enumeradores.SituacaoCurriculo)Enum.Parse(typeof(Enumeradores.SituacaoCurriculo), dr["Idf_Situacao_Curriculo"].ToString()) == Enumeradores.SituacaoCurriculo.Bloqueado;
                    if (dr["Num_DDD_Celular"] != DBNull.Value)
                        objCurriculo.NumeroDDDCelular = dr["Num_DDD_Celular"].ToString();
                    if (dr["Num_Celular"] != DBNull.Value)
                        objCurriculo.NumeroCelular = dr["Num_Celular"].ToString();
                    if (dr["Num_DDD_Telefone"] != DBNull.Value)
                        objCurriculo.NumeroDDDTelefone = dr["Num_DDD_Telefone"].ToString();
                    if (dr["Num_Telefone"] != DBNull.Value)
                        objCurriculo.NumeroTelefone = dr["Num_Telefone"].ToString();
                    if (dr["Dta_Nascimento"] != DBNull.Value)
                        objCurriculo.DataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
                    if (dr["Dta_Atualizacao"] != DBNull.Value)
                        objCurriculo.DataAtualizacaoCurriculo = Convert.ToDateTime(dr["Dta_Atualizacao"]);
                    if (dr["Eml_Pessoa"] != DBNull.Value)
                        objCurriculo.Email = dr["Eml_Pessoa"].ToString();
                    if (dr["Num_CEP"] != DBNull.Value)
                        objCurriculo.NumeroCEP = dr["Num_CEP"].ToString();
                    if (dr["Des_Logradouro"] != DBNull.Value)
                        objCurriculo.Logradouro = dr["Des_Logradouro"].ToString();
                    if (dr["Num_Endereco"] != DBNull.Value)
                        objCurriculo.NumeroEndereco = dr["Num_Endereco"].ToString();
                    if (dr["Des_Complemento"] != DBNull.Value)
                        objCurriculo.Complemento = dr["Des_Complemento"].ToString();
                    if (dr["Des_Bairro"] != DBNull.Value)
                        objCurriculo.Bairro = dr["Des_Bairro"].ToString();
                    if (dr["Nme_Cidade"] != DBNull.Value)
                        objCurriculo.NomeCidade = dr["Nme_Cidade"].ToString();
                    if (dr["Sig_Estado"] != DBNull.Value)
                        objCurriculo.SiglaEstado = dr["Sig_Estado"].ToString();
                    if (dr["Nme_Estado"] != DBNull.Value)
                        objCurriculo.NomeEstado = dr["Nme_Estado"].ToString();
                    if (dr["Vlr_Pretensao_Salarial"] != DBNull.Value)
                        objCurriculo.ValorPretensaoSalarial = Convert.ToDecimal(dr["Vlr_Pretensao_Salarial"]);
                    if (dr["Des_Funcao"] != DBNull.Value)
                        objCurriculo.NomeFuncaoPretendida = dr["Des_Funcao"].ToString();
                    else
                        objCurriculo.NomeFuncaoPretendida = "";

                    if (dr["Num_Idade"] != DBNull.Value)
                        objCurriculo.Idade = Convert.ToInt32(dr["Num_Idade"]);
                    if (dr["Des_Formacao"] != DBNull.Value)
                        objCurriculo.UltimaFormacaoCompleta = dr["Des_Formacao"].ToString();
                    if (dr["Des_Abreviada"] != DBNull.Value)
                        objCurriculo.UltimaFormacaoAbreviada = dr["Des_Abreviada"].ToString();
                    if (dr["Des_Sexo"] != DBNull.Value)
                        objCurriculo.Sexo = dr["Des_Sexo"].ToString();
                    if (dr["Des_Estado_Civil"] != DBNull.Value)
                        objCurriculo.EstadoCivil = dr["Des_Estado_Civil"].ToString();
                    if (dr["Des_Categoria_Habilitacao"] != DBNull.Value)
                        objCurriculo.CategoriaHabilitacao = dr["Des_Categoria_Habilitacao"].ToString();
                    if (dr["Des_Raca"] != DBNull.Value)
                        objCurriculo.Raca = dr["Des_Raca"].ToString();
                    if (dr["Num_Peso"] != DBNull.Value)
                        objCurriculo.Peso = Convert.ToDecimal(dr["Num_Peso"]);
                    if (dr["Num_Altura"] != DBNull.Value)
                        objCurriculo.Altura = Convert.ToDecimal(dr["Num_Altura"]);
                    if (dr["Des_Deficiencia"] != DBNull.Value)
                        objCurriculo.Deficiencia = dr["Des_Deficiencia"].ToString();
                    if (dr["Obs_Curriculo"] != DBNull.Value)
                        objCurriculo.Observacao = dr["Obs_Curriculo"].ToString();
                    if (dr["Des_Conhecimento"] != DBNull.Value)
                        objCurriculo.OutrosConhecimentos = dr["Des_Conhecimento"].ToString();
                    if (dr["Flg_Filhos"] != DBNull.Value)
                        objCurriculo.TemFilhos = Convert.ToBoolean(dr["Flg_Filhos"]);
                    if (dr["Flg_Viagem"] != DBNull.Value)
                        objCurriculo.DisponibilidadeViajar = Convert.ToBoolean(dr["Flg_Viagem"]);
                    if (dr["Vlr_Ultimo_Salario"] != DBNull.Value)
                        objCurriculo.ValorUltimoSalario = Convert.ToDecimal(dr["Vlr_Ultimo_Salario"]);
                    if (dr["Num_DDD_Celular_Contato"] != DBNull.Value)
                        objCurriculo.NumeroDDDCelularRecado = dr["Num_DDD_Celular_Contato"].ToString();
                    if (dr["Num_Celular_Contato"] != DBNull.Value)
                        objCurriculo.NumeroCelularRecado = dr["Num_Celular_Contato"].ToString();
                    if (dr["Nme_Celular_Contato"] != DBNull.Value)
                        objCurriculo.CelularRecadoContato = dr["Nme_Celular_Contato"].ToString();
                    if (dr["Num_DDD_Telefone_Contato"] != DBNull.Value)
                        objCurriculo.NumeroDDDTelefoneRecado = dr["Num_DDD_Telefone_Contato"].ToString();
                    if (dr["Num_Telefone_Contato"] != DBNull.Value)
                        objCurriculo.NumeroTelefoneRecado = dr["Num_Telefone_Contato"].ToString();
                    if (dr["Nme_Telefone_Contato"] != DBNull.Value)
                        objCurriculo.TelefoneRecadoContato = dr["Nme_Telefone_Contato"].ToString();
                    if (dr["Nme_Operadora_Celular"] != DBNull.Value)
                        objCurriculo.NomeOperadoraCelular = dr["Nme_Operadora_Celular"].ToString();
                    if (dr["Des_URL_Logo"] != DBNull.Value)
                        objCurriculo.URLImagemOperadoraCelular = dr["Des_URL_Logo"].ToString();
                    if (dr["Flg_WhatsApp"] != DBNull.Value)
                        objCurriculo.Flg_WhatsApp = Convert.ToBoolean(dr["Flg_WhatsApp"]);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

        [Flags]
        public enum DadosCurriculo
        {
            FuncoesPretendidas,
            ExperienciasProfissionais,
            Idiomas,
            Formacoes,
            Cursos,
            Veiculos,
            DisponibilidadesTrabalho,
            DisponibilidadesMudarCidade,
            Basico,

            Tudo = FuncoesPretendidas | ExperienciasProfissionais | Idiomas | Formacoes | Cursos | Veiculos | DisponibilidadesTrabalho | DisponibilidadesMudarCidade
        }

        #endregion

        #region CarregarCurriculoSolr
        /// <summary>
        /// Método que recupera e retorna um currículo do Solr. Apenas currículos que aparecem na busca terão seus dados no Solr
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="carregaEmail">Booleano indicando se o email deve ser carregado.</param>
        /// <returns></returns>
        public static DTO.Curriculo CarregarCurriculoSolr(int idCurriculo, bool carregaEmail = false)
        {
            var urlCVSolr = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrRetornarCV);
            urlCVSolr = urlCVSolr.Replace("{Idf_Curriculo}", idCurriculo.ToString());

            SolrResponse<CurriculoCompleto.Response> resultado = null;

            try
            {
                //resultado = PesquisaCurriculo.EfetuarRequisicao(urlCVSolr);
                resultado = BNE.Solr.CurriculoCompleto.EfetuarRequisicao(urlCVSolr);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

            if (resultado == null)
                return null;

            if (resultado.response.numFound == 0)
                return null;

            return MapearCurriculo(resultado.response.docs[0], carregaEmail);
        }
        #endregion

        public static DTO.Curriculo MapearCurriculo(CurriculoCompleto.Response curriculo, bool carregaEmail = false)
        {
            var curriculoRetorno = new DTO.Curriculo
            {
                IdCurriculo = curriculo.Idf_Curriculo,
                NumeroCPF = curriculo.Num_CPF,
                NomeCompleto = curriculo.Nme_Pessoa,
                DataNascimento = curriculo.Dta_Nascimento,
                VIP = curriculo.Flg_VIP,
                AceitaEstagio = curriculo.Flg_CurriculoAceitaEstagio,
                DataAtualizacaoCurriculo = curriculo.Dta_Atualizacao,
                Sexo = curriculo.Des_Sexo,
                EstadoCivil = curriculo.Des_Estado_Civil,
                Idade = Helper.CalcularIdade(curriculo.Dta_Nascimento),
                TemFilhos = curriculo.Flg_Filhos,
                UltimaFormacaoAbreviada = curriculo.RecuperarSiglaMaiorFormacao(),
                ValorPretensaoSalarial = curriculo.Vlr_Pretensao_Salarial,
                Logradouro = curriculo.Des_Logradouro,
                NumeroCEP = curriculo.Num_CEP,
                Bairro = curriculo.Des_Bairro,
                NomeCidade = curriculo.Nme_Cidade,
                SiglaEstado = curriculo.Sig_Estado,
                NumeroDDDCelular = curriculo.Num_DDD_Celular,
                NumeroCelular = curriculo.Num_Celular,
                NomeOperadoraCelular = curriculo.Nme_Operadora_Celular,
                URLImagemOperadoraCelular = curriculo.Des_URL_Logo,
                NumeroDDDTelefone = curriculo.Num_DDD_Telefone,
                NumeroTelefone = curriculo.Num_Telefone,
                NomeFuncaoPretendida = curriculo.Des_Funcao?.FirstOrDefault(),
                CategoriaHabilitacao = curriculo.Des_Categoria_Habilitacao,
                Observacao = curriculo.Obs_Curriculo,
                OutrosConhecimentos = curriculo.Des_Conhecimento,
                Altura = curriculo.Num_Altura,
                Peso = curriculo.Num_Peso,
                Raca = curriculo.Des_Raca,
                DisponibilidadeViajar = curriculo.Flg_Viagem,
                Deficiencia = curriculo.Des_Deficiencia,
                UltimaFormacaoCompleta = curriculo.Des_Escolaridade,
                Flg_WhatsApp = curriculo.Flg_WhatsApp
            };

            if (carregaEmail)
                curriculoRetorno.Email = curriculo.Eml_Pessoa;

            curriculoRetorno.FuncoesPretendidas = new List<DTO.FuncaoPretendida>();
            if (curriculo.Des_Funcao != null)
            {
                for (int i = 0; i < curriculo.Des_Funcao.Count; i++)
                {
                    curriculoRetorno.FuncoesPretendidas.Add(new DTO.FuncaoPretendida
                    {
                        NomeFuncaoPretendida = curriculo.Des_Funcao[i],
                        QuantidadeExperiencia = (short?)(curriculo.Qtd_Experiencia != null ? curriculo.Qtd_Experiencia[i] : 0)
                    });

                }
            }

            curriculoRetorno.Idiomas = new List<DTO.Idioma>();
            if (curriculo.Des_Idioma != null)
            {
                for (int i = 0; i < curriculo.Des_Idioma.Count; i++)
                {
                    curriculoRetorno.Idiomas.Add(new DTO.Idioma
                    {
                        DescricaoIdioma = curriculo.Des_Idioma[i],
                        NivelIdioma = curriculo.Des_Nivel_Idioma != null ? curriculo.Des_Nivel_Idioma[i] : null
                    });
                }
            }

            curriculoRetorno.Formacoes = new List<DTO.Formacao>();
            curriculoRetorno.Cursos = new List<DTO.Formacao>();
            if (curriculo.Idf_Grau_Escolaridade != null)
            {
                for (int i = 0; i < curriculo.Idf_Grau_Escolaridade.Count; i++)
                {
                    var formacao = new DTO.Formacao
                    {
                        DescricaoFormacao = curriculo.Des_Escolaridade_Formacao[i],
                        DescricaoCurso = curriculo.Des_Curso[i],
                        SiglaFonte = curriculo.Sig_Fonte != null ? curriculo.Sig_Fonte[i] : string.Empty,
                        NomeFonte = curriculo.Des_Fonte != null ? curriculo.Des_Fonte[i] : string.Empty,
                        AnoConclusao = curriculo.Ano_Conclusao[i] != null && curriculo.Ano_Conclusao[i] != "0" ? Convert.ToInt16(curriculo.Ano_Conclusao[i]) : (Int16?)null,
                        SituacaoFormacao = curriculo.Des_Situacao_Formacao != null && curriculo.Des_Situacao_Formacao[i] != null ? curriculo.Des_Situacao_Formacao[i] : string.Empty,
                        Periodo = curriculo.Num_Periodo != null && curriculo.Num_Periodo[i] != -1 ? curriculo.Num_Periodo[i].ToString() : string.Empty
                    };

                    if (curriculo.Idf_Grau_Escolaridade[i] != 5)
                        curriculoRetorno.Formacoes.Add(formacao);
                    else
                        curriculoRetorno.Cursos.Add(formacao);
                }
            }

            curriculoRetorno.Experiencias = new List<DTO.ExperienciaProfissional>();
            if (curriculo.Raz_Social != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (curriculo.Raz_Social.Count > i && curriculo.Raz_Social[i] != null)
                    {
                        curriculoRetorno.Experiencias.Add(new DTO.ExperienciaProfissional
                        {
                            RazaoSocial = curriculo.Raz_Social[i],
                            DataAdmissao = curriculo.Dta_Admissao[i],
                            DataDemissao = curriculo.Dta_Demissao[i] != null && curriculo.Dta_Demissao[i].ToShortDateString() != "01/01/1900" ? Convert.ToDateTime(curriculo.Dta_Demissao[i]) : (DateTime?)null,
                            Funcao = curriculo.Des_Funcao_Exercida[i],
                            DescricaoAtividade = curriculo.Des_Atividade[i],
                            AreaBNE = curriculo.Des_Atividade_empresa[i],
                            VlrSalario = curriculo.Vlr_Ultimo_Salario > 0 ? curriculo.Vlr_Ultimo_Salario : (decimal?)null
                        });
                    }
                }
            }

            curriculoRetorno.DisponibilidadesTrabalho = new List<DisponibilidadeTrabalho>();
            if (curriculo.Des_Disponibilidade != null)
            {
                for (int i = 0; i < curriculo.Des_Disponibilidade.Count; i++)
                {
                    curriculoRetorno.DisponibilidadesTrabalho.Add(new DTO.DisponibilidadeTrabalho
                    {
                        Descricao = curriculo.Des_Disponibilidade[i]
                    });
                }
            }

            curriculoRetorno.Veiculos = new List<Veiculo>();
            if (curriculo.Des_Tipo_Veiculo != null)
            {
                for (int i = 0; i < curriculo.Des_Tipo_Veiculo.Count; i++)
                {
                    curriculoRetorno.Veiculos.Add(new DTO.Veiculo
                    {
                        Modelo = curriculo.Des_Modelo != null && curriculo.Des_Modelo.Count > i ? curriculo.Des_Modelo[i] : string.Empty,
                        Tipo = curriculo.Des_Tipo_Veiculo[i],
                        Ano = curriculo.Ano_Veiculo != null && curriculo.Ano_Veiculo.Count > i ? curriculo.Ano_Veiculo[i] : string.Empty
                    });
                }
            }

            return curriculoRetorno;
        }

        #region ExisteSemLocalizacao
        public static bool ExisteSemLocalizacao()
        {
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpquantidadesemLocalizacao, null)) > 0;
        }
        #endregion

        #region ListarSemLocalizacao
        /// <summary>
        /// M?todo utilizado por retornar um DataTable com todos os curr?culos sem localiza??o
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarSemLocalizacao(int limite, DateTime? dataAtualizacao)
        {
            object data = DBNull.Value;
            if (dataAtualizacao.HasValue)
                data = dataAtualizacao.Value;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Limite", SqlDbType = SqlDbType.Int, Size = 4, Value = limite },
                    new SqlParameter { ParameterName = "@Dta_Atualizacao", SqlDbType = SqlDbType.DateTime, Value = data }
                };

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SprecuperarsemLocalizacao, parms).Tables[0];
        }
        #endregion

        #region AlterarLocalizacao
        /// <summary>
        /// Altera a localizacao
        /// </summary>
        /// <returns></returns>
        public void AlterarLocalizacao(SqlGeography localizacao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = IdCurriculo } ,
                    new SqlParameter { ParameterName = "@Des_Localizacao", Size = 4, Value = localizacao, UdtTypeName = "Geography" }
                };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpAtualizarLocalizacao, parms);

            BufferAtualizacaoCurriculo.Update(this);
        }
        #endregion

        #region RecuperarHTMLCurriculo

        public string RecuperarHTMLCurriculo(bool mostrarFoto, bool mostrarDados, FormatoVisualizacaoImpressao formatoVisualizacao)
        {
            return MontarHTMLCurriculo(null, null, null, mostrarFoto, mostrarDados, false, false, false, formatoVisualizacao, null, null);
        }
        public string RecuperarHTMLCurriculo(bool mostrarFoto, bool mostrarDados, bool adicionarLinkBotaoDadosCompletos, FormatoVisualizacaoImpressao formatoVisualizacao)
        {
            return MontarHTMLCurriculo(null, null, null, mostrarFoto, mostrarDados, false, false, adicionarLinkBotaoDadosCompletos, formatoVisualizacao, null, null);
        }

        public string RecuperarHTMLCurriculo(int? idFilial, int? idUsuarioFilialPerfil, bool mostrarFoto, bool mostrarDados, bool mostrarLogoCliente, bool mostrarObservacoes, FormatoVisualizacaoImpressao formatoVisualizacao)
        {
            return MontarHTMLCurriculo(idFilial, idUsuarioFilialPerfil, null, mostrarFoto, mostrarDados, mostrarLogoCliente, mostrarObservacoes, false, formatoVisualizacao, null, null);
        }

        public string RecuperarHTMLCurriculo(int? idFilial, int? idUsuarioFilialPerfil, int? idVaga, bool mostrarFoto, bool mostrarDados, bool mostrarLogoCliente, bool mostrarObservacoes, FormatoVisualizacaoImpressao formatoVisualizacao, bool possuiPlano, decimal? cpf, DateTime? dtaNascimento)
        {
            if (possuiPlano)
            {
                return MontarHTMLCurriculo(idFilial, idUsuarioFilialPerfil, idVaga, mostrarFoto, mostrarDados, mostrarLogoCliente, mostrarObservacoes, false, formatoVisualizacao, cpf, dtaNascimento);
            }
            return MontarHTMLCurriculo(idFilial, idUsuarioFilialPerfil, idVaga, mostrarFoto, false, mostrarLogoCliente, mostrarObservacoes, true, formatoVisualizacao, cpf, dtaNascimento);
        }

        private string MontarHTMLCurriculo(int? idFilial, int? idUsuarioFilialPerfil, int? idVaga, bool mostrarFoto, bool mostrarDados, bool mostrarLogoCliente, bool mostrarObservacoes, bool adicionarLinkBotaoDadosCompletos, FormatoVisualizacaoImpressao formatoVisualizacao, decimal? cpf, DateTime? dtaNascimento)
        {
            DTO.Curriculo curriculo = CarregarCurriculoDTO(_idCurriculo, DadosCurriculo.Tudo);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div style='width: 100%;'>");
            stringBuilder.Append("<div style='width: 900px; margin: 0 auto;'>");
            stringBuilder.Append("<table style='font-size: 15px;' cellpadding='0' cellspacing=0'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td width='760px'>");
            stringBuilder.Append("<div style='display: block; font-weight: bold; font-size: 24px; margin: 15px 0 15px 25px; width: 400px;'>");
            stringBuilder.Append(mostrarDados ? curriculo.NomeCompleto : curriculo.PrimeiroNome);
            stringBuilder.Append("</div>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td>");
            stringBuilder.Append("<div>");
            if (mostrarLogoCliente && idFilial.HasValue)
            {
                string str1 = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/Handlers/PessoaJuridicaLogo.ashx?cnpj=", new Filial(idFilial.Value).RecuperarNumeroCNPJ(), "&Origem=Local", "&idFilial=" + idFilial.Value.ToString(), "&idCurriculo=" + curriculo.IdCurriculo.ToString());
                stringBuilder.AppendFormat("<img src='{0}' style='display:block; width:102px;heigth:87px;' />", str1);
            }
            else if (!mostrarLogoCliente && idFilial.HasValue)
            {
                string strQuemMeViu = $"http://{Helper.RecuperarURLAmbiente()}/Handlers/LogoBNERegistrarQuemMeViu.ashx?idFilial={(idFilial != null ? idFilial.Value.ToString() : "0")}&idCurriculo={curriculo.IdCurriculo}";
                stringBuilder.AppendFormat($"<img src='{strQuemMeViu}' style='display:none;' />");
            }
            stringBuilder.Append("</div>");
            stringBuilder.Append("</td>");
            if (mostrarFoto && !mostrarDados) //Se é para mostrar apenas a foto
            {
                stringBuilder.Append("<td width='95px'>");
                stringBuilder.AppendFormat("<img src='{0}' height='70' width='54' style='display:block;'/>", string.Format("http://{0}/Handlers/PessoaFisicaFoto.ashx?cpf={1}&origem=Local", Helper.RecuperarURLAmbiente(), curriculo.NumeroCPF.ToStringNullSafe()));
                stringBuilder.Append("</td>");
            }
            //stringBuilder.Append("<td width='100px'> <img src='http://mailing.bne.com.br/newsletter/padrao/images/logo.png' style='margin:0 auto !important;width:95%;height:auto;'/>");

            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            if (mostrarDados)
            {
                stringBuilder.Append("<div style='border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS;font-size: 15px; font-weight: bold; margin-left: 20px; width: 845px'>");
                stringBuilder.Append("Dados Pessoais");
                stringBuilder.Append("</div>");
                stringBuilder.Append("<table style='margin-left: 20px; width: 743px; float: left;font-size: 15px;' cellpadding='0' cellspacing=0'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 227px;'>");
                stringBuilder.Append("Sexo:");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td style='text-align: left; width:175px;'>");
                stringBuilder.Append(curriculo.Sexo);
                stringBuilder.Append("</td>");
                if (formatoVisualizacao.Equals(FormatoVisualizacaoImpressao.Empresa))
                {
                    //stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 50px;'>");
                    //stringBuilder.Append("CPF:");
                    //stringBuilder.Append("</td>");
                    //stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                    //stringBuilder.Append(Helper.FormatarCPF(curriculo.NumeroCPF));
                    //stringBuilder.Append("</td>");
                }
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 175px;'>");
                stringBuilder.Append("Estado Civil:");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                stringBuilder.Append(curriculo.EstadoCivil);
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 80px;'>");
                if (curriculo.TemFilhos.HasValue)
                    stringBuilder.Append("Filhos:");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                stringBuilder.Append(curriculo.TemFilhos.HasValue ? (curriculo.TemFilhos.Equals(true) ? "Sim" : "não") : string.Empty);
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 175px;'>");
                stringBuilder.Append("Data de Nascimento:");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                stringBuilder.Append(string.Concat(curriculo.DataNascimento.ToShortDateString(), " - ", curriculo.Idade, " Anos"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 50px;'>");
                if (!string.IsNullOrWhiteSpace(curriculo.CategoriaHabilitacao))
                    stringBuilder.Append("Habilitação:");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                stringBuilder.Append(curriculo.CategoriaHabilitacao);
                stringBuilder.Append("</td>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 175px;'>");
                stringBuilder.Append("Telefone Celular:");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                stringBuilder.Append(Helper.FormatarTelefone(curriculo.NumeroDDDCelular, curriculo.NumeroCelular));
                if (!string.IsNullOrEmpty(curriculo.NomeOperadoraCelular))
                {
                    string str2 = "http://" + Helper.RecuperarURLAmbiente() + curriculo.URLImagemOperadoraCelular;
                    stringBuilder.AppendFormat("<img src='{0}' alt='{1}' style='vertical-align: middle;' />", str2, curriculo.NomeOperadoraCelular);
                }
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                string str3 = Helper.FormatarTelefone(curriculo.NumeroDDDTelefone, curriculo.NumeroTelefone);
                if (!string.IsNullOrWhiteSpace(str3))
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 175px;'>");
                    stringBuilder.Append("Telefone Fixo Residencial:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                    stringBuilder.Append(str3);
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 50px;'>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("</tr>");
                }
                string str4 = Helper.FormatarTelefone(curriculo.NumeroDDDTelefoneRecado, curriculo.NumeroTelefoneRecado);
                if (!string.IsNullOrWhiteSpace(str4))
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 175px;'>");
                    stringBuilder.Append("Telefone Fixo Recado:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; '>");
                    stringBuilder.Append(str4);
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 50px;'>");
                    if (!string.IsNullOrWhiteSpace(curriculo.TelefoneRecadoContato))
                        stringBuilder.Append("Falar com:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                    stringBuilder.Append(curriculo.TelefoneRecadoContato);
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                }
                string str5 = Helper.FormatarTelefone(curriculo.NumeroDDDCelularRecado, curriculo.NumeroCelularRecado);
                if (!string.IsNullOrWhiteSpace(str5))
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 175px;'>");
                    stringBuilder.Append("Celular Recado:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                    stringBuilder.Append(str5);
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 50px;'>");
                    if (!string.IsNullOrWhiteSpace(curriculo.CelularRecadoContato))
                        stringBuilder.Append("Falar Com:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                    stringBuilder.Append(curriculo.CelularRecadoContato);
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 175px;'>");
                if (!string.IsNullOrWhiteSpace(curriculo.NumeroCEP))
                    stringBuilder.Append("CEP:");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                stringBuilder.Append(curriculo.NumeroCEP);
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 50px;'>");
                stringBuilder.Append("Cidade:");
                stringBuilder.Append(" </td>");
                stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                stringBuilder.Append(Helper.FormatarCidade(curriculo.NomeCidade, curriculo.SiglaEstado));
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 50px;'>");
                List<string> list = new List<string>();
                if (!string.IsNullOrWhiteSpace(curriculo.Logradouro))
                    list.Add(curriculo.Logradouro);
                if (!string.IsNullOrWhiteSpace(curriculo.NumeroEndereco))
                    list.Add(curriculo.NumeroEndereco);
                if (!string.IsNullOrWhiteSpace(curriculo.Complemento))
                    list.Add(curriculo.Complemento);
                if (!string.IsNullOrWhiteSpace(curriculo.Bairro))
                    list.Add(curriculo.Bairro);
                if (list.Count > 0)
                    stringBuilder.Append("Endereço:");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left;' colspan='3'>");
                stringBuilder.Append(string.Join(", ", list.Select(e => e.Trim())));
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                if (mostrarFoto)
                {
                    stringBuilder.Append("<table style='width: 100px; float: left;font-size: 15px;' cellpadding='0' cellspacing=0'>");
                    stringBuilder.Append("<tr>");
                    string str2 = "http://" + (object)Helper.RecuperarURLAmbiente() + "/Handlers/PessoaFisicaFoto.ashx?cpf=" + curriculo.NumeroCPF.ToStringNullSafe() + "&origem=Local";
                    stringBuilder.Append("<td width='95px' rowspan='7'>");
                    stringBuilder.AppendFormat("<img src='{0}' height='120' width='94' style='display:block;' style='margin-left:-14px' />", str2);
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("</table>");
                }
            }
            else if (adicionarLinkBotaoDadosCompletos)
            {
                stringBuilder.Append("<div style='border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 845px'>");
                stringBuilder.AppendFormat("<a href='{0}?visualizar=true'>Ver Dados Pessoais</a>", SitemapHelper.MontarUrlVisualizacaoCurriculo(curriculo.NomeFuncaoPretendida, curriculo.NomeCidade, curriculo.SiglaEstado, curriculo.IdCurriculo, cpf, dtaNascimento));
                stringBuilder.Append("</div>");
            }
            stringBuilder.Append("<div style='border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 845px; clear:both; margin-top:12px; padding-top:12px;'>");
            stringBuilder.Append("Pretensões");
            stringBuilder.Append("</div>");
            stringBuilder.Append("<table style='margin-left: 20px; width: 600px;font-size: 15px;' cellpadding='0' cellspacing=0'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td width='0' valign='top' style='text-align: left; font-weight: bold; width: 227px;'>");
            stringBuilder.Append("Funções Pretendidas:");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td width='0' valign='top' style='text-align: left; width: 200px'>");
            stringBuilder.Append(string.Join(" <br> ", curriculo.FuncoesPretendidas.Select(fp => fp.NomeFuncaoPretendida).ToArray()));
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 50px;'>");
            stringBuilder.Append("Salário:");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td valign='top' style='text-align: left;'>");
            stringBuilder.Append(curriculo.ValorPretensaoSalarial.HasValue ? "R$ " + curriculo.ValorPretensaoSalarial.Value.ToString("N2") : string.Empty);
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            if ((curriculo.Idiomas != null && curriculo.Idiomas.Count > 0) || (curriculo.Formacoes != null && curriculo.Formacoes.Count > 0) || (curriculo.Cursos != null && curriculo.Cursos.Count > 0))
            {
                stringBuilder.Append("<div style='border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 845px;clear:both; margin-top:12px; padding-top:12px;'>");
                stringBuilder.Append("Escolaridade");
                stringBuilder.Append("</div>");
                stringBuilder.Append("<table style='margin-left: 20px; width: 750px;font-size: 15px;' cellpadding='0' cellspacing=0'>");
                if (curriculo.Formacoes != null && curriculo.Formacoes.Count > 0)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td  valign='top' style='text-align: left; font-weight: bold; width:227px;'>");
                    stringBuilder.Append("Nível:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                    List<string> list1 = new List<string>();
                    foreach (DTO.Formacao formacao in curriculo.Formacoes)
                    {
                        List<string> list2 = new List<string>()
                            {
                                formacao.DescricaoFormacao,
                                formacao.DescricaoCurso,
                                formacao.SiglaFonte,
                                formacao.NomeFonte,
              formacao.AnoConclusao.HasValue ? formacao.AnoConclusao.ToString() : string.Empty,
                                formacao.SituacaoFormacao
                            };
                        if (!string.IsNullOrWhiteSpace(formacao.Periodo))
                            list2.Add(formacao.Periodo + "º Período");
                        list1.Add(string.Join(" - ", list2.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray()));
                    }
                    stringBuilder.Append(string.Join(" <br> ", list1.ToArray()));
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                }
                if (curriculo.Cursos != null && curriculo.Cursos.Count > 0)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td  valign='top' style='text-align: left; font-weight: bold; width:160px;'>");
                    stringBuilder.Append("Outros Cursos:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                    List<string> list1 = new List<string>();
                    foreach (DTO.Formacao formacao in curriculo.Cursos.OrderByDescending(c => c.AnoConclusao))
                    {
                        List<string> list2 = new List<string>()
                            {
                                formacao.DescricaoCurso,
                                formacao.SiglaFonte,
                                formacao.NomeFonte,
              formacao.AnoConclusao.HasValue ? formacao.AnoConclusao.ToString() : string.Empty,
                                formacao.SituacaoFormacao
                            };
                        if (!string.IsNullOrWhiteSpace(formacao.Periodo))
                            list2.Add(formacao.Periodo + "º Período");
                        list1.Add(string.Join(" - ", list2.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray()));
                    }
                    stringBuilder.Append(string.Join(" <br> ", list1.ToArray()));
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                }
                if (curriculo.Idiomas != null && curriculo.Idiomas.Count > 0)
                {
                    stringBuilder.Append("<tr class='linha_idiomas' id='trLinhaIdiomas' runat='server'>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width:160px;'>");
                    stringBuilder.Append("Idiomas:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                    stringBuilder.Append(string.Join(" <br> ", curriculo.Idiomas.Select(i => i.DescricaoIdioma + " - " + i.NivelIdioma)));
                    stringBuilder.Append("</td>");
                    //stringBuilder.Append("<td valign='top' style='text-align: right;'>");
                    //stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("</table>");
            }
            if (curriculo.Experiencias != null && curriculo.Experiencias.Count > 0)
            {
                stringBuilder.Append("<div style='border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; width: 845px;margin-left: 20px; margin-top:12px; padding-top:12px; '>");
                stringBuilder.Append("Experiência Profissional");
                stringBuilder.Append("</div>");
                stringBuilder.Append("<table style='margin-left: 20px; width: 750px;font-size: 15px;' cellpadding='0' cellspacing=0'>");
                int num = 0;
                foreach (DTO.ExperienciaProfissional experienciaProfissional in curriculo.Experiencias)
                {

                    string dataAdmissaoc = experienciaProfissional.DataAdmissao.ToString("dd/MM/yyyy HH:mm:ss");
                    string dataDemissao = experienciaProfissional.DataDemissao.HasValue
                        ? experienciaProfissional.DataDemissao.Value.ToString("dd/MM/yyyy HH:mm:ss") : string.Empty;

                    stringBuilder.Append("<tr>");

                    stringBuilder.Append("<td colspan='2' valign='top' style='text-align: left; width: 550px; font-style: italic; font-size: 15px; padding-top: 5px; font-weight: bold;'>");
                    stringBuilder.Append(experienciaProfissional.RazaoSocial + " - de " + experienciaProfissional.DataAdmissao.ToShortDateString() + " até " + (experienciaProfissional.DataDemissao.HasValue ? experienciaProfissional.DataDemissao.Value.ToShortDateString() : "hoje (Emprego Atual)") +
                         " (" + Helper.CalcularTempoEmprego(dataAdmissaoc, dataDemissao) + ")");

                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 227px; min-width:227px;'>");
                    stringBuilder.Append("Função Exercida:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                    stringBuilder.Append(experienciaProfissional.Funcao);
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width:0px'>");
                    stringBuilder.Append("Atribuições:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: justify; text-justify:inter-word;'>");
                    stringBuilder.Append(experienciaProfissional.DescricaoAtividade);
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                    if (num.Equals(0) && curriculo.ValorUltimoSalario.HasValue)
                    {
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 202px;'>");
                        stringBuilder.Append("Último Salário:");
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("<td valign='top' style='text-align: left;'>");
                        stringBuilder.Append("R$ " + curriculo.ValorUltimoSalario.Value.ToString("N2"));
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("</tr>");
                    }
                    ++num;
                }
                stringBuilder.Append("</table>");
            }


            stringBuilder.Append("<div style='border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 845px; margin-top:12px; padding-top:12px;'>");
            stringBuilder.Append("Observações");
            stringBuilder.Append("</div>");
            stringBuilder.Append("<table style='margin-left: 20px; width: 750px;font-size: 15px;' cellpadding='0' cellspacing=0'>");
            if (!string.IsNullOrWhiteSpace(curriculo.Observacao))
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width:227px; min-width:227px; max-width:227px;'>");
                stringBuilder.Append("Observações:");
                stringBuilder.Append("</td>");
                // stringBuilder.Append("<td width='0' valign='top'>");
                // stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align:justify;text-justify:inter-word;width: 600px'>");
                stringBuilder.Append(curriculo.Observacao.ReplaceBreaks());
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: right;'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: left;'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            if (!string.IsNullOrWhiteSpace(curriculo.OutrosConhecimentos))
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width:227px; min-width:227px; max-width:227px;'>");
                stringBuilder.Append("Outros Conhecimentos:");
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align:justify;text-justify:inter-word;width: 600px'>");
                stringBuilder.Append(curriculo.OutrosConhecimentos.ReplaceBreaks());
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: right;'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: left;'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            List<string> list3 = new List<string>();
            if (!string.IsNullOrWhiteSpace(curriculo.Raca))
                list3.Add(curriculo.Raca);
            if (curriculo.Altura.HasValue)
                list3.Add(curriculo.Altura.Value.ToString("N2"));
            if (curriculo.Peso.HasValue)
                list3.Add(curriculo.Peso.Value.ToString("N2"));
            if (list3.Count > 0)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width:227px; min-width:227px; max-width:227px;'>");
                stringBuilder.Append("Características Pessoais:");
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left; width: 600px'>");
                stringBuilder.Append(string.Join(" - ", list3.Select(c => c.Trim())));
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: right;'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: left;'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            if (curriculo.DisponibilidadesTrabalho != null && curriculo.DisponibilidadesTrabalho.Count > 0)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width:227px; min-width:227px; max-width:227px;'>");
                stringBuilder.Append("Disponibilidade para Trabalho:");
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left; width: 600px'>");
                stringBuilder.Append(string.Join(" ", curriculo.DisponibilidadesTrabalho.Select(x => x.Descricao.Trim())));
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: right;'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: left;'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            if (curriculo.DisponibilidadeViajar.HasValue)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width:227px; min-width:227px; max-width:227px;'>");
                stringBuilder.Append("Disponibilidade para Viagens:");
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left; width: 600px'>");
                stringBuilder.Append(curriculo.DisponibilidadeViajar.Value ? "Sim" : "não");
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: right;'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: left;'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            if (curriculo.DisponibilidadesMorarEm != null && curriculo.DisponibilidadesMorarEm.Count > 0)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width:227px; min-width:227px; max-width:227px;'>");
                stringBuilder.Append("Disponibilidade para Morar em:");
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left; width: 600px'>");
                stringBuilder.Append(string.Join(", ", curriculo.DisponibilidadesMorarEm.Select(disponibilidadeMorarEm => disponibilidadeMorarEm.Descricao.Trim())));
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: right;'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: left;'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            if (curriculo.Veiculos != null && curriculo.Veiculos.Count > 0)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width:227px; min-width:227px; max-width:227px;'>");
                stringBuilder.Append("Tipo de Veículo:");
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left; width: 600px'>");
                stringBuilder.Append(string.Join("<br>", curriculo.Veiculos.Select(veiculo => veiculo.Tipo + " - " + veiculo.Modelo + " - " + veiculo.Ano)));
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: right;'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: left;'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            if (!string.IsNullOrWhiteSpace(curriculo.Deficiencia))
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width:227px; min-width:227px; max-width:227px;'>");
                stringBuilder.Append("Tipo de Deficiência:");
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("<td valign='top' style='text-align: left; width: 600px'>");
                stringBuilder.Append(curriculo.Deficiencia);
                stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: right;'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0'>");
                //stringBuilder.Append("</td>");
                //stringBuilder.Append("<td width='0' valign='top' style='text-align: left;'>");
                //stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            if (mostrarObservacoes && idFilial.HasValue && idUsuarioFilialPerfil.HasValue)
            {
                int total;
                var objFilial = new Filial(idFilial.Value);
                var dt = CurriculoClassificacao.ListarObservacoes(objFilial, objFilial.EmpresaAssociacao() ? new UsuarioFilialPerfil(idUsuarioFilialPerfil.Value) : null, this, 0, Int32.MaxValue, out total);
                if (dt != null)
                {
                    stringBuilder.Append("<div style='border-bottom: 1px solid #999; display: block; font-family: Arial Unicode MS; font-size: 15px; font-weight: bold; margin-left: 20px; width: 845px;  margin-top:12px; padding-top:12px;'>");
                    stringBuilder.Append("Avaliação");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("<table style='margin-left: 20px; width: 600px;font-size: 15px;' cellpadding='0' cellspacing=0'>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; font-weight: bold; width: 150px;'>");
                    stringBuilder.Append("Comentário:");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td width='0' valign='top'>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td valign='top' style='text-align: left; width: 600px'>");
                    foreach (DataRow row in dt.Rows)
                    {
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td>");
                        stringBuilder.Append(row["Des_Observacao"]);
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("</tr>");
                    }
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td width='0' valign='top' style='text-align: right;'>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td width='0'>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td width='0' valign='top' style='text-align: left;'>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("</table>");
                }
            }
            stringBuilder.Append("</div>");
            stringBuilder.Append("</div>");

            stringBuilder.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/>");
            return stringBuilder.ToString();
        }

        #endregion
        #region [ Curriculo Compativel com Estagio ]
        public bool CurriculoCompativelComEstagio()
        {
            if (this.PessoaFisica == null)
            {
                if (this.IdCurriculo <= 0)
                    return false;

                return PessoaFisica.ConsideraEstudantePorCurriculoId(this.IdCurriculo);
            }

            return this.PessoaFisica.ConsideraEstudante();
        }
        #endregion

        #region RecuperarListaCvsEnvio
        public static List<DestinatarioSMS> RecuperarListaCvsEnvio(int idCidade, int idFuncao, bool estagio)
        {
            List<DestinatarioSMS> listaCurriculos = new List<DestinatarioSMS>();

            int totalRegistros = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QtdUsuariosSMSVagaPerfil));

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@idf_Cidade", SqlDbType = SqlDbType.Int, Value = idCidade},
                    new SqlParameter{ ParameterName = "@idf_Funcao", SqlDbType = SqlDbType.Int, Value = idFuncao},
                    new SqlParameter{ ParameterName = "@flg_estagio", SqlDbType = SqlDbType.Bit, Value = estagio },
                    new SqlParameter{ ParameterName = "@total", SqlDbType = SqlDbType.Int, Value = totalRegistros}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "bne.BNE_SP_Lista_Curriculo_Vaga", parms))
            {
                while (dr.Read())
                {
                    listaCurriculos.Add(new DestinatarioSMS
                    {
                        NomePessoa = Convert.ToString(dr["Nme_Pessoa"]),
                        DDDCelular = Convert.ToString(dr["Num_DDD_Celular"]),
                        NumeroCelular = Convert.ToString(dr["Num_Celular"]),
                        IdDestinatario = Convert.ToInt32(dr["idf_curriculo"]),
                        CPF = Convert.ToDecimal(dr["Num_CPF"]),
                        Email = Convert.ToString(dr["Eml_Pessoa"]),
                        DataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"])
                    });
                }
            };

            return listaCurriculos;

        }
        #endregion

        #region RetornarHashLogarCurriculo
        public static void RetornarHashLogarCurriculo(decimal numeroCPF, DateTime dataNascimento, out string listaVagas, out string salaVip, out string vip, out string quemMeViu, out string cadastroCurriculo, out string pesquisaVagas, out string loginCandidato, out string cadastroExperiencias)
        {
            var listaVagasAux = "/vagas-de-emprego";
            var salaVipAux = "/SalaVipEscolherEmpresa.aspx";
            var vipAux = "/vip";
            var quemMeViuAux = "/QuemMeViuTelaMagica.aspx";
            var cadastroCurriculoAux = "/cadastro-de-curriculo-gratis";
            var pesquisaVagasAux = "/pesquisa-de-vagas";
            var loginCandidatoAux = "/login-candidato";
            var cadastroExperienciasAux = "/CadastroCurriculoDados.aspx";

            listaVagas = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(numeroCPF, dataNascimento, listaVagasAux);
            salaVip = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(numeroCPF, dataNascimento, salaVipAux);
            vip = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(numeroCPF, dataNascimento, vipAux);
            quemMeViu = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(numeroCPF, dataNascimento, quemMeViuAux);
            cadastroCurriculo = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(numeroCPF, dataNascimento, cadastroCurriculoAux);
            pesquisaVagas = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(numeroCPF, dataNascimento, pesquisaVagasAux);
            loginCandidato = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(numeroCPF, dataNascimento, loginCandidatoAux);
            cadastroExperiencias = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(numeroCPF, dataNascimento, cadastroExperienciasAux);
        }
        #endregion

        #region RetornarHash
        public static string RetornarHash(int idCurriculo, string pagina)
        {
            string hash = string.Empty;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo},
                    new SqlParameter { ParameterName = "@pagina", SqlDbType = SqlDbType.VarChar, Value = pagina}
                };


            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarHashLogarCurriculo, parms))
            {
                if (dr.Read())
                    hash = dr["hash"].ToString();
            };

            return hash;
        }
        #endregion

        #region ListaCurriculoForaDaFaixaSalarial
        /// <summary>
        /// M�todo que retorna todos os curr�culos fora da Faixa Salarial e que não entrar na amostragem da Pesquisa
        /// </summary>
        /// <returns></returns>
        public static List<DTO.Curriculo> ListaFake()
        {
            const string select = @"
            SELECT TOP 100 Idf_Curriculo FROM bne.BNE_Curriculo C
            WHERE (ABS(CAST((BINARY_CHECKSUM(*) * RAND()) as int)) % 100) < 10
                AND C.Flg_Inativo = 0
                AND (	
				        C.Idf_Situacao_Curriculo <> 6 /* Bloqueado */
				           AND
				        C.Idf_Situacao_Curriculo <> 7 /* Cancelado */
				            AND
				        C.Idf_Situacao_Curriculo <> 8 /* Invis�vel */
                        AND
                        C.Idf_Situacao_Curriculo <> 12 /* Exclusão Lógica */
				     )
            ORDER BY Dta_Atualizacao DESC";

            var lista = new List<DTO.Curriculo>();
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, select, null))
            {
                while (dr.Read())
                    lista.Add(Curriculo.CarregarCurriculoDTO(Convert.ToInt32(dr["Idf_Curriculo"])));
            }
            return lista;
        }
        #endregion

        #region BloquearCVSine
        /// <summary>
        /// Inativa no sine os Curriculos Bronquinha no BNE
        /// </summary>
        /// <param name="objCurriculo"></param>
        /// <param name="motivo"></param>
        public static void InativarCVSine(Curriculo objCurriculo, string motivo)
        {
            new System.Threading.Thread(() =>
            {
                try
                {
                    using (wsSine.AppClient cliente = new wsSine.AppClient())
                    {
                        objCurriculo.PessoaFisica.CompleteObject();
                        cliente.InativarCV(objCurriculo.PessoaFisica.CPF, motivo);
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    throw;
                }
            }).Start();


        }
        #endregion

        #region ExcluirCvSolr
        /// <summary>
        /// Excluir Cv so solr
        /// </summary>
        /// <param name="Idf_Curriculo"></param>
        /// <param name="email"></param>
        public static void ExcluirCVSolr(int Idf_Curriculo, string email)
        {
            string urlSLOR = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrExcluirCV);
            urlSLOR = urlSLOR.ToString().Replace("{1}", Idf_Curriculo.ToString());

            if (!string.IsNullOrEmpty(urlSLOR))
            {
                var request = WebRequest.Create(urlSLOR);
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    response.Close();
                }

            }

            ExcluirCurriculoQueroVagas(Idf_Curriculo, email);

        }

        #endregion


        #region ExcluirCurriculoQueroVagas
        private static void ExcluirCurriculoQueroVagas(int idCurriculo, string email)
        {
            try
            {
                DomainEventsHandler.Handle(new OnRemoverCurriculo(idCurriculo, email));
            }
            catch (Exception e)
            {
                EL.GerenciadorException.GravarExcecao(e, "Erro lançar evento exclusão curriculo");
            }
        }

        #endregion

        public void CompletarCurriculoIntegracao(Sine.Integracao.Model.Curriculo objCurriculoSine)
        {
            PessoaFisica.CompleteObject();

            objCurriculoSine.Celular = new Sine.Integracao.Model.Telefone()
            {
                DDD = Convert.ToInt32(PessoaFisica.NumeroDDDCelular),
                Numero = PessoaFisica.NumeroCelular
            };

            PessoaFisica.Endereco.CompleteObject();
            PessoaFisica.Endereco.Cidade.CompleteObject();
            //PessoaFisica.Endereco.Cidade.Estado.CompleteObject();
            objCurriculoSine.Cidade = new Sine.Integracao.Model.Cidade()
            {
                Nome = PessoaFisica.Endereco.Cidade.NomeCidade,
                Estado = PessoaFisica.Endereco.Cidade.Estado.SiglaEstado
            };

            objCurriculoSine.Cpf = PessoaFisica.CPF.ToString();
            objCurriculoSine.DataAtualizacao = objCurriculoSine.DataAtualizacao;
            objCurriculoSine.DataNascimento = PessoaFisica.DataNascimento;
            objCurriculoSine.Email = PessoaFisica.EmailPessoa;
            objCurriculoSine.Nome = PessoaFisica.NomeCompleto;
            if (this.ValorPretensaoSalarial.HasValue)
                objCurriculoSine.Pretensao = (double?)this.ValorPretensaoSalarial.Value;

            if (PessoaFisica.Sexo != null)
                objCurriculoSine.Sexo = PessoaFisica.Sexo.IdSexo == 1 ? "M" : "F";

            List<FuncaoPretendida> funcoesPretendidas = BLL.FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(this);
            if (funcoesPretendidas.Count > 0)
            {
                FuncaoPretendida objFuncaoPretendida = funcoesPretendidas.Where(f => f.Funcao != null).OrderBy(f => f.DataCadastro).FirstOrDefault();
                if (objFuncaoPretendida != null)
                {
                    objFuncaoPretendida.Funcao.CompleteObject();
                    objCurriculoSine.FuncaoPretendida = objFuncaoPretendida.Funcao.DescricaoFuncao;
                }
                else
                {
                    objFuncaoPretendida = funcoesPretendidas.Where(f => f.Funcao == null).OrderBy(f => f.DataCadastro).FirstOrDefault();
                    if (objFuncaoPretendida != null)
                    {
                        objCurriculoSine.FuncaoPretendida = objFuncaoPretendida.DescricaoFuncaoPretendida;
                    }
                }
            }

            //Listando cursos
            objCurriculoSine.Cursos = new List<Sine.Integracao.Model.Curso>();
            foreach (Formacao formacao in Formacao.ListarFormacaoList(PessoaFisica.IdPessoaFisica, false))
            {
                Sine.Integracao.Model.Curso c = new Sine.Integracao.Model.Curso();
                c.AnoConclusao = Convert.ToInt32(formacao.AnoConclusao);
                c.Cidade = new Sine.Integracao.Model.Cidade();
                if (formacao.Cidade != null)
                {
                    formacao.Cidade.CompleteObject();
                    formacao.Cidade.Estado.CompleteObject();
                    c.Cidade.Nome = formacao.Cidade.NomeCidade;
                    c.Cidade.Estado = formacao.Cidade.Estado.SiglaEstado;
                }
                c.Instituicao = formacao.DescricaoFonte;

                if (formacao.Escolaridade != null)
                {
                    formacao.Escolaridade.CompleteObject();
                    c.Nivel = formacao.Escolaridade.DescricaoBNE;
                }

                c.NomeCurso = formacao.DescricaoCurso;
                c.Periodo = Convert.ToInt32(formacao.NumeroPeriodo);
                if (formacao.SituacaoFormacao != null)
                {
                    formacao.SituacaoFormacao.CompleteObject();
                    c.Situacao = formacao.SituacaoFormacao.DescricaoSituacaoFormacao;
                }
                objCurriculoSine.Cursos.Add(c);
            }

            //Listando Experiencias
            //Experiencia
            List<int> listExp = PessoaFisica.RecuperarExperienciaProfissional(null);
            if (listExp.Count > 0)
            {
                objCurriculoSine.Experiencias = new List<Sine.Integracao.Model.Experiencia>();
            }
            if (listExp.Count >= 1)
            {
                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(listExp[0]);
                objCurriculoSine.Experiencias.Add(new Sine.Integracao.Model.Experiencia());
                objCurriculoSine.Experiencias[0].Atribuicoes = objExperienciaProfissional.DescricaoAtividade;
                objCurriculoSine.Experiencias[0].DataAdmissao = objExperienciaProfissional.DataAdmissao;
                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    objCurriculoSine.Experiencias[0].DataDemissao = objExperienciaProfissional.DataDemissao.Value;
                }
                objCurriculoSine.Experiencias[0].Empresa = new Sine.Integracao.Model.Empresa();
                objCurriculoSine.Experiencias[0].Empresa.Nome = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                {
                    objExperienciaProfissional.AreaBNE.CompleteObject();
                    objCurriculoSine.Experiencias[0].Empresa.Ramo = objExperienciaProfissional.AreaBNE.DescricaoAreaBNE;
                }
                objCurriculoSine.Experiencias[0].FuncaoExercida = objExperienciaProfissional.DescricaoFuncaoExercida;
                if (this.ValorUltimoSalario.HasValue)
                {
                    objCurriculoSine.Experiencias[0].Salario = (double?)this.ValorUltimoSalario;
                }
            }

            if (listExp.Count >= 2)
            {
                //preenche campos segunda experiencia
                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(listExp[1]);
                objCurriculoSine.Experiencias.Add(new Sine.Integracao.Model.Experiencia());
                objCurriculoSine.Experiencias[1].Atribuicoes = objExperienciaProfissional.DescricaoAtividade;
                objCurriculoSine.Experiencias[1].DataAdmissao = objExperienciaProfissional.DataAdmissao;
                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    objCurriculoSine.Experiencias[1].DataDemissao = objExperienciaProfissional.DataDemissao.Value;
                }
                objCurriculoSine.Experiencias[1].Empresa = new Sine.Integracao.Model.Empresa();
                objCurriculoSine.Experiencias[1].Empresa.Nome = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                {
                    objExperienciaProfissional.AreaBNE.CompleteObject();
                    objCurriculoSine.Experiencias[1].Empresa.Ramo = objExperienciaProfissional.AreaBNE.DescricaoAreaBNE;
                }
                objCurriculoSine.Experiencias[1].FuncaoExercida = objExperienciaProfissional.DescricaoFuncaoExercida;
            }

            if (listExp.Count >= 3)
            {
                //preenche campos terceira experiencia
                ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(listExp[2]);
                objCurriculoSine.Experiencias.Add(new Sine.Integracao.Model.Experiencia());
                objCurriculoSine.Experiencias[2].Atribuicoes = objExperienciaProfissional.DescricaoAtividade;
                objCurriculoSine.Experiencias[2].DataAdmissao = objExperienciaProfissional.DataAdmissao;
                if (objExperienciaProfissional.DataDemissao.HasValue)
                {
                    objCurriculoSine.Experiencias[2].DataDemissao = objExperienciaProfissional.DataDemissao.Value;
                }
                objCurriculoSine.Experiencias[2].Empresa = new Sine.Integracao.Model.Empresa();
                objCurriculoSine.Experiencias[2].Empresa.Nome = objExperienciaProfissional.RazaoSocial;
                if (objExperienciaProfissional.AreaBNE != null)
                {
                    objExperienciaProfissional.AreaBNE.CompleteObject();
                    objCurriculoSine.Experiencias[2].Empresa.Ramo = objExperienciaProfissional.AreaBNE.DescricaoAreaBNE;
                }
                objCurriculoSine.Experiencias[2].FuncaoExercida = objExperienciaProfissional.DescricaoFuncaoExercida;
            }
        }

        #endregion

        #region OnSalvarCurriculo
        /// <summary>
        ///     Adiciona ou remove cidades e funcoes a um curriculo
        /// </summary>
        /// <param name="objCurriculo">o Curriculo que está selecionado</param>
        /// <remarks>Luan Fernandes</remarks>
        public static void OnAlterarCurriculo(Curriculo objCurriculo)
        {
            if (objCurriculo != null && objCurriculo.IdCurriculo > 0)
            {
                #region CarregaAlertaCurriculo
                AlertaCurriculos objAlerta;
                try
                {
                    objAlerta = AlertaCurriculos.LoadObject(objCurriculo.IdCurriculo);
                }
                catch (RecordNotFoundException)
                {
                    objAlerta = null;
                }
                #endregion

                #region ManipulacaoRegistro

                //Se o Curriculo está Inativo
                //Ou a Situação do Currículo é diferente de:
                // 1  Publicado
                // 2  Aguardando Publicação
                // 3  Crítica
                // 4  Aguardando Revisão VIP
                // 9  Revisado VIP
                // 10 Auditado

                //E se Existe Alerta, Deleta o Alerta e Registros filhos

                if (objCurriculo.FlagInativo ||
                    (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 1
                     && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 2
                     && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 3
                     && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 4
                     && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 9
                     && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 10)
                    )
                {
                    if (objAlerta != null)
                        Delete(objCurriculo.IdCurriculo);
                }
                else
                {
                    if (objCurriculo.PessoaFisica.CompleteObject())
                    {
                        #region AlertaCurriculo

                        //Se o AlertaCurriculo não existe, cria um novo
                        //Senão atualiza o Email, Nome e VIP

                        if (objAlerta == null)
                        {
                            objAlerta = new AlertaCurriculos();
                            objAlerta.EmailPessoa = (objCurriculo.PessoaFisica.EmailPessoa == null) ? "" : objCurriculo.PessoaFisica.EmailPessoa;
                            objAlerta.IdCurriculo = objCurriculo.IdCurriculo;
                            objAlerta.NomePessoa = (objCurriculo.PessoaFisica.NomePessoa == null) ? "" : objCurriculo.PessoaFisica.NomePessoa;
                            objAlerta.FlagVIP = objCurriculo.FlagVIP;
                            objAlerta.Save();
                        }
                        else
                        {
                            objAlerta.EmailPessoa = (objCurriculo.PessoaFisica.EmailPessoa == null) ? "" : objCurriculo.PessoaFisica.EmailPessoa;
                            objAlerta.NomePessoa = (objCurriculo.PessoaFisica.NomePessoa == null) ? "" : objCurriculo.PessoaFisica.NomePessoa;
                            objAlerta.FlagVIP = objCurriculo.FlagVIP;
                            objAlerta.Save();
                        }
                        #endregion

                        #region AdicionaCidades

                        #region Listas

                        //Lista de Cidades em Alerta
                        var listAlertaCidades = new List<AlertaCidades>();

                        //Lista de Cidades do Candidato
                        var listaCidadesDefault = new List<Cidade>();
                        #endregion

                        #region AdicionaCidadesDefaultCurriculo
                        objCurriculo.PessoaFisica.Cidade.CompleteObject();

                        //Cidade Default do Curriculo, adiciona na lista de Cidades Default do Curriculo
                        if (objCurriculo.PessoaFisica.Cidade != null)
                            listaCidadesDefault.Add(objCurriculo.PessoaFisica.Cidade);

                        //Se a CidadeEndereco for diferente da Cidade, adiciona na lista de Cidades Default do Curriculo
                        if (objCurriculo.CidadeEndereco != null)
                        {
                            objCurriculo.CidadeEndereco.CompleteObject();
                            if (objCurriculo.CidadeEndereco.IdCidade != objCurriculo.PessoaFisica.Cidade.IdCidade)
                                listaCidadesDefault.Add(objCurriculo.CidadeEndereco);
                        }

                        //Se CidadePretendida for diferente da CidadeEndereço e Cidade, adiciona na lista de Cidades Default do Curriculo
                        if (objCurriculo.CidadePretendida != null)
                        {
                            objCurriculo.CidadePretendida.CompleteObject();

                            if (objCurriculo.CidadePretendida.IdCidade != objCurriculo.CidadeEndereco.IdCidade
                                && objCurriculo.CidadePretendida.IdCidade != objCurriculo.PessoaFisica.Cidade.IdCidade)
                                listaCidadesDefault.Add(objCurriculo.CidadePretendida);
                        }

                        //Verifica as cidades que o Candidato tem disponibilidade para trabalhar
                        var dt = CurriculoDisponibilidadeCidade.ListarCidadesPorCurriculo(objCurriculo.IdCurriculo);

                        //Adiciona na lista de Cidades Default do Curriculo
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                Cidade city;
                                if (Cidade.CarregarPorNome(row["Nme_Cidade"].ToString(), out city))
                                    if (city != null)
                                        if (!listaCidadesDefault.Contains(city))
                                            listaCidadesDefault.Add(city);
                            }
                        }
                        #endregion

                        #region AdicionaAlertaCidade
                        //Para cada Cidade Default, veirifica se existe Alerta
                        //Não Existindo, Cria o alerta

                        AlertaCidades alertaCidade;

                        foreach (var cidade in listaCidadesDefault)
                        {
                            try
                            {
                                alertaCidade = AlertaCidades.LoadObject(objCurriculo.IdCurriculo, cidade.IdCidade);
                            }
                            catch (RecordNotFoundException)
                            {
                                alertaCidade = null;
                            }

                            if (alertaCidade == null)
                            {
                                alertaCidade = new AlertaCidades();
                                alertaCidade.IdCidade = cidade.IdCidade;
                                alertaCidade.AlertaCurriculos = objAlerta;
                                alertaCidade.FlagInativo = false;
                                alertaCidade.NomeCidade = cidade.NomeCidade;
                                cidade.Estado.CompleteObject();
                                alertaCidade.SiglaEstado = cidade.Estado.SiglaEstado;

                                if (!listAlertaCidades.Contains(alertaCidade))
                                    listAlertaCidades.Add(alertaCidade);
                            }
                        }

                        if (listAlertaCidades.Count > 0)
                        {
                            AlertaCidades.SalvarListaAlertaCidadesCurriculo(listAlertaCidades);
                        }
                        #endregion

                        #endregion

                        #region AdicionaFuncao_e_Similares

                        #region Listas

                        //Lista para adicionar as Funções em Alerta
                        var listAlertaFuncoes = new List<AlertaFuncoes>();

                        //Lista com as FunçõesPretendidas pelo Candidato
                        var listFuncoesPretendidas = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);

                        //Lista que traz as Funções Similares de cada FunçãoPretendida do candidato
                        var listFuncoesSimilares = new List<Funcao>();
                        #endregion

                        #region AdicionaFuncoesDefaultCurriculo
                        AlertaFuncoes funcao;

                        //Para cada FuncaoPretendida do candidato, verifica se já existe alerta
                        //para a funcao, senão exisite, cria o alerta
                        //Verifica as funcoes similares e senão existir alerta, cria
                        foreach (var funcaoPretendida in listFuncoesPretendidas)
                        {
                            #region CarregaAlertaFuncao
                            try
                            {
                                if (funcaoPretendida.Funcao != null)
                                    funcao = AlertaFuncoes.LoadObject(funcaoPretendida.Funcao.IdFuncao, objCurriculo.IdCurriculo);
                                else
                                    funcao = null;
                            }
                            catch (RecordNotFoundException)
                            {
                                funcao = null;
                            }
                            #endregion

                            #region Se NãoExisteAlerta
                            //Não Existe Alerta para a Funcao Selecionada
                            if (funcao == null)
                            {
                                if (funcaoPretendida.Funcao != null)
                                {
                                    funcaoPretendida.Funcao.CompleteObject();

                                    funcao = new AlertaFuncoes();
                                    funcao.AlertaCurriculos = objAlerta;
                                    funcao.DescricaoFuncao = (funcaoPretendida.Funcao.DescricaoFuncao == null) ? "" : funcaoPretendida.Funcao.DescricaoFuncao;
                                    funcao.FlagInativo = false;
                                    funcao.FlagSimilar = false;
                                    funcao.IdFuncao = funcaoPretendida.Funcao.IdFuncao;

                                    if (!listAlertaFuncoes.Contains(funcao))
                                        listAlertaFuncoes.Add(funcao);
                                }
                            }
                            #endregion

                            #region VerificaSimilares
                            if (funcaoPretendida.Funcao != null)
                            {
                                listFuncoesSimilares = FuncaoPretendida.CarregarFuncoesSimilaresPorFuncao(funcaoPretendida.Funcao.IdFuncao);

                                if (listFuncoesSimilares.Count > 0)
                                {
                                    foreach (var funcaoSimilar in listFuncoesSimilares)
                                    {
                                        try
                                        {
                                            funcao = AlertaFuncoes.LoadObject(funcaoSimilar.IdFuncao, objCurriculo.IdCurriculo);
                                        }
                                        catch (RecordNotFoundException)
                                        {
                                            funcao = null;
                                        }

                                        if (funcao == null)
                                        {
                                            funcaoSimilar.CompleteObject();
                                            funcao = new AlertaFuncoes();
                                            funcao.AlertaCurriculos = objAlerta;
                                            funcao.DescricaoFuncao = (funcaoSimilar.DescricaoFuncao == null) ? "" : funcaoSimilar.DescricaoFuncao;
                                            funcao.FlagInativo = false;
                                            funcao.FlagSimilar = true;
                                            funcao.IdFuncao = funcaoSimilar.IdFuncao;

                                            if (!listAlertaFuncoes.Contains(funcao))
                                                listAlertaFuncoes.Add(funcao);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        //Se adicionou alguma alerta para funcao
                        if (listAlertaFuncoes.Count > 0)
                        {
                            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
                            {
                                conn.Open();
                                using (SqlTransaction trans = conn.BeginTransaction())
                                {
                                    try
                                    {
                                        AlertaFuncoes.SalvarListaAlertaFuncoesCurriculo(listAlertaFuncoes, trans);
                                        trans.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        GerenciadorException.GravarExcecao(ex, "Erro ao cadastrar alertas -> " + objCurriculo.PessoaFisica.NumeroCPF);
                                        trans.Rollback();
                                    }
                                }
                            }

                        }
                        #endregion

                        #endregion
                    }
                }
                #endregion
            }
        }
        #endregion


        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public int MigrationId
        {
            set
            {
                this._idCurriculo = value;
            }
            get { return this._idCurriculo; }
        }

        private List<CurriculoDisponibilidade> _migrationDisponibilidades;
        public List<CurriculoDisponibilidade> MigrationDisponibilidades
        {
            set
            {
                this._migrationDisponibilidades = value;
            }
            get { return this._migrationDisponibilidades; }
        }


        private List<Formacao> _migrationFormacoes;
        public List<Formacao> MigrationFormacoes
        {
            set { this._migrationFormacoes = value; }
            get { return this._migrationFormacoes; }
        }


        private List<Funcao> _migrationFuncoesPretendidas;
        public List<Funcao> MigrationFuncoesPretendidas
        {
            set { this._migrationFuncoesPretendidas = value; }
            get { return this._migrationFuncoesPretendidas; }
        }


        private List<PessoafisicaIdioma> _migrationIdiomas;
        public List<PessoafisicaIdioma> MigrationIdiomas
        {
            set { this._migrationIdiomas = value; }
            get { return this._migrationIdiomas; }
        }

        private List<PessoaFisicaVeiculo> _migrationVeiculos;
        public List<PessoaFisicaVeiculo> MigrationVeiculos
        {
            set { this._migrationVeiculos = value; }
            get { return this._migrationVeiculos; }
        }
        #endregion

        public void Denunciar(string descricao, UsuarioFilialPerfil objUsuarioFilialPerfil)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var objCurriculoCorrecao = new CurriculoCorrecao
                        {
                            DescricaoCorrecao = descricao,
                            UsuarioFilialPerfil = objUsuarioFilialPerfil,
                            Curriculo = this
                        };
                        objCurriculoCorrecao.Save(trans);

                        var objCurriculoDenuncia = new CurriculoDenuncia
                        {
                            CurriculoCorrecao = objCurriculoCorrecao
                        };

                        if (objUsuarioFilialPerfil.Filial.PossuiPlanoAtivo(trans) && CurriculoVisualizacao.VerificarVisualizacaoCV(this, objUsuarioFilialPerfil.Filial, trans))
                        {
                            PlanoAdquirido objPlanoAdquirido;
                            if (PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(objUsuarioFilialPerfil.Filial, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido, trans))
                            {
                                if (!CurriculoDenuncia.JaPossuiDenuncia(this, objPlanoAdquirido, trans))
                                {
                                    objPlanoAdquirido.IncrementarSaldoVisualizacao(trans);
                                    objCurriculoDenuncia.PlanoAdquirido = objPlanoAdquirido;
                                }
                            }
                        }

                        objCurriculoDenuncia.Save(trans);

                        trans.Commit();

                        try
                        {
                            string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ContaPadraoEnvioEmail);
                            string emailDestinatario = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailAlertarBNE);

                            var mensagem = new StringBuilder();
                            mensagem.AppendFormat("Código do Currículo: {0}", objCurriculoCorrecao.Curriculo.IdCurriculo);
                            mensagem.AppendLine();
                            mensagem.AppendFormat("Nome do Usuário Logado: {0}", objUsuarioFilialPerfil.PessoaFisica.NomeCompleto);
                            mensagem.AppendLine();
                            mensagem.AppendFormat("Nome da Empresa: {0}", objUsuarioFilialPerfil.Filial.RazaoSocial);
                            mensagem.AppendLine();
                            mensagem.Append($"Descrição: {descricao}");

                            EmailSenderFactory
                                .Create(TipoEnviadorEmail.Fila)
                                .Enviar("Alertar BNE", mensagem.ToString(), null, emailRemetente, emailDestinatario);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            EL.GerenciadorException.GravarExcecao(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw ex;
                    }
                }
            }

        }

        public static IEnumerable<int> RecuperarIdsParaDW()
        {
            const string sp = @"SELECT  C.Idf_Curriculo
            FROM BNE_IMP.bne.BNE_Curriculo C WITH(NOLOCK)
            INNER JOIN BNE_IMP.BNE.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
            LEFT JOIN DW.BNE_Curriculo DW WITH(NOLOCK) ON DW.Idf_Curriculo = C.Idf_Curriculo
            WHERE C.Dta_Atualizacao < '2018-02-10 00:00:00.000'
            AND c.Dta_Atualizacao > Dw.Dta_Atualizacao
            AND c.Dta_Atualizacao > '2018-01-01 00:00:00.000'
            ORDER BY C.Dta_Atualizacao DESC";

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, sp, null))
            {
                while (dr.Read())
                {
                    yield return Convert.ToInt32(dr["Idf_Curriculo"]);
                }
            }
        }
    }
}
