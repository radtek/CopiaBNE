//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class Endereco : ICloneable // Tabela: TAB_Endereco
	{
        
        #region Consultas

        private const string Spselectporpessoafisica = @"
        SELECT  * 
        FROM    TAB_Endereco E WITH(NOLOCK) 
                INNER JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON PF.Idf_Endereco = E.Idf_Endereco
        WHERE   Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";

        private const string Spselectporfilial = @"
        SELECT  * 
        FROM    TAB_Endereco E WITH(NOLOCK) 
                INNER JOIN TAB_Filial F WITH(NOLOCK) ON F.Idf_Endereco = E.Idf_Endereco
        WHERE   Idf_Filial = @Idf_Filial";

        #endregion

        #region Métodos

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region CarregarPorPessoaFisica
        /// <summary>
        /// Método responsável por carregar uma instancia de endereço através da pessoa física.
        /// </summary>
        /// <param name="objPessoaFisica">Pessoa Física</param>
        /// <param name="objEndereco">Paramero out</param>
        /// <returns>objEndereco</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorPessoaFisica(PessoaFisica objPessoaFisica, out Endereco objEndereco)
        {
            var parms = new List<SqlParameter>
                {
					new SqlParameter { ParameterName = "@Idf_Pessoa_Fisica", SqlDbType = SqlDbType.Int, Size = 4, Value = objPessoaFisica.IdPessoaFisica }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporpessoafisica, parms))
            {
                objEndereco = new Endereco();
                if (SetInstance(dr, objEndereco))
                {
                    if (!dr.IsClosed)
                        dr.Close();

                    return true;
                }

                if (!dr.IsClosed)
                    dr.Close();
            }
            return false;
        }
        #endregion

        #region CarregarPorFilial
        /// <summary>
        /// Método responsável por carregar uma instancia de endereço através da filial.
        /// </summary>
        /// <param name="objFilial">Filial</param>
        /// <param name="objEndereco">Paramero out</param>
        /// <returns>objEndereco</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorFilial(Filial objFilial, out Endereco objEndereco)
        {
            var parms = new List<SqlParameter>
                {
					new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType = SqlDbType.Int, Size = 4, Value = objFilial.IdFilial }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporfilial, parms))
            {
                objEndereco = new Endereco();
                if (SetInstance(dr, objEndereco))
                {
                    if (!dr.IsClosed)
                        dr.Close();

                    return true;
                }

                if (!dr.IsClosed)
                    dr.Close();
            }
            return false;
        }
        #endregion

        #endregion

	}
}