using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class CodigoDescontoPlano
    {

        #region Consultas
        private const string SpCarregarPorTipoCodigoDesconto = @"
        SELECT *
        FROM BNE.BNE_Codigo_Desconto_Plano WITH (NOLOCK)
        WHERE Idf_Tipo_Codigo_Desconto = @Idf_Tipo_Codigo_Desconto
";
        #endregion

        #region Métodos
        public static bool CarregarPorTipoCodigoDesconto(TipoCodigoDesconto objTipoCodigoDesconto, out List<Plano> planos)
        {
            planos = new List<Plano>();

            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Tipo_Codigo_Desconto", SqlDbType.Int, 4)
            };

            parms[0].Value = objTipoCodigoDesconto.IdTipoCodigoDesconto;

            using (IDataReader dr =
                DataAccessLayer.ExecuteReader(CommandType.Text, SpCarregarPorTipoCodigoDesconto, parms))
            {
                while (dr.Read())
                {
                    planos.Add(new Plano(Convert.ToInt32(dr["Idf_Plano"])));
                }
            }

            return planos.Count > 0;
        }
        #endregion
    }
}
