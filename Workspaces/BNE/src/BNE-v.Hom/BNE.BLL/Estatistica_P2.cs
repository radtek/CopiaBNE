//-- Data: 04/05/2010 09:15
//-- Autor: Gieyson Stelmak

using System;
using System.Data;
using BNE.EL;
namespace BNE.BLL
{
    public partial class Estatistica : Entity// Tabela: BNE_Estatistica
    {
        public static Estatistica Estatisticas => Cache.GetItem(typeof(Estatistica).ToString(), RecuperarEstatistica, TimeSpan.FromTicks(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 05, 00).AddDays(1).Ticks));
        private const string SPSELECTESTATISTICA = @"SELECT TOP 1 * FROM BNE_Estatistica ORDER BY Dta_Cadastro DESC";

        #region RecuperarEstatistica
        /// <summary>
        /// Método responsável por retornar a última estatística do site de curriculos e vagas.
        /// </summary>
        /// <returns></returns>
        private static Estatistica RecuperarEstatistica()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTESTATISTICA, null))
            {
                var objEstatistica = new Estatistica();
                if (SetInstance(dr, objEstatistica))
                {
                    long totalParametroContador = Int64.Parse(Parametro.RecuperaValorParametro(Enumeradores.Parametro.ContadorCurriculo));
                    objEstatistica.QuantidadeCurriculo = objEstatistica.QuantidadeCurriculo + totalParametroContador;

                    return objEstatistica;
                }
            }
            throw (new RecordNotFoundException(typeof(Estatistica)));
        }
        #endregion

    }
}