//-- Data: 29/07/2010 11:19
//-- Autor: Gieyson Stelmak
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class ConteudoHTML // Tabela: BNE_Conteudo_HTML
    {

        #region Consultas

        private const string SPLISTARSMS = @"
        SELECT 
            Idf_Conteudo ,
            Nme_Conteudo ,
            Vlr_Conteudo
        FROM BNE_Conteudo_HTML
        WHERE Idf_Conteudo in (33, 73, 74, 75, 76, 77, 78)";

        private const string SPLISTAREMAIL = @"
        SELECT 
        Idf_Conteudo ,
        Nme_Conteudo ,
        Vlr_Conteudo
        FROM BNE_Conteudo_HTML
        WHERE Idf_Conteudo in (12, 38, 59, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95)";

        #region Spselectvalorconteudo
        private const string Spselectvalorconteudo = "SELECT Vlr_Conteudo FROM BNE_Conteudo_HTML WITH(NOLOCK) WHERE Idf_Conteudo = @Idf_Conteudo";
        #endregion

        #endregion

        #region Metodos

        #region RecuperaValorConteudo
        /// <summary>
        /// Método que recupera o valor de um conteúdo a partir do id.
        /// </summary>
        /// <param name="idConteudo">>Identificador do conteúdo.</param>
        /// <param name="trans">Transação com o banco de dados.</param>
        /// <returns>Valor do conteúdo.</returns>
        public static string RecuperaValorConteudo(Enumeradores.ConteudoHTML idConteudo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Conteudo", SqlDbType.Int, 4));
            parms[0].Value = (int)idConteudo;

            string valorConteudo = String.Empty;
            IDataReader dr;

            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectvalorconteudo, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectvalorconteudo, parms);

            if (dr.Read())
                valorConteudo = Convert.ToString(dr["Vlr_Conteudo"]);

            //Fechando o IDataReader caso esteja aberto.
            if (!dr.IsClosed)
                dr.Close();

            //Disposing o IDataReader.
            dr.Dispose();

            return valorConteudo;
        }
        public static string RecuperaValorConteudo(Enumeradores.ConteudoHTML idConteudo)
        {
            return RecuperaValorConteudo(idConteudo, null);
        }
        #endregion

        #region ListarConteudos
        /// <summary>
        /// Lista os conteudos atraves de uma lista de ids
        /// </summary>
        /// <param name="idsConteudos"></param>
        /// <returns>Dicionario de conteudos</returns>
        public static Dictionary<Enumeradores.ConteudoHTML, string> ListarConteudos(List<Enumeradores.ConteudoHTML> idsConteudos)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            Dictionary<Enumeradores.ConteudoHTML, string> itensConteudos = new Dictionary<Enumeradores.ConteudoHTML, string>();
            string query = "select Idf_Conteudo, Vlr_Conteudo from BNE_Conteudo_HTML where Idf_Conteudo in (";
            string nomeConteudo = string.Empty;
            for (int i = 0; i < idsConteudos.Count; i++)
            {
                nomeConteudo = "@parm" + i.ToString();

                if (i > 0)
                    query += ", ";

                query += nomeConteudo;
                parms.Add(new SqlParameter(nomeConteudo, SqlDbType.Int, 4));
                parms[i].Value = Convert.ToInt32(idsConteudos[i]);
            }

            query += ")";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, query, parms))
            {
                Enumeradores.ConteudoHTML conteudo = 0;
                while (dr.Read())
                {
                    //O valor for alterado dr[Idf_Parametro] para  dr["Idf_Conteudo"]
                    conteudo = (Enumeradores.ConteudoHTML)Enum.Parse(typeof(Enumeradores.ConteudoHTML), dr["Idf_Conteudo"].ToString());
                    //O valor for alterado dr[Vlr_Parametro] para  dr["Vlr_Conteudo"]
                    itensConteudos.Add(conteudo, Convert.ToString(dr["Vlr_Conteudo"]));
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return itensConteudos;
        }

        #endregion 

        #region Listar
        /// <summary>
        /// Lista todas as funções.
        /// </summary>
        /// <returns></returns>
        public static IDataReader ListarConteudosSMS()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPLISTARSMS, null);
        }
        public static IDataReader ListarConteudosEmail()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPLISTAREMAIL, null);
        }
        #endregion

        #endregion

    }
}