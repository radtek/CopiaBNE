//-- Autor: Lennon Vidal

using Microsoft.IdentityModel.Claims;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.Auth.Helper;
using BNE.Services.Base.ProcessosAssincronos;

namespace BNE.BLL
{
    public partial class TipoGatilho // Tabela: TAB_Tipo_Gatilho
    {
        public class MelhorVagaModel
        {
            public int IdVaga { get; set; }
            public string DescricaoFuncao { get; set; }
            public decimal SalarioDe { get; set; }
            public decimal SalarioPara { get; set; }
            public string NomeCidade { get; set; }
            public string Estado { get; set; }
            public string Atributos { get; set; }
            public string Requisitos { get; set; }
        }

        private const string SelectGatilhoAtivo = @"
            SELECT TOP 1 1
            FROM [BNE].[TAB_Tipo_Gatilho] AS [tg]
            WHERE [tg].[Idf_tipo_Gatilho] = @p0
        ";

        public static bool GatilhoAtivo(int gatilhoId, SqlTransaction trans = null)
        {
            var sqlParams = new List<SqlParameter>
            {
                new SqlParameter("@p0", gatilhoId) { SqlDbType = SqlDbType.Int }
            };
            object res;

            if (trans == null)
                res = DataAccessLayer.ExecuteScalar(CommandType.Text, SelectGatilhoAtivo, sqlParams);
            else
                res = DataAccessLayer.ExecuteScalar(trans, CommandType.Text, SelectGatilhoAtivo, sqlParams);

            int value;
            if (Convert.IsDBNull(res) || res == null || !Int32.TryParse(res.ToString(), out value) || value != 1)
        {
                return false;
            }
            return true;
        }

        public static void DispararGatilhoPesquisaCandidato(System.Web.HttpContext httpContext, PesquisaVaga objPesquisaVaga)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            ProcessGatilhoPesquisaCandidato(new System.Web.HttpContextWrapper(httpContext), objPesquisaVaga);
        }

