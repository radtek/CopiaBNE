using BNE.BLL;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Enumeradores = BNE.BLL.AsyncServices.Enumeradores;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "RastreadorVagas")]
    public class RastreadorVagas : InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idVaga = objParametros["idVaga"].ValorInt;

            try
            {
                if (idVaga.HasValue)
                {
                    Object retorno = GerarPerfilPorVaga(idVaga.Value);

                    var mensagemEmail = retorno as List<MensagemPlugin.MensagemEmail>;
                    if (mensagemEmail != null)
                        return new MensagemPlugin(this, mensagemEmail, false);
                }
            }
            catch (Exception ex)
            {
                Core.LogError(ex);
            }
            return new MensagemPlugin(this, true);
        }
        #endregion

        #region Métodos

        #region CarregarPerfilPorVaga
        private static DataTable CarregarPerfilPorVaga(Vaga objVaga)
        {
            int? idSexo = objVaga.IdSexo;
            int? idadeMinima = objVaga.IdadeMinima;
            int? idadeMaxima = objVaga.IdadeMaxima;
            decimal? valorSalarioMinimo = objVaga.ValorSalarioInicial;
            decimal? valorSalarioMaximo = objVaga.ValorSalarioFinal;

            int? pesoEscolaridade = null;
            if (objVaga.Escolaridade != null)
                pesoEscolaridade = objVaga.Escolaridade.SequenciaPeso;

            return PesquisarCurriculosRastreadorVaga(objVaga.IdFuncao, objVaga.IdCidade, idSexo, idadeMinima, idadeMaxima, pesoEscolaridade, valorSalarioMinimo, valorSalarioMaximo);
        }
        #endregion

        #region GerarPerfilPorVaga
        public static object GerarPerfilPorVaga(int idVaga)
        {
            var lista = new List<MensagemPlugin.MensagemEmail>();

            //Adicionando Cadastro de Alertas
            GerarAlertaVagas(idVaga);

            var objVaga = new Vaga(idVaga);

            DataTable dt = CarregarPerfilPorVaga(objVaga);

            //Verifica se existem curriculos com perfil da Vaga.
            if (dt.Rows.Count <= 0)
            {
                var sb = new StringBuilder();

                string emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailEnvioVagaSistema);
                string templateEmail = CarregarCartaEmail(objVaga);
                //Adicionando a equipe de caça cv's
                var mandaremailequipecacacv = RecuperaValorParametro(Convert.ToInt32(Enumeradores.Parametro.RastreadorVagasMandarEmailEquipeCacaCV));
                if (!string.IsNullOrWhiteSpace(mandaremailequipecacacv) && Convert.ToBoolean(mandaremailequipecacacv))
                {
                    var listaEmails = RecuperaValorParametro(Convert.ToInt32(Enumeradores.Parametro.RastreadorVagasDestinatariosEquipeCacaCV));
                    string assuntoCacaCvs = "Nova vaga {0}! " + sb.ToString();
                    const string nomeCacaCvs = "Equipe Caça CV's";

                    lista.AddRange(listaEmails.Split(';').Select(email => new MensagemPlugin.MensagemEmail
                    {
                        Descricao = string.Format(templateEmail, nomeCacaCvs),
                        Assunto = string.Format(assuntoCacaCvs, idVaga),
                        To = email,
                        From = emailRemetente
                    }));
                }
            }
            else
            {
                string emailRemetente = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailEnvioVagaSistema);
                string templateEmail = CarregarCartaEmail(objVaga);
                //Adicionando a equipe de caça cv's
                var mandaremailequipecacacv = RecuperaValorParametro(Convert.ToInt32(Enumeradores.Parametro.RastreadorVagasMandarEmailEquipeCacaCV));
                if (!string.IsNullOrWhiteSpace(mandaremailequipecacacv) && Convert.ToBoolean(mandaremailequipecacacv))
                {
                    var listaEmails = RecuperaValorParametro(Convert.ToInt32(Enumeradores.Parametro.RastreadorVagasDestinatariosEquipeCacaCV));
                    const string assuntoCacaCvs = "Nova vaga {0}!";
                    const string nomeCacaCvs = "Equipe Caça CV's";

                    lista.AddRange(listaEmails.Split(';').Select(email => new MensagemPlugin.MensagemEmail
                    {
                        Descricao = string.Format(templateEmail, nomeCacaCvs),
                        Assunto = string.Format(assuntoCacaCvs, idVaga),
                        To = email,
                        From = emailRemetente
                    }));
                }
            }

            return lista;
        }
        #endregion

        #region PesquisarCurriculosRastreadorVaga
        public static DataTable PesquisarCurriculosRastreadorVaga(int idFuncao, int idCidade, int? idSexo, int? idadeMinima, int? idadeMaxima, int? pesoEscolaridade, decimal? valorSalarioMinimo, decimal? valorSalarioMaximo)
        {

            #region Parâmetros
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = idFuncao } ,
                    new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Size = 4, Value = idCidade } ,
                    new SqlParameter { ParameterName = "@Idf_Sexo", SqlDbType = SqlDbType.Int, Size = 4, Value = idSexo.HasValue ? (int)idSexo : (object)DBNull.Value } ,
                    new SqlParameter { ParameterName = "@Idade_Minima", SqlDbType = SqlDbType.Int, Size = 4, Value = idadeMinima.HasValue ? (int)idadeMinima : (object)DBNull.Value } ,
                    new SqlParameter { ParameterName = "@Idade_Maxima", SqlDbType = SqlDbType.Int, Size = 4, Value = idadeMaxima.HasValue ? (int)idadeMaxima : (object)DBNull.Value } ,
                    new SqlParameter { ParameterName = "@Peso_Escolaridade", SqlDbType = SqlDbType.Int, Size = 4, Value = pesoEscolaridade.HasValue ? (int)pesoEscolaridade : (object)DBNull.Value } ,
                    new SqlParameter { ParameterName = "@Valor_Salario_Minimo", SqlDbType = SqlDbType.Int, Size = 4, Value = valorSalarioMinimo.HasValue ? (decimal)valorSalarioMinimo : (object)DBNull.Value } ,
                    new SqlParameter { ParameterName = "@Valor_Salario_Maximo", SqlDbType = SqlDbType.Int, Size = 4, Value = valorSalarioMaximo.HasValue ? (decimal)valorSalarioMaximo : (object)DBNull.Value }
                };
            #endregion

            #region spselect
            const string spselect = @"
            DECLARE @Idf_Regiao_Metropolitana INT
            BEGIN
                SELECT  @Idf_Regiao_Metropolitana = Idf_Regiao_Metropolitana
                FROM    BNE.TAB_Regiao_Metropolitana_Cidade WITH(NOLOCK)
                WHERE   Idf_Cidade = @Idf_Cidade
            END

            DECLARE @query NVARCHAR(MAX)
            SET @query = '
            SELECT  DISTINCT(C.Idf_Curriculo) ,
		            PF.Nme_Pessoa ,
		            PF.Eml_Pessoa ,
		            PF.Num_DDD_Celular ,
		            PF.Num_Celular ,
		            BNE.SF_Calcula_Idade_Em_Anos(PF.Dta_Nascimento) ,
		            PF.Idf_Sexo ,
                    C.Flg_VIP ,
                    Cid.Sig_Estado
            FROM    bne.BNE_Curriculo C WITH(NOLOCK)
                    INNER JOIN bne.TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = C.Idf_Pessoa_Fisica
                    INNER JOIN BNE.TAB_Endereco E WITH(NOLOCK) ON PF.Idf_Endereco = E.Idf_Endereco 				
		            INNER JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON E.Idf_Cidade = Cid.Idf_Cidade
                    LEFT JOIN bne.BNE_Funcao_Pretendida FP WITH(NOLOCK) ON FP.Idf_Curriculo = C.Idf_Curriculo
                    LEFT JOIN BNE.TAB_Regiao_Metropolitana_Cidade RMC WITH(NOLOCK) ON RMC.Idf_Cidade = Cid.Idf_Cidade'
        
            IF ( @Peso_Escolaridade IS NOT NULL ) 
	            BEGIN
		            SET @query = @query + ' LEFT JOIN BNE.BNE_Formacao Form WITH(NOLOCK) ON Form.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
								            LEFT JOIN plataforma.TAB_Escolaridade Esco WITH(NOLOCK) ON Esco.Idf_Escolaridade = Form.Idf_Escolaridade'
	            END
					        
            SET @query = @query + '
		            WHERE   C.Flg_Inativo = 0
                    AND PF.Flg_Inativo = 0
                    AND (	C.Idf_Situacao_Curriculo = 1 /*Publicado*/ 
				            OR C.Idf_Situacao_Curriculo = 2 /*Aguardando Publicação*/ 
				            OR C.Idf_Situacao_Curriculo = 3 /*Crítica*/ 
				            OR C.Idf_Situacao_Curriculo = 4 /*Aguardando Revisão VIP*/
				            OR C.Idf_Situacao_Curriculo = 9 /*Revisado VIP*/ 
				            OR C.Idf_Situacao_Curriculo = 10 /*Auditado*/
			            )
                    AND DATEDIFF(DAY,C.Dta_Atualizacao,GETDATE()) <= 90 /* TODO: Parametrizar */
                    AND FP.Idf_Funcao = @Idf_Funcao
                    AND ( Cid.Idf_Cidade = @Idf_Cidade OR ( RMC.Idf_Regiao_Metropolitana = @Idf_Regiao_Metropolitana AND RMC.Flg_Inativo = 0 ) )'

            IF ( @Idade_Minima IS NOT NULL AND @Idade_Maxima IS NOT NULL ) 
                BEGIN
                    SET @query = @query + ' AND ( BNE.SF_Calcula_Idade_Em_Anos(PF.Dta_Nascimento) >= @Idade_Minima AND BNE.SF_Calcula_Idade_Em_Anos(PF.Dta_Nascimento) <= @Idade_Maxima ) '
                END
            ELSE 
                BEGIN
                    IF ( @Idade_Minima IS NOT NULL ) 
                        BEGIN
                          SET @query = @query + ' AND BNE.SF_Calcula_Idade_Em_Anos(PF.Dta_Nascimento) >= @Idade_Minima '
                        END

                    IF ( @Idade_Maxima IS NOT NULL ) 
                        BEGIN
                          SET @query = @query + ' AND BNE.SF_Calcula_Idade_Em_Anos(PF.Dta_Nascimento) <= @Idade_Maxima'
                        END
                END
    
            IF ( @Idf_Sexo IS NOT NULL ) 
                SET @query = @query + ' AND PF.Idf_Sexo = @Idf_Sexo '
                                                    
            IF ( @Peso_Escolaridade IS NOT NULL ) 
	            BEGIN
		            SET @query = @query + ' AND Esco.Seq_Peso >= @Peso_Escolaridade '
	            END
	
            IF ( @Valor_Salario_Minimo IS NOT NULL AND @Valor_Salario_Maximo IS NOT NULL ) 
                BEGIN
                    SET @query = @query + ' AND ( C.Vlr_Pretensao_Salarial >= @Valor_Salario_Minimo AND C.Vlr_Pretensao_Salarial <= @Valor_Salario_Maximo )'
                END
            ELSE 
                BEGIN
                    IF ( @Valor_Salario_Minimo IS NOT NULL ) 
                        BEGIN
                          SET @query = @query + ' AND C.Vlr_Pretensao_Salarial >= @Valor_Salario_Minimo'
                        END

                    IF ( @Valor_Salario_Maximo IS NOT NULL ) 
                        BEGIN
                          SET @query = @query + ' AND C.Vlr_Pretensao_Salarial <= @Valor_Salario_Maximo'
                        END
                END	

            DECLARE @ParmDefinition NVARCHAR(1000)
            SET @ParmDefinition = N'@Idf_Funcao INT, @Idf_Cidade INT, @Idf_Regiao_Metropolitana INT, @Idade_Minima INT, @Idade_Maxima INT, @Idf_Sexo INT, @Peso_Escolaridade INT, @Valor_Salario_Minimo NUMERIC(10,2), @Valor_Salario_Maximo NUMERIC(10,2)';

            EXEC sp_executesql 
	            @query, 
	            @ParmDefinition, 
	            @Idf_Funcao = @Idf_Funcao, 
	            @Idf_Cidade = @Idf_Cidade, 
	            @Idf_Regiao_Metropolitana = @Idf_Regiao_Metropolitana,
	            @Idade_Minima = @Idade_Minima,
	            @Idade_Maxima = @Idade_Maxima,
                @Idf_Sexo = @Idf_Sexo,
                @Peso_Escolaridade = @Peso_Escolaridade,
                @Valor_Salario_Minimo = @Valor_Salario_Minimo,
                @Valor_Salario_Maximo = @Valor_Salario_Maximo
            ";
            #endregion

            #region Retorno
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, spselect, parms).Tables[0];
            #endregion

        }
        #endregion

        #region CarregarCartaEmail

        private static string CarregarCartaEmail(Vaga objVaga)
        {
            string template = ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ConteudoPadraoEmailNovo);
            string urlSite = string.Concat("http://", Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLAmbiente));
            string urlImagens = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLImagens);

            string corpo = string.Format(ConteudoHTML.RecuperaValorConteudo(BLL.Enumeradores.ConteudoHTML.ConteudoEmailNovoRastreadorVaga), urlSite, objVaga.IdVaga, "{0}", MontarMensagemRastreadorVaga(objVaga, urlImagens));

            return string.Format(template, urlImagens, urlSite, corpo);
        }

        #endregion

        #region MontarMensagemRastreadorVaga
        private static string MontarMensagemRastreadorVaga(Vaga objVaga, string urlImagens)
        {
            var sb = new StringBuilder();
            sb.Append("<table align='center' cellpadding='0' cellspacing='0' style='line-height: 98%;' width='540'>");
            sb.AppendFormat("<tr><td background='{0}/templateemail/img_top_box_vaga.png' bgcolor='#DDF0F8' height='12'><font face='Arial, Helvetica, sans-serif' size='1'>&nbsp;</font></td></tr>", urlImagens);

            MontarMensagemRastreadorVagaConteudo(objVaga, sb);

            sb.AppendFormat("<tr><td background='{0}/templateemail/img_bottom_box_vaga.png' bgcolor='#DDF0F8' height='12'><font face='Arial, Helvetica, sans-serif' size='1'>&nbsp;</font></td></tr>", urlImagens);
            sb.Append("</table>");

            return sb.ToString();
        }
        private static string MontarMensagemRastreadorVagaConteudoRequisito(Vaga objVaga)
        {
            string descricaoRequisito = null;

            if (objVaga.Escolaridade != null)
                descricaoRequisito = string.Format("{0}<br>", objVaga.Escolaridade.DescricaoBNE);

            if (!string.IsNullOrEmpty(objVaga.DescricaoRequisito))
                descricaoRequisito += string.Format("{0}", objVaga.DescricaoRequisito);

            return descricaoRequisito;
        }
        private static void MontarMensagemRastreadorVagaConteudo(Vaga objVaga, StringBuilder sb)
        {
            sb.Append("<tr><td bgcolor='#DDF0F8'><table align='center' cellpadding='0' cellspacing='0' width='500'><tr><td><font size='2' face='Arial, Helvetica, sans-serif'><div>");

            #region Função e Quantidade de Vagas
            sb.AppendFormat("<b>{0} ({1} {2})</b><br>", objVaga.DescricaoFuncao, objVaga.QuantidadeVaga, objVaga.QuantidadeVaga.Equals(1) ? "vaga" : "vagas");
            #endregion

            #region Salário e Cidade
            string salario = "A combinar";

            if (objVaga.ValorSalarioInicial.HasValue && objVaga.ValorSalarioFinal.HasValue)
            {
                if (objVaga.ValorSalarioInicial.Value.Equals(objVaga.ValorSalarioFinal.Value))
                    salario = objVaga.ValorSalarioInicial.Value.ToString("C");
                else
                    salario = string.Format("{0} a {1}", objVaga.ValorSalarioInicial.Value.ToString("C"), objVaga.ValorSalarioFinal.Value.ToString("C"));
            }
            else if (objVaga.ValorSalarioInicial.HasValue)
                salario = objVaga.ValorSalarioInicial.Value.ToString("C");
            else if (objVaga.ValorSalarioFinal.HasValue)
                salario = objVaga.ValorSalarioFinal.Value.ToString("C");

            sb.AppendFormat("{0} - {1}/{2}<br>", salario, objVaga.NomeCidade, objVaga.SiglaEstado);
            #endregion

            #region Atribuições
            if (!string.IsNullOrEmpty(objVaga.DescricaoAtribuicoes))
                sb.AppendFormat("<b>Atribuições:</b> {0}<br>", objVaga.DescricaoAtribuicoes);
            #endregion

            #region Benefícios
            if (!string.IsNullOrEmpty(objVaga.DescricaoBeneficio))
                sb.AppendFormat("<b>Benefícios:</b> {0}<br>", objVaga.DescricaoBeneficio);
            #endregion

            #region Requisitos
            string descricao = MontarMensagemRastreadorVagaConteudoRequisito(objVaga);
            if (!string.IsNullOrEmpty(descricao))
                sb.AppendFormat("<b>Requisitos:</b> {0}<br>", descricao);
            #endregion

            #region Disponibilidade de Trabalho
            if (!string.IsNullOrEmpty(objVaga.DescricaoDisponibilidade))
                sb.AppendFormat("<b>Disponibilidade de Trabalho:</b> {0}<br>", objVaga.DescricaoDisponibilidade);
            #endregion

            #region Tipo de Vínculo
            if (!string.IsNullOrEmpty(objVaga.DescricaoTipoVinculo))
                sb.AppendFormat("<b>Tipo de Contrato:</b> {0}<br>", objVaga.DescricaoTipoVinculo);
            #endregion

            sb.Append("</div></font></td></tr></table></td></tr>");
        }
        #endregion

        #region RecuperaValorParametro

        /// <summary>
        /// Método que recupera o valor de um parâmetro a partir do id.
        /// </summary>
        /// <returns>Valor do parâmetro.</returns>
        public static string RecuperaValorParametro(int idParametro)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = idParametro }
                };

            const string spselvalor = @"
            SELECT  Vlr_Parametro
            FROM    plataforma.TAB_Parametro PAR WITH(NOLOCK)
            WHERE   PAR.Idf_Parametro = @idf_Parametro";

            Object retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, spselvalor, parms);

            if (retorno != null)
                return retorno.ToString();

            return null;
        }
        #endregion

        #region MétodosAlertaJornalVagas

        private static void GerarAlertaVagas(int Idvaga)
        {
            #region Parâmetros

            //Novavaga que irá obter os dados necessários
            BLL.Vaga novaVaga = null;

            #endregion

            #region VerificaVaga

            //Tenta recuperar Vaga
            novaVaga = BLL.Vaga.LoadObject(Idvaga);

            //Verifica se é vaga para PNE
            if (novaVaga.FlagDeficiencia == null)
                novaVaga.FlagDeficiencia = false;

            //Se existir perguntas e for vaga PNE não gera alerta
            if (!novaVaga.ExistePerguntas() && !Convert.ToBoolean(novaVaga.FlagDeficiencia))
            {
                #region VerificaCidadeFuncao

                //Cidade e Funcao Nulas, sai do processamento
                if (novaVaga.Cidade != null && novaVaga.Funcao != null)
                {
                    //Se não tiver Nome de Cidade e Sigla no Estado, sai do processamento
                    #region VerificaCidadeEstado

                    if (string.IsNullOrEmpty(novaVaga.Cidade.NomeCidade) || string.IsNullOrWhiteSpace(novaVaga.Cidade.NomeCidade))
                    {
                        try
                        {
                            if (novaVaga.Cidade.IdCidade > 0)
                            {
                                novaVaga.Cidade = Cidade.LoadObject(novaVaga.Cidade.IdCidade);
                                if (!novaVaga.Cidade.Estado.CompleteObject() && !string.IsNullOrEmpty(novaVaga.Cidade.Estado.SiglaEstado))
                                    novaVaga.Cidade.Estado = Estado.CarregarPorSiglaEstado(novaVaga.Cidade.Estado.SiglaEstado);
                            }
                            else
                            {
                                Cidade cidadeOut;
                                if (Cidade.CarregarPorNome(novaVaga.Cidade.NomeCidade, out cidadeOut))
                                {
                                    novaVaga.Cidade = cidadeOut;
                                    if (!novaVaga.Cidade.Estado.CompleteObject() && !string.IsNullOrEmpty(novaVaga.Cidade.Estado.SiglaEstado))
                                        novaVaga.Cidade.Estado = Estado.CarregarPorSiglaEstado(novaVaga.Cidade.Estado.SiglaEstado);
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }
                        catch
                        {
                            throw new NoDataFoundException("Gerar Alerta Vagas VAGA: " + novaVaga.IdVaga.ToString() + " - Verifica Cidade Estado", "Cidade Não Existente!");
                        }
                    }

                    #endregion

                    #region Processamento

                    #region InsereAlertasNovaVaga

                    try
                    {
                        //AlertaVagas.InsereAlertaVagas(novaVaga.IdVaga);
                    }
                    catch
                    {
                        throw new NoDataFoundException("VAGA: " + novaVaga.IdVaga.ToString(), "Erro ao tentar processar PROC inserção de Nova Vaga");
                    }

                    #endregion

                    #region CandidatarAuto

                    try
                    {
                        VagaCandidato.CandidatarAuto(novaVaga.IdVaga);
                    }
                    catch (Exception ex)
                    {
                        throw new NoDataFoundException("Gerar Alerta Vagas VAGA: " + novaVaga.IdVaga.ToString() + " - Candidatar Auto", ex.Message);
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    throw new NoDataFoundException("Gerar Alerta Vagas - Verifica Cidade Estado", "Cidade ou Funcao Nulo!");
                }

                #endregion
            }

            #endregion

        }

        #endregion

        #endregion

        #region Internal Class

        internal class Vaga
        {

            public int IdVaga { get; set; }
            public string CodigoVaga { get; set; }
            public int IdFuncao { get; set; }
            public int IdCidade { get; set; }
            public int? IdSexo { get; set; }
            public Escolaridade Escolaridade { get; set; }
            public int? IdadeMinima { get; set; }
            public int? IdadeMaxima { get; set; }
            public string DescricaoFuncao { get; set; }
            public int QuantidadeVaga { get; set; }
            public string DescricaoRequisito { get; set; }
            public string DescricaoAtribuicoes { get; set; }
            public string DescricaoBeneficio { get; set; }
            public decimal? ValorSalarioInicial { get; set; }
            public decimal? ValorSalarioFinal { get; set; }
            public string NomeCidade { get; set; }
            public string SiglaEstado { get; set; }
            public string DescricaoDisponibilidade { get; set; }
            public string DescricaoTipoVinculo { get; set; }

            public Vaga(int idVaga)
            {

                #region spselectVaga
                const string spselectVaga = @"
                SELECT  V.Idf_Vaga, 
                        V.Cod_Vaga,
                        V.Idf_Funcao, 
                        Cid.Idf_Cidade, 
                        V.Idf_Sexo, 
                        V.Idf_Escolaridade, 
                        F.Des_Funcao,    
                        V.Des_Atribuicoes,
                        V.Des_Requisito,
                        V.Des_Beneficio,
                        V.Num_Idade_Minima, 
                        V.Num_Idade_Maxima, 
                        V.Vlr_Salario_De, 
                        V.Vlr_Salario_Para,
                        Cid.Nme_Cidade,
                        Cid.Sig_Estado,
                        F.Des_Funcao,
                        V.Qtd_Vaga ,
                        (
			                SELECT	D.Des_Disponibilidade + ' '
			                FROM	BNE_Vaga_Disponibilidade VD WITH(NOLOCK) 
					                LEFT JOIN TAB_Disponibilidade D WITH(NOLOCK) ON VD.Idf_Disponibilidade = D.Idf_Disponibilidade
			                WHERE	V.Idf_Vaga = VD.Idf_Vaga
			                FOR XML PATH('')
                        ) [Des_Disponibilidade] ,
                        (
			                SELECT	TV.Des_Tipo_Vinculo + ' '
			                FROM	BNE_Vaga_Tipo_Vinculo VTV WITH(NOLOCK)
					                LEFT JOIN BNE_Tipo_Vinculo TV WITH(NOLOCK) ON TV.Idf_Tipo_Vinculo = VTV.Idf_Tipo_Vinculo
			                WHERE	VTV.Idf_Vaga = V.Idf_Vaga
			                FOR XML PATH('')
                        ) [Des_Tipo_Vinculo]
                FROM    BNE_Vaga V WITH(NOLOCK)
                        INNER JOIN plataforma.TAB_Cidade Cid WITH(NOLOCK) ON V.Idf_Cidade = Cid.Idf_Cidade
                        INNER JOIN plataforma.TAB_Funcao F WITH(NOLOCK) ON V.Idf_Funcao = F.Idf_Funcao
                WHERE   V.Idf_Vaga = @Idf_Vaga
                ";
                #endregion

                #region Variáveis
                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                };
                #endregion

                #region Objeto Vaga
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselectVaga, parms))
                {
                    if (dr.Read())
                    {
                        IdVaga = Convert.ToInt32(dr["Idf_Vaga"]);

                        if (dr["Cod_Vaga"] != DBNull.Value)
                            CodigoVaga = dr["Cod_Vaga"].ToString();

                        if (dr["Idf_Funcao"] != DBNull.Value)
                            IdFuncao = Convert.ToInt32(dr["Idf_Funcao"]);

                        if (dr["Idf_Cidade"] != DBNull.Value)
                            IdCidade = Convert.ToInt32(dr["Idf_Cidade"]);

                        if (dr["Idf_Sexo"] != DBNull.Value)
                            IdSexo = Convert.ToInt32(dr["Idf_Sexo"]);

                        if (dr["Idf_Escolaridade"] != DBNull.Value)
                            Escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));

                        if (dr["Des_Funcao"] != DBNull.Value)
                            DescricaoFuncao = Convert.ToString(dr["Des_Funcao"]);

                        if (dr["Qtd_Vaga"] != DBNull.Value)
                            QuantidadeVaga = Convert.ToInt16(dr["Qtd_Vaga"]);

                        if (dr["Des_Atribuicoes"] != DBNull.Value)
                            DescricaoAtribuicoes = Convert.ToString(dr["Des_Atribuicoes"]);

                        if (dr["Des_Requisito"] != DBNull.Value)
                            DescricaoRequisito = Convert.ToString(dr["Des_Requisito"]);

                        if (dr["Des_Beneficio"] != DBNull.Value)
                            DescricaoBeneficio = Convert.ToString(dr["Des_Beneficio"]);

                        if (dr["Num_Idade_Minima"] != DBNull.Value)
                            IdadeMinima = Convert.ToInt32(dr["Num_Idade_Minima"]);

                        if (dr["Num_Idade_Maxima"] != DBNull.Value)
                            IdadeMaxima = Convert.ToInt32(dr["Num_Idade_Maxima"]);

                        if (dr["Vlr_Salario_De"] != DBNull.Value)
                            ValorSalarioInicial = Convert.ToInt32(dr["Vlr_Salario_De"]);

                        if (dr["Vlr_Salario_Para"] != DBNull.Value)
                            ValorSalarioFinal = Convert.ToInt32(dr["Vlr_Salario_Para"]);

                        if (dr["Nme_Cidade"] != DBNull.Value)
                            NomeCidade = Convert.ToString(dr["Nme_Cidade"]);

                        if (dr["Sig_Estado"] != DBNull.Value)
                            SiglaEstado = Convert.ToString(dr["Sig_Estado"]);

                        if (dr["Des_Disponibilidade"] != DBNull.Value)
                            DescricaoDisponibilidade = Convert.ToString(dr["Des_Disponibilidade"]);

                        if (dr["Des_Tipo_Vinculo"] != DBNull.Value)
                            DescricaoTipoVinculo = Convert.ToString(dr["Des_Tipo_Vinculo"]);

                    }
                }
                #endregion

            }

        }

        internal class Escolaridade
        {

            public int IdEscolaridade { get; set; }
            public int SequenciaPeso { get; set; }
            public string DescricaoBNE { get; set; }

            public Escolaridade(int idEscolaridade)
            {

                #region spselectescolaridade
                const string spselectescolaridade = @"
                SELECT  Idf_Escolaridade ,
                        Seq_Peso ,
                        Des_BNE
                FROM    plataforma.TAB_Escolaridade WITH(NOLOCK)
                WHERE   Idf_Escolaridade = @Idf_Escolaridade
                ";
                #endregion

                #region Variáveis
                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Escolaridade", SqlDbType = SqlDbType.Int, Size = 4, Value = idEscolaridade }
                };
                #endregion

                #region Objeto escolaridade
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselectescolaridade, parms))
                {
                    if (dr.Read())
                    {
                        IdEscolaridade = Convert.ToInt32(dr["Idf_Escolaridade"]);

                        if (dr["Des_BNE"] != DBNull.Value)
                            DescricaoBNE = Convert.ToString(dr["Des_BNE"]);

                        if (dr["Seq_Peso"] != DBNull.Value)
                            SequenciaPeso = Convert.ToInt16(dr["Seq_Peso"]);

                    }
                }
                #endregion

            }
        }

        #endregion

    }

}