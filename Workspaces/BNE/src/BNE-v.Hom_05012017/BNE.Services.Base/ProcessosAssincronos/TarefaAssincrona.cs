using System;
using System.Collections.Generic;
using System.Data;
using BNE.BLL.AsyncServices;

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

        public int IdfAtividade { get; set; }
        public string InputPlugin { get; set; }
        public string OutputPlugin { get; set; }
        public ParametroExecucaoCollection ParametrosEntrada { get; set; }
        public ParametroExecucaoCollection ParametrosSaida { get; set; }
        public Dictionary<string, byte[]> Anexos { get; set; }
        public string DescricaoCaminhoArquivoUpload { get; set; }
        public string DescricaoCaminhoArquivoGerado { get; set; }
        public DateTime DataSolicitacao { get; set; }

        public static TarefaAssincrona RecuperarTarefaAssincrona(Atividade atividade)
        {
            var objTarefaAssincrona = new TarefaAssincrona();
            //Feio, mas se mudar para a BLL vai ter que refatorar os parametros xml do projeto e todas as suas referencias (vide plugins)
            using (var dr = atividade.RecuperarDadosTarefaAssincrona())
            {
                if (dr.Read())
                {
                    objTarefaAssincrona.LoadFromDataRow(dr);
                    return objTarefaAssincrona;
                }
            }
            return null;
        }

        public void LoadFromDataRow(IDataReader dr)
        {
            IdfAtividade = Convert.ToInt32(dr["Idf_Atividade"]);
            InputPlugin = dr["Des_Plugin_Metadata_Entrada"].ToString();
            OutputPlugin = dr["Des_Plugin_Metadata_Saida"].ToString();
            DataSolicitacao = Convert.ToDateTime(dr["Dta_Cadastro"]);
            DescricaoCaminhoArquivoUpload = dr["Des_Caminho_Arquivo_Upload"].ToString();

            if (!string.IsNullOrEmpty(DescricaoCaminhoArquivoUpload))
            {
                Anexos = new Dictionary<string, byte[]>
                {
                    {DescricaoCaminhoArquivoUpload, ProcessoAssincrono.RecuperarArquivoAnexo(DescricaoCaminhoArquivoUpload)}
                };
            }

            if (dr["Des_Parametros_Entrada"] != DBNull.Value)
            {
                ParametrosEntrada = ProcessoAssincrono.XmlToCollection(dr["Des_Parametros_Entrada"].ToString());
                ParametrosEntrada.Add("IdfAtividade", string.Empty, Convert.ToString(IdfAtividade), string.Empty);
            }

            if (dr["Des_Parametros_Saida"] != DBNull.Value)
            {
                ParametrosSaida = ProcessoAssincrono.XmlToCollection(dr["Des_Parametros_Saida"].ToString());
            }
        }

        //}
        //    ParametrosEntrada.Add("IdfAtividade", string.Empty, Convert.ToString(IdfAtividade), string.Empty);
        //    ParametrosEntrada = ProcessoAssincrono.XmlToCollection(reader);
        //    reader.Read();
        //    XmlReader reader = XmlReader.Create(new StringReader(parametrosEntrada));
        //{

        //private void ReadFromXMLReader(string parametrosEntrada)
        //}
        //    ParametrosEntrada.Add("IdfAtividade", string.Empty, Convert.ToString(IdfAtividade), string.Empty);
        //    ParametrosEntrada = ProcessoAssincrono.XmlToCollection(parametrosEntrada);
        //{

        //private void ReadFromXMLString(string parametrosEntrada)

        //private void ReadFromXMLDocument(string parametrosEntrada)
        //{
        //    var xmlEntrada = new XmlDocument();
        //    xmlEntrada.LoadXml(Convert.ToString(parametrosEntrada));
        //    ParametrosEntrada = ProcessoAssincrono.XmlToCollection(xmlEntrada);
        //    ParametrosEntrada.Add("IdfAtividade", string.Empty, Convert.ToString(IdfAtividade), string.Empty);
        //}
    }
}