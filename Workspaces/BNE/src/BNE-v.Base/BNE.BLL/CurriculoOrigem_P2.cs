//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;
namespace BNE.BLL
{
	public partial class CurriculoOrigem // Tabela: BNE_Curriculo_Origem
    {

        #region Consultas
        private const string Spexisteorigemcurriculo = "SELECT COUNT(*) FROM BNE_Curriculo_Origem WHERE Idf_Origem = @Idf_Origem AND Idf_Curriculo = @Idf_Curriculo";
        private const string Spcarregarorigensdocurriculo = "SELECT Idf_Origem FROM BNE_Curriculo_Origem WHERE Idf_Curriculo = @Idf_Curriculo";
        #endregion 
        
        #region Métodos

        #region ExisteCurriculoNaOrigem
        public static bool ExisteCurriculoNaOrigem(Curriculo objCurriculo, Origem objOrigem)
        {
            return ExisteCurriculoNaOrigem(objCurriculo, objOrigem, null);
        }
	    public static bool ExisteCurriculoNaOrigem(Curriculo objCurriculo, Origem objOrigem, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4),
                                new SqlParameter("@Idf_Origem", SqlDbType.Int, 4)
                            };

            parms[0].Value = objCurriculo.IdCurriculo;
            parms[1].Value = objOrigem.IdOrigem;

            if (trans != null)
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spexisteorigemcurriculo, parms)) > 0;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spexisteorigemcurriculo, parms)) > 0;
        }
        #endregion

        #region CarregarOrigensDoCurriculo
        public static bool CarregarOrigensDoCurriculo(Curriculo curriculo, out List<Origem> listOrigens)
        {
            listOrigens = new List<Origem>();

            var parms = new List<SqlParameter>
            {
                new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
            };

            parms[0].Value = curriculo.IdCurriculo;

            using(IDataReader dr = 
                DataAccessLayer.ExecuteReader(CommandType.Text, Spcarregarorigensdocurriculo, parms))
            {
                if (dr.Read())
                {
                    do
                    {
                        int idOrigem = Convert.ToInt32(dr["Idf_Origem"]);
                        Origem origem = new Origem(idOrigem);
                        listOrigens.Add(origem);
                    } while (dr.Read());

                    return true;
                }

                return false;
            }
        }
        #endregion

        #endregion

    }
}