//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using BNE.Services.AsyncServices.BLL;

namespace BNE.Mensagem.AysncServices.BLL
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
            public Mensagem.AysncServices.BLL.Enumeradores.TipoAtividade TipoAtividade { get; set; }
            /// <summary>
            /// O sistema que está chamando
            /// </summary>
            public Mensagem.Model.Sistema Sistema { get; set; }
            /// <summary>
            /// O template a ser usado
            /// </summary>
            public Mensagem.Model.Template Template { get; set; }
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

        #region Spexcluirtodasfinalizadas
        private const string Spexcluirtodasfinalizadas = "delete from TAB_Atividade where Idf_Status_Atividade not in (1,2)";
        #endregion

        #region Spexcluir
        private const string Spexcluir = "delete from TAB_Atividade where Idf_Atividade = @Idf_Atividade";
        #endregion

        #region Sprecuperaatividadesgrid
        private const string Sprecuperaatividadesgrid = @"
        SELECT * FROM (
        SELECT ROW_NUMBER() over (order by {1} {2}) row,
	        t.*    
        FROM plataforma.TAB_Plugin pluRelatorio
	        CROSS APPLY(
	            SELECT {3}
	              atv.Idf_Atividade,
	              pluEntrada.Des_Plugin Nme_Processo,
                  pluEntrada.Cod_Plugin Cod_Plugin_Entrada, 
	              atv.Dta_Agendamento,
	              tpat.Num_Dias_Expiracao,
	              usu.Num_CPF,
	              pf.Nme_Pessoa,
	              atv.Idf_Status_Atividade,
	              atv.Des_Caminho_Arquivo_Upload,
	              atv.Des_Caminho_Arquivo_Gerado,
	              atv.Des_Parametros_Entrada,
	              atv.Des_Parametros_Saida,
                  atv.Dta_Execucao,
	              ts.Idf_Tipo_Saida      
	            FROM 
	              TAB_Atividade atv
	              INNER JOIN plataforma.TAB_Plugins_Compatibilidade pc on (atv.Idf_Plugins_Compatibilidade = pc.Idf_Plugins_Compatibilidade)
	              INNER JOIN plataforma.TAB_Plugin pluEntrada on (pc.Idf_Plugin_Entrada = pluEntrada.Idf_Plugin)
	              INNER JOIN plataforma.TAB_Plugin pluSaida on (pc.Idf_Plugin_Saida = pluSaida.Idf_Plugin)
	              INNER JOIN plataforma.TAB_Tipo_Atividade tpat on (pluEntrada.Idf_Tipo_Atividade = tpat.Idf_Tipo_Atividade)
	              INNER JOIN TAB_Usuario usu on (atv.Idf_Usuario_Gerador = usu.Idf_Usuario)
                  INNER JOIN plataforma.TAB_Status_Atividade satv on (atv.Idf_Status_Atividade = satv.Idf_Status_Atividade)
	               LEFT JOIN TAB_Pessoa_Fisica pf on (usu.Num_CPF = pf.Num_CPF)
                   LEFT JOIN plataforma.TAB_Tipo_Saida ts on (atv.Idf_Tipo_Saida = ts.Idf_Tipo_Saida)
	            WHERE pluEntrada.Idf_Plugin = pluRelatorio.Idf_Plugin
		            AND DATEADD(day, tpat.Num_Dias_Expiracao, CONVERT(DATETIME, CONVERT(VARCHAR,atv.Dta_Agendamento,103),103)) > GETDATE()
                {0}
            ) AS t
        ) as TEMP
	  
        where 
          row between (@indice * @tamanhopag+1) and ( (@indice + 1) * @tamanhopag)";

        private const string SprecuperaatividadesgridCount = @"
        SELECT COUNT (*)	  
	        FROM plataforma.TAB_Plugin pluRelatorio
	        CROSS APPLY(
	        SELECT {1} pluEntrada.*
	        FROM
	          TAB_Atividade atv
	          INNER JOIN plataforma.TAB_Plugins_Compatibilidade pc on (atv.Idf_Plugins_Compatibilidade = pc.Idf_Plugins_Compatibilidade)
	          INNER JOIN plataforma.TAB_Plugin pluEntrada on (pc.Idf_Plugin_Entrada = pluEntrada.Idf_Plugin)
	          INNER JOIN plataforma.TAB_Plugin pluSaida on (pc.Idf_Plugin_Saida = pluSaida.Idf_Plugin)
	          INNER JOIN plataforma.TAB_Tipo_Atividade tpat on (pluEntrada.Idf_Tipo_Atividade = tpat.Idf_Tipo_Atividade)
              INNER JOIN plataforma.TAB_Status_Atividade satv on (atv.Idf_Status_Atividade = satv.Idf_Status_Atividade)
	          INNER JOIN TAB_Usuario usu on (atv.Idf_Usuario_Gerador = usu.Idf_Usuario)
	           LEFT JOIN TAB_Pessoa_Fisica pf on (usu.Num_CPF = pf.Num_CPF)
                LEFT JOIN plataforma.TAB_Tipo_Saida ts on (atv.Idf_Tipo_Saida = ts.Idf_Tipo_Saida)
	        WHERE pluEntrada.Idf_Plugin = pluRelatorio.Idf_Plugin
		        AND  DATEADD(day, tpat.Num_Dias_Expiracao, CONVERT(DATETIME, CONVERT(VARCHAR,atv.Dta_Agendamento,103),103)) > GETDATE()
	            {0}
        ) AS t
        ";
        #endregion

        #region Sprecuperaparametrosentradaatividade
        private const string Sprecuperaparametrosentradaatividade = @"
        select  atv.Des_Parametros_Entrada
        from    TAB_Atividade atv
        where   atv.Idf_Atividade = @Idf_Atividade";
        #endregion

        #region Spreenviarsolicitacao
        private const string Spreenviarsolicitacao = @"
        INSERT INTO TAB_Atividade
        SELECT Idf_Plugins_Compatibilidade
                   ,@Status_Aguardando_Execucao
                   ,@Idf_Usuario
                   ,Des_Parametros_Entrada
                   ,Des_Parametros_Saida
                   ,GETDATE()
                   ,NULL
                   ,NULL
                   ,GETDATE()
                   ,NULL
                   ,Flg_Inativo
                   ,Idf_Tipo_Saida
        FROM    TAB_Atividade
        WHERE   Idf_Atividade = @Idf_Atividade;
        SET @Idf_Atividade_Retorno = SCOPE_IDENTITY();
        ";
        #endregion

        #region Spreiniciaratividades
        private const String Spreiniciaratividades = "update TAB_Atividade set Idf_Status_Atividade = 1 where Idf_Status_Atividade = 2";
        #endregion

        #region Sprecuperaremexecucao
        private const String Sprecuperaremexecucao =
        @"
        SELECT  atv.Idf_Atividade, plu.Idf_Tipo_Atividade, sistema.SistemaId, sistema.Nome, template.TemplateId, template.Nome
        FROM    TAB_Atividade atv
                INNER JOIN TAB_Plugins_Compatibilidade pc on (atv.Idf_Plugins_Compatibilidade = pc.Idf_Plugins_Compatibilidade)
                INNER JOIN TAB_Plugin plu on (pc.Idf_Plugin_Entrada = plu.Idf_Plugin)
                INNER JOIN mensagem.Sistema sistema on atv.SistemaId = sistema.SistemaId
        where Idf_Status_Atividade = 2";
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
                    if (dr["Des_Parametros_Saida"] != DBNull.Value)
                    {
                        var xmlParametrosSaida = new XmlDocument();
                        xmlParametrosSaida.LoadXml(dr["Des_Parametros_Saida"].ToString());
                        objAtividade._descricaoParametrosSaida = xmlParametrosSaida;
                    }
                    objAtividade._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Des_Erro"] != DBNull.Value)
                        objAtividade._descricaoErro = Convert.ToString(dr["Des_Erro"]);
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
                    new SqlParameter("@Des_Erro", SqlDbType.Text, 16),
                    new SqlParameter("@Dta_Execucao", SqlDbType.DateTime, 8),
                    new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1),
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

            if (this._descricaoErro != null)
                parms[6].Value = this._descricaoErro;
            else
                parms[6].Value = DBNull.Value;

            if (this._dataExecucao.HasValue)
                parms[7].Value = this._dataExecucao;
            else
                parms[7].Value = DBNull.Value;

            parms[8].Value = this._flagInativo;

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

        #region RecuperaAtividadesGrid
        /// <summary>
        /// Recupera as atividades na fila
        /// </summary>
        /// <returns>A datatable com as atividades preenchidas</returns>
        public static DataTable RecuperaAtividadesGrid(int pagina, int tamanhoPagina, String condicoes, List<SqlParameter> parametros, String direcaoOrdenacao,
            String colunaOrdenacao, out int qtdRegistros, string qtdAtividadesPorTipo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@indice", SqlDbType = SqlDbType.Int, Value = pagina } ,
                    new SqlParameter { ParameterName = "@tamanhopag", SqlDbType = SqlDbType.Int, Value = tamanhoPagina }
                };

            if (parametros != null && parametros.Count > 0)
                parms.AddRange(parametros);

            if (String.IsNullOrEmpty(colunaOrdenacao))
            {
                colunaOrdenacao = "Dta_Agendamento";
                direcaoOrdenacao = "desc";
            }

            Object res = DataAccessLayer.ExecuteScalar(CommandType.Text, string.Format(SprecuperaatividadesgridCount, condicoes, qtdAtividadesPorTipo), parms);
            if (!int.TryParse(Convert.ToString(res), out qtdRegistros))
                qtdRegistros = 0;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, string.Format(Sprecuperaatividadesgrid, condicoes, colunaOrdenacao, direcaoOrdenacao, qtdAtividadesPorTipo), parms).Tables[0];
        }
        #endregion

        #region RecuperParametrosEntradaAtividade
        /// <summary>
        /// Recupera o parâmetro de entrada da atividade
        /// </summary>
        /// <param name="idAtividade">O identificador da atividade</param>
        /// <returns>Um XmlDocument com os parâmetros</returns>
        public static XmlDocument RecuperParametrosEntradaAtividade(int idAtividade)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Value = idAtividade }
                };

            Object objXml = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperaparametrosentradaatividade, parms);

            if (objXml != null || objXml != DBNull.Value)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Convert.ToString(objXml));
                return doc;
            }
            return null;
        }
        #endregion

        #region ReenviarSolicitacao
        public static Atividade ReenviarSolicitacao(SqlTransaction trans, int idAtividade, int idUsuario)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Value = idAtividade } ,
                    new SqlParameter { ParameterName = "@Idf_Usuario", SqlDbType = SqlDbType.Int, Value = idUsuario } ,
                    new SqlParameter { ParameterName = "@Status_Aguardando_Execucao", SqlDbType = SqlDbType.Int, Value = (int)Enumeradores.StatusAtividade.AguardandoExecucao } ,
                    new SqlParameter { ParameterName = "@Idf_Atividade_Retorno", SqlDbType = SqlDbType.Int, Value = idAtividade, Direction = ParameterDirection.Output }
                };

            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, Spreenviarsolicitacao, parms);
            Atividade objAtividade = LoadObject(Convert.ToInt32(cmd.Parameters["@Idf_Atividade_Retorno"].Value), trans);

            return objAtividade;
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
        public virtual static Collection<AtividadeResumida> RecuperarAtividadesParadas()
        {
            var resultado = new Collection<AtividadeResumida>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperaremexecucao, new List<SqlParameter>()))
            {
                while (dr.Read())
                {
                    var objAtividade = new AtividadeResumida
                        {
                            IdAtividade = Convert.ToInt32(dr["Idf_Atividade"]),
                            TipoAtividade = (Mensagem.AysncServices.BLL.Enumeradores.TipoAtividade)Convert.ToInt32(dr["Idf_Tipo_Atividade"])
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