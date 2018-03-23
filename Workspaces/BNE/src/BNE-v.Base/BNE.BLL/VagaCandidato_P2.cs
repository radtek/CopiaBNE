//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.BLL.Custom;
using System.Globalization;
using BNE.BLL.Enumeradores;
using BNE.BLL.Custom.Email;

namespace BNE.BLL
{
    public partial class VagaCandidato // Tabela: BNE_Vaga_Candidato
    {

        #region Consultas
        private const string Spselectporvagacurriculo = @"
        SELECT * FROM BNE_Vaga_Candidato WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga AND Idf_Curriculo = @Idf_Curriculo";


        private const string Spselectvagacandidatoporfilial = @"
        SELECT  COUNT(C.Idf_Vaga_Candidato)
        FROM    BNE.BNE_Vaga V WITH ( NOLOCK )
                INNER JOIN BNE.BNE_Vaga_Candidato C WITH ( NOLOCK ) ON C.Idf_Vaga = V.Idf_Vaga
                LEFT JOIN BNE.BNE_Curriculo_Visualizacao cv WITH ( NOLOCK ) ON cv.Idf_Curriculo = C.Idf_Curriculo AND cv.Idf_Filial = @Idf_Filial
        WHERE   V.Idf_Filial = @Idf_Filial
                AND V.Flg_Vaga_Arquivada = 0
                AND V.Flg_Inativo = 0
                AND V.Flg_Vaga_Rapida = 0
                AND cv.Idf_Curriculo_Visualizacao IS NULL";


        private const string Spvagacandidatoquantidade = @"
        SELECT  COUNT(VC.Idf_Curriculo)
        FROM    BNE_Vaga_Candidato VC WITH(NOLOCK)
        WHERE   VC.Idf_Curriculo = @Idf_Curriculo 
                AND VC.Flg_Inativo = 0";

        private const string Spvagacandidatoporcurriculo = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
        SELECT	ROW_NUMBER() OVER (ORDER BY VC.Dta_Cadastro DESC) AS RowID,
		        VC.Dta_Cadastro ,
		        F.Idf_Filial,
		        V.Idf_Vaga,
                FUN.Idf_Funcao,
		        FUN.Des_Funcao ,
		        CASE WHEN Vlr_Salario_De > 0 AND Vlr_Salario_Para > 0
			        THEN ''R$ '' + REPLACE(Vlr_Salario_De, ''.'', '','') + '' a '' + ''R$ '' + REPLACE(Vlr_Salario_Para, ''.'', '','')
			        ELSE ''A combinar''
		        END AS Vlr_Salario ,                                                                                   
		        ( CASE 
                   WHEN ( V.Flg_Confidencial = 1 )
				        THEN ''Empresa Confidencial''
		           WHEN ( O.Idf_Tipo_Origem = 3 )
                        THEN V.Nme_Empresa
                   ELSE F.Raz_Social
		        END ) AS Raz_Social ,
		        ( CASE WHEN ( V.Flg_Confidencial = 1 ) THEN 1 
			          ELSE 0 -- Visible = true
		        END ) AS Img_Empresa_Confidencial_Visible ,
                C.Flg_VIP
        FROM    BNE.BNE_Vaga_Candidato VC WITH(NOLOCK)
		        JOIN BNE.BNE_Curriculo C WITH(NOLOCK) ON VC.Idf_Curriculo = C.Idf_Curriculo
		        JOIN BNE.BNE_Vaga V WITH(NOLOCK) ON VC.Idf_Vaga = V.Idf_Vaga
                JOIN BNE.TAB_Origem O WITH(NOLOCK) ON O.Idf_Origem = V.Idf_Origem
		        JOIN plataforma.TAB_Funcao FUN WITH(NOLOCK) ON V.Idf_Funcao = FUN.Idf_Funcao
		        JOIN BNE.TAB_Filial F WITH(NOLOCK) ON V.Idf_Filial = F.Idf_Filial
           LEFT JOIN BNE.BNE_Vaga_Candidato VC2 ON VC2.Idf_Curriculo = C.Idf_Curriculo AND C.Flg_VIP = 0
				 AND VC2.Flg_Auto_Candidatura = 1 AND VC2.Idf_Vaga = VC.Idf_Vaga
        WHERE   VC.Flg_Inativo = 0 AND VC.Idf_Curriculo = ' + CONVERT(VARCHAR, @Idf_Curriculo) + '
          AND VC2.Idf_Vaga IS NULL'

        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect	+ ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";

