//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace BNE.BLL.AsyncServices
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
        }
        #endregion

        #region Atributos
        private XmlDocument _descricaoParametrosEntrada;
        private XmlDocument _descricaoParametrosSaida;

        #endregion

        #region Propriedades

        #region DescricaoParametrosEntrada
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public XmlDocument DescricaoParametrosEntrada
        {
            get { return _descricaoParametrosEntrada; }
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
        public XmlDocument DescricaoParametrosSaida
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

        #region Spreiniciaratividades
        private const String Spreiniciaratividades = @"
        update  TA 
        SET     Idf_Status_Atividade = 1
        FROM    TAB_Atividade TA
                join TAB_Plugins_Compatibilidade pc on (TA.Idf_Plugins_Compatibilidade = pc.Idf_Plugins_Compatibilidade)
        where   TA.Idf_Status_Atividade = 2
                AND PC.Idf_Tipo_Atividade = @Idf_Tipo_Atividade
        ";
        #endregion

        #region Sprecuperararquivosexcluir
        private const String Sprecuperararquivosexcluir = @"
        select 
          atv.Des_Caminho_Arquivo_Upload,
          atv.Des_Caminho_Arquivo_Gerado  
        from  
          TAB_Atividade atv
          join TAB_Plugins_Compatibilidade pc on (atv.Idf_Plugins_Compatibilidade = pc.Idf_Plugins_Compatibilidade)
          join TAB_Plugin plu on (pc.Idf_Plugin_Entrada = plu.Idf_Plugin)
          join TAB_Tipo_Atividade ta on (plu.Idf_Tipo_Atividade = ta.Idf_Tipo_Atividade)  
        where 
          atv.Dta_Execucao between DATEADD(Day,-(2+ ta.Num_Dias_Expiracao), GETDATE()) and DATEADD(Day,-ta.Num_Dias_Expiracao, GETDATE())
          and 
          (atv.Des_Caminho_Arquivo_Gerado is not null or atv.Des_Caminho_Arquivo_Upload is not null)
        ";
        #endregion

        #region Sprecuperaremexecucao
        private const String Sprecuperaremexecucao =
        @"
        SELECT  atv.Idf_Atividade, pc.Idf_Tipo_Atividade 
        FROM    TAB_Atividade atv
                join TAB_Plugins_Compatibilidade pc on (atv.Idf_Plugins_Compatibilidade = pc.Idf_Plugins_Compatibilidade)
        WHERE   Idf_Status_Atividade = 2
                AND PC.Idf_Tipo_Atividade = @Idf_Tipo_Atividade
        ";
        #endregion

        #region Sprecuperardadostarefaassincrona
        private const String Sprecuperardadostarefaassincrona =
        @"
        SELECT  ATV.Idf_Atividade, 
                ATV.Des_Caminho_Arquivo_Upload, 
                ATV.Des_Parametros_Entrada, 
                ATV.Des_Parametros_Saida, 
                ATV.Dta_Cadastro, 
                SA.Des_Status_Atividade, 
                PE.Des_Plugin_Metadata AS Des_Plugin_Metadata_Entrada, 
                PS.Des_Plugin_Metadata AS Des_Plugin_Metadata_Saida
        FROM    TAB_Atividade ATV
                join TAB_Plugins_Compatibilidade PC on ATV.Idf_Plugins_Compatibilidade = pc.Idf_Plugins_Compatibilidade
                join TAB_Status_Atividade SA ON ATV.Idf_Status_Atividade = SA.Idf_Status_Atividade
                join TAB_Plugin PE ON PC.Idf_Plugin_Entrada = PE.Idf_Plugin
                join TAB_Plugin PS ON PC.Idf_Plugin_Saida = PS.Idf_Plugin
        WHERE   Idf_Atividade = @Idf_Atividade
        ";
        #endregion
        
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
                    objAtividade._pluginsCompatibilidade = new PluginsCompatibilidade(Convert.ToInt32(dr["Idf_Plugins_Compatibilidade"]));
                    objAtividade._statusAtividade = new StatusAtividade(Convert.ToInt32(dr["Idf_Status_Atividade"]));
                    if (dr["Des_Parametros_Entrada"] != DBNull.Value)
                    {
                        var xmlParametrosEntrada = new XmlDocument();
                        xmlParametrosEntrada.LoadXml(dr["Des_Parametros_Entrada"].ToString());
                        objAtividade._descricaoParametrosEntrada = xmlParametrosEntrada;
                    }
                    objAtividade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Des_Caminho_Arquivo_Upload"] != DBNull.Value)
                        objAtividade._descricaoCaminhoArquivoUpload = Convert.ToString(dr["Des_Caminho_Arquivo_Upload"]);
                    if (dr["Des_Erro"] != DBNull.Value)
                        objAtividade._descricaoErro = Convert.ToString(dr["Des_Erro"]);
                    objAtividade._dataAgendamento = Convert.ToDateTime(dr["Dta_Agendamento"]);
                    if (dr["Dta_Execucao"] != DBNull.Value)
                        objAtividade._dataExecucao = Convert.ToDateTime(dr["Dta_Execucao"]);
                    objAtividade._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

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
                new SqlParameter("@Idf_Plugins_Compatibilidade", SqlDbType.Int, 4),
                new SqlParameter("@Idf_Status_Atividade", SqlDbType.Int, 4),
                new SqlParameter("@Des_Parametros_Entrada", SqlDbType.Text),
                new SqlParameter("@Des_Parametros_Saida", SqlDbType.Text),
                new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8),
                new SqlParameter("@Des_Caminho_Arquivo_Upload", SqlDbType.VarChar, 255),
                new SqlParameter("@Des_Caminho_Arquivo_Gerado", SqlDbType.VarChar, 255),
                new SqlParameter("@Des_Erro", SqlDbType.Text, 16),
                new SqlParameter("@Dta_Agendamento", SqlDbType.DateTime, 8),
                new SqlParameter("@Dta_Execucao", SqlDbType.DateTime, 8),
                new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1)
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
                parms[3].Value = this._descricaoParametrosEntrada.InnerXml;
            else
                parms[3].Value = DBNull.Value;

            if (this._descricaoParametrosSaida != null)
                parms[4].Value = this._descricaoParametrosSaida.InnerXml;
            else
                parms[4].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._descricaoCaminhoArquivoUpload))
                parms[6].Value = this._descricaoCaminhoArquivoUpload;
            else
                parms[6].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._descricaoCaminhoArquivoGerado))
                parms[7].Value = this._descricaoCaminhoArquivoGerado;
            else
                parms[7].Value = DBNull.Value;

            if (this._descricaoErro != null)
                parms[8].Value = this._descricaoErro;
            else
                parms[8].Value = DBNull.Value;

            parms[9].Value = this._dataAgendamento;

            if (this._dataExecucao.HasValue)
                parms[10].Value = this._dataExecucao;
            else
                parms[10].Value = DBNull.Value;

            parms[11].Value = this._flagInativo;

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
                new SqlParameter {ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Value = idAtividade},
                new SqlParameter {ParameterName = "@Idf_Status_Atividade", SqlDbType = SqlDbType.Int, Value = (int) tipoAtividade}
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
                new SqlParameter {ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Value = idAtividade},
                new SqlParameter {ParameterName = "@Des_Erro", SqlDbType = SqlDbType.Text, Value = mensagemErro}
            };

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spdefinirstatuserro, parms);
        }
        #endregion

        #region ReiniciarAtividades
        /// <summary>
        /// Muda o status das atividades com o status 2 para status 1
        /// </summary>
        public static void ReiniciarAtividades(Enumeradores.TipoAtividade tipoAtividade)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Tipo_Atividade", SqlDbType = SqlDbType.Int, Value = (int)tipoAtividade}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.Text, Spreiniciaratividades, parms);
        }
        #endregion

        #region RecuperarArquivosAExcluir
        /// <summary>
        /// Recupera os arquivos que podem ser excluídos
        /// </summary>        
        /// <returns>Uma DataTable contendo os arquivos a serem excluídos</returns>
        public static DataTable RecuperarArquivosAExcluir()
        {
            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, Sprecuperararquivosexcluir, null).Tables[0];
        }
        #endregion

        #region RecuperarAtividadesParadas
        /// <summary>
        /// Recupera todas as atividades que estão paradas na fila com o staus "Em Execução"
        /// </summary>
        /// <returns>A coleção com as atividades</returns>
        public static Collection<AtividadeResumida> RecuperarAtividadesParadas(Enumeradores.TipoAtividade tipoAtividade)
        {
            var resultado = new Collection<AtividadeResumida>();

            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Tipo_Atividade", SqlDbType = SqlDbType.Int, Value = (int)tipoAtividade}
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperaremexecucao, parms))
            {
                while (dr.Read())
                {
                    var objAtividade = new AtividadeResumida
                    {
                        IdAtividade = Convert.ToInt32(dr["Idf_Atividade"]),
                        TipoAtividade = (Enumeradores.TipoAtividade)Convert.ToInt32(dr["Idf_Tipo_Atividade"])
                    };
                    resultado.Add(objAtividade);
                }
            }
            return resultado;
        }
        #endregion

        #region RecuperarDadosTarefaAssincrona
        public IDataReader RecuperarDadosTarefaAssincrona()
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Value = _idAtividade}
            };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperardadostarefaassincrona, parms);
        }
        #endregion

        #endregion

    }
}