        public static void DispararGatilhoPesquisaCandidato(System.Web.HttpContextBase httpContext, PesquisaVaga objPesquisaVaga)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            ProcessGatilhoPesquisaCandidato(httpContext, objPesquisaVaga);
        }


        public static IEnumerable<MelhorVagaModel> RetornarMelhoresVagas(int idFuncao, int idCidade, int idCurriculo)
        {
            var sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter("@idf_funcao", idFuncao) { DbType = DbType.Int32 });
            sqlParams.Add(new SqlParameter("@idf_Cidade", idCidade) { DbType = DbType.Int32 });
            sqlParams.Add(new SqlParameter("@idf_Curriculo", idCurriculo) { DbType = DbType.Int32 });

            using (var reader = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "BUSCA_VAGA_CANDIDATO_DESLOGADO", sqlParams))
            {
                while (reader.Read())
                {
                    yield return new MelhorVagaModel
                    {
                        Atributos = (Convert.IsDBNull(reader["Des_Atribuicoes"]) || reader["Des_Atribuicoes"] == null) ? string.Empty : reader["Des_Atribuicoes"].ToString(),
                        Requisitos = (Convert.IsDBNull(reader["Des_Requisito"]) || reader["Des_Requisito"] == null) ? string.Empty : reader["Des_Requisito"].ToString(),
                        Estado = (Convert.IsDBNull(reader["Sig_Estado"]) || reader["Sig_Estado"] == null) ? string.Empty : reader["Sig_Estado"].ToString(),
                        NomeCidade = (Convert.IsDBNull(reader["Nme_Cidade"]) || reader["Nme_Cidade"] == null) ? string.Empty : reader["Nme_Cidade"].ToString(),
                        IdVaga = (Convert.IsDBNull(reader["Idf_Vaga"]) || reader["Idf_Vaga"] == null) ? 0 : Convert.ToInt32(reader["Idf_Vaga"]),
                        DescricaoFuncao = (Convert.IsDBNull(reader["Des_Funcao"]) || reader["Des_Funcao"] == null) ? string.Empty : reader["Des_Funcao"].ToString(),
                        SalarioDe = (Convert.IsDBNull(reader["Vlr_Salario_De"]) || reader["Vlr_Salario_De"] == null) ? 0 : Convert.ToDecimal(reader["Vlr_Salario_De"]),
                        SalarioPara = (Convert.IsDBNull(reader["Vlr_Salario_Para"]) || reader["Vlr_Salario_Para"] == null) ? 0 : Convert.ToDecimal(reader["Vlr_Salario_Para"])
                    };
                }
            }
        }

        private static void ProcessGatilhoPesquisaCandidato(System.Web.HttpContextBase httpContext, PesquisaVaga objPesquisaVaga)
        {
            var user = httpContext.User ?? System.Threading.Thread.CurrentPrincipal;
            if (user == null)
                return;

            var identity = user.Identity ?? (System.Threading.Thread.CurrentPrincipal != null ? System.Threading.Thread.CurrentPrincipal.Identity : null);
            if (identity == null)
                return;

            if (!identity.IsAuthenticated)
                return;

            if (objPesquisaVaga == null)
                return;

            if (objPesquisaVaga.Funcao == null || objPesquisaVaga.Funcao.IdFuncao <= 0)
                return;

            var claimsIdentity = identity as ClaimsIdentity;

            var pfId = GetPessoaFisicaId(claimsIdentity, httpContext, objPesquisaVaga);
            if (!pfId.HasValue)
                return;

            var curId = GetCuriculoId(claimsIdentity, httpContext, objPesquisaVaga);
            if (!curId.HasValue)
                return;

            DispararGatilhoPesquisaCandidato(pfId.Value,
                                            curId.Value,
                                            objPesquisaVaga.IdPesquisaVaga,
                                            objPesquisaVaga.Funcao.IdFuncao,
                                            objPesquisaVaga.Cidade != null ? new int?(objPesquisaVaga.Cidade.IdCidade) : new int?());
        }


        public static void DispararGatilhoPesquisaCandidato(int pfId, int curriculoId, int pesquisaVagaId, int funcaoId, int? cidadeId)
        {
            var parametros = new ParametroExecucaoCollection()
                        {
                            new ParametroExecucao
                            {
                                Parametro = "IdTipoGatilho",
                                DesParametro = "IdTipoGatilho",
                                Valor = ((int)BLL.Enumeradores.TipoGatilho.PesquisaVagaCandidato).ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdCurriculo",
                                DesParametro = "IdCurriculo",
                                Valor = curriculoId.ToString()    
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdPessoaFisica",
                                DesParametro = "IdPessoaFisica",
                                Valor = pfId.ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdPesquisaVaga",
                                DesParametro = "IdPesquisaVaga",
                                Valor = pesquisaVagaId.ToString()
                            },
                            new ParametroExecucao
                            {
                                Parametro = "IdFuncao",
                                DesParametro = "IdFuncao",
                                Valor = funcaoId.ToString()
                            }
                        };

            if (cidadeId.HasValue && cidadeId.Value > 0)
            {
                parametros.Add(new ParametroExecucao
                            {
                                Parametro = "IdCidade",
                                DesParametro = "IdCidade",
                                Valor = cidadeId.Value.ToString()
                            });
            }
            try
            {
                ProcessoAssincrono.IniciarAtividade(
        BNE.BLL.AsyncServices.Enumeradores.TipoAtividade.GatilhosBNE,
        BNE.BLL.AsyncServices.PluginsCompatibilidade.CarregarPorMetadata("GatilhosEmail", "PluginSaidaGatilhos"),
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

        }


        private static int? GetPessoaFisicaId(ClaimsIdentity identity, System.Web.HttpContextBase context, PesquisaVaga objPesquisaVaga)
        {
            if (identity == null)
                return FindPfId(context, objPesquisaVaga);

            var pfId = identity.GetUnchekedPessoaFisicaId();
            if (!pfId.HasValue)
                return FindPfId(context, objPesquisaVaga);

            return pfId.Value;
        }

        private static int? GetCuriculoId(ClaimsIdentity identity, System.Web.HttpContextBase context, PesquisaVaga objPesquisaVaga)
        {
            if (identity == null)
                return FindCurId(identity.GetPessoaFisicaId(), context, objPesquisaVaga);

            var curId = identity.GetUncheckedCurriculoId();
            if (!curId.HasValue)
                return FindCurId(identity.GetPessoaFisicaId(), context, objPesquisaVaga);

            return curId.Value;
        }

        private static int? FindPfId(System.Web.HttpContextBase context, PesquisaVaga objPesquisaVaga)
        {
            if (objPesquisaVaga == null)
                return null;

            if (objPesquisaVaga.UsuarioFilialPerfil == null)
                return null;

            if (objPesquisaVaga.UsuarioFilialPerfil.PessoaFisica == null)
                return null;

            return objPesquisaVaga.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica;
        }

        private static int? FindCurId(int? pfId, System.Web.HttpContextBase context, PesquisaVaga objPesquisaVaga)
        {
            if (objPesquisaVaga == null)
                return null;

            if (objPesquisaVaga.Curriculo == null || objPesquisaVaga.Curriculo.IdCurriculo <= 0)
                return null;

            return objPesquisaVaga.Curriculo.IdCurriculo;
        }


    }
}