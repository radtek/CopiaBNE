//-- Data: 10/08/2010 15:09
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.DTO;
using BNE.Solr;
using FormatObject = BNE.BLL.Common.FormatObject;
using System.Web.UI.WebControls;

namespace BNE.BLL
{
    public partial class PesquisaCurriculo // Tabela: TAB_Pesquisa_Curriculo
    {
        #region Consultas

        private const string Spatualizarquantidadecurriculosretornados = @"
        UPDATE TAB_Pesquisa_Curriculo SET Qtd_Curriculo_Retorno = @Qtd_Curriculo_Retorno WHERE Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo
        ";

        #region [spQuantidadePesquisaRealizadas]
        private const string spQuantidadePesquisaRealizadas = @" select count(idf_pesquisa_curriculo) as Total from tab_pesquisa_curriculo pc with(nolock)
                             join tab_usuario_filial_perfil ufp with(nolock) on ufp.idf_usuario_filial_perfil = pc.idf_usuario_filial_perfil
                             where ufp.idf_filial = @Idf_Filial ";
        #endregion

        #region [spQuantidadeCurriculosNaoVisualizados]
        private const string spQuantidadeCurriculosNaoVisualizados = @" SELECT TOP 1
                    qtd ,
                    pesq.idf_pesquisa_curriculo
	                FROM  BNE.TAB_Pesquisa_Curriculo pesq WITH ( NOLOCK )    
                    JOIN BNE.TAB_Usuario_Filial_Perfil ufp WITH ( NOLOCK ) ON pesq.Idf_Usuario_Filial_Perfil = ufp.Idf_Usuario_Filial_Perfil
	                join bne.tab_pesquisa_curriculo_funcao pcf witH(nolock) on pesq.idf_pesquisa_curriculo = pcf.idf_pesquisa_curriculo
                    CROSS APPLY ( SELECT  COUNT(*) qtd
                            FROM    BNE.TAB_Pesquisa_Curriculo_Curriculos cvs WITH ( NOLOCK )
                                LEFT JOIN BNE.BNE_Curriculo_Quem_Me_Viu qmv WITH ( NOLOCK ) ON cvs.Idf_Curriculo = qmv.Idf_Curriculo  AND qmv.Idf_Filial = @Idf_Filial
                           WHERE    pesq.Idf_Pesquisa_Curriculo = cvs.Idf_Pesquisa_Curriculo
                                AND qmv.Idf_Curriculo_Quem_Me_Viu IS NULL
                          ) cvs
                WHERE  ufp.Idf_Filial = @Idf_Filial
                and pesq.idf_cidade is not null
                    AND qtd > 1
                ORDER BY pesq.Dta_Cadastro DESC, pcf.idf_pesquisa_curriculo_funcao asc";
        #endregion

        #region [spCompletarObjetoPesquisa]
        private const string spCompletarObjetoPesquisa = @" select  tv.des_tipo_veiculo, r.des_raca, d.des_deficiencia, ch.des_categoria_habilitacao, ab.des_area_bne,
                             ec.des_estado_civil, es.des_bne, curso.des_curso, fonte.nme_fonte
   from bne.tab_pesquisa_curriculo pc with(nolock)
 left join plataforma.tab_tipo_veiculo tv with(nolock) on tv.idf_tipo_veiculo = pc.idf_tipo_veiculo
 left join plataforma.tab_raca r with(nolock) on r.idf_raca = pc.idf_raca
 left join plataforma.tab_deficiencia d with(nolock) on d.idf_deficiencia = pc.idf_deficiencia
 left join plataforma.tab_categoria_habilitacao ch with(nolock) on ch.idf_categoria_habilitacao = pc.idf_categoria_habilitacao
 left join plataforma.tab_area_bne ab with(nolock) on ab.idf_area_bne = pc.idf_area_bne
 left join plataforma.tab_estado_civil ec with(nolock) on ec.idf_estado_civil = pc.idf_estado_civil
 left join plataforma.tab_escolaridade es with(nolock) on es.idf_escolaridade = pc.idf_escolaridade
 left join bne.tab_curso curso with(nolock) on curso.idf_curso = pc.Idf_Curso_Tecnico_Graduacao
 left join bne.tab_fonte fonte with(nolock) on fonte.idf_fonte = pc.idf_fonte_tecnico_graduacao
                         where pc.idf_pesquisa_curriculo = @Idf_Pesquisa_Curriculo";
        #endregion

        #region [spCandidatos]

        private const string spCandidatos = @"select vc.idf_curriculo from bne.BNE_Vaga_Candidato vc with(nolock) 
                                            join bne.BNE_Curriculo cv with(nolock) on cv.Idf_Curriculo = vc.Idf_Curriculo
                                 where vc.idf_vaga = @Idf_Vaga
                                        and vc.Flg_Inativo = 0
                                        and cv.Flg_Inativo = 0 ";
        #endregion

        #region [spCandidatosNaoVisualizados]
        private const string spCandidatosNaoVisualizados = @"select vc.Idf_Curriculo from bne.bne_vaga_candidato vc with(nolock)
                                        join bne.BNE_Curriculo cv with(nolock) on cv.Idf_Curriculo = vc.Idf_Curriculo
                                        where vc.Flg_Inativo = 0 
                                            and vc.idf_vaga = @Idf_Vaga
                                            and vc.Dta_Visualizacao is null
                                            and cv.Flg_Inativo = 0
                                order by vc.Idf_Curriculo asc ";
        #endregion

        #region [spCandidatoNoPerfil]
        private const string spCandidatoNoPerfil = @"select top 1 
CASE 
			WHEN
				(		(	V.Idf_Funcao IN (SELECT Idf_Funcao FROM BNE.BNE_Funcao_Pretendida FP WITH(NOLOCK) WHERE C.Idf_Curriculo = FP.Idf_Curriculo)
							OR  V.Idf_Funcao IS NULL )
					AND (	V.Idf_Cidade = pf.Idf_Cidade
							OR V.Idf_Cidade IS NULL )
					AND (	PFEsc.Seq_Peso >= VEsc.Seq_Peso
							OR V.Idf_Escolaridade IS NULL )
					AND (	( ISNULL(V.Vlr_Salario_De, 0) <= C.Vlr_Pretensao_Salarial AND ISNULL(V.Vlr_Salario_Para, 999999) >= C.Vlr_Pretensao_Salarial )
							OR ( V.Vlr_Salario_De IS NULL AND V.Vlr_Salario_Para IS NULL ) )
					AND (   ( BNE.SF_Calcula_Idade_Em_Anos(PF.Dta_Nascimento) >= V.Num_Idade_Minima
							OR V.Num_Idade_Minima IS NULL )
							AND
							( BNE.SF_Calcula_Idade_Em_Anos(PF.Dta_Nascimento) <= V.Num_Idade_Maxima
							OR V.Num_Idade_Maxima IS NULL )	)
					AND (   V.Idf_Sexo = PF.Idf_Sexo
							OR V.Idf_Sexo IS NULL )
					AND (	( ( SELECT COUNT(VD.Idf_Disponibilidade) FROM BNE.BNE_Vaga_Disponibilidade VD WITH(NOLOCK) WHERE VD.Idf_Vaga = V.Idf_Vaga ) = 0 )
							OR 
							( (	SELECT COUNT(VD.Idf_Disponibilidade) 
								FROM BNE.BNE_Vaga_Disponibilidade VD WITH(NOLOCK) 
									INNER JOIN BNE.BNE_Curriculo_Disponibilidade CD WITH(NOLOCK) ON C.Idf_Curriculo = CD.Idf_Curriculo AND VD.Idf_Disponibilidade = CD.Idf_Disponibilidade
								WHERE VD.Idf_Vaga = V.Idf_Vaga ) 
								= 
							  ( SELECT COUNT(VD.Idf_Disponibilidade) FROM BNE.BNE_Vaga_Disponibilidade VD WITH(NOLOCK) WHERE VD.Idf_Vaga = V.Idf_Vaga ) 
							)
						) 
					AND ( 
							/* Verifica se não existe pergunta */
							( ( SELECT COUNT(VP.Idf_Vaga_Pergunta) FROM BNE.BNE_Vaga_Pergunta VP WITH(NOLOCK) WHERE VP.Idf_Vaga = V.Idf_Vaga ) = 0 ) 
							OR 
							/* Se existe pergunta, através dos joins, verifica se a resposta é igual a pergunta */
							( (	SELECT COUNT(VP.Idf_Vaga_Pergunta) 
								FROM BNE.BNE_Vaga_Pergunta VP WITH(NOLOCK) 
									INNER JOIN BNE.BNE_Vaga_Candidato_Pergunta VCP WITH(NOLOCK) 
										ON VCP.Idf_Vaga_Pergunta = VP.Idf_Vaga_Pergunta 
										AND VCP.Idf_Vaga_Candidato = VC.Idf_Vaga_Candidato
										AND VP.Flg_Resposta = VCP.Flg_Resposta
								WHERE VP.Idf_Vaga = V.Idf_Vaga ) 
								= 
							  ( SELECT COUNT(VP.Idf_Vaga_Pergunta) FROM BNE.BNE_Vaga_Pergunta VP WITH(NOLOCK) WHERE VP.Idf_Vaga = V.Idf_Vaga )
							) 
						)	
					AND (
							( V.Idf_Deficiencia = 0 AND ( PF.Idf_Deficiencia = 0 OR PF.Idf_Deficiencia IS NULL ) )
							OR 
							V.Idf_Deficiencia IS NULL
							OR 
							V.Idf_Deficiencia = PF.Idf_Deficiencia
						)
				) THEN 1
				ELSE 0
				END AS Dentro_Perfil
from bne.bne_curriculo c with(nolock)
join bne.TAB_Pessoa_Fisica pf with(nolock) on pf.idf_Pessoa_fisica = c.idf_pessoa_fisica
inner join bne.bne_vaga_Candidato vc with(nolock) on vc.idf_curriculo = c.idf_curriculo
inner join bne.bne_vaga v with(nolock) on v.idf_vaga = vc.idf_vaga
LEFT JOIN plataforma.TAB_Escolaridade PFEsc WITH ( NOLOCK ) ON PFEsc.Idf_Escolaridade = PF.Idf_Escolaridade
LEFT JOIN plataforma.TAB_Escolaridade VEsc WITH ( NOLOCK ) ON VEsc.Idf_Escolaridade = V.Idf_Escolaridade								
where c.idf_curriculo = @Idf_Curriculo and v.idf_vaga = @Idf_vaga";
        #endregion

        #region Notificacao para atualizacao de cvs para pesquisas com poucos currículos

        private const string spPoucosCandidatos = @"
        SELECT  pf.Eml_Pessoa ,
                pf.Num_CPF ,
                pf.Dta_Nascimento ,
                pf.Nme_Pessoa
        FROM    bne.bne_curriculo cv WITH ( NOLOCK )
                JOIN bne.tab_pessoa_fisica pf WITH ( NOLOCK ) ON pf.idf_pessoa_Fisica = cv.idf_pessoa_fisica
                JOIN bne.TAB_Endereco ende WITH ( NOLOCK ) ON ende.idf_endereco = pf.idf_endereco
                JOIN bne.bne_funcao_pretendida fp WITH ( NOLOCK ) ON fp.idf_curriculo = cv.idf_curriculo
                JOIN plataforma.tab_funcao f WITH ( NOLOCK ) ON f.Idf_Funcao = fp.Idf_Funcao
        WHERE   f.Idf_Area_BNE = @Idf_Area_BNE
                AND ende.idf_cidade = @Idf_Cidade
                AND cv.dta_atualizacao < GETDATE() - @Qtd_Dias
                AND pf.Eml_Pessoa IS NOT NULL
                AND cv.flg_inativo = 0
                AND pf.flg_inativo = 0
                AND ( pf.idf_Email_situacao_bloqueio IS NULL
                      OR pf.Idf_Email_Situacao_Bloqueio IN ( 1, 3 )
                    )
                AND cv.Idf_Situacao_Curriculo IN ( 1, 2, 3, 4, 9, 8, 10 )
        GROUP BY pf.Eml_Pessoa ,
                pf.Num_CPF ,
                pf.Dta_Nascimento ,
                pf.Nme_Pessoa";

