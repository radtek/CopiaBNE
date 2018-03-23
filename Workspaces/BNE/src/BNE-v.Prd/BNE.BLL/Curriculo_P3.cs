//-- Data: 21/06/2010 14:15
//-- Autor: Eduardo Ruthes

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using BNE.BLL.Common;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Custom.Solr.Buffer;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.BLL.Mensagem.DTO;
using BNE.BLL.Mensagem;

namespace BNE.BLL
{
    public partial class Curriculo
    {

        #region Consultas
        private const string SelIdporcpf = @"
        SELECT  C.Idf_Curriculo
        FROM    BNE_Curriculo C WITH(NOLOCK)
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        WHERE   PF.Num_Cpf = @Num_Cpf";


        private const string Selporcpf = @"
        SELECT  C.*
        FROM    BNE_Curriculo C WITH(NOLOCK)
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
        WHERE   PF.Num_Cpf = @Num_Cpf";

        #region Spselvagaemmassa
        private const string Spselvagaemmassa = @"
        SELECT TOP 1000
                c.Idf_Curriculo ,
                pf.Eml_Pessoa ,
                perfil.Idf_Usuario_Filial_Perfil ,
                perfil.Idf_Filial,
                pf.Nme_Pessoa
        FROM    BNE.TAB_Pessoa_Fisica pf WITH(NOLOCK)
                JOIN BNE.BNE_Curriculo C WITH(NOLOCK) ON pf.Idf_Pessoa_Fisica = c.Idf_Pessoa_Fisica
                JOIN BNE.BNE_Funcao_Pretendida fp WITH(NOLOCK) ON c.Idf_Curriculo = fp.Idf_Curriculo
                JOIN BNE.TAB_Endereco en WITH(NOLOCK) ON pf.Idf_Endereco = en.Idf_Endereco
                JOIN BNE.TAB_Usuario_Filial_Perfil perfil WITH(NOLOCK) ON pf.Idf_Pessoa_Fisica = perfil.Idf_Pessoa_Fisica
        WHERE   fp.Idf_Funcao = @Idf_Funcao
                AND C.Dta_Atualizacao > DATEADD(DAY, -365, GETDATE())
                AND c.Idf_Curriculo NOT IN ( SELECT Idf_Curriculo
                                                FROM   BNE.BNE_Vaga_Divulgacao WITH(NOLOCK)
                                                WHERE  Idf_Vaga = @Idf_Vaga )
                AND ( C.Idf_Cidade_Pretendida = @Idf_Cidade
                        OR en.Idf_Cidade = @Idf_Cidade
                    )
                AND pf.Idf_Sexo = ISNULL(@Idf_Sexo, pf.Idf_Sexo)
                AND c.Flg_Inativo = 0
                AND perfil.Idf_Filial IS NULL
                AND pf.Eml_Pessoa IS NOT NULL
        ";
        #endregion

        #region [ SelInformacoesCurriculo ]
        private const string SelInformacoesCurriculo = @"
        SELECT c.Idf_Curriculo ,
          c.Flg_VIP ,
          vc.Idf_Vaga_Candidato AS JaEnviou ,
          cvf.Idf_Curriculo AS EmpresaBloqueada ,
          cid.Idf_Cidade AS EstaEmBH,
          experiencia.Idf_Experiencia_Profissional,
          formacao.Idf_Formacao,
          pc.Vlr_Parametro AS saldoCandidatura,
          pce.Vlr_Parametro AS naoTemExperiencia,
          pce.Dta_Alteracao AS dataNaoTemExperiencia,
          ISNULL(vc.Flg_Auto_Candidatura, 0) as Flg_Auto_Candidatura
        FROM BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK )
          JOIN BNE.BNE_Curriculo c WITH ( NOLOCK ) ON c.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
          LEFT JOIN BNE.BNE_Vaga_Candidato vc WITH ( NOLOCK ) ON vc.Idf_Curriculo = c.Idf_Curriculo
                         AND vc.Idf_Vaga = @Idf_Vaga
          LEFT JOIN BNE.bne_curriculo_nao_visivel_filial cvf WITH ( NOLOCK ) ON cvf.idf_curriculo = c.idf_curriculo AND cvf.Idf_Filial IN 
	        (SELECT Idf_Filial FROM BNE.BNE_Vaga v WITH (NOLOCK) WHERE v.Idf_Vaga = @Idf_Vaga)
          LEFT JOIN plataforma.TAB_Cidade cid WITH(NOLOCK) ON cid.Idf_Cidade = pf.Idf_Cidade
                      AND cid.Idf_Cidade = 1411
          LEFT JOIN BNE.TAB_Parametro_Curriculo as pc WITH ( NOLOCK )
					   ON pc.Idf_Curriculo = c.Idf_Curriculo AND pc.Idf_Parametro=@Idf_Parametro
          LEFT JOIN BNE.TAB_Parametro_Curriculo as pce WITH ( NOLOCK )
					   ON pce.Idf_Curriculo = c.Idf_Curriculo AND pce.Idf_Parametro=@Idf_Parametro_TemExperiencia
          OUTER APPLY ( SELECT TOP 1 Idf_Experiencia_Profissional
               FROM  BNE.BNE_Experiencia_Profissional ex WITH ( NOLOCK )
               WHERE  ex.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
             ) experiencia
        OUTER APPLY( SELECT top 1 Idf_Formacao FROM BNE.BNE_Formacao f WITH(NOLOCK) 
	           WHERE f.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica AND f.Flg_Inativo=0
	        ) formacao
        WHERE Num_CPF = @CPF";
        #endregion