        private const string Spcurriculosvagaporstatus = @"
        SELECT  VC.Idf_Vaga_Candidato ,
                VC.Idf_Vaga ,
                VC.Idf_Curriculo ,
                V.Eml_Vaga ,
                F.Des_Funcao ,
                V.Idf_Usuario_Filial_Perfil
        FROM    BNE.BNE_Vaga_Candidato VC WITH(NOLOCK)
                JOIN BNE.BNE_Vaga V WITH(NOLOCK) ON VC.Idf_Vaga = V.Idf_Vaga
                JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON V.Idf_Funcao = F.Idf_Funcao
        WHERE   VC.Idf_Status_Curriculo_Vaga = @Idf_Status_Curriculo_Vaga
                AND VC.Flg_Inativo = 0
        ORDER BY V.Idf_Vaga DESC
        ";

        private const string Spvagacandidatoporvaga = @"
        SELECT  * 
        FROM    BNE.BNE_Vaga_Candidato WITH(NOLOCK)
        WHERE   Flg_Inativo = 0 
                AND Idf_Vaga = @Idf_Vaga
                AND Idf_Status_Curriculo_Vaga = @Idf_Status_Curriculo_Vaga
        ";

        private const string Spselectcurriculojacandidatou = @"
        SELECT count(*) FROM BNE_Vaga_Candidato WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga AND Idf_Curriculo = @Idf_Curriculo";

        private const string SPCANDIDATAAUTOMATICAMENTE = @"SP_CANDIDATA_AUTOMATICAMENTE";

        #region Spselectvagasporfilial
        private const string Spselectvagasporfilial = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)

        SET @FirstRec = @CurrentPage * @PageSize + 1
        SET @LastRec = @CurrentPage * @PageSize + @PageSize

