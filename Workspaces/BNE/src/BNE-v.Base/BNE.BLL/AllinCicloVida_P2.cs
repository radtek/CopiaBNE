//-- Data: 15/07/2014 10:39
//-- Autor: Lennon Vidal

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BNE.BLL
{
    public partial class AllinCicloVida // Tabela: BNE_Allin_Ciclo_Vida
    {
        public static bool CicloDeVidaAtivo(int cicloVidaId, SqlTransaction trans = null)
        {
            var cmd = @"SELECT TOP 1 1 FROM BNE_Allin_Ciclo_Vida AS acv
            JOIN TAB_Tipo_Gatilho gat ON acv.Idf_Tipo_Gatilho = gat.Idf_Tipo_Gatilho
            WHERE  acv.Flg_Inativo = 0 AND gat.Flg_Inativo = 0 AND acv.Idf_Allin_Ciclo_Vida = @Idf_Allin_Ciclo_Vida";

            var sqlParams = new List<SqlParameter> { 
                new SqlParameter("@Idf_Allin_Ciclo_Vida", (int)cicloVidaId) { SqlDbType = SqlDbType.Int  }
            };

            Func<object> resultAccessor;
            if (trans == null)
            {
                resultAccessor = () => DataAccessLayer.ExecuteScalar(CommandType.Text, cmd, sqlParams);
            }
            else
            {
                resultAccessor = () => DataAccessLayer.ExecuteScalar(trans, CommandType.Text, cmd, sqlParams);
            }

            var res = resultAccessor();

            int value;
            if (Convert.IsDBNull(res) || res == null || !Int32.TryParse(res.ToString(), out value) || value != 1)
                return false;

            return true;
        }

        public static IEnumerable<AllinCicloVida> CarregarPorGatilho(Enumeradores.TipoGatilho gatilho, SqlTransaction trans = null)
        {
            var cmd = @"SELECT acv.* FROM BNE_Allin_Ciclo_Vida AS acv
            JOIN TAB_Tipo_Gatilho gat ON acv.Idf_Tipo_Gatilho = gat.Idf_Tipo_Gatilho
            WHERE  acv.Flg_Inativo = 0 AND gat.Flg_Inativo = 0 AND acv.Idf_Tipo_Gatilho = @Idf_Tipo_Gatilho";

            var sqlParams = new List<SqlParameter> { 
                new SqlParameter("@Idf_Tipo_Gatilho", (int)gatilho) { SqlDbType = SqlDbType.Int  }
            };

            Func<IDataReader> readerAccessor;
            if (trans == null)
            {
                readerAccessor = () => DataAccessLayer.ExecuteReader(CommandType.Text, cmd, sqlParams);
            }
            else
            {
                readerAccessor = () => DataAccessLayer.ExecuteReader(trans, CommandType.Text, cmd, sqlParams);
            }

            using (var dr = readerAccessor())
            {
                while (dr.Read())
                {
                    yield return PopularCicloDeVida(dr);
                }
            }
        }

        private static AllinCicloVida PopularCicloDeVida(IDataReader dr)
        {
            var objAllinCicloVida = new AllinCicloVida();

            objAllinCicloVida._idAllinCicloVida = Convert.ToInt32(dr["Idf_Allin_Ciclo_Vida"]);
            if (dr["Idf_Tipo_Gatilho"] != DBNull.Value)
                objAllinCicloVida._tipoGatilho = new TipoGatilho(Convert.ToInt32(dr["Idf_Tipo_Gatilho"]));
            objAllinCicloVida._IdentificadorCicloAllin = Convert.ToString(dr["Identificador_Ciclo_Allin"]);
            objAllinCicloVida._descricaoEvento = Convert.ToString(dr["Des_Evento"]);
            objAllinCicloVida._flagAceitaRepeticao = Convert.ToBoolean(dr["Flg_Aceita_Repeticao"]);
            objAllinCicloVida._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

            if (HasColumn(dr, "Des_Google_Utm"))
                if (dr["Des_Google_Utm"] != DBNull.Value)
                    objAllinCicloVida._descricaoGoogleUtm = Convert.ToString(dr["Des_Google_Utm"]);

            objAllinCicloVida._persisted = true;
            objAllinCicloVida._modified = false;

            return objAllinCicloVida;
        }

        private static bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

    }
}