        #region [ SelInformacoesCurriculoPorIdPessoa ]
        private const string SelInformacoesCurriculoPorIdPessoa = @"
        SELECT c.Idf_Curriculo ,
          c.Flg_VIP ,
          vc.Idf_Vaga_Candidato AS JaEnviou ,
          cvf.Idf_Curriculo AS EmpresaBloqueada ,
          cid.Idf_Cidade AS EstaEmBH,
          experiencia.Idf_Experiencia_Profissional,
          formacao.Idf_Formacao,
          pc.Vlr_Parametro AS saldoCandidatura,
          pce.Vlr_Parametro AS naoTemExperiencia,
          pce.Dta_Alteracao AS dataNaoTemExperiencia
        FROM BNE.TAB_Pessoa_Fisica pf WITH ( NOLOCK )
          JOIN BNE.BNE_Curriculo c WITH ( NOLOCK ) ON c.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
          LEFT JOIN BNE.BNE_Vaga_Candidato vc WITH ( NOLOCK ) ON vc.Idf_Curriculo = c.Idf_Curriculo
                         AND vc.Idf_Vaga = @Idf_Vaga
          LEFT JOIN BNE.bne_curriculo_nao_visivel_filial cvf WITH ( NOLOCK ) ON cvf.idf_curriculo = c.idf_curriculo AND cvf.Idf_Filial IN 
	        (SELECT Idf_Filial FROM BNE.BNE_Vaga v WITH (NOLOCK) WHERE v.Idf_Vaga = @Idf_Vaga)
          LEFT JOIN plataforma.TAB_Cidade cid WITH(NOLOCK) ON cid.Idf_Cidade = pf.Idf_Cidade
                      AND cid.Idf_Cidade = 1411
          LEFT JOIN BNE.TAB_Parametro_Curriculo as pc WITH ( NOLOCK )
					   ON pc.Idf_Curriculo = c.Idf_Curriculo AND pc.Idf_Parametro=@Idf_Parametro
          LEFT JOIN BNE.TAB_Parametro_Curriculo as pce WITH ( NOLOCK )
					   ON pce.Idf_Curriculo = c.Idf_Curriculo AND pce.Idf_Parametro=@Idf_Parametro_TemExperiencia
          OUTER APPLY ( SELECT TOP 1 Idf_Experiencia_Profissional
               FROM  BNE.BNE_Experiencia_Profissional ex WITH ( NOLOCK )
               WHERE  ex.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
             ) experiencia
        OUTER APPLY( SELECT top 1 Idf_Formacao FROM BNE.BNE_Formacao f WITH(NOLOCK) 
	           WHERE f.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica AND f.Flg_Inativo=0
	        ) formacao
        WHERE pf.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region SpCurriculoEnvioNotificacao
        private const string SpCurriculoEnvioNotificacao = @"
        