        SET @iSelect = '
            SELECT 
			        ROW_NUMBER() OVER (ORDER BY Flg_Vaga_Arquivada, V.Dta_Cadastro DESC) AS RowID ,
                    V.Idf_Vaga ,
                    ( BNE.BNE_FUNCAO_VAGA_ORIGEM('+ ISNULL(Convert(VARCHAR,@Idf_Origem), '''''') + ',V.Idf_Vaga) ) AS Des_Funcao, 
                    V.Cod_Vaga ,
                    CONVERT(VARCHAR, CONVERT(DATETIME, V.Dta_Prazo, 103), 103) AS Dta_Prazo ,
                    (   
				        SELECT  COUNT (VCSub.Idf_Vaga_Candidato) 
                        FROM    bne.BNE_Vaga_Candidato VCSub WITH(NOLOCK)
                        WHERE   VCSub.Idf_Vaga = V.Idf_Vaga
                                AND VCSub.Flg_Inativo = 0) as Num_Cvs_Recebidos ,
                    V.Dta_Cadastro ,
                    C.Nme_Cidade ,
                    C.Sig_Estado ,
                    Flg_Vaga_Arquivada ,
                    ( 
				        SELECT  COUNT (VCSub.Idf_Vaga_Candidato) 
                        FROM    bne.BNE_Vaga_Candidato VCSub WITH(NOLOCK)
                        WHERE   VCSub.Idf_Vaga = V.Idf_Vaga 
						        AND VCSub.Idf_Curriculo = ' + CONVERT(VARCHAR, @Idf_Curriculo) + '
                                AND VCSub.Flg_Inativo = 0
                    ) Ja_Candidatou
            FROM    bne.BNE_Vaga V WITH(NOLOCK)
                    LEFT JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON F.Idf_Funcao = V.Idf_Funcao
                    INNER JOIN plataforma.TAB_Cidade C WITH(NOLOCK) ON C.Idf_Cidade = V.Idf_Cidade
            WHERE 
                    V.Idf_Filial = ' + CONVERT(VARCHAR, @Idf_Filial) + '
                    AND V.Flg_Inativo = 0
                    AND V.Flg_Vaga_Rapida = 0
                    AND Flg_Vaga_Arquivada = 0 '

        IF (@ids IS NOT NULL)
            SET @iSelect = @iSelect + ' AND V.Idf_Funcao IN ( '+ @ids +' ) '

        SET @iSelectCount = 'Select Count(1) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect  + ' ) As TblTempPag	Where RowID >= ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #endregion

        #region Métodos

        #region CarregarPorVagaCurriculo
        /// <summary>
        /// Método responsável por carregar uma instancia de VagaCandidato através do
        /// identificar de uma vaga e um curriculo
        /// </summary>
        /// <param name="idVaga">Identificador da Vaga</param>
        /// <param name="idCurriculo">Identificador de Curriculo</param>
        /// <param name="objVagaCandidato">Parametro out VagaCandidato </param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorVagaCurriculo(int idVaga, int idCurriculo, out VagaCandidato objVagaCandidato)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4), /* 0 */
                                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4) /* 1 */ 
                            };
            parms[0].Value = idVaga;
            parms[1].Value = idCurriculo;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporvagacurriculo, parms))
            {
                objVagaCandidato = new VagaCandidato();
                if (SetInstance(dr, objVagaCandidato))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objVagaCandidato = null;
            return false;
        }
        #endregion

        #region Salvar

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objVaga"></param>
        /// <param name="objCurriculo"></param>
        /// <param name="objOrigem">Como as vagas na pesquisa dentro do STC podem vir do BNE, a origem é passada como parametro, para relacionar o CV a origem do STC</param>
        /// <param name="descricaoIP"></param>
        /// <param name="objIntencaoFilial"></param>
        /// <param name="listVagaPergunta"></param>
        public void Salvar(Vaga objVaga, Curriculo objCurriculo, Origem objOrigem, string descricaoIP, IntencaoFilial objIntencaoFilial, List<VagaCandidatoPergunta> listVagaPergunta, bool siteSTC, bool siteSTCCandidaturaLivre, bool avulsaMobile, out int? quantidadeCandidaturasGratis)
        {
            quantidadeCandidaturasGratis = null;
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var podeCandidatar = false;
                        ParametroCurriculo objParametroCurriculo = null;

                        if (objCurriculo.FlagVIP || objVaga.FlagBNERecomenda || (siteSTC && siteSTCCandidaturaLivre) || avulsaMobile || objVaga.Filial.PossuiPlanoAtivo() || PlanoAdquiridoDetalhesCurriculo.RecebeuCampanhaVagaPerfil(objCurriculo, objVaga, trans)) //Se flag vip ou bne recomenda pode candidatar
                            podeCandidatar = true;
                        else
                        {
                            if (siteSTCCandidaturaLivre)
                            {
                                if (objOrigem != null && objVaga.Origem.IdOrigem.Equals(objOrigem.IdOrigem))
                                    podeCandidatar = true;
                                else
                                    quantidadeCandidaturasGratis = 0;
                            }
                            else
                            {
                                //Se tem o parametro, verifica se possui saldo
                                if (ParametroCurriculo.CarregarParametroPorCurriculo(Enumeradores.Parametro.QuantidadeCandidaturaDegustacao, objCurriculo, out objParametroCurriculo, trans))
                                {
                                    var quantidadeCandidaturas = Convert.ToInt32(objParametroCurriculo.ValorParametro);
                                    if (quantidadeCandidaturas > 0)
                                    {
                                        quantidadeCandidaturasGratis = quantidadeCandidaturas;
                                        int candidaturasRestantes = quantidadeCandidaturas - 1;
                                        podeCandidatar = true;
                                        objParametroCurriculo.ValorParametro = candidaturasRestantes.ToString(); //Desconta a candidatura atual
                                    }
                                    else
                                    {
                                        objParametroCurriculo = null;
                                        quantidadeCandidaturasGratis = 0;
                                    }
                                }
                                else
                                {
                                    quantidadeCandidaturasGratis = 0;
                                }
                            }
                        }

                        //Salvando o saldo para o currículo atual
                        if (objParametroCurriculo != null)
                            objParametroCurriculo.Save(trans);

                        if (podeCandidatar) //Se pode candidatar (é vip ou tem saldo)
                        {
                            if (objOrigem != null) //Utilizado quando a Origem é Minha Empresa Oferece Cursos, onde vagas do BNE aparecem na pesquisa de vaga
                            {
                                if (!CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, objOrigem, trans))
                                {
                                    var objCurriculoOrigem = new CurriculoOrigem
                                        {
                                            Curriculo = objCurriculo,
                                            Origem = objOrigem,
                                            DescricaoIP = descricaoIP
                                        };
                                    objCurriculoOrigem.Save(trans);

                                    PlanoAdquirido.ConcederPlanoPF(objCurriculo, new Plano(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoVIPUniversitarioGratis))), trans);
                                }
                            }
                            else
                            {
                                if (!CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, objVaga.Origem, trans))
                                {
                                    var objCurriculoOrigem = new CurriculoOrigem
                                        {
                                            Curriculo = objCurriculo,
                                            Origem = objVaga.Origem,
                                            DescricaoIP = descricaoIP
                                        };
                                    objCurriculoOrigem.Save(trans);
                                }
                            }

                            objIntencaoFilial.Save(trans);

                            StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio);
                            //Se a vaga exigir envio de todos os curriculos, um service irá enviar o email nestes casos
                            if (Convert.ToBoolean(objVaga.FlagReceberTodosCV) && objVaga.FlagReceberTodosCV.HasValue)
                            {
                                //Pode ser que uma vaga esteja ainda indexada no GOOGLE, não deve enviar e-mail para a empresa
                                if (objVaga.FlagVagaArquivada || objVaga.FlagInativo)
                                    StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio);
                                else
                                    StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.AguardoEnvio);
                            }
                            Save(trans);

                            if (listVagaPergunta.Count > 0)
                            {
                                foreach (VagaCandidatoPergunta objVagaCandidatoPergunta in listVagaPergunta)
                                    objVagaCandidatoPergunta.Save(trans);
                            }

                            #region FlagReceberCadaCV - Enviar Email
                            //Se a vaga estiver ativa
                            if (!objVaga.FlagVagaArquivada && !objVaga.FlagInativo)
                            {
                                //Se a vaga exigir envio de cada curriculo por email                        
                                if (objVaga.FlagReceberCadaCV.HasValue && objVaga.FlagReceberCadaCV.Value)
                                {
                                    Vaga.CompleteObject();
                                    Vaga.Funcao.CompleteObject();

                                    string curriculoCandidato;

                                    if (objVaga.Origem.IdOrigem.Equals((int)Enumeradores.Origem.BNE))
                                        curriculoCandidato = objCurriculo.RecuperarHTMLCurriculo(false, false, true, FormatoVisualizacaoImpressao.Empresa);
                                    else
                                        curriculoCandidato = objCurriculo.RecuperarHTMLCurriculo(true, true, FormatoVisualizacaoImpressao.Empresa);

                                    EnviarLinkCurriculosCandidatoVaga(new List<int> { objCurriculo.IdCurriculo }, IdVagaCandidato, Vaga.IdVaga, Vaga.Funcao.DescricaoFuncao, Vaga.EmailVaga, Vaga.UsuarioFilialPerfil.IdUsuarioFilialPerfil, curriculoCandidato, trans);
                                }
                            }
                            #endregion

                            #region Integração de Candidatura
                            //Verificando se a vaga tem integração
                            //Se tiver, efetua a requisição na url parametrizada
                            VagaIntegracao objVagaIntegracao;
                            if (VagaIntegracao.RecuperarIntegradorPorVaga(objVaga.IdVaga, out objVagaIntegracao))
                            {
                                String urlCandidatura = objVagaIntegracao.Integrador.GetValorParametro(Enumeradores.Parametro.Integracao_Url_Retorno_Candidatura);
                                if (!String.IsNullOrEmpty(urlCandidatura))
                                {
                                    BLL.Custom.IntegrationObjects.Bne bneIntObj = new Custom.IntegrationObjects.Bne(objVagaIntegracao.CodigoVagaIntegrador, objCurriculo.IdCurriculo);
                                    String enviaCV = String.Empty;
                                    if (objCurriculo.FlagVIP)
                                    {
                                        enviaCV = objVagaIntegracao.Integrador.GetValorParametro(Enumeradores.Parametro.Integracao_Flg_Envia_CV_Vip);
                                    }
                                    else
                                    {
                                        enviaCV = objVagaIntegracao.Integrador.GetValorParametro(Enumeradores.Parametro.Integracao_Flg_Envia_CV_Nao_Vip);
                                    }
                                    if (!String.IsNullOrEmpty(enviaCV) && (enviaCV == "1" || enviaCV == "true"))
                                    {
                                        bneIntObj.Candidatura.Curriculo.CompletarCurriculo(objCurriculo);
                                    }

                                    bneIntObj.Enviar(urlCandidatura);
                                }
                            }
                            #endregion
                        }
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

        #region CandidatoVagaPorFilial
        /// <summary>
        /// Carrega a quantidade de curriculos candidatados para as vagas de uma determinada empresa.
        /// </summary>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        public static int CandidatoVagaPorFilial(int idFilial)
        {
            var parms = new List<SqlParameter>
                            {
                                    new SqlParameter("@Idf_Filial", SqlDbType.Int, 4)   /* 0 */
                            };
            parms[0].Value = idFilial;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectvagacandidatoporfilial, parms));
        }

        #endregion

        #region QuantidadeVagaCandidato

        public static int QuantidadeVagaCandidato(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
                            };
            parms[0].Value = idCurriculo;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spvagacandidatoquantidade, parms));
        }

        #endregion

        #region CarregarVagaCandidatadaPorCurriculo
        public static DataTable CarregarVagaCandidatadaPorCurriculo(int idCurriculo, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4),
                                new SqlParameter("@CurrentPage", SqlDbType.Int, 4),
                                new SqlParameter("@PageSize", SqlDbType.Int, 4)
                            };

            parms[0].Value = idCurriculo;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spvagacandidatoporcurriculo, parms))
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

        #region CarregarCandidatoVagaPorVaga
        private static List<VagaCandidato> CarregarCandidatoVagaPorVaga(int idVaga, Enumeradores.StatusCurriculoVaga statusCurriculoVaga)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4),
                                new SqlParameter("@Idf_Status_Curriculo_Vaga", SqlDbType.Int, 4)
                            };

            parms[0].Value = idVaga;
            parms[1].Value = statusCurriculoVaga.GetHashCode();

            var lstVagaCandidato = new List<VagaCandidato>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spvagacandidatoporvaga, parms))
            {
                SetInstanceList(dr, lstVagaCandidato);
            }

            return lstVagaCandidato;
        }
        #endregion

        #region SetInstanceList
        private static void SetInstanceList(IDataReader dr, List<VagaCandidato> lstVagaCandidato)
        {
            try
            {
                while (dr.Read())
                {
                    var objVagaCandidato = new VagaCandidato
                                               {
                                                   _idVagaCandidato = Convert.ToInt32(dr["Idf_Vaga_Candidato"]),
                                                   _curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"])),
                                                   _vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"])),
                                                   _dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]),
                                                   _flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]),
                                                   _statusCurriculoVaga = new StatusCurriculoVaga(Convert.ToInt32(dr["Idf_Status_Curriculo_Vaga"])),
                                                   _persisted = true,
                                                   _modified = false
                                               };

                    lstVagaCandidato.Add(objVagaCandidato);
                }
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

        #region EnviarCurriculosAguardandoEnvio
        /// <summary>
        /// Envia os links dos curriculos candidatados para as vagas com a situação aguardo envio
        /// </summary>
        /// <returns></returns>
        public static void EnviarCurriculosAguardandoEnvio()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var parms = new List<SqlParameter>
                                        {
                                            new SqlParameter("@Idf_Status_Curriculo_Vaga", SqlDbType.Int, 4)
                                        };
                        parms[0].Value = (int)Enumeradores.StatusCurriculoVaga.AguardoEnvio;

                        var lstCurriculos = new List<int>();

                        //Seleciona todos os curriculos candidatados as vagas com a situação aguardando envio
                        using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spcurriculosvagaporstatus, parms))
                        {
                            #region Variaveis auxiliares
                            bool primeiraExecucao = true;
                            int idVaga = 0;
                            string desFuncao = string.Empty;
                            string emailVaga = string.Empty;
                            int idUsuarioFilialPerfil = 0;
                            #endregion

                            while (dr.Read())
                            {
                                if (primeiraExecucao)
                                {
                                    idVaga = Convert.ToInt32(dr["Idf_Vaga"]);
                                    primeiraExecucao = false;
                                    lstCurriculos.Add(Convert.ToInt32(dr["Idf_Curriculo"]));
                                }
                                else
                                {
                                    if (idVaga != Convert.ToInt32(dr["Idf_Vaga"]))
                                    {
                                        EnviarLinkCurriculosCandidatoVaga(lstCurriculos,
                                            null,
                                            idVaga,
                                            desFuncao,
                                            emailVaga,
                                            idUsuarioFilialPerfil,
                                            string.Empty,
                                            trans);
                                        idVaga = Convert.ToInt32(dr["Idf_Vaga"]);
                                        lstCurriculos = new List<int>
                                                            {
                                                                Convert.ToInt32(dr["Idf_Curriculo"])
                                                            };
                                    }
                                    else
                                        lstCurriculos.Add(Convert.ToInt32(dr["Idf_Curriculo"]));

                                }

                                desFuncao = Convert.ToString(dr["Des_Funcao"]);
                                emailVaga = Convert.ToString(dr["Eml_Vaga"]);
                                idUsuarioFilialPerfil = Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]);
                            }

                            //Caso a leitura do reader tenha terminado e ainda tenha curriculos na lista
                            if (lstCurriculos.Count > 0)
                            {
                                EnviarLinkCurriculosCandidatoVaga(lstCurriculos,
                                    null,
                                    idVaga,
                                    desFuncao,
                                    emailVaga,
                                    idUsuarioFilialPerfil,
                                    string.Empty,
                                    trans);
                            }

                            if (dr.IsClosed)
                                dr.Close();
                            dr.Dispose();
                        }
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

        #region EnviarLinkCurriculosCandidatoVaga
        /// <summary>
        /// Metodo responsável por enviar os links dos curriculos por email e atualizar a statusCurriculoVaga para Curriculo enviado
        /// </summary>
        /// <param name="lstCurriculos">lista com idCurriculo</param>
        /// <param name="idVagaCandidato">id da Vaga Candidato</param>
        /// <param name="idVaga">id da Vaga</param>
        /// <param name="desFuncao">Descrição da Função</param>
        /// <param name="emailDestinatario">Email da Vaga</param>
        /// <param name="idUsuarioFilialPerfilDes">UsuarioFilialPerfil da Vaga</param>
        /// <param name="curriculoCandidato"> </param>
        /// <param name="trans">Transaction</param>
        public static void EnviarLinkCurriculosCandidatoVaga(List<int> lstCurriculos, int? idVagaCandidato, int idVaga, string desFuncao, string emailDestinatario, int idUsuarioFilialPerfilDes, string curriculoCandidato, SqlTransaction trans)
        {
            #region MensagemCS


            string templateAssunto;
            string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.LinkVisualizacaoCurriculo, out templateAssunto);

            Vaga objVaga = Vaga.LoadObject(idVaga, trans);
            objVaga.Filial.CompleteObject(trans);

            string linkCurriculo = string.IsNullOrEmpty(curriculoCandidato) ? Curriculo.RetornaLinkVisualizacaoCurriculo(lstCurriculos) : curriculoCandidato;

            var parametros = new
            {
                RazaoSocial = objVaga.Filial.RazaoSocial,
                NomeFuncao = desFuncao,
                LinkCurriculo = linkCurriculo
            };
            string assunto = parametros.ToString(templateAssunto);
            string mensagem = parametros.ToString(template);
            string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, trans);


            try
            {
                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Smtp)
                    .Enviar(assunto, mensagem, emailRemetente, emailDestinatario);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha ao enviar mensagem");
            }
            #endregion

            #region VagaCandidato

            if (idVagaCandidato.HasValue)
            {
                VagaCandidato objVagaCandidato = LoadObject(idVagaCandidato.Value, trans);
                objVagaCandidato.StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.Enviado);
                objVagaCandidato.Save(trans);
            }
            else
            {
                List<VagaCandidato> lstCandidato = CarregarCandidatoVagaPorVaga(idVaga, Enumeradores.StatusCurriculoVaga.AguardoEnvio);

                if (lstCandidato.Count > 0)
                {
                    foreach (VagaCandidato objVagaCandidato in lstCandidato)
                    {
                        objVagaCandidato.StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.Enviado);
                        objVagaCandidato.Save(trans);
                    }
                }
            }

            #endregion
        }
        #endregion

        #region CurriculoJaCandidatouVaga
        /// <summary>
        /// Método responsável por verificar se um curriculo já candidatou a uma vaga
        /// </summary>
        /// <param name="objVaga">Identificador da Vaga</param>
        /// <param name="objCurriculo">Identificador de Curriculo</param>
        /// <returns>Boolean</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CurriculoJaCandidatouVaga(Curriculo objCurriculo, Vaga objVaga)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4), /* 0 */
                                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4) /* 1 */ 
                            };
            parms[0].Value = objVaga.IdVaga;
            parms[1].Value = objCurriculo.IdCurriculo;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectcurriculojacandidatou, parms)) > 0;
        }
        #endregion

        #region AssociarCurriculoVaga
        /// <summary>
        /// Metodo utilizado para associar um currículo a uma vaga.
        /// </summary>
        public static bool AssociarCurriculoVaga(Curriculo objCurriculo, Vaga objVaga)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        VagaCandidato objVagaCandidato;
                        if (CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato))
                        {
                            objVagaCandidato.FlagInativo = false;
                        }
                        else
                        {
                            objVagaCandidato = new VagaCandidato
                                {
                                    Curriculo = objCurriculo,
                                    Vaga = objVaga,
                                    StatusCurriculoVaga = new StatusCurriculoVaga((int)Enumeradores.StatusCurriculoVaga.SemEnvio),
                                    FlagAutoCandidatura = false
                                };
                        }

                        objVagaCandidato.Save(trans);

                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }
        #endregion

        #region ListarVagasFilialDT
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static DataTable ListarVagasFilial(int idCurriculo, int idFilial, int? idOrigem, int paginaCorrente, int tamanhoPagina, List<int> listIdFuncoes, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Filial", SqlDbType.Int, 4), 
                    new SqlParameter("@CurrentPage", SqlDbType.Int, 4), 
                    new SqlParameter("@PageSize", SqlDbType.Int, 4), 
                    new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
                };

            parms[0].Value = idFilial;
            parms[1].Value = paginaCorrente;
            parms[2].Value = tamanhoPagina;
            parms[3].Value = idCurriculo;

            var param = new SqlParameter("@ids", SqlDbType.VarChar, 1600);
            if (listIdFuncoes != null && listIdFuncoes.Count > 0)
                param.Value = String.Join(",", listIdFuncoes.Select(item => item.ToString(CultureInfo.CurrentCulture)).ToArray());
            else
                param.Value = DBNull.Value;

            parms.Add(param);

            var parm = new SqlParameter("@Idf_Origem", SqlDbType.Int, 4);

            if (idOrigem.HasValue)
                parm.Value = idOrigem.Value;
            else
                parm.Value = DBNull.Value;

            parms.Add(parm);

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectvagasporfilial, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);

                    if (!dr.IsClosed)
                        dr.Close();
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

        #region Inativar
        public void Inativar()
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        FlagInativo = true;
                        Save(trans);

                        var objPessoaFisica = new PessoaFisica(PessoaFisica.RecuperarIdPorCurriculo(Curriculo));
                        var emailDestinatario = objPessoaFisica.EmailPessoa;

                        if (!string.IsNullOrEmpty(emailDestinatario)) //Se o candidato tem email 
                        {
                            //TODO: Analisar performance: Método estatico apenas para verificar se a vaga é confidencial ou não
                            var dtoVaga = DTO.Vaga.CarregarVaga(Vaga.IdVaga, trans); //Recupera um objeto com todos os dados da vaga

                            if (!dtoVaga.FlagConfidencial) //Se a vaga não é confidencial
                            {
                                if (Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.EnviaCartaAgradecimentoCandidatura, trans))) //Se é para enviar a carta de agradecimento para todos os clientes
                                {
                                    string cartaFilial;
                                    ParametroFilial objParametroFilial;
                                    if (ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.EnviaCartaAgradecimentoCandidatura, new Filial(dtoVaga.IdEmpresa), out objParametroFilial, trans))
                                        cartaFilial = objParametroFilial.ValorParametro; //Recuperando o valor setado no banco
                                    else //Se não possuir parametro específico, recupera a carta padrão
                                        cartaFilial = CartaEmail.RecuperarConteudo(Enumeradores.CartaEmail.AgradecimentoCandidatura, trans);

                                    if (!string.IsNullOrEmpty(cartaFilial)) //Se tiver valor manda a carta
                                    {
                                        string templateAssunto;
                                        var templateMensagem = CartaEmail.RetornarConteudoEmpresa(Enumeradores.CartaEmail.AgradecimentoCandidatura, cartaFilial, dtoVaga.CNPJEmpresa, dtoVaga.NomeEmpresa, out templateAssunto);

                                        //Criando os parametros da mensagem e montando a partir do template
                                        var parametros = new
                                            {
                                                NomeCandidato = objPessoaFisica.NomeCompleto,
                                                NomeFuncao = dtoVaga.DescricaoFuncao,
                                                NomeSelecionadora = dtoVaga.NomeSelecionadora,
                                                NomeEmpresaSelecionadora = dtoVaga.NomeEmpresa,
                                                DadosVaga = CartaEmail.MontarExtratoVaga(dtoVaga)
                                            };
                                        var descricaoMensagem = parametros.ToString(templateMensagem);
                                        var descricaoAssunto = parametros.ToString(templateAssunto);

                                        var emailRemetente = dtoVaga.EmailVaga;

                                        EmailSenderFactory
                                            .Create(TipoEnviadorEmail.Fila)
                                            .Enviar(descricaoAssunto, descricaoMensagem, emailRemetente, emailDestinatario, trans);
                                    }
                                }
                            }
                        }

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
        public static void Inativar(Vaga objVaga, List<int> list)
        {
            foreach (var idCurriculo in list)
            {
                try
                {
                    VagaCandidato objVagaCandidato;
                    if (CarregarPorVagaCurriculo(objVaga.IdVaga, idCurriculo, out objVagaCandidato))
                        objVagaCandidato.Inativar();
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                }
            }
        }
        #endregion

        #region Candidatar
        public static bool Candidatar(Curriculo objCurriculo, Vaga objVaga, Origem objOrigem, List<VagaCandidatoPergunta> listPerguntas, string descricaoIP, bool siteSTC, bool siteSTCCandidaturaLivre, bool avulsaMobile, out int? quantidadeCandidaturasGratis)
        {
            bool retorno = false;

            quantidadeCandidaturasGratis = null;

            IntencaoFilial objIntencaoFilial;
            objVaga.CompleteObject();
            objCurriculo.CompleteObject();
            if (!IntencaoFilial.CarregarPorFilialCurriculo(objCurriculo.IdCurriculo, objVaga.Filial.IdFilial, out objIntencaoFilial))
            {
                objIntencaoFilial = new IntencaoFilial { Curriculo = objCurriculo, Filial = new Filial(objVaga.Filial.IdFilial) };
            }
            objIntencaoFilial.FlagInativo = false;

            VagaCandidato objVagaCandidato;
            if (!VagaCandidato.CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato))
            {
                objVagaCandidato = new VagaCandidato { Curriculo = objCurriculo, Vaga = objVaga, FlagInativo = false };

                foreach (VagaCandidatoPergunta objVagaCandidatoPergunta in listPerguntas)
                {
                    objVagaCandidatoPergunta.VagaCandidato = objVagaCandidato;
                }

                objVagaCandidato.Salvar(objVaga, objCurriculo, objOrigem, descricaoIP, objIntencaoFilial, listPerguntas, siteSTC, siteSTCCandidaturaLivre, avulsaMobile, out quantidadeCandidaturasGratis);
            }
            else
            {
                if (objVagaCandidato != null)
                {
                    if (objVagaCandidato.FlagAutoCandidatura == null)
                        objVagaCandidato.FlagAutoCandidatura = false;
                    if (Convert.ToBoolean(objVagaCandidato.FlagAutoCandidatura) && !objCurriculo.FlagVIP)
                        objVagaCandidato.FlagAutoCandidatura = false;
                    objVagaCandidato.Save();
                }
                else
                {
                    VagaCandidato.CarregarPorVagaCurriculo(objVaga.IdVaga, objCurriculo.IdCurriculo, out objVagaCandidato);
                    if (objVagaCandidato != null && !objCurriculo.FlagVIP)
                    {
                        if (Convert.ToBoolean(objVagaCandidato.FlagAutoCandidatura))
                            objVagaCandidato.FlagAutoCandidatura = false;
                        objVagaCandidato.Save();
                    }
                }
            }

            retorno = true;

            return retorno;
        }

        public static void CandidatarAuto(int Idf_Vaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();

            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int));

            parms[0].Value = Idf_Vaga;

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, SPCANDIDATAAUTOMATICAMENTE, parms);
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

        #endregion

    }
}