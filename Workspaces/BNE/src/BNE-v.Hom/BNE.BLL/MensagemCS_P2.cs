//-- Data: 08/04/2010 14:43
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Enumeradores;
using BNE.BLL.Enumeradores.CustomAttribute;
using BNE.BLL.Mensagem;
using BNE.EL;
using BNE.Services.Base.ProcessosAssincronos;
using FormatObject = BNE.BLL.Common.FormatObject;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;
using BNE.BLL.Mensagem.DTO;
using BNE.BLL.Mensagem.Mailsender;
using BNE.ExtensionsMethods;

namespace BNE.BLL
{
    public partial class MensagemCS // Tabela: BNE_Mensagem_CS
    {
        private static readonly SmsService _smsService = new SmsService();
        private static readonly EmailService _emailService = new EmailService();
        private static readonly string EmailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens);

        #region Atributos
        private int? _idMensagemCSAnterior;
        private int? _idMensagemCSProxima;
        #endregion

        #region Propriedades

        #region IdMensagemCSAnterior
        public int? IdMensagemCSAnterior
        {
            get { return _idMensagemCSAnterior; }
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
            get { return _idMensagemCSProxima; }
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
        private const string Spmensagensenviadas = @"[BNE].[Mensagens_Enviadas]";
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

        #region SpLer
        private const string SpLer = @"
        UPDATE BNE.BNE_Mensagem_CS SET Flg_Lido = 1 WHERE Idf_Mensagem_CS = @Idf_Mensagem_CS";
        #endregion

        #endregion

        #region Metodos

        #region EnviarCurriculo
        public static bool EnviarCurriculo(Dictionary<int, string> dicCurriculo, Filial objFilial, int idUsuarioFilialPerfilLogado, string emailRemetente, string desMensagem,
            List<string> emailDestinatario, string assunto, bool curriculoAbertoEmail, bool anexoPdf, bool descontarVisualizacao, int idOrigem)
        {
            byte[] arquivo = null;
            var nomeArquivo = string.Empty;
            var diasRevisualizacao = int.Parse(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PeriodoReVisualizacaoCurriculo));

            try
            {

                foreach (var kvp in dicCurriculo)
                {
                    var mensagem = desMensagem;
                    var objCurriculo = Curriculo.LoadObject(kvp.Key);

                    if (curriculoAbertoEmail)
                        mensagem += kvp.Value;

                    arquivo = null;

                    if (anexoPdf)
                    {
                        objCurriculo.PessoaFisica.CompleteObject();
                        nomeArquivo = string.Format("Curriculo-{0}.pdf", objCurriculo.PessoaFisica.NomeCompleto).Replace(" ", "_");
                        if (nomeArquivo.Length > 30)//limite do campo da tabela
                            nomeArquivo = string.Format("Curriculo-{0}.pdf", objCurriculo.PessoaFisica.PrimeiroNome).Replace(" ", "_");
                        arquivo = PDF.GerarPdfAPartirdoHtml(kvp.Value);
                    }


                    foreach (var emailDestino in emailDestinatario)
                    {
                        if (anexoPdf && arquivo != null)
                            EmailSenderFactory
                                .Create(TipoEnviadorEmail.Fila)
                                .Enviar(assunto, mensagem, null, emailRemetente, emailDestino, nomeArquivo, arquivo);
                        else
                            EmailSenderFactory
                                .Create(TipoEnviadorEmail.Fila)
                                .Enviar(assunto, mensagem, null, emailRemetente, emailDestino);
                    }

                    //Fluxo utilizado para descontar visualiza��o do curriculo quando os dados da pessoa fisica devem ser mostrados.
                    if (descontarVisualizacao)
                    {
                        var descontar = false;

                        //Verifica se curr�culo n�o � VIP.
                        if (!objCurriculo.FlagVIP)
                        {
                            //Verifica se a filial n�o tem visualiza��o
                            if (!CurriculoVisualizacao.VerificarVisualizacaoCV(objCurriculo, objFilial))
                                descontar = true; //Se o curriculo n�o � vip e a filial n�o possui visualizacao deste currciulo no prazo correto, ent�o deve descontar visualizacao deste curriculo
                        }

                        //Se existe o curriculo na origem STC, ent�o n�o deve descontar visualiza��o
                        if (!idOrigem.Equals((int)Enumeradores.Origem.BNE))
                        {
                            if (CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, new Origem(idOrigem)))
                                descontar = false;
                        }

                        if (descontar)
                        {
                            var descontou = CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, false);
                            if (descontou)
                            {
                                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, new UsuarioFilialPerfil(idUsuarioFilialPerfilLogado), objCurriculo, true, BNE.Common.Helper.RecuperarIP());
                            }
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region SalvarEmail
        public static bool SalvarEmail(Curriculo objCurriculo, UsuarioFilialPerfil objUfpRemetente, UsuarioFilialPerfil objUfpDestinatario, MensagemSistema objMensagemSistema,
          string assunto, string mensagem, Enumeradores.CartaEmail? carta, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo, byte[] arquivoAnexo, SqlTransaction trans)
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


            LogEnvioMensagem objLogoEnvioMensagem = new LogEnvioMensagem
            {
                DesAssunto = assunto,
                EmlDestinatario = emailDestinatario.Trim(),
                CartaEmail = carta.HasValue ? new CartaEmail((int)carta) : null,
                EmlRemetente = emailRemetente,
                curriculo = objCurriculo != null ? new Curriculo(objCurriculo.IdCurriculo) : null
            };

            if (trans != null)
            {
                objMensagem.Save(trans);
                objLogoEnvioMensagem.Save(trans);
            }
            else
            {
                objMensagem.Save();
                objLogoEnvioMensagem.Save();
            }

            try
            {
                var MailSenderAPIKey = Parametro.RecuperaValorParametro(Enumeradores.Parametro.MailSenderAPIKey);

                //MailSenderAPIKey = "IUZMSQ9YY8MC5TRXHOWW";

                if (!string.IsNullOrWhiteSpace(MailSenderAPIKey))
                {
                    if (objMensagem.PossuiAnexo())
                    {
                        _emailService.EnviarEmail(MailSenderAPIKey, objMensagem.DescricaoEmailRemetente, objMensagem.DescricaoEmailDestinatario, objMensagem.DescricaoEmailAssunto, objMensagem.DescricaoMensagem, objMensagem.NomeAnexo, Convert.ToBase64String(objMensagem.ArquivoAnexo));
                    }
                    else
                    {
                        _emailService.EnviarEmail(MailSenderAPIKey, objMensagem.DescricaoEmailRemetente, objMensagem.DescricaoEmailDestinatario, objMensagem.DescricaoEmailAssunto, objMensagem.DescricaoMensagem);
                    }
                    objMensagem.SalvarDataEnvio(trans);
                }
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }

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

                ProcessoAssincrono.IniciarAtividade(TipoAtividade.EnvioSMS, parametros);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            #endregion Envio para o Assincrono

            return objMensagem.IdMensagemCS;
        }
        #endregion

        #region EnviaSMSTanque
        public static int EnviaSMSTanque(Curriculo curriculo, PessoaFisica pessoa, UsuarioFilialPerfil usuarioFilialPerfil, UsuarioFilialPerfil usuarioFilialPerfilDes, string mensagem, string dddTelefone, string numeroTelefone, UsuarioSistemaTanque usuarioTanque, SqlTransaction trans = null, int? Idf_Mensagem_Campanha = null)
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
                    {"mensagem", "Mensagem", objMensagem.DescricaoMensagem, objMensagem.DescricaoMensagem},
                    {"Idf_Mensagem_Campanha", "Idf_Mensagem_Campanha", Idf_Mensagem_Campanha.ToStringNullSafe(), Idf_Mensagem_Campanha.ToStringNullSafe()}
                };

