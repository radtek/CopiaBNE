//-- Data: 01/10/2010 11:40
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PesquisaCurriculoIdioma // Tabela: TAB_Pesquisa_Curriculo_Idioma
    {

        #region Consultas
        private const string Spselectporpesquisa = @"
                    SELECT idf_Idioma, Idf_Nivel_Idioma FROM TAB_Pesquisa_Curriculo_Idioma with(NOLOCK)
	                  WHERE Idf_Pesquisa_Curriculo =@Idf_Pesquisa_Curriculo";

        #region [spListarIdiomaPesquisaFiltro]
        private const string spListarIdiomaPesquisaFiltro = @"select ci.Idf_Idioma,i.Des_Idioma,ni.Des_Nivel_Idioma
 from bne.TAB_Pesquisa_Curriculo_Idioma ci with(nolock)
join bne.TAB_Idioma i with(nolock) on i.Idf_Idioma = ci.Idf_Idioma
left join bne.TAB_Nivel_Idioma ni with(nolock) on ci.Idf_Nivel_Idioma =  ni.Idf_Nivel_Idioma
where ci.Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo ";
        #endregion
        #endregion

        #region Métodos

        #region ListarPorPesquisaList
        private static IDataReader ListarPorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.IdPesquisaCurriculo }
            };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporpesquisa, parms);
        }
        #endregion

        #region ListarIdentificadoresIdiomaPorPesquisa
        public static List<KeyValuePair<int, int>> ListarIdentificadoresIdiomaPorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var lista = new List<KeyValuePair<int, int>>();
           
            using (IDataReader dr = ListarPorPesquisa(objPesquisaCurriculo))
            {
                while (dr.Read()){
                    lista.Add( new KeyValuePair<int,int>(Convert.ToInt32(dr["Idf_Idioma"]), 
                        (dr["idf_Nivel_idioma"] != DBNull.Value ? Convert.ToInt32(dr["idf_Nivel_idioma"]) : 0)));
                }
                    

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #region ListarIdiomaPorPesquisa
        public static List<PesquisaCurriculoIdioma> ListarIdiomaPorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var lista = new List<PesquisaCurriculoIdioma>();
            using (IDataReader dr = ListarPorPesquisa(objPesquisaCurriculo))
            {
                while (dr.Read())
                {
                    PesquisaCurriculoIdioma obj = new PesquisaCurriculoIdioma
                    {
                        Idioma = new Idioma(Convert.ToInt32(dr["idf_Idioma"])),
                        NivelIdioma = dr["idf_Nivel_Idioma"] != DBNull.Value ? new NivelIdioma(Convert.ToInt32(dr["idf_Nivel_Idioma"])) : null
                    };
                    lista.Add(obj);
                }
                   

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #region ListarIdiomaPorPesquisa
        public static Dictionary<int,string> ListarIdiomaPesquisaFiltro(PesquisaCurriculo objPesquisaCurriculo)
        {
            var lista = new Dictionary<int, string>();
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.IdPesquisaCurriculo }
            };
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spListarIdiomaPesquisaFiltro, parms))
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["Idf_Idioma"]), dr["Des_Idioma"].ToString() + " "+ (dr["Des_Nivel_Idioma"] != DBNull.Value ? dr["Des_Nivel_Idioma"].ToString() : ""));
                }


                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #endregion

    }
}