//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class PessoaFisicaRedeSocial // Tabela: TAB_Pessoa_Fisica_Rede_Social
    {

        #region Consultas

        private const string SPSELECTPORPFREDESOCIAL = @"
                                                            SELECT 
                                                                * 
                                                            FROM TAB_Pessoa_Fisica_Rede_Social ( NOLOCK )
                                                            WHERE
                                                                Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica AND
                                                                Idf_Rede_Social_CS = @Idf_Rede_Social
                                                        ";

        #region Spselectporcodigoredesocial
        private const string Spselectporcodigoredesocial = @"
        SELECT  * 
        FROM    TAB_Pessoa_Fisica_Rede_Social WITH(NOLOCK)
        WHERE   Cod_Interno_Rede_Social LIKE @CodigoRedeSocial
                AND Idf_Rede_Social_CS = @Idf_Rede_Social";
        #endregion

        #endregion

        #region CarregarPorPessoaFisicaRedeSocial
        /// <summary>   
        /// Método responsável por carregar uma instância de PessoaFisicaRedeSocial pela PessoaFisica e RedeSocial
        /// independente se está ativa ou não, utilizado nas telas de gestão.
        /// </summary>
        /// <param name="idPessoaFisica">Identificador da Pessoa Fisica</param>
        /// <param name="idRedeSocial">Identificador da Rede Social</param>
        /// <param name="objPessoaFisicaRedeSocial"></param>
        /// <returns>Objeto Pessoa Fisica Rede Social</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPessoaFisicaRedeSocial(int idPessoaFisica, int idRedeSocial, out PessoaFisicaRedeSocial objPessoaFisicaRedeSocial)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = idPessoaFisica } , 
                new SqlParameter{ ParameterName = "@Idf_Rede_Social", SqlDbType = SqlDbType.Int, Size = 4, Value = idRedeSocial }
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPFREDESOCIAL, parms))
            {
                objPessoaFisicaRedeSocial = new PessoaFisicaRedeSocial();
                if (SetInstance(dr, objPessoaFisicaRedeSocial))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objPessoaFisicaRedeSocial = null;
            return false;
        }
        #endregion

        #region CarregarPorCodigoRedeSocial
        /// <summary>   
        /// Método responsável por carregar uma instância de PessoaFisicaRedeSocial pela PessoaFisica e RedeSocial
        /// independente se está ativa ou não, utilizado nas telas de gestão.
        /// </summary>
        /// <param name="codigoRedeSocial">Identificador da Pessoa Fisica</param>
        /// <param name="redeSocial">Identificador da Rede Social</param>
        /// <param name="objPessoaFisicaRedeSocial">Rede Social</param>
        /// <returns>Objeto Pessoa Fisica Rede Social</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorCodigoRedeSocial(string codigoRedeSocial, Enumeradores.RedeSocial redeSocial, out PessoaFisicaRedeSocial objPessoaFisicaRedeSocial)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@CodigoRedeSocial", SqlDbType = SqlDbType.VarChar, Size = 30, Value = codigoRedeSocial } , 
                new SqlParameter{ ParameterName = "@Idf_Rede_Social", SqlDbType = SqlDbType.Int, Size = 4, Value = redeSocial }
            };

            var retorno = false;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporcodigoredesocial, parms))
            {
                objPessoaFisicaRedeSocial = new PessoaFisicaRedeSocial();
                if (SetInstance(dr, objPessoaFisicaRedeSocial))
                    retorno = true;
                else
                    objPessoaFisicaRedeSocial = null;
            }
            return retorno;
        }
        #endregion

    }
}