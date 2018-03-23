//TODO: OLD - Não estava sendo usado no projeto, comentado para realizar exclusão da tabela

////-- Data: 08/02/2012 15:30
////-- Autor: Gieyson Stelmak

//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Globalization;

//namespace BNE.BLL
//{
//    public partial class VagaPalavraChave // Tabela: BNE_Vaga_Palavra_Chave
//    {

//        #region Consultas

//        #region Spselectpalavrachaveporvaga
//        private const string Spselectpalavrachaveporvaga = @"
//        SELECT 
//                Des_Palavra_Chave
//        FROM    BNE_Vaga_Palavra_Chave VPC WITH(NOLOCK) 
//                INNER JOIN BNE_Palavra_Chave PC WITH(NOLOCK) ON VPC.Idf_Palavra_Chave = PC.Idf_Palavra_Chave
//        WHERE   Idf_Vaga = @Idf_Vaga
//                AND VPC.Flg_Inativo = 0";
//        #endregion

//        #region Spselectporpalavrachave
//        private const string Spselectporpalavrachave = @"
//        SELECT  *
//        FROM    BNE_Vaga_Palavra_Chave VPC WITH(NOLOCK) 
//                INNER JOIN BNE_Palavra_Chave PC WITH(NOLOCK) ON VPC.Idf_Palavra_Chave = PC.Idf_Palavra_Chave
//        WHERE   Idf_Vaga = @Idf_Vaga 
//                AND Des_Palavra_Chave = @Des_Palavra_Chave";
//        #endregion

//        #region SpAtualizaPalavrasChaveDaVaga
//        private const string SpAtualizaPalavrasChaveDaVaga = @"UPDATE BNE_Vaga_Palavra_Chave SET Flg_Inativo = 1 WHERE Idf_Vaga = @Idf_Vaga";
//        #endregion

//        #endregion

//        #region ListarPalavrasChave
//        /// <summary>
//        /// Método responsável por retornar uma IDataReader com todas as instâncias de VagaPalavraChave 
//        /// </summary>
//        /// <param name="idVaga">Código identificador de uma vaga</param>
//        /// <returns></returns>
//        public static List<string> ListarPalavrasChave(int idVaga)
//        {
//            var listaPalavrasChave = new List<string>();

//            using (IDataReader dr = ListarPalavraChavePorVaga(idVaga))
//            {
//                while (dr.Read())
//                    listaPalavrasChave.Add(dr["Des_Palavra_Chave"].ToString());

//                if (!dr.IsClosed)
//                    dr.Close();
//            }

//            return listaPalavrasChave;
//        }
//        /// <summary>
//        /// Método responsável por retornar uma IDataReader com todas os ids de VagaTipoVinculo
//        /// </summary>
//        /// <param name="idVaga">Código identificador de uma vaga</param>
//        /// <returns></returns>
//        private static IDataReader ListarPalavraChavePorVaga(int idVaga)
//        {
//            var parms = new List<SqlParameter> {new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4)};
//            parms[0].Value = idVaga;

//            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectpalavrachaveporvaga, parms);
//        }
//        #endregion

//        #region CarregarPorPalavraChave
//        /// <summary>
//        /// Método responsável por carregar uma instancia de VagaPalavraChave através da palavra chave e da vaga
//        /// </summary>
//        /// <param name="palavraChave">Palavra Chave</param>
//        /// <param name="objVaga">Vaga</param>
//        /// <param name="objVagaPalavraChave">Parametro out VagaPalavraChave </param>
//        /// <param name="trans"> </param>
//        /// <returns>Boolean</returns>
//        /// <remarks>Gieyson Stelmak</remarks>
//        public static bool CarregarPorPalavraChave(string palavraChave, Vaga objVaga, out VagaPalavraChave objVagaPalavraChave, SqlTransaction trans)
//        {
//            var parms = new List<SqlParameter>
//                            {
//                                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4),            /* 0 */
//                                new SqlParameter("@Des_Palavra_Chave", SqlDbType.VarChar, 50)   /* 1 */ 
//                            };
//            parms[0].Value = objVaga.IdVaga;
//            parms[1].Value = palavraChave;

//            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectporpalavrachave, parms))
//            {
//                objVagaPalavraChave = new VagaPalavraChave();
//                if (SetInstance(dr, objVagaPalavraChave))
//                    return true;

//                if (!dr.IsClosed)
//                    dr.Close();
//            }
//            objVagaPalavraChave = null;
//            return false;
//        }
//        #endregion

//        #region AtualizaPalavrasChaveDaVaga
//        public static void AtualizaPalavrasChaveDaVaga(Vaga objVaga, List<string> listIdsVagaPalavraChave, SqlTransaction trans)
//        {
//            string sp = SpAtualizaPalavrasChaveDaVaga;

//            if (listIdsVagaPalavraChave.Count > 0)
//                sp += " AND Idf_Vaga_Palavra_Chave NOT IN ('" + listIdsVagaPalavraChave.Aggregate((anterior, proximo) => anterior + "\'" + ',' + "\'" + proximo) + "')";

//            var parms = new List<SqlParameter>
//                            {
//                                new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4)
//                            };
//            parms[0].Value = objVaga.IdVaga;

//            DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, sp, parms);
//        }

//        public static void AtualizaPalavrasChaveDaVaga(Vaga objVaga, List<PalavraChave> listPalavrasChave, SqlTransaction trans)
//        {
//            var listVagaPalavraChave = new List<string>();
//            foreach (var palavraChave in listPalavrasChave)
//            {
//                var palavra = palavraChave.DescricaoPalavraChave.Trim();
//                VagaPalavraChave objVagaPalavraChave;
//                if (!VagaPalavraChave.CarregarPorPalavraChave(palavra, objVaga, out objVagaPalavraChave, trans))
//                {
//                    PalavraChave objPalavraChave;
//                    if (!PalavraChave.CarregarPorDescricao(palavra, out objPalavraChave, trans))
//                    {
//                        objPalavraChave = new PalavraChave
//                        {
//                            DescricaoPalavraChave = palavra,
//                            FlagInativo = false
//                        };
//                        objPalavraChave.Save(trans);
//                    }
//                    objVagaPalavraChave = new VagaPalavraChave
//                    {
//                        PalavraChave = objPalavraChave,
//                        Vaga = objVaga,
//                        FlagInativo = false
//                    };
//                    objVagaPalavraChave.Save(trans);
//                }
//                listVagaPalavraChave.Add(objVagaPalavraChave.IdVagaPalavraChave.ToString(CultureInfo.CurrentCulture));
//            }
//        }
//        #endregion

//    }
//}