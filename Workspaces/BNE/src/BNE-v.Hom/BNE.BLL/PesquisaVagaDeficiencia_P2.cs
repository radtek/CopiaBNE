//-- Data: 26/01/2016 17:17
//-- Autor: Mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class PesquisaVagaDeficiencia // Tabela: TAB_Pesquisa_Vaga_Deficiencia
	{
        #region Consultas

        #region spSelectDeficiencias
        private const string spSelectDeficiencias = @"
        select idf_deficiencia_detalhe from BNE.TAB_Pesquisa_Vaga_Deficiencia with(nolock) where idf_Pesquisa_Vaga = @idf_Pesquisa_Vaga";

        #endregion

        #endregion

        #region Metodos

        #region ListaDeficiencia
        /// <summary>
        /// Lista as Deficiencias detalhes da pesquisa vaga.
        /// </summary>
        /// <param name="idPesquisaVaga"></param>
        /// <returns></returns>
        public static List<DeficienciaDetalhe> listaDeficiencia(int idPesquisaVaga)
        {
            List<DeficienciaDetalhe> lista = new List<DeficienciaDetalhe>();
          
            var parms = new List<SqlParameter> {
                new SqlParameter("@idf_Pesquisa_Vaga",  SqlDbType.Int, 4)
            };

            parms[0].Value = idPesquisaVaga;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spSelectDeficiencias, parms))
            {

                while (dr.Read())
                    lista.Add(DeficienciaDetalhe.LoadObject(Convert.ToInt32(dr["idf_Deficiencia_Detalhe"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #endregion



    }
}