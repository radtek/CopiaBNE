//-- Data: 15/07/2014 10:39
//-- Autor: Lennon Vidal

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class AllinTransacional // Tabela: BNE_Allin_Transacional
    {
        public bool TransacionalAtivo(int transacionalId, SqlTransaction trans = null)
        {
            var cmd = @"SELECT TOP 1 1 FROM BNE_AllIn_Transacional AS tra
              JOIN TAB_Tipo_Gatilho gat ON tra.Idf_Tipo_Gatilho = gat.Idf_Tipo_Gatilho
              WHERE tra.Flg_Inativo = 0 AND gat.Flg_Inativo = 0 AND acv.Idf_Allin_Ciclo_Vida = @Idf_Allin_Transacao";

            var sqlParams = new List<SqlParameter> { 
                new SqlParameter("@Idf_Allin_Transacao", (int)transacionalId) { SqlDbType = SqlDbType.Int  }
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

        public static IEnumerable<AllinTransacional> CarregarPorGatilho(Enumeradores.TipoGatilho gatilho, SqlTransaction trans = null)
        {
            var cmd = @"SELECT tra.* FROM BNE_AllIn_Transacional AS tra
              JOIN TAB_Tipo_Gatilho gat ON tra.Idf_Tipo_Gatilho = gat.Idf_Tipo_Gatilho
              WHERE tra.Flg_Inativo = 0 AND gat.Flg_Inativo = 0 AND tra.Idf_Tipo_Gatilho = @Idf_Tipo_Gatilho";

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
                    yield return PopularTransacional(dr);
                }
            }
        }

        private static AllinTransacional PopularTransacional(IDataReader dr)
        {
            var objAllinTransacional = new AllinTransacional();

            objAllinTransacional._idAllinTransacao = Convert.ToInt32(dr["Idf_Allin_Transacao"]);
            if (dr["Idf_Tipo_Gatilho"] != DBNull.Value)
                objAllinTransacional._tipoGatilho = new TipoGatilho(Convert.ToInt32(dr["Idf_Tipo_Gatilho"]));
            objAllinTransacional._descricaoAssunto = Convert.ToString(dr["Des_Assunto"]);
            objAllinTransacional._emailRemetente = Convert.ToString(dr["Eml_Remetente"]);
            objAllinTransacional._emailResposta = Convert.ToString(dr["Eml_Resposta"]);
            objAllinTransacional._IdentificadorHtmlAllin = Convert.ToString(dr["Identificador_Html_Allin"]);
            objAllinTransacional._nomeRemetente = Convert.ToString(dr["Nme_Remetente"]);
            objAllinTransacional._flagAgendar = Convert.ToBoolean(dr["Flg_Agendar"]);
            if (dr["Qtd_Dias_Disparo"] != DBNull.Value)
                objAllinTransacional._quantidadeDiasDisparo = Convert.ToDecimal(dr["Qtd_Dias_Disparo"]);
            if (dr["Hora_Disparo"] != DBNull.Value)
                objAllinTransacional._horaDisparo = dr["Hora_Disparo"] is TimeSpan ? (TimeSpan)dr["Hora_Disparo"] :
                                                                                     dr["Hora_Disparo"] is int || dr["Hora_Disparo"] is long
                                                                                            ? new TimeSpan(Convert.ToInt64(dr["Hora_Disparo"]))
                                                                                            : TimeSpan.Parse(dr["Hora_Disparo"].ToString());
            objAllinTransacional._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            if (dr["Des_Google_Utm"] != DBNull.Value)
                objAllinTransacional._descricaoGoogleUtm = Convert.ToString(dr["Des_Google_Utm"]);

            objAllinTransacional._persisted = true;
            objAllinTransacional._modified = false;

            return objAllinTransacional;
        }
    }
}