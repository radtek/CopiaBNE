using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class DeficienciaDetalhe
    {

        #region Consultas
        private const string spSelect = @"select idf_deficiencia_detalhe, des_Deficiencia_Detalhe from bne_deficiencia_detalhe with(nolock)
                                          where idf_deficiencia = @idf_deficiencia and flg_inativo = 0";
        #endregion

        #region Metodos

        #region ListaDeficiencia



        public static Dictionary<string, string> listaDeficiencia(int tipoDeficiencia)
        {
            var dicionario = new Dictionary<string, string>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Deficiencia", SqlDbType = SqlDbType.Int, Size = 4, Value = tipoDeficiencia }
                };
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spSelect, parms))
            {

                while (dr.Read())
                    dicionario.Add(dr["idf_Deficiencia_Detalhe"].ToString(), dr["Des_Deficiencia_Detalhe"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionario;
        }
        #endregion
        #endregion
    }
}
