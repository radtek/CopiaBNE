using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class EstatisticaPlano // Tabela: BNE_Estatistica_Plano
    {
        private const string SPSELECTESTATISTICAPLANOPORFUNCAOCATEGORIA = @"
        SELECT TOP 1 * FROM BNE.BNE_Estatistica_Plano WHERE Idf_Parametro=@Idf_Parametro
        ORDER BY Idf_Estatistica DESC";

        #region RecuperarEstatisticaPlano
        /// <summary>
        /// Método responsável por retornar a última estatística do Plano VIP.
        /// </summary>
        /// <returns></returns>
        public static int RecuperarEstatisticaPlano(int idParametro)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Parametro",SqlDbType.Int,4));

            parms[0].Value = idParametro;

            EstatisticaPlano objEstatisticaPlano;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTESTATISTICAPLANOPORFUNCAOCATEGORIA, parms))
            {
                objEstatisticaPlano = new EstatisticaPlano();
                if (SetInstance(dr, objEstatisticaPlano))
                    return objEstatisticaPlano.QuantidadeCurriculo;
            }
            throw (new RecordNotFoundException(typeof(EstatisticaPlano)));
        }
        #endregion
    }
}
