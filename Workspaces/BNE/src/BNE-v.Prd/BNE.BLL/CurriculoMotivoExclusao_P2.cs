using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL
{
    public partial class CurriculoMotivoExclusao
    {
        #region [ Consultas ]
        
        #region [ RegistrarMotivoHibernarCurriculo ]
        private const string RegistrarMotivoHibernarCurriculo = @"INSERT BNE_Curriculo_Motivo_Exclusao (Idf_Curriculo,Idf_Motivo_Exclusao,Flg_Emprego_BNE,Des_Motivo_Exclusao)
                                                                        VALUES(@Idf_Curriculo,@Idf_Motivo_Exclusao,@Flg_Emprego_BNE,@Des_Motivo_Exclusao)";
        #endregion

        #endregion

        #region [ RegistrarCurriculoMotivoExclusao ]
        /// <summary>
        /// Registrar o motivo da exclusão
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="outroMotivo"></param>
        /// <param name="conseguiuEmpregonoBNE"></param>
        /// <returns></returns>
        public static bool RegistrarCurriculoMotivoExclusao(int idCurriculo, string outroMotivo, bool conseguiuEmpregonoBNE, byte idMotivo)
        {

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo } ,
                    new SqlParameter { ParameterName = "@Idf_Motivo_Exclusao", SqlDbType = SqlDbType.TinyInt, Value = idMotivo },
                    new SqlParameter { ParameterName = "@Flg_Emprego_BNE", SqlDbType = SqlDbType.Bit, Value = conseguiuEmpregonoBNE },
                    new SqlParameter { ParameterName = "@Des_Motivo_Exclusao", SqlDbType = SqlDbType.VarChar, Size = 200, Value = outroMotivo }

                };

            return DataAccessLayer.ExecuteNonQuery(System.Data.CommandType.Text,RegistrarMotivoHibernarCurriculo,parms) > 0;

        }
        #endregion
    }
}
