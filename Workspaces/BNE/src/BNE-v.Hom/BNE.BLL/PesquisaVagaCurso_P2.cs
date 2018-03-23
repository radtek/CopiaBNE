//-- Data: 21/12/2016 12:00
//-- Autor: Mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class PesquisaVagaCurso // Tabela: BNE_Pesquisa_Vaga_Curso
	{
        private const string spCarregarPesquisa = "SELECT * FROM BNE_Pesquisa_Vaga_Curso WITH(NOLOCK) WHERE Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga";


        public static PesquisaVagaCurso CarregarPesquisa(int IdPesquisaVaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));
            parms[0].Value = IdPesquisaVaga;

            PesquisaVagaCurso objPesquisa;
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spCarregarPesquisa, parms))
            {
                objPesquisa = new PesquisaVagaCurso();
                if (SetInstance(dr, objPesquisa))
                    return objPesquisa;
            }
            return objPesquisa;
        }

       
    }
}