        private const string spRecuperarPesquisaComPoucosCurriculos = @"
        SELECT  C.Idf_Cidade ,
                Nme_Cidade ,
                PC.Sig_Estado ,
                Funcao.idf_area_bne ,
                Funcao.Des_area_BNE ,
                Funcao.des_funcao_Categoria
        FROM    BNE.TAB_Pesquisa_Curriculo PC WITH ( NOLOCK )
                CROSS APPLY ( SELECT    f.Des_Funcao ,
                                        f.Idf_Funcao ,
                                        area.idf_area_bne ,
                                        area.Des_area_BNE ,
                                        fc.des_funcao_Categoria
                              FROM      BNE.TAB_Pesquisa_Curriculo_Funcao PCF WITH ( NOLOCK )
                                        INNER JOIN plataforma.TAB_Funcao F WITH ( NOLOCK ) ON PCF.Idf_Funcao = F.Idf_Funcao
                                        JOIN plataforma.tab_area_bne area WITH ( NOLOCK ) ON area.idf_area_bne = F.idf_area_bne
                                        JOIN plataforma.tab_funcao_categoria fc WITH ( NOLOCK ) ON fc.idf_Funcao_categoria = f.idf_Funcao_categoria
                              WHERE     PC.Idf_Pesquisa_Curriculo = PCF.Idf_Pesquisa_Curriculo
                            ) AS Funcao
                INNER JOIN plataforma.TAB_Cidade C WITH ( NOLOCK ) ON PC.Idf_Cidade = C.Idf_Cidade
        WHERE   pc.Dta_Cadastro > GETDATE() - 1
                AND Qtd_Curriculo_Retorno <= @QuantidadeMinima
                AND ( Des_Cod_CPF_Nome IS NULL
                      AND Eml_Pessoa IS NULL
                      AND Num_DDD_Telefone IS NULL
                      AND Num_Telefone IS NULL
                      AND Des_Avaliacao IS NULL
                    )
                AND pc.Sig_Estado IS NOT NULL
                AND pc.Flg_Pesquisa_Avancada = 1
                AND pc.Idf_Usuario_Filial_Perfil IS NOT NULL
        GROUP BY C.Idf_Cidade ,
                Nme_Cidade ,
                PC.Sig_Estado ,
                Funcao.idf_area_bne ,
                Funcao.Des_area_BNE ,
                Funcao.des_funcao_Categoria";

        #endregion

        #region [spNotificacaoPesquisaCurriculoAtendimento]
        private const string spNotificacaoPesquisaCurriculoAtendimento = @"select top (@QuantidadeEmpresa) f.num_cnpj, f.raz_social,f.num_ddd_comercial, f.num_comercial,
                                f.dta_cadastro, PlanoEmpresa.des_plano, quantidade.Total from bne.tab_usuario_filial_perfil ufp with(nolock)
                                join bne.tab_filial f with(nolock) on f.idf_Filial = ufp.idf_Filial
	                                outer apply(select top 1 p.des_plano from bne.bne_plano_adquirido pa with(nolock)
					                                join bne.bne_plano p with(nolock) on pa.idf_plano = p.idf_plano
					                                where pa.idf_Filial = f.idf_Filial and pa.idf_plano_situacao = 1--liberado
				                                    ) as PlanoEmpresa
	                                cross apply( select count(cvh.idf_Filial) as Total from bne.bne_curriculo_visualizacao_historico cvh with(nolock)
					                                where  cvh.idf_Filial = ufp.idf_filial 
					                                and flg_visualizacao_completa = 1
					                                and cvh.dta_visualizacao between @Dta_Inicio and @Dta_Fim) as quantidade
                                where f.flg_inativo = 0
                                group by f.num_cnpj, f.raz_social,f.num_ddd_comercial, f.num_comercial,
                                f.dta_cadastro, PlanoEmpresa.des_plano, quantidade.Total
                                order by quantidade.total desc";
        #endregion
        #endregion

        #region BuscaCurriculo
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tamanhoPagina">Tamanho do Página</param>
        /// <param name="paginaAtual">Página atual</param>
        /// <param name="idOrigem">Origem da sessão</param>
        /// <param name="idFilial">Filial logada, se tiver</param>
        /// <param name="idUsuarioFilialPerfil">Usuário logado, se tiver</param>
        /// <param name="objPesquisaCurriculo">Parametros de busca</param>
        /// <param name="numeroRegistros"></param>
        /// <param name="mediaSalarial"></param>
        /// <returns></returns>
        public static DataTable BuscaCurriculo(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo, out int numeroRegistros, out decimal mediaSalarial, out Dictionary<String, int> ListaFiltros, bool buscaCurriculosMaisUmAno = false, bool CidadeDisponibilidade = false)
        {
            return BuscaCurriculo(tamanhoPagina, paginaAtual, idOrigem, idFilial, idUsuarioFilialPerfil, objPesquisaCurriculo, null, null, out numeroRegistros, out mediaSalarial, out ListaFiltros, null, buscaCurriculosMaisUmAno, CidadeDisponibilidade);
        }
        public static DataTable BuscaCurriculo(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo, out int numeroRegistros, out decimal mediaSalarial, out Dictionary<String, int> ListaFiltros, bool buscaCurriculosMaisUmAno = false)
        {
            return BuscaCurriculo(tamanhoPagina, paginaAtual, idOrigem, idFilial, idUsuarioFilialPerfil, objPesquisaCurriculo, null, null, out numeroRegistros, out mediaSalarial, out ListaFiltros, null, buscaCurriculosMaisUmAno);
        }
        public static DataTable BuscaCurriculo(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, RastreadorCurriculo objRastreadorCurriculo, DateTime? dataUltimaVisualizacaoRastreador, out int numeroRegistros, out decimal mediaSalarial, out Dictionary<String, int> ListaFiltros)
        {
            return BuscaCurriculo(tamanhoPagina, paginaAtual, idOrigem, idFilial, idUsuarioFilialPerfil, null, objRastreadorCurriculo, null, out numeroRegistros, out mediaSalarial, out ListaFiltros, dataUltimaVisualizacaoRastreador);
        }
        public static DataTable BuscaCurriculo(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, Vaga objVaga, out int numeroRegistros, out decimal mediaSalarial, out Dictionary<String, int> ListaFiltros)
        {
            return BuscaCurriculo(tamanhoPagina, paginaAtual, idOrigem, idFilial, idUsuarioFilialPerfil, null, null, objVaga, out numeroRegistros, out mediaSalarial, out ListaFiltros);
        }

