//------------------------------------------------------------------------------
// <copyright file="CSSqlStoredProcedure.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Messaging;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    private const string CaminhoPadraoQueue = "FormatName:Direct=OS:EMPVW0114203\\private$\\";

    #region EnvioSMSQueue
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void EnvioSMSQueue(SqlInt32 idMensagem, SqlString numeroDDD, SqlString numeroCelular, SqlString descricaoMensagem)
    {
        const string queueName = "bne_enviosms";

        var parametros = new ParametroExecucaoCollection
                            {
                                {"idMensagem", "Mensagem", idMensagem.ToString(), idMensagem.ToString()},
                                {"numeroDDD", "DDD", numeroDDD.ToString().Trim(), numeroDDD.ToString()},
                                {"numeroCelular", "Telefone", numeroCelular.ToString().Trim(), numeroCelular.ToString()},
                                {"mensagem", "Mensagem", descricaoMensagem.ToString(), descricaoMensagem.ToString()}
                            };

        using (var conn = new SqlConnection("context connection=true"))
        {
            conn.Open();

            using (var cmd = new SqlCommand(@"INSERT INTO bne.TAB_Atividade (Idf_Plugins_Compatibilidade, Idf_Status_Atividade, Des_Parametros_Entrada, Dta_Cadastro, Dta_Agendamento, Flg_Inativo) 
                                                                VALUES (@Idf_Plugins_Compatibilidade, @Idf_Status_Atividade, @Des_Parametros_Entrada, @Dta_Cadastro, @Dta_Agendamento, 0);SET @Idf_Atividade = SCOPE_IDENTITY();", conn))
            {
                var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Size = 4, Direction = ParameterDirection.Output},
                    new SqlParameter{ ParameterName = "@Idf_Plugins_Compatibilidade", SqlDbType = SqlDbType.Int, Size = 4, Value = 5},
                    new SqlParameter{ ParameterName = "@Idf_Status_Atividade", SqlDbType = SqlDbType.Int, Size = 4, Value = 1},
                    new SqlParameter{ ParameterName = "@Des_Parametros_Entrada", SqlDbType = SqlDbType.Text, Value = parametros.ToXML()},
                    new SqlParameter{ ParameterName = "@Dta_Cadastro", SqlDbType = SqlDbType.DateTime, Size = 8, Value = DateTime.Now},
                    new SqlParameter{ ParameterName = "@Dta_Agendamento", SqlDbType = SqlDbType.DateTime, Size = 8, Value = DateTime.Now}
                };

                foreach (SqlParameter sqlParameter in (IEnumerable<SqlParameter>)parms)
                {
                    if (cmd.Parameters.Contains(sqlParameter))
                        cmd.Parameters[sqlParameter.ParameterName] = sqlParameter;
                    else
                        cmd.Parameters.Add(sqlParameter);
                }

                cmd.ExecuteNonQuery();
                var atividade = Convert.ToInt32(cmd.Parameters["@Idf_Atividade"].Value);
                cmd.Parameters.Clear();

                if (atividade > 0)
                {
                    var xmlAtividade = new MensagemAtividade
                    {
                        IdfAtividade = atividade
                    };

                    var mq = new MessageQueue(CaminhoPadraoQueue + queueName);
                    var m = new Message(xmlAtividade);
                    mq.Send(m);
                }
            }
        }
    }
    #endregion

    #region EnvioEmailQueue
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void EnvioEmailQueue(SqlInt32 idMensagem, SqlString de, SqlString para, SqlString assunto, [SqlFacet(MaxSize = -1)] SqlString mensagem)
    {
        const string queueName = "bne_envioemail";

        var parametros = new ParametroExecucaoCollection
                {
                    {"idMensagem", "Mensagem", idMensagem.ToString(), idMensagem.ToString()},
                    {"emailRemetente", "Remetente", de.ToString().Trim(), de.ToString()},
                    {"emailDestinatario", "Destinatário", para.ToString().Trim(), para.ToString()},
                    {"assunto", "Assunto", assunto.ToString(), "Assunto" },
                    {"mensagem", "Mensagem", mensagem.ToString(), "Mensagem" }
                };

        using (var conn = new SqlConnection("context connection=true"))
        {
            conn.Open();

            using (var cmd = new SqlCommand(@"INSERT INTO bne.TAB_Atividade (Idf_Plugins_Compatibilidade, Idf_Status_Atividade, Des_Parametros_Entrada, Dta_Cadastro, Dta_Agendamento, Flg_Inativo) 
                                                                VALUES (@Idf_Plugins_Compatibilidade, @Idf_Status_Atividade, @Des_Parametros_Entrada, @Dta_Cadastro, @Dta_Agendamento, 0);SET @Idf_Atividade = SCOPE_IDENTITY();", conn))
            {
                var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Size = 4, Direction = ParameterDirection.Output},
                    new SqlParameter{ ParameterName = "@Idf_Plugins_Compatibilidade", SqlDbType = SqlDbType.Int, Size = 4, Value = 6},
                    new SqlParameter{ ParameterName = "@Idf_Status_Atividade", SqlDbType = SqlDbType.Int, Size = 4, Value = 1},
                    new SqlParameter{ ParameterName = "@Des_Parametros_Entrada", SqlDbType = SqlDbType.Xml, Value = parametros.ToXML()},
                    new SqlParameter{ ParameterName = "@Dta_Cadastro", SqlDbType = SqlDbType.DateTime, Size = 8, Value = DateTime.Now},
                    new SqlParameter{ ParameterName = "@Dta_Agendamento", SqlDbType = SqlDbType.DateTime, Size = 8, Value = DateTime.Now}
                };

                foreach (SqlParameter sqlParameter in (IEnumerable<SqlParameter>)parms)
                {
                    if (cmd.Parameters.Contains(sqlParameter))
                        cmd.Parameters[sqlParameter.ParameterName] = sqlParameter;
                    else
                        cmd.Parameters.Add(sqlParameter);
                }

                cmd.ExecuteNonQuery();
                var atividade = Convert.ToInt32(cmd.Parameters["@Idf_Atividade"].Value);
                cmd.Parameters.Clear();

                if (atividade > 0)
                {
                    var xmlAtividade = new MensagemAtividade
                    {
                        IdfAtividade = atividade
                    };

                    var mq = new MessageQueue(CaminhoPadraoQueue + queueName);
                    var m = new Message(xmlAtividade);
                    mq.Send(m);
                }
            }
        }
    }
    #endregion

    #region EnvioEmailQueue
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void EnvioEmailMailingQueue(SqlInt32 idMensagem, SqlString de, SqlString para, SqlString assunto, [SqlFacet(MaxSize = -1)] SqlString mensagem)
    {
        const string queueName = "bne_envioemailmailing";

        var parametros = new ParametroExecucaoCollection
                {
                    {"idMensagem", "Mensagem", idMensagem.ToString(), idMensagem.ToString()},
                    {"emailRemetente", "Remetente", de.ToString().Trim(), de.ToString()},
                    {"emailDestinatario", "Destinatário", para.ToString().Trim(), para.ToString()},
                    {"assunto", "Assunto", assunto.ToString(), "Assunto" },
                    {"mensagem", "Mensagem", mensagem.ToString(), "Mensagem" }
                };

        using (var conn = new SqlConnection("context connection=true"))
        {
            conn.Open();

            using (var cmd = new SqlCommand(@"INSERT INTO bne.TAB_Atividade (Idf_Plugins_Compatibilidade, Idf_Status_Atividade, Des_Parametros_Entrada, Dta_Cadastro, Dta_Agendamento, Flg_Inativo) 
                                                                VALUES (@Idf_Plugins_Compatibilidade, @Idf_Status_Atividade, @Des_Parametros_Entrada, @Dta_Cadastro, @Dta_Agendamento, 0);SET @Idf_Atividade = SCOPE_IDENTITY();", conn))
            {
                var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Atividade", SqlDbType = SqlDbType.Int, Size = 4, Direction = ParameterDirection.Output},
                    new SqlParameter{ ParameterName = "@Idf_Plugins_Compatibilidade", SqlDbType = SqlDbType.Int, Size = 4, Value = 21},
                    new SqlParameter{ ParameterName = "@Idf_Status_Atividade", SqlDbType = SqlDbType.Int, Size = 4, Value = 1},
                    new SqlParameter{ ParameterName = "@Des_Parametros_Entrada", SqlDbType = SqlDbType.Xml, Value = parametros.ToXML()},
                    new SqlParameter{ ParameterName = "@Dta_Cadastro", SqlDbType = SqlDbType.DateTime, Size = 8, Value = DateTime.Now},
                    new SqlParameter{ ParameterName = "@Dta_Agendamento", SqlDbType = SqlDbType.DateTime, Size = 8, Value = DateTime.Now}
                };

                foreach (SqlParameter sqlParameter in (IEnumerable<SqlParameter>)parms)
                {
                    if (cmd.Parameters.Contains(sqlParameter))
                        cmd.Parameters[sqlParameter.ParameterName] = sqlParameter;
                    else
                        cmd.Parameters.Add(sqlParameter);
                }

                cmd.ExecuteNonQuery();
                var atividade = Convert.ToInt32(cmd.Parameters["@Idf_Atividade"].Value);
                cmd.Parameters.Clear();

                if (atividade > 0)
                {
                    var xmlAtividade = new MensagemAtividade
                    {
                        IdfAtividade = atividade
                    };

                    var mq = new MessageQueue(CaminhoPadraoQueue + queueName);
                    var m = new Message(xmlAtividade);
                    mq.Send(m);
                }
            }
        }
    }
    #endregion

    #region Mensagem
    [Serializable]
    public class MensagemAtividade
    {
        /// <summary>
        /// O idf da atividade no banco
        /// </summary>
        public int IdfAtividade { get; set; }

    }
    #endregion Mensagem

    #region Parametros

    [Serializable]
    public class ParametroExecucaoValor
    {
        [XmlAttribute]
        public String Valor { get; set; }
        [XmlAttribute]
        public String DesValor { get; set; }
    }

    /// <summary>
    /// Os parâmetros de execução
    /// </summary>
    [Serializable]
    public class ParametroExecucao
    {
        public ParametroExecucao()
        {
            Valores = new List<ParametroExecucaoValor>();
        }

        #region Valores
        public List<ParametroExecucaoValor> Valores { get; set; }
        #endregion

        #region Parametro
        /// <summary>
        /// Parametro
        /// </summary>
        public String Parametro { get; set; }
        #endregion

        #region DesParametro
        /// <summary>
        /// Descrição do parâmetro - Usado em tela
        /// </summary>
        public String DesParametro { get; set; }
        #endregion

        #region Valor
        /// <summary>
        /// Valor do primeiro parâmetro
        /// </summary>
        [XmlIgnore]
        public String Valor
        {
            get
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }
                return Valores[0].Valor;
            }
            set
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }
                Valores[0].Valor = value;
            }
        }
        #endregion

        #region DesValor
        /// <summary>
        /// Descrição do valor primeiro do parâmetro - Usado em tela
        /// </summary>
        [XmlIgnore]
        public String DesValor
        {
            get
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }

                var vals = new String[Valores.Count];
                for (int i = 0; i < Valores.Count; i++)
                {
                    vals[i] = Valores[i].DesValor;
                }

                return String.Join(", ", vals);
            }
            set
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }
                Valores[0].DesValor = value;
            }

        }
        #endregion

    }

    [Serializable]
    [XmlRoot(ElementName = "Parametros")]
    public class ParametroExecucaoCollection : Collection<ParametroExecucao>
    {

        #region Métodos

        #region ToXML

        /// <summary>
        /// Convert a coleção para XML
        /// </summary>
        /// <returns>A string xml que representa a coleção</returns>
        public String ToXML()
        {
            var serializer = new XmlSerializer(GetType());

            var settings = new XmlWriterSettings
            {
                Encoding = new UnicodeEncoding(false, false),
                Indent = false,
                OmitXmlDeclaration = false
            };

            using (var textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, this);
                }

                String res = textWriter.ToString();
                if (String.IsNullOrEmpty(res))
                    return String.Empty;

                return res.Substring(res.IndexOf("?>", StringComparison.Ordinal) + 2);
            }
        }

        #endregion

        #region Add
        /// <summary>
        /// Adiciona um ítem a coleção
        /// </summary>
        /// <param name="parametro">O parametro</param>
        /// <param name="desParametro">A descrição do parâmetro</param>
        /// <param name="valor">O valor do parâmetro</param>
        /// <param name="desValor">A descrição do valor do parâmetro</param>
        public void Add(String parametro, String desParametro, String valor, String desValor)
        {
            var objParametro = new ParametroExecucao
            {
                Parametro = parametro,
                DesParametro = desParametro,
                Valor = valor,
                DesValor = desValor
            };

            Add(objParametro);
        }
        #endregion

        #endregion

    }

    #endregion

}
