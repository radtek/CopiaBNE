//-- Data: 18/09/2013 15:11
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class CampanhaCurriculo // Tabela: BNE_Campanha_Curriculo
    {

        #region Consultas

        #region Sprecuperarcurriculos
        private const string Sprecuperarcurriculos = "SELECT * FROM BNE_Campanha_Curriculo WHERE Idf_Campanha = @Idf_Campanha";
        #endregion

        #endregion

        #region RecuperarListaCurriculosPorCampanha
        /// <summary>
        /// Recuperar todos os currículos selecionados para a campanha
        /// </summary>
        /// <returns></returns>
        public static List<CampanhaCurriculo> RecuperarListaCurriculosPorCampanha(Campanha objCampanha)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Campanha", SqlDbType = SqlDbType.Int, Size = 4, Value = objCampanha.IdCampanha }
                };

            var lista = new List<CampanhaCurriculo>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarcurriculos, parms))
            {
                while (dr.Read())
                {
                    var objCampanhaCurriculo = new CampanhaCurriculo();
                    if (SetInstanceNonDispose(dr, objCampanhaCurriculo))
                        lista.Add(objCampanhaCurriculo);
                }
            }

            return lista;
        }
        #endregion

        #region SetInstanceNonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objCampanhaCurriculo">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceNonDispose(IDataReader dr, CampanhaCurriculo objCampanhaCurriculo)
        {
            objCampanhaCurriculo._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
            objCampanhaCurriculo._campanha = new Campanha(Convert.ToInt32(dr["Idf_Campanha"]));
            objCampanhaCurriculo._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
            objCampanhaCurriculo._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
            objCampanhaCurriculo._numeroCelular = Convert.ToString(dr["Num_Celular"]);
            objCampanhaCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

            objCampanhaCurriculo._persisted = true;
            objCampanhaCurriculo._modified = false;

            return true;
        }
        #endregion

    }
}