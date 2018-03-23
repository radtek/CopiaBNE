//-- Data: 08/04/2010 14:43
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.BLL.AsyncServices;
using BNE.BLL.DTO;
using BNE.EL;
using BNE.BLL.Enumeradores;
using BNE.BLL.Custom;
using System.Globalization;
using BNE.BLL.Custom.Email;
using BNE.Services.Base.ProcessosAssincronos;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;
using System.Net;

namespace BNE.BLL
{
    public partial class MensagemCS // Tabela: BNE_Mensagem_CS
    {

        #region Atributos

        private int? _idMensagemCSAnterior;
        private int? _idMensagemCSProxima;

        #endregion

        #region Propriedades

        #region IdMensagemCSAnterior
        public int? IdMensagemCSAnterior
        {
            get
            {
                return _idMensagemCSAnterior;
            }
            set
            {
                _idMensagemCSAnterior = value;
                _modified = true;
            }
        }
        #endregion

        #region IdMensagemProxima
        public int? IdMensagemProxima
        {
            get
            {
                return _idMensagemCSProxima;
            }
            set
            {
                _idMensagemCSProxima = value;
                _modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas

        #region Spcountmensagensrecebidas
        private const string Spcountmensagensrecebidas = @"
        SELECT  COUNT(Idf_Usuario_Filial_Des)
        FROM    BNE_Mensagem_CS WITH(NOLOCK)
        WHERE   Idf_Usuario_Filial_Des = @Idf_Usuario_Filial_Des 
                AND Flg_Inativo = 0
        ";
        #endregion

        #region Spcountmensagensrecebidas_naoLidas
        private const string Spcountmensagensrecebidas_naoLidas = @"
        SELECT  COUNT(Idf_Usuario_Filial_Des)
        FROM    BNE_Mensagem_CS WITH(NOLOCK)
        WHERE   Idf_Usuario_Filial_Des = @Idf_Usuario_Filial_Des 
                AND Flg_Inativo = 0
                AND Flg_Lido = 0 
        ";
        #endregion

        #region Spcountmensagensenviadas
        private const string Spcountmensagensenviadas = @"
        SELECT  COUNT(MCS.Idf_Usuario_Filial_Perfil)
        FROM    BNE.BNE_Mensagem_CS MCS WITH(NOLOCK)
		        LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON MCS.Idf_Usuario_Filial_Des = UFP.Idf_Usuario_Filial_Perfil
                LEFT JOIN BNE.Tab_Pessoa_Fisica TPF WITH(NOLOCK) ON TPF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
        WHERE   MCS.Idf_Usuario_Filial_Perfil =  @Idf_Usuario_Filial_Perfil
                AND MCS.Flg_Inativo = 0
                AND UFP.Idf_Filial IS NULL
        ";
        #endregion

        #region Spmensagemanteriorproximactemensagensenviadas
        private const string Spmensagemanteriorproximactemensagensenviadas = @"
        DECLARE @Query VARCHAR(8000)
        SET @Query = '
        ;WITH CTE AS ( 
        SELECT  ROW_NUMBER() OVER ( ORDER BY MCS.Dta_Cadastro DESC ) AS RowID ,
                MCS.*
        FROM    BNE.BNE_Mensagem_CS MCS WITH(NOLOCK)
				LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON MCS.Idf_Usuario_Filial_Des = UFP.Idf_Usuario_Filial_Perfil
				LEFT JOIN BNE.Tab_Pessoa_Fisica TPF WITH(NOLOCK) ON TPF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
		WHERE   MCS.Idf_Usuario_Filial_Perfil =' + CONVERT(VARCHAR, @Idf_Usuario_Filial_Perfil) + '
				AND MCS.Flg_Inativo = 0 '
				                                                             
		IF ( @Des_Filtro IS NOT NULL ) 
			SET @Query = @Query + ' AND ( CONVERT(VARCHAR, MCS.Dta_Cadastro, 103) = ''' + CONVERT(VARCHAR,@Des_Filtro) + '''
			                        OR MCS.Des_Mensagem LIKE ''%'+ CONVERT(VARCHAR,@Des_Filtro) + '%''											
			                        OR TPF.Nme_Pessoa LIKE ''%'+ CONVERT(VARCHAR,@Des_Filtro) + '%''
			                        OR MCS.Des_Email_Assunto LIKE ''%' + CONVERT(VARCHAR,@Des_Filtro) + '%'') )'
		ELSE 
			SET @Query = @Query + ') '  
				                     
		SET @Query = @Query + '
            SELECT  RowAtual.* ,
					RowAnterior.Idf_Mensagem_CS AS Idf_Anterior ,
					RowProxima.Idf_Mensagem_CS AS Idf_Proxima
			FROM    CTE AS RowAtual
					LEFT JOIN CTE AS RowAnterior ON RowAnterior.RowID = RowAtual.RowID - 1
					LEFT JOIN CTE AS RowProxima ON RowProxima.RowID = RowAtual.RowID + 1
			WHERE   RowAtual.Idf_Mensagem_CS = ' + CONVERT(VARCHAR, @Idf_Mensagem_CS)
    
        EXEC(@Query)
        ";
        #endregion

        #region Spmensagemanteriorproximactemensagensrecebidas
        private const string Spmensagemanteriorproximactemensagensrecebidas = @"
        DECLARE @Query VARCHAR(8000)
        SET @Query = '
        ;WITH CTE AS ( 
        SELECT  ROW_NUMBER() OVER ( ORDER BY MCS.Dta_Cadastro DESC ) AS RowID ,
                MCS.*
        FROM    BNE.BNE_Mensagem_CS MCS
                LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP ON MCS.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
                LEFT JOIN BNE.TAB_Filial F ON UFP.Idf_Filial = F.Idf_Filial
		WHERE   MCS.Idf_Usuario_Filial_Des =' + CONVERT(VARCHAR, @Idf_Usuario_Filial_Des) + '
				AND MCS.Flg_Inativo = 0 '
				                                                             
		IF ( @Des_Filtro IS NOT NULL ) 
			SET @Query = @Query + ' AND ( CONVERT(VARCHAR, MCS.Dta_Cadastro, 103) = ''' + CONVERT(VARCHAR,@Des_Filtro) + '''
				                    OR MCS.Des_Mensagem LIKE ''%'+ CONVERT(VARCHAR,@Des_Filtro) + '%''											
				                    OR F.Raz_Social LIKE ''%'+ CONVERT(VARCHAR,@Des_Filtro) + '%''
				                    OR MCS.Des_Email_Assunto LIKE ''%' + CONVERT(VARCHAR,@Des_Filtro) + '%'') )'
		ELSE 
			SET @Query = @Query + ') '  
				                     
		SET @Query = @Query + ' 
            SELECT  RowAtual.* ,
					RowAnterior.Idf_Mensagem_CS AS Idf_Anterior ,
					RowProxima.Idf_Mensagem_CS AS Idf_Proxima
			FROM    CTE AS RowAtual
					LEFT JOIN CTE AS RowAnterior ON RowAnterior.RowID = RowAtual.RowID - 1
					LEFT JOIN CTE AS RowProxima ON RowProxima.RowID = RowAtual.RowID + 1
			WHERE   RowAtual.Idf_Mensagem_CS = ' + CONVERT(VARCHAR, @Idf_Mensagem_CS)
    
        EXEC(@Query)
        ";
        #endregion

        #region Spmensagensrecebidas
        //TODO: Retirar processamento desnecess�rio na query SQL como o LEN + ...
        private const string Spmensagensrecebidas = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
                        SELECT 
                            ROW_NUMBER() OVER (ORDER BY MCS.Dta_Cadastro DESC) AS RowID,
                            Idf_Mensagem_CS,                                                                                    
                                ( CASE WHEN Idf_Tipo_Mensagem_CS = 1
                                    THEN CASE WHEN LEN(MCS.Des_Mensagem) >= 60
                                                THEN CONVERT(VARCHAR(60), MCS.Des_Mensagem)
                                                    + ''...''
                                                ELSE MCS.Des_Mensagem
                                            END
                                    ELSE CASE WHEN LEN(MCS.Des_Email_Assunto) >= 60
                                        THEN CONVERT(VARCHAR(60), MCS.Des_Email_Assunto)
                                            + ''...''
                                                ELSE MCS.Des_Email_Assunto
                                            END
                                  END ) AS Des_Email_Assunto,
                            Idf_Tipo_Mensagem_CS,
                            ( CASE WHEN LEN(F.Raz_Social) >= 60
                                THEN CONVERT(VARCHAR(60), F.Raz_Social)
                                    + ''...''
                                ELSE F.Raz_Social
                             END ) AS Remetente,
                            MCS.Des_Mensagem,
                            MCS.Dta_Cadastro,
                            MCS.Flg_Lido
                    FROM    BNE.BNE_Mensagem_CS MCS WITH(NOLOCK)
                            LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON MCS.Idf_Usuario_Filial_Des = UFP.Idf_Usuario_Filial_Perfil
                            LEFT JOIN BNE.TAB_Filial F WITH(NOLOCK) ON UFP.Idf_Filial = F.Idf_Filial
                    WHERE   MCS.Idf_Usuario_Filial_Des = ' + CONVERT(VARCHAR, @Idf_Usuario_Filial_Des) + '
                            AND MCS.Flg_Inativo = 0'

        IF(@Des_Filtro IS NOT NULL)
        BEGIN
            SET @iSelect = @iSelect + ' AND ( CONVERT(VARCHAR, MCS.Dta_Cadastro, 103) = ''' + CONVERT(VARCHAR,@Des_Filtro) + '''
                                        OR MCS.Des_Mensagem LIKE ''%'+ CONVERT(VARCHAR,@Des_Filtro) + '%''
                                        OR F.Raz_Social LIKE ''%'+ CONVERT(VARCHAR,@Des_Filtro) + '%''
                                        OR MCS.Des_Email_Assunto LIKE ''%' + CONVERT(VARCHAR,@Des_Filtro) + '%'')'
        END

        IF(@Flg_Usuario_Candidato = 0)
            SET @iSelect = @iSelect + ' AND UFP.Idf_Filial IS NOT NULL'
        ELSE
            SET @iSelect = @iSelect + ' AND UFP.Idf_Filial IS NULL'
                                                                    		
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)

        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #region Spmensagensenviadas
        private const string Spmensagensenviadas = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        SET @LastRec = ( @CurrentPage * @PageSize + 1 )
        SET @iSelect = '
        SELECT  ROW_NUMBER() OVER (ORDER BY MCS.Dta_Cadastro DESC) AS RowID,
                Idf_Mensagem_CS,
                Idf_Tipo_Mensagem_CS,                                                                                    
                ( CASE WHEN Idf_Tipo_Mensagem_CS = 1
                    THEN    CASE WHEN LEN(MCS.Des_Mensagem) >= 60
                                THEN CONVERT(VARCHAR(60), MCS.Des_Mensagem)
                                    + ''...''
                                ELSE MCS.Des_Mensagem
                            END
                    ELSE CASE WHEN LEN(MCS.Des_Email_Assunto) >= 60
                        THEN CONVERT(VARCHAR(60), MCS.Des_Email_Assunto)
                            + ''...''
                                ELSE MCS.Des_Email_Assunto
                            END
                END ) AS Des_Email_Assunto ,
                ( CASE WHEN LEN(TPF.Nme_Pessoa) >= 60
                                THEN CONVERT(VARCHAR(60), TPF.Nme_Pessoa)
                                    + ''...''
                                ELSE TPF.Nme_Pessoa
                             END ) AS Destinatario,
                MCS.Des_Mensagem,
                MCS.Dta_Cadastro,
                C.Idf_Curriculo 
        FROM    BNE.BNE_Mensagem_CS MCS WITH(NOLOCK)
                LEFT JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON MCS.Idf_Usuario_Filial_Des = UFP.Idf_Usuario_Filial_Perfil
                LEFT JOIN BNE.Tab_Pessoa_Fisica TPF WITH(NOLOCK) ON TPF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
                LEFT JOIN BNE.BNE_Curriculo C WITH(NOLOCK) ON TPF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
        WHERE   MCS.Idf_Usuario_Filial_Perfil = ' + CONVERT(VARCHAR, @Idf_Usuario_Filial_Perfil) + '
                AND MCS.Flg_Inativo = 0 
                AND MCS.Idf_Usuario_Filial_Perfil IS NOT NULL'                                                     

        IF(@Des_Filtro IS NOT NULL)
            BEGIN
                SET @iSelect = @iSelect + ' AND ( CONVERT(VARCHAR, MCS.Dta_Cadastro, 103) = ''' + CONVERT(VARCHAR,@Des_Filtro) + '''
								            OR	MCS.Des_Mensagem LIKE ''%'+ CONVERT(VARCHAR,@Des_Filtro) + '%''											
								            OR TPF.Nme_Pessoa LIKE ''%'+ CONVERT(VARCHAR,@Des_Filtro) + '%''
								            OR MCS.Des_Email_Assunto LIKE ''%' + CONVERT(VARCHAR,@Des_Filtro) + '%'')'
	        END                                                
                                                                    		
        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID > ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID < ' + CONVERT(VARCHAR, @LastRec)
        
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #region Spselectdataultimamensagemenviadafilialportipo
        private const string Spselectdataultimamensagemenviadafilialportipo = @"  
        SELECT  TOP 1 M.Dta_Cadastro, Des_Mensagem 
        FROM    BNE_Mensagem_CS M WITH(NOLOCK)
                INNER JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON M.Idf_Usuario_Filial_Perfil = UFP.Idf_Usuario_Filial_Perfil
        WHERE 
                UFP.Idf_Filial = (SELECT Idf_Filial FROM TAB_Usuario_Filial_Perfil WITH(NOLOCK) WHERE Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial) AND
                M.Idf_Usuario_Filial_Des = @Idf_Usuario_Filial_Des AND
                M.Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS
        ORDER BY M.Dta_Cadastro DESC
        ";
        #endregion

        #region Spverificaminutoscadastromensagem
        private const string Spverificaminutoscadastromensagem = @"
        SELECT  TOP 1 *
        FROM    BNE.BNE_Mensagem_CS
        WHERE   Dta_Cadastro BETWEEN DATEADD(MI, -5, GETDATE()) AND DATEADD(D, 1, GETDATE())
                AND Idf_Usuario_Filial_Des = @Idf_Usuario_Filial_Des
                AND Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil 
                AND Idf_Tipo_Mensagem_CS = @Idf_Tipo_Mensagem_CS";
        #endregion

        #region SpSalvarDataEnvio
        private const string SpSalvarDataEnvio = @"
        UPDATE  BNE.BNE_Mensagem_CS 
        SET     Dta_Envio = GETDATE(), Idf_Status_Mensagem_CS = 2
        WHERE   Idf_Mensagem_CS = @Idf_Mensagem_CS";
        #endregion

        #region SpSalvarErro
        private const string SpSalvarErro = @"
        UPDATE  BNE.BNE_Mensagem_CS 
        SET     Idf_Status_Mensagem_CS = 3
        WHERE   Idf_Mensagem_CS = @Idf_Mensagem_CS";
        #endregion

        #endregion

        #region M�todos

        #region EnviarCurriculo
        public static bool EnviarCurriculo(Dictionary<int, string> dicCurriculo, Filial objFilial, int idUsuarioFilialPerfilLogado, string emailRemetente, string desMensagem,
            List<string> emailDestinatario, string assunto, bool curriculoAbertoEmail, bool anexoPdf, bool descontarVisualizacao, int idOrigem)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        int diasRevisualizacao = Int32.Parse(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PeriodoReVisualizacaoCurriculo, trans));

                        foreach (KeyValuePair<int, string> kvp in dicCurriculo)
                        {
                            var mensagem = desMensagem;
                            var objCurriculo = Curriculo.LoadObject(kvp.Key, trans);

                            if (curriculoAbertoEmail)
                                mensagem += kvp.Value;

                            //string assuntoMensagem;
                            //mensagem = CartaEmail.RetornarConteudoBNE(mensagem, out assuntoMensagem);

                            foreach (string emailDestino in emailDestinatario)
                            {
                                if (anexoPdf)
                                {
                                    string nomeArquivo = "Curriculo.pdf";
                                    byte[] arquivo = PDF.RecuperarPDFUsandoAbc(kvp.Value);

                                    EmailSenderFactory
                                        .Create(TipoEnviadorEmail.Smtp)
                                        .Enviar(assunto, mensagem, emailRemetente, emailDestino, nomeArquivo, arquivo);
                                }
                                else
                                    EmailSenderFactory
                                        .Create(TipoEnviadorEmail.Smtp)
                                        .Enviar(assunto, mensagem, emailRemetente, emailDestino);
                            }

                            //Fluxo utilizado para descontar visualiza��o do curriculo quando os dados da pessoa fisica devem ser mostrados.
                            if (descontarVisualizacao)
                            {
                                bool descontar = false;

                                //Verifica se curr�culo n�o � VIP.
                                if (!objCurriculo.FlagVIP)
                                {
                                    //Verifica se a filial n�o tem visualiza��o
                                    CurriculoVisualizacao objCurriculoVisualizacao;
                                    if (!CurriculoVisualizacao.CarregarPorCurriculoFilial(objCurriculo, objFilial, diasRevisualizacao, trans, out objCurriculoVisualizacao))
                                        descontar = true; //Se o curriculo n�o � vip e a filial n�o possui visualizacao deste currciulo no prazo correto, ent�o deve descontar visualizacao deste curriculo
                                }

                                //Se existe o curriculo na origem STC, ent�o n�o deve descontar visualiza��o
                                if (!idOrigem.Equals((int)Enumeradores.Origem.BNE))
                                {
                                    if (CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, new Origem(idOrigem), trans))
                                        descontar = false;
                                }

                                if (descontar)
                                {
                                    CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, false);
                                }
                            }
                        }

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

        #region SalvarEmail
        public static bool SalvarEmail(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, MensagemSistema objMensagemSistema,
            string assunto, string mensagem, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo, SqlTransaction trans)
        {
            var objMensagem = new MensagemCS
            {
                Curriculo = objCurriculo,
                UsuarioFilialPerfil = objUfpRemetente,
                UsuarioFilialDes = objUfpDestinatario,
                MensagemSistema = objMensagemSistema,
                DescricaoMensagem = mensagem,
                DescricaoEmailAssunto = assunto,
                DescricaoEmailRemetente = emailRemetente,
                DescricaoEmailDestinatario = emailDestinatario,
                TipoMensagemCS = new TipoMensagemCS((int)TipoMensagem.Email),
                StatusMensagemCS = new StatusMensagemCS((int)StatusMensagem.Naoenviado),
                IdSistema = (int)Enumeradores.Sistema.BNE,
                IdCentroServico = (int)Enumeradores.CentroServico.BNE,
                FlagInativo = false,
                NomeAnexo = nomeArquivoAnexo,
                ArquivoAnexo = arquivoAnexo
            };

            if (trans != null)
                objMensagem.Save(trans);
            else
                objMensagem.Save();

            #region Envio para o Assíncrono
            var parametros = new ParametroExecucaoCollection
                            {
                                {"idMensagem", "Mensagem", objMensagem.IdMensagemCS.ToString(), objMensagem.IdMensagemCS.ToString()},
                                {"emailRemetente", "Remetente", objMensagem.DescricaoEmailRemetente, objMensagem.DescricaoEmailRemetente},
                                {"emailDestinatario", "Destinatário", objMensagem.DescricaoEmailDestinatario, objMensagem.DescricaoEmailDestinatario},
                                {"assunto", "Assunto", objMensagem.DescricaoEmailAssunto, "" },
                                {"mensagem", "Mensagem", objMensagem.DescricaoMensagem, "" }
                            };

            try
            {
                ProcessoAssincrono.IniciarAtividade(
                TipoAtividade.EnvioEmail,
                PluginsCompatibilidade.CarregarPorMetadata("EnvioEmail", "PluginSaidaEmailSMS"),
                parametros,
                null,
                null,
                objMensagem.ArquivoAnexo,
                objMensagem.NomeAnexo,
                DateTime.Now);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

            #endregion Envio para o Assincrono

            return objMensagem.IdMensagemCS > 0;
        }
        #endregion

        #region SalvarSMS
        public static int SalvarSMS(Curriculo curriculo, UsuarioFilialPerfil usuarioFilialPerfil, UsuarioFilialPerfil usuarioFilialPerfilDes, string mensagem, string dddTelefone, string numeroTelefone, SqlTransaction trans = null)
        {
            var objMensagem = new MensagemCS
            {
                Curriculo = curriculo,
                DescricaoMensagem = mensagem,
                UsuarioFilialPerfil = usuarioFilialPerfil,
                UsuarioFilialDes = usuarioFilialPerfilDes,
                NumeroDDDCelular = dddTelefone,
                NumeroCelular = numeroTelefone,
                TipoMensagemCS = new TipoMensagemCS((int)TipoMensagem.SMS),
                StatusMensagemCS = new StatusMensagemCS((int)StatusMensagem.Naoenviado),
                IdSistema = (int)Enumeradores.Sistema.BNE,
                IdCentroServico = (int)Enumeradores.CentroServico.BNE,
                FlagInativo = false,
                DescricaoEmailRemetente = " "
            };

            if (trans != null)
                objMensagem.Save(trans);
            else
                objMensagem.Save();

            #region Envio para o Assíncrono
            try
            {
                var parametros = new ParametroExecucaoCollection
                            {
                                {"idMensagem", "Mensagem", objMensagem.IdMensagemCS.ToString(), objMensagem.IdMensagemCS.ToString()},
                                {"numeroDDD", "DDD", objMensagem.NumeroDDDCelular, objMensagem.NumeroDDDCelular},
                                {"numeroCelular", "Telefone", objMensagem.NumeroCelular, objMensagem.NumeroCelular},
                                {"mensagem", "Mensagem", objMensagem.DescricaoMensagem, objMensagem.DescricaoMensagem}
                            };

                ProcessoAssincrono.IniciarAtividade(
                    TipoAtividade.EnvioSMS,
                    PluginsCompatibilidade.CarregarPorMetadata("EnvioSMS", "PluginSaidaEmailSMS"),
                    parametros,
                    null,
                    null,
                    null,
                    null,
                    DateTime.Now);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

            #endregion Envio para o Assincrono

            return objMensagem.IdMensagemCS;
        }
        #endregion

        #region EnviaSMSTanque

        public static int EnviaSMSTanque(Curriculo curriculo, PessoaFisica pessoa, UsuarioFilialPerfil usuarioFilialPerfil, UsuarioFilialPerfil usuarioFilialPerfilDes, string mensagem, string dddTelefone, string numeroTelefone, Enumeradores.UsuarioSistemaTanque usuarioTanque, SqlTransaction trans = null)
        {
            var objMensagem = new MensagemCS
            {
                Curriculo = curriculo,
                DescricaoMensagem = mensagem,
                UsuarioFilialPerfil = usuarioFilialPerfil,
                UsuarioFilialDes = usuarioFilialPerfilDes,
                NumeroDDDCelular = dddTelefone,
                NumeroCelular = numeroTelefone,
                TipoMensagemCS = new TipoMensagemCS((int)TipoMensagem.SMS),
                StatusMensagemCS = new StatusMensagemCS((int)StatusMensagem.Naoenviado),
                IdSistema = (int)Enumeradores.Sistema.BNE,
                IdCentroServico = (int)Enumeradores.CentroServico.BNE,
                FlagInativo = false,
                DescricaoEmailRemetente = " "
            };

            if (trans != null)
                objMensagem.Save(trans);
            else
                objMensagem.Save();

            #region Envio para o Assíncrono
            try
            {
                var parametros = new ParametroExecucaoCollection
                            {
                                {"idMensagemCS", "idMensagemCS", objMensagem.IdMensagemCS.ToString(), objMensagem.IdMensagemCS.ToString()},
                                {"idUsuarioTanque", "idUsuarioTanque", usuarioTanque.ToString(), usuarioTanque.ToString()},
                                {"idCurriculo", "idCurriculo", curriculo.IdCurriculo.ToString(), curriculo.IdCurriculo.ToString()},
                                {"numeroDDD", "DDD", objMensagem.NumeroDDDCelular, objMensagem.NumeroDDDCelular},
                                {"numeroCelular", "Telefone", objMensagem.NumeroCelular, objMensagem.NumeroCelular},
                                {"nomePessoa", "nomePessoa", pessoa.NomeCompleto, pessoa.NomeCompleto},
                                {"mensagem", "Mensagem", objMensagem.DescricaoMensagem, objMensagem.DescricaoMensagem}
                            };

                ProcessoAssincrono.IniciarAtividade(
                    TipoAtividade.EnvioSMSTanque,
                    PluginsCompatibilidade.CarregarPorMetadata("EnvioSMSTanque", "PluginSaidaSMSTanque"),
                    parametros,
                    null,
                    null,
                    null,
                    null,
                    DateTime.Now);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

            #endregion Envio para o Assincrono

            return objMensagem.IdMensagemCS;
        }

        #endregion

        #region EnviarSMSDescontarSaldo
        /// <summary>
        /// M�todo respons�vel por salvar uma mensagem no banco e descontar a utiliza��o de um SMS da Filial
        /// </summary>
        /// <param name="curriculo"></param>
        /// <param name="usuarioFilialPerfil"></param>
        /// <param name="usuarioFilialPerfilDes"> </param>
        /// <param name="mensagem"></param>
        /// <param name="dddTelefone"></param>
        /// <param name="numeroTelefone"></param>
        /// <param name="objFilial"></param>
        /// <returns></returns>
        public static int EnviarSMSDescontarSaldo(Curriculo curriculo, UsuarioFilialPerfil usuarioFilialPerfil, UsuarioFilialPerfil usuarioFilialPerfilDes, string mensagem, string dddTelefone, string numeroTelefone, Filial objFilial)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        int idMensagem = 0;
                        if (objFilial.DescontarEnvioSMS(trans))
                            idMensagem = SalvarSMS(curriculo, usuarioFilialPerfil, usuarioFilialPerfilDes, mensagem, dddTelefone, numeroTelefone, trans);

                        trans.Commit();
                        return idMensagem;
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

        #region QuantidadeMensagensRecebidas
        public static int QuantidadeMensagensRecebidas(int idUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Des", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcountmensagensrecebidas, parms));
        }
        #endregion

        #region QuantidadeMensagensRecebidas_NaoLidas
        public static int QuantidadeMensagensRecebidas_NaoLidas(int idUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Des", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil }                    
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcountmensagensrecebidas_naoLidas, parms));
        }
        #endregion

        #region QuantidadeMensagensEnviadas
        public static int QuantidadeMensagensEnviadas(int idUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcountmensagensenviadas, parms));
        }
        #endregion

        #region CarregarMensagensRecebidas
        /// <summary>
        /// Carrega as mensagens recebidas do usuarioFilialPerfil informado
        /// </summary>
        public static DataTable CarregarMensagensRecebidas(int idUsuarioFilialPerfil, int paginaCorrente, int tamanhoPagina, string desPesquisa, bool usuarioCandidato, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@CurrentPage", SqlDbType.Int, 4), 
                    new SqlParameter("@PageSize", SqlDbType.Int, 4), 
                    new SqlParameter("@Idf_Usuario_Filial_Des", SqlDbType.Int, 4), 
                    new SqlParameter("@Des_Filtro", SqlDbType.VarChar, 1000), 
                    new SqlParameter("@Flg_Usuario_Candidato", SqlDbType.Bit, 4)
                };

            parms[0].Value = paginaCorrente;
            parms[1].Value = tamanhoPagina;
            parms[2].Value = idUsuarioFilialPerfil;

            if (!string.IsNullOrEmpty(desPesquisa))
                parms[3].Value = desPesquisa;
            else
                parms[3].Value = DBNull.Value;

            parms[4].Value = usuarioCandidato;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spmensagensrecebidas, parms))
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

        #region CarregarMensagensEnviadas
        /// <summary>
        /// Carrega as mensagens enviadas do usuarioFilialPerfil informado
        /// </summary>
        public static DataTable CarregarMensagensEnviadas(int idUsuarioFilialPerfil, int paginaCorrente, int tamanhoPagina, string desPesquisa, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@CurrentPage", SqlDbType.Int, 4), 
                    new SqlParameter("@PageSize", SqlDbType.Int, 4), 
                    new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4), 
                    new SqlParameter("@Des_Filtro", SqlDbType.VarChar, 1000)
                };

            parms[0].Value = paginaCorrente;
            parms[1].Value = tamanhoPagina;
            parms[2].Value = idUsuarioFilialPerfil;

            if (!string.IsNullOrEmpty(desPesquisa))
                parms[3].Value = desPesquisa;
            else
                parms[3].Value = DBNull.Value;

            totalRegistros = 0;
            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spmensagensenviadas, parms))
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

        #region ListaDataReaderAnteriorProximaMensagensEnviadas
        /// <summary>
        /// </summary>
        /// <returns></returns>
        private static IDataReader ListaDataReaderAnteriorProximaMensagensEnviadas(int idMensagemCS, int idUsuarioFilialPerfil, string desPesquisa)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4), 
                    new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4), 
                    new SqlParameter("@Des_Filtro", SqlDbType.VarChar, 1000)
                };

            parms[0].Value = idMensagemCS;
            parms[1].Value = idUsuarioFilialPerfil;

            if (!string.IsNullOrEmpty(desPesquisa))
                parms[2].Value = desPesquisa;
            else
                parms[2].Value = DBNull.Value;

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spmensagemanteriorproximactemensagensenviadas, parms);
        }
        #endregion

        #region ListaDataReaderAnteriorProximaMensagensRecebidas
        /// <summary>
        /// </summary>
        /// <returns></returns>
        private static IDataReader ListaDataReaderAnteriorProximaMensagensRecebidas(int idMensagemCS, int idUsuarioFilialPerfil, string desPesquisa)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Mensagem_CS", SqlDbType.Int, 4), 
                    new SqlParameter("@Idf_Usuario_Filial_Des", SqlDbType.Int, 4), 
                    new SqlParameter("@Des_Filtro", SqlDbType.VarChar, 1000)
                };

            parms[0].Value = idMensagemCS;
            parms[1].Value = idUsuarioFilialPerfil;

            if (!string.IsNullOrEmpty(desPesquisa))
                parms[2].Value = desPesquisa;
            else
                parms[2].Value = DBNull.Value;

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spmensagemanteriorproximactemensagensrecebidas, parms);
        }
        #endregion

        #region LoadObjectAnteriorProximoMensagensEnviadas
        /// <summary>
        /// </summary>
        /// <returns>Inst�ncia de Noticia.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static MensagemCS LoadObjectAnteriorProximoMensagensEnviadas(int idMensagemCS, int idUsuarioFilialPerfil, string desPesquisa)
        {
            using (IDataReader dr = ListaDataReaderAnteriorProximaMensagensEnviadas(idMensagemCS, idUsuarioFilialPerfil, desPesquisa))
            {
                var objMensagemCS = new MensagemCS();
                if (SetInstanceAnteriorProximo(dr, objMensagemCS))
                    return objMensagemCS;
            }
            throw (new RecordNotFoundException(typeof(MensagemCS)));
        }
        #endregion

        #region LoadObjectAnteriorProximoMensagensRecebidas
        /// <summary>
        /// </summary>
        /// <returns>Inst�ncia de Noticia.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static MensagemCS LoadObjectAnteriorProximoMensagensRecebidas(int idMensagemCS, int idUsuarioFilialPerfil, string desPesquisa)
        {
            using (IDataReader dr = ListaDataReaderAnteriorProximaMensagensRecebidas(idMensagemCS, idUsuarioFilialPerfil, desPesquisa))
            {
                var objMensagemCS = new MensagemCS();
                if (SetInstanceAnteriorProximo(dr, objMensagemCS))
                    return objMensagemCS;
            }
            throw (new RecordNotFoundException(typeof(MensagemCS)));
        }
        #endregion

        #region SetInstanceAnteriorProximo
        /// <summary>
        /// M�todo auxiliar utilizado pelos m�todos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objMensagemCS">Inst�ncia a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a opera��o foi executada com sucesso.</returns>
        /// <remarks>Vinicius Maciel</remarks>
        private static bool SetInstanceAnteriorProximo(IDataReader dr, MensagemCS objMensagemCS)
        {
            try
            {
                if (dr.Read())
                {
                    objMensagemCS._idMensagemCS = Convert.ToInt32(dr["Idf_Mensagem_CS"]);

                    if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                        objMensagemCS._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    else
                        objMensagemCS._usuarioFilialPerfil = null;

                    if (dr["Idf_Curriculo"] != DBNull.Value)
                        objMensagemCS._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    else
                        objMensagemCS._curriculo = null;

                    objMensagemCS._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);

                    if (dr["Dta_Envio"] != DBNull.Value)
                        objMensagemCS._dataEnvio = Convert.ToDateTime(dr["Dta_Envio"]);
                    else
                        objMensagemCS._dataEnvio = null;

                    if (dr["Idf_Rede_Social_CS"] != DBNull.Value)
                        objMensagemCS._redeSocialCS = new RedeSocialCS(Convert.ToInt32(dr["Idf_Rede_Social_CS"]));
                    else
                        objMensagemCS._redeSocialCS = null;

                    objMensagemCS._tipoMensagemCS = new TipoMensagemCS(Convert.ToInt32(dr["Idf_Tipo_Mensagem_CS"]));

                    if (!String.IsNullOrEmpty(Convert.ToString(dr["Des_Email_Destinatario"])))
                        objMensagemCS._descricaoEmailDestinatario = Convert.ToString(dr["Des_Email_Destinatario"]);
                    else
                        objMensagemCS._descricaoEmailDestinatario = string.Empty;

                    if (!String.IsNullOrEmpty(Convert.ToString(dr["Des_Email_Assunto"])))
                        objMensagemCS._descricaoEmailAssunto = Convert.ToString(dr["Des_Email_Assunto"]);
                    else
                        objMensagemCS._descricaoEmailAssunto = Convert.ToString(dr["Des_Email_Assunto"]);

                    objMensagemCS._statusMensagemCS = new StatusMensagemCS(Convert.ToInt32(dr["Idf_Status_Mensagem_CS"]));
                    objMensagemCS._descricaoEmailRemetente = Convert.ToString(dr["Des_Email_Remetente"]);

                    if (!String.IsNullOrEmpty(Convert.ToString(dr["Num_DDD_Celular"])))
                        objMensagemCS._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
                    else
                        objMensagemCS._numeroDDDCelular = string.Empty;

                    if (!String.IsNullOrEmpty(Convert.ToString(dr["Num_Celular"])))
                        objMensagemCS._numeroCelular = Convert.ToString(dr["Num_Celular"]);
                    else
                        objMensagemCS._numeroCelular = string.Empty;

                    objMensagemCS._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

                    if (!String.IsNullOrEmpty(Convert.ToString(dr["Nme_Anexo"])))
                        objMensagemCS._nomeAnexo = Convert.ToString(dr["Nme_Anexo"]);
                    else
                        objMensagemCS._nomeAnexo = string.Empty;

                    if (!String.IsNullOrEmpty(Convert.ToString(dr["Des_Obs"])))
                        objMensagemCS._descricaoObs = Convert.ToString(dr["Des_Obs"]);
                    else
                        objMensagemCS._descricaoObs = string.Empty;

                    objMensagemCS._idSistema = Convert.ToInt32(dr["Idf_Sistema"]);
                    objMensagemCS._idCentroServico = Convert.ToInt32(dr["Idf_Centro_Servico"]);

                    if (dr["Arq_Anexo"] != DBNull.Value)
                        objMensagemCS._arquivoAnexo = (byte[])dr["Arq_Anexo"];
                    else
                        objMensagemCS._arquivoAnexo = null;

                    if (dr["Idf_Mensagem_Sistema"] != DBNull.Value)
                        objMensagemCS._mensagemSistema = new MensagemSistema(Convert.ToInt32(dr["Idf_Mensagem_Sistema"]));
                    else
                        objMensagemCS._mensagemSistema = null;

                    if (dr["Idf_Usuario_Filial_Des"] != DBNull.Value)
                        objMensagemCS._usuarioFilialDes = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Des"]));
                    else
                        objMensagemCS._usuarioFilialDes = null;

                    objMensagemCS._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                    if (dr["Idf_Anterior"] != DBNull.Value)
                        objMensagemCS._idMensagemCSAnterior = Convert.ToInt32(dr["Idf_Anterior"]);

                    if (dr["Idf_Proxima"] != DBNull.Value)
                        objMensagemCS._idMensagemCSProxima = Convert.ToInt32(dr["Idf_Proxima"]);

                    objMensagemCS._persisted = true;
                    objMensagemCS._modified = false;

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

        #region RecuperarDataUltimaMensagemEnviadaFilial
        public static DateTime? RecuperarDataUltimaMensagemEnviadaFilial(UsuarioFilialPerfil objUsuarioFilialPerfilRemetente, UsuarioFilialPerfil objUsuarioFilialPerfilDestinatario, TipoMensagemCS objTipoMensagemCS, out string mensagem)
        {
            mensagem = string.Empty;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objUsuarioFilialPerfilRemetente.IdUsuarioFilialPerfil } , 
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Des", SqlDbType = SqlDbType.Int, Size = 4, Value = objUsuarioFilialPerfilDestinatario.IdUsuarioFilialPerfil }, 
                    new SqlParameter { ParameterName = "@Idf_Tipo_Mensagem_CS",  SqlDbType = SqlDbType.Int, Size = 4, Value = objTipoMensagemCS.IdTipoMensagemCS }
                };

            DateTime? retorno = null;
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectdataultimamensagemenviadafilialportipo, parms))
            {
                if (dr.Read())
                {
                    retorno = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    mensagem = Convert.ToString(dr["Des_Mensagem"]);
                }
            }

            return retorno;
        }
        #endregion

        #region EnviarSmsSolr
        public static void EnviarSmsSolr(int Idf_Curriculo)
        {
            string urlSLOR = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrAtualizaSms);

            if (!String.IsNullOrEmpty(urlSLOR))
            {
                urlSLOR = urlSLOR + Idf_Curriculo;
                WebRequest request = WebRequest.Create(urlSLOR);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            }

        }
        #endregion

        #region EnviarChameFacil
        public static bool EnviarChameFacil(UsuarioFilialPerfil objUsuarioFilialPerfil, Filial objFilial, List<Curriculo> listCurriculo, string desMensagemSMS, string desMensagemEmail, bool enviarSMS, bool enviarEmail, out string mensagemErro, out int quantidadeSMSEnviado, out int quantidadeEmailEnviado)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        mensagemErro = string.Empty;
                        quantidadeEmailEnviado = quantidadeSMSEnviado = 0;

                        var emailComercial = string.Empty;
                        UsuarioFilial objUsuarioFilial;
                        if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                            emailComercial = objUsuarioFilial.EmailComercial;

                        var destinatarios = ListaDestinatarios(listCurriculo.ToList()).ToList();
                        /*
                         Variável para controlar se foi enviado campanha em massa ou não, isso para enviar apenas uma campanha para todos os curriculos e mesmo
                         assim varrer todos os currículos para enviar e-mail e tratar o envio individual também
                        */
                        bool envioCampanhaMassa = false;
                        if (enviarSMS && destinatarios.Count > 1 && CelularSelecionador.VerificaCelularEstaLiberado(objUsuarioFilialPerfil))
                        {
                            envioCampanhaMassa = true;

                            #region SMS
                            var listaCampanha =
                                destinatarios.Where(
                                    d =>
                                        !String.IsNullOrWhiteSpace(d.NumeroDDDCelular) &&
                                        !String.IsNullOrWhiteSpace(d.NumeroCelular) &&
                                        d.NumeroDDDCelular.Trim().All(char.IsDigit) &&
                                        d.NumeroCelular.Trim().All(char.IsDigit))
                                    .Select(curriculo => new CampanhaCurriculo
                                    {
                                        Curriculo = new Curriculo(curriculo.IdCurriculo),
                                        NomePessoa = curriculo.Nome,
                                        NumeroDDDCelular = curriculo.NumeroDDDCelular.Trim(),
                                        NumeroCelular = curriculo.NumeroCelular.Trim()
                                    }).ToList();

                            if (listaCampanha.Count <= 0)
                            {
                                mensagemErro = "Não foi possível enviar, nenhum número de celular é válido.";
                                return false;
                            }

                            int idCampanha;
                            string erroOuObservacao;

                            if (!Campanha.SalvarCampanha(objUsuarioFilialPerfil, "Campanha_" + DateTime.Now, desMensagemSMS, listaCampanha, out idCampanha, out erroOuObservacao))
                            {
                                mensagemErro = erroOuObservacao;
                            }

                            quantidadeSMSEnviado = listaCampanha.Count;
                            #endregion
                        }

                        foreach (var objCurriculo in destinatarios)
                        {

                            #region Envio de SMS
                            if (enviarSMS && !envioCampanhaMassa)
                            {
                                //Se o usuário tem CelSel
                                if (CelularSelecionador.VerificaCelularEstaLiberado(objUsuarioFilialPerfil))
                                {

                                    #region Envio Pelo Tanque

                                    var limiteDiario = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.BNEEnviaQuantidadeLimiteSMSDiario));
                                    var quantidadeEnviada = Campanha.QuantidadeCurriculosEnviadosHoje(objUsuarioFilialPerfil);
                                    var limiteDisponivel = limiteDiario - quantidadeEnviada;

                                    decimal decAux;
                                    if (!decimal.TryParse(objCurriculo.NumeroDDDCelular, out decAux))
                                    {
                                        mensagemErro = "O currículo não possui um número de celular válido, não será possível enviar uma mensagem.";
                                        return false;
                                    }
                                    if (!decimal.TryParse(objCurriculo.NumeroCelular, out decAux))
                                    {
                                        mensagemErro = "O currículo não possui um número de celular válido, não será possível enviar uma mensagem.";
                                        return false;
                                    }

                                    var destinatario = new CelularSelecionadorDestinatario
                                    {
                                        IdCurriculo = objCurriculo.IdCurriculo,
                                        IdPessoaFisica = objCurriculo.IdPessoaFisica,
                                        Nome = objCurriculo.Nome,
                                        Email = objCurriculo.Email,
                                        VIP = objCurriculo.VIP,
                                        NumeroDDDCelular = objCurriculo.NumeroDDDCelular,
                                        NumeroCelular = objCurriculo.NumeroCelular
                                    };

                                    if (limiteDisponivel < 1)
                                        mensagemErro = string.Format("Você possui apenas {0} SMS disponíveis. Diminua a quantidade de candidatos!", limiteDisponivel);
                                    else
                                    {
                                        var listaCampanha = new List<CampanhaCurriculo>
                                        {
                                            new CampanhaCurriculo
                                            {
                                                Curriculo = new Curriculo(objCurriculo.IdCurriculo),
                                                NomePessoa = destinatario.Nome,
                                                NumeroDDDCelular = destinatario.NumeroDDDCelular,
                                                NumeroCelular = destinatario.NumeroCelular
                                            }
                                        };


                                        int idCampanha;
                                        string erroOuObservacao;
                                        if (!Campanha.SalvarCampanha(objUsuarioFilialPerfil, "Campanha_" + DateTime.Now, desMensagemSMS, listaCampanha, out idCampanha, out erroOuObservacao))
                                        {
                                            mensagemErro = erroOuObservacao;
                                            return false;
                                        }

                                        if (!string.IsNullOrEmpty(erroOuObservacao))
                                        {
                                            mensagemErro = erroOuObservacao;
                                            return false;
                                        }

                                        foreach (var objCampanhaCurriculo in listaCampanha)
                                        {
                                            EnviarSmsSolr(objCampanhaCurriculo.Curriculo.IdCurriculo);
                                        }

                                        quantidadeSMSEnviado++;
                                    }
                                    #endregion

                                }
                                else
                                {

                                    #region EnvioPelaTww

                                    if (objFilial.PossuiPlanoAtivo()) //Se a empresa possui plano ativo
                                    {
                                        var saldoSMS = objFilial.RecuperarSaldoSMS();
                                        if (saldoSMS > 0 || objCurriculo.VIP)
                                        {
                                            if (PodeEnviarMensagem(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, (int)TipoMensagem.SMS, objCurriculo.IdUsuarioFilialPerfil))
                                            {
                                                quantidadeSMSEnviado++;
                                                SalvarSMS(new Curriculo(objCurriculo.IdCurriculo), objUsuarioFilialPerfil, new UsuarioFilialPerfil(objCurriculo.IdUsuarioFilialPerfil), desMensagemSMS, objCurriculo.NumeroDDDCelular, objCurriculo.NumeroCelular, trans);
                                                objFilial.DescontarEnvioSMS(trans);
                                                EnviarSmsSolr(objCurriculo.IdCurriculo);
                                            }
                                        }
                                        else
                                        {
                                            mensagemErro = "Quantidade de SMS insuficiente!";
                                        }
                                    }
                                    else
                                    {
                                        if (objCurriculo.VIP)
                                        {
                                            if (objFilial.EmpresaSemPlanoPodeEnviarSMS(1))
                                            {
                                                if (PodeEnviarMensagem(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, (int)TipoMensagem.SMS, objCurriculo.IdUsuarioFilialPerfil))
                                                {
                                                    quantidadeSMSEnviado++;
                                                    SalvarSMS(new Curriculo(objCurriculo.IdCurriculo), objUsuarioFilialPerfil, new UsuarioFilialPerfil(objCurriculo.IdUsuarioFilialPerfil), desMensagemSMS, objCurriculo.NumeroDDDCelular, objCurriculo.NumeroCelular, trans);
                                                    EnviarSmsSolr(objCurriculo.IdCurriculo);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            mensagemErro = "Quantidade de SMS insuficiente!";
                                        }
                                    }

                                    #endregion

                                }
                            }
                            #endregion

                            #region Envio de E-mail
                            if (enviarEmail && !string.IsNullOrEmpty(objCurriculo.Email) && !string.IsNullOrEmpty(emailComercial))
                            {
                                if (enviarSMS || objCurriculo.VIP || CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, new Curriculo(objCurriculo.IdCurriculo), false))
                                {
                                    if (PodeEnviarMensagem(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, (int)TipoMensagem.Email, objCurriculo.IdUsuarioFilialPerfil))
                                    {
                                        if (EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(new Curriculo(objCurriculo.IdCurriculo), objUsuarioFilialPerfil, new UsuarioFilialPerfil(objCurriculo.IdUsuarioFilialPerfil), string.Format("Mensagem enviada por {0}", objFilial.NomeFantasia), desMensagemEmail, emailComercial, objCurriculo.Email, trans))
                                            quantidadeEmailEnviado++;
                                    }
                                }
                                else
                                {
                                    mensagemErro = "Quantidade de visualização insuficiente!";
                                }
                            }
                            #endregion

                        }

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

        #region ListaDestinatarios
        private static IEnumerable<CelularSelecionadorDestinatario> ListaDestinatarios(List<Curriculo> listaCurriculos)
        {
            foreach (var curriculo in listaCurriculos)
            {
                var objCurriculoDTO = Curriculo.CarregarCurriculoDTO(curriculo.IdCurriculo);

                yield return new CelularSelecionadorDestinatario { IdCurriculo = objCurriculoDTO.IdCurriculo, IdPessoaFisica = objCurriculoDTO.IdPessoaFisica, IdUsuarioFilialPerfil = objCurriculoDTO.IdUsuarioFilialPerfil, Nome = objCurriculoDTO.PrimeiroNome, Email = objCurriculoDTO.Email, VIP = objCurriculoDTO.VIP, NumeroDDDCelular = objCurriculoDTO.NumeroDDDCelular, NumeroCelular = objCurriculoDTO.NumeroCelular };
            }
        }
        #endregion

        #region EnviarNotificacaoVisualizacaoCurriculo
        public static void EnviarNotificacaoVisualizacaoCurriculo(PessoaFisica objPessoaFisica, string nomeFantasiaEmpresa, bool flagVIP, SqlTransaction trans)
        {
            if (flagVIP)
            {
                objPessoaFisica.CompleteObject(trans);
                BLL.Curriculo objCurriculo;
                Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);

                string mensagem = string.Format(ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.MensagemSMSVisualizacaoCurriculoParaVIP), objPessoaFisica.PrimeiroNome, nomeFantasiaEmpresa);

                //SalvarSMS(null, null, null, mensagem, objPessoaFisica.NumeroDDDCelular, objPessoaFisica.NumeroCelular);
                EnviaSMSTanque(objCurriculo, objPessoaFisica, null, null, mensagem, objPessoaFisica.NumeroDDDCelular, objPessoaFisica.NumeroCelular, Enumeradores.UsuarioSistemaTanque.QuemMeViu);
            }
        }
        #endregion

        #region EnviarCompartilhamentoVaga
        public static void EnviarCompartilhamentoVaga(UsuarioFilialPerfil objUsuarioFilialPerfil, Vaga objVaga, string urlVaga, string emailDestinatario)
        {
            objUsuarioFilialPerfil.CompleteObject();
            objUsuarioFilialPerfil.PessoaFisica.CompleteObject();
            objVaga.CompleteObject();
            objVaga.Filial.CompleteObject();
            objVaga.Cidade.CompleteObject();
            objVaga.Cidade.Estado.CompleteObject();
            if (objVaga.Escolaridade != null)
                objVaga.Escolaridade.CompleteObject();

            #region Renderiza��o dos dados do remetente
            string nomeCompletoRemetente = objUsuarioFilialPerfil.PessoaFisica.NomeCompleto;
            string emailRemetente = objUsuarioFilialPerfil.PessoaFisica.EmailPessoa;

            #endregion

            #region Renderiza��o da atribui��o
            string atribuicoesVaga = objVaga.DescricaoAtribuicoes;
            if (!String.IsNullOrEmpty(atribuicoesVaga) && atribuicoesVaga.Length > 280)
                atribuicoesVaga = atribuicoesVaga.Substring(0, 280) + "...";
            #endregion

            #region Renderiza��o do requisito
            string requisitos =
                String.Format("{0} {1}", objVaga.Escolaridade == null ?
                    String.Empty :
                    objVaga.Escolaridade.DescricaoBNE, objVaga.DescricaoRequisito);
            if (!String.IsNullOrEmpty(requisitos) && requisitos.Length > 280)
                requisitos = requisitos.Substring(0, 280) + "...";
            #endregion

            #region Renderiza��o do sal�rio
            string salarioFormat = String.Empty;
            decimal salarioDe = objVaga.ValorSalarioDe ?? Decimal.Zero;
            decimal salarioPara = objVaga.ValorSalarioPara ?? Decimal.Zero;
            if (salarioDe == Decimal.Zero || salarioPara == Decimal.Zero)
                salarioFormat = "Sal�rio a combinar";
            else if (salarioDe == salarioPara)
                salarioFormat = "Sal�rio de R$ {0}";
            else
                salarioFormat = "Sal�rio de R$ {0} a R$ {1}";
            string salario = String.Format(salarioFormat,
                salarioDe.ToString("0,0.00", CultureInfo.CurrentCulture),
                salarioPara.ToString("0,0.00", CultureInfo.CurrentCulture));
            #endregion

            #region Renderiza��o da logo da empresa
            string urlLogoFormat = "http://{0}/Handlers/PessoaJuridicaLogo.ashx?cnpj={1}&Origem=Local";
            string urlLogo = String.Format(urlLogoFormat,
                Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente),
                objVaga.Filial.NumeroCNPJ);
            if (objVaga.FlagConfidencial)
                urlLogo = String.Empty;
            #endregion

            #region Renderiza��o da cidade e UF da vaga
            string cidade = objVaga.Cidade.NomeCidade;
            string uf = objVaga.Cidade.Estado.SiglaEstado;
            string cidadeEstado = Helper.FormatarCidade(cidade, uf);
            #endregion

            #region Obten��o dos templates de HTML
            string formatAssunto;
            string formatConteudoPadrao =
                CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.CompartilhamentoVagaPorEmail, out formatAssunto);
            #endregion

            #region Montagem da mensagem
            object parametrosCorpoEmail = new
            {
                UrlLogoEmpresa = urlLogo,
                Funcao = objVaga.DescricaoFuncao ?? String.Empty,
                Salario = salario,
                Cidade = cidadeEstado,
                Atribuicoes = atribuicoesVaga ?? String.Empty,
                Requisitos = requisitos ?? String.Empty,
                UrlVaga = urlVaga,
                NomeRemetente = nomeCompletoRemetente,
                EmailRemetente = emailRemetente
            };

            object parametrosAssuntoEmail = new
            {
                NomeRemetente = nomeCompletoRemetente
            };

            string desMensagem = parametrosCorpoEmail.ToString(formatConteudoPadrao);
            string desAssunto = parametrosAssuntoEmail.ToString(formatAssunto);
            #endregion

            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(null, objUsuarioFilialPerfil, null, desAssunto, desMensagem, emailRemetente, emailDestinatario);
        }
        #endregion

        #region PodeEnviarMensagem
        /// <summary>
        /// Metodo responsavel por verificar se a mensagem foi enviada ha mais de 1 minuto. Se foi, permitir inser��o na tabela de mensagem.
        /// </summary>
        /// <returns></returns>
        public static bool PodeEnviarMensagem(int idUsuarioFilialPerfil, int idTipoMensagem, int idUsuarioFilialDes)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil },
                    new SqlParameter { ParameterName = "@Idf_Tipo_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = idTipoMensagem },
                    new SqlParameter { ParameterName = "@Idf_Usuario_Filial_Des", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialDes },
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spverificaminutoscadastromensagem, parms))
            {
                return !dr.Read();  // se o m�todo IDataReader.Read() retornar false de cara, � porque a consulta n�o retornou nenhuma linha 
                // e, portanto, n�o teve envio de email nos ultimos minutos
            }
        }
        #endregion

        #region SalvarDataEnvio
        public void SalvarDataEnvio()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = this.IdMensagemCS }
                };

            DataAccessLayer.ExecuteScalar(CommandType.Text, SpSalvarDataEnvio, parms);
        }
        #endregion SalvarDataEnvio

        #region SalvarErro
        public void SalvarErro()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = this.IdMensagemCS }
                };

            DataAccessLayer.ExecuteScalar(CommandType.Text, SpSalvarErro, parms);
        }
        #endregion SalvarDataEnvio

        #endregion

    }
}