        private static DataTable BuscaCurriculoVagaCampanha(int tamanhoPagina, int paginaAtual, int idFilial, int? idUsuaioFilialPerfil, FiltroCurriculoVaga filtroCurriculoVaga, int idVaga, bool? Visualizados, out decimal mediaSalarial, out int TotalRegistros)
        {
            var dt = BuscaCurriculoVaga(tamanhoPagina, paginaAtual, idFilial, idFilial, filtroCurriculoVaga, idVaga, Visualizados, out mediaSalarial, out TotalRegistros);


            #region [Adicionar se é Candidato de campanha]
            ///
            dt.Columns.Add(new DataColumn("Flg_Campanha", Type.GetType("System.Boolean")));
            if (dt != null && dt.Rows.Count > 0)
            {
                using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                item["Flg_Campanha"] = CampanhaRecrutamentoCurriculo.VerificaCurriculoCandidatouCampanha(idVaga, Convert.ToInt32(item["Idf_Curriculo"]), trans);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            #endregion


            return dt;
        }
        private static DataTable BuscaCurriculoVaga(int tamanhoPagina, int paginaAtual, int idFilial, int? idUsuaioFilialPerfil, FiltroCurriculoVaga filtroCurriculoVaga, int idVaga, bool? Visualizados, out decimal mediaSalarial, out int TotalRegistros)
        {
            //não tem os candidatos da vaga no solr
            Dictionary<string, int> ListaFiltros = new Dictionary<string, int>();
            return BuscaCurriculo(tamanhoPagina, paginaAtual, 1, idFilial, idUsuaioFilialPerfil, null, null, null, out TotalRegistros, out mediaSalarial,
                out ListaFiltros, null, false, false, idVaga, filtroCurriculoVaga, Visualizados);
        }

        private static DataTable BuscaCurriculo(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil,
                                    PesquisaCurriculo objPesquisaCurriculo,
                                 RastreadorCurriculo objRastreadorCurriculo, Vaga objVaga, out int numeroRegistros, out decimal mediaSalarial,
                                 out Dictionary<String, int> ListaFiltros, DateTime? dataUltimaVisualizacaoRastreador = null,
                                 bool buscaCurriculosMaisUmAno = false, bool CidadeDisponibilidade = false, int? idVaga = null, FiltroCurriculoVaga filtroCandiatosVaga = null, bool? Visualizados = null)
        {
            Filial objFilial = null;
            UsuarioFilialPerfil objUsuarioFilialPerfil = null;

            if (idFilial.HasValue)
                objFilial = new Filial(idFilial.Value);

            if (idUsuarioFilialPerfil.HasValue)
                objUsuarioFilialPerfil = new UsuarioFilialPerfil(idUsuarioFilialPerfil.Value);

            SolrResponse<CurriculoCompleto.Response> resultado = null;
            if (objPesquisaCurriculo != null)
            {
                resultado = CurriculoCompleto.EfetuarRequisicao(Custom.BuscaCurriculo.MontarQuery(paginaAtual, tamanhoPagina, CidadeDisponibilidade, objFilial, objUsuarioFilialPerfil, objPesquisaCurriculo, buscaCurriculosMaisUmAno));
            }
            else if (objRastreadorCurriculo != null)
            {
                resultado = CurriculoCompleto.EfetuarRequisicao(Custom.BuscaCurriculo.MontarQuery(paginaAtual, tamanhoPagina, CidadeDisponibilidade, objRastreadorCurriculo, dataUltimaVisualizacaoRastreador));
            }
            else if (objVaga != null)
            {
                resultado = CurriculoCompleto.EfetuarRequisicao(Custom.BuscaCurriculo.MontarQuery(paginaAtual, tamanhoPagina, CidadeDisponibilidade, objFilial, objUsuarioFilialPerfil, objVaga));
            }
            else if (idVaga != null)
            {
                resultado = Custom.BuscaCurriculo.MontarQueryCandidatosVaga(paginaAtual, tamanhoPagina, idFilial.Value, idUsuarioFilialPerfil, idVaga, filtroCandiatosVaga, Visualizados);
            }

            ListaFiltros = resultado.facet_counts.facet_queries;
            var tableCvsSolr = RecuperarDataTable();

            SolrResponse<CurriculoCompleto.Response> resultadoCvClassificacao = null;
            SolrResponse<CurriculoCompleto.Response> resultadoCvSms = null;

            if (objFilial != null)
            {
                var curriculosEncontrados = resultado.response.docs.Select(item => item.Idf_Curriculo).ToList();

                //Se possui pelo menos 1 currículo
                if (curriculosEncontrados.Any())
                {
                    resultadoCvClassificacao = CurriculoCompleto.EfetuarRequisicao(Custom.BuscaCurriculo.MontarQueryBuscaClassificacao(objFilial, objUsuarioFilialPerfil, curriculosEncontrados));
                    resultadoCvSms = CurriculoCompleto.EfetuarRequisicao(Custom.BuscaCurriculo.MontarQueryBuscaSMS(objFilial, objUsuarioFilialPerfil, curriculosEncontrados, tamanhoPagina));
                }
            }


            foreach (var document in resultado.response.docs)
            {
                int? idfAvaliacao = null;
                string desAvaliacao = string.Empty;
                string desFuncoes = string.Empty;
                string descricaoSMS = string.Empty;
                string estadoCivil = string.Empty;

                int pertenceOrigem = 0;

                //Ajusta campo funcao
                if (document.Des_Funcao != null)
                    desFuncoes = document.Des_Funcao.Aggregate(desFuncoes, (current, funcoes) => current + (funcoes + ";"));

                //Se filial preenchida verifica se tem classificacao
                if (idFilial.HasValue && resultadoCvClassificacao != null)
                {
                    var classificacao = resultadoCvClassificacao.response.docs.OrderByDescending(c => c.Dta_Cadastro).FirstOrDefault(c => c.Idf_Curriculo == document.Idf_Curriculo);
                    if (classificacao != null)
                    {
                        idfAvaliacao = classificacao.Idf_Avaliacao;
                        desAvaliacao = string.IsNullOrWhiteSpace(classificacao.Des_Observacao) ? string.Empty : classificacao.Des_Observacao.Trim();
                    }
                }
                if (idFilial.HasValue && resultadoCvSms != null)
                {
                    var sms = resultadoCvSms.response.docs.Where(c => c.Idf_Curriculo == document.Idf_Curriculo).OrderByDescending(o => o.Dta_Cadastro).Take(3);
                    if (sms != null)
                        descricaoSMS = sms.Aggregate(descricaoSMS, (current, mensagemSMS) => current + string.Format("{0} enviado em {1}<br>", mensagemSMS.Des_Mensagem, mensagemSMS.Dta_Cadastro.AddHours(-3)));
                }

                if (document.Idfs_Origens != null && idOrigem != 1)
                {
                    if (document.Idfs_Origens.Any(origens => idOrigem == origens))
                        pertenceOrigem = 1;
                }

                if (document.Des_Estado_Civil != null)
                {
                    estadoCivil = document.Des_Estado_Civil;
                    estadoCivil = estadoCivil.Substring(0, 1);
                }

                #region Tratamento Maior Formacao

                int idMaiorEscolaridade;
                string descricaoMaiorEscolaridade, descricaoGrauEscolaridadeFormacao, descricaoCursoFormacao, descricaoFonteFormacao, dataConclusaoFormacao;
                int? identificadorGrauEscolaridadeFormacao, identificadorEscolaridadeFormacao;

                document.RecuperarMaiorFormacao(out idMaiorEscolaridade, out descricaoMaiorEscolaridade, out identificadorGrauEscolaridadeFormacao, out identificadorEscolaridadeFormacao, out descricaoGrauEscolaridadeFormacao, out descricaoCursoFormacao, out descricaoFonteFormacao, out dataConclusaoFormacao);
                #endregion

                #region Tratamento Experiencia

                string razaoSocial1, descricaoFuncao1, descricaoAtividade1, descricaoAtividadeEmpresa1, razaoSocial2, descricaoFuncao2, descricaoAtividade2, descricaoAtividadeEmpresa2, razaoSocial3, descricaoFuncao3, descricaoAtividade3, descricaoAtividadeEmpresa3, razaoSocial4, descricaoFuncao4, descricaoAtividade4, descricaoAtividadeEmpresa4, razaoSocial5, descricaoFuncao5, descricaoAtividade5, descricaoAtividadeEmpresa5;
                razaoSocial1 = descricaoFuncao1 = descricaoAtividade1 = descricaoAtividadeEmpresa1 = razaoSocial2 = descricaoFuncao2 = descricaoAtividade2 = descricaoAtividadeEmpresa2 = razaoSocial3 = descricaoFuncao3 = descricaoAtividade3 = descricaoAtividadeEmpresa3 = razaoSocial4 = descricaoFuncao4 = descricaoAtividade4 = descricaoAtividadeEmpresa4 = razaoSocial5 = descricaoFuncao5 = descricaoAtividade5 = descricaoAtividadeEmpresa5 = string.Empty;
                DateTime dataAdmissao1, dataAdmissao2, dataAdmissao3, dataAdmissao4, dataAdmissao5;
                dataAdmissao1 = dataAdmissao2 = dataAdmissao3 = dataAdmissao4 = dataAdmissao5 = new DateTime();
                DateTime? dataDemissao1, dataDemissao2, dataDemissao3, dataDemissao4, dataDemissao5;
                dataDemissao1 = dataDemissao2 = dataDemissao3 = dataDemissao4 = dataDemissao5 = null;

                if (document.Raz_Social != null)
                {
                    if (document.Raz_Social.Count >= 1)
                    {
                        razaoSocial1 = document.Raz_Social[0];
                        dataAdmissao1 = document.Dta_Admissao[0];

                        if (document.Dta_Demissao[0] != null && document.Dta_Demissao[0].ToShortDateString() != "01/01/1900")
                            dataDemissao1 = Convert.ToDateTime(document.Dta_Demissao[0]);

                        descricaoFuncao1 = document.Des_Funcao_Exercida[0];
                        descricaoAtividade1 = document.Des_Atividade[0];
                        descricaoAtividadeEmpresa1 = document.Des_Atividade_empresa[0];
                    }
                    if (document.Raz_Social.Count >= 2)
                    {
                        razaoSocial2 = document.Raz_Social[1];
                        dataAdmissao2 = document.Dta_Admissao[1];

                        if (document.Dta_Demissao[1] != null && document.Dta_Demissao[1].ToShortDateString() != "01/01/1900")
                            dataDemissao2 = Convert.ToDateTime(document.Dta_Demissao[1]);

                        descricaoFuncao2 = document.Des_Funcao_Exercida[1];
                        descricaoAtividade2 = document.Des_Atividade[1];
                        descricaoAtividadeEmpresa2 = document.Des_Atividade_empresa[1];
                    }
                    if (document.Raz_Social.Count >= 3)
                    {
                        razaoSocial3 = document.Raz_Social[2];
                        dataAdmissao3 = document.Dta_Admissao[2];

                        if (document.Dta_Demissao[2] != null && document.Dta_Demissao[2].ToShortDateString() != "01/01/1900")
                            dataDemissao3 = Convert.ToDateTime(document.Dta_Demissao[2]);

                        descricaoFuncao3 = document.Des_Funcao_Exercida[2];
                        descricaoAtividade3 = document.Des_Atividade[2];
                        descricaoAtividadeEmpresa3 = document.Des_Atividade_empresa[2];
                    }

                    if (document.Raz_Social.Count >= 4)
                    {
                        razaoSocial4 = document.Raz_Social[3];
                        dataAdmissao4 = document.Dta_Admissao[3];

                        if (document.Dta_Demissao[3] != null && document.Dta_Demissao[3].ToShortDateString() != "01/01/1900")
                            dataDemissao4 = Convert.ToDateTime(document.Dta_Demissao[3]);

                        descricaoFuncao4 = document.Des_Funcao_Exercida[3];
                        descricaoAtividade4 = document.Des_Atividade[3];
                        descricaoAtividadeEmpresa4 = document.Des_Atividade_empresa[3];
                    }

                    if (document.Raz_Social.Count >= 5)
                    {
                        razaoSocial5 = document.Raz_Social[4];
                        dataAdmissao5 = document.Dta_Admissao[4];

                        if (document.Dta_Demissao[4] != null && document.Dta_Demissao[4].ToShortDateString() != "01/01/1900")
                            dataDemissao5 = Convert.ToDateTime(document.Dta_Demissao[4]);

                        descricaoFuncao5 = document.Des_Funcao_Exercida[4];
                        descricaoAtividade5 = document.Des_Atividade[4];
                        descricaoAtividadeEmpresa5 = document.Des_Atividade_empresa[4];
                    }
                }
                #endregion

                #region Tratamento Função Pretendida
                string descricaoFuncaoPretendida1, descricaoFuncaoPretendida2, descricaoFuncaoPretendida3, descricaoFuncaoPretendida4, descricaoFuncaoPretendida5;
                descricaoFuncaoPretendida1 = descricaoFuncaoPretendida2 = descricaoFuncaoPretendida3 = descricaoFuncaoPretendida4 = descricaoFuncaoPretendida5 = string.Empty;
                int quantidadeExperienciaFuncaoPretendida1, quantidadeExperienciaFuncaoPretendida2, quantidadeExperienciaFuncaoPretendida3, quantidadeExperienciaFuncaoPretendida4, quantidadeExperienciaFuncaoPretendida5;
                quantidadeExperienciaFuncaoPretendida1 = quantidadeExperienciaFuncaoPretendida2 = quantidadeExperienciaFuncaoPretendida3 = quantidadeExperienciaFuncaoPretendida4 = quantidadeExperienciaFuncaoPretendida5 = 0;
                if (document.Des_Funcao != null)
                {
                    if (document.Des_Funcao.Count >= 1)
                    {
                        descricaoFuncaoPretendida1 = document.Des_Funcao[0];
                        quantidadeExperienciaFuncaoPretendida1 = document.Qtd_Experiencia != null ? document.Qtd_Experiencia[0] : 0;
                    }
                    if (document.Des_Funcao.Count >= 2)
                    {
                        descricaoFuncaoPretendida2 = document.Des_Funcao[1];
                        quantidadeExperienciaFuncaoPretendida2 = document.Qtd_Experiencia != null ? document.Qtd_Experiencia[1] : 0;
                    }
                    if (document.Des_Funcao.Count >= 3)
                    {
                        descricaoFuncaoPretendida3 = document.Des_Funcao[2];
                        quantidadeExperienciaFuncaoPretendida3 = document.Qtd_Experiencia != null ? document.Qtd_Experiencia[2] : 0;
                    }
                    if (document.Des_Funcao.Count >= 4)
                    {
                        descricaoFuncaoPretendida4 = document.Des_Funcao[3];
                        quantidadeExperienciaFuncaoPretendida4 = document.Qtd_Experiencia != null ? document.Qtd_Experiencia[3] : 0;
                    }
                    if (document.Des_Funcao.Count >= 5)
                    {
                        descricaoFuncaoPretendida5 = document.Des_Funcao[4];
                        quantidadeExperienciaFuncaoPretendida5 = document.Qtd_Experiencia != null ? document.Qtd_Experiencia[4] : 0;
                    }
                }


                #endregion

                tableCvsSolr.Rows.Add(
                            document.Idf_Curriculo,
                            idFilial.HasValue ? document.Nme_Pessoa : PessoaFisica.RetornarPrimeiroNome(document.Nme_Pessoa),
                            document.Sig_Sexo,
                            estadoCivil,
                            Helper.CalcularIdade(document.Dta_Nascimento),
                            idMaiorEscolaridade,
                            descricaoMaiorEscolaridade,
                            document.Vlr_Pretensao_Salarial,
                            document.Des_Bairro,
                            document.Nme_Cidade,
                            desFuncoes,
                            document.Total_Experiencia + " m",
                            document.Des_Categoria_Habilitacao,
                            0, //Des_atualizacao ver se campo é utilizado
                            document.Flg_VIP ? 1 : 0,
                            0,//score
                            " ",//string msg
                            idfAvaliacao,//int Avaliacao
                            desAvaliacao,//string Des_avaliacao
                            document.Dentro_Perfil,//int dentro do perfil
                            pertenceOrigem,//int pertence origem
                            document.Dta_Atualizacao,
                            document.Sig_Estado,
                            document.Num_CPF,
                            document.Vlr_Ultimo_Salario,
                            identificadorGrauEscolaridadeFormacao,
                            identificadorEscolaridadeFormacao,
                            descricaoGrauEscolaridadeFormacao,
                            descricaoCursoFormacao,
                            descricaoFonteFormacao,
                            dataConclusaoFormacao,
                            razaoSocial1,
                            dataAdmissao1,
                            dataDemissao1,
                            descricaoFuncao1,
                            descricaoAtividade1,
                            descricaoAtividadeEmpresa1,
                            razaoSocial2,
                            dataAdmissao2,
                            dataDemissao2,
                            descricaoFuncao2,
                            descricaoAtividade2,
                            descricaoAtividadeEmpresa2,
                            razaoSocial3,
                            dataAdmissao3,
                            dataDemissao3,
                            descricaoFuncao3,
                            descricaoAtividade3,
                            descricaoAtividadeEmpresa3,
                            razaoSocial4,
                            dataAdmissao4,
                            dataDemissao4,
                            descricaoFuncao4,
                            descricaoAtividade4,
                            descricaoAtividadeEmpresa4,
                            razaoSocial5,
                            dataAdmissao5,
                            dataDemissao5,
                            descricaoFuncao5,
                            descricaoAtividade5,
                            descricaoAtividadeEmpresa5,
                            document.Idf_Deficiencia,
                            document.Nme_Anexo,
                            descricaoSMS,
                            descricaoFuncaoPretendida1,
                            quantidadeExperienciaFuncaoPretendida1,
                            descricaoFuncaoPretendida2,
                            quantidadeExperienciaFuncaoPretendida2,
                            descricaoFuncaoPretendida3,
                            quantidadeExperienciaFuncaoPretendida3,
                            descricaoFuncaoPretendida4,
                            quantidadeExperienciaFuncaoPretendida4,
                            descricaoFuncaoPretendida5,
                            quantidadeExperienciaFuncaoPretendida5,
                            document.Flg_Auto_Candidatura
                            );
            }

            //Numero de registros
            numeroRegistros = resultado.response.numFound;

            var limiteResultado = idFilial.HasValue ? Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.LimiteResultadoPesquisaLogado)) : Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.LimiteResultadoPesquisaDeslogado));
            if (numeroRegistros > limiteResultado)
                numeroRegistros = limiteResultado;

            if (paginaAtual.Equals(0) && objPesquisaCurriculo != null) //Se for a primeira página, para evitar salvar quando for paginação
                objPesquisaCurriculo.AtualizarQuantidadeRegistros(numeroRegistros);

            mediaSalarial = 0;

            return tableCvsSolr;
        }
        #endregion

        #region RecuperarDataTable
        /// <summary>
        /// DataTable utilizado no resultado da pesquisa de cv
        /// </summary>
        /// <returns></returns>
        private static DataTable RecuperarDataTable()
        {
            var tableCvsSolr = new DataTable();
            tableCvsSolr.Columns.Add("Idf_Curriculo", typeof(int));
            tableCvsSolr.Columns.Add("Nme_Pessoa", typeof(string));
            tableCvsSolr.Columns.Add("Des_Genero", typeof(string));
            tableCvsSolr.Columns.Add("Des_Estado_Civil", typeof(string));
            tableCvsSolr.Columns.Add("Num_Idade", typeof(int));
            tableCvsSolr.Columns.Add("Idf_Escolaridade", typeof(int));
            tableCvsSolr.Columns.Add("Sig_Escolaridade", typeof(string));
            tableCvsSolr.Columns.Add("Vlr_Pretensao_Salarial", typeof(Decimal));
            tableCvsSolr.Columns.Add("Des_Bairro", typeof(string));
            tableCvsSolr.Columns.Add("Nme_Cidade", typeof(string));
            tableCvsSolr.Columns.Add("Des_Funcao", typeof(string));
            tableCvsSolr.Columns.Add("Des_Experiencia", typeof(string));
            tableCvsSolr.Columns.Add("Des_Categoria_Habilitacao", typeof(string));
            tableCvsSolr.Columns.Add("Des_Atualizacao", typeof(string));
            tableCvsSolr.Columns.Add("Vip", typeof(int));
            tableCvsSolr.Columns.Add("Valor", typeof(float));
            tableCvsSolr.Columns.Add("Mensagem", typeof(string));
            tableCvsSolr.Columns.Add("Avaliacao", typeof(int));
            tableCvsSolr.Columns.Add("Des_avaliacao", typeof(string));
            tableCvsSolr.Columns.Add("Dentro_Perfil", typeof(int));
            tableCvsSolr.Columns.Add("Pertence_origem", typeof(int));
            tableCvsSolr.Columns.Add("Dta_Atualizacao", typeof(DateTime));
            tableCvsSolr.Columns.Add("Sig_Estado", typeof(string));
            tableCvsSolr.Columns.Add("Num_CPF", typeof(string));
            tableCvsSolr.Columns.Add("Vlr_Ultimo_Salario", typeof(Decimal));
            tableCvsSolr.Columns.Add("Idf_Grau_Escolaridade_Formacao", typeof(int));
            tableCvsSolr.Columns.Add("Idf_Escolaridade_Formacao", typeof(int));
            tableCvsSolr.Columns.Add("Des_Grau_Escolaridade_Formacao", typeof(string));
            tableCvsSolr.Columns.Add("Des_Curso_Formacao", typeof(string));
            tableCvsSolr.Columns.Add("Des_Fonte_Formacao", typeof(string));
            tableCvsSolr.Columns.Add("Dta_Conclusao_Formacao", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Raz_Social_1", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Admissao_1", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Demissao_1", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Des_Funcao_1", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Atividade_1", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Area_BNE_1", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Raz_Social_2", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Admissao_2", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Demissao_2", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Des_Funcao_2", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Atividade_2", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Area_BNE_2", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Raz_Social_3", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Admissao_3", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Demissao_3", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Des_Funcao_3", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Atividade_3", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Area_BNE_3", typeof(string));

            tableCvsSolr.Columns.Add("Experiencia_Raz_Social_4", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Admissao_4", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Demissao_4", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Des_Funcao_4", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Atividade_4", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Area_BNE_4", typeof(string));

            tableCvsSolr.Columns.Add("Experiencia_Raz_Social_5", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Admissao_5", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Dta_Demissao_5", typeof(DateTime));
            tableCvsSolr.Columns.Add("Experiencia_Des_Funcao_5", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Atividade_5", typeof(string));
            tableCvsSolr.Columns.Add("Experiencia_Des_Area_BNE_5", typeof(string));

            tableCvsSolr.Columns.Add("Idf_Deficiencia", typeof(int));
            tableCvsSolr.Columns.Add("Nme_Anexo", typeof(string));
            tableCvsSolr.Columns.Add("Des_SMS", typeof(string));
            tableCvsSolr.Columns.Add("Funcao_Pretendida_1", typeof(string));
            tableCvsSolr.Columns.Add("Quantidade_Funcao_Pretendida_1", typeof(string));
            tableCvsSolr.Columns.Add("Funcao_Pretendida_2", typeof(string));
            tableCvsSolr.Columns.Add("Quantidade_Funcao_Pretendida_2", typeof(string));
            tableCvsSolr.Columns.Add("Funcao_Pretendida_3", typeof(string));
            tableCvsSolr.Columns.Add("Quantidade_Funcao_Pretendida_3", typeof(string));

            tableCvsSolr.Columns.Add("Funcao_Pretendida_4", typeof(string));
            tableCvsSolr.Columns.Add("Quantidade_Funcao_Pretendida_4", typeof(string));

            tableCvsSolr.Columns.Add("Funcao_Pretendida_5", typeof(string));
            tableCvsSolr.Columns.Add("Quantidade_Funcao_Pretendida_5", typeof(string));
            //usado apenas nos candidatos da vaga.
            tableCvsSolr.Columns.Add("Flg_Auto_Candidatura", typeof(bool));

            return tableCvsSolr;
        }
        #endregion

        #region MontaURLSolrBuscaCVsCampanha
        public static string MontaURLSolrBuscaCVsCampanha(CampanhaRecrutamento objCampanhaRecrutamento, Funcao objFuncao)
        {
            var urlCVsCampanhaSolr = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrBuscaCVCampanha);

            //Paginação
            urlCVsCampanhaSolr += "&start={pagina}&rows={tamanhoPagina}";

            //Filtros+Sort
            urlCVsCampanhaSolr += CompletarURLCampanhaRecrutamento(objCampanhaRecrutamento, objFuncao);

            return urlCVsCampanhaSolr;
        }
        #endregion

        #region BuscaCurriculosCampanha
        public static List<DTO.Curriculo> BuscaCurriculosCampanha(string urlCVsCampanhaSolr, int start, int tamanhoPagina)
        {
            try
            {
                List<DTO.Curriculo> listaCVs = new List<DTO.Curriculo>();

                urlCVsCampanhaSolr = urlCVsCampanhaSolr.Replace("{pagina}", start.ToString());
                urlCVsCampanhaSolr = urlCVsCampanhaSolr.Replace("{tamanhoPagina}", tamanhoPagina.ToString());

                var resultado = CurriculoCampanha.EfetuarRequisicao(urlCVsCampanhaSolr);

                foreach (var document in resultado.response.docs)
                {
                    var cv = new DTO.Curriculo
                    {
                        IdCurriculo = document.Idf_Curriculo,
                        NomeCompleto = document.Nme_Pessoa,
                        Email = document.Eml_Pessoa,
                        NumeroDDDCelular = document.Num_DDD_Celular,
                        NumeroCelular = document.Num_Celular,
                        NumeroCPF = document.Num_CPF,
                        DataNascimento = document.Dta_Nascimento
                    };

                    listaCVs.Add(cv);
                }
                return listaCVs;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha na requisição de CVs para campanha via Solr.");
                return new List<DTO.Curriculo>();
            }
        }
        #endregion

        #region CompletarURLCampanhaRecrutamento
        private static string CompletarURLCampanhaRecrutamento(CampanhaRecrutamento objCampanhaRecrutamento, Funcao objFuncao)
        {
            string complementoUrlFiltros = string.Empty;

            if (!objCampanhaRecrutamento.EnvioCampanhaRecrutamento()) //Busca apenas pela função e cidade informada na pesquisa de CV
            {
                //Filtro por data de atualização do CV
                var dataAtualizacaoCVMinima = DateTime.Now.AddYears(-1);
                complementoUrlFiltros += "&fq=Dta_Atualizacao:[" + dataAtualizacaoCVMinima.Year + "-" + dataAtualizacaoCVMinima.Month.ToString().PadLeft(2, '0') + "-" + dataAtualizacaoCVMinima.Day.ToString().PadLeft(2, '0') + "T00:00:00.833Z+TO+*]";

                //Função
                complementoUrlFiltros += "&fq=Idfs_Funcoes:" + objFuncao.IdFuncao;

                //Cidade
                complementoUrlFiltros += "&fq=Idfs_Cidades:" + objCampanhaRecrutamento.PesquisaCurriculo.Cidade.IdCidade;

                //Não Enviar Para VIP
                complementoUrlFiltros += "&fq=Flg_VIP:false";

                //Sort
                complementoUrlFiltros += "&sort=termfreq(Idf_Cidade," + objCampanhaRecrutamento.PesquisaCurriculo.Cidade.IdCidade + ")+desc,";

                complementoUrlFiltros += "termfreq(Idf_Funcao," + objFuncao.IdFuncao + ")+desc";
            }
            else //Busca por Função, Cidade e Salário da Vaga
            {
                var filtroSalario = string.Empty;
                var desFuncao = string.Empty;
                var idCidade = string.Empty;
                var idFuncao = string.Empty;

                complementoUrlFiltros = "&q=(Idfs_Funcoes:{Idf_Funcao}+OR+buscaBNE:%22{DesFuncao}%22)"
                            + " AND Idfs_Cidades:{Idf_Cidade}"
                            + " AND Vlr_Pretensao_Salarial:{FitroSalario}"
                            + "&wt=json"
                            + "&fq=Flg_VIP:false"
                            + "&sort=termfreq(Idf_Cidade,{Idf_Cidade})+desc,termfreq(Idf_Funcao,{Idf_Funcao})+desc,Dta_Atualizacao+desc";

                #region Salario
                if (objCampanhaRecrutamento.Vaga.ValorSalarioDe.HasValue && objCampanhaRecrutamento.Vaga.ValorSalarioPara.HasValue)
                {
                    if (objCampanhaRecrutamento.Vaga.ValorSalarioDe.Value == objCampanhaRecrutamento.Vaga.ValorSalarioPara.Value)
                        filtroSalario += "[*+TO+" + (objCampanhaRecrutamento.Vaga.ValorSalarioPara.Value + (objCampanhaRecrutamento.Vaga.ValorSalarioPara.Value * 25 / 100)).ToString().Replace(",", ".") + "]&";
                    else
                        filtroSalario += "[" + objCampanhaRecrutamento.Vaga.ValorSalarioDe.Value.ToString().Replace(",", ".") + "+TO+" + objCampanhaRecrutamento.Vaga.ValorSalarioPara.Value.ToString().Replace(",", ".") + "]&";
                }
                else
                {
                    if (objCampanhaRecrutamento.Vaga.ValorSalarioDe.HasValue)
                        filtroSalario += "[" + objCampanhaRecrutamento.Vaga.ValorSalarioDe.Value.ToString().Replace(",", ".") + "+TO+*]&";

                    if (objCampanhaRecrutamento.Vaga.ValorSalarioPara.HasValue)
                        filtroSalario += "[*+TO+" + objCampanhaRecrutamento.Vaga.ValorSalarioPara.Value.ToString().Replace(",", ".") + "]&";
                }
                #endregion

                #region Cidade e Função
                idCidade = objCampanhaRecrutamento.Vaga.Cidade.IdCidade.ToString();
                idFuncao = objCampanhaRecrutamento.Vaga.Funcao.IdFuncao.ToString();
                desFuncao = objCampanhaRecrutamento.Vaga.DescricaoFuncao;

                complementoUrlFiltros = complementoUrlFiltros.Replace("{Idf_Funcao}", idFuncao);
                complementoUrlFiltros = complementoUrlFiltros.Replace("{DesFuncao}", desFuncao);
                complementoUrlFiltros = complementoUrlFiltros.Replace("{Idf_Cidade}", idCidade);
                complementoUrlFiltros = complementoUrlFiltros.Replace("{FitroSalario}", filtroSalario);
                #endregion
            }

            return complementoUrlFiltros;
        }
        #endregion

        #region BuscaCurriculoQuePertenceAOrigem
        public static DataTable BuscaCurriculoQuePertenceAOrigem(int tamanhoPagina, int paginaAtual, int idOrigem, int idFilial, int? idUsuarioFilialPerfil, List<int> listIdFuncoes, string descricaoCurso, out int numeroRegistros)
        {
            Object valueFuncoes = DBNull.Value;
            Object valueCurso = DBNull.Value;

            if (listIdFuncoes != null && listIdFuncoes.Count > 0)
                valueFuncoes = String.Join(",", listIdFuncoes.Select(item => item.ToString(CultureInfo.CurrentCulture)).ToArray());

            if (!string.IsNullOrEmpty(descricaoCurso))
                valueCurso = descricaoCurso;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Size = 4, Value = 0, Direction = ParameterDirection.Output },
                    new SqlParameter { ParameterName = "@Idf_Origem", SqlDbType = SqlDbType.Int, Size = 4, Value = idOrigem } ,
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial } ,
                    new SqlParameter { ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual } ,
                    new SqlParameter { ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina } ,
                    new SqlParameter { ParameterName = "@Funcoes", SqlDbType = SqlDbType.VarChar, Size = 2000, Value = valueFuncoes } ,
                    new SqlParameter { ParameterName = "@Des_Curso", SqlDbType = SqlDbType.VarChar, Size = 100, Value = valueCurso }
                };

            if (idUsuarioFilialPerfil.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil });

            DataTable dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "SP_BUSCA_CURRICULO_PERTENCE_ORIGEM", parms).Tables[0];
            numeroRegistros = Convert.ToInt32(parms[0].Value);

            return dt;
        }
        #endregion

        #region AtualizarQuantidadeRegistros
        /// <summary>
        /// Se a quantidade mínima de currículos não foi atingida pela busca, envia um e-mail com a busca realizada.
        /// </summary>
        /// <param name="numeroRegistros"></param>
        private void AtualizarQuantidadeRegistros(int numeroRegistros)
        {
            try
            {
                //Iniciando uma thread para preencher a localização do endereço
                var thread = new Thread(AtualizarNumeroRetornos);
                thread.Start(numeroRegistros);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region AtualizarNumeroRetornos
        private void AtualizarNumeroRetornos(object quantidadeCurriculoEncontrado)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = _idPesquisaCurriculo } ,
                    new SqlParameter { ParameterName = "@Qtd_Curriculo_Retorno", SqlDbType = SqlDbType.Int, Size = 4, Value = quantidadeCurriculoEncontrado }
                };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spatualizarquantidadecurriculosretornados, parms);
        }
        #endregion

        #region RecuperarCurriculosNaoVisualisadosVaga
        /// <summary>
        /// Método utilizado na tela de Pesquisa de Curriculo, retorna todos os currículos relacionados a um rastreador de CV
        /// </summary>
        /// <param name="paginaAtual"> </param>
        /// <param name="idFilial"></param>
        /// <param name="idVaga"></param>
        /// <param name="numeroRegistros"></param>
        /// <param name="mediaSalarial"></param>
        /// <param name="tamanhoPagina"> </param>
        /// <returns></returns>
        public static DataTable RecuperarCurriculosNaoVisualisadosVaga(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil, FiltroCurriculoVaga filtros, out int numeroRegistros, out decimal mediaSalarial)
        {
            //Configurando parametros basicos da pesquisa

            mediaSalarial = 0;
            numeroRegistros = 0;
            DataTable resultado = new DataTable();
            try
            {
                resultado = BuscaCurriculoVaga(tamanhoPagina, paginaAtual, idFilial, idUsuarioFilialPerfil, filtros, idVaga, false, out mediaSalarial, out numeroRegistros);
                return resultado;

            }
            catch (Exception ex)
            {
            }
            if (resultado != null && resultado.Rows.Count > 0)
            {
                using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            //ver se esta dentro do perfil
                            for (int i = 0; i < resultado.Rows.Count; i++)
                            {
                                if (CandidatoNoPerfil(Convert.ToInt32(resultado.Rows[i]["Idf_Curriculo"]), idVaga, trans))
                                    resultado.Rows[i]["Dentro_Perfil"] = 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex, "Erro ao buscar os candidatos da vaga");
                        }
                    }
                    conn.Close();
                }
                return resultado;
            }

            var parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual } ,
                                new SqlParameter { ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina } ,
                                new SqlParameter { ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Size = 4, Value = 0, Direction = ParameterDirection.Output} ,
                                new SqlParameter { ParameterName = "@MediaSalarial", SqlDbType = SqlDbType.Decimal, Value = 0, Direction = ParameterDirection.Output } ,
                                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial },
                                new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                            };

            if (idUsuarioFilialPerfil.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idUsuarioFilialPerfil });

            var dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BUSCA_CURRICULO_VAGA_NAO_VISUALIZADO", parms).Tables[0];

            numeroRegistros = Convert.ToInt32(parms[2].Value);

            mediaSalarial = parms[3].Value != DBNull.Value ? Convert.ToDecimal(parms[3].Value) : 0M;

            return dt;

        }
        #endregion

        #region RecuperarCurriculosNaoVisualisadosVagaCampanha
        /// <summary>
        /// Método utilizado na tela de Pesquisa de Curriculo, retorna todos os currículos relacionados a um rastreador de CV
        /// </summary>
        /// <param name="paginaAtual"> </param>
        /// <param name="idFilial"></param>
        /// <param name="idVaga"></param>
        /// <param name="numeroRegistros"></param>
        /// <param name="mediaSalarial"></param>
        /// <param name="tamanhoPagina"> </param>
        /// <returns></returns>
        public static DataTable RecuperarCurriculosNaoVisualisadosVagaCampanha(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil,
            FiltroCurriculoVaga filtros, out int numeroRegistros, out decimal mediaSalarial)
        {
            DataTable dt = null;
            mediaSalarial = 0;
            numeroRegistros = 0;
            try
            {//Busca no Solr
                dt = BuscaCurriculoVagaCampanha(tamanhoPagina, paginaAtual, idFilial, idUsuarioFilialPerfil, filtros, idVaga, false, out mediaSalarial, out numeroRegistros);

            }
            catch (Exception)
            {
                //Configurando parametros basicos da pesquisa
                var parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual } ,
                                new SqlParameter { ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina } ,
                                new SqlParameter { ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Size = 4, Value = 0, Direction = ParameterDirection.Output} ,
                                new SqlParameter { ParameterName = "@MediaSalarial", SqlDbType = SqlDbType.Decimal, Value = 0, Direction = ParameterDirection.Output } ,
                                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial },
                                new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                            };

                if (idUsuarioFilialPerfil.HasValue)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idUsuarioFilialPerfil });

                dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BUSCA_CURRICULO_VAGA_NAO_VISUALIZADO_CAMPANHA", parms).Tables[0];

                numeroRegistros = Convert.ToInt32(parms[2].Value);

                mediaSalarial = parms[3].Value != DBNull.Value ? Convert.ToDecimal(parms[3].Value) : 0M;

            }
            return dt;

        }
        #endregion

        #region RecuperarCurriculosRelacionadosVagaCampanha
        /// <summary>
        /// Método utilizado na tela de Pesquisa de Curriculo, retorna todos os currículos relacionados a um rastreador de CV Quando gerado por campanha.
        /// </summary>
        /// <param name="paginaAtual"> </param>
        /// <param name="idFilial"></param>
        /// <param name="idVaga"></param>
        /// <param name="numeroRegistros"></param>
        /// <param name="mediaSalarial"></param>
        /// <param name="tamanhoPagina"> </param>
        /// <returns></returns>
        public static DataTable RecuperarCurriculosRelacionadosVagaCampanha(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil, FiltroCurriculoVaga filtros,
            out int numeroRegistros, out decimal mediaSalarial)
        {
            DataTable dt = null;
            mediaSalarial = 0;
            numeroRegistros = 0;
            try
            {//busca no solr
                dt = BuscaCurriculoVagaCampanha(tamanhoPagina, paginaAtual, idFilial, idUsuarioFilialPerfil, filtros, idVaga, null, out mediaSalarial, out numeroRegistros);
            }
            catch (Exception)
            {
                //Configurando parametros basicos da pesquisa
                var parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual } ,
                                new SqlParameter { ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina } ,
                                new SqlParameter { ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Size = 4, Value = 0, Direction = ParameterDirection.Output} ,
                                new SqlParameter { ParameterName = "@MediaSalarial", SqlDbType = SqlDbType.Decimal, Value = 0, Direction = ParameterDirection.Output } ,
                                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial },
                                new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                            };

                if (idUsuarioFilialPerfil.HasValue)
                    parms.Add(new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idUsuarioFilialPerfil });

                dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BUSCA_CURRICULO_VAGA_VISUALIZADO_CAMPANHA ", parms).Tables[0];

                numeroRegistros = Convert.ToInt32(parms[2].Value);

                mediaSalarial = parms[3].Value != DBNull.Value ? Convert.ToDecimal(parms[3].Value) : 0M;

            }
            return dt;

        }
        #endregion

        #region Salvar
        public void Salvar(List<PesquisaCurriculoFuncao> listaFuncao)
        {
            Salvar(listaFuncao, new List<PesquisaCurriculoIdioma>(), new List<PesquisaCurriculoDisponibilidade>(), new List<CampoPalavraChavePesquisaCurriculo>(), new List<PesquisaCurriculoDeficiencia>());
        }
        public void Salvar(List<PesquisaCurriculoFuncao> listaFuncao, List<PesquisaCurriculoIdioma> listaIdioma, List<PesquisaCurriculoDisponibilidade> listaDisponibilidade, List<CampoPalavraChavePesquisaCurriculo> listaPalavraChave, List<PesquisaCurriculoDeficiencia> listaDeficiencia, bool queroContratarEstagiarios = false, bool queroContratarAprendiz = false)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        if (queroContratarEstagiarios)
                            this.IdEscolaridadeWebStagio = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfsEscolaridadeWebEstagiosQueroEstagiario, trans);

                        if (queroContratarAprendiz)
                            this.FlagAprendiz = true;

                        Save(trans);

                        foreach (var item in listaFuncao)
                        {
                            item.PesquisaCurriculo = this;
                            item.Save(trans);
                        }

                        foreach (var item in listaIdioma)
                        {
                            item.PesquisaCurriculo = this;
                            item.Save(trans);
                        }
                        foreach (var item in listaDisponibilidade)
                        {
                            item.PesquisaCurriculo = this;
                            item.Save(trans);
                        }
                        foreach (var item in listaPalavraChave)
                        {
                            item.PesquisaCurriculo = this;
                            item.Save(trans);
                        }

                        foreach (var item in listaDeficiencia)
                        {
                            item.PesquisaCurriculo = this;
                            item.Save(trans);
                        }

                        trans.Commit();

                        CampanhaRecrutamento.EnviarCampanhaPesquisaCV(this);
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

        #region Avaliações
        private List<PesquisaCurriculoAvaliacao> _avaliacoes;

        public List<PesquisaCurriculoAvaliacao> Avaliacoes
        {
            get
            {
                if (this._avaliacoes == null)
                    this._avaliacoes = this.CarregarAvaliacoes();
                if (this._avaliacoes == null)
                    this._avaliacoes = new List<PesquisaCurriculoAvaliacao>();
                return this._avaliacoes;
            }
        }

        public void InserirAvaliacao(BNE.BLL.Enumeradores.CurriculoClassificacao avaliacao)
        {

            PesquisaCurriculoAvaliacao objAvaliacao = new PesquisaCurriculoAvaliacao()
            {
                Avaliacao = new Avaliacao((int)avaliacao)
            };

            if (this._avaliacoes == null)
            {
                this._avaliacoes = new List<PesquisaCurriculoAvaliacao>();
                objAvaliacao.PesquisaCurriculo = this;
                this._avaliacoes.Add(objAvaliacao);
                this._modified = true;
            }
            else
            {
                var count = this._avaliacoes.Where(o => o.Avaliacao.IdAvaliacao == objAvaliacao.Avaliacao.IdAvaliacao).Count();
                if (count == 0)
                {
                    objAvaliacao.PesquisaCurriculo = this;
                    this._avaliacoes.Add(objAvaliacao);
                    this._modified = true;
                }
            }
        }

        private List<PesquisaCurriculoAvaliacao> CarregarAvaliacoes()
        {
            List<PesquisaCurriculoAvaliacao> avaliacoes = null;
            if (this._idPesquisaCurriculo > 0)
            {
                var query = "SELECT Idf_Pesquisa_Curriculo_Avaliacao FROM BNE.BNE_Pesquisa_Curriculo_Avaliacao WITH (NOLOCK) WHERE Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo;";
                var parms = new List<SqlParameter>() { new SqlParameter() { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Value = this._idPesquisaCurriculo } };
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, query, parms))
                {
                    while (dr.Read())
                    {
                        if (avaliacoes == null)
                            avaliacoes = new List<PesquisaCurriculoAvaliacao>();

                        avaliacoes.Add(new PesquisaCurriculoAvaliacao(dr.GetInt32(0)));
                    }
                }
            }
            return avaliacoes;
        }

        public void SalvarAvaliacoes()
        {
            foreach (var ava in this.Avaliacoes)
            {
                ava.Save();
            }
            return;
        }
        #endregion

        #region [ RecuperaUltimaPesquisa ]
        public static bool RecuperarUltimaPesquisa(int usuarioFilialPerfil, out PesquisaCurriculo pesquisaCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = usuarioFilialPerfil }
                };

            const string spSelectUltimaPesquisa = @"SELECT TOP 1 * FROM BNE.TAB_Pesquisa_Curriculo AS pesq
              WHERE  pesq.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
              ORDER BY Idf_Pesquisa_Curriculo DESC";

            using (IDataReader dr = trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, spSelectUltimaPesquisa, parms)
                                                : DataAccessLayer.ExecuteReader(trans, CommandType.Text, spSelectUltimaPesquisa, parms))
            {
                pesquisaCurriculo = new PesquisaCurriculo();
                if (SetInstance(dr, pesquisaCurriculo))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            pesquisaCurriculo = null;
            return false;
        }
        #endregion

        #region RecuperarDadosPesquisaCurriculo
        public static void RecuperarDadosPesquisaCurriculo(string funcao, string cidade, string palavraChave, int? IdUsuarioFilialPerfilLogadoEmpresa, int? IdUsuarioFilialPerfilLogadoCandidato, int? IdCurriculo, out BLL.PesquisaCurriculo objPesquisaCurriculo)
        {
            objPesquisaCurriculo = new BLL.PesquisaCurriculo();

            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue || IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                objPesquisaCurriculo.UsuarioFilialPerfil = new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? IdUsuarioFilialPerfilLogadoEmpresa.Value : IdUsuarioFilialPerfilLogadoCandidato.Value);

            if (IdCurriculo.HasValue)
                objPesquisaCurriculo.Curriculo = new Curriculo(IdCurriculo.Value);

            //objPesquisaCurriculo.DescricaoIP = Common.Helper.RecuperarIP();

            Cidade objCidade;
            if (Cidade.CarregarPorNome(cidade, out objCidade))
            {
                objPesquisaCurriculo.Cidade = objCidade;
                objPesquisaCurriculo.Estado = objCidade.Estado;
                //CidadeMaster.Value = txtCidadeMaster.Text = objCidade.ToString();
            }

            objPesquisaCurriculo.DescricaoPalavraChave = palavraChave;
            objPesquisaCurriculo.FlagPesquisaAvancada = false;

            var listaFuncao = new List<PesquisaCurriculoFuncao>();

            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(funcao, out objFuncao))
            {
                listaFuncao.Add(new PesquisaCurriculoFuncao { Funcao = objFuncao });

                //FuncaoMaster.Value = txtFuncaoMaster.Text = objFuncao.DescricaoFuncao;
            }

            objPesquisaCurriculo.Salvar(listaFuncao);
        }
        #endregion

        #region NotificarQuantidadeInsuficienteDeCurriculo
        /// <summary>
        /// Método que vai notificar uma pessoa (parametrizado) sobre pesquisas de currículo que não tiveram a quantidade suficiente de resultado.
        /// </summary>
        public static void NotificarQuantidadeInsuficienteDeCurriculo()
        {
            string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);
            string emailDestinatario = Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeMinimaCurriculoResultadoBuscaDestinatario);

            string assunto;
            var templateMensagem = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.AlertaPoucosCvsNaPesquisa, out assunto);

            var gerencial = MontarHTMLPesquisaCurriculo(RecuperarQuantidadeInsuficienteDeCurriculo(Enumeradores.FuncaoCategoria.Gestao));
            var especialista = MontarHTMLPesquisaCurriculo(RecuperarQuantidadeInsuficienteDeCurriculo(Enumeradores.FuncaoCategoria.Especialista));
            var apoio = MontarHTMLPesquisaCurriculo(RecuperarQuantidadeInsuficienteDeCurriculo(Enumeradores.FuncaoCategoria.Apoio));
            var operacao = MontarHTMLPesquisaCurriculo(RecuperarQuantidadeInsuficienteDeCurriculo(Enumeradores.FuncaoCategoria.Operacao));

            var parametros = new
            {
                Gerencial = gerencial,
                Especialista = especialista,
                Apoio = apoio,
                Operacao = operacao
            };

            var mensagem = FormatObject.ToString(parametros, templateMensagem);

            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem, Enumeradores.CartaEmail.AlertaPoucosCvsNaPesquisa, emailRemetente, emailDestinatario);
        }
        #endregion

        #region MontarHTMLPesquisaCurriculo
        /// <summary>
        /// Monta o inner html de uma pesquisa
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private static string MontarHTMLPesquisaCurriculo(List<PesquisaCurriculoQuantidadeInsuficiente> lista)
        {
            var sb = new StringBuilder();

            foreach (var item in lista)
            {
                sb.Append("<tr>");
                sb.Append("<td style='display: block' width='500'>");
                sb.AppendFormat("•{0}: {1}", "Parâmetros", item);
                sb.Append("</td>");
                sb.Append("</tr>");
            }

            return sb.ToString();
        }
        #endregion

        #region RecuperarQuantidadeInsuficienteDeCurriculo
        /// <summary>
        /// Retorna as ultimas pesquisas (7 dias) que não tiveram a quantidade ideal de currículos em seu retorno (parametro por categoria de função)
        /// </summary>
        /// <param name="enumerador"></param>
        /// <returns></returns>
        public static List<PesquisaCurriculoQuantidadeInsuficiente> RecuperarQuantidadeInsuficienteDeCurriculo(Enumeradores.FuncaoCategoria enumerador)
        {
            var lista = new List<PesquisaCurriculoQuantidadeInsuficiente>();

            int quantidade = 0;
            switch (enumerador)
            {
                case Enumeradores.FuncaoCategoria.Gestao:
                    quantidade = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeMinimaCurriculoGerencialResultadoBusca));
                    break;
                case Enumeradores.FuncaoCategoria.Especialista:
                    quantidade = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeMinimaCurriculoEspecialistaResultadoBusca));
                    break;
                case Enumeradores.FuncaoCategoria.Operacao:
                    quantidade = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeMinimaCurriculoOperacionalResultadoBusca));
                    break;
                case Enumeradores.FuncaoCategoria.Apoio:
                    quantidade = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeMinimaCurriculoApoioResultadoBusca));
                    break;
            }
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@QuantidadeMinima", SqlDbType = SqlDbType.Int, Size = 4, Value = quantidade },
                    new SqlParameter{ ParameterName = "@CategoriaFuncao", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)enumerador }
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "SP_Alerta_Poucos_Cvs_Busca", parms))
            {
                while (dr.Read())
                {
                    var obj = new PesquisaCurriculoQuantidadeInsuficiente();
                    if (dr["Des_Funcao"] != DBNull.Value)
                    {
                        var funcao = Convert.ToString(dr["Des_Funcao"]);
                        obj.Funcao = funcao.Remove(funcao.Length - 2);
                    }
                    if (dr["Nme_Cidade"] != DBNull.Value)
                        obj.Cidade = Convert.ToString(dr["Nme_Cidade"]);
                    if (dr["Des_Palavra_Chave"] != DBNull.Value)
                        obj.PalavraChave = Convert.ToString(dr["Des_Palavra_Chave"]);
                    if (dr["Sig_Estado"] != DBNull.Value)
                        obj.SiglaEstado = Convert.ToString(dr["Sig_Estado"]);
                    if (dr["Des_Disponibilidade"] != DBNull.Value)
                        obj.SiglaEstado = Convert.ToString(dr["Des_Disponibilidade"]);
                    if (dr["Des_Idioma"] != DBNull.Value)
                        obj.SiglaEstado = Convert.ToString(dr["Des_Idioma"]);
                    if (dr["Des_Sexo"] != DBNull.Value)
                        obj.Sexo = Convert.ToString(dr["Des_Sexo"]);
                    if (dr["Des_BNE"] != DBNull.Value)
                        obj.Escolaridade = Convert.ToString(dr["Des_BNE"]);
                    if (dr["Des_Logradouro"] != DBNull.Value)
                        obj.Logradouro = Convert.ToString(dr["Des_Logradouro"]);
                    if (dr["Des_Bairro"] != DBNull.Value)
                        obj.Bairro = Convert.ToString(dr["Des_Bairro"]);
                    if (dr["Des_Curso_Tecnico_Graduacao"] != DBNull.Value)
                        obj.CursoTecnicoGraduacao = Convert.ToString(dr["Des_Curso_Tecnico_Graduacao"]);
                    if (dr["Nme_Fonte_Tecnico_Graduacao"] != DBNull.Value)
                        obj.NomeFonteTecnicoGraduacao = Convert.ToString(dr["Nme_Fonte_Tecnico_Graduacao"]);
                    if (dr["Des_Curso_Outros_Cursos"] != DBNull.Value)
                        obj.CursoOutros = Convert.ToString(dr["Des_Curso_Outros_Cursos"]);
                    if (dr["Nme_Fonte_Outros"] != DBNull.Value)
                        obj.NomeFonteOutros = Convert.ToString(dr["Nme_Fonte_Outros"]);
                    if (dr["Raz_Social"] != DBNull.Value)
                        obj.Empresa = Convert.ToString(dr["Raz_Social"]);
                    if (dr["Des_Area_BNE"] != DBNull.Value)
                        obj.AreaEmpresa = Convert.ToString(dr["Des_Area_BNE"]);
                    if (dr["Des_Estado_Civil"] != DBNull.Value)
                        obj.EstadoCivil = Convert.ToString(dr["Des_Estado_Civil"]);
                    if (dr["Des_Raca_BNE"] != DBNull.Value)
                        obj.Raca = Convert.ToString(dr["Des_Raca_BNE"]);
                    if (dr["Des_Deficiencia"] != DBNull.Value)
                        obj.Deficiencia = Convert.ToString(dr["Des_Deficiencia"]);
                    if (dr["Des_Categoria_Habilitacao"] != DBNull.Value)
                        obj.CategoriaHabilitacao = Convert.ToString(dr["Des_Categoria_Habilitacao"]);
                    if (dr["Des_Tipo_Veiculo"] != DBNull.Value)
                        obj.TipoVeiculo = Convert.ToString(dr["Des_Tipo_Veiculo"]);
                    if (dr["Flg_Filhos"] != DBNull.Value)
                        obj.PossuiFilhos = Convert.ToBoolean(dr["Flg_Filhos"]);
                    if (dr["Num_CEP_Min"] != DBNull.Value)
                        obj.NumeroCEPMinimo = Convert.ToString(dr["Num_CEP_Min"]);
                    if (dr["Num_CEP_Max"] != DBNull.Value)
                        obj.NumeroCEPMaximo = Convert.ToString(dr["Num_CEP_Max"]);
                    if (dr["Num_Idade_Min"] != DBNull.Value)
                        obj.IdadeMinima = Convert.ToInt16(dr["Num_Idade_Min"]);
                    if (dr["Num_Idade_Max"] != DBNull.Value)
                        obj.IdadeMaxima = Convert.ToInt16(dr["Num_Idade_Max"]);
                    if (dr["Num_Salario_Min"] != DBNull.Value)
                        obj.SalarioMinimo = Convert.ToDecimal(dr["Num_Salario_Min"]);
                    if (dr["Num_Salario_Max"] != DBNull.Value)
                        obj.SalarioMaximo = Convert.ToDecimal(dr["Num_Salario_Max"]);

                    lista.Add(obj);
                }
            }

            return lista;
        }
        #endregion

        #region [QuantidadePesquisaRealizadas]
        /// <summary>
        /// Recupera a quantidades de pesquisas de curriculos realizada por uma filial
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static int QuantidadePesquisaRealizadas(int idFilial)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial }
                };
            int total = 0;
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spQuantidadePesquisaRealizadas, parms))
            {
                while (dr.Read())
                {
                    total = Convert.ToInt32(dr["Total"]);
                }
            }
            return total;
        }
        #endregion

        #region [QuantidadeCurriculosNaoVisualizados]
        /// <summary>
        /// Retora a quantidades de curriculos não visualizados da ultima pesquisa feita pela filial.
        /// </summary>
        /// <param name="idFilial"></param>
        /// <param name="idfPesquisa"></param>
        /// <returns></returns>
        public static int QuantidadeCurriculosNaoVisualizados(int idFilial, out int? idfPesquisa)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial }
                };
            int total = 0;
            idfPesquisa = null;
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spQuantidadeCurriculosNaoVisualizados, parms))
            {
                if (dr.Read())
                {
                    total = Convert.ToInt32(dr["qtd"]);
                    idfPesquisa = Convert.ToInt32(dr["Idf_Pesquisa_Curriculo"]);
                }
            }
            return total;
        }
        #endregion

        #region [CarregarFiltrosPesquisa]
        public static DataTable CarregarFiltrosPesquisa(PesquisaCurriculo objPesquisaCurriculo, Dictionary<String, int> listFiltros, int TotalRegistro)
        {

            var dt = new DataTable();
            dt.Columns.Add("Filtro", typeof(string));
            dt.Columns.Add("Coluna", typeof(string));
            dt.Columns.Add("qtdCvs", typeof(int));
            dt.Columns.Add("FiltroValor", typeof(string));

            if (listFiltros.Count <= 0 || objPesquisaCurriculo == null)
                return dt;
            objPesquisaCurriculo.CompletarObjetoPesquisa();

            #region [Pretensão do Candidato]
            var dispon = PesquisaCurriculoDisponibilidade.ListarDisponibilidadePorPesquisa(objPesquisaCurriculo);
            foreach (var item in dispon)
            {
                dt.Rows.Add("Disponibilidade", $"Idf_Disponibilidade{item.Key}", (from disp in listFiltros where disp.Key.Contains($"Idf_Disponibilidade{item.Key}") select disp.Value).FirstOrDefault() - TotalRegistro, item.Value);
            }
            if (objPesquisaCurriculo.NumeroSalarioMin.HasValue)
                dt.Rows.Add("Salário Mínimo", "NumeroSalarioMin", (from item in listFiltros where item.Key.Contains("Vlr_Pretensao_Salarial_Min") select item).FirstOrDefault().Value, objPesquisaCurriculo.NumeroSalarioMin.Value);
            if (objPesquisaCurriculo.NumeroSalarioMax.HasValue)
                dt.Rows.Add("Salário Máximo", "NumeroSalarioMax", (from item in listFiltros where item.Key.Contains("Vlr_Pretensao_Salarial_Max") select item).FirstOrDefault().Value, objPesquisaCurriculo.NumeroSalarioMax.Value);

            #endregion

            #region [Região e Localidade]
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoLogradouro))
                dt.Rows.Add("Endereço", "DescricaoLogradouro", (from item in listFiltros where item.Key.Contains("Des_Logradouro") select item).FirstOrDefault().Value, objPesquisaCurriculo.DescricaoLogradouro);
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoBairro))
                dt.Rows.Add("Bairro", "DescricaoBairro", (from item in listFiltros where item.Key.Contains("Des_Bairro") select item).FirstOrDefault().Value, objPesquisaCurriculo.DescricaoBairro);
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.NumeroCEPMin))
                dt.Rows.Add("CEP Mìnima", "NumeroCEPMin", (from item in listFiltros where item.Key.Contains("Num_CEP_Inicio") select item.Value).FirstOrDefault(), objPesquisaCurriculo.NumeroCEPMin);
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.NumeroCEPMax))
                dt.Rows.Add("CEP Máxima", "NumeroCEPMax", (from item in listFiltros where item.Key.Contains("Num_CEP_Fim") select item.Value).FirstOrDefault(), objPesquisaCurriculo.NumeroCEPMax);

            #endregion

            #region [Definir Palavra-Chave]
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoPalavraChave))
            {
                var list = objPesquisaCurriculo.DescricaoPalavraChave.Split(',');
                for (int i = 0; i < list.Length; i++)
                    dt.Rows.Add($"Incluir Palavra Chave: {list[i]} - {(from it in listFiltros where it.Equals(i) select it.Value).FirstOrDefault() }", $"DescricaoPalavraChave,{list[i]}");

            }
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoFiltroExcludente))
                dt.Rows.Add($"Excluir Palavra-Chave: {objPesquisaCurriculo.DescricaoFiltroExcludente}", "DescricaoFiltroExcludente");
            #endregion

            #region [Formação e Escolaridade]
            if (objPesquisaCurriculo.Escolaridade != null)
                dt.Rows.Add("Escolaridade Mínima", "Escolaridade", (from item in listFiltros where item.Key.Contains("Idf_Escolaridade") select item.Value).FirstOrDefault(), objPesquisaCurriculo.Escolaridade.DescricaoBNE);
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao))
                dt.Rows.Add("Técnico / Graduação", "DescricaoCursoTecnicoGraduacao", (from item in listFiltros where item.Key.Contains("Des_Curso") select item.Value).FirstOrDefault(), objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao);
            if (objPesquisaCurriculo.CursoTecnicoGraduacao != null)
                dt.Rows.Add("Técnico / Graduação", "CursoTecnicoGraduacao", (from item in listFiltros where item.Key.Contains("Des_Curso") select item.Value).FirstOrDefault(), objPesquisaCurriculo.CursoTecnicoGraduacao.DescricaoCurso);
            if (objPesquisaCurriculo.CursoOutrosCursos != null)
                dt.Rows.Add("Outros Cursos", "CursoOutrosCursos", (from item in listFiltros where item.Key.Contains("Des_Curso_Outro") select item.Value).FirstOrDefault(), objPesquisaCurriculo.CursoOutrosCursos.DescricaoCurso);
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCursoOutrosCursos))
                dt.Rows.Add("Outros Cursos", "DescricaoCursoOutrosCursos", (from item in listFiltros where item.Key.Contains("Des_Curso_Outro") select item.Value).FirstOrDefault(), objPesquisaCurriculo.DescricaoCursoOutrosCursos);
            if (objPesquisaCurriculo.FonteTecnicoGraduacao != null)
                dt.Rows.Add("Instituição", "FonteTecnicoGraduacao", (from item in listFiltros where item.Key.Contains("Idf_Fonte") select item.Value).FirstOrDefault(), objPesquisaCurriculo.FonteTecnicoGraduacao.NomeFonte);
            var ListIdioma = PesquisaCurriculoIdioma.ListarIdiomaPesquisaFiltro(objPesquisaCurriculo);
            //foreach (var idioma in ListIdioma)
            //{
            //    dt.Rows.Add("Idioma", $"Idf_Idioma{idioma.Key}", (from item in listFiltros where item.Key.Contains($"Idf_Idioma{idioma.Key}") select item.Value).FirstOrDefault(), idioma.Value);
            //}



            #endregion

            #region [Expêriencia do Candidato]
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.RazaoSocial))
                dt.Rows.Add("Empresa", "RazaoSocial", (from item in listFiltros where item.Key.Contains("Raz_Social") select item.Value).FirstOrDefault(), objPesquisaCurriculo.RazaoSocial);
            if (objPesquisaCurriculo.AreaBNE != null)
                dt.Rows.Add("Área", "AreaBNE", (from item in listFiltros where item.Key.Contains("Idf_Area_BNE") select item.Value).FirstOrDefault(), objPesquisaCurriculo.AreaBNE.DescricaoAreaBNE);
            #endregion

            #region [Características]
            if (objPesquisaCurriculo.NumeroIdadeMin.HasValue)
                dt.Rows.Add("Idade Mínima", "NumeroIdadeMin", (from item in listFiltros where item.Key.Contains("Idade_Min") select item.Value).FirstOrDefault(), objPesquisaCurriculo.NumeroIdadeMin.Value);
            if (objPesquisaCurriculo.NumeroIdadeMax.HasValue)
                dt.Rows.Add("Idade Máxima", "NumeroIdadeMax", (from item in listFiltros where item.Key.Contains("Idade_Max") select item.Value).FirstOrDefault(), objPesquisaCurriculo.NumeroIdadeMax.Value);

            if (objPesquisaCurriculo.Sexo != null)
                dt.Rows.Add("Sexo", "Sexo", (from item in listFiltros where item.Key.Contains("Idf_Sexo") select item).FirstOrDefault().Value, objPesquisaCurriculo.Sexo.IdSexo.Equals((int)Enumeradores.Sexo.Masculino) ? Enumeradores.Sexo.Masculino : Enumeradores.Sexo.Feminino);


            if (objPesquisaCurriculo.Raca != null)
                dt.Rows.Add("Raça", "Raca", (from item in listFiltros where item.Key.Contains("Idf_Raca") select item.Value).FirstOrDefault(), objPesquisaCurriculo.Raca.DescricaoRaca);
            if (objPesquisaCurriculo.CategoriaHabilitacao != null)
                dt.Rows.Add("Categoria habilitação", "CategoriaHabilitacao", (from item in listFiltros where item.Key.Contains("Idf_Categoria_Habilitacao") select item.Value).FirstOrDefault(), objPesquisaCurriculo.CategoriaHabilitacao.DescricaoCategoriaHabilitacao);
            if (objPesquisaCurriculo.FlagFilhos.HasValue)
                dt.Rows.Add("Filhos", "FlagFilhos", (from item in listFiltros where item.Key.Contains("Flg_Filhos") select item.Value).FirstOrDefault(), (objPesquisaCurriculo.FlagFilhos.Value ? "Sim" : "Não"));
            if (objPesquisaCurriculo.TipoVeiculo != null)
                dt.Rows.Add("Veiculo", "TipoVeiculo", (from item in listFiltros where item.Key.Contains("Idf_tipo_Veiculo") select item.Value).FirstOrDefault(), objPesquisaCurriculo.TipoVeiculo.DescricaoTipoVeiculo);
            if (objPesquisaCurriculo.EstadoCivil != null)
                dt.Rows.Add("Estado Civil", "EstadoCivil", (from item in listFiltros where item.Key.Contains("Idf_Estado_Civil") select item.Value).FirstOrDefault(), objPesquisaCurriculo.EstadoCivil.DescricaoEstadoCivil);
            if (objPesquisaCurriculo.Deficiencia != null)
                dt.Rows.Add("PCD", "Deficiencia", 0, objPesquisaCurriculo.Deficiencia.DescricaoDeficiencia);

            #endregion

            #region [Candidato Especifico]
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCodCPFNome))
                dt.Rows.Add("Nome, CPF ou Código CV", "DescricaoCodCPFNome", 0, objPesquisaCurriculo.DescricaoCodCPFNome);
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.EmailPessoa))
                dt.Rows.Add("E-mail", "EmailPessoa", (from item in listFiltros where item.Key.Contains("Eml_Pessoa") select item.Value).FirstOrDefault(), objPesquisaCurriculo.EmailPessoa);
            if (!string.IsNullOrEmpty(objPesquisaCurriculo.NumeroDDDTelefone) && !string.IsNullOrEmpty(objPesquisaCurriculo.NumeroTelefone))
                dt.Rows.Add("Telefone", "NumeroDDDTelefone", (from item in listFiltros where item.Key.Contains("Num_Telefone") || item.Key.Contains("Num_DDD_Telefone") select item.Value).Sum(), Helper.FormatarTelefone(objPesquisaCurriculo.NumeroDDDTelefone, objPesquisaCurriculo.NumeroTelefone));

            #endregion

            #region [Avaliação do Candidato]

            #endregion

            var remover = dt.Select("qtdCvs <= 0");
            foreach (DataRow row in remover)
                dt.Rows.Remove(row);

            return dt;

        }
        #endregion

        #region [CompletarObjetoPesquisa]
        private void CompletarObjetoPesquisa()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName ="@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Value = this.IdPesquisaCurriculo }
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spCompletarObjetoPesquisa, parms))
            {
                if (dr.Read())
                {
                    if (this.Raca != null)
                        this.Raca.DescricaoRaca = dr["des_raca"].ToString();
                    if (this.CategoriaHabilitacao != null)
                        this.CategoriaHabilitacao.DescricaoCategoriaHabilitacao = dr["des_categoria_habilitacao"].ToString();
                    if (this.TipoVeiculo != null)
                        this.TipoVeiculo.DescricaoTipoVeiculo = dr["des_tipo_veiculo"].ToString();
                    if (this.Deficiencia != null)
                        this.Deficiencia.DescricaoDeficiencia = dr["des_deficiencia"].ToString();
                    if (this.AreaBNE != null)
                        this.AreaBNE.DescricaoAreaBNE = dr["des_area_bne"].ToString();
                    if (this.EstadoCivil != null)
                        this.EstadoCivil.DescricaoEstadoCivil = dr["Des_Estado_Civil"].ToString();
                    if (this.Escolaridade != null)
                        this.Escolaridade.DescricaoBNE = dr["Des_bne"].ToString();
                    if (this.CursoFormacao != null)
                        this.CursoFormacao.DescricaoCurso = dr["des_curso"].ToString();
                    if (this.FonteTecnicoGraduacao != null)
                        this.FonteTecnicoGraduacao.NomeFonte = dr["nme_fonte"].ToString();
                }
            }
        }
        #endregion

        #region [CandidatoDaVaga]

        #region RecuperarCurriculosRelacionadosVaga
        /// <summary>
        /// Método utilizado na tela de Pesquisa de Curriculo, retorna todos os currículos relacionados a um rastreador de CV
        /// </summary>
        /// <param name="paginaAtual"> </param>
        /// <param name="idFilial"></param>
        /// <param name="idVaga"></param>
        /// <param name="numeroRegistros"></param>
        /// <param name="mediaSalarial"></param>
        /// <param name="tamanhoPagina"> </param>
        /// <returns></returns>
        public static DataTable RecuperarCurriculosRelacionadosVaga(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil, FiltroCurriculoVaga filtros, out int numeroRegistros, out decimal mediaSalarial, out bool SemFiltro)
        {
            //Mudar para o solr
            //Pegar no banco os curriculos da vaga 

            DataTable resultado = new DataTable();
            mediaSalarial = 0;
            numeroRegistros = 0;
            try
            {//busca no solr
                resultado = BuscaCurriculoVaga(tamanhoPagina, paginaAtual, idFilial, idUsuarioFilialPerfil, filtros, idVaga, null, out mediaSalarial, out numeroRegistros);

            }
            catch (Exception ex)
            {//busca no sql
                SemFiltro = true;
                return RecuperarCurriculosRelacionadosVaga(tamanhoPagina, paginaAtual, idFilial, idVaga, idUsuarioFilialPerfil, out numeroRegistros, out mediaSalarial);
            }
            SemFiltro = false;
            //
            if (resultado != null && resultado.Rows.Count > 0)
            {
                using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            //Passar essa parte para quando indexa no solr.
                            //ver se esta dentro do perfil
                            for (int i = 0; i < resultado.Rows.Count; i++)
                            {
                                if (CandidatoNoPerfil(Convert.ToInt32(resultado.Rows[i]["Idf_Curriculo"]), idVaga, trans))
                                    resultado.Rows[i]["Dentro_Perfil"] = 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex, "Erro ao buscar os candidatos da vaga");
                        }
                    }
                    conn.Close();
                }
                return resultado;
            }

            return resultado;
        }
        #endregion

        private static bool CandidatoNoPerfil(int idCurriculo, int idVaga, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo },
                                new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                            };
            using (var dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, spCandidatoNoPerfil, parms))
            {
                if (dr.Read())
                {
                    if (Convert.ToBoolean(dr["Dentro_Perfil"]))
                        return true;
                }
                return false;
            }

        }

        #endregion

        #region RecuperarCurriculosRelacionadosVaga
        /// <summary>
        /// Método utilizado na tela de Pesquisa de Curriculo, retorna todos os currículos relacionados a um rastreador de CV
        /// </summary>
        /// <param name="paginaAtual"> </param>
        /// <param name="idFilial"></param>
        /// <param name="idVaga"></param>
        /// <param name="numeroRegistros"></param>
        /// <param name="mediaSalarial"></param>
        /// <param name="tamanhoPagina"> </param>
        /// <returns></returns>
        public static DataTable RecuperarCurriculosRelacionadosVaga(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil, out int numeroRegistros, out decimal mediaSalarial)
        {
            //Configurando parametros basicos da pesquisa
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@PaginaAtual", SqlDbType = SqlDbType.Int, Size = 4, Value = paginaAtual } ,
                                new SqlParameter { ParameterName = "@TamanhoPagina", SqlDbType = SqlDbType.Int, Size = 4, Value = tamanhoPagina } ,
                                new SqlParameter { ParameterName = "@TotalRegistros", SqlDbType = SqlDbType.Int, Size = 4, Value = 0, Direction = ParameterDirection.Output} ,
                                new SqlParameter { ParameterName = "@MediaSalarial", SqlDbType = SqlDbType.Decimal, Value = 0, Direction = ParameterDirection.Output } ,
                                new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = idFilial },
                                new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                            };

            if (idUsuarioFilialPerfil.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idUsuarioFilialPerfil });

            var dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BUSCA_CURRICULO_VAGA_VISUALIZADO", parms).Tables[0];

            numeroRegistros = Convert.ToInt32(parms[2].Value);

            mediaSalarial = parms[3].Value != DBNull.Value ? Convert.ToDecimal(parms[3].Value) : 0M;

            return dt;
        }
        #endregion

        #region NotificarAtualizarCVPesquisaNumeroInsuficiente

        /// <summary>
        /// Recuperar lista com as pesquisas com poucos(parametro) currículos retornados na pesquisa.
        /// </summary>
        /// <returns></returns>
        public static List<PesquisaCurriculoQuantidadeInsuficiente> RecuperarPesquisasComPoucosCurriculos()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@QuantidadeMinima", SqlDbType = SqlDbType.Int, Size = 4, Value = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeMinimaCurriculoEspecialistaResultadoBusca)) }
            };

            var lista = new List<PesquisaCurriculoQuantidadeInsuficiente>();
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spRecuperarPesquisaComPoucosCurriculos, parms))
            {
                while (dr.Read())
                {
                    var obj = new PesquisaCurriculoQuantidadeInsuficiente();

                    if (dr["Nme_Cidade"] != DBNull.Value)
                        obj.Cidade = Convert.ToString(dr["Nme_Cidade"]);
                    if (dr["Sig_Estado"] != DBNull.Value)
                        obj.SiglaEstado = Convert.ToString(dr["Sig_Estado"]);
                    obj.IdfCidade = Convert.ToInt32(dr["Idf_Cidade"]);
                    obj.IdfArea = Convert.ToInt32(dr["Idf_Area_BNE"]);
                    obj.AreaFuncao = dr["Des_Area_BNE"].ToString();
                    obj.FuncaoCategoria = dr["Des_Funcao_Categoria"].ToString();

                    lista.Add(obj);
                }
            }

            return lista;
        }

        public static List<PesquisaCurriculoQuantidadeInsuficienteCurriculo> RecuperarCurriculosParaPesquisaComPoucosCurriculos(PesquisaCurriculoQuantidadeInsuficiente pesquisaCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Value =  pesquisaCurriculo.IdfCidade },
                new SqlParameter {ParameterName = "@Idf_Area_BNE", SqlDbType = SqlDbType.Int, Value = pesquisaCurriculo.IdfArea },
                new SqlParameter {ParameterName = "@Qtd_Dias", SqlDbType = SqlDbType.Int, Value =  Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.DiasSemAtualizarCurriculo))}
            };

            var lista = new List<PesquisaCurriculoQuantidadeInsuficienteCurriculo>();
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spPoucosCandidatos, parms))
            {
                while (dr.Read())
                {
                    lista.Add(new PesquisaCurriculoQuantidadeInsuficienteCurriculo
                    {
                        PrimeiroNome = Helper.RetornarPrimeiroNome(dr["Nme_Pessoa"].ToString()),
                        Email = Convert.ToString(dr["Eml_Pessoa"]),
                        CPF = Convert.ToDecimal(dr["Num_CPF"]),
                        DataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"])
                    });
                }
            }

            return lista;
        }

        #endregion

        #region [NotificacaoPesquisaCurriculoAtendimento]
        /// <summary>
        /// 
        /// </summary>
        public static void NotificacaoPesquisaCurriculoAtendimento(DateTime DataInicio, DateTime DataFim)
        {
            try
            {
                //parametros
                string EmailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailRemetentePesquisaCurriculo);
                var objCarta = CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.PesquisasRealizadasPelaEmpresa);
                var QtdeEmpresas = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QtdEmpresasVisualizaoCvCompleta));
                GridView GridView1 = new GridView();

                List<SqlParameter> parametros = new List<SqlParameter>()
            {
                new SqlParameter {ParameterName = "@QuantidadeEmpresa", SqlDbType = SqlDbType.Int, Value = QtdeEmpresas },
                new SqlParameter {ParameterName = "@Dta_Inicio", SqlDbType = SqlDbType.DateTime, Value = DataInicio  },
                new SqlParameter {ParameterName = "@Dta_Fim", SqlDbType = SqlDbType.DateTime,Value = DataFim }
            };
                StringBuilder sbTablelinha = new StringBuilder();
                var linha = "<tr><td>{CNPJ} </td><td>{RazaoSocial}</td><td>{Telefone}</td><td>{Plano}</td><td>{Vendedor}</td><td>{Quantidade}</td></tr>";
                using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spNotificacaoPesquisaCurriculoAtendimento, parametros))
                {
                    while (dr.Read())
                    {
                        sbTablelinha.Append(linha.Replace("{CNPJ}", dr["num_cnpj"].ToString())
                            .Replace("{RazaoSocial}", dr["raz_social"].ToString())
                            .Replace("{Telefone}", Helper.FormatarTelefone(dr["num_ddd_comercial"].ToString(), dr["num_comercial"].ToString()))
                            .Replace("{Plano}", dr["des_plano"].ToString())
                            .Replace("{Quantidade}", dr["Total"].ToString())
                            .Replace("{Vendedor}", Filial.Vendedor(Convert.ToDecimal(dr["num_cnpj"])).NomeVendedor));
                    }
                }

                objCarta.Assunto = objCarta.Assunto.Replace("{Data}", DataFim.ToString("dd/MM/yyyy"));
                objCarta.Conteudo = objCarta.Conteudo.Replace("{LinhasTable}", sbTablelinha.ToString()).Replace("{Data1}", DataInicio.ToString("dd/MM/yyyy HH:mm:ss"))
                    .Replace("{Data2}", DataFim.ToString("dd/MM/yyyy HH:mm:ss"))
                    .Replace("{QuantidadeEmpresa}", QtdeEmpresas.ToString());

                ///Enviar Email
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                    .Enviar(objCarta.Assunto, objCarta.Conteudo, Enumeradores.CartaEmail.PesquisasRealizadasPelaEmpresa, EmailRemetente,
                        EmailRemetente);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, " erro ao enviar e-mail NotificacaoPesquisaCurriculoAtendimento");
            }

        }
        #endregion

    }
}