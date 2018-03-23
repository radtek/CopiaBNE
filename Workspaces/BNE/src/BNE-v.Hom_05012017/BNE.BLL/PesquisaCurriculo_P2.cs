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
                                LEFT JOIN BNE.BNE_Curriculo_Quem_Me_Viu qmv WITH ( NOLOCK ) ON cvs.Idf_Curriculo = qmv.Idf_Curriculo  AND qmv.Idf_Filial = 75145--@Idf_Filial
                           WHERE    pesq.Idf_Pesquisa_Curriculo = cvs.Idf_Pesquisa_Curriculo
                                AND qmv.Idf_Curriculo_Quem_Me_Viu IS NULL
                          ) cvs
                WHERE  ufp.Idf_Filial = @Idf_Filial
                and pesq.idf_cidade is not null
                    AND qtd > 1
                ORDER BY pesq.Dta_Cadastro DESC, pcf.idf_pesquisa_curriculo_funcao asc";
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
        public static DataTable BuscaCurriculo(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo, out int numeroRegistros, out decimal mediaSalarial, bool buscaCurriculosMaisUmAno = false)
        {
            return BuscaCurriculo(tamanhoPagina, paginaAtual, idOrigem, idFilial, idUsuarioFilialPerfil, objPesquisaCurriculo, null, null, out numeroRegistros, out mediaSalarial, null, buscaCurriculosMaisUmAno);
        }
        public static DataTable BuscaCurriculo(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, RastreadorCurriculo objRastreadorCurriculo, DateTime? dataUltimaVisualizacaoRastreador, out int numeroRegistros, out decimal mediaSalarial)
        {
            return BuscaCurriculo(tamanhoPagina, paginaAtual, idOrigem, idFilial, idUsuarioFilialPerfil, null, objRastreadorCurriculo, null, out numeroRegistros, out mediaSalarial, dataUltimaVisualizacaoRastreador);
        }
        public static DataTable BuscaCurriculo(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, Vaga objVaga, out int numeroRegistros, out decimal mediaSalarial)
        {
            return BuscaCurriculo(tamanhoPagina, paginaAtual, idOrigem, idFilial, idUsuarioFilialPerfil, null, null, objVaga, out numeroRegistros, out mediaSalarial);
        }
        private static DataTable BuscaCurriculo(int tamanhoPagina, int paginaAtual, int idOrigem, int? idFilial, int? idUsuarioFilialPerfil, PesquisaCurriculo objPesquisaCurriculo, RastreadorCurriculo objRastreadorCurriculo, Vaga objVaga, out int numeroRegistros, out decimal mediaSalarial, DateTime? dataUltimaVisualizacaoRastreador = null, bool buscaCurriculosMaisUmAno = false)
        {
            Filial objFilial = null;
            UsuarioFilialPerfil objUsuarioFilialPerfil = null;

            if (idFilial.HasValue)
                objFilial = new Filial(idFilial.Value);

            if (idUsuarioFilialPerfil.HasValue)
                objUsuarioFilialPerfil = new UsuarioFilialPerfil(idUsuarioFilialPerfil.Value);

            SolrHighlightResponse<CurriculoCompleto.Response> resultado = null;
            if (objPesquisaCurriculo != null)
            {
                resultado = CurriculoCompleto.EfetuarRequisicao(Custom.BuscaCurriculo.MontarQuery(paginaAtual, tamanhoPagina, objFilial, objUsuarioFilialPerfil, objPesquisaCurriculo, buscaCurriculosMaisUmAno));
            }
            else if (objRastreadorCurriculo != null)
            {
                resultado = CurriculoCompleto.EfetuarRequisicao(Custom.BuscaCurriculo.MontarQuery(paginaAtual, tamanhoPagina, objRastreadorCurriculo, dataUltimaVisualizacaoRastreador));
            }
            else if (objVaga != null)
            {
                resultado = CurriculoCompleto.EfetuarRequisicao(Custom.BuscaCurriculo.MontarQuery(paginaAtual, tamanhoPagina, objFilial, objUsuarioFilialPerfil, objVaga));
            }

            var tableCvsSolr = RecuperarDataTable();

            SolrHighlightResponse<CurriculoCompleto.Response> resultadoCvClassificacao = null;
            SolrHighlightResponse<CurriculoCompleto.Response> resultadoCvSms = null;

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
                            1,//int dentro do perfil
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
                            quantidadeExperienciaFuncaoPretendida5
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
        public static DataTable RecuperarCurriculosNaoVisualisadosVaga(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil, out int numeroRegistros, out decimal mediaSalarial)
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
        public static DataTable RecuperarCurriculosNaoVisualisadosVagaCampanha(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil, out int numeroRegistros, out decimal mediaSalarial)
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

            var dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BUSCA_CURRICULO_VAGA_NAO_VISUALIZADO_CAMPANHA", parms).Tables[0];

            numeroRegistros = Convert.ToInt32(parms[2].Value);

            mediaSalarial = parms[3].Value != DBNull.Value ? Convert.ToDecimal(parms[3].Value) : 0M;

            return dt;
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
        public static DataTable RecuperarCurriculosRelacionadosVagaCampanha(int tamanhoPagina, int paginaAtual, int idFilial, int idVaga, int? idUsuarioFilialPerfil, out int numeroRegistros, out decimal mediaSalarial)
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

            var dt = DataAccessLayer.ExecuteReaderDs(CommandType.StoredProcedure, "BUSCA_CURRICULO_VAGA_VISUALIZADO_CAMPANHA ", parms).Tables[0];

            numeroRegistros = Convert.ToInt32(parms[2].Value);

            mediaSalarial = parms[3].Value != DBNull.Value ? Convert.ToDecimal(parms[3].Value) : 0M;

            return dt;
        }
        #endregion

        #region Salvar
        public void Salvar(List<PesquisaCurriculoFuncao> listaFuncao)
        {
            Salvar(listaFuncao, new List<PesquisaCurriculoIdioma>(), new List<PesquisaCurriculoDisponibilidade>(), new List<CampoPalavraChavePesquisaCurriculo>());
        }
        public void Salvar(List<PesquisaCurriculoFuncao> listaFuncao, List<PesquisaCurriculoIdioma> listaIdioma, List<PesquisaCurriculoDisponibilidade> listaDisponibilidade, List<CampoPalavraChavePesquisaCurriculo> listaPalavraChave, bool queroContratarEstagiarios = false, bool queroContratarAprendiz = false)
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

            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(assunto, mensagem,Enumeradores.CartaEmail.AlertaPoucosCvsNaPesquisa, emailRemetente, emailDestinatario);
        }
        #endregion

        #region MontarHTMLPesquisaCurriculo
        /// <summary>
        /// Monta o inner html de uma pesquisa
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        private static string MontarHTMLPesquisaCurriculo(List<PesquisaCurriculoQuantidadeInsuficienteCurriculo> lista)
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
        public static List<PesquisaCurriculoQuantidadeInsuficienteCurriculo> RecuperarQuantidadeInsuficienteDeCurriculo(Enumeradores.FuncaoCategoria enumerador)
        {
            var lista = new List<PesquisaCurriculoQuantidadeInsuficienteCurriculo>();

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
                    var obj = new PesquisaCurriculoQuantidadeInsuficienteCurriculo();
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
    }
}