        SELECT
	        CV.Idf_Curriculo, 
	        pf.Idf_Pessoa_Fisica, 
	        pf.Eml_Pessoa, 
	        pf.Num_CPF, 
	        pf.Nme_Pessoa, 
            pf.Dta_Nascimento,
	        cv.Dta_Cadastro, 
	        pa.Dta_Inicio_Plano, 
	        ufp.Idf_Usuario_Filial_Perfil,
	        fp.Des_Funcao,
	        c.Nme_Cidade,
	        c.Sig_Estado,
			pf.Num_DDD_Celular,
			pf.Num_Celular,
		    pf.Idf_Sexo
        FROM BNE.BNE_Curriculo cv WITH(NOLOCK)
	        JOIN BNE.TAB_Pessoa_Fisica pf WITH(NOLOCK) ON cv.Idf_Pessoa_Fisica = pf.Idf_Pessoa_Fisica
	        JOIN plataforma.TAB_Cidade c WITH ( NOLOCK ) ON cv.Idf_Cidade_Endereco = c.Idf_Cidade
	        JOIN BNE.TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON pf.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
	        JOIN BNE.BNE_Plano_Adquirido pa WITH(NOLOCK) ON UFP.Idf_Usuario_Filial_Perfil = pa.Idf_Usuario_Filial_Perfil AND pa.Idf_Plano_Situacao = 1
			JOIN BNE.BNE_Plano p WITH(NOLOCK) ON pa.Idf_Plano = p.Idf_Plano
			JOIN BNE.BNE_Plano_Tipo pt WITH(NOLOCK) ON p.Idf_Plano_Tipo = pt.Idf_Plano_Tipo
	        LEFT JOIN BNE.TAB_Parametro_Curriculo pc WITH(NOLOCK) ON cv.Idf_Curriculo = pc.Idf_Curriculo AND PC.Idf_Parametro = 522         
	        CROSS APPLY ( SELECT TOP 1
                                            f.Des_Funcao
                                  FROM      BNE_Funcao_Pretendida fp WITH ( NOLOCK )
                                            JOIN plataforma.TAB_Funcao f WITH ( NOLOCK ) ON fp.Idf_Funcao = f.Idf_Funcao
                                  WHERE     fp.Idf_Curriculo = cv.Idf_Curriculo
                                  ORDER BY  fp.Dta_Cadastro
						          ) fp
        WHERE Flg_VIP = 1 
            AND	pt.Idf_Plano_Tipo = 1
	        AND PA.Idf_Plano_Situacao = 1
	        AND pa.Dta_Fim_Plano > GETDATE()
            AND cv.Idf_Situacao_Curriculo IN ( 1, 2, 3, 4, 9, 10 )
	        AND (CONVERT(DATE, Vlr_Parametro) < CONVERT(DATE, @data) OR Vlr_Parametro IS NULL)
        ORDER BY cv.Dta_Cadastro DESC

        ";
        #endregion

        #region [ SelectMetricasCurriculo ]
        private const string SelectMetricasCurriculo = @"EXEC BNE.SP_Metricas_CV @idf_Curriculo = @idf_Curriculo";
        #endregion

        #endregion

        #region Metodos

        #region CarregarPorCpf
        public static bool CarregarPorCpf(decimal numCpf, out Curriculo objCurriculo, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Num_Cpf", SqlDbType = SqlDbType.Decimal, Value = numCpf }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Selporcpf, parms))
            {
                objCurriculo = new Curriculo();
                if (!SetInstance(dr, objCurriculo))
                {
                    objCurriculo = null;
                    return false;
                }
            }

            return true;
        }

