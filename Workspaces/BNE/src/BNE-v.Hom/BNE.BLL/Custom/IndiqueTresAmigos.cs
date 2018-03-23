using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL.Custom
{
    public static class IndiqueTresAmigos
    {

        private static string QUERY_COM_RM = " SELECT COUNT(Cities.Idf_Cidade) FROM ( " +
                                             " SELECT c.Idf_Cidade FROM plataforma.TAB_Cidade c WITH (NOLOCK) " +
                                             " INNER JOIN BNE.TAB_Regiao_Metropolitana_Cidade rgm WITH (NOLOCK)  ON c.Idf_Cidade = rgm.Idf_Cidade " +
                                             " INNER JOIN BNE.TAB_Regiao_Metropolitana rm WITH(NOLOCK) ON rm.Idf_Regiao_Metropolitana = rgm.Idf_Regiao_Metropolitana " +
                                             " WHERE rm.Idf_Cidade IN({0}) " +
                                             " UNION " +
                                             " SELECT c.Idf_Cidade FROM plataforma.TAB_Cidade c WITH (NOLOCK) " +
                                             " WHERE  c.Idf_Cidade IN({0})) AS Cities WHERE Cities.Idf_Cidade = @Idf_Cidade;";


        private static string QUERY_SEM_RM = " SELECT COUNT(Idf_Cidade) FROM plataforma.TAB_Cidade WHERE Idf_Cidade IN({0}) AND Idf_Cidade = @Idf_Cidade;";

        public static bool EstaNaRegiaoDeCampanha(PessoaFisica pf)
        {
            return EstaNaRegiaoDeCampanha(pf.Cidade.IdCidade);
        }

        public static bool EstaNaRegiaoDeCampanha(int idf_Cidade) 
        {
            List<Enumeradores.Parametro> list = new List<Enumeradores.Parametro>();
            list.Add(Enumeradores.Parametro.CampanhaIndiqueCidades);
            list.Add(Enumeradores.Parametro.CampanhaIndiqueUsarRM);

            Dictionary<Enumeradores.Parametro, string> prms =  Parametro.ListarParametros(list);
            string query = (prms[Enumeradores.Parametro.CampanhaIndiqueUsarRM].Equals("1")) ? QUERY_COM_RM : QUERY_SEM_RM;

            if (prms[Enumeradores.Parametro.CampanhaIndiqueCidades] == "") 
            {
                return false;
            }
            else
            {
                query = string.Format(query, prms[Enumeradores.Parametro.CampanhaIndiqueCidades]);
                SqlParameter Idf_Cidade = new SqlParameter("Idf_Cidade", SqlDbType.Int, 4);
                
                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Size = 4, Value = idf_Cidade }
                };

                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, query, parms)) > 0;
            }
        } 
    }
}
