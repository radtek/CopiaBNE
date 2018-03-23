//-- Data: 23/07/2013 15:25
//-- Autor: Luan Fernandes

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using BNE.BLL.Custom;
using System.Text;
namespace BNE.BLL
{
    public partial class AlertaCurriculos // Tabela: alerta.Tab_Alerta_Curriculos
    {
        #region Consultas

        #region SPDELETEALERTAS
        private const string SPDELETEALERTAS = @"SP_DELETE_ALERTAS";
        #endregion

        #region SPBUSCACVSALERTA
        private const string SPBUSCACVSALERTA = @" SELECT AC.Idf_Curriculo, AC.Nme_Pessoa, Ac.Eml_Pessoa, Ac.Flg_VIP 
               FROM alerta.Tab_Alerta_Curriculos AC WITH(NOLOCK)
         INNER JOIN alerta.Tab_Alerta_Cidades ACD WITH(NOLOCK) ON ACD.Idf_Curriculo = AC.Idf_Curriculo
         INNER JOIN alerta.Tab_Alerta_Funcoes AF WITH(NOLOCK)  ON AF.Idf_Curriculo = AC.Idf_Curriculo 
              WHERE ACD.Idf_Cidade = @Idf_Cidade
                AND AF.Idf_Funcao  = @Idf_Funcao
		   GROUP BY AC.Idf_Curriculo, AC.Nme_Pessoa, Ac.Eml_Pessoa, Ac.Flg_VIP";

        private const string SPBUSCATODOSCVSALERTA = @" SELECT AC.Idf_Curriculo, AC.Nme_Pessoa, Ac.Eml_Pessoa, Ac.Flg_VIP 
               FROM alerta.Tab_Alerta_Curriculos AC WITH(NOLOCK)
		   GROUP BY AC.Idf_Curriculo, AC.Nme_Pessoa, Ac.Eml_Pessoa, Ac.Flg_VIP";
        #endregion

        #region SPAJUSTARALERTACVS
        /// <summary>
        /// Ajusta os alertas para os CVS que foram atualizados ou excluidos...
        /// </summary>
        private const string SPAJUSTARALERTACVS = @"SP_AJUSTAR_ALERTACVS";

        #endregion

        #endregion

        #region Métodos

