//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class Feriado // Tabela: TAB_Feriado
    {

        #region Consultas

        private const string SPSELECTFERIADO = @"SELECT COUNT(Dta_Feriado) FROM TAB_Feriado 
                                                WHERE CONVERT(varchar,Dta_Feriado,103) = @Data";
        #endregion

        #region Métodos

        #region VerificarFeriado

        /// <summary>
        /// Método que verifica se há algum feriado no dia passado como parâmetro
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Cont Dta_Feriado</returns>
        public static bool VerificarFeriado(DateTime data)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Data", SqlDbType.DateTime, 4));
            parms[0].Value = data;

            int resultado = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTFERIADO, parms));
            if (resultado > 1)
                return true;
            else
                return false;

        }

        #endregion

        #region RetornarDiaUtil

        public static int RetornarDiaUtilVencimento(DateTime dataVencimento)
        {            
            bool feriadoNaoEncontrado = false;
            int adicionarDias = 0;

            //Verifica se existe feriado no dia informado
            while (!feriadoNaoEncontrado)
            {
                if (VerificarFeriado(dataVencimento))
                {
                    feriadoNaoEncontrado = false;                    
                    dataVencimento.AddDays(adicionarDias++);
                }
                else
                    feriadoNaoEncontrado = true;
            }

            bool diaUtil = false;

            //Depois verifica se a data de vencimento é um dia Util.
            while (!diaUtil)
            {
                switch (dataVencimento.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        dataVencimento = dataVencimento.AddDays(1); 
                        adicionarDias++;
                        
                         diaUtil = false;
                        break;
                    case DayOfWeek.Sunday:
                        dataVencimento = dataVencimento.AddDays(1);
                        adicionarDias++;
                        diaUtil = false;
                        break;
                    default:
                        diaUtil = true;
                        break;
                }                       
            }

            return adicionarDias;
        }

        #endregion

        #endregion


    }
}