//-- Data: 28/03/2016 15:45
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using BNE.BLL.Custom;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CurriculoNaoVisivelFilial // Tabela: BNE_Curriculo_Nao_Visivel_Filial
    {
        #region Consultas

        #region spListaMotivoBloqueio
        private const string spListaMotivoBloqueio = @" 
        SELECT idf_motivo_bloqueio, des_motivo_bloqueio
        FROM bne_motivo_bloqueio WITH(NOLOCK)
        order by idf_motivo_bloqueio desc";
        #endregion

        #region spListaFilialBloqueadaUsuario
        private const string spListaFilialBloqueadaUsuario = @"
                    SELECT 
                    F.Idf_Filial ,
                    Raz_Social AS Nme_Empresa ,
					des_motivo_curriculo_nao_visivel_filial,
					des_motivo_bloqueio,
					cvf.dta_cadastro as dta_bloqueio
                  FROM    TAB_Filial F WITH(NOLOCK)
                LEFT JOIN plataforma.TAB_CNAE_Sub_Classe CSC WITH(NOLOCK) ON CSC.Idf_CNAE_Sub_Classe = F.Idf_CNAE_Principal
				INNER JOIN bne.bne_curriculo_nao_visivel_filial cvf with(nolock) on cvf.idf_filial = f.idf_filial
				INNER join bne.bne_motivo_bloqueio mb with(nolock) on mb.idf_motivo_bloqueio = cvf.idf_motivo_bloqueio
            WHERE   F.Flg_Inativo = 0
                AND F.Idf_Situacao_Filial = 1 
				AND cvf.idf_Curriculo = @Idf_Curriculo
				order by cvf.dta_cadastro desc";
        #endregion

        #region spListaCandidatura
        private const string spListaCandidatura = @"select vc.idf_vaga_candidato
                             from BNE_Vaga_Candidato vc WITH(NOLOCK)
                            join bne_vaga v WITH(NOLOCK) on v.idf_vaga =vc.idf_vaga
                            where vc.idf_curriculo = @Idf_Curriculo
                            and v.idf_filial = @Idf_Filial
                            and (vc.idf_status_Candidatura <> @idf_Status_Candidatura or vc.idf_status_candidatura is null) ";
        #endregion

        #region spCurriculoVisivelParaEmpresa
        private const string spCurriculoVisivelParaEmpresa = @"
        select  count(idf_filial)
        from bne_curriculo_nao_visivel_filial WITH(NOLOCK)
        where idf_filial =  @Idf_Filial
        AND idf_curriculo =  @Idf_Curriculo";
        #endregion

        #region spEmpresaBloqueada
        private const string spEmpresaBloqueada = @"select count(cu.idf_Curriculo) from tab_pessoa_fisica pf with(nolock)
                                    join bne_curriculo cu WITH(NOLOCK) on pf.idf_pessoa_fisica = cu.idf_pessoa_fisica
                                    join bne_curriculo_nao_visivel_filial cvf with(nolock) on cvf.idf_curriculo = cu.idf_curriculo
                                    join bne_vaga v with(nolock) on v.idf_filial = cvf.idf_filial
                                    where pf.num_cpf = @Num_Cpf AND v.idf_vaga = @Idf_Vaga";
        #endregion

        #endregion

        #region Métodos

        #region ListarMotivoBloqueio
        /// <summary>
        ///     Lista motivos do bloqueio
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ListarMotivoBloqueio()
        {
            var lista = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spListaMotivoBloqueio, null))
            {
                while (dr.Read())
                {
                    lista.Add(dr["idf_Motivo_Bloqueio"].ToString(), dr["Des_Motivo_Bloqueio"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }
            return lista;
        }
        #endregion

        #region ListaFilialBloqueadaUsuario
        /// <summary>
        ///     Lista das empresas bloqueadas por um usuario
        /// </summary>
        /// <param name="razaoRamoCidade"></param>
        /// <param name="idCurriculo"></param>
        /// <param name="paginaCorrente"></param>
        /// <param name="tamanhoPagina"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public static DataTable ListaFilialBloqueadaUsuario(int idCurriculo)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms[0].Value = idCurriculo;

            DataTable dt = null;
            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spListaFilialBloqueadaUsuario, parms))
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

        #region InativarCandidaturas
        private static void InativarCandidaturas(int idCurriculo, int idFilial)
        {
            var lista = ListaCandidaturas(idCurriculo, idFilial);
            if (!string.IsNullOrEmpty(lista))
            {
                var spInativarCandidaturas = string.Format("update bne_vaga_candidato set flg_inativo = 1, idf_status_candidatura = {0} where idf_vaga_candidato in ({1})", (int) Enumeradores.StatusCandidatura.VagaEmpresaBloqueada, lista);
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, spInativarCandidaturas, null);
            }
            BufferBloquearEmpresa.bloquear(idCurriculo);
        }
        #endregion

        #region AtivarCandidaturas
        private static void AtivarCandidaturas(int idCurriculo, int idFilial)
        {
            var lista = ListaCandidaturas(idCurriculo, idFilial);
            if (!string.IsNullOrEmpty(lista))
            {
                var spInativarCandidaturas = string.Format("update bne_vaga_candidato set flg_inativo = 0, idf_status_candidatura = {0} where idf_vaga_candidato in ({1}) ", (int) Enumeradores.StatusCandidatura.Candidatado, lista);
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, spInativarCandidaturas, null);
            }
            var urlSolr = string.Empty;
            //empresa desbloqueada, cv dev voltar as pesquisas dela
            try
            {
                urlSolr = string.Format(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLSolrExcluirCVBloqueio), idCurriculo, idFilial);
                var request = WebRequest.Create(urlSolr);
                var response = (HttpWebResponse) request.GetResponse();
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, "Erro ao excluir o bloqueio da empresa no solr: " + urlSolr);
            }
        }
        #endregion

        #region ListaCandidatuas
        /// <summary>
        ///     Listas as candidaturas feitas para a empresa bloqueada para ativar ou inativar
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="idFilial"></param>
        /// <returns></returns>
        private static string ListaCandidaturas(int idCurriculo, int idFilial)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Status_Candidatura", SqlDbType.Int, 4));
            parms[0].Value = idCurriculo;
            parms[1].Value = idFilial;
            parms[2].Value = (int) Enumeradores.StatusCandidatura.ExcluidoProcessoSelecao;

            var ids = string.Empty;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spListaCandidatura, parms))
            {
                while (dr.Read())
                {
                    ids += string.Format(",{0}", dr["Idf_Vaga_Candidato"]);
                }
                if (!string.IsNullOrEmpty(ids))
                    ids = ids.Remove(0, 1);
            }
            return ids;
        }
        #endregion

        #region CurriculoVisivelParaEmpresa
        public static bool CurriculoVisivelParaEmpresa(int idCurriculo, int idFilial)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms[0].Value = idCurriculo;
            parms[1].Value = idFilial;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, spCurriculoVisivelParaEmpresa, parms)) > 0;
        }
        #endregion

        #region EmpresaBloqueada
        /// <summary>
        ///     Verifica se a vaga passada é de empresa bloqueada pelo usuario
        /// </summary>
        /// <param name="idVaga"></param>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool EmpresaBloqueada(int idVaga, decimal cpf)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_Cpf", SqlDbType.Decimal, 11));
            parms[0].Value = idVaga;
            parms[1].Value = cpf;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, spEmpresaBloqueada, parms)) > 0;
        }
        #endregion

        #endregion
    }
}