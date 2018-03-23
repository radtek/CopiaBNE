//-- Data: 29/06/2015 17:58
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class Bairro // Tabela: plataforma.TAB_Bairro
    {

        #region Spselectnomecidade
        private const string Spselectnomecidade = @"
        SELECT * FROM plataforma.TAB_Bairro WHERE Nme_Bairro LIKE @Nme_Bairro AND Idf_Cidade = @Idf_Cidade
        ";
        #endregion

        #region CarregarPorNomeCidade
        /// <summary>
        /// Método utilizado para retornar uma instância de Bairro a partir do banco de dados."
        /// </summary>
        /// <param name="nomeBairro">Nome do Bairro</param>
        /// <param name="objCidade">Cidade</param>
        /// <returns>objBairro</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorNomeCidade(string nomeBairro, Cidade objCidade, out Bairro objBairro)
        {
            if (!string.IsNullOrEmpty(nomeBairro))
            {
                nomeBairro = nomeBairro.Trim();

                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Nme_Bairro", SqlDbType = SqlDbType.VarChar, Size = 80, Value = nomeBairro },
                    new SqlParameter { ParameterName = "@Idf_Cidade", SqlDbType = SqlDbType.Int, Value = objCidade.IdCidade }
                };

                objBairro = new Bairro();

                using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectnomecidade, parms))
                {
                    if (SetInstance(dr, objBairro))
                        return true;
                }
            }

            objBairro = null;
            return false;
        }
        #endregion

    }
}