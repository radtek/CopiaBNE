//-- Data: 21/06/2010 14:15
//-- Autor: Eduardo Ruthes

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;

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

        #endregion

        #region Metodos

        #region CarregarPorCpf
        public static bool CarregarPorCpf(decimal numCpf, out Curriculo objCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Num_Cpf", SqlDbType = SqlDbType.Decimal, Value = numCpf }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Selporcpf, parms))
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

                string url = SitemapHelper.MontarUrlCurriculo(curriculoDadosSitemap.DescricaoFuncao, curriculoDadosSitemap.NomeCidade, curriculoDadosSitemap.SiglaEstado, (int)curriculoDadosSitemap.IdfCurriculo);
                sb.AppendFormat("<a href=\"{0}\">{1}</a><br>", url, curriculoDadosSitemap.NomePessoa);
            }

            return sb.ToString();
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
            if (!String.IsNullOrEmpty(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.EmailPessoa)) //Só envia mensagem caso o usuário possua e-mail
            {
                string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, trans);

                #region Confirmação de Pagamento
                string assunto;
                string template = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.ConfirmacaoPagamentoVIP, out assunto);
                var parametros = new
                {
                    NomeCandidato = objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NomeCompleto
                };
                string mensagem = parametros.ToString(template);

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(null, null, objPlanoAdquirido.UsuarioFilialPerfil, assunto, mensagem, emailRemetente, objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.EmailPessoa, trans);
                #endregion

                #region CartilhaVIP
                string assuntoCartilhaVIP;
                string mensagemCartilhaVIP = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.CartilhaVIP, out assuntoCartilhaVIP);

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(null, null, objPlanoAdquirido.UsuarioFilialPerfil, assuntoCartilhaVIP, mensagemCartilhaVIP, emailRemetente, objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.EmailPessoa, trans);
                #endregion
            }

            //Envia sms
            if (!string.IsNullOrEmpty(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular) && !string.IsNullOrEmpty(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroCelular))
            {
                MensagemCS.SalvarSMS(null, null, objPlanoAdquirido.UsuarioFilialPerfil, CartaSMS.RecuperaValorConteudo(Enumeradores.CartaSMS.BoasVindasVIP), objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular, objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroCelular, trans);
            }

            return true;
        }

        #endregion

        /// <summary>
        /// Salvar Pretenção salarial
        /// </summary>
        /// <param name="objCurriculo"></param>
        #region SalvarCurriculoPretencaoSalarial
        public void SalvarCurriculoPretencaoSalarial(Curriculo objCurriculo)
        {
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        objCurriculo.Save();

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
