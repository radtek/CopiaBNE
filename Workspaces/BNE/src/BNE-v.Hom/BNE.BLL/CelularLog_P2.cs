//-- Data: 16/07/2015 17:45
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;

namespace BNE.BLL
{
    public partial class CelularLog // Tabela: plataforma.Tab_Celular_Log
    {

        #region Spinsert
        private const string Spinsert = @"
        INSERT INTO plataforma.Tab_Celular_Log (Idf_Operadora_Celular, Num_DDD_Celular_Log, Num_Celular_Log)
        SELECT {0}, {1} AS DDD, Data AS FONE FROM dbo.GieysonSplit('{2}', ',') OPTION (MAXRECURSION 0)
        ";
        #endregion

        #region InserirTelefone
        public static void InserirTelefone(int idOperadoraCelular, int ddd, params string[] numeros)
        {
            string listaNumeros = String.Join(",", numeros);

            string sql = String.Format(Spinsert, idOperadoraCelular == 0 ? "NULL" : idOperadoraCelular.ToString(), ddd, listaNumeros);

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, sql, null);
        }
        #endregion

        #region JaPesquisado
        public static bool JaPesquisado(string telefone)
        {
            string sql = @"
            SELECT COUNT(*) FROM plataforma.Tab_Celular_Log 
            WHERE LTRIM(RTRIM(Num_DDD_Celular_Log)) + LTRIM(RTRIM(Num_Celular_Log)) like LTRIM(RTRIM({0}))";

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, string.Format(sql, telefone), null)) > 0;
        }
        #endregion

        #region JaPesquisado
        public static List<string> JaPesquisado()
        {
            string sql = @"
            SELECT Num_DDD_Celular_Log, Num_Celular_Log FROM plataforma.Tab_Celular_Log 
            ";

            var lista = new List<string>();
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, sql, null))
            {
                while (dr.Read())
                    lista.Add(dr["Num_DDD_Celular_Log"].ToString().Trim() + dr["Num_Celular_Log"].ToString().Trim());
            }

            return lista;
        }
        #endregion

    }
}