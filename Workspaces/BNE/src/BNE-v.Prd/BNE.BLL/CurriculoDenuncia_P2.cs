//-- Data: 19/09/2017 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class CurriculoDenuncia // Tabela: BNE_Curriculo_Denuncia
    {
        private const string Spjapossuidenuncia = @"
        SELECT  COUNT(CD.Idf_Curriculo_Correcao)
        FROM    BNE_Curriculo_Denuncia CD 
                INNER JOIN BNE_Curriculo_Correcao CC ON CD.Idf_Curriculo_Correcao = CC.Idf_Curriculo_Correcao
        WHERE   CD.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
                AND CC.Idf_Curriculo = @Idf_Curriculo
        ";

        internal static bool JaPossuiDenuncia(Curriculo curriculo, PlanoAdquirido planoAdquirido, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = curriculo.IdCurriculo},
                new SqlParameter{ ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = planoAdquirido.IdPlanoAdquirido},
            };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spjapossuidenuncia, parms)) > 0;
        }
    }
}