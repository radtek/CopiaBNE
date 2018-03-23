using System;
using System.Collections.Generic;
using Enumeradores = BNE.BLL.AsyncServices.Enumeradores;

namespace BNE.Services.Base.ProcessosAssincronos
{
    [Serializable]
    public class TarefaAssincrona
    {
        public TarefaAssincrona()
        {
            ParametrosEntrada = new ParametroExecucaoCollection();
            ParametrosSaida = new ParametroExecucaoCollection();
        }
        public string InputPlugin { get; set; }

        public ParametroExecucaoCollection ParametrosEntrada { get; set; }
        public ParametroExecucaoCollection ParametrosSaida { get; set; }
        public Enumeradores.TipoAtividade TipoAtividade { get; set; }

        public Dictionary<String, byte[]> Anexos { get; set; }

        public int IdfAtividade { get; set; }

        public string OutputPlugin { get; set; }

        public TipoSaida? TipoSaida
        {
            get;
            set;
        }

        public String DescricaoTipoSaida
        {
            get;
            set;
        }

        public string NmeProcesso { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public DateTime DataExpiracao { get; set; }
        public DateTime? DataExecucao { get; set; }

        public string Usuario { get; set; }

        public string Status { get; set; }
        public int IdfStatus { get; set; }

        public string DescricaoCaminhoArquivoUpload { get; set; }

        public string DescricaoCaminhoArquivoGerado { get; set; }

        public string CodigoPlugin { get; set; }


        public static TarefaAssincrona LoadFromDataRow(System.Data.DataRow row, String urlDownloadArquivoGerado, String urlDownloadArquivoAnexo)
        {
            var objTarefa = new TarefaAssincrona
                {
                    IdfAtividade = Convert.ToInt32(row["Idf_Atividade"])
                };

            if (row["Cod_Plugin_Entrada"] != DBNull.Value && row["Cod_Plugin_Entrada"] != null)
            {
                objTarefa.NmeProcesso = String.Format("{0} - {1}",
                                                    Convert.ToString(row["Cod_Plugin_Entrada"]),
                                                    Convert.ToString(row["Nme_Processo"]));
            }
            else
            {
                objTarefa.NmeProcesso = String.Format(Convert.ToString(row["Nme_Processo"]));
            }

            objTarefa.DataSolicitacao = Convert.ToDateTime(row["Dta_Agendamento"]);
            objTarefa.DataExpiracao = objTarefa.DataSolicitacao.AddDays(Convert.ToInt32(row["Num_Dias_Expiracao"]));

            if (row["Dta_Execucao"] != DBNull.Value && row["Dta_Execucao"] != null)
                objTarefa.DataExecucao = Convert.ToDateTime(row["Dta_Execucao"]);
            else
                objTarefa.DataExecucao = null;

            objTarefa.Usuario = Convert.ToString(row["Nme_Pessoa"]);

            objTarefa.IdfStatus = Convert.ToInt32(row["Idf_Status_Atividade"]);
            var status = (Enumeradores.StatusAtividade)objTarefa.IdfStatus;
            // Retorna a descrição da enumeration - Inédito na Webfopag =)
            objTarefa.Status = status.GetDescription();

            objTarefa.DescricaoCaminhoArquivoUpload = Convert.ToString(row["Des_Caminho_Arquivo_Upload"]);
            if (!String.IsNullOrEmpty(objTarefa.DescricaoCaminhoArquivoUpload))
                objTarefa.DescricaoCaminhoArquivoUpload = urlDownloadArquivoAnexo + objTarefa.DescricaoCaminhoArquivoUpload;

            objTarefa.DescricaoCaminhoArquivoGerado = Convert.ToString(row["Des_Caminho_Arquivo_Gerado"]);
            if (!String.IsNullOrEmpty(objTarefa.DescricaoCaminhoArquivoGerado))
                objTarefa.DescricaoCaminhoArquivoGerado = urlDownloadArquivoGerado + objTarefa.DescricaoCaminhoArquivoGerado;

            // Parâmetros de entrada
            var xmlEntrada = new System.Xml.XmlDocument();

            if (!String.IsNullOrEmpty(Convert.ToString(row["Des_Parametros_Entrada"])))
            {
                xmlEntrada.LoadXml(Convert.ToString(row["Des_Parametros_Entrada"]));
                objTarefa.ParametrosEntrada = ProcessoAssincrono.XmlToCollection(xmlEntrada);
            }

            if (!String.IsNullOrEmpty(Convert.ToString(row["Des_Parametros_Saida"])))
            {
                // Parâmetros de saída
                var xmlSaida = new System.Xml.XmlDocument();
                xmlSaida.LoadXml(Convert.ToString(row["Des_Parametros_Saida"]));
                objTarefa.ParametrosSaida = ProcessoAssincrono.XmlToCollection(xmlSaida);
            }

            if (row["Idf_Tipo_Saida"] != DBNull.Value && row["Idf_Tipo_Saida"] != null)
            {
                objTarefa.TipoSaida = (TipoSaida)Convert.ToInt32(row["Idf_Tipo_Saida"]);
                // Usa o extension method de Employer.Components.UI.Web.Extensions
                objTarefa.DescricaoTipoSaida = objTarefa.TipoSaida.Value.GetDescription();
            }
            else
            {
                objTarefa.TipoSaida = null;
                objTarefa.DescricaoTipoSaida = String.Empty;
            }

            objTarefa.CodigoPlugin = Convert.ToString(row["Cod_Plugin_Entrada"]);

            return objTarefa;

        }
    }
}