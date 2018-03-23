using System;
using System.Collections.Generic;
using BNE.Services.AsyncServices.Base.ProcessosAssincronos;
using Enumeradores = BNE.Mensagem.AsyncServices.BLL.Enumeradores;

namespace BNE.Mensagem.AsyncServices.Base.ProcessosAssincronos
{
    public class TarefaAssincrona
    {
        public TarefaAssincrona()
        {
            ParametrosEntrada = new ParametroExecucaoCollection();
            ParametrosSaida = new ParametroExecucaoCollection();
        }
        public string InputPlugin { get; set; }

        public Model.Sistema Sistema { get; set; }
        public Model.Template Template { get; set; }
        public ParametroExecucaoCollection ParametrosEntrada { get; set; }
        public ParametroExecucaoCollection ParametrosSaida { get; set; }
        public Enumeradores.TipoAtividade TipoAtividade { get; set; }

        public Dictionary<String, byte[]> Anexos { get; set; }

        public int IdfAtividade { get; set; }
        public string OutputPlugin { get; set; }

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

        public string CodigoPlugin { get; set; }

        public static TarefaAssincrona LoadFromDataRow(System.Data.DataRow row)
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

            if (row["Dta_Execucao"] != DBNull.Value && row["Dta_Execucao"] != null)
                objTarefa.DataExecucao = Convert.ToDateTime(row["Dta_Execucao"]);
            else
                objTarefa.DataExecucao = null;

            objTarefa.Usuario = Convert.ToString(row["Nme_Pessoa"]);

            objTarefa.IdfStatus = Convert.ToInt32(row["Idf_Status_Atividade"]);
            var status = (Enumeradores.StatusAtividade)objTarefa.IdfStatus;
            // Retorna a descrição da enumeration - Inédito na Webfopag =)
            objTarefa.Status = status.GetDescription();

            var parametrosEntrada = Convert.ToString(row["Des_Parametros_Entrada"]);
            if (!String.IsNullOrWhiteSpace(parametrosEntrada))
                objTarefa.ParametrosEntrada = ProcessoAssincrono.RecuperarParametros(parametrosEntrada);

            var parametrosSaida = Convert.ToString(row["Des_Parametros_Saida"]);
            if (!String.IsNullOrWhiteSpace(parametrosSaida))
                objTarefa.ParametrosSaida = ProcessoAssincrono.RecuperarParametros(parametrosSaida);

            objTarefa.CodigoPlugin = Convert.ToString(row["Cod_Plugin_Entrada"]);

            return objTarefa;

        }

    }
}
