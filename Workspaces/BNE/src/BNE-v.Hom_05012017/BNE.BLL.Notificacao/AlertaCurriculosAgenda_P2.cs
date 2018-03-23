using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public partial class AlertaCurriculosAgenda // Tabela alerta.Tab_Alerta_Curriculos_Agenda_Semanal
    {
        #region Consultas

        #region spSelectDiasDaSemanaCandidato
        private const string spSelectDiasDaSemanaCandidato = @"
        SELECT Idf_Dia_Da_Semana FROM alerta.Tab_Alerta_Curriculos_Agenda_Semanal
        WHERE Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #endregion

        #region Métodos

        public static DataTable ListarDiasDaSemana(int idCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 8));
            parms[0].Value = idCurriculo;

            DataTable dt = null;
            try
            {
                using (DataSet ds = DataAccessLayer.ExecuteReaderDs(CommandType.Text, spSelectDiasDaSemanaCandidato, parms, DataAccessLayer.CONN_STRING))
                {
                    dt = ds.Tables[0];
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }

        #region SalvarDiasDaSemana
        /// <summary>
        /// Salvar os dias da semana no CurrículoAgenda
        /// </summary>
        public static void SalvarDiasDaSemana(int idCurriculo, Dictionary<int,bool> diasDaSemana, SqlTransaction trans = null)
        {
            AlertaCurriculosAgenda alerta = null;

            foreach (var dia in diasDaSemana)
            {
                //checar se existe registro para o dia
                alerta = AlertaCurriculosAgenda.LoadObject(idCurriculo, dia.Key);

                //se não existir alerta para o dia e o dia estiver checado
                //vai inserir o novo registro
                if (alerta == null && dia.Value)
                {
                    alerta = new AlertaCurriculosAgenda();
                    alerta.IdCurriculo = idCurriculo;
                    alerta.IdDiaDaSemana = dia.Key;
                    alerta.DataCadastro = DateTime.Now;
                    if (trans != null)
                        alerta.Save(trans);
                    else
                        alerta.Save();
                }
                else if(alerta != null && !dia.Value) //se dia foi desmarcado vai apagar da base
                {
                    AlertaCurriculosAgenda.Delete(idCurriculo, dia.Key);
                }

                alerta = null;
            }
        }
        #endregion


        #endregion
    }
}