        #region OnSalvarCurriculo
        /// <summary>
        /// Adiciona ou remove cidades e funcoes a um curriculo
        /// </summary>
        /// <param name="objCurriculo">o Curriculo que está selecionado</param>
        /// <remarks>Luan Fernandes</remarks>
        public static void OnAlterarCurriculo(Curriculo objCurriculo)
        {
            if (objCurriculo != null && objCurriculo.IdCurriculo > 0)
            {
                #region CarregaAlertaCurriculo

                AlertaCurriculos objAlerta;
                try
                {
                    objAlerta = AlertaCurriculos.LoadObject(objCurriculo.IdCurriculo);
                }
                catch (RecordNotFoundException)
                {
                    objAlerta = null;
                }

                #endregion

                #region ManipulacaoRegistro

                //Se o Curriculo está Inativo
                //Ou a Situação do Currículo é diferente de:
                // 1  Publicado
                // 2  Aguardando Publicação
                // 3  Crítica
                // 4  Aguardando Revisão VIP
                // 9  Revisado VIP
                // 10 Auditado

                //E se Existe Alerta, Deleta o Alerta e Registros filhos

                if (objCurriculo.FlagInativo ||
                    (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 1
                    && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 2
                    && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 3
                    && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 4
                    && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 9
                    && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != 10)
                    )
                {
                    if (objAlerta != null)
                        Delete(objCurriculo.IdCurriculo);
                }
                else
                {
                    if (objCurriculo.PessoaFisica.CompleteObject())
                    {

                        #region AlertaCurriculo

                        //Se o AlertaCurriculo não existe, cria um novo
                        //Senão atualiza o Email, Nome e VIP

                        if (objAlerta == null)
                        {
                            objAlerta = new AlertaCurriculos();
                            objAlerta.EmailPessoa = (objCurriculo.PessoaFisica.EmailPessoa == null) ? "" : objCurriculo.PessoaFisica.EmailPessoa;
                            objAlerta.IdCurriculo = objCurriculo.IdCurriculo;
                            objAlerta.NomePessoa = (objCurriculo.PessoaFisica.NomePessoa == null) ? "" : objCurriculo.PessoaFisica.NomePessoa;
                            objAlerta.FlagVIP = objCurriculo.FlagVIP;
                            objAlerta.Save();
                        }
                        else
                        {
                            objAlerta.EmailPessoa = (objCurriculo.PessoaFisica.EmailPessoa == null) ? "" : objCurriculo.PessoaFisica.EmailPessoa;
                            objAlerta.NomePessoa = (objCurriculo.PessoaFisica.NomePessoa == null) ? "" : objCurriculo.PessoaFisica.NomePessoa;
                            objAlerta.FlagVIP = objCurriculo.FlagVIP;
                            objAlerta.Save();
                        }

                        #endregion

                        #region AdicionaCidades

                        #region Listas

                        //Lista de Cidades em Alerta
                        List<AlertaCidades> listAlertaCidades = new List<AlertaCidades>();

                        //Lista de Cidades do Candidato
                        List<Cidade> listaCidadesDefault = new List<Cidade>();

                        #endregion

                        #region AdicionaCidadesDefaultCurriculo

                        objCurriculo.PessoaFisica.Cidade.CompleteObject();

                        //Cidade Default do Curriculo, adiciona na lista de Cidades Default do Curriculo
                        if (objCurriculo.PessoaFisica.Cidade != null)
                            listaCidadesDefault.Add(objCurriculo.PessoaFisica.Cidade);

                        //Se a CidadeEndereco for diferente da Cidade, adiciona na lista de Cidades Default do Curriculo
                        if (objCurriculo.CidadeEndereco != null)
                        {
                            objCurriculo.CidadeEndereco.CompleteObject();
                            if (objCurriculo.CidadeEndereco.IdCidade != objCurriculo.PessoaFisica.Cidade.IdCidade)
                                listaCidadesDefault.Add(objCurriculo.CidadeEndereco);
                        }

                        //Se CidadePretendida for diferente da CidadeEndereço e Cidade, adiciona na lista de Cidades Default do Curriculo
                        if (objCurriculo.CidadePretendida != null)
                        {
                            objCurriculo.CidadePretendida.CompleteObject();

                            if (objCurriculo.CidadePretendida.IdCidade != objCurriculo.CidadeEndereco.IdCidade
                                && objCurriculo.CidadePretendida.IdCidade != objCurriculo.PessoaFisica.Cidade.IdCidade)
                                listaCidadesDefault.Add(objCurriculo.CidadePretendida);
                        }

                        //Verifica as cidades que o Candidato tem disponibilidade para trabalhar
                        DataTable dt = CurriculoDisponibilidadeCidade.ListarCidadesPorCurriculo(objCurriculo.IdCurriculo);

                        //Adiciona na lista de Cidades Default do Curriculo
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                Cidade city;
                                if (Cidade.CarregarPorNome(row["Nme_Cidade"].ToString(), out city))
                                    if (city != null)
                                        if (!listaCidadesDefault.Contains(city))
                                            listaCidadesDefault.Add(city);
                            }
                        }

                        #endregion

                        #region AdicionaAlertaCidade
                        //Para cada Cidade Default, veirifica se existe Alerta
                        //Não Existindo, Cria o alerta

                        AlertaCidades alertaCidade;

                        foreach (Cidade cidade in listaCidadesDefault)
                        {

                            try
                            {
                                alertaCidade = AlertaCidades.LoadObject(objCurriculo.IdCurriculo, cidade.IdCidade);
                            }
                            catch (RecordNotFoundException)
                            {
                                alertaCidade = null;
                            }

                            if (alertaCidade == null)
                            {
                                alertaCidade = new AlertaCidades();
                                alertaCidade.IdCidade = cidade.IdCidade;
                                alertaCidade.AlertaCurriculos = objAlerta;
                                alertaCidade.FlagInativo = false;
                                alertaCidade.NomeCidade = cidade.NomeCidade;
                                cidade.Estado.CompleteObject();
                                alertaCidade.SiglaEstado = cidade.Estado.SiglaEstado;

                                if (!listAlertaCidades.Contains(alertaCidade))
                                    listAlertaCidades.Add(alertaCidade);
                            }
                        }

                        if (listAlertaCidades.Count > 0)
                        {
                            AlertaCidades.SalvarListaAlertaCidadesCurriculo(listAlertaCidades);
                        }
                        #endregion

                        #endregion

                        #region AdicionaFuncao_e_Similares

                        #region Listas

                        //Lista para adicionar as Funções em Alerta
                        List<AlertaFuncoes> listAlertaFuncoes = new List<AlertaFuncoes>();

                        //Lista com as FunçõesPretendidas pelo Candidato
                        List<FuncaoPretendida> listFuncoesPretendidas = FuncaoPretendida.CarregarFuncoesPretendidasPorCurriculo(objCurriculo);

                        //Lista que traz as Funções Similares de cada FunçãoPretendida do candidato
                        List<Funcao> listFuncoesSimilares = new List<Funcao>();

                        #endregion

                        #region AdicionaFuncoesDefaultCurriculo

                        AlertaFuncoes funcao;

                        //Para cada FuncaoPretendida do candidato, verifica se já existe alerta
                        //para a funcao, senão exisite, cria o alerta
                        //Verifica as funcoes similares e senão existir alerta, cria
                        foreach (FuncaoPretendida funcaoPretendida in listFuncoesPretendidas)
                        {
                            #region CarregaAlertaFuncao

                            try
                            {
                                if (funcaoPretendida.Funcao != null)
                                    funcao = AlertaFuncoes.LoadObject(funcaoPretendida.Funcao.IdFuncao, objCurriculo.IdCurriculo);
                                else
                                    funcao = null;

                            }
                            catch (RecordNotFoundException)
                            {
                                funcao = null;
                            }

                            #endregion

                            #region Se NãoExisteAlerta
                            //Não Existe Alerta para a Funcao Selecionada
                            if (funcao == null)
                            {
                                if (funcaoPretendida.Funcao != null)
                                {
                                    funcaoPretendida.Funcao.CompleteObject();

                                    funcao = new AlertaFuncoes();
                                    funcao.AlertaCurriculos = objAlerta;
                                    funcao.DescricaoFuncao = (funcaoPretendida.Funcao.DescricaoFuncao == null) ? "" : funcaoPretendida.Funcao.DescricaoFuncao;
                                    funcao.FlagInativo = false;
                                    funcao.FlagSimilar = false;
                                    funcao.IdFuncao = funcaoPretendida.Funcao.IdFuncao;

                                    if (!listAlertaFuncoes.Contains(funcao))
                                        listAlertaFuncoes.Add(funcao);
                                }
                            }

                            #endregion

                            #region VerificaSimilares

                            if (funcaoPretendida.Funcao != null)
                            {
                                listFuncoesSimilares = FuncaoPretendida.CarregarFuncoesSimilaresPorFuncao(funcaoPretendida.Funcao.IdFuncao);

                                if (listFuncoesSimilares.Count > 0)
                                {
                                    foreach (Funcao funcaoSimilar in listFuncoesSimilares)
                                    {
                                        try
                                        {
                                            funcao = AlertaFuncoes.LoadObject(funcaoSimilar.IdFuncao, objCurriculo.IdCurriculo);
                                        }
                                        catch (RecordNotFoundException)
                                        {
                                            funcao = null;
                                        }

                                        if (funcao == null)
                                        {
                                            funcaoSimilar.CompleteObject();
                                            funcao = new AlertaFuncoes();
                                            funcao.AlertaCurriculos = objAlerta;
                                            funcao.DescricaoFuncao = (funcaoSimilar.DescricaoFuncao == null) ? "" : funcaoSimilar.DescricaoFuncao;
                                            funcao.FlagInativo = false;
                                            funcao.FlagSimilar = true;
                                            funcao.IdFuncao = funcaoSimilar.IdFuncao;

                                            if (!listAlertaFuncoes.Contains(funcao))
                                                listAlertaFuncoes.Add(funcao);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        //Se adicionou alguma alerta para funcao
                        if (listAlertaFuncoes.Count > 0)
                        {
                            AlertaFuncoes.SalvarListaAlertaFuncoesCurriculo(listAlertaFuncoes);
                        }
                        #endregion

                        #endregion
                    }
                }

                #endregion
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Método utilizado para excluir uma instância de AlertaCurriculos no banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <remarks>Luan Fernandes</remarks>
        public static void Delete(int idCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.StoredProcedure, SPDELETEALERTAS, parms);
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

        #region ListarCv'sAlertas

        public static List<AlertaCurriculos> ListarCvsAlertas(int IdCidade, int IdFuncao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();

            parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 8));
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 8));

            parms[0].Value = IdCidade;
            parms[1].Value = IdFuncao;

            List<AlertaCurriculos> lista = new List<AlertaCurriculos>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPBUSCACVSALERTA, parms, DataAccessLayer.CONN_NOTIFICACAO))
            {
                while (dr.Read())
                {
                    lista.Add(new AlertaCurriculos
                    {
                        IdCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]),
                        NomePessoa = dr["Nme_Pessoa"].ToString(),
                        EmailPessoa = dr["Eml_Pessoa"].ToString(),
                        FlagVIP = Convert.ToBoolean(dr["Flg_VIP"])
                    });
                }
            }
            return lista;
        }

        public static List<AlertaCurriculos> ListarCvsAlertas(SqlTransaction trans)
        {

            List<AlertaCurriculos> lista = new List<AlertaCurriculos>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPBUSCATODOSCVSALERTA, null))
            {
                while (dr.Read())
                {
                    lista.Add(new AlertaCurriculos
                    {
                        IdCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]),
                        NomePessoa = dr["Nme_Pessoa"].ToString(),
                        EmailPessoa = dr["Eml_Pessoa"].ToString(),
                        FlagVIP = Convert.ToBoolean(dr["Flg_VIP"])
                    });
                }
            }
            return lista;
        }

        #endregion

        #region AjustarAlertasCVs
        /// <summary>
        /// Ajusta os alertas para novos Cvs, Vips adquiridos ou CVs bloqueados, invativados
        /// </summary>
        public static void AjustarAlertasCVs()
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_NOTIFICACAO))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.StoredProcedure, SPAJUSTARALERTACVS, null);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                        trans.Rollback();
                    }
                    trans.Dispose();
                }
                conn.Close();
                conn.Dispose();
            }
        }
        #endregion

        #endregion
    }
}