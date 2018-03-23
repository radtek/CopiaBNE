using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace BNE.Busca
{
    public partial class CLR
    {

        //[SqlFunction(FillRowMethodName = "FillRowComStopword", TableDefinition = "Des_Palavra NVARCHAR(200)")]
        //public static IEnumerable ParseComStopword(String queryuser)
        //{
        //    string[] q = queryuser.Split(" ,.:;\"'~`!@$%^&*?()={}[]<>/|\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        //    List<string> palavras = new List<string>(); //remover repetidas
        //    foreach (string p in q)
        //    {
        //        if (!palavras.Contains(p))
        //            palavras.Add(p);
        //    }
        //    return q;
        //}

        //public static void FillRowComStopword(Object obj, out SqlString palavra)
        //{
        //    String pal = (String)obj;
        //    palavra = LimpaLower(pal);
        //}

        //[SqlFunction(FillRowMethodName = "FillRowSemStopword", DataAccess = DataAccessKind.Read, TableDefinition = "Des_Palavra NVARCHAR(100)")]
        //public static IEnumerable ParseSemStopword(String queryuser)
        //{
        //    string[] q = queryuser.Split(" ,.:;\"'~`!@$%^&*?()={}[]<>/|\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        //    List<string> stopwords = new List<string>();

        //    using (SqlConnection conn = new SqlConnection("context connection=true"))
        //    {
        //        conn.Open();

        //        using (SqlCommand cmd = new SqlCommand("SELECT Des_StopWord FROM vaga.TAB_StopWord", conn))
        //        {
        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    stopwords.Add(dr["Des_StopWord"].ToString());
        //                }
        //                dr.Close();
        //            }
        //        }
        //    }

        //    List<string> palavras = new List<string>(); //remover repetidas
        //    foreach (string p in q)
        //    {
        //        string pal = LimpaLower(p);
        //        if (!palavras.Contains(pal))
        //        {
        //            if(!stopwords.Contains(pal))
        //                palavras.Add(pal);
        //        }
        //    }
        //    return palavras;
        //}

        //public static void FillRowSemStopword(Object obj, out SqlString palavra)
        //{
        //    String pal = (String)obj;
        //    palavra = pal;
        //}

        //[SqlFunction]
        //public static SqlString LimpaQueryLower(String queryusr)
        //{
        //    return new SqlString(LimpaLower(queryusr));
        //}

        //private static string LimpaLower(String queryusr)
        //{
        //    string palavraSemAcento = null;
        //    string caracterComAcento = "áàãâäéèêëíìîïóòõôöúùûüçÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÖÔÚÙÛÜÇ,.:;\"'~`!@$%^&*?()={}[]<>/|\\";
        //    string caracterSemAcento = "aaaaaeeeeiiiiooooouuuucaaaaaeeeeiiiiooooouuuuc                            ";

        //    for (int i = 0; i < queryusr.Length; i++)
        //    {
        //        if (caracterComAcento.IndexOf(Convert.ToChar(queryusr.Substring(i, 1))) >= 0)
        //        {
        //            int car = caracterComAcento.IndexOf(Convert.ToChar(queryusr.Substring(i, 1)));
        //            palavraSemAcento += caracterSemAcento.Substring(car, 1);
        //        }
        //        else
        //        {
        //            palavraSemAcento += queryusr.Substring(i, 1);
        //        }
        //    }
        //    return palavraSemAcento.ToLower().Trim();
        //}
    }
}