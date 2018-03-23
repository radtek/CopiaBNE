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


        #region SP_LIST_BY_CITY_ID
        private const string SP_LIST_BY_CITY_ID = @"
        SELECT B.Idf_Bairro, B.Nme_Bairro
        FROM plataforma.TAB_Bairro B WITH(NOLOCK)
        INNER JOIN plataforma.TAB_Cidade C WITH(NOLOCK) ON B.Idf_Cidade = C.Idf_Cidade
        INNER JOIN plataforma.TAB_Estado E WITH(NOLOCK) ON C.Sig_Estado = E.Sig_Estado
        WHERE 1 = 1 
	        AND C.Nme_Cidade =  @Nme_Cidade 
	        AND E.Sig_Estado =  @Sig_Estado 
	        AND B.Nme_Bairro LIKE @Nme_Bairro";
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



        public class Bairro_Rst { public int id; public string descricao; }
        public static List<Bairro_Rst> ListByCity(string NomeCidade, string SiglaEstado, string Query)
        {
            List<Bairro_Rst> lst = new List<Bairro_Rst>();

            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Nme_Cidade", SqlDbType = SqlDbType.VarChar, Value = NomeCidade },
                new SqlParameter { ParameterName = "@Sig_Estado", SqlDbType = SqlDbType.VarChar, Value = SiglaEstado },
                new SqlParameter { ParameterName = "@Nme_Bairro", SqlDbType = SqlDbType.VarChar, Value = Query + "%" },
            };


            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_LIST_BY_CITY_ID, parms))
            {
                var bairro_rst = new Bairro_Rst();
                while (dr.Read())
                {
                    lst.Add(new Bairro_Rst()
                    {
                        id = Convert.ToInt32(dr["Idf_Bairro"]),
                        descricao = Convert.ToString(dr["Nme_Bairro"])
                    });
                }
            }

            return lst;
        }

    }
}