        public static bool CarregarIdPorCpf(decimal numCpf, out int curriculoId)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Num_Cpf", SqlDbType = SqlDbType.Decimal, Value = numCpf }
                };

            var ret = DataAccessLayer.ExecuteScalar(CommandType.Text, SelIdporcpf, parms);

            if (Convert.IsDBNull(ret) || ret == null || !Int32.TryParse(ret.ToString(), out curriculoId))
            {
                curriculoId = 0;
                return false;
            }

            return true;
        }
        #endregion

        #region ListarPorVagaEmMassa
        /// <summary>
        /// Seleciona curriculums que batam comm o perfil de uma vaga em massa
        /// </summary>
        /// <param name="idfFuncao">Código da função</param>
        /// <param name="idfVaga">Código da vaga</param>
        /// <param name="idfCidade">Código da cidade</param>
        /// <param name="idfSexo">Sexo</param>
        /// <returns>DataTable com os curriculumsselecionados</returns>
        public static DataTable ListarPorVagaEmMassa(int idfFuncao, int idfVaga, int idfCidade, int? idfSexo)
        {
            Object paramSexo = DBNull.Value;

            if (idfSexo.HasValue)
                paramSexo = idfSexo.Value;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Value = idfFuncao },
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Value = idfVaga },
                    new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Value = idfCidade },
                    new SqlParameter { ParameterName = "@Idf_Sexo", SqlDbType = SqlDbType.Int, Value = paramSexo }
                };

            var lstSituacaoCurriculo = new List<Enumeradores.SituacaoCurriculo>
                {
                    Enumeradores.SituacaoCurriculo.Publicado,
                    Enumeradores.SituacaoCurriculo.AguardandoPublicacao,
                    Enumeradores.SituacaoCurriculo.AguardandoRevisaoVIP,
                    Enumeradores.SituacaoCurriculo.RevisadoVIP,
                    Enumeradores.SituacaoCurriculo.Auditado
                };

            string query = Spselvagaemmassa + " AND c.Idf_Situacao_Curriculo IN (";

            for (int i = 0; i < lstSituacaoCurriculo.Count; i++)
            {
                string nomeParametro = lstSituacaoCurriculo[i].GetHashCode().ToString(CultureInfo.CurrentCulture);

                if (i > 0)
                    query += ", ";

                query += nomeParametro;
            }

            query += ")  ORDER BY c.dta_atualizacao DESC";

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, query, parms).Tables[0];
        }
        #endregion

        #region RetornaLinkVisualizacaoCurriculo
        /// <summary>
        /// Ajusta os links da visualização de curriculo
        /// </summary>
        /// <param name="lstIdCurriculo"></param>
        /// <returns></returns>
        public static String RetornaLinkVisualizacaoCurriculo(List<int> lstIdCurriculo)
        {
            var sb = new StringBuilder();

            foreach (int idCurriculo in lstIdCurriculo)
            {
                var curriculoDadosSitemap = Curriculo.RecuperarCurriculoSitemap(new Curriculo(idCurriculo));

                /* bug 15394 */
                if (curriculoDadosSitemap == null)
                    continue;

                string url = SitemapHelper.MontarUrlVisualizacaoCurriculo(curriculoDadosSitemap.DescricaoFuncao, curriculoDadosSitemap.NomeCidade, curriculoDadosSitemap.SiglaEstado, (int)curriculoDadosSitemap.IdfCurriculo);
                sb.AppendFormat("<a href=\"{0}\">{1}</a><br>", url, curriculoDadosSitemap.NomePessoa);
            }

            return sb.ToString();
        }
        #endregion

        #region URL
        public string URL()
        {
            var curriculoDadosSitemap = Curriculo.RecuperarCurriculoSitemap(this);

            if (curriculoDadosSitemap.IdfCurriculo != null)
                return SitemapHelper.MontarUrlVisualizacaoCurriculo(curriculoDadosSitemap.DescricaoFuncao, curriculoDadosSitemap.NomeCidade, curriculoDadosSitemap.SiglaEstado, (int)curriculoDadosSitemap.IdfCurriculo);

            return string.Empty;
        }
        #endregion

        #region LiberarVIP

        public static bool LiberarVIP(PlanoAdquirido objPlanoAdquirido, out Curriculo objCurriculo, SqlTransaction trans = null)
        {
            if (null == objPlanoAdquirido.UsuarioFilialPerfil
                || null == objPlanoAdquirido.UsuarioFilialPerfil.Perfil
                || null == objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica
                || String.IsNullOrEmpty(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NomePessoa))
            {
                if (null == trans)
                {
                    objPlanoAdquirido.CompleteObject();
                    objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                    objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CompleteObject();
                }
                else
                {
                    objPlanoAdquirido.CompleteObject(trans);
                    objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject(trans);
                    objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);
                }
            }

            if (!Curriculo.CarregarPorPessoaFisica(trans, objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo))
                return false;

            objCurriculo.FlagVIP = true;
            objCurriculo.SituacaoCurriculo = new SituacaoCurriculo((int)Enumeradores.SituacaoCurriculo.AguardandoRevisaoVIP);
            objCurriculo.Save(trans);

            //Atualiza o perfil do candidato
            objPlanoAdquirido.UsuarioFilialPerfil.Perfil = new Perfil((int)Enumeradores.Perfil.AcessoVIP);
            objPlanoAdquirido.UsuarioFilialPerfil.Save(trans);

            //Envia email

            if (objPlanoAdquirido.QuantidadeDeParcelasPagaPlanoAdquirido(trans) == 0)
            {
                var parmPrimeiroNome = new
                {
                    Primeiro_Nome = Helper.RetornarPrimeiroNome(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NomeCompleto)
                };

                if ((!String.IsNullOrEmpty(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.EmailPessoa) && !objPlanoAdquirido.PlanoLiberadoNaoPago)) //Só envia mensagem caso o usuário possua e-mail e plano pago e liberado
                {
                    string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, trans);

                    #region Confirmação de Pagamento
                    string assunto;
                    var template = CartaEmail.LoadObject((int)Enumeradores.CartaEmail.ConfirmacaoPagamentoVIP);
                    var parametros = new
                    {
                        Nome_Completo = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NomeCompleto
                    };
                    string mensagem = parametros.ToString(template.ValorCartaEmail);

                    assunto = parmPrimeiroNome.ToString(template.DescricaoAssunto);

                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(null, null, objPlanoAdquirido.UsuarioFilialPerfil, assunto, mensagem, Enumeradores.CartaEmail.ConfirmacaoPagamentoVIP, emailRemetente, objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.EmailPessoa, trans);
                    #endregion

                    //Retirada Task 39695
                    #region CartilhaVIP
                    //string assuntoCartilhaVIP;
                    //string mensagemCartilhaVIP = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.CartilhaVIP, out assuntoCartilhaVIP);

                    //EmailSenderFactory
                    //    .Create(TipoEnviadorEmail.Fila)
                    //    .Enviar(null, null, objPlanoAdquirido.UsuarioFilialPerfil, assuntoCartilhaVIP, mensagemCartilhaVIP, Enumeradores.CartaEmail.CartilhaVIP, emailRemetente, objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.EmailPessoa, trans);
                    #endregion
                }

                //Envia sms
                if (!string.IsNullOrEmpty(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular) && !string.IsNullOrEmpty(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroCelular))
                {
                    string sms = parmPrimeiroNome.ToString(CartaSMS.RecuperaValorConteudo(Enumeradores.CartaSMS.BoasVindasVIP));

                    if (objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.Sexo.IdSexo == (int)Enumeradores.Sexo.Feminino)
                        sms = sms.Replace("Bem-vindo", "Bem-vinda");

                    MensagemCS.SalvarSMS(null, null, objPlanoAdquirido.UsuarioFilialPerfil, sms, objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular, objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroCelular, trans);
                }
            }

            BufferAtualizacaoCurriculo.Update(objCurriculo);
            //Task 45613
            try
            {
                Notificacao.AlertaCurriculosAgenda.SalvarAlertaTodosOsDias(objCurriculo.IdCurriculo);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, String.Format("Não adicionou alerta para o cv {0} ao liberar o Vip", objCurriculo.IdCurriculo));
            }
            return true;
        }

        #endregion

        #region SalvarCurriculoPretencaoSalarial
        /// <summary>
        /// Salvar Pretensão salarial
        /// </summary>
        /// <param name="objCurriculo"></param>
        public void SalvarCurriculoPretencaoSalarial(Curriculo objCurriculo)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objCurriculo.Save(trans);

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

        public static DataTable CarregarInformacoesCurriculo(int idVaga, decimal cpf)
        {
            var objDataTable = new DataTable();
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Value = idVaga },
                    new SqlParameter { ParameterName = "@CPF", SqlDbType = SqlDbType.Decimal, Value = cpf },
                    new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Value = BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao },
                    new SqlParameter { ParameterName = "@Idf_Parametro_TemExperiencia", SqlDbType = SqlDbType.Int, Value = BLL.Enumeradores.Parametro.CurriculoSemExperiencia }

                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SelInformacoesCurriculo, parms))
            {
                objDataTable.Load(dr);
            }
            return objDataTable;

            //return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SelInformacoesCurriculo, parms).Tables[0];
        }

        public static DataTable CarregarInformacoesCurriculo(int idVaga, int idPessoaFisica)
        {
            var objDataTable = new DataTable();
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Value = idVaga },
                    new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Value = idPessoaFisica },
                    new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Value = BLL.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao },
                    new SqlParameter { ParameterName = "@Idf_Parametro_TemExperiencia", SqlDbType = SqlDbType.Int, Value = BLL.Enumeradores.Parametro.CurriculoSemExperiencia }

                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SelInformacoesCurriculoPorIdPessoa, parms))
            {
                objDataTable.Load(dr);
            }
            return objDataTable;
        }

        #region [ CarregarMetricasCurriculo ]
        /// <summary>
        /// Carregar Metricas como, visualizações, pesquisas, quem me viu
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static DataTable CarregarMetricasCurriculo(int idCurriculo)
        {
            var objDataTable = new DataTable();
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo },

                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SelectMetricasCurriculo, parms))
            {
                objDataTable.Load(dr);
            }

            return objDataTable;
        }
        #endregion

        #region ListarCurriculosEnvioNotificacao
        public static DataTable ListarCurriculosEnvioNotificacao(DateTime data)
        {
            var objDataTable = new DataTable();
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@data", SqlDbType = SqlDbType.DateTime, Value = data },
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpCurriculoEnvioNotificacao, parms))
            {
                objDataTable.Load(dr);
            }

            return objDataTable;
        }
        #endregion

        #region CarregarExtratoCurriculo
        public static DataTable CarregarExtratoCurriculo(int idCurriculo)
        {
            var objDataTable = new DataTable();
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo },
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "BNE.SP_Metricas_CV_Extrato", parms))
            {
                objDataTable.Load(dr);
            }

            return objDataTable;
        }
        #endregion

        #region CarregarQtdAparicaoPesquisa
        public static int CarregarQtdAparicaoPesquisa(int idCurriculo)
        {
            var objDataTable = new DataTable();
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo },
                };

            var ret = DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "BNE.SP_VIP_QTD_APARICAO_PESQUISA", parms);

            if (ret != null && !Convert.IsDBNull(ret))
                return Convert.ToInt32(ret);
            else
                return 0;
        }
        #endregion

        #region CarregarQtdCvNaFrente
        public static int CarregarQtdCvNaFrente(int idCurriculo)
        {
            var objDataTable = new DataTable();
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo },
                };

            var ret = DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "BNE.SP_VIP_QTD_CVS_NA_FRENTE_PESQUISA", parms);

            if (ret != null && !Convert.IsDBNull(ret))
                return Convert.ToInt32(ret);
            else
                return 0;
        }
        #endregion

        #region CarregarQtdEmpresasNovas
        public static int CarregarQtdEmpresasNovas(int idCurriculo)
        {
            var objDataTable = new DataTable();
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo },
                };

            var ret = DataAccessLayer.ExecuteScalar(CommandType.StoredProcedure, "BNE.SP_VIP_QTD_EMPRESAS_NOVAS_CIDADE", parms);

            if (ret != null && !Convert.IsDBNull(ret))
                return Convert.ToInt32(ret);
            else
                return 0;
        }
        #endregion

        #region [AdicionarAtividadeAssincronoSalvarAlertasCurriculo]
        /// <summary>
        /// Integração de Alerta Curriculos - 
        /// </summary>
        /// <param name="idCurriculo"></param>
        public void AdicionarAtividadeAssincronoSalvarAlertasCurriculo(string email, int idCurriculo)
        {
            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    var parametrosAtividade = new ParametroExecucaoCollection
                                    {
                                        {"IdCurriculo","IdCurriculo", this.IdCurriculo.ToString(), this.IdCurriculo.ToString() }
                                    };
                    ProcessoAssincrono.IniciarAtividade(AsyncServices.Enumeradores.TipoAtividade.SalvarAlertasCurriculo, parametrosAtividade);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "Erro para Salvar alerta cv curriculo novo");
                }
            }
        }
        #endregion

        #region [SolicitarAtualização]
        /// <summary>
        /// Envia e-mail e sms sem descontar da empresa, solicitando ao candidato atualizar o curriculo.
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="UFP"></param>
        /// <param name="filial"></param>
        public static void SolicitarAtualizacao(int idCurriculo, int UFP, int filial)
        {

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var carta = CartaEmail.LoadObject((int)Enumeradores.CartaEmail.SolicitarAtualizacaoCV);
                        var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ContaPadraoEnvioEmail);

                        var objCurriculo = LoadObject(idCurriculo, trans);
                        objCurriculo.PessoaFisica.CompleteObject(trans);
                        UsuarioFilialPerfil objUsuarioFilialPerfil = new UsuarioFilialPerfil(UFP);
                        var linkLogarAutomatico = Common.LoginAutomatico.GerarUrl(objCurriculo.PessoaFisica.CPF, objCurriculo.PessoaFisica.DataNascimento, "/cadastro-de-curriculo-gratis");

                        #region [Monta Email]
                        //carta.DescricaoAssunto = carta.DescricaoAssunto.Replace("", "");
                        carta.ValorCartaEmail = carta.ValorCartaEmail.Replace("{Nome_Completo}", objCurriculo.PessoaFisica.NomeCompleto)
                            .Replace("{Nome}", objCurriculo.PessoaFisica.PrimeiroNome)
                            .Replace("{Link_Atualizar}", linkLogarAutomatico)
                            .Replace("{Ramo_Empresa}", Filial.AreaBne(filial));


                        #endregion

                        #region [Salvar na Tabela]

                        SolicitacaoAtualizacaoCV objSo = new SolicitacaoAtualizacaoCV
                        {
                            Curriculo = objCurriculo,
                            UsuarioFilialPerfil = objUsuarioFilialPerfil
                        };
                        objSo.Save(trans);
                        #endregion

                        #region [SMS]
                        Custom.EnvioMensagens.EnvioMensagens obj = new Custom.EnvioMensagens.EnvioMensagens();
                       
                        if (!string.IsNullOrEmpty(objCurriculo.PessoaFisica.NumeroDDDCelular) &&
                            !string.IsNullOrEmpty(objCurriculo.PessoaFisica.NumeroCelular))
                        {
                            var templateSMS = new CampanhaTanque().GetTextoCampanha(Mensagem.Enumeradores.CampanhaTanque.SolicitacaoAtualizarCurriculo);
                            templateSMS.mensagem = templateSMS.mensagem.Replace("{Nome}", objCurriculo.PessoaFisica.PrimeiroNome);

                            MensagemCS.EnviaSMSTanque(new Curriculo(idCurriculo), objCurriculo.PessoaFisica, objUsuarioFilialPerfil, objUsuarioFilialPerfil, templateSMS.mensagem,
                                objCurriculo.PessoaFisica.NumeroDDDCelular, objCurriculo.PessoaFisica.NumeroCelular,
                                Enumeradores.UsuarioSistemaTanque.SolicitarAtualizacaoCurriculo, trans, templateSMS.id);
                            
                        }
                        #endregion

                        if (Validacao.ValidarEmail(objCurriculo.PessoaFisica.EmailPessoa))
                        {
                            EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                                .Enviar(carta.DescricaoAssunto, carta.ValorCartaEmail, Enumeradores.CartaEmail.SolicitarAtualizacaoCV, emailRemetente,
                                    objCurriculo.PessoaFisica.EmailPessoa);
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex, $"Erro ao enviar solicitação de atulização ao CV {idCurriculo}");
                        throw;
                    }

                }
            }
        }
        #endregion


        #region [Informações S.A Curriculo]
        /// <summary>
        /// Recuperar os numeros para exibir na tela do S.A candidato
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="DtaInicio"></param>
        /// <param name="DtaFim"></param>
        /// <returns></returns>
        public static DataTable InformacoesSA(int idCurriculo, DateTime? DtaInicio = null, DateTime? DtaFim = null)
        {
                var objDataTable = new DataTable();
                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Value = idCurriculo }
                };

            if(DtaInicio.HasValue)
                    parms.Add(new SqlParameter { ParameterName = "@Dta_Inicio", SqlDbType = SqlDbType.DateTime, Value =  DtaInicio.Value});
            else
                parms.Add(new SqlParameter { ParameterName = "@Dta_Inicio", SqlDbType = SqlDbType.DateTime, Value = DBNull.Value });

            if (DtaFim.HasValue)
                parms.Add(new SqlParameter { ParameterName = "@Dta_Fim", SqlDbType = SqlDbType.DateTime, Value = DtaFim.Value });
            else
                parms.Add(new SqlParameter { ParameterName = "@Dta_Fim", SqlDbType = SqlDbType.DateTime, Value = DBNull.Value });


            using (var dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "[BNE].[SP_Informacoes_Curriculo]", parms))
                {
                    objDataTable.Load(dr);
                }
                return objDataTable;

        }
        #endregion
        #endregion


    }
}