                ProcessoAssincrono.IniciarAtividade(TipoAtividade.EnvioSMSTanque, parametros);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
            #endregion Envio para o Assincrono

            return objMensagem.IdMensagemCS;
        }
        #endregion

        #region QuantidadeMensagensRecebidas
        public static int QuantidadeMensagensRecebidas(int idUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Usuario_Filial_Des", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil}
            };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcountmensagensrecebidas, parms));
        }
        #endregion

        #region QuantidadeMensagensRecebidas_NaoLidas
        public static int QuantidadeMensagensRecebidas_NaoLidas(int idUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil}
            };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "SP_Mensagens_Enviadas_Selecionadora", parms));
        }
        #endregion

        #region QuantidadeMensagensEnviadas
        public static int QuantidadeMensagensEnviadas(int idUsuarioFilialPerfil)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil}
            };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "[BNE].[Mensagens_Enviadas_Quantitativo]", parms));
        }
        #endregion

        #region CarregarMensagensRecebidas
        /// <summary>
        ///     Carrega as mensagens recebidas do usuarioFilialPerfil informado
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
        ///     Carrega as mensagens enviadas do usuarioFilialPerfil informado
        /// </summary>
        public static DataTable CarregarMensagensEnviadas(int idUsuarioFilialPerfil, int paginaCorrente, int tamanhoPagina, string desPesquisa, out int totalRegistros)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter("@PageIndex", SqlDbType.Int, 4),
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
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, Spmensagensenviadas, parms))
                {
                    dt = new DataTable();
                    dt.Load(dr);

                    if (dt.Rows.Count > 0)
                    {
                        totalRegistros = Convert.ToInt32(dt.Rows[0]["TotalCount"]);
                    }
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
            using (var dr = ListaDataReaderAnteriorProximaMensagensEnviadas(idMensagemCS, idUsuarioFilialPerfil, desPesquisa))
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
            using (var dr = ListaDataReaderAnteriorProximaMensagensRecebidas(idMensagemCS, idUsuarioFilialPerfil, desPesquisa))
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
        ///     M�todo auxiliar utilizado pelos m�todos LoadObject e CompleteObject para percorrer um IDataReader e vincular as
        ///     colunas com os atributos da classe.
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

                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Des_Email_Destinatario"])))
                        objMensagemCS._descricaoEmailDestinatario = Convert.ToString(dr["Des_Email_Destinatario"]);
                    else
                        objMensagemCS._descricaoEmailDestinatario = string.Empty;

                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Des_Email_Assunto"])))
                        objMensagemCS._descricaoEmailAssunto = Convert.ToString(dr["Des_Email_Assunto"]);
                    else
                        objMensagemCS._descricaoEmailAssunto = Convert.ToString(dr["Des_Email_Assunto"]);

                    objMensagemCS._statusMensagemCS = new StatusMensagemCS(Convert.ToInt32(dr["Idf_Status_Mensagem_CS"]));
                    objMensagemCS._descricaoEmailRemetente = Convert.ToString(dr["Des_Email_Remetente"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Num_DDD_Celular"])))
                        objMensagemCS._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
                    else
                        objMensagemCS._numeroDDDCelular = string.Empty;

                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Num_Celular"])))
                        objMensagemCS._numeroCelular = Convert.ToString(dr["Num_Celular"]);
                    else
                        objMensagemCS._numeroCelular = string.Empty;

                    objMensagemCS._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Nme_Anexo"])))
                        objMensagemCS._nomeAnexo = Convert.ToString(dr["Nme_Anexo"]);
                    else
                        objMensagemCS._nomeAnexo = string.Empty;

                    if (!string.IsNullOrEmpty(Convert.ToString(dr["Des_Obs"])))
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
                new SqlParameter {ParameterName = "@Idf_Usuario_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objUsuarioFilialPerfilRemetente.IdUsuarioFilialPerfil},
                new SqlParameter {ParameterName = "@Idf_Usuario_Filial_Des", SqlDbType = SqlDbType.Int, Size = 4, Value = objUsuarioFilialPerfilDestinatario.IdUsuarioFilialPerfil},
                new SqlParameter {ParameterName = "@Idf_Tipo_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = objTipoMensagemCS.IdTipoMensagemCS}
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

        #region EnviarNotificacaoVisualizacaoCurriculo
        public static void EnviarNotificacaoVisualizacaoCurriculo(PessoaFisica objPessoaFisica, string nomeFantasiaEmpresa, bool flagVIP, SqlTransaction trans)
        {
            objPessoaFisica.CompleteObject(trans);
            Curriculo objCurriculo;
            Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);
            if (flagVIP)
            {
                var mensagem = string.Format(ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.MensagemSMSVisualizacaoCurriculoParaVIP), objPessoaFisica.PrimeiroNome, nomeFantasiaEmpresa);

                EnviaSMSTanque(objCurriculo, objPessoaFisica, null, null, mensagem, objPessoaFisica.NumeroDDDCelular, objPessoaFisica.NumeroCelular, UsuarioSistemaTanque.QuemMeViu);
            }
            else//não vip enviar e-mail
            {
                #region [MontarCarta]
                try
                {
                    if (!string.IsNullOrEmpty(objPessoaFisica.EmailPessoa))
                    {
                        int ultimaCarta = LogEnvioMensagem.RetornaIdUltimaCartaEnviadaQuemMeViu(objPessoaFisica.EmailPessoa);

                        var CartaLayout = Enumeradores.CartaEmail.QuemMeViuSmile;
                        switch (++ultimaCarta)
                        {
                            case (int)Enumeradores.CartaEmail.CIACarrinhoAbandonado: //se atravesou na sequencia de idf 
                            case (int)Enumeradores.CartaEmail.QuemMeViuCheck:
                                CartaLayout = Enumeradores.CartaEmail.QuemMeViuCheck;
                                break;
                            case (int)Enumeradores.CartaEmail.QuemMeViuLupaCvs:
                                CartaLayout = Enumeradores.CartaEmail.QuemMeViuLupaCvs;
                                break;
                            case (int)Enumeradores.CartaEmail.QuemMeViuLupaLista:
                                CartaLayout = Enumeradores.CartaEmail.QuemMeViuLupaLista;
                                break;
                            case (int)Enumeradores.CartaEmail.QuemMeViuWantYou:
                                CartaLayout = Enumeradores.CartaEmail.QuemMeViuWantYou;
                                break;
                            case (int)Enumeradores.CartaEmail.QuemMeViuBatLuz:
                                CartaLayout = Enumeradores.CartaEmail.QuemMeViuBatLuz;
                                break;
                        }

                        //TODO: Melhorar esse envio, colocar em um lugar unico
                        var mailsenderId = CartaLayout.GetAttribute<MailsenderAttribute>().Id;

                        var logEnvioMensagem = new LogEnvioMensagem
                        {
                            EmlDestinatario = objPessoaFisica.EmailPessoa,
                            CartaEmail = new CartaEmail((int)CartaLayout),
                            EmlRemetente = EmailRemetente
                        };
                        logEnvioMensagem.Save();

                        var parametros = new MailsenderParameters<MailsenderSubstitutionParametersQuemMeViu, MailsenderSectionParametersQuemMeViu>
                        {
                            Substitution = new MailsenderSubstitutionParametersQuemMeViu()
                        };

                        parametros.Substitution.nome.Add(objPessoaFisica.NomeCompleto.Trim());
                        parametros.Substitution.link.Add(LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica.CPF, objPessoaFisica.DataNascimento, "/" + Rota.RecuperarURLRota(Enumeradores.RouteCollection.QuemMeViuSemPlano)));

                        var to = new List<string> { objPessoaFisica.EmailPessoa };

                        _emailService.EnviarEmail(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MailSenderAPIKey), EmailRemetente, to, mailsenderId, parametros.Substitution);
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Enviar e-mail do quem me viu para não vip ");
                }

                #endregion
            }
        }
        #endregion

        #region EnviarCompartilhamentoVaga
        public static void EnviarCompartilhamentoVaga(PessoaFisica objPessoaFisica, Vaga objVaga, string urlVaga, string emailDestinatario)
        {
            objVaga.CompleteObject();
            objVaga.Filial.CompleteObject();
            objVaga.Cidade.CompleteObject();
            objVaga.Cidade.Estado.CompleteObject();
            if (objVaga.Escolaridade != null)
                objVaga.Escolaridade.CompleteObject();

            #region Renderiza��o dos dados do remetente
            var nomeCompletoRemetente = objPessoaFisica.NomeCompleto;
            var emailRemetente = objPessoaFisica.EmailPessoa;
            #endregion

            #region Renderização da atribui��o
            var atribuicoesVaga = objVaga.DescricaoAtribuicoes;
            if (!string.IsNullOrEmpty(atribuicoesVaga) && atribuicoesVaga.Length > 280)
                atribuicoesVaga = atribuicoesVaga.Substring(0, 280) + "...";
            #endregion

            #region Renderiza��o do requisito
            var requisitos =
                string.Format("{0} {1}", objVaga.Escolaridade == null ?
                    string.Empty :
                    objVaga.Escolaridade.DescricaoBNE, objVaga.DescricaoRequisito);
            if (!string.IsNullOrEmpty(requisitos) && requisitos.Length > 280)
                requisitos = requisitos.Substring(0, 280) + "...";
            #endregion

            #region Renderiza��o do sal�rio
            var salarioFormat = string.Empty;
            var salarioDe = objVaga.ValorSalarioDe ?? decimal.Zero;
            var salarioPara = objVaga.ValorSalarioPara ?? decimal.Zero;
            if (salarioDe == decimal.Zero || salarioPara == decimal.Zero)
                salarioFormat = "Salário a combinar";
            else if (salarioDe == salarioPara)
                salarioFormat = "Salário de R$ {0}";
            else
                salarioFormat = "Salário de R$ {0} a R$ {1}";
            var salario = string.Format(salarioFormat,
                salarioDe.ToString("0,0.00", CultureInfo.CurrentCulture),
                salarioPara.ToString("0,0.00", CultureInfo.CurrentCulture));
            #endregion

            #region Renderiza��o da logo da empresa
            var urlLogoFormat = "http://{0}/Handlers/PessoaJuridicaLogo.ashx?cnpj={1}&Origem=Local";
            var urlLogo = string.Format(urlLogoFormat,
                Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente),
                objVaga.Filial.NumeroCNPJ);
            if (objVaga.FlagConfidencial)
                urlLogo = string.Empty;
            #endregion

            #region Renderiza��o da cidade e UF da vaga
            var cidade = objVaga.Cidade.NomeCidade;
            var uf = objVaga.Cidade.Estado.SiglaEstado;
            var cidadeEstado = Helper.FormatarCidade(cidade, uf);
            #endregion

            #region Obten��o dos templates de HTML
            string formatAssunto;
            var formatConteudoPadrao =
                CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.CompartilhamentoVagaPorEmail, out formatAssunto);
            #endregion

            #region Montagem da mensagem
            //Funcao estagio
            var funcao = VagaCurso.ListaCursoParaCartaEmail(objVaga.DescricaoFuncao, objVaga.IdVaga);

            object parametrosCorpoEmail = new
            {
                UrlLogoEmpresa = urlLogo,
                Funcao = funcao ?? string.Empty,
                Salario = salario,
                Cidade = cidadeEstado,
                Atribuicoes = atribuicoesVaga ?? string.Empty,
                Requisitos = requisitos ?? string.Empty,
                UrlVaga = urlVaga,
                NomeRemetente = nomeCompletoRemetente,
                EmailRemetente = emailRemetente,
                UtmCampaign = String.Format("v_{0}", objVaga.IdVaga)
            };

            object parametrosAssuntoEmail = new
            {
                NomeRemetente = nomeCompletoRemetente
            };

            var desMensagem = FormatObject.ToString(parametrosCorpoEmail, formatConteudoPadrao);
            var desAssunto = FormatObject.ToString(parametrosAssuntoEmail, formatAssunto);
            #endregion

            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(desAssunto, desMensagem, Enumeradores.CartaEmail.CompartilhamentoVagaPorEmail, emailRemetente, emailDestinatario);
        }
        #endregion

        #region PodeEnviarMensagem
        /// <summary>
        ///     Metodo responsavel por verificar se a mensagem foi enviada ha mais de 1 minuto. Se foi, permitir inser��o na tabela
        ///     de mensagem.
        /// </summary>
        /// <returns></returns>
        public static bool PodeEnviarMensagem(int idUsuarioFilialPerfil, int idTipoMensagem, int idUsuarioFilialDes)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Usuario_Filial_Perfil", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialPerfil},
                new SqlParameter {ParameterName = "@Idf_Tipo_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = idTipoMensagem},
                new SqlParameter {ParameterName = "@Idf_Usuario_Filial_Des", SqlDbType = SqlDbType.Int, Size = 4, Value = idUsuarioFilialDes}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spverificaminutoscadastromensagem, parms))
            {
                return !dr.Read(); // se o m�todo IDataReader.Read() retornar false de cara, � porque a consulta n�o retornou nenhuma linha 
                // e, portanto, n�o teve envio de email nos ultimos minutos
            }
        }
        #endregion

        #region SalvarDataEnvio
        public void SalvarDataEnvio(SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = IdMensagemCS}
            };

            if (trans != null)
            {
                DataAccessLayer.ExecuteScalar(trans, CommandType.Text, SpSalvarDataEnvio, parms);
            }
            else
            {
                DataAccessLayer.ExecuteScalar(CommandType.Text, SpSalvarDataEnvio, parms);
            }
        }
        #endregion SalvarDataEnvio

        #region SalvarErro
        public void SalvarErro()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = IdMensagemCS}
            };

            DataAccessLayer.ExecuteScalar(CommandType.Text, SpSalvarErro, parms);
        }
        #endregion SalvarDataEnvio

        #region EnvioDeEmailComValidacao
        /// <summary>
        ///     Método efetua o envio do email, e armazena a informação na tabela Mensagem_CS
        /// </summary>
        /// <param name="enumTipoEnviadorEmail"></param>
        /// <param name="assunto"></param>
        /// <param name="mensagem"></param>
        /// <param name="emailRemetente"></param>
        /// <param name="emailContato"></param>
        public static void EnvioDeEmailComValidacao(TipoEnviadorEmail enumTipoEnviadorEmail, string assunto, string mensagem, Enumeradores.CartaEmail? carta, string emailRemetente, string emailDestinatario, string nomeArquivoAnexo = null, byte[] arquivoAnexo = null, SqlTransaction trans = null)
        {
            var emails = Regex.Split(emailDestinatario, @"[\;\,\ ]");
            var rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            foreach (var email in emails)
            {
                try
                {
                    if (rg.IsMatch(email))
                    {
                        EmailSenderFactory.Create(enumTipoEnviadorEmail).Enviar(assunto, mensagem, carta, emailRemetente, email, nomeArquivoAnexo, arquivoAnexo, trans);
                    }
                }
                catch (Exception ex)
                {
                    GerenciadorException.GravarExcecao(ex, "problema no envio de da menssagem");
                }
            }
        }
        #endregion

        #region Ler
        public void Ler()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Mensagem_CS", SqlDbType = SqlDbType.Int, Size = 4, Value = this._idMensagemCS }
                };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpLer, parms);
        }
        #endregion

        #region EnvioSMSTanque
        public static void EnvioSMSTanque(string idUsuarioFilialPerfilRemetente, List<DestinatarioSMS> listaMensagens, bool agendar = false)
        {
            _smsService.Enviar(listaMensagens, idUsuarioFilialPerfilRemetente, agendar);
        }
        #endregion

        #region PossuiAnexo
        public bool PossuiAnexo()
        {
            return !string.IsNullOrWhiteSpace(NomeAnexo) && ArquivoAnexo != null;
        }
        #endregion

        #region GetAllToSend
        public static IEnumerable<MensagemCS> GetAllToSend()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "[bne].[RecuperarMensagemCSAguardandoEnvio]", null))
            {
                while (dr.Read())
                {
                    var mensagemCs = new MensagemCS();
                    SetInstanceNotDispose(dr, mensagemCs);
                    yield return mensagemCs;
                }
            }
        }
        #endregion

        #region SetInstanceNotDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objMensagemCS">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Vinicius Maciel</remarks>
        private static void SetInstanceNotDispose(IDataReader dr, MensagemCS objMensagemCS)
        {
            try
            {
                objMensagemCS._idMensagemCS = Convert.ToInt32(dr["Idf_Mensagem_CS"]);
                if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                    objMensagemCS._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                if (dr["Idf_Curriculo"] != DBNull.Value)
                    objMensagemCS._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                objMensagemCS._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);
                if (dr["Dta_Envio"] != DBNull.Value)
                    objMensagemCS._dataEnvio = Convert.ToDateTime(dr["Dta_Envio"]);
                if (dr["Idf_Rede_Social_CS"] != DBNull.Value)
                    objMensagemCS._redeSocialCS = new RedeSocialCS(Convert.ToInt32(dr["Idf_Rede_Social_CS"]));
                objMensagemCS._tipoMensagemCS = new TipoMensagemCS(Convert.ToInt32(dr["Idf_Tipo_Mensagem_CS"]));
                if (dr["Des_Email_Destinatario"] != DBNull.Value)
                    objMensagemCS._descricaoEmailDestinatario = Convert.ToString(dr["Des_Email_Destinatario"]);
                if (dr["Des_Email_Assunto"] != DBNull.Value)
                    objMensagemCS._descricaoEmailAssunto = Convert.ToString(dr["Des_Email_Assunto"]);
                objMensagemCS._statusMensagemCS = new StatusMensagemCS(Convert.ToInt32(dr["Idf_Status_Mensagem_CS"]));
                objMensagemCS._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objMensagemCS._descricaoEmailRemetente = Convert.ToString(dr["Des_Email_Remetente"]);
                if (dr["Num_DDD_Celular"] != DBNull.Value)
                    objMensagemCS._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
                if (dr["Num_Celular"] != DBNull.Value)
                    objMensagemCS._numeroCelular = Convert.ToString(dr["Num_Celular"]);
                objMensagemCS._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                if (dr["Nme_Anexo"] != DBNull.Value)
                    objMensagemCS._nomeAnexo = Convert.ToString(dr["Nme_Anexo"]);
                if (dr["Des_Obs"] != DBNull.Value)
                    objMensagemCS._descricaoObs = Convert.ToString(dr["Des_Obs"]);
                objMensagemCS._idSistema = Convert.ToInt32(dr["Idf_Sistema"]);
                objMensagemCS._idCentroServico = Convert.ToInt32(dr["Idf_Centro_Servico"]);
                if (dr["Arq_Anexo"] != DBNull.Value)
                    objMensagemCS._arquivoAnexo = ((byte[])(dr["Arq_Anexo"]));
                if (dr["Idf_Mensagem_Sistema"] != DBNull.Value)
                    objMensagemCS._mensagemSistema = new MensagemSistema(Convert.ToInt32(dr["Idf_Mensagem_Sistema"]));
                if (dr["Idf_Usuario_Filial_Des"] != DBNull.Value)
                    objMensagemCS._usuarioFilialDes = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Des"]));
                objMensagemCS._flagLido = Convert.ToBoolean(dr["Flg_Lido"]);

                objMensagemCS._persisted = true;
                objMensagemCS._modified = false;


            }
            catch { }

            return;
        }
        #endregion

        #endregion

    }
}