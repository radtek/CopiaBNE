//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace BNE.Mensagem.AsyncServices.BLL
{
    public partial class Atividade // Tabela: TAB_Atividade
    {

        #region AtividadeResumida
        /// <summary>
        /// Representação resumida da atividade
        /// </summary>
        public class AtividadeResumida
        {
            /// <summary>
            /// Código da atividade
            /// </summary>
            public int IdAtividade { get; set; }
            /// <summary>
            /// O tipo da atividade
            /// </summary>
            public Enumeradores.TipoAtividade TipoAtividade { get; set; }
            /// <summary>
            /// O sistema que está chamando
            /// </summary>
            public Model.Sistema Sistema { get; set; }
            /// <summary>
            /// O template a ser usado
            /// </summary>
            public Model.Template Template { get; set; }
        }
        #endregion

        #region Atributos

        private string _descricaoParametrosEntrada;
        private string _descricaoParametrosSaida;

        #endregion

        #region Propriedades


        #region DescricaoParametrosEntrada
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string DescricaoParametrosEntrada
        {
            get
            {
                return _descricaoParametrosEntrada;
            }
            set
            {
                _descricaoParametrosEntrada = value;
                _modified = true;
            }
        }
        #endregion

        #region DescricaoParametrosSaida
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public string DescricaoParametrosSaida
        {
            get
            {
                return _descricaoParametrosSaida;
            }
            set
            {
                _descricaoParametrosSaida = value;
                _modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas

        #region Spatualizarstatusatividade
        private const string Spatualizarstatusatividade = @"update TAB_Atividade set Idf_Status_Atividade = @Idf_Status_Atividade, Dta_Execucao = GetDate() where Idf_Atividade = @Idf_Atividade";
        #endregion

        #region Spdefinirstatuserro
        private const string Spdefinirstatuserro = @"update TAB_Atividade set Idf_Status_Atividade = 3, Des_Erro = @Des_Erro, Dta_Execucao = GETDATE() where Idf_Atividade = @Idf_Atividade";
        #endregion

        #region Spexcluirtodasfinalizadas
        private const string Spexcluirtodasfinalizadas = "delete from TAB_Atividade where Idf_Status_Atividade not in (1,2)";
        #endregion

        #region Spexcluir
        private const string Spexcluir = "delete from TAB_Atividade where Idf_Atividade = @Idf_Atividade";
        #endregion

        #region Sprecuperaparametrosentradaatividade
        private const string Sprecuperaparametrosentradaatividade = @"
        select  atv.Des_Parametros_Entrada
        from    TAB_Atividade atv
        where   atv.Idf_Atividade = @Idf_Atividade";
        #endregion

        #region Spreiniciaratividades
        private const String Spreiniciaratividades = "update TAB_Atividade set Idf_Status_Atividade = 1 where Idf_Status_Atividade = 2";
        #endregion

        #region Sprecuperaremexecucao
        private const String Sprecuperaremexecucao =
        @"
        SELECT  atv.Idf_Atividade, ATIS.Idf_Tipo_Atividade, sistema.IdSistema, sistema.Nome as NomeSistema, te.IdTemplateEmail, te.Nome as NomeEmail, ts.IdTemplateSMS, ts.Nome as NomeSMS
        FROM    TAB_Atividade atv
                INNER JOIN TAB_Plugins_Compatibilidade pc on (atv.Idf_Plugins_Compatibilidade = pc.Idf_Plugins_Compatibilidade)
                INNER JOIN TAB_Plugin plu on (pc.Idf_Plugin_Entrada = plu.Idf_Plugin)
		        INNER JOIN TAB_Tipo_Atividade_Sistema ATIS ON ATIS.Idf_Tipo_Atividade_Sistema = ATV.Idf_Tipo_Atividade_Sistema
                INNER JOIN mensagem.Sistema sistema on ATIS.IdSistema = sistema.IdSistema
                LEFT JOIN mensagem.TemplateEmail TE on ATIS.IdTemplateEmail = TE.IdTemplateEmail
                LEFT JOIN mensagem.TemplateSMS TS on ATIS.IdTemplateSMS = TS.IdTemplateSMS
        WHERE   Idf_Status_Atividade = 2";
        #endregion

        private const string SPSELECTID = @"
        SELECT *, sistema.IdSistema, sistema.Nome as NomeSistema, te.IdTemplateEmail, te.Nome as NomeEmail, ts.IdTemplateSMS, ts.Nome as NomeSMS 
        FROM    TAB_Atividade ATV 
                INNER JOIN TAB_Tipo_Atividade_Sistema TAS ON ATV.Idf_Tipo_Atividade_Sistema = TAS.Idf_Tipo_Atividade_Sistema
                INNER JOIN mensagem.Sistema sistema on TAS.IdSistema = sistema.IdSistema
                LEFT JOIN mensagem.TemplateEmail TE on TAS.IdTemplateEmail = TE.IdTemplateEmail
                LEFT JOIN mensagem.TemplateSMS TS on TAS.IdTemplateSMS = TS.IdTemplateSMS
        WHERE Idf_Atividade = @Idf_Atividade";

        #endregion

        #region Métodos

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objAtividade">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, Atividade objAtividade)
        {
            try
            {
                if (dr.Read())
                {
                    objAtividade._idAtividade = Convert.ToInt32(dr["Idf_Atividade"]);
                    objAtividade._pluginsCompatibilidade = new PluginsCompatibilidade(Convert.ToInt16(dr["Idf_Plugins_Compatibilidade"]));
                    objAtividade._statusAtividade = new StatusAtividade(Convert.ToByte(dr["Idf_Status_Atividade"]));
                    if (dr["Des_Parametros_Entrada"] != DBNull.Value)
                        objAtividade._descricaoParametrosEntrada = dr["Des_Parametros_Entrada"].ToString();
                    if (dr["Des_Parametros_Saida"] != DBNull.Value)
                        objAtividade._descricaoParametrosSaida = dr["Des_Parametros_Saida"].ToString();
                    objAtividade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Des_Erro"] != DBNull.Value)
                        objAtividade._descricaoErro = Convert.ToString(dr["Des_Erro"]);
                    if (dr["Dta_Execucao"] != DBNull.Value)
                        objAtividade._dataExecucao = Convert.ToDateTime(dr["Dta_Execucao"]);
                    objAtividade._tipoAtividadeSistema = new TipoAtividadeSistema(Convert.ToInt16(dr["Idf_Tipo_Atividade_Sistema"]));

                    objAtividade._persisted = true;
                    objAtividade._modified = false;

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

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private List<SqlParameter> GetParameters()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Atividade", SqlDbType.Int, 4),
                    new SqlParameter("@Idf_Plugins_Compatibilidade", SqlDbType.SmallInt, 2),
                    new SqlParameter("@Idf_Status_Atividade", SqlDbType.TinyInt, 1),
                    new SqlParameter("@Des_Parametros_Entrada", SqlDbType.VarChar, -1),
                    new SqlParameter("@Des_Parametros_Saida", SqlDbType.VarChar, -1),
                    new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8),
                    new SqlParameter("@Des_Erro", SqlDbType.VarChar, -1),
                    new SqlParameter("@Dta_Execucao", SqlDbType.DateTime, 8),
                    new SqlParameter("@Idf_Tipo_Atividade_Sistema", SqlDbType.SmallInt, 2)
                };
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idAtividade;
            parms[1].Value = this._pluginsCompatibilidade.IdPluginsCompatibilidade;
            parms[2].Value = this._statusAtividade.IdStatusAtividade;

            if (this._descricaoParametrosEntrada != null)
                parms[3].Value = this._descricaoParametrosEntrada;
            else
                parms[3].Value = DBNull.Value;

            if (this._descricaoParametrosSaida != null)
                parms[4].Value = this._descricaoParametrosSaida;
            else
                parms[4].Value = DBNull.Value;

            if (this._descricaoErro != null)
                parms[6].Value = this._descricaoErro;
            else
                parms[6].Value = DBNull.Value;

            if (this._dataExecucao.HasValue)
                parms[7].Value = this._dataExecucao;
            else
                parms[7].Value = DBNull.Value;

            parms[8].Value = this.TipoAtividadeSistema.IdTipoAtividadeSistema;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }

            parms[5].Value = this._dataCadastro;
        }
        #endregion

        #region AtualizarStatusAtividade
        /// <summary>
        /// Atualiza o status de uma atividade
        /// </summary>
        /// <param name="idAtividade">A atividade a ser atualizada</param>
        /// <param name="tipoAtividade">O novo status da atividade</param>
        public static void AtualizarStatusAtividade(int idAtividade, Enumeradores.StatusAtividade tipoAtividade)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Value = idAtividade },
                    new SqlParameter { ParameterName = "@Idf_Status_Atividade", SqlDbType = SqlDbType.Int, Value = (int)tipoAtividade }
                };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spatualizarstatusatividade, parms);
        }
        #endregion

        #region DefinirStatusErro
        /// <summary>
        /// Atualiza o status da atividade como Finalizado Com Erro e grava a mensagem de erro
        /// </summary>
        /// <param name="idAtividade">A atividade a ser atualizada</param>
        /// <param name="mensagemErro">A mensagem de erro</param>
        public static void DefinirStatusErro(int idAtividade, String mensagemErro)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Value = idAtividade },
                    new SqlParameter { ParameterName = "@Des_Erro", SqlDbType = SqlDbType.Text, Value = mensagemErro }
                };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spdefinirstatuserro, parms);
        }
        #endregion

        #region ExcluirAtividadesConcluidas
        /// <summary>
        /// Exclui todas as atividades finalizadas
        /// </summary>
        public static void ExcluirAtividadesConcluidas()
        {
            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spexcluirtodasfinalizadas, null);
        }
        #endregion

        #region Excluir
        /// <summary>
        /// Exclui a atividade
        /// </summary>
        /// <param name="idAtividade">O Código da atividade</param>
        public static void Excluir(int idAtividade)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Value = idAtividade }
                };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spexcluir, parms);
        }
        #endregion

        #region RecuperParametrosEntradaAtividade
        /// <summary>
        /// Recupera o parâmetro de entrada da atividade
        /// </summary>
        /// <param name="idAtividade">O identificador da atividade</param>
        /// <returns>Um XmlDocument com os parâmetros</returns>
        public static string RecuperParametrosEntradaAtividade(int idAtividade)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Value = idAtividade }
                };

            var objXml = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperaparametrosentradaatividade, parms);

            return (string)objXml;
        }
        #endregion

        #region ReiniciarAtividades
        /// <summary>
        /// Muda o status das atividades com o status 2 para status 1
        /// </summary>
        public static void ReiniciarAtividades()
        {
            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spreiniciaratividades, null);
        }
        #endregion

        #region RecuperarAtividadesParadas
        /// <summary>
        /// Recupera todas as atividades que estão paradas na fila com o staus "Em Execução"
        /// </summary>
        /// <returns>A coleção com as atividades</returns>
        public static Collection<AtividadeResumida> RecuperarAtividadesParadas()
        {
            var resultado = new Collection<AtividadeResumida>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperaremexecucao, new List<SqlParameter>()))
            {
                while (dr.Read())
                {
                    Model.Template objTemplate;
                    if (dr["IdTemplateEmail"] != DBNull.Value)
                        objTemplate = new Model.TemplateEmail { Id = Convert.ToInt32(dr["IdTemplateEmail"]), Nome = Convert.ToString(dr["NomeEmail"]) };
                    else
                        objTemplate = new Model.TemplateSMS { Id = Convert.ToInt32(dr["IdTemplateSMS"]), Nome = Convert.ToString(dr["NomeSMS"]) };

                    var objAtividade = new AtividadeResumida
                        {
                            IdAtividade = Convert.ToInt32(dr["Idf_Atividade"]),
                            TipoAtividade = (Enumeradores.TipoAtividade)Convert.ToInt32(dr["Idf_Tipo_Atividade"]),
                            Sistema = new Model.Sistema { Id = Convert.ToByte(dr["IdSistema"]), Nome = Convert.ToString(dr["NomeSistema"]) },
                            Template = objTemplate
                        };
                    resultado.Add(objAtividade);
                }
            }
            return resultado;
        }
        #endregion

        #endregion

    }
}